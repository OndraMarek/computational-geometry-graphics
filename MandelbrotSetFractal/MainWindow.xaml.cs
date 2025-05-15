using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace MandelbrotSetFractal
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DrawMandelbrotSet_Click_Click(object sender, RoutedEventArgs e)
        {
            int iterations = iterationsTextBox.Text != "" ? int.Parse(iterationsTextBox.Text) : 0;
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