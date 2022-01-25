using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xenious.Forms.Tools
{
    partial class NumericEditBox : Form
    {
        public uint value;

        public void set_value(uint val)
        {
            numericUpDown1.Value = val;
            value = val;
        }
        public void set_max(uint val)
        {
            numericUpDown1.Maximum = val;
        }
        public NumericEditBox()
        {
            InitializeComponent();
        }

        private void NumericExitBox_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            value = (uint)numericUpDown1.Value;
            this.Close();
        }
    }
}
