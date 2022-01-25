using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360.XUIZ
{
    public class Header
    {
        public string Magic; //5855495A - XUIZ.
        public UInt32 Flags; // I think this is flags.
        public UInt32 FileEnd; // Pointer to endof the file (minus magic + flags) or 8 bytes.
        public UInt32 Unknown1; // usually 1.
        public UInt32 FileSystemHeaderLen; // Header size of the filesystem entrys. 
        public UInt16 EmbeddedFileCount;
    }
}
