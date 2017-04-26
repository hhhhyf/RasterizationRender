using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace YFRenderer.Core
{
    public class Material
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public string DiffuseTextureName { get; set; }
    }

    public class Texture
    {
        private byte[] internalBuffer;
        private int width;
        private int height;

        // 材质尺寸需要是2的次方（如：512x512、1024x1024等）  
        public Texture(string filename, int width, int height)
        {
            this.width = width;
            this.height = height;
            Load(filename);
        }

        void Load(string filename)
        {
            string path = Directory.GetCurrentDirectory() + "/" + filename;

            using (BinaryReader loader = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                FileInfo fd = new FileInfo(path);
                int Length = (int)fd.Length;
                byte[] buf = new byte[Length];
                buf = loader.ReadBytes((int)fd.Length);
                loader.Dispose();
                loader.Close();

                //开始加载图像  
                BitmapImage bim = new BitmapImage();
                bim.BeginInit();
                bim.StreamSource = new MemoryStream(buf);
                bim.EndInit();
 
                int height = bim.PixelHeight;
                int width = bim.PixelWidth;
                int stride = width * ((bim.Format.BitsPerPixel + 7) / 8);

               internalBuffer = new byte[height * stride];
                bim.CopyPixels(internalBuffer, stride, 0);


                //internalBuffer = _wb;
            }

        }

        // 获得Blender导出的UV坐标并将其对应的像素颜色返回  
        public Color Map(float tu, float tv)
        {
            // 图像尚未加载  
            if (internalBuffer == null)
            {
                return (Color)ColorConverter.ConvertFromString("White");
            }
            // 使用%运算符来循环/重复需要的这个纹理  
            int u = Math.Abs((int)(tu * width) % width);
            int v = Math.Abs((int)(tv * height) % height);

            int pos = (u + v * width) * 4;
            byte r = internalBuffer[pos];
            byte g = internalBuffer[pos + 1];
            byte b = internalBuffer[pos + 2];
            byte a = internalBuffer[pos + 3];

 
            return Color.FromArgb( a, r,g,b);
        }
    }
}
