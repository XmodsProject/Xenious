using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xenious
{
    partial class Launcher : Form
    {
        public Launcher()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if(listView1.SelectedItems != null)
            {

                switch(listView1.SelectedItems[0].Text)
                {
                    case "Meta Editor":
                        MetaEditor editor = new MetaEditor();
                        editor.ShowDialog();
                        break;
                }
            }
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            listView1.Items.Clear();

            ListViewItem cxi = new ListViewItem();
            cxi.Text = "Create XEX";
            cxi.SubItems.Add("0.0.155.0");
            listView1.Items.Add(cxi);
            cxi = null;

            ListViewItem mei = new ListViewItem();
            mei.Text = "Meta Editor";
            mei.SubItems.Add("0.0.2000.0");
            listView1.Items.Add(mei);
            mei = null;

            ListViewItem dbgi = new ListViewItem();
            dbgi.Text = "XEX Editor & Debugger";
            dbgi.SubItems.Add("0.0.340.0");
            listView1.Items.Add(dbgi);
            dbgi = null;

            ListViewItem xdbgi = new ListViewItem();
            xdbgi.Text = "XDK Editor & Debugger";
            xdbgi.SubItems.Add("0.0.340.0");
            listView1.Items.Add(xdbgi);
            xdbgi = null;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
