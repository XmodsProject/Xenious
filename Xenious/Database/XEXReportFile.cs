using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xbox360;
using Xbox360.XEX;
using System.IO;

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

        }
        public static void generate_report(string report_file, XenonExecutable xex) 
        {
            // Add Header info to report.
            string report = "{" +
                            "\"magic\"=\"" + xex.magic + "\"," +
                            "\"module_flags\"=\"" + xex.module_flags.ToString() + "\"," +
                            "\"pe_data_offset\"=\"" + xex.pe_data_offset.ToString() + "\"," +
                            "\"reserved=\"" + xex.reserved.ToString() + "\"," +
                            "\"certificate_pos\"=\"" + xex.certificate_pos.ToString() + "\",";

            // Add optional headers to report.
            report += "\"option_headers\"={";

            int num = 0;
            foreach (XeOptHeader oh in xex.opt_headers)
            {
                report += "\"" + num + "\"={\"key\"=\"" + oh.key.ToString() + "\", \"data\"=\"" + oh.data.ToString() + "\"}";
                // Add ending tag if needed.
                if (num == (xex.opt_headers.Count - 1)) { report += ","; }
            }

            // Add option headers data to report.
            for (int i = 0; i < xex.opt_headers.Count; i++)
            {
                switch ((uint)xex.opt_headers[i].key)
                {
                    case (uint)XeHeaderKeys.RESOURCE_INFO:
                        break;
                    case (uint)XeHeaderKeys.FILE_FORMAT_INFO: break;
                    case (uint)XeHeaderKeys.DELTA_PATCH_DESCRIPTOR: break;
                    case (uint)XeHeaderKeys.BASE_REFERENCE: break;
                    case (uint)XeHeaderKeys.XGD3_MEDIA_KEY:
                        break;
                    case (uint)XeHeaderKeys.BOUNDING_PATH:
                        break;
                    case (uint)XeHeaderKeys.DEVICE_ID: break;
                    case (uint)XeHeaderKeys.ORIGINAL_BASE_ADDRESS:
                        break;
                    case (uint)XeHeaderKeys.ENTRY_POINT:
                        break;
                    case (uint)XeHeaderKeys.IMAGE_BASE_ADDRESS:
                        break;
                    case (uint)XeHeaderKeys.IMPORT_LIBRARIES: break;
                    case (uint)XeHeaderKeys.CHECKSUM_TIMESTAMP:
                        break;
                    case (uint)XeHeaderKeys.ENABLED_FOR_CALLCAP:
                        break;
                    case (uint)XeHeaderKeys.ENABLED_FOR_FASTCAP: break;
                    case (uint)XeHeaderKeys.ORIGINAL_PE_NAME:
                        break;
                    case (uint)XeHeaderKeys.STATIC_LIBRARIES:
                        break;
                    case (uint)XeHeaderKeys.TLS_INFO: break;
                    case (uint)XeHeaderKeys.DEFAULT_STACK_SIZE: break;
                    case (uint)XeHeaderKeys.DEFAULT_FILESYSTEM_CACHE_SIZE: break;
                    case (uint)XeHeaderKeys.DEFAULT_HEAP_SIZE: break;
                    case (uint)XeHeaderKeys.PAGE_HEAP_SIZE_AND_FLAGS: break;
                    case (uint)XeHeaderKeys.SYSTEM_FLAGS:
                        break;
                    //case (uint)XeHeaderKeys.UNKNOWN1: break; // Found in dash.xex...
                    case (uint)XeHeaderKeys.EXECUTION_INFO:
                        break;
                    case (uint)XeHeaderKeys.TITLE_WORKSPACE_SIZE: break;
                    case (uint)XeHeaderKeys.GAME_RATINGS:
                        break;
                    case (uint)XeHeaderKeys.LAN_KEY: break;
                    case (uint)XeHeaderKeys.XBOX360_LOGO: break;
                    case (uint)XeHeaderKeys.MULTIDISC_MEDIA_IDS: break;
                    case (uint)XeHeaderKeys.ALTERNATE_TITLE_IDS: break;
                    case (uint)XeHeaderKeys.ADDITIONAL_TITLE_MEMORY: break;
                    case (uint)XeHeaderKeys.EXPORTS_BY_NAME: break;

                    default:
                        //unk_headers.Add(xex.opt_headers[i]);
                        break;
                }
            }
            // Add pe headers to report.

            // add pe sections to report.

            // Output report.
            report += "}";
            File.WriteAllText(report_file, report);
        }
    }
}
