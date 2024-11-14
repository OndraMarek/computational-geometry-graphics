using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DrawBresenhamLine
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DrawLine(0, 0, 100, 200);
        }

        public void DrawLine(int x1, int y1, int x2, int y2)
        {

            DrawPoint(x1, y1);
            DrawPoint(x2, y2);

            ConvertCordinates(ref x1, ref y1);
            ConvertCordinates(ref x2, ref y2);
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
    }
}