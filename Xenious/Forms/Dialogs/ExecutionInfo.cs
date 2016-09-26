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
            ver.version = xex.xeinfo.version;
            textBox11.Text = ver.major.ToString();
            textBox12.Text = ver.minor.ToString();
            textBox13.Text = ver.build.ToString();
            textBox14.Text = ver.qfe.ToString();
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
            this.Close();
        }
    }
}
