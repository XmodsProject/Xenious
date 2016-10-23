using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenious.IO;

namespace Xbox360.ISO
{
    public enum FS_Entry_Type : byte
    {
        Folder = 0x10,
        Directory = 0x90,
        File = 0x80
    }
    public class FilesystemEntry
    {
        public UInt32 unknown;
        public UInt32 size;
        public UInt32 sector;
        public FS_Entry_Type attributes;
        public byte length;
        public string name;
    }
}
