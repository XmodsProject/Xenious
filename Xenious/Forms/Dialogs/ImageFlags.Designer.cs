namespace Xenious.Forms.Dialogs
{
    partial class ImageFlags
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "MANUFACTURING_UTILITY",
            "MANUFACTURING_SUPPORT_TOOLS",
            "XGD2_MEDIA_ONLY",
            "CARDEA_KEY",
            "XEIKA_KEY",
            "USERMODE_TITLE",
            "USERMODE_SYSTEM",
            "ORANGE0",
            "ORANGE1",
            "ORANGE2",
            "IPTV_SIGNUP_APPLICATION",
            "IPTV_TITLE_APPLICATION",
            "KEYVAULT_PRIVILEGES_REQUIRED",
            "ONLINE_ACTIVATION_REQUIRED",
            "PAGE_SIZE_4KB",
            "REGION_FREE",
            "REVOCATION_CHECK_OPTIONAL",
            "REVOCATION_CHECK_REQUIRED"});
            this.checkedListBox1.Location = new System.Drawing.Point(14, 17);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(418, 382);
            this.checkedListBox1.TabIndex = 1;
            this.checkedListBox1.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 423);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(418, 35);
            this.button1.TabIndex = 2;
            this.button1.Text = "Clear Image Flags";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ImageFlags
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(449, 475);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkedListBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ImageFlags";
            this.Padding = new System.Windows.Forms.Padding(14);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Image Flags";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ImageFlags_FormClosing);
            this.Load += new System.EventHandler(this.ImageFlags_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button button1;


    }
}
