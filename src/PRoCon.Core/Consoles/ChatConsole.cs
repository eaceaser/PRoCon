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
using System.Collections;
using System.Collections.Generic;
using System.Text;

// This class will move to .Core once ProConClient is in .Core.
namespace PRoCon.Core.Consoles {
    using Core;
    using Core.Logging;
    using Core.Players;
    using Core.Consoles.Chat;
    using Core.Remote;

    public class ChatConsole : Loggable {

        public event WriteConsoleHandler WriteConsole;
        public event WriteConsoleHandler WriteConsoleViaCommand;

        public delegate void IsEnabledHandler(bool isEnabled);
        public event IsEnabledHandler LogJoinLeavingChanged;
        public event IsEnabledHandler LogKillsChanged;
        public event IsEnabledHandler ScrollingChanged;

        public delegate void IndexChangedHandler(int index);
        public event IndexChangedHandler DisplayTypeChanged;
        public event IndexChangedHandler DisplayTimeChanged;

        private PRoConClient m_prcClient;

        public Queue<ChatMessage> MessageHistory {
            get;
            private set;
        }

        private bool m_blLogJoinLeaving;
        public bool LogJoinLeaving {
            get {
                return this.m_blLogJoinLeaving;
            }
            set {
                if (this.LogJoinLeavingChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.LogJoinLeavingChanged.GetInvocationList(), value);
                }

                this.m_blLogJoinLeaving = value;
            }
        }

