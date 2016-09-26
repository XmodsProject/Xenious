/*
 * Thanks Xorloser
 * xextool / idc script.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360.XEX
{
    public enum ConnectX : uint
    {
        CxGetVersion = 0x00000001,
	    NbtNetbios = 0x00000002,
	    SmbCloseHandle = 0x00000003,
	    SmbCreateDirectoryW = 0x00000004,
	    SmbCreateFileW = 0x00000005,
	    SmbDeleteFileW = 0x00000006,
	    SmbFindClose = 0x00000007,
	    SmbFindFirstFileW = 0x00000008,
	    SmbFindNextFile = 0x00000009,
	    SmbFlushFileBuffers = 0x0000000A,
	    SmbGetDiskFreeSpaceW = 0x0000000B,
	    SmbGetFileAttributesW = 0x0000000C,
	    SmbGetFileInformationByHandle = 0x0000000D,
	    SmbGetFileSize = 0x0000000E,
	    SmbGetFileTime = 0x0000000F,
	    SmbMoveFileW = 0x00000010,
	    SmbReadFile = 0x00000011,
	    SmbRemoveDirectoryW = 0x00000012,
	    SmbSetEndOfFile = 0x00000013,
	    SmbSetFileAttributesW = 0x00000014,
	    SmbSetFilePointer = 0x00000015,
	    SmbSetFileTime = 0x00000016,
	    SmbStartup = 0x00000017,
        SmbWriteFile = 0x00000018
    }
    public enum CreateProfile : uint
    {
        CreateProfile_Register = 0x00000001,
        CreateProfile_Unregister = 0x00000002
    }
}
