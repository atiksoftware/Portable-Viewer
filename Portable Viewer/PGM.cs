using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Portable_Viewer {
    public class PGM : PM {  

        public PGM(string path) : base(path) { }

        override public void Parse() { 
            magic = ReadToEndOfLine();
            if(magic != "P2" && magic != "P5") throw new Exception("Not supported PGM format. Only P2 and P5 are supported.");
            if(buffer[cursor] == '#') comment = ReadToEndOfLine();

            string[] size = ReadToEndOfLine().Split(' ');
            width = int.Parse(size[0]);
            height = int.Parse(size[1]);

            maxval = int.Parse(ReadToEndOfLine());
            
            switch (magic) {
                case "P2":
                    Parse_P2();
                    break;
                case "P5":
                    Parse_P5();
                    break;
            }
        }

        void Parse_P2() {  
            Image = new Bitmap(width, height);
            int value = 0;
            string[] values;
            for (int y = 0; y < height; y++) {
                values = ReadToEndOfLine().Split(' ');
                for (int x = 0; x < width; x++) { 
                    value = (int)(int.Parse(values[x]) * 255.0 / maxval);
                     Image.SetPixel(x, y, Color.FromArgb(value, value, value)); 
                }
                OnProgressChanged((int)(100.0 * y / height));
            }
        }

        void Parse_P5() {  
            Image = new Bitmap(width, height);
            int value = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) { 
                    value = (int)(buffer[cursor++] * 255.0 / maxval);
                    Image.SetPixel(x, y, Color.FromArgb(value, value, value)); 
                }
                OnProgressChanged((int)(100.0 * y / height));
            }
        } 
    }
}
