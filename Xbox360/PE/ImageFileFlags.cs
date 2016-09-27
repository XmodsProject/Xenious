using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbox360.PE
{
    public enum ImageFlags : ushort
    {
        RELOCS_STRIPPED = 0x0001,
        EXECUTABLE_IMAGE = 0x0002, 
        LINE_NUMS_STRIPPED = 0x0004,
        LOCAL_SYMS_STRIPPED = 0x0008,
        MINIMAL_OBJECT = 0x0010,
        UPDATE_OBJECT = 0x0020,
        SIXTEENBIT_MACHINE = 0x0040,
        BYTES_REVERSED_LO = 0x0080,
        THIRTYTWOBIT_MACHINE = 0x0100,
        DEBUG_STRIPPED = 0x0200,
        PATCH = 0x0400,
        SYSTEM = 0x1000,
        DLL = 0x2000,
        BYTES_REVERSED_HI = 0x8000
    }
}
