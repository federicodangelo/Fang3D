using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Fang3D
{
    internal class Texture2DImporter
    {
        static public bool LoadFromFile(Texture2D texture, String fileName)
        {
            if (File.Exists(fileName))
            {
                Bitmap bmp = new Bitmap(fileName);
                LoadFromBitmap(texture, bmp);
                Console.WriteLog(fileName + " : Cargado con exito");

                return true;
            }

            return false;
        }

        static public void LoadFromBitmap(Texture2D texture, Bitmap bmp)
        {
            texture.width = bmp.Width;
            texture.height = bmp.Height;
            texture.format = Texture2DFormat.RGB;
            texture.bitmap = bmp;

            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, texture.width, texture.height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            IntPtr ptr = bmpData.Scan0;
            int bytes = bmpData.Stride * bmp.Height;
            texture.data = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, texture.data, 0, bytes);

            bmp.UnlockBits(bmpData);
        }
    }
}