        private bool m_blLogKills;
        public bool LogKills {
            get {
                return this.m_blLogKills;
            }
            set {
                if (this.LogKillsChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.LogKillsChanged.GetInvocationList(), value);
                }

                this.m_blLogKills = value;
            }
        }

        private bool m_blScrolling;
        public bool Scrolling
        {
            get
            {
                return this.m_blScrolling;
            }
            set
            {
                if (this.ScrollingChanged != null)
                {
                    FrostbiteConnection.RaiseEvent(this.ScrollingChanged.GetInvocationList(), value);
                }

                this.m_blScrolling = value;
            }
        }

        private int m_iDisplayTypeIndex;
        public int DisplayTypeIndex {
            get {
                return m_iDisplayTypeIndex;
            }
            set {
                this.m_iDisplayTypeIndex = value;

                if (this.DisplayTypeChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.DisplayTypeChanged.GetInvocationList(), this.m_iDisplayTypeIndex);
                }
            }
        }

        private int m_iDisplayTimeIndex;
        public int DisplayTimeIndex {
            get {
                return m_iDisplayTimeIndex;
            }
            set {
                this.m_iDisplayTimeIndex = value;

                if (this.DisplayTimeChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.DisplayTimeChanged.GetInvocationList(), this.m_iDisplayTimeIndex);
                }
            }
        }

        public List<string> Settings {
            get {
                return new List<string>() { this.LogJoinLeaving.ToString(), this.LogKills.ToString(), this.Scrolling.ToString(), this.DisplayTypeIndex.ToString(), this.DisplayTimeIndex.ToString() };
            }
            set {
                if (value.Count > 0) {
                    bool isEnabled = true;
                    int iIndex = 0;

                    if (value.Count >= 1 && bool.TryParse(value[0], out isEnabled) == true) {
                        this.LogJoinLeaving = isEnabled;
                    }

                    if (value.Count >= 2 && bool.TryParse(value[1], out isEnabled) == true) {
                        this.LogKills = isEnabled;
                    }

                    if (value.Count >= 3 && bool.TryParse(value[2], out isEnabled) == true) {
                        this.Scrolling = isEnabled;
                    }

                    if (value.Count >= 4 && int.TryParse(value[3], out iIndex) == true)
                    {
                        this.DisplayTypeIndex = iIndex;
                    }

                    if (value.Count >= 5 && int.TryParse(value[4], out iIndex) == true) {
                        this.DisplayTimeIndex = iIndex;
                    }
                }
            }
        }

        public ChatConsole(PRoConClient prcClient)
            : base() {

            this.m_prcClient = prcClient;

            this.FileHostNamePort = this.m_prcClient.FileHostNamePort;
            this.LoggingStartedPrefix = "Chat logging started";
            this.LoggingStoppedPrefix = "Chat logging stopped";
            this.FileNameSuffix = "chat";

            this.LogJoinLeaving = false;
            this.LogKills = false;
            this.Scrolling = true;
            this.DisplayTypeIndex = 0;
            this.DisplayTimeIndex = 0;

            this.MessageHistory = new Queue<ChatMessage>();

            this.m_prcClient.Game.Chat += new FrostbiteClient.RawChatHandler(m_prcClient_Chat);

            this.m_prcClient.PlayerKilled += new PRoConClient.PlayerKilledHandler(m_prcClient_PlayerKilled);
            this.m_prcClient.Game.PlayerJoin += new FrostbiteClient.PlayerEventHandler(m_prcClient_PlayerJoin);
            this.m_prcClient.Game.PlayerLeft += new FrostbiteClient.PlayerLeaveHandler(m_prcClient_PlayerLeft);

            this.m_prcClient.ProconAdminSaying += new PRoConClient.ProconAdminSayingHandler(m_prcClient_ProconAdminSaying);
            this.m_prcClient.ProconAdminYelling += new PRoConClient.ProconAdminYellingHandler(m_prcClient_ProconAdminYelling);

            this.m_prcClient.ReadRemoteChatConsole += new PRoConClient.ReadRemoteConsoleHandler(m_prcClient_ReadRemoteChatConsole);
        }

        private void EnqueueMessage(ChatMessage message) {

            this.MessageHistory.Enqueue(message);

            while (this.MessageHistory.Count > 100) {
                this.MessageHistory.Dequeue();
            }
        }

        public string ToJsonString(int historyLength) {

            Hashtable messages = new Hashtable();
            ArrayList messageList = new ArrayList();

            List<ChatMessage> chatHistory = new List<ChatMessage>(this.MessageHistory);
            //chatHistory.Reverse();

            for (int i = chatHistory.Count - 1; i > chatHistory.Count - 1 - historyLength && i >= 0; i--) {
                messageList.Add(chatHistory[i].ToHashtable());
            }

            messages.Add("messages", messageList);

            return JSON.JsonEncode(messages);
        }

        public string ToJsonString(DateTime newerThan) {

            Hashtable messages = new Hashtable();
            ArrayList messageList = new ArrayList();

            foreach (ChatMessage message in this.MessageHistory) {
                if (message.LoggedTime >= newerThan) {
                    messageList.Add(message.ToHashtable());
                }
            }

            messages.Add("messages", messageList);

            return JSON.JsonEncode(messages);
        }

        private void m_prcClient_ProconAdminYelling(PRoConClient sender, string strAdminStack, string strMessage, int iMessageDuration, CPlayerSubset spsAudience) {

            string strAdminName = this.m_prcClient.Language.GetLocalized("uscChatPanel.rtbChatBox.Admin", null);
            string formattedMessage = String.Empty;

            if (strAdminStack.Length > 0) {
                strAdminName = String.Join(" via ", CPluginVariable.DecodeStringArray(strAdminStack));
            }

            if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.All) {
                formattedMessage = String.Format("^b^2{0}^0 > ^2{1}", strAdminName.ToUpper(), strMessage.ToUpper());
            }
            else if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.Player) {
                formattedMessage = String.Format("^b^2{0}^0 -^2 {1}^0 > ^2{2}", strAdminName.ToUpper(), spsAudience.SoldierName.ToUpper(), strMessage.ToUpper());
            }
            else if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.Team) {
                formattedMessage = String.Format("^b^2{0}^0 -^2 {1}^0 >^2 {2}", strAdminName.ToUpper(), this.m_prcClient.GetLocalizedTeamName(spsAudience.TeamID, this.m_prcClient.CurrentServerInfo.Map).ToUpper(), strMessage.ToUpper());
            }
            else if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.Squad) {
                formattedMessage = String.Format("^b^2{0}^0 -^2 {1}^0 - ^2{2}^0 >^2 {3}", strAdminName.ToUpper(), this.m_prcClient.GetLocalizedTeamName(spsAudience.TeamID, this.m_prcClient.CurrentServerInfo.Map).ToUpper(), this.m_prcClient.Language.GetLocalized("global.Squad" + spsAudience.SquadID.ToString(), null), strMessage).ToUpper();
            }

            if (formattedMessage.Length > 0) {
                //this.EnqueueMessage(new ChatMessage(DateTime.Now, strAdminName, strMessage, true, true, spsAudience));
                //this.Write(DateTime.Now, formattedMessage);
                this.EnqueueMessage(new ChatMessage(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), strAdminName, strMessage, true, true, spsAudience));
                this.Write(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), formattedMessage);
            }
        }

        private void m_prcClient_ProconAdminSaying(PRoConClient sender, string strAdminStack, string strMessage, CPlayerSubset spsAudience) {
            string strAdminName = this.m_prcClient.Language.GetLocalized("uscChatPanel.rtbChatBox.Admin", null);
            string formattedMessage = String.Empty;

            if (strAdminStack.Length > 0) {
                strAdminName = String.Join(" via ", CPluginVariable.DecodeStringArray(strAdminStack));
            }

            if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.All) {
                formattedMessage = String.Format("^b^2{0}^0 > ^2{1}", strAdminName, strMessage);
            }
            else if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.Player) {
                formattedMessage = String.Format("^b^2{0}^0 -^2 {1}^0 > ^2{2}", strAdminName, spsAudience.SoldierName, strMessage);
            }
            else if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.Team) {
                formattedMessage = String.Format("^b^2{0}^0 -^2 {1}^0 >^2 {2}", strAdminName, this.m_prcClient.GetLocalizedTeamName(spsAudience.TeamID, this.m_prcClient.CurrentServerInfo.Map), strMessage);
            }
            else if (spsAudience.Subset == CPlayerSubset.PlayerSubsetType.Squad) {
                formattedMessage = String.Format("^b^2{0}^0 -^2 {1}^0 - ^2{2}^0 >^2 {3}", strAdminName, this.m_prcClient.GetLocalizedTeamName(spsAudience.TeamID, this.m_prcClient.CurrentServerInfo.Map), this.m_prcClient.Language.GetLocalized("global.Squad" + spsAudience.SquadID.ToString(), null), strMessage);
            }

            if (formattedMessage.Length > 0) {
                this.EnqueueMessage(new ChatMessage(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), strAdminName, strMessage, true, false, spsAudience));
                this.Write(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), formattedMessage);
            }
        }

        private void m_prcClient_Chat(FrostbiteClient sender, List<string> rawChat) {
            int iTeamID = 0;
            string formattedMessage = String.Empty;
            

            if (String.Compare(rawChat[0], "server", true) != 0) {
                if (rawChat.Count == 2) { // < R9 Support.
                    formattedMessage = String.Format("^b^4{0}^0 > ^4{1}", rawChat[0], rawChat[1]);
                }
                else if (rawChat.Count == 3 && String.Compare(rawChat[2], "all", true) == 0) {
                    formattedMessage = String.Format("^b^4{0}^0 > ^4{1}", rawChat[0], rawChat[1]);
                }
                else if (rawChat.Count >= 4 && String.Compare(rawChat[2], "team", true) == 0) {
                    if (int.TryParse(rawChat[3], out iTeamID) == true) {
                        formattedMessage = String.Format("^b^4{0}^0 - ^4{1}^0 >^4 {2}", rawChat[0], this.m_prcClient.GetLocalizedTeamName(iTeamID, this.m_prcClient.CurrentServerInfo.Map), rawChat[1]);
                    }
                }
                else if (rawChat.Count >= 5 && String.Compare(rawChat[2], "squad", true) == 0) {
                    if (int.TryParse(rawChat[3], out iTeamID) == true) {
                        if (String.Compare(rawChat[4], "0") != 0) {
                            formattedMessage = String.Format("^b^4{0}^0 - ^4{1}^0 - ^4{2}^0 >^4 {3}", rawChat[0], this.m_prcClient.GetLocalizedTeamName(iTeamID, this.m_prcClient.CurrentServerInfo.Map), this.m_prcClient.Language.GetLocalized("global.Squad" + rawChat[4], null), rawChat[1]);
                        }
                        else {
                            // TO DO: Localize and change uscPlayerListPanel.lsvPlayers.colSquad 
                            formattedMessage = String.Format("^b^4{0}^0 - ^4{1}^0 - ^4{2}^0 >^4 {3}", rawChat[0], this.m_prcClient.GetLocalizedTeamName(iTeamID, this.m_prcClient.CurrentServerInfo.Map), this.m_prcClient.Language.GetLocalized("uscPlayerListPanel.lsvPlayers.colSquad", null), rawChat[1]);
                        }
                    }
                }

                if (rawChat.Count >= 3) {

                    this.EnqueueMessage(new ChatMessage(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), rawChat[0], rawChat[1], false, false, new CPlayerSubset(rawChat.GetRange(2, rawChat.Count - 2))));
                }

                if (formattedMessage.Length > 0) {
                    this.Write(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), formattedMessage);
                }
            }
        }

        private void m_prcClient_PlayerLeft(FrostbiteClient sender, string playerName, CPlayerInfo cpiPlayer) {
            if (this.LogJoinLeaving == true) {
                if (cpiPlayer != null) {
                    this.Write(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), String.Format("^1{0}", this.m_prcClient.Language.GetLocalized("uscChatPanel.chkDisplayOnJoinLeaveEvents.Left", string.Format("{0} {1}", cpiPlayer.ClanTag, cpiPlayer.SoldierName))));
                }
                else {
                    this.Write(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), String.Format("^1{0}", this.m_prcClient.Language.GetLocalized("uscChatPanel.chkDisplayOnJoinLeaveEvents.Left", playerName)));
                }
            }
        }

        private void m_prcClient_PlayerJoin(FrostbiteClient sender, string playerName) {
            if (this.LogJoinLeaving == true) {
                this.Write(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), String.Format("^4{0}", this.m_prcClient.Language.GetLocalized("uscChatPanel.chkDisplayOnJoinLeaveEvents.Joined", playerName)));
            }
        }

        private void m_prcClient_PlayerKilled(PRoConClient sender, Kill kKillerVictimDetails) {
            if (this.LogKills == true) {

                string strKillerName = kKillerVictimDetails.Killer.SoldierName, strVictimName = kKillerVictimDetails.Victim.SoldierName;

                if (this.m_prcClient.PlayerList.Contains(kKillerVictimDetails.Killer) == true) {
                    strKillerName = String.Format("{0} {1}", this.m_prcClient.PlayerList[kKillerVictimDetails.Killer.SoldierName].ClanTag, kKillerVictimDetails.Killer.SoldierName);
                }

                if (this.m_prcClient.PlayerList.Contains(kKillerVictimDetails.Victim) == true) {
                    strVictimName = String.Format("{0} {1}", this.m_prcClient.PlayerList[kKillerVictimDetails.Victim.SoldierName].ClanTag, kKillerVictimDetails.Victim.SoldierName);
                }

                if (kKillerVictimDetails.Headshot == false) {
                    if (kKillerVictimDetails.DamageType.Length > 0) {
                        this.Write(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), String.Format("{0} [^3{1}^0] {2}", strKillerName, this.m_prcClient.Language.GetLocalized(String.Format("global.Weapons.{0}", kKillerVictimDetails.DamageType.ToLower())), strVictimName));
                    }
                    else {
                        this.Write(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), String.Format("{0} [^3{1}^0] {2}", strKillerName, this.m_prcClient.Language.GetLocalized("uscChatPanel.chkDisplayOnKilledEvents.Killed"), strVictimName));
                    }
                }
                else {
                    // show headshot
                    if (kKillerVictimDetails.DamageType.Length > 0) {
                        this.Write(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), String.Format("{0} [^3{1}^0] {2} [^2{3}^0]", strKillerName, this.m_prcClient.Language.GetLocalized(String.Format("global.Weapons.{0}", kKillerVictimDetails.DamageType.ToLower())), strVictimName, this.m_prcClient.Language.GetLocalized("uscChatPanel.chkDisplayOnKilledEvents.HeadShot")));
                    }
                    else {
                        this.Write(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), String.Format("{0} [^3{1}^0] {2} [^2{3}^0]", strKillerName, this.m_prcClient.Language.GetLocalized("uscChatPanel.chkDisplayOnKilledEvents.Killed"), strVictimName, this.m_prcClient.Language.GetLocalized("uscChatPanel.chkDisplayOnKilledEvents.HeadShot")));
                    }
                }
            }
        }

        private void m_prcClient_ReadRemoteChatConsole(PRoConClient sender, DateTime loggedTime, string text) {
            this.Write(loggedTime, text);
        }

        /// <summary>
        /// This public method is used whenever the chat console has been written to via
        /// the procon.protected.chat.write command (basically plugin output)
        /// </summary>
        /// <param name="strText"></param>
        public void WriteViaCommand(string strText) {

            this.Write(DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), strText);

            if (this.WriteConsoleViaCommand != null) {
                FrostbiteConnection.RaiseEvent(this.WriteConsoleViaCommand.GetInvocationList(), DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime(), strText);
            }
        }

        /*
        private void Write(string strFormat, params string[] a_objArguments) {

            DateTime dtLoggedTime = DateTime.Now;
            string strText = String.Format(strFormat, a_objArguments);

            this.WriteLogLine(String.Format("[{0}] {1}", dtLoggedTime.ToString("HH:mm:ss"), strText.Replace("{", "{{").Replace("}", "}}")));

            if (this.WriteConsole != null) {
                FrostbiteConnection.RaiseEvent(this.WriteConsole.GetInvocationList(), dtLoggedTime, strText);
            }
        }*/

        private void Write(DateTime dtLoggedTime, string strText) {

            this.WriteLogLine(String.Format("[{0}] {1}", dtLoggedTime.ToString("HH:mm:ss"), strText.Replace("{", "{{").Replace("}", "}}")));

            if (this.WriteConsole != null) {
                FrostbiteConnection.RaiseEvent(this.WriteConsole.GetInvocationList(), dtLoggedTime, strText);
            }
        }

    }
}
