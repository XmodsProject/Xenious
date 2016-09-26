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
        HARDDISK = 0x00000001,
        DVD_X2 = 0x00000002,
        DVD_CD = 0x00000004,
        DVD_5 = 0x00000008,
        DVD_9 = 0x00000010,
        SYSTEM_FLASH = 0x00000020,
        MEMORY_UNIT = 0x00000080,
        USB_MASS_STORAGE_DEVICE = 0x00000100,
        NETWORK = 0x00000200,
        DIRECT_FROM_MEMORY = 0x00000400,
        RAM_DRIVE = 0x00000800,
        SVOD = 0x00001000,
        INSECURE_PACKAGE = 0x01000000,
        SAVEGAME_PACKAGE = 0x02000000,
        LOCALLY_SIGNED_PACKAGE = 0x04000000,
        LIVE_SIGNED_PACKAGE = 0x08000000,
        XBOX_PACKAGE = 0x10000000,
    }
}
