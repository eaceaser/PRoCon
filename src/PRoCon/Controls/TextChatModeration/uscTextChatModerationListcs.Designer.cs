namespace PRoCon.Controls.TextChatModeration {
    partial class uscTextChatModerationListcs {
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Muted", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Normal", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Voice", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Admin", System.Windows.Forms.HorizontalAlignment.Left);
            this.pnlReservedPanel = new System.Windows.Forms.Panel();
            this.btnTextChatModerationRemoveSoldier = new System.Windows.Forms.Button();
            this.lblTextChatModerationCurrent = new System.Windows.Forms.Label();
            this.pnlReservedAddSoldierName = new System.Windows.Forms.Panel();
            this.cboTextChatModerationLevels = new System.Windows.Forms.ComboBox();
            this.txtTextChatModerationAddSoldierName = new System.Windows.Forms.TextBox();
            this.picTextChatModerationAddSoldierName = new System.Windows.Forms.PictureBox();
            this.lblTextChatModerationAddSoldierName = new System.Windows.Forms.Label();
            this.lnkTextChatModerationAddSoldierName = new System.Windows.Forms.LinkLabel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.picTextChatModerationList = new System.Windows.Forms.PictureBox();
            this.panel10 = new System.Windows.Forms.Panel();
            this.lsvTextChatModerationList = new PRoCon.Controls.ControlsEx.ListViewNF();
            this.colSoldierNames = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlReservedPanel.SuspendLayout();
            this.pnlReservedAddSoldierName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTextChatModerationAddSoldierName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTextChatModerationList)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlReservedPanel
            // 
            this.pnlReservedPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.pnlReservedPanel.Controls.Add(this.btnTextChatModerationRemoveSoldier);
            this.pnlReservedPanel.Controls.Add(this.lblTextChatModerationCurrent);
            this.pnlReservedPanel.Controls.Add(this.pnlReservedAddSoldierName);
            this.pnlReservedPanel.Controls.Add(this.panel9);
            this.pnlReservedPanel.Controls.Add(this.picTextChatModerationList);
            this.pnlReservedPanel.Controls.Add(this.panel10);
            this.pnlReservedPanel.Controls.Add(this.lsvTextChatModerationList);
            this.pnlReservedPanel.Location = new System.Drawing.Point(158, 3);
            this.pnlReservedPanel.Name = "pnlReservedPanel";
            this.pnlReservedPanel.Size = new System.Drawing.Size(567, 678);
            this.pnlReservedPanel.TabIndex = 28;
            // 
            // btnTextChatModerationRemoveSoldier
            // 
            this.btnTextChatModerationRemoveSoldier.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTextChatModerationRemoveSoldier.Enabled = false;
            this.btnTextChatModerationRemoveSoldier.FlatAppearance.BorderSize = 0;
            this.btnTextChatModerationRemoveSoldier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTextChatModerationRemoveSoldier.Location = new System.Drawing.Point(492, 240);
            this.btnTextChatModerationRemoveSoldier.Name = "btnTextChatModerationRemoveSoldier";
            this.btnTextChatModerationRemoveSoldier.Size = new System.Drawing.Size(41, 27);
            this.btnTextChatModerationRemoveSoldier.TabIndex = 1;
            this.btnTextChatModerationRemoveSoldier.UseVisualStyleBackColor = true;
            this.btnTextChatModerationRemoveSoldier.Click += new System.EventHandler(this.btnTextChatModerationRemoveSoldier_Click);
            // 
            // lblTextChatModerationCurrent
            // 
            this.lblTextChatModerationCurrent.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblTextChatModerationCurrent.AutoSize = true;
            this.lblTextChatModerationCurrent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTextChatModerationCurrent.Location = new System.Drawing.Point(37, 20);
            this.lblTextChatModerationCurrent.Name = "lblTextChatModerationCurrent";
            this.lblTextChatModerationCurrent.Size = new System.Drawing.Size(147, 15);
            this.lblTextChatModerationCurrent.TabIndex = 14;
            this.lblTextChatModerationCurrent.Text = "Text chat moderation list";
            // 
            // pnlReservedAddSoldierName
            // 
            this.pnlReservedAddSoldierName.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pnlReservedAddSoldierName.Controls.Add(this.cboTextChatModerationLevels);
            this.pnlReservedAddSoldierName.Controls.Add(this.txtTextChatModerationAddSoldierName);
            this.pnlReservedAddSoldierName.Controls.Add(this.picTextChatModerationAddSoldierName);
            this.pnlReservedAddSoldierName.Controls.Add(this.lblTextChatModerationAddSoldierName);
            this.pnlReservedAddSoldierName.Controls.Add(this.lnkTextChatModerationAddSoldierName);
            this.pnlReservedAddSoldierName.Location = new System.Drawing.Point(41, 516);
            this.pnlReservedAddSoldierName.Name = "pnlReservedAddSoldierName";
            this.pnlReservedAddSoldierName.Size = new System.Drawing.Size(502, 69);
            this.pnlReservedAddSoldierName.TabIndex = 2;
            // 
            // cboTextChatModerationLevels
            // 
            this.cboTextChatModerationLevels.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTextChatModerationLevels.FormattingEnabled = true;
            this.cboTextChatModerationLevels.Items.AddRange(new object[] {
            "Muted",
            "Normal",
            "Voice",
            "Admin"});
            this.cboTextChatModerationLevels.Location = new System.Drawing.Point(275, 31);
            this.cboTextChatModerationLevels.Name = "cboTextChatModerationLevels";
            this.cboTextChatModerationLevels.Size = new System.Drawing.Size(73, 23);
            this.cboTextChatModerationLevels.TabIndex = 103;
            // 
            // txtTextChatModerationAddSoldierName
            // 
            this.txtTextChatModerationAddSoldierName.Location = new System.Drawing.Point(40, 31);
            this.txtTextChatModerationAddSoldierName.Name = "txtTextChatModerationAddSoldierName";
            this.txtTextChatModerationAddSoldierName.Size = new System.Drawing.Size(228, 23);
            this.txtTextChatModerationAddSoldierName.TabIndex = 1;
            // 
            // picTextChatModerationAddSoldierName
            // 
            this.picTextChatModerationAddSoldierName.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picTextChatModerationAddSoldierName.Location = new System.Drawing.Point(14, 9);
            this.picTextChatModerationAddSoldierName.Name = "picTextChatModerationAddSoldierName";
            this.picTextChatModerationAddSoldierName.Size = new System.Drawing.Size(19, 18);
            this.picTextChatModerationAddSoldierName.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTextChatModerationAddSoldierName.TabIndex = 102;
            this.picTextChatModerationAddSoldierName.TabStop = false;
            // 
            // lblTextChatModerationAddSoldierName
            // 
            this.lblTextChatModerationAddSoldierName.AutoSize = true;
            this.lblTextChatModerationAddSoldierName.Location = new System.Drawing.Point(36, 10);
            this.lblTextChatModerationAddSoldierName.Name = "lblTextChatModerationAddSoldierName";
            this.lblTextChatModerationAddSoldierName.Size = new System.Drawing.Size(161, 15);
            this.lblTextChatModerationAddSoldierName.TabIndex = 93;
            this.lblTextChatModerationAddSoldierName.Text = "Add a soldier name to the list";
            // 
            // lnkTextChatModerationAddSoldierName
            // 
            this.lnkTextChatModerationAddSoldierName.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkTextChatModerationAddSoldierName.AutoSize = true;
            this.lnkTextChatModerationAddSoldierName.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkTextChatModerationAddSoldierName.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkTextChatModerationAddSoldierName.Location = new System.Drawing.Point(373, 35);
            this.lnkTextChatModerationAddSoldierName.Name = "lnkTextChatModerationAddSoldierName";
            this.lnkTextChatModerationAddSoldierName.Size = new System.Drawing.Size(67, 15);
            this.lnkTextChatModerationAddSoldierName.TabIndex = 2;
            this.lnkTextChatModerationAddSoldierName.TabStop = true;
            this.lnkTextChatModerationAddSoldierName.Text = "Add soldier";
            this.lnkTextChatModerationAddSoldierName.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkTextChatModerationAddSoldierName_LinkClicked);
            // 
            // panel9
            // 
            this.panel9.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panel9.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel9.Location = new System.Drawing.Point(41, 498);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(495, 1);
            this.panel9.TabIndex = 95;
            // 
            // picTextChatModerationList
            // 
            this.picTextChatModerationList.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picTextChatModerationList.Location = new System.Drawing.Point(55, 47);
            this.picTextChatModerationList.Name = "picTextChatModerationList";
            this.picTextChatModerationList.Size = new System.Drawing.Size(19, 18);
            this.picTextChatModerationList.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picTextChatModerationList.TabIndex = 91;
            this.picTextChatModerationList.TabStop = false;
            // 
            // panel10
            // 
            this.panel10.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel10.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel10.Location = new System.Drawing.Point(41, 29);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(495, 1);
            this.panel10.TabIndex = 16;
            // 
            // lsvTextChatModerationList
            // 
            this.lsvTextChatModerationList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.lsvTextChatModerationList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSoldierNames});
            this.lsvTextChatModerationList.FullRowSelect = true;
            listViewGroup1.Header = "Muted";
            listViewGroup1.Name = "muted";
            listViewGroup2.Header = "Normal";
            listViewGroup2.Name = "normal";
            listViewGroup3.Header = "Voice";
            listViewGroup3.Name = "voice";
            listViewGroup4.Header = "Admin";
            listViewGroup4.Name = "admin";
            this.lsvTextChatModerationList.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4});
            this.lsvTextChatModerationList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsvTextChatModerationList.HideSelection = false;
            this.lsvTextChatModerationList.Location = new System.Drawing.Point(80, 47);
            this.lsvTextChatModerationList.MultiSelect = false;
            this.lsvTextChatModerationList.Name = "lsvTextChatModerationList";
            this.lsvTextChatModerationList.Size = new System.Drawing.Size(406, 431);
            this.lsvTextChatModerationList.TabIndex = 0;
            this.lsvTextChatModerationList.UseCompatibleStateImageBehavior = false;
            this.lsvTextChatModerationList.View = System.Windows.Forms.View.Details;
            this.lsvTextChatModerationList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lsvTextChatModerationList_ColumnClick);
            this.lsvTextChatModerationList.SelectedIndexChanged += new System.EventHandler(this.lsvTextChatModerationList_SelectedIndexChanged);
            // 
            // colSoldierNames
            // 
            this.colSoldierNames.Text = "Soldier Names";
            this.colSoldierNames.Width = 323;
            // 
            // uscTextChatModerationListcs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlReservedPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "uscTextChatModerationListcs";
            this.Size = new System.Drawing.Size(885, 685);
            this.pnlReservedPanel.ResumeLayout(false);
            this.pnlReservedPanel.PerformLayout();
            this.pnlReservedAddSoldierName.ResumeLayout(false);
            this.pnlReservedAddSoldierName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTextChatModerationAddSoldierName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTextChatModerationList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlReservedPanel;
        private System.Windows.Forms.Label lblTextChatModerationCurrent;
        private System.Windows.Forms.Panel pnlReservedAddSoldierName;
        private System.Windows.Forms.TextBox txtTextChatModerationAddSoldierName;
        private System.Windows.Forms.PictureBox picTextChatModerationAddSoldierName;
        private System.Windows.Forms.Label lblTextChatModerationAddSoldierName;
        private System.Windows.Forms.LinkLabel lnkTextChatModerationAddSoldierName;
        private System.Windows.Forms.Button btnTextChatModerationRemoveSoldier;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.PictureBox picTextChatModerationList;
        private System.Windows.Forms.Panel panel10;
        private PRoCon.Controls.ControlsEx.ListViewNF lsvTextChatModerationList;
        private System.Windows.Forms.ColumnHeader colSoldierNames;
        private System.Windows.Forms.ComboBox cboTextChatModerationLevels;
    }
}
