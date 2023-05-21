using System;
using System.Collections.Generic;
using System.Text; 

namespace Portable_Viewer {
    public class Parser {

        public byte[] buffer;
        public int cursor;
        
        char[] separators = { ' ', '\t', '\r', '\n' };

        public Parser(byte[] buffer) {
            this.buffer = buffer;
            cursor = 0;
        }

        
        public string ReadString(){

            while (cursor < buffer.Length) {
                if (buffer[cursor] >= 33 && buffer[cursor] <= 122) break;
                cursor++; 
            }

            if(buffer[cursor] == 35){
                while (cursor < buffer.Length) {
                    if (buffer[cursor] == '\r' || buffer[cursor] == '\n') {
                        break;
                    } 
                    cursor++;
                }
                while (cursor < buffer.Length) {
                    if (buffer[cursor] >= 33 && buffer[cursor] <= 122) break;
                    cursor++;
                }
            }

            int start = cursor;
            string result = "";

            while (cursor < buffer.Length) { 
                if(buffer[cursor] < 33 || buffer[cursor] > 122) { 
                    result = Encoding.ASCII.GetString(buffer, start, cursor - start); 
                    cursor = cursor + 1;
                    break;
                } 
                cursor++;
            }

             
            return result;
        }

        public int ReadInt() {
            return int.Parse(ReadString()); 
        }

        public byte[] ReadBytes(int count) {
            byte[] result = new byte[count];
            int size = Math.Min(count, buffer.Length - cursor);
            // copy from buffer to result
            Buffer.BlockCopy(buffer, cursor, result, 0, size);
            cursor += size;
            return result;
        }

        public byte[] ReadBytesByInts(int count) {
            byte[] result = new byte[count];
            for (int i = 0; i < count; i++) {
                result[i] = (byte)ReadInt();
            }
            return result;
        }

        public byte[] ReadBytesByBits(int count) {
            byte[] result = new byte[count]; 
            int size = Math.Min(count / 8, (buffer.Length - cursor)); 
            byte b;
            for (int i = 0; i < size; i++) {
                b = buffer[cursor + i];
                for (int j = 0; j < 8; j++) {
                    result[i * 8 + j] = (byte)((b >> (7 - j)) & 1);
                }
            }
            cursor += size;
            return result;
        }
        
    }
}
