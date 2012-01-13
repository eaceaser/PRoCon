﻿/*  Copyright 2010 Geoffrey 'Phogue' Green

    http://www.phogue.net
 
    This file is part of PRoCon Frostbite.

    PRoCon Frostbite is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    PRoCon Frostbite is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with PRoCon Frostbite.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace PRoCon {
    using Core;
    using Core.Maps;
    using Core.Remote;
    using PRoCon.Forms;
    using PRoCon.Controls.ControlsEx;

    public partial class uscListControlPanel : UserControl {

        private frmMain m_frmMain;
        private uscServerConnection m_uscConnectionPanel;

        private PRoConClient m_prcClient;

        private ListViewColumnSorter m_lvwReservedSlotsColumnSorter;
        private ListViewColumnSorter m_lvwBanlistColumnSorter;
        private Font m_fntComboBoxSelectedFont;

        private Dictionary<string, AsyncStyleSetting> m_dicAsyncSettingControls;
        //private Dictionary<string, string> m_dicFriendlyPlaylistNames; // strPlaylist, strGamemode
        //private Dictionary<string, Dictionary<string, string>> m_dicMaplistsPerPlaylist; // strPlaylist, Dictionary<strLevel, strPublicLevelName>>

        //public delegate void SendCommandDelegate(List<string> lstCommand);
        //public event SendCommandDelegate SendCommand;

       // private int m_iReselectShufflingMapIndex = 0;

        private bool m_blSettingAppendingReservedPlayer;
        private bool m_blSettingRemovingReservedPlayer;

        //private bool m_blSettingAppendingSingleMap;
        //private bool m_blSettingNewPlaylist;

        private string[] ma_strTimeDescriptionsShort;
        private string[] ma_strTimeDescriptionsLong;

        private CPrivileges m_spPrivileges;

        private Regex m_regIP = null;
        private Regex m_regPbGUID = null;
        private Regex m_regBc2GUID = null;

        /*
        public List<string> SetListsSettings {
            set {
                bool blChecked = true;

                if (value.Count >= 1 && bool.TryParse(value[0], out blChecked) == true) {
                    this.spltBanlistManualBans.Panel2Collapsed = !blChecked;
                    this.picCloseOpenManualBans_Click(null, null);
                }
            }
        }

        public string ListsSettings {
            get {
                return String.Format("{0}", this.spltBanlistManualBans.Panel2Collapsed);
            }
        }
        */
        public uscListControlPanel() {
            InitializeComponent();

            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true); 

            this.m_regIP = new Regex(@"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$", RegexOptions.Compiled);
            this.m_regPbGUID = new Regex("^[A-Fa-f0-9]{32}$", RegexOptions.Compiled);
            this.m_regBc2GUID = new Regex("^EA_[A-Fa-f0-9]{32}$", RegexOptions.Compiled);
            
            this.m_lvwReservedSlotsColumnSorter = new ListViewColumnSorter();
            this.lsvReservedList.ListViewItemSorter = this.m_lvwReservedSlotsColumnSorter;

            this.m_lvwBanlistColumnSorter = new ListViewColumnSorter();
            this.lsvBanlist.ListViewItemSorter = this.m_lvwBanlistColumnSorter;

            //this.pnlCurrentMaplist.Dock = DockStyle.Fill;
            //this.pnlMaplistConfirmation.Dock = DockStyle.Fill;
            this.pnlReservedPanel.Dock = DockStyle.Fill;

            //this.m_dicFriendlyPlaylistNames = new Dictionary<string, string>();
            //this.m_dicMaplistsPerPlaylist = new Dictionary<string, Dictionary<string, string>>();

            this.m_fntComboBoxSelectedFont = new Font("Segoe UI", 10, FontStyle.Bold);

            //this.m_iReselectShufflingMapIndex = 0;
            //this.m_blSettingNewPlaylist = false;

            this.m_blSettingAppendingReservedPlayer = false;
            this.m_blSettingRemovingReservedPlayer = false;

            this.m_dicAsyncSettingControls = new Dictionary<string, AsyncStyleSetting>();
            
            // Maplist updates

            //this.m_dicAsyncSettingControls.Add("local.playlist.change", new AsyncStyleSetting(this.picMaplistChangePlaylist, this.lsvMaplist, new Control[] { this.pnlMaplistPositionControls, this.pnlMaplistAddMap, this.lsvMaplist, this.lnkMaplistChangePlaylist }, true));
            //this.m_dicAsyncSettingControls.Add("local.maplist.change", new AsyncStyleSetting(this.picMaplistAlterMaplist, this.lsvMaplist, new Control[] { this.pnlMaplistPositionControls, this.pnlMaplistAddMap, this.lsvMaplist, this.lnkMaplistChangePlaylist }, true));

            //this.m_blSettingAppendingSingleMap = false;
            //this.m_dicAsyncSettingControls.Add("local.maplist.append", new AsyncStyleSetting(this.picMaplistAppendMap, this.lsvMaplist, new Control[] { this.lblMaplistPool, this.lnkMaplistAddMapToList, this.lnkMaplistSetNextMap }, true));
            //this.m_dicAsyncSettingControls.Add("local.maplist.setnextlevel", new AsyncStyleSetting(this.picMaplistAppendMap, this.lsvMaplist, new Control[] { this.lblMaplistPool, this.lnkMaplistAddMapToList, this.lnkMaplistSetNextMap }, true));

            //this.m_dicAsyncSettingControls.Add("local.maplist.runnextlevel", new AsyncStyleSetting(this.picMaplistRunNextMap, this.lsvMaplist, new Control[] { this.lnkMaplistRunNextMap }, true));
            //this.m_dicAsyncSettingControls.Add("local.maplist.restartlevel", new AsyncStyleSetting(this.picMaplistRestartMap, this.lsvMaplist, new Control[] { this.lnkMaplistRestartMap }, true));

            // Reservedlist updates
            this.m_dicAsyncSettingControls.Add("local.reservedlist.list", new AsyncStyleSetting(this.picReservedList, this.lsvReservedList, new Control[] { this.btnReservedSlotsListRefresh }, true)); 
            this.m_dicAsyncSettingControls.Add("local.reservedlist.append", new AsyncStyleSetting(this.picReservedAddSoldierName, this.lsvReservedList, new Control[] { this.lblReservedAddSoldierName, this.txtReservedAddSoldierName, this.lnkReservedAddSoldierName }, true));
            this.m_dicAsyncSettingControls.Add("local.reservedlist.remove", new AsyncStyleSetting(this.picReservedList, this.lsvReservedList, new Control[] { this.btnReservedRemoveSoldier }, true));

            this.m_dicAsyncSettingControls.Add("local.banlist.clearlist", new AsyncStyleSetting(this.picClearLists, this.btnBanlistClearBanlist, new Control[] { this.btnBanlistClearBanlist }, true));
            this.m_dicAsyncSettingControls.Add("local.banlist.unban", new AsyncStyleSetting(this.picUnbanPlayer, this.btnBanlistUnban, new Control[] { this.btnBanlistUnban }, true));

            this.m_dicAsyncSettingControls.Add("local.banlist.banning", new AsyncStyleSetting(this.picBanlistManualBanOkay, this.btnBanlistAddBan, new Control[] { this.btnBanlistAddBan }, false));

            this.ma_strTimeDescriptionsShort = new string[] { "y ", "y ", "M ", "M ", "w ", "w ", "d ", "d ", "h ", "h ", "m ", "m ", "s ", "s " };
            this.ma_strTimeDescriptionsLong = new string[] { " year ", " years ", " month ", " months ", " week ", " weeks ", " day ", " days ", " hour ", " hours ", " minute ", " minutes ", " second", " seconds" };
            this.cboBanlistTimeMultiplier.SelectedIndex = 0;

            this.m_spPrivileges = new CPrivileges(CPrivileges.FullPrivilegesFlags);
            //this.m_spPrivileges.PrivilegesFlags = CPrivileges.FullPrivilegesFlags;
        }

        public void m_prcClient_ProconPrivileges(PRoConClient sender, CPrivileges spPrivs) {

            this.m_spPrivileges = spPrivs;

            //this.m_dicAsyncSettingControls["local.playlist.change"].m_blReEnableControls = this.m_spPrivileges.CanEditMapList;
            //this.lnkMaplistChangePlaylist.Enabled = this.m_spPrivileges.CanEditMapList;

            //this.m_dicAsyncSettingControls["local.maplist.change"].m_blReEnableControls = this.m_spPrivileges.CanEditMapList;
            //this.m_dicAsyncSettingControls["local.maplist.append"].m_blReEnableControls = this.m_spPrivileges.CanEditMapList;
            //this.lnkMaplistAddMapToList.Enabled = this.m_spPrivileges.CanEditMapList;
            //if (this.lsvMaplist.SelectedItems.Count > 0) { 
            //    this.pnlMaplistPositionControls.Enabled = this.m_spPrivileges.CanEditMapList;
            //} // ELSE It'll already be disabled 

            //this.m_dicAsyncSettingControls["local.maplist.setnextlevel"].m_blReEnableControls = this.m_spPrivileges.CanUseMapFunctions;
            //this.m_dicAsyncSettingControls["local.maplist.restartlevel"].m_blReEnableControls = this.m_spPrivileges.CanUseMapFunctions;
            //this.m_dicAsyncSettingControls["local.maplist.restartlevel"].m_blReEnableControls = this.m_spPrivileges.CanUseMapFunctions;
            //this.lnkMaplistSetNextMap.Enabled = this.m_spPrivileges.CanUseMapFunctions;
            //this.lnkMaplistRunNextMap.Enabled = this.m_spPrivileges.CanUseMapFunctions;
            //this.lnkMaplistRestartMap.Enabled = this.m_spPrivileges.CanUseMapFunctions;

            this.m_dicAsyncSettingControls["local.reservedlist.append"].m_blReEnableControls = this.m_spPrivileges.CanEditReservedSlotsList;
            this.m_dicAsyncSettingControls["local.reservedlist.remove"].m_blReEnableControls = this.m_spPrivileges.CanEditReservedSlotsList;
            if (this.lsvReservedList.SelectedItems.Count > 0) {
                this.btnReservedRemoveSoldier.Enabled = this.m_spPrivileges.CanEditReservedSlotsList;
            } // ELSE It'll already be disabled 
            this.lnkReservedAddSoldierName.Enabled = this.m_spPrivileges.CanEditReservedSlotsList;

            this.m_dicAsyncSettingControls["local.banlist.clearlist"].m_blReEnableControls = this.m_spPrivileges.CanEditBanList;
            this.m_dicAsyncSettingControls["local.banlist.unban"].m_blReEnableControls = this.m_spPrivileges.CanEditBanList;
            this.btnBanlistClearBanlist.Enabled = this.m_spPrivileges.CanEditBanList;

            if (this.lsvBanlist.SelectedItems.Count > 0) {
                this.btnBanlistUnban.Enabled = this.m_spPrivileges.CanEditBanList;
            } // ELSE It'll already be disabled 

            // Manual banning..
            this.rdoBanlistPbGUID.Enabled = this.rdoBanlistPermanent.Enabled = this.m_spPrivileges.CanPermanentlyBanPlayers;

            if (this.rdoBanlistPbGUID.Checked == true && this.rdoBanlistPbGUID.Enabled == false) {
                this.rdoBanlistName.Checked = true;
            }

            if (this.rdoBanlistPermanent.Checked == true && this.rdoBanlistPermanent.Enabled == false) {
                this.rdoBanlistTemporary.Checked = true;
            }

            this.spltBanlistManualBans.Panel2.Enabled = this.m_spPrivileges.CanTemporaryBanPlayers;
        }

        public void Initialize(frmMain frmMainWindow, uscServerConnection uscConnectionPanel) {
            this.m_frmMain = frmMainWindow;
            this.m_uscConnectionPanel = uscConnectionPanel;

            this.uscMaplist1.Initialize(frmMainWindow, uscConnectionPanel);

            this.tbcLists.ImageList = this.m_frmMain.iglIcons;

            this.tabBanlist.ImageKey = "mouse_ban.png";
            this.tabMaplist.ImageKey = "world.png";
            this.tabReservedSlots.ImageKey = "user.png";

            this.btnBanlistRefresh.ImageList = this.m_frmMain.iglIcons;
            this.btnBanlistRefresh.ImageKey = "arrow_refresh.png";

            //this.btnMaplistMoveMapUp.ImageList = this.m_frmMain.iglIcons;
            //this.btnMaplistMoveMapUp.ImageKey = "bullet_arrow_up.png";

            //this.btnMaplistRemoveMap.ImageList = this.m_frmMain.iglIcons;
            //this.btnMaplistRemoveMap.ImageKey = "cross.png";

            this.btnReservedRemoveSoldier.ImageList = this.m_frmMain.iglIcons;
            this.btnReservedRemoveSoldier.ImageKey = "cross.png";

            this.btnReservedSlotsListRefresh.ImageList = this.m_frmMain.iglIcons;
            this.btnReservedSlotsListRefresh.ImageKey = "arrow_refresh.png";

            //this.btnMaplistMoveMapDown.ImageList = this.m_frmMain.iglIcons;
            //this.btnMaplistMoveMapDown.ImageKey = "bullet_arrow_down.png";

            this.picCloseOpenManualBans.Image = this.m_frmMain.iglIcons.Images["arrow_down.png"];

            this.picBanlistIPError.Image = this.picBanlistGUIDError.Image = this.m_frmMain.iglIcons.Images["cross.png"];
            //this.picBanlistManualBanOkay.Image = this.m_frmMain.iglIcons.Images["tick.png"];

            this.copyToolStripMenuItem.Image = this.m_frmMain.iglIcons.Images["page_copy.png"];

            this.uscTextChatModerationListcs1.Initialize(frmMainWindow);
        }

        private void uscListControlPanel_Load(object sender, EventArgs e) {
            if (this.m_prcClient != null) {
                //this.RefreshLocalMaplist();

                this.SendCommand("mapList.list", "rounds");

                if (this.m_prcClient.GameType == "MOH") {
                    this.tabReservedSlots.Enabled = false;
                    this.lblMohNotice.Visible = true;
                }

            }
        }

        public void SetConnection(PRoConClient prcClient) {
            if ((this.m_prcClient = prcClient) != null) {
                if (this.m_prcClient.Game != null) {
                    this.m_prcClient_GameTypeDiscovered(prcClient);
                }
                else {
                    this.m_prcClient.GameTypeDiscovered += new PRoConClient.EmptyParamterHandler(m_prcClient_GameTypeDiscovered);
                }

                this.uscMaplist1.SetConnection(prcClient);
                this.uscTextChatModerationListcs1.SetConnection(prcClient);
            }
        }

        private void m_prcClient_GameTypeDiscovered(PRoConClient sender) {

            this.m_prcClient.Game.ReservedSlotsSave += new FrostbiteClient.EmptyParamterHandler(this.OnReservedSlotsSave);
            this.m_prcClient.Game.ReservedSlotsList += new FrostbiteClient.ReservedSlotsListHandler(this.OnReservedSlotsList);
            this.m_prcClient.Game.ReservedSlotsPlayerAdded += new FrostbiteClient.ReservedSlotsPlayerHandler(this.OnReservedSlotsPlayerAdded);
            this.m_prcClient.Game.ReservedSlotsPlayerRemoved += new FrostbiteClient.ReservedSlotsPlayerHandler(this.OnReservedSlotsPlayerRemoved);

            this.m_prcClient.Game.BanListClear += new FrostbiteClient.EmptyParamterHandler(this.OnClearBanList);
            this.m_prcClient.FullBanListList += new PRoConClient.FullBanListListHandler(this.OnBanList);
            this.m_prcClient.PunkbusterPlayerUnbanned += new PRoConClient.PunkbusterBanHandler(m_prcClient_PunkbusterPlayerUnbanned);
            this.m_prcClient.Game.BanListRemove += new FrostbiteClient.BanListRemoveHandler(this.OnUnban);
            this.m_prcClient.PunkbusterPlayerBanned += new PRoConClient.PunkbusterBanHandler(this.OnPbGuidBan);


            this.m_prcClient.ProconPrivileges += new PRoConClient.ProconPrivilegesHandler(m_prcClient_ProconPrivileges);

            //this.m_prcClient.Reasons.ItemRemoved += new NotificationList<string>.ItemModifiedHandler(Reasons_ItemRemoved);
            this.m_prcClient.Reasons.ItemAdded += new NotificationList<string>.ItemModifiedHandler(Reasons_ItemAdded);

            this.m_prcClient.ListSettings.ManualBansVisibleChange += new PRoCon.Core.Lists.ListsSettings.ManualBansVisibleChangeHandler(ListSettings_ManualBansVisibleChange);

            this.m_prcClient.ListSettings.ManualBansVisible = this.m_prcClient.ListSettings.ManualBansVisible;

            this.cboBanlistReason.Items.Clear();
            foreach (string strReason in this.m_prcClient.Reasons) {
                this.Reasons_ItemAdded(0, strReason);
            }

            if (this.m_prcClient.FullVanillaBanList != null) {
                this.OnBanList(this.m_prcClient, this.m_prcClient.FullVanillaBanList);
            }

            if (this.m_prcClient.ReservedSlotList != null) {
                this.OnReservedSlotsList(this.m_prcClient.Game, new List<string>(this.m_prcClient.ReservedSlotList));
            }
        }

        public void SetLocalization(CLocalization clocLanguage) {
            //this.m_prcClient.Language = clocLanguage;

            // private string[] m_astrTimeDescriptionsShort = new string[] { "y ", "y ", "M ", "M ", "w ", "w ", "d ", "d ", "h ", "h ", "m ", "m ", "s ", "s " };
            this.ma_strTimeDescriptionsShort[13] = clocLanguage.GetLocalized("global.Seconds.Short", null);
            this.ma_strTimeDescriptionsShort[12] = clocLanguage.GetLocalized("global.Seconds.Short", null);
            this.ma_strTimeDescriptionsShort[11] = clocLanguage.GetLocalized("global.Minutes.Short", null) + " ";
            this.ma_strTimeDescriptionsShort[10] = clocLanguage.GetLocalized("global.Minutes.Short", null) + " ";
            this.ma_strTimeDescriptionsShort[9] = clocLanguage.GetLocalized("global.Hours.Short", null) + " ";
            this.ma_strTimeDescriptionsShort[8] = clocLanguage.GetLocalized("global.Hours.Short", null) + " ";
            this.ma_strTimeDescriptionsShort[7] = clocLanguage.GetLocalized("global.Days.Short", null) + " ";
            this.ma_strTimeDescriptionsShort[6] = clocLanguage.GetLocalized("global.Days.Short", null) + " ";
            this.ma_strTimeDescriptionsShort[5] = clocLanguage.GetLocalized("global.Weeks.Short", null) + " ";
            this.ma_strTimeDescriptionsShort[4] = clocLanguage.GetLocalized("global.Weeks.Short", null) + " ";
            this.ma_strTimeDescriptionsShort[3] = clocLanguage.GetLocalized("global.Months.Short", null) + " ";
            this.ma_strTimeDescriptionsShort[2] = clocLanguage.GetLocalized("global.Months.Short", null) + " ";
            this.ma_strTimeDescriptionsShort[1] = clocLanguage.GetLocalized("global.Years.Short", null) + " ";
            this.ma_strTimeDescriptionsShort[0] = clocLanguage.GetLocalized("global.Years.Short", null) + " ";

            this.ma_strTimeDescriptionsLong[13] = " " + clocLanguage.GetLocalized("global.Seconds.Plural", null).ToLower();
            this.ma_strTimeDescriptionsLong[12] = " " + clocLanguage.GetLocalized("global.Seconds.Singular", null).ToLower();
            this.ma_strTimeDescriptionsLong[11] = " " + clocLanguage.GetLocalized("global.Minutes.Plural", null).ToLower() + " ";
            this.ma_strTimeDescriptionsLong[10] = " " + clocLanguage.GetLocalized("global.Minutes.Singular", null).ToLower() + " ";
            this.ma_strTimeDescriptionsLong[9] = " " + clocLanguage.GetLocalized("global.Hours.Plural", null).ToLower() + " ";
            this.ma_strTimeDescriptionsLong[8] = " " + clocLanguage.GetLocalized("global.Hours.Singular", null).ToLower() + " ";
            this.ma_strTimeDescriptionsLong[7] = " " + clocLanguage.GetLocalized("global.Days.Plural", null).ToLower() + " ";
            this.ma_strTimeDescriptionsLong[6] = " " + clocLanguage.GetLocalized("global.Days.Singular", null).ToLower() + " ";
            this.ma_strTimeDescriptionsLong[5] = " " + clocLanguage.GetLocalized("global.Weeks.Plural", null).ToLower() + " ";
            this.ma_strTimeDescriptionsLong[4] = " " + clocLanguage.GetLocalized("global.Weeks.Singular", null).ToLower() + " ";
            this.ma_strTimeDescriptionsLong[3] = " " + clocLanguage.GetLocalized("global.Months.Plural", null).ToLower() + " ";
            this.ma_strTimeDescriptionsLong[2] = " " + clocLanguage.GetLocalized("global.Months.Singular", null).ToLower() + " ";
            this.ma_strTimeDescriptionsLong[1] = " " + clocLanguage.GetLocalized("global.Years.Plural", null).ToLower() + " ";
            this.ma_strTimeDescriptionsLong[0] = " " + clocLanguage.GetLocalized("global.Years.Singular", null).ToLower() + " ";

            this.cboBanlistTimeMultiplier.Items[0] = clocLanguage.GetLocalized("global.Minutes.Plural", null);
            this.cboBanlistTimeMultiplier.Items[1] = clocLanguage.GetLocalized("global.Hours.Plural", null);
            this.cboBanlistTimeMultiplier.Items[2] = clocLanguage.GetLocalized("global.Days.Plural", null);
            this.cboBanlistTimeMultiplier.Items[3] = clocLanguage.GetLocalized("global.Weeks.Plural", null);
            this.cboBanlistTimeMultiplier.Items[4] = clocLanguage.GetLocalized("global.Months.Plural", null);

            this.tabBanlist.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist", null);
            this.colName.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.lsvBanlist.colName", null);
            this.colIP.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.lsvBanlist.colIP", null);
            this.colGUID.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.lsvBanlist.colGUID", null);
            this.colType.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.lsvBanlist.colType", null);
            //this.colTime.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.lsvBanlist.colTime", null);
            //this.colBanLength.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.lsvBanlist.colBanLength", null);
            this.colTimeRemaining.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.lsvBanlist.colTimeRemaining", null);
            this.colReason.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.lsvBanlist.colReason", null);
            this.btnBanlistClearBanlist.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.btnBanlistClearBanlist", null);
            //this.btnBanlistClearBanlist.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.btnBanlistClearNameList", null);
            //this.btnBanlistClearIPList.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.btnBanlistClearIPList", null);
            this.btnBanlistUnban.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.btnBanlistUnban", null);

            this.tabMaplist.Text = clocLanguage.GetLocalized("uscListControlPanel.tabMaplist");

            this.tabReservedSlots.Text = clocLanguage.GetLocalized("uscListControlPanel.tabReservedSlots", null);
            this.lblReservedCurrent.Text = clocLanguage.GetLocalized("uscListControlPanel.tabReservedSlots.lblReservedCurrent", null);
            this.colSoldierNames.Text = clocLanguage.GetLocalized("uscListControlPanel.tabReservedSlots.lsvReservedList.colSoldierNames", null);
            this.lblReservedAddSoldierName.Text = clocLanguage.GetLocalized("uscListControlPanel.tabReservedSlots.lblReservedAddSoldierName", null);
            this.lnkReservedAddSoldierName.Text = clocLanguage.GetLocalized("uscListControlPanel.tabReservedSlots.lnkReservedAddSoldierName", null);
        
            this.lnkCloseOpenManualBans.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.lnkCloseOpenManualBans.Close", null);
            this.rdoBanlistName.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.rdoBanlistName", null);
            //this.rdoBanlistPbGUID.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.rdoBanlistGUID", null);
            this.lblBanlistReason.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.lblBanlistReason", null) + ":";
            this.rdoBanlistPermanent.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.rdoBanlistPermanent", null);
            this.rdoBanlistTemporary.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.rdoBanlistTemporary", null);
            this.lblBanlistTime.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.lblBanlistTime", null) + ":";
            this.btnBanlistAddBan.Text = clocLanguage.GetLocalized("uscListControlPanel.tabBanlist.btnBanlistAddBan", null);

            this.uscMaplist1.SetLocalization(clocLanguage);

            this.tabTextChatModeration.Text = clocLanguage.GetLocalized("uscTextChatModerationList.Title", null);
            this.uscTextChatModerationListcs1.SetLocalization(clocLanguage);
        }

        //public delegate void OnTabChangeDelegate(object sender, Stack<string> stkTabIndexes);
        public event uscServerConnection.OnTabChangeDelegate OnTabChange;

        private void tbcLists_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.OnTabChange != null) {
                Stack<string> stkTabIndexes = new Stack<string>();
                stkTabIndexes.Push(tbcLists.SelectedTab.Name);

                this.OnTabChange(this, stkTabIndexes);
            }
        }

        public void SetTabIndexes(Stack<string> stkTabIndexes) {
            if (tbcLists.TabPages.ContainsKey(stkTabIndexes.Peek()) == true) {
                this.tbcLists.SelectedTab = tbcLists.TabPages[stkTabIndexes.Pop()];
            }
        }

        private void SendCommand(params string[] a_strCommand) {
            if (this.m_prcClient != null) {
                this.m_prcClient.SendRequest(new List<string>(a_strCommand));
            }
        }

        #region Settings Animator

        private void SetControlValue(Control ctrlTarget, object objValue) {

            if (objValue != null) {
                if (ctrlTarget is TextBox) {
                    ((TextBox)ctrlTarget).Text = (string)objValue;
                }
                else if (ctrlTarget is CheckBox) {
                    ((CheckBox)ctrlTarget).Checked = (bool)objValue;
                }
                else if (ctrlTarget is NumericUpDown) {
                    ((NumericUpDown)ctrlTarget).Value = (decimal)objValue;
                }
                else if (ctrlTarget is Label) {
                    ((Label)ctrlTarget).Text = (string)objValue;
                }
            }
        }

        private void WaitForSettingResponse(string strResponseCommand) {

            if (this.m_dicAsyncSettingControls.ContainsKey(strResponseCommand) == true) {
                //this.m_dicAsyncSettingControls[strResponseCommand].m_objOriginalValue = String.Empty;
                this.m_dicAsyncSettingControls[strResponseCommand].m_picStatus.Image = this.m_frmMain.picAjaxStyleLoading.Image;
                this.m_dicAsyncSettingControls[strResponseCommand].m_iTimeout = AsyncStyleSetting.INT_ANIMATEDSETTING_TIMEOUT_TICKS;

                this.tmrTimeoutCheck.Enabled = true;


                foreach (Control ctrlEnable in this.m_dicAsyncSettingControls[strResponseCommand].ma_ctrlEnabledInputs) {
                    if (ctrlEnable is TextBox) {
                        ((TextBox)ctrlEnable).ReadOnly = true;
                    }
                    else {
                        ctrlEnable.Enabled = false;
                    }
                }
            }
        }

        public void OnSettingResponse(string strResponseCommand, bool blSuccess) {

            if (this.m_dicAsyncSettingControls.ContainsKey(strResponseCommand) == true) {

                if (this.m_dicAsyncSettingControls[strResponseCommand].m_blReEnableControls == true) {
                    foreach (Control ctrlEnable in this.m_dicAsyncSettingControls[strResponseCommand].ma_ctrlEnabledInputs) {
                        if (ctrlEnable is TextBox) {
                            ((TextBox)ctrlEnable).ReadOnly = false;
                        }
                        else {
                            ctrlEnable.Enabled = true;
                        }
                    }
                }

                this.m_dicAsyncSettingControls[strResponseCommand].IgnoreEvent = true;

                if (blSuccess == true) {
                    this.m_dicAsyncSettingControls[strResponseCommand].m_picStatus.Image = this.m_frmMain.picAjaxStyleSuccess.Image;
                    this.m_dicAsyncSettingControls[strResponseCommand].m_iTimeout = AsyncStyleSetting.INT_ANIMATEDSETTING_SHOWRESULT_TICKS;
                    this.m_dicAsyncSettingControls[strResponseCommand].m_blSuccess = true;
                }
                else {
                    this.m_dicAsyncSettingControls[strResponseCommand].m_picStatus.Image = this.m_frmMain.picAjaxStyleFail.Image;
                    this.m_dicAsyncSettingControls[strResponseCommand].m_iTimeout = AsyncStyleSetting.INT_ANIMATEDSETTING_SHOWRESULT_TICKS;
                    this.m_dicAsyncSettingControls[strResponseCommand].m_blSuccess = false;
                }

                this.tmrTimeoutCheck.Enabled = true;

                this.m_dicAsyncSettingControls[strResponseCommand].IgnoreEvent = false;
            }
        }


        private int CountTicking() {
            int i = 0;

            foreach (KeyValuePair<string, AsyncStyleSetting> kvpAsync in this.m_dicAsyncSettingControls) {
                if (kvpAsync.Value.m_iTimeout >= 0) {
                    i++;
                }
            }

            return i;
        }

        private void tmrSettingsAnimator_Tick(object sender, EventArgs e) {
            //if (((from o in this.m_dicAsyncSettingControls where o.Value.m_iTimeout >= 0 select o).Count()) > 0) {
            if (this.CountTicking() > 0) {
                foreach (KeyValuePair<string, AsyncStyleSetting> kvpAsyncSetting in this.m_dicAsyncSettingControls) {

                    kvpAsyncSetting.Value.m_iTimeout--;
                    if (kvpAsyncSetting.Value.m_iTimeout == 0 && kvpAsyncSetting.Value.m_blSuccess == false) {
                        kvpAsyncSetting.Value.m_picStatus.Image = this.m_frmMain.picAjaxStyleFail.Image;
                        kvpAsyncSetting.Value.m_iTimeout = AsyncStyleSetting.INT_ANIMATEDSETTING_SHOWRESULT_TICKS;

                        kvpAsyncSetting.Value.m_blSuccess = true;
                    }
                    else if (kvpAsyncSetting.Value.m_iTimeout == 0 && kvpAsyncSetting.Value.m_blSuccess == true) {
                        kvpAsyncSetting.Value.m_picStatus.Image = null;

                        if (kvpAsyncSetting.Value.m_blReEnableControls == true) {
                            foreach (Control ctrlEnable in kvpAsyncSetting.Value.ma_ctrlEnabledInputs) {
                                if (ctrlEnable is TextBox) {
                                    ((TextBox)ctrlEnable).ReadOnly = false;
                                }
                                else {
                                    ctrlEnable.Enabled = true;
                                }
                            }
                        }
                    }
                }
            }
            else {
                this.tmrTimeoutCheck.Enabled = false;
            }
        }

        #endregion

        #region Reserved Slots

        public void OnReservedSlotsList(FrostbiteClient sender, List<string> lstSoldierNames) {
            this.lsvReservedList.BeginUpdate();
            this.lsvReservedList.Items.Clear();
            foreach (string strSoldierName in lstSoldierNames) {
                if (this.lsvReservedList.Items.ContainsKey(strSoldierName) == false) {

                    ListViewItem lsvNewSoldier = new ListViewItem(strSoldierName);
                    lsvNewSoldier.Name = strSoldierName;

                    this.lsvReservedList.Items.Add(lsvNewSoldier);
                }
            }
            this.lsvReservedList.EndUpdate();
        }

        public void OnReservedSlotsPlayerRemoved(FrostbiteClient sender, string strSoldierName) {
            if (this.lsvReservedList.Items.ContainsKey(strSoldierName) == true) {
                this.lsvReservedList.Items.RemoveByKey(strSoldierName);
            }
        }

        public void OnReservedSlotsPlayerAdded(FrostbiteClient sender, string strSoldierName) {
            if (this.lsvReservedList.Items.ContainsKey(strSoldierName) == false) {

                ListViewItem lsvNewSoldier = new ListViewItem(strSoldierName);
                lsvNewSoldier.Name = strSoldierName;

                this.lsvReservedList.Items.Add(lsvNewSoldier);
            }
        }

        public void OnReservedSlotsSave(FrostbiteClient sender) {
            if (this.m_blSettingAppendingReservedPlayer == true) {
                this.OnSettingResponse("local.reservedlist.append", true);
                this.m_blSettingAppendingReservedPlayer = false;
            }
            else if (this.m_blSettingRemovingReservedPlayer == true) {
                this.OnSettingResponse("local.reservedlist.remove", true);
                this.m_blSettingRemovingReservedPlayer = false;
            }
            
        }

        private void lnkReservedAddSoldierName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {

            if (this.txtReservedAddSoldierName.Text.Length > 0) {
                this.m_blSettingAppendingReservedPlayer = true;
                this.WaitForSettingResponse("local.reservedlist.append");

                this.m_prcClient.Game.SendReservedSlotsAddPlayerPacket(this.txtReservedAddSoldierName.Text);
                //this.SendCommand("reservedSlots.addPlayer", );

                this.m_prcClient.Game.SendReservedSlotsSavePacket();
                //this.SendCommand("reservedSlots.save");

                this.txtReservedAddSoldierName.Clear();
                this.txtReservedAddSoldierName.Focus();
            }
        }

        private void lsvReservedList_SelectedIndexChanged(object sender, EventArgs e) {

            if (this.lsvReservedList.SelectedItems.Count > 0 && this.lsvReservedList.FocusedItem != null) {
                this.btnReservedRemoveSoldier.Enabled = true && this.m_spPrivileges.CanEditReservedSlotsList;
            }

        }

        private void btnReservedRemoveSoldier_Click(object sender, EventArgs e) {

            if (this.lsvReservedList.SelectedItems.Count > 0) {

                this.m_blSettingRemovingReservedPlayer = true;
                this.WaitForSettingResponse("local.reservedlist.remove");

                this.m_prcClient.Game.SendReservedSlotsRemovePlayerPacket(this.lsvReservedList.SelectedItems[0].Name);
                //this.SendCommand("reservedSlots.removePlayer", this.lsvReservedList.SelectedItems[0].Name);

                this.m_prcClient.Game.SendReservedSlotsSavePacket();
                //this.SendCommand("reservedSlots.save");
            }
        }

        private void txtReservedAddSoldierName_TextChanged(object sender, EventArgs e) {

            if (this.txtReservedAddSoldierName.Text.Length > 0) {
                this.lnkReservedAddSoldierName.Enabled = true && this.m_spPrivileges.CanEditReservedSlotsList;
            }
            else {
                this.lnkReservedAddSoldierName.Enabled = false;
            }
        }

        private void txtReservedAddSoldierName_KeyDown(object sender, KeyEventArgs e) {
            if (this.txtReservedAddSoldierName.Text.Length > 0 && e.KeyData == Keys.Enter) {
                this.lnkReservedAddSoldierName_LinkClicked(this, null);
                e.SuppressKeyPress = true;
            }
        }

        private void lsvReservedList_ColumnClick(object sender, ColumnClickEventArgs e) {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == this.m_lvwReservedSlotsColumnSorter.SortColumn) {
                // Reverse the current sort direction for this column.
                if (this.m_lvwReservedSlotsColumnSorter.Order == SortOrder.Ascending) {
                    this.m_lvwReservedSlotsColumnSorter.Order = SortOrder.Descending;
                }
                else {
                    this.m_lvwReservedSlotsColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else {
                // Set the column number that is to be sorted; default to ascending.
                this.m_lvwReservedSlotsColumnSorter.SortColumn = e.Column;
                this.m_lvwReservedSlotsColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lsvReservedList.Sort();
        }

        private void btnReservedSlotsListRefresh_Click(object sender, EventArgs e)
        {
            this.m_prcClient.Game.SendReservedSlotsLoadPacket();
            this.m_prcClient.Game.SendReservedSlotsListPacket();
        }

        #endregion

        #region Banlist

        private void Reasons_ItemRemoved(int iIndex, string item) {
            if (this.cboBanlistReason.Items.Contains(item) == true) {
                this.cboBanlistReason.Items.Remove(item);
            }
        }

        private void Reasons_ItemAdded(int iIndex, string item) {
            this.cboBanlistReason.Items.Add(item);
        }

        /*
        public ComboBox.ObjectCollection PunkbusterReasons {
            get {
                return this.cboBanlistReason.Items;
            }
        }
        */

        public string SecondsToText(UInt32 iSeconds, string[] a_strTimeDescriptions) {
            string strReturn = String.Empty;

            double dblSeconds = iSeconds;
            double dblMinutes = (iSeconds / 60);
            double dblHours = (dblMinutes / 60);
            double dblDays = (dblHours / 24);
            double dblWeeks = (dblDays / 7);
            double dblMonths = (dblWeeks / 4);
            double dblYears = (dblMonths / 12);

            if ((Int32)dblYears > 0) {
                strReturn += String.Empty + ((Int32)dblYears) + (((Int32)dblYears) == 1 ? a_strTimeDescriptions[0] : a_strTimeDescriptions[1]);
            }
            if ((Int32)dblMonths % 12 > 0) {
                strReturn += String.Empty + ((Int32)dblMonths) % 12 + (((Int32)dblMonths % 12) == 1 ? a_strTimeDescriptions[2] : a_strTimeDescriptions[3]);
            }
            if ((Int32)dblWeeks % 4 > 0) {
                strReturn += String.Empty + ((Int32)dblWeeks) % 4 + (((Int32)dblWeeks % 4) == 1 ? a_strTimeDescriptions[4] : a_strTimeDescriptions[5]);
            }
            if ((Int32)dblDays % 7 > 0) {
                strReturn += String.Empty + ((Int32)dblDays) % 7 + (((Int32)dblDays % 7) == 1 ? a_strTimeDescriptions[6] : a_strTimeDescriptions[7]);
            }
            if ((Int32)dblHours % 24 > 0) {
                strReturn += String.Empty + ((Int32)dblHours) % 24 + (((Int32)dblHours % 24) == 1 ? a_strTimeDescriptions[8] : a_strTimeDescriptions[9]);
            }
            if ((Int32)dblMinutes % 60 > 0) {
                strReturn += String.Empty + ((Int32)dblMinutes) % 60 + (((Int32)dblMinutes % 60) == 1 ? a_strTimeDescriptions[10] : a_strTimeDescriptions[11]);
            }

            if (iSeconds < 60) {
                if ((Int32)dblSeconds % 60 > 0) {
                    strReturn += String.Empty + ((Int32)dblSeconds) % 60 + (((Int32)dblSeconds % 60) == 1 ? a_strTimeDescriptions[12] : a_strTimeDescriptions[13]);
                }
            }

            return strReturn;
        }

        private ListViewItem CreateBlankBanEntry(string strName) {
            ListViewItem lviNewBanEntry = new ListViewItem();
            lviNewBanEntry.Name = strName;
            lviNewBanEntry.Text = String.Empty;

            ListViewItem.ListViewSubItem lvisIp = new ListViewItem.ListViewSubItem();
            lvisIp.Name = "ip";
            lvisIp.Text = String.Empty;
            lviNewBanEntry.SubItems.Add(lvisIp);

            ListViewItem.ListViewSubItem lvisGuid = new ListViewItem.ListViewSubItem();
            lvisGuid.Name = "guid";
            lvisGuid.Text = String.Empty;
            lviNewBanEntry.SubItems.Add(lvisGuid);

            ListViewItem.ListViewSubItem lvisType = new ListViewItem.ListViewSubItem();
            lvisType.Name = "type";
            lvisType.Text = String.Empty;
            lviNewBanEntry.SubItems.Add(lvisType);

            /*
            ListViewItem.ListViewSubItem lvisTimeOfBan = new ListViewItem.ListViewSubItem();
            lvisTimeOfBan.Name = "timedate";
            lvisTimeOfBan.Text = String.Empty;
            lviNewBanEntry.SubItems.Add(lvisTimeOfBan);

            ListViewItem.ListViewSubItem lvisBanLength = new ListViewItem.ListViewSubItem();
            lvisBanLength.Name = "banlength";
            lvisBanLength.Text = String.Empty;
            lviNewBanEntry.SubItems.Add(lvisBanLength);
            */

            ListViewItem.ListViewSubItem lvisTimeRemaining = new ListViewItem.ListViewSubItem();
            lvisTimeRemaining.Name = "timeremaining";
            lvisTimeRemaining.Text = String.Empty;
            lviNewBanEntry.SubItems.Add(lvisTimeRemaining);

            ListViewItem.ListViewSubItem lvisReason = new ListViewItem.ListViewSubItem();
            lvisReason.Name = "reason";
            lvisReason.Text = String.Empty;
            lviNewBanEntry.SubItems.Add(lvisReason);

            return lviNewBanEntry;
        }

        private string GetFriendlyTypeName(string strType) {
            string strFriendlyTypeName = String.Empty;

            if (String.Compare(strType, "name", true) == 0) {
                strFriendlyTypeName = this.m_prcClient.Language.GetLocalized("uscListControlPanel.tabBanlist.lsvBanlist.colType.Name", null);
            }
            else if (String.Compare(strType, "ip", true) == 0) {
                strFriendlyTypeName = "IpAddress";
            }
            else if (String.Compare(strType, "guid", true) == 0) {
                strFriendlyTypeName = "Guid";
            }
            else if (String.Compare(strType, "pbguid", true) == 0) {
                strFriendlyTypeName = "PB Guid";
            }

            return strFriendlyTypeName;
        }

        private ListViewItem CreateBanEntry(CBanInfo cbiPlayerBan) {

            ListViewItem lviNewBanEntry = null;

            if (String.Compare("name", cbiPlayerBan.IdType, true) == 0) {
                lviNewBanEntry = this.CreateBlankBanEntry(String.Format("{0}\r\n\r\n", cbiPlayerBan.SoldierName));
                lviNewBanEntry.Text = cbiPlayerBan.SoldierName;

                lviNewBanEntry.SubItems["type"].Tag = cbiPlayerBan.IdType;
                lviNewBanEntry.SubItems["type"].Text = this.GetFriendlyTypeName(cbiPlayerBan.IdType);

                //lviNewBanEntry.SubItems["banlength"].Tag = cbiPlayerBan.BanLength;
                lviNewBanEntry.SubItems["timeremaining"].Tag = cbiPlayerBan.BanLength;

                lviNewBanEntry.SubItems["reason"].Text = cbiPlayerBan.Reason;
            }
            else if (String.Compare("ip", cbiPlayerBan.IdType, true) == 0) {

                lviNewBanEntry = this.CreateBlankBanEntry(String.Format("\r\n{0}\r\n", cbiPlayerBan.IpAddress));
                lviNewBanEntry.Text = String.Empty;

                lviNewBanEntry.SubItems["ip"].Text = cbiPlayerBan.IpAddress;

                lviNewBanEntry.SubItems["type"].Tag = cbiPlayerBan.IdType;
                lviNewBanEntry.SubItems["type"].Text = this.GetFriendlyTypeName(cbiPlayerBan.IdType);

                //lviNewBanEntry.SubItems["banlength"].Tag = cbiPlayerBan.BanLength;
                lviNewBanEntry.SubItems["timeremaining"].Tag = cbiPlayerBan.BanLength;

                lviNewBanEntry.SubItems["reason"].Text = cbiPlayerBan.Reason;

            }
            else if (String.Compare("guid", cbiPlayerBan.IdType, true) == 0) {

                lviNewBanEntry = this.CreateBlankBanEntry(String.Format("\r\n\r\n{0}", cbiPlayerBan.Guid));
                lviNewBanEntry.Text = cbiPlayerBan.SoldierName;
                lviNewBanEntry.SubItems["guid"].Text = cbiPlayerBan.Guid;
                lviNewBanEntry.SubItems["ip"].Text = cbiPlayerBan.IpAddress;

                lviNewBanEntry.SubItems["type"].Tag = cbiPlayerBan.IdType;
                lviNewBanEntry.SubItems["type"].Text = this.GetFriendlyTypeName(cbiPlayerBan.IdType);

                //lviNewBanEntry.SubItems["banlength"].Tag = cbiPlayerBan.BanLength;
                lviNewBanEntry.SubItems["timeremaining"].Tag = cbiPlayerBan.BanLength;

                lviNewBanEntry.SubItems["reason"].Text = cbiPlayerBan.Reason;
            }

            else if (String.Compare("pbguid", cbiPlayerBan.IdType, true) == 0) {

                lviNewBanEntry = this.CreateBlankBanEntry(String.Format("\r\n\r\n{0}", cbiPlayerBan.Guid));
                lviNewBanEntry.Text = cbiPlayerBan.SoldierName;
                lviNewBanEntry.SubItems["guid"].Text = cbiPlayerBan.Guid;
                lviNewBanEntry.SubItems["ip"].Text = cbiPlayerBan.IpAddress;

                lviNewBanEntry.SubItems["type"].Tag = cbiPlayerBan.IdType;
                lviNewBanEntry.SubItems["type"].Text = this.GetFriendlyTypeName(cbiPlayerBan.IdType);

                //lviNewBanEntry.SubItems["banlength"].Tag = cbiPlayerBan.BanLength;
                lviNewBanEntry.SubItems["timeremaining"].Tag = cbiPlayerBan.BanLength;

                lviNewBanEntry.SubItems["reason"].Text = cbiPlayerBan.Reason.TrimEnd('"'); ;
            }

            return lviNewBanEntry;
        }

        public void RemoveDeletedBans(List<CBanInfo> lstBans) {

            for (int i = 0; i < this.lsvBanlist.Items.Count; i++) {
                bool blFoundBan = false;
                foreach (CBanInfo cbiBan in lstBans) {

                    switch ((string)this.lsvBanlist.Items[i].SubItems["type"].Tag) {
                        case "name":
                            blFoundBan = (String.Compare(this.lsvBanlist.Items[i].Name, String.Format("{0}\r\n\r\n", cbiBan.SoldierName)) == 0);
                            break;
                        case "ip":
                            blFoundBan = (String.Compare(this.lsvBanlist.Items[i].Name, String.Format("\r\n{0}\r\n", cbiBan.IpAddress)) == 0);
                            break;
                        case "guid":
                            blFoundBan = (String.Compare(this.lsvBanlist.Items[i].Name, String.Format("\r\n\r\n{0}", cbiBan.Guid)) == 0);
                            break;
                        case "pbguid":
                            // Ignore pb ban entries, handled in the pb event method.
                            blFoundBan = true;
                            break;
                        default:
                            break;
                    }

                    if (blFoundBan == true) {
                        break;
                    }
                }

                if (blFoundBan == false && String.Compare((string)this.lsvBanlist.Items[i].SubItems["type"].Tag, "pbguid") != 0) {
                    this.lsvBanlist.Items.Remove(this.lsvBanlist.Items[i]);
                    i--;
                }
            }
        }

        public void OnBanList(PRoConClient sender, List<CBanInfo> lstBans) {

            this.lsvBanlist.BeginUpdate();

            foreach (CBanInfo cbiBan in lstBans) {
                
                string strKey = String.Empty;

                if (String.Compare(cbiBan.IdType, "name") == 0) {
                    strKey = String.Format("{0}\r\n\r\n", cbiBan.SoldierName);
                }
                else if (String.Compare(cbiBan.IdType, "ip") == 0) {
                    strKey = String.Format("\r\n{0}\r\n", cbiBan.IpAddress);
                }
                else if (String.Compare(cbiBan.IdType, "guid") == 0) {
                    strKey = String.Format("\r\n\r\n{0}", cbiBan.Guid);
                }

                if (this.lsvBanlist.Items.ContainsKey(strKey) == false) {
                    this.lsvBanlist.Items.Add(this.CreateBanEntry(cbiBan));
                }
                else {
                    ListViewItem lviBanEntry = this.lsvBanlist.Items[strKey];
                    lviBanEntry.Text = cbiBan.SoldierName;
                    lviBanEntry.SubItems["type"].Tag = cbiBan.IdType;
                    lviBanEntry.SubItems["type"].Text = this.GetFriendlyTypeName(cbiBan.IdType);
                    lviBanEntry.SubItems["timeremaining"].Tag = cbiBan.BanLength;
                    lviBanEntry.SubItems["reason"].Text = cbiBan.Reason;
                }
            }

            this.RemoveDeletedBans(lstBans);

            for (int i = 0; i < this.lsvBanlist.Columns.Count; i++) {
                this.lsvBanlist.Columns[i].Width = -2;
            }

            this.tmrRefreshBanlist_Tick(null, null);

            this.lsvBanlist.EndUpdate();

        }

        private void m_prcClient_PunkbusterPlayerUnbanned(PRoConClient sender, CBanInfo cbiUnbannedPlayer) {
            this.OnUnban(sender.Game, cbiUnbannedPlayer);    
        }

        public void OnUnban(FrostbiteClient sender, CBanInfo cbiBan) {

            string strKey = String.Empty;

            if (String.Compare(cbiBan.IdType, "name") == 0) {
                strKey = String.Format("{0}\r\n\r\n", cbiBan.SoldierName);
            }
            else if (String.Compare(cbiBan.IdType, "ip") == 0) {
                strKey = String.Format("\r\n{0}\r\n", cbiBan.IpAddress);
            }
            else if (String.Compare(cbiBan.IdType, "guid") == 0 || String.Compare(cbiBan.IdType, "pbguid") == 0) {
                strKey = String.Format("\r\n\r\n{0}", cbiBan.Guid);
            }

            if (this.lsvBanlist.Items.ContainsKey(strKey) == true) {
                this.lsvBanlist.Items[strKey].Remove();
                this.OnSettingResponse("local.banlist.unban", true);
            }
        }

        private void tmrRefreshBanlist_Tick(object sender, EventArgs e) {
            foreach (ListViewItem lviBanEntry in this.lsvBanlist.Items) {

                if (lviBanEntry.SubItems["timeremaining"].Tag != null) {

                    TimeoutSubset ctsTimeout = (TimeoutSubset)lviBanEntry.SubItems["timeremaining"].Tag;

                    if (ctsTimeout.Subset == TimeoutSubset.TimeoutSubsetType.Permanent) {
                        lviBanEntry.SubItems["timeremaining"].Text = this.m_prcClient.Language.GetLocalized("uscListControlPanel.tabBanlist.lsvBanlist.colTimeRemaining.Permanent", null);
                    }
                    else if (ctsTimeout.Subset == TimeoutSubset.TimeoutSubsetType.Round) {
                        lviBanEntry.SubItems["timeremaining"].Text = this.m_prcClient.Language.GetLocalized("uscListControlPanel.tabBanlist.lsvBanlist.colTimeRemaining.Round", null);
                    }
                    else if (ctsTimeout.Subset == TimeoutSubset.TimeoutSubsetType.Seconds) {

                        if (ctsTimeout.Seconds > 0) {
                            lviBanEntry.SubItems["timeremaining"].Text = this.SecondsToText((UInt32)ctsTimeout.Seconds, this.ma_strTimeDescriptionsShort);

                            ctsTimeout.Seconds -= (this.tmrRefreshBanlist.Interval / 1000);
                        }
                        else {
                            // I was going to remove it here but I want it to display unbanned until next banList update.
                            lviBanEntry.SubItems["timeremaining"].Text = this.m_prcClient.Language.GetLocalized("uscListControlPanel.tabBanlist.lsvBanlist.colTimeRemaining.Unbanned", null);
                        }
                    }
                }
            }
        }

        public void OnPbGuidUnban(CBanInfo cbiGuidBan) {
            if (this.lsvBanlist.Items.ContainsKey(String.Format("\r\n\r\n{0}", cbiGuidBan.Guid)) == true) {
                this.lsvBanlist.Items[String.Format("\r\n\r\n{0}", cbiGuidBan.Guid)].Remove();
                this.OnSettingResponse("local.banlist.unban", true);
            }
        }

        public void OnPbGuidBan(PRoConClient sender, CBanInfo cbiGuidBan) {
            this.lsvBanlist.BeginUpdate();

            if (this.lsvBanlist.Items.ContainsKey(String.Format("\r\n\r\n{0}", cbiGuidBan.Guid)) == false) {
                this.lsvBanlist.Items.Add(this.CreateBanEntry(cbiGuidBan));
            }
            else {
                ListViewItem lviBanEntry = this.lsvBanlist.Items[String.Format("\r\n\r\n{0}", cbiGuidBan.Guid)];
                lviBanEntry.Text = cbiGuidBan.SoldierName;
                lviBanEntry.SubItems["guid"].Text = cbiGuidBan.Guid;
                lviBanEntry.SubItems["ip"].Text = cbiGuidBan.IpAddress;

                lviBanEntry.SubItems["type"].Tag = cbiGuidBan.IdType;
                lviBanEntry.SubItems["type"].Text = this.GetFriendlyTypeName(cbiGuidBan.IdType);

                lviBanEntry.SubItems["timeremaining"].Tag = cbiGuidBan.BanLength;

                lviBanEntry.SubItems["reason"].Tag = cbiGuidBan.Reason.TrimEnd('"');
            }

            for (int i = 0; i < this.lsvBanlist.Columns.Count; i++) {
                this.lsvBanlist.Columns[i].Width = -2;
            }

            this.lsvBanlist.EndUpdate();
        }
        
        private void unbanToolStripMenuItem_Click(object sender, EventArgs e) {
            this.btnBanlistUnban_Click(sender, e);
        }

        private void btnBanlistUnban_Click(object sender, EventArgs e) {

            if (this.lsvBanlist.SelectedItems.Count > 0) {
                this.WaitForSettingResponse("local.banlist.unban");

                if (String.Compare((string)this.lsvBanlist.SelectedItems[0].SubItems["type"].Tag, "name") == 0) {
                    this.SendCommand("banList.remove", "name", this.lsvBanlist.SelectedItems[0].Text);
                }
                else if (String.Compare((string)this.lsvBanlist.SelectedItems[0].SubItems["type"].Tag, "ip") == 0) {
                    this.SendCommand("banList.remove", "ip", this.lsvBanlist.SelectedItems[0].SubItems["ip"].Text);
                }
                else if (String.Compare((string)this.lsvBanlist.SelectedItems[0].SubItems["type"].Tag, "guid") == 0) {
                    this.SendCommand("banList.remove", "guid", this.lsvBanlist.SelectedItems[0].SubItems["guid"].Text);
                }
                else if (String.Compare((string)this.lsvBanlist.SelectedItems[0].SubItems["type"].Tag, "pbguid") == 0) {
                    this.SendCommand("punkBuster.pb_sv_command", String.Format("pb_sv_unbanguid {0}", this.lsvBanlist.SelectedItems[0].SubItems["guid"].Text));
                    this.SendCommand("punkBuster.pb_sv_command", "pb_sv_updbanfile");
                    this.SendCommand("punkBuster.pb_sv_command", "pb_sv_banload");
                }

                this.m_prcClient.Game.SendBanListSavePacket();
                this.m_prcClient.Game.SendBanListListPacket();
            }
        }

        private void lsvBanlist_SelectedIndexChanged(object sender, EventArgs e) {

            if (this.lsvBanlist.SelectedItems.Count > 0) {
                this.btnBanlistUnban.Enabled = true && this.m_spPrivileges.CanEditBanList;
            }
            else if (this.lsvBanlist.FocusedItem != null) {
                this.btnBanlistUnban.Enabled = false;
            }

        }

        private void lsvBanlist_ColumnClick(object sender, ColumnClickEventArgs e) {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == this.m_lvwBanlistColumnSorter.SortColumn) {
                // Reverse the current sort direction for this column.
                if (this.m_lvwBanlistColumnSorter.Order == SortOrder.Ascending) {
                    this.m_lvwBanlistColumnSorter.Order = SortOrder.Descending;
                }
                else {
                    this.m_lvwBanlistColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else {
                // Set the column number that is to be sorted; default to ascending.
                this.m_lvwBanlistColumnSorter.SortColumn = e.Column;
                this.m_lvwBanlistColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lsvBanlist.Sort();
        }

        public void OnClearBanList(FrostbiteClient sender) {
            this.OnSettingResponse("local.banlist.clearlist", true);
        }

        private void btnBanlistClearNameList_Click(object sender, EventArgs e) {

            DialogResult dlgClearList = MessageBox.Show(this.m_prcClient.Language.GetLocalized("uscListControlPanel.tabBanlist.btnBanlistClearBanlist.Question", null), this.m_prcClient.Language.GetLocalized("uscListControlPanel.tabBanlist.btnBanlistClearBanlist.Title", null), MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dlgClearList == DialogResult.Yes) {
                this.WaitForSettingResponse("local.banlist.clearlist");

                this.m_prcClient.Game.SendBanListClearPacket();
                this.m_prcClient.Game.SendBanListSavePacket();
                this.m_prcClient.Game.SendBanListListPacket();
            }
        }

        private void rdoBanlistName_CheckedChanged(object sender, EventArgs e) {
            if (this.rdoBanlistName.Checked == true) {
                //this.ValidateManualBan();
                this.txtBanlistManualBanName.Focus();
                this.txtBanlistManualBanName.Enabled = true;
                this.txtBanlistManualBanIP.Enabled = false;
                this.txtBanlistManualBanGUID.Enabled = false;
                //this.cboBanlistReason.Enabled = false;
                //this.lblBanlistReason.Enabled = false;

                this.rdoBanlistTemporary.Enabled = true;

                this.UpdateConfirmationLabel();
            }
        }

        private void rdoBanlistIP_CheckedChanged(object sender, EventArgs e) {
            if (this.rdoBanlistIP.Checked == true) {
                this.txtBanlistManualBanIP.Focus();
                this.txtBanlistManualBanName.Enabled = false;
                this.txtBanlistManualBanIP.Enabled = true;
                this.txtBanlistManualBanGUID.Enabled = false;
                //this.cboBanlistReason.Enabled = false;
                //this.lblBanlistReason.Enabled = false;

                this.rdoBanlistTemporary.Enabled = true;

                this.UpdateConfirmationLabel();
            }
        }

        private void rdoBanlistBc2GUID_CheckedChanged(object sender, EventArgs e) {
            if (this.rdoBanlistBc2GUID.Checked == true) {
                this.txtBanlistManualBanGUID.Focus();
                this.txtBanlistManualBanName.Enabled = false;
                this.txtBanlistManualBanIP.Enabled = false;
                this.txtBanlistManualBanGUID.Enabled = true;
                //this.cboBanlistReason.Enabled = true;
                //this.lblBanlistReason.Enabled = true;
                this.txtBanlistManualBanIP.ForeColor = SystemColors.WindowText;

                this.rdoBanlistTemporary.Enabled = true;
                //this.rdoBanlistPermanent.Checked = true;

                this.UpdateConfirmationLabel();
            }
        }

        private void rdoBanlistGUID_CheckedChanged(object sender, EventArgs e) {
            if (this.rdoBanlistPbGUID.Checked == true) {
                this.txtBanlistManualBanGUID.Focus();
                this.txtBanlistManualBanName.Enabled = true;
                this.txtBanlistManualBanIP.Enabled = true;
                this.txtBanlistManualBanGUID.Enabled = true;
                //this.cboBanlistReason.Enabled = true;
                //this.lblBanlistReason.Enabled = true;
                this.txtBanlistManualBanIP.ForeColor = SystemColors.WindowText;

                this.rdoBanlistTemporary.Enabled = false;
                this.rdoBanlistPermanent.Checked = true;

                this.UpdateConfirmationLabel();
            }
        }

        private void ListSettings_ManualBansVisibleChange(bool isVisible) {
            if (isVisible == true) {
                this.lnkCloseOpenManualBans.Text = this.m_prcClient.Language.GetLocalized("uscListControlPanel.tabBanlist.lnkCloseOpenManualBans.Close", null);
                this.picCloseOpenManualBans.Image = this.m_frmMain.iglIcons.Images["arrow_down.png"];

                this.spltBanlistManualBans.Panel2Collapsed = false;
            }
            else {
                this.lnkCloseOpenManualBans.Text = this.m_prcClient.Language.GetLocalized("uscListControlPanel.tabBanlist.lnkCloseOpenManualBans.Open", null);
                this.picCloseOpenManualBans.Image = this.m_frmMain.iglIcons.Images["arrow_up.png"];

                this.spltBanlistManualBans.Panel2Collapsed = true;
            }
        }

        private void lnkAddBan_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (this.m_prcClient != null) {
                this.m_prcClient.ListSettings.ManualBansVisible = !this.m_prcClient.ListSettings.ManualBansVisible;
            }
        }

        private void picCloseOpenManualBans_Click(object sender, EventArgs e) {
            if (this.m_prcClient != null) {
                this.m_prcClient.ListSettings.ManualBansVisible = !this.m_prcClient.ListSettings.ManualBansVisible;
            }
        }

        private void btnBanlistAddBan_Click(object sender, EventArgs e) {

            if (this.rdoBanlistName.Checked == true) {
                if (this.rdoBanlistPermanent.Checked == true) {
                    this.SendCommand("banList.add", "name", this.txtBanlistManualBanName.Text, "perm", this.cboBanlistReason.Text);
                }
                else {
                    this.SendCommand("banList.add", "name", this.txtBanlistManualBanName.Text, "seconds", (uscPlayerPunishPanel.GetBanLength(this.txtBanlistTime, this.cboBanlistTimeMultiplier) * 60).ToString(), this.cboBanlistReason.Text);
                }
            }
            else if (this.rdoBanlistIP.Checked == true) {
                if (this.rdoBanlistPermanent.Checked == true) {
                    this.SendCommand("banList.add", "ip", this.txtBanlistManualBanIP.Text, "perm", this.cboBanlistReason.Text);
                }
                else {
                    this.SendCommand("banList.add", "ip", this.txtBanlistManualBanIP.Text, "seconds", (uscPlayerPunishPanel.GetBanLength(this.txtBanlistTime, this.cboBanlistTimeMultiplier) * 60).ToString(), this.cboBanlistReason.Text);
                }
            }
            else if (this.rdoBanlistBc2GUID.Checked == true) {
                if (this.rdoBanlistPermanent.Checked == true) {
                    this.SendCommand("banList.add", "guid", this.txtBanlistManualBanGUID.Text, "perm", this.cboBanlistReason.Text);
                }
                else {
                    this.SendCommand("banList.add", "guid", this.txtBanlistManualBanGUID.Text, "seconds", (uscPlayerPunishPanel.GetBanLength(this.txtBanlistTime, this.cboBanlistTimeMultiplier) * 60).ToString(), this.cboBanlistReason.Text);
                }
            }
            else if (this.rdoBanlistPbGUID.Checked == true) {
                this.SendCommand("punkBuster.pb_sv_command", String.Format("pb_sv_banguid {0} \"{1}\" \"{2}\" \"BC2! {3}\"", this.txtBanlistManualBanGUID.Text, this.txtBanlistManualBanName.Text.Length > 0 ? this.txtBanlistManualBanName.Text : "???", this.txtBanlistManualBanIP.Text.Length > 0 ? this.txtBanlistManualBanIP.Text : "???", this.cboBanlistReason.Text));
                this.SendCommand("punkBuster.pb_sv_command", this.m_prcClient.Variables.GetVariable<string>("PUNKBUSTER_BANLIST_REFRESH", "pb_sv_banlist BC2! "));
            }
            
            this.txtBanlistManualBanName.Focus();
            this.txtBanlistManualBanName.Clear();
            this.txtBanlistManualBanGUID.Clear();
            this.txtBanlistManualBanIP.Clear();

            this.m_prcClient.Game.SendBanListSavePacket();
            this.m_prcClient.Game.SendBanListListPacket();
        }

        private void txtBanlistManualBanName_TextChanged(object sender, EventArgs e) {
            this.UpdateConfirmationLabel();
        }

        private void txtBanlistManualBanIP_TextChanged(object sender, EventArgs e) {
            this.UpdateConfirmationLabel();
        }

        private void txtBanlistManualBanGUID_TextChanged(object sender, EventArgs e) {
            this.UpdateConfirmationLabel();
        }

        private void lsvBanlist_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Right) {

                if (this.lsvBanlist.SelectedItems.Count > 0) {
                    Point pntMouseLocation = new Point(e.X, e.Y);
                    this.ctxBanlistMenuStrip.Show(this.lsvBanlist, pntMouseLocation);
                }
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e) {

            if (this.lsvBanlist.SelectedItems.Count > 0) {

                string strClipboard = this.lsvBanlist.SelectedItems[0].Text;

                foreach (ListViewItem.ListViewSubItem lvsiItem in this.lsvBanlist.SelectedItems[0].SubItems) {
                    strClipboard += " " + lvsiItem.Text;
                }

                try {
                    Clipboard.SetDataObject(strClipboard, true, 5, 10);
                }
                catch (Exception) {
                    // Nope, another thread is accessing the clipboard..
                }
            }
        }

        private void rdoBanlistTemporary_CheckedChanged(object sender, EventArgs e) {
            this.pnlBanlistTime.Enabled = this.rdoBanlistTemporary.Checked;
            this.UpdateConfirmationLabel();
        }

        private void rdoBanlistPermanent_CheckedChanged(object sender, EventArgs e) {
            this.pnlBanlistTime.Enabled = this.rdoBanlistTemporary.Checked;
            this.UpdateConfirmationLabel();
        }

        private void UpdateConfirmationLabel() {

            string strBanDescription = String.Empty;

            if (this.rdoBanlistPbGUID.Checked == true || this.rdoBanlistBc2GUID.Checked == true) {
                strBanDescription = this.txtBanlistManualBanGUID.Text;
            }
            else if (this.rdoBanlistIP.Checked == true) {
                strBanDescription = this.txtBanlistManualBanIP.Text;
            }
            else if (this.rdoBanlistName.Checked == true) {
                strBanDescription = this.txtBanlistManualBanName.Text;
            }

            bool blAbleToPunish = false;

            if (this.m_uscConnectionPanel != null) {
                this.lblBanlistConfirmation.Text = uscPlayerPunishPanel.GetConfirmationLabel(out blAbleToPunish, strBanDescription, this.m_spPrivileges,
                                                                                             this.m_prcClient.Language, false, false, this.rdoBanlistPermanent.Checked,
                                                                                             this.rdoBanlistTemporary.Checked, this.txtBanlistTime, this.cboBanlistTimeMultiplier,
                                                                                             this.ma_strTimeDescriptionsLong, this.m_prcClient.SV_Variables.GetVariable<int>("TEMP_BAN_CEILING", 3600));
            }

            if (this.rdoBanlistIP.Checked == true) {
                this.btnBanlistAddBan.Enabled = (this.txtBanlistManualBanIP.Text.Length > 0 && this.m_regIP.Match(this.txtBanlistManualBanIP.Text).Success) && blAbleToPunish == true;
                this.picBanlistIPError.Visible = !this.btnBanlistAddBan.Enabled && blAbleToPunish == true;

                if (this.btnBanlistAddBan.Enabled == false && blAbleToPunish == true) {
                    this.txtBanlistManualBanIP.ForeColor = Color.Maroon;
                }
                else {
                    this.txtBanlistManualBanIP.ForeColor = SystemColors.WindowText;
                }
            }
            else {
                this.picBanlistIPError.Visible = false;
            }

            if (this.rdoBanlistPbGUID.Checked == true) {
                this.btnBanlistAddBan.Enabled = (this.txtBanlistManualBanGUID.Text.Length > 0 && this.m_regPbGUID.Match(this.txtBanlistManualBanGUID.Text).Success) && blAbleToPunish == true;
                this.picBanlistGUIDError.Visible = !this.btnBanlistAddBan.Enabled && blAbleToPunish == true;

                if (this.btnBanlistAddBan.Enabled == false && blAbleToPunish == true) {
                    this.txtBanlistManualBanGUID.ForeColor = Color.Maroon;
                }
                else {
                    this.txtBanlistManualBanGUID.ForeColor = SystemColors.WindowText;
                }
            }
            else if (this.rdoBanlistBc2GUID.Checked == true) {
                this.btnBanlistAddBan.Enabled = (this.txtBanlistManualBanGUID.Text.Length > 0 && this.m_regBc2GUID.Match(this.txtBanlistManualBanGUID.Text).Success) && blAbleToPunish == true;
                this.picBanlistGUIDError.Visible = !this.btnBanlistAddBan.Enabled && blAbleToPunish == true;

                if (this.btnBanlistAddBan.Enabled == false && blAbleToPunish == true) {
                    this.txtBanlistManualBanGUID.ForeColor = Color.Maroon;
                }
                else {
                    this.txtBanlistManualBanGUID.ForeColor = SystemColors.WindowText;
                }
            }
            else {
                this.picBanlistGUIDError.Visible = false;
            }

            if (this.rdoBanlistName.Checked == true) {
                this.btnBanlistAddBan.Enabled = (this.txtBanlistManualBanName.Text.Length > 0) && blAbleToPunish == true;
            }

        }

        private void txtBanlistTime_TextChanged(object sender, EventArgs e) {
            this.UpdateConfirmationLabel();
        }

        private void cboBanlistTimeMultiplier_SelectedIndexChanged(object sender, EventArgs e) {
            this.UpdateConfirmationLabel();
        }

        private void txtBanlistTime_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b');
        }

        private void btnBanlistRefresh_Click(object sender, EventArgs e) {

            this.m_prcClient.Game.SendBanListListPacket();
            // .SendPunkbusterThing
            this.SendCommand("punkBuster.pb_sv_command", this.m_prcClient.Variables.GetVariable<string>("PUNKBUSTER_BANLIST_REFRESH", "pb_sv_banlist BC2! "));
        }

        #endregion

    }
}
