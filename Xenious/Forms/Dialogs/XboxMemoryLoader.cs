using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xbox360;

namespace Xenious.Forms.Dialogs
{
    partial class XboxMemoryLoader : Form
    {
        XenonExecutable xex;
        public bool exit = false;
        public List<Xecutable.LocalAppImport> local_imports;
        public List<Xecutable.KernalImport> kernal_imports;

        public XboxMemoryLoader(XenonExecutable in_xex)
        {
            InitializeComponent();
            xex = in_xex;
        }

        private void XboxMemoryLoader_Load(object sender, EventArgs e)
        {
            textBox1.Text = xex.IO.file;
            local_imports = new List<Xecutable.LocalAppImport>();
            kernal_imports = new List<Xecutable.KernalImport>();
            // Check for libary imports in local dir.
            foreach (Xbox360.XEX.XeImportLibary lib in xex.import_libs)
            {
                if (lib.name != "xam.xex" &&
                   lib.name != "xboxkrnl.exe" && 
                   lib.name != "xbdm.xex")
                {
                    string dir = Path.GetDirectoryName(xex.IO.file);

                    if (File.Exists(dir + "/" + lib.name))
                    {
                        checkedListBox1.Items.Add(lib.name);
                        local_imports.Add(new Xecutable.LocalAppImport()
                        {
                            filename = dir + "/" + lib.name,
                            include = false,
                        });
                    }
                }
            }

            // Now Kernal Libary imports.
            foreach (Xbox360.XEX.XeImportLibary lib in xex.import_libs)
            {
                if (lib.name == "xam.xex" ||
                   lib.name == "xboxkrnl.exe" ||
                   lib.name == "xbdm.xex")
                {
                    if(Xecutable.XEXLoader.import_libary_exists(lib.name))
                    {
                        checkedListBox2.Items.Add(lib.name);
                        kernal_imports.Add(new Xecutable.KernalImport()
                        {
                            filename = Application.StartupPath + "/kernel/imports/" + lib.name,
                            include = false
                        });
                    }
                }
            }
            checkedListBox1.Update();
            checkedListBox2.Update();

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Get Imports.
            int num = 0;
            foreach(Xecutable.LocalAppImport imp in local_imports)
            {
                if(checkedListBox1.GetItemChecked(num))
                {
                    local_imports[num].include = true;
                }
                else
                {
                    local_imports[num].include = false;
                }
                num++;
            }

            num = 0;
            // Get Kernal Imports.
            foreach(Xecutable.KernalImport imp in kernal_imports)
            {
                if(checkedListBox2.GetItemChecked(num))
                {
                    kernal_imports[num].include = true;
                }
                else
                {
                    kernal_imports[num].include = false;
                }
                num++;
            }
            this.Close();
        }

        private void XboxMemoryLoader_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.exit = true;
        }
    }
}
