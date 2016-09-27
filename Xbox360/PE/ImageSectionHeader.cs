using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360.PE
{
    /*
     * Grabbed from pe_image.h on windows.
     */
    public class ImageSectionHeader
    {
        public string Name;
        public UInt32 Misc;
        public UInt32 VirtualAddress;
        public UInt32 SizeOfRawData;
        public UInt32 RawDataPtr;
        public UInt32 RelocationsPtr;
        public UInt32 LineNumsPtr;
        public UInt16 NumRelocations;
        public UInt16 NUmLineNumbers;
        public UInt32 Characteristics;
    }
}
