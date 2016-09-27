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

namespace Xenious
{
    public partial class MainEditor : Form
    {
        XenonExecutable in_xex;
        bool rebuild_xex = false;
        string current_dir = Application.StartupPath;
        bool is_xexp = false;


        public void __log(string msg)
        {
            richTextBox1.Text += string.Format("[ {0} ] - {1}\n", DateTime.Now, msg);
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

        /* Loading functions */
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
            if (in_xex.has_dos_header == true && in_xex.has_file_header == true && in_xex.has_opt_header == true)
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
        public void clear_cache()
        {
            string[] files = Directory.GetFiles(Application.StartupPath + "/cache");

            foreach (string file in files)
            {
                File.Delete(file);
            }
            __log("Cache has been cleared...");
        }
        public void init_xex(string file)
        {
            // Parse all xex meta info.
            in_xex.parse_certificate();
            in_xex.parse_sections();
            in_xex.parse_optional_headers();

            // Encryption and Compression Check.
            // Current unsupported so run it through 
            // xextool -e u -c u default.xex
            if (in_xex.base_file_info_h.enc_type == XeEncryptionType.Encrypted ||
                in_xex.base_file_info_h.comp_type == XeCompressionType.Compressed ||
                in_xex.base_file_info_h.comp_type == XeCompressionType.DeltaCompressed)
            {
                init_gui();
                __log("Executable is encrypted, only edits are available...");
                return;
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
                is_xexp = true;
                MessageBox.Show("Support for XEX Patches is coming soon, the file will now be closed...", "Nearly there !");
                close_xex();
                return;
            }
            else
            {
                // Load PE.
                in_xex.read_dos_header();
                if (in_xex.has_dos_header == true)
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
                        //byte[] xdbf_title = BitConverter.GetBytes(in_xex.xeinfo.title_id);
                        //Array.Reverse(xdbf_title);

                        //if (File.Exists(Application.StartupPath + "/cache/" + BitConverter.ToString(xdbf_title).ToUpper().Replace("-", "")))
                        //{
                        //    XDBF xdbf = new XDBF(Application.StartupPath + "/cache/" + BitConverter.ToString(xdbf_title).ToUpper().Replace("-", ""));
                        //    xdbf.read_header();
                        //    xdbf.read_entrys();
                        //    xdbf.read_free_entrys();
                        //    //MessageBox.Show("");
                        //}
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
            in_xex.save_xex();
            __log(string.Format("Saved edits to '{0}'...", Path.GetFileName(in_xex.IO.file)));
        }
        public void close_xex()
        {
            in_xex.IO.close();
            in_xex = null;
            clear_cache();
            disable_ui();
        }

        public MainEditor()
        {
            InitializeComponent();
        }
        private void MainEditor_Load(object sender, EventArgs e)
        {
            __log("Xenious - The Xenon Executable Editor v0.1 - By [ Hect0r ] has started...");
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
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (in_xex != null)
            {
                close_xex();
            }
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select a Xenon Executable...";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
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
            sf.set_flags(in_xex.system_flags);
            sf.ShowDialog();
            in_xex.system_flags = sf.get_flags();
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
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // This loads the About form.
            Forms.Dialogs.About about = new Forms.Dialogs.About();
            about.ShowDialog();
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
            bool has_rating_header = false;
            for (int i = 0; i < in_xex.opt_headers.Count; i++)
            {
                if (in_xex.opt_headers[i].key == XeHeaderKeys.GAME_RATINGS)
                {
                    has_rating_header = true;
                }
            }

            if (has_rating_header == true)
            {
                rdata.has_ratings_header = true;
                rdata.set_ratings_data(in_xex.ratings);
            }
            else
            {
                rdata.has_ratings_header = false;
            }
            rdata.ShowDialog();

            if (rdata.add_ratings == true)
            {
                if (has_rating_header == true)
                {
                    // Then just replace them.
                    in_xex.ratings = rdata.get_ratings();
                }
                else
                {
                    // add them.
                    in_xex.ratings = rdata.get_ratings();

                    XeOptHeader opt = new XeOptHeader();
                    opt.key = XeHeaderKeys.GAME_RATINGS;
                    in_xex.opt_headers.Add(opt);
                }

                rebuild_xex = true;
            }
            else if (rdata.delete_ratings == true)
            {
                List<XeOptHeader> headers = new List<XeOptHeader>();
                // Recompile optheaders.
                for (int i = 0; i < in_xex.opt_headers.Count; i++)
                {
                    if (in_xex.opt_headers[i].key != XeHeaderKeys.GAME_RATINGS)
                    {
                        headers.Add(in_xex.opt_headers[i]);
                    }
                }

                in_xex.opt_headers = headers;

                rebuild_xex = true;
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
            bool has_header = false;
            foreach (XeOptHeader opt in in_xex.opt_headers)
            {
                if((UInt32)opt.key == (UInt32)XeHeaderKeys.ALTERNATE_TITLE_IDS)
                {
                    has_header = true;
                    atids = new Forms.Dialogs.AlternativeTitleIDs(in_xex.alternative_title_ids);
                }
            }

            atids.Show();

            listoftids = atids.title_ids;

            if(listoftids.Count == 0)
            {
                return;
            }

            if(has_header == false)
            {
                XeOptHeader nopt = new XeOptHeader();
                nopt.key = XeHeaderKeys.ALTERNATE_TITLE_IDS;
                in_xex.opt_headers.Add(nopt);
            }
            in_xex.alternative_title_ids = listoftids;
        }

        private void rebuildToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save rebuilt xex as...";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                in_xex.rebuild_xex(sfd.FileName, true);
                __log("Rebuilt executable " + sfd.FileName + "...");
            }
        }
    }
}
