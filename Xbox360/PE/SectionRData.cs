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
    public enum Static_Type
    {
        Byte,
        Bytes,
        UInt16,
        UInt32,
        UInt64,
        Int16,
        Int32,
        Int64,
        Float,
        String
    }

    public class RData_Static
    {
        public byte[] data;
        public Static_Type type;

        public RData_Static(byte d)
        {
            data = new byte[1] { d };
        }
        public RData_Static(byte[] d)
        {
            data = d;
        }
        public RData_Static(UInt16 d)
        {
            data = new byte[2];
            //byte[] buf = BitCoverter.GetBytes()
        }

    }
    public class SectionRData
    {
        public List<RData_Import> imports;
        

    }
}
