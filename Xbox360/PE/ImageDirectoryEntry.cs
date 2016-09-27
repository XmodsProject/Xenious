using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbox360.PE
{
    public enum ImageDirectoryEntry : uint
    {
        EXPORT = 0,   /* Export Directory */
        IMPORT = 1,   /* Import Directory */
        RESOURCE = 2,   /* Resource Directory */
        EXCEPTION = 3,   /* Exception Directory */
        SECURITY = 4,   /* Security Directory */
        BASERELOC = 5,   /* Base Relocation Table */
        DEBUG = 6,   /* Debug Directory */
        COPYRIGHT = 7,   /* Description String */
        GLOBALPTR = 8,  /* Machine Value (MIPS GP) */
        TLS = 9,   /* TLS Directory */
        LOAD_CONFIG = 10   /* Load Configuration Directory */
    }
}
