using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CantorSetFractal
{
    public partial class MainWindow : Window
    {
        private const double LineHeight = 20;
        private const double CanvasMargin = 25;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawCantorSetRecursive(double x, double y, double width, int iterations, int maxIterations)
        {
            if (iterations < 0)
            {
                return;
            }

            double yOffset = (maxIterations - iterations) * LineHeight;

            DrawLine(x, x + width, y + yOffset, y + yOffset);

            double oneThirdWidth = width / 3;

            DrawCantorSetRecursive(x, y, oneThirdWidth, iterations - 1, maxIterations);
            DrawCantorSetRecursive(x + 2 * oneThirdWidth, y, oneThirdWidth, iterations - 1, maxIterations);
        }

        private void DrawLine(double x1, double x2, double y1, double y2)
        {
            drawCantorSetCanvas.Children.Add(new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            });
        }

        private void DrawCantorSet_Click(object sender, RoutedEventArgs e)
        {
            drawCantorSetCanvas.Children.Clear();

            int iterations = iterationsTextBox.Text != "" ? int.Parse(iterationsTextBox.Text) : 0;
            double canvasWidth = drawCantorSetCanvas.ActualWidth - 2 * CanvasMargin;

            DrawCantorSetRecursive(CanvasMargin, CanvasMargin, canvasWidth, iterations, iterations);
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