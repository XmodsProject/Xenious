using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xbox360;
using Xbox360.XEX;

namespace Xenious.Forms.Panels.Editor
{
    public partial class Resources : UserControl
    {
        XenonExecutable xex;
        MainEditor main;
        public Resources(XenonExecutable in_xex, MainEditor in_main)
        {
            InitializeComponent();
            xex = in_xex;
            main = in_main;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Resources_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            for (int i = 0; i < xex.resources.Count; i++)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = i;
                item.Text = xex.resources[i].name;
                item.SubItems.Add(xex.resources[i].size.ToString().Replace("\0", ""));
                item.SubItems.Add("0x" + xex.resources[i].address.ToString("X8"));
                item.SubItems.Add("0x" + (xex.pe_data_offset + (xex.resources[i].address - xex.cert.load_address)).ToString("X8"));
                listView1.Items.Add(item);
            }
            listView1.Update();
            main.__log("Loaded resources...");
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                if (listView1.SelectedItems.Count > 1)
                {
                    contextMenuStrip3.Show(e.X, e.Y);
                }
                else if (listView1.SelectedItems.Count == 1)
                {
                    contextMenuStrip1.Show(e.X, e.Y);

                }
                else
                {
                    contextMenuStrip2.Show(e.X, e.Y);
                }
            }
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListViewItem item = listView1.SelectedItems[0];
            int resource = (int)(item.Tag);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = item.Text;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                xex.extract_resource(sfd.FileName, xex.resources[resource]);
                main.__log(string.Format("Extracted '{0}' to '{1}'...", item.Text, sfd.FileName));
            }
        }

        private void extractSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                foreach (ListViewItem item in listView1.Items)
                {
                    foreach (XeResourceInfo res in xex.resources)
                    {
                        if (item.Text == res.name.Replace("\0", ""))
                        {
                            xex.extract_resource(fbd.SelectedPath + "/" + item.Text, res);
                        }
                    }
                }

                MessageBox.Show("Your resources have been extracted", "All Done...");
            }
        }

        private void contextMenuStrip3_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
