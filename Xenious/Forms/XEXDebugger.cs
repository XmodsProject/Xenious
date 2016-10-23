using Microsoft.VisualBasic.Devices;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xbox360;
using Xbox360.Kernal.Memory;
using Xbox360.XEX;

namespace Xenious.Forms
{
    public partial class XEXDebugger : Form
    {
        /* Version for Launcher. */
        public static string tool_version = "0.0.420.0";

        XenonExecutable in_xex;
        Xbox360.Kernal.Memory.XboxMemory in_mem;
        bool has_xextool = false;

        public void __log(string msg)
        {
            richTextBox1.Text += string.Format("[ {0} ] - {1}\n", DateTime.Now.ToString(), msg);
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }

        /* Init Funcs */
        public void clear_cache()
        {
            string[] files = Directory.GetFiles(Application.StartupPath + "/cache");

            foreach (string file in files)
            {
                File.Delete(file);
            }
            __log("Cache has been cleared...");
        }

        /* XEX Loading Functions */
        public void close_xex()
        {
            if(in_xex != null)
            {
                in_xex.IO.close();
                in_xex = null;
            }
        }
        public void load_xex(string filename)
        {
            // Load XEX.
            try
            {
                in_xex = new XenonExecutable(filename);
                in_xex.read_header();
                in_xex.parse_certificate();
                in_xex.parse_sections();

                int x = in_xex.parse_optional_headers();

                if(x > 0)
                {
                    throw new Exception("Unable to parse Xenon Executable Option Headers, ErrorCode : " + x.ToString());
                }

                // Check for encryption.
                if (in_xex.base_file_info_h.enc_type == XeEncryptionType.Encrypted ||
                in_xex.base_file_info_h.comp_type == XeCompressionType.Compressed ||
                in_xex.base_file_info_h.comp_type == XeCompressionType.DeltaCompressed)
                {
                    if (has_xextool == false)
                    {
                        MessageBox.Show("Either add xextool to the local directory in a bin folder or decrypt and decompress this executable...", "Error : ");
                        return;
                    }
                    else
                    {
                        // Copy over any imports.
                        foreach (XeImportLibary lib in in_xex.import_libs)
                        {
                            if(System.IO.File.Exists(System.IO.Path.GetDirectoryName(in_xex.IO.file) + "/" + lib.name))
                            {
                                __log("Copying over " + lib.name + " to local cache...");
                                System.IO.File.Copy(System.IO.Path.GetDirectoryName(in_xex.IO.file) + "/" + lib.name, Application.StartupPath + "/cache/" + lib.name);
                            }
                        }

                        // Dump a zero'ed xex to cache.
                        if (Xecutable.xextool.xextool_to_raw_xextool(in_xex.IO.file, Application.StartupPath + "/cache/original.xex"))
                        {
                            // Parse all xex meta info.
                            try
                            {
                                in_xex.IO.close();
                                in_xex = null;
                                in_xex = new XenonExecutable(Application.StartupPath + "/cache/original.xex");
                                in_xex.read_header();
                                in_xex.parse_certificate();
                                in_xex.parse_sections();
                                x = in_xex.parse_optional_headers();
                                if (x != 0)
                                {
                                    throw new Exception("Unable to parse optional headers, error code : " + x.ToString());
                                }
                            }
                            catch
                            {
                                throw new Exception("Unable to parse the xenon executable meta...");
                            }
                        }
                        else
                        {
                            __log("Unable to open decompressed and decrypted cache file...");
                            in_xex = null;
                            return;
                        }
                    }
                }
            }
            catch
            {
                throw new Exception("Unable to parse Xenon Executable...");
            }
            Forms.Dialogs.XboxMemoryLoader loader = new Dialogs.XboxMemoryLoader(in_xex);
            loader.ShowDialog();

            // Check if we need kernal memory aswell.
            if(loader.kernal_imports.Count > 0)
            {
                foreach(Xecutable.KernalImport imp in loader.kernal_imports)
                {
                    if(imp.include == true)
                    {
                        in_mem = new Xbox360.Kernal.Memory.XboxMemory(0x20000000);
                    }
                }
            }
            else // Just game memory needed.
            {
                in_mem = new Xbox360.Kernal.Memory.XboxMemory(0x10000000);
            }

            // Setup Xenon Memory.
            Xecutable.XenonMemory.setup_xenon_memory(in_xex, loader.local_imports, loader.kernal_imports, in_mem);
            return;
        }

        public XEXDebugger()
        {
            InitializeComponent();
        }

        private void XEXDebugger_Load(object sender, EventArgs e)
        {
            ulong mem = ulong.Parse(new ComputerInfo().AvailablePhysicalMemory.ToString());
            if(mem / (1024 * 1024) < 512)
            {
                MessageBox.Show("Not enough free memory available (Need 512mb, Have : " + (mem / (1024 * 1024)) + ")", "Error : ");
                this.Close();
            }
            __log("Xenon Executable Editor and Debugger has started...");

            // Check for xextool Support.
            if(Xecutable.xextool.check_xextool_exists())
            {
                has_xextool = true;
                __log("xextool Tool Support Enabled...");
            }
            else
            {
                has_xextool = false;
                __log("xextool Tool Support Disabled...");
            }

            clear_cache();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Xenon Executables (*.xex*)|*.xex|All Files (*.*)|*.";
            ofd.Title = "Please open a Xenon Executable...";

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                load_xex(ofd.FileName);
            }
        }
    }
}
