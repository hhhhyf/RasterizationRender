using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFRenderer.Core;
using System.Windows.Media;

namespace YFRenderer.Primitives
{
    class Edge
    {
       public int ymax; //边上端的的纵坐标
       public float x;  //下端点的X坐标
       public float dx; //斜率的倒数
    };

    public class Polygon2d
    {
        public List<Vector2d> Vertices = new List<Vector2d>();
        List<List<Edge>> EdgeTable = new List<List<Edge>>();

        int ymin = 0;
        int ymax = 0;

        //scanline fill
        public void SacnlineFill()
        {
            if (Vertices.Count == 0)
                return;

            ymin = (int)Vertices[0].y;
            ymax = (int)Vertices[0].y;
            for (int i = 0; i < Vertices.Count; i++)
            {
                if (Vertices[i].y > ymax)
                {
                    ymax = (int)Vertices[i].y;
                }

                if (Vertices[i].y < ymin)
                {
                    ymin = (int)Vertices[i].y;
                }
            }
             

            for (int i = 0; i < ymax - ymin + 1; i++)
            {
                List<Edge> list = new List<Edge>();
                EdgeTable.Add(list);
            }

            InitScanLineNewEdgeTable();
            HorizonEdgeFill();
            ProcessScanLineFill();
        }

        void InitScanLineNewEdgeTable()
        {

            for (int i = 0; i < Vertices.Count; i++)
            {
                Vector2d ps = Vertices[i];
                Vector2d pe = Vertices[(i + 1) % Vertices.Count];
                Vector2d pss = Vertices[(i - 1 + Vertices.Count) % Vertices.Count];
                Vector2d pee = Vertices[(i + 2) % Vertices.Count];
                Edge e = new Edge();
                //前一个点和当前点不水平
                if (pe.y != ps.y)
                {
                    e.dx = (float)(pe.x - ps.x) / (float)(pe.y - ps.y);
                    //后面一个点比当前点高
                    if (pe.y > ps.y)
                    {
                        e.x = ps.x;
                        //连续单调递减的三个点，中间点要分开，不然水平扫描的时候就奇数个交点了。
                        if (pee.y >= pe.y)
                            e.ymax = (int)pe.y - 1;
                        else
                            e.ymax = (int)pe.y;

                        EdgeTable[(int)ps.y - ymin].Add(e);
                    }
                    else
                    {
                        e.x = pe.x;
                        if (pss.y >= ps.y)
                            e.ymax = (int)ps.y - 1;
                        else
                            e.ymax = (int)ps.y;
                        EdgeTable[(int)pe.y - ymin] .Add(e);
                    }
                }
            }
        }

        void InsertNetListToAet (List<Edge> edgeList, ref List<Edge> AET )
        {
            for(int i = 0; i < edgeList.Count; i++)
            {     
                AET.Add(edgeList[i]);            
            }
        }

        void FillAetScanLine(List<Edge> AET, int y)
        {
            if (AET.Count <= 1) return;
            for (int i =0; i < AET.Count; i++)
            {
                int x1 = (int)AET[i].x;
                i++;
                int x2 = (int)AET[i].x;
                if (x1 > x2)
                    Common.Swap<int>(ref x1, ref x2);
                for(int width = x1; width <= x2; width++)
                {
                    RenderBuffer.Instance.SetPixel(width, y);
                }
            }
        }

        void RemoveNonActiveEdgeFromAet(ref List<Edge> AET, int y)
        {
            for (int i = 0; i < AET.Count; i++)
            {
                if (AET[i].ymax == y)
                {
                    AET.RemoveAt(i);
                }
            }
        }

        void UpdateAndResortAet(ref List<Edge> AET)
        {
            for (int i = 0; i < AET.Count; i++)
            {
                AET[i].x += AET[i].dx;
            }


            AET.Sort(delegate (Edge a, Edge b)
            {
                if (a.x <= b.x)
                    return 1;
                else
                    return -1;
            });
        }

        void HorizonEdgeFill()
        {
            for(int i=0; i < Vertices.Count; i++)
            {
                Vector2d StartPoint = Vertices[i];
                Vector2d EndPoint = Vertices[(i + 1) % Vertices.Count];
                if (StartPoint.y == EndPoint.y)
                    Draw.Draw2DSegement(StartPoint, EndPoint);
            }
        }

        void ProcessScanLineFill()
        {
            List<Edge> AET = new List<Edge>();
            for (int y = ymin; y <= ymax; y++)
            {
                InsertNetListToAet(EdgeTable[y - ymin],ref AET);
                FillAetScanLine(AET, y);
                RemoveNonActiveEdgeFromAet(ref AET, y);
                UpdateAndResortAet(ref AET);
            }
        }

        //Flood Fill 
        public void FloodFill()
        {
            if (Vertices.Count == 0)
                return;

            for (int i = 0; i < Vertices.Count; i++)
            {
                Draw.Draw2DSegement(Vertices[i], Vertices[(i + 1) % Vertices.Count]);
            }

            Stack<Vector2d> q = new Stack<Vector2d>();
            q.Push(new Vector2d(Vertices[0].x, Vertices[0].y));
            Color c = (Color)ColorConverter.ConvertFromString("White");

            while (q.Count > 0 )
            {
                Vector2d v = q.Pop();
                List<Vector2d> list = new List<Vector2d>();

                //4个方向
                list.Add(new Vector2d(v.x + 1, v.y));
                list.Add(new Vector2d(v.x - 1, v.y));
                list.Add(new Vector2d(v.x , v.y + 1));
                list.Add(new Vector2d(v.x , v.y - 1));

                RenderBuffer.Instance.SetPixel(v.x, v.y);
                for(int i=0; i < list.Count; i++)
                {
                    if (RenderBuffer.Instance.GetPixelColor((int)list[i].x, (int)list[i].y) != c && Draw.IsPointInPolygon(list[i], Vertices))
                    {
                        q.Push(list[i]);
                    }
                }    
            }

        }

        void FloodFillImplement(int x, int y)
        {
            //递归会堆栈溢出。。。
            Vector2d v = new Vector2d(x, y);
            if (!Draw.IsPointInPolygon(v, Vertices))
                return;
            Color c = (Color)ColorConverter.ConvertFromString("White");
            if (RenderBuffer.Instance.GetPixelColor(x, y) != c)
                return;

            RenderBuffer.Instance.SetPixel(x, y);

            FloodFillImplement(x + 1, y );
            FloodFillImplement(x - 1, y);
            FloodFillImplement(x, y +1 );
            FloodFillImplement(x, y -1);
        }
    }

}
