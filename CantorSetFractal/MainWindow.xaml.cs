using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CantorSetFractal
{
    public partial class MainWindow : Window
    {
        private const double LineHeight = 20;
        private const double CanvasMargin = 20;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawCantorSetRecursive(Canvas canvas, double x, double y, double width, int iterations, int maxIterations)
        {
            if (iterations <= 0)
            {
                return;
            }

            Line line = new()
            {
                X1 = x,
                Y1 = y + (maxIterations - iterations) * LineHeight,
                Y2 = y + (maxIterations - iterations) * LineHeight,
                X2 = x + width,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            canvas.Children.Add(line);

            if (width < 1)
            {
                return;
            }

            double oneThirdWidth = width / 3;

            DrawCantorSetRecursive(canvas, x, y, oneThirdWidth, iterations - 1, maxIterations);
            DrawCantorSetRecursive(canvas, x + 2 * oneThirdWidth, y, oneThirdWidth, iterations - 1, maxIterations);
        }

        private void DrawCantorSet_Click(object sender, RoutedEventArgs e)
        {
            drawCantorSetCanvas.Children.Clear();

            int iterations = iterationsTextBox.Text != "" ? int.Parse(iterationsTextBox.Text) : 0;
            double canvasWidth = drawCantorSetCanvas.ActualWidth - 2 * CanvasMargin;

            DrawCantorSetRecursive(drawCantorSetCanvas, CanvasMargin, CanvasMargin, canvasWidth, iterations, iterations);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}