using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawLineBresenham
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void DrawLine(int x1, int y1, int x2, int y2)
        {
            DrawPoint(x1, y1);
            DrawPoint(x2, y2);

            ConvertCoordinates(ref x1, ref y1);
            ConvertCoordinates(ref x2, ref y2);

            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = x1 < x2 ? 1 : -1;
            int sy = y1 < y2 ? 1 : -1;
            int e = dx - dy;

            while (true)
            {
                DrawPixel(x1, y1);

                if (x1 == x2 && y1 == y2) break;

                int e2 = 2 * e;
                if (e2 > -dy)
                {
                    e -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    e += dx;
                    y1 += sy;
                }
            }
        }

        private void DrawPixel(double x, double y)
        {
            Rectangle pixel = new Rectangle
            {
                Width = 1,
                Height = 1,
                Fill = new SolidColorBrush(Colors.Black)
            };

            Canvas.SetLeft(pixel, x);
            Canvas.SetTop(pixel, y);

            drawLineCanvas.Children.Add(pixel);
        }

        private void DrawPoint(int x, int y)
        {
            ConvertCoordinates(ref x, ref y);

            Ellipse point = new Ellipse
            {
                Width = 5,
                Height = 5,
                Fill = new SolidColorBrush(Colors.Red)
            };

            Canvas.SetLeft(point, x - 2.5);
            Canvas.SetTop(point, y - 2.5);

            drawLineCanvas.Children.Add(point);
        }

        private void ConvertCoordinates(ref int x, ref int y)
        {
            y = -y;
            x += (int)(drawLineCanvas.Width / 2);
            y += (int)(drawLineCanvas.Height / 2);
        }

        private void ClearCanvas()
        {
            drawLineCanvas.Children.Clear();
        }
        private void drawLineButton_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
            int x1 = x1TextBox.Text != "" ? int.Parse(x1TextBox.Text) : 0;
            int y1 = y1TextBox.Text != "" ? int.Parse(y1TextBox.Text) : 0;
            int x2 = x2TextBox.Text != "" ? int.Parse(x2TextBox.Text) : 0;
            int y2 = y2TextBox.Text != "" ? int.Parse(y2TextBox.Text) : 0;
            DrawLine(x1, y1, x2, y2);
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}