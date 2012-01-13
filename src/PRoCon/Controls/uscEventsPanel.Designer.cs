namespace PRoCon {
    partial class uscEventsPanel {
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
            this.picOpenCloseCaptures = new System.Windows.Forms.PictureBox();
            this.lnkShowHide = new System.Windows.Forms.LinkLabel();
            this.spltEvents = new System.Windows.Forms.SplitContainer();
            this.lsvEvents = new PRoCon.Controls.ControlsEx.ListViewNF();
            this.colSource = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colEvent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gpbCaptures = new System.Windows.Forms.GroupBox();
            this.lsvCapturedEvents = new System.Windows.Forms.ListView();
            this.colCapturedList = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.numMaximumEvents = new System.Windows.Forms.NumericUpDown();
            this.btncleareventbox = new System.Windows.Forms.Button();
            this.btnAddCapture = new System.Windows.Forms.Button();
            this.lblEvents = new System.Windows.Forms.Label();
            this.lblMaximumDisplayed = new System.Windows.Forms.Label();
            this.btnRemoveCapture = new System.Windows.Forms.Button();
            this.lnkAddCapture = new System.Windows.Forms.LinkLabel();
            this.lblCapturingEvents = new System.Windows.Forms.Label();
            this.cboEvents = new System.Windows.Forms.ComboBox();
            this.colAdmin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.picOpenCloseCaptures)).BeginInit();
            this.spltEvents.Panel1.SuspendLayout();
            this.spltEvents.Panel2.SuspendLayout();
            this.spltEvents.SuspendLayout();
            this.gpbCaptures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaximumEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // picOpenCloseCaptures
            // 
            this.picOpenCloseCaptures.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picOpenCloseCaptures.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picOpenCloseCaptures.Location = new System.Drawing.Point(777, 3);
            this.picOpenCloseCaptures.Name = "picOpenCloseCaptures";
            this.picOpenCloseCaptures.Size = new System.Drawing.Size(16, 16);
            this.picOpenCloseCaptures.TabIndex = 29;
            this.picOpenCloseCaptures.TabStop = false;
            this.picOpenCloseCaptures.Click += new System.EventHandler(this.picOpenCloseCaptures_Click);
            // 
            // lnkShowHide
            // 
            this.lnkShowHide.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkShowHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkShowHide.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkShowHide.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkShowHide.Location = new System.Drawing.Point(555, 3);
            this.lnkShowHide.Name = "lnkShowHide";
            this.lnkShowHide.Size = new System.Drawing.Size(222, 15);
            this.lnkShowHide.TabIndex = 28;
            this.lnkShowHide.TabStop = true;
            this.lnkShowHide.Text = "Close captures";
            this.lnkShowHide.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkShowHide.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkShowHide_LinkClicked);
            // 
            // spltEvents
            // 
            this.spltEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.spltEvents.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spltEvents.Location = new System.Drawing.Point(3, 20);
            this.spltEvents.Name = "spltEvents";
            // 
            // spltEvents.Panel1
            // 
            this.spltEvents.Panel1.Controls.Add(this.lsvEvents);
            // 
            // spltEvents.Panel2
            // 
            this.spltEvents.Panel2.Controls.Add(this.gpbCaptures);
            this.spltEvents.Size = new System.Drawing.Size(793, 560);
            this.spltEvents.SplitterDistance = 533;
            this.spltEvents.TabIndex = 27;
            // 
            // lsvEvents
            // 
            this.lsvEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colSource,
            this.colTime,
            this.colAdmin,
            this.colEvent,
            this.colMessage});
            this.lsvEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsvEvents.FullRowSelect = true;
            this.lsvEvents.GridLines = true;
            this.lsvEvents.Location = new System.Drawing.Point(0, 0);
            this.lsvEvents.Name = "lsvEvents";
            this.lsvEvents.Size = new System.Drawing.Size(533, 560);
            this.lsvEvents.TabIndex = 1;
            this.lsvEvents.UseCompatibleStateImageBehavior = false;
            this.lsvEvents.View = System.Windows.Forms.View.Details;
            this.lsvEvents.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lsvEvents_ColumnClick);
            // 
            // colSource
            // 
            this.colSource.Text = "Source";
            // 
            // colTime
            // 
            this.colTime.Text = "Date and Time";
            // 
            // colEvent
            // 
            this.colEvent.Text = "Event";
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            // 
            // gpbCaptures
            // 
            this.gpbCaptures.Controls.Add(this.lsvCapturedEvents);
            this.gpbCaptures.Controls.Add(this.numMaximumEvents);
            this.gpbCaptures.Controls.Add(this.btncleareventbox);
            this.gpbCaptures.Controls.Add(this.btnAddCapture);
            this.gpbCaptures.Controls.Add(this.lblEvents);
            this.gpbCaptures.Controls.Add(this.lblMaximumDisplayed);
            this.gpbCaptures.Controls.Add(this.btnRemoveCapture);
            this.gpbCaptures.Controls.Add(this.lnkAddCapture);
            this.gpbCaptures.Controls.Add(this.lblCapturingEvents);
            this.gpbCaptures.Controls.Add(this.cboEvents);
            this.gpbCaptures.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpbCaptures.Location = new System.Drawing.Point(0, 0);
            this.gpbCaptures.Name = "gpbCaptures";
            this.gpbCaptures.Padding = new System.Windows.Forms.Padding(8);
            this.gpbCaptures.Size = new System.Drawing.Size(256, 560);
            this.gpbCaptures.TabIndex = 2;
            this.gpbCaptures.TabStop = false;
            this.gpbCaptures.Text = "Captures";
            // 
            // lsvCapturedEvents
            // 
            this.lsvCapturedEvents.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvCapturedEvents.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCapturedList});
            this.lsvCapturedEvents.FullRowSelect = true;
            this.lsvCapturedEvents.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsvCapturedEvents.Location = new System.Drawing.Point(15, 143);
            this.lsvCapturedEvents.MultiSelect = false;
            this.lsvCapturedEvents.Name = "lsvCapturedEvents";
            this.lsvCapturedEvents.Size = new System.Drawing.Size(195, 377);
            this.lsvCapturedEvents.TabIndex = 32;
            this.lsvCapturedEvents.UseCompatibleStateImageBehavior = false;
            this.lsvCapturedEvents.View = System.Windows.Forms.View.Details;
            this.lsvCapturedEvents.SelectedIndexChanged += new System.EventHandler(this.lsvCapturedEvents_SelectedIndexChanged);
            // 
            // colCapturedList
            // 
            this.colCapturedList.Text = "Captured List";
            this.colCapturedList.Width = 186;
            // 
            // numMaximumEvents
            // 
            this.numMaximumEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.numMaximumEvents.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numMaximumEvents.Location = new System.Drawing.Point(15, 42);
            this.numMaximumEvents.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numMaximumEvents.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numMaximumEvents.Name = "numMaximumEvents";
            this.numMaximumEvents.Size = new System.Drawing.Size(196, 23);
            this.numMaximumEvents.TabIndex = 31;
            this.numMaximumEvents.Value = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numMaximumEvents.ValueChanged += new System.EventHandler(this.numMaximumEvents_ValueChanged);
            // 
            // btncleareventbox
            // 
            this.btncleareventbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btncleareventbox.Location = new System.Drawing.Point(14, 526);
            this.btncleareventbox.Name = "btncleareventbox";
            this.btncleareventbox.Size = new System.Drawing.Size(197, 23);
            this.btncleareventbox.TabIndex = 30;
            this.btncleareventbox.Text = "Clear Eventbox";
            this.btncleareventbox.UseVisualStyleBackColor = true;
            this.btncleareventbox.Click += new System.EventHandler(this.btncleareventbox_Click);
            // 
            // btnAddCapture
            // 
            this.btnAddCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddCapture.Enabled = false;
            this.btnAddCapture.FlatAppearance.BorderSize = 0;
            this.btnAddCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCapture.Location = new System.Drawing.Point(216, 92);
            this.btnAddCapture.Name = "btnAddCapture";
            this.btnAddCapture.Size = new System.Drawing.Size(28, 23);
            this.btnAddCapture.TabIndex = 29;
            this.btnAddCapture.UseVisualStyleBackColor = true;
            this.btnAddCapture.Click += new System.EventHandler(this.btnAddCapture_Click);
            // 
            // lblEvents
            // 
            this.lblEvents.AutoSize = true;
            this.lblEvents.Location = new System.Drawing.Point(12, 75);
            this.lblEvents.Name = "lblEvents";
            this.lblEvents.Size = new System.Drawing.Size(41, 15);
            this.lblEvents.TabIndex = 28;
            this.lblEvents.Text = "Events";
            // 
            // lblMaximumDisplayed
            // 
            this.lblMaximumDisplayed.AutoSize = true;
            this.lblMaximumDisplayed.Location = new System.Drawing.Point(12, 24);
            this.lblMaximumDisplayed.Name = "lblMaximumDisplayed";
            this.lblMaximumDisplayed.Size = new System.Drawing.Size(109, 15);
            this.lblMaximumDisplayed.TabIndex = 27;
            this.lblMaximumDisplayed.Text = "Maximum captures";
            // 
            // btnRemoveCapture
            // 
            this.btnRemoveCapture.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnRemoveCapture.Enabled = false;
            this.btnRemoveCapture.FlatAppearance.BorderSize = 0;
            this.btnRemoveCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveCapture.Location = new System.Drawing.Point(217, 304);
            this.btnRemoveCapture.Name = "btnRemoveCapture";
            this.btnRemoveCapture.Size = new System.Drawing.Size(28, 23);
            this.btnRemoveCapture.TabIndex = 25;
            this.btnRemoveCapture.UseVisualStyleBackColor = true;
            this.btnRemoveCapture.Click += new System.EventHandler(this.btnRemoveCapture_Click);
            // 
            // lnkAddCapture
            // 
            this.lnkAddCapture.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkAddCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkAddCapture.AutoSize = true;
            this.lnkAddCapture.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkAddCapture.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkAddCapture.Location = new System.Drawing.Point(182, 119);
            this.lnkAddCapture.Name = "lnkAddCapture";
            this.lnkAddCapture.Size = new System.Drawing.Size(29, 15);
            this.lnkAddCapture.TabIndex = 24;
            this.lnkAddCapture.TabStop = true;
            this.lnkAddCapture.Text = "Add";
            this.lnkAddCapture.Visible = false;
            // 
            // lblCapturingEvents
            // 
            this.lblCapturingEvents.AutoSize = true;
            this.lblCapturingEvents.Location = new System.Drawing.Point(11, 125);
            this.lblCapturingEvents.Name = "lblCapturingEvents";
            this.lblCapturingEvents.Size = new System.Drawing.Size(97, 15);
            this.lblCapturingEvents.TabIndex = 2;
            this.lblCapturingEvents.Text = "Capturing Events";
            // 
            // cboEvents
            // 
            this.cboEvents.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboEvents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEvents.FormattingEnabled = true;
            this.cboEvents.Location = new System.Drawing.Point(14, 93);
            this.cboEvents.Name = "cboEvents";
            this.cboEvents.Size = new System.Drawing.Size(196, 23);
            this.cboEvents.TabIndex = 0;
            this.cboEvents.SelectedIndexChanged += new System.EventHandler(this.cboEvents_SelectedIndexChanged);
            // 
            // colAdmin
            // 
            this.colAdmin.Text = "Admin";
            // 
            // uscEventsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.picOpenCloseCaptures);
            this.Controls.Add(this.lnkShowHide);
            this.Controls.Add(this.spltEvents);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "uscEventsPanel";
            this.Size = new System.Drawing.Size(796, 583);
            this.Load += new System.EventHandler(this.uscEventsPanel_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picOpenCloseCaptures)).EndInit();
            this.spltEvents.Panel1.ResumeLayout(false);
            this.spltEvents.Panel2.ResumeLayout(false);
            this.spltEvents.ResumeLayout(false);
            this.gpbCaptures.ResumeLayout(false);
            this.gpbCaptures.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMaximumEvents)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picOpenCloseCaptures;
        private System.Windows.Forms.LinkLabel lnkShowHide;
        private System.Windows.Forms.SplitContainer spltEvents;
        private PRoCon.Controls.ControlsEx.ListViewNF lsvEvents;
        private System.Windows.Forms.ColumnHeader colSource;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colEvent;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.GroupBox gpbCaptures;
        private System.Windows.Forms.Button btnAddCapture;
        private System.Windows.Forms.Label lblEvents;
        private System.Windows.Forms.Label lblMaximumDisplayed;
        private System.Windows.Forms.Button btnRemoveCapture;
        private System.Windows.Forms.LinkLabel lnkAddCapture;
        private System.Windows.Forms.Label lblCapturingEvents;
        private System.Windows.Forms.ComboBox cboEvents;
        private System.Windows.Forms.Button btncleareventbox;
        private System.Windows.Forms.NumericUpDown numMaximumEvents;
        private System.Windows.Forms.ListView lsvCapturedEvents;
        private System.Windows.Forms.ColumnHeader colCapturedList;
        private System.Windows.Forms.ColumnHeader colAdmin;


    }
}
