using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Xbox360;
using Xbox360.XEX;

namespace Xenious.Forms.Dialogs
{
    partial class ExecutionInfo : Form
    {
        XenonExecutable xex;
        public ExecutionInfo(XenonExecutable in_xex)
        {
            InitializeComponent();
            xex = in_xex;
        }

        private void ExecutionInfo_Load(object sender, EventArgs e)
        {
            byte[] data = BitConverter.GetBytes(xex.xeinfo.media_id);
            if (BitConverter.IsLittleEndian) { Array.Reverse(data); }
            textBox4.Text = BitConverter.ToString(data).Replace("-", "");
            data = BitConverter.GetBytes(xex.xeinfo.title_id);
            if (BitConverter.IsLittleEndian) { Array.Reverse(data); }
            textBox5.Text = BitConverter.ToString(data).Replace("-", "");
            data = BitConverter.GetBytes(xex.xeinfo.savegame_id);
            if (BitConverter.IsLittleEndian) { Array.Reverse(data); }
            textBox6.Text = BitConverter.ToString(data).Replace("-", "");
            numericUpDown1.Value = xex.xeinfo.platform;
            numericUpDown2.Value = xex.xeinfo.executable_table;
            numericUpDown3.Value = xex.xeinfo.disc_number;
            numericUpDown4.Value = xex.xeinfo.disc_count;

            // Version
            XeVersion ver = new XeVersion();
            ver.version = xex.xeinfo.version;
            textBox7.Text = ver.major.ToString();
            textBox8.Text = ver.minor.ToString();
            textBox9.Text = ver.build.ToString();
            textBox10.Text = ver.qfe.ToString();

            // Base Version
            ver = new XeVersion();
            ver.version = xex.xeinfo.base_version;
            textBox14.Text = ver.major.ToString();
            textBox13.Text = ver.minor.ToString();
            textBox12.Text = ver.build.ToString();
            textBox11.Text = ver.qfe.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = IO.IOFuncs.StringToByteArrayFastest(textBox4.Text);
            if (BitConverter.IsLittleEndian) { Array.Reverse(data); }
            xex.xeinfo.media_id = BitConverter.ToUInt32(data, 0);

            data = IO.IOFuncs.StringToByteArrayFastest(textBox5.Text);
            if (BitConverter.IsLittleEndian) { Array.Reverse(data); }
            xex.xeinfo.title_id = BitConverter.ToUInt32(data, 0);

            data = IO.IOFuncs.StringToByteArrayFastest(textBox6.Text);
            if (BitConverter.IsLittleEndian) { Array.Reverse(data); }
            xex.xeinfo.savegame_id = BitConverter.ToUInt32(data, 0);

            xex.xeinfo.executable_table = (byte)numericUpDown2.Value;
            xex.xeinfo.platform = (byte)numericUpDown1.Value;
            xex.xeinfo.disc_number = (byte)numericUpDown3.Value;
            xex.xeinfo.disc_count = (byte)numericUpDown4.Value;

            // Change Version.
            XeVersion version = new XeVersion();
            version.major = byte.Parse(textBox7.Text);
            version.minor = byte.Parse(textBox8.Text);
            version.build = UInt16.Parse(textBox9.Text);
            version.qfe = byte.Parse(textBox10.Text);

            xex.xeinfo.version = version.version;
            version = null;

            XeVersion base_version = new XeVersion();
            base_version.major = byte.Parse(textBox14.Text);
            base_version.minor = byte.Parse(textBox13.Text);
            base_version.build = UInt16.Parse(textBox12.Text);
            base_version.qfe = byte.Parse(textBox11.Text);

            xex.xeinfo.base_version = base_version.version;
            base_version = null;

            this.Close();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if(textBox7.Text != "" && byte.Parse(textBox7.Text) > 15)
            {
                MessageBox.Show("0-15", "Invalid Setting");
                textBox7.Text = "0";
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox8.Text != "" && byte.Parse(textBox8.Text) > 15)
            {
                MessageBox.Show("0-15", "Invalid Setting");
                textBox8.Text = "0";
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (textBox13.Text != "" && byte.Parse(textBox13.Text) > 15)
            {
                MessageBox.Show("0-15", "Invalid Setting");
                textBox13.Text = "0";
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            if (textBox14.Text != "" && byte.Parse(textBox14.Text) > 15)
            {
                MessageBox.Show("0-15", "Invalid Setting");
                textBox14.Text = "0";
            }
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            if (textBox9.Text != "" && int.Parse(textBox9.Text) > 65535)
            {
                MessageBox.Show("0 - 65535", "Invalid Setting");
                textBox9.Text = "0";
            }
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            if (textBox10.Text != "" && int.Parse(textBox10.Text) > 255)
            {
                MessageBox.Show("0-255", "Invalid Setting");
                textBox10.Text = "0";
            }
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text != "" && int.Parse(textBox12.Text) > 65535)
            {
                MessageBox.Show("0 - 65535", "Invalid Setting");
                textBox12.Text = "0";
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text != "" && int.Parse(textBox11.Text) > 255)
            {
                MessageBox.Show("0-255", "Invalid Setting");
                textBox11.Text = "0";
            }
        }
    }
}
