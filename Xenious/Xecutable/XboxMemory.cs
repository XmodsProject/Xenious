using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbox360.XEX;

namespace Xenious.Xecutable
{
    public class XenonMemory
    {
        public static void setup_xenon_memory(Xbox360.XenonExecutable main_app, List<Xecutable.LocalAppImport> local_imports, List<Xecutable.KernalImport> kernal_imports, Xbox360.Kernal.Memory.XboxMemory memory)
        {
            // Prepare Imports.
            List<Xbox360.XenonExecutable> imports = new List<Xbox360.XenonExecutable>();
            foreach(Xecutable.LocalAppImport imp in local_imports)
            {
                if(imp.include == true)
                {
                    // Load XEX.
                    try
                    {
                        Xbox360.XenonExecutable xex = new Xbox360.XenonExecutable(imp.filename);
                        xex.read_header();
                        xex.parse_certificate();
                        xex.parse_sections();

                        int x = xex.parse_optional_headers();

                        if (x > 0)
                        {
                            throw new Exception("Unable to parse Import Xenon Executable Option Headers (" + System.IO.Path.GetFileName(imp.filename) + "), ErrorCode : " + x.ToString());
                        }

                        // Check for encryption.
                        if (xex.base_file_info_h.enc_type == XeEncryptionType.Encrypted ||
                        xex.base_file_info_h.comp_type == XeCompressionType.Compressed ||
                        xex.base_file_info_h.comp_type == XeCompressionType.DeltaCompressed)
                        {
                            if (Xecutable.xextool.check_xextool_exists() == false)
                            {
                                throw new Exception("Unable to decrypt and decompress Import Xenon Executable...");
                            }
                            else
                            {
                                string file = xex.IO.file;

                                // Close original.
                                xex.IO.close();
                                xex = null;

                                // Dump a zero'ed xex to cache.
                                if (Xecutable.xextool.xextool_to_raw_xextool_original(file, "-c u -e u"))
                                {
                                    // Parse all xex meta info.
                                    try
                                    {
                                        xex = new Xbox360.XenonExecutable(AppDomain.CurrentDomain.BaseDirectory + "cache/" + System.IO.Path.GetFileName(file));
                                        xex.read_header();
                                        xex.parse_certificate();
                                        xex.parse_sections();
                                        x = xex.parse_optional_headers();
                                        if (x != 0)
                                        {
                                            throw new Exception("Unable to parse optional headers, error code : " + x.ToString());
                                        }
                                        // Debug - MessageBox.Show(x.ToString());
                                    }
                                    catch
                                    {
                                        throw new Exception("Unable to parse the xenon executable meta...");
                                    }
                                }
                                else
                                {
                                    throw new Exception("Unable to read decompressed and decrypted file :(");
                                }
                            }
                        }

                        // Try Load pe.
                        if(xex.load_pe() != true)
                        {
                            throw new Exception("Unable to load pe from import...");
                        }

                        // Add XEX to imports.
                        imports.Add(xex);
                    }
                    catch
                    {
                        throw new Exception("Unable to parse Import Xenon Executable (" + System.IO.Path.GetFileName(imp.filename) + ")");
                    }
                }
            }

            // Now Load Into Xenon memory.
            memory.LoadApp(main_app, imports);
        }
    }
}
