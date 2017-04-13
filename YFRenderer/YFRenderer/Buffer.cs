using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;


namespace YFRenderer
{
    class Buffer
    {
        private static Buffer instance;

        private Buffer() { }
        //用一个数组模拟缓冲区
        private static byte[,,] pixels = new byte[Common.CanvasHeight, Common.CanvasWidth, 4];

        public static Buffer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Buffer();
                }
                return instance;
            }
        }

        //画像素
        public void SetPixel(int x, int y, Color color)
        {
            pixels[x, y, 0] = color.R;
            pixels[x, y, 1] = color.G;
            pixels[x, y, 2] = color.B;
            pixels[x, y, 3] = color.A;
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
