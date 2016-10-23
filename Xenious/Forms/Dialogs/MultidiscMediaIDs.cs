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
    partial class MultidiscMediaIDs : Form
    {
        public List<byte[]> media_ids;
        int selected_index = -1;
        public MultidiscMediaIDs(List<byte[]> multidisc_media_ids)
        {
            InitializeComponent();
            media_ids = multidisc_media_ids;
        }

        private void AlternativeMediaIDs_Load(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            for(int i = 0; i < media_ids.Count; i++)
            {
                TreeNode node = new TreeNode();
                node.Text = BitConverter.ToString(media_ids[i]).Replace("-", "");
                node.Tag = i.ToString();
                treeView1.Nodes.Add(node);
            }
            treeView1.Update();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            media_ids[selected_index] = IO.IOFuncs.StringToByteArrayFastest(textBox1.Text);
            treeView1.SelectedNode.Text = BitConverter.ToString(media_ids[selected_index]).Replace("-", "");
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selected_index = int.Parse(treeView1.SelectedNode.Tag.ToString());
            textBox1.Text = BitConverter.ToString(media_ids[selected_index]).Replace("-", "");
        }
    }
}
