using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Xbox360;
using Xbox360.XEX;

namespace Xenious.Forms.Dialogs
{
    partial class ImageFlags : Form
    {
        public UInt32 image_flags;

        public void set_flag(UInt32 flags)
        {
            image_flags = flags;
        }
        public UInt32 get_flags() { return image_flags; }
        public ImageFlags()
        {
            InitializeComponent();
        }

        private void ImageFlags_Load(object sender, EventArgs e)
        {
            init_image_data();
        }

        private void init_image_data()
        {
            #region Image Flags
            if ((image_flags & (uint)XeImageFlags.MANUFACTURING_UTILITY) == (uint)XeImageFlags.MANUFACTURING_UTILITY) { checkedListBox1.SetItemChecked(0, true); }
            if ((image_flags & (uint)XeImageFlags.MANUFACTURING_SUPPORT_TOOLS) == (uint)XeImageFlags.MANUFACTURING_SUPPORT_TOOLS) { checkedListBox1.SetItemChecked(1, true); }
            if ((image_flags & (uint)XeImageFlags.XGD2_MEDIA_ONLY) == (uint)XeImageFlags.XGD2_MEDIA_ONLY) { checkedListBox1.SetItemChecked(2, true); }
            if ((image_flags & (uint)XeImageFlags.CARDEA_KEY) == (uint)XeImageFlags.CARDEA_KEY) { checkedListBox1.SetItemChecked(3, true); }
            if ((image_flags & (uint)XeImageFlags.XEIKA_KEY) == (uint)XeImageFlags.XEIKA_KEY) { checkedListBox1.SetItemChecked(4, true); }
            if ((image_flags & (uint)XeImageFlags.USERMODE_TITLE) == (uint)XeImageFlags.USERMODE_TITLE) { checkedListBox1.SetItemChecked(5, true); }
            if ((image_flags & (uint)XeImageFlags.USERMODE_SYSTEM) == (uint)XeImageFlags.USERMODE_SYSTEM) { checkedListBox1.SetItemChecked(6, true); }
            if ((image_flags & (uint)XeImageFlags.ORANGE0) == (uint)XeImageFlags.ORANGE0) { checkedListBox1.SetItemChecked(7, true); }
            if ((image_flags & (uint)XeImageFlags.ORANGE1) == (uint)XeImageFlags.ORANGE1) { checkedListBox1.SetItemChecked(8, true); }
            if ((image_flags & (uint)XeImageFlags.ORANGE2) == (uint)XeImageFlags.ORANGE2) { checkedListBox1.SetItemChecked(9, true); }
            if ((image_flags & (uint)XeImageFlags.IPTV_SIGNUP_APPLICATION) == (uint)XeImageFlags.IPTV_SIGNUP_APPLICATION) { checkedListBox1.SetItemChecked(10, true); }
            if ((image_flags & (uint)XeImageFlags.IPTV_TITLE_APPLICATION) == (uint)XeImageFlags.IPTV_TITLE_APPLICATION) { checkedListBox1.SetItemChecked(11, true); }
            if ((image_flags & (uint)XeImageFlags.KEYVAULT_PRIVILEGES_REQUIRED) == (uint)XeImageFlags.KEYVAULT_PRIVILEGES_REQUIRED) { checkedListBox1.SetItemChecked(12, true); }
            if ((image_flags & (uint)XeImageFlags.ONLINE_ACTIVATION_REQUIRED) == (uint)XeImageFlags.ONLINE_ACTIVATION_REQUIRED) { checkedListBox1.SetItemChecked(13, true); }
            if ((image_flags & (uint)XeImageFlags.PAGE_SIZE_4KB) == (uint)XeImageFlags.PAGE_SIZE_4KB) { checkedListBox1.SetItemChecked(14, true); }
            if ((image_flags & (uint)XeImageFlags.REGION_FREE) == (uint)XeImageFlags.REGION_FREE) { checkedListBox1.SetItemChecked(15, true); }
            if ((image_flags & (uint)XeImageFlags.REVOCATION_CHECK_OPTIONAL) == (uint)XeImageFlags.REVOCATION_CHECK_OPTIONAL) { checkedListBox1.SetItemChecked(16, true); }
            if ((image_flags & (uint)XeImageFlags.REVOCATION_CHECK_REQUIRED) == (uint)XeImageFlags.REVOCATION_CHECK_REQUIRED) { checkedListBox1.SetItemChecked(17, true); }
            #endregion
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            checkedListBox1.SetItemChecked(0, false);
            checkedListBox1.SetItemChecked(1, false);
            checkedListBox1.SetItemChecked(2, false);
            checkedListBox1.SetItemChecked(3, false);
            checkedListBox1.SetItemChecked(4, false);
            checkedListBox1.SetItemChecked(5, false);
            checkedListBox1.SetItemChecked(6, false);
            checkedListBox1.SetItemChecked(7, false);
            checkedListBox1.SetItemChecked(8, false);
            checkedListBox1.SetItemChecked(9, false);
            checkedListBox1.SetItemChecked(10, false);
            checkedListBox1.SetItemChecked(11, false);
            checkedListBox1.SetItemChecked(12, false);
            checkedListBox1.SetItemChecked(13, false);
            checkedListBox1.SetItemChecked(14, false);
            checkedListBox1.SetItemChecked(15, false);
            checkedListBox1.SetItemChecked(16, false);
            checkedListBox1.SetItemChecked(17, false);
        }

