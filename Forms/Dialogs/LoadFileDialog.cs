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
    partial class LoadFileDialog : Form
    {
        public bool load_file = false;
        public string input_file;
        public string output_file = "";

        public Xbox360.XEX.XeCompressionType output_comp_type;
        public Xbox360.XEX.XeEncryptionType output_enc_type;

        bool has_xextool = false;
        public LoadFileDialog(bool has_xextool_support)
        {
            InitializeComponent();
            has_xextool = has_xextool_support;
        }

        private void LoadFileDialog_Load(object sender, EventArgs e)
        {
            if(has_xextool == false)
            {
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
            }
            else
            {
                comboBox1.SelectedIndex = 1;
                comboBox2.SelectedIndex = 0;
            }
            comboBox1.Update();
            comboBox2.Update();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open a Xenon Executable...";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                input_file = ofd.FileName;
                textBox1.Text = ofd.FileName;
                button3.Enabled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedIndex)
            {
                case 0:
                    output_enc_type = Xbox360.XEX.XeEncryptionType.Encrypted;
                    break;
                case 1:
                    output_enc_type = Xbox360.XEX.XeEncryptionType.NotEncrypted;
                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox2.SelectedIndex)
            {
                case 0:
                    output_comp_type = Xbox360.XEX.XeCompressionType.Zeroed;
                    break;
                case 1:
                    output_comp_type = Xbox360.XEX.XeCompressionType.Raw;
                    break;
                case 2:
                    output_comp_type = Xbox360.XEX.XeCompressionType.Compressed;
                    break;
                case 3:
                    output_comp_type = Xbox360.XEX.XeCompressionType.DeltaCompressed;
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save Xenon Executable As...";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = sfd.FileName;
                output_file = sfd.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            load_file = true;
            this.Close();
        }
    }
}
