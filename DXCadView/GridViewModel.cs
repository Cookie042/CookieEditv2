using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;
using Color = System.Windows.Media.Color;

namespace CookieEdit2.DXCadView
{
    public class GridViewModel
    {
        private int _minorDivisions = 5;

        public enum GridOrigin { Min, Center, Max }

        public Color GridColor1 { get; set; }
        public Color GridColor2 { get; set; }

        public Transform3D Xform { get; set; }

        public LineGeometry3D MajorGrid { get; set; }
        public LineGeometry3D MinorGrid { get; set; }

        public double MajorGridThickness { get; set; } = 1.5;
        public double MinorGridThickness { get; set; } = .5;

        public float UnitScale { get; set; } = 1;

        public int MinorDivisions
        {
            get => _minorDivisions;
            set
            {
                _minorDivisions = value;
                BuildLineGeo();
            }
        }

        public Point3DInt GridSize { get; set; }


        public GridViewModel(Color color1, Color color2, Point3D position, Rotation3D rotation, Point3DInt size, Vector3D normalAxis)
        {
            Console.WriteLine(UnitScale);

            GridColor1 = color1;
            GridColor2 = color2;

            GridSize = size;

            var transform = new MatrixTransform3D(Matrix3D.Identity);

            transform.Transform(position);
            transform.PrependTransform(new RotateTransform3D(rotation));
            transform.PrependTransform(new ScaleTransform3D(size));

            Xform = transform;

            BuildLineGeo();
        }

        private void BuildLineGeo()
        {
            double md = MinorDivisions;
            int xCount = (int)Math.Ceiling(GridSize.x / md + .01);
            int yCount = (int)Math.Ceiling(GridSize.y / md + .01);

            var lb = new LineBuilder();
            var s = ((Vector3D)GridSize).ToVector3();

            for (int x = 0; x < xCount; x++)
            {
                lb.AddLine(
                    new Vector3(x * MinorDivisions, 0, 0),
                    new Vector3(x * MinorDivisions, s.Y, 0));
            }

            for (int y = 0; y < yCount; y++)
            {
                lb.AddLine(
                    new Vector3(0, y * MinorDivisions, 0),
                    new Vector3(s.X, y * MinorDivisions, 0));
            }

            MajorGrid = lb.ToLineGeometry3D();

            lb = new LineBuilder();

            for (int x = 0; x < GridSize.x; x++)
            {
                if (x % MinorDivisions == 0) continue;
                lb.AddLine(
                    new Vector3(x, 0, 0),
                    new Vector3(x, s.Y, 0));
            }

            for (int y = 0; y < GridSize.y; y++)
            {
                if (y % MinorDivisions == 0) continue;
                lb.AddLine(
                    new Vector3(0, y, 0),
                    new Vector3(s.X, y, 0));
            }

            MinorGrid = lb.ToLineGeometry3D();
        }
    }
}
