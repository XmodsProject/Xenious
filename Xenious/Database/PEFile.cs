using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenious.Database
{ 
    public enum PEEntityType
    {
        Byte,
        Bytes,
        UInt16,
        Int16,
        UInt32,
        Int32,
        UInt64,
        Int64,
        Float
    }
    public class PEEntity {
        public byte byte_value;
        public byte[] bytes_value;
        public UInt16 uint16_value;
        public Int16 int16_value;
        public UInt32 uint32_value;
        public Int32 int32_value;
        public UInt64 uint64_value;
        public Int64 int64_value;
        public float float_value;
        public PEEntityType value_type;
        public UInt32 start_address;
    }
    public class PEImport
    {
        public UInt16 kernel_id;
        public UInt16 ordinal;
    }

    public class PEFunction
    {
        public UInt32 start_address;
        public UInt32 end_address;
        public string func_name;

        public List<byte[]> op_codes;

        public List<byte[]> get_all_out_funcs()
        {
            List<byte[]> result = new List<byte[]>();
            foreach (byte[] op in op_codes)
            {
                if(BitConverter.IsLittleEndian)
                {
                    Array.Reverse(op);
                }

                switch((XenonPowerPC.PowerPC.op_codes)XenonPowerPC.PowerPC.Functions.find_func(BitConverter.ToUInt32(op, 0)).id)
                {
                    case XenonPowerPC.PowerPC.op_codes.bne:
                    case XenonPowerPC.PowerPC.op_codes.beq:
                    case XenonPowerPC.PowerPC.op_codes.b:
                    case XenonPowerPC.PowerPC.op_codes.bl:
                    case XenonPowerPC.PowerPC.op_codes.ba:
                    case XenonPowerPC.PowerPC.op_codes.bla:
                        result.Add(op);
                        break;
                }
            }
            return result;
        }
    }
    public class PEFileSection {
        public string section_name;
        public UInt32 start_address;
        public UInt32 end_address;
        public List<PEFunction> functions;
        public List<PEImport> imports;
        public List<PEEntity> entitys;
    }

    public class PEFileDatabase
    {
        public static UInt32 current_version = 100;
        public static UInt32 last_old_version_support = 100;

        public string exe_name;
        public UInt32 start_address;
        public UInt32 end_address;
        public List<PEFileSection> sections;

        public List<Xbox360.XEX.XeSection> xesections;

        public PEFileDatabase()
        {

        }
        /* This function loads a .xedb file. */
        public PEFileDatabase(string in_file)
        {
            Xenious.IO.FileIO IO = new Xenious.IO.FileIO(in_file, System.IO.FileMode.Open);
            if(this.load_from_db_file(IO))
            {

            }
            IO.close();
            IO = null;
        }
        public bool load_from_db_file(Xenious.IO.FileIO IO)
        {
            IO.position = 0;
            
            if(IO.read_string(4) == "XEDB")
            {
                // Read Header.
                UInt32 cv = IO.read_uint32(Xbox360.IO.Endian.High);
                UInt32 xsptr = IO.read_uint32(Xbox360.IO.Endian.High);
                UInt32 sptr = IO.read_uint32(Xbox360.IO.Endian.High);

                // Check version.
                if(cv < last_old_version_support)
                {
                    return false;
                }

                // Read Meta.
                IO.position = 32;
                byte len = IO.read_byte();

                exe_name = IO.read_string(len);
                start_address = IO.read_uint32(Xbox360.IO.Endian.High);
                end_address = IO.read_uint32(Xbox360.IO.Endian.High);

                // Read XeSections.
                IO.position = xsptr;

                UInt32 size = (IO.read_uint32(Xbox360.IO.Endian.High) / 28);
                this.xesections = new List<Xbox360.XEX.XeSection>();
                for(uint i = 0; i < size; i++)
                {
                    Xbox360.XEX.XeSection sec = new Xbox360.XEX.XeSection();
                    sec.value = IO.read_uint32(Xbox360.IO.Endian.High);
                    sec.page_size = IO.read_uint32(Xbox360.IO.Endian.High);
                    sec.digest = IO.read_bytes(20);
                    this.xesections.Add(sec);
                }

                // Read Sections.
                IO.position = sptr;

                UInt32 num_sections = IO.read_uint32(Xbox360.IO.Endian.High);
                this.sections = new List<PEFileSection>();
                for(uint i = 0; i < num_sections; i++)
                {
                    PEFileSection sec = new PEFileSection();
                    byte len2 = IO.read_byte();
                    sec.section_name = IO.read_string(len2);
                    sec.start_address = IO.read_uint32(Xbox360.IO.Endian.High);
                    sec.end_address = IO.read_uint32(Xbox360.IO.Endian.High);

                    UInt32 num_funcs = IO.read_uint32(Xbox360.IO.Endian.High);
                    sec.functions = new List<PEFunction>();

                    for(uint x = 0; x < num_funcs; x++)
                    {
                        PEFunction func = new PEFunction();
                        byte len3 = IO.read_byte();

                        func.func_name = IO.read_string(len3);
                        func.start_address = IO.read_uint32(Xbox360.IO.Endian.High);
                        func.end_address = IO.read_uint32(Xbox360.IO.Endian.High);
                    }
                }
            }

            return false;
        }
        public void save_to_db_file(string out_filename)
        {
            // Lets keep it a static.
            string db_dir = AppDomain.CurrentDomain.BaseDirectory + "/Database/";

            // Check for extension.
            if (out_filename.Substring(-5, 5) != ".xedb")
            {
                out_filename += ".xedb";
            }

            // If it exists, delete it.
            if (System.IO.File.Exists(db_dir + out_filename))
            {
                System.IO.File.Delete(db_dir + out_filename);
            }

            // Now create a new handle.
            Xenious.IO.FileIO IO = new Xenious.IO.FileIO(db_dir + out_filename, System.IO.FileMode.Create);

            IO.position = 0;

            // Write out Header Info.
            IO.write(Encoding.ASCII.GetBytes("XEDB")); // Magic.
            IO.write(current_version, Xbox360.IO.Endian.High); // FileVersion.
            IO.write((UInt32)4096, Xbox360.IO.Endian.High); // XeSections Pointer.
            IO.write((UInt32)0, Xbox360.IO.Endian.High); // Leave Sections pointer blank for now.
            IO.write((UInt32)0, Xbox360.IO.Endian.High); // Reserved.
            IO.write((UInt32)0, Xbox360.IO.Endian.High); // Reserved.
            IO.write((UInt32)0, Xbox360.IO.Endian.High); // Reserved.
            IO.write((UInt32)0, Xbox360.IO.Endian.High); // Reserved.

            // Write Meta
            IO.write((byte)exe_name.Length);
            IO.write(Encoding.ASCII.GetBytes(exe_name));
            IO.write(start_address, Xbox360.IO.Endian.High);
            IO.write(end_address, Xbox360.IO.Endian.High);

            // Write out XeSections.
            IO.position = 4096;
            IO.write(0, Xbox360.IO.Endian.High); // Leave Blank for now.

            UInt32 size = 0;
            foreach(Xbox360.XEX.XeSection section in this.xesections)
            {
                IO.write(section.value, Xbox360.IO.Endian.High);
                IO.write(section.page_size, Xbox360.IO.Endian.High);
                IO.write(section.digest);
                size += 28;
            }

            long pos = IO.position; // Save Position for sections.
            IO.position = 4096;
            IO.write(size, Xbox360.IO.Endian.High); // Write out XeSections Size.

            // Write out sections Pointer.
            IO.position = 12;
            IO.write((UInt32)pos, Xbox360.IO.Endian.High);

            // Now Write out sections.
            IO.position = pos;

            // Write out number of sections.
            IO.write((UInt32)this.sections.Count, Xbox360.IO.Endian.High);

            // Write out sections.
            foreach(PEFileSection section in this.sections)
            {
                // Write out name length.
                IO.write((byte)section.section_name.Length);

                // Write out name.
                IO.write(Encoding.ASCII.GetBytes(section.section_name));

                // Write out start and end address of section.
                IO.write(section.start_address, Xbox360.IO.Endian.High);
                IO.write(section.end_address, Xbox360.IO.Endian.High);

                // Write out number of funcs.
                IO.write((UInt32)section.functions.Count, Xbox360.IO.Endian.High);

                // Write out functions.
                foreach(PEFunction func in section.functions)
                {
                    // Write out function name length.
                    IO.write((byte)func.func_name.Length);

                    // Write out Function Name.
                    IO.write(Encoding.ASCII.GetBytes(func.func_name));

                    // Write out start and end address of the function.
                    IO.write(func.start_address, Xbox360.IO.Endian.High);
                    IO.write(func.end_address, Xbox360.IO.Endian.High);
                }
            }

        }
    }

    public class PEDBFuncs {

        public static bool xex_pe_db_exists(UInt32 titleid, UInt32 mediaid, UInt32 region)
        {
            byte[] data = BitConverter.GetBytes(titleid);
            byte[] data2 = BitConverter.GetBytes(mediaid);
            byte[] data3 = BitConverter.GetBytes(region);

            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
                Array.Reverse(data2);
                Array.Reverse(data3);
            }

            string filename = string.Format("{0}/Database/{1}-{2}-{3}.xedb",
                AppDomain.CurrentDomain.BaseDirectory,
                BitConverter.ToString(data).Replace("-", ""),
                BitConverter.ToString(data2).Replace("-", ""),
                BitConverter.ToString(data3).Replace("-", "")
                );

            if(System.IO.File.Exists(filename))
            {
                return true;
            }
            return false;
        }
        public static PEFileDatabase get_pedb_by_info(UInt32 titleid, UInt32 mediaid, UInt32 region)
        {
            byte[] data = BitConverter.GetBytes(titleid);
            byte[] data2 = BitConverter.GetBytes(mediaid);
            byte[] data3 = BitConverter.GetBytes(region);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
                Array.Reverse(data2);
                Array.Reverse(data3);
            }

            string filename = string.Format("{0}/Database/{1}-{2}-{3}.xedb",
                AppDomain.CurrentDomain.BaseDirectory,
                BitConverter.ToString(data).Replace("-", ""),
                BitConverter.ToString(data2).Replace("-", ""),
                BitConverter.ToString(data3).Replace("-", "")
                );

            return new PEFileDatabase(filename);
        }
    }
}
