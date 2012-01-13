namespace PRoCon {
    partial class uscListControlPanel {
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Name", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("IpAddress", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Guid", System.Windows.Forms.HorizontalAlignment.Left);
            this.tbcLists = new System.Windows.Forms.TabControl();
            this.tabBanlist = new System.Windows.Forms.TabPage();
            this.spltBanlistManualBans = new System.Windows.Forms.SplitContainer();
            this.btnBanlistRefresh = new System.Windows.Forms.Button();
            this.picCloseOpenManualBans = new System.Windows.Forms.PictureBox();
            this.lnkCloseOpenManualBans = new System.Windows.Forms.LinkLabel();
            this.picClearLists = new System.Windows.Forms.PictureBox();
            this.picUnbanPlayer = new System.Windows.Forms.PictureBox();
            this.btnBanlistClearBanlist = new System.Windows.Forms.Button();
            this.btnBanlistUnban = new System.Windows.Forms.Button();
            this.lsvBanlist = new PRoCon.Controls.ControlsEx.ListViewNF();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGUID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTimeRemaining = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colReason = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.rdoBanlistBc2GUID = new System.Windows.Forms.RadioButton();
            this.lblBanlistConfirmation = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.rdoBanlistTemporary = new System.Windows.Forms.RadioButton();
            this.rdoBanlistPermanent = new System.Windows.Forms.RadioButton();
            this.pnlBanlistTime = new System.Windows.Forms.Panel();
            this.lblBanlistTime = new System.Windows.Forms.Label();
            this.txtBanlistTime = new System.Windows.Forms.TextBox();
            this.cboBanlistTimeMultiplier = new System.Windows.Forms.ComboBox();
            this.picBanlistManualBanOkay = new System.Windows.Forms.PictureBox();
            this.picBanlistIPError = new System.Windows.Forms.PictureBox();
            this.picBanlistGUIDError = new System.Windows.Forms.PictureBox();
            this.txtBanlistManualBanIP = new System.Windows.Forms.TextBox();
            this.txtBanlistManualBanGUID = new System.Windows.Forms.TextBox();
            this.lblBanlistReason = new System.Windows.Forms.Label();
            this.cboBanlistReason = new System.Windows.Forms.ComboBox();
            this.btnBanlistAddBan = new System.Windows.Forms.Button();
            this.rdoBanlistPbGUID = new System.Windows.Forms.RadioButton();
            this.rdoBanlistIP = new System.Windows.Forms.RadioButton();
            this.rdoBanlistName = new System.Windows.Forms.RadioButton();
            this.txtBanlistManualBanName = new System.Windows.Forms.TextBox();
            this.tabMaplist = new System.Windows.Forms.TabPage();
            this.uscMaplist1 = new PRoCon.Controls.Maplist.uscMaplist();
            this.tabReservedSlots = new System.Windows.Forms.TabPage();
            this.lblMohNotice = new System.Windows.Forms.Label();
            this.pnlReservedPanel = new System.Windows.Forms.Panel();
            this.lblReservedCurrent = new System.Windows.Forms.Label();
            this.pnlReservedAddSoldierName = new System.Windows.Forms.Panel();
            this.txtReservedAddSoldierName = new System.Windows.Forms.TextBox();
            this.picReservedAddSoldierName = new System.Windows.Forms.PictureBox();
            this.lblReservedAddSoldierName = new System.Windows.Forms.Label();
            this.lnkReservedAddSoldierName = new System.Windows.Forms.LinkLabel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.btnReservedRemoveSoldier = new System.Windows.Forms.Button();
            this.panel9 = new System.Windows.Forms.Panel();
            this.picReservedList = new System.Windows.Forms.PictureBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.lsvReservedList = new PRoCon.Controls.ControlsEx.ListViewNF();
            this.colSoldierNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabTextChatModeration = new System.Windows.Forms.TabPage();
            this.uscTextChatModerationListcs1 = new PRoCon.Controls.TextChatModeration.uscTextChatModerationListcs();
            this.tmrTimeoutCheck = new System.Windows.Forms.Timer(this.components);
            this.tmrRefreshBanlist = new System.Windows.Forms.Timer(this.components);
            this.ctxBanlistMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.unbanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnReservedSlotsListRefresh = new System.Windows.Forms.Button();
            this.tbcLists.SuspendLayout();
            this.tabBanlist.SuspendLayout();
            this.spltBanlistManualBans.Panel1.SuspendLayout();
            this.spltBanlistManualBans.Panel2.SuspendLayout();
            this.spltBanlistManualBans.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCloseOpenManualBans)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClearLists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUnbanPlayer)).BeginInit();
            this.panel7.SuspendLayout();
            this.pnlBanlistTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBanlistManualBanOkay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBanlistIPError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBanlistGUIDError)).BeginInit();
            this.tabMaplist.SuspendLayout();
            this.tabReservedSlots.SuspendLayout();
            this.pnlReservedPanel.SuspendLayout();
            this.pnlReservedAddSoldierName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picReservedAddSoldierName)).BeginInit();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picReservedList)).BeginInit();
            this.tabTextChatModeration.SuspendLayout();
            this.ctxBanlistMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbcLists
            // 
            this.tbcLists.Controls.Add(this.tabBanlist);
            this.tbcLists.Controls.Add(this.tabMaplist);
            this.tbcLists.Controls.Add(this.tabReservedSlots);
            this.tbcLists.Controls.Add(this.tabTextChatModeration);
            this.tbcLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcLists.Location = new System.Drawing.Point(0, 0);
            this.tbcLists.Name = "tbcLists";
            this.tbcLists.SelectedIndex = 0;
            this.tbcLists.Size = new System.Drawing.Size(1046, 856);
            this.tbcLists.TabIndex = 0;
            this.tbcLists.SelectedIndexChanged += new System.EventHandler(this.tbcLists_SelectedIndexChanged);
            // 
            // tabBanlist
            // 
            this.tabBanlist.Controls.Add(this.spltBanlistManualBans);
            this.tabBanlist.Location = new System.Drawing.Point(4, 24);
            this.tabBanlist.Name = "tabBanlist";
            this.tabBanlist.Padding = new System.Windows.Forms.Padding(8);
            this.tabBanlist.Size = new System.Drawing.Size(1038, 828);
            this.tabBanlist.TabIndex = 2;
            this.tabBanlist.Text = "Banlist";
            this.tabBanlist.UseVisualStyleBackColor = true;
            // 
            // spltBanlistManualBans
            // 
            this.spltBanlistManualBans.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltBanlistManualBans.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spltBanlistManualBans.IsSplitterFixed = true;
            this.spltBanlistManualBans.Location = new System.Drawing.Point(8, 8);
            this.spltBanlistManualBans.Name = "spltBanlistManualBans";
            this.spltBanlistManualBans.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spltBanlistManualBans.Panel1
            // 
            this.spltBanlistManualBans.Panel1.Controls.Add(this.btnBanlistRefresh);
            this.spltBanlistManualBans.Panel1.Controls.Add(this.picCloseOpenManualBans);
            this.spltBanlistManualBans.Panel1.Controls.Add(this.lnkCloseOpenManualBans);
            this.spltBanlistManualBans.Panel1.Controls.Add(this.picClearLists);
            this.spltBanlistManualBans.Panel1.Controls.Add(this.picUnbanPlayer);
            this.spltBanlistManualBans.Panel1.Controls.Add(this.btnBanlistClearBanlist);
            this.spltBanlistManualBans.Panel1.Controls.Add(this.btnBanlistUnban);
            this.spltBanlistManualBans.Panel1.Controls.Add(this.lsvBanlist);
            // 
            // spltBanlistManualBans.Panel2
            // 
            this.spltBanlistManualBans.Panel2.Controls.Add(this.rdoBanlistBc2GUID);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.lblBanlistConfirmation);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.panel7);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.pnlBanlistTime);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.picBanlistManualBanOkay);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.picBanlistIPError);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.picBanlistGUIDError);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.txtBanlistManualBanIP);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.txtBanlistManualBanGUID);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.lblBanlistReason);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.cboBanlistReason);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.btnBanlistAddBan);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.rdoBanlistPbGUID);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.rdoBanlistIP);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.rdoBanlistName);
            this.spltBanlistManualBans.Panel2.Controls.Add(this.txtBanlistManualBanName);
            this.spltBanlistManualBans.Size = new System.Drawing.Size(1022, 812);
            this.spltBanlistManualBans.SplitterDistance = 690;
            this.spltBanlistManualBans.TabIndex = 94;
            // 
            // btnBanlistRefresh
            // 
            this.btnBanlistRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBanlistRefresh.FlatAppearance.BorderSize = 0;
            this.btnBanlistRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBanlistRefresh.Location = new System.Drawing.Point(3, 630);
            this.btnBanlistRefresh.Name = "btnBanlistRefresh";
            this.btnBanlistRefresh.Size = new System.Drawing.Size(35, 23);
            this.btnBanlistRefresh.TabIndex = 107;
            this.btnBanlistRefresh.UseVisualStyleBackColor = true;
            this.btnBanlistRefresh.Click += new System.EventHandler(this.btnBanlistRefresh_Click);
            // 
            // picCloseOpenManualBans
            // 
            this.picCloseOpenManualBans.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.picCloseOpenManualBans.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picCloseOpenManualBans.Location = new System.Drawing.Point(8, 674);
            this.picCloseOpenManualBans.Name = "picCloseOpenManualBans";
            this.picCloseOpenManualBans.Size = new System.Drawing.Size(16, 16);
            this.picCloseOpenManualBans.TabIndex = 106;
            this.picCloseOpenManualBans.TabStop = false;
            this.picCloseOpenManualBans.Click += new System.EventHandler(this.picCloseOpenManualBans_Click);
            // 
            // lnkCloseOpenManualBans
            // 
            this.lnkCloseOpenManualBans.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkCloseOpenManualBans.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkCloseOpenManualBans.AutoSize = true;
            this.lnkCloseOpenManualBans.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkCloseOpenManualBans.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkCloseOpenManualBans.Location = new System.Drawing.Point(24, 675);
            this.lnkCloseOpenManualBans.Name = "lnkCloseOpenManualBans";
            this.lnkCloseOpenManualBans.Size = new System.Drawing.Size(107, 15);
            this.lnkCloseOpenManualBans.TabIndex = 105;
            this.lnkCloseOpenManualBans.TabStop = true;
            this.lnkCloseOpenManualBans.Text = "Close manual bans";
            this.lnkCloseOpenManualBans.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAddBan_LinkClicked);
            // 
            // picClearLists
            // 
            this.picClearLists.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picClearLists.Location = new System.Drawing.Point(600, 634);
            this.picClearLists.Name = "picClearLists";
            this.picClearLists.Size = new System.Drawing.Size(16, 16);
            this.picClearLists.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picClearLists.TabIndex = 104;
            this.picClearLists.TabStop = false;
            // 
            // picUnbanPlayer
            // 
            this.picUnbanPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.picUnbanPlayer.Location = new System.Drawing.Point(816, 634);
            this.picUnbanPlayer.Name = "picUnbanPlayer";
            this.picUnbanPlayer.Size = new System.Drawing.Size(16, 16);
            this.picUnbanPlayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picUnbanPlayer.TabIndex = 103;
            this.picUnbanPlayer.TabStop = false;
            // 
            // btnBanlistClearBanlist
            // 
            this.btnBanlistClearBanlist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBanlistClearBanlist.Location = new System.Drawing.Point(622, 630);
            this.btnBanlistClearBanlist.Name = "btnBanlistClearBanlist";
            this.btnBanlistClearBanlist.Size = new System.Drawing.Size(172, 23);
            this.btnBanlistClearBanlist.TabIndex = 100;
            this.btnBanlistClearBanlist.Text = "Clear Banlist";
            this.btnBanlistClearBanlist.UseVisualStyleBackColor = true;
            this.btnBanlistClearBanlist.Click += new System.EventHandler(this.btnBanlistClearNameList_Click);
            // 
            // btnBanlistUnban
            // 
            this.btnBanlistUnban.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBanlistUnban.Enabled = false;
            this.btnBanlistUnban.Location = new System.Drawing.Point(838, 630);
            this.btnBanlistUnban.Name = "btnBanlistUnban";
            this.btnBanlistUnban.Size = new System.Drawing.Size(179, 23);
            this.btnBanlistUnban.TabIndex = 102;
            this.btnBanlistUnban.Text = "Unban Player";
            this.btnBanlistUnban.UseVisualStyleBackColor = true;
            this.btnBanlistUnban.Click += new System.EventHandler(this.btnBanlistUnban_Click);
            // 
            // lsvBanlist
            // 
            this.lsvBanlist.AllowColumnReorder = true;
            this.lsvBanlist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvBanlist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName,
            this.colIP,
            this.colGUID,
            this.colType,
            this.colTimeRemaining,
            this.colReason});
            this.lsvBanlist.FullRowSelect = true;
            this.lsvBanlist.GridLines = true;
            listViewGroup1.Header = "Name";
            listViewGroup1.Name = "lvgName";
            listViewGroup2.Header = "IpAddress";
            listViewGroup2.Name = "lvgIP";
            listViewGroup3.Header = "Guid";
            listViewGroup3.Name = "lvgGUID";
            this.lsvBanlist.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.lsvBanlist.HideSelection = false;
            this.lsvBanlist.Location = new System.Drawing.Point(0, 0);
            this.lsvBanlist.MultiSelect = false;
            this.lsvBanlist.Name = "lsvBanlist";
            this.lsvBanlist.ShowGroups = false;
            this.lsvBanlist.ShowItemToolTips = true;
            this.lsvBanlist.Size = new System.Drawing.Size(1022, 624);
            this.lsvBanlist.TabIndex = 99;
            this.lsvBanlist.UseCompatibleStateImageBehavior = false;
            this.lsvBanlist.View = System.Windows.Forms.View.Details;
            this.lsvBanlist.SelectedIndexChanged += new System.EventHandler(this.lsvBanlist_SelectedIndexChanged);
            this.lsvBanlist.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lsvBanlist_MouseUp);
            // 
            // colName
            // 
            this.colName.Text = "Name";
            // 
            // colIP
            // 
            this.colIP.Text = "IpAddress";
            // 
            // colGUID
            // 
            this.colGUID.Text = "Guid";
            // 
            // colType
            // 
            this.colType.Text = "Type";
            // 
            // colTimeRemaining
            // 
            this.colTimeRemaining.Text = "Remaining";
            this.colTimeRemaining.Width = 106;
            // 
            // colReason
            // 
            this.colReason.Text = "Reason";
            // 
            // rdoBanlistBc2GUID
            // 
            this.rdoBanlistBc2GUID.AutoSize = true;
            this.rdoBanlistBc2GUID.Location = new System.Drawing.Point(278, 6);
            this.rdoBanlistBc2GUID.Name = "rdoBanlistBc2GUID";
            this.rdoBanlistBc2GUID.Size = new System.Drawing.Size(74, 19);
            this.rdoBanlistBc2GUID.TabIndex = 116;
            this.rdoBanlistBc2GUID.Text = "BC2 Guid";
            this.rdoBanlistBc2GUID.UseVisualStyleBackColor = true;
            this.rdoBanlistBc2GUID.CheckedChanged += new System.EventHandler(this.rdoBanlistBc2GUID_CheckedChanged);
            // 
            // lblBanlistConfirmation
            // 
            this.lblBanlistConfirmation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblBanlistConfirmation.Location = new System.Drawing.Point(313, 57);
            this.lblBanlistConfirmation.Name = "lblBanlistConfirmation";
            this.lblBanlistConfirmation.Size = new System.Drawing.Size(497, 41);
            this.lblBanlistConfirmation.TabIndex = 115;
            this.lblBanlistConfirmation.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.rdoBanlistTemporary);
            this.panel7.Controls.Add(this.rdoBanlistPermanent);
            this.panel7.Location = new System.Drawing.Point(3, 53);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(136, 65);
            this.panel7.TabIndex = 114;
            // 
            // rdoBanlistTemporary
            // 
            this.rdoBanlistTemporary.AutoSize = true;
            this.rdoBanlistTemporary.Location = new System.Drawing.Point(5, 28);
            this.rdoBanlistTemporary.Name = "rdoBanlistTemporary";
            this.rdoBanlistTemporary.Size = new System.Drawing.Size(83, 19);
            this.rdoBanlistTemporary.TabIndex = 113;
            this.rdoBanlistTemporary.Text = "Temporary";
            this.rdoBanlistTemporary.UseVisualStyleBackColor = true;
            this.rdoBanlistTemporary.CheckedChanged += new System.EventHandler(this.rdoBanlistTemporary_CheckedChanged);
            // 
            // rdoBanlistPermanent
            // 
            this.rdoBanlistPermanent.AutoSize = true;
            this.rdoBanlistPermanent.Checked = true;
            this.rdoBanlistPermanent.Location = new System.Drawing.Point(5, 6);
            this.rdoBanlistPermanent.Name = "rdoBanlistPermanent";
            this.rdoBanlistPermanent.Size = new System.Drawing.Size(83, 19);
            this.rdoBanlistPermanent.TabIndex = 112;
            this.rdoBanlistPermanent.TabStop = true;
            this.rdoBanlistPermanent.Text = "Permanent";
            this.rdoBanlistPermanent.UseVisualStyleBackColor = true;
            this.rdoBanlistPermanent.CheckedChanged += new System.EventHandler(this.rdoBanlistPermanent_CheckedChanged);
            // 
            // pnlBanlistTime
            // 
            this.pnlBanlistTime.Controls.Add(this.lblBanlistTime);
            this.pnlBanlistTime.Controls.Add(this.txtBanlistTime);
            this.pnlBanlistTime.Controls.Add(this.cboBanlistTimeMultiplier);
            this.pnlBanlistTime.Enabled = false;
            this.pnlBanlistTime.Location = new System.Drawing.Point(142, 57);
            this.pnlBanlistTime.Name = "pnlBanlistTime";
            this.pnlBanlistTime.Size = new System.Drawing.Size(165, 58);
            this.pnlBanlistTime.TabIndex = 113;
            // 
            // lblBanlistTime
            // 
            this.lblBanlistTime.AutoSize = true;
            this.lblBanlistTime.Location = new System.Drawing.Point(3, 2);
            this.lblBanlistTime.Name = "lblBanlistTime";
            this.lblBanlistTime.Size = new System.Drawing.Size(37, 15);
            this.lblBanlistTime.TabIndex = 0;
            this.lblBanlistTime.Text = "Time:";
            // 
            // txtBanlistTime
            // 
            this.txtBanlistTime.Location = new System.Drawing.Point(6, 23);
            this.txtBanlistTime.MaxLength = 3;
            this.txtBanlistTime.Name = "txtBanlistTime";
            this.txtBanlistTime.Size = new System.Drawing.Size(39, 23);
            this.txtBanlistTime.TabIndex = 1;
            this.txtBanlistTime.Text = "1";
            this.txtBanlistTime.TextChanged += new System.EventHandler(this.txtBanlistTime_TextChanged);
            this.txtBanlistTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBanlistTime_KeyPress);
            // 
            // cboBanlistTimeMultiplier
            // 
            this.cboBanlistTimeMultiplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBanlistTimeMultiplier.FormattingEnabled = true;
            this.cboBanlistTimeMultiplier.Items.AddRange(new object[] {
            "Minutes",
            "Hours",
            "Days",
            "Weeks",
            "Months"});
            this.cboBanlistTimeMultiplier.Location = new System.Drawing.Point(53, 23);
            this.cboBanlistTimeMultiplier.Name = "cboBanlistTimeMultiplier";
            this.cboBanlistTimeMultiplier.Size = new System.Drawing.Size(102, 23);
            this.cboBanlistTimeMultiplier.TabIndex = 2;
            this.cboBanlistTimeMultiplier.SelectedIndexChanged += new System.EventHandler(this.cboBanlistTimeMultiplier_SelectedIndexChanged);
            // 
            // picBanlistManualBanOkay
            // 
            this.picBanlistManualBanOkay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picBanlistManualBanOkay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picBanlistManualBanOkay.Location = new System.Drawing.Point(816, 82);
            this.picBanlistManualBanOkay.Name = "picBanlistManualBanOkay";
            this.picBanlistManualBanOkay.Size = new System.Drawing.Size(16, 16);
            this.picBanlistManualBanOkay.TabIndex = 109;
            this.picBanlistManualBanOkay.TabStop = false;
            this.picBanlistManualBanOkay.Visible = false;
            // 
            // picBanlistIPError
            // 
            this.picBanlistIPError.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picBanlistIPError.Location = new System.Drawing.Point(121, 8);
            this.picBanlistIPError.Name = "picBanlistIPError";
            this.picBanlistIPError.Size = new System.Drawing.Size(16, 16);
            this.picBanlistIPError.TabIndex = 108;
            this.picBanlistIPError.TabStop = false;
            this.picBanlistIPError.Visible = false;
            // 
            // picBanlistGUIDError
            // 
            this.picBanlistGUIDError.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picBanlistGUIDError.Location = new System.Drawing.Point(256, 9);
            this.picBanlistGUIDError.Name = "picBanlistGUIDError";
            this.picBanlistGUIDError.Size = new System.Drawing.Size(16, 16);
            this.picBanlistGUIDError.TabIndex = 107;
            this.picBanlistGUIDError.TabStop = false;
            this.picBanlistGUIDError.Visible = false;
            // 
            // txtBanlistManualBanIP
            // 
            this.txtBanlistManualBanIP.Enabled = false;
            this.txtBanlistManualBanIP.Location = new System.Drawing.Point(142, 28);
            this.txtBanlistManualBanIP.Name = "txtBanlistManualBanIP";
            this.txtBanlistManualBanIP.Size = new System.Drawing.Size(128, 23);
            this.txtBanlistManualBanIP.TabIndex = 67;
            this.txtBanlistManualBanIP.TextChanged += new System.EventHandler(this.txtBanlistManualBanIP_TextChanged);
            // 
            // txtBanlistManualBanGUID
            // 
            this.txtBanlistManualBanGUID.Enabled = false;
            this.txtBanlistManualBanGUID.Location = new System.Drawing.Point(276, 28);
            this.txtBanlistManualBanGUID.Name = "txtBanlistManualBanGUID";
            this.txtBanlistManualBanGUID.Size = new System.Drawing.Size(243, 23);
            this.txtBanlistManualBanGUID.TabIndex = 66;
            this.txtBanlistManualBanGUID.TextChanged += new System.EventHandler(this.txtBanlistManualBanGUID_TextChanged);
            // 
            // lblBanlistReason
            // 
            this.lblBanlistReason.AutoSize = true;
            this.lblBanlistReason.Location = new System.Drawing.Point(522, 10);
            this.lblBanlistReason.Name = "lblBanlistReason";
            this.lblBanlistReason.Size = new System.Drawing.Size(48, 15);
            this.lblBanlistReason.TabIndex = 65;
            this.lblBanlistReason.Text = "Reason:";
            // 
            // cboBanlistReason
            // 
            this.cboBanlistReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboBanlistReason.FormattingEnabled = true;
            this.cboBanlistReason.Items.AddRange(new object[] {
            "Team Killing",
            "Hacking/Cheating",
            "Admin abuse"});
            this.cboBanlistReason.Location = new System.Drawing.Point(525, 28);
            this.cboBanlistReason.Name = "cboBanlistReason";
            this.cboBanlistReason.Size = new System.Drawing.Size(492, 23);
            this.cboBanlistReason.TabIndex = 64;
            // 
            // btnBanlistAddBan
            // 
            this.btnBanlistAddBan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBanlistAddBan.Enabled = false;
            this.btnBanlistAddBan.ForeColor = System.Drawing.Color.Black;
            this.btnBanlistAddBan.Location = new System.Drawing.Point(838, 79);
            this.btnBanlistAddBan.Name = "btnBanlistAddBan";
            this.btnBanlistAddBan.Size = new System.Drawing.Size(179, 23);
            this.btnBanlistAddBan.TabIndex = 6;
            this.btnBanlistAddBan.Text = "Ban";
            this.btnBanlistAddBan.UseVisualStyleBackColor = true;
            this.btnBanlistAddBan.Click += new System.EventHandler(this.btnBanlistAddBan_Click);
            // 
            // rdoBanlistPbGUID
            // 
            this.rdoBanlistPbGUID.AutoSize = true;
            this.rdoBanlistPbGUID.Location = new System.Drawing.Point(386, 6);
            this.rdoBanlistPbGUID.Name = "rdoBanlistPbGUID";
            this.rdoBanlistPbGUID.Size = new System.Drawing.Size(67, 19);
            this.rdoBanlistPbGUID.TabIndex = 5;
            this.rdoBanlistPbGUID.Text = "PB Guid";
            this.rdoBanlistPbGUID.UseVisualStyleBackColor = true;
            this.rdoBanlistPbGUID.CheckedChanged += new System.EventHandler(this.rdoBanlistGUID_CheckedChanged);
            // 
            // rdoBanlistIP
            // 
            this.rdoBanlistIP.AutoSize = true;
            this.rdoBanlistIP.Location = new System.Drawing.Point(142, 6);
            this.rdoBanlistIP.Name = "rdoBanlistIP";
            this.rdoBanlistIP.Size = new System.Drawing.Size(77, 19);
            this.rdoBanlistIP.TabIndex = 4;
            this.rdoBanlistIP.Text = "IpAddress";
            this.rdoBanlistIP.UseVisualStyleBackColor = true;
            this.rdoBanlistIP.CheckedChanged += new System.EventHandler(this.rdoBanlistIP_CheckedChanged);
            // 
            // rdoBanlistName
            // 
            this.rdoBanlistName.AutoSize = true;
            this.rdoBanlistName.Checked = true;
            this.rdoBanlistName.Location = new System.Drawing.Point(8, 6);
            this.rdoBanlistName.Name = "rdoBanlistName";
            this.rdoBanlistName.Size = new System.Drawing.Size(57, 19);
            this.rdoBanlistName.TabIndex = 3;
            this.rdoBanlistName.TabStop = true;
            this.rdoBanlistName.Text = "Name";
            this.rdoBanlistName.UseVisualStyleBackColor = true;
            this.rdoBanlistName.CheckedChanged += new System.EventHandler(this.rdoBanlistName_CheckedChanged);
            // 
            // txtBanlistManualBanName
            // 
            this.txtBanlistManualBanName.Location = new System.Drawing.Point(8, 28);
            this.txtBanlistManualBanName.Name = "txtBanlistManualBanName";
            this.txtBanlistManualBanName.Size = new System.Drawing.Size(128, 23);
            this.txtBanlistManualBanName.TabIndex = 1;
            this.txtBanlistManualBanName.TextChanged += new System.EventHandler(this.txtBanlistManualBanName_TextChanged);
            // 
            // tabMaplist
            // 
            this.tabMaplist.Controls.Add(this.uscMaplist1);
            this.tabMaplist.Location = new System.Drawing.Point(4, 24);
            this.tabMaplist.Name = "tabMaplist";
            this.tabMaplist.Padding = new System.Windows.Forms.Padding(8);
            this.tabMaplist.Size = new System.Drawing.Size(1038, 828);
            this.tabMaplist.TabIndex = 0;
            this.tabMaplist.Text = "Maplist";
            this.tabMaplist.UseVisualStyleBackColor = true;
            // 
            // uscMaplist1
            // 
            this.uscMaplist1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uscMaplist1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscMaplist1.Location = new System.Drawing.Point(8, 8);
            this.uscMaplist1.Name = "uscMaplist1";
            this.uscMaplist1.SettingFail = null;
            this.uscMaplist1.SettingLoading = null;
            this.uscMaplist1.SettingSuccess = null;
            this.uscMaplist1.Size = new System.Drawing.Size(1022, 812);
            this.uscMaplist1.TabIndex = 1;
            // 
            // tabReservedSlots
            // 
            this.tabReservedSlots.Controls.Add(this.lblMohNotice);
            this.tabReservedSlots.Controls.Add(this.pnlReservedPanel);
            this.tabReservedSlots.Location = new System.Drawing.Point(4, 24);
            this.tabReservedSlots.Name = "tabReservedSlots";
            this.tabReservedSlots.Padding = new System.Windows.Forms.Padding(8);
            this.tabReservedSlots.Size = new System.Drawing.Size(1038, 828);
            this.tabReservedSlots.TabIndex = 1;
            this.tabReservedSlots.Text = "Reserved Slots";
            this.tabReservedSlots.UseVisualStyleBackColor = true;
            // 
            // lblMohNotice
            // 
            this.lblMohNotice.AutoSize = true;
            this.lblMohNotice.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMohNotice.ForeColor = System.Drawing.Color.Maroon;
            this.lblMohNotice.Location = new System.Drawing.Point(11, 11);
            this.lblMohNotice.Name = "lblMohNotice";
            this.lblMohNotice.Size = new System.Drawing.Size(320, 17);
            this.lblMohNotice.TabIndex = 28;
            this.lblMohNotice.Text = "This panel is disabled for MoH until further notice";
            this.lblMohNotice.Visible = false;
            // 
            // pnlReservedPanel
            // 
            this.pnlReservedPanel.Controls.Add(this.lblReservedCurrent);
            this.pnlReservedPanel.Controls.Add(this.pnlReservedAddSoldierName);
            this.pnlReservedPanel.Controls.Add(this.panel8);
            this.pnlReservedPanel.Controls.Add(this.panel9);
            this.pnlReservedPanel.Controls.Add(this.picReservedList);
            this.pnlReservedPanel.Controls.Add(this.panel10);
            this.pnlReservedPanel.Controls.Add(this.lsvReservedList);
            this.pnlReservedPanel.Location = new System.Drawing.Point(229, 31);
            this.pnlReservedPanel.Name = "pnlReservedPanel";
            this.pnlReservedPanel.Size = new System.Drawing.Size(486, 445);
            this.pnlReservedPanel.TabIndex = 27;
            // 
            // lblReservedCurrent
            // 
            this.lblReservedCurrent.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblReservedCurrent.AutoSize = true;
            this.lblReservedCurrent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReservedCurrent.Location = new System.Drawing.Point(32, 17);
            this.lblReservedCurrent.Name = "lblReservedCurrent";
            this.lblReservedCurrent.Size = new System.Drawing.Size(131, 15);
            this.lblReservedCurrent.TabIndex = 14;
            this.lblReservedCurrent.Text = "Current reserved slots";
            // 
            // pnlReservedAddSoldierName
            // 
            this.pnlReservedAddSoldierName.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pnlReservedAddSoldierName.Controls.Add(this.txtReservedAddSoldierName);
            this.pnlReservedAddSoldierName.Controls.Add(this.picReservedAddSoldierName);
            this.pnlReservedAddSoldierName.Controls.Add(this.lblReservedAddSoldierName);
            this.pnlReservedAddSoldierName.Controls.Add(this.lnkReservedAddSoldierName);
            this.pnlReservedAddSoldierName.Location = new System.Drawing.Point(35, 305);
            this.pnlReservedAddSoldierName.Name = "pnlReservedAddSoldierName";
            this.pnlReservedAddSoldierName.Size = new System.Drawing.Size(430, 60);
            this.pnlReservedAddSoldierName.TabIndex = 2;
            // 
            // txtReservedAddSoldierName
            // 
            this.txtReservedAddSoldierName.Location = new System.Drawing.Point(34, 27);
            this.txtReservedAddSoldierName.Name = "txtReservedAddSoldierName";
            this.txtReservedAddSoldierName.Size = new System.Drawing.Size(280, 23);
            this.txtReservedAddSoldierName.TabIndex = 1;
            this.txtReservedAddSoldierName.TextChanged += new System.EventHandler(this.txtReservedAddSoldierName_TextChanged);
            this.txtReservedAddSoldierName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtReservedAddSoldierName_KeyDown);
            // 
            // picReservedAddSoldierName
            // 
            this.picReservedAddSoldierName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picReservedAddSoldierName.Location = new System.Drawing.Point(12, 8);
            this.picReservedAddSoldierName.Name = "picReservedAddSoldierName";
            this.picReservedAddSoldierName.Size = new System.Drawing.Size(16, 16);
            this.picReservedAddSoldierName.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picReservedAddSoldierName.TabIndex = 102;
            this.picReservedAddSoldierName.TabStop = false;
            // 
            // lblReservedAddSoldierName
            // 
            this.lblReservedAddSoldierName.AutoSize = true;
            this.lblReservedAddSoldierName.Location = new System.Drawing.Point(31, 9);
            this.lblReservedAddSoldierName.Name = "lblReservedAddSoldierName";
            this.lblReservedAddSoldierName.Size = new System.Drawing.Size(161, 15);
            this.lblReservedAddSoldierName.TabIndex = 93;
            this.lblReservedAddSoldierName.Text = "Add a soldier name to the list";
            // 
            // lnkReservedAddSoldierName
            // 
            this.lnkReservedAddSoldierName.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkReservedAddSoldierName.AutoSize = true;
            this.lnkReservedAddSoldierName.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkReservedAddSoldierName.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkReservedAddSoldierName.Location = new System.Drawing.Point(320, 30);
            this.lnkReservedAddSoldierName.Name = "lnkReservedAddSoldierName";
            this.lnkReservedAddSoldierName.Size = new System.Drawing.Size(67, 15);
            this.lnkReservedAddSoldierName.TabIndex = 2;
            this.lnkReservedAddSoldierName.TabStop = true;
            this.lnkReservedAddSoldierName.Text = "Add soldier";
            this.lnkReservedAddSoldierName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkReservedAddSoldierName_LinkClicked);
            // 
            // panel8
            // 
            this.panel8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel8.Controls.Add(this.btnReservedSlotsListRefresh);
            this.panel8.Controls.Add(this.btnReservedRemoveSoldier);
            this.panel8.Location = new System.Drawing.Point(424, 120);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(41, 100);
            this.panel8.TabIndex = 1;
            // 
            // btnReservedRemoveSoldier
            // 
            this.btnReservedRemoveSoldier.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnReservedRemoveSoldier.Enabled = false;
            this.btnReservedRemoveSoldier.FlatAppearance.BorderSize = 0;
            this.btnReservedRemoveSoldier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReservedRemoveSoldier.Location = new System.Drawing.Point(3, 32);
            this.btnReservedRemoveSoldier.Name = "btnReservedRemoveSoldier";
            this.btnReservedRemoveSoldier.Size = new System.Drawing.Size(35, 23);
            this.btnReservedRemoveSoldier.TabIndex = 1;
            this.btnReservedRemoveSoldier.UseVisualStyleBackColor = true;
            this.btnReservedRemoveSoldier.Click += new System.EventHandler(this.btnReservedRemoveSoldier_Click);
            // 
            // panel9
            // 
            this.panel9.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel9.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel9.Location = new System.Drawing.Point(35, 289);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(424, 1);
            this.panel9.TabIndex = 95;
            // 
            // picReservedList
            // 
            this.picReservedList.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picReservedList.Location = new System.Drawing.Point(47, 41);
            this.picReservedList.Name = "picReservedList";
            this.picReservedList.Size = new System.Drawing.Size(16, 16);
            this.picReservedList.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picReservedList.TabIndex = 91;
            this.picReservedList.TabStop = false;
            // 
            // panel10
            // 
            this.panel10.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel10.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel10.Location = new System.Drawing.Point(35, 25);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(424, 1);
            this.panel10.TabIndex = 16;
            // 
            // lsvReservedList
            // 
            this.lsvReservedList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lsvReservedList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSoldierNames});
            this.lsvReservedList.FullRowSelect = true;
            this.lsvReservedList.HideSelection = false;
            this.lsvReservedList.Location = new System.Drawing.Point(69, 41);
            this.lsvReservedList.MultiSelect = false;
            this.lsvReservedList.Name = "lsvReservedList";
            this.lsvReservedList.Size = new System.Drawing.Size(349, 232);
            this.lsvReservedList.TabIndex = 0;
            this.lsvReservedList.UseCompatibleStateImageBehavior = false;
            this.lsvReservedList.View = System.Windows.Forms.View.Details;
            this.lsvReservedList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lsvReservedList_ColumnClick);
            this.lsvReservedList.SelectedIndexChanged += new System.EventHandler(this.lsvReservedList_SelectedIndexChanged);
            // 
            // colSoldierNames
            // 
            this.colSoldierNames.Text = "Soldier Names";
            this.colSoldierNames.Width = 317;
            // 
            // tabTextChatModeration
            // 
            this.tabTextChatModeration.Controls.Add(this.uscTextChatModerationListcs1);
            this.tabTextChatModeration.Location = new System.Drawing.Point(4, 24);
            this.tabTextChatModeration.Name = "tabTextChatModeration";
            this.tabTextChatModeration.Padding = new System.Windows.Forms.Padding(3);
            this.tabTextChatModeration.Size = new System.Drawing.Size(1038, 828);
            this.tabTextChatModeration.TabIndex = 3;
            this.tabTextChatModeration.Text = "Text chat moderation";
            this.tabTextChatModeration.UseVisualStyleBackColor = true;
            // 
            // uscTextChatModerationListcs1
            // 
            this.uscTextChatModerationListcs1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uscTextChatModerationListcs1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscTextChatModerationListcs1.Location = new System.Drawing.Point(3, 3);
            this.uscTextChatModerationListcs1.Name = "uscTextChatModerationListcs1";
            this.uscTextChatModerationListcs1.SettingFail = null;
            this.uscTextChatModerationListcs1.SettingLoading = null;
            this.uscTextChatModerationListcs1.SettingSuccess = null;
            this.uscTextChatModerationListcs1.Size = new System.Drawing.Size(1032, 822);
            this.uscTextChatModerationListcs1.TabIndex = 0;
            // 
            // tmrTimeoutCheck
            // 
            this.tmrTimeoutCheck.Tick += new System.EventHandler(this.tmrSettingsAnimator_Tick);
            // 
            // tmrRefreshBanlist
            // 
            this.tmrRefreshBanlist.Enabled = true;
            this.tmrRefreshBanlist.Interval = 15000;
            this.tmrRefreshBanlist.Tick += new System.EventHandler(this.tmrRefreshBanlist_Tick);
            // 
            // ctxBanlistMenuStrip
            // 
            this.ctxBanlistMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.toolStripMenuItem1,
            this.unbanToolStripMenuItem});
            this.ctxBanlistMenuStrip.Name = "ctxBanlistMenuStrip";
            this.ctxBanlistMenuStrip.Size = new System.Drawing.Size(172, 54);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.copyToolStripMenuItem.Text = "Copy to Clipboard";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(168, 6);
            // 
            // unbanToolStripMenuItem
            // 
            this.unbanToolStripMenuItem.Name = "unbanToolStripMenuItem";
            this.unbanToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.unbanToolStripMenuItem.Text = "Unban";
            this.unbanToolStripMenuItem.Click += new System.EventHandler(this.unbanToolStripMenuItem_Click);
            // 
            // btnReservedSlotsListRefresh
            // 
            this.btnReservedSlotsListRefresh.FlatAppearance.BorderSize = 0;
            this.btnReservedSlotsListRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReservedSlotsListRefresh.Location = new System.Drawing.Point(3, 74);
            this.btnReservedSlotsListRefresh.Name = "btnReservedSlotsListRefresh";
            this.btnReservedSlotsListRefresh.Size = new System.Drawing.Size(35, 23);
            this.btnReservedSlotsListRefresh.TabIndex = 109;
            this.btnReservedSlotsListRefresh.UseVisualStyleBackColor = true;
            this.btnReservedSlotsListRefresh.Click += new System.EventHandler(this.btnReservedSlotsListRefresh_Click);
            // 
            // uscListControlPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbcLists);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "uscListControlPanel";
            this.Size = new System.Drawing.Size(1046, 856);
            this.Load += new System.EventHandler(this.uscListControlPanel_Load);
            this.tbcLists.ResumeLayout(false);
            this.tabBanlist.ResumeLayout(false);
            this.spltBanlistManualBans.Panel1.ResumeLayout(false);
            this.spltBanlistManualBans.Panel1.PerformLayout();
            this.spltBanlistManualBans.Panel2.ResumeLayout(false);
            this.spltBanlistManualBans.Panel2.PerformLayout();
            this.spltBanlistManualBans.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picCloseOpenManualBans)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClearLists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUnbanPlayer)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.pnlBanlistTime.ResumeLayout(false);
            this.pnlBanlistTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBanlistManualBanOkay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBanlistIPError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBanlistGUIDError)).EndInit();
            this.tabMaplist.ResumeLayout(false);
            this.tabReservedSlots.ResumeLayout(false);
            this.tabReservedSlots.PerformLayout();
            this.pnlReservedPanel.ResumeLayout(false);
            this.pnlReservedPanel.PerformLayout();
            this.pnlReservedAddSoldierName.ResumeLayout(false);
            this.pnlReservedAddSoldierName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picReservedAddSoldierName)).EndInit();
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picReservedList)).EndInit();
            this.tabTextChatModeration.ResumeLayout(false);
            this.ctxBanlistMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbcLists;
        private System.Windows.Forms.TabPage tabMaplist;
        private System.Windows.Forms.TabPage tabReservedSlots;
        private System.Windows.Forms.TabPage tabBanlist;
        private System.Windows.Forms.Timer tmrTimeoutCheck;
        private System.Windows.Forms.Panel pnlReservedPanel;
        private System.Windows.Forms.Panel pnlReservedAddSoldierName;
        private System.Windows.Forms.PictureBox picReservedAddSoldierName;
        private System.Windows.Forms.Label lblReservedAddSoldierName;
        private System.Windows.Forms.LinkLabel lnkReservedAddSoldierName;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Button btnReservedRemoveSoldier;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.PictureBox picReservedList;
        private System.Windows.Forms.Panel panel10;
        private PRoCon.Controls.ControlsEx.ListViewNF lsvReservedList;
        private System.Windows.Forms.ColumnHeader colSoldierNames;
        private System.Windows.Forms.Label lblReservedCurrent;
        private System.Windows.Forms.TextBox txtReservedAddSoldierName;
        private System.Windows.Forms.Timer tmrRefreshBanlist;
        private System.Windows.Forms.SplitContainer spltBanlistManualBans;
        private System.Windows.Forms.LinkLabel lnkCloseOpenManualBans;
        private System.Windows.Forms.PictureBox picClearLists;
        private System.Windows.Forms.PictureBox picUnbanPlayer;
        private System.Windows.Forms.Button btnBanlistClearBanlist;
        private System.Windows.Forms.Button btnBanlistUnban;
        private PRoCon.Controls.ControlsEx.ListViewNF lsvBanlist;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colIP;
        private System.Windows.Forms.ColumnHeader colGUID;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colTimeRemaining;
        private System.Windows.Forms.ColumnHeader colReason;
        private System.Windows.Forms.TextBox txtBanlistManualBanName;
        private System.Windows.Forms.RadioButton rdoBanlistPbGUID;
        private System.Windows.Forms.RadioButton rdoBanlistIP;
        private System.Windows.Forms.RadioButton rdoBanlistName;
        private System.Windows.Forms.Button btnBanlistAddBan;
        private System.Windows.Forms.Label lblBanlistReason;
        private System.Windows.Forms.ComboBox cboBanlistReason;
        private System.Windows.Forms.PictureBox picCloseOpenManualBans;
        private System.Windows.Forms.TextBox txtBanlistManualBanIP;
        private System.Windows.Forms.TextBox txtBanlistManualBanGUID;
        private System.Windows.Forms.PictureBox picBanlistGUIDError;
        private System.Windows.Forms.PictureBox picBanlistIPError;
        private System.Windows.Forms.PictureBox picBanlistManualBanOkay;
        private System.Windows.Forms.ContextMenuStrip ctxBanlistMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem unbanToolStripMenuItem;
        private System.Windows.Forms.Panel pnlBanlistTime;
        private System.Windows.Forms.Label lblBanlistTime;
        private System.Windows.Forms.TextBox txtBanlistTime;
        private System.Windows.Forms.ComboBox cboBanlistTimeMultiplier;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.RadioButton rdoBanlistTemporary;
        private System.Windows.Forms.RadioButton rdoBanlistPermanent;
        private System.Windows.Forms.Label lblBanlistConfirmation;
        private System.Windows.Forms.RadioButton rdoBanlistBc2GUID;
        private System.Windows.Forms.Button btnBanlistRefresh;
        private Controls.Maplist.uscMaplist uscMaplist1;
        private System.Windows.Forms.TabPage tabTextChatModeration;
        private Controls.TextChatModeration.uscTextChatModerationListcs uscTextChatModerationListcs1;
        private System.Windows.Forms.Label lblMohNotice;
        private System.Windows.Forms.Button btnReservedSlotsListRefresh;
    }
}
