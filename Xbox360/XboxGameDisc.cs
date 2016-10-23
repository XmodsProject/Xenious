using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenious.IO;

namespace Xbox360
{
    public enum ISOType
    {
        GamePartition,
        XGD2,
        XGD3,
        None
    }

    public class XboxGameDisc
    {
        ISOType iso_type = ISOType.None;
        long root_sector = 0;
        uint root_size = 0;
        long iso_timestamp = 0;

        public FileIO IO;

        public ISOType isotype
        {
            get { return iso_type; }
        }
        public DateTime isotimestamp
        {
            get
            {
                return DateTime.FromFileTime(iso_timestamp);
            }
        }
        public List<ISO.FilesystemEntry> get_root_fs()
        {
            return parse_fs(root_sector, root_size);
        }

        public XboxGameDisc(string file)
        {
            IO = new FileIO(file, System.IO.FileMode.Open);
        }

        public void get_info()
        {
            IO.position = 65536;
            // Game Partition.
            if (IO.read_string(20) == "MICROSOFT*XBOX*MEDIA")
            {
                root_sector = (2048 * IO.read_uint32(Endian.Low));
                iso_type = ISOType.GamePartition;
            }
            else
            {
                IO.position = 265945088;
                // XGD2.
                if (IO.read_string(20) == "MICROSOFT*XBOX*MEDIA")
                {
                    root_sector = (2048 * (IO.read_uint32(Endian.Low) + 129824));
                    iso_type = ISOType.XGD2;
                }
                else
                {
                    IO.position = 34144256;
                    // XGD3.
                    if (IO.read_string(20) == "MICROSOFT*XBOX*MEDIA")
                    {
                        long x = (IO.read_uint32(Endian.Low) + 16640);
                        root_sector = (2048 * x);
                        iso_type = ISOType.XGD3;
                    }
                }
            }
            root_size = IO.read_uint32(Endian.Low);
            iso_timestamp = IO.read_int64(Endian.Low);
        }
        public List<ISO.FilesystemEntry> parse_fs(long position, uint block_size = 2048)
        {
            int read_len = 0;
            List<ISO.FilesystemEntry> entrys = new List<ISO.FilesystemEntry>();
            ISO.FilesystemEntry ent;

            while ((uint)read_len < block_size)
            {
                IO.position = (long)(position + (uint)read_len);
                // Check for the FFs.
                ent = new ISO.FilesystemEntry();
                int pad = 0;
                long x = IO.position;
                while (IO.read_byte() == 0xFF) { pad++; } // Skip through FFs at start.
                IO.position -= 1;

                if (pad <= 3) // Since rest of sector is null/0xff, check to stop reading the next sectors fs.
                {
                    ent.unknown = IO.read_uint32(Endian.Low);
                    ent.sector = IO.read_uint32(Endian.Low);
                    ent.size = IO.read_uint32(Endian.Low);
                    ent.attributes = (ISO.FS_Entry_Type)IO.read_byte();
                    ent.length = IO.read_byte();
                    ent.name = IO.read_string(ent.length);
                    if (ent.name != "" && ent.length != 255) { entrys.Add(ent); }
                }

                read_len += (int)(IO.position - x);

            }

            return entrys;
        }
        public void extract_file(ISO.FilesystemEntry entry, string outputfile)
        {
            System.IO.FileStream output = new System.IO.FileStream(outputfile, System.IO.FileMode.Create);

            switch(iso_type)
            {
                case ISOType.GamePartition:
                    IO.position = (2048 * entry.sector);
                    break;
                case ISOType.XGD2:
                    IO.position = (2048 * (entry.sector + 129856));
                    break;
            }
            output.Write(IO.read_bytes((int)entry.size), 0, (int)entry.size);
            output.Flush();
            output.Close();
            output = null;
        }
    }
}
