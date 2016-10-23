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
        TITLE = 1,
        EXPORTS_TO_TITLE = 2,
        SYSTEM_DEBUGGER = 4,
        DLL_MODULE = 8,
        MODULE_PATCH = 16,
        PATCH_FULL = 32,
        PATCH_DELTA = 64,
        USER_MODE = 128,
    }
}
