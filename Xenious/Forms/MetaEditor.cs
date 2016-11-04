using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.ComponentModel.Design;
using Xenious.Database;
using Xbox360;
using Xbox360.XEX;
using Xbox360.PE;

namespace Xenious.Forms
{
    public partial class MetaEditor : Form
    {
        /* Version for Launcher. */
        public static string tool_version = "0.0.1000.0";

        /* Input / Output */
        XenonExecutable in_xex;

        /* Form Options and Settings */
        bool rebuild_xex = false;
        bool has_xextool = false;
        string input_file = "";

        /* Dialogs */
        Forms.Dialogs.HexEditBox hebox;
        Forms.Dialogs.TextEditBox tebox;
        Forms.Dialogs.NumericEditBox nebox;
        Forms.Dialogs.LoadFileDialog lfd;

        /* Output File */
        string output_file = "";
        XeEncryptionType output_enc_type;
        XeCompressionType output_comp_type;

        /* UI Funcs */
        public void __log(string msg)
        {
            richTextBox1.Text += string.Format("[ {0} ] - {1}\n", DateTime.Now, msg);
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
        private void disable_ui()
        {
            splitContainer1.Panel2.Controls.Clear();
            treeView1.Nodes.Clear();
            treeView1.Enabled = false;
            treeView1.Update();
            toolsToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;
        }
        public void enable_ui()
        {
            toolsToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            closeToolStripMenuItem.Enabled = true;
            treeView1.Enabled = true;
            treeView1.Nodes[0].Expand();
            treeView1.Update();
        }
        public void init_gui()
        {
            // Disable menu items, as we currently cannot rebuild.
            foreach (XeOptHeader opth in in_xex.opt_headers)
            {
                switch (opth.key)
                {
                    case XeHeaderKeys.EXECUTION_INFO:
                        executionToolStripMenuItem.Enabled = true;
                        break;
                    case XeHeaderKeys.GAME_RATINGS:
                        ratingsToolStripMenuItem1.Enabled = true;
                        break;
                    case XeHeaderKeys.ALTERNATE_TITLE_IDS:
                        alternativeToolStripMenuItem.Enabled = true;
                        break;
                    case XeHeaderKeys.LAN_KEY:
                        lanKeyToolStripMenuItem1.Enabled = true;
                        break;
                    case XeHeaderKeys.IMAGE_BASE_ADDRESS:
                        imageBaseAddressToolStripMenuItem1.Enabled = true;
                        break;
                    case XeHeaderKeys.ORIGINAL_BASE_ADDRESS:
                        originalBaseAddressToolStripMenuItem1.Enabled = true;
                        break;
                    case XeHeaderKeys.ENTRY_POINT:
                        entryPointToolStripMenuItem1.Enabled = true;
                        break;
                    case XeHeaderKeys.TITLE_WORKSPACE_SIZE:
                        titleWorkspaceSizeToolStripMenuItem.Enabled = true;
                        break;
                     case XeHeaderKeys.ENABLED_FOR_CALLCAP:
                         callcapToolStripMenuItem.Enabled = true;
                         break;
                    case XeHeaderKeys.DEVICE_ID:
                        deviceIDToolStripMenuItem1.Enabled = true;
                        break;
                    case XeHeaderKeys.BOUNDING_PATH:
                        boundingPathToolStripMenuItem1.Enabled = true;
                        break;
                    case XeHeaderKeys.DEFAULT_STACK_SIZE:
                        stackSizeToolStripMenuItem.Enabled = true;
                        break;
                    case XeHeaderKeys.DEFAULT_FILESYSTEM_CACHE_SIZE:
                        filesystemCacheSizeToolStripMenuItem.Enabled = true;
                        break;
                    case XeHeaderKeys.DEFAULT_HEAP_SIZE:
                        heapSizeToolStripMenuItem.Enabled = true;
                        break;
                    case XeHeaderKeys.XGD3_MEDIA_KEY:
                        xGD3MediaIDToolStripMenuItem1.Enabled = true;
                        break;
                    case XeHeaderKeys.ORIGINAL_PE_NAME:
                        originalPENameToolStripMenuItem.Enabled = true;
                        break;
                }
            }
            if (in_xex.base_file_info_h.enc_type == XeEncryptionType.Encrypted ||
                in_xex.base_file_info_h.comp_type == XeCompressionType.Compressed ||
                in_xex.base_file_info_h.comp_type == XeCompressionType.DeltaCompressed)
            {
                extractToolStripMenuItem.Enabled = false;
            }


            // Build treeView with original pe name
            // if we can, if not then filename.
            TreeNode node = new TreeNode();
            if (in_xex.orig_pe_name != null)
            {
                node.Text = in_xex.orig_pe_name;
            }
            else
            {
                node.Text = Path.GetFileName(in_xex.IO.file);
            }

            TreeNode secs = new TreeNode();
            secs.Text = "Security Sections";
            secs.Tag = "page sections";
            node.Nodes.Add(secs);

            TreeNode opt = new TreeNode();
            opt.Text = "Optional Data";
            opt.Tag = "page opt_headers";
            node.Nodes.Add(opt);

            // Add Opt Header Nodes.
            #region Add Optional Header Nodes to Tree.
            for (int i = 0; i < in_xex.opt_headers.Count; i++)
            {
                switch ((XeHeaderKeys)in_xex.opt_headers[i].key)
                {
                    case XeHeaderKeys.RESOURCE_INFO:
                        TreeNode node3 = new TreeNode();
                        node3.Text = "Resources";
                        node3.Tag = "page resources";
                        node.Nodes.Add(node3);
                        break;
                    case XeHeaderKeys.IMPORT_LIBRARIES:
                        TreeNode node4 = new TreeNode();
                        node4.Text = "Imports";

                        foreach (XeImportLibary lib in in_xex.import_libs)
                        {
                            TreeNode nlib = new TreeNode();
                            nlib.Text = lib.name;
                            nlib.Tag = "page import_libs " + lib.name;
                            node4.Nodes.Add(nlib);

                        }
                        node4.Collapse();
                        node.Nodes.Add(node4);
                        break;
                    case XeHeaderKeys.XBOX360_LOGO:
                        TreeNode node5 = new TreeNode();
                        node5.Text = "Xbox 360 Logo";
                        node5.Tag = "page xbox_360_logo";
                        node.Nodes.Add(node5);
                        break;
                    case XeHeaderKeys.STATIC_LIBRARIES:
                        TreeNode node6 = new TreeNode();
                        node6.Text = "Static Libaries";
                        node6.Tag = "page static_libs";
                        node.Nodes.Add(node6);
                        break;
                }
            }
            #endregion
            // Add PE Sections.
            if (in_xex.img_opt_h != null && in_xex.img_opt_h.Magic == 267)
            {
                TreeNode sections_node = new TreeNode();
                sections_node.Text = "PE Sections";

                foreach (ImageSectionHeader sec in in_xex.img_sections)
                {
                    TreeNode nodesec = new TreeNode();
                    nodesec.Text = sec.Name;
                    nodesec.Tag = "page section_edit " + sec.Name;
                    sections_node.Nodes.Add(nodesec);
                }
                sections_node.Collapse();
                node.Nodes.Add(sections_node);
            }
            treeView1.Nodes.Add(node);
            treeView1.Update();
        }

        /* Loading functions */
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
                    init_gui();
                    __log("Executable is encrypted, only edits are available...");
                    return;
                }
                else
                {
                    // Dump a zero'ed xex to cache.
                    if(Xecutable.xextool.xextool_to_raw_xextool(in_xex.IO.file, Application.StartupPath + "/cache/original.xex"))
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
                        __log("Unable to open decompressed and decrypted cache file...");
                        disable_ui();
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
                in_xex.read_dos_header();
                if (in_xex.img_dos_h.e_magic == 23117)
                {
                    in_xex.read_file_header();
                    in_xex.read_image_opt_header();
                    in_xex.read_image_sections();
                    
                    if(in_xex.img_file_h.Machine != 498)
                    {
                        MessageBox.Show("This executable has a unknown machine id, cannot continue, the xex is currupt !", "Ohhh noooo");
                        close_xex();
                        return;
                    }

                    // Extract PE Sections.
                    // HudUISkin.xex freaks here with a currupt section o.0 - TODO.
                    foreach (ImageSectionHeader ish in in_xex.img_sections)
                    {
                        in_xex.extract_pe_section(Application.StartupPath + "/cache/" + ish.Name.Replace("\0", "").Replace(".", "") + ".bin", ish);
                    }
                    __log("Extracted PE Image Sections to cache...");

                    in_xex.parse_image_sections();
                    __log("Parsed PE Image Sections...");

                    // Extract Resources.
                    if (in_xex.resources.Count > 0)
                    {
                        foreach (XeResourceInfo res in in_xex.resources)
                        {
                            in_xex.extract_resource(Application.StartupPath + "/cache/" + res.name.Replace("\0", ""), res);
                        }
                        __log("Extracted XEX Resources to cache...");

                        #region Parse XDBF
                        // Check for Game XDBF.
                        /*byte[] xdbf_title = BitConverter.GetBytes(in_xex.xeinfo.title_id);
                        Array.Reverse(xdbf_title);

                        if (File.Exists(Application.StartupPath + "/cache/" + BitConverter.ToString(xdbf_title).ToUpper().Replace("-", "")))
                        {
                            XDBF xdbf = new XDBF(Application.StartupPath + "/cache/" + BitConverter.ToString(xdbf_title).ToUpper().Replace("-", ""));
                            xdbf.read_header();
                            xdbf.read_entrys();
                            xdbf.read_free_entrys();
                            MessageBox.Show("");
                        }*/
                        #endregion
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
                        default:
                            MessageBox.Show("It appears this executable is different, please consider emailing me this xex (compress it with winrar) to sysop@staticpi.net", "What the o.0");
                            close_xex();
                            return;
                    }
                }
                init_gui();
            }
        }
        public void load_xex(string file)
        {
            in_xex = new XenonExecutable(file);
            in_xex.read_header();

            if (in_xex.is_xex == true)
            {
                init_xex(file);
                
                // Incase it was rejected for unsupport decrypt and decompress.
                if (in_xex == null)
                {
                    __log(string.Format("Unable to load file '{0}'...", file));
                    disable_ui();
                }
                else
                {
                    __log(string.Format("Loaded '{0}'...", file));
                    enable_ui();
                }
            }
            else
            {
                __log("The provided file is not a valid Xenon Executable...");
            }
        }
        public void save_xex() 
        {
            
            if(has_xextool == true && File.Exists(Application.StartupPath + "/cache/original.xex"))
            {
                
                in_xex.save_xex();

                // Delete original.
                string filename = input_file;
                in_xex.IO.close();
                in_xex = null;
                File.Delete(filename);

                // Copy original to new output file.
                File.Copy(Application.StartupPath + "/cache/original.xex", filename);
                disable_ui();
                load_xex(filename);
                __log(string.Format("Saved edits to '{0}'...", Path.GetFileName(in_xex.IO.file)));
            }
            else
            {
                in_xex.save_xex();
                __log(string.Format("Saved edits to '{0}'...", Path.GetFileName(in_xex.IO.file)));
            }
        }
        public void close_xex()
        {
            if (in_xex != null)
            {
                in_xex.IO.close();
                in_xex = null;
            }
            clear_cache();
            disable_ui();
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

        /* Form Funcs and Events */
        public MetaEditor()
        {
            InitializeComponent();
        }
        private void MainEditor_Load(object sender, EventArgs e)
        {
            __log("Meta Editor has started...");
            disable_ui();

            // Check we have a cache directory.
            if (Directory.Exists(Application.StartupPath + "/cache") == false)
            {
                Directory.CreateDirectory(Application.StartupPath + "/cache");
            }
            else // Delete previous crap.
            {
                clear_cache();
            }

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
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*lfd = new Forms.Dialogs.LoadFileDialog(has_xextool);
            lfd.ShowDialog();
            if(lfd.load_file == true)
            {
                // Close original.
                if(in_xex != null)
                {
                    in_xex.IO.close();
                    in_xex = null;
                }

                // Load new.
                output_file = lfd.output_file;
                if(has_xextool == true)
                {
                    output_enc_type = lfd.output_enc_type;
                    output_comp_type = lfd.output_comp_type;
                }
                load_xex(lfd.input_file);
            } */

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open a Xenon Executable File...";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                // Close original.
                if (in_xex != null)
                {
                    close_xex();
                }
                input_file = ofd.FileName;
                load_xex(ofd.FileName);
            }  
        }
        private void regionFlagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Xenious.Forms.Dialogs.RegionFlags rf = new Xenious.Forms.Dialogs.RegionFlags();
            rf.set_flags(in_xex.cert.game_regions);
            rf.ShowDialog();
            in_xex.cert.game_regions = rf.get_flags();
        }
        private void mediaFlagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Xenious.Forms.Dialogs.MediaFlags mf = new Xenious.Forms.Dialogs.MediaFlags();
            mf.set_flag(in_xex.cert.media_flags);
            mf.ShowDialog();
            in_xex.cert.media_flags = mf.get_flags();
        }
        private void imageFlagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Xenious.Forms.Dialogs.ImageFlags imgf = new Xenious.Forms.Dialogs.ImageFlags();
            imgf.set_flag(in_xex.cert.image_flags);
            imgf.ShowDialog();
            in_xex.cert.image_flags = imgf.get_flags();
        }
        private void systemFlagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Xenious.Forms.Dialogs.SystemFlags sf = new Xenious.Forms.Dialogs.SystemFlags();
            if (in_xex.has_header_key(XeHeaderKeys.SYSTEM_FLAGS))
            {
                sf.set_flags(in_xex.system_flags);
            }
            else
            {
                sf.set_flags(0);
            }
            sf.ShowDialog();

            if ((int)sf.get_flags() > 0)
            {
                in_xex.system_flags = sf.get_flags();
            }
            else
            {
                if(in_xex.has_header_key(XeHeaderKeys.SYSTEM_FLAGS))
                {
                    in_xex.remove_header_key(XeHeaderKeys.SYSTEM_FLAGS);
                }
            }
        }
        private void moduleFlagsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Xenious.Forms.Dialogs.ModuleFlags modf = new Xenious.Forms.Dialogs.ModuleFlags();
            modf.set_flags(in_xex.module_flags);
            modf.ShowDialog();
            in_xex.module_flags = modf.get_flags();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save_xex();
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            close_xex();
            __log("File close without saving...");
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            if (node.Text == Path.GetFileName(in_xex.IO.file) || node.Text == in_xex.orig_pe_name || node.Tag == null)
            {
                // Cancel.
                return;
            }
            string[] args = node.Tag.ToString().Split(' ');

            switch (args[0])
            {
                case "page":
                    switch (args[1])
                    {
                        case "sections":
                            Forms.Panels.Editor.Sections sections_panel = new Forms.Panels.Editor.Sections(in_xex.sections);
                            sections_panel.Dock = DockStyle.Fill;
                            splitContainer1.Panel2.Controls.Clear();
                            splitContainer1.Panel2.Controls.Add(sections_panel);
                            break;
                        case "resources":
                            Forms.Panels.Editor.Resources resources_panel = new Forms.Panels.Editor.Resources(in_xex, this);
                            resources_panel.Dock = DockStyle.Fill;

                            splitContainer1.Panel2.Controls.Clear();
                            splitContainer1.Panel2.Controls.Add(resources_panel);
                            break;
                        case "opt_headers":
                            Forms.Panels.Editor.OptHeaders opt_panel = new Forms.Panels.Editor.OptHeaders(in_xex, this);
                            opt_panel.Dock = DockStyle.Fill;

                            splitContainer1.Panel2.Controls.Clear();
                            splitContainer1.Panel2.Controls.Add(opt_panel);
                            break;
                        case "import_libs":
                            string kernal = args[2];
                            Forms.Panels.Editor.Imports imports_panel = new Forms.Panels.Editor.Imports(new XeImportLibary());
                            for (int x = 0; x < in_xex.import_libs.Count; x++)
                            {
                                if (kernal == in_xex.import_libs[x].name)
                                {
                                    imports_panel = new Forms.Panels.Editor.Imports(in_xex.import_libs[x]);
                                }
                            }
                            imports_panel.Dock = DockStyle.Fill;
                            splitContainer1.Panel2.Controls.Clear();
                            splitContainer1.Panel2.Controls.Add(imports_panel);
                            break;
                        case "xbox_360_logo":
                            ByteViewer x360logo_panel = new ByteViewer();
                            x360logo_panel.Dock = DockStyle.Fill;
                            x360logo_panel.SetBytes(in_xex.xbox_360_logo);
                            splitContainer1.Panel2.Controls.Clear();
                            splitContainer1.Panel2.Controls.Add(x360logo_panel);
                            break;
                        case "static_libs":
                            Forms.Panels.Editor.StaticLibs libs = new Forms.Panels.Editor.StaticLibs(in_xex.static_libs);
                            libs.Dock = DockStyle.Fill;
                            splitContainer1.Panel2.Controls.Clear();
                            splitContainer1.Panel2.Controls.Add(libs);
                            break;
                        case "section_edit":
                            string section = args[2];
                            ByteViewer section_panel = new ByteViewer();
                            section_panel.Dock = DockStyle.Fill;
                            section_panel.SetFile(Application.StartupPath + "/cache/" + section.Replace(".", "").Replace("\0", "") + ".bin");
                            splitContainer1.Panel2.Controls.Clear();
                            splitContainer1.Panel2.Controls.Add(section_panel);
                            break;
                    }
                    break;
            }
        }
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Dialogs.ExecutionInfo exinf = new Forms.Dialogs.ExecutionInfo(in_xex);
            exinf.Show();

            
        }
        private void addToDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (in_xex.orig_pe_name != null)
            {
                sfd.FileName = in_xex.orig_pe_name.Replace(".", "_") + "_report.json";
            }
            else
            {
                sfd.FileName = Path.GetFileName(in_xex.IO.file).Replace(".xex", "") + "_report.json";
            }
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                XEXReportFile.generate_report(sfd.FileName, in_xex);
            }
        }
        private void ratingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Xenious.Forms.Dialogs.RatingsData rdata = new Xenious.Forms.Dialogs.RatingsData();

            if (in_xex.has_header_key(XeHeaderKeys.GAME_RATINGS))
            {
                rdata.has_ratings_header = true;
                rdata.set_ratings_data(in_xex.ratings);
            }
            else
            {
                rdata.has_ratings_header = false;
            }
            rdata.ShowDialog();

            if(Xecutable.Misc.ratings_data_is_null(rdata.get_ratings()))
            {
                if(in_xex.has_header_key(XeHeaderKeys.GAME_RATINGS))
                {
                    in_xex.remove_header_key(XeHeaderKeys.GAME_RATINGS);
                }
            }
            else
            {
                in_xex.ratings = rdata.get_ratings();
            }
        }
        private void executableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                in_xex.extract_pe(sfd.FileName, true);
                __log("Extracted executable...");
            }
        }
        private void otherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Dialogs.Certificate cert = new Forms.Dialogs.Certificate(in_xex.cert);
            cert.ShowDialog();

            in_xex.cert = cert.xec;
        }
        private void titleIDsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<byte[]> listoftids = new List<byte[]>();
            Forms.Dialogs.AlternativeTitleIDs atids = new Forms.Dialogs.AlternativeTitleIDs(listoftids);
            if (in_xex.has_header_key(XeHeaderKeys.ALTERNATE_TITLE_IDS))
            {
                atids = new Forms.Dialogs.AlternativeTitleIDs(in_xex.alternative_title_ids);
            }
            else
            {
                atids = new Dialogs.AlternativeTitleIDs(listoftids);
            }

            atids.Show();

            listoftids = atids.title_ids;

            if (listoftids.Count == 0)
            {
                // Delete, as sign of remove.
                if (in_xex.has_header_key(XeHeaderKeys.ALTERNATE_TITLE_IDS))
                {
                    in_xex.remove_header_key(XeHeaderKeys.ALTERNATE_TITLE_IDS);
                }
                return;
            }

            // Add header if not there.
            if (in_xex.has_header_key(XeHeaderKeys.ALTERNATE_TITLE_IDS) == false)
            {
                in_xex.opt_headers.Add(new XeOptHeader()
                {
                    key = XeHeaderKeys.ALTERNATE_TITLE_IDS
                });

            }
            in_xex.alternative_title_ids = listoftids;
        }
        private void rebuildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save rebuilt xex as...";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                // Since we can only rebuild xex's with xextool support with encrypted / compressed xecutables...
                // Rebuild pe only if xextool support is enabled | xecutable is raw/zeroed and decrypted.
                // 
                in_xex.rebuild_xex(sfd.FileName, 
                    (has_xextool || 
                    in_xex.base_file_info_h.comp_type == XeCompressionType.Raw && in_xex.base_file_info_h.enc_type == XeEncryptionType.NotEncrypted || 
                    in_xex.base_file_info_h.comp_type == XeCompressionType.Zeroed && in_xex.base_file_info_h.enc_type == XeEncryptionType.NotEncrypted));
                __log("Rebuilt executable " + sfd.FileName + "...");
            }
        }
        private void xGD3MediaIDToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hebox = new Forms.Dialogs.HexEditBox();
            hebox.set_max_len(16);
            byte[] blank = new byte[16]
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            if (in_xex.has_header_key(XeHeaderKeys.XGD3_MEDIA_KEY))
            {
                hebox.set_value(in_xex.xgd3_media_id);
            }
            else
            {
                hebox.set_value(blank);
            }
            hebox.ShowDialog();

            if (hebox.value == blank)
            {
                if(in_xex.has_header_key(XeHeaderKeys.XGD3_MEDIA_KEY))
                {
                    in_xex.remove_header_key(XeHeaderKeys.XGD3_MEDIA_KEY);
                }
            }
            else {
                in_xex.xgd3_media_id = hebox.value;
            }
        }
        private void lanKeyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hebox = new Forms.Dialogs.HexEditBox();
            hebox.set_max_len(16);
            byte[] blank = new byte[16]
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            if (in_xex.has_header_key(XeHeaderKeys.LAN_KEY))
            {
                hebox.set_value(in_xex.lan_key);
            }
            else
            {
                hebox.set_value(blank);
            }
            hebox.ShowDialog();

            if(hebox.value != blank)
            {
                in_xex.lan_key = hebox.value;
            }
            else
            {
                if(in_xex.has_header_key(XeHeaderKeys.LAN_KEY))
                {
                    in_xex.remove_header_key(XeHeaderKeys.LAN_KEY);
                }
            }
        }
        private void deviceIDToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hebox = new Forms.Dialogs.HexEditBox();
            hebox.set_max_len(16);
            byte[] blank = new byte[16]
            {
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            if (in_xex.has_header_key(XeHeaderKeys.DEVICE_ID))
            {
                hebox.set_value(in_xex.device_id);
            }
            else
            {
                hebox.set_value(blank);
            }
            hebox.ShowDialog();

            if (hebox.value != blank)
            {
                in_xex.device_id = hebox.value;
            }
            else
            {
                if(in_xex.has_header_key(XeHeaderKeys.DEVICE_ID))
                {
                    in_xex.remove_header_key(XeHeaderKeys.DEVICE_ID);
                }
            }
        }
        private void boundingPathToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tebox = new Forms.Dialogs.TextEditBox();
            tebox.set_max_len(255);

            if(in_xex.has_header_key(XeHeaderKeys.BOUNDING_PATH))
            {
                tebox.set_value(in_xex.bound_path);
            }
            else
            {
                tebox.set_value("");
            }
            tebox.ShowDialog();

            if (tebox.value != "")
            {
                in_xex.bound_path = tebox.value;
            }
            else
            {
                if(in_xex.has_header_key(XeHeaderKeys.BOUNDING_PATH))
                {
                    in_xex.remove_header_key(XeHeaderKeys.BOUNDING_PATH);
                }
            }
        }
        private void originalPENameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tebox = new Forms.Dialogs.TextEditBox();
            if (in_xex.has_header_key(XeHeaderKeys.ORIGINAL_PE_NAME))
            {
                tebox.set_value(in_xex.orig_pe_name);
                
            }
            else
            {
                tebox.set_value("");
            }
            tebox.set_max_len(255);

            tebox.ShowDialog();
            
            if(tebox.value != "")
            {
                if(in_xex.has_header_key(XeHeaderKeys.ORIGINAL_PE_NAME) == false)
                {
                    in_xex.opt_headers.Add(new XeOptHeader()
                    {
                        key = XeHeaderKeys.ORIGINAL_PE_NAME
                    });
                }
                in_xex.orig_pe_name = tebox.value;
                treeView1.Nodes[0].Text = tebox.value;
            }
            else
            {
                if(in_xex.has_header_key(XeHeaderKeys.ORIGINAL_PE_NAME))
                {
                    // Remove Header.
                    in_xex.remove_header_key(XeHeaderKeys.ORIGINAL_PE_NAME);
                }
            }
            
        }
        private void imageBaseAddressToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);
            if (in_xex.has_header_key(XeHeaderKeys.IMAGE_BASE_ADDRESS))
            {
                nebox.set_value(in_xex.img_base_addr);
            }
            else
            {
                nebox.set_value(0);
            }
            nebox.ShowDialog();

            if (nebox.value > 0)
            {
                in_xex.img_base_addr = nebox.value;
            }
            else
            {
                if(in_xex.has_header_key(XeHeaderKeys.IMAGE_BASE_ADDRESS))
                {
                    in_xex.remove_header_key(XeHeaderKeys.IMAGE_BASE_ADDRESS);
                }
            }
        }
        private void originalBaseAddressToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);

            if (in_xex.has_header_key(XeHeaderKeys.ORIGINAL_BASE_ADDRESS))
            {
                nebox.set_value(in_xex.orig_base_addr);
            }
            else
            {
                nebox.set_value(0);
            }
            nebox.ShowDialog();

            if (nebox.value > 0)
            {
                in_xex.orig_base_addr = nebox.value;
            }
            else
            {
                if(in_xex.has_header_key(XeHeaderKeys.ORIGINAL_BASE_ADDRESS))
                {
                    in_xex.remove_header_key(XeHeaderKeys.ORIGINAL_BASE_ADDRESS);
                }
            }
        }
        private void entryPointToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);

            if (in_xex.has_header_key(XeHeaderKeys.ENTRY_POINT))
            {
                nebox.set_value(in_xex.exe_entry_point);
            }
            else
            {
                nebox.set_value(0);
            }
            nebox.ShowDialog();

            if (nebox.value > 0)
            {
                in_xex.exe_entry_point = nebox.value;
            }
            else
            {
                if(in_xex.has_header_key(XeHeaderKeys.ENTRY_POINT))
                {
                    in_xex.remove_header_key(XeHeaderKeys.ENTRY_POINT);
                }
            }
        }
        private void titleWorkspaceSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);

            if (in_xex.has_header_key(XeHeaderKeys.TITLE_WORKSPACE_SIZE))
            {
                nebox.set_value(in_xex.title_workspace_size);
            }
            else
            {
                nebox.set_value(0);
            }
            nebox.ShowDialog();

            if (nebox.value > 0)
            {
                in_xex.title_workspace_size = nebox.value;
            }
            else
            {
                if(in_xex.has_header_key(XeHeaderKeys.TITLE_WORKSPACE_SIZE))
                {
                    in_xex.remove_header_key(XeHeaderKeys.TITLE_WORKSPACE_SIZE);
                }
            }
        }
        private void stackSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);
            if (in_xex.has_header_key(XeHeaderKeys.DEFAULT_STACK_SIZE))
            {
                nebox.set_value(in_xex.default_stack_size);
            }
            else
            {
                nebox.set_value(0);
            }
            nebox.ShowDialog();

            if (nebox.value > 0)
            {
                in_xex.default_stack_size = nebox.value;
            }
            else
            {
                if(in_xex.has_header_key(XeHeaderKeys.DEFAULT_STACK_SIZE))
                {
                    in_xex.remove_header_key(XeHeaderKeys.DEFAULT_STACK_SIZE);
                }
            }
        }
        private void filesystemCacheSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);
            if (in_xex.has_header_key(XeHeaderKeys.DEFAULT_FILESYSTEM_CACHE_SIZE))
            {
                nebox.set_value(in_xex.default_fs_cache_size);
            }
            else
            {
                nebox.set_value(0);
            }
            nebox.ShowDialog();

            if (nebox.value > 0)
            {
                in_xex.default_fs_cache_size = nebox.value;
            }
            else
            {
                if(in_xex.has_header_key(XeHeaderKeys.DEFAULT_FILESYSTEM_CACHE_SIZE))
                {
                    in_xex.remove_header_key(XeHeaderKeys.DEFAULT_FILESYSTEM_CACHE_SIZE);
                }
            }
        }
        private void heapSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);

            if (in_xex.has_header_key(XeHeaderKeys.DEFAULT_HEAP_SIZE))
            {
                nebox.set_value(in_xex.default_heap_size);
            }
            else
            {
                nebox.set_value(0);
            }
            nebox.ShowDialog();

            if (nebox.value > 0)
            {
                in_xex.default_heap_size = nebox.value;
            }
            else
            {
                if(in_xex.has_header_key(XeHeaderKeys.DEFAULT_HEAP_SIZE))
                {
                    in_xex.remove_header_key(XeHeaderKeys.DEFAULT_HEAP_SIZE);
                }
            }
        }
        private void MainEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            close_xex();
            clear_cache();
        }
        private void callcapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Dialogs.CallcapData callcap = new Dialogs.CallcapData();
            if (in_xex.has_header_key(XeHeaderKeys.ENABLED_FOR_CALLCAP))
            {
                callcap.start = in_xex.callcap_start;
                callcap.end = in_xex.callcap_end;
            }
            else
            {
                callcap.start = 0;
                callcap.end = 0;
            }
            callcap.ShowDialog();


            if (callcap.start > 0 && callcap.end > 0)
            {
                // Add new Header.
                in_xex.opt_headers.Add(new XeOptHeader()
                {
                    key = XeHeaderKeys.ENABLED_FOR_CALLCAP
                });

                // Save edits.
                in_xex.callcap_start = callcap.start;
                in_xex.callcap_end = callcap.end;
            }
            else
            {
                if(in_xex.has_header_key(XeHeaderKeys.ENABLED_FOR_CALLCAP))
                {
                    in_xex.remove_header_key(XeHeaderKeys.ENABLED_FOR_CALLCAP);
                }
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
