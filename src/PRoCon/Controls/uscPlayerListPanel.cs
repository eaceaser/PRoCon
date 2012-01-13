/*  Copyright 2010 Geoffrey 'Phogue' Green

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
using System.Windows.Forms;
using System.Collections;
//using System.Text.RegularExpressions;
using System.Net;

namespace PRoCon {
    using Core;
    using Core.Players;
    using Core.Plugin;
    using Core.Players.Items;
    using Core.Remote;
    using Core.TextChatModeration;
    using PRoCon.Forms;
    using PRoCon.Controls.ControlsEx;

    public partial class uscPlayerListPanel : UserControl {

        private frmMain m_frmMain;
        private uscServerConnection m_uscConnectionPanel;

        private PRoConClient m_prcClient;

        private CLocalization m_clocLanguage;
        private PlayerListColumnSorter m_lvwColumnSorter;

        private CPrivileges m_spPrivileges;

        private const int INT_NEUTRAL_SQUAD = 0;
        private const int INT_NEUTRAL_TEAM = 0;
        private const int INT_MAX_TEAMS = 5; //0 = neutral, 1, 2, 3, 4..
        
        public uscPlayerListPanel() {
            InitializeComponent();

            this.m_clocLanguage = null;
            this.m_lvwColumnSorter = new PlayerListColumnSorter();
            this.m_frmMain = null;
            this.m_uscConnectionPanel = null;

            this.m_spPrivileges = new CPrivileges();
            this.m_spPrivileges.PrivilegesFlags = CPrivileges.FullPrivilegesFlags;

            this.spltListAdditionalInfo.Panel2Collapsed = true;
            this.spltTwoSplit.Panel2Collapsed = true;
            this.spltFourSplit.Panel2Collapsed = true;
        }

        private bool m_isSplitterBeingSet;
        private void SetSplitterDistances() {

            this.m_isSplitterBeingSet = true;

            if (this.m_prcClient != null && this.m_prcClient.PlayerListSettings != null) {
                int iTwoSplitterDistance = (int)(this.spltTwoSplit.Width * this.m_prcClient.PlayerListSettings.TwoSplitterPercentage);
                int iFourSplitterDistance = (int)(this.spltFourSplit.Height * this.m_prcClient.PlayerListSettings.FourSplitterPercentage);

                if (iTwoSplitterDistance < this.spltTwoSplit.Panel1MinSize) {
                    this.spltBottomTwoSplit.SplitterDistance = this.spltTwoSplit.SplitterDistance = this.spltTwoSplit.Panel1MinSize;
                }
                else if (iTwoSplitterDistance > this.spltTwoSplit.Width - this.spltTwoSplit.Panel2MinSize) {
                    this.spltBottomTwoSplit.SplitterDistance = this.spltTwoSplit.SplitterDistance = this.spltTwoSplit.Width - this.spltTwoSplit.Panel2MinSize;
                }
                else {
                    this.spltBottomTwoSplit.SplitterDistance = this.spltTwoSplit.SplitterDistance = iTwoSplitterDistance;
                }

                if (iFourSplitterDistance < this.spltFourSplit.Panel1MinSize) {
                    this.spltFourSplit.SplitterDistance = this.spltFourSplit.Panel1MinSize;
                }
                else if (iFourSplitterDistance > this.spltFourSplit.Height - this.spltFourSplit.Panel2MinSize) {
                    this.spltFourSplit.SplitterDistance = this.spltFourSplit.Height - this.spltFourSplit.Panel2MinSize;
                }
                else {
                    this.spltFourSplit.SplitterDistance = iFourSplitterDistance;
                }

                for (int i = 0; i < this.lsvTeamOnePlayers.Columns.Count; i++) {
                    this.lsvTeamOnePlayers.Columns[i].Width = -2;
                    this.lsvTeamTwoPlayers.Columns[i].Width = -2;
                    this.lsvTeamThreePlayers.Columns[i].Width = -2;
                    this.lsvTeamFourPlayers.Columns[i].Width = -2;
                }

                this.Invalidate();
            }

            this.m_isSplitterBeingSet = false;
        }

        private void uscPlayerListPanel_Load(object sender, EventArgs e) {
            if (this.m_prcClient != null) {
                this.m_clocLanguage = this.m_prcClient.Language;

            }
        }

        public void Initialize(frmMain frmMainWindow, uscServerConnection uscConnectionPanel) {
            this.m_frmMain = frmMainWindow;
            this.m_uscConnectionPanel = uscConnectionPanel;

            this.kbpPunkbusterPunishPanel.Punkbuster = true;
            this.kbpPunkbusterPunishPanel.PunishPlayer += new uscPlayerPunishPanel.PunishPlayerDelegate(kbpPunkbusterPunishPanel_PunishPlayer);
            this.kbpPunkbusterPunishPanel.Initialize(uscConnectionPanel);
            this.kbpBfbcPunishPanel.PunishPlayer += new uscPlayerPunishPanel.PunishPlayerDelegate(kbpBfbcPunishPanel_PunishPlayer);
            this.kbpBfbcPunishPanel.Initialize(uscConnectionPanel);
            
            this.lsvTeamOnePlayers.SmallImageList = this.m_frmMain.iglFlags;
            this.lsvTeamTwoPlayers.SmallImageList = this.m_frmMain.iglFlags;
            this.lsvTeamThreePlayers.SmallImageList = this.m_frmMain.iglFlags;
            this.lsvTeamFourPlayers.SmallImageList = this.m_frmMain.iglFlags;
            this.lsvTeamOnePlayers.ListViewItemSorter = this.m_lvwColumnSorter;
            this.lsvTeamTwoPlayers.ListViewItemSorter = this.m_lvwColumnSorter;
            this.lsvTeamThreePlayers.ListViewItemSorter = this.m_lvwColumnSorter;
            this.lsvTeamFourPlayers.ListViewItemSorter = this.m_lvwColumnSorter;

            this.btnCloseAdditionalInfo.ImageList = this.m_frmMain.iglIcons;
            this.btnCloseAdditionalInfo.ImageKey = "cross.png";

            this.btnSplitTeams.ImageList = this.m_frmMain.iglIcons;
            this.btnSplitTeams.ImageKey = "application_tile_horizontal.png";
        }

        // If we disconnect clear the player list so it's fresh on reconnection.
        private void m_prcClient_ConnectionClosed(PRoConClient sender) {
            foreach (KeyValuePair<string, ListViewItem> kvpPlayer in this.m_dicPlayers) {
                kvpPlayer.Value.Remove();
            }

            this.m_dicPlayers.Clear();
        }

        //public void OnConnectionClosed() {

        //}

        internal class AdditionalPlayerInfo {
            public CPunkbusterInfo m_pbInfo;
            //public string m_strCountryName;
            public string m_strResolvedHostName;
            public CPlayerInfo m_cpiPlayer;
            public Inventory m_spawnedInventory;

            public Dictionary<Kits, int> KitCounter {
                get;
                private set;
            }

            public AdditionalPlayerInfo() {
                this.KitCounter = new Dictionary<Kits, int>();
            }

            public void AddKitCount(Kits kit) {

                if (this.KitCounter.ContainsKey(kit) == true) {
                    this.KitCounter[kit] = this.KitCounter[kit] + 1;
                }
                else {
                    this.KitCounter.Add(kit, 1);
                }

            }
        }

        public void PlayerSelectionChange(string strSoldierName) {
            this.SelectPlayer(strSoldierName);
        }

        private void kbpBfbcPunishPanel_PunishPlayer(List<string> lstWords) {
            this.m_prcClient.SendRequest(lstWords);

            this.m_prcClient.Game.SendBanListSavePacket();
            this.m_prcClient.Game.SendBanListListPacket();
        }

        private void kbpPunkbusterPunishPanel_PunishPlayer(List<string> lstWords) {
            this.m_prcClient.SendRequest(lstWords);
        }

        public void SetConnection(PRoConClient prcClient) {
            if ((this.m_prcClient = prcClient) != null) {
                if (this.m_prcClient.Game != null) {
                    this.m_prcClient_GameTypeDiscovered(prcClient);
                }
                else {
                    this.m_prcClient.GameTypeDiscovered += new PRoConClient.EmptyParamterHandler(m_prcClient_GameTypeDiscovered);
                }
            }
        }

        private void m_prcClient_GameTypeDiscovered(PRoConClient sender) {

            this.m_prcClient.Game.ListPlayers += new FrostbiteClient.ListPlayersHandler(m_prcClient_ListPlayers);
            this.m_prcClient.Game.PlayerJoin += new FrostbiteClient.PlayerEventHandler(m_prcClient_PlayerJoin);
            this.m_prcClient.Game.PlayerLeft += new FrostbiteClient.PlayerLeaveHandler(m_prcClient_PlayerLeft);
            this.m_prcClient.PunkbusterPlayerInfo += new PRoConClient.PunkbusterPlayerInfoHandler(m_prcClient_PunkbusterPlayerInfo);
            this.m_prcClient.PlayerKilled += new PRoConClient.PlayerKilledHandler(m_prcClient_PlayerKilled);

            this.m_prcClient.Game.PlayerChangedTeam += new FrostbiteClient.PlayerTeamChangeHandler(m_prcClient_PlayerChangedTeam);
            this.m_prcClient.Game.PlayerChangedSquad += new FrostbiteClient.PlayerTeamChangeHandler(m_prcClient_PlayerChangedSquad);

            this.m_prcClient.ProconPrivileges += new PRoConClient.ProconPrivilegesHandler(m_prcClient_ProconPrivileges);

            this.m_prcClient.ConnectionClosed += new PRoConClient.EmptyParamterHandler(m_prcClient_ConnectionClosed);

            this.m_prcClient.Game.LevelStarted += new FrostbiteClient.EmptyParamterHandler(m_prcClient_LevelStarted);

            this.kbpPunkbusterPunishPanel.SetConnection(this.m_prcClient);
            this.kbpBfbcPunishPanel.SetConnection(this.m_prcClient);

            this.m_prcClient.Reasons.ItemAdded += new NotificationList<string>.ItemModifiedHandler(Reasons_ItemAdded);
            this.m_prcClient.Reasons.ItemRemoved += new NotificationList<string>.ItemModifiedHandler(Reasons_ItemRemoved);

            this.m_prcClient.PlayerListSettings.SplitTypeChanged += new PlayerListSettings.IndexChangedHandler(PlayerListSettings_SplitTypeChanged);
            this.m_prcClient.PlayerListSettings.TwoSplitterPercentageChanged += new PlayerListSettings.PercentageChangedHandler(PlayerListSettings_TwoSplitterPercentageChanged);
            this.m_prcClient.PlayerListSettings.FourSplitterPercentageChanged += new PlayerListSettings.PercentageChangedHandler(PlayerListSettings_FourSplitterPercentageChanged);

            this.m_prcClient.PlayerSpawned += new PRoConClient.PlayerSpawnedHandler(m_prcClient_PlayerSpawned);

            foreach (string strReason in this.m_prcClient.Reasons) {
                this.Reasons_ItemAdded(0, strReason);
            }

            this.m_prcClient.PlayerListSettings.SplitType = this.m_prcClient.PlayerListSettings.SplitType;

            this.m_prcClient_ListPlayers(this.m_prcClient.Game, new List<CPlayerInfo>(this.m_prcClient.PlayerList), new CPlayerSubset(CPlayerSubset.PlayerSubsetType.All));

            if (sender.Game.HasSquads == false) {
                this.lsvTeamOnePlayers.Columns.Remove(this.colSquad1);
                this.lsvTeamTwoPlayers.Columns.Remove(this.colSquad2);
                this.lsvTeamThreePlayers.Columns.Remove(this.colSquad3);
                this.lsvTeamFourPlayers.Columns.Remove(this.colSquad4);
                
                //this.colSquad1.Text = this.colSquad2.Text = this.colSquad3.Text = this.colSquad4.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lsvPlayers.colSquad", null);

            }

            this.SetSplitterDistances();
        }

        private void PlayerListSettings_SplitTypeChanged(int index) {
            if (index == 1) {
                this.btnSplitTeams.ImageKey = "application_tile_horizontal.png";

                this.spltTwoSplit.Panel2Collapsed = true;
                this.spltFourSplit.Panel2Collapsed = true;
            }
            else if (index == 2) {
                this.btnSplitTeams.ImageKey = "application_tile.png";

                this.spltTwoSplit.Panel2Collapsed = false;
                this.spltFourSplit.Panel2Collapsed = true;
            }
            else if (index == 4) {
                this.btnSplitTeams.ImageKey = "application.png";

                this.spltTwoSplit.Panel2Collapsed = false;
                this.spltFourSplit.Panel2Collapsed = false;
            }
        }

        private void Reasons_ItemRemoved(int iIndex, string item) {
            if (this.kbpBfbcPunishPanel.Reasons.Contains(item) == true) {
                this.kbpBfbcPunishPanel.Reasons.Remove(item);
            }

            if (this.kbpPunkbusterPunishPanel.Reasons.Contains(item) == true) {
                this.kbpPunkbusterPunishPanel.Reasons.Remove(item);
            }
        }

        private void Reasons_ItemAdded(int iIndex, string item) {
            this.kbpBfbcPunishPanel.Reasons.Add(item);
            this.kbpPunkbusterPunishPanel.Reasons.Add(item);
        }

        private void m_prcClient_ProconPrivileges(PRoConClient sender, CPrivileges spPrivs) {
            this.m_spPrivileges = spPrivs;

            this.kbpPunkbusterPunishPanel.Enabled = (!this.m_spPrivileges.CannotPunishPlayers && this.m_spPrivileges.CanIssueLimitedPunkbusterCommands);
            this.kbpPunkbusterPunishPanel.SetPrivileges(this.m_spPrivileges);

            this.kbpBfbcPunishPanel.Enabled = !this.m_spPrivileges.CannotPunishPlayers;
            this.kbpBfbcPunishPanel.SetPrivileges(this.m_spPrivileges);
        }

        public void SetLocalization(CLocalization clocLanguage) {
            this.m_clocLanguage = clocLanguage;

            this.colSlotID1.Text = this.colSlotID2.Text = this.colSlotID3.Text = this.colSlotID4.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lsvPlayers.colSlotID", null);
            this.colTags1.Text = this.colTags2.Text = this.colTags3.Text = this.colTags4.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lsvPlayers.colTags", null);
            this.colPlayerName1.Text = this.colPlayerName2.Text = this.colPlayerName3.Text = this.colPlayerName4.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lsvPlayers.colPlayerName", null);
            this.colSquad1.Text = this.colSquad2.Text = this.colSquad3.Text = this.colSquad4.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lsvPlayers.colSquad", null);
            this.colKit1.Text = this.colKit2.Text = this.colKit3.Text = this.colKit4.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lsvPlayers.colKit", null);
            this.colKills1.Text = this.colKills2.Text = this.colKills3.Text = this.colKills4.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lsvPlayers.colKills", null);
            this.colDeaths1.Text = this.colDeaths2.Text = this.colDeaths3.Text = this.colDeaths4.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lsvPlayers.colDeaths", null);
            this.colKdr1.Text = this.colKdr2.Text = this.colKdr3.Text = this.colKdr4.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lsvPlayers.colKdr", null);
            this.colScore1.Text = this.colScore2.Text = this.colScore3.Text = this.colScore4.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lsvPlayers.colScore", null);
            this.colPing1.Text = this.colPing2.Text = this.colPing3.Text = this.colPing4.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lsvPlayers.colPing", null);
            
            this.btnPlayerListSelectedCheese.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.btnPlayerListSelectedCheese", null);

            this.chkPlayerListShowTeams.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.chkPlayerListShowTeams", null);

            this.tabCourtMartialBFBC.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.tabCourtMartialBFBC", null);
            this.tabCourtMartialPunkbuster.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.tabCourtMartialPunkbuster", null);

            this.lblInventory.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lblInventory") + ":";

            // Player Context Menu
            this.textChatModerationToolStripMenuItem.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.ctxPlayerOptions.textChatModerationToolStripMenuItem");
            this.reservedSlotToolStripMenuItem.Text = this.m_clocLanguage.GetLocalized("uscPlayerListPanel.ctxPlayerOptions.reservedSlotToolStripMenuItem");

            this.statsLookupToolStripMenuItem.Text = this.m_clocLanguage.GetDefaultLocalized("Stats Lookup", "uscPlayerListPanel.ctxPlayerOptions.statsLookupToolStripMenuItem");
            
            this.kbpBfbcPunishPanel.SetLocalization(this.m_clocLanguage);
            this.kbpPunkbusterPunishPanel.SetLocalization(this.m_clocLanguage);
        }

        //private CServerInfo m_csiLastServerInfo = null;

        /*
        private string GetTeamName(int iTeamID) {

            string strReturnTeamName = String.Empty;

            if (this.m_csiLastServerInfo != null) {
                
                strReturnTeamName = this.m_prcClient.GetLocalizedTeamName(iTeamID, this.m_prcClient.CurrentServerInfo.Map);

            }

            return strReturnTeamName;
        }
        */
        /*
        private int GetTotalTeamsForMap() {

            int iTotalTeams = this.m_prcClient.GetLocalizedTeamNameCount(this.m_prcClient.CurrentServerInfo.Map);
            
            return iTotalTeams;
        }
        */
          
        //public void OnServerInfo(CServerInfo csiInfo) {
        //    this.m_csiLastServerInfo = csiInfo;
        //}
        
        private readonly object m_objPlayerDictionaryLocker = new object();
        private Dictionary<string, ListViewItem> m_dicPlayers = new Dictionary<string, ListViewItem>();
        //private bool m_blSplitList = false;
        //private int m_iSplitPlayerLists = 1;

        private ListViewItem CreateTotalsPlayer(CPlayerInfo cpiDummyPlayer, int iTeamID) {
            ListViewItem lviReturn = this.CreatePlayer(new CPlayerInfo(cpiDummyPlayer.SoldierName, String.Empty, iTeamID, 0));
            lviReturn.Name = cpiDummyPlayer.ClanTag;
            lviReturn.Font = new Font(this.Font, FontStyle.Bold);

            return lviReturn;
        }

        private ListViewItem CreatePlayer(CPlayerInfo cpiPlayer) {
            ListViewItem lviNewPlayer = new ListViewItem("");
            lviNewPlayer.Name = cpiPlayer.SoldierName;
            lviNewPlayer.Tag = null;
            lviNewPlayer.UseItemStyleForSubItems = true;

            AdditionalPlayerInfo sapiAdditional = new AdditionalPlayerInfo();
            sapiAdditional.m_cpiPlayer = cpiPlayer;
            sapiAdditional.m_strResolvedHostName = String.Empty;
            lviNewPlayer.Tag = sapiAdditional;

            ListViewItem.ListViewSubItem lviTags = new ListViewItem.ListViewSubItem();
            lviTags.Name = "tags";
            lviTags.Text = cpiPlayer.ClanTag;
            lviNewPlayer.SubItems.Add(lviTags);

            ListViewItem.ListViewSubItem lviTagsName = new ListViewItem.ListViewSubItem();
            lviTagsName.Name = "soldiername";
            lviTagsName.Text = cpiPlayer.SoldierName;
            lviNewPlayer.SubItems.Add(lviTagsName);

            if (this.m_prcClient != null && this.m_prcClient.Game != null && this.m_prcClient.Game.HasSquads == true) {
                ListViewItem.ListViewSubItem lviSquad = new ListViewItem.ListViewSubItem();
                lviSquad.Name = "squad";
                if (cpiPlayer.SquadID != uscPlayerListPanel.INT_NEUTRAL_SQUAD) {
                    lviSquad.Text = this.m_clocLanguage.GetLocalized("global.Squad" + cpiPlayer.SquadID.ToString(), null);
                }
                lviNewPlayer.SubItems.Add(lviSquad);
            }

            ListViewItem.ListViewSubItem lviKit = new ListViewItem.ListViewSubItem();
            lviKit.Name = "kit";
            lviKit.Text = String.Empty;
            lviNewPlayer.SubItems.Add(lviKit);

            ListViewItem.ListViewSubItem lviScore = new ListViewItem.ListViewSubItem();
            lviScore.Name = "score";
            lviScore.Text = cpiPlayer.Score.ToString();
            lviNewPlayer.SubItems.Add(lviScore);

            ListViewItem.ListViewSubItem lviKills = new ListViewItem.ListViewSubItem();
            lviKills.Name = "kills";
            lviKills.Tag = (Double)cpiPlayer.Kills;
            lviKills.Text = cpiPlayer.Kills.ToString();
            lviNewPlayer.SubItems.Add(lviKills);

            ListViewItem.ListViewSubItem lviDeaths = new ListViewItem.ListViewSubItem();
            lviDeaths.Name = "deaths";
            lviDeaths.Tag = (Double)cpiPlayer.Deaths;
            lviDeaths.Text = cpiPlayer.Deaths.ToString();
            lviNewPlayer.SubItems.Add(lviDeaths);

            ListViewItem.ListViewSubItem lviKDr = new ListViewItem.ListViewSubItem();
            lviKDr.Name = "kdr";
            lviKDr.Text = cpiPlayer.Deaths > 0 ? String.Format("{0:0.00}", (Double)cpiPlayer.Kills / (Double)cpiPlayer.Deaths) : String.Format("{0:0.00}", (Double)cpiPlayer.Kills);
            lviNewPlayer.SubItems.Add(lviKDr);

            ListViewItem.ListViewSubItem lviPing = new ListViewItem.ListViewSubItem();
            lviPing.Name = "ping";
            lviPing.Text = cpiPlayer.Ping.ToString();
            lviNewPlayer.SubItems.Add(lviPing);

            return lviNewPlayer;
        }

        private int GetPlayerTeamID(ListViewItem lviPlayer) {

            int iReturnTeamID = 0;

            if (lviPlayer.Tag != null && ((AdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer != null) {
                iReturnTeamID = ((AdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.TeamID;
            }

            return iReturnTeamID;
        }

        private int GetTotalPlayersByTeamID(int iTeamID) {
            int iTotalPlayers = 0;

            foreach (KeyValuePair<string, ListViewItem> kvpPlayer in this.m_dicPlayers) {
                if (this.GetPlayerTeamID(kvpPlayer.Value) == iTeamID) {
                    iTotalPlayers++;
                }
            }

            // - 2 to account for the totals.
            return iTotalPlayers - 2;
        }

        private void SetPlayerTeamID(ListViewItem lviPlayer, int iTeamID) {
            if (lviPlayer.Tag != null && ((AdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer != null) {

                //if (((SAdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.TeamID != iTeamID && iTeamID != uscPlayerListPanel.INT_NEUTRAL_TEAM) {
                //    this.m_uscConnectionPanel.ThrowEvent(this, uscEventsPanel.CapturableEvents.PlayerSwitchedTeams, new string[] { ((SAdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.SoldierName, this.GetTeamName(((SAdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.TeamID), this.GetTeamName(iTeamID) });
                //}

                ((AdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.TeamID = iTeamID;
            }
        }

        private void SetPlayerSquadID(ListViewItem lviPlayer, int iSquadID) {
            if (lviPlayer.Tag != null && ((AdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer != null) {

                /*
                if (((SAdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.SquadID != iSquadID) {
                    if (iSquadID != uscPlayerListPanel.INT_NEUTRAL_SQUAD) {

                        if (((SAdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.SquadID != uscPlayerListPanel.INT_NEUTRAL_SQUAD) {
                            this.m_uscConnectionPanel.ThrowEvent(this, uscEventsPanel.CapturableEvents.PlayerSwitchedSquads, new string[] { ((SAdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.SoldierName, this.m_clocLanguage.GetLocalized("global.Squad" + ((SAdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.SquadID.ToString(), null), this.m_clocLanguage.GetLocalized("global.Squad" + iSquadID.ToString(), null) });
                        }
                        else {
                            // TO DO: Localize None
                            this.m_uscConnectionPanel.ThrowEvent(this, uscEventsPanel.CapturableEvents.PlayerSwitchedSquads, new string[] { ((SAdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.SoldierName, "None", this.m_clocLanguage.GetLocalized("global.Squad" + iSquadID.ToString(), null) });
                        }
                    }
                    else {
                        this.m_uscConnectionPanel.ThrowEvent(this, uscEventsPanel.CapturableEvents.PlayerSwitchedSquads, new string[] { ((SAdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.SoldierName, this.m_clocLanguage.GetLocalized("global.Squad" + ((SAdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.SquadID.ToString(), null), "None" });
                    }
                }
                */

                if (this.m_prcClient != null && this.m_prcClient.Game != null && this.m_prcClient.Game.HasSquads == true) {
                    ((AdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.SquadID = iSquadID;
                    if (iSquadID != uscPlayerListPanel.INT_NEUTRAL_SQUAD) {
                        lviPlayer.SubItems["squad"].Text = this.m_clocLanguage.GetLocalized("global.Squad" + ((AdditionalPlayerInfo)lviPlayer.Tag).m_cpiPlayer.SquadID.ToString(), null);
                    }
                    else {
                        lviPlayer.SubItems["squad"].Text = String.Empty;
                    }
                }
            }
        }


        private void UpdateTeamNames() {
            // All four lists have the same number of groups in them..
            for (int i = 0; i < uscPlayerListPanel.INT_MAX_TEAMS; i++) {

                string score = String.Empty;

                if (this.m_prcClient != null && this.m_prcClient.CurrentServerInfo != null && this.m_prcClient.CurrentServerInfo.TeamScores != null) {

                    if (i > 0 && this.m_prcClient.CurrentServerInfo.TeamScores.Count > i - 1) {
                        score = String.Format(" - {0} {1}", this.m_prcClient.CurrentServerInfo.TeamScores[i - 1].Score, this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lsvPlayers.Groups.Tickets"));
                    }
                }

                this.lsvTeamOnePlayers.Groups[i].Header = String.Format("{1} - {0}{2}", this.m_prcClient.GetLocalizedTeamName(i, this.m_prcClient.CurrentServerInfo.Map), this.lsvTeamOnePlayers.Groups[i].Items.Count - 2, score);
                this.lsvTeamTwoPlayers.Groups[i].Header = String.Format("{1} - {0}{2}", this.m_prcClient.GetLocalizedTeamName(i, this.m_prcClient.CurrentServerInfo.Map), this.lsvTeamTwoPlayers.Groups[i].Items.Count - 2, score);
                this.lsvTeamThreePlayers.Groups[i].Header = String.Format("{1} - {0}{2}", this.m_prcClient.GetLocalizedTeamName(i, this.m_prcClient.CurrentServerInfo.Map), this.lsvTeamThreePlayers.Groups[i].Items.Count - 2, score);
                this.lsvTeamFourPlayers.Groups[i].Header = String.Format("{1} - {0}{2}", this.m_prcClient.GetLocalizedTeamName(i, this.m_prcClient.CurrentServerInfo.Map), this.lsvTeamFourPlayers.Groups[i].Items.Count - 2, score);
            }
        }

        private void SetTotalsZero(int iTeamID) {
            if (this.m_dicPlayers.ContainsKey(String.Format("procon.playerlist.totals{0}", iTeamID)) == true) {

                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).KitCounter.Clear();
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kills = 0;
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Deaths = 0;
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Score = 0;
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Ping = 0;
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.SquadID = 0;
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kdr = 0.0F;

                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["kit"].Text = String.Empty;
                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["kills"].Text = "0";
                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["deaths"].Text = "0";
                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["score"].Text = "0";
                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["ping"].Text = String.Empty;
                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["kdr"].Text = "0.00";

                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["kit"].Text = String.Empty;
                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["kills"].Text = "0.00";
                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["deaths"].Text = "0.00";
                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["score"].Text = "0.00";
                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["ping"].Text = "0.00";
                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["kdr"].Text = "0.00";
            }
        }

        private void AddTotalsPlayerDetails(int iTeamID, AdditionalPlayerInfo player) {
            if (this.m_dicPlayers.ContainsKey(String.Format("procon.playerlist.totals{0}", iTeamID)) == true) {

                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kills += player.m_cpiPlayer.Kills;
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Deaths += player.m_cpiPlayer.Deaths;
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Score += player.m_cpiPlayer.Score; ;
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Ping += player.m_cpiPlayer.Ping;
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kdr += (player.m_cpiPlayer.Deaths > 0 ? (float)player.m_cpiPlayer.Kills / (float)player.m_cpiPlayer.Deaths : player.m_cpiPlayer.Kills);
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.SquadID++;

                if (player.m_spawnedInventory != null) {
                    ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).AddKitCount(player.m_spawnedInventory.Kit);
                }

                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["kills"].Text = ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kills.ToString();
                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["deaths"].Text = ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Deaths.ToString();
                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["score"].Text = ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Score.ToString();
                //this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["ping"].Text = ((SAdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Ping.ToString();
                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["kdr"].Text = String.Format("{0:0.00}", ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kdr);

                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["kills"].Text = String.Format("{0:0.00}", (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kills / (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.SquadID);
                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["deaths"].Text = String.Format("{0:0.00}", (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Deaths / (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.SquadID);
                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["score"].Text = String.Format("{0:0.00}", (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Score / (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.SquadID);
                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["ping"].Text = String.Format("{0:0}", (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Ping / (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.SquadID);
                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["kdr"].Text = String.Format("{0:0.00}", ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kdr / (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.SquadID);

                int mostUsedKitCount = 0;
                Kits mostUsedKit = Kits.None;
                List<string> kitTotals = new List<string>();

                foreach (KeyValuePair<Kits, int> kitCount in ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).KitCounter) {
                    if (kitCount.Value > mostUsedKitCount) {
                        mostUsedKitCount = kitCount.Value;
                        mostUsedKit = kitCount.Key;
                    }

                    kitTotals.Add(String.Format("{0}{1}", kitCount.Value, this.m_clocLanguage.GetLocalized(String.Format("global.Kits.{0}.Short", kitCount.Key.ToString()))));
                }

                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["kit"].Text = String.Join(",", kitTotals.ToArray());

                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["kit"].Text = this.m_clocLanguage.GetLocalized(String.Format("global.Kits.{0}", mostUsedKit.ToString()));

                //this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["kills"].Tag = (int)(this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["kills"].Tag) + cpiPlayer.Kills;
                //this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["deaths"].Tag = (int)(this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["deaths"].Tag) + cpiPlayer.Deaths;
                //this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["score"].Tag = (int)(this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["score"].Tag) + cpiPlayer.Score;
                //this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["ping"].Tag = (int)(this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["ping"].Tag) + cpiPlayer.Ping;
            }
        }

        /*
        private void AddTotalsPlayerDetails(int iTeamID, CPlayerInfo cpiPlayer) {
            if (this.m_dicPlayers.ContainsKey(String.Format("procon.playerlist.totals{0}", iTeamID)) == true) {

                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kills += cpiPlayer.Kills;
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Deaths += cpiPlayer.Deaths;
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Score += cpiPlayer.Score; ;
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Ping += cpiPlayer.Ping;
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kdr += (cpiPlayer.Deaths > 0 ? (float)cpiPlayer.Kills / (float)cpiPlayer.Deaths : cpiPlayer.Kills);
                ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.SquadID++;

                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["kills"].Text = ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kills.ToString();
                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["deaths"].Text = ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Deaths.ToString();
                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["score"].Text = ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Score.ToString();
                //this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["ping"].Text = ((SAdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Ping.ToString();
                this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["kdr"].Text = String.Format("{0:0.00}", ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kdr);

                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["kills"].Text = String.Format("{0:0.00}", (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kills / (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.SquadID);
                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["deaths"].Text = String.Format("{0:0.00}", (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Deaths / (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.SquadID);
                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["score"].Text = String.Format("{0:0.00}", (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Score / (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.SquadID);
                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["ping"].Text = String.Format("{0:0}", (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Ping / (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.SquadID);
                this.m_dicPlayers[String.Format("procon.playerlist.averages{0}", iTeamID)].SubItems["kdr"].Text = String.Format("{0:0.00}", ((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.Kdr / (float)((AdditionalPlayerInfo)this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].Tag).m_cpiPlayer.SquadID);



                //this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["kills"].Tag = (int)(this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["kills"].Tag) + cpiPlayer.Kills;
                //this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["deaths"].Tag = (int)(this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["deaths"].Tag) + cpiPlayer.Deaths;
                //this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["score"].Tag = (int)(this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["score"].Tag) + cpiPlayer.Score;
                //this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["ping"].Tag = (int)(this.m_dicPlayers[String.Format("procon.playerlist.totals{0}", iTeamID)].SubItems["ping"].Tag) + cpiPlayer.Ping;
            }
        }
        */

        // Simply puts the players into the correct list.
        private void ArrangePlayers() {

            this.m_blPropogatingIndexChange = true;
            if (this.m_prcClient != null) {

                this.SetTotalsZero(1);
                this.SetTotalsZero(2);
                this.SetTotalsZero(3);
                this.SetTotalsZero(4);

                if (this.m_prcClient.PlayerListSettings.SplitType == 1) {
                    this.lsvTeamOnePlayers.BeginUpdate();

                    foreach (KeyValuePair<string, ListViewItem> kvpPlayer in this.m_dicPlayers) {

                        int iPlayerTeamID = this.GetPlayerTeamID(kvpPlayer.Value);
                        bool isTotalsPlayer = this.m_lvwColumnSorter.TotalsAveragesChecker.IsMatch(kvpPlayer.Key);

                        if (isTotalsPlayer == false) {
                            this.AddTotalsPlayerDetails(iPlayerTeamID, (AdditionalPlayerInfo)kvpPlayer.Value.Tag);
                        }

                        if (isTotalsPlayer == false || this.GetTotalPlayersByTeamID(iPlayerTeamID) > 0) {

                            if (this.lsvTeamOnePlayers.Items.ContainsKey(kvpPlayer.Key) == false) {
                                kvpPlayer.Value.Remove();

                                kvpPlayer.Value.Group = this.lsvTeamOnePlayers.Groups[iPlayerTeamID];
                                this.lsvTeamOnePlayers.Items.Add(kvpPlayer.Value);
                            }
                            else {
                                kvpPlayer.Value.Group = this.lsvTeamOnePlayers.Groups[iPlayerTeamID];
                            }

                        }
                    }

                    this.lsvTeamOnePlayers.EndUpdate();
                }
                else if (this.m_prcClient.PlayerListSettings.SplitType == 2) {
                    this.lsvTeamOnePlayers.BeginUpdate();
                    this.lsvTeamTwoPlayers.BeginUpdate();

                    foreach (KeyValuePair<string, ListViewItem> kvpPlayer in this.m_dicPlayers) {
                        int iTeamID = this.GetPlayerTeamID(kvpPlayer.Value);
                        bool isTotalsPlayer = this.m_lvwColumnSorter.TotalsAveragesChecker.IsMatch(kvpPlayer.Key); ;

                        if (isTotalsPlayer == false) {
                            this.AddTotalsPlayerDetails(iTeamID, (AdditionalPlayerInfo)kvpPlayer.Value.Tag);
                        }

                        if (isTotalsPlayer == false || this.GetTotalPlayersByTeamID(iTeamID) > 0) {

                            if (this.lsvTeamOnePlayers.Items.ContainsKey(kvpPlayer.Key) == false && iTeamID == 1 || iTeamID == 3) {
                                kvpPlayer.Value.Remove();

                                kvpPlayer.Value.Group = this.lsvTeamOnePlayers.Groups[iTeamID];
                                this.lsvTeamOnePlayers.Items.Add(kvpPlayer.Value);
                            }
                            else if (this.lsvTeamOnePlayers.Items.ContainsKey(kvpPlayer.Key) == true && iTeamID == 1 || iTeamID == 3) {
                                kvpPlayer.Value.Group = this.lsvTeamOnePlayers.Groups[iTeamID];
                            }
                            else if (this.lsvTeamTwoPlayers.Items.ContainsKey(kvpPlayer.Key) == false && iTeamID == 2 || iTeamID == 4) {
                                kvpPlayer.Value.Remove();

                                kvpPlayer.Value.Group = this.lsvTeamTwoPlayers.Groups[iTeamID];
                                this.lsvTeamTwoPlayers.Items.Add(kvpPlayer.Value);
                            }
                            else if (this.lsvTeamOnePlayers.Items.ContainsKey(kvpPlayer.Key) == true && iTeamID == 2 || iTeamID == 4) {
                                kvpPlayer.Value.Group = this.lsvTeamOnePlayers.Groups[iTeamID];
                            }
                        }
                    }

                    this.lsvTeamTwoPlayers.EndUpdate();
                    this.lsvTeamOnePlayers.EndUpdate();
                }
                else if (this.m_prcClient.PlayerListSettings.SplitType == 4) {
                    this.lsvTeamOnePlayers.BeginUpdate();
                    this.lsvTeamTwoPlayers.BeginUpdate();
                    this.lsvTeamThreePlayers.BeginUpdate();
                    this.lsvTeamFourPlayers.BeginUpdate();

                    foreach (KeyValuePair<string, ListViewItem> kvpPlayer in this.m_dicPlayers) {
                        int iTeamID = this.GetPlayerTeamID(kvpPlayer.Value);
                        bool isTotalsPlayer = this.m_lvwColumnSorter.TotalsAveragesChecker.IsMatch(kvpPlayer.Key);

                        if (isTotalsPlayer == false) {
                            this.AddTotalsPlayerDetails(iTeamID, (AdditionalPlayerInfo)kvpPlayer.Value.Tag);
                        }

                        if (isTotalsPlayer == false || this.GetTotalPlayersByTeamID(iTeamID) > 0) {

                            if (this.lsvTeamOnePlayers.Items.ContainsKey(kvpPlayer.Key) == false && iTeamID == 1) {
                                kvpPlayer.Value.Remove();

                                kvpPlayer.Value.Group = this.lsvTeamOnePlayers.Groups[iTeamID];
                                this.lsvTeamOnePlayers.Items.Add(kvpPlayer.Value);
                            }
                            else if (this.lsvTeamTwoPlayers.Items.ContainsKey(kvpPlayer.Key) == false && iTeamID == 2) {
                                kvpPlayer.Value.Remove();

                                kvpPlayer.Value.Group = this.lsvTeamTwoPlayers.Groups[iTeamID];
                                this.lsvTeamTwoPlayers.Items.Add(kvpPlayer.Value);
                            }
                            else if (this.lsvTeamThreePlayers.Items.ContainsKey(kvpPlayer.Key) == false && iTeamID == 3) {
                                kvpPlayer.Value.Remove();

                                kvpPlayer.Value.Group = this.lsvTeamThreePlayers.Groups[iTeamID];
                                this.lsvTeamThreePlayers.Items.Add(kvpPlayer.Value);
                            }
                            else if (this.lsvTeamFourPlayers.Items.ContainsKey(kvpPlayer.Key) == false && iTeamID == 4) {
                                kvpPlayer.Value.Remove();

                                kvpPlayer.Value.Group = this.lsvTeamFourPlayers.Groups[iTeamID];
                                this.lsvTeamFourPlayers.Items.Add(kvpPlayer.Value);
                            }
                        }
                    }

                    this.lsvTeamFourPlayers.EndUpdate();
                    this.lsvTeamThreePlayers.EndUpdate();
                    this.lsvTeamTwoPlayers.EndUpdate();
                    this.lsvTeamOnePlayers.EndUpdate();

                    GC.Collect();
                }

                this.UpdateTeamNames();

                // All three lists have the same number of columns in them..
                for (int i = 0; i < this.lsvTeamOnePlayers.Columns.Count; i++) {
                    this.lsvTeamOnePlayers.Columns[i].Width = -2;
                    this.lsvTeamTwoPlayers.Columns[i].Width = -2;
                    this.lsvTeamThreePlayers.Columns[i].Width = -2;
                    this.lsvTeamFourPlayers.Columns[i].Width = -2;
                }

                this.lsvTeamOnePlayers.Sort();
                this.lsvTeamTwoPlayers.Sort();
                this.lsvTeamThreePlayers.Sort();
                this.lsvTeamFourPlayers.Sort();
            }
            this.m_blPropogatingIndexChange = false;
        }

        private void m_prcClient_PlayerLeft(FrostbiteClient sender, string playerName, CPlayerInfo cpiPlayer) {
            if (this.m_dicPlayers.ContainsKey(playerName) == true) {
                this.m_dicPlayers[playerName].Remove();
                this.m_dicPlayers.Remove(playerName);
            }

            this.UpdateTeamNames();

            this.RefreshSelectedPlayer();
        }

        private void m_prcClient_PlayerJoin(FrostbiteClient sender, string playerName) {
            if (this.m_dicPlayers.ContainsKey(playerName) == false) {
                this.m_dicPlayers.Add(playerName, this.CreatePlayer(new CPlayerInfo(playerName, String.Empty, 0, 0)));

                this.ArrangePlayers();
            }
        }

        /*
        public void OnPlayerJoin(string strSoldierName) {

        }

        public void OnPlayerLeave(string strSoldierName) {


        }
         * 
        */

        private void m_prcClient_PlayerSpawned(PRoConClient sender, string soldierName, Inventory spawnedInventory) {

            this.m_blPropogatingIndexChange = true;

            if (this.m_dicPlayers.ContainsKey(soldierName) == true) {
                AdditionalPlayerInfo sapiAdditional;

                if (this.m_dicPlayers[soldierName].Tag != null) {
                    sapiAdditional = (AdditionalPlayerInfo)this.m_dicPlayers[soldierName].Tag;

                    sapiAdditional.m_spawnedInventory = spawnedInventory;

                    if (this.m_dicPlayers.ContainsKey(soldierName) == true) {
                        this.m_dicPlayers[soldierName].SubItems["kit"].Text = this.m_clocLanguage.GetLocalized(String.Format("global.Kits.{0}", spawnedInventory.Kit.ToString()));
                    }

                    if (sapiAdditional.m_pbInfo != null) {
                        if (this.m_frmMain.iglFlags.Images.ContainsKey(sapiAdditional.m_pbInfo.PlayerCountryCode + ".png") == true) {
                            this.m_dicPlayers[sapiAdditional.m_pbInfo.SoldierName].ImageIndex = this.m_frmMain.iglFlags.Images.IndexOfKey(sapiAdditional.m_pbInfo.PlayerCountryCode + ".png");
                        }
                    }

                    this.m_dicPlayers[soldierName].Tag = sapiAdditional;
                }

                this.RefreshSelectedPlayer();
            }

            this.ArrangePlayers();

            this.m_blPropogatingIndexChange = false;
        }

        private void m_prcClient_PunkbusterPlayerInfo(PRoConClient sender, CPunkbusterInfo pbInfo) {
            this.m_blPropogatingIndexChange = true;

            if (this.m_dicPlayers.ContainsKey(pbInfo.SoldierName) == true) {

                AdditionalPlayerInfo sapiAdditional;

                if (this.m_dicPlayers[pbInfo.SoldierName].Tag == null) {
                    sapiAdditional = new AdditionalPlayerInfo();
                    sapiAdditional.m_strResolvedHostName = String.Empty;
                }
                else {
                    sapiAdditional = (AdditionalPlayerInfo)this.m_dicPlayers[pbInfo.SoldierName].Tag;
                }

                sapiAdditional.m_pbInfo = pbInfo;

                this.m_dicPlayers[pbInfo.SoldierName].Tag = sapiAdditional;

                this.m_dicPlayers[pbInfo.SoldierName].Text = pbInfo.SlotID;
                
                //string strCountryCode = this.m_frmMain.GetCountryCode(pbInfo.Ip);
                if (this.m_frmMain.iglFlags.Images.ContainsKey(pbInfo.PlayerCountryCode + ".png") == true && this.m_dicPlayers[sapiAdditional.m_pbInfo.SoldierName].ImageIndex < 0) {
                    this.m_dicPlayers[pbInfo.SoldierName].ImageIndex = this.m_frmMain.iglFlags.Images.IndexOfKey(pbInfo.PlayerCountryCode + ".png");
                }

                this.RefreshSelectedPlayer();
            }

            this.m_blPropogatingIndexChange = false;
        }

        /*
        public void OnPlayerPunkbusterInfo(CPunkbusterInfo pbInfo) {


        }
        */

        private void m_prcClient_ListPlayers(FrostbiteClient sender, List<CPlayerInfo> lstPlayers, CPlayerSubset cpsSubset) {
            if (cpsSubset.Subset == CPlayerSubset.PlayerSubsetType.All) {
                foreach (CPlayerInfo cpiPlayer in lstPlayers) {
                    if (this.m_dicPlayers.ContainsKey(cpiPlayer.SoldierName) == true) {

                        //if (this.m_blSplitList == false) {
                        //    this.m_dicPlayers[cpiPlayer.SoldierName].Group = this.lsvSinglePlayers.Groups[cpiPlayer.TeamId];
                        //}

                        if (this.m_prcClient != null && this.m_prcClient.Game != null && this.m_prcClient.Game.HasSquads == true) {
                            if (cpiPlayer.SquadID != uscPlayerListPanel.INT_NEUTRAL_SQUAD) {
                                this.m_dicPlayers[cpiPlayer.SoldierName].SubItems["squad"].Text = this.m_clocLanguage.GetLocalized("global.Squad" + cpiPlayer.SquadID.ToString(), null);
                            }
                            else {
                                this.m_dicPlayers[cpiPlayer.SoldierName].SubItems["squad"].Text = String.Empty;
                            }
                        }

                        this.m_dicPlayers[cpiPlayer.SoldierName].SubItems["tags"].Text = cpiPlayer.ClanTag;

                        this.m_dicPlayers[cpiPlayer.SoldierName].SubItems["score"].Text = cpiPlayer.Score.ToString();
                        this.m_dicPlayers[cpiPlayer.SoldierName].SubItems["kills"].Tag = (Double)cpiPlayer.Kills;
                        this.m_dicPlayers[cpiPlayer.SoldierName].SubItems["kills"].Text = cpiPlayer.Kills.ToString();

                        this.m_dicPlayers[cpiPlayer.SoldierName].SubItems["deaths"].Tag = (Double)cpiPlayer.Deaths;
                        this.m_dicPlayers[cpiPlayer.SoldierName].SubItems["deaths"].Text = cpiPlayer.Deaths.ToString();

                        this.m_dicPlayers[cpiPlayer.SoldierName].SubItems["kdr"].Text = cpiPlayer.Deaths > 0 ? String.Format("{0:0.00}", (Double)cpiPlayer.Kills / (Double)cpiPlayer.Deaths) : String.Format("{0:0.00}", (Double)cpiPlayer.Kills);

                        this.m_dicPlayers[cpiPlayer.SoldierName].SubItems["ping"].Text = cpiPlayer.Ping.ToString();

                        AdditionalPlayerInfo sapiAdditional;

                        if (this.m_dicPlayers[cpiPlayer.SoldierName].Tag == null) {
                            sapiAdditional = new AdditionalPlayerInfo();
                            sapiAdditional.m_strResolvedHostName = String.Empty;
                        }
                        else {
                            sapiAdditional = (AdditionalPlayerInfo)this.m_dicPlayers[cpiPlayer.SoldierName].Tag;
                        }

                        sapiAdditional.m_cpiPlayer = cpiPlayer;
                        this.m_dicPlayers[cpiPlayer.SoldierName].Tag = sapiAdditional;
                    }
                    else {
                        this.m_dicPlayers.Add(cpiPlayer.SoldierName, this.CreatePlayer(cpiPlayer));
                    }
                }

                List<string> lstKeys = new List<string>(this.m_dicPlayers.Keys);

                for (int i = 0; i < lstKeys.Count; i++) {
                    bool blFoundPlayer = false;

                    foreach (CPlayerInfo cpiPlayer in lstPlayers) {
                        if (String.Compare(cpiPlayer.SoldierName, this.m_dicPlayers[lstKeys[i]].Name) == 0) {
                            blFoundPlayer = true;
                            break;
                        }
                    }

                    if (blFoundPlayer == false) {
                        this.m_dicPlayers[lstKeys[i]].Remove();
                        this.m_dicPlayers.Remove(lstKeys[i]);
                    }
                }

                this.m_dicPlayers.Add("procon.playerlist.totals1", this.CreateTotalsPlayer(new CPlayerInfo("Totals", "procon.playerlist.totals1", 1, 0), 1));
                this.m_dicPlayers.Add("procon.playerlist.totals2", this.CreateTotalsPlayer(new CPlayerInfo("Totals", "procon.playerlist.totals2", 2, 0), 2));
                this.m_dicPlayers.Add("procon.playerlist.totals3", this.CreateTotalsPlayer(new CPlayerInfo("Totals", "procon.playerlist.totals3", 3, 0), 3));
                this.m_dicPlayers.Add("procon.playerlist.totals4", this.CreateTotalsPlayer(new CPlayerInfo("Totals", "procon.playerlist.totals4", 4, 0), 4));

                this.m_dicPlayers.Add("procon.playerlist.averages1", this.CreateTotalsPlayer(new CPlayerInfo("Averages", "procon.playerlist.averages1", 1, 0), 1));
                this.m_dicPlayers.Add("procon.playerlist.averages2", this.CreateTotalsPlayer(new CPlayerInfo("Averages", "procon.playerlist.averages2", 2, 0), 2));
                this.m_dicPlayers.Add("procon.playerlist.averages3", this.CreateTotalsPlayer(new CPlayerInfo("Averages", "procon.playerlist.averages3", 3, 0), 3));
                this.m_dicPlayers.Add("procon.playerlist.averages4", this.CreateTotalsPlayer(new CPlayerInfo("Averages", "procon.playerlist.averages4", 4, 0), 4));

                this.ArrangePlayers();
            }
        }

        /*
        public void OnPlayerList(List<CPlayerInfo> lstPlayers) {


        }
        */

        private void m_prcClient_LevelStarted(FrostbiteClient sender) {
            foreach (KeyValuePair<string, ListViewItem> kvpPlayer in this.m_dicPlayers) {
                kvpPlayer.Value.SubItems["score"].Text = String.Empty;
                kvpPlayer.Value.SubItems["kills"].Tag = new Double();
                kvpPlayer.Value.SubItems["kills"].Text = String.Empty;
                kvpPlayer.Value.SubItems["deaths"].Tag = new Double();
                kvpPlayer.Value.SubItems["deaths"].Text = String.Empty;
                kvpPlayer.Value.SubItems["kdr"].Text = String.Empty;
                kvpPlayer.Value.SubItems["kit"].Text = String.Empty;

                if (kvpPlayer.Value.ImageIndex >= 0) {
                    kvpPlayer.Value.ImageIndex = this.m_frmMain.iglFlags.Images.IndexOfKey("flag_death.png");
                }

                if (kvpPlayer.Value.Tag != null) {
                    ((AdditionalPlayerInfo)kvpPlayer.Value.Tag).m_spawnedInventory = null;
                }
            }
        }

        private void chkPlayerListShowTeams_CheckedChanged(object sender, EventArgs e) {
            this.lsvTeamOnePlayers.ShowGroups = this.lsvTeamTwoPlayers.ShowGroups = this.lsvTeamThreePlayers.ShowGroups = this.lsvTeamFourPlayers.ShowGroups = this.chkPlayerListShowTeams.Checked;

            this.ArrangePlayers();
        }

        private void btnCloseAdditionalInfo_Click(object sender, EventArgs e) {
            this.SelectNoPlayer();

            this.ClearPunishmentPanel();
        }

        private void uscPlayerListPanel_Resize(object sender, EventArgs e) {

            this.lsvTeamOnePlayers.Scrollable = false;
            this.lsvTeamTwoPlayers.Scrollable = false;
            this.lsvTeamThreePlayers.Scrollable = false;
            this.lsvTeamFourPlayers.Scrollable = false;

            this.SetSplitterDistances();

            this.lsvTeamOnePlayers.Scrollable = true;
            this.lsvTeamTwoPlayers.Scrollable = true;
            this.lsvTeamThreePlayers.Scrollable = true;
            this.lsvTeamFourPlayers.Scrollable = true;
        }

        private void ClearPunishmentPanel() {

            if (this.m_blPropogatingIndexChange == false) {

                this.kbpBfbcPunishPanel.SoldierName = String.Empty;
                this.kbpPunkbusterPunishPanel.SoldierName = String.Empty;

                this.txtPlayerListSelectedIP.Text = String.Empty;
                this.lblPlayerListSelectedName.Text = String.Empty;
                this.txtPlayerListSelectedGUID.Text = String.Empty;
                this.txtPlayerListSelectedBc2GUID.Text = String.Empty;

                this.pnlAdditionalInfo.Enabled = false;
                this.tbcCourtMartial.Enabled = false;

                this.spltListAdditionalInfo.Panel2Collapsed = true;
                this.btnCloseAdditionalInfo.Enabled = false;
            }
        }

        private bool m_blPropogatingIndexChange = false;
        private void SelectPlayer(string strPlayerName) {

            this.m_blPropogatingIndexChange = true;

            foreach (ListViewItem lviPlayer in this.lsvTeamOnePlayers.Items) {
                if (String.Compare(lviPlayer.Name, strPlayerName) == 0) {
                    lviPlayer.Selected = true;
                }
                else {
                    lviPlayer.Selected = false;
                }
            }

            foreach (ListViewItem lviPlayer in this.lsvTeamTwoPlayers.Items) {
                if (String.Compare(lviPlayer.Name, strPlayerName) == 0) {
                    lviPlayer.Selected = true;
                }
                else {
                    lviPlayer.Selected = false;
                }
            }

            foreach (ListViewItem lviPlayer in this.lsvTeamThreePlayers.Items) {
                if (String.Compare(lviPlayer.Name, strPlayerName) == 0) {
                    lviPlayer.Selected = true;
                }
                else {
                    lviPlayer.Selected = false;
                }
            }

            foreach (ListViewItem lviPlayer in this.lsvTeamFourPlayers.Items) {
                if (String.Compare(lviPlayer.Name, strPlayerName) == 0) {
                    lviPlayer.Selected = true;
                }
                else {
                    lviPlayer.Selected = false;
                }
            }

            this.m_blPropogatingIndexChange = false;
        }

        private void SelectNoPlayer() {
            this.m_blPropogatingIndexChange = true;

            foreach (ListViewItem lviPlayer in this.lsvTeamOnePlayers.Items) {
                lviPlayer.Selected = false;
            }

            foreach (ListViewItem lviPlayer in this.lsvTeamTwoPlayers.Items) {
                lviPlayer.Selected = false;
            }

            foreach (ListViewItem lviPlayer in this.lsvTeamThreePlayers.Items) {
                lviPlayer.Selected = false;
            }

            foreach (ListViewItem lviPlayer in this.lsvTeamFourPlayers.Items) {
                lviPlayer.Selected = false;
            }

            this.m_blPropogatingIndexChange = false;

            this.ClearPunishmentPanel();
        }

        private void lsvTeamOnePlayers_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.lsvTeamOnePlayers.SelectedItems.Count > 0 && this.m_blPropogatingIndexChange == false) {
                this.SelectPlayer(this.lsvTeamOnePlayers.SelectedItems[0].Name);
            }
            else if (this.lsvTeamOnePlayers.FocusedItem != null && this.m_blPropogatingIndexChange == false) {
                this.SelectNoPlayer();
            }
        }

        private void lsvTeamTwoPlayers_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.lsvTeamTwoPlayers.SelectedItems.Count > 0 && this.m_blPropogatingIndexChange == false) {
                this.SelectPlayer(this.lsvTeamTwoPlayers.SelectedItems[0].Name);
            }
            else if (this.lsvTeamTwoPlayers.FocusedItem != null && this.m_blPropogatingIndexChange == false) {
                this.SelectNoPlayer();
            }
        }

        private void RefreshSelectedPlayer() {
            if (this.lsvTeamOnePlayers.SelectedItems.Count > 0) {
                this.lsvPlayers_SelectedIndexChanged(this.lsvTeamOnePlayers, null);
            }
            else if (this.lsvTeamTwoPlayers.SelectedItems.Count > 0) {
                this.lsvPlayers_SelectedIndexChanged(this.lsvTeamTwoPlayers, null);
            }
            else if (this.lsvTeamThreePlayers.SelectedItems.Count > 0) {
                this.lsvPlayers_SelectedIndexChanged(this.lsvTeamThreePlayers, null);
            }
            else if (this.lsvTeamFourPlayers.SelectedItems.Count > 0) {
                this.lsvPlayers_SelectedIndexChanged(this.lsvTeamFourPlayers, null);
            }
            else {
                this.ClearPunishmentPanel();
            }
        }

        private void lsvPlayers_SelectedIndexChanged(object sender, EventArgs e) {

            if (((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems.Count > 0 && this.m_lvwColumnSorter.TotalsAveragesChecker.IsMatch(((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems[0].Name)== false) {

                if (this.m_blPropogatingIndexChange == false) {
                    this.SelectPlayer(((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems[0].Name);
                }

                if (((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems.Count > 0) {

                    this.spltListAdditionalInfo.Panel2Collapsed = false;
                    this.spltInfoPunish.Panel1MinSize = 306;
                    this.spltInfoPunish.Panel2MinSize = 191;
                    this.spltInfoPunish.FixedPanel = FixedPanel.Panel2;
                    this.btnCloseAdditionalInfo.Enabled = true;

                    this.kbpBfbcPunishPanel.SoldierName = ((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems[0].Name;
                    this.kbpPunkbusterPunishPanel.SoldierName = ((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems[0].Name;
                    this.tbcCourtMartial.Enabled = true;

                    //PunkbusterInfo pbInfo = null;
                    //string strCountryName = String.Empty;

                    AdditionalPlayerInfo sapiAdditional = (AdditionalPlayerInfo)((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems[0].Tag;

                    if (((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems[0].Tag != null && sapiAdditional.m_pbInfo != null) {

                        //string strResolvedHost = (string)((object[])this.lsvPlayers.SelectedItems[0].Tag)[2];

                        this.lblPlayerListSelectedName.Text = String.Format("{0} {1} ({2})", ((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems[0].SubItems["tags"].Text, ((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems[0].SubItems["soldiername"].Text, sapiAdditional.m_pbInfo.PlayerCountry);

                        // new string[] { strID, strSoldierName, strGUID, strIP, this.m_frmParent.GetCountryName(strIP) }
                        this.txtPlayerListSelectedGUID.Text = sapiAdditional.m_pbInfo.GUID;

                        string[] a_strSplitIp = sapiAdditional.m_pbInfo.Ip.Split(':');

                        if (sapiAdditional.m_strResolvedHostName.Length > 0) {
                            this.txtPlayerListSelectedIP.Text = String.Format("{0} ({1})", sapiAdditional.m_pbInfo.Ip, sapiAdditional.m_strResolvedHostName);
                        }
                        else {
                            if (this.ResolvePlayerHost() == true && a_strSplitIp.Length >= 1) {
                                try {
                                    SResolvePlayerIP srpResolve = new SResolvePlayerIP();
                                    srpResolve.lviPlayer = ((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems[0];
                                    srpResolve.plpListPanel = this;

                                    Dns.BeginGetHostEntry(IPAddress.Parse(a_strSplitIp[0]), m_asyncResolvePlayerIP, srpResolve);
                                }
                                catch (Exception) { }
                            }

                            this.txtPlayerListSelectedIP.Text = sapiAdditional.m_pbInfo.Ip;
                        }

                        this.pnlAdditionalInfo.Enabled = true;

                        this.kbpPunkbusterPunishPanel.SlotID = sapiAdditional.m_pbInfo.SlotID;
                        this.kbpPunkbusterPunishPanel.IP = a_strSplitIp.Length > 0 ? a_strSplitIp[0] : String.Empty;
                        this.kbpPunkbusterPunishPanel.GUID = sapiAdditional.m_pbInfo.GUID;
                        this.kbpBfbcPunishPanel.IP = a_strSplitIp.Length > 0 ? a_strSplitIp[0] : String.Empty;

                        this.kbpPunkbusterPunishPanel.Enabled = true && (!this.m_spPrivileges.CannotPunishPlayers && this.m_spPrivileges.CanIssueLimitedPunkbusterCommands);
                    }
                    else {
                        this.lblPlayerListSelectedName.Text = String.Format("{0} {1}", ((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems[0].SubItems["tags"].Text, ((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems[0].SubItems["soldiername"].Text);
                        //this.tabCourtMartialPunkbuster.Hide();
                        this.txtPlayerListSelectedGUID.Text = String.Empty;
                        this.txtPlayerListSelectedIP.Text = String.Empty;

                        this.kbpPunkbusterPunishPanel.SlotID = String.Empty;
                        this.kbpPunkbusterPunishPanel.IP = String.Empty;
                        this.kbpPunkbusterPunishPanel.GUID = String.Empty;
                        this.kbpBfbcPunishPanel.IP = String.Empty;
                        this.kbpPunkbusterPunishPanel.Enabled = false;
                    }

                    if (((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems[0].Tag != null && sapiAdditional.m_cpiPlayer != null) {
                        this.txtPlayerListSelectedBc2GUID.Text = sapiAdditional.m_cpiPlayer.GUID;
                        this.kbpBfbcPunishPanel.GUID = sapiAdditional.m_cpiPlayer.GUID;

                        this.pnlAdditionalInfo.Enabled = true;
                    }
                    else {
                        this.txtPlayerListSelectedBc2GUID.Text = String.Empty;
                        this.kbpBfbcPunishPanel.GUID = String.Empty;
                    }

                    if (((PRoCon.Controls.ControlsEx.ListViewNF)sender).SelectedItems[0].Tag != null && sapiAdditional.m_spawnedInventory != null) {

                        //List<string> inventory = new List<string>();

                        string[] inventory = new string[6];

                        foreach (Weapon weapon in sapiAdditional.m_spawnedInventory.Weapons) {

                            int weaponSlot = -1;

                            if (weapon.Slot == WeaponSlots.Primary) {
                                weaponSlot = 0;
                            }
                            else if (weapon.Slot == WeaponSlots.Auxiliary) {
                                weaponSlot = 1;
                            }
                            else if (weapon.Slot == WeaponSlots.Secondary) {
                                weaponSlot = 2;
                            }
                            
                            if (weaponSlot >= 0) {
                                inventory[weaponSlot] = this.m_clocLanguage.GetLocalized(String.Format("global.Weapons.{0}", weapon.Name.ToLower()));
                            }
                        }

                        int specializationSlot = 3;
                        
                        foreach (Specialization spec in sapiAdditional.m_spawnedInventory.Specializations) {
                            inventory[specializationSlot++] = this.m_clocLanguage.GetLocalized(String.Format("global.Specialization.{0}", spec.Name));
                        }

                        List<string> inventoryList = new List<string>(inventory);
                        inventoryList.RemoveAll(String.IsNullOrEmpty);
                        this.lblPlayersInventory.Text = String.Join(", ", inventoryList.ToArray());
                    }
                    else {
                        this.lblPlayersInventory.Text = String.Empty;
                    }

                }
            }
            else if ((((PRoCon.Controls.ControlsEx.ListViewNF)sender).FocusedItem != null || ((((PRoCon.Controls.ControlsEx.ListViewNF)sender).FocusedItem != null && this.m_lvwColumnSorter.TotalsAveragesChecker.IsMatch(((PRoCon.Controls.ControlsEx.ListViewNF)sender).FocusedItem.Name) == true) && this.m_blPropogatingIndexChange == false))) {
                this.SelectNoPlayer();
            }

            this.kbpBfbcPunishPanel.RefreshPanel();
            this.kbpPunkbusterPunishPanel.RefreshPanel();
        }

        private void m_prcClient_PlayerChangedTeam(FrostbiteClient sender, string strSoldierName, int iTeamID, int iSquadID) {
            this.m_blPropogatingIndexChange = true;

            lock (this.m_objPlayerDictionaryLocker) {

                if (this.m_dicPlayers.ContainsKey(strSoldierName) == true) {
                    this.SetPlayerTeamID(this.m_dicPlayers[strSoldierName], iTeamID);

                    this.ArrangePlayers();
                    // Save the SquadChange event for onSquadChange
                    //this.SetPlayerSquadID(this.m_dicPlayers[strSoldierName], iSquadID);
                }

            }

            this.m_blPropogatingIndexChange = false;
        }

        private void m_prcClient_PlayerChangedSquad(FrostbiteClient sender, string strSoldierName, int iTeamID, int iSquadID) {

            lock (this.m_objPlayerDictionaryLocker) {
                if (this.m_dicPlayers.ContainsKey(strSoldierName) == true) {
                    this.SetPlayerTeamID(this.m_dicPlayers[strSoldierName], iTeamID);
                    this.SetPlayerSquadID(this.m_dicPlayers[strSoldierName], iSquadID);
                }
            }
        }

        //public void OnPlayerTeamChange(string strSoldierName, int iTeamID, int iSquadID) {

        //}

        //public void OnPlayerSquadChange(string strSoldierName, int iTeamID, int iSquadID) {

        //}

        // Called by all three lists..
        private void lsvPlayers_ColumnClick(object sender, ColumnClickEventArgs e) {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == this.m_lvwColumnSorter.SortColumn) {
                // Reverse the current sort direction for this column.
                if (this.m_lvwColumnSorter.Order == SortOrder.Ascending) {
                    this.m_lvwColumnSorter.Order = SortOrder.Descending;
                }
                else {
                    this.m_lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            else {
                // Set the column number that is to be sorted; default to ascending.
                this.m_lvwColumnSorter.SortColumn = e.Column;
                this.m_lvwColumnSorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lsvTeamOnePlayers.Sort();
            this.lsvTeamTwoPlayers.Sort();
            this.lsvTeamThreePlayers.Sort();
            this.lsvTeamFourPlayers.Sort();

            //this.lsvTeamOnePlayers.
        }

        private AsyncCallback m_asyncResolvePlayerIP = new AsyncCallback(uscPlayerListPanel.ResolvePlayerIP);

        private struct SResolvePlayerIP {
            public ListViewItem lviPlayer;
            public uscPlayerListPanel plpListPanel;
        }

        public delegate void PlayerIPResolvedDelegate(ListViewItem lviPlayer, string strHostName);
        private void PlayerIPResolved(ListViewItem lviPlayer, string strHostName) {

            AdditionalPlayerInfo sapiAdditional = (AdditionalPlayerInfo)lviPlayer.Tag;
            sapiAdditional.m_strResolvedHostName = strHostName;
            lviPlayer.Tag = sapiAdditional;

            this.RefreshSelectedPlayer();
        }
        private static void ResolvePlayerIP(IAsyncResult ar) {

            try {
                if (ar != null) {
                    SResolvePlayerIP srpResolve = (SResolvePlayerIP)ar.AsyncState;
                    IPHostEntry ipHost = Dns.EndGetHostEntry(ar);
                    srpResolve.plpListPanel.Invoke(new PlayerIPResolvedDelegate(srpResolve.plpListPanel.PlayerIPResolved), new object[] { srpResolve.lviPlayer, ipHost.HostName });
                }
            }
            catch (Exception) { }
        }

        public bool ResolvePlayerHost() {
            return this.m_prcClient.Variables.GetVariable<bool>("RESOLVE_PLAYER_HOST", false);
        }

        private void btnSplitTeams_Click(object sender, EventArgs e) {

            if (this.m_prcClient != null) {

                if (this.m_prcClient.PlayerListSettings.SplitType == 1) {
                    this.m_prcClient.PlayerListSettings.SplitType = 2;
                    //this.btnSplitTeams.ImageKey = "application_tile.png";

                    //this.spltTwoSplit.Panel2Collapsed = false;
                    //this.spltFourSplit.Panel2Collapsed = true;
                }
                else if (this.m_prcClient.PlayerListSettings.SplitType == 2) {
                    this.m_prcClient.PlayerListSettings.SplitType = 4;
                    //this.btnSplitTeams.ImageKey = "application.png";

                    //this.spltTwoSplit.Panel2Collapsed = false;
                    //this.spltFourSplit.Panel2Collapsed = false;
                }
                else if (this.m_prcClient.PlayerListSettings.SplitType == 4) {
                    this.m_prcClient.PlayerListSettings.SplitType = 1;
                    //this.btnSplitTeams.ImageKey = "application_tile_horizontal.png";

                    //this.spltTwoSplit.Panel2Collapsed = true;
                    //this.spltFourSplit.Panel2Collapsed = true;
                }

                this.ArrangePlayers();
            }
        }

        private void m_prcClient_PlayerKilled(PRoConClient sender, Kill kKillerVictimDetails) {

            lock (this.m_objPlayerDictionaryLocker) {

                if (this.m_dicPlayers.ContainsKey(kKillerVictimDetails.Killer.SoldierName) == true && String.Compare(kKillerVictimDetails.Killer.SoldierName, kKillerVictimDetails.Victim.SoldierName, true) != 0) {

                    if (this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].Tag != null && ((AdditionalPlayerInfo)this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].Tag).m_cpiPlayer != null) {
                        ((AdditionalPlayerInfo)this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].Tag).m_cpiPlayer.Kills++;
                    }

                    this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].SubItems["kills"].Tag = ((Double)this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].SubItems["kills"].Tag) + 1;
                    this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].SubItems["kills"].Text = ((Double)this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].SubItems["kills"].Tag).ToString();

                    this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].SubItems["kdr"].Tag = kKillerVictimDetails;

                    if (this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].SubItems["deaths"].Tag != null && (Double)this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].SubItems["deaths"].Tag > 0) {
                        this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].SubItems["kdr"].Text = String.Format("{0:0.00}", ((Double)this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].SubItems["kills"].Tag / (Double)this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].SubItems["deaths"].Tag));
                    }
                    else {
                        this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].SubItems["kdr"].Text = String.Format("{0:0.00}", (Double)this.m_dicPlayers[kKillerVictimDetails.Killer.SoldierName].SubItems["kills"].Tag);
                    }
                }

                if (this.m_dicPlayers.ContainsKey(kKillerVictimDetails.Victim.SoldierName) == true || String.Compare(kKillerVictimDetails.Killer.SoldierName, kKillerVictimDetails.Victim.SoldierName, true) == 0) {
                    if (this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].Tag != null && ((AdditionalPlayerInfo)this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].Tag).m_cpiPlayer != null) {
                        ((AdditionalPlayerInfo)this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].Tag).m_cpiPlayer.Deaths++;
                    }

                    this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].SubItems["deaths"].Tag = (Double)this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].SubItems["deaths"].Tag + 1;
                    this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].SubItems["deaths"].Text = ((Double)this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].SubItems["deaths"].Tag).ToString();

                    this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].SubItems["kdr"].Tag = kKillerVictimDetails;

                    if (this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].SubItems["deaths"].Tag != null && (Double)this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].SubItems["deaths"].Tag > 0) {
                        this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].SubItems["kdr"].Text = String.Format("{0:0.00}", ((Double)this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].SubItems["kills"].Tag / (Double)this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].SubItems["deaths"].Tag));
                    }
                    else {
                        this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].SubItems["kdr"].Text = String.Format("{0:0.00}", (Double)this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].SubItems["kills"].Tag);
                    }

                    this.m_dicPlayers[kKillerVictimDetails.Victim.SoldierName].ImageIndex = this.m_frmMain.iglFlags.Images.IndexOfKey("flag_death.png");

                }

                this.tmrKillDeathHighlight.Enabled = true;
            }

            this.ArrangePlayers();
        }

        /*
        public void OnPlayerKilled(string strKillerName, string strVictimName) {

            if (this.m_dicPlayers.ContainsKey(strKillerName) == true && String.Compare(strKillerName, strVictimName, true) != 0) {

                this.m_dicPlayers[strKillerName].SubItems["kills"].Tag = ((Double)this.m_dicPlayers[strKillerName].SubItems["kills"].Tag) + 1;
                this.m_dicPlayers[strKillerName].SubItems["kills"].Text = ((Double)this.m_dicPlayers[strKillerName].SubItems["kills"].Tag).ToString();

                if (this.m_dicPlayers[strKillerName].SubItems["deaths"].Tag != null && (Double)this.m_dicPlayers[strKillerName].SubItems["deaths"].Tag > 0) {
                    this.m_dicPlayers[strKillerName].SubItems["kdr"].Text = String.Format("{0:0.00}", ((Double)this.m_dicPlayers[strKillerName].SubItems["kills"].Tag / (Double)this.m_dicPlayers[strKillerName].SubItems["deaths"].Tag));
                }
                else {
                    this.m_dicPlayers[strKillerName].SubItems["kdr"].Text = String.Format("{0:0.00}", (Double)this.m_dicPlayers[strKillerName].SubItems["kills"].Tag);
                }
            }

            if (this.m_dicPlayers.ContainsKey(strVictimName) == true || String.Compare(strKillerName, strVictimName, true) == 0) {
                this.m_dicPlayers[strVictimName].SubItems["deaths"].Tag = (Double)this.m_dicPlayers[strVictimName].SubItems["deaths"].Tag + 1;
                this.m_dicPlayers[strVictimName].SubItems["deaths"].Text = ((Double)this.m_dicPlayers[strVictimName].SubItems["deaths"].Tag).ToString();

                if (this.m_dicPlayers[strVictimName].SubItems["deaths"].Tag != null && (Double)this.m_dicPlayers[strVictimName].SubItems["deaths"].Tag > 0) {
                    this.m_dicPlayers[strVictimName].SubItems["kdr"].Text = String.Format("{0:0.00}", ((Double)this.m_dicPlayers[strVictimName].SubItems["kills"].Tag / (Double)this.m_dicPlayers[strVictimName].SubItems["deaths"].Tag));
                }
                else {
                    this.m_dicPlayers[strVictimName].SubItems["kdr"].Text = String.Format("{0:0.00}", (Double)this.m_dicPlayers[strVictimName].SubItems["kills"].Tag);
                }
            }
        }
        */


        private void lsvPlayers_DragDrop(object sender, DragEventArgs e) {
            Point pntClient = ((PRoCon.Controls.ControlsEx.ListViewNF)sender).PointToClient(new Point(e.X, e.Y));
            ListViewItem lviHover = ((PRoCon.Controls.ControlsEx.ListViewNF)sender).GetItemAt(pntClient.X, pntClient.Y);

            lviHover = lviHover == null ? ((PRoCon.Controls.ControlsEx.ListViewNF)sender).GetItemAt(pntClient.X, pntClient.Y - 1) : lviHover;
            lviHover = lviHover == null ? ((PRoCon.Controls.ControlsEx.ListViewNF)sender).GetItemAt(pntClient.X, pntClient.Y + 1) : lviHover;

            if (((PRoCon.Controls.ControlsEx.ListViewNF)sender).Items.Count > 0 && lviHover == null) {
                // This includes the group header (team name)
                lviHover = ((PRoCon.Controls.ControlsEx.ListViewNF)sender).GetItemAt(pntClient.X, pntClient.Y + ((PRoCon.Controls.ControlsEx.ListViewNF)sender).Items[0].Bounds.Height);
            }

            CPlayerInfo cpiSwitchingPlayer = ((CPlayerInfo)e.Data.GetData(typeof(CPlayerInfo)));

            // TO DO: PunishPlayer should be renamed to SendCommand
            if (lviHover != null && lviHover.Tag != null && ((AdditionalPlayerInfo)lviHover.Tag).m_cpiPlayer != null && cpiSwitchingPlayer != null && cpiSwitchingPlayer.TeamID != ((AdditionalPlayerInfo)lviHover.Tag).m_cpiPlayer.TeamID) {
                if (Program.m_application.OptionsSettings.AdminMoveMessage)
                    this.m_prcClient.Game.SendAdminSayPacket("You have been moved to another team/squad by an admin.", new CPlayerSubset(CPlayerSubset.PlayerSubsetType.Player, cpiSwitchingPlayer.SoldierName));
                this.m_prcClient.Game.SendAdminMovePlayerPacket(cpiSwitchingPlayer.SoldierName, ((AdditionalPlayerInfo)lviHover.Tag).m_cpiPlayer.TeamID, this.m_prcClient.GetDefaultSquadIDByMapname(this.m_prcClient.CurrentServerInfo.Map), true);
                
                //this.m_prcClient.SendRequest(new List<string>() { "admin.say", "You have been moved to another team/squad by an admin.", "player", cpiSwitchingPlayer.SoldierName });
                //this.m_prcClient.SendRequest((new List<string>() { "admin.movePlayer", cpiSwitchingPlayer.SoldierName, ((AdditionalPlayerInfo)lviHover.Tag).m_cpiPlayer.TeamID.ToString(), this.m_prcClient.GetDefaultSquadIDByMapname(this.m_prcClient.CurrentServerInfo.Map).ToString(), "true" }));
            }

            this.HoverTeamBackground(null, -1);
        }

        private void lsvPlayers_DragEnter(object sender, DragEventArgs e) {
            e.Effect = DragDropEffects.Move;
        }

        private ListViewItem CreatePlaceHolder(PRoCon.Controls.ControlsEx.ListViewNF lsvList, int iTeamID) {
            ListViewItem lviPlaceHolder = new ListViewItem(".");
            lviPlaceHolder.ForeColor = SystemColors.WindowText;
            lviPlaceHolder.UseItemStyleForSubItems = true;

            AdditionalPlayerInfo sapiInfo = new AdditionalPlayerInfo();
            sapiInfo.m_strResolvedHostName = String.Empty;
            sapiInfo.m_pbInfo = null;
            sapiInfo.m_cpiPlayer = new CPlayerInfo("", String.Empty, iTeamID, 0);
            lviPlaceHolder.Tag = sapiInfo;

            lviPlaceHolder.Group = lsvList.Groups[iTeamID];

            return lviPlaceHolder;
        }

        private bool m_blPlaceHoldersDrawn = false;

        private void AddTeamPlaceHolders() {

            this.m_blPlaceHoldersDrawn = true;

            if (this.m_prcClient != null) {

                int iTeams = this.m_prcClient.GetLocalizedTeamNameCount(this.m_prcClient.CurrentServerInfo.Map);

                if (this.m_prcClient.PlayerListSettings.SplitType == 1) {
                    for (int i = 1; i < iTeams && i < this.lsvTeamOnePlayers.Groups.Count; i++) {
                        if (this.lsvTeamOnePlayers.Groups[i].Items.Count <= 0) {
                            this.lsvTeamOnePlayers.Items.Add(this.CreatePlaceHolder(this.lsvTeamOnePlayers, i));
                        }
                    }
                }
                else if (this.m_prcClient.PlayerListSettings.SplitType == 2) {

                    if (iTeams == 5) {
                        if (this.lsvTeamOnePlayers.Groups[1].Items.Count <= 0) {
                            this.lsvTeamOnePlayers.Items.Add(this.CreatePlaceHolder(this.lsvTeamOnePlayers, 1));
                        }

                        if (this.lsvTeamOnePlayers.Groups[3].Items.Count <= 0) {
                            this.lsvTeamOnePlayers.Items.Add(this.CreatePlaceHolder(this.lsvTeamOnePlayers, 3));
                        }

                        if (this.lsvTeamTwoPlayers.Groups[2].Items.Count <= 0) {
                            this.lsvTeamTwoPlayers.Items.Add(this.CreatePlaceHolder(this.lsvTeamTwoPlayers, 2));
                        }

                        if (this.lsvTeamTwoPlayers.Groups[4].Items.Count <= 0) {
                            this.lsvTeamTwoPlayers.Items.Add(this.CreatePlaceHolder(this.lsvTeamTwoPlayers, 4));
                        }
                    }
                    else if (iTeams == 3) {
                        if (this.lsvTeamOnePlayers.Groups[1].Items.Count <= 0) {
                            this.lsvTeamOnePlayers.Items.Add(this.CreatePlaceHolder(this.lsvTeamOnePlayers, 1));
                        }

                        if (this.lsvTeamTwoPlayers.Groups[2].Items.Count <= 0) {
                            this.lsvTeamTwoPlayers.Items.Add(this.CreatePlaceHolder(this.lsvTeamTwoPlayers, 2));
                        }
                    }
                }
                else if (this.m_prcClient.PlayerListSettings.SplitType == 4) {
                    if (iTeams == 5) {
                        if (this.lsvTeamOnePlayers.Groups[1].Items.Count <= 0) {
                            this.lsvTeamOnePlayers.Items.Add(this.CreatePlaceHolder(this.lsvTeamOnePlayers, 1));
                        }

                        if (this.lsvTeamTwoPlayers.Groups[2].Items.Count <= 0) {
                            this.lsvTeamTwoPlayers.Items.Add(this.CreatePlaceHolder(this.lsvTeamTwoPlayers, 2));
                        }

                        if (this.lsvTeamThreePlayers.Groups[3].Items.Count <= 0) {
                            this.lsvTeamThreePlayers.Items.Add(this.CreatePlaceHolder(this.lsvTeamThreePlayers, 3));
                        }

                        if (this.lsvTeamFourPlayers.Groups[4].Items.Count <= 0) {
                            this.lsvTeamFourPlayers.Items.Add(this.CreatePlaceHolder(this.lsvTeamFourPlayers, 4));
                        }
                    }
                    else if (iTeams == 3) {
                        if (this.lsvTeamOnePlayers.Groups[1].Items.Count <= 0) {
                            this.lsvTeamOnePlayers.Items.Add(this.CreatePlaceHolder(this.lsvTeamOnePlayers, 1));
                        }

                        if (this.lsvTeamTwoPlayers.Groups[2].Items.Count <= 0) {
                            this.lsvTeamTwoPlayers.Items.Add(this.CreatePlaceHolder(this.lsvTeamTwoPlayers, 2));
                        }
                    }
                }
            }
        }

        private void RemovePlaceHolders(PRoCon.Controls.ControlsEx.ListViewNF lsvList) {
            for (int i = 0; i < lsvList.Items.Count; i++) {
                if (String.Compare(lsvList.Items[i].Name, String.Empty) == 0) {
                    lsvList.Items[i].Remove();
                    i--;
                }
            }

            this.m_blPlaceHoldersDrawn = false;

            //this.ArrangePlayers();
        }

        public void BeginDragDrop() {
            this.AddTeamPlaceHolders();
        }

        public void EndDragDrop() {
            this.RemovePlaceHolders(this.lsvTeamOnePlayers);
            this.RemovePlaceHolders(this.lsvTeamTwoPlayers);
            this.RemovePlaceHolders(this.lsvTeamThreePlayers);
            this.RemovePlaceHolders(this.lsvTeamFourPlayers);
        }

        private void lsvPlayers_ItemDrag(object sender, ItemDragEventArgs e) {
            ListViewItem lviSelected = (ListViewItem)e.Item;
            
            if (e.Button == MouseButtons.Left) {

                if (lviSelected != null && lviSelected.Tag != null && ((AdditionalPlayerInfo)lviSelected.Tag).m_cpiPlayer != null && this.m_lvwColumnSorter.TotalsAveragesChecker.IsMatch(lviSelected.Name) == false) {

                    if (this.m_uscConnectionPanel.BeginDragDrop() == true) {
                        ((PRoCon.Controls.ControlsEx.ListViewNF)sender).DoDragDrop(((AdditionalPlayerInfo)lviSelected.Tag).m_cpiPlayer, DragDropEffects.None | DragDropEffects.Move);

                        this.m_uscConnectionPanel.EndDragDrop();
                    }
                }
            }
        }

        private void SetPlaceHoldersColours(PRoCon.Controls.ControlsEx.ListViewNF lsvList, ListViewItem lviIgnoreItem, Color clForeColor, Color clBackColor) {
            for (int i = 0; i < lsvList.Items.Count; i++) {
                if (String.Compare(lsvList.Items[i].Name, String.Empty) == 0 && lsvList.Items[i] != lviIgnoreItem) {
                    lsvList.Items[i].BackColor = clBackColor;
                    lsvList.Items[i].ForeColor = clForeColor;
                }
            }
        }

        private void HoverTeamBackground(ListViewItem lviReserved, int iTeamID) {

            Color clLighLightHighlight = ControlPaint.LightLight(SystemColors.Highlight);

            foreach (KeyValuePair<string, ListViewItem> kvpPlayer in this.m_dicPlayers) {
                if (kvpPlayer.Value.Tag != null && ((AdditionalPlayerInfo)kvpPlayer.Value.Tag).m_cpiPlayer != null) {
                    if (((AdditionalPlayerInfo)kvpPlayer.Value.Tag).m_cpiPlayer.TeamID == iTeamID) {
                        kvpPlayer.Value.BackColor = clLighLightHighlight;
                        kvpPlayer.Value.ForeColor = SystemColors.HighlightText;
                    }
                    else {
                        kvpPlayer.Value.BackColor = SystemColors.Window;
                        kvpPlayer.Value.ForeColor = SystemColors.WindowText;
                    }
                }
            }

            if (lviReserved != null && String.Compare(lviReserved.Name, String.Empty) == 0) {
                lviReserved.BackColor = clLighLightHighlight;
                lviReserved.ForeColor = clLighLightHighlight;
            }

            this.SetPlaceHoldersColours(this.lsvTeamOnePlayers, lviReserved, SystemColors.Window, SystemColors.Window);
            this.SetPlaceHoldersColours(this.lsvTeamTwoPlayers, lviReserved, SystemColors.Window, SystemColors.Window);
            this.SetPlaceHoldersColours(this.lsvTeamThreePlayers, lviReserved, SystemColors.Window, SystemColors.Window);
            this.SetPlaceHoldersColours(this.lsvTeamFourPlayers, lviReserved, SystemColors.Window, SystemColors.Window);
        }

        private void lsvPlayers_DragOver(object sender, DragEventArgs e) {

            Point pntClient = ((PRoCon.Controls.ControlsEx.ListViewNF)sender).PointToClient(new Point(e.X, e.Y));
            ListViewItem lviHover = ((PRoCon.Controls.ControlsEx.ListViewNF)sender).GetItemAt(pntClient.X, pntClient.Y);

            lviHover = lviHover == null ? ((PRoCon.Controls.ControlsEx.ListViewNF)sender).GetItemAt(pntClient.X, pntClient.Y - 1) : lviHover;
            lviHover = lviHover == null ? ((PRoCon.Controls.ControlsEx.ListViewNF)sender).GetItemAt(pntClient.X, pntClient.Y + 1) : lviHover;

            if (((PRoCon.Controls.ControlsEx.ListViewNF)sender).Items.Count > 0 && lviHover == null) {
                // This includes the group header (team name)
                lviHover = ((PRoCon.Controls.ControlsEx.ListViewNF)sender).GetItemAt(pntClient.X, pntClient.Y + ((PRoCon.Controls.ControlsEx.ListViewNF)sender).Items[0].Bounds.Height);
            }

            if (lviHover == null) {
                e.Effect = DragDropEffects.None;
                this.HoverTeamBackground(null , -1);
            }
            else {
                if (lviHover.Tag != null && ((AdditionalPlayerInfo)lviHover.Tag).m_cpiPlayer != null && ((CPlayerInfo)e.Data.GetData(typeof(CPlayerInfo))).TeamID != ((AdditionalPlayerInfo)lviHover.Tag).m_cpiPlayer.TeamID) {
                    e.Effect = DragDropEffects.Move;

                    this.HoverTeamBackground(lviHover, ((AdditionalPlayerInfo)lviHover.Tag).m_cpiPlayer.TeamID);
                }
                else {
                    e.Effect = DragDropEffects.None;
                    this.HoverTeamBackground(lviHover , -1);
                }
            }
        }

        private void PlayerListSettings_TwoSplitterPercentageChanged(float percentage) {
            this.SetSplitterDistances();
        }

        private void PlayerListSettings_FourSplitterPercentageChanged(float percentage) {
            this.SetSplitterDistances();
        }

        private bool m_isSettingSlaveSplitter;

        private void spltTwoSplit_SplitterMoved(object sender, SplitterEventArgs e) {
            if (this.m_prcClient != null && this.m_isSplitterBeingSet == false && this.m_isSettingSlaveSplitter == false) {
                this.m_prcClient.PlayerListSettings.TwoSplitterPercentage = (float)e.SplitX / (float)this.spltTwoSplit.Width;
            }
        }

        private void spltBottomTwoSplit_SplitterMoved(object sender, SplitterEventArgs e) {
            if (this.m_prcClient != null && this.m_isSplitterBeingSet == false && this.m_isSettingSlaveSplitter == false) {
                //    this.m_prcClient.PlayerListSettings.TwoSplitterPercentage = (float)e.SplitX / (float)this.spltBottomTwoSplit.Width;
                this.m_isSettingSlaveSplitter = true;
                this.spltTwoSplit.SplitterDistance = e.SplitX;
                this.m_isSettingSlaveSplitter = false;
            }
        }

        private void spltFourSplit_SplitterMoved(object sender, SplitterEventArgs e) {
            if (this.m_prcClient != null && this.m_isSplitterBeingSet == false) {
                this.m_prcClient.PlayerListSettings.FourSplitterPercentage = (float)this.spltFourSplit.SplitterDistance / (float)this.spltFourSplit.Height;
            }
        }

        private void tmrKillDeathHighlight_Tick(object sender, EventArgs e) {

            if (this.m_blPlaceHoldersDrawn == false) {

                lock (this.m_objPlayerDictionaryLocker) {

                    bool isStillFadingKill = false;

                    foreach (KeyValuePair<string, ListViewItem> kvpPlayer in this.m_dicPlayers) {
                        if (kvpPlayer.Value.SubItems["kdr"].Tag != null && kvpPlayer.Value.SubItems["kdr"].Tag is Kill) {

                            TimeSpan tsDifference = DateTime.Now - ((Kill)kvpPlayer.Value.SubItems["kdr"].Tag).TimeOfDeath;

                            float Opacity = ((5000.0F - (float)tsDifference.TotalMilliseconds) / 5000.0F) * 0.5F;

                            if (Opacity <= 0.0F) {
                                kvpPlayer.Value.SubItems["kdr"].Tag = null;
                                kvpPlayer.Value.BackColor = SystemColors.Window;
                            }
                            else if (Opacity > 0.0F && Opacity <= 1.0F) {
                                if (String.Compare(kvpPlayer.Key, ((Kill)kvpPlayer.Value.SubItems["kdr"].Tag).Victim.SoldierName) == 0) {
                                    kvpPlayer.Value.BackColor = Color.FromArgb((int)((Color.Maroon.R - 255) * Opacity + 255), (int)((Color.Maroon.G - 255) * Opacity + 255), (int)((Color.Maroon.B - 255) * Opacity + 255));
                                }
                                else {
                                    kvpPlayer.Value.BackColor = Color.FromArgb((int)((Color.LightSeaGreen.R - 255) * Opacity + 255), (int)((Color.LightSeaGreen.G - 255) * Opacity + 255), (int)((Color.LightSeaGreen.B - 255) * Opacity + 255));
                                    //kvpPlayer.Value.BackColor = Color.FromArgb((int)(255 * Opacity), Color.Maroon);
                                }

                                isStillFadingKill = true;
                            }
                        }
                    }

                    if (isStillFadingKill == false) {
                        this.tmrKillDeathHighlight.Enabled = false;
                    }
                }
            }
        }

        #region Right Click Context Menu

        private void lsvPlayers_MouseDown(object sender, MouseEventArgs e) {

            //Point pntClient = ((PRoCon.Controls.ControlsEx.ListViewNF)sender).PointToClient(new Point(e.X, e.Y));
            ListViewItem lviSelected = ((PRoCon.Controls.ControlsEx.ListViewNF)sender).GetItemAt(e.X, e.Y);

            if (e.Button == MouseButtons.Left) {

                if (lviSelected == null) {
                    this.SelectNoPlayer();
                }
                else {
                    if (this.m_lvwColumnSorter.TotalsAveragesChecker.IsMatch(lviSelected.Name) == true) {
                        this.SelectNoPlayer();
                    }
                    //else {
                    //    this.SelectPlayer(lviSelected.Name);
                    //}
                }
            }
            else if (e.Button == MouseButtons.Right && lviSelected != null && lviSelected.Tag != null && lviSelected.Tag is AdditionalPlayerInfo) {

                CPlayerInfo player = ((AdditionalPlayerInfo)lviSelected.Tag).m_cpiPlayer;

                if (player != null && lviSelected != null && this.m_lvwColumnSorter.TotalsAveragesChecker.IsMatch(lviSelected.Name) == false) {

                    this.moveToSquadToolStripMenuItem.Text = this.m_clocLanguage.GetDefaultLocalized("Move Player to..", "uscPlayerListPanel.ctxPlayerOptions.moveToSquadToolStripMenuItem", player.SoldierName);
                    this.moveToSquadToolStripMenuItem.DropDownItems.Clear();

                    foreach (CTeamName team in this.m_prcClient.TeamNameList) {
                        if (String.Compare(team.MapFilename, this.m_prcClient.CurrentServerInfo.Map, true) == 0 && team.TeamID != uscPlayerListPanel.INT_NEUTRAL_TEAM) {
                            
                            ToolStripMenuItem teamChange = new ToolStripMenuItem(this.m_clocLanguage.GetDefaultLocalized(String.Format("Team {0}", team.TeamID), "uscPlayerListPanel.ctxPlayerOptions.moveToSquadToolStripMenuItem.Team", this.m_clocLanguage.GetLocalized(team.LocalizationKey)));
                            teamChange.Tag = new object[] { player, team };
                            teamChange.Click += new EventHandler(teamChange_Click);
                            
                            if (team.TeamID == player.TeamID) {
                                teamChange.Checked = true;
                                teamChange.Enabled = false;
                            }

                            this.moveToSquadToolStripMenuItem.DropDownItems.Add(teamChange);
                        }
                    }
                        // uscPlayerListPanel.INT_NEUTRAL_TEAM
                    if (this.m_prcClient.Game.HasSquads == true) {

                        this.moveToSquadToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());

                        for (int i = 0; i <= 8; i++) {

                            ToolStripMenuItem squadChange = new ToolStripMenuItem(this.m_clocLanguage.GetDefaultLocalized(String.Format("Squad {0}", i), "uscPlayerListPanel.ctxPlayerOptions.moveToSquadToolStripMenuItem.Squad", this.m_clocLanguage.GetLocalized(String.Format("global.Squad{0}", i)))); 
                            squadChange.Tag = new object[] { player, i };
                            squadChange.Click += new EventHandler(squadChange_Click);

                            if (player.SquadID == i) {
                                squadChange.Checked = true;
                                squadChange.Enabled = false;
                            }

                            this.moveToSquadToolStripMenuItem.DropDownItems.Add(squadChange);
                        }
                    }

                    this.reservedSlotToolStripMenuItem.Checked = this.m_prcClient.ReservedSlotList.Contains(player.SoldierName);
                    this.reservedSlotToolStripMenuItem.Tag = player;

                    if (this.m_prcClient.FullTextChatModerationList.Contains(player.SoldierName) == true) {

                        TextChatModerationEntry entry = this.m_prcClient.FullTextChatModerationList[player.SoldierName];

                        this.mutedToolStripMenuItem.Checked = (entry.PlayerModerationLevel == PlayerModerationLevelType.Muted);
                        this.normalToolStripMenuItem.Checked = (entry.PlayerModerationLevel == PlayerModerationLevelType.Normal);
                        this.voiceToolStripMenuItem.Checked = (entry.PlayerModerationLevel == PlayerModerationLevelType.Voice);
                        this.adminToolStripMenuItem.Checked = (entry.PlayerModerationLevel == PlayerModerationLevelType.Admin);
                    }
                    else {
                        this.mutedToolStripMenuItem.Checked = this.voiceToolStripMenuItem.Checked = this.adminToolStripMenuItem.Checked = false;
                        this.normalToolStripMenuItem.Checked = true;
                    }

                    this.mutedToolStripMenuItem.Enabled = !this.mutedToolStripMenuItem.Checked;
                    this.normalToolStripMenuItem.Enabled = !this.normalToolStripMenuItem.Checked;
                    this.voiceToolStripMenuItem.Enabled = !this.voiceToolStripMenuItem.Checked;
                    this.adminToolStripMenuItem.Enabled = !this.adminToolStripMenuItem.Checked;

                    this.statsLookupToolStripMenuItem.Tag = this.mutedToolStripMenuItem.Tag = this.normalToolStripMenuItem.Tag = this.voiceToolStripMenuItem.Tag = this.adminToolStripMenuItem.Tag = player;

                    if (this.m_prcClient != null && this.m_prcClient.GameType == "MOH") {
                        this.reservedSlotToolStripMenuItem.Enabled = false;
                    }

                    //show the context menu strip
                    Point menuPosition = Cursor.Position;
                    menuPosition.Offset(1, 1);
                    ctxPlayerOptions.Show(this, this.PointToClient(menuPosition));
                }
            }
        }

        private void squadChange_Click(object sender, EventArgs e) {
            if (sender is ToolStripMenuItem) {
                ToolStripMenuItem squadChange = (ToolStripMenuItem)sender;
                CPlayerInfo player = (CPlayerInfo)((object[])squadChange.Tag)[0];
                int destinationSquadId = (int)((object[])squadChange.Tag)[1];

                this.m_prcClient.Game.SendAdminMovePlayerPacket(player.SoldierName, player.TeamID, destinationSquadId, true);
            }
        }

        private void teamChange_Click(object sender, EventArgs e) {
            if (sender is ToolStripMenuItem) {
                ToolStripMenuItem teamChange = (ToolStripMenuItem)sender;

                CPlayerInfo player = (CPlayerInfo)((object[])teamChange.Tag)[0];
                CTeamName destinationTeam = (CTeamName)((object[])teamChange.Tag)[1];
                
                this.m_prcClient.Game.SendAdminMovePlayerPacket(player.SoldierName, destinationTeam.TeamID, this.m_prcClient.GetDefaultSquadIDByMapname(destinationTeam.MapFilename), true);
            }
        }
        
        private void reservedSlotToolStripMenuItem_Click(object sender, EventArgs e) {

            if (this.reservedSlotToolStripMenuItem.Tag is CPlayerInfo) {
                if (this.reservedSlotToolStripMenuItem.Checked == false) {
                    this.m_prcClient.Game.SendReservedSlotsAddPlayerPacket(((CPlayerInfo)this.reservedSlotToolStripMenuItem.Tag).SoldierName);
                }
                else {
                    this.m_prcClient.Game.SendReservedSlotsRemovePlayerPacket(((CPlayerInfo)this.reservedSlotToolStripMenuItem.Tag).SoldierName);
                }

                this.m_prcClient.Game.SendReservedSlotsSavePacket();
            }
        }

        private void mutedToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.mutedToolStripMenuItem.Tag is CPlayerInfo) {
                this.m_prcClient.Game.SendTextChatModerationListAddPacket(new TextChatModerationEntry(PlayerModerationLevelType.Muted, ((CPlayerInfo)this.reservedSlotToolStripMenuItem.Tag).SoldierName));

                this.m_prcClient.Game.SendTextChatModerationListSavePacket();
            }
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.normalToolStripMenuItem.Tag is CPlayerInfo) {
                this.m_prcClient.Game.SendTextChatModerationListAddPacket(new TextChatModerationEntry(PlayerModerationLevelType.Normal, ((CPlayerInfo)this.reservedSlotToolStripMenuItem.Tag).SoldierName));

                this.m_prcClient.Game.SendTextChatModerationListSavePacket();
            }
        }

        private void voiceToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.voiceToolStripMenuItem.Tag is CPlayerInfo) {
                this.m_prcClient.Game.SendTextChatModerationListAddPacket(new TextChatModerationEntry(PlayerModerationLevelType.Voice, ((CPlayerInfo)this.reservedSlotToolStripMenuItem.Tag).SoldierName));

                this.m_prcClient.Game.SendTextChatModerationListSavePacket();
            }
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.adminToolStripMenuItem.Tag is CPlayerInfo) {
                this.m_prcClient.Game.SendTextChatModerationListAddPacket(new TextChatModerationEntry(PlayerModerationLevelType.Admin, ((CPlayerInfo)this.reservedSlotToolStripMenuItem.Tag).SoldierName));

                this.m_prcClient.Game.SendTextChatModerationListSavePacket();
            }
        }

        private void statsLookupToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.voiceToolStripMenuItem.Tag is CPlayerInfo) {
                if (this.m_prcClient.Game is MoHClient) {
                    System.Diagnostics.Process.Start("http://mohstats.com/stats_pc/" + ((CPlayerInfo)this.voiceToolStripMenuItem.Tag).SoldierName);
                }
                else if (this.m_prcClient.Game is BFBC2Client) {
                    System.Diagnostics.Process.Start("http://bfbcs.com/stats_pc/" + ((CPlayerInfo)this.voiceToolStripMenuItem.Tag).SoldierName);
                }
            }
        }

        #endregion


    }
}
