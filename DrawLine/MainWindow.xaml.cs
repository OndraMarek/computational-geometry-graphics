using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawLine
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void DrawLine(int x1, int y1, int x2, int y2)
        {

            DrawPoint(x1, y1);
            DrawPoint(x2, y2);

            ConvertCordinates(ref x1, ref y1);
            ConvertCordinates(ref x2, ref y2);

            int dx = x2 - x1;
            int dy = y2 - y1; 
            double steps = Math.Abs(dx) > Math.Abs(dy) ?  Math.Abs(dx) : Math.Abs(dy);
            double x = x1;
            double y = y1;
            double DX = dx / steps;
            double DY = dy / steps;
            
            for(int i = 0; i <= steps; i++)
            {
                DrawPixel(Math.Round(x), Math.Round(y));
                x = x + DX;
                y = y + DY;
            }

        }

        private void DrawPixel(double x, double y)
        {
            Rectangle pixel = new Rectangle
            {
                Width = 1,
                Height = 1, 
                Fill = new SolidColorBrush(Colors.Black)
            };

            Canvas.SetLeft(pixel, x);
            Canvas.SetTop(pixel, y); 

            drawLineCanvas.Children.Add(pixel); 
        }

        private void DrawPoint(int x, int y)
        {
            ConvertCordinates(ref x, ref y);

            Ellipse point = new Ellipse
            {
                Width = 5,
                Height = 5,
                Fill = new SolidColorBrush(Colors.Red)
            };

            Canvas.SetLeft(point, x - 2.5);
            Canvas.SetTop(point, y - 2.5); 

            drawLineCanvas.Children.Add(point);
        }

        private void ConvertCordinates(ref int x, ref int y)
        {
            y = -y;
            x += (int)(drawLineCanvas.Width / 2);
            y += (int)(drawLineCanvas.Height / 2);
        }
        private void ClearCanvas()
        {
            drawLineCanvas.Children.Clear();
        }

        private void drawLineButton_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
            int x1 = x1TextBox.Text != "" ? int.Parse(x1TextBox.Text) : 0;
            int y1 = y1TextBox.Text != "" ? int.Parse(y1TextBox.Text) : 0;
            int x2 = x2TextBox.Text != "" ? int.Parse(x2TextBox.Text) : 0;
            int y2 = y2TextBox.Text != "" ? int.Parse(y2TextBox.Text) : 0;
            DrawLine(x1, y1, x2, y2);
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9-]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}