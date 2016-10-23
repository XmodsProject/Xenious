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
using Xenious.IO;

namespace Xenious
{
    public partial class MetaEditor : Form
    {
        public enum InputType
        {
            XEX,
            PE,
            STFS,
            ISO,
            None
        };

        /* Input / Output */
        XenonExecutable in_xex; // Default.xex
        Xbox360.Kernal.XenonMemory in_memory;
        PortableExecutable in_exe;
        XenonISO in_iso;
        SecureTransactedFileSystem in_stfs;

        /* Form Options and Settings */
        bool rebuild_xex = false;
        bool has_xextool = false;
        bool is_stfs = false;
        bool is_iso = false;
        bool is_edit_mode = false;
        string input_file = "";
        InputType in_type = InputType.None;

        /* Dialogs */
        Forms.Dialogs.HexEditBox hebox;
        Forms.Dialogs.TextEditBox tebox;
        Forms.Dialogs.NumericEditBox nebox;
        Forms.Dialogs.LoadFileDialog lfd;

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
        public void enable_xex_gui()
        {
            treeView1.Nodes.Clear();
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
                    case XeHeaderKeys.MULTIDISC_MEDIA_IDS:
                        multidiscMediaIDsToolStripMenuItem.Enabled = true;
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

            TreeNode pe_node = new TreeNode();
            pe_node.Text = "Portable Executable";

            // Add PE Headers.
            TreeNode dos_head = new TreeNode();
            dos_head.Text = "DOS Header";
            dos_head.Tag = "page edit_pe_dos_header";
            pe_node.Nodes.Add(dos_head);

            TreeNode file_head = new TreeNode();
            file_head.Text = "File Header";
            file_head.Tag = "page edit_pe_file_header";
            pe_node.Nodes.Add(file_head);

            TreeNode opt_head = new TreeNode();
            opt_head.Text = "Optional Header";
            opt_head.Tag = "page edit_pe_opt_header";
            pe_node.Nodes.Add(opt_head);

            // Add PE Sections.
            if (in_xex.pe.img_opt_h != null && in_xex.pe.img_opt_h.Magic == 267)
            {
                TreeNode sections_node = new TreeNode();
                sections_node.Text = "Code Sections";

                foreach (ImageSectionHeader sec in in_xex.pe.img_sections)
                {
                    TreeNode nodesec = new TreeNode();
                    nodesec.Text = sec.Name;
                    nodesec.Tag = "page section_edit " + sec.Name;
                    sections_node.Nodes.Add(nodesec);
                }
                sections_node.Collapse();
                pe_node.Nodes.Add(sections_node);
            }

            node.Nodes.Add(pe_node);
            treeView1.Nodes.Add(node);
            treeView1.Update();
        }

        public void init_gui_edit()
        {
            switch(in_type)
            {
                case InputType.STFS:
                    break;
                case InputType.XEX:
                    enable_xex_gui();
                    break;
                case InputType.PE:
                    break;
                case InputType.ISO:
                    break;
            }
            
        }

