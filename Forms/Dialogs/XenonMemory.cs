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
    partial class XenonMemory : Form
    {
        public string output_file;
        public byte[] cpu_key;
        public byte[] firstbl_key;
        public string in_nand;
        public int memory_size;

        public XenonMemory()
        {
            InitializeComponent();
        }

        private void XenonMemory_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Please select a Xbox 360 Nand Image...";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                in_nand = ofd.FileName;
                textBox2.Text = ofd.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length < 1) { MessageBox.Show("Please set a filename for the Xenon Memory output file...", "Error : "); return; }
            if (comboBox1.SelectedIndex == 1)
            {
                cpu_key = IO.IOFuncs.StringToByteArrayFastest(textBox3.Text);
                firstbl_key = IO.IOFuncs.StringToByteArrayFastest(textBox4.Text);
            }
            output_file = textBox1.Text;
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    label1.Text = "Allocated Memory : 0x10000000";
                    memory_size = 0x10000000;
                    groupBox3.Enabled = false;
                    break;
                case 1:
                    label1.Text = "Allocated Memory : 0x20000000";
                    memory_size = 0x20000000;
                    groupBox3.Enabled = true;
                    break;
            }
        }
    }
}
