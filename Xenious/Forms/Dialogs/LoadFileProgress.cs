using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xenious.Forms.Dialogs
{
    partial class LoadFileProgress : Form
    {
        public LoadFileProgress()
        {
            InitializeComponent();
        }

        public void set_progress(int value)
        {
            progressBar1.Value = value;
        }
        public void set_status(string text)
        {
            textBox1.Text = text;
        }

        private void LoadFileProgress_Load(object sender, EventArgs e)
        {

        }
    }
}
