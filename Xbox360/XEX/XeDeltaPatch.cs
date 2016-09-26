using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xbox360.XEX
{
    public class XeDeltaPatch
    {
        public byte[] header_data;
        public UInt32 size;

        public UInt32 target_version;
        public UInt32 source_version;
        public byte[] hash_source;
        public byte[] key_source;
        public UInt32 target_headers_size;
        public UInt32 headers_source_offset;
        public UInt32 headers_source_size;
        public UInt32 headers_target_offset;
        public UInt32 image_source_offset;
        public UInt32 image_source_size;
        public UInt32 image_target_offset;
    }
}
