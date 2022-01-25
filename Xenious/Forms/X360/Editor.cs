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
using Xbox360.XEX;
using Xbox360.PE;
using Hect0rs.IO;
using Xbox360;

namespace Xenious.Forms.X360
{
    public partial class Editor : Form
    {
        /* Input / Output */
        public XenonExecutable in_xex;
        public PortableExecutable in_pe;
        public XboxUserInterfaceResource in_xuiz;

        /* Form Options and Settings */
        bool rebuild_xex = false;
        bool has_xextool = false;
        string input_file = "";

        /* Dialogs */
        Forms.Tools.HexEditBox hebox;
        Forms.Tools.TextEditBox tebox;
        Forms.Tools.NumericEditBox nebox;
        Forms.Dialogs.LoadFileDialog lfd;

        /* Output File */
        string output_file = "";
        XeEncryptionType output_enc_type;
        XeCompressionType output_comp_type;

        bool UseDevKitKeys = false;
        bool IsStrictChecks = true;

        /* UI Funcs */
        public void __log(string msg)
        {
            richTextBox1.Text += string.Format("[ {0} ] - {1}\n", DateTime.Now, msg);
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
        public void __status(string msg)
        {
            toolStripLabel1.Text = string.Format("[ Status: {0} ]", msg);
        }

        public void __title(string msg)
        {
            this.Text = string.Format("Xenious - The Xbox Executable Editor - [ {0} ]", msg);
        }
        private void disable_ui()
        {
            splitContainer2.Panel2.Controls.Clear();
            treeView1.Nodes.Clear();
            treeView1.Enabled = false;
            treeView1.Update();
            toolsToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            rebuildToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;

            __status("Ready.");
            __title("Ready");
        }
        public void enable_ui()
        {
            toolsToolStripMenuItem.Enabled = true;
            saveToolStripMenuItem.Enabled = true;
            closeToolStripMenuItem.Enabled = true;
            // temp until its fixed.
            rebuildToolStripMenuItem.Enabled = false;
            treeView1.Enabled = true;
            treeView1.Update();

            __status("Loaded.");
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        public void init_gui()
        {
            __status("Loading.");
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
            if (in_xex.base_file_info_h.comp_type == XeCompressionType.Compressed ||
                in_xex.base_file_info_h.comp_type == XeCompressionType.DeltaCompressed)
            {
                //extractToolStripMenuItem.Enabled = false;
            }


            // Build treeView with original pe name
            // if we can, if not then filename.
            TreeNode node = new TreeNode();
            if (in_xex.bound_path != null && in_xex.bound_path != "")
            {
                node.Text = Path.GetFileName(in_xex.bound_path.Replace("\0", ""));
            }
            else if (in_xex.orig_pe_name != null && in_xex.orig_pe_name != "")
            {
                node.Text = in_xex.orig_pe_name.Replace("\0", "");
            }
            else 
            {
                node.Text = Path.GetFileName(in_xex.IO.File);
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

                        // Make entry tree of resources.
                        TreeNode resent;
                        Hect0rs.IO.IOH resio = null;
                        string Tag = "";
                        for (int r = 0; r < in_xex.resources.Count; r++)
                        {
                            // check if resource has been extracted to cache.
                            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "/cache/" + in_xex.resources[r].name.Replace("\0", "")))
                            {
                                // open a file io handle.
                                resio = new IOH(AppDomain.CurrentDomain.BaseDirectory + "/cache/" + in_xex.resources[r].name.Replace("\0", ""), FileMode.Open);
                            }
                            else
                            {
                                this.in_xex.extract_resource_from_pe(this.in_pe.IO, AppDomain.CurrentDomain.BaseDirectory + "/cache/" + in_xex.resources[r].name.Replace("\0", ""), in_xex.resources[r]);
                                resio = new IOH(AppDomain.CurrentDomain.BaseDirectory + "/cache/" + in_xex.resources[r].name.Replace("\0", ""), FileMode.Open);
                            }

                            // Check Magic of resource.
                            string rm = resio.ReadString(4);
                            
                            // We dont need the io to the cache file anymore.
                            resio.close();
                            resio = null;

                            switch (rm)
                            {
                                case "XUIZ":
                                    Tag = "page resxuiz " + r.ToString();
                                    break;
                                case "XDBF":
                                    Tag = "page resxdbf " + r.ToString();
                                    break;
                                default:
                                    Tag = "page reshex " + r.ToString();
                                    break;
                            }

                            // Setup inner node.
                            resent = new TreeNode();
                            resent.Text = in_xex.resources[r].name.Replace("\0", "");
                            resent.Tag = Tag;

                            // Add inner node and delete pointer.
                            node3.Nodes.Add(resent);
                            resent = null;
                        }
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
            if (in_pe.img_opt_h != null && in_pe.img_opt_h.Magic == 267)
            {
                TreeNode sections_node = new TreeNode();
                sections_node.Text = "PE Sections";

                foreach (ImageSectionHeader sec in in_pe.img_sections)
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
            //treeView1.Nodes[0].Expand();
            //node.Expand();
            treeView1.Update();

            __status("Done.");
        }

        /* Loading functions */
        public void init_xex(string file)
        {
            // Parse all xex meta info.
            try
            {
                in_xex.parse_certificate();
                in_xex.parse_sections();
                in_xex.parse_optional_headers();
                // Debug - MessageBox.Show(x.ToString());
            }
            catch(Exception ex)
            {
                this.__log(ex.ToString());
                return;
            }

            // Check for unknown headers.
            /*if (in_xex.unk_headers.Count > 0)
            {
                MessageBox.Show("This executable has option header data that is currently unsupported by this editor, please consider emailing me this xex (compress it with winrar) to sysop@xenious.net, thanks !");
                close_xex();
                return;
            }*/
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

                // Extract PE.
                in_xex.extract_pe(AppDomain.CurrentDomain.BaseDirectory + "/cache/pe.exe", IsStrictChecks, true, UseDevKitKeys);

                // Load PE.
                in_pe = new PortableExecutable(AppDomain.CurrentDomain.BaseDirectory + "/cache/pe.exe");
                in_pe.read_dos_header();
                if (in_pe.img_dos_h.e_magic == 23117)
                {
                    in_pe.read_file_header();
                    in_pe.read_image_opt_header();
                    in_pe.read_image_sections();

                    if (in_pe.img_file_h.Machine != 498)
                    {
                        MessageBox.Show("This executable has a unknown machine id, cannot continue, the xex is currupt !", "Ohhh noooo");
                        close_xex();
                        return;
                    }

                    // Extract PE Sections.
                    // HudUISkin.xex freaks here with a currupt section o.0 - TODO.
                    foreach (ImageSectionHeader ish in in_pe.img_sections)
                    {
                        in_pe.extract_pe_section(Application.StartupPath + "/cache/" + ish.Name.Replace("\0", "").Replace(".", "") + ".bin", ish);
                    }
                    __log("Extracted PE Image Sections to cache...");

                    // Extract Resources.
                    if (in_xex.resources.Count > 0)
                    {
                        foreach (XeResourceInfo res in in_xex.resources)
                        {
                            in_xex.extract_resource_from_pe(in_pe.IO, Application.StartupPath + "/cache/" + res.name.Replace("\0", ""), res);
                        }
                        __log("Extracted XEX Resources to cache...");
                    }
                }
                
                else
                {
                    // Investigate pe.
                    in_xex.IO.Position = in_xex.pe_data_offset;

                    // Read 4 bytes, as it cleary has a xuiz or other header.
                    string magic = in_xex.IO.ReadString(4);

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
                __log(string.Format("Saved edits to '{0}'...", Path.GetFileName(in_xex.IO.File)));
            }
            else
            {
                in_xex.save_xex();
                __log(string.Format("Saved edits to '{0}'...", Path.GetFileName(in_xex.IO.File)));
            }
        }
        public void close_xex()
        {
            if (in_xex != null)
            {
                in_xex.IO.close();
                in_xex = null;
            }
            if (in_pe != null)
            {
                in_pe.IO.close();
                in_pe = null;
            }
            if (in_xuiz != null)
            {
                in_xuiz.IO.close();
                in_xuiz = null;
            }
            clear_cache();
            disable_ui();
        }

        /* Init Funcs */
        public void clear_cache()
        {
            try
            {
                string[] files = Directory.GetFiles(Application.StartupPath + "/cache");

                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            __log("Cache has been cleared...");
        }

        /* Form Funcs and Events */
        public Editor()
        {
            InitializeComponent();
        }
        private void MainEditor_Load(object sender, EventArgs e)
        {
            __log("Editor has started...");
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

            // Need to find a nicer a prettier way to set this.
            if(UseDevKitKeys == true) { KeysSwitch.Text = "[ Mode: DevKit ]"; }
            else { KeysSwitch.Text = "[ Mode: Retail ]"; }

            if (this.IsStrictChecks == true) { ValidatinButton.Text = "[ Validation: Strict ]"; }
            else { ValidatinButton.Text = "[ Validation: Relaxed ]"; }
            __status("Ready.");
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        { 
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
            __title(string.Format("Editor->{0}->RegionFlags", Path.GetFileName(in_xex.IO.File)));
            Xenious.Forms.Dialogs.RegionFlags rf = new Xenious.Forms.Dialogs.RegionFlags();
            rf.set_flags(in_xex.cert.game_regions);
            rf.ShowDialog();
            in_xex.cert.game_regions = rf.get_flags();
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void mediaFlagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->MediaFlags", Path.GetFileName(in_xex.IO.File)));
            Xenious.Forms.Dialogs.MediaFlags mf = new Xenious.Forms.Dialogs.MediaFlags();
            mf.set_flag(in_xex.cert.media_flags);
            mf.ShowDialog();
            in_xex.cert.media_flags = mf.get_flags();
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void imageFlagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->ImageFlags", Path.GetFileName(in_xex.IO.File)));
            Xenious.Forms.Dialogs.ImageFlags imgf = new Xenious.Forms.Dialogs.ImageFlags();
            imgf.set_flag(in_xex.cert.image_flags);
            imgf.ShowDialog();
            in_xex.cert.image_flags = imgf.get_flags();
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void systemFlagsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->SystemFlags", Path.GetFileName(in_xex.IO.File)));
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void moduleFlagsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->ModuleFlags", Path.GetFileName(in_xex.IO.File)));
            Xenious.Forms.Dialogs.ModuleFlags modf = new Xenious.Forms.Dialogs.ModuleFlags();
            modf.set_flags(in_xex.module_flags);
            modf.ShowDialog();
            in_xex.module_flags = modf.get_flags();
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
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
            // Need to close this.
            if(this.in_xuiz != null)
            {
                this.in_xuiz.IO.close();
                this.in_xuiz = null;
            }
            TreeNode node = treeView1.SelectedNode;
            if (node.Text == null || node.Text == Path.GetFileName(in_xex.IO.File).Replace("\0", "") || node.Text == in_xex.orig_pe_name.Replace("\0", "") || node.Tag == null)
            {
                // Cancel.
                return;
            }
            string[] args = node.Tag.ToString().Split(' ');
            int ridx = -1;
            switch (args[0])
            {
                case "page":
                    switch (args[1])
                    {
                        case "sections":
                            __title(string.Format("Editor->{0}->Sections", Path.GetFileName(in_xex.IO.File)));
                            Forms.X360.Panels.Editor.Sections sections_panel = new Forms.X360.Panels.Editor.Sections(in_xex.sections);
                            sections_panel.Dock = DockStyle.Fill;
                            splitContainer2.Panel2.Controls.Clear();
                            splitContainer2.Panel2.Controls.Add(sections_panel);
                            break;
                        case "reshex":
                            ridx = Convert.ToInt32(args[2]);
                            ByteViewer res_hex = new ByteViewer();
                            res_hex.Dock = DockStyle.Fill;
                            res_hex.SetFile(Application.StartupPath + "/cache/" + in_xex.resources[ridx].name.Replace("\0", ""));
                            splitContainer2.Panel2.Controls.Clear();
                            splitContainer2.Panel2.Controls.Add(res_hex);
                            __title(string.Format("Editor->{0}->Resource['{1}']->Hex", Path.GetFileName(in_xex.IO.File), in_xex.resources[ridx].name.Replace("\0", "")));
                            break;
                        case "resxuiz":
                            ridx = Convert.ToInt32(args[2]);
                            in_xuiz = new XboxUserInterfaceResource(AppDomain.CurrentDomain.BaseDirectory + "/cache/" + in_xex.resources[ridx].name.Replace("\0", ""));

                            Forms.X360.Panels.Editor.XUIZEditor xuizeditor = new Panels.Editor.XUIZEditor(in_xuiz, this);
                            xuizeditor.Dock = DockStyle.Fill;
                            splitContainer2.Panel2.Controls.Clear();
                            splitContainer2.Panel2.Controls.Add(xuizeditor);
                            __title(string.Format("Editor->{0}->Resource[\'{1}\']->XUIZEditor", Path.GetFileName(in_xex.IO.File), in_xex.resources[ridx].name.Replace("\0", "")));
                            break;
                        case "opt_headers":
                            Forms.X360.Panels.Editor.OptHeaders opt_panel = new Forms.X360.Panels.Editor.OptHeaders(in_xex, this);
                            opt_panel.Dock = DockStyle.Fill;

                            splitContainer2.Panel2.Controls.Clear();
                            splitContainer2.Panel2.Controls.Add(opt_panel);
                            __title(string.Format("Editor->{0}->OptHeadData", Path.GetFileName(in_xex.IO.File)));

                            break;
                        case "import_libs":
                            string kernel = args[2];
                            Forms.X360.Panels.Editor.Imports imports_panel = new Forms.X360.Panels.Editor.Imports(new XeImportLibary());
                            for (int x = 0; x < in_xex.import_libs.Count; x++)
                            {
                                if (kernel == in_xex.import_libs[x].name)
                                {
                                    imports_panel = new Forms.X360.Panels.Editor.Imports(in_xex.import_libs[x]);
                                }
                            }
                            imports_panel.Dock = DockStyle.Fill;
                            splitContainer2.Panel2.Controls.Clear();
                            splitContainer2.Panel2.Controls.Add(imports_panel);
                            __title(string.Format("Editor->{0}->Imports[\'{1}\']", Path.GetFileName(in_xex.IO.File), kernel));
                            break;
                        case "xbox_360_logo":
                            ByteViewer x360logo_panel = new ByteViewer();
                            x360logo_panel.Dock = DockStyle.Fill;
                            x360logo_panel.SetBytes(in_xex.xbox_360_logo);
                            splitContainer2.Panel2.Controls.Clear();
                            splitContainer2.Panel2.Controls.Add(x360logo_panel);
                            __title(string.Format("Editor->{0}->Xbox360Logo->Hex", Path.GetFileName(in_xex.IO.File)));
                            break;
                        case "static_libs":
                            Forms.X360.Panels.Editor.StaticLibs libs = new Forms.X360.Panels.Editor.StaticLibs(in_xex.static_libs);
                            libs.Dock = DockStyle.Fill;
                            splitContainer2.Panel2.Controls.Clear();
                            splitContainer2.Panel2.Controls.Add(libs);
                            __title(string.Format("Editor->{0}->StaticLibrarys->Editor", Path.GetFileName(in_xex.IO.File)));
                            break;
                        case "section_edit":
                            string section = args[2];
                            ByteViewer section_panel = new ByteViewer();
                            section_panel.Dock = DockStyle.Fill;
                            section_panel.SetFile(Application.StartupPath + "/cache/" + section.Replace(".", "").Replace("\0", "") + ".bin");
                            splitContainer2.Panel2.Controls.Clear();
                            splitContainer2.Panel2.Controls.Add(section_panel);
                            __title(string.Format("Editor->{0}->PE->Section[\'{1}\']->Hex", Path.GetFileName(in_xex.IO.File), section.Replace(".", "").Replace("\0", "")));
                            break;
                    }
                    break;
            }
            splitContainer2.Panel2.Update();
        }
        private void infoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->ExecutionInfo", Path.GetFileName(in_xex.IO.File)));
            Forms.X360.Dialogs.ExecutionInfo exinf = new Forms.X360.Dialogs.ExecutionInfo(in_xex);
            exinf.Show();
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));

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
                sfd.FileName = Path.GetFileName(in_xex.IO.File).Replace(".xex", "") + "_report.json";
            }
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                XEXReportFile.generate_report(sfd.FileName, in_xex);
            }
        }
        private void ratingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Xenious.Forms.Dialogs.RatingsData rdata = new Xenious.Forms.Dialogs.RatingsData();
            __title(string.Format("Editor->{0}->GameRatings", Path.GetFileName(in_xex.IO.File)));
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void executableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                in_xex.extract_pe(sfd.FileName, IsStrictChecks, true, UseDevKitKeys);
                __log("Extracted executable...");
            }
        }
        private void otherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->Certificate", Path.GetFileName(in_xex.IO.File)));
            Forms.X360.Dialogs.Certificate cert = new Forms.X360.Dialogs.Certificate(in_xex.cert);
            cert.ShowDialog();

            in_xex.cert = cert.xec;
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void titleIDsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->AlternativeTitleIDs", Path.GetFileName(in_xex.IO.File)));
            List<byte[]> listoftids = new List<byte[]>();
            Forms.X360.Dialogs.AlternativeTitleIDs atids = new Forms.X360.Dialogs.AlternativeTitleIDs(listoftids);
            if (in_xex.has_header_key(XeHeaderKeys.ALTERNATE_TITLE_IDS))
            {
                atids = new Forms.X360.Dialogs.AlternativeTitleIDs(in_xex.alternative_title_ids);
            }
            else
            {
                atids = new Forms.X360.Dialogs.AlternativeTitleIDs(listoftids);
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

            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
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
            __title(string.Format("Editor->{0}->OptHeader['XGD3MediaID']", Path.GetFileName(in_xex.IO.File)));
            hebox = new Forms.Tools.HexEditBox();
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void lanKeyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->OptHeader['LANKey']", Path.GetFileName(in_xex.IO.File)));
            hebox = new Forms.Tools.HexEditBox();
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void deviceIDToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->OptHeader['DeviceID']", Path.GetFileName(in_xex.IO.File)));
            hebox = new Forms.Tools.HexEditBox();
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void boundingPathToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->OptHeader['BoundingPath']", Path.GetFileName(in_xex.IO.File)));
            tebox = new Forms.Tools.TextEditBox();
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void originalPENameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->OptHeader['ExecutableName']", Path.GetFileName(in_xex.IO.File)));
            tebox = new Forms.Tools.TextEditBox();
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void imageBaseAddressToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->OptHeader['ImageBaseAddress']", Path.GetFileName(in_xex.IO.File)));
            nebox = new Forms.Tools.NumericEditBox();
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void originalBaseAddressToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->OptHeader['OriginalBaseAddress']", Path.GetFileName(in_xex.IO.File)));
            nebox = new Forms.Tools.NumericEditBox();
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void entryPointToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->OptHeader['EntryPoint']", Path.GetFileName(in_xex.IO.File)));
            nebox = new Forms.Tools.NumericEditBox();
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void titleWorkspaceSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->OptHeader['TitleWorkspaceSize']", Path.GetFileName(in_xex.IO.File)));
            nebox = new Forms.Tools.NumericEditBox();
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void stackSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->OptHeader['StackSize']", Path.GetFileName(in_xex.IO.File)));
            nebox = new Forms.Tools.NumericEditBox();
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void filesystemCacheSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->OptHeader['DefaultFileSystemCacheSize']", Path.GetFileName(in_xex.IO.File)));
            nebox = new Forms.Tools.NumericEditBox();
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void heapSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->OptHeader['HeapSize']", Path.GetFileName(in_xex.IO.File)));
            nebox = new Forms.Tools.NumericEditBox();
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void MainEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            close_xex();
            clear_cache();
        }
        private void callcapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            __title(string.Format("Editor->{0}->OptHeader['CallCap']", Path.GetFileName(in_xex.IO.File)));
            Forms.X360.Dialogs.CallcapData callcap = new Forms.X360.Dialogs.CallcapData();
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
            __title(string.Format("Editor->{0}", Path.GetFileName(in_xex.IO.File)));
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.About about = new About();
            about.ShowDialog();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/XenProject/Xenious/wiki");
            this.Close();
        }

        private void KeysSwitch_Click(object sender, EventArgs e)
        {
            if (UseDevKitKeys == true)
            {
                this.UseDevKitKeys = false;
                KeysSwitch.Text = "[ Mode: Retail ]";
            }
            else
            {
                this.UseDevKitKeys = true;
                KeysSwitch.Text = "[ Mode: DevKit ]";
            }
        }

        private void ValidatinButton_Click(object sender, EventArgs e)
        {
            if (this.IsStrictChecks == true)
            { 
                this.IsStrictChecks = false;
                ValidatinButton.Text = "[ Validation: Relaxed ]";
            }
            else
            {
                this.IsStrictChecks = true;
                ValidatinButton.Text = "[ Validation: Strict ]";
            }
        }
    }
}
