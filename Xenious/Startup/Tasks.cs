using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenious.Startup
{
    public class Tasks
    {
        /*
         * Check Kernal Imports
         * This function checks if the kernal imports exist, if not it 
         * creates a folder called "Kernel/Imports".
         * It then checks if they are in decrypted and decompress format,
         * if They arnt, then it will decrypt and decompress them.
         * 
         */
        public static void check_kernal_imports()
        {
            // Check directory structure.
            if(Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "kernel/"))
            {
                if(!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "kernel/imports/"))
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "kernel/imports/");
                    return;
                }
            }
            else
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "kernel/");
                return;
            }

            // Check import files.
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "kernel/imports/");
            Xenious.Program.imports_available = new List<string>();
            foreach(string file in files)
            {
                switch(Path.GetFileName(file))
                {
                    case "xam.xex":
                    case "xbdm.xex":
                    case "xapi.xex":
                        // Load as a xex.
                        try
                        {
                            if(Xecutable.XEXLoader.xex_is_encrypted_or_compressed(file))
                            {
                                // Decrypt and decomrpess.
                                if(Xecutable.xextool.xextool_to_raw_xextool_original(file, "-c u -e u"))
                                {
                                    Xenious.Program.imports_available.Add(file);
                                }
                            }
                            else
                            {
                                Xenious.Program.imports_available.Add(file);
                            }
                        }
                        catch
                        {
                            // Unable to open file, skip it / do nothing.
                        }
                        break;
                    case "xboxkrnl.exe":
                        break;

                }
            }
        }
    }
}
