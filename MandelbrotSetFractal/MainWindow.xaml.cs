using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MandelbrotSetFractal
{
    public partial class MainWindow : Window
    {
        const int width = 800;
        const int height = 450;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawMadMandelbrotSet(int maxIterations)
        {
            WriteableBitmap bitmap = new(width, height, 96, 96, PixelFormats.Bgra32, null);
            int stride = width * 4;
            byte[] pixels = new byte[height * stride];

            double scaleX = 3.5 / width;
            double scaleY = 2.0 / height;

            for (int py = 0; py < height; py++)
            {
                double y0 = (py * scaleY) - 1.0;

                for (int px = 0; px < width; px++)
                {
                    double x0 = (px * scaleX) - 2.5;
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

                    int color = iteration == maxIterations ? 0 : 255 - (iteration * 255 / maxIterations);
                    int pixelIndex = py * stride + px * 4;
                    pixels[pixelIndex + 0] = (byte)color;
                    pixels[pixelIndex + 1] = (byte)color;
                    pixels[pixelIndex + 2] = (byte)color;
                    pixels[pixelIndex + 3] = 255;
                }
            }

            bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, stride, 0);
            mandelbrotImage.Source = bitmap;
        }

        private void DrawMandelbrotSet_Click(object sender, RoutedEventArgs e)
        {
            int maxIterations = iterationsTextBox.Text != "" ? int.Parse(iterationsTextBox.Text) : 0;
            DrawMadMandelbrotSet(maxIterations);
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
