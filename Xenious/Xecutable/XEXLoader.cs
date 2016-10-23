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
    }
}
