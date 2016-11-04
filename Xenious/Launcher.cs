using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xenious
{
    partial class Launcher : Form
    {
        public Launcher()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Forms.Dialogs.About about = new Forms.Dialogs.About();
            about.ShowDialog();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if(listView1.SelectedItems != null)
            {
                switch(listView1.SelectedItems[0].Text)
                {
                    case "Meta Editor":
                        Forms.MetaEditor medit = new Forms.MetaEditor();
                        medit.ShowDialog();
                        break;
                    case "Executable Editor":
                        Forms.ExeEditor xedtr = new Forms.ExeEditor();
                        xedtr.ShowDialog();
                        break;
                    case "Executable Debugger":
                        Forms.ExeDebugger xebdgr = new Forms.ExeDebugger();
                        xebdgr.ShowDialog();
                        break;
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            // Add Meta Editor.
            ListViewItem medit = new ListViewItem();
            medit.Text = "Meta Editor";
            medit.SubItems.Add(Forms.MetaEditor.tool_version);
            listView1.Items.Add(medit);

            // Add XEXDebugger
            ListViewItem edtr = new ListViewItem();
            edtr.Text = "Executable Editor";
            edtr.SubItems.Add(Forms.ExeEditor.tool_version);
            listView1.Items.Add(edtr);

            ListViewItem dbgr = new ListViewItem();
            dbgr.Text = "Executable Debugger";
            dbgr.SubItems.Add(Forms.ExeDebugger.tool_version);
            listView1.Items.Add(dbgr);

            listView1.Update();

            // Get Version.
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);

            // Get Xbox 360 Version
            assembly = Assembly.LoadFrom("Xbox360.dll");
            Version ver = assembly.GetName().Version;

            // Get XenonPowerPC version.
            assembly = Assembly.LoadFrom("XenonPowerPC.dll");
            Version ver2 = assembly.GetName().Version;

            label3.Text = string.Format("[Xenious : {0}] - [Xbox360 : {1}] - [PowerPC : {2}]", fvi.FileVersion.ToString(), ver.ToString(), ver2.ToString());
        }

        private void label4_Click(object sender, EventArgs e)
        {
            Forms.Dialogs.SettingsDialog settings = new Forms.Dialogs.SettingsDialog();

            settings.ShowDialog();
        }
    }
}
