using System;
using System.Windows;
using System.Windows.Forms;

namespace Color_Picker__Kaspersoft_
{
    public partial class ColorProfile : Window
    {
        private System.Windows.Forms.Timer dragTimer;
        private double mdX, mdY;
        private MainWindow parent;

        public ColorProfile(MainWindow parent)
        {
            InitializeComponent();
            dragTimer = new System.Windows.Forms.Timer();
            dragTimer.Interval = 1;
            dragTimer.Tick += dragTimer_Tick;
            this.parent = parent;
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.parent.savedColors.Children.Remove(this.parent.decidedColorLabel);
            this.Close();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
