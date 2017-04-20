using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using YFRenderer.Core;
using YFRenderer.Primitives;

namespace YFRenderer
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private static WriteableBitmap _wb = new WriteableBitmap(Common.CanvasWidth, Common.CanvasHeight, 96, 96, PixelFormats.Bgra32, null);

        private static Int32Rect _rect = new Int32Rect(0, 0, Common.CanvasWidth, Common.CanvasHeight);

        private static int _stride = 4 * Common.CanvasWidth;

        byte[] _pixelArray = new byte[Common.CanvasWidth * Common.CanvasHeight * 4];

        Mesh mesh = new Mesh("Cube", 8, 12);

        Camera camera = new Camera();

        public MainWindow()
        {
            InitializeComponent();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(TickEvent);
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0, 10);
            dispatcherTimer.Start();
            RenderBuffer.Instance.clearBuffer();


            mesh.Vertices[0] = new Vector3d(-1, 1, 1);
            mesh.Vertices[1] = new Vector3d(1, 1, 1);
            mesh.Vertices[2] = new Vector3d(-1, -1, 1);
            mesh.Vertices[3] = new Vector3d(1, -1, 1);
            mesh.Vertices[4] = new Vector3d(-1, 1, -1);
            mesh.Vertices[5] = new Vector3d(1, 1, -1);
            mesh.Vertices[6] = new Vector3d(1, -1, -1);
            mesh.Vertices[7] = new Vector3d(-1, -1, -1);

            mesh.Faces[0] = new Face { A = 0, B = 1, C = 2 };
            mesh.Faces[1] = new Face { A = 1, B = 2, C = 3 };
            mesh.Faces[2] = new Face { A = 1, B = 3, C = 6 };
            mesh.Faces[3] = new Face { A = 1, B = 5, C = 6 };
            mesh.Faces[4] = new Face { A = 0, B = 1, C = 4 };
            mesh.Faces[5] = new Face { A = 1, B = 4, C = 5 };

            mesh.Faces[6] = new Face { A = 2, B = 3, C = 7 };
            mesh.Faces[7] = new Face { A = 3, B = 6, C = 7 };
            mesh.Faces[8] = new Face { A = 0, B = 2, C = 7 };
            mesh.Faces[9] = new Face { A = 0, B = 4, C = 7 };
            mesh.Faces[10] = new Face { A = 4, B = 5, C = 6 };
            mesh.Faces[11] = new Face { A = 4, B = 6, C = 7 };

            camera.Position = new Vector3d(0, 0, 10.0f);
            camera.Target = Vector3d.Zero;
        }

        private void TickEvent(object sender, EventArgs e)
        {
            RenderBuffer.Instance.clearBuffer();

            mesh.Rotation.x = mesh.Rotation.x+ 0.01f;
            mesh.Rotation.y = mesh.Rotation.y + 0.01f;

            RenderBuffer.Instance.Render(camera, mesh);

            RenderBuffer.Instance.WriteToPixelArray(_pixelArray);
            _wb.WritePixels(_rect, _pixelArray, _stride, 0);
            this.image.Source = _wb;
            
        }
    }
}
