using System.Windows.Media.Media3D;
using Microsoft.Win32.SafeHandles;

namespace CookieEdit2
{
    public struct Point3DInt
    {
        public int x, y, z;
        public Point3DInt(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static Point3DInt Zero => new Point3DInt(0,0,0);
        public static Point3DInt Up => new Point3DInt(0,0,1);
        public static Point3DInt Forward => new Point3DInt(0,1,0);
        public static Point3DInt Right => new Point3DInt(1,0,0);
        public static Point3DInt Down => new Point3DInt(0,0,-1);
        public static Point3DInt Back => new Point3DInt(0,-1,0);
        public static Point3DInt Left => new Point3DInt(-1,0,0);

        public static implicit operator Point3D(Point3DInt v) => new Point3D(v.x, v.y, v.z);
        public static implicit operator Point3DInt(Point3D v) => new Point3DInt((int)v.Z, (int)v.Y, (int)v.Z);

        public static implicit operator Vector3D(Point3DInt v) => new Vector3D(v.x, v.y, v.z);
        public static implicit operator Point3DInt(Vector3D v) => new Point3DInt((int)v.Z, (int)v.Y, (int)v.Z);
    }
}