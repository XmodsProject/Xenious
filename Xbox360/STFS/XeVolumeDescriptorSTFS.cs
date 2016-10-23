using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenious.IO;

namespace Xbox360.STFS
{
    public class XeVolumeDescriptorSTFS
    {
        public byte descriptor_size;
        public byte reserved;
        public byte block_seperation;
        public Int16 file_table_block_count;
        public Int32 file_table_block_number;
        public byte[] top_hash_table_digest;
        public Int32 total_allocated_block_count;
        public Int32 total_unallocated_block_count;

        public void read(FileIO IO)
        {
            descriptor_size = IO.read_byte();
            reserved = IO.read_byte();
            block_seperation = IO.read_byte();
            file_table_block_count = IO.read_int16(Endian.High);

            // Int24.
            byte[] data = IO.read_bytes(3);
            file_table_block_number = BitConverter.ToInt32(new byte[4] { 0x00, data[0], data[1], data[2] }, 0);
            top_hash_table_digest = IO.read_bytes(20);
            total_allocated_block_count = IO.read_int32(Endian.High);
            total_unallocated_block_count = IO.read_int32(Endian.High);
        }
    }
}
