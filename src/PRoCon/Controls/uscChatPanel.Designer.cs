namespace PRoCon {
    partial class uscChatPanel {
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
            this.pnlChatEnclosure = new System.Windows.Forms.Panel();
            this.rtbChatBox = new PRoCon.CodRichTextBox();
            this.chkDisplayScrollingEvents = new System.Windows.Forms.CheckBox();
            this.btnclearchat = new System.Windows.Forms.Button();
            this.txtChat = new System.Windows.Forms.TextBox();
            this.lblDisplay = new System.Windows.Forms.Label();
            this.btnChatSend = new System.Windows.Forms.Button();
            this.cboDisplayList = new System.Windows.Forms.ComboBox();
            this.cboPlayerList = new System.Windows.Forms.ComboBox();
            this.chkDisplayOnJoinLeaveEvents = new System.Windows.Forms.CheckBox();
            this.lblAudience = new System.Windows.Forms.Label();
            this.chkDisplayOnKilledEvents = new System.Windows.Forms.CheckBox();
            this.cboDisplayChatTime = new System.Windows.Forms.ComboBox();
            this.lblDisplayFor = new System.Windows.Forms.Label();
            this.pnlChatEnclosure.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlChatEnclosure
            // 
            this.pnlChatEnclosure.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlChatEnclosure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlChatEnclosure.Controls.Add(this.rtbChatBox);
            this.pnlChatEnclosure.Location = new System.Drawing.Point(4, 4);
            this.pnlChatEnclosure.Name = "pnlChatEnclosure";
            this.pnlChatEnclosure.Size = new System.Drawing.Size(874, 350);
            this.pnlChatEnclosure.TabIndex = 36;
            // 
            // rtbChatBox
            // 
            this.rtbChatBox.BackColor = System.Drawing.SystemColors.Window;
            this.rtbChatBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbChatBox.DetectUrls = false;
            this.rtbChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbChatBox.Location = new System.Drawing.Point(0, 0);
            this.rtbChatBox.Name = "rtbChatBox";
            this.rtbChatBox.ReadOnly = true;
            this.rtbChatBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtbChatBox.Size = new System.Drawing.Size(872, 348);
            this.rtbChatBox.TabIndex = 1;
            this.rtbChatBox.Text = "";
            // 
            // chkDisplayScrollingEvents
            // 
            this.chkDisplayScrollingEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDisplayScrollingEvents.AutoSize = true;
            this.chkDisplayScrollingEvents.Location = new System.Drawing.Point(4, 404);
            this.chkDisplayScrollingEvents.Name = "chkDisplayScrollingEvents";
            this.chkDisplayScrollingEvents.Size = new System.Drawing.Size(109, 19);
            this.chkDisplayScrollingEvents.TabIndex = 35;
            this.chkDisplayScrollingEvents.Text = "Enable scrolling";
            this.chkDisplayScrollingEvents.UseVisualStyleBackColor = true;
            this.chkDisplayScrollingEvents.CheckedChanged += new System.EventHandler(this.chkDisplayScrollingEvents_CheckedChanged);
            // 
            // btnclearchat
            // 
            this.btnclearchat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnclearchat.Location = new System.Drawing.Point(775, 404);
            this.btnclearchat.Name = "btnclearchat";
            this.btnclearchat.Size = new System.Drawing.Size(101, 23);
            this.btnclearchat.TabIndex = 34;
            this.btnclearchat.Text = "Clear Chat";
            this.btnclearchat.UseVisualStyleBackColor = true;
            this.btnclearchat.Click += new System.EventHandler(this.btnclearchat_Click);
            // 
            // txtChat
            // 
            this.txtChat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtChat.Location = new System.Drawing.Point(4, 376);
            this.txtChat.MaxLength = 100;
            this.txtChat.Name = "txtChat";
            this.txtChat.Size = new System.Drawing.Size(506, 23);
            this.txtChat.TabIndex = 23;
            this.txtChat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtChat_KeyDown);
            // 
            // lblDisplay
            // 
            this.lblDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDisplay.AutoSize = true;
            this.lblDisplay.Location = new System.Drawing.Point(606, 358);
            this.lblDisplay.Name = "lblDisplay";
            this.lblDisplay.Size = new System.Drawing.Size(45, 15);
            this.lblDisplay.TabIndex = 33;
            this.lblDisplay.Text = "Display";
            // 
            // btnChatSend
            // 
            this.btnChatSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChatSend.Location = new System.Drawing.Point(775, 375);
            this.btnChatSend.Name = "btnChatSend";
            this.btnChatSend.Size = new System.Drawing.Size(101, 23);
            this.btnChatSend.TabIndex = 24;
            this.btnChatSend.Text = "Send";
            this.btnChatSend.UseVisualStyleBackColor = true;
            this.btnChatSend.Click += new System.EventHandler(this.btnChatSend_Click);
            // 
            // cboDisplayList
            // 
            this.cboDisplayList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDisplayList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDisplayList.FormattingEnabled = true;
            this.cboDisplayList.Items.AddRange(new object[] {
            "Say",
            "Yell"});
            this.cboDisplayList.Location = new System.Drawing.Point(609, 375);
            this.cboDisplayList.Name = "cboDisplayList";
            this.cboDisplayList.Size = new System.Drawing.Size(56, 23);
            this.cboDisplayList.TabIndex = 32;
            this.cboDisplayList.SelectedIndexChanged += new System.EventHandler(this.cboDisplayList_SelectedIndexChanged);
            // 
            // cboPlayerList
            // 
            this.cboPlayerList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPlayerList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboPlayerList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlayerList.DropDownWidth = 200;
            this.cboPlayerList.FormattingEnabled = true;
            this.cboPlayerList.Location = new System.Drawing.Point(516, 376);
            this.cboPlayerList.Name = "cboPlayerList";
            this.cboPlayerList.Size = new System.Drawing.Size(87, 24);
            this.cboPlayerList.TabIndex = 22;
            this.cboPlayerList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboPlayerList_DrawItem);
            // 
            // chkDisplayOnJoinLeaveEvents
            // 
            this.chkDisplayOnJoinLeaveEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDisplayOnJoinLeaveEvents.AutoSize = true;
            this.chkDisplayOnJoinLeaveEvents.Location = new System.Drawing.Point(4, 431);
            this.chkDisplayOnJoinLeaveEvents.Name = "chkDisplayOnJoinLeaveEvents";
            this.chkDisplayOnJoinLeaveEvents.Size = new System.Drawing.Size(130, 19);
            this.chkDisplayOnJoinLeaveEvents.TabIndex = 30;
            this.chkDisplayOnJoinLeaveEvents.Text = "Display join/leaving";
            this.chkDisplayOnJoinLeaveEvents.UseVisualStyleBackColor = true;
            this.chkDisplayOnJoinLeaveEvents.CheckedChanged += new System.EventHandler(this.chkDisplayOnJoinLeaveEvents_CheckedChanged);
            // 
            // lblAudience
            // 
            this.lblAudience.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAudience.AutoSize = true;
            this.lblAudience.Location = new System.Drawing.Point(513, 358);
            this.lblAudience.Name = "lblAudience";
            this.lblAudience.Size = new System.Drawing.Size(57, 15);
            this.lblAudience.TabIndex = 26;
            this.lblAudience.Text = "Audience";
            // 
            // chkDisplayOnKilledEvents
            // 
            this.chkDisplayOnKilledEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDisplayOnKilledEvents.AutoSize = true;
            this.chkDisplayOnKilledEvents.Location = new System.Drawing.Point(4, 456);
            this.chkDisplayOnKilledEvents.Name = "chkDisplayOnKilledEvents";
            this.chkDisplayOnKilledEvents.Size = new System.Drawing.Size(127, 19);
            this.chkDisplayOnKilledEvents.TabIndex = 29;
            this.chkDisplayOnKilledEvents.Text = "Display kills/deaths";
            this.chkDisplayOnKilledEvents.UseVisualStyleBackColor = true;
            this.chkDisplayOnKilledEvents.CheckedChanged += new System.EventHandler(this.chkDisplayOnKilledEvents_CheckedChanged);
            // 
            // cboDisplayChatTime
            // 
            this.cboDisplayChatTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cboDisplayChatTime.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboDisplayChatTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDisplayChatTime.Enabled = false;
            this.cboDisplayChatTime.FormattingEnabled = true;
            this.cboDisplayChatTime.Items.AddRange(new object[] {
            "2 seconds",
            "4 seconds",
            "6 seconds",
            "8 seconds",
            "10 seconds",
            "15 seconds",
            "20 seconds",
            "25 seconds",
            "30 seconds",
            "35 seconds",
            "40 seconds",
            "45 seconds",
            "50 seconds",
            "55 seconds",
            "60 seconds"});
            this.cboDisplayChatTime.Location = new System.Drawing.Point(671, 376);
            this.cboDisplayChatTime.Name = "cboDisplayChatTime";
            this.cboDisplayChatTime.Size = new System.Drawing.Size(98, 24);
            this.cboDisplayChatTime.TabIndex = 27;
            this.cboDisplayChatTime.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboDisplayChatTime_DrawItem);
            this.cboDisplayChatTime.SelectedIndexChanged += new System.EventHandler(this.cboDisplayChatTime_SelectedIndexChanged);
            // 
            // lblDisplayFor
            // 
            this.lblDisplayFor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDisplayFor.AutoSize = true;
            this.lblDisplayFor.Enabled = false;
            this.lblDisplayFor.Location = new System.Drawing.Point(668, 358);
            this.lblDisplayFor.Name = "lblDisplayFor";
            this.lblDisplayFor.Size = new System.Drawing.Size(63, 15);
            this.lblDisplayFor.TabIndex = 28;
            this.lblDisplayFor.Text = "Display for";
            // 
            // uscChatPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlChatEnclosure);
            this.Controls.Add(this.chkDisplayScrollingEvents);
            this.Controls.Add(this.btnclearchat);
            this.Controls.Add(this.txtChat);
            this.Controls.Add(this.lblDisplay);
            this.Controls.Add(this.btnChatSend);
            this.Controls.Add(this.cboDisplayList);
            this.Controls.Add(this.cboPlayerList);
            this.Controls.Add(this.chkDisplayOnJoinLeaveEvents);
            this.Controls.Add(this.lblAudience);
            this.Controls.Add(this.chkDisplayOnKilledEvents);
            this.Controls.Add(this.cboDisplayChatTime);
            this.Controls.Add(this.lblDisplayFor);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "uscChatPanel";
            this.Size = new System.Drawing.Size(880, 482);
            this.Load += new System.EventHandler(this.uscChatPanel_Load);
            this.pnlChatEnclosure.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkDisplayOnJoinLeaveEvents;
        private System.Windows.Forms.CheckBox chkDisplayOnKilledEvents;
        private System.Windows.Forms.Label lblDisplayFor;
        private System.Windows.Forms.ComboBox cboDisplayChatTime;
        private System.Windows.Forms.Label lblAudience;
        private System.Windows.Forms.ComboBox cboPlayerList;
        private System.Windows.Forms.Button btnChatSend;
        private System.Windows.Forms.TextBox txtChat;
        private System.Windows.Forms.Label lblDisplay;
        private System.Windows.Forms.ComboBox cboDisplayList;
        private System.Windows.Forms.Button btnclearchat;
        private System.Windows.Forms.CheckBox chkDisplayScrollingEvents;
        private System.Windows.Forms.Panel pnlChatEnclosure;
        private CodRichTextBox rtbChatBox;

    }
}
