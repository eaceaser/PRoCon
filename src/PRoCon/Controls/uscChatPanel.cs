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
using System.Text.RegularExpressions;
using System.IO;

namespace PRoCon {
    using Core;
    using Core.Players;
    using Core.Plugin;
    using Core.Remote;

    public partial class uscChatPanel : uscPage {

        private uscServerConnection m_uscParent;
        private CLocalization m_clocLanguage;

        private PRoConClient m_prcClient;

        //private FileStream m_stmChatFile;
        //private StreamWriter m_stwChatFile;

        //private Dictionary<string, Color> m_dicChatTextColours;

        private LinkedList<string> m_llChatHistory;
        private LinkedListNode<string> m_llChatHistoryCurrentNode;

        //public delegate void SendCommandDelegate(List<string> lstCommand);
        //public event SendCommandDelegate SendCommand;

        string m_strAllPlayers = "All Players";

        Regex m_regRemoveCaretCodes;

        /*
        private bool m_blChatLogging;
        public bool ChatLogging {
            get {
                return m_blChatLogging;
            }
            set {

                if (value == true) {

                    try {

                        if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Logs" + Path.DirectorySeparatorChar + this.m_uscParent.BFBC2Connection.FileHostNamePort + Path.DirectorySeparatorChar) == false) {
                            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Logs" + Path.DirectorySeparatorChar + this.m_uscParent.BFBC2Connection.FileHostNamePort + Path.DirectorySeparatorChar);
                        }

                        if (this.m_stmChatFile == null) {
                            if ((this.m_stmChatFile = new FileStream(String.Format(@"{0}{1}", AppDomain.CurrentDomain.BaseDirectory + "Logs" + Path.DirectorySeparatorChar + this.m_uscParent.BFBC2Connection.FileHostNamePort + Path.DirectorySeparatorChar, DateTime.Now.ToString("yyyyMMdd") + "_chat.log"), FileMode.Append)) != null) {
                                if ((this.m_stwChatFile = new StreamWriter(this.m_stmChatFile, Encoding.Unicode)) != null) {

                                    this.m_stwChatFile.WriteLine("Chat logging started: {0}", DateTime.Now.ToString("dddd, d MMMM yyyy HH:mm:ss"));
                                    this.m_stwChatFile.Flush();
                                }
                            }

                            this.m_blChatLogging = true;
                        }
                    }
                    catch (Exception) {
                        this.m_blChatLogging = false;
                    }
                }
                else {
                    if (this.m_stwChatFile != null) {

                        this.m_stwChatFile.WriteLine("Chat logging stopped: {0}", DateTime.Now.ToString("dddd, d MMMM yyyy HH:mm:ss"));

                        this.m_stwChatFile.Close();
                        this.m_stwChatFile.Dispose();
                        this.m_stwChatFile = null;
                    }

                    if (this.m_stmChatFile != null) {
                        this.m_stmChatFile.Close();
                        this.m_stmChatFile.Dispose();
                        this.m_stmChatFile = null;
                    }

                    this.m_blChatLogging = false;
                }
            }
        }
        */

        private void SendCommand(List<string> lstWords) {
            if (lstWords.Count > 0) {
                if (String.Compare(lstWords[0], "admin.yell", true) == 0) {
                    if (lstWords.Count >= 5) {
                        this.m_prcClient.SendProconAdminYell(lstWords[1], lstWords[2], lstWords[3], lstWords[4]);
                    }
                    else {
                        this.m_prcClient.SendProconAdminYell(lstWords[1], lstWords[2], lstWords[3], String.Empty);
                    }
                }
                else if (String.Compare(lstWords[0], "admin.say", true) == 0) {
                    if (lstWords.Count >= 4) {
                        this.m_prcClient.SendProconAdminSay(lstWords[1], lstWords[2], lstWords[3]);
                    }
                    else {
                        this.m_prcClient.SendProconAdminSay(lstWords[1], lstWords[2], String.Empty);
                    }
                }
            }
        }

        /*
        public List<string> SetChatSettings {
            set {
                if (value.Count > 0) {
                    bool blChecked = true;
                    int iIndex = 0;

                    if (value.Count >= 1 && bool.TryParse(value[0], out blChecked) == true) {
                        this.chkDisplayOnJoinLeaveEvents.Checked = blChecked;
                    }

                    if (value.Count >= 2 && bool.TryParse(value[1], out blChecked) == true) {
                        this.chkDisplayOnKilledEvents.Checked = blChecked;
                    }

                    if (value.Count >= 3 && int.TryParse(value[2], out iIndex) == true && iIndex < this.cboDisplayList.Items.Count) {
                        this.cboDisplayList.SelectedIndex = iIndex;
                    }

                    if (value.Count >= 4 && int.TryParse(value[3], out iIndex) == true && iIndex < this.cboDisplayChatTime.Items.Count) {
                        this.cboDisplayChatTime.SelectedIndex = iIndex;
                    }
                }

            }
        }

        public string ChatSettings {
            get {
                return String.Format("{0} {1} {2} {3}", this.chkDisplayOnJoinLeaveEvents.Checked, this.chkDisplayOnKilledEvents.Checked, this.cboDisplayList.SelectedIndex, this.cboDisplayChatTime.SelectedIndex);
            }
        }
        */

