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
        public void AnimateRollingCircles(int x, int y, int radius1, int radius2)
        {
            ConvertCordinates(ref x, ref y);

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
            ClearCanvas();
            int x = xTextBox.Text != "" ? int.Parse(xTextBox.Text) : 0;
            int y = yTextBox.Text != "" ? int.Parse(yTextBox.Text) : 0;
            int radius1 = radius1TextBox.Text != "" ? int.Parse(radius1TextBox.Text) : 0;
            int radius2 = radius2TextBox.Text != "" ? int.Parse(radius2TextBox.Text) : 0;
            AnimateRollingCircles(x, y, radius1, radius2);
        }
    }
}