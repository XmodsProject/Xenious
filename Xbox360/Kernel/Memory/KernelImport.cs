using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xbox360.Kernel.Memory
{
    public enum KernelImportType
    {
        XEX,
        BootLoader,
        HyperVisor,
        XboxKernel
    }
    public class KernelImport
    {
        public byte[] import_data;
        public XenonExecutable import_xex;
        public KernelImportType import_type;
    }
}
