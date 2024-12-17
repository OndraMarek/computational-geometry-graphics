using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

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
            ClearCanvas();
            int x = string.IsNullOrEmpty(xTextBox.Text) ? 0 : int.Parse(xTextBox.Text);
            int y = string.IsNullOrEmpty(yTextBox.Text) ? 0 : int.Parse(yTextBox.Text);
            int radius1 = string.IsNullOrEmpty(radius1TextBox.Text) ? 0 : int.Parse(radius1TextBox.Text);
            int radius2 = string.IsNullOrEmpty(radius2TextBox.Text) ? 0 : int.Parse(radius2TextBox.Text);
            AnimateRollingCircles(x, y, radius1, radius2);
        }

        private void StartRollingAnimation(UIElement circle, int xCenter, int yCenter, int radius1, int radius2)
        {
            int orbitRadius = radius1 + radius2;
            double orbitDuration = 5000;

            DoubleAnimationUsingKeyFrames animationX = CreateOrbitAnimation(xCenter, orbitRadius, radius2, orbitDuration, true);
            DoubleAnimationUsingKeyFrames animationY = CreateOrbitAnimation(yCenter, orbitRadius, radius2, orbitDuration, false);

            Storyboard orbitStoryboard = new Storyboard();
            SetupStoryboard(orbitStoryboard, circle, animationX, "(Canvas.Left)");
            SetupStoryboard(orbitStoryboard, circle, animationY, "(Canvas.Top)");

            orbitStoryboard.Begin();

            StartRotationAnimation(circle, orbitDuration, radius1, radius2);
        }

        private void StartRotationAnimation(UIElement circle, double orbitDuration, int radius1, int radius2)
        {
            circle.RenderTransform = new RotateTransform(0, radius2, radius2);

            double rotations = (radius1 + radius2) / radius2;

            DoubleAnimation rotationAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360 * rotations,
                Duration = TimeSpan.FromMilliseconds(orbitDuration),
                RepeatBehavior = RepeatBehavior.Forever
            };

            Storyboard rotationStoryboard = new Storyboard();
            SetupStoryboard(rotationStoryboard, circle, rotationAnimation, "(UIElement.RenderTransform).(RotateTransform.Angle)");

            rotationStoryboard.Begin();
        }

        private DoubleAnimationUsingKeyFrames CreateOrbitAnimation(int center, int orbitRadius, int radius2, double duration, bool isX)
        {
            DoubleAnimationUsingKeyFrames animation = new DoubleAnimationUsingKeyFrames
            {
                RepeatBehavior = RepeatBehavior.Forever
            };

            for (int i = 0; i <= 360; i++)
            {
                double angleInRadians = i * Math.PI / 180;
                double offset = orbitRadius * (isX ? Math.Cos(angleInRadians) : Math.Sin(angleInRadians)) - radius2;
                animation.KeyFrames.Add(
                    new EasingDoubleKeyFrame(
                        center + offset, KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(i * duration / 360))
                        )
                    );
            }
            return animation;
        }

        private void SetupStoryboard(Storyboard storyboard, UIElement target, DoubleAnimationBase animation, string targetProperty)
        {
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, new PropertyPath(targetProperty));
        }
    }
}