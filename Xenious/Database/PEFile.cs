using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenious.Database
{
    public class PEFunction
    {
        public UInt32 start_address;
        public UInt32 end_address;
        public string func_name;

        public List<UInt32> op_codes;
    }
    public class PEFileSection {
        public string section_name;
        public List<PEFunction> functions; 
    }

    public class PEFileDatabase
    {
        public string exe_name;
        public List<PEFileSection> sections;

        public PEFileDatabase()
        {

        }
    }
}
