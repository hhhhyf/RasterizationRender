using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace YFRenderer.Primitives
{
    public class Vector3d
    {
        public float x;
        public float y;
        public float z;

        public static readonly Vector3d Zero = new Vector3d(0,0,0);

        public static readonly Vector3d UnitX = new Vector3d(1.0f, 0.0f, 0.0f);
        public static readonly Vector3d UnitY = new Vector3d(0.0f, 1.0f, 0.0f);
        public static readonly Vector3d UnitZ = new Vector3d(0.0f, 0.0f, 1.0f);

        public static readonly Vector3d One = new Vector3d(1.0f, 1.0f, 1.0f);

        public static readonly Vector3d Up = new Vector3d(0.0f, 1.0f, 0.0f);
        public static readonly Vector3d Down = new Vector3d(0.0f, -1.0f, 0.0f);
        public static readonly Vector3d Left = new Vector3d(-1.0f, 0.0f, 0.0f);
        public static readonly Vector3d Right = new Vector3d(1.0f, 0.0f, 0.0f);

        public Vector3d(float x1, float y1, float z1)
        {
            x = x1;
            y = y1;
            z = z1;
        }

        public static Vector3d TransformCoordinate( Vector3d coordinate,  Matrix transform)
        {
            Vector3d result;
            float X = (coordinate.x * transform.M11) + (coordinate.y * transform.M21) + (coordinate.z * transform.M31) + transform.M41;
            float Y = (coordinate.x * transform.M12) + (coordinate.y * transform.M22) + (coordinate.z * transform.M32) + transform.M42;
            float Z = (coordinate.x * transform.M13) + (coordinate.y * transform.M23) + (coordinate.z * transform.M33) + transform.M43;
            float W = 1f / ((coordinate.x * transform.M14) + (coordinate.y * transform.M24) + (coordinate.z * transform.M34) + transform.M44);
            result = new Vector3d(X * W, Y * W, Z * W);
            return result;
        }

        public float Length()
        {
            return (float)Math.Sqrt((x * x) + (y * y) + (z * z));
        }

        public static void Subtract(ref Vector3d left, ref Vector3d right, out Vector3d result)
        {
            result = new Vector3d(left.x - right.x, left.y - right.y, left.z - right.z);
        }


        public static Vector3d operator -(Vector3d left, Vector3d right)
        {
            return new Vector3d(left.x - right.x, left.y - right.y, left.z - right.z);
        }

        public static Vector3d operator +(Vector3d left, Vector3d right)
        {
            return new Vector3d(left.x + right.x, left.y + right.y, left.z + right.z);
        }

        public static Vector3d operator /(Vector3d left, float right)
        {
            return new Vector3d(left.x /right, left.y / right, left.z / right);
        }

        /// Converts the vector into a unit vector.
        public static void Normalize(ref Vector3d value, out Vector3d result)
        {
            result = value;
            result.Normalize();
        }

        public void Normalize()
        {
            float length = Length();
            if (length > 0)
            {
                float inv = 1.0f / length;
                x *= inv;
                y *= inv;
                z *= inv;
            }
        }

        public static void Cross(ref Vector3d left, ref Vector3d right, out Vector3d result)
        {
            result = new Vector3d(
                (left.y * right.z) - (left.z * right.y),
                (left.z * right.x) - (left.x * right.z),
                (left.x * right.y) - (left.y * right.x));
        }

        public static void Dot(ref Vector3d left, ref Vector3d right, out float result)
        {
            result = (left.x * right.x) + (left.y * right.y) + (left.z * right.z);
        }

        public static float Dot(Vector3d left, Vector3d right)
        {
            return (left.x * right.x) + (left.y * right.y) + (left.z * right.z);
        }
    }
}
