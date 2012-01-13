namespace PRoCon.Forms {
    partial class frmAbout {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.okButton = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabAbout = new System.Windows.Forms.TabPage();
            this.lnkVisitForum = new System.Windows.Forms.LinkLabel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.tabThanks = new System.Windows.Forms.TabPage();
            this.lnkZboss = new System.Windows.Forms.LinkLabel();
            this.lnkCptNeeda = new System.Windows.Forms.LinkLabel();
            this.lnk1349 = new System.Windows.Forms.LinkLabel();
            this.lnkIntruder = new System.Windows.Forms.LinkLabel();
            this.lnkSinex = new System.Windows.Forms.LinkLabel();
            this.lnkTimmsy = new System.Windows.Forms.LinkLabel();
            this.tabCopyright = new System.Windows.Forms.TabPage();
            this.pnlCopyright = new System.Windows.Forms.Panel();
            this.lnlDotNetLibLibrary = new System.Windows.Forms.LinkLabel();
            this.lblDotNetZipLibrary = new System.Windows.Forms.Label();
            this.lnkMaxMind = new System.Windows.Forms.LinkLabel();
            this.picMaxMind = new System.Windows.Forms.PictureBox();
            this.tabSpacefishSteve = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.picSpacefishSteve = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tabControl.SuspendLayout();
            this.tabAbout.SuspendLayout();
            this.tabThanks.SuspendLayout();
            this.tabCopyright.SuspendLayout();
            this.pnlCopyright.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMaxMind)).BeginInit();
            this.tabSpacefishSteve.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSpacefishSteve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.okButton.Location = new System.Drawing.Point(485, 248);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(87, 27);
            this.okButton.TabIndex = 24;
            this.okButton.Text = "Close";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.tabAbout);
            this.tabControl.Controls.Add(this.tabThanks);
            this.tabControl.Controls.Add(this.tabCopyright);
            this.tabControl.Controls.Add(this.tabSpacefishSteve);
            this.tabControl.Location = new System.Drawing.Point(90, 14);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(482, 227);
            this.tabControl.TabIndex = 25;
            // 
            // tabAbout
            // 
            this.tabAbout.Controls.Add(this.lnkVisitForum);
            this.tabAbout.Controls.Add(this.lblVersion);
            this.tabAbout.Controls.Add(this.lblProductName);
            this.tabAbout.Location = new System.Drawing.Point(4, 24);
            this.tabAbout.Name = "tabAbout";
            this.tabAbout.Padding = new System.Windows.Forms.Padding(3);
            this.tabAbout.Size = new System.Drawing.Size(474, 199);
            this.tabAbout.TabIndex = 0;
            this.tabAbout.Text = "About";
            this.tabAbout.UseVisualStyleBackColor = true;
            // 
            // lnkVisitForum
            // 
            this.lnkVisitForum.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkVisitForum.LinkArea = new System.Windows.Forms.LinkArea(0, 48);
            this.lnkVisitForum.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkVisitForum.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkVisitForum.Location = new System.Drawing.Point(9, 97);
            this.lnkVisitForum.Name = "lnkVisitForum";
            this.lnkVisitForum.Size = new System.Drawing.Size(303, 68);
            this.lnkVisitForum.TabIndex = 2;
            this.lnkVisitForum.TabStop = true;
            this.lnkVisitForum.Text = "Visit the forums for support and bug submissions";
            this.lnkVisitForum.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Location = new System.Drawing.Point(9, 73);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(46, 15);
            this.lblVersion.TabIndex = 1;
            this.lblVersion.Text = "Version";
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblProductName.Location = new System.Drawing.Point(7, 28);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(212, 30);
            this.lblProductName.TabIndex = 0;
            this.lblProductName.Text = "PRoCon Frostbite";
            // 
            // tabThanks
            // 
            this.tabThanks.Controls.Add(this.lnkZboss);
            this.tabThanks.Controls.Add(this.lnkCptNeeda);
            this.tabThanks.Controls.Add(this.lnk1349);
            this.tabThanks.Controls.Add(this.lnkIntruder);
            this.tabThanks.Controls.Add(this.lnkSinex);
            this.tabThanks.Controls.Add(this.lnkTimmsy);
            this.tabThanks.Location = new System.Drawing.Point(4, 24);
            this.tabThanks.Name = "tabThanks";
            this.tabThanks.Padding = new System.Windows.Forms.Padding(3);
            this.tabThanks.Size = new System.Drawing.Size(474, 199);
            this.tabThanks.TabIndex = 1;
            this.tabThanks.Text = "Thanks";
            this.tabThanks.UseVisualStyleBackColor = true;
            // 
            // lnkZboss
            // 
            this.lnkZboss.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkZboss.LinkArea = new System.Windows.Forms.LinkArea(9, 8);
            this.lnkZboss.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkZboss.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkZboss.Location = new System.Drawing.Point(6, 157);
            this.lnkZboss.Name = "lnkZboss";
            this.lnkZboss.Size = new System.Drawing.Size(461, 39);
            this.lnkZboss.TabIndex = 10;
            this.lnkZboss.TabStop = true;
            this.lnkZboss.Text = "ZBoss at Z-Gaming for taking over the website and giving betterer words to everyp" +
                "eople!";
            this.lnkZboss.UseCompatibleTextRendering = true;
            this.lnkZboss.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkZboss_LinkClicked);
            // 
            // lnkCptNeeda
            // 
            this.lnkCptNeeda.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkCptNeeda.AutoSize = true;
            this.lnkCptNeeda.LinkArea = new System.Windows.Forms.LinkArea(13, 15);
            this.lnkCptNeeda.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkCptNeeda.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkCptNeeda.Location = new System.Drawing.Point(6, 38);
            this.lnkCptNeeda.Name = "lnkCptNeeda";
            this.lnkCptNeeda.Size = new System.Drawing.Size(303, 21);
            this.lnkCptNeeda.TabIndex = 9;
            this.lnkCptNeeda.TabStop = true;
            this.lnkCptNeeda.Text = "Cpt-Needa at Habitat4Hookers for lending me a server";
            this.lnkCptNeeda.UseCompatibleTextRendering = true;
            this.lnkCptNeeda.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCptNeeda_LinkClicked);
            // 
            // lnk1349
            // 
            this.lnk1349.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnk1349.AutoSize = true;
            this.lnk1349.LinkArea = new System.Windows.Forms.LinkArea(24, 13);
            this.lnk1349.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnk1349.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnk1349.Location = new System.Drawing.Point(6, 9);
            this.lnk1349.Name = "lnk1349";
            this.lnk1349.Size = new System.Drawing.Size(230, 21);
            this.lnk1349.TabIndex = 8;
            this.lnk1349.TabStop = true;
            this.lnk1349.Text = "-1349- for.. I dunno... being a muse?  Fag.";
            this.lnk1349.UseCompatibleTextRendering = true;
            this.lnk1349.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnk1349_LinkClicked);
            // 
            // lnkIntruder
            // 
            this.lnkIntruder.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkIntruder.AutoSize = true;
            this.lnkIntruder.LinkArea = new System.Windows.Forms.LinkArea(12, 11);
            this.lnkIntruder.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkIntruder.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkIntruder.Location = new System.Drawing.Point(6, 67);
            this.lnkIntruder.Name = "lnkIntruder";
            this.lnkIntruder.Size = new System.Drawing.Size(181, 21);
            this.lnkIntruder.TabIndex = 7;
            this.lnkIntruder.TabStop = true;
            this.lnkIntruder.Text = "Intruder at o) Solstice for testing";
            this.lnkIntruder.UseCompatibleTextRendering = true;
            this.lnkIntruder.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkIntruder_LinkClicked);
            // 
            // lnkSinex
            // 
            this.lnkSinex.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkSinex.LinkArea = new System.Windows.Forms.LinkArea(9, 13);
            this.lnkSinex.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkSinex.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkSinex.Location = new System.Drawing.Point(6, 96);
            this.lnkSinex.Name = "lnkSinex";
            this.lnkSinex.Size = new System.Drawing.Size(461, 24);
            this.lnkSinex.TabIndex = 6;
            this.lnkSinex.TabStop = true;
            this.lnkSinex.Text = "Sinex at (U3) Unreal 3 for images/icons, testing and a sweet over the top video =" +
                ")";
            this.lnkSinex.UseCompatibleTextRendering = true;
            this.lnkSinex.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSinex_LinkClicked);
            // 
            // lnkTimmsy
            // 
            this.lnkTimmsy.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkTimmsy.AutoSize = true;
            this.lnkTimmsy.LinkArea = new System.Windows.Forms.LinkArea(10, 13);
            this.lnkTimmsy.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkTimmsy.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkTimmsy.Location = new System.Drawing.Point(6, 128);
            this.lnkTimmsy.Name = "lnkTimmsy";
            this.lnkTimmsy.Size = new System.Drawing.Size(294, 21);
            this.lnkTimmsy.TabIndex = 5;
            this.lnkTimmsy.TabStop = true;
            this.lnkTimmsy.Text = "Timmsy at (U3) Unreal 3 for images/icons and testing";
            this.lnkTimmsy.UseCompatibleTextRendering = true;
            this.lnkTimmsy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTimmsy_LinkClicked);
            // 
            // tabCopyright
            // 
            this.tabCopyright.Controls.Add(this.pnlCopyright);
            this.tabCopyright.Location = new System.Drawing.Point(4, 24);
            this.tabCopyright.Name = "tabCopyright";
            this.tabCopyright.Padding = new System.Windows.Forms.Padding(3);
            this.tabCopyright.Size = new System.Drawing.Size(474, 199);
            this.tabCopyright.TabIndex = 2;
            this.tabCopyright.Text = "Copyright";
            this.tabCopyright.UseVisualStyleBackColor = true;
            // 
            // pnlCopyright
            // 
            this.pnlCopyright.AutoScroll = true;
            this.pnlCopyright.Controls.Add(this.lnlDotNetLibLibrary);
            this.pnlCopyright.Controls.Add(this.lblDotNetZipLibrary);
            this.pnlCopyright.Controls.Add(this.lnkMaxMind);
            this.pnlCopyright.Controls.Add(this.picMaxMind);
            this.pnlCopyright.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCopyright.Location = new System.Drawing.Point(3, 3);
            this.pnlCopyright.Name = "pnlCopyright";
            this.pnlCopyright.Padding = new System.Windows.Forms.Padding(5);
            this.pnlCopyright.Size = new System.Drawing.Size(468, 193);
            this.pnlCopyright.TabIndex = 0;
            // 
            // lnlDotNetLibLibrary
            // 
            this.lnlDotNetLibLibrary.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnlDotNetLibLibrary.LinkArea = new System.Windows.Forms.LinkArea(44, 14);
            this.lnlDotNetLibLibrary.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnlDotNetLibLibrary.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnlDotNetLibLibrary.Location = new System.Drawing.Point(8, 126);
            this.lnlDotNetLibLibrary.Name = "lnlDotNetLibLibrary";
            this.lnlDotNetLibLibrary.Size = new System.Drawing.Size(346, 36);
            this.lnlDotNetLibLibrary.TabIndex = 3;
            this.lnlDotNetLibLibrary.TabStop = true;
            this.lnlDotNetLibLibrary.Text = "PRoConUpdate utilises the DotNetLib Library available here.";
            this.lnlDotNetLibLibrary.UseCompatibleTextRendering = true;
            this.lnlDotNetLibLibrary.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked_1);
            // 
            // lblDotNetZipLibrary
            // 
            this.lblDotNetZipLibrary.AutoSize = true;
            this.lblDotNetZipLibrary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDotNetZipLibrary.Location = new System.Drawing.Point(5, 100);
            this.lblDotNetZipLibrary.Name = "lblDotNetZipLibrary";
            this.lblDotNetZipLibrary.Size = new System.Drawing.Size(140, 18);
            this.lblDotNetZipLibrary.TabIndex = 2;
            this.lblDotNetZipLibrary.Text = "DotNetZip Library";
            // 
            // lnkMaxMind
            // 
            this.lnkMaxMind.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkMaxMind.LinkArea = new System.Windows.Forms.LinkArea(70, 19);
            this.lnkMaxMind.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkMaxMind.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkMaxMind.Location = new System.Drawing.Point(8, 54);
            this.lnkMaxMind.Name = "lnkMaxMind";
            this.lnkMaxMind.Size = new System.Drawing.Size(346, 36);
            this.lnkMaxMind.TabIndex = 1;
            this.lnkMaxMind.TabStop = true;
            this.lnkMaxMind.Text = "This product includes GeoLite data created by MaxMind, available from http://maxm" +
                "ind.com/";
            this.lnkMaxMind.UseCompatibleTextRendering = true;
            this.lnkMaxMind.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMaxMind_LinkClicked);
            // 
            // picMaxMind
            // 
            this.picMaxMind.Image = ((System.Drawing.Image)(resources.GetObject("picMaxMind.Image")));
            this.picMaxMind.Location = new System.Drawing.Point(8, 16);
            this.picMaxMind.Name = "picMaxMind";
            this.picMaxMind.Size = new System.Drawing.Size(117, 35);
            this.picMaxMind.TabIndex = 0;
            this.picMaxMind.TabStop = false;
            // 
            // tabSpacefishSteve
            // 
            this.tabSpacefishSteve.Controls.Add(this.panel1);
            this.tabSpacefishSteve.Location = new System.Drawing.Point(4, 24);
            this.tabSpacefishSteve.Name = "tabSpacefishSteve";
            this.tabSpacefishSteve.Padding = new System.Windows.Forms.Padding(3);
            this.tabSpacefishSteve.Size = new System.Drawing.Size(474, 199);
            this.tabSpacefishSteve.TabIndex = 3;
            this.tabSpacefishSteve.Text = "Spacefish Steve";
            this.tabSpacefishSteve.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.picSpacefishSteve);
            this.panel1.Location = new System.Drawing.Point(59, 22);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(356, 155);
            this.panel1.TabIndex = 1;
            // 
            // picSpacefishSteve
            // 
            this.picSpacefishSteve.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSpacefishSteve.Image = ((System.Drawing.Image)(resources.GetObject("picSpacefishSteve.Image")));
            this.picSpacefishSteve.Location = new System.Drawing.Point(80, 11);
            this.picSpacefishSteve.Name = "picSpacefishSteve";
            this.picSpacefishSteve.Size = new System.Drawing.Size(202, 134);
            this.picSpacefishSteve.TabIndex = 1;
            this.picSpacefishSteve.TabStop = false;
            this.picSpacefishSteve.Click += new System.EventHandler(this.picSpacefishSteve_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(13, 38);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(71, 200);
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(17, 20);
            this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabel1.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.linkLabel1.Location = new System.Drawing.Point(94, 254);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(223, 21);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Copyright © 2010 Geoff \'Phogue\' Green";
            this.linkLabel1.UseCompatibleTextRendering = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // frmAbout
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 289);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tabControl);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmAbout";
            this.tabControl.ResumeLayout(false);
            this.tabAbout.ResumeLayout(false);
            this.tabAbout.PerformLayout();
            this.tabThanks.ResumeLayout(false);
            this.tabThanks.PerformLayout();
            this.tabCopyright.ResumeLayout(false);
            this.pnlCopyright.ResumeLayout(false);
            this.pnlCopyright.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMaxMind)).EndInit();
            this.tabSpacefishSteve.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picSpacefishSteve)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.LinkLabel lnkVisitForum;
        private System.Windows.Forms.TabPage tabThanks;
        private System.Windows.Forms.TabPage tabCopyright;
        private System.Windows.Forms.Panel pnlCopyright;
        private System.Windows.Forms.LinkLabel lnkMaxMind;
        private System.Windows.Forms.PictureBox picMaxMind;
        private System.Windows.Forms.TabPage tabSpacefishSteve;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox picSpacefishSteve;
        private System.Windows.Forms.LinkLabel lnlDotNetLibLibrary;
        private System.Windows.Forms.Label lblDotNetZipLibrary;
        private System.Windows.Forms.LinkLabel lnkCptNeeda;
        private System.Windows.Forms.LinkLabel lnk1349;
        private System.Windows.Forms.LinkLabel lnkIntruder;
        private System.Windows.Forms.LinkLabel lnkSinex;
        private System.Windows.Forms.LinkLabel lnkTimmsy;
        private System.Windows.Forms.LinkLabel lnkZboss;
    }
}
