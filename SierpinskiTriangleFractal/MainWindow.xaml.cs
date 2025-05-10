using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SierpinskiTriangleFractal
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawSierpinskiTriangleRecursive(int iterations)
        {
            double centerX = drawSierpinskiTriangleCanvas.ActualWidth / 2;
            double centerY = drawSierpinskiTriangleCanvas.ActualHeight / 2;
            double size = Math.Min(drawSierpinskiTriangleCanvas.ActualWidth, drawSierpinskiTriangleCanvas.ActualHeight) * 0.8 / 2;

            Point p1 = new Point(centerX, centerY - size * Math.Sqrt(3) / 2);
            Point p2 = new Point(centerX - size, centerY + size * Math.Sqrt(3) / 2 / 2);
            Point p3 = new Point(centerX + size, centerY + size * Math.Sqrt(3) / 2 / 2);

            DrawTriangle(p1, p2, p3, iterations);
        }

        private void DrawTriangle(Point p1, Point p2, Point p3, int depth)
        {
            DrawFilledTriangle(p1, p2, p3, Brushes.Black);

            if (depth > 0)
            {
                Point mid12 = new Point((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2);
                Point mid23 = new Point((p2.X + p3.X) / 2, (p2.Y + p3.Y) / 2);
                Point mid31 = new Point((p3.X + p1.X) / 2, (p3.Y + p1.Y) / 2);

                DrawFilledTriangle(mid12, mid23, mid31, Brushes.White);

                DrawTriangle(p1, mid12, mid31, depth - 1);
                DrawTriangle(mid12, p2, mid23, depth - 1);
                DrawTriangle(mid31, mid23, p3, depth - 1);
            }
        }

        private void DrawFilledTriangle(Point p1, Point p2, Point p3, Brush fill)
        {
            Polygon triangle = new Polygon();
            triangle.Points = new PointCollection { p1, p2, p3 };
            triangle.Fill = fill;
            drawSierpinskiTriangleCanvas.Children.Add(triangle);
        }

        private void DrawSierpinskiTriangle_Click(object sender, RoutedEventArgs e)
        {
            drawSierpinskiTriangleCanvas.Children.Clear();
            int iterations = iterationsTextBox.Text != "" ? int.Parse(iterationsTextBox.Text) : 0;
            DrawSierpinskiTriangleRecursive(iterations);
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