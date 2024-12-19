using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnimateRollingCircles
{
    public partial class MainWindow : Window
    {
        private double angle = 0;
        private DispatcherTimer timer;

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

            StartRollingAnimation(circle2, xCenter, yCenter, radius1, radius2);
        }

        private Canvas DrawCircle(int xCenter, int yCenter, int radius)
        {
            Canvas groupCanvas = new Canvas();

            groupCanvas.Children.Add(DrawEllipse(radius));
            groupCanvas.Children.Add(DrawLine(0, radius, radius * 2, radius));
            groupCanvas.Children.Add(DrawLine(radius, 0, radius, radius * 2));

            Canvas.SetLeft(groupCanvas, xCenter - radius);
            Canvas.SetTop(groupCanvas, yCenter - radius);

            return groupCanvas;
        }

        private Ellipse DrawEllipse(int radius)
        {
            return new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
        }

        private Line DrawLine(int x1, int y1, int x2, int y2)
        {
            return new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
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
            SetDefaultValues(xTextBox, "0");
            SetDefaultValues(yTextBox, "0");
            SetDefaultValues(radius1TextBox, "50");
            SetDefaultValues(radius2TextBox, "50");

            ClearCanvas();
            int x = int.Parse(xTextBox.Text);
            int y = int.Parse(yTextBox.Text);
            int radius1 = int.Parse(radius1TextBox.Text);
            int radius2 = int.Parse(radius2TextBox.Text);
            AnimateRollingCircles(x, y, radius1, radius2);
        }

        private void SetDefaultValues(TextBox textBox, string defaultValue)
        {
            if (string.IsNullOrEmpty(textBox.Text) || !int.TryParse(textBox.Text, out _))
            {
                textBox.Text = defaultValue;
            }
        }

        private void StartRollingAnimation(Canvas circle2, int xCenter, int yCenter, int radius1, int radius2)
        {
            if (timer != null)
            {
                timer.Stop();
            }

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(16);
            timer.Tick += (sender, e) => Timer_Tick(sender, e, circle2, xCenter, yCenter, radius1, radius2);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e, Canvas circle2, int xCenter, int yCenter, int radius1, int radius2)
        {
            angle = (angle + 2) % 360;

            double radians = angle * Math.PI / 180;
            double orbitRadius = radius1 + radius2;
            double x = xCenter + orbitRadius * Math.Cos(radians) - radius2;
            double y = yCenter + orbitRadius * Math.Sin(radians) - radius2;

            Canvas.SetLeft(circle2, x);
            Canvas.SetTop(circle2, y);

            double rotationAngle = CalculateRotationAngle(orbitRadius, radians, radius2);

            ApplyRotation(circle2, rotationAngle, radius2);
        }

        private double CalculateRotationAngle(double orbitRadius, double radians, int radius2)
        {
            double circumference = 2 * Math.PI * radius2;
            double distance = orbitRadius * radians;
            return (distance / circumference) * 360;
        }

        private void ApplyRotation(Canvas circle2, double rotationAngle, int radius2)
        {
            RotateTransform rotateTransform = new RotateTransform(rotationAngle, radius2, radius2);
            circle2.RenderTransform = rotateTransform;
        }
    }
}