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
        MANUFACTURING_UTILITY = 2,
        MANUFACTURING_SUPPORT_TOOLS = 4,
        XGD2_MEDIA_ONLY = 8,
        CARDEA_KEY = 256,
        XEIKA_KEY = 512,
        USERMODE_TITLE = 1024,
        USERMODE_SYSTEM = 2048,
        ORANGE0 = 4096,
        ORANGE1 = 8192,
        ORANGE2 = 16384,
        IPTV_SIGNUP_APPLICATION = 65536,
        IPTV_TITLE_APPLICATION = 131072,
        KEYVAULT_PRIVILEGES_REQUIRED = 67108864,
        ONLINE_ACTIVATION_REQUIRED = 134217728,
        PAGE_SIZE_4KB = 134217728,
        REGION_FREE = 536870912,
        REVOCATION_CHECK_OPTIONAL = 1073741824,
        REVOCATION_CHECK_REQUIRED = 2147483648,
    }
}
