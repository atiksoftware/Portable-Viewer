using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Portable_Viewer {
    public class PPM : PM {

        public PPM(string path) : base(path) { }

        override public void Parse() {
            magic = ReadToEndOfLine();
            if (magic != "P3" && magic != "P6") throw new Exception("Not supported PPM format. Only P3 and P6 are supported.");
            if (buffer[cursor] == '#') comment = ReadToEndOfLine();

            string[] size = ReadToEndOfLine().Split(' ');
            width = int.Parse(size[0]);
            height = int.Parse(size[1]);
 
            maxval = int.Parse(ReadToEndOfLine());

            switch (magic) {
                case "P3":
                    Parse_P3();
                    break;
                case "P6":
                    Parse_P6();
                    break;
            }
        }

        void Parse_P3() {
            Image = new Bitmap(width, height);
            int r = 0, g = 0, b = 0;
            string[] values;
            for (int y = 0; y < height; y++) {
                values = ReadToEndOfLine().Split(' ');
                for (int x = 0; x < width; x++) {
                    r = (int)(int.Parse(values[x * 3]) * 255.0 / maxval);
                    g = (int)(int.Parse(values[x * 3 + 1]) * 255.0 / maxval);
                    b = (int)(int.Parse(values[x * 3 + 2]) * 255.0 / maxval);
                    Image.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
                OnProgressChanged((int)(100.0 * y / height));
            }
        }

        void Parse_P6() {
            Image = new Bitmap(width, height);
            int r = 0, g = 0, b = 0;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    r = (int)(buffer[cursor++] * 255.0 / maxval);
                    g = (int)(buffer[cursor++] * 255.0 / maxval);
                    b = (int)(buffer[cursor++] * 255.0 / maxval);
                    Image.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
                OnProgressChanged((int)(100.0 * y / height));
            }
        }
    }
}
