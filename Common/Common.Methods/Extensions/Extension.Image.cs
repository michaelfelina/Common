using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Common.Methods.Extensions
{
    public static class ImageExtensions
    {
        public static byte[] ToBytes(this Image picImage)
        {
            if (picImage != null)
            {
                var ms = new MemoryStream();
                picImage.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
            return null;
        }

        public static Image ToImage(this byte[] buff)
        {
            if (buff != null)
            {
                return Image.FromStream(new MemoryStream(buff));
            }
            return null;
        }

        public static Image ToImage(this object buff)
        {
            Image result = null;
            try
            {
                byte[] imagebuff = buff as byte[];
                if (imagebuff != null)
                {
                    result = Image.FromStream(new MemoryStream(imagebuff));
                }
            }
            catch (Exception)
            {
            }
            
            return result;
        }
    }
}
