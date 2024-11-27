using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
namespace AnimateRollingCircles
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ConvertCordinates(ref int x, ref int y)
        {
            y = -y;
            x += (int)(animateRollingCirclesCanvas.Width / 2);
            y += (int)(animateRollingCirclesCanvas.Height / 2);
        }

        private void ClearCanvas()
        {
            animateRollingCirclesCanvas.Children.Clear();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void animateRollingCirclesButton_Click(object sender, RoutedEventArgs e)
        {
            return;
        }
    }
}