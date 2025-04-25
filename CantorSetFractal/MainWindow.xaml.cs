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

        private void DrawCantorSet_Click(object sender, RoutedEventArgs e)
        {
            drawCantorSetCanvas.Children.Clear();
            if (int.TryParse(iterationsTextBox.Text, out int iterations))
            {
                double canvasWidth = drawCantorSetCanvas.ActualWidth - 2 * CanvasMargin;
                if (canvasWidth > 0)
                {
                    DrawCantorSetRecursive(drawCantorSetCanvas, CanvasMargin, CanvasMargin, canvasWidth, iterations);
                }
            }
            else
            {
                MessageBox.Show("Zadejte prosím platné číslo pro počet iterací.", "Chyba vstupu");
            }
        }

        private void DrawCantorSetRecursive(Canvas canvas, double x, double y, double width, int iterations)
        {
            if (iterations <= 0)
            {
                return;
            }

            double currentY = y + iterations * LineHeight;
            Line line = new Line
            {
                X1 = x,
                Y1 = currentY,
                X2 = x + width,
                Y2 = currentY,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            canvas.Children.Add(line);

            if (width < 1)
            {
                return;
            }

            double newWidth = width / 3;
            double x1 = x;
            double x2 = x + 2 * newWidth;
            double newY = y;

            DrawCantorSetRecursive(canvas, x1, newY, newWidth, iterations - 1);
            DrawCantorSetRecursive(canvas, x2, newY, newWidth, iterations - 1);
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