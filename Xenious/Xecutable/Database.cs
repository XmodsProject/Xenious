using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xenious.Database;

namespace Xenious.Xecutable
{
    public class Database
    {

        public static PEFileDatabase generate_pe_file_template(Xbox360.XenonExecutable xex)
        {
            PEFileDatabase pefdb = new PEFileDatabase();

            pefdb.exe_name = System.IO.Path.GetFileName(xex.IO.file);
            pefdb.start_address = xex.cert.load_address;
            pefdb.end_address = (xex.cert.load_address + xex.cert.image_size);
            pefdb.sections = new List<PEFileSection>();

            foreach (Xbox360.PE.ImageSectionHeader sec in xex.img_sections)
            {
                pefdb.sections.Add(new PEFileSection()
                {
                    section_name = sec.Name.Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("\0", ""),
                    functions = new List<PEFunction>()
                });
            }

            return pefdb;
        }
    }
}
