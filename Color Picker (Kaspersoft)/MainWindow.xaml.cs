using System;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Input;

namespace Color_Picker__Kaspersoft_
{
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.Timer dragTimer, screenCaptureTimer;
        private double mdX, mdY;
        System.Drawing.Color c;
        public System.Windows.Controls.Label decidedColorLabel;

        public MainWindow()
        {
            InitializeComponent();

            dragTimer = new System.Windows.Forms.Timer();
            dragTimer.Interval = 1;
            dragTimer.Tick += dragTimer_Tick;

            screenCaptureTimer = new System.Windows.Forms.Timer();
            screenCaptureTimer.Interval = 100;
            screenCaptureTimer.Tick += screenCaptureTimer_Tick;
            screenCaptureTimer.Start();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            mdX = e.GetPosition(this).X;
            mdY = e.GetPosition(this).Y;
            dragTimer.Start();
        }

        private void Border_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            dragTimer.Stop();
        }

        private void dragTimer_Tick(object sender, EventArgs e)
        {
            this.Left = Control.MousePosition.X - mdX;
            this.Top = Control.MousePosition.Y - mdY;
        }

        private void screenCaptureTimer_Tick(object sender, EventArgs e)
        {
            System.Drawing.Point cursor = new System.Drawing.Point();
            GetCursorPos(ref cursor);

            c = GetColorAt(cursor);

            CL.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(c.A, c.R, c.G, c.B));

            ATB.Text = c.A.ToString();
            RTB.Text = c.R.ToString();
            GTB.Text = c.G.ToString();
            BTB.Text = c.B.ToString();

            GC.Collect();
        }

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref System.Drawing.Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        private void myWindow_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key.ToString().Equals("LeftCtrl"))
            {
                System.Windows.Controls.Label newC = new System.Windows.Controls.Label();
                newC.Content = "";
                newC.Background = CL.Background;
                newC.Height = 20;
                newC.BorderBrush = System.Windows.Media.Brushes.White;
                newC.BorderThickness = new Thickness(1);
                newC.MouseDoubleClick += newC_MouseDoubleClick;
                savedColors.Children.Add(newC);
                savedColorsScrollViewer.ScrollToBottom();
            }
        }

        private void newC_MouseDoubleClick(object sender, EventArgs e)
        {
            System.Windows.Controls.Label newC = (System.Windows.Controls.Label)sender;
            decidedColorLabel = newC;
            ColorProfile cp = new ColorProfile(this);
            cp.Left = this.Left + (this.Width - cp.Width) / 2;
            cp.Top = this.Top + (this.Height - cp.Height) / 2;
            cp.CL.Background = newC.Background;
            cp.HexTB.Text = newC.Background.ToString();
            cp.ATB.Text = ((SolidColorBrush)newC.Background).Color.A.ToString();
            cp.RTB.Text = ((SolidColorBrush)newC.Background).Color.R.ToString();
            cp.GTB.Text = ((SolidColorBrush)newC.Background).Color.G.ToString();
            cp.BTB.Text = ((SolidColorBrush)newC.Background).Color.B.ToString();
            cp.ShowDialog();
        }

        private void myWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.A))
                System.Windows.MessageBox.Show("Hi, I'm Keyhan Mohammadi the Developer who made this app . I hope you enjoy.");
        }

        Bitmap screenPixel = new Bitmap(50, 50, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        public System.Drawing.Color GetColorAt(System.Drawing.Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 50, 50, hSrcDC, location.X - 25, location.Y - 25, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(screenPixel.GetHbitmap(),
                                  IntPtr.Zero,
                                  Int32Rect.Empty,
                                  BitmapSizeOptions.FromEmptyOptions());
                                  
            screenCaptureMonitor.Background = new ImageBrush(bitmapSource);

            return screenPixel.GetPixel(25, 25);
        }
    }
}