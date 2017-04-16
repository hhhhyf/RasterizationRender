using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


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

        public void SetPixel(int x, int y, float brightness)
        {
            SetPixel(x, y, DefaultColor, brightness);
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

    }
}
