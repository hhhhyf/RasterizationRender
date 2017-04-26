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

    public struct Vertex
    {
        public Vector3d Normal;
        public Vector3d Coordinates;
        public Vector3d WorldCoordinates;
        public Vector2d TextureCoordinates;
    }

    public struct ScanLineData
    {
        public int currentY;
        
        public float ndotla;
        public float ndotlb;
        public float ndotlc;
        public float ndotld;

        public float ua;
        public float ub;
        public float uc;
        public float ud;

        public float va;
        public float vb;
        public float vc;
        public float vd;
    }
    
    public class Mesh
    {
        public string Name { get; set; }
        //顶点
        public Vertex[] Vertices { get; private set; }
        //面
        public Face[] Faces { get; set; }
        //纹理
        public Texture Texture { get; set; }

        public Vector3d Position { get; set; }
        public Vector3d Rotation { get; set; }

        public Mesh(string name, int verticesCount, int facesCount)
        {
            Vertices = new Vertex[verticesCount];
            Name = name;
            Position = new Vector3d(0, 0, 0);
            Rotation = new Vector3d(0, 0, 0);
            Faces = new Face[facesCount];
        }
    }
}
