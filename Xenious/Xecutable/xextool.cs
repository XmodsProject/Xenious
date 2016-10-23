using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xenious.Xecutable
{
    public class xextool
    {
        public static bool check_xextool_exists()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/bin/xextool.exe"))
            {
                return true;
            }
            return false;
        }


        // I need the hypervisor and xboxkernal, for routines 
        // Compress, Decompression, Encryption, Decryption...
        // We use xextool instead, thanks again to xorloser !
        public static bool xex_process_xextool(string inputfile, string args, string outputfile)
        {
            Process process = new Process
            {
                StartInfo = {
                    FileName = AppDomain.CurrentDomain.BaseDirectory + "/bin/xextool.exe",
                    Arguments = string.Format("{0} {1} {2}{3}{4}", args, ((outputfile != "") ? string.Format("-o {0}{1}{2}", '"', outputfile, '"') : ""), '"', inputfile, '"'),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            process.Start();
            string str = process.StandardError.ReadToEnd();
            process.WaitForExit();
            if (str.Length > 0)
            {
                Console.WriteLine("xextool error :");
                Console.WriteLine(str);
                return false;
            }

            if (File.Exists(outputfile) == false)
            {
                return false;
            }
            return true;
        }
        public static bool xextool_to_raw_xextool(Xenious.IO.FileIO IO, string filename)
        {
            if (xex_process_xextool(IO.file, "-c u -e u", AppDomain.CurrentDomain.BaseDirectory + "/cache/" + filename) == true)
            {
                return true;
            }
            return false;
        }
    }
}
