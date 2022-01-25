using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Hect0rs.IO;

namespace Xenious.Forms.X360.Dialogs
{
    partial class AlternativeTitleIDs : Form
    {
        public List<byte[]> title_ids;
        int selected_titleid;
        public AlternativeTitleIDs(List<byte[]> tids)
        {
            InitializeComponent();
            title_ids = tids;
        }

        private void AlternativeTitleIDs_Load(object sender, EventArgs e)
        {
            if(title_ids.Count > 0)
            {
                treeView1.Nodes.Clear();
                foreach(byte[] titleid in title_ids)
                {
                    treeView1.Nodes.Add(BitConverter.ToString(titleid).Replace("-", ""));
                }
                treeView1.Update();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            if(treeView1.SelectedNode != null & e.Button == MouseButtons.Right)
            {
                //contextMenuStrip1.Show(e.X, e.Y);
            }
            else if(treeView1.SelectedNode == null & e.Button == MouseButtons.Right)
            {
                //contextMenuStrip2.Show(e.X, e.Y);
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < title_ids.Count; i++)
            {
                if(BitConverter.ToString(title_ids[i]).Replace("-", "") == treeView1.SelectedNode.Text)
                {
                    title_ids.Remove(title_ids[i]);
                }
            }

            treeView1.Nodes.Remove(treeView1.SelectedNode);
            textBox1.Text = "";
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(TreeNode node in treeView1.Nodes)
            {
                if(node.Text == "00000000")
                {
                    treeView1.SelectedNode = node;
                    treeView1.Update();
                    return;
                }
            }
            title_ids.Add(new byte[4] { 0x00, 0x00, 0x00, 0x00 });
            treeView1.Nodes.Add("00000000");
            treeView1.Update();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            textBox1.Text = treeView1.SelectedNode.Text;
            selected_titleid = treeView1.SelectedNode.Index;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(textBox1.Text.Length < 8)
            {
                MessageBox.Show("The title id must be 8 characters long...");
                return;
            }
            byte[] data = IOF.StringToByteArrayFastest(textBox1.Text);
            title_ids[selected_titleid] = data;
            treeView1.SelectedNode.Text = textBox1.Text.ToUpper();
        }
    }
}
