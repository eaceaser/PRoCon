namespace PRoCon.Controls {
    partial class uscMapViewer {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(uscMapViewer));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbPointer = new System.Windows.Forms.ToolStripButton();
            this.tsbTeamColours = new System.Windows.Forms.ToolStripButton();
            this.tsbMeasuringTool = new System.Windows.Forms.ToolStripButton();
            this.tsbMapZonesTools = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblImagePack = new System.Windows.Forms.ToolStripLabel();
            this.cboImagePacks = new System.Windows.Forms.ToolStripComboBox();
            this.tsbCalibration = new System.Windows.Forms.ToolStripButton();
            this.spltCalibration = new System.Windows.Forms.SplitContainer();
            this.spltZoneTags = new System.Windows.Forms.SplitContainer();
            this.grpZoneTags = new System.Windows.Forms.GroupBox();
            this.lblTagsHelp = new System.Windows.Forms.Label();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.cboTagList = new System.Windows.Forms.ComboBox();
            this.txtTagList = new System.Windows.Forms.TextBox();
            this.lblMapPackFilePath = new System.Windows.Forms.Label();
            this.grpMarkerCollection = new System.Windows.Forms.GroupBox();
            this.btnClearMarkers = new System.Windows.Forms.Button();
            this.lblTrackPlayersList = new System.Windows.Forms.Label();
            this.cboPlayers = new System.Windows.Forms.ComboBox();
            this.btnSaveCalibration = new System.Windows.Forms.Button();
            this.grpRotation = new System.Windows.Forms.GroupBox();
            this.numRotation = new System.Windows.Forms.NumericUpDown();
            this.lblRotation = new System.Windows.Forms.Label();
            this.btnClockwise = new System.Windows.Forms.Button();
            this.btnCounterClockwise = new System.Windows.Forms.Button();
            this.grpZoom = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblZoomYValue = new System.Windows.Forms.Label();
            this.trkZoomY = new System.Windows.Forms.TrackBar();
            this.lblZoomXValue = new System.Windows.Forms.Label();
            this.trkZoomX = new System.Windows.Forms.TrackBar();
            this.grpOffset = new System.Windows.Forms.GroupBox();
            this.lblOffsetYValue = new System.Windows.Forms.Label();
            this.lblOffsetXValue = new System.Windows.Forms.Label();
            this.trkOriginY = new System.Windows.Forms.TrackBar();
            this.chkLockAxis = new System.Windows.Forms.CheckBox();
            this.lblOriginY = new System.Windows.Forms.Label();
            this.trkOriginX = new System.Windows.Forms.TrackBar();
            this.lblOriginX = new System.Windows.Forms.Label();
            this.uscBattlemap = new PRoCon.Controls.BattlemapView();
            this.toolStrip1.SuspendLayout();
            this.spltCalibration.Panel1.SuspendLayout();
            this.spltCalibration.Panel2.SuspendLayout();
            this.spltCalibration.SuspendLayout();
            this.spltZoneTags.Panel1.SuspendLayout();
            this.spltZoneTags.Panel2.SuspendLayout();
            this.spltZoneTags.SuspendLayout();
            this.grpZoneTags.SuspendLayout();
            this.grpMarkerCollection.SuspendLayout();
            this.grpRotation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRotation)).BeginInit();
            this.grpZoom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkZoomY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkZoomX)).BeginInit();
            this.grpOffset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkOriginY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkOriginX)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbPointer,
            this.tsbTeamColours,
            this.tsbMeasuringTool,
            this.tsbMapZonesTools,
            this.toolStripSeparator1,
            this.lblImagePack,
            this.cboImagePacks,
            this.tsbCalibration});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(918, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbPointer
            // 
            this.tsbPointer.Checked = true;
            this.tsbPointer.CheckOnClick = true;
            this.tsbPointer.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbPointer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPointer.Image = ((System.Drawing.Image)(resources.GetObject("tsbPointer.Image")));
            this.tsbPointer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPointer.Name = "tsbPointer";
            this.tsbPointer.Size = new System.Drawing.Size(23, 22);
            this.tsbPointer.Text = "Pointer";
            this.tsbPointer.CheckedChanged += new System.EventHandler(this.tsbPointer_CheckedChanged);
            // 
            // tsbTeamColours
            // 
            this.tsbTeamColours.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbTeamColours.Image = ((System.Drawing.Image)(resources.GetObject("tsbTeamColours.Image")));
            this.tsbTeamColours.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTeamColours.Name = "tsbTeamColours";
            this.tsbTeamColours.Size = new System.Drawing.Size(23, 22);
            this.tsbTeamColours.Text = "Enemy Colours";
            this.tsbTeamColours.Click += new System.EventHandler(this.tsbTeamColours_Click);
            // 
            // tsbMeasuringTool
            // 
            this.tsbMeasuringTool.CheckOnClick = true;
            this.tsbMeasuringTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMeasuringTool.Image = ((System.Drawing.Image)(resources.GetObject("tsbMeasuringTool.Image")));
            this.tsbMeasuringTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMeasuringTool.Name = "tsbMeasuringTool";
            this.tsbMeasuringTool.Size = new System.Drawing.Size(23, 22);
            this.tsbMeasuringTool.Text = "Measuring Tool";
            this.tsbMeasuringTool.CheckedChanged += new System.EventHandler(this.tsbDistanceTool_CheckedChanged);
            // 
            // tsbMapZonesTools
            // 
            this.tsbMapZonesTools.CheckOnClick = true;
            this.tsbMapZonesTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMapZonesTools.Image = ((System.Drawing.Image)(resources.GetObject("tsbMapZonesTools.Image")));
            this.tsbMapZonesTools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMapZonesTools.Name = "tsbMapZonesTools";
            this.tsbMapZonesTools.Size = new System.Drawing.Size(23, 22);
            this.tsbMapZonesTools.Text = "Map Zones";
            this.tsbMapZonesTools.CheckedChanged += new System.EventHandler(this.tsbMapZonesTools_CheckedChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblImagePack
            // 
            this.lblImagePack.Name = "lblImagePack";
            this.lblImagePack.Size = new System.Drawing.Size(71, 22);
            this.lblImagePack.Text = "Image pack:";
            // 
            // cboImagePacks
            // 
            this.cboImagePacks.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboImagePacks.Name = "cboImagePacks";
            this.cboImagePacks.Size = new System.Drawing.Size(233, 25);
            this.cboImagePacks.SelectedIndexChanged += new System.EventHandler(this.cboImagePacks_SelectedIndexChanged);
            // 
            // tsbCalibration
            // 
            this.tsbCalibration.CheckOnClick = true;
            this.tsbCalibration.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCalibration.Image = ((System.Drawing.Image)(resources.GetObject("tsbCalibration.Image")));
            this.tsbCalibration.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCalibration.Name = "tsbCalibration";
            this.tsbCalibration.Size = new System.Drawing.Size(23, 22);
            this.tsbCalibration.Text = "Calibration";
            this.tsbCalibration.CheckedChanged += new System.EventHandler(this.tsbCalibration_CheckedChanged);
            // 
            // spltCalibration
            // 
            this.spltCalibration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltCalibration.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spltCalibration.IsSplitterFixed = true;
            this.spltCalibration.Location = new System.Drawing.Point(0, 25);
            this.spltCalibration.Name = "spltCalibration";
            // 
            // spltCalibration.Panel1
            // 
            this.spltCalibration.Panel1.Controls.Add(this.spltZoneTags);
            // 
            // spltCalibration.Panel2
            // 
            this.spltCalibration.Panel2.Controls.Add(this.lblMapPackFilePath);
            this.spltCalibration.Panel2.Controls.Add(this.grpMarkerCollection);
            this.spltCalibration.Panel2.Controls.Add(this.btnSaveCalibration);
            this.spltCalibration.Panel2.Controls.Add(this.grpRotation);
            this.spltCalibration.Panel2.Controls.Add(this.grpZoom);
            this.spltCalibration.Panel2.Controls.Add(this.grpOffset);
            this.spltCalibration.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.spltCalibration.Panel2Collapsed = true;
            this.spltCalibration.Size = new System.Drawing.Size(918, 644);
            this.spltCalibration.SplitterDistance = 628;
            this.spltCalibration.SplitterWidth = 5;
            this.spltCalibration.TabIndex = 2;
            // 
            // spltZoneTags
            // 
            this.spltZoneTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spltZoneTags.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spltZoneTags.IsSplitterFixed = true;
            this.spltZoneTags.Location = new System.Drawing.Point(0, 0);
            this.spltZoneTags.Name = "spltZoneTags";
            // 
            // spltZoneTags.Panel1
            // 
            this.spltZoneTags.Panel1.Controls.Add(this.uscBattlemap);
            // 
            // spltZoneTags.Panel2
            // 
            this.spltZoneTags.Panel2.Controls.Add(this.grpZoneTags);
            this.spltZoneTags.Panel2.Enabled = false;
            this.spltZoneTags.Panel2.Padding = new System.Windows.Forms.Padding(3);
            this.spltZoneTags.Panel2Collapsed = true;
            this.spltZoneTags.Size = new System.Drawing.Size(918, 644);
            this.spltZoneTags.SplitterDistance = 609;
            this.spltZoneTags.TabIndex = 3;
            // 
            // grpZoneTags
            // 
            this.grpZoneTags.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.grpZoneTags.Controls.Add(this.lblTagsHelp);
            this.grpZoneTags.Controls.Add(this.btnAddTag);
            this.grpZoneTags.Controls.Add(this.cboTagList);
            this.grpZoneTags.Controls.Add(this.txtTagList);
            this.grpZoneTags.Location = new System.Drawing.Point(6, 6);
            this.grpZoneTags.Name = "grpZoneTags";
            this.grpZoneTags.Padding = new System.Windows.Forms.Padding(6);
            this.grpZoneTags.Size = new System.Drawing.Size(256, 257);
            this.grpZoneTags.TabIndex = 0;
            this.grpZoneTags.TabStop = false;
            this.grpZoneTags.Text = "Zone Tags";
            // 
            // lblTagsHelp
            // 
            this.lblTagsHelp.Location = new System.Drawing.Point(12, 22);
            this.lblTagsHelp.Name = "lblTagsHelp";
            this.lblTagsHelp.Size = new System.Drawing.Size(231, 49);
            this.lblTagsHelp.TabIndex = 3;
            this.lblTagsHelp.Text = "Tags and map functionality are provided by plugins.";
            // 
            // btnAddTag
            // 
            this.btnAddTag.FlatAppearance.BorderSize = 0;
            this.btnAddTag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTag.Location = new System.Drawing.Point(209, 73);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(34, 23);
            this.btnAddTag.TabIndex = 2;
            this.btnAddTag.UseVisualStyleBackColor = true;
            this.btnAddTag.Click += new System.EventHandler(this.btnAddTag_Click);
            // 
            // cboTagList
            // 
            this.cboTagList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTagList.FormattingEnabled = true;
            this.cboTagList.Location = new System.Drawing.Point(9, 74);
            this.cboTagList.Name = "cboTagList";
            this.cboTagList.Size = new System.Drawing.Size(194, 23);
            this.cboTagList.TabIndex = 1;
            // 
            // txtTagList
            // 
            this.txtTagList.Location = new System.Drawing.Point(9, 102);
            this.txtTagList.Multiline = true;
            this.txtTagList.Name = "txtTagList";
            this.txtTagList.Size = new System.Drawing.Size(234, 146);
            this.txtTagList.TabIndex = 0;
            this.txtTagList.TextChanged += new System.EventHandler(this.txtTagList_TextChanged);
            // 
            // lblMapPackFilePath
            // 
            this.lblMapPackFilePath.Location = new System.Drawing.Point(13, 471);
            this.lblMapPackFilePath.Name = "lblMapPackFilePath";
            this.lblMapPackFilePath.Size = new System.Drawing.Size(245, 112);
            this.lblMapPackFilePath.TabIndex = 11;
            // 
            // grpMarkerCollection
            // 
            this.grpMarkerCollection.Controls.Add(this.btnClearMarkers);
            this.grpMarkerCollection.Controls.Add(this.lblTrackPlayersList);
            this.grpMarkerCollection.Controls.Add(this.cboPlayers);
            this.grpMarkerCollection.Location = new System.Drawing.Point(6, 6);
            this.grpMarkerCollection.Name = "grpMarkerCollection";
            this.grpMarkerCollection.Size = new System.Drawing.Size(252, 100);
            this.grpMarkerCollection.TabIndex = 10;
            this.grpMarkerCollection.TabStop = false;
            this.grpMarkerCollection.Text = "Marker Collection";
            // 
            // btnClearMarkers
            // 
            this.btnClearMarkers.Location = new System.Drawing.Point(74, 66);
            this.btnClearMarkers.Name = "btnClearMarkers";
            this.btnClearMarkers.Size = new System.Drawing.Size(167, 23);
            this.btnClearMarkers.TabIndex = 2;
            this.btnClearMarkers.Text = "Clear Markers";
            this.btnClearMarkers.UseVisualStyleBackColor = true;
            this.btnClearMarkers.Click += new System.EventHandler(this.btnClearMarkers_Click);
            // 
            // lblTrackPlayersList
            // 
            this.lblTrackPlayersList.AutoSize = true;
            this.lblTrackPlayersList.Location = new System.Drawing.Point(7, 19);
            this.lblTrackPlayersList.Name = "lblTrackPlayersList";
            this.lblTrackPlayersList.Size = new System.Drawing.Size(98, 15);
            this.lblTrackPlayersList.TabIndex = 1;
            this.lblTrackPlayersList.Text = "Mark suicides by:";
            // 
            // cboPlayers
            // 
            this.cboPlayers.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlayers.FormattingEnabled = true;
            this.cboPlayers.Location = new System.Drawing.Point(10, 37);
            this.cboPlayers.Name = "cboPlayers";
            this.cboPlayers.Size = new System.Drawing.Size(231, 23);
            this.cboPlayers.TabIndex = 0;
            // 
            // btnSaveCalibration
            // 
            this.btnSaveCalibration.Location = new System.Drawing.Point(123, 586);
            this.btnSaveCalibration.Name = "btnSaveCalibration";
            this.btnSaveCalibration.Size = new System.Drawing.Size(135, 23);
            this.btnSaveCalibration.TabIndex = 9;
            this.btnSaveCalibration.Text = "Save";
            this.btnSaveCalibration.UseVisualStyleBackColor = true;
            this.btnSaveCalibration.Click += new System.EventHandler(this.btnSaveCalibration_Click);
            // 
            // grpRotation
            // 
            this.grpRotation.Controls.Add(this.numRotation);
            this.grpRotation.Controls.Add(this.lblRotation);
            this.grpRotation.Controls.Add(this.btnClockwise);
            this.grpRotation.Controls.Add(this.btnCounterClockwise);
            this.grpRotation.Location = new System.Drawing.Point(6, 402);
            this.grpRotation.Name = "grpRotation";
            this.grpRotation.Size = new System.Drawing.Size(252, 66);
            this.grpRotation.TabIndex = 8;
            this.grpRotation.TabStop = false;
            this.grpRotation.Text = "Rotation";
            // 
            // numRotation
            // 
            this.numRotation.Location = new System.Drawing.Point(101, 24);
            this.numRotation.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
            this.numRotation.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.numRotation.Name = "numRotation";
            this.numRotation.Size = new System.Drawing.Size(46, 23);
            this.numRotation.TabIndex = 3;
            this.numRotation.ValueChanged += new System.EventHandler(this.numRotation_ValueChanged);
            // 
            // lblRotation
            // 
            this.lblRotation.AutoSize = true;
            this.lblRotation.Location = new System.Drawing.Point(148, 27);
            this.lblRotation.Name = "lblRotation";
            this.lblRotation.Size = new System.Drawing.Size(12, 15);
            this.lblRotation.TabIndex = 2;
            this.lblRotation.Text = "°";
            // 
            // btnClockwise
            // 
            this.btnClockwise.FlatAppearance.BorderSize = 0;
            this.btnClockwise.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClockwise.Location = new System.Drawing.Point(206, 22);
            this.btnClockwise.Name = "btnClockwise";
            this.btnClockwise.Size = new System.Drawing.Size(35, 23);
            this.btnClockwise.TabIndex = 1;
            this.btnClockwise.UseVisualStyleBackColor = true;
            this.btnClockwise.Click += new System.EventHandler(this.btnClockwise_Click);
            // 
            // btnCounterClockwise
            // 
            this.btnCounterClockwise.FlatAppearance.BorderSize = 0;
            this.btnCounterClockwise.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCounterClockwise.Location = new System.Drawing.Point(30, 22);
            this.btnCounterClockwise.Name = "btnCounterClockwise";
            this.btnCounterClockwise.Size = new System.Drawing.Size(35, 23);
            this.btnCounterClockwise.TabIndex = 0;
            this.btnCounterClockwise.UseVisualStyleBackColor = true;
            this.btnCounterClockwise.Click += new System.EventHandler(this.btnCounterClockwise_Click);
            // 
            // grpZoom
            // 
            this.grpZoom.Controls.Add(this.label2);
            this.grpZoom.Controls.Add(this.label3);
            this.grpZoom.Controls.Add(this.lblZoomYValue);
            this.grpZoom.Controls.Add(this.trkZoomY);
            this.grpZoom.Controls.Add(this.lblZoomXValue);
            this.grpZoom.Controls.Add(this.trkZoomX);
            this.grpZoom.Location = new System.Drawing.Point(6, 272);
            this.grpZoom.Name = "grpZoom";
            this.grpZoom.Size = new System.Drawing.Size(252, 124);
            this.grpZoom.TabIndex = 7;
            this.grpZoom.TabStop = false;
            this.grpZoom.Text = "Zoom";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(14, 15);
            this.label2.TabIndex = 11;
            this.label2.Text = "Y";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 15);
            this.label3.TabIndex = 10;
            this.label3.Text = "X";
            // 
            // lblZoomYValue
            // 
            this.lblZoomYValue.AutoSize = true;
            this.lblZoomYValue.Location = new System.Drawing.Point(217, 76);
            this.lblZoomYValue.Name = "lblZoomYValue";
            this.lblZoomYValue.Size = new System.Drawing.Size(14, 15);
            this.lblZoomYValue.TabIndex = 9;
            this.lblZoomYValue.Text = "Y";
            // 
            // trkZoomY
            // 
            this.trkZoomY.BackColor = System.Drawing.SystemColors.Window;
            this.trkZoomY.LargeChange = 10;
            this.trkZoomY.Location = new System.Drawing.Point(30, 66);
            this.trkZoomY.Maximum = 2000;
            this.trkZoomY.Minimum = 1;
            this.trkZoomY.Name = "trkZoomY";
            this.trkZoomY.Size = new System.Drawing.Size(181, 45);
            this.trkZoomY.TabIndex = 8;
            this.trkZoomY.TickFrequency = 5;
            this.trkZoomY.Value = 1;
            this.trkZoomY.ValueChanged += new System.EventHandler(this.trkZoomY_ValueChanged);
            // 
            // lblZoomXValue
            // 
            this.lblZoomXValue.AutoSize = true;
            this.lblZoomXValue.Location = new System.Drawing.Point(217, 25);
            this.lblZoomXValue.Name = "lblZoomXValue";
            this.lblZoomXValue.Size = new System.Drawing.Size(14, 15);
            this.lblZoomXValue.TabIndex = 7;
            this.lblZoomXValue.Text = "Y";
            // 
            // trkZoomX
            // 
            this.trkZoomX.BackColor = System.Drawing.SystemColors.Window;
            this.trkZoomX.LargeChange = 10;
            this.trkZoomX.Location = new System.Drawing.Point(30, 22);
            this.trkZoomX.Maximum = 2000;
            this.trkZoomX.Minimum = 1;
            this.trkZoomX.Name = "trkZoomX";
            this.trkZoomX.Size = new System.Drawing.Size(181, 45);
            this.trkZoomX.TabIndex = 0;
            this.trkZoomX.TickFrequency = 5;
            this.trkZoomX.Value = 1;
            this.trkZoomX.ValueChanged += new System.EventHandler(this.trkZoom_ValueChanged);
            // 
            // grpOffset
            // 
            this.grpOffset.Controls.Add(this.lblOffsetYValue);
            this.grpOffset.Controls.Add(this.lblOffsetXValue);
            this.grpOffset.Controls.Add(this.trkOriginY);
            this.grpOffset.Controls.Add(this.chkLockAxis);
            this.grpOffset.Controls.Add(this.lblOriginY);
            this.grpOffset.Controls.Add(this.trkOriginX);
            this.grpOffset.Controls.Add(this.lblOriginX);
            this.grpOffset.Location = new System.Drawing.Point(6, 112);
            this.grpOffset.Name = "grpOffset";
            this.grpOffset.Size = new System.Drawing.Size(252, 153);
            this.grpOffset.TabIndex = 6;
            this.grpOffset.TabStop = false;
            this.grpOffset.Text = "Offset";
            // 
            // lblOffsetYValue
            // 
            this.lblOffsetYValue.AutoSize = true;
            this.lblOffsetYValue.Location = new System.Drawing.Point(216, 96);
            this.lblOffsetYValue.Name = "lblOffsetYValue";
            this.lblOffsetYValue.Size = new System.Drawing.Size(14, 15);
            this.lblOffsetYValue.TabIndex = 6;
            this.lblOffsetYValue.Text = "Y";
            // 
            // lblOffsetXValue
            // 
            this.lblOffsetXValue.AutoSize = true;
            this.lblOffsetXValue.Location = new System.Drawing.Point(216, 50);
            this.lblOffsetXValue.Name = "lblOffsetXValue";
            this.lblOffsetXValue.Size = new System.Drawing.Size(14, 15);
            this.lblOffsetXValue.TabIndex = 5;
            this.lblOffsetXValue.Text = "X";
            // 
            // trkOriginY
            // 
            this.trkOriginY.BackColor = System.Drawing.SystemColors.Window;
            this.trkOriginY.Enabled = false;
            this.trkOriginY.LargeChange = 32;
            this.trkOriginY.Location = new System.Drawing.Point(30, 93);
            this.trkOriginY.Maximum = 8192;
            this.trkOriginY.Minimum = -8192;
            this.trkOriginY.Name = "trkOriginY";
            this.trkOriginY.Size = new System.Drawing.Size(181, 45);
            this.trkOriginY.TabIndex = 2;
            this.trkOriginY.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trkOriginY.ValueChanged += new System.EventHandler(this.trkOriginY_ValueChanged);
            // 
            // chkLockAxis
            // 
            this.chkLockAxis.AutoSize = true;
            this.chkLockAxis.Checked = true;
            this.chkLockAxis.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLockAxis.Location = new System.Drawing.Point(30, 22);
            this.chkLockAxis.Name = "chkLockAxis";
            this.chkLockAxis.Size = new System.Drawing.Size(75, 19);
            this.chkLockAxis.TabIndex = 0;
            this.chkLockAxis.Text = "Lock Axis";
            this.chkLockAxis.UseVisualStyleBackColor = true;
            this.chkLockAxis.CheckedChanged += new System.EventHandler(this.chkLockAxis_CheckedChanged);
            // 
            // lblOriginY
            // 
            this.lblOriginY.AutoSize = true;
            this.lblOriginY.Location = new System.Drawing.Point(7, 96);
            this.lblOriginY.Name = "lblOriginY";
            this.lblOriginY.Size = new System.Drawing.Size(14, 15);
            this.lblOriginY.TabIndex = 4;
            this.lblOriginY.Text = "Y";
            // 
            // trkOriginX
            // 
            this.trkOriginX.BackColor = System.Drawing.SystemColors.Window;
            this.trkOriginX.LargeChange = 32;
            this.trkOriginX.Location = new System.Drawing.Point(30, 48);
            this.trkOriginX.Maximum = 8192;
            this.trkOriginX.Minimum = -8192;
            this.trkOriginX.Name = "trkOriginX";
            this.trkOriginX.Size = new System.Drawing.Size(181, 45);
            this.trkOriginX.TabIndex = 1;
            this.trkOriginX.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trkOriginX.ValueChanged += new System.EventHandler(this.trkOriginX_ValueChanged);
            // 
            // lblOriginX
            // 
            this.lblOriginX.AutoSize = true;
            this.lblOriginX.Location = new System.Drawing.Point(7, 50);
            this.lblOriginX.Name = "lblOriginX";
            this.lblOriginX.Size = new System.Drawing.Size(14, 15);
            this.lblOriginX.TabIndex = 3;
            this.lblOriginX.Text = "X";
            // 
            // uscBattlemap
            // 
            this.uscBattlemap.DisplayCalibrationGrid = false;
            this.uscBattlemap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uscBattlemap.ErrorRadius = 14;
            this.uscBattlemap.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.uscBattlemap.FullyLoadMap = false;
            this.uscBattlemap.KillColours = PRoCon.Controls.Battlemap.KillDisplay.KillDisplayColours.TeamColours;
            this.uscBattlemap.KillEventFadeout = 5000;
            this.uscBattlemap.LoadedMapImagePack = null;
            this.uscBattlemap.Location = new System.Drawing.Point(0, 0);
            this.uscBattlemap.Name = "uscBattlemap";
            this.uscBattlemap.SelectedTool = PRoCon.Controls.Battlemap.BattlemapViewTools.Pointer;
            this.uscBattlemap.Size = new System.Drawing.Size(918, 644);
            this.uscBattlemap.TabIndex = 2;
            this.uscBattlemap.ZoomFactor = 1F;
            this.uscBattlemap.KillColoursChanged += new PRoCon.Controls.BattlemapView.KillColoursChangedHandler(this.uscBattlemap_KillColoursChanged);
            // 
            // uscMapViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.spltCalibration);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "uscMapViewer";
            this.Size = new System.Drawing.Size(918, 669);
            this.Load += new System.EventHandler(this.uscMapViewer_Load);
            this.Resize += new System.EventHandler(this.uscMapViewer_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.spltCalibration.Panel1.ResumeLayout(false);
            this.spltCalibration.Panel2.ResumeLayout(false);
            this.spltCalibration.ResumeLayout(false);
            this.spltZoneTags.Panel1.ResumeLayout(false);
            this.spltZoneTags.Panel2.ResumeLayout(false);
            this.spltZoneTags.ResumeLayout(false);
            this.grpZoneTags.ResumeLayout(false);
            this.grpZoneTags.PerformLayout();
            this.grpMarkerCollection.ResumeLayout(false);
            this.grpMarkerCollection.PerformLayout();
            this.grpRotation.ResumeLayout(false);
            this.grpRotation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRotation)).EndInit();
            this.grpZoom.ResumeLayout(false);
            this.grpZoom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkZoomY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkZoomX)).EndInit();
            this.grpOffset.ResumeLayout(false);
            this.grpOffset.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkOriginY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkOriginX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbMapZonesTools;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox cboImagePacks;
        private System.Windows.Forms.ToolStripLabel lblImagePack;
        private System.Windows.Forms.ToolStripButton tsbPointer;
        private System.Windows.Forms.SplitContainer spltCalibration;
        private BattlemapView uscBattlemap;
        private System.Windows.Forms.ToolStripButton tsbCalibration;
        private System.Windows.Forms.GroupBox grpOffset;
        private System.Windows.Forms.TrackBar trkOriginY;
        private System.Windows.Forms.CheckBox chkLockAxis;
        private System.Windows.Forms.Label lblOriginY;
        private System.Windows.Forms.TrackBar trkOriginX;
        private System.Windows.Forms.Label lblOriginX;
        private System.Windows.Forms.Label lblOffsetYValue;
        private System.Windows.Forms.Label lblOffsetXValue;
        private System.Windows.Forms.GroupBox grpZoom;
        private System.Windows.Forms.TrackBar trkZoomX;
        private System.Windows.Forms.Label lblZoomXValue;
        private System.Windows.Forms.GroupBox grpRotation;
        private System.Windows.Forms.Button btnClockwise;
        private System.Windows.Forms.Button btnCounterClockwise;
        private System.Windows.Forms.Label lblRotation;
        private System.Windows.Forms.Button btnSaveCalibration;
        private System.Windows.Forms.GroupBox grpMarkerCollection;
        private System.Windows.Forms.Label lblTrackPlayersList;
        private System.Windows.Forms.ComboBox cboPlayers;
        private System.Windows.Forms.Button btnClearMarkers;
        private System.Windows.Forms.Label lblMapPackFilePath;
        private System.Windows.Forms.ToolStripButton tsbMeasuringTool;
        private System.Windows.Forms.ToolStripButton tsbTeamColours;
        private System.Windows.Forms.SplitContainer spltZoneTags;
        private System.Windows.Forms.GroupBox grpZoneTags;
        private System.Windows.Forms.TextBox txtTagList;
        private System.Windows.Forms.ComboBox cboTagList;
        private System.Windows.Forms.Button btnAddTag;
        private System.Windows.Forms.Label lblTagsHelp;
        private System.Windows.Forms.NumericUpDown numRotation;
        private System.Windows.Forms.Label lblZoomYValue;
        private System.Windows.Forms.TrackBar trkZoomY;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
