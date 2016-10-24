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

        Xbox360.Kernal.Memory.XboxMemory in_mem;
        public List<Database.PEFileDatabase> pe_dbs;
        bool has_xextool = false;
        public string xex_text;

        public void __log(string msg)
        {
            richTextBox1.Text += string.Format("[ {0} ] - {1}\n", DateTime.Now.ToString(), msg);
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
        public void __cmd(string[] cmd)
        {
            switch (cmd[0])
            {
                case "goto":
                    switch (cmd[1])
                    {
                        case "pos":

                            break;
                    }
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

        /* XEX Loading Functions */
        public void show_loader(XenonExecutable in_xex)
        {
            Forms.Dialogs.XboxMemoryLoader loader = new Dialogs.XboxMemoryLoader(in_xex);
            loader.ShowDialog();

            // Check if we need kernal memory aswell.
            bool kernal = false;
            if (loader.kernal_imports.Count > 0)
            {
                foreach (Xecutable.KernalImport imp in loader.kernal_imports)
                {
                    if (imp.include == true)
                    {
                        kernal = true;
                    }
                }
            }
            if (kernal) // Just game memory needed.
            {
                in_mem = new Xbox360.Kernal.Memory.XboxMemory(0x20000000);
            }
            else { 
                in_mem = new Xbox360.Kernal.Memory.XboxMemory(0x10000000);
            }

            // Setup Xenon Memory.
            Xecutable.XenonMemory.setup_xenon_memory(in_xex, loader.local_imports, loader.kernal_imports, in_mem);
        }
        public void close_xex()
        {
            // Delete Memory.
            in_mem = null;
            if (File.Exists(Application.StartupPath + "cache/xbox_memory.bin"))
            {
                File.Delete(Application.StartupPath + "cache/xbox_memory.bin");
            }
            disable_gui();
            __log("Closing Xenon Memory...");
        }
        public void load_xex(string filename)
        {
            XenonExecutable in_xex;
            // Load XEX.
            try
            {
                in_xex = new XenonExecutable(filename);
                in_xex.read_header();
                in_xex.parse_certificate();
                in_xex.parse_sections();

                int x = in_xex.parse_optional_headers();

                if (x > 0)
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
                        #region Copy over Imports to cache dir.
                        foreach (XeImportLibary lib in in_xex.import_libs)
                        {
                            if(System.IO.File.Exists(System.IO.Path.GetDirectoryName(in_xex.IO.file) + "/" + lib.name))
                            {
                                __log("Copying over " + lib.name + " to local cache...");
                                System.IO.File.Copy(System.IO.Path.GetDirectoryName(in_xex.IO.file) + "/" + lib.name, Application.StartupPath + "/cache/" + lib.name);
                            }
                        }
                        #endregion

                        // Dump a zero'ed xex to cache.
                        #region Decrypt and Decompress XEX
                        if (Xecutable.xextool.xextool_to_raw_xextool(in_xex.IO.file, Application.StartupPath + "/cache/original.xex"))
                        {
                            // Parse all xex meta info.
                            try
                            {
                                close_xex();

                                // Open New xex.
                                in_xex = new XenonExecutable(Application.StartupPath + "/cache/original.xex");

                                // Parse xex.
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
                        #endregion
                    }
                }
            }
            catch
            {
                throw new Exception("Unable to parse Xenon Executable...");
            }

            // Now check we have a pe.
            if(!in_xex.load_pe())
            {
                throw new Exception("Error : Expecting a PE File...");
            }

            // Init Xbox memory, Show Loader.
            show_loader(in_xex);

            // Setup PEDatabase.
            pe_dbs = new List<Database.PEFileDatabase>();
            Database.PEFileDatabase pe_db = Xecutable.Database.generate_pe_file_template(in_xex);

            // Start to destroy.
            if(Xecutable.XEXLoader.load_xex_from_load_address(pe_db, in_mem) != true)
            {
                throw new Exception("Wtf :(");
            }

            pe_dbs.Add(pe_db);

            // Now this may take a while.

            // Now Init GUI.
            init_gui();

            // Build output xex.
            build_text();

            richTextBox2.Text = xex_text;

            // Enable GUI.
            enable_gui();
            return;
        }

        /* GUI Functions. */
        public void enable_gui()
        {
            treeView1.Enabled = true;
            treeView1.Update();
            saveToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = true;
            richTextBox2.Enabled = true;
        }
        public void disable_gui()
        {
            treeView1.Nodes.Clear();
            treeView1.Enabled = false;
            treeView1.Update();
            richTextBox2.Text = "";
            richTextBox2.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;
        }
        public void init_gui()
        {
            // First node is main executable.
            TreeNode main = new TreeNode();
            main.Text = (in_mem.MainApp.has_header_key(XeHeaderKeys.ORIGINAL_PE_NAME) ? in_mem.MainApp.orig_pe_name : Path.GetFileName(in_mem.MainApp.IO.file));

            // Add Sections and Functions.
            for(int i = 0; i < pe_dbs[0].sections.Count; i++)
            {
                TreeNode sn = new TreeNode();
                sn.Text = pe_dbs[0].sections[i].section_name;

                for(int x = 0; x < pe_dbs[0].sections[i].functions.Count; x++)
                {
                    TreeNode fnc = new TreeNode();
                    fnc.Text = pe_dbs[0].sections[i].functions[x].func_name;
                    fnc.Tag = "goto pos 0x" + pe_dbs[0].sections[i].functions[x].start_address.ToString("X8");
                    sn.Nodes.Add(fnc);
                }

                main.Nodes.Add(sn);
            }
            treeView1.Nodes.Add(main);

            // Now Add Imports.
            List<XenonExecutable> imports = in_mem.AppImports;

            if(imports != null && imports.Count > 0)
            {
                for (int i = 0; i < imports.Count; i++)
                {
                    TreeNode import = new TreeNode();
                    import.Text = (imports[i].has_header_key(XeHeaderKeys.ORIGINAL_PE_NAME) ? imports[i].orig_pe_name : Path.GetFileName(imports[i].IO.file));


                    treeView1.Nodes.Add(import);
                }
            }
            
        }
        public void build_text()
        {
            // Firstly the Main App.
            for (int i = 0; i < pe_dbs[0].sections.Count; i++)
            {
                xex_text += (string.Format(".section_name {0}{1}{2}\n\n", '"', pe_dbs[0].sections[i].section_name, '"'));
                
                for (int x = 0; x < pe_dbs[0].sections[i].functions.Count; x++)
                {
                    xex_text += (string.Format("{0}:{1}    .{2}: {3}\n",
                        pe_dbs[0].sections[i].section_name,
                        pe_dbs[0].sections[i].functions[x].start_address.ToString("X8"),
                        pe_dbs[0].sections[i].functions[x].func_name,
                        '{'
                    ));

                    // List Function code.
                    UInt32 start_Addr = pe_dbs[0].sections[i].functions[x].start_address;
                    foreach (UInt32 op in pe_dbs[0].sections[i].functions[x].op_codes)
                    {

                        XenonPowerPC.PowerPC.FuncID opcode = XenonPowerPC.PowerPC.Functions.find_func(op);

                        xex_text += string.Format("{0}:{1}    {2}\n", 
                            pe_dbs[0].sections[i].section_name,
                            start_Addr.ToString("X8"),
                            op.ToString("X8"));
                        start_Addr += 4;
                    }

                    xex_text += string.Format("{0}:{1}    {2}\n\n",
                            pe_dbs[0].sections[i].section_name,
                            (start_Addr - 4).ToString("X8"),
                            '}'
                    );
                }

                xex_text += ".end_section\n\n";
            }
        }

        public XEXDebugger()
        {
            InitializeComponent();
        }
        private void XEXDebugger_Load(object sender, EventArgs e)
        {
            __log("Xenon Executable Editor and Debugger has started...");

            // Check for xextool Support.
            if(Xecutable.xextool.check_xextool_exists())
            {
                has_xextool = true;
                __log("xextool Tool Support Enabled...");
            }
            else
            {
                MessageBox.Show("This app needs xextool, please install xextool...", "Error : ");
                this.Close();
            }

            clear_cache();
            disable_gui();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Xenon Executables (*.xex*)|*.xex|All Files (*.*)|*.";
            ofd.Title = "Please open a Xenon Executable...";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                close_xex(); // Close anyway.
                load_xex(ofd.FileName);
            }
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode.Tag != null)
            {
                string[] cmd = treeView1.SelectedNode.Tag.ToString().Split(' ');

                __cmd(cmd);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            close_xex();

        }
    }
}
