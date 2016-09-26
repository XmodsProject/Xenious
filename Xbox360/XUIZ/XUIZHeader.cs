using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360.XUIZ
{
    public class XUIZHeader
    {
        public string magic; //5855495A - XUIZ.
        public UInt32 unknown;
        public UInt32 unknown2;
        public UInt32 unknown3;
        public UInt32 unknown4;
        public UInt16 unknown5;
        public UInt32 name_len;
        public string name;
    }
}
