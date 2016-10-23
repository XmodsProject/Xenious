using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenious.Database
{
    public class PEBranchesOut
    {
        public UInt32 OPcode;
        public UInt32 address;
        public UInt32 branch_address;
    }
    public class PE_Function
    {
        public string name;
        public UInt32 start; // Start address in xenon memory.
        public UInt32 end; // End address in xenon memory.

        public List<byte[]> op_codes;

        public PE_Function(Xbox360.Kernal.XenonMemory in_memory)
        {
            in_memory.position = start;
            op_codes = new List<byte[]>();
            while (in_memory.position != end)
            {
                op_codes.Add(in_memory.read_bytes(4));
            }
            return;
        }

        public List<PEBranchesOut> get_branches()
        {
            return new List<PEBranchesOut>();
        }
    }
}
