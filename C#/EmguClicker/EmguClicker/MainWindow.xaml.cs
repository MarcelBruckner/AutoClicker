using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;
using System.Windows.Media;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace EmguClicker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ImageSource Primary { get; private set; }
        public Image<Bgr, byte> Secondary { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            LoopImage();
        }

        private void LoopImage()
        {
            Primary = VideoFeed.Instance.CopyScreen;

        }

        
    }

}
