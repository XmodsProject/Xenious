/*
 * Reversed by [ Hect0r ] @ www.staticpi.net
 * With thanks to : http://www.free60.org/wiki/XEX
 * For inital stuff :)
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xenios.Crypto;

namespace Xbox360.XEX
{
    public class XeCertificate
    {
        public UInt32 header_size;
        public UInt32 image_size;
        public byte[] rsa_sig;
        public UInt32 UnkLen;
        public UInt32 image_flags;
        public UInt32 load_address;
        public byte[] section_disgest;
        public UInt32 import_table_count;
        public byte[] import_table_digest;
        public byte[] xgd2_media_id;
        public byte[] seed_key;
        public byte[] dencrypt_key;
        public UInt32 export_table_pos;
        public byte[] header_digest;
        public UInt32 game_regions;
        public UInt32 media_flags;

        public bool is_header_hash_valid
        {
            get { return false; }
        }
        public bool is_section_hash_valid
        {
            get { return false;  }
        }
        public bool is_rsa_sig_valid
        {
            get { return false; }
        }
    }
}
