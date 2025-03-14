using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace RayBoxIntersection
{
    public partial class MainWindow : Window
    {
        private int[] b;
        private int[] v;

        private int[] a;
        private int[] q;

        public MainWindow()
        {
            b = [ 0, 0, 0 ];
            v = [50, 100, 150];

            a = [-60, -60, -60];
            q = [-1, -1, -1];

            InitializeComponent();

            DrawCube();
            SetupCamera();
        }

        private void DrawCube()
        {
            var lines = new LinesVisual3D { Color = Colors.Blue, Thickness = 2 };

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

        private void SetupCamera()
        {
            Point3D center = new Point3D(
                b[0] + v[0] / 2.0,
                b[1] + v[1] / 2.0,
                b[2] + v[2] / 2.0
            );

            double maxDimension = Math.Max(Math.Max(v[0], v[1]), v[2]);
            double cameraDistance = maxDimension * 2;

            Point3D cameraPosition = new Point3D(
                center.X + cameraDistance,
                center.Y + cameraDistance,
                center.Z + cameraDistance
            );

            PerspectiveCamera camera = new PerspectiveCamera
            {
                Position = cameraPosition,
                LookDirection = new Vector3D(
                    center.X - cameraPosition.X,
                    center.Y - cameraPosition.Y,
                    center.Z - cameraPosition.Z
                ),
                UpDirection = new Vector3D(0, 1, 0),
                FieldOfView = 60
            };

            viewport.Camera = camera;
        }

        private bool RayIntersectsCuboid()
        {
            return true;
        }

        private void testIntersection_Click(object sender, RoutedEventArgs e)
        {
            bool intersects = RayIntersectsCuboid();
            MessageBox.Show($"Ray intersects cuboid: {intersects}", "Intersection Test");
        }
    }
}