/*
 * Reversed by [ Hect0r ] @ www.staticpi.net
 * With thanks to : http://www.free60.org/wiki/XEX
 * For inital stuff :)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbox360.IO;
using Xenious.IO;

namespace Xbox360.XEX
{
    public class XeImportOridinal
    {
        public UInt32 ord;
        public UInt32 actual_pos;
    }

    public class XeImportLibary
    {
        public byte[] data;
        public string name;
        public byte[] hash
        {
            get
            {
                byte[] buf = new byte[20];
                Array.Copy(data, 0, buf, 0, 20);
                return buf;
            }
            set
            {
                Array.Copy(value, 0, data, 0, 20);
            }
        }
        public UInt32 import_id
        {
            get
            {
                byte[] buf = new byte[4];
                Array.Copy(data, 20, buf, 0, 4);
                Array.Reverse(buf, 0, 4);
                return BitConverter.ToUInt32(buf, 0);
            }
            set
            {
                byte[] buf = BitConverter.GetBytes(value);
                Array.Reverse(buf);
                Array.Copy(buf, 0, data, 20, 4);
            }
        }
        public byte[] version
        {
            get
            {
                byte[] buf = new byte[4];
                Array.Copy(data, 24, buf, 0, 4);
                return buf;
            }
            set
            {
                Array.Copy(value, 0, data, 24, 4);
            }
        }
        public byte[] min_version
        {
            get
            {
                byte[] buf = new byte[4];
                Array.Copy(data, 28, buf, 0, 4);
                return buf;
            }
            set
            {
                Array.Copy(value, 0, data, 28, 4);
            }
        }
        public UInt32 record_count
        {
            get
            {
                byte[] buf = new byte[4];
                Array.Copy(data, 32, buf, 0, 4);
                Array.Reverse(buf, 0, 4);
                return BitConverter.ToUInt32(buf, 0);
            }
            set
            {
                byte[] buf = BitConverter.GetBytes(value);
                Array.Reverse(buf);
                Array.Copy(buf, 0, data, 32, 4);
            }
        }
        public List<XeImportOridinal> records
        {
            get
            {
                byte[] buf = new byte[data.Length - 36];
                Array.Copy(data, 36, buf, 0, (data.Length - 36));

                List<XeImportOridinal> buf2 = new List<XeImportOridinal>();
                for(int i = 0; i < (buf.Length / 8); i++)
                {
                    byte[] buf3 = new byte[4];
                    byte[] buf4 = new byte[4];
                    Array.Copy(buf, (i == 0 ? 0 : (8 * i)), buf3, 0, 4);
                    Array.Copy(buf, (i == 0 ? 4 : (8 * i) + 4), buf4, 0, 4);
                    
                    if(BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(buf3);
                        Array.Reverse(buf4);
                    }

                    XeImportOridinal ord = new XeImportOridinal();
                    ord.actual_pos = BitConverter.ToUInt32(buf4, 0);
                    ord.ord = BitConverter.ToUInt32(buf3, 0);
                    buf2.Add(ord);
                }
                return buf2;
            }
            set
            {
                
            }
        }
        public bool record_count_flag
        {
            get { return (bool)(record_count > 65536); }
        }

        public void read(FileIO IO)
        {
            UInt32 size = IO.read_uint32(Endian.High) - 4;
            data = IO.read_bytes((int)size);
        }
        public void write(FileIO IO)
        {
            IO.write((UInt32)(data.Length + 4), Endian.High);
            IO.write(data);
        }
        public bool table_scrambled
        {
            get
            {
                return ((record_count & 65536) == 65536);
            }
        }

    }
}
