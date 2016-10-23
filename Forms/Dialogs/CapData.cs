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
    partial class CapData : Form
    {
        public UInt32 start = 0;
        public UInt32 end = 0;
        public CapData()
        {
            InitializeComponent();
        }

        private void CapData_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = start;
            numericUpDown2.Value = end;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            start = (uint)numericUpDown1.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            end = (uint)numericUpDown2.Value;
        }
    }
}
