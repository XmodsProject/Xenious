using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Xenious.Forms.Dialogs
{
    partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        private void About_Load(object sender, EventArgs e)
        {
            // Get Version.
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);

            // Get Xbox 360 Version
            assembly = Assembly.LoadFrom("Xbox360.dll");
            Version ver = assembly.GetName().Version;

            // Get XenonPowerPC version.
            assembly = Assembly.LoadFrom("XenonPowerPC.dll");
            Version ver2 = assembly.GetName().Version;

            label3.Text = string.Format("Version : [Xenious : {0}] - [Xbox360 : {1}] - [PowerPC : {2}]", fvi.FileVersion.ToString(), ver.ToString(), ver2.ToString());
            return;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://hect0r.com/donate");
            this.Close();
        }
    }
}
