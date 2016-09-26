/*
 * Thanks Xorloser
 * http://www.xboxhacker.org/index.php?topic=9344.msg60570#msg60570
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360.XEX
{
    public struct XeCompBaseFileInfo
    {
        public Int32 info_size;
        public XeEncryptionType enc_type;
        public XeCompressionType comp_type;
        public UInt32 compression_window;
        public XeCompBaseFileBlock block;
    }
}
