using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawCircleLineSegment
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void DrawCircle(int x, int y, int points, int radius)
        {
            return;
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

            drawCircleCanvas.Children.Add(pixel);
        }

        private void ConvertCordinates(ref int x, ref int y)
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
            int points = pointsTextBox.Text != "" ? int.Parse(pointsTextBox.Text) : 0;
            int radius = radiusTextBox.Text != "" ? int.Parse(radiusTextBox.Text) : 0;
            DrawCircle(x, y, points, radius);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}