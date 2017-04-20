using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using YFRenderer.Primitives;
 

namespace YFRenderer.Core
{
   public class RenderBuffer
    {
        private static RenderBuffer instance;

        private Color DefaultColor = (Color)ColorConverter.ConvertFromString("White");

        private RenderBuffer() { }

        //用一个数组模拟缓冲区
        private static byte[,,] pixels = new byte[Common.CanvasHeight, Common.CanvasWidth, 4];

        public static RenderBuffer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RenderBuffer();
                }
                return instance;
            }
        }

        //画像素
        public void SetPixel(int x, int y, Color color, float brightness)
        {
            if (x >= pixels.GetLength(1) || y >= pixels.GetLength(0))
                return;
            color.ScA = color.ScA * brightness;

            pixels[y, x, 0] = color.R;
            pixels[y, x, 1] = color.G;
            pixels[y, x, 2] = color.B;
            pixels[y, x, 3] = color.A;
        }

        public void SetPixel(int x, int y)
        {
            SetPixel(x, y, DefaultColor, 1);
        }

        public void SetPixel(float x, float y)
        {
            SetPixel((int)x, (int)y, DefaultColor, 1);
        }

        public void SetPixel(int x, int y, float brightness)
        {
            SetPixel(x, y, DefaultColor, brightness);
        }

        public Color GetPixelColor(int x , int y)
        {
            Color c = new Color();
            c = Color.FromScRgb(0,0,0,0);
            if (x >= pixels.GetLength(1) || y >= pixels.GetLength(0))
                return c;

            c.R = pixels[y, x, 0];
            c.G = pixels[y, x, 1];
            c.B = pixels[y, x, 2];
            c.A = pixels[y, x, 3];

            return c;
        }

        //填充缓冲区
        public void clearBuffer()
        {
            for (int row = 0; row < Common.CanvasHeight; row++)
            {
                for (int col = 0; col < Common.CanvasWidth; col++)
                {
                    //RGB
                    for (int i = 0; i < 3; i++)
                        pixels[row, col, i] = 0;
                    //ALPHA
                    pixels[row, col, 3] = 255;
                }
            }
        }

        //输出缓冲区
        public void WriteToPixelArray(byte[] _pixelArray)
        {
            int index = 0;
            for (int row = 0; row < Common.CanvasHeight; row++)
            {
                for (int col = 0; col < Common.CanvasWidth; col++)
                {
                    for (int i = 0; i < 4; i++)
                        _pixelArray[index++] = pixels[row, col, i];
                }
            }
        }

        public Vector2d Project(Vector3d coord, Primitives.Matrix transMat)
        {
            // 进行坐标变换  
            var point = Vector3d.TransformCoordinate(coord, transMat);

            // 变换后的坐标起始点是坐标系的中心点  
            // 但是，在屏幕上，我们以左上角为起始点  
            // 我们需要重新计算使他们的起始点变成左上角  
            var x = point.x * Common.CanvasWidth + Common.CanvasWidth / 2.0f;
            var y = -point.y * Common.CanvasHeight + Common.CanvasHeight / 2.0f;
            return (new Vector2d(x, y));
        }

        public void Render(Camera camera, params Mesh[] meshes)
        {
            //观察矩阵
            var viewMatrix = Primitives.Matrix.LookAtLH(camera.Position, camera.Target, Vector3d.UnitY);
            //投影矩阵
            var projectionMatrix = Primitives.Matrix.PerspectiveFovLH(0.78f,
                                                           (float)Common.CanvasWidth / Common.CanvasHeight,
                                                           0.01f, 1.0f);

            foreach (Mesh mesh in meshes)
            {
                //  世界矩阵
                var worldMatrix = Primitives.Matrix.RotationYawPitchRoll(mesh.Rotation.y,
                                                              mesh.Rotation.x, mesh.Rotation.z) *
                                  Primitives.Matrix.Translation(mesh.Position);

                var transformMatrix = worldMatrix * viewMatrix * projectionMatrix;

                foreach (var face in mesh.Faces)
                {
                    var vertexA = mesh.Vertices[face.A];
                    var vertexB = mesh.Vertices[face.B];
                    var vertexC = mesh.Vertices[face.C];

                    var pixelA = Project(vertexA, transformMatrix);
                    var pixelB = Project(vertexB, transformMatrix);
                    var pixelC = Project(vertexC, transformMatrix);

                    Draw.Draw2DSegement3(pixelA, pixelB);
                    Draw.Draw2DSegement3(pixelB, pixelC);
                    Draw.Draw2DSegement3(pixelC, pixelA);
                }
            }
        }

    }
}