        public uscChatPanel() {
            InitializeComponent();

            //this.m_stmChatFile = null;
            //this.m_stwChatFile = null;
            //this.m_blChatLogging = false;
            this.m_clocLanguage = null;

            this.m_llChatHistory = new LinkedList<string>();

            this.cboDisplayChatTime.Items.Clear();
            this.cboDisplayChatTime.Items.Add(2000);
            this.cboDisplayChatTime.Items.Add(4000);
            this.cboDisplayChatTime.Items.Add(6000);
            this.cboDisplayChatTime.Items.Add(8000);
            this.cboDisplayChatTime.Items.Add(10000);
            this.cboDisplayChatTime.Items.Add(15000);
            this.cboDisplayChatTime.Items.Add(20000);
            this.cboDisplayChatTime.Items.Add(25000);
            this.cboDisplayChatTime.Items.Add(30000);
            this.cboDisplayChatTime.Items.Add(35000);
            this.cboDisplayChatTime.Items.Add(40000);
            this.cboDisplayChatTime.Items.Add(45000);
            this.cboDisplayChatTime.Items.Add(50000);
            this.cboDisplayChatTime.Items.Add(55000);
            this.cboDisplayChatTime.Items.Add(59999);

            this.cboDisplayChatTime.SelectedIndex = 3;
            this.cboDisplayList.SelectedIndex = 0;

            this.m_regRemoveCaretCodes = new Regex(@"\^[0-9]|\^b|\^i|\^n", RegexOptions.Compiled);
        }
        
        private void uscChatPanel_Load(object sender, EventArgs e) {
            if (this.m_prcClient != null) {

                // Setting fires events which neatens the code a little elsewhere.
                this.m_prcClient.ChatConsole.LogJoinLeaving = this.m_prcClient.ChatConsole.LogJoinLeaving;
                this.m_prcClient.ChatConsole.LogKills = this.m_prcClient.ChatConsole.LogKills;
                this.m_prcClient.ChatConsole.Scrolling = this.m_prcClient.ChatConsole.Scrolling;
                this.m_prcClient.ChatConsole.DisplayTypeIndex = this.m_prcClient.ChatConsole.DisplayTypeIndex;
                this.m_prcClient.ChatConsole.DisplayTimeIndex = this.m_prcClient.ChatConsole.DisplayTimeIndex;
            }
        }

        public override void SetConnection(PRoConClient prcClient) {
            if ((this.m_prcClient = prcClient) != null) {
                if (this.m_prcClient.Game != null) {
                    this.m_prcClient_GameTypeDiscovered(prcClient);
                }
                else {
                    this.m_prcClient.GameTypeDiscovered += new PRoConClient.EmptyParamterHandler(m_prcClient_GameTypeDiscovered);
                }
                // update max length
                this.chatUpdTxtLength();
            }
        }

        private void m_prcClient_GameTypeDiscovered(PRoConClient sender) {
            this.m_prcClient.ChatConsole.WriteConsole += new PRoCon.Core.Logging.Loggable.WriteConsoleHandler(ChatConsole_WriteConsole);
            this.m_prcClient.ChatConsole.LogJoinLeavingChanged += new PRoCon.Core.Consoles.ChatConsole.IsEnabledHandler(ChatConsole_LogJoinLeavingChanged);
            this.m_prcClient.ChatConsole.LogKillsChanged += new PRoCon.Core.Consoles.ChatConsole.IsEnabledHandler(ChatConsole_LogKillsChanged);
            this.m_prcClient.ChatConsole.ScrollingChanged += new PRoCon.Core.Consoles.ChatConsole.IsEnabledHandler(ChatConsole_ScrollingChanged);

            this.m_prcClient.ChatConsole.DisplayTimeChanged += new PRoCon.Core.Consoles.ChatConsole.IndexChangedHandler(ChatConsole_DisplayTimeChanged);
            this.m_prcClient.ChatConsole.DisplayTypeChanged += new PRoCon.Core.Consoles.ChatConsole.IndexChangedHandler(ChatConsole_DisplayTypeChanged);

            this.m_prcClient.Game.ListPlayers += new FrostbiteClient.ListPlayersHandler(m_prcClient_ListPlayers);
            this.m_prcClient.Game.ServerInfo += new FrostbiteClient.ServerInfoHandler(m_prcClient_ServerInfo);

            if (sender.Game is MoHClient) {
                cboDisplayList.Items.RemoveAt(1);
            }
        }

