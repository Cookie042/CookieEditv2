using System.Collections.Generic;
using System.IO;
using CookieEdit2.DXCadView;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;

using Media3D = System.Windows.Media.Media3D;
using Point3D = System.Windows.Media.Media3D.Point3D;
using Vector3D = System.Windows.Media.Media3D.Vector3D;
using Transform3D = System.Windows.Media.Media3D.Transform3D;
using Rotation3D = System.Windows.Media.Media3D.Rotation3D;

using Color = System.Windows.Media.Color;
using Plane = SharpDX.Plane;
using Vector3 = SharpDX.Vector3;
using Colors = System.Windows.Media.Colors;
using Color4 = SharpDX.Color4;

namespace CookieEdit2
{
    public class DxViewModel : BaseViewModel
    {
        public Vector3 UpDirVector => new Vector3(0, 1, 1);
        public Vector3D UpDirVectorD => new Vector3D(0, 0, 1);
        public Vector3 LightDir { get; private set; }
        public Color LightColor { get; set; } = Colors.White;

        public List<PointLight3D> PointLights { get; private set; } = new List<PointLight3D>();

        public Material MeshMaterial { get; private set; }
        public MeshGeometry3D BoxMesh { get; set; }
        public Transform3D BoxTransform { get; private set; }

        public Stream BackgroundTexture { get; }

        public GridViewModel GridView { get; set; }

        public DxViewModel()
        {
            EffectsManager = new DefaultEffectsManager();

            //TableGrid = LineBuilder.GenerateGrid(UpDirVector,-24,24,-12,12);
            //TableGridColor = Color.FromArgb(85, 229, 229, 229);
            //TableXform = new Media3D.TranslateTransform3D(0, 0, 0);

            //TableGrid2 = LineBuilder.GenerateGrid(UpDirVector,-2,2,-1,1);
            //TableGridColor2 = Color.FromArgb(85, 65, 65, 65);
            //TableXform2 = new Media3D.TranslateTransform3D(0, 0, 0).PrependTransform(new Media3D.ScaleTransform3D(12,12,12));

            GridView = new GridViewModel(
                Color.FromArgb(80, 0, 0, 0),
                Color.FromArgb(87, 80, 80, 80),
                new Point3D(0, 0, 0),
                Rotation3D.Identity,
                new Point3DInt(48, 24, 0),
                new Vector3D(0, 0, 1));
            GridView.MinorDivisions = 12;
            MeshBuilder mb = new MeshBuilder();
            mb.AddBox(Vector3.Zero, 48, 24, 1);
            BoxMesh = mb.ToMesh();

            BoxTransform = new Media3D.TranslateTransform3D(24, 12, -.51);

            MeshMaterial = PhongMaterials.White;

            var dir = new Vector3D(3, -10, 7);
            dir.Normalize();

            double distance = 48;

            Camera = new PerspectiveCamera()
            {
                Position = (Point3D) (dir * distance + new Vector3D(24,12,0)),
                FarPlaneDistance = 1000,
                LookDirection = -dir * distance,
                UpDirection = UpDirVectorD
            };

            BackgroundTexture =
                BitmapExtensions.CreateLinearGradientBitmapStream(EffectsManager, 128, 128, Direct2DImageFormat.Bmp,
                    new Vector2(0, 0), new Vector2(64, 128), new SharpDX.Direct2D1.GradientStop[]
                    {
                        new SharpDX.Direct2D1.GradientStop(){ Color = Color.FromRgb(19, 19, 19).ToColor4(), Position = 0f },
                        new SharpDX.Direct2D1.GradientStop(){ Color = Color.FromRgb(47, 47, 47).ToColor4(), Position = 1f }
                    });


        }
    }
}
