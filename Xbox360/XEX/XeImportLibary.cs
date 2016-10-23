/*
 * Reversed by [ Hect0r ] @ www.staticpi.net
 * With thanks to : http://www.free60.org/wiki/XEX
 * For inital stuff :)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xenious.IO;

namespace Xbox360.XEX
{
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
        public UInt32 version
        {
            get
            {
                byte[] buf = new byte[4];
                Array.Copy(data, 24, buf, 0, 4);
                Array.Reverse(buf, 0, 4);
                return BitConverter.ToUInt32(buf, 0);
            }
            set
            {
                byte[] buf = BitConverter.GetBytes(value);
                Array.Reverse(buf);
                Array.Copy(buf, 0, data, 24, 4);
            }
        }
        public UInt32 min_version
        {
            get
            {
                byte[] buf = new byte[4];
                Array.Copy(data, 28, buf, 0, 4);
                Array.Reverse(buf, 0, 4);
                return BitConverter.ToUInt32(buf, 0);
            }
            set
            {
                byte[] buf = BitConverter.GetBytes(value);
                Array.Reverse(buf);
                Array.Copy(buf, 0, data, 28, 4);
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
        public List<UInt32> records
        {
            get
            {
                byte[] buf = new byte[data.Length - 36];
                Array.Copy(data, 36, buf, 0, (data.Length - 36));

                List<UInt32> buf2 = new List<uint>();
                for(int i = 0; i < (buf.Length / 4); i++)
                {
                    byte[] buf3 = new byte[4];
                    Array.Copy(buf, (4 * i), buf3, 0, 4);
                    Array.Reverse(buf3);
                    buf2.Add(BitConverter.ToUInt32(buf3, 0));
                }
                return buf2;
            }
            set
            {
                if((value.Count * 4) > data.Length || (value.Count * 4) < data.Length)
                {
                    byte[] data2 = new byte[(value.Count * 4)];
                    Array.Copy(data, 0, data2, 0, 36);

                    for (int i = 0; i < value.Count; i++)
                    {
                        byte[] buf = BitConverter.GetBytes(value[i]);
                        Array.Reverse(buf);
                        Array.Copy(buf, 0, data2, (36 + (4 * i)), 4);
                    }
                    data = data2;
                }
                else
                {
                    for(int i = 0; i < value.Count; i++)
                    {
                        byte[] buf = BitConverter.GetBytes(value[i]);
                        Array.Reverse(buf);
                        Array.Copy(buf, 0, data, (36 + (4 * i)), 4);
                    }
                }
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
    }
}