        public void Initialize(uscServerConnection uscParent) {
            this.m_uscParent = uscParent;

            this.cboPlayerList.Items.Add(new CPlayerInfo("", String.Empty, -10, -10));
            this.cboPlayerList.SelectedIndex = 0;
        }

        public void SetColour(string strVariable, string strValue) {
            this.rtbChatBox.SetColour(strVariable, strValue);
        }

        public override void SetLocalization(CLocalization clocLanguage) {
            this.m_clocLanguage = clocLanguage;

            this.btnChatSend.Text = this.m_clocLanguage.GetLocalized("uscChatPanel.btnChatSend", null);
            this.m_strAllPlayers = this.m_clocLanguage.GetLocalized("uscChatPanel.cboPlayerList.AllPlayers", null);
            this.lblAudience.Text = this.m_clocLanguage.GetLocalized("uscChatPanel.lblAudience", null);
            this.lblDisplayFor.Text = this.m_clocLanguage.GetLocalized("uscChatPanel.lblDisplayFor", null);

            this.lblDisplay.Text = this.m_clocLanguage.GetLocalized("uscChatPanel.lblDisplay", null);

            this.cboDisplayList.Items[0] = this.m_clocLanguage.GetLocalized("uscChatPanel.cboDisplayList.Say", null);

            if (this.cboDisplayList.Items.Count > 1) {
                this.cboDisplayList.Items[1] = this.m_clocLanguage.GetLocalized("uscChatPanel.cboDisplayList.Yell", null);
            }

            this.chkDisplayOnJoinLeaveEvents.Text = this.m_clocLanguage.GetLocalized("uscChatPanel.chkDisplayOnJoinLeaveEvents", null);
            this.chkDisplayOnKilledEvents.Text = this.m_clocLanguage.GetLocalized("uscChatPanel.chkDisplayOnKilledEvents", null);
            this.chkDisplayScrollingEvents.Text = this.m_clocLanguage.GetLocalized("uscChatPanel.chkDisplayScrolling", null);
            this.btnclearchat.Text = this.m_clocLanguage.GetLocalized("uscChatPanel.btnclearchat", null);
            this.cboDisplayChatTime.Refresh();
        }

        private void cboDisplayChatTime_DrawItem(object sender, DrawItemEventArgs e) {
            if (e.Index != -1) {
                int iDrawItemData = ((int)cboDisplayChatTime.Items[e.Index]);

                e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
                if (iDrawItemData == 8000) {
                    e.Graphics.DrawString(String.Format("{0} {1}", iDrawItemData / 1000, this.m_clocLanguage.GetLocalized("global.Seconds.Plural", null).ToLower()), new Font("Calibri", 10, FontStyle.Bold), SystemBrushes.WindowText, e.Bounds.Left, e.Bounds.Top, StringFormat.GenericDefault);
                }
                else {
                    e.Graphics.DrawString(String.Format("{0} {1}", Math.Ceiling((double)iDrawItemData / 1000), this.m_clocLanguage.GetLocalized("global.Seconds.Plural", null).ToLower()), this.Font, SystemBrushes.WindowText, e.Bounds.Left, e.Bounds.Top, StringFormat.GenericDefault);
                }
            }
        }

        private bool isInComboList(CPlayerInfo cpiPlayer) {

            bool blFound = false;

            foreach (CPlayerInfo cpiInfo in this.cboPlayerList.Items) {
                if (String.Compare(cpiInfo.SoldierName, cpiPlayer.SoldierName) == 0) {
                    blFound = true;
                    break;
                }
            }

            return blFound;
        }

        private void m_prcClient_ListPlayers(FrostbiteClient sender, List<CPlayerInfo> lstPlayers, CPlayerSubset cpsSubset) {
            if (cpsSubset.Subset == CPlayerSubset.PlayerSubsetType.All) {

                CPlayerInfo objSelected = (CPlayerInfo)this.cboPlayerList.SelectedItem;

                this.cboPlayerList.BeginUpdate();

                // So much easier with linq..
                foreach (CPlayerInfo cpiPlayer in lstPlayers) {
                    if (this.isInComboList(cpiPlayer) == false) {
                        this.cboPlayerList.Items.Add(cpiPlayer);
                    }
                }

                for (int i = 0; i < this.cboPlayerList.Items.Count; i++) {
                    bool blFound = false;

                    CPlayerInfo cpiInfo = (CPlayerInfo)this.cboPlayerList.Items[i];

                    foreach (CPlayerInfo cpiPlayer in lstPlayers) {
                        if (String.Compare(cpiInfo.SoldierName, cpiPlayer.SoldierName) == 0) {
                            blFound = true;
                            break;
                        }
                    }

                    if (blFound == false && cpiInfo.SquadID != -10 && cpiInfo.TeamID != -10) {
                        this.cboPlayerList.Items.RemoveAt(i);
                        i--;
                    }
                }

                this.cboPlayerList.EndUpdate();

            }
        }

