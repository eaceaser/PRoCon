namespace PRoCon {
    partial class uscServerSettingsPanel {
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
            this.cboSelectedSettingsPanel = new System.Windows.Forms.ComboBox();
            this.pnlSettingsPanels = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // cboSelectedSettingsPanel
            // 
            this.cboSelectedSettingsPanel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSelectedSettingsPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboSelectedSettingsPanel.FormattingEnabled = true;
            this.cboSelectedSettingsPanel.Location = new System.Drawing.Point(13, 13);
            this.cboSelectedSettingsPanel.Name = "cboSelectedSettingsPanel";
            this.cboSelectedSettingsPanel.Size = new System.Drawing.Size(365, 23);
            this.cboSelectedSettingsPanel.TabIndex = 0;
            this.cboSelectedSettingsPanel.SelectedIndexChanged += new System.EventHandler(this.cboSelectedSettingsPanel_SelectedIndexChanged);
            // 
            // pnlSettingsPanels
            // 
            this.pnlSettingsPanels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSettingsPanels.Location = new System.Drawing.Point(13, 42);
            this.pnlSettingsPanels.Name = "pnlSettingsPanels";
            this.pnlSettingsPanels.Size = new System.Drawing.Size(987, 449);
            this.pnlSettingsPanels.TabIndex = 1;
            // 
            // uscServerSettingsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlSettingsPanels);
            this.Controls.Add(this.cboSelectedSettingsPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "uscServerSettingsPanel";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(1013, 504);
            this.Load += new System.EventHandler(this.uscServerSettingsPanel_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboSelectedSettingsPanel;
        private System.Windows.Forms.Panel pnlSettingsPanels;

    }
}
