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
        RESOURCE_INFO = 767,
        FILE_FORMAT_INFO = 1023,
        DELTA_PATCH_DESCRIPTOR = 1535,
        BASE_REFERENCE = 1029,
        XGD3_MEDIA_KEY = 17156,
        BOUNDING_PATH = 33023,
        DEVICE_ID = 33029,
        ORIGINAL_BASE_ADDRESS = 65537,
        ENTRY_POINT = 65792,
        IMAGE_BASE_ADDRESS = 66049,
        IMPORT_LIBRARIES = 66559,
        CHECKSUM_TIMESTAMP = 98306,
        ENABLED_FOR_CALLCAP = 98562,
        ENABLED_FOR_FASTCAP = 98816,
        ORIGINAL_PE_NAME = 99327,
        STATIC_LIBRARIES = 131327,
        TLS_INFO = 131332,
        DEFAULT_STACK_SIZE = 131584,
        DEFAULT_FILESYSTEM_CACHE_SIZE = 131841,
        DEFAULT_HEAP_SIZE = 132097,
        PAGE_HEAP_SIZE_AND_FLAGS = 163842,
        SYSTEM_FLAGS = 196608,
        UNKNOWN1 = 196864, // Found in dash.xex...
        EXECUTION_INFO = 262150,
        TITLE_WORKSPACE_SIZE = 262657,
        GAME_RATINGS = 262928,
        LAN_KEY = 263172,
        XBOX360_LOGO = 263679,
        MULTIDISC_MEDIA_IDS = 263935,
        ALTERNATE_TITLE_IDS = 264191,
        ADDITIONAL_TITLE_MEMORY = 264193,
        EXPORTS_BY_NAME = 14746626
    }
}
