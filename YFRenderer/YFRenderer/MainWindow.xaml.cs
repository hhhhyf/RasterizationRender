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
using System.IO;

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

        Mesh[] meshes;

        DateTime previousDate;

        public MainWindow()
        {
            InitializeComponent();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(TickEvent);
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0, 5);
            dispatcherTimer.Start();
            RenderBuffer.Instance.clearBuffer();
 

            meshes = Common.LoadJSONFileAsync("monkey.babylon");

            camera.Position = new Vector3d(0, 0, 10.0f);
            camera.Target = Vector3d.Zero;

         
        }

        private void TickEvent(object sender, EventArgs e)
        {
            RenderBuffer.Instance.clearBuffer();

            foreach (var mesh in meshes)
            {
               // mesh.Rotation.x = mesh.Rotation.x + 0.01f;
               mesh.Rotation.y = mesh.Rotation.y + 0.02f;
            }

            var now = DateTime.Now;
            var currentFps = 1000.0 / (now - previousDate).TotalMilliseconds;
            previousDate = now;

            FPS.Text = string.Format("{0:0.00} fps", currentFps);
            RenderBuffer.Instance.Render(camera, meshes);

            RenderBuffer.Instance.WriteToPixelArray(_pixelArray);
            _wb.WritePixels(_rect, _pixelArray, _stride, 0);
            this.image.Source = _wb;
            
        }
    }
}
