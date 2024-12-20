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
        private double totalDistance = 0;
        private DispatcherTimer? timer;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void AnimateRollingCircles(double xCenter, double yCenter, double radius1, double radius2)
        {
            ConvertCoordinates(ref xCenter, ref yCenter);

            Canvas circle1 = DrawCircle(xCenter, yCenter, radius1);
            Canvas circle2 = DrawCircle(xCenter + radius1 + radius2, yCenter, radius2);

            animateRollingCirclesCanvas.Children.Add(circle1);
            animateRollingCirclesCanvas.Children.Add(circle2);

            StartRollingAnimation(circle2, xCenter, yCenter, radius1, radius2);
        }

        private Canvas DrawCircle(double xCenter, double yCenter, double radius)
        {
            Canvas groupCanvas = new Canvas();

            groupCanvas.Children.Add(DrawEllipse(radius));
            groupCanvas.Children.Add(DrawLine(0, radius, radius * 2, radius));
            groupCanvas.Children.Add(DrawLine(radius, 0, radius, radius * 2));

            Canvas.SetLeft(groupCanvas, xCenter - radius);
            Canvas.SetTop(groupCanvas, yCenter - radius);

            return groupCanvas;
        }

        private Ellipse DrawEllipse(double radius)
        {
            return new Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };
        }

        private Line DrawLine(double x1, double y1, double x2, double y2)
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

        private void ConvertCoordinates(ref double x, ref double y)
        {
            y = -y;
            x += animateRollingCirclesCanvas.Width / 2;
            y += animateRollingCirclesCanvas.Height / 2;
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
            ResetAnimation();

            SetDefaultValues(xTextBox, "0");
            SetDefaultValues(yTextBox, "0");
            SetDefaultValues(radius1TextBox, "50");
            SetDefaultValues(radius2TextBox, "50");

            ClearCanvas();

            double x = double.Parse(xTextBox.Text);
            double y = double.Parse(yTextBox.Text);
            double radius1 = Math.Abs(double.Parse(radius1TextBox.Text));
            double radius2 = Math.Abs(double.Parse(radius2TextBox.Text));
            AnimateRollingCircles(x, y, radius1, radius2);
        }

        private void SetDefaultValues(TextBox textBox, string defaultValue)
        {
            if (string.IsNullOrEmpty(textBox.Text) || !double.TryParse(textBox.Text, out _))
            {
                textBox.Text = defaultValue;
            }
        }

        private void StartRollingAnimation(Canvas circle2, double xCenter, double yCenter, double radius1, double radius2)
        {
            timer?.Stop();

            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };
            timer.Tick += (sender, e) => AnimateCircle(circle2, xCenter, yCenter, radius1, radius2);
            timer.Start();
        }

        private void AnimateCircle(Canvas circle2, double xCenter, double yCenter, double radius1, double radius2)
        {
            double previousAngle = angle;
            angle = (angle + 2) % 360;

            double radians = angle * Math.PI / 180;
            double orbitRadius = radius1 + radius2;

            double x = xCenter + orbitRadius * Math.Cos(radians) - radius2;
            double y = yCenter + orbitRadius * Math.Sin(radians) - radius2;

            Canvas.SetLeft(circle2, x);
            Canvas.SetTop(circle2, y);

            double rotationAngle = CalculateRotationAngle(ref totalDistance, orbitRadius, previousAngle, angle, radius2);

            ApplyRotation(circle2, rotationAngle, radius2);
        }

        private double CalculateRotationAngle(ref double totalDistance, double orbitRadius, double previousAngle, double currentAngle, double radius2)
        {
            double incrementalDistance = orbitRadius * (currentAngle - previousAngle) * Math.PI / 180;
            if (incrementalDistance < 0) incrementalDistance += 2 * Math.PI * orbitRadius;

            totalDistance += incrementalDistance;
            double circumference = 2 * Math.PI * radius2;

            return (totalDistance / circumference) * 360;
        }

        private void ApplyRotation(Canvas circle2, double rotationAngle, double radius2)
        {
            RotateTransform rotateTransform = new RotateTransform(rotationAngle, radius2, radius2);
            circle2.RenderTransform = rotateTransform;
        }

        private void ResetAnimation()
        {
            timer?.Stop();
            timer = null;

            angle = 0;
            totalDistance = 0;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            timer?.Stop();
            timer = null;
        }
    }
}
