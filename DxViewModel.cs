
using System.IO;
using CookieEdit2.DXCadView;
using HelixToolkit.Wpf.SharpDX;
using SharpDX;

using Media3D = System.Windows.Media.Media3D;
using Point3D = System.Windows.Media.Media3D.Point3D;
using Vector3D = System.Windows.Media.Media3D.Vector3D;
using Transform3D = System.Windows.Media.Media3D.Transform3D;
using Color = System.Windows.Media.Color;
using Plane = SharpDX.Plane;
using Vector3 = SharpDX.Vector3;
using Colors = System.Windows.Media.Colors;
using Color4 = SharpDX.Color4;

namespace CookieEdit2
{
    public class DxViewModel : BaseViewModel
    {
        public Vector3 UpDirVector => new Vector3(0,1,1);
        public Vector3D UpDirVectorD => new Vector3D(0,0,1);
        public Vector3 LightDir { get; private set; }
        public Color LightColor = Colors.White;

        public LineGeometry3D TableGrid { get; private set; }
        public Color TableGridColor { get; private set; }
        public Transform3D TableXform { get; private set; }

        public LineGeometry3D TableGrid2 { get; private set; }
        public Color TableGridColor2 { get; private set; }
        public Transform3D TableXform2 { get; private set; }

        public Stream BackgroundTexture { get; }

        public DxViewModel()
        {
            EffectsManager = new DefaultEffectsManager();

            TableGrid = LineBuilder.GenerateGrid(UpDirVector,-24,24,-12,12);
            TableGridColor = Color.FromArgb(85, 229, 229, 229);
            TableXform = new Media3D.TranslateTransform3D(0, 0, -2);

            TableGrid2 = LineBuilder.GenerateGrid(UpDirVector,-2,2,-1,1);
            TableGridColor2 = Color.FromArgb(85, 65, 65, 65);
            TableXform2 = new Media3D.TranslateTransform3D(0, 0, -2).PrependTransform(new Media3D.ScaleTransform3D(12,12,12));

            Camera = new PerspectiveCamera()
            {
                Position = new Point3D(3, -10, 7),
                FarPlaneDistance = 1000,
                LookDirection = new Vector3D(-3, 10, -7),
                UpDirection = UpDirVectorD
            };

            BackgroundTexture =
                BitmapExtensions.CreateLinearGradientBitmapStream(EffectsManager, 128, 128, Direct2DImageFormat.Bmp,
                    new Vector2(0, 0), new Vector2(0, 128), new SharpDX.Direct2D1.GradientStop[]
                    {
                        new SharpDX.Direct2D1.GradientStop(){ Color = Color.FromRgb(19, 19, 19).ToColor4(), Position = 0f },
                        new SharpDX.Direct2D1.GradientStop(){ Color = Color.FromRgb(47, 47, 47).ToColor4(), Position = 1f }
                    });


        }
    }
}
