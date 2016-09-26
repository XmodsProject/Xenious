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
    public enum XeImageFlags : uint
    {
        MANUFACTURING_UTILITY = 0x00000002,
        MANUFACTURING_SUPPORT_TOOLS = 0x00000004,
        XGD2_MEDIA_ONLY = 0x00000008,
        CARDEA_KEY = 0x00000100,
        XEIKA_KEY = 0x00000200,
        USERMODE_TITLE = 0x00000400,
        USERMODE_SYSTEM = 0x00000800,
        ORANGE0 = 0x00001000,
        ORANGE1 = 0x00002000,
        ORANGE2 = 0x00004000,
        IPTV_SIGNUP_APPLICATION = 0x00010000,
        IPTV_TITLE_APPLICATION = 0x00020000,
        KEYVAULT_PRIVILEGES_REQUIRED = 0x04000000,
        ONLINE_ACTIVATION_REQUIRED = 0x08000000,
        PAGE_SIZE_4KB = 0x10000000,
        REGION_FREE = 0x20000000,
        REVOCATION_CHECK_OPTIONAL = 0x40000000,
        REVOCATION_CHECK_REQUIRED = 0x80000000,
    }
}
