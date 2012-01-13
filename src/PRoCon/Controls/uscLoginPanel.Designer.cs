namespace PRoCon {
    partial class uscLoginPanel {
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
            this.pnlConnectionOptions = new System.Windows.Forms.Panel();
            this.pnlLoginBGBox = new System.Windows.Forms.Panel();
            this.picConnecting = new System.Windows.Forms.PictureBox();
            this.txtConnectionsUsername = new System.Windows.Forms.TextBox();
            this.lblConnectionUsername = new System.Windows.Forms.Label();
            this.lblLargeServer = new System.Windows.Forms.Label();
            this.chkAutomaticallyConnect = new System.Windows.Forms.CheckBox();
            this.lblConnectionPassword = new System.Windows.Forms.Label();
            this.lnkConnectionsConnect = new System.Windows.Forms.LinkLabel();
            this.txtConnectionsPassword = new System.Windows.Forms.TextBox();
            this.lblError = new System.Windows.Forms.Label();
            this.tlTipLoginPanel = new System.Windows.Forms.ToolTip(this.components);
            this.pnlConnectionOptions.SuspendLayout();
            this.pnlLoginBGBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picConnecting)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlConnectionOptions
            // 
            this.pnlConnectionOptions.BackColor = System.Drawing.Color.Transparent;
            this.pnlConnectionOptions.Controls.Add(this.pnlLoginBGBox);
            this.pnlConnectionOptions.Controls.Add(this.lblError);
            this.pnlConnectionOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConnectionOptions.Location = new System.Drawing.Point(0, 0);
            this.pnlConnectionOptions.Name = "pnlConnectionOptions";
            this.pnlConnectionOptions.Size = new System.Drawing.Size(862, 516);
            this.pnlConnectionOptions.TabIndex = 8;
            // 
            // pnlLoginBGBox
            // 
            this.pnlLoginBGBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlLoginBGBox.BackColor = System.Drawing.Color.LightGray;
            this.pnlLoginBGBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLoginBGBox.Controls.Add(this.picConnecting);
            this.pnlLoginBGBox.Controls.Add(this.txtConnectionsUsername);
            this.pnlLoginBGBox.Controls.Add(this.lblConnectionUsername);
            this.pnlLoginBGBox.Controls.Add(this.lblLargeServer);
            this.pnlLoginBGBox.Controls.Add(this.chkAutomaticallyConnect);
            this.pnlLoginBGBox.Controls.Add(this.lblConnectionPassword);
            this.pnlLoginBGBox.Controls.Add(this.lnkConnectionsConnect);
            this.pnlLoginBGBox.Controls.Add(this.txtConnectionsPassword);
            this.pnlLoginBGBox.Location = new System.Drawing.Point(255, 157);
            this.pnlLoginBGBox.Name = "pnlLoginBGBox";
            this.pnlLoginBGBox.Size = new System.Drawing.Size(353, 203);
            this.pnlLoginBGBox.TabIndex = 12;
            // 
            // picConnecting
            // 
            this.picConnecting.Location = new System.Drawing.Point(122, 164);
            this.picConnecting.Name = "picConnecting";
            this.picConnecting.Size = new System.Drawing.Size(16, 16);
            this.picConnecting.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picConnecting.TabIndex = 11;
            this.picConnecting.TabStop = false;
            this.picConnecting.Visible = false;
            // 
            // txtConnectionsUsername
            // 
            this.txtConnectionsUsername.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtConnectionsUsername.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtConnectionsUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConnectionsUsername.Location = new System.Drawing.Point(132, 53);
            this.txtConnectionsUsername.Name = "txtConnectionsUsername";
            this.txtConnectionsUsername.Size = new System.Drawing.Size(153, 23);
            this.txtConnectionsUsername.TabIndex = 6;
            this.tlTipLoginPanel.SetToolTip(this.txtConnectionsUsername, "You only require a username to login to a procon server, otherwise just enter you" +
                    "r password.");
            this.txtConnectionsUsername.Enter += new System.EventHandler(this.txtConnectionsUsername_Enter);
            // 
            // lblConnectionUsername
            // 
            this.lblConnectionUsername.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblConnectionUsername.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectionUsername.Location = new System.Drawing.Point(6, 56);
            this.lblConnectionUsername.Name = "lblConnectionUsername";
            this.lblConnectionUsername.Size = new System.Drawing.Size(114, 44);
            this.lblConnectionUsername.TabIndex = 4;
            this.lblConnectionUsername.Text = "Username:";
            this.lblConnectionUsername.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblLargeServer
            // 
            this.lblLargeServer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblLargeServer.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLargeServer.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblLargeServer.Location = new System.Drawing.Point(2, 6);
            this.lblLargeServer.Name = "lblLargeServer";
            this.lblLargeServer.Size = new System.Drawing.Size(347, 40);
            this.lblLargeServer.TabIndex = 0;
            this.lblLargeServer.Text = "127.0.0.1:5555";
            this.lblLargeServer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkAutomaticallyConnect
            // 
            this.chkAutomaticallyConnect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chkAutomaticallyConnect.AutoSize = true;
            this.chkAutomaticallyConnect.Location = new System.Drawing.Point(137, 124);
            this.chkAutomaticallyConnect.Name = "chkAutomaticallyConnect";
            this.chkAutomaticallyConnect.Size = new System.Drawing.Size(148, 19);
            this.chkAutomaticallyConnect.TabIndex = 10;
            this.chkAutomaticallyConnect.Text = "Automatically Connect";
            this.chkAutomaticallyConnect.UseVisualStyleBackColor = true;
            this.chkAutomaticallyConnect.CheckedChanged += new System.EventHandler(this.chkAutomaticallyConnect_CheckedChanged);
            // 
            // lblConnectionPassword
            // 
            this.lblConnectionPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblConnectionPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectionPassword.Location = new System.Drawing.Point(3, 100);
            this.lblConnectionPassword.Name = "lblConnectionPassword";
            this.lblConnectionPassword.Size = new System.Drawing.Size(117, 43);
            this.lblConnectionPassword.TabIndex = 5;
            this.lblConnectionPassword.Text = "Password:";
            this.lblConnectionPassword.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lnkConnectionsConnect
            // 
            this.lnkConnectionsConnect.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkConnectionsConnect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lnkConnectionsConnect.AutoSize = true;
            this.lnkConnectionsConnect.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkConnectionsConnect.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkConnectionsConnect.Location = new System.Drawing.Point(144, 164);
            this.lnkConnectionsConnect.Name = "lnkConnectionsConnect";
            this.lnkConnectionsConnect.Size = new System.Drawing.Size(52, 15);
            this.lnkConnectionsConnect.TabIndex = 8;
            this.lnkConnectionsConnect.TabStop = true;
            this.lnkConnectionsConnect.Text = "Connect";
            this.lnkConnectionsConnect.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkConnectionsConnect_LinkClicked);
            // 
            // txtConnectionsPassword
            // 
            this.txtConnectionsPassword.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtConnectionsPassword.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtConnectionsPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConnectionsPassword.Location = new System.Drawing.Point(132, 95);
            this.txtConnectionsPassword.Name = "txtConnectionsPassword";
            this.txtConnectionsPassword.PasswordChar = '*';
            this.txtConnectionsPassword.Size = new System.Drawing.Size(153, 23);
            this.txtConnectionsPassword.TabIndex = 7;
            // 
            // lblError
            // 
            this.lblError.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblError.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Maroon;
            this.lblError.Location = new System.Drawing.Point(258, 373);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(350, 100);
            this.lblError.TabIndex = 11;
            this.lblError.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tlTipLoginPanel
            // 
            this.tlTipLoginPanel.IsBalloon = true;
            this.tlTipLoginPanel.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.tlTipLoginPanel.ToolTipTitle = "Title";
            // 
            // uscLoginPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlConnectionOptions);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "uscLoginPanel";
            this.Size = new System.Drawing.Size(862, 516);
            this.Load += new System.EventHandler(this.uscLoginPanel_Load);
            this.pnlConnectionOptions.ResumeLayout(false);
            this.pnlLoginBGBox.ResumeLayout(false);
            this.pnlLoginBGBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picConnecting)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlConnectionOptions;
        private System.Windows.Forms.LinkLabel lnkConnectionsConnect;
        private System.Windows.Forms.TextBox txtConnectionsPassword;
        private System.Windows.Forms.TextBox txtConnectionsUsername;
        private System.Windows.Forms.Label lblConnectionPassword;
        private System.Windows.Forms.Label lblConnectionUsername;
        private System.Windows.Forms.Label lblLargeServer;
        private System.Windows.Forms.ToolTip tlTipLoginPanel;
        private System.Windows.Forms.CheckBox chkAutomaticallyConnect;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Panel pnlLoginBGBox;
        private System.Windows.Forms.PictureBox picConnecting;
    }
}
