using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Portable_Viewer {
    public class PM {
        public byte[] buffer;
        public int cursor = 0;
  
        public string magic;
        public string comment;
        public int width;
        public int height; 
        public int maxval;

        public Bitmap Image;

        public delegate void ProgressChangedEventHandler(object sender, int progress);
        public event ProgressChangedEventHandler Progress;

        public PM(string path) {
            buffer = System.IO.File.ReadAllBytes(path); 
        }

        public string ReadToEndOfLine() { 
            int nextEOLPos = cursor;
            while (true) {
                if (buffer[nextEOLPos] == '\n') {
                    int nextCursor = nextEOLPos + 1;
                    if(buffer[nextEOLPos - 1] == '\r')
                        nextEOLPos--;
                    
                    string result = Encoding.ASCII.GetString(buffer, cursor, nextEOLPos - cursor);
                    cursor = nextCursor;
                    return result;
                }
                nextEOLPos++;
            } 
        }

        public virtual void Parse() { }

        protected virtual void OnProgressChanged(int progress) {
            if (Progress != null) {
                Progress(this, progress);
            }
        }
    }
}
