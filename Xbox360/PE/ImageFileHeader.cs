using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360.PE
{
    /*
     * Grabbed from pe_image.h on windows.
     */
    public class ImageFileHeader
    {
        public UInt32 magic;
        public UInt16 Machine;
        public UInt16 NumberOfSections;
        public UInt32 TimeDateStamp;
        public UInt32 PointerToSymbolTable;
        public UInt32 NumberOfSymbols;
        public UInt16 SizeOfOptionalHeader;
        public UInt16 Characteristics;
    }
}
