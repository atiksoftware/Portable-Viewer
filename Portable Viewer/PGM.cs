using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Portable_Viewer {
    public class PGM : PM {  

        public PGM(string path) : base(path) { }
 
        override public void Load() { 
            magic = parser.ReadString();
            if(magic != "P2" && magic != "P5") throw new Exception("Not supported PGM format. Only P2 and P5 are supported.");

            width = parser.ReadInt();
            height = parser.ReadInt();
            maxval = parser.ReadInt();

            switch (magic) {
                case "P2":
                    values = parser.ReadBytesByInts(width * height);
                    break;
                case "P5":
                    values = parser.ReadBytes(width * height);
                    break;
            }
            
            if(maxval != 255) Normalize();
 
            Image = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bitmapData = Image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, Image.PixelFormat);
            IntPtr ptr = bitmapData.Scan0;

            for (int i = 0; i < values.Length; i++) { 
                System.Runtime.InteropServices.Marshal.WriteByte(ptr, i * 3, values[i]);
                System.Runtime.InteropServices.Marshal.WriteByte(ptr, i * 3 + 1, values[i]);
                System.Runtime.InteropServices.Marshal.WriteByte(ptr, i * 3 + 2, values[i]);
            }

            Image.UnlockBits(bitmapData);
            
        }  
    }
}
