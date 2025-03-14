using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RayBoxIntersection
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DrawCuboid([0, 0, 0], [200, 100, 100]);
        }

        private void ConvertCoordinates(ref double x, ref double y)
        {
            y = -y;
            x += drawCanvas.Width / 2;
            y += drawCanvas.Height / 2;
        }

        private void ClearCanvas()
        {
            drawCanvas.Children.Clear();
        }

        private void DrawCuboid(double[] b, double[] v)
        {
            double x = b[0];
            double y = b[1];
            double z = b[2];
            double width = v[0];
            double height = v[1];
            double depth = v[2];

            ConvertCoordinates(ref x, ref y);

            x = x + (z / 2);
            y = y - (z / 2);

            double offsetX = depth / 2;
            double offsetY = depth / 2;

            double x1 = x;
            double y1 = y + height;
            double x2 = x + width;
            double y2 = y + height;
            double x3 = x + width;
            double y3 = y;
            double x4 = x;
            double y4 = y;

            double x5 = x + offsetX;
            double y5 = y + height - offsetY;
            double x6 = x + width + offsetX;
            double y6 = y + height - offsetY;
            double x7 = x + width + offsetX;
            double y7 = y - offsetY;
            double x8 = x + offsetX;
            double y8 = y - offsetY;

            DrawLine(x1, y1, x2, y2);
            DrawLine(x2, y2, x3, y3);
            DrawLine(x3, y3, x4, y4);
            DrawLine(x4, y4, x1, y1);

            DrawLine(x5, y5, x6, y6, true);
            DrawLine(x6, y6, x7, y7);
            DrawLine(x7, y7, x8, y8);
            DrawLine(x8, y8, x5, y5, true);

            DrawLine(x1, y1, x5, y5, true);
            DrawLine(x2, y2, x6, y6);
            DrawLine(x3, y3, x7, y7);
            DrawLine(x4, y4, x8, y8);
        }

        private void DrawLine(double x1, double y1, double x2, double y2, bool isDashed = false)
        {
            Line line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            if (isDashed)
            {
                line.StrokeDashArray = new DoubleCollection() { 4, 4 };
            }
            drawCanvas.Children.Add(line);
        }
    }
}