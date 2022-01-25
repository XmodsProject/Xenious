using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xbox360;

namespace Xenious.Forms.X360.Panels.Editor
{
    public partial class XUIZEditor : UserControl
    {
        Xbox360.XboxUserInterfaceResource xuiz;
        Xbox360.XboxUnicodeInformationStorage in_xuis;
        Xenious.Forms.X360.Editor main;
        Image BackImage;
        
        public XUIZEditor(Xbox360.XboxUserInterfaceResource in_xuiz, Xenious.Forms.X360.Editor in_main)
        {
            InitializeComponent();
            xuiz = in_xuiz;
            main = in_main;
        }

        private void XUIZEditor_Load(object sender, EventArgs e)
        {
            main.__log("Loading Xbox User Interface Resource Package..");

            label1.Text = string.Format("File Size: {0}", xuiz.Header.FileEnd.ToString());
            label2.Text = string.Format("File System Entrys Header Size: {0}", xuiz.Header.FileSystemHeaderLen.ToString());
            label3.Text = String.Format("Is Valid: {0}", (xuiz.IsValid ? "Valid :)" : "Invalid:("));

            numericUpDown1.Value = xuiz.Header.Flags;
            numericUpDown1.Minimum = 0;
            numericUpDown1.Maximum = UInt32.MaxValue;

            numericUpDown2.Value = xuiz.Header.Unknown1;
            numericUpDown2.Minimum = 0;
            numericUpDown2.Maximum = UInt32.MaxValue;

            TreeNode node = null;

            for (int i = 0; i < xuiz.Entrys.Count; i++)
            {
                node = new TreeNode();

                node.Text = xuiz.Entrys[i].Name;
                node.ToolTipText = String.Format("SourceFile: {0}", xuiz.GetDebugSourceFile(xuiz.Entrys[i]).Replace("\\", "/"));
                switch (Path.GetExtension(xuiz.Entrys[i].Name.Replace("\0", "")))
                {
                    case ".xus":
                    case "xus":
                        node.Tag = "xuis " + i.ToString();
                        break;
                    case ".png":
                    case "png":
                        node.Tag = "png " + i.ToString();
                        break;
                    case ".jpg":
                    case "jpg":
                    case ".jpeg":
                    case "jpeg":
                        node.Tag = "jpeg " + i.ToString();
                        break;
                    default:
                        break;
                }

                treeView1.Nodes.Add(node);
                node = null;
            }
            main.__log(String.Format("{0}, {1}", splitContainer1.Panel2.Width, splitContainer1.Panel2.Height));
            main.__log("Done Loading XUIZ...");
        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode == null || treeView1.SelectedNode.Tag == null)
            {
                return;
            }
            if(BackImage != null)
            {
                BackImage = null;
            }

            if (this.in_xuis != null)
            {
                this.in_xuis.IO.close();
                this.in_xuis = null;
            }

            string[] buf = treeView1.SelectedNode.Tag.ToString().Split((char)0x20);

            byte[] data;
            int x;
            splitContainer1.Panel2.Controls.Clear();
            switch (buf[0])
            {
                case "png":
                    x = Convert.ToInt32(buf[1]);
                    data = this.xuiz.ExtractEntry(xuiz.Entrys[x]);

                    MemoryStream pngbuf = new MemoryStream(data);
                    BackImage = Image.FromStream(pngbuf);


                    splitContainer1.Panel2.BackgroundImage = BackImage;
                    splitContainer1.Panel2.BackColor = Color.Green;
                    splitContainer1.Panel2.BackgroundImageLayout = ImageLayout.Stretch;
                    splitContainer1.Panel2.Update();
                    break;
                case "jpeg":
                    x = Convert.ToInt32(buf[1]);
                    data = this.xuiz.ExtractEntry(xuiz.Entrys[x]);

                    MemoryStream jpegbuf = new MemoryStream(data);
                    BackImage = Image.FromStream(jpegbuf);


                    splitContainer1.Panel2.BackgroundImage = BackImage;
                    splitContainer1.Panel2.BackColor = Color.Green;
                    splitContainer1.Panel2.BackgroundImageLayout = ImageLayout.Stretch;
                    splitContainer1.Panel2.Update();
                    break;
                case "xuis":
                    x = Convert.ToInt32(buf[1]);

                    data = this.xuiz.ExtractEntry(xuiz.Entrys[x]);
                    this.in_xuis = new Xbox360.XboxUnicodeInformationStorage(data);
                    this.in_xuis.Parse();

                    TreeView StringView = new TreeView();
                    TreeNode node = null;

                    for (int i = 0; i < this.in_xuis.Entrys.Count; i++)
                    {
                        node = new TreeNode();
                        node.Text = this.in_xuis.Entrys[i].ToString();
                        StringView.Nodes.Add(node);

                        node = null;
                    }
                    StringView.Dock = DockStyle.Fill;
                    splitContainer1.Panel2.Controls.Add(StringView);
                    splitContainer1.Panel2.Update();
                    break;
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}
