namespace PRoCon.Controls {
    partial class BattlemapView {
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
            this.tmrKillFadeout = new System.Windows.Forms.Timer(this.components);
            this.ctxPointer = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fitOnScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.actualPixelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxZones = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxPointer.SuspendLayout();
            this.ctxZones.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrKillFadeout
            // 
            this.tmrKillFadeout.Enabled = true;
            this.tmrKillFadeout.Interval = 50;
            this.tmrKillFadeout.Tick += new System.EventHandler(this.tmrKillFadeout_Tick);
            // 
            // ctxPointer
            // 
            this.ctxPointer.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fitOnScreenToolStripMenuItem,
            this.actualPixelsToolStripMenuItem});
            this.ctxPointer.Name = "ctxPointer";
            this.ctxPointer.Size = new System.Drawing.Size(143, 48);
            // 
            // fitOnScreenToolStripMenuItem
            // 
            this.fitOnScreenToolStripMenuItem.Name = "fitOnScreenToolStripMenuItem";
            this.fitOnScreenToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.fitOnScreenToolStripMenuItem.Text = "Fit on Screen";
            this.fitOnScreenToolStripMenuItem.Click += new System.EventHandler(this.fitOnScreenToolStripMenuItem_Click);
            // 
            // actualPixelsToolStripMenuItem
            // 
            this.actualPixelsToolStripMenuItem.Name = "actualPixelsToolStripMenuItem";
            this.actualPixelsToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.actualPixelsToolStripMenuItem.Text = "Actual Pixels";
            this.actualPixelsToolStripMenuItem.Click += new System.EventHandler(this.actualPixelsToolStripMenuItem_Click);
            // 
            // ctxZones
            // 
            this.ctxZones.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.ctxZones.Name = "ctxZones";
            this.ctxZones.Size = new System.Drawing.Size(153, 48);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // BattlemapView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "BattlemapView";
            this.Size = new System.Drawing.Size(175, 173);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BattlemapView_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OverheadMapView_MouseDown);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OverheadMapView_MouseUp);
            this.ctxPointer.ResumeLayout(false);
            this.ctxZones.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer tmrKillFadeout;
        private System.Windows.Forms.ContextMenuStrip ctxPointer;
        private System.Windows.Forms.ToolStripMenuItem fitOnScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem actualPixelsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ctxZones;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}
