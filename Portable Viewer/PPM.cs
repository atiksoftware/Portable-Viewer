using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Portable_Viewer {
    public class PPM : PM {

        public PPM(string path) : base(path) { }
  
        override public void Load() { 
            magic = parser.ReadString();
            if(magic != "P3" && magic != "P6") throw new Exception("Not supported PPM format. Only P3 and P6 are supported.");

            width = parser.ReadInt();
            height = parser.ReadInt();
            maxval = parser.ReadInt();

            switch (magic) {
                case "P3":
                    values = parser.ReadBytesByInts(width * height * 3);
                    break;
                case "P6":
                    values = parser.ReadBytes(width * height * 3);
                    break;
            }
            
            if(maxval != 255) Normalize();
 
            Image = new Bitmap(width, height, PixelFormat.Format32bppArgb);

            BitmapData bitmapData = Image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, Image.PixelFormat);
            IntPtr ptr = bitmapData.Scan0;

            for (int i = 0; i < values.Length / 3; i++) {  
                System.Runtime.InteropServices.Marshal.WriteInt32(
                    ptr, i * 4, 
                    (255 << 24) | (values[i * 3 + 0] << 16) | (values[i * 3 + 1] << 8) | (values[i * 3 + 2] << 0)
                ); 
            }

            Image.UnlockBits(bitmapData); 
            
        }  
    }
}
