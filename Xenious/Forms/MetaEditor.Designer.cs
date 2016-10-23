namespace Xenious.Forms
{
    partial class MetaEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MetaEditor));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rebuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.certificateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regionFlagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mediaFlagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageFlagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.otherToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemFlagsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moduleFlagsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.executionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ratingsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alternativeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.titleIDsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionalHeadersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.systemToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lanKeyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.imageBaseAddressToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.originalBaseAddressToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.entryPointToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.titleWorkspaceSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deviceIDToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.boundingPathToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stackSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filesystemCacheSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.heapSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xGD3MediaIDToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.originalPENameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.databaseToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1260, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.rebuildToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(156, 30);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(156, 30);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // rebuildToolStripMenuItem
            // 
            this.rebuildToolStripMenuItem.Enabled = false;
            this.rebuildToolStripMenuItem.Name = "rebuildToolStripMenuItem";
            this.rebuildToolStripMenuItem.Size = new System.Drawing.Size(156, 30);
            this.rebuildToolStripMenuItem.Text = "Rebuild";
            this.rebuildToolStripMenuItem.Click += new System.EventHandler(this.rebuildToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(156, 30);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(153, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(156, 30);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.certificateToolStripMenuItem,
            this.systemToolStripMenuItem,
            this.executionToolStripMenuItem,
            this.ratingsToolStripMenuItem1,
            this.extractToolStripMenuItem,
            this.alternativeToolStripMenuItem,
            this.optionalHeadersToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(65, 29);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // certificateToolStripMenuItem
            // 
            this.certificateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.regionFlagsToolStripMenuItem,
            this.mediaFlagsToolStripMenuItem,
            this.imageFlagsToolStripMenuItem,
            this.otherToolStripMenuItem});
            this.certificateToolStripMenuItem.Name = "certificateToolStripMenuItem";
            this.certificateToolStripMenuItem.Size = new System.Drawing.Size(236, 30);
            this.certificateToolStripMenuItem.Text = "Certificate";
            // 
            // regionFlagsToolStripMenuItem
            // 
            this.regionFlagsToolStripMenuItem.Name = "regionFlagsToolStripMenuItem";
            this.regionFlagsToolStripMenuItem.Size = new System.Drawing.Size(198, 30);
            this.regionFlagsToolStripMenuItem.Text = "Region Flags";
            this.regionFlagsToolStripMenuItem.Click += new System.EventHandler(this.regionFlagsToolStripMenuItem_Click);
            // 
            // mediaFlagsToolStripMenuItem
            // 
            this.mediaFlagsToolStripMenuItem.Name = "mediaFlagsToolStripMenuItem";
            this.mediaFlagsToolStripMenuItem.Size = new System.Drawing.Size(198, 30);
            this.mediaFlagsToolStripMenuItem.Text = "Media Flags";
            this.mediaFlagsToolStripMenuItem.Click += new System.EventHandler(this.mediaFlagsToolStripMenuItem_Click);
            // 
            // imageFlagsToolStripMenuItem
            // 
            this.imageFlagsToolStripMenuItem.Name = "imageFlagsToolStripMenuItem";
            this.imageFlagsToolStripMenuItem.Size = new System.Drawing.Size(198, 30);
            this.imageFlagsToolStripMenuItem.Text = "Image Flags";
            this.imageFlagsToolStripMenuItem.Click += new System.EventHandler(this.imageFlagsToolStripMenuItem_Click);
            // 
            // otherToolStripMenuItem
            // 
            this.otherToolStripMenuItem.Name = "otherToolStripMenuItem";
            this.otherToolStripMenuItem.Size = new System.Drawing.Size(198, 30);
            this.otherToolStripMenuItem.Text = "Other";
            this.otherToolStripMenuItem.Click += new System.EventHandler(this.otherToolStripMenuItem_Click);
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemFlagsToolStripMenuItem,
            this.moduleFlagsToolStripMenuItem1});
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(236, 30);
            this.systemToolStripMenuItem.Text = "System";
            // 
            // systemFlagsToolStripMenuItem
            // 
            this.systemFlagsToolStripMenuItem.Name = "systemFlagsToolStripMenuItem";
            this.systemFlagsToolStripMenuItem.Size = new System.Drawing.Size(204, 30);
            this.systemFlagsToolStripMenuItem.Text = "System Flags";
            this.systemFlagsToolStripMenuItem.Click += new System.EventHandler(this.systemFlagsToolStripMenuItem_Click);
            // 
            // moduleFlagsToolStripMenuItem1
            // 
            this.moduleFlagsToolStripMenuItem1.Name = "moduleFlagsToolStripMenuItem1";
            this.moduleFlagsToolStripMenuItem1.Size = new System.Drawing.Size(204, 30);
            this.moduleFlagsToolStripMenuItem1.Text = "Module Flags";
            this.moduleFlagsToolStripMenuItem1.Click += new System.EventHandler(this.moduleFlagsToolStripMenuItem1_Click);
            // 
            // executionToolStripMenuItem
            // 
            this.executionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.infoToolStripMenuItem});
            this.executionToolStripMenuItem.Enabled = false;
            this.executionToolStripMenuItem.Name = "executionToolStripMenuItem";
            this.executionToolStripMenuItem.Size = new System.Drawing.Size(236, 30);
            this.executionToolStripMenuItem.Text = "Execution";
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            this.infoToolStripMenuItem.Size = new System.Drawing.Size(129, 30);
            this.infoToolStripMenuItem.Text = "Info";
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // ratingsToolStripMenuItem1
            // 
            this.ratingsToolStripMenuItem1.Enabled = false;
            this.ratingsToolStripMenuItem1.Name = "ratingsToolStripMenuItem1";
            this.ratingsToolStripMenuItem1.Size = new System.Drawing.Size(236, 30);
            this.ratingsToolStripMenuItem1.Text = "Ratings";
            this.ratingsToolStripMenuItem1.Click += new System.EventHandler(this.ratingsToolStripMenuItem1_Click);
            // 
            // extractToolStripMenuItem
            // 
            this.extractToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.executableToolStripMenuItem});
            this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            this.extractToolStripMenuItem.Size = new System.Drawing.Size(236, 30);
            this.extractToolStripMenuItem.Text = "Extract";
            // 
            // executableToolStripMenuItem
            // 
            this.executableToolStripMenuItem.Name = "executableToolStripMenuItem";
            this.executableToolStripMenuItem.Size = new System.Drawing.Size(180, 30);
            this.executableToolStripMenuItem.Text = "Executable";
            this.executableToolStripMenuItem.Click += new System.EventHandler(this.executableToolStripMenuItem_Click);
            // 
            // alternativeToolStripMenuItem
            // 
            this.alternativeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.titleIDsToolStripMenuItem});
            this.alternativeToolStripMenuItem.Enabled = false;
            this.alternativeToolStripMenuItem.Name = "alternativeToolStripMenuItem";
            this.alternativeToolStripMenuItem.Size = new System.Drawing.Size(236, 30);
            this.alternativeToolStripMenuItem.Text = "Alternative";
            // 
            // titleIDsToolStripMenuItem
            // 
            this.titleIDsToolStripMenuItem.Name = "titleIDsToolStripMenuItem";
            this.titleIDsToolStripMenuItem.Size = new System.Drawing.Size(160, 30);
            this.titleIDsToolStripMenuItem.Text = "Title IDs";
            this.titleIDsToolStripMenuItem.Click += new System.EventHandler(this.titleIDsToolStripMenuItem_Click);
            // 
            // optionalHeadersToolStripMenuItem
            // 
            this.optionalHeadersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemToolStripMenuItem1,
            this.defaultsToolStripMenuItem,
            this.gameToolStripMenuItem,
            this.originalPENameToolStripMenuItem});
            this.optionalHeadersToolStripMenuItem.Name = "optionalHeadersToolStripMenuItem";
            this.optionalHeadersToolStripMenuItem.Size = new System.Drawing.Size(236, 30);
            this.optionalHeadersToolStripMenuItem.Text = "Optional Headers";
            // 
            // systemToolStripMenuItem1
            // 
            this.systemToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lanKeyToolStripMenuItem1,
            this.imageBaseAddressToolStripMenuItem1,
            this.originalBaseAddressToolStripMenuItem1,
            this.entryPointToolStripMenuItem1,
            this.titleWorkspaceSizeToolStripMenuItem,
            this.deviceIDToolStripMenuItem1,
            this.boundingPathToolStripMenuItem1});
            this.systemToolStripMenuItem1.Name = "systemToolStripMenuItem1";
            this.systemToolStripMenuItem1.Size = new System.Drawing.Size(235, 30);
            this.systemToolStripMenuItem1.Text = "System";
            // 
            // lanKeyToolStripMenuItem1
            // 
            this.lanKeyToolStripMenuItem1.Enabled = false;
            this.lanKeyToolStripMenuItem1.Name = "lanKeyToolStripMenuItem1";
            this.lanKeyToolStripMenuItem1.Size = new System.Drawing.Size(271, 30);
            this.lanKeyToolStripMenuItem1.Text = "Lan Key";
            this.lanKeyToolStripMenuItem1.Click += new System.EventHandler(this.lanKeyToolStripMenuItem1_Click);
            // 
            // imageBaseAddressToolStripMenuItem1
            // 
            this.imageBaseAddressToolStripMenuItem1.Enabled = false;
            this.imageBaseAddressToolStripMenuItem1.Name = "imageBaseAddressToolStripMenuItem1";
            this.imageBaseAddressToolStripMenuItem1.Size = new System.Drawing.Size(271, 30);
            this.imageBaseAddressToolStripMenuItem1.Text = "Image Base Address";
            this.imageBaseAddressToolStripMenuItem1.Click += new System.EventHandler(this.imageBaseAddressToolStripMenuItem1_Click);
            // 
            // originalBaseAddressToolStripMenuItem1
            // 
            this.originalBaseAddressToolStripMenuItem1.Enabled = false;
            this.originalBaseAddressToolStripMenuItem1.Name = "originalBaseAddressToolStripMenuItem1";
            this.originalBaseAddressToolStripMenuItem1.Size = new System.Drawing.Size(271, 30);
            this.originalBaseAddressToolStripMenuItem1.Text = "Original base Address";
            this.originalBaseAddressToolStripMenuItem1.Click += new System.EventHandler(this.originalBaseAddressToolStripMenuItem1_Click);
            // 
            // entryPointToolStripMenuItem1
            // 
            this.entryPointToolStripMenuItem1.Enabled = false;
            this.entryPointToolStripMenuItem1.Name = "entryPointToolStripMenuItem1";
            this.entryPointToolStripMenuItem1.Size = new System.Drawing.Size(271, 30);
            this.entryPointToolStripMenuItem1.Text = "Entry Point";
            this.entryPointToolStripMenuItem1.Click += new System.EventHandler(this.entryPointToolStripMenuItem1_Click);
            // 
            // titleWorkspaceSizeToolStripMenuItem
            // 
            this.titleWorkspaceSizeToolStripMenuItem.Enabled = false;
            this.titleWorkspaceSizeToolStripMenuItem.Name = "titleWorkspaceSizeToolStripMenuItem";
            this.titleWorkspaceSizeToolStripMenuItem.Size = new System.Drawing.Size(271, 30);
            this.titleWorkspaceSizeToolStripMenuItem.Text = "Title Workspace Size";
            this.titleWorkspaceSizeToolStripMenuItem.Click += new System.EventHandler(this.titleWorkspaceSizeToolStripMenuItem_Click);
            // 
            // deviceIDToolStripMenuItem1
            // 
            this.deviceIDToolStripMenuItem1.Enabled = false;
            this.deviceIDToolStripMenuItem1.Name = "deviceIDToolStripMenuItem1";
            this.deviceIDToolStripMenuItem1.Size = new System.Drawing.Size(271, 30);
            this.deviceIDToolStripMenuItem1.Text = "Device ID";
            this.deviceIDToolStripMenuItem1.Click += new System.EventHandler(this.deviceIDToolStripMenuItem1_Click);
            // 
            // boundingPathToolStripMenuItem1
            // 
            this.boundingPathToolStripMenuItem1.Enabled = false;
            this.boundingPathToolStripMenuItem1.Name = "boundingPathToolStripMenuItem1";
            this.boundingPathToolStripMenuItem1.Size = new System.Drawing.Size(271, 30);
            this.boundingPathToolStripMenuItem1.Text = "Bounding Path";
            this.boundingPathToolStripMenuItem1.Click += new System.EventHandler(this.boundingPathToolStripMenuItem1_Click);
            // 
            // defaultsToolStripMenuItem
            // 
            this.defaultsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stackSizeToolStripMenuItem,
            this.filesystemCacheSizeToolStripMenuItem,
            this.heapSizeToolStripMenuItem});
            this.defaultsToolStripMenuItem.Name = "defaultsToolStripMenuItem";
            this.defaultsToolStripMenuItem.Size = new System.Drawing.Size(235, 30);
            this.defaultsToolStripMenuItem.Text = "Defaults";
            // 
            // stackSizeToolStripMenuItem
            // 
            this.stackSizeToolStripMenuItem.Enabled = false;
            this.stackSizeToolStripMenuItem.Name = "stackSizeToolStripMenuItem";
            this.stackSizeToolStripMenuItem.Size = new System.Drawing.Size(267, 30);
            this.stackSizeToolStripMenuItem.Text = "Stack Size";
            this.stackSizeToolStripMenuItem.Click += new System.EventHandler(this.stackSizeToolStripMenuItem_Click);
            // 
            // filesystemCacheSizeToolStripMenuItem
            // 
            this.filesystemCacheSizeToolStripMenuItem.Enabled = false;
            this.filesystemCacheSizeToolStripMenuItem.Name = "filesystemCacheSizeToolStripMenuItem";
            this.filesystemCacheSizeToolStripMenuItem.Size = new System.Drawing.Size(267, 30);
            this.filesystemCacheSizeToolStripMenuItem.Text = "Filesystem Cache Size";
            this.filesystemCacheSizeToolStripMenuItem.Click += new System.EventHandler(this.filesystemCacheSizeToolStripMenuItem_Click);
            // 
            // heapSizeToolStripMenuItem
            // 
            this.heapSizeToolStripMenuItem.Enabled = false;
            this.heapSizeToolStripMenuItem.Name = "heapSizeToolStripMenuItem";
            this.heapSizeToolStripMenuItem.Size = new System.Drawing.Size(267, 30);
            this.heapSizeToolStripMenuItem.Text = "Heap Size";
            this.heapSizeToolStripMenuItem.Click += new System.EventHandler(this.heapSizeToolStripMenuItem_Click);
            // 
            // gameToolStripMenuItem
            // 
            this.gameToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xGD3MediaIDToolStripMenuItem1});
            this.gameToolStripMenuItem.Name = "gameToolStripMenuItem";
            this.gameToolStripMenuItem.Size = new System.Drawing.Size(235, 30);
            this.gameToolStripMenuItem.Text = "Game";
            // 
            // xGD3MediaIDToolStripMenuItem1
            // 
            this.xGD3MediaIDToolStripMenuItem1.Enabled = false;
            this.xGD3MediaIDToolStripMenuItem1.Name = "xGD3MediaIDToolStripMenuItem1";
            this.xGD3MediaIDToolStripMenuItem1.Size = new System.Drawing.Size(220, 30);
            this.xGD3MediaIDToolStripMenuItem1.Text = "XGD3 Media ID";
            this.xGD3MediaIDToolStripMenuItem1.Click += new System.EventHandler(this.xGD3MediaIDToolStripMenuItem1_Click);
            // 
            // originalPENameToolStripMenuItem
            // 
            this.originalPENameToolStripMenuItem.Enabled = false;
            this.originalPENameToolStripMenuItem.Name = "originalPENameToolStripMenuItem";
            this.originalPENameToolStripMenuItem.Size = new System.Drawing.Size(235, 30);
            this.originalPENameToolStripMenuItem.Text = "Original PE Name";
            this.originalPENameToolStripMenuItem.Click += new System.EventHandler(this.originalPENameToolStripMenuItem_Click);
            // 
            // databaseToolStripMenuItem
            // 
            this.databaseToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToDatabaseToolStripMenuItem});
            this.databaseToolStripMenuItem.Enabled = false;
            this.databaseToolStripMenuItem.Name = "databaseToolStripMenuItem";
            this.databaseToolStripMenuItem.Size = new System.Drawing.Size(98, 29);
            this.databaseToolStripMenuItem.Text = "Database";
            // 
            // addToDatabaseToolStripMenuItem
            // 
            this.addToDatabaseToolStripMenuItem.Name = "addToDatabaseToolStripMenuItem";
            this.addToDatabaseToolStripMenuItem.Size = new System.Drawing.Size(225, 30);
            this.addToDatabaseToolStripMenuItem.Text = "Generate Report";
            this.addToDatabaseToolStripMenuItem.Click += new System.EventHandler(this.addToDatabaseToolStripMenuItem_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.Black;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.richTextBox1.ForeColor = System.Drawing.Color.LimeGreen;
            this.richTextBox1.Location = new System.Drawing.Point(0, 564);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(1260, 142);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 33);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Size = new System.Drawing.Size(1260, 531);
            this.splitContainer1.SplitterDistance = 263;
            this.splitContainer1.TabIndex = 2;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(263, 531);
            this.treeView1.TabIndex = 0;
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // MetaEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1260, 706);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MetaEditor";
            this.Text = "Xenious - Meta Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainEditor_FormClosing);
            this.Load += new System.EventHandler(this.MainEditor_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem certificateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regionFlagsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mediaFlagsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageFlagsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemFlagsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moduleFlagsToolStripMenuItem1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ToolStripMenuItem executionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem databaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ratingsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem executableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem otherToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alternativeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem titleIDsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rebuildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionalHeadersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem lanKeyToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem imageBaseAddressToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem originalBaseAddressToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem entryPointToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem defaultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stackSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem titleWorkspaceSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filesystemCacheSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem heapSizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deviceIDToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem boundingPathToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem gameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xGD3MediaIDToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem originalPENameToolStripMenuItem;
    }
}