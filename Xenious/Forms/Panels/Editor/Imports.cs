using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xbox360.XEX;

namespace Xenious.Forms.Panels.Editor
{
    public partial class Imports : UserControl
    {
        XeImportLibary libinf;
        public Imports(XeImportLibary lib)
        {
            InitializeComponent();
            libinf = lib;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Imports_Load(object sender, EventArgs e)
        {
            label1.Text = "Kernal : " + libinf.name;
            label2.Text = "ID : 0x" + libinf.import_id.ToString("X8");
            XeVersion ver = new XeVersion();
            ver.version = libinf.version;
            label3.Text = "Version : " + ver.get_string;
            ver.version = libinf.min_version;
            label4.Text = "Min Version : " + ver.get_string;
            label5.Text = "Records : " + libinf.record_count.ToString();

            // Add Records.
            listView1.Items.Clear();
            int num = 0;
            foreach (uint import in libinf.records)
            {
                ListViewItem item = new ListViewItem();
                item.Text = num.ToString();
                item.SubItems.Add("0x" + import.ToString("X8"));
                listView1.Items.Add(item);
                num++;
            }
            listView1.Update();
        }
    }
}
