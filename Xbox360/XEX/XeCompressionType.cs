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
    public enum XeCompressionType : uint
    {
        Zeroed = 0,
        Raw = 1,
        Compressed = 2,
        DeltaCompressed = 3
    }
}
