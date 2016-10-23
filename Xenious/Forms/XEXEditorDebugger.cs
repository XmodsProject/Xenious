using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xbox360;
using Xbox360.XEX;

namespace Xenious.Forms
{
    public partial class XEXEditorDebugger : Form
    {
        Forms.Dialogs.LoadFileProgress lfpd;
        Xbox360.XenonExecutable in_xex;
        bool has_xextool = Xenious.Xecutable.xextool.check_xextool_exists();

        public XEXEditorDebugger()
        {
            InitializeComponent();
        }

        public void load_file_with_db(string file, string dbfile)
        {

        }

        /* XEX Loading functions */
        public void close_xex()
        {
            in_xex.IO.close();
            in_xex = null;
        }
        public void init_xex(string file)
        {
            // Parse all xex meta info.
            try
            {
                in_xex.parse_certificate();
                in_xex.parse_sections();

                int x = in_xex.parse_optional_headers();
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

            if (in_xex.base_file_info_h.enc_type == XeEncryptionType.Encrypted ||
                in_xex.base_file_info_h.comp_type == XeCompressionType.Compressed ||
                in_xex.base_file_info_h.comp_type == XeCompressionType.DeltaCompressed)
            {
                if (has_xextool == false)
                {
                    MessageBox.Show("Please place xextool in the applications startup path called bin...", "xextool Needed...");
                    return;
                }
                else
                {
                    // Dump a zero'ed xex to cache.
                    if (Xenious.Xecutable.xextool.xextool_to_raw_xextool(in_xex.IO, "original.xex"))
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
                            int x = in_xex.parse_optional_headers();
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
                        in_xex = null;
                        return;
                    }
                }
            }

            // Check for unknown headers.
            if (in_xex.unk_headers.Count > 0)
            {
                MessageBox.Show("This executable has option header data that is currently unsupported by this editor, please consider emailing me this xex (compress it with winrar) to sysop@staticpi.net, thanks !");
                close_xex();
                return;
            }
            if ((in_xex.module_flags & (uint)XeModuleFlags.PATCH_DELTA) == (uint)XeModuleFlags.PATCH_DELTA ||
                (in_xex.module_flags & (uint)XeModuleFlags.MODULE_PATCH) == (uint)XeModuleFlags.MODULE_PATCH ||
                (in_xex.module_flags & (uint)XeModuleFlags.PATCH_FULL) == (uint)XeModuleFlags.PATCH_FULL)
            {
                MessageBox.Show("Support for XEX Patches is coming soon, the file will now be closed...", "Nearly there !");
                close_xex();
                return;
            }
            else
            {
                // Load PE.
                in_xex.pe = new PortableExecutable(in_xex.IO, (int)in_xex.pe_data_offset);
                in_xex.pe.read_dos_header();
                if (in_xex.pe.img_dos_h.e_magic == 23117)
                {
                    in_xex.pe.read_file_header();
                    in_xex.pe.read_image_opt_header();
                    in_xex.pe.read_image_sections();

                    if (in_xex.pe.img_file_h.Machine != 498)
                    {
                        MessageBox.Show("This executable has a unknown machine id, cannot continue, the xex is currupt !", "Ohhh noooo");
                        close_xex();
                        return;
                    }

                }
                else
                {
                    // Investigate pe.
                    in_xex.IO.position = in_xex.pe_data_offset;

                    // Read 4 bytes, as it cleary has a xuiz or other header.
                    string magic = in_xex.IO.read_string(4);

                    switch (magic)
                    {
                        case "XUIZ":
                            MessageBox.Show("Support for XUIZ xex's is coming soon, the file will now be closed...", "Nearly there !");
                            close_xex();
                            return;
                    }
                }
                //init_gui();
                return;
            }
        }
        public void load_xex(string file)
        {
            in_xex = new Xbox360.XenonExecutable(file);
            in_xex.read_header();

            if (in_xex.is_xex == true)
            {
                init_xex(file);
            }
            else
            {
                MessageBox.Show("The provided file is not a valid Xenon Executable...", "Error : ");
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open a Xenon Executable...";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                load_xex(ofd.FileName);
            }
        }

        private void XEXEditorDebugger_Load(object sender, EventArgs e)
        {

        }
    }
}
