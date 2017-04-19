using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFRenderer.Primitives;


namespace YFRenderer.Core
{
    public static class Draw
    {
        //Bresenham直线算法  我感觉就是微分的思想。。
        public static void Draw2DSegement(Vector2d P1, Vector2d P2)
        {
            Vector2d StartPoint = new Vector2d(P1.x, P1.y);
            Vector2d EndPoint = new Vector2d(P2.x, P2.y);

            bool bSteep = Math.Abs(EndPoint.y - StartPoint.y) > Math.Abs(EndPoint.x - StartPoint.x);
            if (bSteep)
            {
                //如果斜率小于0就把xy坐标互换
                Common.Swap<int>(ref StartPoint.x, ref StartPoint.y);
                Common.Swap<int>(ref EndPoint.x, ref EndPoint.y);
            }

            //保证从左往右画
            if (StartPoint.x > EndPoint.x)
            {
                Common.Swap<int>(ref StartPoint.x, ref EndPoint.x);
                Common.Swap<int>(ref StartPoint.y, ref EndPoint.y);
            }

            int deltX = EndPoint.x - StartPoint.x;
            int deltY = Math.Abs(EndPoint.y - StartPoint.y);
            int error = deltX/2;
            int yStep;
            int y = StartPoint.y;
            if (StartPoint.y < EndPoint.y)
            {
                yStep = 1;
            }
            else
            {
                yStep = -1;
            }

            for (int x = StartPoint.x; x <= EndPoint.x; x++)
            {
                if(bSteep)
                {
                    RenderBuffer.Instance.SetPixel(y, x);
                }
                else
                {
                    RenderBuffer.Instance.SetPixel(x, y);
                }

                error = error - deltY;
                if(error < 0)
                {
                    y = y + yStep;
                    error = error + deltX;
                }
            }

          //  Console.WriteLine("draw line start: {0}, {1} end: {2} ,{3}", P1.x, P1.y, P2.x, P2.y);
        }

        //吴小林直线算法 
        public static void Draw2DSegement2(Vector2d P1, Vector2d P2)
        {
            Vector2d StartPoint = new Vector2d(P1.x, P1.y);
            Vector2d EndPoint = new Vector2d(P2.x, P2.y);

            double dx = EndPoint.x - StartPoint.x;
            double dy = EndPoint.y - StartPoint.y;

            bool bSteep = Math.Abs(EndPoint.y - StartPoint.y) > Math.Abs(EndPoint.x - StartPoint.x);
            //如果斜率小于0就把xy坐标互换
            if (Math.Abs(dx) < Math.Abs(dy))
            {
                Common.Swap<int>(ref StartPoint.x, ref StartPoint.y);
                Common.Swap<int>(ref EndPoint.x, ref EndPoint.y);
                Common.Swap<double>(ref dx, ref dy);
            }
            //保证从左往右画
            if (StartPoint.x > EndPoint.x)
            {
                Common.Swap<int>(ref StartPoint.x, ref EndPoint.x);
                Common.Swap<int>(ref StartPoint.y, ref EndPoint.y);
            }

            //处理左端点
            double gradient = dy / dx;
            double xend = Math.Round((double)StartPoint.x);
            double yend = StartPoint.y + gradient*(xend - StartPoint.x);
            double xgap = 1 - Common.GetFractionalPart(StartPoint.x + 0.5);
            int xpx11 = (int)xend;
            int ypxl1 = (int)yend;
            //下降的直线，画的时候xy互换
            if (bSteep)
            {
                RenderBuffer.Instance.SetPixel(ypxl1, xpx11, (float)(1 - Common.GetFractionalPart(yend) * xgap));
                RenderBuffer.Instance.SetPixel(ypxl1 + 1, xpx11, (float)(Common.GetFractionalPart(yend) * xgap));
            }
            else
            {

                RenderBuffer.Instance.SetPixel(xpx11, ypxl1, (float)(1 - Common.GetFractionalPart(yend) * xgap));
                RenderBuffer.Instance.SetPixel(xpx11, ypxl1 + 1, (float)(Common.GetFractionalPart(yend) * xgap));
            }

            double intery = yend + gradient;

             xend = Math.Round((double)EndPoint.x);
             yend = EndPoint.y + gradient * (xend - EndPoint.x);
             xgap = 1 - Common.GetFractionalPart(EndPoint.x + 0.5);
            int xpx12 = (int)xend;
            int ypxl2 = (int)yend;

            if (bSteep)
            {
                RenderBuffer.Instance.SetPixel(ypxl2, xpx12, (float)(1 - Common.GetFractionalPart(yend) * xgap));
                RenderBuffer.Instance.SetPixel(ypxl2 + 1, xpx12, (float)(Common.GetFractionalPart(yend) * xgap));
            }
            else
            {
                RenderBuffer.Instance.SetPixel(xpx12, ypxl2, (float)(1 - Common.GetFractionalPart(yend) * xgap));
                RenderBuffer.Instance.SetPixel(xpx12, ypxl2 + 1, (float)(Common.GetFractionalPart(yend) * xgap));
            }
          
            for(int x =xpx11 +1; x <= xpx12-1; x++)
            {
                if (bSteep)
                {
                    RenderBuffer.Instance.SetPixel((int)intery,x , (float)(1 - Common.GetFractionalPart(intery)));
                    RenderBuffer.Instance.SetPixel((int)intery + 1, x, (float)(Common.GetFractionalPart(intery)));
                }
                else
                {
                    RenderBuffer.Instance.SetPixel(x, (int)intery, (float)(1 - Common.GetFractionalPart(intery)));
                    RenderBuffer.Instance.SetPixel(x, (int)intery + 1, (float)(Common.GetFractionalPart(intery)));
                }
                intery = intery + gradient;
            }
        }

