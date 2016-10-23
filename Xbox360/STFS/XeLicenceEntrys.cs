using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenious.IO;

namespace Xbox360.STFS
{
    public class XeLicenceEntrys
    {
        public Int64 id; // XUID / PUID / ConsoleID.
        public Int32 bits;
        public Int32 flags;

        public void read(FileIO IO)
        {
            id = IO.read_int64(Endian.High);
            bits = IO.read_int32(Endian.High);
            flags = IO.read_int32(Endian.High);
        }
    }
}
