using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Utility
{
    public class ImageUtility
    {
        public static void ResizeImage(string FileName, string NewFileName, int MaxWidth, int MaxHeight)
        {

            Bitmap ActualImage = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(FileName);
            Bitmap OrigImg = (System.Drawing.Bitmap)ActualImage.Clone();
            Size ResizedDimensions = GetDimensions(MaxWidth, MaxHeight, ref OrigImg);
            Bitmap NewImg = new Bitmap(OrigImg, ResizedDimensions);

            //saveFile(NewImg, NewFileName, NewFileExtension);
            NewImg.Save(NewFileName);

            ActualImage.Dispose();
            OrigImg.Dispose();
            NewImg.Dispose();
        }
        public static Size GetDimensions(int MaxWidth, int MaxHeight, ref Bitmap Img)
        {
            int Height; int Width; float Multiplier;
            Height = Img.Height; Width = Img.Width;

            if (Height <= MaxHeight && Width <= MaxWidth)
                return new Size(Width, Height);

            Multiplier = (float)((float)MaxWidth / (float)Width);

            if ((Height * Multiplier) <= MaxHeight)
            {
                Height = (int)(Height * Multiplier);
                return new Size(MaxWidth, Height);
            }

            Multiplier = (float)MaxHeight / (float)Height;
            Width = (int)(Width * Multiplier);

            return new Size(Width, MaxHeight);

        }
        /// <summary> 
        /// Saves an image as a jpeg image, with the given quality 
        /// </summary> 
        /// <param name="path"> Path to which the image would be saved. </param> 
        /// <param name="quality"> An integer from 0 to 100, with 100 being the highest quality. </param> 
        public static void SaveJpeg(string path, Image img, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");

            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, quality);
            // JPEG image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");
            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            img.Save(path, jpegCodec, encoderParams);
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];

            return null;
        } 
    }
}