        //public void OnPlayerList(List<CPlayerInfo> lstPlayers) {


        //}

        /*
        public void OnPlayerKilled(string strKillerName, string strVictimName) {
            if (this.chkDisplayOnKilledEvents.Checked == true) {

                foreach (CPlayerInfo cpiInfo in this.cboPlayerList.Items) {
                    if (String.Compare(cpiInfo.SoldierName, strKillerName) == 0) {
                        strKillerName = String.Format("{0} {1}", cpiInfo.ClanTag, cpiInfo.SoldierName);
                    }

                    if (String.Compare(cpiInfo.SoldierName, strVictimName) == 0) {
                        strVictimName = String.Format("{0} {1}", cpiInfo.ClanTag, cpiInfo.SoldierName);
                    }
                }

                // Silly bug =\
                this.m_strPreviousAddition = String.Empty;
                this.Write(String.Format("{0} [^3{1}^0] {2}", strKillerName, this.m_clocLanguage.GetLocalized("uscChatPanel.chkDisplayOnKilledEvents.Killed", null), strVictimName));
            }
        }
        

        public void OnPlayerJoin(string strSoldierName) {
            if (this.chkDisplayOnJoinLeaveEvents.Checked == true) {
                this.Write(String.Format("^4{0}", this.m_clocLanguage.GetLocalized("uscChatPanel.chkDisplayOnJoinLeaveEvents.Joined", new string[] { strSoldierName })));
            }
        }

        public void OnPlayerLeave(string strSoldierName) {
            if (this.chkDisplayOnJoinLeaveEvents.Checked == true) {
                this.Write(String.Format("^1{0}", this.m_clocLanguage.GetLocalized("uscChatPanel.chkDisplayOnJoinLeaveEvents.Left", new string[] { strSoldierName })));
            }
        }
        

        public void OnChat(List<string> lstRawChat) {

            if (String.Compare(lstRawChat[0], "server", true) != 0) {
                if (lstRawChat.Count == 2) { // < R9 Support.
                    this.Write(String.Format("^b^4{0}^0 > ^4{1}", lstRawChat[0], lstRawChat[1]));
                }
                else if (lstRawChat.Count == 3 && String.Compare(lstRawChat[2], "all", true) == 0) {
                    this.Write(String.Format("^b^4{0}^0 > ^4{1}", lstRawChat[0], lstRawChat[1]));
                }
                else if (lstRawChat.Count >= 4 && String.Compare(lstRawChat[2], "team", true) == 0) {
                    this.Write(String.Format("^b^4{0}^0 - ^4{1}^0 >^4 {2}", lstRawChat[0], this.GetLocalizedTeamName(lstRawChat[3]), lstRawChat[1]));
                }
                else if (lstRawChat.Count >= 5 && String.Compare(lstRawChat[2], "squad", true) == 0) {
                    if (String.Compare(lstRawChat[4], "0") != 0) {
                        this.Write(String.Format("^b^4{0}^0 - ^4{1}^0 - ^4{2}^0 >^4 {3}", lstRawChat[0], this.GetLocalizedTeamName(lstRawChat[3]), this.m_clocLanguage.GetLocalized("global.Squad" + lstRawChat[4], null), lstRawChat[1]));
                    }
                    else {
                        // TO DO: Localize and change uscPlayerListPanel.lsvPlayers.colSquad 
                        this.Write(String.Format("^b^4{0}^0 - ^4{1}^0 - ^4{2}^0 >^4 {3}", lstRawChat[0], this.GetLocalizedTeamName(lstRawChat[3]), this.m_clocLanguage.GetLocalized("uscPlayerListPanel.lsvPlayers.colSquad", null), lstRawChat[1]));
                    }
                }
            }
        }
        */

        // Quick R3 hack to stop the chat getting spammed out..
        //private string m_strPreviousAddition = String.Empty;

