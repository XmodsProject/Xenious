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
        NTSCU = 0x000000FF,
        NTSCJ = 0x0000FF00,
        NTSCJ_JAPAN = 0x00000100,
        NTSCJ_CHINA = 0x00000200,
        PAL = 0x00FF0000,
        PAL_AU_NZ = 0x00010000,
        OTHER_ASIA = 0xFF000000,
        ALL = 0xFFFFFFFF,
    }
}
