using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFMediaKit.DirectShow.Controls;

namespace Face
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void btnCapture_Click(object sender, RoutedEventArgs e)
        {
            vce.Play();
            SaveControlImage(b, "b.jpg");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (MultimediaUtil.VideoInputNames.Length > 0)
                vce.VideoCaptureSource = MultimediaUtil.VideoInputNames[0];
            else
                MessageBox.Show("没有检测到任何摄像头");
        }
        public static void SaveControlImage(FrameworkElement ui, string fileName)
        {
            System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
            RenderTargetBitmap bmp = new RenderTargetBitmap(200, 150, 0, 0,
            PixelFormats.Pbgra32);
            bmp.Render(ui);
            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));
            encoder.Save(fs);
            fs.Close();
        }
        private void btnanew_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                vce.Play();
                SaveControlImage(b, "bp.jpg");
                vce.Stop();
                var client = new Baidu.Aip.Face.Face("API KEY", "Secret Key");
                var image1 = File.ReadAllBytes("b.jpg");
                var image2 = File.ReadAllBytes("bp.jpg");
                var images = new byte[][] { image1, image2 };
                var result = double.Parse(client.FaceMatch(images).First.First.Last.Last.First.ToString());
                if (result >= 90)
                    Title = "识别成功";
                else Title = "识别失败";
            }
            catch { Title = "识别失败"; }
        }
    }
}
