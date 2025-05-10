using System.Windows;
using System.Windows.Input;
using System.Text.RegularExpressions;

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