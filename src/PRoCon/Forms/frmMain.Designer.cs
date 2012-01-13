namespace PRoCon.Forms
{
    using PRoCon;

    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.stsMain = new System.Windows.Forms.StatusStrip();
            this.toolStripDownloadProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripDownloading = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripRssFeed = new System.Windows.Forms.ToolStripStatusLabel();
            this.picLayerOffline = new System.Windows.Forms.PictureBox();
            this.picLayerOnline = new System.Windows.Forms.PictureBox();
            this.picPortCheckerClosed = new System.Windows.Forms.PictureBox();
            this.picPortCheckerOpen = new System.Windows.Forms.PictureBox();
            this.picPortCheckerUnknown = new System.Windows.Forms.PictureBox();
            this.picAjaxStyleSuccess = new System.Windows.Forms.PictureBox();
            this.picAjaxStyleFail = new System.Windows.Forms.PictureBox();
            this.picAjaxStyleLoading = new System.Windows.Forms.PictureBox();
            this.iglFlags = new System.Windows.Forms.ImageList(this.components);
            this.ntfIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.iglIcons = new System.Windows.Forms.ImageList(this.components);
            this.tltpUpdateComplete = new System.Windows.Forms.ToolTip(this.components);
            this.pnlWindows = new System.Windows.Forms.Panel();
            this.iglGameIcons = new System.Windows.Forms.ImageList(this.components);
            this.tlsConnections = new PRoCon.Controls.ControlsEx.ToolStripNF();
            this.btnStartPage = new System.Windows.Forms.ToolStripButton();
            this.btnShiftServerPrevious = new System.Windows.Forms.ToolStripButton();
            this.btnShiftServerNext = new System.Windows.Forms.ToolStripButton();
            this.cboServerList = new System.Windows.Forms.ToolStripComboBox();
            this.toolsStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnConnectDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.chkAutomaticallyConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageAccountsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changelogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stsMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLayerOffline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLayerOnline)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPortCheckerClosed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPortCheckerOpen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPortCheckerUnknown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAjaxStyleSuccess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAjaxStyleFail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAjaxStyleLoading)).BeginInit();
            this.tlsConnections.SuspendLayout();
            this.SuspendLayout();
            // 
            // stsMain
            // 
            this.stsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDownloadProgress,
            this.toolStripDownloading,
            this.toolStripRssFeed});
            this.stsMain.Location = new System.Drawing.Point(0, 740);
            this.stsMain.Name = "stsMain";
            this.stsMain.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.stsMain.Size = new System.Drawing.Size(1008, 22);
            this.stsMain.TabIndex = 1;
            this.stsMain.Text = "statusStrip1";
            // 
            // toolStripDownloadProgress
            // 
            this.toolStripDownloadProgress.Name = "toolStripDownloadProgress";
            this.toolStripDownloadProgress.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripDownloadProgress.Size = new System.Drawing.Size(100, 16);
            this.toolStripDownloadProgress.Visible = false;
            // 
            // toolStripDownloading
            // 
            this.toolStripDownloading.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.toolStripDownloading.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.toolStripDownloading.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.toolStripDownloading.Name = "toolStripDownloading";
            this.toolStripDownloading.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripDownloading.Size = new System.Drawing.Size(19, 17);
            this.toolStripDownloading.Text = "....";
            this.toolStripDownloading.Visible = false;
            this.toolStripDownloading.Click += new System.EventHandler(this.toolStripDownloading_Click);
            // 
            // toolStripRssFeed
            // 
            this.toolStripRssFeed.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.toolStripRssFeed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripRssFeed.IsLink = true;
            this.toolStripRssFeed.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.toolStripRssFeed.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(220)))));
            this.toolStripRssFeed.Name = "toolStripRssFeed";
            this.toolStripRssFeed.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripRssFeed.Size = new System.Drawing.Size(991, 17);
            this.toolStripRssFeed.Spring = true;
            this.toolStripRssFeed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picLayerOffline
            // 
            this.picLayerOffline.Image = ((System.Drawing.Image)(resources.GetObject("picLayerOffline.Image")));
            this.picLayerOffline.Location = new System.Drawing.Point(300, 87);
            this.picLayerOffline.Name = "picLayerOffline";
            this.picLayerOffline.Size = new System.Drawing.Size(65, 52);
            this.picLayerOffline.TabIndex = 7;
            this.picLayerOffline.TabStop = false;
            this.picLayerOffline.Visible = false;
            // 
            // picLayerOnline
            // 
            this.picLayerOnline.Image = ((System.Drawing.Image)(resources.GetObject("picLayerOnline.Image")));
            this.picLayerOnline.Location = new System.Drawing.Point(371, 87);
            this.picLayerOnline.Name = "picLayerOnline";
            this.picLayerOnline.Size = new System.Drawing.Size(65, 52);
            this.picLayerOnline.TabIndex = 6;
            this.picLayerOnline.TabStop = false;
            this.picLayerOnline.Visible = false;
            // 
            // picPortCheckerClosed
            // 
            this.picPortCheckerClosed.Image = ((System.Drawing.Image)(resources.GetObject("picPortCheckerClosed.Image")));
            this.picPortCheckerClosed.Location = new System.Drawing.Point(285, 180);
            this.picPortCheckerClosed.Name = "picPortCheckerClosed";
            this.picPortCheckerClosed.Size = new System.Drawing.Size(69, 52);
            this.picPortCheckerClosed.TabIndex = 5;
            this.picPortCheckerClosed.TabStop = false;
            this.picPortCheckerClosed.Visible = false;
            // 
            // picPortCheckerOpen
            // 
            this.picPortCheckerOpen.Image = ((System.Drawing.Image)(resources.GetObject("picPortCheckerOpen.Image")));
            this.picPortCheckerOpen.Location = new System.Drawing.Point(250, 180);
            this.picPortCheckerOpen.Name = "picPortCheckerOpen";
            this.picPortCheckerOpen.Size = new System.Drawing.Size(28, 25);
            this.picPortCheckerOpen.TabIndex = 4;
            this.picPortCheckerOpen.TabStop = false;
            this.picPortCheckerOpen.Visible = false;
            // 
            // picPortCheckerUnknown
            // 
            this.picPortCheckerUnknown.Image = ((System.Drawing.Image)(resources.GetObject("picPortCheckerUnknown.Image")));
            this.picPortCheckerUnknown.Location = new System.Drawing.Point(178, 180);
            this.picPortCheckerUnknown.Name = "picPortCheckerUnknown";
            this.picPortCheckerUnknown.Size = new System.Drawing.Size(65, 52);
            this.picPortCheckerUnknown.TabIndex = 3;
            this.picPortCheckerUnknown.TabStop = false;
            this.picPortCheckerUnknown.Visible = false;
            // 
            // picAjaxStyleSuccess
            // 
            this.picAjaxStyleSuccess.Image = ((System.Drawing.Image)(resources.GetObject("picAjaxStyleSuccess.Image")));
            this.picAjaxStyleSuccess.Location = new System.Drawing.Point(250, 87);
            this.picAjaxStyleSuccess.Name = "picAjaxStyleSuccess";
            this.picAjaxStyleSuccess.Size = new System.Drawing.Size(28, 25);
            this.picAjaxStyleSuccess.TabIndex = 2;
            this.picAjaxStyleSuccess.TabStop = false;
            this.picAjaxStyleSuccess.Visible = false;
            // 
            // picAjaxStyleFail
            // 
            this.picAjaxStyleFail.Image = ((System.Drawing.Image)(resources.GetObject("picAjaxStyleFail.Image")));
            this.picAjaxStyleFail.Location = new System.Drawing.Point(213, 85);
            this.picAjaxStyleFail.Name = "picAjaxStyleFail";
            this.picAjaxStyleFail.Size = new System.Drawing.Size(30, 27);
            this.picAjaxStyleFail.TabIndex = 1;
            this.picAjaxStyleFail.TabStop = false;
            this.picAjaxStyleFail.Visible = false;
            // 
            // picAjaxStyleLoading
            // 
            this.picAjaxStyleLoading.Image = ((System.Drawing.Image)(resources.GetObject("picAjaxStyleLoading.Image")));
            this.picAjaxStyleLoading.Location = new System.Drawing.Point(160, 85);
            this.picAjaxStyleLoading.Name = "picAjaxStyleLoading";
            this.picAjaxStyleLoading.Size = new System.Drawing.Size(45, 40);
            this.picAjaxStyleLoading.TabIndex = 0;
            this.picAjaxStyleLoading.TabStop = false;
            this.picAjaxStyleLoading.Visible = false;
            // 
            // iglFlags
            // 
            this.iglFlags.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iglFlags.ImageStream")));
            this.iglFlags.TransparentColor = System.Drawing.Color.Transparent;
            this.iglFlags.Images.SetKeyName(0, "ad.png");
            this.iglFlags.Images.SetKeyName(1, "ae.png");
            this.iglFlags.Images.SetKeyName(2, "af.png");
            this.iglFlags.Images.SetKeyName(3, "ag.png");
            this.iglFlags.Images.SetKeyName(4, "ai.png");
            this.iglFlags.Images.SetKeyName(5, "al.png");
            this.iglFlags.Images.SetKeyName(6, "am.png");
            this.iglFlags.Images.SetKeyName(7, "an.png");
            this.iglFlags.Images.SetKeyName(8, "ao.png");
            this.iglFlags.Images.SetKeyName(9, "ar.png");
            this.iglFlags.Images.SetKeyName(10, "as.png");
            this.iglFlags.Images.SetKeyName(11, "at.png");
            this.iglFlags.Images.SetKeyName(12, "au.png");
            this.iglFlags.Images.SetKeyName(13, "aw.png");
            this.iglFlags.Images.SetKeyName(14, "ax.png");
            this.iglFlags.Images.SetKeyName(15, "az.png");
            this.iglFlags.Images.SetKeyName(16, "ba.png");
            this.iglFlags.Images.SetKeyName(17, "bb.png");
            this.iglFlags.Images.SetKeyName(18, "bd.png");
            this.iglFlags.Images.SetKeyName(19, "be.png");
            this.iglFlags.Images.SetKeyName(20, "bf.png");
            this.iglFlags.Images.SetKeyName(21, "bg.png");
            this.iglFlags.Images.SetKeyName(22, "bh.png");
            this.iglFlags.Images.SetKeyName(23, "bi.png");
            this.iglFlags.Images.SetKeyName(24, "bj.png");
            this.iglFlags.Images.SetKeyName(25, "bm.png");
            this.iglFlags.Images.SetKeyName(26, "bn.png");
            this.iglFlags.Images.SetKeyName(27, "bo.png");
            this.iglFlags.Images.SetKeyName(28, "br.png");
            this.iglFlags.Images.SetKeyName(29, "bs.png");
            this.iglFlags.Images.SetKeyName(30, "bt.png");
            this.iglFlags.Images.SetKeyName(31, "bv.png");
            this.iglFlags.Images.SetKeyName(32, "bw.png");
            this.iglFlags.Images.SetKeyName(33, "by.png");
            this.iglFlags.Images.SetKeyName(34, "bz.png");
            this.iglFlags.Images.SetKeyName(35, "ca.png");
            this.iglFlags.Images.SetKeyName(36, "catalonia.png");
            this.iglFlags.Images.SetKeyName(37, "cc.png");
            this.iglFlags.Images.SetKeyName(38, "cd.png");
            this.iglFlags.Images.SetKeyName(39, "cf.png");
            this.iglFlags.Images.SetKeyName(40, "cg.png");
            this.iglFlags.Images.SetKeyName(41, "ch.png");
            this.iglFlags.Images.SetKeyName(42, "ci.png");
            this.iglFlags.Images.SetKeyName(43, "ck.png");
            this.iglFlags.Images.SetKeyName(44, "cl.png");
            this.iglFlags.Images.SetKeyName(45, "cm.png");
            this.iglFlags.Images.SetKeyName(46, "cn.png");
            this.iglFlags.Images.SetKeyName(47, "co.png");
            this.iglFlags.Images.SetKeyName(48, "cr.png");
            this.iglFlags.Images.SetKeyName(49, "cs.png");
            this.iglFlags.Images.SetKeyName(50, "cu.png");
            this.iglFlags.Images.SetKeyName(51, "cv.png");
            this.iglFlags.Images.SetKeyName(52, "cx.png");
            this.iglFlags.Images.SetKeyName(53, "cy.png");
            this.iglFlags.Images.SetKeyName(54, "cz.png");
            this.iglFlags.Images.SetKeyName(55, "de.png");
            this.iglFlags.Images.SetKeyName(56, "dj.png");
            this.iglFlags.Images.SetKeyName(57, "dk.png");
            this.iglFlags.Images.SetKeyName(58, "dm.png");
            this.iglFlags.Images.SetKeyName(59, "do.png");
            this.iglFlags.Images.SetKeyName(60, "dz.png");
            this.iglFlags.Images.SetKeyName(61, "ec.png");
            this.iglFlags.Images.SetKeyName(62, "ee.png");
            this.iglFlags.Images.SetKeyName(63, "eg.png");
            this.iglFlags.Images.SetKeyName(64, "eh.png");
            this.iglFlags.Images.SetKeyName(65, "england.png");
            this.iglFlags.Images.SetKeyName(66, "er.png");
            this.iglFlags.Images.SetKeyName(67, "es.png");
            this.iglFlags.Images.SetKeyName(68, "et.png");
            this.iglFlags.Images.SetKeyName(69, "europeanunion.png");
            this.iglFlags.Images.SetKeyName(70, "fam.png");
            this.iglFlags.Images.SetKeyName(71, "fi.png");
            this.iglFlags.Images.SetKeyName(72, "fj.png");
            this.iglFlags.Images.SetKeyName(73, "fk.png");
            this.iglFlags.Images.SetKeyName(74, "fm.png");
            this.iglFlags.Images.SetKeyName(75, "fo.png");
            this.iglFlags.Images.SetKeyName(76, "fr.png");
            this.iglFlags.Images.SetKeyName(77, "ga.png");
            this.iglFlags.Images.SetKeyName(78, "gb.png");
            this.iglFlags.Images.SetKeyName(79, "gd.png");
            this.iglFlags.Images.SetKeyName(80, "ge.png");
            this.iglFlags.Images.SetKeyName(81, "gf.png");
            this.iglFlags.Images.SetKeyName(82, "gh.png");
            this.iglFlags.Images.SetKeyName(83, "gi.png");
            this.iglFlags.Images.SetKeyName(84, "gl.png");
            this.iglFlags.Images.SetKeyName(85, "gm.png");
            this.iglFlags.Images.SetKeyName(86, "gn.png");
            this.iglFlags.Images.SetKeyName(87, "gp.png");
            this.iglFlags.Images.SetKeyName(88, "gq.png");
            this.iglFlags.Images.SetKeyName(89, "gr.png");
            this.iglFlags.Images.SetKeyName(90, "gs.png");
            this.iglFlags.Images.SetKeyName(91, "gt.png");
            this.iglFlags.Images.SetKeyName(92, "gu.png");
            this.iglFlags.Images.SetKeyName(93, "gw.png");
            this.iglFlags.Images.SetKeyName(94, "gy.png");
            this.iglFlags.Images.SetKeyName(95, "hk.png");
            this.iglFlags.Images.SetKeyName(96, "hm.png");
            this.iglFlags.Images.SetKeyName(97, "hn.png");
            this.iglFlags.Images.SetKeyName(98, "hr.png");
            this.iglFlags.Images.SetKeyName(99, "ht.png");
            this.iglFlags.Images.SetKeyName(100, "hu.png");
            this.iglFlags.Images.SetKeyName(101, "id.png");
            this.iglFlags.Images.SetKeyName(102, "ie.png");
            this.iglFlags.Images.SetKeyName(103, "il.png");
            this.iglFlags.Images.SetKeyName(104, "in.png");
            this.iglFlags.Images.SetKeyName(105, "io.png");
            this.iglFlags.Images.SetKeyName(106, "iq.png");
            this.iglFlags.Images.SetKeyName(107, "ir.png");
            this.iglFlags.Images.SetKeyName(108, "is.png");
            this.iglFlags.Images.SetKeyName(109, "it.png");
            this.iglFlags.Images.SetKeyName(110, "jm.png");
            this.iglFlags.Images.SetKeyName(111, "jo.png");
            this.iglFlags.Images.SetKeyName(112, "jp.png");
            this.iglFlags.Images.SetKeyName(113, "ke.png");
            this.iglFlags.Images.SetKeyName(114, "kg.png");
            this.iglFlags.Images.SetKeyName(115, "kh.png");
            this.iglFlags.Images.SetKeyName(116, "ki.png");
            this.iglFlags.Images.SetKeyName(117, "km.png");
            this.iglFlags.Images.SetKeyName(118, "kn.png");
            this.iglFlags.Images.SetKeyName(119, "kp.png");
            this.iglFlags.Images.SetKeyName(120, "kr.png");
            this.iglFlags.Images.SetKeyName(121, "kw.png");
            this.iglFlags.Images.SetKeyName(122, "ky.png");
            this.iglFlags.Images.SetKeyName(123, "kz.png");
            this.iglFlags.Images.SetKeyName(124, "la.png");
            this.iglFlags.Images.SetKeyName(125, "lb.png");
            this.iglFlags.Images.SetKeyName(126, "lc.png");
            this.iglFlags.Images.SetKeyName(127, "li.png");
            this.iglFlags.Images.SetKeyName(128, "lk.png");
            this.iglFlags.Images.SetKeyName(129, "lr.png");
            this.iglFlags.Images.SetKeyName(130, "ls.png");
            this.iglFlags.Images.SetKeyName(131, "lt.png");
            this.iglFlags.Images.SetKeyName(132, "lu.png");
            this.iglFlags.Images.SetKeyName(133, "lv.png");
            this.iglFlags.Images.SetKeyName(134, "ly.png");
            this.iglFlags.Images.SetKeyName(135, "ma.png");
            this.iglFlags.Images.SetKeyName(136, "mc.png");
            this.iglFlags.Images.SetKeyName(137, "md.png");
            this.iglFlags.Images.SetKeyName(138, "me.png");
            this.iglFlags.Images.SetKeyName(139, "mg.png");
            this.iglFlags.Images.SetKeyName(140, "mh.png");
            this.iglFlags.Images.SetKeyName(141, "mk.png");
            this.iglFlags.Images.SetKeyName(142, "ml.png");
            this.iglFlags.Images.SetKeyName(143, "mm.png");
            this.iglFlags.Images.SetKeyName(144, "mn.png");
            this.iglFlags.Images.SetKeyName(145, "mo.png");
            this.iglFlags.Images.SetKeyName(146, "mp.png");
            this.iglFlags.Images.SetKeyName(147, "mq.png");
            this.iglFlags.Images.SetKeyName(148, "mr.png");
            this.iglFlags.Images.SetKeyName(149, "ms.png");
            this.iglFlags.Images.SetKeyName(150, "mt.png");
            this.iglFlags.Images.SetKeyName(151, "mu.png");
            this.iglFlags.Images.SetKeyName(152, "mv.png");
            this.iglFlags.Images.SetKeyName(153, "mw.png");
            this.iglFlags.Images.SetKeyName(154, "mx.png");
            this.iglFlags.Images.SetKeyName(155, "my.png");
            this.iglFlags.Images.SetKeyName(156, "mz.png");
            this.iglFlags.Images.SetKeyName(157, "na.png");
            this.iglFlags.Images.SetKeyName(158, "nc.png");
            this.iglFlags.Images.SetKeyName(159, "ne.png");
            this.iglFlags.Images.SetKeyName(160, "nf.png");
            this.iglFlags.Images.SetKeyName(161, "ng.png");
            this.iglFlags.Images.SetKeyName(162, "ni.png");
            this.iglFlags.Images.SetKeyName(163, "nl.png");
            this.iglFlags.Images.SetKeyName(164, "no.png");
            this.iglFlags.Images.SetKeyName(165, "np.png");
            this.iglFlags.Images.SetKeyName(166, "nr.png");
            this.iglFlags.Images.SetKeyName(167, "nu.png");
            this.iglFlags.Images.SetKeyName(168, "nz.png");
            this.iglFlags.Images.SetKeyName(169, "om.png");
            this.iglFlags.Images.SetKeyName(170, "pa.png");
            this.iglFlags.Images.SetKeyName(171, "pe.png");
            this.iglFlags.Images.SetKeyName(172, "pf.png");
            this.iglFlags.Images.SetKeyName(173, "pg.png");
            this.iglFlags.Images.SetKeyName(174, "ph.png");
            this.iglFlags.Images.SetKeyName(175, "pk.png");
            this.iglFlags.Images.SetKeyName(176, "pl.png");
            this.iglFlags.Images.SetKeyName(177, "pm.png");
            this.iglFlags.Images.SetKeyName(178, "pn.png");
            this.iglFlags.Images.SetKeyName(179, "pr.png");
            this.iglFlags.Images.SetKeyName(180, "ps.png");
            this.iglFlags.Images.SetKeyName(181, "pt.png");
            this.iglFlags.Images.SetKeyName(182, "pw.png");
            this.iglFlags.Images.SetKeyName(183, "py.png");
            this.iglFlags.Images.SetKeyName(184, "qa.png");
            this.iglFlags.Images.SetKeyName(185, "re.png");
            this.iglFlags.Images.SetKeyName(186, "ro.png");
            this.iglFlags.Images.SetKeyName(187, "rs.png");
            this.iglFlags.Images.SetKeyName(188, "ru.png");
            this.iglFlags.Images.SetKeyName(189, "rw.png");
            this.iglFlags.Images.SetKeyName(190, "sa.png");
            this.iglFlags.Images.SetKeyName(191, "sb.png");
            this.iglFlags.Images.SetKeyName(192, "sc.png");
            this.iglFlags.Images.SetKeyName(193, "scotland.png");
            this.iglFlags.Images.SetKeyName(194, "sd.png");
            this.iglFlags.Images.SetKeyName(195, "se.png");
            this.iglFlags.Images.SetKeyName(196, "sg.png");
            this.iglFlags.Images.SetKeyName(197, "sh.png");
            this.iglFlags.Images.SetKeyName(198, "si.png");
            this.iglFlags.Images.SetKeyName(199, "sj.png");
            this.iglFlags.Images.SetKeyName(200, "sk.png");
            this.iglFlags.Images.SetKeyName(201, "sl.png");
            this.iglFlags.Images.SetKeyName(202, "sm.png");
            this.iglFlags.Images.SetKeyName(203, "sn.png");
            this.iglFlags.Images.SetKeyName(204, "so.png");
            this.iglFlags.Images.SetKeyName(205, "sr.png");
            this.iglFlags.Images.SetKeyName(206, "st.png");
            this.iglFlags.Images.SetKeyName(207, "sv.png");
            this.iglFlags.Images.SetKeyName(208, "sy.png");
            this.iglFlags.Images.SetKeyName(209, "sz.png");
            this.iglFlags.Images.SetKeyName(210, "tc.png");
            this.iglFlags.Images.SetKeyName(211, "td.png");
            this.iglFlags.Images.SetKeyName(212, "tf.png");
            this.iglFlags.Images.SetKeyName(213, "tg.png");
            this.iglFlags.Images.SetKeyName(214, "th.png");
            this.iglFlags.Images.SetKeyName(215, "tj.png");
            this.iglFlags.Images.SetKeyName(216, "tk.png");
            this.iglFlags.Images.SetKeyName(217, "tl.png");
            this.iglFlags.Images.SetKeyName(218, "tm.png");
            this.iglFlags.Images.SetKeyName(219, "tn.png");
            this.iglFlags.Images.SetKeyName(220, "to.png");
            this.iglFlags.Images.SetKeyName(221, "tr.png");
            this.iglFlags.Images.SetKeyName(222, "tt.png");
            this.iglFlags.Images.SetKeyName(223, "tv.png");
            this.iglFlags.Images.SetKeyName(224, "tw.png");
            this.iglFlags.Images.SetKeyName(225, "tz.png");
            this.iglFlags.Images.SetKeyName(226, "ua.png");
            this.iglFlags.Images.SetKeyName(227, "ug.png");
            this.iglFlags.Images.SetKeyName(228, "um.png");
            this.iglFlags.Images.SetKeyName(229, "unknown.png");
            this.iglFlags.Images.SetKeyName(230, "us.png");
            this.iglFlags.Images.SetKeyName(231, "uy.png");
            this.iglFlags.Images.SetKeyName(232, "uz.png");
            this.iglFlags.Images.SetKeyName(233, "va.png");
            this.iglFlags.Images.SetKeyName(234, "vc.png");
            this.iglFlags.Images.SetKeyName(235, "ve.png");
            this.iglFlags.Images.SetKeyName(236, "vg.png");
            this.iglFlags.Images.SetKeyName(237, "vi.png");
            this.iglFlags.Images.SetKeyName(238, "vn.png");
            this.iglFlags.Images.SetKeyName(239, "vu.png");
            this.iglFlags.Images.SetKeyName(240, "wales.png");
            this.iglFlags.Images.SetKeyName(241, "wf.png");
            this.iglFlags.Images.SetKeyName(242, "ws.png");
            this.iglFlags.Images.SetKeyName(243, "ye.png");
            this.iglFlags.Images.SetKeyName(244, "yt.png");
            this.iglFlags.Images.SetKeyName(245, "za.png");
            this.iglFlags.Images.SetKeyName(246, "zm.png");
            this.iglFlags.Images.SetKeyName(247, "zw.png");
            this.iglFlags.Images.SetKeyName(248, "flag_death.png");
            // 
            // ntfIcon
            // 
            this.ntfIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("ntfIcon.Icon")));
            this.ntfIcon.Text = "PRoCon Frostbite";
            this.ntfIcon.Visible = true;
            this.ntfIcon.DoubleClick += new System.EventHandler(this.ntfIcon_DoubleClick);
            // 
            // iglIcons
            // 
            this.iglIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iglIcons.ImageStream")));
            this.iglIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.iglIcons.Images.SetKeyName(0, "plugin.png");
            this.iglIcons.Images.SetKeyName(1, "application_xp_terminal.png");
            this.iglIcons.Images.SetKeyName(2, "comments.png");
            this.iglIcons.Images.SetKeyName(3, "table.png");
            this.iglIcons.Images.SetKeyName(4, "vcard.png");
            this.iglIcons.Images.SetKeyName(5, "mouse.png");
            this.iglIcons.Images.SetKeyName(6, "server_edit.png");
            this.iglIcons.Images.SetKeyName(7, "mouse_ban.png");
            this.iglIcons.Images.SetKeyName(8, "world.png");
            this.iglIcons.Images.SetKeyName(9, "user.png");
            this.iglIcons.Images.SetKeyName(10, "money_dollar.png");
            this.iglIcons.Images.SetKeyName(11, "information.png");
            this.iglIcons.Images.SetKeyName(12, "plugin_disabled.png");
            this.iglIcons.Images.SetKeyName(13, "plugin_edit.png");
            this.iglIcons.Images.SetKeyName(14, "status_offline.png");
            this.iglIcons.Images.SetKeyName(15, "sitemap_color.png");
            this.iglIcons.Images.SetKeyName(16, "bullet_arrow_up.png");
            this.iglIcons.Images.SetKeyName(17, "bullet_arrow_down.png");
            this.iglIcons.Images.SetKeyName(18, "flag_blue.png");
            this.iglIcons.Images.SetKeyName(19, "bfbc2server.png");
            this.iglIcons.Images.SetKeyName(20, "connect.png");
            this.iglIcons.Images.SetKeyName(21, "layer.png");
            this.iglIcons.Images.SetKeyName(22, "cross.png");
            this.iglIcons.Images.SetKeyName(23, "new.png");
            this.iglIcons.Images.SetKeyName(24, "key_delete.png");
            this.iglIcons.Images.SetKeyName(25, "key.png");
            this.iglIcons.Images.SetKeyName(26, "punkbuster.png");
            this.iglIcons.Images.SetKeyName(27, "arrow_up.png");
            this.iglIcons.Images.SetKeyName(28, "arrow_down.png");
            this.iglIcons.Images.SetKeyName(29, "tick.png");
            this.iglIcons.Images.SetKeyName(30, "page_copy.png");
            this.iglIcons.Images.SetKeyName(31, "arrow_right.png");
            this.iglIcons.Images.SetKeyName(32, "arrow_left.png");
            this.iglIcons.Images.SetKeyName(33, "add.png");
            this.iglIcons.Images.SetKeyName(34, "arrow_refresh.png");
            this.iglIcons.Images.SetKeyName(35, "application.png");
            this.iglIcons.Images.SetKeyName(36, "application_tile_horizontal.png");
            this.iglIcons.Images.SetKeyName(37, "application_tile.png");
            this.iglIcons.Images.SetKeyName(38, "star.png");
            this.iglIcons.Images.SetKeyName(39, "cursor.png");
            this.iglIcons.Images.SetKeyName(40, "shape_rotate_anticlockwise.png");
            this.iglIcons.Images.SetKeyName(41, "shape_rotate_clockwise.png");
            this.iglIcons.Images.SetKeyName(42, "shape_square_edit.png");
            this.iglIcons.Images.SetKeyName(43, "layer-shape-line.png");
            this.iglIcons.Images.SetKeyName(44, "status_online.png");
            this.iglIcons.Images.SetKeyName(45, "map-pin.png");
            this.iglIcons.Images.SetKeyName(46, "layers-ungroup.png");
            this.iglIcons.Images.SetKeyName(47, "block.png");
            this.iglIcons.Images.SetKeyName(48, "arrow-transition.png");
            this.iglIcons.Images.SetKeyName(49, "arrow-transition-180.png");
            this.iglIcons.Images.SetKeyName(50, "plug-connect.png");
            this.iglIcons.Images.SetKeyName(51, "plug-disconnect.png");
            this.iglIcons.Images.SetKeyName(52, "exclamation-button.png");
            this.iglIcons.Images.SetKeyName(53, "tick-button.png");
            this.iglIcons.Images.SetKeyName(54, "smiley48.ico");
            this.iglIcons.Images.SetKeyName(55, "cross-button.png");
            this.iglIcons.Images.SetKeyName(56, "arrow-curve-000-left.png");
            this.iglIcons.Images.SetKeyName(57, "arrow-curve-180-left.png");
            this.iglIcons.Images.SetKeyName(58, "arrow-retweet.png");
            this.iglIcons.Images.SetKeyName(59, "arrow-step-over.png");
            this.iglIcons.Images.SetKeyName(60, "home.png");
            this.iglIcons.Images.SetKeyName(61, "wrench.png");
            this.iglIcons.Images.SetKeyName(62, "check.png");
            // 
            // tltpUpdateComplete
            // 
            this.tltpUpdateComplete.IsBalloon = true;
            // 
            // pnlWindows
            // 
            this.pnlWindows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWindows.Location = new System.Drawing.Point(0, 30);
            this.pnlWindows.Name = "pnlWindows";
            this.pnlWindows.Size = new System.Drawing.Size(1008, 710);
            this.pnlWindows.TabIndex = 8;
            // 
            // iglGameIcons
            // 
            this.iglGameIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("iglGameIcons.ImageStream")));
            this.iglGameIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.iglGameIcons.Images.SetKeyName(0, "bfbc2.png");
            this.iglGameIcons.Images.SetKeyName(1, "moh.png");
            this.iglGameIcons.Images.SetKeyName(2, "bfbc2.bc2.png");
            this.iglGameIcons.Images.SetKeyName(3, "bfbc2.vietnam.png");
            // 
            // tlsConnections
            // 
            this.tlsConnections.CanOverflow = false;
            this.tlsConnections.GripMargin = new System.Windows.Forms.Padding(0);
            this.tlsConnections.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tlsConnections.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnStartPage,
            this.btnShiftServerPrevious,
            this.btnShiftServerNext,
            this.cboServerList,
            this.toolsStripDropDownButton});
            this.tlsConnections.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.tlsConnections.Location = new System.Drawing.Point(0, 0);
            this.tlsConnections.Name = "tlsConnections";
            this.tlsConnections.Padding = new System.Windows.Forms.Padding(1, 1, 1, 0);
            this.tlsConnections.Size = new System.Drawing.Size(1008, 30);
            this.tlsConnections.Stretch = true;
            this.tlsConnections.TabIndex = 4;
            this.tlsConnections.Text = "toolStrip1";
            this.tlsConnections.SizeChanged += new System.EventHandler(this.tlsConnections_SizeChanged);
            // 
            // btnStartPage
            // 
            this.btnStartPage.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnStartPage.Image = ((System.Drawing.Image)(resources.GetObject("btnStartPage.Image")));
            this.btnStartPage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStartPage.Name = "btnStartPage";
            this.btnStartPage.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.btnStartPage.Padding = new System.Windows.Forms.Padding(3);
            this.btnStartPage.Size = new System.Drawing.Size(26, 26);
            this.btnStartPage.Text = "Start Page";
            this.btnStartPage.Click += new System.EventHandler(this.btnStartPage_Click);
            // 
            // btnShiftServerPrevious
            // 
            this.btnShiftServerPrevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShiftServerPrevious.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftServerPrevious.Image")));
            this.btnShiftServerPrevious.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShiftServerPrevious.Name = "btnShiftServerPrevious";
            this.btnShiftServerPrevious.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.btnShiftServerPrevious.Padding = new System.Windows.Forms.Padding(3);
            this.btnShiftServerPrevious.Size = new System.Drawing.Size(26, 26);
            this.btnShiftServerPrevious.Text = "Previous connection";
            this.btnShiftServerPrevious.Click += new System.EventHandler(this.btnShiftServerPrevious_Click);
            // 
            // btnShiftServerNext
            // 
            this.btnShiftServerNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnShiftServerNext.Image = ((System.Drawing.Image)(resources.GetObject("btnShiftServerNext.Image")));
            this.btnShiftServerNext.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShiftServerNext.Name = "btnShiftServerNext";
            this.btnShiftServerNext.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.btnShiftServerNext.Padding = new System.Windows.Forms.Padding(3);
            this.btnShiftServerNext.Size = new System.Drawing.Size(26, 26);
            this.btnShiftServerNext.Text = "Next connection";
            this.btnShiftServerNext.Click += new System.EventHandler(this.btnShiftServerNext_Click);
            // 
            // cboServerList
            // 
            this.cboServerList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboServerList.DropDownWidth = 500;
            this.cboServerList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboServerList.Name = "cboServerList";
            this.cboServerList.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.cboServerList.Padding = new System.Windows.Forms.Padding(2);
            this.cboServerList.Size = new System.Drawing.Size(75, 27);
            this.cboServerList.SelectedIndexChanged += new System.EventHandler(this.cboServerList_SelectedIndexChanged);
            // 
            // toolsStripDropDownButton
            // 
            this.toolsStripDropDownButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolsStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConnectDisconnect,
            this.chkAutomaticallyConnect,
            this.toolStripSeparator3,
            this.optionsToolStripMenuItem,
            this.manageAccountsToolStripMenuItem,
            this.toolStripSeparator1,
            this.checkForUpdatesToolStripMenuItem,
            this.changelogToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.toolsStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("toolsStripDropDownButton.Image")));
            this.toolsStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolsStripDropDownButton.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.toolsStripDropDownButton.Name = "toolsStripDropDownButton";
            this.toolsStripDropDownButton.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolsStripDropDownButton.Padding = new System.Windows.Forms.Padding(3);
            this.toolsStripDropDownButton.Size = new System.Drawing.Size(71, 26);
            this.toolsStripDropDownButton.Text = "Tools";
            // 
            // btnConnectDisconnect
            // 
            this.btnConnectDisconnect.Name = "btnConnectDisconnect";
            this.btnConnectDisconnect.Size = new System.Drawing.Size(196, 22);
            this.btnConnectDisconnect.Text = "Connect";
            this.btnConnectDisconnect.Click += new System.EventHandler(this.btnConnectDisconnect_Click);
            this.btnConnectDisconnect.MouseEnter += new System.EventHandler(this.btnConnectDisconnect_MouseEnter);
            this.btnConnectDisconnect.MouseLeave += new System.EventHandler(this.btnConnectDisconnect_MouseLeave);
            // 
            // chkAutomaticallyConnect
            // 
            this.chkAutomaticallyConnect.CheckOnClick = true;
            this.chkAutomaticallyConnect.Name = "chkAutomaticallyConnect";
            this.chkAutomaticallyConnect.Size = new System.Drawing.Size(196, 22);
            this.chkAutomaticallyConnect.Text = "Automatically Connect";
            this.chkAutomaticallyConnect.Click += new System.EventHandler(this.chkAutomaticallyConnect_CheckedChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(193, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.optionsToolStripMenuItem.Text = "Options..";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem1_Click);
            // 
            // manageAccountsToolStripMenuItem
            // 
            this.manageAccountsToolStripMenuItem.Name = "manageAccountsToolStripMenuItem";
            this.manageAccountsToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.manageAccountsToolStripMenuItem.Text = "Manage Accounts";
            this.manageAccountsToolStripMenuItem.Click += new System.EventHandler(this.userManagerToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(193, 6);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // changelogToolStripMenuItem
            // 
            this.changelogToolStripMenuItem.Name = "changelogToolStripMenuItem";
            this.changelogToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.changelogToolStripMenuItem.Text = "Changelog";
            this.changelogToolStripMenuItem.Click += new System.EventHandler(this.changelogToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutToolStripMenuItem.Image")));
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.aboutToolStripMenuItem.Text = "About..";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 762);
            this.Controls.Add(this.pnlWindows);
            this.Controls.Add(this.picLayerOffline);
            this.Controls.Add(this.tlsConnections);
            this.Controls.Add(this.picLayerOnline);
            this.Controls.Add(this.picPortCheckerClosed);
            this.Controls.Add(this.picPortCheckerOpen);
            this.Controls.Add(this.stsMain);
            this.Controls.Add(this.picPortCheckerUnknown);
            this.Controls.Add(this.picAjaxStyleSuccess);
            this.Controls.Add(this.picAjaxStyleLoading);
            this.Controls.Add(this.picAjaxStyleFail);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1024, 800);
            this.Name = "frmMain";
            this.Text = "Procon Frostbite";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResizeEnd += new System.EventHandler(this.frmMain_ResizeEnd);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.stsMain.ResumeLayout(false);
            this.stsMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLayerOffline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLayerOnline)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPortCheckerClosed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPortCheckerOpen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPortCheckerUnknown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAjaxStyleSuccess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAjaxStyleFail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAjaxStyleLoading)).EndInit();
            this.tlsConnections.ResumeLayout(false);
            this.tlsConnections.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip stsMain;
        public System.Windows.Forms.ImageList iglFlags;
        private System.Windows.Forms.NotifyIcon ntfIcon;
        public System.Windows.Forms.PictureBox picAjaxStyleLoading;
        public System.Windows.Forms.PictureBox picAjaxStyleFail;
        public System.Windows.Forms.PictureBox picAjaxStyleSuccess;
        public System.Windows.Forms.PictureBox picLayerOffline;
        public System.Windows.Forms.PictureBox picLayerOnline;
        public System.Windows.Forms.PictureBox picPortCheckerClosed;
        public System.Windows.Forms.PictureBox picPortCheckerOpen;
        public System.Windows.Forms.PictureBox picPortCheckerUnknown;
        public System.Windows.Forms.ImageList iglIcons;
        private System.Windows.Forms.ToolStripStatusLabel toolStripDownloading;
        private System.Windows.Forms.ToolStripProgressBar toolStripDownloadProgress;
        private System.Windows.Forms.ToolTip tltpUpdateComplete;
        private System.Windows.Forms.ToolStripStatusLabel toolStripRssFeed;
        private PRoCon.Controls.ControlsEx.ToolStripNF tlsConnections;
        private System.Windows.Forms.ToolStripComboBox cboServerList;
        private System.Windows.Forms.ToolStripButton btnShiftServerPrevious;
        private System.Windows.Forms.ToolStripButton btnShiftServerNext;
        private System.Windows.Forms.Panel pnlWindows;
        private System.Windows.Forms.ImageList iglGameIcons;
        private System.Windows.Forms.ToolStripButton btnStartPage;
        private System.Windows.Forms.ToolStripDropDownButton toolsStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageAccountsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem changelogToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem btnConnectDisconnect;
        private System.Windows.Forms.ToolStripMenuItem chkAutomaticallyConnect;
    }
}