        /* XEX Loading functions */
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
                    init_gui_edit();
                    __log("Executable is encrypted, only edits are available...");
                    return;
                }
                else
                {
                    // Dump a zero'ed xex to cache.
                    if(xextool_to_raw_xextool())
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
                in_xex.pe = new PortableExecutable(in_xex.IO, (int)in_xex.pe_data_offset);
                in_xex.pe.read_dos_header();
                if (in_xex.pe.img_dos_h.e_magic == 23117)
                {
                    in_xex.pe.read_file_header();
                    in_xex.pe.read_image_opt_header();
                    in_xex.pe.read_image_sections();
                    
                    if(in_xex.pe.img_file_h.Machine != 498)
                    {
                        MessageBox.Show("This executable has a unknown machine id, cannot continue, the xex is currupt !", "Ohhh noooo");
                        close_xex();
                        return;
                    }

                    __log("Parsed Portable Executable...");

                    // Extract PE Sections.
                    // HudUISkin.xex freaks here with a currupt section o.0 - TODO.
                    foreach (ImageSectionHeader ish in in_xex.pe.img_sections)
                    {
                        in_xex.pe.extract_section(Application.StartupPath + "/cache/" + ish.Name.Replace("\0", "").Replace(".", "") + ".bin", ish);
                    }
                    __log("Extracted PE Image Sections to cache...");

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
                init_gui_edit();
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
                    is_edit_mode = true;
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

            if (is_edit_mode == true)
            {
                if (has_xextool == true && File.Exists(Application.StartupPath + "/cache/original.xex"))
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
            else
            {
                switch(in_type)
                {
                    case InputType.ISO:

                        break;
                    case InputType.PE:
                        break;
                    case InputType.STFS:
                        break;
                    case InputType.XEX:
                        // Since it isnt edit mode, we know its being 
                        // disasembeled.

                        break;
                }
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

        /* PE Loading Funtions */
        public void init_pe(string file)
        {
            treeView1.Nodes.Clear();
            TreeNode node = new TreeNode();
            node.Text = Path.GetFileName(file);

            TreeNode dos_head = new TreeNode();
            dos_head.Text = "DOS Header";
            dos_head.Tag = "page edit_pe_dos_header";
            node.Nodes.Add(dos_head);

            TreeNode file_head = new TreeNode();
            file_head.Text = "File Header";
            file_head.Tag = "page edit_pe_file_header";
            node.Nodes.Add(file_head);

            TreeNode opt_head = new TreeNode();
            opt_head.Text = "Optional Header";
            opt_head.Tag = "page edit_pe_opt_header";
            node.Nodes.Add(opt_head);

            TreeNode sections = new TreeNode();
            sections.Text = "Code Sections";

            foreach (ImageSectionHeader ish in in_exe.img_sections)
            {
                TreeNode sec = new TreeNode();
                sec.Text = ish.Name.Replace("\r", "").Replace("\n", "").Replace("\0", "");
                sec.Tag = "page section_edit " + sec.Name;
                sections.Nodes.Add(sec);
            }
            node.Nodes.Add(sections);

            // Init TreeView1.
            treeView1.Nodes.Add(node);

        }
        public void load_pe(string file)
        {
            in_exe = new PortableExecutable(file);
            in_exe.read_dos_header();
            in_exe.read_file_header();
            in_exe.read_image_opt_header();
            in_exe.read_image_sections();

            if (in_exe.img_file_h.Machine == 498)
            {
                foreach (ImageSectionHeader ish in in_exe.img_sections)
                {
                    in_exe.extract_section(Application.StartupPath + "/cache/" + ish.Name.Replace("\0", "").Replace(".", "") + ".bin", ish);
                }
                __log("Extracted PE Image Sections to cache...");

                init_pe(file);

                __log(string.Format("Loaded '{0}'...", file));
                
                 enable_ui();
            }
            else
            {
                __log("The provided file is not a valid Xenon Portable Executable...");
            }
        }

        /* STFS Loading Functions */
        public void load_stfs(string file)
        {
            in_stfs = new SecureTransactedFileSystem(file);

            in_stfs.read_header();
            return;
        }

        /* ISO Loading Functions */
        public void load_iso(string file)
        {
            in_iso = new XenonISO(file);
            in_iso.get_info();

            if(in_iso.isotype != ISOType.None)
            {
                __log("Loading " + in_iso.isotype.ToString() + " Image...");
                List<Xbox360.ISO.FilesystemEntry> entrys = in_iso.get_root_fs();

                List<Xbox360.ISO.FilesystemEntry> extract = new List<Xbox360.ISO.FilesystemEntry>();

                // Now pullout any xex's.
                foreach (Xbox360.ISO.FilesystemEntry entry in entrys)
                {
                    if (entry.name.Length > 4)
                    {
                        string ext = entry.name.Substring(entry.name.Length - 4, 4);
                        switch (ext)
                        {
                            case ".xex":
                            case "xexp":
                            case ".dll":
                                extract.Add(entry);
                                break;

                        }
                    }
                }

                // Extract Executables.
                foreach(Xbox360.ISO.FilesystemEntry entry in extract)
                {
                    in_iso.extract_file(entry, Application.StartupPath + "/cache/" + entry.name);
                }
                return;
            }
            else
            {
                MessageBox.Show("Invalid Xbox 360 ISO / Game Partition :(", "Error : ");
                close_file();
            }
        }

        public void load_file(string file)
        {
            // Get the magic first.
            string magic = "";
            try
            {
                FileIO io = new FileIO(file, FileMode.Open);
                io.position = 0;
                magic = io.read_string(4);
                io.close();
                io = null;
            }
            catch
            {
                throw new Exception("Unable to read input file, probably being used by another process.");
            }
            // Workout what kind of file it is.
            switch(magic)
            {
                case "CON ":
                case "LIVE":
                case "PIRS":
                    in_type = InputType.STFS;
                    load_stfs(file);
                    break;

                case "XEX2":
                case "XEX1":
                    in_type = InputType.XEX;
                    load_xex(file);
                    break;
                default:
                    // Check if its a PE.
                    if(magic.Substring(0, 2) == "MZ")
                    {
                        // Xenon PE.
                        in_type = InputType.PE;
                        load_pe(file);
                    }
                    else
                    {
                        // Check if its a iso.
                        FileInfo info = new FileInfo(file);

                        if(info.Length >= 1073741824)
                        {
                            load_iso(file);
                        }
                        else
                        {
                            __log("Unable to determine file format, aborting load...");
                        }
                    }
                    break;
            }
        }
        public void close_file()
        {
            switch(in_type)
            {
                case InputType.XEX:
                    in_xex.IO.close();
                    in_xex = null;
                    break;
                case InputType.PE:
                    in_exe.IO.close();
                    in_exe = null;
                    break;
                case InputType.STFS:
                    in_stfs.IO.close();
                    in_stfs = null;
                    break;
                case InputType.ISO:
                    in_iso.IO.close();
                    in_iso = null;
                    break;
            }
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
        public void check_xextool_exists()
        {
            if(File.Exists(Application.StartupPath + "/bin/xextool.exe"))
            {
                has_xextool = true;
                __log("xextool Support Enabled...");
            }
            else
            {
                has_xextool = false;
                __log("xextool Support Disabled...");
            }
        }


        // I need the hypervisor and xboxkernal, for routines 
        // Compress, Decompression, Encryption, Decryption...
        // We use xextool instead, thanks again to xorloser !
        public bool xex_process_xextool(string inputfile, string args, string outputfile)
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

            if(File.Exists(outputfile) == false)
            {
                return false;
            }
            return true;
        }
        public bool xextool_to_raw_xextool()
        {
            if(xex_process_xextool(in_xex.IO.file, "-c u -e u", Application.StartupPath + "/cache/original.xex") == true)
            {
                __log("xextool : Decompressed and Decrypted Executable...");
                return true;
            }
            return false;
        }

        // Xenon Memory Funcs */
        public void create_new_xenon_memory()
        {
            // Create new memory.
            Forms.Dialogs.XenonMemory memory = new Forms.Dialogs.XenonMemory();
            memory.ShowDialog();

            if(memory.output_file != null)
            {
                // Make new output file.
                FileStream fs = new FileStream(Application.StartupPath + "/kernal/" + memory.output_file + ".bin", FileMode.Create);
                fs.SetLength(memory.memory_size);

                // Fill out new file.
                __log("Filling out Xenon Memory file, this may take a while...");

                for(int i = 0; i < (memory.memory_size / 64); i++)
                {
                    byte[] blank = new byte[64]
                    {
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00
                    };

                    fs.Write(blank, (i * 64), 64);
                }

                fs.Flush();
                __log(string.Format("Finished creating Xenon memory file '{0}'...", memory.output_file));

                // Close.
                fs.Close();
                fs = null;

                in_memory = new Xbox360.Kernal.XenonMemory(Application.StartupPath + "/kernal/" + memory.output_file + ".bin");

                // Setup settings for reinit.
                Xenious.Properties.Settings.Default.MemoryFile = Application.StartupPath + "/kernal/" + memory.output_file + ".bin";

                switch (memory.memory_size)
                {
                    case 0x10000000:
                        // App Only.
                        // Do nothing.
                        break;
                    case 0x20000000:
                        // Kernal and Xecutable.

                        // Decrypt HyperVisor.
                        // Mount HyperVisor.
                        // Decrypt Kernal.
                        // Mount Kernal.
                        // Decrypt Dashboard.
                        // Mount Dashboard
                        break;
                }
            }
        }

        /* Form Funcs and Events */
        public MetaEditor()
        {
            InitializeComponent();
        }
        private void MainEditor_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            __log(string.Format("Xenious - The Xenon Executable Editor [{0}] - By [ Hect0r ] has started...", fvi.FileVersion));
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

            check_xextool_exists();
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
                if(in_type != InputType.None)
                {
                    close_file();
                }
                input_file = ofd.FileName;
                load_file(ofd.FileName);
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
        private void xGD3MediaIDToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hebox = new Forms.Dialogs.HexEditBox();
            hebox.set_max_len(16);
            hebox.set_value(in_xex.xgd3_media_id);
            hebox.ShowDialog();
            in_xex.xgd3_media_id = hebox.value;
        }
        private void lanKeyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hebox = new Forms.Dialogs.HexEditBox();
            hebox.set_max_len(16);
            hebox.set_value(in_xex.lan_key);
            hebox.ShowDialog();
            in_xex.lan_key = hebox.value;
        }
        private void deviceIDToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            hebox = new Forms.Dialogs.HexEditBox();
            hebox.set_max_len(16);
            hebox.set_value(in_xex.device_id);
            hebox.ShowDialog();
            in_xex.device_id = hebox.value;
        }
        private void boundingPathToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            tebox = new Forms.Dialogs.TextEditBox();
            tebox.set_value(in_xex.bound_path);
            tebox.set_max_len(in_xex.bound_path.Length);
            tebox.ShowDialog();
            in_xex.bound_path = tebox.value;
        }
        private void originalPENameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tebox = new Forms.Dialogs.TextEditBox();
            tebox.set_value(in_xex.orig_pe_name);
            tebox.set_max_len(in_xex.orig_pe_name.Length);
            tebox.ShowDialog();
            in_xex.orig_pe_name = tebox.value;
            treeView1.Nodes[0].Text = tebox.value;
        }
        private void imageBaseAddressToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);
            nebox.set_value(in_xex.img_base_addr);
            nebox.ShowDialog();
            in_xex.img_base_addr = nebox.value;
        }
        private void originalBaseAddressToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);
            nebox.set_value(in_xex.orig_base_addr);
            nebox.ShowDialog();
            in_xex.orig_base_addr = nebox.value;
        }
        private void entryPointToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);
            nebox.set_value(in_xex.exe_entry_point);
            nebox.ShowDialog();
            in_xex.exe_entry_point = nebox.value;
        }
        private void titleWorkspaceSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);
            nebox.set_value(in_xex.title_workspace_size);
            nebox.ShowDialog();
            in_xex.title_workspace_size = nebox.value;
        }
        private void stackSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);
            nebox.set_value(in_xex.default_stack_size);
            nebox.ShowDialog();
            in_xex.default_stack_size = nebox.value;
        }
        private void filesystemCacheSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);
            nebox.set_value(in_xex.default_fs_cache_size);
            nebox.ShowDialog();
            in_xex.default_fs_cache_size = nebox.value;
        }
        private void heapSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nebox = new Forms.Dialogs.NumericEditBox();
            nebox.set_max(UInt32.MaxValue);
            nebox.set_value(in_xex.default_heap_size);
            nebox.ShowDialog();
            in_xex.default_heap_size = nebox.value;
        }
        private void MainEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            close_xex();
            clear_cache();
        }
        private void treeView1_DoubleClick_1(object sender, EventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            if ((in_xex != null && node.Text == Path.GetFileName(in_xex.IO.file)) || 
                (in_xex != null && node.Text == in_xex.orig_pe_name) || 
                node.Tag == null)
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
        private void multidiscMediaIDsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<byte[]> listofmids = new List<byte[]>();
            Forms.Dialogs.MultidiscMediaIDs atmdids = new Forms.Dialogs.MultidiscMediaIDs(listofmids);
            bool has_header = false;
            foreach (XeOptHeader opt in in_xex.opt_headers)
            {
                if ((UInt32)opt.key == (UInt32)XeHeaderKeys.MULTIDISC_MEDIA_IDS)
                {
                    has_header = true;
                    atmdids = new Forms.Dialogs.MultidiscMediaIDs(in_xex.multidisc_media_ids);
                }
            }

            atmdids.Show();

            listofmids = atmdids.media_ids;

            if (listofmids.Count == 0)
            {
                return;
            }

            if (has_header == false)
            {
                XeOptHeader nopt = new XeOptHeader();
                nopt.key = XeHeaderKeys.MULTIDISC_MEDIA_IDS;
                in_xex.opt_headers.Add(nopt);
            }
            in_xex.multidisc_media_ids = listofmids;
        }
        private void callcapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.Dialogs.CapData cd = new Forms.Dialogs.CapData();
            cd.start = in_xex.callcap_start;
            cd.end = in_xex.callcap_end;
            cd.ShowDialog();

            in_xex.callcap_start = cd.start;
            in_xex.callcap_end = cd.end;
        }
    }
}
