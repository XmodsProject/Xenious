using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenonPowerPC.PowerPC
{
    public enum OpCodeGroup: UInt32
    {
        // Basic Power Pc Instructions.
        Unknown = 0,
        Addx = 1,
        Bx = 2,
    }
}
