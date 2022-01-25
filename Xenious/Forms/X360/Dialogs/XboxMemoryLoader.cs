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
        public XboxMemoryLoader(XenonExecutable in_xex)
        {
            InitializeComponent();
            xex = in_xex;
        }

        private void XboxMemoryLoader_Load(object sender, EventArgs e)
        {


        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
  
        }

        private void XboxMemoryLoader_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.exit = true;
        }
    }
}
