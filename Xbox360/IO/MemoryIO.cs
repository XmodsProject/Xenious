using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbox360.IO
{
    public class MemoryIO
    {
        Endian machine_endian;
        MemoryStream handle;
        BinaryReader br;
        BinaryWriter bw;
        public bool Opened = false;

        public long position
        {
            get { return handle.Position; }
            set { handle.Position = value; }
        }
        public long length
        {
            get { return handle.Length; }
            set { handle.SetLength(value); }
        }

        public MemoryIO()
        {
            handle = new MemoryStream();
            br = new BinaryReader(handle);
            bw = new BinaryWriter(handle);
            Opened = true;
            machine_endian = get_machine_endian();
            handle.Seek(0, SeekOrigin.Begin);
        }
        public MemoryIO(MemoryStream stream)
        {
            handle = stream;
            br = new BinaryReader(handle);
            bw = new BinaryWriter(handle);
            Opened = true;
            machine_endian = get_machine_endian();
            handle.Seek(0, SeekOrigin.Begin);
        }

        private Endian get_machine_endian()
        {
            return BitConverter.IsLittleEndian ? Endian.Low : Endian.High;
        }

        /* Read Functions */
        public byte read_byte()
        {
            return br.ReadByte();
        }
        public byte[] read_bytes(int len, bool reverse = false)
        {
            byte[] buf = br.ReadBytes(len);

            if (reverse == true)
            {
                Array.Reverse(buf);
            }
            return buf;
        }
        public UInt16 read_uint16(Endian e)
        {
            byte[] data;
            if (e != this.machine_endian) { data = this.read_bytes(2, true); }
            else { data = this.read_bytes(2); }

            return BitConverter.ToUInt16(data, 0);
        }
        public Int16 read_int16(Endian e)
        {
            byte[] data;
            if (e != this.machine_endian) { data = this.read_bytes(2, true); }
            else { data = this.read_bytes(2); }

            return BitConverter.ToInt16(data, 0);
        }
        public UInt32 read_uint32(Endian e)
        {
            byte[] data;
            if (e != this.machine_endian) { data = this.read_bytes(4, true); }
            else { data = this.read_bytes(4); }

            return BitConverter.ToUInt32(data, 0);
        }
        public Int32 read_int32(Endian e)
        {
            byte[] data;
            if (e != this.machine_endian) { data = this.read_bytes(4, true); }
            else { data = this.read_bytes(4); }

            return BitConverter.ToInt32(data, 0);
        }
        public UInt64 read_uint64(Endian e)
        {
            byte[] data;
            if (e != this.machine_endian) { data = this.read_bytes(8, true); }
            else { data = this.read_bytes(8); }

            return BitConverter.ToUInt64(data, 0);
        }
        public Int64 read_int64(Endian e)
        {
            byte[] data;
            if (e != this.machine_endian) { data = this.read_bytes(8, true); }
            else { data = this.read_bytes(8); }

            return BitConverter.ToInt64(data, 0);
        }
        public string read_string(int len)
        {
            return Encoding.ASCII.GetString(this.read_bytes(len));
        }
        public string read_unicode_string(int len)
        {
            len *= 2;
            return Encoding.Unicode.GetString(this.read_bytes(len));
        }

        public void write(byte b)
        {
            bw.Write(b);
            bw.Flush();
        }
        public void write(byte[] bs)
        {
            bw.Write(bs);
            bw.Flush();
        }
        public void write(UInt16 value, Endian e)
        {
            byte[] bs = BitConverter.GetBytes(value);

            if (e != this.machine_endian)
            {
                Array.Reverse(bs);
            }
            bw.Write(bs);
            bw.Flush();
        }
        public void write(UInt32 value, Endian e)
        {
            byte[] bs = BitConverter.GetBytes(value);

            if (e != this.machine_endian)
            {
                Array.Reverse(bs);
            }
            bw.Write(bs);
            bw.Flush();
        }
        public void write(UInt64 value, Endian e)
        {
            byte[] bs = BitConverter.GetBytes(value);

            if (e != this.machine_endian)
            {
                Array.Reverse(bs);
            }
            bw.Write(bs);
            bw.Flush();
        }
        public void write(Int16 value, Endian e)
        {
            byte[] bs = BitConverter.GetBytes(value);

            if (e != this.machine_endian)
            {
                Array.Reverse(bs);
            }
            bw.Write(bs);
            bw.Flush();
        }
        public void write(Int32 value, Endian e)
        {
            byte[] bs = BitConverter.GetBytes(value);

            if (e != this.machine_endian)
            {
                Array.Reverse(bs);
            }
            bw.Write(bs);
            bw.Flush();
        }
        public void write(Int64 value, Endian e)
        {
            byte[] bs = BitConverter.GetBytes(value);

            if (e != this.machine_endian)
            {
                Array.Reverse(bs);
            }
            bw.Write(bs);
            bw.Flush();
        }

        public void close()
        {
            br.Close();
            bw.Close();
            handle.Close();
        }

        public void save_stream(string outfile)
        {
            FileStream fs = new FileStream(outfile, FileMode.Create);
            fs.Position = 0;

            byte[] data = this.handle.ToArray();

            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }
    }
}
