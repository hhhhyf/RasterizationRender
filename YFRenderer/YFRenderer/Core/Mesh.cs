using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YFRenderer.Primitives;

namespace YFRenderer.Core
{
    public struct Face
    {
        public int A;
        public int B;
        public int C;
    }

    public class Mesh
    {
        public string Name { get; set; }
        //顶点
        public Vector3d[] Vertices { get; private set; }
        //面
        public Face[] Faces { get; set; }  

        public Vector3d Position { get; set; }
        public Vector3d Rotation { get; set; }

        public Mesh(string name, int verticesCount, int facesCount)
        {
            Vertices = new Vector3d[verticesCount];
            Name = name;
            Position = new Vector3d(0, 0, 0);
            Rotation = new Vector3d(0, 0, 0);
            Faces = new Face[facesCount];
        }
    }
}
