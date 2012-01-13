namespace PRoCon {
    partial class uscPlayerPunishPanel {
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
            this.rdoKick = new System.Windows.Forms.RadioButton();
            this.btnPunish = new System.Windows.Forms.Button();
            this.lblReason = new System.Windows.Forms.Label();
            this.cboReason = new System.Windows.Forms.ComboBox();
            this.rdoTemporaryBan = new System.Windows.Forms.RadioButton();
            this.rdoPermanentlyBan = new System.Windows.Forms.RadioButton();
            this.lblConfirmation = new System.Windows.Forms.Label();
            this.pnlTime = new System.Windows.Forms.Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.txtTime = new System.Windows.Forms.TextBox();
            this.cboTimeMultiplier = new System.Windows.Forms.ComboBox();
            this.rdoPunishOnName = new System.Windows.Forms.RadioButton();
            this.rdoPunishOnIP = new System.Windows.Forms.RadioButton();
            this.rdoPunishOnGUID = new System.Windows.Forms.RadioButton();
            this.pnlPunishType = new System.Windows.Forms.Panel();
            this.rdoKill = new System.Windows.Forms.RadioButton();
            this.pnlTime.SuspendLayout();
            this.pnlPunishType.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdoKick
            // 
            this.rdoKick.AutoSize = true;
            this.rdoKick.Location = new System.Drawing.Point(3, 25);
            this.rdoKick.Name = "rdoKick";
            this.rdoKick.Size = new System.Drawing.Size(47, 19);
            this.rdoKick.TabIndex = 1;
            this.rdoKick.Text = "Kick";
            this.rdoKick.UseVisualStyleBackColor = true;
            this.rdoKick.CheckedChanged += new System.EventHandler(this.rdoKick_CheckedChanged);
            // 
            // btnPunish
            // 
            this.btnPunish.Location = new System.Drawing.Point(190, 124);
            this.btnPunish.Name = "btnPunish";
            this.btnPunish.Size = new System.Drawing.Size(238, 23);
            this.btnPunish.TabIndex = 7;
            this.btnPunish.Text = "Dishonorably Discharge";
            this.btnPunish.UseVisualStyleBackColor = true;
            this.btnPunish.Click += new System.EventHandler(this.btnPunish_Click);
            // 
            // lblReason
            // 
            this.lblReason.AutoSize = true;
            this.lblReason.Location = new System.Drawing.Point(167, 9);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(48, 15);
            this.lblReason.TabIndex = 63;
            this.lblReason.Text = "Reason:";
            // 
            // cboReason
            // 
            this.cboReason.FormattingEnabled = true;
            //this.cboReason.Items.AddRange(new object[] {
            //"Team Killing",
            //"Hacking/Cheating",
            //"Admin abuse"});
            this.cboReason.Location = new System.Drawing.Point(170, 27);
            this.cboReason.Name = "cboReason";
            this.cboReason.Size = new System.Drawing.Size(258, 23);
            this.cboReason.TabIndex = 5;
            // 
            // rdoTemporaryBan
            // 
            this.rdoTemporaryBan.AutoSize = true;
            this.rdoTemporaryBan.Location = new System.Drawing.Point(3, 75);
            this.rdoTemporaryBan.Name = "rdoTemporaryBan";
            this.rdoTemporaryBan.Size = new System.Drawing.Size(106, 19);
            this.rdoTemporaryBan.TabIndex = 3;
            this.rdoTemporaryBan.Text = "Temporary ban";
            this.rdoTemporaryBan.UseVisualStyleBackColor = true;
            this.rdoTemporaryBan.CheckedChanged += new System.EventHandler(this.rdoTemporaryBan_CheckedChanged);
            // 
            // rdoPermanentlyBan
            // 
            this.rdoPermanentlyBan.AutoSize = true;
            this.rdoPermanentlyBan.Location = new System.Drawing.Point(3, 50);
            this.rdoPermanentlyBan.Name = "rdoPermanentlyBan";
            this.rdoPermanentlyBan.Size = new System.Drawing.Size(115, 19);
            this.rdoPermanentlyBan.TabIndex = 2;
            this.rdoPermanentlyBan.Text = "Permanently ban";
            this.rdoPermanentlyBan.UseVisualStyleBackColor = true;
            this.rdoPermanentlyBan.CheckedChanged += new System.EventHandler(this.rdoPermanentlyBan_CheckedChanged);
            // 
            // lblConfirmation
            // 
            this.lblConfirmation.Location = new System.Drawing.Point(170, 87);
            this.lblConfirmation.Name = "lblConfirmation";
            this.lblConfirmation.Size = new System.Drawing.Size(258, 34);
            this.lblConfirmation.TabIndex = 68;
            this.lblConfirmation.Text = "Temporarily ban \'(U3)Phogue\' for 88 minutes, two months 4 days:";
            this.lblConfirmation.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // pnlTime
            // 
            this.pnlTime.Controls.Add(this.lblTime);
            this.pnlTime.Controls.Add(this.txtTime);
            this.pnlTime.Controls.Add(this.cboTimeMultiplier);
            this.pnlTime.Enabled = false;
            this.pnlTime.Location = new System.Drawing.Point(0, 96);
            this.pnlTime.Name = "pnlTime";
            this.pnlTime.Size = new System.Drawing.Size(261, 63);
            this.pnlTime.TabIndex = 4;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(0, 8);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(37, 15);
            this.lblTime.TabIndex = 0;
            this.lblTime.Text = "Time:";
            // 
            // txtTime
            // 
            this.txtTime.Location = new System.Drawing.Point(3, 29);
            this.txtTime.MaxLength = 3;
            this.txtTime.Name = "txtTime";
            this.txtTime.Size = new System.Drawing.Size(39, 23);
            this.txtTime.TabIndex = 1;
            this.txtTime.TextChanged += new System.EventHandler(this.txtTime_TextChanged);
            this.txtTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTime_KeyPress);
            // 
            // cboTimeMultiplier
            // 
            this.cboTimeMultiplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTimeMultiplier.FormattingEnabled = true;
            this.cboTimeMultiplier.Items.AddRange(new object[] {
            "Minutes",
            "Hours",
            "Days",
            "Weeks",
            "Months"});
            this.cboTimeMultiplier.Location = new System.Drawing.Point(50, 29);
            this.cboTimeMultiplier.Name = "cboTimeMultiplier";
            this.cboTimeMultiplier.Size = new System.Drawing.Size(102, 23);
            this.cboTimeMultiplier.TabIndex = 2;
            this.cboTimeMultiplier.SelectedIndexChanged += new System.EventHandler(this.cboTimeMultiplier_SelectedIndexChanged);
            // 
            // rdoPunishOnName
            // 
            this.rdoPunishOnName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdoPunishOnName.AutoSize = true;
            this.rdoPunishOnName.Checked = true;
            this.rdoPunishOnName.Location = new System.Drawing.Point(37, 5);
            this.rdoPunishOnName.Name = "rdoPunishOnName";
            this.rdoPunishOnName.Size = new System.Drawing.Size(57, 19);
            this.rdoPunishOnName.TabIndex = 1;
            this.rdoPunishOnName.TabStop = true;
            this.rdoPunishOnName.Text = "Name";
            this.rdoPunishOnName.UseVisualStyleBackColor = true;
            // 
            // rdoPunishOnIP
            // 
            this.rdoPunishOnIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdoPunishOnIP.AutoSize = true;
            this.rdoPunishOnIP.Enabled = false;
            this.rdoPunishOnIP.Location = new System.Drawing.Point(101, 5);
            this.rdoPunishOnIP.Name = "rdoPunishOnIP";
            this.rdoPunishOnIP.Size = new System.Drawing.Size(35, 19);
            this.rdoPunishOnIP.TabIndex = 2;
            this.rdoPunishOnIP.Text = "Ip";
            this.rdoPunishOnIP.UseVisualStyleBackColor = true;
            // 
            // rdoPunishOnGUID
            // 
            this.rdoPunishOnGUID.AutoSize = true;
            this.rdoPunishOnGUID.Enabled = false;
            this.rdoPunishOnGUID.Location = new System.Drawing.Point(142, 5);
            this.rdoPunishOnGUID.Name = "rdoPunishOnGUID";
            this.rdoPunishOnGUID.Size = new System.Drawing.Size(50, 19);
            this.rdoPunishOnGUID.TabIndex = 3;
            this.rdoPunishOnGUID.TabStop = true;
            this.rdoPunishOnGUID.Text = "Guid";
            this.rdoPunishOnGUID.UseVisualStyleBackColor = true;
            // 
            // pnlPunishType
            // 
            this.pnlPunishType.Controls.Add(this.rdoPunishOnGUID);
            this.pnlPunishType.Controls.Add(this.rdoPunishOnIP);
            this.pnlPunishType.Controls.Add(this.rdoPunishOnName);
            this.pnlPunishType.Location = new System.Drawing.Point(230, 56);
            this.pnlPunishType.Name = "pnlPunishType";
            this.pnlPunishType.Size = new System.Drawing.Size(198, 25);
            this.pnlPunishType.TabIndex = 6;
            // 
            // rdoKill
            // 
            this.rdoKill.AutoSize = true;
            this.rdoKill.Checked = true;
            this.rdoKill.Location = new System.Drawing.Point(3, 0);
            this.rdoKill.Name = "rdoKill";
            this.rdoKill.Size = new System.Drawing.Size(41, 19);
            this.rdoKill.TabIndex = 69;
            this.rdoKill.TabStop = true;
            this.rdoKill.Text = "Kill";
            this.rdoKill.UseVisualStyleBackColor = true;
            this.rdoKill.CheckedChanged += new System.EventHandler(this.rdoKill_CheckedChanged);
            // 
            // uscPlayerPunishPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdoKill);
            this.Controls.Add(this.pnlPunishType);
            this.Controls.Add(this.rdoKick);
            this.Controls.Add(this.btnPunish);
            this.Controls.Add(this.lblReason);
            this.Controls.Add(this.cboReason);
            this.Controls.Add(this.rdoTemporaryBan);
            this.Controls.Add(this.rdoPermanentlyBan);
            this.Controls.Add(this.lblConfirmation);
            this.Controls.Add(this.pnlTime);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "uscPlayerPunishPanel";
            this.Size = new System.Drawing.Size(439, 157);
            this.pnlTime.ResumeLayout(false);
            this.pnlTime.PerformLayout();
            this.pnlPunishType.ResumeLayout(false);
            this.pnlPunishType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdoKick;
        private System.Windows.Forms.Button btnPunish;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.ComboBox cboReason;
        private System.Windows.Forms.RadioButton rdoTemporaryBan;
        private System.Windows.Forms.RadioButton rdoPermanentlyBan;
        private System.Windows.Forms.Label lblConfirmation;
        private System.Windows.Forms.Panel pnlTime;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.TextBox txtTime;
        private System.Windows.Forms.ComboBox cboTimeMultiplier;
        private System.Windows.Forms.RadioButton rdoPunishOnName;
        private System.Windows.Forms.RadioButton rdoPunishOnIP;
        private System.Windows.Forms.RadioButton rdoPunishOnGUID;
        private System.Windows.Forms.Panel pnlPunishType;
        private System.Windows.Forms.RadioButton rdoKill;
    }
}
