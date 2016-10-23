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
    public enum XeSystemFlags : uint
    {
        NO_FORCED_REBOOT = 1,
        FOREGROUND_TASKS = 2,
        NO_ODD_MAPPING = 4,
        HANDLE_MCE_INPUT = 8,
        RESTRICTED_HUD_FEATURES = 16,
        HANDLE_GAMEPAD_DISCONNECT = 32,
        INSECURE_SOCKETS = 64,
        XBOX1_INTEROPERABILITY = 128,
        DASH_CONTEXT = 256,
        USES_GAME_VOICE_CHANNEL = 512,
        PAL50_INCOMPATIBLE = 1024,
        INSECURE_UTILITY_DRIVE = 2048,
        XAM_HOOKS = 4096,
        ACCESS_PII = 8192,
        CROSS_PLATFORM_SYSTEM_LINK = 16384,
        MULTIDISC_SWAP = 32768,
        MULTIDISC_INSECURE_MEDIA = 65536,
        AP25_MEDIA = 131072,
        NO_CONFIRM_EXIT = 262144,
        ALLOW_BACKGROUND_DOWNLOAD = 524288,
        CREATE_PERSISTABLE_RAMDRIVE = 1048576,
        INHERIT_PERSISTENT_RAMDRIVE = 2097152,
        ALLOW_HUD_VIBRATION = 4194304,
        ACCESS_UTILITY_PARTITIONS = 8388608,
        IPTV_INPUT_SUPPORTED = 16777216,
        PREFER_BIG_BUTTON_INPUT = 33554432,
        ALLOW_EXTENDED_SYSTEM_RESERVATION = 67108864,
        MULTIDISC_CROSS_TITLE = 134217728,
        INSTALL_INCOMPATIBLE = 268435456,
        ALLOW_AVATAR_GET_METADATA_BY_XUID = 536870912,
        ALLOW_CONTROLLER_SWAPPING = 1073741824,
        DASH_EXTENSIBILITY_MODULE = 2147483648,
        //ALLOW_NETWORK_READ_CANCEL            = 0x0,
        //UNINTERRUPTABLE_READS                = 0x0,
        //REQUIRE_FULL_EXPERIENCE              = 0x0,
        //GAME_VOICE_REQUIRED_UI               = 0x0,
        //CAMERA_ANGLE                         = 0x0,
        //SKELETAL_TRACKING_REQUIRED           = 0x0,
        //SKELETAL_TRACKING_SUPPORTED          = 0x0,
    }
}
