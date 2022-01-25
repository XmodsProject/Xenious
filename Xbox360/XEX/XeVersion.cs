/*
 * Reversed by [ Hect0r ] @ www.staticpi.net
 * With thanks to : http://www.free60.org/wiki/XEX
 * For inital stuff :)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360.XEX
{
    public class XeVersion
    {
        public byte[] version;

        public byte major
        {
            get { return (byte)(version[0] & 0xF); }
            set { version[0] ^= (byte)(value & 0xF); }
        }
        public byte minor
        {
            get { return (byte)((version[0] & 0x0F) << 4); }
            set { version[0] ^= (byte)((value >> 4) & 0x0F); }
        }
        public UInt16 build
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                {
                    return BitConverter.ToUInt16(new byte[2] { version[2], version[1] }, 0);
                }
                return BitConverter.ToUInt16(new byte[2] { version[1], version[2] }, 0);
            }
            set
            {
                byte[] data = BitConverter.GetBytes(value);

                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(data);
                }
                version[1] = data[1];
                version[2] = data[2];
            }
        }
        public byte qfe
        {
            get { return version[3]; }
            set { version[3] = value; }
        }
        public string get_string
        {
            get { return major + "." + minor + "." + build + "." + qfe; }
        }
        public XeVersion(byte[] val)
        {
            Array.Reverse(val);
            version = val;
        }
        public XeVersion() { }

        public XeVersion(UInt32 val)
        {
            byte[] buf = BitConverter.GetBytes(val);
            Array.Reverse(buf);

            version = buf;

            buf = null;
        }
}
}
