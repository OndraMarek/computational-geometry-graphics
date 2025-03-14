using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace RayBoxIntersection
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            int[] b = [0, 0, 0];
            int[] v = [50, 100, 150];


            InitializeComponent();

            DrawCube(b[0], b[1], b[2], v[0], v[1], v[2]);
            SetupCamera(b[0], b[1], b[2], v[0], v[1], v[2]);
        }

        private void DrawCube(int posX, int posY, int posZ, int width, int length, int height)
        {
            var lines = new LinesVisual3D { Color = Colors.Blue, Thickness = 2 };

            Point3D[] vertices =
            [
                new Point3D(posX, posY, posZ),
                new Point3D(posX + width, posY, posZ),
                new Point3D(posX + width, posY + length, posZ),
                new Point3D(posX, posY + length, posZ),
                new Point3D(posX, posY, posZ + height),
                new Point3D(posX + width, posY, posZ + height),
                new Point3D(posX + width, posY + length, posZ + height),
                new Point3D(posX, posY + length, posZ + height)
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

        private void SetupCamera(int posX, int posY, int posZ, int width, int length, int height)
        {
            Point3D center = new Point3D(
                posX + width / 2.0,
                posY + length / 2.0,
                posZ + height / 2.0
            );

            double maxDimension = Math.Max(Math.Max(width, length), height);
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
    }
}
