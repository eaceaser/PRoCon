namespace PRoCon {
    partial class uscPluginPanel {
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
            this.webDescription = new System.Windows.Forms.WebBrowser();
            this.ppgScriptSettings = new System.Windows.Forms.PropertyGrid();
            this.lblLoadedPlugins = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rtbScriptConsole = new PRoCon.CodRichTextBox();
            this.tabPluginDetails = new System.Windows.Forms.TabPage();
            this.spltPlugins = new System.Windows.Forms.SplitContainer();
            this.lnkReloadPlugins = new System.Windows.Forms.LinkLabel();
            this.lnkMorePlugins = new System.Windows.Forms.LinkLabel();
            this.lsvLoadedPlugins = new PRoCon.Controls.ControlsEx.ListViewNF();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tbcPluginDetails = new System.Windows.Forms.TabControl();
            this.tabPluginSettings = new System.Windows.Forms.TabPage();
            this.panel1.SuspendLayout();
            this.tabPluginDetails.SuspendLayout();
            this.spltPlugins.Panel1.SuspendLayout();
            this.spltPlugins.Panel2.SuspendLayout();
            this.spltPlugins.SuspendLayout();
            this.tbcPluginDetails.SuspendLayout();
            this.tabPluginSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // webDescription
            // 
            this.webDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webDescription.Location = new System.Drawing.Point(3, 3);
            this.webDescription.MinimumSize = new System.Drawing.Size(20, 20);
            this.webDescription.Name = "webDescription";
            this.webDescription.Size = new System.Drawing.Size(393, 232);
            this.webDescription.TabIndex = 0;
            // 
            // ppgScriptSettings
            // 
            this.ppgScriptSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ppgScriptSettings.HelpVisible = false;
            this.ppgScriptSettings.Location = new System.Drawing.Point(3, 3);
            this.ppgScriptSettings.Name = "ppgScriptSettings";
            this.ppgScriptSettings.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.ppgScriptSettings.Size = new System.Drawing.Size(393, 235);
            this.ppgScriptSettings.TabIndex = 3;
            this.ppgScriptSettings.ToolbarVisible = false;
            this.ppgScriptSettings.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.ppgScriptSettings_PropertyValueChanged);
            // 
            // lblLoadedPlugins
            // 
            this.lblLoadedPlugins.AutoSize = true;
            this.lblLoadedPlugins.Location = new System.Drawing.Point(0, 8);
            this.lblLoadedPlugins.Name = "lblLoadedPlugins";
            this.lblLoadedPlugins.Size = new System.Drawing.Size(88, 15);
            this.lblLoadedPlugins.TabIndex = 17;
            this.lblLoadedPlugins.Text = "Loaded plugins";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.rtbScriptConsole);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(659, 238);
            this.panel1.TabIndex = 14;
            // 
            // rtbScriptConsole
            // 
            this.rtbScriptConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbScriptConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbScriptConsole.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rtbScriptConsole.Location = new System.Drawing.Point(0, 0);
            this.rtbScriptConsole.Name = "rtbScriptConsole";
            this.rtbScriptConsole.ReadOnly = true;
            this.rtbScriptConsole.Size = new System.Drawing.Size(657, 236);
            this.rtbScriptConsole.TabIndex = 0;
            this.rtbScriptConsole.Text = "";
            // 
            // tabPluginDetails
            // 
            this.tabPluginDetails.Controls.Add(this.webDescription);
            this.tabPluginDetails.Location = new System.Drawing.Point(4, 24);
            this.tabPluginDetails.Name = "tabPluginDetails";
            this.tabPluginDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tabPluginDetails.Size = new System.Drawing.Size(399, 238);
            this.tabPluginDetails.TabIndex = 2;
            this.tabPluginDetails.Text = "Details";
            this.tabPluginDetails.UseVisualStyleBackColor = true;
            // 
            // spltPlugins
            // 
            this.spltPlugins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltPlugins.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spltPlugins.Location = new System.Drawing.Point(0, 0);
            this.spltPlugins.Name = "spltPlugins";
            this.spltPlugins.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spltPlugins.Panel1
            // 
            this.spltPlugins.Panel1.Controls.Add(this.lnkReloadPlugins);
            this.spltPlugins.Panel1.Controls.Add(this.lnkMorePlugins);
            this.spltPlugins.Panel1.Controls.Add(this.lsvLoadedPlugins);
            this.spltPlugins.Panel1.Controls.Add(this.tbcPluginDetails);
            this.spltPlugins.Panel1.Controls.Add(this.lblLoadedPlugins);
            this.spltPlugins.Panel1MinSize = 250;
            // 
            // spltPlugins.Panel2
            // 
            this.spltPlugins.Panel2.Controls.Add(this.panel1);
            this.spltPlugins.Panel2.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.spltPlugins.Size = new System.Drawing.Size(664, 520);
            this.spltPlugins.SplitterDistance = 277;
            this.spltPlugins.SplitterWidth = 5;
            this.spltPlugins.TabIndex = 18;
            // 
            // lnkReloadPlugins
            // 
            this.lnkReloadPlugins.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkReloadPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkReloadPlugins.AutoSize = true;
            this.lnkReloadPlugins.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkReloadPlugins.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkReloadPlugins.Location = new System.Drawing.Point(3, 257);
            this.lnkReloadPlugins.Name = "lnkReloadPlugins";
            this.lnkReloadPlugins.Size = new System.Drawing.Size(85, 15);
            this.lnkReloadPlugins.TabIndex = 19;
            this.lnkReloadPlugins.TabStop = true;
            this.lnkReloadPlugins.Text = "Reload plugins";
            this.lnkReloadPlugins.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkReloadPlugins_LinkClicked);
            // 
            // lnkMorePlugins
            // 
            this.lnkMorePlugins.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkMorePlugins.AutoSize = true;
            this.lnkMorePlugins.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkMorePlugins.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkMorePlugins.Location = new System.Drawing.Point(163, 8);
            this.lnkMorePlugins.Name = "lnkMorePlugins";
            this.lnkMorePlugins.Size = new System.Drawing.Size(85, 15);
            this.lnkMorePlugins.TabIndex = 21;
            this.lnkMorePlugins.TabStop = true;
            this.lnkMorePlugins.Text = "Reload plugins";
            this.lnkMorePlugins.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkMorePlugins.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkMorePlugins_LinkClicked);
            // 
            // lsvLoadedPlugins
            // 
            this.lsvLoadedPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.lsvLoadedPlugins.CheckBoxes = true;
            this.lsvLoadedPlugins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lsvLoadedPlugins.FullRowSelect = true;
            this.lsvLoadedPlugins.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lsvLoadedPlugins.HideSelection = false;
            this.lsvLoadedPlugins.Location = new System.Drawing.Point(3, 27);
            this.lsvLoadedPlugins.MultiSelect = false;
            this.lsvLoadedPlugins.Name = "lsvLoadedPlugins";
            this.lsvLoadedPlugins.Size = new System.Drawing.Size(245, 226);
            this.lsvLoadedPlugins.TabIndex = 20;
            this.lsvLoadedPlugins.UseCompatibleStateImageBehavior = false;
            this.lsvLoadedPlugins.View = System.Windows.Forms.View.Details;
            this.lsvLoadedPlugins.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lsvLoadedPlugins_ItemChecked);
            this.lsvLoadedPlugins.SelectedIndexChanged += new System.EventHandler(this.lsvLoadedPlugins_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 186;
            // 
            // tbcPluginDetails
            // 
            this.tbcPluginDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcPluginDetails.Controls.Add(this.tabPluginDetails);
            this.tbcPluginDetails.Controls.Add(this.tabPluginSettings);
            this.tbcPluginDetails.Location = new System.Drawing.Point(254, 8);
            this.tbcPluginDetails.Name = "tbcPluginDetails";
            this.tbcPluginDetails.SelectedIndex = 0;
            this.tbcPluginDetails.Size = new System.Drawing.Size(407, 266);
            this.tbcPluginDetails.TabIndex = 18;
            this.tbcPluginDetails.SelectedIndexChanged += new System.EventHandler(this.tbcPluginDetails_SelectedIndexChanged);
            // 
            // tabPluginSettings
            // 
            this.tabPluginSettings.Controls.Add(this.ppgScriptSettings);
            this.tabPluginSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPluginSettings.Name = "tabPluginSettings";
            this.tabPluginSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPluginSettings.Size = new System.Drawing.Size(399, 241);
            this.tabPluginSettings.TabIndex = 1;
            this.tabPluginSettings.Text = "Plugin Settings";
            this.tabPluginSettings.UseVisualStyleBackColor = true;
            // 
            // uscPluginPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spltPlugins);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "uscPluginPanel";
            this.Size = new System.Drawing.Size(664, 520);
            this.Resize += new System.EventHandler(this.uscPluginPanel_Resize);
            this.panel1.ResumeLayout(false);
            this.tabPluginDetails.ResumeLayout(false);
            this.spltPlugins.Panel1.ResumeLayout(false);
            this.spltPlugins.Panel1.PerformLayout();
            this.spltPlugins.Panel2.ResumeLayout(false);
            this.spltPlugins.ResumeLayout(false);
            this.tbcPluginDetails.ResumeLayout(false);
            this.tabPluginSettings.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webDescription;
        private CodRichTextBox rtbScriptConsole;
        private System.Windows.Forms.PropertyGrid ppgScriptSettings;
        private Controls.ControlsEx.ListViewNF lsvLoadedPlugins;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label lblLoadedPlugins;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tabPluginDetails;
        private System.Windows.Forms.SplitContainer spltPlugins;
        private System.Windows.Forms.LinkLabel lnkMorePlugins;
        private System.Windows.Forms.LinkLabel lnkReloadPlugins;
        private System.Windows.Forms.TabControl tbcPluginDetails;
        private System.Windows.Forms.TabPage tabPluginSettings;

    }
}
