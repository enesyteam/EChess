using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Media;
using System.IO;

namespace SrcChess2
{
    public enum ImageFormat
    {
        JPG, BMP, PNG, GIF, TIF
    }
    public class Model
    {
        public Model()
        {
            Images = new List<System.Drawing.Bitmap>();
            BitmapSources = new List<BitmapSource>();
        }
        public List<System.Drawing.Bitmap> Images;
        public List<BitmapSource> BitmapSources;
        private ObservableCollection<BitmapSource> imagecollection;
        public ObservableCollection<BitmapSource> ImageCollection
        {
            get
            {
                this.imagecollection = this.imagecollection ?? new ObservableCollection<BitmapSource>();
                return this.imagecollection;
            }
        }

        public RenderTargetBitmap RenderVisaulToBitmap(Visual vsual, int width, int height)
        {
            RenderTargetBitmap rtb = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);
            rtb.Render(vsual);

            BitmapSource bsource = rtb;
            this.ImageCollection.Add(bsource);

            return rtb;
        }

        public void GenerateBitmapSources(System.Drawing.Bitmap b)
        {
            BitmapSource bsource = Helper.BitmapHelper.GetBitmapSource(b);
            BitmapSources.Add(bsource);
        }

        public MemoryStream GenerateImage(Visual vsual, int widhth, int height, ImageFormat format)
        {
            BitmapEncoder encoder = null;

            switch (format)
            {
                case ImageFormat.JPG :
                    encoder = new JpegBitmapEncoder();
                    break;
                case ImageFormat.PNG:
                    encoder = new PngBitmapEncoder();
                    break;
                case ImageFormat.BMP:
                    encoder = new BmpBitmapEncoder();
                    break;
                case ImageFormat.GIF:
                    encoder = new GifBitmapEncoder();
                    break;
                case ImageFormat.TIF:
                    encoder = new TiffBitmapEncoder();
                    break;

            }

            if (encoder == null) return null;

            RenderTargetBitmap rtb = this.RenderVisaulToBitmap(vsual, widhth, height);
            MemoryStream file = new MemoryStream();
            encoder.Frames.Add(BitmapFrame.Create(rtb));
            encoder.Save(file);

            return file;
        }

        
    }
}
