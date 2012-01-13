namespace PRoCon.Controls.ServerSettings {
    partial class uscServerSettingsConfigGenerator {
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
            this.btnCopyToClipboard = new System.Windows.Forms.Button();
            this.txtConfig = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnCopyToClipboard
            // 
            this.btnCopyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyToClipboard.Location = new System.Drawing.Point(392, 610);
            this.btnCopyToClipboard.Name = "btnCopyToClipboard";
            this.btnCopyToClipboard.Size = new System.Drawing.Size(420, 24);
            this.btnCopyToClipboard.TabIndex = 3;
            this.btnCopyToClipboard.Text = "Copy to Clipboard";
            this.btnCopyToClipboard.UseVisualStyleBackColor = true;
            this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
            // 
            // txtConfig
            // 
            this.txtConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConfig.BackColor = System.Drawing.SystemColors.Window;
            this.txtConfig.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConfig.Location = new System.Drawing.Point(13, 13);
            this.txtConfig.Multiline = true;
            this.txtConfig.Name = "txtConfig";
            this.txtConfig.ReadOnly = true;
            this.txtConfig.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtConfig.Size = new System.Drawing.Size(799, 591);
            this.txtConfig.TabIndex = 2;
            // 
            // uscServerSettingsConfigGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnCopyToClipboard);
            this.Controls.Add(this.txtConfig);
            this.Name = "uscServerSettingsConfigGenerator";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Size = new System.Drawing.Size(825, 647);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCopyToClipboard;
        private System.Windows.Forms.TextBox txtConfig;
    }
}