        private void ChatConsole_WriteConsole(DateTime dtLoggedTime, string strLoggedText) {
            string strFormattedConsoleOutput = String.Format("[{0}] {1}{2}", dtLoggedTime.ToString("HH:mm:ss"), strLoggedText, Environment.NewLine);

            this.rtbChatBox.AppendText(strFormattedConsoleOutput);
            //this.rtbConsoleBox.AppendText(String.Format("[{0}] {1}\n", DateTime.Now.ToString("HH:mm:ss"), strConsoleOutput));

            // We only pass the length of the original text to exclude the time from being formatted.
            //this.ColourizeConsoleOutput(strConsoleOutput.Length);

            //if (this.ChatLogging == true && this.m_stwChatFile != null) {
            //    this.m_stwChatFile.Write(this.m_regRemoveCaretCodes.Replace(strFormattedConsoleOutput, ""));
            //    this.m_stwChatFile.Flush();
            //}

            if (this.m_prcClient.ChatConsole.Scrolling == true)
            {
                this.rtbChatBox.ScrollToCaret();
            }
            this.rtbChatBox.ReadOnly = false;

            while (this.rtbChatBox.Lines.Length > this.m_prcClient.Variables.GetVariable<int>("MAX_CHAT_LINES", 75)) {
                this.rtbChatBox.Select(0, this.rtbChatBox.Lines[0].Length + 1);

                this.rtbChatBox.SelectedText = String.Empty;
            }
            this.rtbChatBox.ReadOnly = true;
        }
        /*
        public void Write(string strConsoleOutput) {

            if (String.Compare(strConsoleOutput, m_strPreviousAddition) != 0) {

                m_strPreviousAddition = strConsoleOutput;

                string strFormattedConsoleOutput = String.Format("[{0}] {1}{2}", DateTime.Now.ToString("HH:mm:ss"), strConsoleOutput, Environment.NewLine);
                
                this.rtbChatBox.AppendText(strFormattedConsoleOutput);
                //this.rtbConsoleBox.AppendText(String.Format("[{0}] {1}\n", DateTime.Now.ToString("HH:mm:ss"), strConsoleOutput));

                // We only pass the length of the original text to exclude the time from being formatted.
                //this.ColourizeConsoleOutput(strConsoleOutput.Length);

                if (this.ChatLogging == true && this.m_stwChatFile != null) {
                    this.m_stwChatFile.Write(this.m_regRemoveCaretCodes.Replace(strFormattedConsoleOutput, ""));
                    this.m_stwChatFile.Flush();
                }

                this.rtbChatBox.ScrollToCaret();
                this.rtbChatBox.ReadOnly = false;

                while (this.rtbChatBox.Lines.Length > this.m_uscParent.GetVariableInt("MAX_CHAT_LINES", 75)) {
                    this.rtbChatBox.Select(0, this.rtbChatBox.Lines[0].Length + 1);
                    
                    this.rtbChatBox.SelectedText = String.Empty;
                }
                this.rtbChatBox.ReadOnly = true;
            }
        }
        */
        private void txtChat_KeyDown(object sender, KeyEventArgs e) {

            if (e.KeyData == Keys.Enter) {

                this.btnChatSend_Click(this, null);

                this.m_llChatHistory.AddFirst(this.txtChat.Text);
                if (this.m_llChatHistory.Count > 20) {
                    this.m_llChatHistory.RemoveLast();
                }
                this.m_llChatHistoryCurrentNode = null;
                this.txtChat.Clear();
                this.txtChat.Focus();
                e.SuppressKeyPress = true;
                
                // update max length
                this.chatUpdTxtLength();
            }

            if (e.KeyData == Keys.Up) {
                e.SuppressKeyPress = true;

                if (this.m_llChatHistoryCurrentNode == null && this.m_llChatHistory.First != null) {
                    this.m_llChatHistoryCurrentNode = this.m_llChatHistory.First;
                    this.txtChat.Text = this.m_llChatHistoryCurrentNode.Value;

                    this.txtChat.Select(this.txtChat.Text.Length, 0);
                }
                else if (this.m_llChatHistoryCurrentNode != null && this.m_llChatHistoryCurrentNode.Next != null) {
                    this.m_llChatHistoryCurrentNode = this.m_llChatHistoryCurrentNode.Next;
                    this.txtChat.Text = this.m_llChatHistoryCurrentNode.Value;

                    this.txtChat.Select(this.txtChat.Text.Length, 0);
                }
            }
            else if (e.KeyData == Keys.Down) {

                if (this.m_llChatHistoryCurrentNode != null && this.m_llChatHistoryCurrentNode.Previous != null) {
                    this.m_llChatHistoryCurrentNode = this.m_llChatHistoryCurrentNode.Previous;
                    this.txtChat.Text = this.m_llChatHistoryCurrentNode.Value;

                    this.txtChat.Select(this.txtChat.Text.Length, 0);
                }

                e.SuppressKeyPress = true;
            }
        }
        /*
        private string GetLocalizedTeamName(string strTeamID) {

            string strReturn = "Unknown";
            int iTeamID = 0;

            if (int.TryParse(strTeamID, out iTeamID) == true) {
                strReturn = this.m_uscParent.GetLocalizedTeamName(iTeamID, this.m_strCurrentMapFileName);
            }

            return strReturn;
        }
        /*
        public void OnProconAdminSaying(string strAdminStack, string strMessage, CPlayerSubset spsAudience) {

            string strAdminName = this.m_clocLanguage.GetLocalized("uscChatPanel.rtbChatBox.Admin", null);

            if (strAdminStack.Length > 0) {
                strAdminName = String.Join(" via ", CPluginVariable.DecodeStringArray(strAdminStack));
            }

            if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.All) {
                this.Write(String.Format("^b^2{0}^0 > ^2{1}", strAdminName, strMessage));
            }
            else if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.Player) {
                this.Write(String.Format("^b^2{0}^0 -^2 {1}^0 > ^2{2}", strAdminName, spsAudience.SoldierName, strMessage));
            }
            else if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.Team) {
                this.Write(String.Format("^b^2{0}^0 -^2 {1}^0 >^2 {2}", strAdminName, this.m_uscParent.GetLocalizedTeamName(spsAudience.TeamID, this.m_strCurrentMapFileName), strMessage));
            }
            else if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.Squad) {
                this.Write(String.Format("^b^2{0}^0 -^2 {1}^0 - ^2{2}^0 >^2 {3}", strAdminName, this.m_uscParent.GetLocalizedTeamName(spsAudience.TeamID, this.m_strCurrentMapFileName), this.m_clocLanguage.GetLocalized("global.Squad" + spsAudience.SquadID.ToString(), null), strMessage));
            }
        }

        public void OnProconAdminYelling(string strAdminStack, string strMessage, CPlayerSubset spsAudience) {

            string strAdminName = this.m_clocLanguage.GetLocalized("uscChatPanel.rtbChatBox.Admin", null);

            if (strAdminStack.Length > 0) {
                strAdminName = String.Join(" via ", CPluginVariable.DecodeStringArray(strAdminStack));
            }

            if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.All) {
                this.Write(String.Format("^b^2{0}^0 > ^2{1}", strAdminName.ToUpper(), strMessage.ToUpper()));
            }
            else if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.Player) {
                this.Write(String.Format("^b^2{0}^0 -^2 {1}^0 > ^2{2}", strAdminName.ToUpper(), spsAudience.SoldierName.ToUpper(), strMessage.ToUpper()));
            }
            else if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.Team) {
                this.Write(String.Format("^b^2{0}^0 -^2 {1}^0 >^2 {2}", strAdminName.ToUpper(), this.m_uscParent.GetLocalizedTeamName(spsAudience.TeamID, this.m_strCurrentMapFileName).ToUpper(), strMessage.ToUpper()));
            }
            else if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.Squad) {
                this.Write(String.Format("^b^2{0}^0 -^2 {1}^0 - ^2{2}^0 >^2 {3}", strAdminName.ToUpper(), this.m_uscParent.GetLocalizedTeamName(spsAudience.TeamID, this.m_strCurrentMapFileName).ToUpper(), this.m_clocLanguage.GetLocalized("global.Squad" + spsAudience.SquadID.ToString(), null), strMessage).ToUpper());
            }

        }
        
        public void OnAdminYelling(string strMessage, string strPlayerSubset, string strTarget) {

            if (String.Compare(strPlayerSubset, "all", true) == 0) {
                this.Write(String.Format("^b^4{0}^0 > ^2{1}", this.m_clocLanguage.GetLocalized("uscChatPanel.rtbChatBox.Admin", null), strMessage));
            }
            else if (String.Compare(strPlayerSubset, "player", true) == 0) {
                this.Write(String.Format("^b^4{0}^0 >^4 {1}:^0 {2} > ^2{3}", this.m_clocLanguage.GetLocalized("uscChatPanel.rtbChatBox.Admin", null), this.m_clocLanguage.GetLocalized("uscChatPanel.rtbChatBox.Private", null), strTarget, strMessage));
            }
            else if (String.Compare(strPlayerSubset, "team", true) == 0) {
                this.Write(String.Format("^b^4{0}^0 >^4 Team:^2{1}^0 >^2 {2}", this.m_clocLanguage.GetLocalized("uscChatPanel.rtbChatBox.Admin", null), strTarget, strMessage));
            }
            else if (String.Compare(strPlayerSubset, "squad", true) == 0) {
                this.Write(String.Format("^b^4{0}^0 >^4 Squad:^2{1}^0 >^2 {2}", this.m_clocLanguage.GetLocalized("uscChatPanel.rtbChatBox.Admin", null), strTarget, strMessage));
            }
        }*/

