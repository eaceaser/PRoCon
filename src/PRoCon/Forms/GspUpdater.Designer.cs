namespace PRoCon.Forms {
    partial class GspUpdater {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GspUpdater));
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.tmrUpdateChecker = new System.Windows.Forms.Timer(this.components);
            this.txtArguments = new System.Windows.Forms.TextBox();
            this.lblArguments = new System.Windows.Forms.Label();
            this.lblDownloadStatus = new System.Windows.Forms.Label();
            this.iglIcons = new System.Windows.Forms.ImageList(this.components);
            this.lblSearchInstalls = new System.Windows.Forms.Label();
            this.folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.txtBrowseFolder = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lsvInstalls = new PRoCon.Controls.ControlsEx.ListViewNF();
            this.colStatus = new System.Windows.Forms.ColumnHeader();
            this.colVersion = new System.Windows.Forms.ColumnHeader();
            this.colDirectory = new System.Windows.Forms.ColumnHeader();
            this.colPath = new System.Windows.Forms.ColumnHeader();
            this.ctxInstall = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.alToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxInstall.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(344, 252);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(87, 27);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(14, 251);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(87, 27);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(683, 251);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(87, 27);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // tmrUpdateChecker
            // 
            this.tmrUpdateChecker.Interval = 1000;
            this.tmrUpdateChecker.Tick += new System.EventHandler(this.tmrUpdateChecker_Tick);
            // 
            // txtArguments
            // 
            this.txtArguments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtArguments.Location = new System.Drawing.Point(182, 255);
            this.txtArguments.Name = "txtArguments";
            this.txtArguments.Size = new System.Drawing.Size(156, 23);
            this.txtArguments.TabIndex = 6;
            this.txtArguments.Text = "-name \"PRoCon: %directory%\" -console 1";
            // 
            // lblArguments
            // 
            this.lblArguments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblArguments.AutoSize = true;
            this.lblArguments.Location = new System.Drawing.Point(107, 257);
            this.lblArguments.Name = "lblArguments";
            this.lblArguments.Size = new System.Drawing.Size(69, 15);
            this.lblArguments.TabIndex = 7;
            this.lblArguments.Text = "Arguments:";
            // 
            // lblDownloadStatus
            // 
            this.lblDownloadStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDownloadStatus.Location = new System.Drawing.Point(437, 257);
            this.lblDownloadStatus.Name = "lblDownloadStatus";
            this.lblDownloadStatus.Size = new System.Drawing.Size(238, 21);
            this.lblDownloadStatus.TabIndex = 8;
            this.lblDownloadStatus.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // iglIcons
            // 
            this.iglIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iglIcons.ImageStream")));
            this.iglIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.iglIcons.Images.SetKeyName(0, "running.png");
            this.iglIcons.Images.SetKeyName(1, "stopped.png");
            // 
            // lblSearchInstalls
            // 
            this.lblSearchInstalls.AutoSize = true;
            this.lblSearchInstalls.Location = new System.Drawing.Point(11, 12);
            this.lblSearchInstalls.Name = "lblSearchInstalls";
            this.lblSearchInstalls.Size = new System.Drawing.Size(115, 15);
            this.lblSearchInstalls.TabIndex = 9;
            this.lblSearchInstalls.Text = "Search for installs at:";
            // 
            // txtBrowseFolder
            // 
            this.txtBrowseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBrowseFolder.BackColor = System.Drawing.SystemColors.Window;
            this.txtBrowseFolder.Location = new System.Drawing.Point(133, 9);
            this.txtBrowseFolder.Name = "txtBrowseFolder";
            this.txtBrowseFolder.ReadOnly = true;
            this.txtBrowseFolder.Size = new System.Drawing.Size(544, 23);
            this.txtBrowseFolder.TabIndex = 10;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(683, 6);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(87, 26);
            this.btnBrowse.TabIndex = 11;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lsvInstalls
            // 
            this.lsvInstalls.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvInstalls.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colStatus,
            this.colVersion,
            this.colDirectory,
            this.colPath});
            this.lsvInstalls.FullRowSelect = true;
            this.lsvInstalls.GridLines = true;
            this.lsvInstalls.HideSelection = false;
            this.lsvInstalls.Location = new System.Drawing.Point(14, 38);
            this.lsvInstalls.Name = "lsvInstalls";
            this.lsvInstalls.Size = new System.Drawing.Size(756, 206);
            this.lsvInstalls.SmallImageList = this.iglIcons;
            this.lsvInstalls.TabIndex = 0;
            this.lsvInstalls.UseCompatibleStateImageBehavior = false;
            this.lsvInstalls.View = System.Windows.Forms.View.Details;
            this.lsvInstalls.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lsvInstalls_MouseClick);
            this.lsvInstalls.SelectedIndexChanged += new System.EventHandler(this.lstInstalls_SelectedIndexChanged);
            this.lsvInstalls.DoubleClick += new System.EventHandler(this.lsvInstalls_DoubleClick);
            this.lsvInstalls.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lsvInstalls_ColumnClick);
            // 
            // colStatus
            // 
            this.colStatus.Text = "Status";
            // 
            // colVersion
            // 
            this.colVersion.Text = "Version";
            // 
            // colDirectory
            // 
            this.colDirectory.Text = "Directory";
            // 
            // colPath
            // 
            this.colPath.Text = "Path";
            // 
            // ctxInstall
            // 
            this.ctxInstall.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolderToolStripMenuItem,
            this.toolStripMenuItem1,
            this.selectToolStripMenuItem,
            this.alToolStripMenuItem});
            this.ctxInstall.Name = "ctxInstall";
            this.ctxInstall.Size = new System.Drawing.Size(153, 98);
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openFolderToolStripMenuItem.Text = "Open Folder...";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.selectToolStripMenuItem.Text = "Select All";
            this.selectToolStripMenuItem.Click += new System.EventHandler(this.selectToolStripMenuItem_Click);
            // 
            // alToolStripMenuItem
            // 
            this.alToolStripMenuItem.Name = "alToolStripMenuItem";
            this.alToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.alToolStripMenuItem.Text = "Select None";
            this.alToolStripMenuItem.Click += new System.EventHandler(this.alToolStripMenuItem_Click);
            // 
            // GspUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 292);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtBrowseFolder);
            this.Controls.Add(this.lblSearchInstalls);
            this.Controls.Add(this.lblDownloadStatus);
            this.Controls.Add(this.lblArguments);
            this.Controls.Add(this.txtArguments);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lsvInstalls);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 200);
            this.Name = "GspUpdater";
            this.Text = "GSP PRoCon Updater";
            this.Load += new System.EventHandler(this.GspUpdater_Load);
            this.Activated += new System.EventHandler(this.GspUpdater_Activated);
            this.ctxInstall.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PRoCon.Controls.ControlsEx.ListViewNF lsvInstalls;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ColumnHeader colVersion;
        private System.Windows.Forms.ColumnHeader colDirectory;
        private System.Windows.Forms.ColumnHeader colPath;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Timer tmrUpdateChecker;
        private System.Windows.Forms.TextBox txtArguments;
        private System.Windows.Forms.Label lblArguments;
        private System.Windows.Forms.Label lblDownloadStatus;
        private System.Windows.Forms.ImageList iglIcons;
        private System.Windows.Forms.Label lblSearchInstalls;
        private System.Windows.Forms.FolderBrowserDialog folderBrowser;
        private System.Windows.Forms.TextBox txtBrowseFolder;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.ContextMenuStrip ctxInstall;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem alToolStripMenuItem;
    }
}