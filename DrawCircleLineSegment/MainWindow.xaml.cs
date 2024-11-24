using System.Text.RegularExpressions;
using System.Windows;
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

        public void DrawCircle(int xCenter, int yCenter, int points, int radius)
        {
            ConvertCordinates(ref xCenter, ref yCenter);

            double alpha = 2 * Math.PI / points;
            double x1 = radius;
            double y1 = 0;
            double x2, y2;

            for (int i = 1; i <= points; i++)
            {
                x2 = radius * Math.Cos(i * alpha);
                y2 = radius * Math.Sin(i * alpha);

                DrawLine(xCenter + x1, yCenter + y1, xCenter + x2, yCenter + y2);

                x1 = x2;
                y1 = y2;
            }
        }

        private void DrawLine(double x1, double y1, double x2, double y2)
        {
            Line line = new Line
            {
                Stroke = Brushes.Black,
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                StrokeThickness = 1
            };

            drawCircleCanvas.Children.Add(line);
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