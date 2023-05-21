using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Portable_Viewer {
    public class PM {
        public byte[] buffer;
        public Parser parser;
  
        public string magic; 
        public int width;
        public int height; 
        public int maxval;
        public byte[] values;

        public Bitmap Image; 

        public PM(string path) {
            buffer = System.IO.File.ReadAllBytes(path); 
            parser = new Parser(buffer);
        }
 
        public virtual void Load() { }
         

        
        public void Normalize(){
            for (int i = 0; i < values.Length; i++) {
                values[i] = (byte)(values[i] * 255.0 / maxval);
            }
        }
    }
}
