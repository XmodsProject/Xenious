/*
 * Reversed by [ Hect0r ] @ www.staticpi.net
 * With thanks to : http://www.free60.org/wiki/XEX
 * For inital stuff :)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360.XEX
{
    public enum XeModuleFlags : uint
    {
        TITLE = 0x00000001,
        EXPORTS_TO_TITLE = 0x00000002,
        SYSTEM_DEBUGGER = 0x00000004,
        DLL_MODULE = 0x00000008,
        MODULE_PATCH = 0x00000010,
        PATCH_FULL = 0x00000020,
        PATCH_DELTA = 0x00000040,
        USER_MODE = 0x00000080,
    }
}
