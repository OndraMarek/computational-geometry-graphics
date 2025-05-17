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
        const int imageWidth = 800;
        const int imageHeight = 500;

        private double viewportMinX = -2.5;
        private double viewportMaxX = 1;
        private double viewportMinY = -1;
        private double viewportMaxY = 1;

        public MainWindow()
        {
            InitializeComponent();
            mandelbrotImage.MouseLeftButtonDown += MandelbrotImage_MouseLeftButtonDown;
            mandelbrotImage.MouseRightButtonDown += MandelbrotImage_MouseRightButtonDown;
        }

        private void DrawMadMandelbrotSet(int maxIterations)
        {
            WriteableBitmap mandelbrotBitmap = new(imageWidth, imageHeight, 96, 96, PixelFormats.Bgra32, null);
            byte[] pixelsBuffer = new byte[imageHeight * imageWidth * 4];

            double pixelToViewportScaleX = (viewportMaxX - viewportMinX) / imageWidth;
            double pixelToViewportScaleY = (viewportMaxY - viewportMinY) / imageHeight;
            string? selectedColorScheme = ((ComboBoxItem)colorSchemeComboBox.SelectedItem).Content.ToString();

            for (int py = 0; py < imageHeight; py++)
            {
                double viewportY = viewportMinY + py * pixelToViewportScaleY;
                for (int px = 0; px < imageWidth; px++)
                {
                    double viewportX = viewportMinX + px * pixelToViewportScaleX;
                    int currentIteration = CalculateMandelbrotIterations(viewportX, viewportY, maxIterations);
                    (byte r, byte g, byte b) = GetColorScheme(currentIteration, maxIterations, selectedColorScheme);
                    SetPixelColor(pixelsBuffer, py * imageWidth + px, r, g, b);
                }
            }

            mandelbrotBitmap.WritePixels(new Int32Rect(0, 0, imageWidth, imageHeight), pixelsBuffer, imageWidth * 4, 0);
            mandelbrotImage.Source = mandelbrotBitmap;
        }

        private static int CalculateMandelbrotIterations(double x0, double y0, int maxIterations)
        {
            double x = 0.0, y = 0.0;
            int iteration = 0;
            while (x * x + y * y <= 4 && iteration < maxIterations)
            {
                double xtemp = x * x - y * y + x0;
                y = 2 * x * y + y0;
                x = xtemp;
                iteration++;
            }
            return iteration;
        }

        private static void SetPixelColor(byte[] pixels, int index, byte r, byte g, byte b)
        {
            int pixelIndex = index * 4;
            pixels[pixelIndex + 0] = b;
            pixels[pixelIndex + 1] = g;
            pixels[pixelIndex + 2] = r;
            pixels[pixelIndex + 3] = 255;
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
                double mousePixelX = pos.X;
                double mousePixelY = pos.Y;

                double viewportCenterX = viewportMinX + mousePixelX / imageWidth * (viewportMaxX - viewportMinX);
                double viewportCenterY = viewportMinY + mousePixelY / imageHeight * (viewportMaxY - viewportMinY);

                double newViewportWidth = (viewportMaxX - viewportMinX) / 2;
                double newViewportHeight = (viewportMaxY - viewportMinY) / 2;

                viewportMinX = viewportCenterX - newViewportWidth / 2;
                viewportMaxX = viewportCenterX + newViewportWidth / 2;
                viewportMinY = viewportCenterY - newViewportHeight / 2;
                viewportMaxY = viewportCenterY + newViewportHeight / 2;

                DrawMadMandelbrotSet(maxIterations);
            }
        }

        private void MandelbrotImage_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewportMinX = -2.5;
            viewportMaxX = 1;
            viewportMinY = -1;
            viewportMaxY = 1;

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