        private void btnChatSend_Click(object sender, EventArgs e) {

            this.m_llChatHistory.AddFirst(this.txtChat.Text);
            if (this.m_llChatHistory.Count > 20) {
                this.m_llChatHistory.RemoveLast();
            }
            this.m_llChatHistoryCurrentNode = null;

            CPlayerInfo objSelected = (CPlayerInfo)this.cboPlayerList.SelectedItem;

            if (objSelected != null) {

                if (this.cboDisplayList.SelectedIndex == 0) {

                    string sayOutput = String.Empty;
                    // PK
                    if (this.txtChat.Text.Length > 0 && this.txtChat.Text[0] == '/') {
                        sayOutput = this.txtChat.Text;
                    }
                    else {
                        if (Program.m_application.OptionsSettings.ChatDisplayAdminName)
                        {
                            sayOutput = String.Format("{0}: {1}", this.m_prcClient.Username.Length > 0 ? this.m_prcClient.Username : "Admin", this.txtChat.Text);
                        }
                        else
                        {
                            sayOutput = this.txtChat.Text;
                        }
                    }

                    if (objSelected.SquadID == -10 && objSelected.TeamID == -10) {

                        this.SendCommand(new List<string> { "admin.say", sayOutput, "all" });
                    }
                    else if (objSelected.SquadID == -10 && objSelected.TeamID > 0) {
                        this.SendCommand(new List<string> { "admin.say", sayOutput, "team", objSelected.TeamID.ToString() });
                    }
                    else {
                        this.SendCommand(new List<string> { "admin.say", sayOutput, "player", objSelected.SoldierName });
                    }
                }
                else if (this.cboDisplayList.SelectedIndex == 1) {
                    if (objSelected.SquadID == -10 && objSelected.TeamID == -10) {
                        this.SendCommand(new List<string> { "admin.yell", this.txtChat.Text, ((int)cboDisplayChatTime.SelectedItem).ToString(), "all" });
                    }
                    else if (objSelected.SquadID == -10 && objSelected.TeamID > 0) {
                        this.SendCommand(new List<string> { "admin.yell", this.txtChat.Text, ((int)cboDisplayChatTime.SelectedItem).ToString(), "team", objSelected.TeamID.ToString() });
                    }
                    else {
                        this.SendCommand(new List<string> { "admin.yell", this.txtChat.Text, ((int)cboDisplayChatTime.SelectedItem).ToString(), "player", objSelected.SoldierName });
                    }
                }
            }
            
            this.txtChat.Clear();
            this.txtChat.Focus();
            // update max length
            this.chatUpdTxtLength();
        }
        private void btnclearchat_Click(object sender, EventArgs e)
        {
            this.rtbChatBox.Clear();
            // update max length
            this.chatUpdTxtLength();
            this.txtChat.Clear();
        }

