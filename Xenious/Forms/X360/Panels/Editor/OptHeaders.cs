using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Xbox360.XEX;
using Xbox360;
using Xenious.Forms.Dialogs;

namespace Xenious.Forms.X360.Panels.Editor
{
    public partial class OptHeaders : UserControl
    {
        XenonExecutable xex;
        Forms.X360.Editor main;

        public OptHeaders(XenonExecutable in_xex, Forms.X360.Editor in_main)
        {
            InitializeComponent();
            xex = in_xex;
            main = in_main;
        }

        private void OptHeaders_Load(object sender, EventArgs e)
        {
            ListViewItem item;
            for (int i = 0; i < xex.opt_headers.Count; i++)
            {
                switch ((XeHeaderKeys)xex.opt_headers[i].key)
                {
                    case XeHeaderKeys.XGD3_MEDIA_KEY:
                        item = new ListViewItem();
                        item.Text = "XGD3 Media ID";
                        item.SubItems.Add(BitConverter.ToString(xex.xgd3_media_id).Replace("-", ""));
                        item.Tag = "edit xgd3_media_id";
                        listView1.Items.Add(item);
                        break;
                    case XeHeaderKeys.BOUNDING_PATH:
                        item = new ListViewItem();
                        item.Text = "Bound Path";
                        item.SubItems.Add(xex.bound_path);
                        item.Tag = "edit bound_path";
                        listView1.Items.Add(item);
                        break;
                    case XeHeaderKeys.ENTRY_POINT:
                        item = new ListViewItem();
                        item.Text = "Entry Point";
                        item.SubItems.Add("0x" + xex.exe_entry_point.ToString("X8"));
                        item.Tag = "edit entry_point";
                        listView1.Items.Add(item);
                        break;
                    case XeHeaderKeys.DEVICE_ID:
                        item = new ListViewItem();
                        item.Text = "Device ID";
                        item.SubItems.Add(BitConverter.ToString(xex.device_id).Replace("-", ""));
                        item.Tag = "edit device_id";
                        listView1.Items.Add(item);
                        break;
                    case XeHeaderKeys.ORIGINAL_BASE_ADDRESS:
                        item = new ListViewItem();
                        item.Text = "Original Base Address";
                        item.SubItems.Add("0x" + xex.orig_base_addr.ToString("X8"));
                        item.Tag = "edit orig_base_addr";
                        listView1.Items.Add(item);
                        break;
                    case XeHeaderKeys.IMAGE_BASE_ADDRESS:
                        item = new ListViewItem();
                        item.Text = "Image Base Address";
                        item.SubItems.Add("0x" + xex.img_base_addr.ToString("X8"));
                        item.Tag = "edit image_base_addr";
                        listView1.Items.Add(item);
                        break;
                    case XeHeaderKeys.LAN_KEY:
                        item = new ListViewItem();
                        item.Text = "Lan Key";
                        item.SubItems.Add(BitConverter.ToString(xex.lan_key).Replace("-", ""));
                        item.Tag = "edit lan_key";
                        listView1.Items.Add(item);
                        break;
                    case XeHeaderKeys.UNKNOWN1:
                        item = new ListViewItem();
                        item.Text = "Unknown Opt (Key: 00030100)";
                        item.SubItems.Add(xex.Unknown_OPT_Data.ToString());
                        item.Tag = "edit unknown1";
                        listView1.Items.Add(item);
                        break;
                    case XeHeaderKeys.DEFAULT_STACK_SIZE:
                        item = new ListViewItem();
                        item.Text = "Default Stack Size";
                        item.SubItems.Add(xex.default_stack_size.ToString());
                        item.Tag = "edit default_stack_size";
                        listView1.Items.Add(item);
                        break;
                    case XeHeaderKeys.TITLE_WORKSPACE_SIZE:
                        item = new ListViewItem();
                        item.Text = "Title Workspace Size";
                        item.SubItems.Add(xex.title_workspace_size.ToString());
                        item.Tag = "edit title_workspace_size";
                        listView1.Items.Add(item);
                        break;
                    case XeHeaderKeys.DEFAULT_FILESYSTEM_CACHE_SIZE:
                        item = new ListViewItem();
                        item.Text = "Default Filesystem Cache Size";
                        item.SubItems.Add(xex.default_fs_cache_size.ToString());
                        item.Tag = "edit default_fs_cache_size";
                        listView1.Items.Add(item);
                        break;
                    case XeHeaderKeys.DEFAULT_HEAP_SIZE:
                        item = new ListViewItem();
                        item.Text = "Default Heap Size";
                        item.SubItems.Add(xex.default_fs_cache_size.ToString());
                        item.Tag = "edit default_heap_size";
                        listView1.Items.Add(item);
                        break;

                }
            }
            listView1.Update();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = listView1.SelectedItems[0];

            if(item.Tag.ToString() != "")
            {
                string[] args = item.Tag.ToString().Split(' ');

                if(args[0] == "edit")
                {
                    switch(args[1])
                    {
                        case "xgd3_media_id":
                            break;
                    }
                }
            }
        }
    }
}
