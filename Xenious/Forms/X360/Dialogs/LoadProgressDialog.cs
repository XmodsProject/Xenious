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
    partial class LoadProgressDialog : Form
    {
        public LoadProgressDialog()
        {
            InitializeComponent();

            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.BackColor = Color.FromName("Control");
        }

        public void set_status(string msg)
        {
            textBox1.Text = msg;
        }
        public void set_progress(int value)
        {
            progressBar1.Value = value;
        }

        private void LoadProgressDialog_Load(object sender, EventArgs e)
        {

        }
    }
}
