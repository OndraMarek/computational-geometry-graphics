using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace RayBoxIntersection
{
    public partial class MainWindow : Window
    {
        private double[] b;
        private double[] v;

        private double[] a;
        private double[] q;

        private Point3D? intersection;
        private PointsVisual3D? lastIntersectionPoint;

        public MainWindow()
        {
            b = [ 0, 0, 0 ];
            v = [0, 0, 0];

            a = [0, 0, 0];
            q = [0, 0, 0];

            InitializeComponent();
        }

        private void DrawCuboid()
        {
            viewport.Children.Clear();
            LinesVisual3D lines = new LinesVisual3D { Color = Colors.Black, Thickness = 1 };

            Point3D[] vertices =
            [
                new Point3D(b[0], b[1], b[2]),
                new Point3D(b[0] + v[0], b[1], b[2]),
                new Point3D(b[0] + v[0], b[1] + v[1], b[2]),
                new Point3D(b[0], b[1] + v[1], b[2]),
                new Point3D(b[0], b[1], b[2] + v[2]),
                new Point3D(b[0] + v[0], b[1], b[2] + v[2]),
                new Point3D(b[0] + v[0], b[1] + v[1], b[2] + v[2]),
                new Point3D(b[0], b[1] + v[1], b[2] + v[2])
            ];

            int[,] edges = new int[,]
            {
                {0,1}, {1,2}, {2,3}, {3,0},
                {4,5}, {5,6}, {6,7}, {7,4},
                {0,4}, {1,5}, {2,6}, {3,7}
            };

            for (int i = 0; i < edges.GetLength(0); i++)
            {
                lines.Points.Add(vertices[edges[i, 0]]);
                lines.Points.Add(vertices[edges[i, 1]]);
            }

            viewport.Children.Add(lines);
        }

        private void DrawIntersectionPoint()
        {
            if (intersection.HasValue)
            {
                if (lastIntersectionPoint != null)
                {
                    viewport.Children.Remove(lastIntersectionPoint);
                }
                var redPoint = new PointsVisual3D
                {
                    Color = Colors.Red,
                    Size = 5,
                    Points = new Point3DCollection { intersection.Value }
                };

                viewport.Children.Add(redPoint);
                lastIntersectionPoint = redPoint;
            }
        }

        private bool RayIntersectsCuboid()
        {
            double tn = 0.0;
            double tf = double.PositiveInfinity;

            for (int i = 0; i < 3; i++)
            {
                double tni = (b[i] - a[i]) / q[i];
                double tfi = (v[i] - a[i]) / q[i];

                if (tni > tfi)
                {
                    (tni, tfi) = (tfi, tni);
                }

                tn = Math.Max(tn, tni);
                tf = Math.Min(tf, tfi);

                if (tn > tf)
                {
                    intersection = null;
                    return false;
                }
            }

            if (tn >= 0 && tf >= 0)
            {
                intersection = new Point3D(a[0] + tn * q[0], a[1] + tn * q[1], a[2] + tn * q[2]);
                return true;
            }
            intersection = null;
            return false;
        }

        private void testIntersection_Click(object sender, RoutedEventArgs e)
        {
            a = [double.Parse(rayA0.Text), double.Parse(rayA1.Text), double.Parse(rayA2.Text)];
            q = [double.Parse(rayQ0.Text), double.Parse(rayQ1.Text), double.Parse(rayQ2.Text)];

            bool intersects = RayIntersectsCuboid();
            if (intersects)
            {
                DrawIntersectionPoint();
                MessageBox.Show($"Ray intersects cuboid (detected intersection market by red dot)", "Intersection Test");
            }
            else 
            {
                MessageBox.Show("Ray does not intersect cuboid", "Intersection Test");
            }
        }

        private void drawCuboidButton_Click(object sender, RoutedEventArgs e)
        {
            b = [ double.Parse(cuboidB0.Text), double.Parse(cuboidB0.Text), double.Parse(cuboidB0.Text) ];
            v = [double.Parse(cuboidV0.Text), double.Parse(cuboidV1.Text), double.Parse(cuboidV2.Text)];

            DrawCuboid();

            Point3D center = new Point3D(b[0] + v[0] / 2, b[1] + v[1] / 2, b[2] + v[2] / 2);
            double distance = Math.Max(v[0], Math.Max(v[1], v[2])) * 5;
            Point3D cameraPosition = new Point3D(center.X + distance, center.Y + distance, center.Z + distance);

            viewport.Camera.Position = cameraPosition;
            viewport.Camera.LookDirection = new Vector3D(center.X - cameraPosition.X, center.Y - cameraPosition.Y, center.Z - cameraPosition.Z);
            viewport.Camera.UpDirection = new Vector3D(0, 0, 1);
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}