namespace PRoCon.Controls.ServerSettings {
    partial class uscServerSettingsGameplay {
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
            this.chkSettingsFriendlyFire = new System.Windows.Forms.CheckBox();
            this.picSettingsFriendlyFire = new System.Windows.Forms.PictureBox();
            this.chkSettingsHardcore = new System.Windows.Forms.CheckBox();
            this.picSettingsHardcore = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picSettingsFriendlyFire)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSettingsHardcore)).BeginInit();
            this.SuspendLayout();
            // 
            // chkSettingsFriendlyFire
            // 
            this.chkSettingsFriendlyFire.AutoSize = true;
            this.chkSettingsFriendlyFire.Location = new System.Drawing.Point(43, 47);
            this.chkSettingsFriendlyFire.Name = "chkSettingsFriendlyFire";
            this.chkSettingsFriendlyFire.Size = new System.Drawing.Size(88, 19);
            this.chkSettingsFriendlyFire.TabIndex = 218;
            this.chkSettingsFriendlyFire.Text = "Friendly fire";
            this.chkSettingsFriendlyFire.UseVisualStyleBackColor = true;
            this.chkSettingsFriendlyFire.CheckedChanged += new System.EventHandler(this.chkSettingsFriendlyFire_CheckedChanged);
            // 
            // picSettingsFriendlyFire
            // 
            this.picSettingsFriendlyFire.Location = new System.Drawing.Point(16, 47);
            this.picSettingsFriendlyFire.Name = "picSettingsFriendlyFire";
            this.picSettingsFriendlyFire.Size = new System.Drawing.Size(16, 16);
            this.picSettingsFriendlyFire.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSettingsFriendlyFire.TabIndex = 217;
            this.picSettingsFriendlyFire.TabStop = false;
            // 
            // chkSettingsHardcore
            // 
            this.chkSettingsHardcore.AutoSize = true;
            this.chkSettingsHardcore.Location = new System.Drawing.Point(43, 10);
            this.chkSettingsHardcore.Name = "chkSettingsHardcore";
            this.chkSettingsHardcore.Size = new System.Drawing.Size(75, 19);
            this.chkSettingsHardcore.TabIndex = 220;
            this.chkSettingsHardcore.Text = "Hardcore";
            this.chkSettingsHardcore.UseVisualStyleBackColor = true;
            this.chkSettingsHardcore.CheckedChanged += new System.EventHandler(this.chkSettingsHardcore_CheckedChanged);
            // 
            // picSettingsHardcore
            // 
            this.picSettingsHardcore.Location = new System.Drawing.Point(16, 12);
            this.picSettingsHardcore.Name = "picSettingsHardcore";
            this.picSettingsHardcore.Size = new System.Drawing.Size(16, 16);
            this.picSettingsHardcore.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSettingsHardcore.TabIndex = 219;
            this.picSettingsHardcore.TabStop = false;
            // 
            // uscServerSettingsGameplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkSettingsHardcore);
            this.Controls.Add(this.picSettingsHardcore);
            this.Controls.Add(this.chkSettingsFriendlyFire);
            this.Controls.Add(this.picSettingsFriendlyFire);
            this.Name = "uscServerSettingsGameplay";
            this.Size = new System.Drawing.Size(817, 189);
            ((System.ComponentModel.ISupportInitialize)(this.picSettingsFriendlyFire)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSettingsHardcore)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSettingsFriendlyFire;
        private System.Windows.Forms.PictureBox picSettingsFriendlyFire;
        private System.Windows.Forms.CheckBox chkSettingsHardcore;
        private System.Windows.Forms.PictureBox picSettingsHardcore;
    }
}
