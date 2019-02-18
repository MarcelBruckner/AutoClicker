
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace EmguClicker
{

    public class VideoFeed
    {
        private static VideoFeed _instance;

        private VideoFeed() { }

        public static VideoFeed Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new VideoFeed();
                }
                return _instance;
            }
        }

        public List<Rectangle> ScreenDimensions
        {
            get
            {
                List<Rectangle> dimensions = new List<Rectangle>();
                foreach (Screen screen in Screen.AllScreens)
                {
                    dimensions.Add(screen.Bounds);
                }
                return dimensions;
            }
        }

        //private List<Image<Bgr, Byte>> GetImages()
        public Bitmap Feed
        {
            get
            {
                //Create a new bitmap.
                var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                               Screen.PrimaryScreen.Bounds.Height,
                                               PixelFormat.Format32bppArgb);

                // Create a graphics object from the bitmap.
                var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

                // Take the screenshot from the upper left corner to the right bottom corner.
                gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                            Screen.PrimaryScreen.Bounds.Y,
                                            0,
                                            0,
                                            Screen.PrimaryScreen.Bounds.Size,
                                            CopyPixelOperation.SourceCopy);


                return new Bitmap(bmpScreenshot.Width, bmpScreenshot.Height, gfxScreenshot);
            }
        }

        public BitmapSource CopyScreen
        {
            get
            {
                using (var screenBmp = new Bitmap(
                    (int)SystemParameters.PrimaryScreenWidth,
                    (int)SystemParameters.PrimaryScreenHeight,
                    PixelFormat.Format32bppArgb))
                {
                    using (var bmpGraphics = Graphics.FromImage(screenBmp))
                    {
                        bmpGraphics.CopyFromScreen(0, 0, 0, 0, screenBmp.Size);
                        return Imaging.CreateBitmapSourceFromHBitmap(
                            screenBmp.GetHbitmap(),
                            IntPtr.Zero,
                            Int32Rect.Empty,
                            BitmapSizeOptions.FromEmptyOptions());
                    }
                }
            }
        }
    }
}