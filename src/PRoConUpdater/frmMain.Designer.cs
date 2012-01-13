namespace PRoConUpdater {
    partial class frmMain {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tmrKiBsCounter = new System.Windows.Forms.Timer(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.lblCheckingProconOpen = new System.Windows.Forms.Label();
            this.lblBackingUpConfigs = new System.Windows.Forms.Label();
            this.lblUpdatingDirectory = new System.Windows.Forms.Label();
            this.picCheckingProconOpen = new System.Windows.Forms.PictureBox();
            this.picBackingUpConfigs = new System.Windows.Forms.PictureBox();
            this.picUpdatingDirectory = new System.Windows.Forms.PictureBox();
            this.picError = new System.Windows.Forms.PictureBox();
            this.picSuccess = new System.Windows.Forms.PictureBox();
            this.picLoading = new System.Windows.Forms.PictureBox();
            this.lblProconReloading = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picCheckingProconOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackingUpConfigs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUpdatingDirectory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSuccess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(475, 104);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 24);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lblCheckingProconOpen
            // 
            this.lblCheckingProconOpen.AutoSize = true;
            this.lblCheckingProconOpen.ForeColor = System.Drawing.Color.DimGray;
            this.lblCheckingProconOpen.Location = new System.Drawing.Point(34, 38);
            this.lblCheckingProconOpen.Name = "lblCheckingProconOpen";
            this.lblCheckingProconOpen.Size = new System.Drawing.Size(163, 15);
            this.lblCheckingProconOpen.TabIndex = 2;
            this.lblCheckingProconOpen.Text = "Checking if PRoCon is open..";
            // 
            // lblBackingUpConfigs
            // 
            this.lblBackingUpConfigs.AutoSize = true;
            this.lblBackingUpConfigs.ForeColor = System.Drawing.Color.DimGray;
            this.lblBackingUpConfigs.Location = new System.Drawing.Point(34, 13);
            this.lblBackingUpConfigs.Name = "lblBackingUpConfigs";
            this.lblBackingUpConfigs.Size = new System.Drawing.Size(145, 15);
            this.lblBackingUpConfigs.TabIndex = 3;
            this.lblBackingUpConfigs.Text = "Backing up your configs..";
            // 
            // lblUpdatingDirectory
            // 
            this.lblUpdatingDirectory.AutoSize = true;
            this.lblUpdatingDirectory.ForeColor = System.Drawing.Color.DimGray;
            this.lblUpdatingDirectory.Location = new System.Drawing.Point(34, 63);
            this.lblUpdatingDirectory.Name = "lblUpdatingDirectory";
            this.lblUpdatingDirectory.Size = new System.Drawing.Size(67, 15);
            this.lblUpdatingDirectory.TabIndex = 4;
            this.lblUpdatingDirectory.Text = "Updating...";
            // 
            // picCheckingProconOpen
            // 
            this.picCheckingProconOpen.Location = new System.Drawing.Point(12, 37);
            this.picCheckingProconOpen.Name = "picCheckingProconOpen";
            this.picCheckingProconOpen.Size = new System.Drawing.Size(16, 16);
            this.picCheckingProconOpen.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCheckingProconOpen.TabIndex = 5;
            this.picCheckingProconOpen.TabStop = false;
            // 
            // picBackingUpConfigs
            // 
            this.picBackingUpConfigs.Location = new System.Drawing.Point(12, 12);
            this.picBackingUpConfigs.Name = "picBackingUpConfigs";
            this.picBackingUpConfigs.Size = new System.Drawing.Size(16, 16);
            this.picBackingUpConfigs.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBackingUpConfigs.TabIndex = 6;
            this.picBackingUpConfigs.TabStop = false;
            // 
            // picUpdatingDirectory
            // 
            this.picUpdatingDirectory.Location = new System.Drawing.Point(12, 63);
            this.picUpdatingDirectory.Name = "picUpdatingDirectory";
            this.picUpdatingDirectory.Size = new System.Drawing.Size(16, 16);
            this.picUpdatingDirectory.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picUpdatingDirectory.TabIndex = 7;
            this.picUpdatingDirectory.TabStop = false;
            // 
            // picError
            // 
            this.picError.Image = ((System.Drawing.Image)(resources.GetObject("picError.Image")));
            this.picError.Location = new System.Drawing.Point(220, 63);
            this.picError.Name = "picError";
            this.picError.Size = new System.Drawing.Size(16, 16);
            this.picError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picError.TabIndex = 10;
            this.picError.TabStop = false;
            this.picError.Visible = false;
            // 
            // picSuccess
            // 
            this.picSuccess.Image = ((System.Drawing.Image)(resources.GetObject("picSuccess.Image")));
            this.picSuccess.Location = new System.Drawing.Point(220, 37);
            this.picSuccess.Name = "picSuccess";
            this.picSuccess.Size = new System.Drawing.Size(16, 16);
            this.picSuccess.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picSuccess.TabIndex = 9;
            this.picSuccess.TabStop = false;
            this.picSuccess.Visible = false;
            // 
            // picLoading
            // 
            this.picLoading.Image = ((System.Drawing.Image)(resources.GetObject("picLoading.Image")));
            this.picLoading.Location = new System.Drawing.Point(220, 12);
            this.picLoading.Name = "picLoading";
            this.picLoading.Size = new System.Drawing.Size(16, 16);
            this.picLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picLoading.TabIndex = 8;
            this.picLoading.TabStop = false;
            this.picLoading.Visible = false;
            // 
            // lblProconReloading
            // 
            this.lblProconReloading.AutoSize = true;
            this.lblProconReloading.Image = ((System.Drawing.Image)(resources.GetObject("lblProconReloading.Image")));
            this.lblProconReloading.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblProconReloading.Location = new System.Drawing.Point(11, 89);
            this.lblProconReloading.Name = "lblProconReloading";
            this.lblProconReloading.Size = new System.Drawing.Size(292, 15);
            this.lblProconReloading.TabIndex = 11;
            this.lblProconReloading.Text = "      PRoCon will restart once the update is complete";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(577, 140);
            this.Controls.Add(this.lblProconReloading);
            this.Controls.Add(this.picError);
            this.Controls.Add(this.picSuccess);
            this.Controls.Add(this.picLoading);
            this.Controls.Add(this.picUpdatingDirectory);
            this.Controls.Add(this.picBackingUpConfigs);
            this.Controls.Add(this.picCheckingProconOpen);
            this.Controls.Add(this.lblUpdatingDirectory);
            this.Controls.Add(this.lblBackingUpConfigs);
            this.Controls.Add(this.lblCheckingProconOpen);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PRoCon Frostbite Updater";
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picCheckingProconOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBackingUpConfigs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUpdatingDirectory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSuccess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrKiBsCounter;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblCheckingProconOpen;
        private System.Windows.Forms.Label lblBackingUpConfigs;
        private System.Windows.Forms.Label lblUpdatingDirectory;
        private System.Windows.Forms.PictureBox picCheckingProconOpen;
        private System.Windows.Forms.PictureBox picBackingUpConfigs;
        private System.Windows.Forms.PictureBox picUpdatingDirectory;
        private System.Windows.Forms.PictureBox picError;
        private System.Windows.Forms.PictureBox picSuccess;
        private System.Windows.Forms.PictureBox picLoading;
        private System.Windows.Forms.Label lblProconReloading;
    }
}

