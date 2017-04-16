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

        public MainWindow()
        {
            InitializeComponent();

            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(TickEvent);
            dispatcherTimer.Interval = new TimeSpan(0,0,0,0, 500);
            dispatcherTimer.Start();
            RenderBuffer.Instance.clearBuffer();
        }

        private void TickEvent(object sender, EventArgs e)
        {
            Vector2d p1 = new Vector2d(0, 0);
            Vector2d p2 = new Vector2d(400, 600);
            Draw.Draw2DSegement(p1, p2);

            Vector2d p3 = new Vector2d(400, 0);
            Vector2d p4 = new Vector2d(800, 600);
            Draw.Draw2DSegement2(p3, p4);

            RenderBuffer.Instance.WriteToPixelArray(_pixelArray);
            _wb.WritePixels(_rect, _pixelArray, _stride, 0);
            this.image.Source = _wb;
        }
    }
}
