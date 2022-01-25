using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xbox360.XEX;

namespace Xenious.Forms.X360.Panels.Editor
{
    public partial class StaticLibs : UserControl
    {
        List<XeStaticLib> lib;
        public StaticLibs(List<XeStaticLib> libs)
        {
            InitializeComponent();
            lib = libs;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void StaticLibs_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            foreach (XeStaticLib l in lib)
            {
                ListViewItem item = new ListViewItem();
                item.Text = l.name;
                item.SubItems.Add(l.major.ToString());
                item.SubItems.Add(l.minor.ToString());
                item.SubItems.Add(l.build.ToString());
                item.SubItems.Add(l.qfe.ToString());
                item.SubItems.Add(l.approval.ToString());
                listView1.Items.Add(item);
            }
            listView1.Update();
        }
    }
}