        private int ListContainsTeam(int iTeamID) {
            int iReturnTeamIndex = -1;

            for (int i = 0; i < this.cboPlayerList.Items.Count; i++) {
                if (((CPlayerInfo)cboPlayerList.Items[i]).SquadID == -10 && ((CPlayerInfo)cboPlayerList.Items[i]).TeamID == iTeamID) {
                    iReturnTeamIndex = i;
                    break;
                }
            }

            return iReturnTeamIndex;
        }

        List<string> m_lstTeamNames = new List<string>(17);
        string m_strCurrentMapFileName = String.Empty;

        private void m_prcClient_ServerInfo(FrostbiteClient sender, CServerInfo csiServerInfo) {
            this.cboPlayerList.BeginUpdate();

            int iTotalTeams = this.m_prcClient.GetLocalizedTeamNameCount(csiServerInfo.Map);
            this.m_strCurrentMapFileName = csiServerInfo.Map;

            // Add all the teams.
            for (int i = 1; i < iTotalTeams; i++) {

                int iTeamIndex = -1;

                if ((iTeamIndex = this.ListContainsTeam(i)) == -1) {
                    this.cboPlayerList.Items.Insert(1, new CPlayerInfo(this.m_prcClient.GetLocalizedTeamName(i, csiServerInfo.Map), String.Empty, i, -10));
                }
                else if (iTeamIndex >= 0 && iTeamIndex < this.cboPlayerList.Items.Count) {
                    this.cboPlayerList.Items[iTeamIndex] = new CPlayerInfo(this.m_prcClient.GetLocalizedTeamName(i, csiServerInfo.Map), String.Empty, i, -10);
                }
            }

            // Remove any excess teams (change gamemode)
            for (int i = 0; i < this.cboPlayerList.Items.Count; i++) {
                if (((CPlayerInfo)cboPlayerList.Items[i]).SquadID == -10 && ((CPlayerInfo)cboPlayerList.Items[i]).TeamID > iTotalTeams) {
                    cboPlayerList.Items.RemoveAt(i);
                    i--;
                }
            }

            this.cboPlayerList.EndUpdate();
        }
        /*
        public void OnServerInfo(CServerInfo csiInfo) {

        }
        */
        private void cboPlayerList_DrawItem(object sender, DrawItemEventArgs e) {
            
            if (e.Index != -1) {
                //e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                CPlayerInfo cpiDraw = ((CPlayerInfo)cboPlayerList.Items[e.Index]);

                e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
                
                if (cpiDraw.SquadID == -10 && cpiDraw.TeamID == -10) {
                    e.Graphics.DrawString(this.m_strAllPlayers, new Font("Calibri", 10, FontStyle.Bold), SystemBrushes.WindowText, e.Bounds.Left + 5, e.Bounds.Top, StringFormat.GenericDefault);
                }
                else if (cpiDraw.SquadID == -10 && cpiDraw.TeamID > 0) {
                    e.Graphics.DrawString(cpiDraw.SoldierName, new Font("Calibri", 10, FontStyle.Bold), SystemBrushes.WindowText, e.Bounds.Left + 5, e.Bounds.Top, StringFormat.GenericDefault);
                }
                else {
                    e.Graphics.DrawString(String.Format("{0} {1}", cpiDraw.ClanTag, cpiDraw.SoldierName), this.Font, SystemBrushes.WindowText, e.Bounds.Left + 5, e.Bounds.Top, StringFormat.GenericDefault);
                }
            }
        }

