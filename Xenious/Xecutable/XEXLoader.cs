using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbox360;
using Xbox360.XEX;

namespace Xenious.Xecutable
{
    public class LocalAppImport
    {
        public string filename;
        public bool include = false;
    }
    public class KernalImport
    {
        public string filename;
        public bool include = false;
        
        public bool is_binary()
        {
            return filename.Substring(-4, 4) == ".xex";
        }
    }
    public class XEXLoader
    {
        public static bool xex_is_encrypted_or_compressed(string file)
        {
            try
            {
                XenonExecutable xex = new XenonExecutable(file);
                xex.read_header();
                int x = xex.parse_optional_headers();

                if(x == 0)
                {
                    if (xex.base_file_info_h.enc_type == XeEncryptionType.Encrypted ||
                        xex.base_file_info_h.comp_type == XeCompressionType.Compressed ||
                        xex.base_file_info_h.comp_type == XeCompressionType.DeltaCompressed)
                    {
                        return true;
                    }
                }
            }
            catch { throw new Exception("Unable to parse executable..."); }
            return false;
        }
        public static bool import_libary_exists(string import_name)
        {
            foreach(string import in Xenious.Program.imports_available)
            {
                if(import_name == Path.GetFileName(import))
                {
                    return true;
                }
            }
            return false;
        }
        public static Xbox360.XenonExecutable get_import_libary(string import_name)
        {
            if(import_libary_exists(import_name))
            {
                return new Xbox360.XenonExecutable(AppDomain.CurrentDomain.BaseDirectory + "/kernal/imports/" + import_name);
            }
            throw new Exception("Unable to import libary from local import directory, Import Required : " + import_name);
        }
        public static bool load_xex_from_load_address(Xenious.Database.PEFileDatabase pe_db, Xbox360.Kernal.Memory.XboxMemory memory)
        {
            // First set entry point.
            memory.Position = memory.MainApp.exe_entry_point;

            // Now loop through until we hit a blr to end the function of start.
            byte[] op = new byte[4] { 0x01, 0x00, 0x00, 0x00 };

            // Get section index of text.
            int text_idx = 0;
            foreach(Xenious.Database.PEFileSection sec in pe_db.sections)
            {
                if(sec.section_name == ".text")
                {
                    break;
                }
                else
                {
                    text_idx++;
                }
            }
            // Init Text Functions List.
            pe_db.sections[text_idx].functions = new List<Xenious.Database.PEFunction>();

            // Make First One - Start.
            Xenious.Database.PEFunction pef = new Xenious.Database.PEFunction();
            pef.func_name = "start";
            pef.start_address = memory.MainApp.exe_entry_point;
            UInt32 end_addr = memory.MainApp.exe_entry_point;
            pef.op_codes = new List<byte[]>();

            // The first function ends with 0.
            while (op[0] != 0)
            {
                end_addr += 4;
                op = memory.ReadBytes(4, false);
                pef.op_codes.Add(op);
            }

            pef.end_address = end_addr;

            // Add first function to list.
            pe_db.sections[text_idx].functions.Add(pef);
            pef = null;

            return true;
        }
    }
}
