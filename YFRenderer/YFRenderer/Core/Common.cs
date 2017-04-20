using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YFRenderer.Core
{
    public static class Common
    {
        public static int CanvasWidth = 800;
        public static int CanvasHeight = 600;

        public static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }

        public static double GetFractionalPart(double num)
        {
            return (num - Math.Truncate(num));
        }

      
    }
}
