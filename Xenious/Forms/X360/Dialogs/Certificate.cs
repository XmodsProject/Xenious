using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Hect0rs.IO;
using Xbox360.XEX;

namespace Xenious.Forms.X360.Dialogs
{
    partial class Certificate : Form
    {
        public XeCertificate xec;
        public Certificate(XeCertificate cert)
        {
            InitializeComponent();
            xec = cert;
        }

        private void Certificate_Load(object sender, EventArgs e)
        {
            byte[] buf = BitConverter.GetBytes(xec.load_address);
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(buf);
            }

            textBox1.Text = BitConverter.ToString(buf).Replace("-", "");
            textBox2.Text = BitConverter.ToString(xec.dencrypt_key).Replace("-", "");
            textBox3.Text = BitConverter.ToString(xec.xgd2_media_id).Replace("-", "");
            label2.Text = string.Format("Header Hash : {0}", (xec.is_header_hash_valid ? "Valid" : "Invalid"));
            label3.Text = string.Format("Section Table Hash : {0}", (xec.is_section_hash_valid ? "Valid" : "Invalid"));
            label4.Text = string.Format("RSA Signature : {0}", (xec.is_rsa_sig_valid ? "Valid" : "Invalid"));
            // TODO Sort out retail or devkit.
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text.ToUpper();
            textBox1.SelectionStart = textBox1.Text.Length;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = IOF.StringToByteArrayFastest(textBox1.Text);
            if(BitConverter.IsLittleEndian)
            {
                Array.Reverse(data);
            }
            xec.load_address = BitConverter.ToUInt32(data, 0);
            xec.seed_key = IOF.StringToByteArrayFastest(textBox2.Text);
            xec.xgd2_media_id = IOF.StringToByteArrayFastest(textBox3.Text);
            this.Close();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
