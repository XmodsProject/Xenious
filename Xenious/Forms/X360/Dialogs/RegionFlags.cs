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
    partial class RegionFlags : Form
    {
        public UInt32 region_data;

        public void set_flags(UInt32 flags) {
            region_data = flags;
        }
        public UInt32 get_flags() { return region_data; }
        public RegionFlags()
        {
            InitializeComponent();
        }
        private void RegionFlags_Load(object sender, EventArgs e)
        {
            init_region_data();
            if ((region_data & (uint)XeRegionFlags.ALL) == (uint)XeRegionFlags.ALL)
            {
                button1.Text = "Clear Region";
            }
        }

        private void init_region_data()
        {
            if ((region_data & (uint)XeRegionFlags.ALL) == (uint)XeRegionFlags.ALL)
            {
                checkedListBox3.SetItemChecked(0, true);
                checkedListBox3.SetItemChecked(1, true);
                checkedListBox3.SetItemChecked(2, true);
                checkedListBox3.SetItemChecked(3, true);
                checkedListBox3.SetItemChecked(4, true);
                checkedListBox3.SetItemChecked(5, true);
                checkedListBox3.SetItemChecked(6, true);

            }
            else
            {
                if ((region_data & (uint)XeRegionFlags.NTSCU) == (uint)XeRegionFlags.NTSCU) { checkedListBox3.SetItemChecked(0, true); }
                if ((region_data & (uint)XeRegionFlags.NTSCJ) == (uint)XeRegionFlags.NTSCJ) { checkedListBox3.SetItemChecked(1, true); }
                if ((region_data & (uint)XeRegionFlags.NTSCJ_JAPAN) == (uint)XeRegionFlags.NTSCJ_JAPAN) { checkedListBox3.SetItemChecked(2, true); }
                if ((region_data & (uint)XeRegionFlags.NTSCJ_CHINA) == (uint)XeRegionFlags.NTSCJ_CHINA) { checkedListBox3.SetItemChecked(3, true); }
                if ((region_data & (uint)XeRegionFlags.PAL) == (uint)XeRegionFlags.PAL) { checkedListBox3.SetItemChecked(4, true); }
                if ((region_data & (uint)XeRegionFlags.PAL_AU_NZ) == (uint)XeRegionFlags.PAL_AU_NZ) { checkedListBox3.SetItemChecked(5, true); }
                if ((region_data & (uint)XeRegionFlags.OTHER_ASIA) == (uint)XeRegionFlags.OTHER_ASIA) { checkedListBox3.SetItemChecked(6, true); }
            }
        }

        private void clear_region_data()
        {
            checkedListBox3.SetItemChecked(0, false);
            checkedListBox3.SetItemChecked(1, false);
            checkedListBox3.SetItemChecked(2, false);
            checkedListBox3.SetItemChecked(3, false);
            checkedListBox3.SetItemChecked(4, false);
            checkedListBox3.SetItemChecked(5, false);
            checkedListBox3.SetItemChecked(6, false);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if ((region_data & (uint)XeRegionFlags.ALL) != (uint)XeRegionFlags.ALL)
            {
                region_data = (uint)XeRegionFlags.ALL;
                init_region_data();
                button1.Text = "Clear Region";
            }
            else
            {
                region_data = (uint)0;
                clear_region_data();
                button1.Text = "Region Free";
            }
        }

        private void RegionFlags_FormClosing(object sender, FormClosingEventArgs e)
        {
            UInt32 data = 0;
            foreach (int indexChecked in checkedListBox3.CheckedIndices)
            {
                switch (indexChecked)
                {
                    case 0:
                        data |= (uint)XeRegionFlags.NTSCU;
                        break;
                    case 1:
                        data |= (uint)XeRegionFlags.NTSCJ;
                        break;
                    case 2:
                        data |= (uint)XeRegionFlags.NTSCJ_JAPAN;
                        break;
                    case 3:
                        data |= (uint)XeRegionFlags.NTSCJ_CHINA;
                        break;
                    case 4:
                        data |= (uint)XeRegionFlags.PAL;
                        break;
                    case 5:
                        data |= (uint)XeRegionFlags.PAL_AU_NZ;
                        break;
                    case 6:
                        data |= (uint)XeRegionFlags.OTHER_ASIA;
                        break;

                }
            }
            region_data = data;
        }
    }
}