        public void PlayerSelectionChange(string strSoldierName) {
            foreach (CPlayerInfo cpiInfo in this.cboPlayerList.Items) {
                if (String.Compare(cpiInfo.SoldierName, strSoldierName) == 0) {
                    this.cboPlayerList.SelectedItem = cpiInfo;
                    break;
                }
            }
        }
        
        private void cboDisplayList_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.m_prcClient != null) {
                this.m_prcClient.ChatConsole.DisplayTypeIndex = this.cboDisplayList.SelectedIndex;
            }
        }

        private void ChatConsole_DisplayTypeChanged(int index) {
            if (index >= 0 && index < this.cboDisplayList.Items.Count) {
                this.cboDisplayList.SelectedIndex = index;
            }

            if (this.cboDisplayList.SelectedIndex == 0) {
                this.lblDisplayFor.Enabled = false;
                this.cboDisplayChatTime.Enabled = false;
            }
            else if (this.cboDisplayList.SelectedIndex == 1) {
                this.lblDisplayFor.Enabled = true;
                this.cboDisplayChatTime.Enabled = true;
            }
        }

        private void chkDisplayOnJoinLeaveEvents_CheckedChanged(object sender, EventArgs e) {
            if (this.m_prcClient != null) {
                this.m_prcClient.ChatConsole.LogJoinLeaving = this.chkDisplayOnJoinLeaveEvents.Checked;
            }
        }

        private void chkDisplayOnKilledEvents_CheckedChanged(object sender, EventArgs e) {
            if (this.m_prcClient != null) {
                this.m_prcClient.ChatConsole.LogKills = this.chkDisplayOnKilledEvents.Checked;
            }
        }

        private void chkDisplayScrollingEvents_CheckedChanged(object sender, EventArgs e)
        {
            if (this.m_prcClient != null)
            {
                this.m_prcClient.ChatConsole.Scrolling = this.chkDisplayScrollingEvents.Checked;
            }
        }

        private void ChatConsole_ScrollingChanged(bool isEnabled) {
            this.chkDisplayScrollingEvents.Checked = isEnabled;
            this.chatUpdTxtLength();
            this.txtChat.Clear();
        }
        
        private void ChatConsole_LogKillsChanged(bool isEnabled) {
            this.chkDisplayOnKilledEvents.Checked = isEnabled;
        }

        private void ChatConsole_LogJoinLeavingChanged(bool isEnabled) {
            this.chkDisplayOnJoinLeaveEvents.Checked = isEnabled;
        }

        private void ChatConsole_DisplayTimeChanged(int index) {
            if (index >= 0 && index < this.cboDisplayChatTime.Items.Count) {
                this.cboDisplayChatTime.SelectedIndex = index;
            }
        }

        private void cboDisplayChatTime_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.m_prcClient != null) {
                this.m_prcClient.ChatConsole.DisplayTimeIndex = this.cboDisplayChatTime.SelectedIndex;
            }
        }

        private void chatUpdTxtLength()
        {
            // update max length
            if (Program.m_application.OptionsSettings.ChatDisplayAdminName)
            {
                if (this.m_prcClient.Username.Length > 0)
                {
                    this.txtChat.MaxLength = 100 - (this.m_prcClient.Username.Length + 2);
                }
                else
                {
                    this.txtChat.MaxLength = 100 - 7; // "Admin: "
                }
            }
            else
            {
                this.txtChat.MaxLength = 100;
            }
            //
        }
    }
}
