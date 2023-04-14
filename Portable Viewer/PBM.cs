using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Portable_Viewer {
    public class PBM : PM {
        
        public PBM(string path) : base(path) { }

        override public void Parse() { 
            magic = ReadToEndOfLine();
            if(magic != "P1" && magic != "P4") throw new Exception("Not supported PBM format. Only P1 and P4 are supported.");
            if(buffer[cursor] == '#') comment = ReadToEndOfLine();

            string[] size = ReadToEndOfLine().Split(' ');
            width = int.Parse(size[0]);
            height = int.Parse(size[1]); 
            
            switch (magic) {
                case "P1":
                    Parse_P1();
                    break;
                case "P4":
                    Parse_P4();
                    break;
            }
        }

        void Parse_P1() {  
            Image = new Bitmap(width, height);
            int value = 0;
            string[] values;
            for (int y = 0; y < height; y++) {
                values = ReadToEndOfLine().Split(' ');
                for (int x = 0; x < width; x++) { 
                    value = int.Parse(values[x]) == 0 ? 255 : 0;
                    Image.SetPixel(x, y, Color.FromArgb(value, value, value)); 
                }
                OnProgressChanged((int)(100.0 * y / height));
            }
        }

        void Parse_P4() {  
            Image = new Bitmap(width, height);
            int value = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) { 
                    value = (buffer[cursor] & (1 << (7 - x % 8))) == 0 ? 255 : 0;
                    Image.SetPixel(x, y, Color.FromArgb(value, value, value)); 
                    if(x % 8 == 7) cursor++;
                }
                OnProgressChanged((int)(100.0 * y / height));
            }
        }
    }
}
