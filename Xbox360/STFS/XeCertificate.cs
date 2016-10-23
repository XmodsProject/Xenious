using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenious.IO;

namespace Xbox360.STFS
{
    public class XeCertificate
    {
        public UInt16 certificate_size;
        public byte[] console_id;
        public string console_part_number;
        public byte console_type;
        public string generation_date;
        public byte[] exponent;
        public byte[] modulus;
        public byte[] certificate_signature;
        public byte[] signature;

        public void read(FileIO IO)
        {
            IO.position = 4;
            certificate_size = IO.read_uint16(Endian.High);
            console_id = IO.read_bytes(5);
            console_part_number = IO.read_string(20);
            console_type = IO.read_byte();
            generation_date = IO.read_string(8);
            exponent = IO.read_bytes(4);
            modulus = IO.read_bytes(0x80);
            certificate_signature = IO.read_bytes(0x100);
            signature = IO.read_bytes(0x80);
        }
    }
}
