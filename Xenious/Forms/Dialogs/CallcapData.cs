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
    partial class CallcapData : Form
    {
        public UInt32 start = 0;
        public UInt32 end = 0;

        public CallcapData()
        {
            InitializeComponent();
        }

        private void CallcapData_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = start;
            numericUpDown2.Value = end;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            start = (UInt32)numericUpDown1.Value;
            end = (UInt32)numericUpDown2.Value;
            this.Close();
        }
    }
}
