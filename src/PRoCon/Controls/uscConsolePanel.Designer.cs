namespace PRoCon {
    partial class uscConsolePanel {
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.tbcConsoles = new System.Windows.Forms.TabControl();
            this.tabConsole = new System.Windows.Forms.TabPage();
            this.chkEnableOutput = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.chkEvents = new System.Windows.Forms.CheckBox();
            this.lblOutputKiBs = new System.Windows.Forms.Label();
            this.btnConsoleSend = new System.Windows.Forms.Button();
            this.txtConsoleCommand = new System.Windows.Forms.TextBox();
            this.pnlConsoleEnclosure = new System.Windows.Forms.Panel();
            this.tabPunkbuster = new System.Windows.Forms.TabPage();
            this.chkEnablePunkbusterOutput = new System.Windows.Forms.CheckBox();
            this.btnPunkbusterSend = new System.Windows.Forms.Button();
            this.txtPunkbusterCommand = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkConEnableScrolling = new System.Windows.Forms.CheckBox();
            this.rtbConsoleBox = new PRoCon.CodRichTextBox();
            this.rtbPunkbusterBox = new PRoCon.CodRichTextBox();
            this.chkPBEnableScrolling = new System.Windows.Forms.CheckBox();
            this.tbcConsoles.SuspendLayout();
            this.tabConsole.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlConsoleEnclosure.SuspendLayout();
            this.tabPunkbuster.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbcConsoles
            // 
            this.tbcConsoles.Controls.Add(this.tabConsole);
            this.tbcConsoles.Controls.Add(this.tabPunkbuster);
            this.tbcConsoles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcConsoles.Location = new System.Drawing.Point(0, 0);
            this.tbcConsoles.Name = "tbcConsoles";
            this.tbcConsoles.SelectedIndex = 0;
            this.tbcConsoles.Size = new System.Drawing.Size(766, 567);
            this.tbcConsoles.TabIndex = 0;
            this.tbcConsoles.SelectedIndexChanged += new System.EventHandler(this.tbcConsoles_SelectedIndexChanged);
            // 
            // tabConsole
            // 
            this.tabConsole.Controls.Add(this.chkConEnableScrolling);
            this.tabConsole.Controls.Add(this.chkEnableOutput);
            this.tabConsole.Controls.Add(this.flowLayoutPanel1);
            this.tabConsole.Controls.Add(this.btnConsoleSend);
            this.tabConsole.Controls.Add(this.txtConsoleCommand);
            this.tabConsole.Controls.Add(this.pnlConsoleEnclosure);
            this.tabConsole.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabConsole.Location = new System.Drawing.Point(4, 24);
            this.tabConsole.Name = "tabConsole";
            this.tabConsole.Padding = new System.Windows.Forms.Padding(8);
            this.tabConsole.Size = new System.Drawing.Size(758, 539);
            this.tabConsole.TabIndex = 0;
            this.tabConsole.Text = "Console";
            this.tabConsole.UseVisualStyleBackColor = true;
            // 
            // chkEnableOutput
            // 
            this.chkEnableOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkEnableOutput.AutoSize = true;
            this.chkEnableOutput.Checked = true;
            this.chkEnableOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnableOutput.Location = new System.Drawing.Point(11, 480);
            this.chkEnableOutput.Name = "chkEnableOutput";
            this.chkEnableOutput.Size = new System.Drawing.Size(100, 19);
            this.chkEnableOutput.TabIndex = 27;
            this.chkEnableOutput.Text = "Enable output";
            this.chkEnableOutput.UseVisualStyleBackColor = true;
            this.chkEnableOutput.CheckedChanged += new System.EventHandler(this.chkEnableOutput_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.chkDebug);
            this.flowLayoutPanel1.Controls.Add(this.chkEvents);
            this.flowLayoutPanel1.Controls.Add(this.lblOutputKiBs);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(363, 476);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(384, 24);
            this.flowLayoutPanel1.TabIndex = 29;
            // 
            // chkDebug
            // 
            this.chkDebug.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkDebug.AutoSize = true;
            this.chkDebug.Location = new System.Drawing.Point(320, 3);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(61, 19);
            this.chkDebug.TabIndex = 25;
            this.chkDebug.Text = "Debug";
            this.chkDebug.UseVisualStyleBackColor = true;
            this.chkDebug.CheckedChanged += new System.EventHandler(this.chkDebug_CheckedChanged);
            // 
            // chkEvents
            // 
            this.chkEvents.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkEvents.AutoSize = true;
            this.chkEvents.Location = new System.Drawing.Point(254, 3);
            this.chkEvents.Name = "chkEvents";
            this.chkEvents.Size = new System.Drawing.Size(60, 19);
            this.chkEvents.TabIndex = 26;
            this.chkEvents.Text = "Events";
            this.chkEvents.UseVisualStyleBackColor = true;
            this.chkEvents.CheckedChanged += new System.EventHandler(this.chkEvents_CheckedChanged);
            // 
            // lblOutputKiBs
            // 
            this.lblOutputKiBs.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblOutputKiBs.AutoSize = true;
            this.lblOutputKiBs.Location = new System.Drawing.Point(205, 5);
            this.lblOutputKiBs.Name = "lblOutputKiBs";
            this.lblOutputKiBs.Size = new System.Drawing.Size(43, 15);
            this.lblOutputKiBs.TabIndex = 28;
            this.lblOutputKiBs.Text = "            ";
            this.lblOutputKiBs.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnConsoleSend
            // 
            this.btnConsoleSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConsoleSend.Location = new System.Drawing.Point(656, 505);
            this.btnConsoleSend.Name = "btnConsoleSend";
            this.btnConsoleSend.Size = new System.Drawing.Size(90, 23);
            this.btnConsoleSend.TabIndex = 24;
            this.btnConsoleSend.Text = "Send";
            this.btnConsoleSend.UseVisualStyleBackColor = true;
            this.btnConsoleSend.Click += new System.EventHandler(this.btnConsoleSend_Click);
            // 
            // txtConsoleCommand
            // 
            this.txtConsoleCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConsoleCommand.AutoCompleteCustomSource.AddRange(new string[] {
            "punkBuster.pb_sv_command ",
            "punkBuster.pb_sv_command pb_sv_plist",
            "punkBuster.pb_sv_command pb_sv_banlist",
            "punkBuster.pb_sv_command pb_sv_banguid",
            "admin.yell ",
            "login.plainText ",
            "login.hashed ",
            "logout",
            "quit",
            "version",
            "help",
            "admin.runScript ",
            "serverInfo",
            "admin.runNextLevel",
            "admin.currentLevel",
            "admin.nextLevel ",
            "admin.restartMap",
            "admin.supportedMaps ",
            "admin.setPlaylist ",
            "admin.getPlaylist ",
            "admin.getPlaylists",
            "admin.kickPlayer ",
            "admin.listPlayers ",
            "admin.listPlayers all",
            "admin.listPlayers team",
            "admin.listPlayers squad",
            "admin.listPlayers player",
            "admin.banPlayer ",
            "admin.banIP ",
            "admin.unbanPlayer ",
            "admin.unbanIP ",
            "admin.clearPlayerBanList",
            "admin.clearIPBanList",
            "admin.listPlayerBans",
            "admin.listIPBans",
            "reservedSlots.configFile ",
            "reservedSlots.load",
            "reservedSlots.save",
            "reservedSlots.addPlayer ",
            "reservedSlots.removePlayer ",
            "reservedSlots.clear",
            "reservedSlots.list",
            "reservedTagSlots.load",
            "reservedTagSlots.save",
            "reservedTagSlots.setTag ",
            "reservedTagSlots.addPlayer ",
            "reservedTagSlots.removePlayer ",
            "reservedTagSlots.clear",
            "reservedTagSlots.list",
            "mapList.configFile ",
            "mapList.load",
            "mapList.save",
            "mapList.list",
            "mapList.clear",
            "mapList.remove ",
            "mapList.append ",
            "mapList.nextLevelIndex ",
            "vars.adminPassword",
            "vars.gamePassword",
            "vars.punkBuster",
            "vars.hardCore",
            "vars.ranked",
            "vars.rankLimit",
            "vars.teamBalance",
            "vars.friendlyFire",
            "vars.currentPlayerLimit",
            "vars.maxPlayerLimit",
            "vars.playerLimit",
            "vars.bannerUrl",
            "vars.serverDescription",
            "vars.killCam",
            "vars.miniMap",
            "vars.crossHair",
            "vars.3dSpotting",
            "vars.miniMapSpotting",
            "vars.thirdPersonVehicleCameras"});
            this.txtConsoleCommand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.txtConsoleCommand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtConsoleCommand.Location = new System.Drawing.Point(11, 506);
            this.txtConsoleCommand.Name = "txtConsoleCommand";
            this.txtConsoleCommand.Size = new System.Drawing.Size(639, 23);
            this.txtConsoleCommand.TabIndex = 23;
            this.txtConsoleCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtConsoleCommand_KeyDown);
            // 
            // pnlConsoleEnclosure
            // 
            this.pnlConsoleEnclosure.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConsoleEnclosure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlConsoleEnclosure.Controls.Add(this.rtbConsoleBox);
            this.pnlConsoleEnclosure.Location = new System.Drawing.Point(11, 14);
            this.pnlConsoleEnclosure.Name = "pnlConsoleEnclosure";
            this.pnlConsoleEnclosure.Size = new System.Drawing.Size(736, 460);
            this.pnlConsoleEnclosure.TabIndex = 22;
            // 
            // tabPunkbuster
            // 
            this.tabPunkbuster.Controls.Add(this.chkPBEnableScrolling);
            this.tabPunkbuster.Controls.Add(this.chkEnablePunkbusterOutput);
            this.tabPunkbuster.Controls.Add(this.btnPunkbusterSend);
            this.tabPunkbuster.Controls.Add(this.txtPunkbusterCommand);
            this.tabPunkbuster.Controls.Add(this.panel1);
            this.tabPunkbuster.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabPunkbuster.Location = new System.Drawing.Point(4, 24);
            this.tabPunkbuster.Name = "tabPunkbuster";
            this.tabPunkbuster.Padding = new System.Windows.Forms.Padding(3);
            this.tabPunkbuster.Size = new System.Drawing.Size(758, 539);
            this.tabPunkbuster.TabIndex = 1;
            this.tabPunkbuster.Text = "PunkBuster";
            this.tabPunkbuster.UseVisualStyleBackColor = true;
            // 
            // chkEnablePunkbusterOutput
            // 
            this.chkEnablePunkbusterOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkEnablePunkbusterOutput.AutoSize = true;
            this.chkEnablePunkbusterOutput.Checked = true;
            this.chkEnablePunkbusterOutput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnablePunkbusterOutput.Location = new System.Drawing.Point(11, 482);
            this.chkEnablePunkbusterOutput.Name = "chkEnablePunkbusterOutput";
            this.chkEnablePunkbusterOutput.Size = new System.Drawing.Size(100, 19);
            this.chkEnablePunkbusterOutput.TabIndex = 33;
            this.chkEnablePunkbusterOutput.Text = "Enable output";
            this.chkEnablePunkbusterOutput.UseVisualStyleBackColor = true;
            this.chkEnablePunkbusterOutput.CheckedChanged += new System.EventHandler(this.chkEnablePunkbusterOutput_CheckedChanged);
            // 
            // btnPunkbusterSend
            // 
            this.btnPunkbusterSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPunkbusterSend.Location = new System.Drawing.Point(656, 507);
            this.btnPunkbusterSend.Name = "btnPunkbusterSend";
            this.btnPunkbusterSend.Size = new System.Drawing.Size(90, 23);
            this.btnPunkbusterSend.TabIndex = 30;
            this.btnPunkbusterSend.Text = "Send";
            this.btnPunkbusterSend.UseVisualStyleBackColor = true;
            this.btnPunkbusterSend.Click += new System.EventHandler(this.btnPunkbusterSend_Click);
            // 
            // txtPunkbusterCommand
            // 
            this.txtPunkbusterCommand.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPunkbusterCommand.AutoCompleteCustomSource.AddRange(new string[] {
            "pb_sv_autoupdban ",
            "pb_sv_badname ",
            "pb_sv_badnamedel ",
            "pb_sv_badnamelist ",
            "pb_sv_ban ",
            "pb_sv_banempty ",
            "pb_sv_banguid ",
            "pb_sv_banlist ",
            "pb_sv_banload ",
            "pb_sv_banmask ",
            "pb_sv_bannameempty ",
            "pb_sv_bindsrch ",
            "pb_sv_cvar ",
            "pb_sv_cvarchanged ",
            "pb_sv_cvardel ",
            "pb_sv_cvarempty ",
            "pb_sv_cvarlist ",
            "pb_sv_cvarsrch ",
            "pb_sv_cvaruser ",
            "pb_sv_cvarval ",
            "pb_sv_disable ",
            "pb_sv_enable ",
            "pb_sv_file ",
            "pb_sv_filedel ",
            "pb_sv_fileempty ",
            "pb_sv_filelist ",
            "pb_sv_getss ",
            "pb_sv_homepath ",
            "pb_sv_ipguard ",
            "pb_sv_kick ",
            "pb_sv_load ",
            "pb_sv_md5tool ",
            "pb_sv_md5tooldel ",
            "pb_sv_md5toolempty ",
            "pb_sv_md5toollist ",
            "pb_sv_namelock ",
            "pb_sv_namelockempty ",
            "pb_sv_namelocklist ",
            "pb_sv_newlog ",
            "pb_sv_plist ",
            "pb_sv_powerguid ",
            "pb_sv_powerpoints ",
            "pb_sv_protectname ",
            "pb_sv_protecttag ",
            "pb_sv_reban ",
            "pb_sv_restart ",
            "pb_sv_tlist ",
            "pb_sv_task ",
            "pb_sv_taskdel ",
            "pb_sv_taskempty ",
            "pb_sv_unban ",
            "pb_sv_unbanguid ",
            "pb_sv_updbanfile ",
            "pb_sv_update ",
            "pb_sv_ver ",
            "pb_sv_writecfg "});
            this.txtPunkbusterCommand.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.txtPunkbusterCommand.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtPunkbusterCommand.Location = new System.Drawing.Point(11, 508);
            this.txtPunkbusterCommand.Name = "txtPunkbusterCommand";
            this.txtPunkbusterCommand.Size = new System.Drawing.Size(638, 23);
            this.txtPunkbusterCommand.TabIndex = 29;
            this.txtPunkbusterCommand.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPunkbusterCommand_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.rtbPunkbusterBox);
            this.panel1.Location = new System.Drawing.Point(11, 14);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(736, 462);
            this.panel1.TabIndex = 28;
            // 
            // chkConEnableScrolling
            // 
            this.chkConEnableScrolling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkConEnableScrolling.AutoSize = true;
            this.chkConEnableScrolling.Checked = true;
            this.chkConEnableScrolling.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkConEnableScrolling.Location = new System.Drawing.Point(188, 480);
            this.chkConEnableScrolling.Name = "chkConEnableScrolling";
            this.chkConEnableScrolling.Size = new System.Drawing.Size(109, 19);
            this.chkConEnableScrolling.TabIndex = 30;
            this.chkConEnableScrolling.Text = "Enable scrolling";
            this.chkConEnableScrolling.UseVisualStyleBackColor = true;
            this.chkConEnableScrolling.CheckedChanged += new System.EventHandler(this.chkConEnableScrolling_CheckedChanged);
            // 
            // rtbConsoleBox
            // 
            this.rtbConsoleBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbConsoleBox.DetectUrls = false;
            this.rtbConsoleBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbConsoleBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbConsoleBox.Location = new System.Drawing.Point(0, 0);
            this.rtbConsoleBox.Name = "rtbConsoleBox";
            this.rtbConsoleBox.ReadOnly = true;
            this.rtbConsoleBox.Size = new System.Drawing.Size(734, 458);
            this.rtbConsoleBox.TabIndex = 0;
            this.rtbConsoleBox.Text = "";
            // 
            // rtbPunkbusterBox
            // 
            this.rtbPunkbusterBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbPunkbusterBox.DetectUrls = false;
            this.rtbPunkbusterBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbPunkbusterBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbPunkbusterBox.ForeColor = System.Drawing.Color.RoyalBlue;
            this.rtbPunkbusterBox.Location = new System.Drawing.Point(0, 0);
            this.rtbPunkbusterBox.Name = "rtbPunkbusterBox";
            this.rtbPunkbusterBox.ReadOnly = true;
            this.rtbPunkbusterBox.Size = new System.Drawing.Size(734, 460);
            this.rtbPunkbusterBox.TabIndex = 0;
            this.rtbPunkbusterBox.Text = "";
            // 
            // chkPBEnableScrolling
            // 
            this.chkPBEnableScrolling.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkPBEnableScrolling.AutoSize = true;
            this.chkPBEnableScrolling.Checked = true;
            this.chkPBEnableScrolling.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPBEnableScrolling.Location = new System.Drawing.Point(188, 482);
            this.chkPBEnableScrolling.Name = "chkPBEnableScrolling";
            this.chkPBEnableScrolling.Size = new System.Drawing.Size(109, 19);
            this.chkPBEnableScrolling.TabIndex = 34;
            this.chkPBEnableScrolling.Text = "Enable scrolling";
            this.chkPBEnableScrolling.UseVisualStyleBackColor = true;
            this.chkPBEnableScrolling.CheckedChanged += new System.EventHandler(this.chkPBEnableScrolling_CheckedChanged);
            // 
            // uscConsolePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbcConsoles);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "uscConsolePanel";
            this.Size = new System.Drawing.Size(766, 567);
            this.Load += new System.EventHandler(this.uscConsolePanel_Load);
            this.tbcConsoles.ResumeLayout(false);
            this.tabConsole.ResumeLayout(false);
            this.tabConsole.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.pnlConsoleEnclosure.ResumeLayout(false);
            this.tabPunkbuster.ResumeLayout(false);
            this.tabPunkbuster.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbcConsoles;
        private System.Windows.Forms.TabPage tabConsole;
        private System.Windows.Forms.CheckBox chkEnableOutput;
        private System.Windows.Forms.CheckBox chkEvents;
        private System.Windows.Forms.CheckBox chkDebug;
        private System.Windows.Forms.Button btnConsoleSend;
        private System.Windows.Forms.TextBox txtConsoleCommand;
        private System.Windows.Forms.Panel pnlConsoleEnclosure;
        private CodRichTextBox rtbConsoleBox;
        private System.Windows.Forms.TabPage tabPunkbuster;
        private System.Windows.Forms.CheckBox chkEnablePunkbusterOutput;
        private System.Windows.Forms.Button btnPunkbusterSend;
        private System.Windows.Forms.TextBox txtPunkbusterCommand;
        private System.Windows.Forms.Panel panel1;
        private CodRichTextBox rtbPunkbusterBox;
        private System.Windows.Forms.Label lblOutputKiBs;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.CheckBox chkConEnableScrolling;
        private System.Windows.Forms.CheckBox chkPBEnableScrolling;

    }
}
