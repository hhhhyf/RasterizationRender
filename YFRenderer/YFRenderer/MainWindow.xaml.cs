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
            Buffer.Instance.clearBuffer();
        }

        private void TickEvent(object sender, EventArgs e)
        {
            var color = (Color)ColorConverter.ConvertFromString("White");
              for (int i = 100; i < 300; i++)
              {
                for (int j  = 100;j < 300; j++)
                    Buffer.Instance.SetPixel(i, j, color);
              }

            Buffer.Instance.WriteToPixelArray(_pixelArray);
            _wb.WritePixels(_rect, _pixelArray, _stride, 0);
            this.image.Source = _wb;
        }
    }
}
