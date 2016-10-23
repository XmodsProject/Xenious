namespace Xenious.Forms.Dialogs
{
    partial class MediaFlags
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
            this.checkedListBox2 = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedListBox2
            // 
            this.checkedListBox2.FormattingEnabled = true;
            this.checkedListBox2.Items.AddRange(new object[] {
            "HARDDISK",
            "DVD_X2",
            "DVD_CD,",
            "DVD_5",
            "DVD_9",
            "SYSTEM_FLASH",
            "MEMORY_UNIT",
            "USB_MASS_STORAGE_DEVICE",
            "NETWORK",
            "DIRECT_FROM_MEMORY",
            "RAM_DRIVE",
            "SVOD",
            "INSECURE_PACKAGE",
            "SAVEGAME_PACKAGE",
            "LOCALLY_SIGNED_PACKAGE",
            "LIVE_SIGNED_PACKAGE",
            "XBOX_PACKAGE"});
            this.checkedListBox2.Location = new System.Drawing.Point(14, 14);
            this.checkedListBox2.Name = "checkedListBox2";
            this.checkedListBox2.Size = new System.Drawing.Size(383, 361);
            this.checkedListBox2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 397);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(383, 42);
            this.button1.TabIndex = 3;
            this.button1.Text = "Clear Media Flags";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MediaFlags
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 456);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkedListBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MediaFlags";
            this.Padding = new System.Windows.Forms.Padding(14);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Media Flags";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MediaFlags_FormClosing);
            this.Load += new System.EventHandler(this.MediaFlags_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox2;
        private System.Windows.Forms.Button button1;

    }
}
