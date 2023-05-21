using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Portable_Viewer {
    public class PBM : PM {
        
        public PBM(string path) : base(path) { }

        override public void Load() { 
            magic = parser.ReadString();
            if(magic != "P1" && magic != "P4") throw new Exception("Not supported PBM format. Only P1 and P4 are supported.");

            width = parser.ReadInt();
            height = parser.ReadInt(); 

            switch (magic) {
                case "P1":
                    values = parser.ReadBytesByInts(width * height );
                    break;
                case "P4":
                    values = parser.ReadBytesByBits(width * height );
                    break;
            } 
 
            Image = new Bitmap(width, height, PixelFormat.Format24bppRgb);

            BitmapData bitmapData = Image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, Image.PixelFormat);
            IntPtr ptr = bitmapData.Scan0;
            byte value = 0;
            for (int i = 0; i < values.Length; i++) {
                value = values[i] == 0 ? (byte)255 : (byte)0;
                System.Runtime.InteropServices.Marshal.WriteByte(ptr, i * 3 + 0, value);
                System.Runtime.InteropServices.Marshal.WriteByte(ptr, i * 3 + 1, value);
                System.Runtime.InteropServices.Marshal.WriteByte(ptr, i * 3 + 2, value); 
            }

            Image.UnlockBits(bitmapData);
            
        }   
    }
}
