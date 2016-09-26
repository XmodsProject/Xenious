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
    partial class MediaFlags : Form
    {
        public UInt32 media_flags;
        public void set_flag(UInt32 flags)
        {
            media_flags = flags;
        }
        public UInt32 get_flags() { return media_flags; }
        private void init_media_flags()
        {
            #region Media Flags
            if ((media_flags & (uint)XeMediaFlags.HARDDISK) == (uint)XeMediaFlags.HARDDISK) { checkedListBox2.SetItemChecked(0, true); }
            if ((media_flags & (uint)XeMediaFlags.DVD_X2) == (uint)XeMediaFlags.DVD_X2) { checkedListBox2.SetItemChecked(1, true); }
            if ((media_flags & (uint)XeMediaFlags.DVD_CD) == (uint)XeMediaFlags.DVD_CD) { checkedListBox2.SetItemChecked(2, true); }
            if ((media_flags & (uint)XeMediaFlags.DVD_5) == (uint)XeMediaFlags.DVD_5) { checkedListBox2.SetItemChecked(3, true); }
            if ((media_flags & (uint)XeMediaFlags.DVD_9) == (uint)XeMediaFlags.DVD_9) { checkedListBox2.SetItemChecked(4, true); }
            if ((media_flags & (uint)XeMediaFlags.SYSTEM_FLASH) == (uint)XeMediaFlags.SYSTEM_FLASH) { checkedListBox2.SetItemChecked(5, true); }
            if ((media_flags & (uint)XeMediaFlags.MEMORY_UNIT) == (uint)XeMediaFlags.MEMORY_UNIT) { checkedListBox2.SetItemChecked(6, true); }
            if ((media_flags & (uint)XeMediaFlags.USB_MASS_STORAGE_DEVICE) == (uint)XeMediaFlags.USB_MASS_STORAGE_DEVICE) { checkedListBox2.SetItemChecked(7, true); }
            if ((media_flags & (uint)XeMediaFlags.NETWORK) == (uint)XeMediaFlags.NETWORK) { checkedListBox2.SetItemChecked(8, true); }
            if ((media_flags & (uint)XeMediaFlags.DIRECT_FROM_MEMORY) == (uint)XeMediaFlags.DIRECT_FROM_MEMORY) { checkedListBox2.SetItemChecked(9, true); }
            if ((media_flags & (uint)XeMediaFlags.RAM_DRIVE) == (uint)XeMediaFlags.RAM_DRIVE) { checkedListBox2.SetItemChecked(10, true); }
            if ((media_flags & (uint)XeMediaFlags.SVOD) == (uint)XeMediaFlags.SVOD) { checkedListBox2.SetItemChecked(11, true); }
            if ((media_flags & (uint)XeMediaFlags.INSECURE_PACKAGE) == (uint)XeMediaFlags.INSECURE_PACKAGE) { checkedListBox2.SetItemChecked(12, true); }
            if ((media_flags & (uint)XeMediaFlags.SAVEGAME_PACKAGE) == (uint)XeMediaFlags.SAVEGAME_PACKAGE) { checkedListBox2.SetItemChecked(13, true); }
            if ((media_flags & (uint)XeMediaFlags.LOCALLY_SIGNED_PACKAGE) == (uint)XeMediaFlags.LOCALLY_SIGNED_PACKAGE) { checkedListBox2.SetItemChecked(14, true); }
            if ((media_flags & (uint)XeMediaFlags.LIVE_SIGNED_PACKAGE) == (uint)XeMediaFlags.LIVE_SIGNED_PACKAGE) { checkedListBox2.SetItemChecked(15, true); }
            if ((media_flags & (uint)XeMediaFlags.XBOX_PACKAGE) == (uint)XeMediaFlags.XBOX_PACKAGE) { checkedListBox2.SetItemChecked(16, true); }
            #endregion
        }
        public MediaFlags()
        {
            InitializeComponent();
        }

        private void MediaFlags_Load(object sender, EventArgs e)
        {
            init_media_flags();
        }

        private void MediaFlags_FormClosing(object sender, FormClosingEventArgs e)
        {
            UInt32 data = 0;
            foreach (int indexChecked in checkedListBox2.CheckedIndices)
            {
                switch (indexChecked)
                {
                    case 0:
                        data |= (uint)XeMediaFlags.HARDDISK;
                        break;
                    case 1:
                        data |= (uint)XeMediaFlags.DVD_X2;
                        break;
                    case 2:
                        data |= (uint)XeMediaFlags.DVD_CD;
                        break;
                    case 3:
                        data |= (uint)XeMediaFlags.DVD_5;
                        break;
                    case 4:
                        data |= (uint)XeMediaFlags.DVD_9;
                        break;
                    case 5:
                        data |= (uint)XeMediaFlags.SYSTEM_FLASH;
                        break;
                    case 6:
                        data |= (uint)XeMediaFlags.MEMORY_UNIT;
                        break;
                    case 7:
                        data |= (uint)XeMediaFlags.USB_MASS_STORAGE_DEVICE;
                        break;
                    case 8:
                        data |= (uint)XeMediaFlags.NETWORK;
                        break;
                    case 9:
                        data |= (uint)XeMediaFlags.DIRECT_FROM_MEMORY;
                        break;
                    case 10:
                        data |= (uint)XeMediaFlags.RAM_DRIVE;
                        break;
                    case 11:
                        data |= (uint)XeMediaFlags.SVOD;
                        break;
                    case 12:
                        data |= (uint)XeMediaFlags.INSECURE_PACKAGE;
                        break;
                    case 13:
                        data |= (uint)XeMediaFlags.SAVEGAME_PACKAGE;
                        break;
                    case 14:
                        data |= (uint)XeMediaFlags.LOCALLY_SIGNED_PACKAGE;
                        break;
                    case 15:
                        data |= (uint)XeMediaFlags.LIVE_SIGNED_PACKAGE;
                        break;
                    case 16:
                        data |= (uint)XeMediaFlags.XBOX_PACKAGE;
                        break;

                }
            }
            media_flags = data;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(media_flags == 0)
            {
                checkedListBox2.SetItemChecked(0, true);
                checkedListBox2.SetItemChecked(1, true);
                checkedListBox2.SetItemChecked(2, true);
                checkedListBox2.SetItemChecked(3, true);
                checkedListBox2.SetItemChecked(4, true);
                checkedListBox2.SetItemChecked(5, true);
                checkedListBox2.SetItemChecked(6, true);
                checkedListBox2.SetItemChecked(7, true);
                checkedListBox2.SetItemChecked(8, true);
                checkedListBox2.SetItemChecked(9, true);
                checkedListBox2.SetItemChecked(10, true);
                checkedListBox2.SetItemChecked(11, true);
                checkedListBox2.SetItemChecked(12, true);
                checkedListBox2.SetItemChecked(13, true);
                checkedListBox2.SetItemChecked(14, true);
                checkedListBox2.SetItemChecked(15, true);
                checkedListBox2.SetItemChecked(16, true);
                button1.Text = "Clear Media Flags";
                media_flags = (uint)520101823;
            }
            else
            {
                checkedListBox2.SetItemChecked(0, false);
                checkedListBox2.SetItemChecked(1, false);
                checkedListBox2.SetItemChecked(2, false);
                checkedListBox2.SetItemChecked(3, false);
                checkedListBox2.SetItemChecked(4, false);
                checkedListBox2.SetItemChecked(5, false);
                checkedListBox2.SetItemChecked(6, false);
                checkedListBox2.SetItemChecked(7, false);
                checkedListBox2.SetItemChecked(8, false);
                checkedListBox2.SetItemChecked(9, false);
                checkedListBox2.SetItemChecked(10, false);
                checkedListBox2.SetItemChecked(11, false);
                checkedListBox2.SetItemChecked(12, false);
                checkedListBox2.SetItemChecked(13, false);
                checkedListBox2.SetItemChecked(14, false);
                checkedListBox2.SetItemChecked(15, false);
                checkedListBox2.SetItemChecked(16, false);
                media_flags = (uint)0;
                button1.Text = "All Media Flags";
            }
        }
    }
}
