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
    partial class TextEditBox : Form
    {
        public string value;

        public void set_max_len(int max)
        {
            textBox1.MaxLength = max;
        }
        public void set_value(string val)
        {
            textBox1.Text = val;
            value = val;
        }
        public TextEditBox()
        {
            InitializeComponent();
        }

        private void TextEditBox_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            value = textBox1.Text;
            this.Close();
        }
    }
}
