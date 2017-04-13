using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFRenderer.Primitives
{
    class Vector2d
    {
        public float x;
        public float y;

        public Vector2d(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2d operator +(Vector2d c1, Vector2d c2)
        {
            return new Vector2d(c1.x + c2.x, c1.y + c2.y);
        }

        public static Vector2d operator -(Vector2d c1, Vector2d c2)
        {
            return new Vector2d(c1.x - c2.x, c1.y - c2.y);
        }
       
        public static double operator |(Vector2d c1, Vector2d c2)
        {
            return c1.x * c2.x + c1.y * c2.y;
        }

        public static double DotProduct(Vector2d c1, Vector2d c2)
        {
            return c1 | c2;
        }

        public static double operator ^(Vector2d c1, Vector2d c2)
        {
            return c1.x * c2.y - c1.y * c2.x;
        }

        public static double CrossProduct(Vector2d c1, Vector2d c2)
        {
            return c1^c2;
        }

        public void DrawLine(Vector2d c1, Vector2d c2)
        {

        }
     
    }
}
