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
    /*
     * Thanks Xorloser
     * http://www.xboxhacker.org/index.php?topic=9344.msg60570#msg60570
     */
    public struct XeBaseFileInfoHeader
    {
        public Int32 info_size;
        public XeEncryptionType enc_type;
        public XeCompressionType comp_type;
    }
}
