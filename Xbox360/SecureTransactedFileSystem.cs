using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenious.IO;

namespace Xbox360
{
    public class SecureTransactedFileSystem
    {
        public FileIO IO;
        public string magic;

        /* CON */
        public Xbox360.STFS.XeCertificate certificate;

        /* LIVE / PIRS */
        public byte[] pkg_signature;
        public byte[] pkg_padding;

        public List<Xbox360.STFS.XeLicenceEntrys> license_entrys;
        public byte[] header_disgest;
        public UInt32 header_size;
        public Xbox360.STFS.XeContentType content_type;
        public Int32 meta_version;
        public Int64 content_size;
        public UInt32 media_id;
        public Int32 version;
        public Int32 base_version;
        public UInt32 title_id;
        public byte platform;
        public byte executable_type;
        public byte discnumber;
        public byte disc_in_set;
        public UInt32 savegame_id;
        public byte[] console_id;
        public byte[] profile_id;
        public STFS.XeVolumeDescriptorSTFS volume_descriptor;
        public Int32 data_file_count;
        public Int64 Data_file_combined_size;
        public Int32 descriptor_type;
        public Int32 reserved;
        public byte[] padding;
        public byte[] device_id;
        public string[] display_names;
        public string[] display_descs;
        public string publisher_name;
        public string title_name;
        public byte transfer_flags;
        public Int32 thumbnail_img_size;
        public Int32 title_thumbnail_img_size;
        public byte[] thumbnail_img;
        public byte[] title_thumbnail_img;

        public SecureTransactedFileSystem(string file)
        {
            IO = new FileIO(file, System.IO.FileMode.Open);
            IO.position = 0;
        }
        public void read_header()
        {
            magic = IO.read_string(4);

            switch(magic)
            {
                case "CON ":
                    break;
                case "LIVE":
                case "PIRS":
                    pkg_signature = IO.read_bytes(0x100);
                    pkg_padding = IO.read_bytes(0x128);
                    break;
                default: return;
            }

            IO.position = 0x22c;
            // Read License entrys.
            license_entrys = new List<STFS.XeLicenceEntrys>();
            
            for(int i = 0; i < 15; i++)
            {
                Xbox360.STFS.XeLicenceEntrys lic = new STFS.XeLicenceEntrys();
                lic.read(IO);
                license_entrys.Add(lic);
            }

            header_disgest = IO.read_bytes(20);
            header_size = IO.read_uint32(Endian.High);
            content_type = (Xbox360.STFS.XeContentType)IO.read_int32(BitConverter.IsLittleEndian ? Endian.Low:Endian.High);
            meta_version = IO.read_int32(Endian.High);
            content_size = IO.read_int64(Endian.High);
            media_id = IO.read_uint32(Endian.High);
            version = IO.read_int32(Endian.High);
            base_version = IO.read_int32(Endian.High);
            title_id = IO.read_uint32(Endian.High);
            platform = IO.read_byte();
            executable_type = IO.read_byte();
            discnumber = IO.read_byte();
            disc_in_set = IO.read_byte();
            savegame_id = IO.read_uint32(Endian.High);
            console_id = IO.read_bytes(5);
            profile_id = IO.read_bytes(8);
            volume_descriptor = new STFS.XeVolumeDescriptorSTFS();
            volume_descriptor.read(IO);
            data_file_count = IO.read_int32(Endian.High);
            Data_file_combined_size = IO.read_int64(Endian.High);
            descriptor_type = IO.read_int32(Endian.High);
            reserved = IO.read_int32(Endian.High);
            padding = IO.read_bytes(0x4c);
            device_id = IO.read_bytes(0x14);

            display_names = new string[(0x900 / 0x80)];
            display_descs = new string[(0x900 / 0x80)];

            for (int i = 0; i < (0x900 / 0x80); i++)
            {
                display_names[i] = IO.read_unicode_string(64);
            }

            for (int i = 0; i < (0x900 / 0x80); i++)
            {
                display_descs[i] = IO.read_unicode_string(64);
            }

            publisher_name = IO.read_unicode_string(64);
            title_name = IO.read_unicode_string(64);
            transfer_flags = IO.read_byte();
            thumbnail_img_size = IO.read_int32(Endian.High);
            title_thumbnail_img_size = IO.read_int32(Endian.High);
            thumbnail_img = IO.read_bytes(thumbnail_img_size);

            IO.position -= thumbnail_img_size;
            IO.position += 0x4000;

            title_thumbnail_img = IO.read_bytes(title_thumbnail_img_size);

        }
        public void get_fs_table(int block)
        {
            int pos = compute_data_block(block);

            IO.position = pos;


        }
        public Int32 block_to_pos(int block)
        {
            int buf = 0;
            if(block > 0xFFFFFF) { buf = -1; }
            else { buf = ((((int)header_size + 0xFFF) & 0xF000) + (block << 12)); }
            return buf;
        }
        public Int32 compute_data_block(int block)
        {
            int shift;
            if((((int)header_size + 0xFFF) & 0xF000) == 0xB000) { shift = 1; }
            else
            {
                if((volume_descriptor.block_seperation & 1) == 1)
                {
                    shift = 0;
                }
                else
                {
                    shift = 1;
                }
            }

            int xbase = ((block + 0xAA) / 0xAA);
            if(magic == "CON ") { xbase = (xbase << shift); }
            int xreturn = (xbase + block);

            if(block > 0xAA)
            {
                xbase = ((block + 0x70e4) / 0x70e4);
                if(magic == "CON ")
                {
                    xbase = (xbase << shift);
                }
                xreturn += xbase;

                if(block > 0x70e4)
                {
                    xbase = ((block + 0x4af768) / 0x4af768);

                    int magic2 = BitConverter.ToInt32(Encoding.ASCII.GetBytes(magic), 0);
                    if(magic2 == shift) { xbase = (xbase << 1); }
                    xreturn = (xreturn + xbase);
                }
            }

            return xreturn;
        }
    }
}
