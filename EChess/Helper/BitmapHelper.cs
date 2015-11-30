using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace SrcChess2.Helper
{
    public static class BitmapHelper
    {
        public static void JPEGFromSource(string fileName, int quality, BitmapSource bmp)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            BitmapFrame outputFrame = BitmapFrame.Create(bmp);
            encoder.Frames.Add(outputFrame);
            encoder.QualityLevel = quality;

            using (FileStream file = File.OpenWrite(fileName))
            {
                encoder.Save(file);
            }
        }

        /// <summary>
        /// Chuyển đổi BitmapSource thành Bitmap
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static System.Drawing.Bitmap GetBitmap(BitmapSource source)
        {
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap
            (
              source.PixelWidth,
              source.PixelHeight,
              System.Drawing.Imaging.PixelFormat.Format32bppRgb
            );

            System.Drawing.Imaging.BitmapData data = bmp.LockBits
            (
                new System.Drawing.Rectangle(System.Drawing.Point.Empty, bmp.Size),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb
            );

            source.CopyPixels
            (
              Int32Rect.Empty,
              data.Scan0,
              data.Height * data.Stride,
              data.Stride
            );

            bmp.UnlockBits(data);

            return bmp;
        }
        public static BitmapSource GetBitmapSource(Bitmap bitmap)
        {
            BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap
            (
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );

            return bitmapSource;
        }
        public static void PNGFromBitmapSource(BitmapSource source, int iCount)
        {
            //int width = 128;
            //int height = 128;
            //int stride = width;
            //byte[] pixels = new byte[height * stride];

            //// Define the image palette
            //BitmapPalette myPalette = BitmapPalettes.Halftone256;

            //// Creates a new empty image with the pre-defined palette

            //BitmapSource image = BitmapSource.Create(
            //    width,
            //    height,
            //    96,
            //    96,
            //    PixelFormats.Indexed8,
            //    myPalette,
            //    pixels,
            //    stride);

            FileStream stream = new FileStream("new" + iCount + ".png", FileMode.Create);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            //TextBlock myTextBlock = new TextBlock();
            //myTextBlock.Text = "Codec Author is: " + encoder.CodecInfo.Author.ToString();
            encoder.Interlace = PngInterlaceOption.On;
            encoder.Frames.Add(BitmapFrame.Create(source));
            encoder.Save(stream);
        }

        public static System.Drawing.Bitmap BMPFromBitmapSource(BitmapSource source, int iCount)
        {
            //int width = 128;
            //int height = 128;
            //int stride = width;
            //byte[] pixels = new byte[height * stride];

            //// Define the image palette
            //BitmapPalette myPalette = BitmapPalettes.Halftone256;

            //// Creates a new empty image with the pre-defined palette

            //BitmapSource image = BitmapSource.Create(
            //    width,
            //    height,
            //    96,
            //    96,
            //    PixelFormats.Indexed8,
            //    myPalette,
            //    pixels,
            //    stride);

            FileStream stream = new FileStream("image" + iCount + ".BMP", FileMode.Create);
            BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            //TextBlock myTextBlock = new TextBlock();
            //myTextBlock.Text = "Codec Author is: " + encoder.CodecInfo.Author.ToString();
            //encoder.Interlace = PngInterlaceOption.On;
            encoder.Frames.Add(BitmapFrame.Create(source));
            encoder.Save(stream);

            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(stream);
            return bitmap;
        }

        public static BitmapImage BMPFromBMPSource(BitmapSource bitmapSource)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
            encoder.Save(memoryStream);

            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(memoryStream.ToArray());
            bImg.EndInit();

            memoryStream.Close();

            return bImg;
        }

        public static System.Drawing.Bitmap BitmapFromSource(BitmapSource bitmapsource)
        {
            System.Drawing.Bitmap bitmap;
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();

                enc.Frames.Add(BitmapFrame.Create(bitmapsource));
                enc.Save(outStream);
                bitmap = new System.Drawing.Bitmap(outStream);
            }
            return bitmap;
        }
    }
}
