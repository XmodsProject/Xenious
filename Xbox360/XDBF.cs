using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xenious.IO;

namespace Xbox360
{
    public enum XDBFSectionType : uint
    {
        MetaData = 0x0001,
        Image = 0x0002,
        StringTable = 0x0003
    };
    public enum XDBFLocale : uint
    {
        Unknown = 0,
        English = 1,
        Japanese = 2,
        German = 3,
        French = 4,
        Spanish = 5,
        Italian = 6,
        Korean = 7,
        Chinese = 8,
    };
    public struct XDBFHeader
    {
        public string magic;
        public UInt32 version;
        public UInt32 entry_count;
        public UInt32 entry_used;
        public UInt32 free_count;
        public UInt32 free_used;
    };
    public struct XDBFEntry
    {
        public UInt16 section;
        public UInt64 id;
        public UInt32 offset;
        public UInt32 size;
    };
    public struct XDBFXSTC
    {
        public string magic;
        public UInt32 version;
        public UInt32 size;
        public UInt32 default_lang;
    };
    public struct XDBFXSTRHeader
    {
        public string magic;
        public UInt32 version;
        public UInt32 size;
        public UInt32 str_count;
    };
    public struct XDBFStringTableEntry
    {
        public UInt16 id;
        public UInt16 str_len;
    };
    public struct XDBFFileLoc
    {
        public UInt32 offset;
        public UInt32 size;
    }

    public class XDBF
    {
        FileIO IO;
        XDBFHeader xdbf_h;
        List<XDBFEntry> xdbf_entrys;
        List<XDBFFileLoc> xdbf_free_entrys;

        long entry_start = 0;

        public bool is_xdbf
        {
            get
            {
                if(IO != null && xdbf_h.magic == "XDBF") { return true; }
                return false;
            }
        }

        public XDBF()
        {

        }
        public XDBF(string file)
        {
            IO = new FileIO(file, FileMode.Open);
        }
        public bool read_header()
        {
            if (IO != null)
            {
                IO.position = 0;
                xdbf_h = new XDBFHeader();
                xdbf_h.magic = IO.read_string(4);
                xdbf_h.version = IO.read_uint32(Endian.High);
                xdbf_h.entry_count = IO.read_uint32(Endian.High);
                xdbf_h.entry_used = IO.read_uint32(Endian.High);
                xdbf_h.free_count = IO.read_uint32(Endian.High);
                xdbf_h.free_used = IO.read_uint32(Endian.High);
                return true;
            }
            return false;
        }
        public bool read_entrys()
        {
            if (IO != null)
            {
                IO.position = 24;
                xdbf_entrys = new List<XDBFEntry>();
                for (int i = 0; i < (int)xdbf_h.entry_count; i++)
                {
                    XDBFEntry entry = new XDBFEntry();
                    entry.section = IO.read_uint16(Endian.High);
                    entry.id = IO.read_uint64(Endian.High);
                    entry.offset = IO.read_uint32(Endian.High);
                    entry.size = IO.read_uint32(Endian.High);
                    xdbf_entrys.Add(entry);
                }
            }
            return false;
        }
        public bool read_free_entrys()
        {
            if (IO != null)
            {
                xdbf_free_entrys = new List<XDBFFileLoc>();
                for (int i = 0; i < xdbf_h.free_count; i++)
                {
                    XDBFFileLoc loc = new XDBFFileLoc();
                    loc.offset = IO.read_uint32(Endian.High);
                    loc.size = IO.read_uint32(Endian.High);
                    xdbf_free_entrys.Add(loc);
                }
                entry_start = IO.position;
                return true;
            }
            return false;
        }
    }
}
