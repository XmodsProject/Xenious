using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbox360.PE
{
    public enum ImageSectionType : uint
    {
        REGULAR = 0x00000000,
        DUMMY = 0x00000001,  // Reserved. */
        NO_LOAD = 0x00000002,  /* Reserved. */
        GROUPED = 0x00000004,  /* Used for 16-bit offset code. */
        NO_PAD = 0x00000008,  /* Reserved. */
        COPY = 0x00000010  /* Reserved. */
    }

    public enum ImageSectionContainsFlags : uint
    {
        CODE = 0x00000020,  /* Section contains code. */
        INITIALIZED_DATA = 0x00000040,  /* Section contains initialized data. */
        UNINITIALIZED_DATA = 0x00000080  /* Section contains uninitialized data. */
    }

    public enum ImageSectionLinkerFlags : uint
    {
        OTHER = 0x00000100,  /* Reserved. */
        INFO = 0x00000200,  /* Section contains comments or some other type of information. */
        OVERLAY = 0x00000400,  /* Section contains an overlay. */
        REMOVE = 0x00000800,  /* Section contents will not become part of image. */
        COMDAT = 0x00001000  /* Section contents comdat. */
    }
    public enum ImageSectionAlignFlags : uint
    {
        ONEBYTES = 0x00100000,
        TWOBYTES = 0x00200000,
        FOURBYTES = 0x00300000,
        EIGHTBYTES = 0x00400000,
        SIXTEENBYTES = 0x00500000,  /* Default alignment if no others are specified. */
        THIRTYTWOBYTES = 0x00600000,
        SIXTYFOURBYTES = 0x00700000
    }
}
