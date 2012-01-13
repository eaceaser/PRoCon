namespace PRoCon.Forms {
    partial class frmManageAccounts {
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManageAccounts));
            this.lstAccounts = new System.Windows.Forms.ListView();
            this.imlUsers = new System.Windows.Forms.ImageList(this.components);
            this.pnlChooseAccount = new System.Windows.Forms.Panel();
            this.picCreateNewAccount = new System.Windows.Forms.PictureBox();
            this.lblSelectAccountTitle = new System.Windows.Forms.Label();
            this.lnkCreateNewAccount = new System.Windows.Forms.LinkLabel();
            this.pnlEditingUser = new System.Windows.Forms.Panel();
            this.picDeleteAccount = new System.Windows.Forms.PictureBox();
            this.picRemovePrivileges = new System.Windows.Forms.PictureBox();
            this.picEditGlobalPrivileges = new System.Windows.Forms.PictureBox();
            this.lnkSetGlobalPrivileges = new System.Windows.Forms.LinkLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lnkEditAnotherAccount = new System.Windows.Forms.LinkLabel();
            this.lnkRemovePrivileges = new System.Windows.Forms.LinkLabel();
            this.lnkDeleteUser = new System.Windows.Forms.LinkLabel();
            this.txtChangePassword = new System.Windows.Forms.TextBox();
            this.lblChangePassword = new System.Windows.Forms.Label();
            this.lblEditingAccountTitle = new System.Windows.Forms.Label();
            this.lnkUpdatePassword = new System.Windows.Forms.LinkLabel();
            this.tltipExtraInfo = new System.Windows.Forms.ToolTip(this.components);
            this.pnlConfirmation = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.lblExtraInformation = new System.Windows.Forms.Label();
            this.lblConfirmationQuestion = new System.Windows.Forms.Label();
            this.lblConfirmationTitle = new System.Windows.Forms.Label();
            this.pnlCreateAccount = new System.Windows.Forms.Panel();
            this.lblUserNameExistsError = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.btnCancelNewAccount = new System.Windows.Forms.Button();
            this.btnCreateAccount = new System.Windows.Forms.Button();
            this.lblCreateNewAccountTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlAlterPrivileges = new System.Windows.Forms.Panel();
            this.uscSetPrivileges = new PRoCon.uscPrivilegesSelection();
            this.pnlChooseAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCreateNewAccount)).BeginInit();
            this.pnlEditingUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDeleteAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRemovePrivileges)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEditGlobalPrivileges)).BeginInit();
            this.pnlConfirmation.SuspendLayout();
            this.pnlCreateAccount.SuspendLayout();
            this.pnlAlterPrivileges.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstAccounts
            // 
            this.lstAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lstAccounts.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstAccounts.LargeImageList = this.imlUsers;
            this.lstAccounts.Location = new System.Drawing.Point(30, 48);
            this.lstAccounts.MultiSelect = false;
            this.lstAccounts.Name = "lstAccounts";
            this.lstAccounts.Size = new System.Drawing.Size(391, 362);
            this.lstAccounts.TabIndex = 0;
            this.lstAccounts.UseCompatibleStateImageBehavior = false;
            this.lstAccounts.View = System.Windows.Forms.View.Tile;
            this.lstAccounts.SelectedIndexChanged += new System.EventHandler(this.lstAccounts_SelectedIndexChanged);
            // 
            // imlUsers
            // 
            this.imlUsers.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlUsers.ImageStream")));
            this.imlUsers.TransparentColor = System.Drawing.Color.Transparent;
            this.imlUsers.Images.SetKeyName(0, "user-offline.ico");
            this.imlUsers.Images.SetKeyName(1, "user-online.ico");
            // 
            // pnlChooseAccount
            // 
            this.pnlChooseAccount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlChooseAccount.BackColor = System.Drawing.SystemColors.Window;
            this.pnlChooseAccount.Controls.Add(this.picCreateNewAccount);
            this.pnlChooseAccount.Controls.Add(this.lblSelectAccountTitle);
            this.pnlChooseAccount.Controls.Add(this.lnkCreateNewAccount);
            this.pnlChooseAccount.Controls.Add(this.lstAccounts);
            this.pnlChooseAccount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlChooseAccount.Location = new System.Drawing.Point(14, 14);
            this.pnlChooseAccount.Name = "pnlChooseAccount";
            this.pnlChooseAccount.Size = new System.Drawing.Size(456, 532);
            this.pnlChooseAccount.TabIndex = 29;
            // 
            // picCreateNewAccount
            // 
            this.picCreateNewAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.picCreateNewAccount.Location = new System.Drawing.Point(30, 413);
            this.picCreateNewAccount.Name = "picCreateNewAccount";
            this.picCreateNewAccount.Size = new System.Drawing.Size(19, 18);
            this.picCreateNewAccount.TabIndex = 14;
            this.picCreateNewAccount.TabStop = false;
            // 
            // lblSelectAccountTitle
            // 
            this.lblSelectAccountTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectAccountTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectAccountTitle.Location = new System.Drawing.Point(26, 24);
            this.lblSelectAccountTitle.Name = "lblSelectAccountTitle";
            this.lblSelectAccountTitle.Size = new System.Drawing.Size(397, 21);
            this.lblSelectAccountTitle.TabIndex = 3;
            this.lblSelectAccountTitle.Text = "Choose the account you would like to change";
            // 
            // lnkCreateNewAccount
            // 
            this.lnkCreateNewAccount.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkCreateNewAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkCreateNewAccount.AutoSize = true;
            this.lnkCreateNewAccount.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkCreateNewAccount.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkCreateNewAccount.Location = new System.Drawing.Point(56, 414);
            this.lnkCreateNewAccount.Name = "lnkCreateNewAccount";
            this.lnkCreateNewAccount.Size = new System.Drawing.Size(121, 15);
            this.lnkCreateNewAccount.TabIndex = 1;
            this.lnkCreateNewAccount.TabStop = true;
            this.lnkCreateNewAccount.Text = "Create a new account";
            this.lnkCreateNewAccount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAddNewAccount_LinkClicked);
            // 
            // pnlEditingUser
            // 
            this.pnlEditingUser.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlEditingUser.BackColor = System.Drawing.SystemColors.Window;
            this.pnlEditingUser.Controls.Add(this.picDeleteAccount);
            this.pnlEditingUser.Controls.Add(this.picRemovePrivileges);
            this.pnlEditingUser.Controls.Add(this.picEditGlobalPrivileges);
            this.pnlEditingUser.Controls.Add(this.lnkSetGlobalPrivileges);
            this.pnlEditingUser.Controls.Add(this.panel2);
            this.pnlEditingUser.Controls.Add(this.lnkEditAnotherAccount);
            this.pnlEditingUser.Controls.Add(this.lnkRemovePrivileges);
            this.pnlEditingUser.Controls.Add(this.lnkDeleteUser);
            this.pnlEditingUser.Controls.Add(this.txtChangePassword);
            this.pnlEditingUser.Controls.Add(this.lblChangePassword);
            this.pnlEditingUser.Controls.Add(this.lblEditingAccountTitle);
            this.pnlEditingUser.Controls.Add(this.lnkUpdatePassword);
            this.pnlEditingUser.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlEditingUser.Location = new System.Drawing.Point(489, 569);
            this.pnlEditingUser.Name = "pnlEditingUser";
            this.pnlEditingUser.Size = new System.Drawing.Size(456, 532);
            this.pnlEditingUser.TabIndex = 30;
            this.pnlEditingUser.Visible = false;
            // 
            // picDeleteAccount
            // 
            this.picDeleteAccount.Location = new System.Drawing.Point(42, 228);
            this.picDeleteAccount.Name = "picDeleteAccount";
            this.picDeleteAccount.Size = new System.Drawing.Size(19, 18);
            this.picDeleteAccount.TabIndex = 15;
            this.picDeleteAccount.TabStop = false;
            // 
            // picRemovePrivileges
            // 
            this.picRemovePrivileges.Location = new System.Drawing.Point(42, 189);
            this.picRemovePrivileges.Name = "picRemovePrivileges";
            this.picRemovePrivileges.Size = new System.Drawing.Size(19, 18);
            this.picRemovePrivileges.TabIndex = 14;
            this.picRemovePrivileges.TabStop = false;
            // 
            // picEditGlobalPrivileges
            // 
            this.picEditGlobalPrivileges.Location = new System.Drawing.Point(42, 144);
            this.picEditGlobalPrivileges.Name = "picEditGlobalPrivileges";
            this.picEditGlobalPrivileges.Size = new System.Drawing.Size(19, 18);
            this.picEditGlobalPrivileges.TabIndex = 13;
            this.picEditGlobalPrivileges.TabStop = false;
            // 
            // lnkSetGlobalPrivileges
            // 
            this.lnkSetGlobalPrivileges.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkSetGlobalPrivileges.AutoSize = true;
            this.lnkSetGlobalPrivileges.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkSetGlobalPrivileges.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkSetGlobalPrivileges.Location = new System.Drawing.Point(68, 144);
            this.lnkSetGlobalPrivileges.Name = "lnkSetGlobalPrivileges";
            this.lnkSetGlobalPrivileges.Size = new System.Drawing.Size(169, 15);
            this.lnkSetGlobalPrivileges.TabIndex = 10;
            this.lnkSetGlobalPrivileges.TabStop = true;
            this.lnkSetGlobalPrivileges.Text = "Set privilges on all server layers";
            this.tltipExtraInfo.SetToolTip(this.lnkSetGlobalPrivileges, "Overwrites each layers individual privileges with a new set of rules");
            this.lnkSetGlobalPrivileges.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAlterGlobalPrivileges_LinkClicked);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel2.Location = new System.Drawing.Point(0, 428);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(456, 1);
            this.panel2.TabIndex = 9;
            // 
            // lnkEditAnotherAccount
            // 
            this.lnkEditAnotherAccount.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkEditAnotherAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lnkEditAnotherAccount.AutoSize = true;
            this.lnkEditAnotherAccount.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkEditAnotherAccount.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkEditAnotherAccount.Location = new System.Drawing.Point(45, 432);
            this.lnkEditAnotherAccount.Name = "lnkEditAnotherAccount";
            this.lnkEditAnotherAccount.Size = new System.Drawing.Size(117, 15);
            this.lnkEditAnotherAccount.TabIndex = 8;
            this.lnkEditAnotherAccount.TabStop = true;
            this.lnkEditAnotherAccount.Text = "Edit another account";
            this.lnkEditAnotherAccount.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkEditAnotherAccount_LinkClicked);
            // 
            // lnkRemovePrivileges
            // 
            this.lnkRemovePrivileges.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkRemovePrivileges.AutoSize = true;
            this.lnkRemovePrivileges.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkRemovePrivileges.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkRemovePrivileges.Location = new System.Drawing.Point(68, 189);
            this.lnkRemovePrivileges.Name = "lnkRemovePrivileges";
            this.lnkRemovePrivileges.Size = new System.Drawing.Size(207, 15);
            this.lnkRemovePrivileges.TabIndex = 7;
            this.lnkRemovePrivileges.TabStop = true;
            this.lnkRemovePrivileges.Text = "Remove all individual server privileges";
            this.tltipExtraInfo.SetToolTip(this.lnkRemovePrivileges, "Removes this users access to all of the servers, essentially resetting the accoun" +
                    "t.");
            this.lnkRemovePrivileges.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRemovePrivileges_LinkClicked);
            // 
            // lnkDeleteUser
            // 
            this.lnkDeleteUser.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkDeleteUser.AutoSize = true;
            this.lnkDeleteUser.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkDeleteUser.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkDeleteUser.Location = new System.Drawing.Point(68, 230);
            this.lnkDeleteUser.Name = "lnkDeleteUser";
            this.lnkDeleteUser.Size = new System.Drawing.Size(86, 15);
            this.lnkDeleteUser.TabIndex = 6;
            this.lnkDeleteUser.TabStop = true;
            this.lnkDeleteUser.Text = "Delete account";
            this.tltipExtraInfo.SetToolTip(this.lnkDeleteUser, "Deletes the user and removes their access from all servers.");
            this.lnkDeleteUser.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDeleteUser_LinkClicked);
            // 
            // txtChangePassword
            // 
            this.txtChangePassword.Location = new System.Drawing.Point(54, 93);
            this.txtChangePassword.Name = "txtChangePassword";
            this.txtChangePassword.Size = new System.Drawing.Size(195, 23);
            this.txtChangePassword.TabIndex = 5;
            this.txtChangePassword.TextChanged += new System.EventHandler(this.txtChangePassword_TextChanged);
            // 
            // lblChangePassword
            // 
            this.lblChangePassword.AutoSize = true;
            this.lblChangePassword.Location = new System.Drawing.Point(50, 73);
            this.lblChangePassword.Name = "lblChangePassword";
            this.lblChangePassword.Size = new System.Drawing.Size(101, 15);
            this.lblChangePassword.TabIndex = 4;
            this.lblChangePassword.Text = "Change password";
            // 
            // lblEditingAccountTitle
            // 
            this.lblEditingAccountTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblEditingAccountTitle.Location = new System.Drawing.Point(26, 24);
            this.lblEditingAccountTitle.Name = "lblEditingAccountTitle";
            this.lblEditingAccountTitle.Size = new System.Drawing.Size(414, 21);
            this.lblEditingAccountTitle.TabIndex = 3;
            this.lblEditingAccountTitle.Text = "Editing Phogue";
            // 
            // lnkUpdatePassword
            // 
            this.lnkUpdatePassword.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkUpdatePassword.AutoSize = true;
            this.lnkUpdatePassword.Enabled = false;
            this.lnkUpdatePassword.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkUpdatePassword.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.lnkUpdatePassword.Location = new System.Drawing.Point(257, 97);
            this.lnkUpdatePassword.Name = "lnkUpdatePassword";
            this.lnkUpdatePassword.Size = new System.Drawing.Size(98, 15);
            this.lnkUpdatePassword.TabIndex = 1;
            this.lnkUpdatePassword.TabStop = true;
            this.lnkUpdatePassword.Text = "Update password";
            this.lnkUpdatePassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkUpdatePassword_LinkClicked);
            // 
            // pnlConfirmation
            // 
            this.pnlConfirmation.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlConfirmation.BackColor = System.Drawing.SystemColors.Window;
            this.pnlConfirmation.Controls.Add(this.btnCancel);
            this.pnlConfirmation.Controls.Add(this.btnConfirm);
            this.pnlConfirmation.Controls.Add(this.lblExtraInformation);
            this.pnlConfirmation.Controls.Add(this.lblConfirmationQuestion);
            this.pnlConfirmation.Controls.Add(this.lblConfirmationTitle);
            this.pnlConfirmation.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlConfirmation.Location = new System.Drawing.Point(14, 569);
            this.pnlConfirmation.Name = "pnlConfirmation";
            this.pnlConfirmation.Size = new System.Drawing.Size(456, 532);
            this.pnlConfirmation.TabIndex = 31;
            this.pnlConfirmation.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnCancel.Location = new System.Drawing.Point(332, 423);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 27);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnConfirm.Location = new System.Drawing.Point(153, 423);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(173, 27);
            this.btnConfirm.TabIndex = 6;
            this.btnConfirm.Text = "Delete Account";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // lblExtraInformation
            // 
            this.lblExtraInformation.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblExtraInformation.Location = new System.Drawing.Point(42, 61);
            this.lblExtraInformation.Name = "lblExtraInformation";
            this.lblExtraInformation.Size = new System.Drawing.Size(378, 125);
            this.lblExtraInformation.TabIndex = 5;
            this.lblExtraInformation.Text = "Deleting Phogue\'s account will immediately disconnect them if they are logged in";
            // 
            // lblConfirmationQuestion
            // 
            this.lblConfirmationQuestion.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblConfirmationQuestion.Location = new System.Drawing.Point(42, 365);
            this.lblConfirmationQuestion.Name = "lblConfirmationQuestion";
            this.lblConfirmationQuestion.Size = new System.Drawing.Size(380, 44);
            this.lblConfirmationQuestion.TabIndex = 4;
            this.lblConfirmationQuestion.Text = "Are you sure you want to delete Phogue\'s account?";
            this.lblConfirmationQuestion.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // lblConfirmationTitle
            // 
            this.lblConfirmationTitle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblConfirmationTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmationTitle.Location = new System.Drawing.Point(21, 24);
            this.lblConfirmationTitle.Name = "lblConfirmationTitle";
            this.lblConfirmationTitle.Size = new System.Drawing.Size(414, 21);
            this.lblConfirmationTitle.TabIndex = 3;
            this.lblConfirmationTitle.Text = "Confirmation";
            // 
            // pnlCreateAccount
            // 
            this.pnlCreateAccount.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlCreateAccount.BackColor = System.Drawing.SystemColors.Window;
            this.pnlCreateAccount.Controls.Add(this.lblUserNameExistsError);
            this.pnlCreateAccount.Controls.Add(this.txtPassword);
            this.pnlCreateAccount.Controls.Add(this.lblPassword);
            this.pnlCreateAccount.Controls.Add(this.txtUsername);
            this.pnlCreateAccount.Controls.Add(this.lblUsername);
            this.pnlCreateAccount.Controls.Add(this.btnCancelNewAccount);
            this.pnlCreateAccount.Controls.Add(this.btnCreateAccount);
            this.pnlCreateAccount.Controls.Add(this.lblCreateNewAccountTitle);
            this.pnlCreateAccount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlCreateAccount.Location = new System.Drawing.Point(477, 14);
            this.pnlCreateAccount.Name = "pnlCreateAccount";
            this.pnlCreateAccount.Size = new System.Drawing.Size(456, 532);
            this.pnlCreateAccount.TabIndex = 32;
            this.pnlCreateAccount.Visible = false;
            // 
            // lblUserNameExistsError
            // 
            this.lblUserNameExistsError.AutoSize = true;
            this.lblUserNameExistsError.ForeColor = System.Drawing.Color.Maroon;
            this.lblUserNameExistsError.Location = new System.Drawing.Point(257, 97);
            this.lblUserNameExistsError.Name = "lblUserNameExistsError";
            this.lblUserNameExistsError.Size = new System.Drawing.Size(132, 15);
            this.lblUserNameExistsError.TabIndex = 12;
            this.lblUserNameExistsError.Text = "Username already exists";
            this.lblUserNameExistsError.Visible = false;
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(54, 159);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(195, 23);
            this.txtPassword.TabIndex = 11;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(50, 138);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(57, 15);
            this.lblPassword.TabIndex = 10;
            this.lblPassword.Text = "Password";
            // 
            // txtUsername
            // 
            this.txtUsername.Location = new System.Drawing.Point(54, 93);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(195, 23);
            this.txtUsername.TabIndex = 9;
            this.txtUsername.TextChanged += new System.EventHandler(this.txtUsername_TextChanged);
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(50, 73);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(60, 15);
            this.lblUsername.TabIndex = 8;
            this.lblUsername.Text = "Username";
            // 
            // btnCancelNewAccount
            // 
            this.btnCancelNewAccount.Location = new System.Drawing.Point(337, 233);
            this.btnCancelNewAccount.Name = "btnCancelNewAccount";
            this.btnCancelNewAccount.Size = new System.Drawing.Size(87, 27);
            this.btnCancelNewAccount.TabIndex = 7;
            this.btnCancelNewAccount.Text = "Cancel";
            this.btnCancelNewAccount.UseVisualStyleBackColor = true;
            this.btnCancelNewAccount.Click += new System.EventHandler(this.btnCancelNewAccount_Click);
            // 
            // btnCreateAccount
            // 
            this.btnCreateAccount.Enabled = false;
            this.btnCreateAccount.Location = new System.Drawing.Point(157, 233);
            this.btnCreateAccount.Name = "btnCreateAccount";
            this.btnCreateAccount.Size = new System.Drawing.Size(173, 27);
            this.btnCreateAccount.TabIndex = 6;
            this.btnCreateAccount.Text = "Create Account";
            this.btnCreateAccount.UseVisualStyleBackColor = true;
            this.btnCreateAccount.Click += new System.EventHandler(this.btnCreateAccount_Click);
            // 
            // lblCreateNewAccountTitle
            // 
            this.lblCreateNewAccountTitle.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreateNewAccountTitle.Location = new System.Drawing.Point(26, 24);
            this.lblCreateNewAccountTitle.Name = "lblCreateNewAccountTitle";
            this.lblCreateNewAccountTitle.Size = new System.Drawing.Size(414, 21);
            this.lblCreateNewAccountTitle.TabIndex = 3;
            this.lblCreateNewAccountTitle.Text = "Create new account";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1274, 1187);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 27);
            this.btnClose.TabIndex = 33;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlAlterPrivileges
            // 
            this.pnlAlterPrivileges.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pnlAlterPrivileges.Controls.Add(this.uscSetPrivileges);
            this.pnlAlterPrivileges.Location = new System.Drawing.Point(952, 14);
            this.pnlAlterPrivileges.Name = "pnlAlterPrivileges";
            this.pnlAlterPrivileges.Size = new System.Drawing.Size(409, 532);
            this.pnlAlterPrivileges.TabIndex = 34;
            this.pnlAlterPrivileges.Visible = false;
            // 
            // uscSetPrivileges
            // 
            this.uscSetPrivileges.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uscSetPrivileges.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscSetPrivileges.Location = new System.Drawing.Point(0, 0);
            this.uscSetPrivileges.Name = "uscSetPrivileges";
            this.uscSetPrivileges.Size = new System.Drawing.Size(409, 532);
            this.uscSetPrivileges.TabIndex = 0;
            // 
            // frmManageAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1375, 1054);
            this.Controls.Add(this.pnlAlterPrivileges);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlEditingUser);
            this.Controls.Add(this.pnlChooseAccount);
            this.Controls.Add(this.pnlCreateAccount);
            this.Controls.Add(this.pnlConfirmation);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmManageAccounts";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmManageUsers";
            this.Load += new System.EventHandler(this.frmManageAccounts_Load);
            this.Shown += new System.EventHandler(this.frmManageAccounts_Shown);
            this.pnlChooseAccount.ResumeLayout(false);
            this.pnlChooseAccount.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCreateNewAccount)).EndInit();
            this.pnlEditingUser.ResumeLayout(false);
            this.pnlEditingUser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDeleteAccount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRemovePrivileges)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picEditGlobalPrivileges)).EndInit();
            this.pnlConfirmation.ResumeLayout(false);
            this.pnlCreateAccount.ResumeLayout(false);
            this.pnlCreateAccount.PerformLayout();
            this.pnlAlterPrivileges.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lstAccounts;
        private System.Windows.Forms.Panel pnlChooseAccount;
        private System.Windows.Forms.ImageList imlUsers;
        private System.Windows.Forms.LinkLabel lnkCreateNewAccount;
        private System.Windows.Forms.Label lblSelectAccountTitle;
        private System.Windows.Forms.Panel pnlEditingUser;
        private System.Windows.Forms.Label lblEditingAccountTitle;
        private System.Windows.Forms.LinkLabel lnkUpdatePassword;
        private System.Windows.Forms.TextBox txtChangePassword;
        private System.Windows.Forms.Label lblChangePassword;
        private System.Windows.Forms.LinkLabel lnkDeleteUser;
        private System.Windows.Forms.LinkLabel lnkRemovePrivileges;
        private System.Windows.Forms.ToolTip tltipExtraInfo;
        private System.Windows.Forms.Panel pnlConfirmation;
        private System.Windows.Forms.Label lblExtraInformation;
        private System.Windows.Forms.Label lblConfirmationQuestion;
        private System.Windows.Forms.Label lblConfirmationTitle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Panel pnlCreateAccount;
        private System.Windows.Forms.Button btnCancelNewAccount;
        private System.Windows.Forms.Button btnCreateAccount;
        private System.Windows.Forms.Label lblCreateNewAccountTitle;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblUserNameExistsError;
        private System.Windows.Forms.LinkLabel lnkEditAnotherAccount;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlAlterPrivileges;
        private uscPrivilegesSelection uscSetPrivileges;
        private System.Windows.Forms.LinkLabel lnkSetGlobalPrivileges;
        private System.Windows.Forms.PictureBox picDeleteAccount;
        private System.Windows.Forms.PictureBox picRemovePrivileges;
        private System.Windows.Forms.PictureBox picEditGlobalPrivileges;
        private System.Windows.Forms.PictureBox picCreateNewAccount;
    }
}