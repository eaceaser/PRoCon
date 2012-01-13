namespace PRoCon {
    partial class uscServerConnection {
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
            this.tbcClientTabs = new System.Windows.Forms.TabControl();
            this.tabPlayerList = new System.Windows.Forms.TabPage();
            this.uscPlayers = new PRoCon.uscPlayerListPanel();
            this.tabChat = new System.Windows.Forms.TabPage();
            this.uscChat = new PRoCon.uscChatPanel();
            this.pnlChatEnclosure = new System.Windows.Forms.Panel();
            this.rtbChatBox = new System.Windows.Forms.RichTextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblRecipientUser = new System.Windows.Forms.Label();
            this.cboRecipient = new System.Windows.Forms.ComboBox();
            this.tabMapView = new System.Windows.Forms.TabPage();
            this.uscMap = new PRoCon.Controls.uscMapViewer();
            this.tabEvents = new System.Windows.Forms.TabPage();
            this.uscEvents = new PRoCon.uscEventsPanel();
            this.tabLists = new System.Windows.Forms.TabPage();
            this.uscLists = new PRoCon.uscListControlPanel();
            this.tabServerSettings = new System.Windows.Forms.TabPage();
            this.uscSettings = new PRoCon.uscServerSettingsPanel();
            this.tabPlugins = new System.Windows.Forms.TabPage();
            this.uscPlugins = new PRoCon.uscPluginPanel();
            this.tabAccounts = new System.Windows.Forms.TabPage();
            this.uscAccounts = new PRoCon.uscAccountsPanel();
            this.tabConsole = new System.Windows.Forms.TabPage();
            this.uscServerConsole = new PRoCon.uscConsolePanel();
            this.lblCurrentMapName = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.tmrPortCheckTester = new System.Windows.Forms.Timer(this.components);
            this.lblCurrentRound = new System.Windows.Forms.Label();
            this.uscLogin = new PRoCon.uscLoginPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.lblRoundTime = new System.Windows.Forms.Label();
            this.pnlMapControls = new System.Windows.Forms.FlowLayoutPanel();
            this.picRestartRound = new System.Windows.Forms.PictureBox();
            this.btnRestartRound = new System.Windows.Forms.Button();
            this.picNextRound = new System.Windows.Forms.PictureBox();
            this.btnNextRound = new System.Windows.Forms.Button();
            this.lblPlasmaStatus = new System.Windows.Forms.Label();
            this.lblLayerVersion = new System.Windows.Forms.Label();
            this.lblServerUptime = new System.Windows.Forms.Label();
            this.lblMappack = new System.Windows.Forms.Label();
            this.toolTipPlasma = new System.Windows.Forms.ToolTip(this.components);
            this.toolTipMapControls = new System.Windows.Forms.ToolTip(this.components);
            this.tmrTimerTicks = new System.Windows.Forms.Timer(this.components);
            this.tbcClientTabs.SuspendLayout();
            this.tabPlayerList.SuspendLayout();
            this.tabChat.SuspendLayout();
            this.pnlChatEnclosure.SuspendLayout();
            this.tabMapView.SuspendLayout();
            this.tabEvents.SuspendLayout();
            this.tabLists.SuspendLayout();
            this.tabServerSettings.SuspendLayout();
            this.tabPlugins.SuspendLayout();
            this.tabAccounts.SuspendLayout();
            this.tabConsole.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.pnlMapControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRestartRound)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNextRound)).BeginInit();
            this.SuspendLayout();
            // 
            // tbcClientTabs
            // 
            this.tbcClientTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcClientTabs.Controls.Add(this.tabPlayerList);
            this.tbcClientTabs.Controls.Add(this.tabChat);
            this.tbcClientTabs.Controls.Add(this.tabMapView);
            this.tbcClientTabs.Controls.Add(this.tabEvents);
            this.tbcClientTabs.Controls.Add(this.tabLists);
            this.tbcClientTabs.Controls.Add(this.tabServerSettings);
            this.tbcClientTabs.Controls.Add(this.tabPlugins);
            this.tbcClientTabs.Controls.Add(this.tabAccounts);
            this.tbcClientTabs.Controls.Add(this.tabConsole);
            this.tbcClientTabs.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbcClientTabs.Location = new System.Drawing.Point(8, 30);
            this.tbcClientTabs.Name = "tbcClientTabs";
            this.tbcClientTabs.SelectedIndex = 0;
            this.tbcClientTabs.Size = new System.Drawing.Size(1386, 963);
            this.tbcClientTabs.TabIndex = 0;
            this.tbcClientTabs.SelectedIndexChanged += new System.EventHandler(this.tbcClientTabs_SelectedIndexChanged);
            // 
            // tabPlayerList
            // 
            this.tabPlayerList.Controls.Add(this.uscPlayers);
            this.tabPlayerList.Location = new System.Drawing.Point(4, 24);
            this.tabPlayerList.Name = "tabPlayerList";
            this.tabPlayerList.Padding = new System.Windows.Forms.Padding(9);
            this.tabPlayerList.Size = new System.Drawing.Size(1378, 935);
            this.tabPlayerList.TabIndex = 1;
            this.tabPlayerList.Text = "Players";
            this.tabPlayerList.UseVisualStyleBackColor = true;
            // 
            // uscPlayers
            // 
            this.uscPlayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uscPlayers.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscPlayers.Location = new System.Drawing.Point(9, 9);
            this.uscPlayers.Name = "uscPlayers";
            this.uscPlayers.Size = new System.Drawing.Size(1360, 917);
            this.uscPlayers.TabIndex = 0;
            // 
            // tabChat
            // 
            this.tabChat.Controls.Add(this.uscChat);
            this.tabChat.Controls.Add(this.pnlChatEnclosure);
            this.tabChat.Controls.Add(this.txtMessage);
            this.tabChat.Controls.Add(this.lblMessage);
            this.tabChat.Controls.Add(this.lblRecipientUser);
            this.tabChat.Controls.Add(this.cboRecipient);
            this.tabChat.Location = new System.Drawing.Point(4, 24);
            this.tabChat.Name = "tabChat";
            this.tabChat.Padding = new System.Windows.Forms.Padding(9);
            this.tabChat.Size = new System.Drawing.Size(1378, 935);
            this.tabChat.TabIndex = 0;
            this.tabChat.Text = "Chat";
            this.tabChat.UseVisualStyleBackColor = true;
            // 
            // uscChat
            // 
            this.uscChat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uscChat.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscChat.Location = new System.Drawing.Point(9, 9);
            this.uscChat.Name = "uscChat";
            this.uscChat.SettingFail = null;
            this.uscChat.SettingLoading = null;
            this.uscChat.SettingSuccess = null;
            this.uscChat.Size = new System.Drawing.Size(1360, 917);
            this.uscChat.TabIndex = 17;
            // 
            // pnlChatEnclosure
            // 
            this.pnlChatEnclosure.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlChatEnclosure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlChatEnclosure.Controls.Add(this.rtbChatBox);
            this.pnlChatEnclosure.Location = new System.Drawing.Point(13, 13);
            this.pnlChatEnclosure.Name = "pnlChatEnclosure";
            this.pnlChatEnclosure.Size = new System.Drawing.Size(0, 0);
            this.pnlChatEnclosure.TabIndex = 13;
            // 
            // rtbChatBox
            // 
            this.rtbChatBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbChatBox.Location = new System.Drawing.Point(0, 0);
            this.rtbChatBox.Name = "rtbChatBox";
            this.rtbChatBox.ReadOnly = true;
            this.rtbChatBox.Size = new System.Drawing.Size(0, 0);
            this.rtbChatBox.TabIndex = 0;
            this.rtbChatBox.Text = "";
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(134, -32860);
            this.txtMessage.MaxLength = 128;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(0, 23);
            this.txtMessage.TabIndex = 7;
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(131, -32860);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(56, 15);
            this.lblMessage.TabIndex = 6;
            this.lblMessage.Text = "Message:";
            // 
            // lblRecipientUser
            // 
            this.lblRecipientUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRecipientUser.AutoSize = true;
            this.lblRecipientUser.Location = new System.Drawing.Point(13, -32860);
            this.lblRecipientUser.Name = "lblRecipientUser";
            this.lblRecipientUser.Size = new System.Drawing.Size(59, 15);
            this.lblRecipientUser.TabIndex = 4;
            this.lblRecipientUser.Text = "Recipient:";
            // 
            // cboRecipient
            // 
            this.cboRecipient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboRecipient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRecipient.FormattingEnabled = true;
            this.cboRecipient.Items.AddRange(new object[] {
            "All Players"});
            this.cboRecipient.Location = new System.Drawing.Point(16, -32860);
            this.cboRecipient.Name = "cboRecipient";
            this.cboRecipient.Size = new System.Drawing.Size(112, 23);
            this.cboRecipient.TabIndex = 5;
            // 
            // tabMapView
            // 
            this.tabMapView.Controls.Add(this.uscMap);
            this.tabMapView.Location = new System.Drawing.Point(4, 24);
            this.tabMapView.Name = "tabMapView";
            this.tabMapView.Padding = new System.Windows.Forms.Padding(8);
            this.tabMapView.Size = new System.Drawing.Size(1378, 935);
            this.tabMapView.TabIndex = 8;
            this.tabMapView.Text = "Map";
            this.tabMapView.UseVisualStyleBackColor = true;
            // 
            // uscMap
            // 
            this.uscMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uscMap.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscMap.IsMapLoaded = false;
            this.uscMap.IsMapSelected = false;
            this.uscMap.Location = new System.Drawing.Point(8, 8);
            this.uscMap.Name = "uscMap";
            this.uscMap.Size = new System.Drawing.Size(1362, 919);
            this.uscMap.TabIndex = 0;
            // 
            // tabEvents
            // 
            this.tabEvents.Controls.Add(this.uscEvents);
            this.tabEvents.Location = new System.Drawing.Point(4, 24);
            this.tabEvents.Name = "tabEvents";
            this.tabEvents.Padding = new System.Windows.Forms.Padding(9);
            this.tabEvents.Size = new System.Drawing.Size(1378, 935);
            this.tabEvents.TabIndex = 7;
            this.tabEvents.Text = "Events";
            this.tabEvents.UseVisualStyleBackColor = true;
            // 
            // uscEvents
            // 
            this.uscEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uscEvents.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscEvents.Location = new System.Drawing.Point(9, 9);
            this.uscEvents.Name = "uscEvents";
            this.uscEvents.Size = new System.Drawing.Size(1360, 917);
            this.uscEvents.TabIndex = 0;
            // 
            // tabLists
            // 
            this.tabLists.Controls.Add(this.uscLists);
            this.tabLists.Location = new System.Drawing.Point(4, 24);
            this.tabLists.Name = "tabLists";
            this.tabLists.Padding = new System.Windows.Forms.Padding(9);
            this.tabLists.Size = new System.Drawing.Size(1378, 935);
            this.tabLists.TabIndex = 4;
            this.tabLists.Text = "Lists";
            this.tabLists.UseVisualStyleBackColor = true;
            // 
            // uscLists
            // 
            this.uscLists.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uscLists.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscLists.Location = new System.Drawing.Point(9, 9);
            this.uscLists.Name = "uscLists";
            this.uscLists.Size = new System.Drawing.Size(1360, 917);
            this.uscLists.TabIndex = 0;
            // 
            // tabServerSettings
            // 
            this.tabServerSettings.Controls.Add(this.uscSettings);
            this.tabServerSettings.Location = new System.Drawing.Point(4, 24);
            this.tabServerSettings.Name = "tabServerSettings";
            this.tabServerSettings.Padding = new System.Windows.Forms.Padding(9);
            this.tabServerSettings.Size = new System.Drawing.Size(1378, 935);
            this.tabServerSettings.TabIndex = 5;
            this.tabServerSettings.Text = "Server Settings";
            this.tabServerSettings.UseVisualStyleBackColor = true;
            // 
            // uscSettings
            // 
            this.uscSettings.BackColor = System.Drawing.Color.Transparent;
            this.uscSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uscSettings.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscSettings.Location = new System.Drawing.Point(9, 9);
            this.uscSettings.Name = "uscSettings";
            this.uscSettings.Padding = new System.Windows.Forms.Padding(10);
            this.uscSettings.Size = new System.Drawing.Size(1360, 917);
            this.uscSettings.TabIndex = 0;
            // 
            // tabPlugins
            // 
            this.tabPlugins.Controls.Add(this.uscPlugins);
            this.tabPlugins.Location = new System.Drawing.Point(4, 24);
            this.tabPlugins.Name = "tabPlugins";
            this.tabPlugins.Padding = new System.Windows.Forms.Padding(9);
            this.tabPlugins.Size = new System.Drawing.Size(1378, 935);
            this.tabPlugins.TabIndex = 2;
            this.tabPlugins.Text = "Plugins";
            this.tabPlugins.UseVisualStyleBackColor = true;
            // 
            // uscPlugins
            // 
            this.uscPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uscPlugins.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscPlugins.LocalPlugins = false;
            this.uscPlugins.Location = new System.Drawing.Point(9, 9);
            this.uscPlugins.Name = "uscPlugins";
            this.uscPlugins.Size = new System.Drawing.Size(1360, 917);
            this.uscPlugins.TabIndex = 1;
            // 
            // tabAccounts
            // 
            this.tabAccounts.Controls.Add(this.uscAccounts);
            this.tabAccounts.Location = new System.Drawing.Point(4, 24);
            this.tabAccounts.Name = "tabAccounts";
            this.tabAccounts.Padding = new System.Windows.Forms.Padding(9);
            this.tabAccounts.Size = new System.Drawing.Size(1378, 935);
            this.tabAccounts.TabIndex = 6;
            this.tabAccounts.Text = "Accounts";
            this.tabAccounts.UseVisualStyleBackColor = true;
            // 
            // uscAccounts
            // 
            this.uscAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uscAccounts.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscAccounts.Location = new System.Drawing.Point(9, 9);
            this.uscAccounts.Name = "uscAccounts";
            this.uscAccounts.Size = new System.Drawing.Size(1360, 917);
            this.uscAccounts.TabIndex = 0;
            // 
            // tabConsole
            // 
            this.tabConsole.Controls.Add(this.uscServerConsole);
            this.tabConsole.Location = new System.Drawing.Point(4, 24);
            this.tabConsole.Name = "tabConsole";
            this.tabConsole.Padding = new System.Windows.Forms.Padding(9);
            this.tabConsole.Size = new System.Drawing.Size(1378, 935);
            this.tabConsole.TabIndex = 3;
            this.tabConsole.Text = "Console";
            this.tabConsole.UseVisualStyleBackColor = true;
            // 
            // uscServerConsole
            // 
            this.uscServerConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uscServerConsole.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscServerConsole.Location = new System.Drawing.Point(9, 9);
            this.uscServerConsole.Name = "uscServerConsole";
            this.uscServerConsole.Size = new System.Drawing.Size(1360, 917);
            this.uscServerConsole.TabIndex = 0;
            // 
            // lblCurrentMapName
            // 
            this.lblCurrentMapName.AutoSize = true;
            this.lblCurrentMapName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentMapName.Location = new System.Drawing.Point(3, 0);
            this.lblCurrentMapName.Name = "lblCurrentMapName";
            this.lblCurrentMapName.Size = new System.Drawing.Size(106, 15);
            this.lblCurrentMapName.TabIndex = 5;
            this.lblCurrentMapName.Text = "Map: blahblahblah";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblVersion.Location = new System.Drawing.Point(386, 0);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(65, 15);
            this.lblVersion.TabIndex = 6;
            this.lblVersion.Text = "Version: R6";
            // 
            // tmrPortCheckTester
            // 
            this.tmrPortCheckTester.Interval = 88;
            // 
            // lblCurrentRound
            // 
            this.lblCurrentRound.AutoSize = true;
            this.lblCurrentRound.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentRound.Location = new System.Drawing.Point(115, 0);
            this.lblCurrentRound.Name = "lblCurrentRound";
            this.lblCurrentRound.Size = new System.Drawing.Size(71, 15);
            this.lblCurrentRound.TabIndex = 8;
            this.lblCurrentRound.Text = "Round: 1 / 5";
            this.lblCurrentRound.MouseEnter += new System.EventHandler(this.lblCurrentRound_MouseEnter);
            this.lblCurrentRound.MouseLeave += new System.EventHandler(this.lblCurrentRound_MouseLeave);
            // 
            // uscLogin
            // 
            this.uscLogin.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscLogin.Location = new System.Drawing.Point(887, 3);
            this.uscLogin.Name = "uscLogin";
            this.uscLogin.Size = new System.Drawing.Size(182, 78);
            this.uscLogin.TabIndex = 7;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.lblCurrentMapName);
            this.flowLayoutPanel1.Controls.Add(this.lblCurrentRound);
            this.flowLayoutPanel1.Controls.Add(this.lblRoundTime);
            this.flowLayoutPanel1.Controls.Add(this.pnlMapControls);
            this.flowLayoutPanel1.Controls.Add(this.lblPlasmaStatus);
            this.flowLayoutPanel1.Controls.Add(this.lblVersion);
            this.flowLayoutPanel1.Controls.Add(this.lblLayerVersion);
            this.flowLayoutPanel1.Controls.Add(this.lblMappack);
            this.flowLayoutPanel1.Controls.Add(this.lblServerUptime);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(8, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1382, 19);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // lblRoundTime
            // 
            this.lblRoundTime.AutoSize = true;
            this.lblRoundTime.Location = new System.Drawing.Point(192, 0);
            this.lblRoundTime.Name = "lblRoundTime";
            this.lblRoundTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblRoundTime.Size = new System.Drawing.Size(13, 15);
            this.lblRoundTime.TabIndex = 121;
            this.lblRoundTime.Text = "  ";
            // 
            // pnlMapControls
            // 
            this.pnlMapControls.Controls.Add(this.picRestartRound);
            this.pnlMapControls.Controls.Add(this.btnRestartRound);
            this.pnlMapControls.Controls.Add(this.picNextRound);
            this.pnlMapControls.Controls.Add(this.btnNextRound);
            this.pnlMapControls.Location = new System.Drawing.Point(208, 0);
            this.pnlMapControls.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMapControls.Name = "pnlMapControls";
            this.pnlMapControls.Size = new System.Drawing.Size(67, 16);
            this.pnlMapControls.TabIndex = 118;
            // 
            // picRestartRound
            // 
            this.picRestartRound.Location = new System.Drawing.Point(3, 3);
            this.picRestartRound.Name = "picRestartRound";
            this.picRestartRound.Size = new System.Drawing.Size(8, 8);
            this.picRestartRound.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picRestartRound.TabIndex = 116;
            this.picRestartRound.TabStop = false;
            // 
            // btnRestartRound
            // 
            this.btnRestartRound.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRestartRound.FlatAppearance.BorderSize = 0;
            this.btnRestartRound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestartRound.Location = new System.Drawing.Point(14, 0);
            this.btnRestartRound.Margin = new System.Windows.Forms.Padding(0);
            this.btnRestartRound.Name = "btnRestartRound";
            this.btnRestartRound.Size = new System.Drawing.Size(19, 16);
            this.btnRestartRound.TabIndex = 114;
            this.toolTipMapControls.SetToolTip(this.btnRestartRound, "Restart round");
            this.btnRestartRound.UseVisualStyleBackColor = true;
            this.btnRestartRound.Click += new System.EventHandler(this.btnRestartRound_Click);
            // 
            // picNextRound
            // 
            this.picNextRound.Location = new System.Drawing.Point(36, 3);
            this.picNextRound.Name = "picNextRound";
            this.picNextRound.Size = new System.Drawing.Size(8, 8);
            this.picNextRound.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picNextRound.TabIndex = 117;
            this.picNextRound.TabStop = false;
            // 
            // btnNextRound
            // 
            this.btnNextRound.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnNextRound.FlatAppearance.BorderSize = 0;
            this.btnNextRound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextRound.Location = new System.Drawing.Point(47, 0);
            this.btnNextRound.Margin = new System.Windows.Forms.Padding(0);
            this.btnNextRound.Name = "btnNextRound";
            this.btnNextRound.Size = new System.Drawing.Size(19, 16);
            this.btnNextRound.TabIndex = 115;
            this.toolTipMapControls.SetToolTip(this.btnNextRound, "Next round");
            this.btnNextRound.UseVisualStyleBackColor = true;
            this.btnNextRound.Click += new System.EventHandler(this.btnNextRound_Click);
            // 
            // lblPlasmaStatus
            // 
            this.lblPlasmaStatus.AutoSize = true;
            this.lblPlasmaStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlasmaStatus.Location = new System.Drawing.Point(278, 0);
            this.lblPlasmaStatus.Name = "lblPlasmaStatus";
            this.lblPlasmaStatus.Size = new System.Drawing.Size(102, 15);
            this.lblPlasmaStatus.TabIndex = 9;
            this.lblPlasmaStatus.Text = "AcceptingPlayers";
            this.toolTipPlasma.SetToolTip(this.lblPlasmaStatus, "the game server is connected to the Plasma backend, visible in the server browser" +
        ", and players can join the server");
            // 
            // lblLayerVersion
            // 
            this.lblLayerVersion.AutoSize = true;
            this.lblLayerVersion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLayerVersion.Location = new System.Drawing.Point(454, 0);
            this.lblLayerVersion.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblLayerVersion.Name = "lblLayerVersion";
            this.lblLayerVersion.Size = new System.Drawing.Size(28, 15);
            this.lblLayerVersion.TabIndex = 120;
            this.lblLayerVersion.Text = "       ";
            this.lblLayerVersion.Visible = false;
            // 
            // lblServerUptime
            // 
            this.lblServerUptime.AutoSize = true;
            this.lblServerUptime.Location = new System.Drawing.Point(562, 0);
            this.lblServerUptime.Name = "lblServerUptime";
            this.lblServerUptime.Size = new System.Drawing.Size(25, 15);
            this.lblServerUptime.TabIndex = 122;
            this.lblServerUptime.Text = "      ";
            this.lblServerUptime.Visible = false;
            // 
            // lblMappack
            // 
            this.lblMappack.AutoSize = true;
            this.lblMappack.Location = new System.Drawing.Point(488, 0);
            this.lblMappack.Name = "lblMappack";
            this.lblMappack.Size = new System.Drawing.Size(68, 15);
            this.lblMappack.TabIndex = 123;
            this.lblMappack.Text = "Mappack: 1";
            // 
            // toolTipPlasma
            // 
            this.toolTipPlasma.IsBalloon = true;
            // 
            // tmrTimerTicks
            // 
            this.tmrTimerTicks.Enabled = true;
            this.tmrTimerTicks.Interval = 1000;
            this.tmrTimerTicks.Tick += new System.EventHandler(this.tmrTimerTicks_Tick);
            // 
            // uscServerConnection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uscLogin);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.tbcClientTabs);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "uscServerConnection";
            this.Size = new System.Drawing.Size(1400, 998);
            this.Load += new System.EventHandler(this.uscServerConnection_Load);
            this.Resize += new System.EventHandler(this.uscServerConnection_Resize);
            this.tbcClientTabs.ResumeLayout(false);
            this.tabPlayerList.ResumeLayout(false);
            this.tabChat.ResumeLayout(false);
            this.tabChat.PerformLayout();
            this.pnlChatEnclosure.ResumeLayout(false);
            this.tabMapView.ResumeLayout(false);
            this.tabEvents.ResumeLayout(false);
            this.tabLists.ResumeLayout(false);
            this.tabServerSettings.ResumeLayout(false);
            this.tabPlugins.ResumeLayout(false);
            this.tabAccounts.ResumeLayout(false);
            this.tabConsole.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.pnlMapControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picRestartRound)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picNextRound)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbcClientTabs;
        private System.Windows.Forms.TabPage tabPlayerList;
        private System.Windows.Forms.TabPage tabPlugins;
        private System.Windows.Forms.TabPage tabConsole;
        private System.Windows.Forms.TabPage tabLists;
        private System.Windows.Forms.TabPage tabChat;
        private System.Windows.Forms.Label lblCurrentMapName;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblRecipientUser;
        private System.Windows.Forms.ComboBox cboRecipient;
        private System.Windows.Forms.Panel pnlChatEnclosure;
        private System.Windows.Forms.RichTextBox rtbChatBox;
        private System.Windows.Forms.TabPage tabServerSettings;
        private System.Windows.Forms.TabPage tabAccounts;
        private System.Windows.Forms.Timer tmrPortCheckTester;
        private uscPlayerListPanel uscPlayers;
        private uscConsolePanel uscServerConsole;
        private uscServerSettingsPanel uscSettings;
        private uscListControlPanel uscLists;
        private uscChatPanel uscChat;
        private uscAccountsPanel uscAccounts;
        private uscLoginPanel uscLogin;
        private System.Windows.Forms.TabPage tabEvents;
        private uscEventsPanel uscEvents;
        private System.Windows.Forms.Label lblCurrentRound;
        private System.Windows.Forms.TabPage tabMapView;
        private PRoCon.Controls.uscMapViewer uscMap;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label lblPlasmaStatus;
        private System.Windows.Forms.ToolTip toolTipPlasma;
        private System.Windows.Forms.Button btnRestartRound;
        private System.Windows.Forms.Button btnNextRound;
        private System.Windows.Forms.PictureBox picRestartRound;
        private System.Windows.Forms.PictureBox picNextRound;
        private System.Windows.Forms.FlowLayoutPanel pnlMapControls;
        private System.Windows.Forms.ToolTip toolTipMapControls;
        private System.Windows.Forms.Label lblLayerVersion;
        private uscPluginPanel uscPlugins;
        private System.Windows.Forms.Label lblRoundTime;
        private System.Windows.Forms.Timer tmrTimerTicks;
        private System.Windows.Forms.Label lblServerUptime;
        private System.Windows.Forms.Label lblMappack;
    }
}
