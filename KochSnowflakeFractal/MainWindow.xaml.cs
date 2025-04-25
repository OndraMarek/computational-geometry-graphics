using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Text.RegularExpressions;

namespace KochSnowflakeFractal
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }
        private void DrawKochSnowflakeRecursive(int iterations)
        {
            double width = drawKochSnowflakeCanvas.ActualWidth;
            double height = drawKochSnowflakeCanvas.ActualHeight;

            double sideLength = width * 0.3;

            double centerX = width / 2;
            double centerY = height / 2 + sideLength * Math.Sqrt(3) / 6;

            Point p1 = new(centerX - sideLength / 2, centerY);
            Point p2 = new(centerX + sideLength / 2, centerY);
            Point p3 = new(centerX, centerY - Math.Sqrt(3) / 2 * sideLength);

            DrawEdge(p1, p2, iterations);
            DrawEdge(p2, p3, iterations);
            DrawEdge(p3, p1, iterations);
        }

        private void DrawEdge(Point a, Point b, int iteration)
        {
            if (iteration == 0)
            {
                DrawLine(a, b);
            }
            else
            {
                Point oneThird = new((2 * a.X + b.X) / 3, (2 * a.Y + b.Y) / 3);
                Point twoThirds = new((a.X + 2 * b.X) / 3, (a.Y + 2 * b.Y) / 3);

                double dx = twoThirds.X - oneThird.X;
                double dy = twoThirds.Y - oneThird.Y;

                Point peak = new(
                    oneThird.X + dx / 2 - Math.Sqrt(3) / 2 * dy,
                    oneThird.Y + dy / 2 + Math.Sqrt(3) / 2 * dx
                );

                DrawEdge(a, oneThird, iteration - 1);
                DrawEdge(oneThird, peak, iteration - 1);
                DrawEdge(peak, twoThirds, iteration - 1);
                DrawEdge(twoThirds, b, iteration - 1);
            }
        }

        private void DrawLine(Point a, Point b)
        {
            drawKochSnowflakeCanvas.Children.Add(new Line()
            {
                X1 = a.X,
                Y1 = a.Y,
                X2 = b.X,
                Y2 = b.Y,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            });
        }

        private void DrawKochSnowflake_Click(object sender, RoutedEventArgs e)
        {
            drawKochSnowflakeCanvas.Children.Clear();
            int iterations = iterationsTextBox.Text != "" ? int.Parse(iterationsTextBox.Text) : 0;
            DrawKochSnowflakeRecursive(iterations);
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