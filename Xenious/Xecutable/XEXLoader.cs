using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static bool import_libary_exists(string import_name)
        {
            if(File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/kernel/imports/" + import_name))
            {
                return true;
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
            UInt32 op = 1;

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
            pef.op_codes = new List<uint>();

            // The first function ends with 0.
            while (op != 0)
            {
                end_addr += 4;
                op = BitConverter.ToUInt32(memory.ReadBytes(4, false), 0);
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
