using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawCircleBresenham
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void DrawCircle(int xCenter, int yCenter, int radius)
        {
            ConvertCoordinates(ref xCenter, ref yCenter);

            int x = 0;
            int y = radius;
            int dvex = 3;
            int dvey = 2 * radius - 2;
            int prediction = 1 - radius;

            while (x <= y)
            {
                DrawSymmetricPoints(xCenter, yCenter, x, y);

                if (prediction >= 0)
                {
                    prediction -= dvey;
                    dvey -= 2;
                    y--;
                }

                prediction += dvex;
                dvex += 2;
                x++;
            }
        }

        private void DrawSymmetricPoints(int xCenter, int yCenter, int x, int y)
        {
            DrawPixel(xCenter + x, yCenter + y);
            DrawPixel(xCenter - x, yCenter + y);
            DrawPixel(xCenter + x, yCenter - y);
            DrawPixel(xCenter - x, yCenter - y);
            DrawPixel(xCenter + y, yCenter + x);
            DrawPixel(xCenter - y, yCenter + x);
            DrawPixel(xCenter + y, yCenter - x);
            DrawPixel(xCenter - y, yCenter - x);
        }

        private void DrawPixel(int x, int y)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = 1,
                Height = 1,
                Fill = Brushes.Black
            };
            Canvas.SetLeft(rectangle, x);
            Canvas.SetTop(rectangle, y);
            drawCircleCanvas.Children.Add(rectangle);
        }


        private void ConvertCoordinates(ref int x, ref int y)
        {
            y = -y;
            x += (int)(drawCircleCanvas.Width / 2);
            y += (int)(drawCircleCanvas.Height / 2);
        }

        private void ClearCanvas()
        {
            drawCircleCanvas.Children.Clear();
        }

        private void drawCircleButton_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
            int x = xTextBox.Text != "" ? int.Parse(xTextBox.Text) : 0;
            int y = yTextBox.Text != "" ? int.Parse(yTextBox.Text) : 0;
            int radius = radiusTextBox.Text != "" ? int.Parse(radiusTextBox.Text) : 0;
            DrawCircle(x, y, radius);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}