        private void ImageFlags_FormClosing(object sender, FormClosingEventArgs e)
        {
            UInt32 data = 0;
            foreach (int indexChecked in checkedListBox1.CheckedIndices)
            {
                switch (indexChecked)
                {
                    case 0:
                        data |= (uint)XeImageFlags.MANUFACTURING_UTILITY;
                        break;
                    case 1:
                        data |= (uint)XeImageFlags.MANUFACTURING_SUPPORT_TOOLS;
                        break;
                    case 2:
                        data |= (uint)XeImageFlags.XGD2_MEDIA_ONLY;
                        break;
                    case 3:
                        data |= (uint)XeImageFlags.CARDEA_KEY;
                        break;
                    case 4:
                        data |= (uint)XeImageFlags.XEIKA_KEY;
                        break;
                    case 5:
                        data |= (uint)XeImageFlags.USERMODE_TITLE;
                        break;
                    case 6:
                        data |= (uint)XeImageFlags.USERMODE_SYSTEM;
                        break;
                    case 7:
                        data |= (uint)XeImageFlags.ORANGE0;
                        break;
                    case 8:
                        data |= (uint)XeImageFlags.ORANGE1;
                        break;
                    case 9:
                        data |= (uint)XeImageFlags.ORANGE2;
                        break;
                    case 10:
                        data |= (uint)XeImageFlags.IPTV_SIGNUP_APPLICATION;
                        break;
                    case 11:
                        data |= (uint)XeImageFlags.IPTV_TITLE_APPLICATION;
                        break;
                    case 12:
                        data |= (uint)XeImageFlags.KEYVAULT_PRIVILEGES_REQUIRED;
                        break;
                    case 13:
                        data |= (uint)XeImageFlags.ONLINE_ACTIVATION_REQUIRED;
                        break;
                    case 14:
                        data |= (uint)XeImageFlags.PAGE_SIZE_4KB;
                        break;
                    case 15:
                        data |= (uint)XeImageFlags.REGION_FREE;
                        break;
                    case 16:
                        data |= (uint)XeImageFlags.REVOCATION_CHECK_OPTIONAL;
                        break;
                    case 17:
                        data |= (uint)XeImageFlags.REVOCATION_CHECK_REQUIRED;
                        break;

                }
            }
            image_flags = data;
        }
    }
}
