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
    public partial class Sections : UserControl
    {
        public List<XeSection> sections;
        public Sections(List<XeSection> secs)
        {
            InitializeComponent();
            sections = secs;
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Sections_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            foreach(XeSection sec in sections)
            {
                ListViewItem item = new ListViewItem();
                item.Text = sec.page_size.ToString();
                item.SubItems.Add(sec.page_count.ToString());
                item.SubItems.Add(BitConverter.ToString(sec.digest).Replace("-", ""));
                item.SubItems.Add(sec.type.ToString());
                item.SubItems.Add("Unknown"); // Todo
                listView1.Items.Add(item);
            }
            listView1.Update();
        }
    }
}
