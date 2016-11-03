using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbox360;
using Xbox360.XEX;
using System.IO;
using System.Security.Cryptography;

namespace Xenious.Database
{
    public class XEXReportFile
    {
        public static void generate_txt_report(string txt_file, XenonExecutable xex)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            string header = "[ Xenious - The Xenon Executable Editor ]\n" +
                            "Version : " + fvi.FileVersion.ToString() + "\n" +
                            "Author : [Hect0r] - staticpi.net\n";


        }

        public static void generate_html_report(string html_file, XenonExecutable xex)
        {
            string header = "";

            string report;
        }
        public static void generate_report(string report_file, XenonExecutable xex)
        {
            Crypto.Crc32 crc = new Crypto.Crc32();
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();

        }
    }
}
