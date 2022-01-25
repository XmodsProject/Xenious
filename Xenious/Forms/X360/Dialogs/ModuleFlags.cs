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
    partial class ModuleFlags : Form
    {
        UInt32 Module_Flags;

        public void set_flags(UInt32 flags)
        {
            Module_Flags = flags;
        }
        public UInt32 get_flags() { return Module_Flags; }
        private void init_module_data()
        {
            if ((Module_Flags & (uint)XeModuleFlags.TITLE) == (uint)XeModuleFlags.TITLE) { checkedListBox1.SetItemChecked(0, true); }
            if ((Module_Flags & (uint)XeModuleFlags.EXPORTS_TO_TITLE) == (uint)XeModuleFlags.EXPORTS_TO_TITLE) { checkedListBox1.SetItemChecked(1, true); }
            if ((Module_Flags & (uint)XeModuleFlags.SYSTEM_DEBUGGER) == (uint)XeModuleFlags.SYSTEM_DEBUGGER) { checkedListBox1.SetItemChecked(2, true); }
            if ((Module_Flags & (uint)XeModuleFlags.DLL_MODULE) == (uint)XeModuleFlags.DLL_MODULE) { checkedListBox1.SetItemChecked(3, true); }
            if ((Module_Flags & (uint)XeModuleFlags.MODULE_PATCH) == (uint)XeModuleFlags.MODULE_PATCH) { checkedListBox1.SetItemChecked(4, true); }
            if ((Module_Flags & (uint)XeModuleFlags.PATCH_FULL) == (uint)XeModuleFlags.PATCH_FULL) { checkedListBox1.SetItemChecked(5, true); }
            if ((Module_Flags & (uint)XeModuleFlags.PATCH_DELTA) == (uint)XeModuleFlags.PATCH_DELTA) { checkedListBox1.SetItemChecked(6, true); }
            if ((Module_Flags & (uint)XeModuleFlags.USER_MODE) == (uint)XeModuleFlags.USER_MODE) { checkedListBox1.SetItemChecked(7, true); }
        }
        public ModuleFlags()
        {
            InitializeComponent();
        }

        private void ModuleFlags_Load(object sender, EventArgs e)
        {
            init_module_data();
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
        }

        private void ModuleFlags_FormClosing(object sender, FormClosingEventArgs e)
        {
            UInt32 data = 0;
            foreach (int indexChecked in checkedListBox1.CheckedIndices)
            {
                switch (indexChecked)
                {
                    case 0:
                        data |= (uint)XeModuleFlags.TITLE;
                        break;
                    case 1:
                        data |= (uint)XeModuleFlags.EXPORTS_TO_TITLE;
                        break;
                    case 2:
                        data |= (uint)XeModuleFlags.SYSTEM_DEBUGGER;
                        break;
                    case 3:
                        data |= (uint)XeModuleFlags.DLL_MODULE;
                        break;
                    case 4:
                        data |= (uint)XeModuleFlags.MODULE_PATCH;
                        break;
                    case 5:
                        data |= (uint)XeModuleFlags.PATCH_FULL;
                        break;
                    case 6:
                        data |= (uint)XeModuleFlags.PATCH_DELTA;
                        break;
                    case 7:
                        data |= (uint)XeModuleFlags.USER_MODE;
                        break;

                }
            }
            Module_Flags = data;
        }
    }
}
