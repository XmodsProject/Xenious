namespace Xenious.Forms.Dialogs
{
    partial class SystemFlags
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkedListBox4 = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // checkedListBox4
            // 
            this.checkedListBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox4.FormattingEnabled = true;
            this.checkedListBox4.Items.AddRange(new object[] {
            "NO_FORCED_REBOOT",
            "FOREGROUND_TASKS",
            "NO_ODD_MAPPING",
            "HANDLE_MCE_INPUT",
            "RESTRICTED_HUD_FEATURES",
            "HANDLE_GAMEPAD_DISCONNECT",
            "INSECURE_SOCKETS",
            "XBOX1_INTEROPERABILITY",
            "DASH_CONTEXT",
            "USES_GAME_VOICE_CHANNEL",
            "PAL50_INCOMPATIBLE",
            "INSECURE_UTILITY_DRIVE",
            "XAM_HOOKS",
            "ACCESS_PII",
            "CROSS_PLATFORM_SYSTEM_LINK",
            "MULTIDISC_SWAP",
            "MULTIDISC_INSECURE_MEDIA",
            "AP25_MEDIA",
            "NO_CONFIRM_EXIT",
            "ALLOW_BACKGROUND_DOWNLOAD",
            "CREATE_PERSISTABLE_RAMDRIVE",
            "INHERIT_PERSISTENT_RAMDRIVE",
            "ALLOW_HUD_VIBRATION",
            "ACCESS_UTILITY_PARTITIONS",
            "IPTV_INPUT_SUPPORTED",
            "PREFER_BIG_BUTTON_INPUT",
            "ALLOW_EXTENDED_SYSTEM_RESERVATION",
            "MULTIDISC_CROSS_TITLE",
            "INSTALL_INCOMPATIBLE",
            "ALLOW_AVATAR_GET_METADATA_BY_XUID",
            "ALLOW_CONTROLLER_SWAPPING",
            "DASH_EXTENSIBILITY_MODULE"});
            this.checkedListBox4.Location = new System.Drawing.Point(14, 14);
            this.checkedListBox4.Name = "checkedListBox4";
            this.checkedListBox4.Size = new System.Drawing.Size(444, 443);
            this.checkedListBox4.TabIndex = 1;
            // 
            // SystemFlags
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 471);
            this.Controls.Add(this.checkedListBox4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemFlags";
            this.Padding = new System.Windows.Forms.Padding(14);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "System Flags";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SystemFlags_FormClosing);
            this.Load += new System.EventHandler(this.SystemFlags_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox4;

    }
}
