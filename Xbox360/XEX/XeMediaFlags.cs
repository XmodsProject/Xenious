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
    public enum XeMediaFlags : uint
    {
        HARDDISK = 1,
        DVD_X2 = 2,
        DVD_CD = 4,
        DVD_5 = 8,
        DVD_9 = 16,
        SYSTEM_FLASH = 32,
        MEMORY_UNIT = 128,
        USB_MASS_STORAGE_DEVICE = 0256,
        NETWORK = 512,
        DIRECT_FROM_MEMORY = 1024,
        RAM_DRIVE = 2048,
        SVOD = 4096,
        INSECURE_PACKAGE = 16777216,
        SAVEGAME_PACKAGE = 33554432,
        LOCALLY_SIGNED_PACKAGE = 67108864,
        LIVE_SIGNED_PACKAGE = 134217728,
        XBOX_PACKAGE = 268435456,
    }
}
