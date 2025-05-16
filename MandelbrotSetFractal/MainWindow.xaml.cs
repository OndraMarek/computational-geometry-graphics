using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MandelbrotSetFractal
{
    public partial class MainWindow : Window
    {
        const int width = 800;
        const int height = 500;

        private double minX = -2.5;
        private double maxX = 1;
        private double minY = -1;
        private double maxY = 1;

        public MainWindow()
        {
            InitializeComponent();
            mandelbrotImage.MouseLeftButtonDown += MandelbrotImage_MouseLeftButtonDown;
            mandelbrotImage.MouseRightButtonDown += MandelbrotImage_MouseRightButtonDown;
        }

        private void DrawMadMandelbrotSet(int maxIterations)
        {
            WriteableBitmap bitmap = new(width, height, 96, 96, PixelFormats.Bgra32, null);
            int stride = width * 4;
            byte[] pixels = new byte[height * stride];

            double scaleX = (maxX - minX) / width;
            double scaleY = (maxY - minY) / height;

            string? selectedScheme = ((ComboBoxItem)colorSchemeComboBox.SelectedItem)?.Content.ToString();

            for (int py = 0; py < height; py++)
            {
                double y0 = minY + py * scaleY;

                for (int px = 0; px < width; px++)
                {
                    double x0 = minX + px * scaleX;
                    double x = 0.0;
                    double y = 0.0;
                    int iteration = 0;

                    while (x * x + y * y <= 4 && iteration < maxIterations)
                    {
                        double xtemp = x * x - y * y + x0;
                        y = 2 * x * y + y0;
                        x = xtemp;
                        iteration++;
                    }

                    (byte r, byte g, byte b) = GetColorScheme(iteration, maxIterations, selectedScheme);

                    int pixelIndex = py * stride + px * 4;
                    pixels[pixelIndex + 0] = b;
                    pixels[pixelIndex + 1] = g;
                    pixels[pixelIndex + 2] = r;
                    pixels[pixelIndex + 3] = 255;
                }
            }

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0);
            mandelbrotImage.Source = bitmap;
        }

        private static (byte r, byte g, byte b) GetColorScheme(int iteration, int maxIterations, string scheme)
        {
            byte r, g, b;

            if (iteration == maxIterations)
            {
                r = g = b = 0;
            }
            else
            {
                double t = (double)iteration / maxIterations;

                switch (scheme)
                {
                    case "Fire":
                        (r,g,b) = GetFireColorScheme(t);
                        break;
                    case "Ocean":
                        (r, g, b) = GetOceanColorScheme(t);
                        break;
                    case "Rainbow":
                        (r, g, b) = GetRainbowColorScheme(t);
                        break;
                    case "Grayscale":
                    default:
                        byte gray = (byte)(255 - (iteration * 255 / maxIterations));
                        r = g = b = gray;
                        break;
                }
            }

            return (r, g, b);
        }

        private static (byte r, byte g, byte b) GetFireColorScheme(double t)
        {
            byte r = (byte)Math.Min(255, 255 * t * 3);
            byte g = (byte)Math.Min(255, 255 * t * t);
            byte b = (byte)Math.Min(255, 100 * t);
            return (r, g, b);
        }

        private static (byte r, byte g, byte b) GetOceanColorScheme(double t)
        {
            byte r = 0;
            byte g = (byte)Math.Min(255, 255 * Math.Sqrt(t));
            byte b = (byte)Math.Min(255, 255 * t);
            return (r, g, b);
        }

        private static (byte r, byte g, byte b) GetRainbowColorScheme(double t)
        {
            byte r = (byte)(255 * Math.Sin(t * 2 * Math.PI));
            byte g = (byte)(255 * Math.Sin(t * 2 * Math.PI + 2 * Math.PI / 3));
            byte b = (byte)(255 * Math.Sin(t * 2 * Math.PI + 4 * Math.PI / 3));
            return (r, g, b);
        }

        private void DrawMandelbrotSet_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(iterationsTextBox.Text, out int maxIterations))
            {
                DrawMadMandelbrotSet(maxIterations);
            }
        }

        private void MandelbrotImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2 && int.TryParse(iterationsTextBox.Text, out int maxIterations))
            {
                Point pos = e.GetPosition(mandelbrotImage);
                double mouseX = pos.X;
                double mouseY = pos.Y;

                double xCenter = minX + mouseX / width * (maxX - minX);
                double yCenter = minY + mouseY / height * (maxY - minY);

                double newWidth = (maxX - minX) / 2;
                double newHeight = (maxY - minY) / 2;

                minX = xCenter - newWidth / 2;
                maxX = xCenter + newWidth / 2;
                minY = yCenter - newHeight / 2;
                maxY = yCenter + newHeight / 2;

                DrawMadMandelbrotSet(maxIterations);
            }
        }

        private void MandelbrotImage_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            minX = -2.5;
            maxX = 1;
            minY = -1;
            maxY = 1;

            if (int.TryParse(iterationsTextBox.Text, out int maxIterations))
            {
                DrawMadMandelbrotSet(maxIterations);
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }
    }
}
