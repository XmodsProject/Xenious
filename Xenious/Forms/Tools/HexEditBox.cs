using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Hect0rs.IO;

namespace Xenious.Forms.Tools
{
    partial class HexEditBox : Form
    {
        public byte[] value;
        public HexEditBox()
        {
            InitializeComponent();
        }
        public void set_max_len(int max_len)
        {
            richTextBox1.MaxLength = max_len * 2;
        }
        public void set_value(byte[] val)
        {
            richTextBox1.Text = BitConverter.ToString(val).Replace("-","");
            value = val;
        }
        private void HexEditBox_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            value = IOF.StringToByteArrayFastest(richTextBox1.Text);
            this.Close();
        }
    }
}
