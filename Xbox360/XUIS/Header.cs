using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360.XUIS
{
    public class Header
    {
        public string Magic; //5855495A - XUIZ.
        public byte Flags; // I think this is flags.
        public byte FileVersion;
        public UInt32 FileEnd; // Pointer to endof the file.
        public UInt16 StringsEmbedded;
    }
}
