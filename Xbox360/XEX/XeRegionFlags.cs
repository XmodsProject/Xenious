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
    public enum XeRegionFlags : uint
    {
        NTSCU = 255,
        NTSCJ = 65280,
        NTSCJ_JAPAN = 256,
        NTSCJ_CHINA = 512,
        PAL = 16711680,
        PAL_AU_NZ = 65536,
        OTHER_ASIA = 4278190080,
        ALL = 4294967295,
    }
}
