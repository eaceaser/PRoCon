namespace PRoCon.Controls.Maplist {
    partial class uscMaplist {
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
            this.picMaplistAlterMaplist = new System.Windows.Forms.PictureBox();
            this.picMaplistChangePlaylist = new System.Windows.Forms.PictureBox();
            this.cboMaplistPlaylists = new System.Windows.Forms.ComboBox();
            this.lblMaplistCurrentPlayList = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lnkMaplistChangePlaylist = new System.Windows.Forms.LinkLabel();
            this.lblMaplistCurrentMaplist = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblMaplistRounds = new System.Windows.Forms.Label();
            this.numRoundsSelect = new System.Windows.Forms.NumericUpDown();
            this.lblMaplistPool = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lblMaplistMustChangeWarning = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlMaplistAddMap = new System.Windows.Forms.Panel();
            this.btnRemoveMap = new System.Windows.Forms.Button();
            this.btnAddMap = new System.Windows.Forms.Button();
            this.picMaplistAppendMap = new System.Windows.Forms.PictureBox();
            this.lsvMaplistPool = new PRoCon.Controls.ControlsEx.ListViewNF();
            this.colPoolGameType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPoolMapname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPoolMapFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lsvMaplist = new PRoCon.Controls.ControlsEx.ListViewNF();
            this.colMapPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colGameType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMapname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMapFilename = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMapRounds = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.picMaplistAlterMaplist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMaplistChangePlaylist)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRoundsSelect)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlMaplistAddMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMaplistAppendMap)).BeginInit();
            this.SuspendLayout();
            // 
            // picMaplistAlterMaplist
            // 
            this.picMaplistAlterMaplist.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picMaplistAlterMaplist.Location = new System.Drawing.Point(17, -8);
            this.picMaplistAlterMaplist.Name = "picMaplistAlterMaplist";
            this.picMaplistAlterMaplist.Size = new System.Drawing.Size(19, 18);
            this.picMaplistAlterMaplist.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMaplistAlterMaplist.TabIndex = 91;
            this.picMaplistAlterMaplist.TabStop = false;
            // 
            // picMaplistChangePlaylist
            // 
            this.picMaplistChangePlaylist.Location = new System.Drawing.Point(9, 46);
            this.picMaplistChangePlaylist.Name = "picMaplistChangePlaylist";
            this.picMaplistChangePlaylist.Size = new System.Drawing.Size(16, 16);
            this.picMaplistChangePlaylist.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMaplistChangePlaylist.TabIndex = 90;
            this.picMaplistChangePlaylist.TabStop = false;
            // 
            // cboMaplistPlaylists
            // 
            this.cboMaplistPlaylists.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cboMaplistPlaylists.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMaplistPlaylists.FormattingEnabled = true;
            this.cboMaplistPlaylists.Location = new System.Drawing.Point(31, 42);
            this.cboMaplistPlaylists.Name = "cboMaplistPlaylists";
            this.cboMaplistPlaylists.Size = new System.Drawing.Size(366, 24);
            this.cboMaplistPlaylists.TabIndex = 0;
            this.cboMaplistPlaylists.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboMaplistPlaylists_DrawItem);
            this.cboMaplistPlaylists.SelectedIndexChanged += new System.EventHandler(this.cboMaplistPlaylists_SelectedIndexChanged);
            // 
            // lblMaplistCurrentPlayList
            // 
            this.lblMaplistCurrentPlayList.AutoSize = true;
            this.lblMaplistCurrentPlayList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMaplistCurrentPlayList.Location = new System.Drawing.Point(30, 24);
            this.lblMaplistCurrentPlayList.Name = "lblMaplistCurrentPlayList";
            this.lblMaplistCurrentPlayList.Size = new System.Drawing.Size(94, 15);
            this.lblMaplistCurrentPlayList.TabIndex = 1;
            this.lblMaplistCurrentPlayList.Text = "Current play list";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Location = new System.Drawing.Point(39, 32);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(915, 1);
            this.panel1.TabIndex = 16;
            // 
            // lnkMaplistChangePlaylist
            // 
            this.lnkMaplistChangePlaylist.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkMaplistChangePlaylist.AutoSize = true;
            this.lnkMaplistChangePlaylist.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkMaplistChangePlaylist.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkMaplistChangePlaylist.Location = new System.Drawing.Point(404, 45);
            this.lnkMaplistChangePlaylist.Name = "lnkMaplistChangePlaylist";
            this.lnkMaplistChangePlaylist.Size = new System.Drawing.Size(88, 15);
            this.lnkMaplistChangePlaylist.TabIndex = 1;
            this.lnkMaplistChangePlaylist.TabStop = true;
            this.lnkMaplistChangePlaylist.Text = "Change playlist";
            this.lnkMaplistChangePlaylist.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMaplistChangePlaylist_LinkClicked);
            // 
            // lblMaplistCurrentMaplist
            // 
            this.lblMaplistCurrentMaplist.AutoSize = true;
            this.lblMaplistCurrentMaplist.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMaplistCurrentMaplist.Location = new System.Drawing.Point(495, 22);
            this.lblMaplistCurrentMaplist.Name = "lblMaplistCurrentMaplist";
            this.lblMaplistCurrentMaplist.Size = new System.Drawing.Size(93, 15);
            this.lblMaplistCurrentMaplist.TabIndex = 14;
            this.lblMaplistCurrentMaplist.Text = "Current maplist";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel2.Controls.Add(this.picMaplistAlterMaplist);
            this.panel2.Location = new System.Drawing.Point(499, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(455, 1);
            this.panel2.TabIndex = 17;
            // 
            // lblMaplistRounds
            // 
            this.lblMaplistRounds.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMaplistRounds.AutoSize = true;
            this.lblMaplistRounds.Location = new System.Drawing.Point(21, 285);
            this.lblMaplistRounds.Name = "lblMaplistRounds";
            this.lblMaplistRounds.Size = new System.Drawing.Size(47, 15);
            this.lblMaplistRounds.TabIndex = 111;
            this.lblMaplistRounds.Text = "Rounds";
            // 
            // numRoundsSelect
            // 
            this.numRoundsSelect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.numRoundsSelect.Location = new System.Drawing.Point(21, 303);
            this.numRoundsSelect.Name = "numRoundsSelect";
            this.numRoundsSelect.Size = new System.Drawing.Size(51, 23);
            this.numRoundsSelect.TabIndex = 110;
            // 
            // lblMaplistPool
            // 
            this.lblMaplistPool.AutoSize = true;
            this.lblMaplistPool.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMaplistPool.Location = new System.Drawing.Point(28, 22);
            this.lblMaplistPool.Name = "lblMaplistPool";
            this.lblMaplistPool.Size = new System.Drawing.Size(89, 15);
            this.lblMaplistPool.TabIndex = 107;
            this.lblMaplistPool.Text = "Available maps";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lblMaplistMustChangeWarning);
            this.splitContainer1.Panel1.Controls.Add(this.lblMaplistCurrentPlayList);
            this.splitContainer1.Panel1.Controls.Add(this.lnkMaplistChangePlaylist);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.cboMaplistPlaylists);
            this.splitContainer1.Panel1.Controls.Add(this.picMaplistChangePlaylist);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblMaplistPool);
            this.splitContainer1.Panel2.Controls.Add(this.panel3);
            this.splitContainer1.Panel2.Controls.Add(this.lblMaplistCurrentMaplist);
            this.splitContainer1.Panel2.Controls.Add(this.pnlMaplistAddMap);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.lsvMaplistPool);
            this.splitContainer1.Panel2.Controls.Add(this.lsvMaplist);
            this.splitContainer1.Size = new System.Drawing.Size(1001, 852);
            this.splitContainer1.SplitterDistance = 90;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 28;
            // 
            // lblMaplistMustChangeWarning
            // 
            this.lblMaplistMustChangeWarning.AutoSize = true;
            this.lblMaplistMustChangeWarning.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblMaplistMustChangeWarning.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblMaplistMustChangeWarning.Location = new System.Drawing.Point(30, 69);
            this.lblMaplistMustChangeWarning.Name = "lblMaplistMustChangeWarning";
            this.lblMaplistMustChangeWarning.Size = new System.Drawing.Size(272, 15);
            this.lblMaplistMustChangeWarning.TabIndex = 113;
            this.lblMaplistMustChangeWarning.Text = "You must change the play list to add these maps";
            this.lblMaplistMustChangeWarning.Visible = false;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Location = new System.Drawing.Point(31, 30);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(366, 1);
            this.panel3.TabIndex = 113;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.pictureBox1.Location = new System.Drawing.Point(-27, -8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(19, 18);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 91;
            this.pictureBox1.TabStop = false;
            // 
            // pnlMaplistAddMap
            // 
            this.pnlMaplistAddMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlMaplistAddMap.Controls.Add(this.btnRemoveMap);
            this.pnlMaplistAddMap.Controls.Add(this.btnAddMap);
            this.pnlMaplistAddMap.Controls.Add(this.picMaplistAppendMap);
            this.pnlMaplistAddMap.Controls.Add(this.lblMaplistRounds);
            this.pnlMaplistAddMap.Controls.Add(this.numRoundsSelect);
            this.pnlMaplistAddMap.Location = new System.Drawing.Point(403, 46);
            this.pnlMaplistAddMap.Name = "pnlMaplistAddMap";
            this.pnlMaplistAddMap.Size = new System.Drawing.Size(89, 684);
            this.pnlMaplistAddMap.TabIndex = 112;
            // 
            // btnRemoveMap
            // 
            this.btnRemoveMap.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRemoveMap.Enabled = false;
            this.btnRemoveMap.FlatAppearance.BorderSize = 0;
            this.btnRemoveMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveMap.Location = new System.Drawing.Point(24, 332);
            this.btnRemoveMap.Name = "btnRemoveMap";
            this.btnRemoveMap.Size = new System.Drawing.Size(41, 27);
            this.btnRemoveMap.TabIndex = 114;
            this.btnRemoveMap.UseVisualStyleBackColor = true;
            this.btnRemoveMap.Click += new System.EventHandler(this.btnRemoveMap_Click);
            // 
            // btnAddMap
            // 
            this.btnAddMap.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddMap.Enabled = false;
            this.btnAddMap.FlatAppearance.BorderSize = 0;
            this.btnAddMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddMap.Location = new System.Drawing.Point(24, 255);
            this.btnAddMap.Name = "btnAddMap";
            this.btnAddMap.Size = new System.Drawing.Size(41, 27);
            this.btnAddMap.TabIndex = 113;
            this.btnAddMap.UseVisualStyleBackColor = true;
            this.btnAddMap.Click += new System.EventHandler(this.btnAddMap_Click);
            // 
            // picMaplistAppendMap
            // 
            this.picMaplistAppendMap.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picMaplistAppendMap.Location = new System.Drawing.Point(35, 233);
            this.picMaplistAppendMap.Name = "picMaplistAppendMap";
            this.picMaplistAppendMap.Size = new System.Drawing.Size(16, 16);
            this.picMaplistAppendMap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picMaplistAppendMap.TabIndex = 112;
            this.picMaplistAppendMap.TabStop = false;
            // 
            // lsvMaplistPool
            // 
            this.lsvMaplistPool.AllowDrop = true;
            this.lsvMaplistPool.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lsvMaplistPool.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPoolGameType,
            this.colPoolMapname,
            this.colPoolMapFilename});
            this.lsvMaplistPool.FullRowSelect = true;
            this.lsvMaplistPool.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsvMaplistPool.HideSelection = false;
            this.lsvMaplistPool.Location = new System.Drawing.Point(33, 46);
            this.lsvMaplistPool.MultiSelect = false;
            this.lsvMaplistPool.Name = "lsvMaplistPool";
            this.lsvMaplistPool.Size = new System.Drawing.Size(364, 684);
            this.lsvMaplistPool.TabIndex = 109;
            this.lsvMaplistPool.UseCompatibleStateImageBehavior = false;
            this.lsvMaplistPool.View = System.Windows.Forms.View.Details;
            this.lsvMaplistPool.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lsvMaplistPool_ItemDrag);
            this.lsvMaplistPool.SelectedIndexChanged += new System.EventHandler(this.lsvMaplistPool_SelectedIndexChanged);
            // 
            // colPoolGameType
            // 
            this.colPoolGameType.Text = "Gametype";
            // 
            // colPoolMapname
            // 
            this.colPoolMapname.Tag = "Name";
            this.colPoolMapname.Text = "Mapname";
            this.colPoolMapname.Width = 73;
            // 
            // colPoolMapFilename
            // 
            this.colPoolMapFilename.Text = "Filename";
            // 
            // lsvMaplist
            // 
            this.lsvMaplist.AllowDrop = true;
            this.lsvMaplist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvMaplist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colMapPosition,
            this.colGameType,
            this.colMapname,
            this.colMapFilename,
            this.colMapRounds});
            this.lsvMaplist.FullRowSelect = true;
            this.lsvMaplist.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsvMaplist.HideSelection = false;
            this.lsvMaplist.LabelEdit = true;
            this.lsvMaplist.Location = new System.Drawing.Point(498, 46);
            this.lsvMaplist.MultiSelect = false;
            this.lsvMaplist.Name = "lsvMaplist";
            this.lsvMaplist.Size = new System.Drawing.Size(456, 684);
            this.lsvMaplist.TabIndex = 2;
            this.lsvMaplist.UseCompatibleStateImageBehavior = false;
            this.lsvMaplist.View = System.Windows.Forms.View.Details;
            this.lsvMaplist.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.lsvMaplist_BeforeLabelEdit);
            this.lsvMaplist.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lsvMaplist_ItemDrag);
            this.lsvMaplist.SelectedIndexChanged += new System.EventHandler(this.lsvMaplist_SelectedIndexChanged);
            this.lsvMaplist.DragDrop += new System.Windows.Forms.DragEventHandler(this.lsvMaplist_DragDrop);
            this.lsvMaplist.DragEnter += new System.Windows.Forms.DragEventHandler(this.lsvMaplist_DragEnter);
            this.lsvMaplist.DragOver += new System.Windows.Forms.DragEventHandler(this.lsvMaplist_DragOver);
            this.lsvMaplist.DragLeave += new System.EventHandler(this.lsvMaplist_DragLeave);
            this.lsvMaplist.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lsvMaplist_MouseDoubleClick);
            // 
            // colMapPosition
            // 
            this.colMapPosition.Text = "#";
            // 
            // colGameType
            // 
            this.colGameType.Text = "Gametype";
            // 
            // colMapname
            // 
            this.colMapname.Tag = "Name";
            this.colMapname.Text = "Mapname";
            this.colMapname.Width = 73;
            // 
            // colMapFilename
            // 
            this.colMapFilename.Text = "Filename";
            // 
            // colMapRounds
            // 
            this.colMapRounds.Text = "Rounds";
            // 
            // uscMaplist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "uscMaplist";
            this.Size = new System.Drawing.Size(1001, 852);
            ((System.ComponentModel.ISupportInitialize)(this.picMaplistAlterMaplist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMaplistChangePlaylist)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numRoundsSelect)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlMaplistAddMap.ResumeLayout(false);
            this.pnlMaplistAddMap.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picMaplistAppendMap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picMaplistAlterMaplist;
        private System.Windows.Forms.PictureBox picMaplistChangePlaylist;
        private System.Windows.Forms.ComboBox cboMaplistPlaylists;
        private System.Windows.Forms.Label lblMaplistCurrentPlayList;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel lnkMaplistChangePlaylist;
        private PRoCon.Controls.ControlsEx.ListViewNF lsvMaplist;
        private System.Windows.Forms.ColumnHeader colMapPosition;
        private System.Windows.Forms.ColumnHeader colMapname;
        private System.Windows.Forms.ColumnHeader colMapFilename;
        private System.Windows.Forms.ColumnHeader colMapRounds;
        private System.Windows.Forms.Label lblMaplistCurrentMaplist;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ColumnHeader colGameType;
        private System.Windows.Forms.Label lblMaplistRounds;
        private System.Windows.Forms.NumericUpDown numRoundsSelect;
        private PRoCon.Controls.ControlsEx.ListViewNF lsvMaplistPool;
        private System.Windows.Forms.ColumnHeader colPoolGameType;
        private System.Windows.Forms.ColumnHeader colPoolMapname;
        private System.Windows.Forms.ColumnHeader colPoolMapFilename;
        private System.Windows.Forms.Label lblMaplistPool;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel pnlMaplistAddMap;
        private System.Windows.Forms.Label lblMaplistMustChangeWarning;
        private System.Windows.Forms.PictureBox picMaplistAppendMap;
        private System.Windows.Forms.Button btnRemoveMap;
        private System.Windows.Forms.Button btnAddMap;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
