using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbox360.PE
{
    public enum ImageLoaderFlags : uint
    {
        BREAK_ON_LOAD = 0x00000001,
        DEBUG_ON_LOAD = 0x00000002
    }
}
