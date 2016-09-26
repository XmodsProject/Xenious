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
    public enum XeHeaderKeys : uint
    {
        RESOURCE_INFO = 0x000002FF,
        FILE_FORMAT_INFO = 0x000003FF,
        DELTA_PATCH_DESCRIPTOR = 0x000005FF,
        BASE_REFERENCE = 0x00000405,
        XGD3_MEDIA_KEY = 0x00004304,
        BOUNDING_PATH = 0x000080FF,
        DEVICE_ID = 0x00008105,
        ORIGINAL_BASE_ADDRESS = 0x00010001,
        ENTRY_POINT = 0x00010100,
        IMAGE_BASE_ADDRESS = 0x00010201,
        IMPORT_LIBRARIES = 0x000103FF,
        CHECKSUM_TIMESTAMP = 0x00018002,
        ENABLED_FOR_CALLCAP = 0x00018102,
        ENABLED_FOR_FASTCAP = 0x00018200,
        ORIGINAL_PE_NAME = 0x000183FF,
        STATIC_LIBRARIES = 0x000200FF,
        TLS_INFO = 0x00020104,
        DEFAULT_STACK_SIZE = 0x00020200,
        DEFAULT_FILESYSTEM_CACHE_SIZE = 0x00020301,
        DEFAULT_HEAP_SIZE = 0x00020401,
        PAGE_HEAP_SIZE_AND_FLAGS = 0x00028002,
        SYSTEM_FLAGS = 0x00030000,
        UNKNOWN1 = 0x00030100, // Found in dash.xex...
        EXECUTION_INFO = 0x00040006,
        TITLE_WORKSPACE_SIZE = 0x00040201,
        GAME_RATINGS = 0x00040310,
        LAN_KEY = 0x00040404,
        XBOX360_LOGO = 0x000405FF,
        MULTIDISC_MEDIA_IDS = 0x000406FF,
        ALTERNATE_TITLE_IDS = 0x000407FF,
        ADDITIONAL_TITLE_MEMORY = 0x00040801,
        EXPORTS_BY_NAME = 0x00E10402,
    }
}
