using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Xenious.Forms
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

            // Get LZX Version, I used XeVersion for the version in the dll.
            UInt32 lzxv = Xbox360.Compression.LZX.Version();
            Xbox360.XEX.XeVersion ver_t = new Xbox360.XEX.XeVersion(lzxv);
            

            richTextBox3.Text = string.Format("Version :\n -> [ Xenious : {0} ]\n -> [ Xbox360 : {1} ]\n -> [ PowerPC : {2} ]\n -> [ LZX: {3} ]", fvi.FileVersion.ToString(), ver.ToString(), ver2.ToString(), ver_t.get_string);

            // Clean Up.
            ver = null;
            ver2 = null;
            ver_t = null;
            assembly = null;
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
