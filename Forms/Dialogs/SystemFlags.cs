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
    partial class SystemFlags : Form
    {
        public UInt32 system_flags;

        public UInt32 get_flags() { return system_flags; }
        public void set_flags(UInt32 flags)
        {
            system_flags = flags;
        }
        public SystemFlags()
        {
            InitializeComponent();
        }

        private void SystemFlags_Load(object sender, EventArgs e)
        {
            init_system_data();
        }

        private void init_system_data()
        {
            #region System Flags
            if ((system_flags & (uint)XeSystemFlags.NO_FORCED_REBOOT) == (uint)XeSystemFlags.NO_FORCED_REBOOT) { checkedListBox4.SetItemChecked(0, true); }
            if ((system_flags & (uint)XeSystemFlags.FOREGROUND_TASKS) == (uint)XeSystemFlags.FOREGROUND_TASKS) { checkedListBox4.SetItemChecked(1, true); }
            if ((system_flags & (uint)XeSystemFlags.NO_ODD_MAPPING) == (uint)XeSystemFlags.NO_ODD_MAPPING) { checkedListBox4.SetItemChecked(2, true); }
            if ((system_flags & (uint)XeSystemFlags.HANDLE_MCE_INPUT) == (uint)XeSystemFlags.HANDLE_MCE_INPUT) { checkedListBox4.SetItemChecked(3, true); }
            if ((system_flags & (uint)XeSystemFlags.RESTRICTED_HUD_FEATURES) == (uint)XeSystemFlags.RESTRICTED_HUD_FEATURES) { checkedListBox4.SetItemChecked(4, true); }
            if ((system_flags & (uint)XeSystemFlags.HANDLE_GAMEPAD_DISCONNECT) == (uint)XeSystemFlags.HANDLE_GAMEPAD_DISCONNECT) { checkedListBox4.SetItemChecked(5, true); }
            if ((system_flags & (uint)XeSystemFlags.HANDLE_GAMEPAD_DISCONNECT) == (uint)XeSystemFlags.HANDLE_GAMEPAD_DISCONNECT) { checkedListBox4.SetItemChecked(6, true); }
            if ((system_flags & (uint)XeSystemFlags.INSECURE_SOCKETS) == (uint)XeSystemFlags.INSECURE_SOCKETS) { checkedListBox4.SetItemChecked(7, true); }
            if ((system_flags & (uint)XeSystemFlags.XBOX1_INTEROPERABILITY) == (uint)XeSystemFlags.XBOX1_INTEROPERABILITY) { checkedListBox4.SetItemChecked(8, true); }
            if ((system_flags & (uint)XeSystemFlags.DASH_CONTEXT) == (uint)XeSystemFlags.DASH_CONTEXT) { checkedListBox4.SetItemChecked(9, true); }
            if ((system_flags & (uint)XeSystemFlags.USES_GAME_VOICE_CHANNEL) == (uint)XeSystemFlags.USES_GAME_VOICE_CHANNEL) { checkedListBox4.SetItemChecked(10, true); }
            if ((system_flags & (uint)XeSystemFlags.PAL50_INCOMPATIBLE) == (uint)XeSystemFlags.PAL50_INCOMPATIBLE) { checkedListBox4.SetItemChecked(10, true); }
            if ((system_flags & (uint)XeSystemFlags.INSECURE_UTILITY_DRIVE) == (uint)XeSystemFlags.INSECURE_UTILITY_DRIVE) { checkedListBox4.SetItemChecked(11, true); }
            if ((system_flags & (uint)XeSystemFlags.XAM_HOOKS) == (uint)XeSystemFlags.XAM_HOOKS) { checkedListBox4.SetItemChecked(12, true); }
            if ((system_flags & (uint)XeSystemFlags.ACCESS_PII) == (uint)XeSystemFlags.ACCESS_PII) { checkedListBox4.SetItemChecked(13, true); }
            if ((system_flags & (uint)XeSystemFlags.CROSS_PLATFORM_SYSTEM_LINK) == (uint)XeSystemFlags.CROSS_PLATFORM_SYSTEM_LINK) { checkedListBox4.SetItemChecked(14, true); }
            if ((system_flags & (uint)XeSystemFlags.MULTIDISC_SWAP) == (uint)XeSystemFlags.MULTIDISC_SWAP) { checkedListBox4.SetItemChecked(15, true); }
            if ((system_flags & (uint)XeSystemFlags.MULTIDISC_INSECURE_MEDIA) == (uint)XeSystemFlags.MULTIDISC_INSECURE_MEDIA) { checkedListBox4.SetItemChecked(16, true); }
            if ((system_flags & (uint)XeSystemFlags.AP25_MEDIA) == (uint)XeSystemFlags.AP25_MEDIA) { checkedListBox4.SetItemChecked(17, true); }
            if ((system_flags & (uint)XeSystemFlags.NO_CONFIRM_EXIT) == (uint)XeSystemFlags.NO_CONFIRM_EXIT) { checkedListBox4.SetItemChecked(18, true); }
            if ((system_flags & (uint)XeSystemFlags.ALLOW_BACKGROUND_DOWNLOAD) == (uint)XeSystemFlags.ALLOW_BACKGROUND_DOWNLOAD) { checkedListBox4.SetItemChecked(19, true); }
            if ((system_flags & (uint)XeSystemFlags.CREATE_PERSISTABLE_RAMDRIVE) == (uint)XeSystemFlags.CREATE_PERSISTABLE_RAMDRIVE) { checkedListBox4.SetItemChecked(20, true); }
            if ((system_flags & (uint)XeSystemFlags.INHERIT_PERSISTENT_RAMDRIVE) == (uint)XeSystemFlags.INHERIT_PERSISTENT_RAMDRIVE) { checkedListBox4.SetItemChecked(21, true); }
            if ((system_flags & (uint)XeSystemFlags.ALLOW_HUD_VIBRATION) == (uint)XeSystemFlags.ALLOW_HUD_VIBRATION) { checkedListBox4.SetItemChecked(22, true); }
            if ((system_flags & (uint)XeSystemFlags.ACCESS_UTILITY_PARTITIONS) == (uint)XeSystemFlags.ACCESS_UTILITY_PARTITIONS) { checkedListBox4.SetItemChecked(23, true); }
            if ((system_flags & (uint)XeSystemFlags.IPTV_INPUT_SUPPORTED) == (uint)XeSystemFlags.IPTV_INPUT_SUPPORTED) { checkedListBox4.SetItemChecked(24, true); }
            if ((system_flags & (uint)XeSystemFlags.PREFER_BIG_BUTTON_INPUT) == (uint)XeSystemFlags.PREFER_BIG_BUTTON_INPUT) { checkedListBox4.SetItemChecked(25, true); }
            if ((system_flags & (uint)XeSystemFlags.ALLOW_EXTENDED_SYSTEM_RESERVATION) == (uint)XeSystemFlags.ALLOW_EXTENDED_SYSTEM_RESERVATION) { checkedListBox4.SetItemChecked(26, true); }
            if ((system_flags & (uint)XeSystemFlags.MULTIDISC_CROSS_TITLE) == (uint)XeSystemFlags.MULTIDISC_CROSS_TITLE) { checkedListBox4.SetItemChecked(27, true); }
            if ((system_flags & (uint)XeSystemFlags.INSTALL_INCOMPATIBLE) == (uint)XeSystemFlags.INSTALL_INCOMPATIBLE) { checkedListBox4.SetItemChecked(28, true); }
            if ((system_flags & (uint)XeSystemFlags.ALLOW_AVATAR_GET_METADATA_BY_XUID) == (uint)XeSystemFlags.ALLOW_AVATAR_GET_METADATA_BY_XUID) { checkedListBox4.SetItemChecked(29, true); }
            if ((system_flags & (uint)XeSystemFlags.ALLOW_CONTROLLER_SWAPPING) == (uint)XeSystemFlags.ALLOW_CONTROLLER_SWAPPING) { checkedListBox4.SetItemChecked(30, true); }
            if ((system_flags & (uint)XeSystemFlags.DASH_EXTENSIBILITY_MODULE) == (uint)XeSystemFlags.DASH_EXTENSIBILITY_MODULE) { checkedListBox4.SetItemChecked(31, true); }
            #endregion
        }

        private void SystemFlags_FormClosing(object sender, FormClosingEventArgs e)
        {
            UInt32 data = 0;
            foreach (int indexChecked in checkedListBox4.CheckedIndices)
            {
                switch (indexChecked)
                {
                    case 0:
                        data |= (uint)XeSystemFlags.NO_FORCED_REBOOT;
                        break;
                    case 1:
                        data |= (uint)XeSystemFlags.FOREGROUND_TASKS;
                        break;
                    case 2:
                        data |= (uint)XeSystemFlags.NO_ODD_MAPPING;
                        break;
                    case 3:
                        data |= (uint)XeSystemFlags.HANDLE_MCE_INPUT;
                        break;
                    case 4:
                        data |= (uint)XeSystemFlags.RESTRICTED_HUD_FEATURES;
                        break;
                    case 5:
                        data |= (uint)XeSystemFlags.HANDLE_GAMEPAD_DISCONNECT;
                        break;
                    case 6:
                        data |= (uint)XeSystemFlags.INSECURE_SOCKETS;
                        break;
                    case 7:
                        data |= (uint)XeSystemFlags.XBOX1_INTEROPERABILITY;
                        break;
                    case 8:
                        data |= (uint)XeSystemFlags.DASH_CONTEXT;
                        break;
                    case 9:
                        data |= (uint)XeSystemFlags.USES_GAME_VOICE_CHANNEL;
                        break;
                    case 10:
                        data |= (uint)XeSystemFlags.PAL50_INCOMPATIBLE;
                        break;
                    case 11:
                        data |= (uint)XeSystemFlags.INSECURE_UTILITY_DRIVE;
                        break;
                    case 12:
                        data |= (uint)XeSystemFlags.XAM_HOOKS;
                        break;
                    case 13:
                        data |= (uint)XeSystemFlags.ACCESS_PII;
                        break;
                    case 14:
                        data |= (uint)XeSystemFlags.CROSS_PLATFORM_SYSTEM_LINK;
                        break;
                    case 15:
                        data |= (uint)XeSystemFlags.MULTIDISC_SWAP;
                        break;
                    case 16:
                        data |= (uint)XeSystemFlags.MULTIDISC_INSECURE_MEDIA;
                        break;
                    case 17:
                        data |= (uint)XeSystemFlags.AP25_MEDIA;
                        break;
                    case 18:
                        data |= (uint)XeSystemFlags.NO_CONFIRM_EXIT;
                        break;
                    case 19:
                        data |= (uint)XeSystemFlags.ALLOW_BACKGROUND_DOWNLOAD;
                        break;
                    case 20:
                        data |= (uint)XeSystemFlags.CREATE_PERSISTABLE_RAMDRIVE;
                        break;
                    case 21:
                        data |= (uint)XeSystemFlags.INHERIT_PERSISTENT_RAMDRIVE;
                        break;
                    case 22:
                        data |= (uint)XeSystemFlags.ALLOW_HUD_VIBRATION;
                        break;
                    case 23:
                        data |= (uint)XeSystemFlags.ACCESS_UTILITY_PARTITIONS;
                        break;
                    case 24:
                        data |= (uint)XeSystemFlags.IPTV_INPUT_SUPPORTED;
                        break;
                    case 25:
                        data |= (uint)XeSystemFlags.PREFER_BIG_BUTTON_INPUT;
                        break;
                    case 26:
                        data |= (uint)XeSystemFlags.ALLOW_EXTENDED_SYSTEM_RESERVATION;
                        break;
                    case 27:
                        data |= (uint)XeSystemFlags.MULTIDISC_CROSS_TITLE;
                        break;
                    case 28:
                        data |= (uint)XeSystemFlags.INSTALL_INCOMPATIBLE;
                        break;
                    case 29:
                        data |= (uint)XeSystemFlags.ALLOW_AVATAR_GET_METADATA_BY_XUID;
                        break;
                    case 30:
                        data |= (uint)XeSystemFlags.ALLOW_CONTROLLER_SWAPPING;
                        break;
                    case 31:
                        data |= (uint)XeSystemFlags.DASH_EXTENSIBILITY_MODULE;
                        break;
                }
            }
            system_flags = data;
        }
    }
}
