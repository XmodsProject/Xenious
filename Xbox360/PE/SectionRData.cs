using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360.PE
{
    public class RData_Import
    {
        public UInt16 kernal_idx;
        public UInt16 import_id;
    }
    public class SectionRData
    {
        public List<RData_Import> imports;

    }
}