        //点在多边形内
        public static bool IsPointInPolygon(Vector2d Point, List<Vector2d> Vertices )
        {
            List<int> polyX = new List<int>();
            List<int> polyY = new List<int>();
            foreach(var p in Vertices)
            {
                polyX.Add(p.x);
                polyY.Add(p.y);
            }

            int x = Point.x;
            int y = Point.y;
            int polySides = polyX.Count;
            int i, j = polySides - 1;
            bool oddNodes = false;
            for (i = 0; i < polySides; i++)
            {
                if ((polyY[i] < y && polyY[j] >= y || polyY[j] < y && polyY[i] >= y) && (polyX[i] <= x || polyX[j] <= x))
                {
                    if (polyX[i] + (y - polyY[i]) / (polyY[j] - polyY[i]) * (polyX[j] - polyX[i]) < x)
                    {
                        oddNodes = !oddNodes;
                    }
                }
                j = i;
            }
            return oddNodes;

        }

        //逆时针排序  点乘根据角度判断
        public static void SortPoints(ref List<Vector2d> Vertices)
        {
            Vector2d center = new Vector2d(Vertices[0].x, Vertices[0].y);

            Vertices.Sort(delegate (Vector2d a, Vector2d b)
            {          
                if (a.x - center.x >= 0 && b.x - center.x < 0)
                    return 1;
                if (a.x - center.x < 0 && b.x - center.x>= 0)
                    return -1;
                if (a.x - center.x == 0 && b.x - center.x == 0)
                {
                    if (a.y - center.y >= 0 || b.y - center.y >= 0)
                    {
                        if (a.y > b.y)
                            return 1;
                        else
                            return -1;
                    }
                    if (b.y > a.y)
                        return 1;
                    else
                        return -1;
                }

                // compute the cross product of vectors (center -> a) x (center -> b)
                int det = (a.x - center.x) * (b.y - center.y) - (b.x - center.x) * (a.y - center.y);
                if (det < 0)
                    return 1;
                if (det > 0)
                    return -1;

                // points a and b are on the same line from the center
                // check which point is closer to the center
                int d1 = (a.x - center.x) * (a.x - center.x) + (a.y - center.y) * (a.y - center.y);
                int d2 = (b.x - center.x) * (b.y - center.x) + (b.y - center.y) * (b.y - center.y);
                if (d1 > d2)
                    return 1;
                else
                    return -1;

            });

        }
    }
}
