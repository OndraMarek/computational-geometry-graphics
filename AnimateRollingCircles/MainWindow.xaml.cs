using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnimateRollingCircles
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        public void AnimateRollingCircles(int xCenter, int yCenter, int radius1, int radius2)
        {
            ConvertCoordinates(ref xCenter, ref yCenter);
            Canvas circle1 = DrawCircle(xCenter, yCenter, radius1);
            Canvas circle2 = DrawCircle(xCenter + radius1 + radius2, yCenter, radius2);
            animateRollingCirclesCanvas.Children.Add(circle1);
            animateRollingCirclesCanvas.Children.Add(circle2);
            StartRotationAnimation(circle2);
            StartOrbitAnimation(circle2, xCenter, yCenter, radius1, radius2);
        }

        private Canvas DrawCircle(int xCenter, int yCenter, int radius)
        {
            Canvas groupCanvas = new Canvas();

            groupCanvas.Children.Add(DrawEllipse(radius));
            groupCanvas.Children.Add(DrawLine(0, radius, radius * 2, radius));
            groupCanvas.Children.Add(DrawLine(radius, 0, radius, radius * 2));

            Canvas.SetLeft(groupCanvas, xCenter - radius);
            Canvas.SetTop(groupCanvas, yCenter - radius);

            groupCanvas.RenderTransform = new RotateTransform(0, radius, radius);

            return groupCanvas;
        }

        private Ellipse DrawEllipse(int radius)
        {
            Ellipse ellipse = new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
            Canvas.SetLeft(ellipse, 0);
            Canvas.SetTop(ellipse, 0);
            return ellipse;
        }

        private Line DrawLine(int x1, int y1, int x2, int y2)
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
            return line;
        }

        private void ConvertCoordinates(ref int x, ref int y)
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

        private void StartRotationAnimation(UIElement element)
        {
            Storyboard storyboard = (Storyboard)FindResource("RotateAnimation");
            Storyboard.SetTarget(storyboard, element);
            storyboard.Begin();
        }

        private void StartOrbitAnimation(UIElement element, int xCenter, int yCenter, int radius1, int radius2)
        {
            int orbitRadius = radius1 + radius2;
            double angle = 0;

            DispatcherTimer timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(40)
            };
            timer.Tick += (s, e) =>
            {
                angle += 1.7;
                if (angle >= 360) angle = 0;

                double radians = angle * Math.PI / 180;
                double newX = xCenter + orbitRadius * Math.Cos(radians) - radius2;
                double newY = yCenter + orbitRadius * Math.Sin(radians) - radius2;

                Canvas.SetLeft(element, newX);
                Canvas.SetTop(element, newY);
            };
            timer.Start();
        }
    }
}
