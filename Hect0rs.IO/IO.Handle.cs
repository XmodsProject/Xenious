using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hect0rs.IO
{
    public class IOH
    {
        private Endian MachineEndian;
        private Stream Handle;
        private BinaryReader BRH;
        private BinaryWriter BWH;
        private bool Opened = false;
        private string FileName;

        public long Position
        {
            get { return Handle.Position; }
            set { Handle.Position = value; }
        }
        public bool IsOpen
        {
            get { return this.Opened; }
        }
        public long Length
        {
            get { return Handle.Length; }
        }
        public string File
        {
            get { return FileName; }
        }

        public IOH(string file, FileMode mode)
        {
            FileName = file;
            Handle = new FileStream(file, mode);
            BRH = new BinaryReader(Handle);
            BWH = new BinaryWriter(Handle);
            Opened = true;
            MachineEndian = get_MachineEndian();
            Handle.Seek(0, SeekOrigin.Begin);
        }
        public IOH(FileStream file)
        {
            Handle = file;
            BRH = new BinaryReader(Handle);
            BWH = new BinaryWriter(Handle);
            Opened = true;
            MachineEndian = get_MachineEndian();
            Handle.Seek(0, SeekOrigin.Begin);
        }
        public IOH(MemoryStream file)
        {
            Handle = file;
            BRH = new BinaryReader(Handle);
            BWH = new BinaryWriter(Handle);
            Opened = true;
            MachineEndian = get_MachineEndian();
            Handle.Seek(0, SeekOrigin.Begin);
        }

        public IOH(byte[] Data)
        {
            Handle = new MemoryStream(Data);
            BRH = new BinaryReader(Handle);
            BWH = new BinaryWriter(Handle);
            Opened = true;
            MachineEndian = get_MachineEndian();
            Handle.Seek(0, SeekOrigin.Begin);
        }

        private Endian get_MachineEndian()
        {
            return BitConverter.IsLittleEndian ? Endian.Low : Endian.High;
        }

        /* Read Functions */
        public byte ReadByte()
        {
            return BRH.ReadByte();
        }
        public byte[] ReadBytes(int len, bool reverse = false)
        {
            byte[] buf = BRH.ReadBytes(len);

            if (reverse == true)
            {
                Array.Reverse(buf);
            }
            return buf;
        }
        public UInt16 ReadUInt16(Endian e)
        {
            byte[] data;
            if (e != this.MachineEndian) { data = this.ReadBytes(2, true); }
            else { data = this.ReadBytes(2); }

            return BitConverter.ToUInt16(data, 0);
        }
        public Int16 ReadInt16(Endian e)
        {
            byte[] data;
            if (e != this.MachineEndian) { data = this.ReadBytes(2, true); }
            else { data = this.ReadBytes(2); }

            return BitConverter.ToInt16(data, 0);
        }
        public UInt32 ReadUInt32(Endian e)
        {
            byte[] data;
            if (e != this.MachineEndian) { data = this.ReadBytes(4, true); }
            else { data = this.ReadBytes(4); }

            return BitConverter.ToUInt32(data, 0);
        }
        public UInt32 ReadUInt32()
        {
            byte[] data = this.ReadBytes(4);
            return BitConverter.ToUInt32(data, 0);
        }
        public Int32 ReadInt32(Endian e)
        {
            byte[] data;
            if (e != this.MachineEndian) { data = this.ReadBytes(4, true); }
            else { data = this.ReadBytes(4); }

            return BitConverter.ToInt32(data, 0);
        }
        public UInt64 ReadUInt64(Endian e)
        {
            byte[] data;
            if (e != this.MachineEndian) { data = this.ReadBytes(8, true); }
            else { data = this.ReadBytes(8); }

            return BitConverter.ToUInt64(data, 0);
        }
        public Int64 ReadInt64(Endian e)
        {
            byte[] data;
            if (e != this.MachineEndian) { data = this.ReadBytes(8, true); }
            else { data = this.ReadBytes(8); }

            return BitConverter.ToInt64(data, 0);
        }
        public string ReadString(int len)
        {
            return Encoding.ASCII.GetString(this.ReadBytes(len));
        }
        public string ReadUnicodeString(int len, Endian e)
        {
            len *= 2;
            byte[] data = this.ReadBytes(len);

            if (e != this.MachineEndian)
            {
                byte[] buf = new byte[len];

                for (int i = 0; i < len / 2; i += 2)
                {
                    buf[i] = (byte)(data[i + 1]);
                    buf[i + 1] = (byte)(data[i]);
                }

                data = buf;
            }
            return Encoding.Unicode.GetString(data);
        }
        public string ReadUnicodeStringFixedLength(int len, Endian e)
        {
            byte[] data = this.ReadBytes(len);

            if (e != this.MachineEndian)
            {
                
                /*for (int i = 0; i < len / 2; i += 2)
                {
                    buf[i] = (byte)(data[i + 1]);
                    buf[i + 1] = (byte)(data[i]);
                }*/

                //data = buf;
            }
            return Encoding.BigEndianUnicode.GetString(data);
        }

        public void Write(byte b)
        {
            BWH.Write(b);
            BWH.Flush();
        }
        public void Write(byte[] bs)
        {
            BWH.Write(bs);
            BWH.Flush();
        }
        public void Write(UInt16 value, Endian e)
        {
            byte[] bs = BitConverter.GetBytes(value);

            if (e != this.MachineEndian)
            {
                Array.Reverse(bs);
            }
            BWH.Write(bs);
            BWH.Flush();
        }
        public void Write(UInt32 value, Endian e)
        {
            byte[] bs = BitConverter.GetBytes(value);

            if (e != this.MachineEndian)
            {
                Array.Reverse(bs);
            }
            BWH.Write(bs);
            BWH.Flush();
        }
        public void Write(UInt64 value, Endian e)
        {
            byte[] bs = BitConverter.GetBytes(value);

            if (e != this.MachineEndian)
            {
                Array.Reverse(bs);
            }
            BWH.Write(bs);
            BWH.Flush();
        }
        public void Write(Int16 value, Endian e)
        {
            byte[] bs = BitConverter.GetBytes(value);

            if (e != this.MachineEndian)
            {
                Array.Reverse(bs);
            }
            BWH.Write(bs);
            BWH.Flush();
        }
        public void Write(Int32 value, Endian e)
        {
            byte[] bs = BitConverter.GetBytes(value);

            if (e != this.MachineEndian)
            {
                Array.Reverse(bs);
            }
            BWH.Write(bs);
            BWH.Flush();
        }
        public void Write(Int64 value, Endian e)
        {
            byte[] bs = BitConverter.GetBytes(value);

            if (e != this.MachineEndian)
            {
                Array.Reverse(bs);
            }
            BWH.Write(bs);
            BWH.Flush();
        }

        public void close()
        {
            BRH.Close();
            BWH.Close();
            Handle.Close();
        }
    }
}
