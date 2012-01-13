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
using System.Windows.Forms;

namespace PRoCon.Controls.ServerSettings {
    using Core;
    using Core.Remote;
    public partial class uscServerSettingsConfigGenerator : uscServerSettings {

        private Dictionary<string, string[]> m_dicSettings = new Dictionary<string, string[]>();

        public string HeaderText {
            get;
            protected set;
        }

        public uscServerSettingsConfigGenerator() {
            InitializeComponent();

            this.HeaderText = @"# This script file is run automatically when the server is launched.
# It contains commands on the same format as you would type them in the command console.
# It is always processed as if it is run by a logged-in user.
# It is run even if the remote-admin interface is disabled.
#
# When editing this file, you can check that it is syntactically correct by performing 'admin.runScript Startup.txt' from
#  the command console, and check the result.
#
# This is a good place to put one-time configurations of the server's gamemode (friendly fire on/off etc).
# the Symbol # needs to be removed from the front of the line in order to activate that var.
#
# ------------------------------------------------------------------------------------------------------------------------------------
# File generated by PRoCon Frostbite.  Replace the contents of your /cfg/AdminScripts/startup.txt 
# on your game server with this file for your settings to remain persistent on game server restart.
# ------------------------------------------------------------------------------------------------------------------------------------

";
        }

        public override void SetLocalization(CLocalization clocLanguage) {
            base.SetLocalization(clocLanguage);

            this.DisplayName = clocLanguage.GetLocalized("ConfigGenerator.Title");
            this.btnCopyToClipboard.Text = clocLanguage.GetLocalized("ConfigGenerator.btnCopyToClipboard");
        }

        public override void SetConnection(Core.Remote.PRoConClient prcClient) {
            base.SetConnection(prcClient);

            if (this.Client != null) {
                if (this.Client.Game != null) {
                    this.Client_GameTypeDiscovered(prcClient);
                }
                else {
                    this.Client.GameTypeDiscovered += new PRoConClient.EmptyParamterHandler(Client_GameTypeDiscovered);
                }
            }
        }

        private void Client_GameTypeDiscovered(PRoConClient sender) {
            this.Client.Game.ServerName += new FrostbiteClient.ServerNameHandler(Client_ServerName);
            this.Client.Game.GamePassword += new FrostbiteClient.PasswordHandler(Client_GamePassword);
            this.Client.Game.Punkbuster += new FrostbiteClient.IsEnabledHandler(Client_Punkbuster);
            this.Client.Game.Hardcore += new FrostbiteClient.IsEnabledHandler(Client_Hardcore);
            this.Client.Game.Ranked += new FrostbiteClient.IsEnabledHandler(Client_Ranked);
            //this.Client.Game.RankLimit += new FrostbiteClient.LimitHandler(Client_RankLimit);
            //this.Client.Game.TeamBalance += new FrostbiteClient.IsEnabledHandler(Client_TeamBalance);
            this.Client.Game.FriendlyFire += new FrostbiteClient.IsEnabledHandler(Client_FriendlyFire);
            this.Client.Game.PlayerLimit += new FrostbiteClient.LimitHandler(Client_PlayerLimit);
            this.Client.Game.BannerUrl += new FrostbiteClient.BannerUrlHandler(Client_BannerUrl);
            this.Client.Game.ServerDescription += new FrostbiteClient.ServerDescriptionHandler(Client_ServerDescription);

            //this.Client.Game.KillCam += new FrostbiteClient.IsEnabledHandler(Client_KillCam);
            //this.Client.Game.MiniMap += new FrostbiteClient.IsEnabledHandler(Client_MiniMap);
            //this.Client.Game.CrossHair += new FrostbiteClient.IsEnabledHandler(Client_CrossHair);
            //this.Client.Game.ThreeDSpotting += new FrostbiteClient.IsEnabledHandler(Client_ThreeDSpotting);
            //this.Client.Game.ThirdPersonVehicleCameras += new FrostbiteClient.IsEnabledHandler(Client_ThirdPersonVehicleCameras);
            //this.Client.Game.MiniMapSpotting += new FrostbiteClient.IsEnabledHandler(Client_MiniMapSpotting);

            this.Client.Game.TeamKillCountForKick += new FrostbiteClient.LimitHandler(Client_TeamKillCountForKick);
            this.Client.Game.TeamKillValueForKick += new FrostbiteClient.LimitHandler(Client_TeamKillValueForKick);
            this.Client.Game.TeamKillValueIncrease += new FrostbiteClient.LimitHandler(Client_TeamKillValueIncrease);
            this.Client.Game.TeamKillValueDecreasePerSecond += new FrostbiteClient.LimitHandler(Client_TeamKillValueDecreasePerSecond);

            this.Client.Game.IdleTimeout += new FrostbiteClient.LimitHandler(Client_IdleTimeout);
            this.Client.Game.ProfanityFilter += new FrostbiteClient.IsEnabledHandler(Client_ProfanityFilter);

            this.Client.Game.LevelVariablesList += new FrostbiteClient.LevelVariableListHandler(Client_LevelVariablesList);

            this.Client.Game.ListPlaylists += new FrostbiteClient.ListPlaylistsHandler(Client_ListPlaylists);
            this.Client.Game.SupportedMaps += new FrostbiteClient.SupportedMapsHandler(Client_SupportedMaps);
        }


        void Client_ServerName(FrostbiteClient sender, string strServerName) {
            this.AppendSetting("vars.serverName", strServerName);
        }

        void Client_GamePassword(FrostbiteClient sender, string password) {
            this.AppendSetting("vars.gamePassword", password);
        }

        void Client_Punkbuster(FrostbiteClient sender, bool isEnabled) {
            this.AppendSetting("vars.punkBuster", isEnabled.ToString());
        }

        void Client_Hardcore(FrostbiteClient sender, bool isEnabled) {
            this.AppendSetting("vars.hardCore", isEnabled.ToString());
        }

        void Client_Ranked(FrostbiteClient sender, bool isEnabled) {
            this.AppendSetting("vars.ranked", isEnabled.ToString());
        }
        
        void Client_FriendlyFire(FrostbiteClient sender, bool isEnabled) {
            this.AppendSetting("vars.friendlyFire", isEnabled.ToString());
        }

        void Client_PlayerLimit(FrostbiteClient sender, int limit) {
            this.AppendSetting("vars.playerLimit", limit.ToString());
        }

        void Client_BannerUrl(FrostbiteClient sender, string url) {
            this.AppendSetting("vars.bannerUrl", url);
        }

        void Client_ServerDescription(FrostbiteClient sender, string serverDescription) {
            this.AppendSetting("vars.serverDescription", serverDescription);
        }

        void Client_TeamKillCountForKick(FrostbiteClient sender, int limit) {
            this.AppendSetting("vars.teamKillCountForKick", limit.ToString());
        }

        void Client_TeamKillValueDecreasePerSecond(FrostbiteClient sender, int limit) {
            this.AppendSetting("vars.teamKillValueDecreasePerSecond", limit.ToString());
        }

        void Client_TeamKillValueIncrease(FrostbiteClient sender, int limit) {
            this.AppendSetting("vars.teamKillValueIncrease", limit.ToString());
        }

        void Client_TeamKillValueForKick(FrostbiteClient sender, int limit) {
            this.AppendSetting("vars.teamKillValueForKick", limit.ToString());
        }

        void Client_ProfanityFilter(FrostbiteClient sender, bool isEnabled) {
            this.AppendSetting("vars.profanityFilter", isEnabled.ToString());
        }

        void Client_IdleTimeout(FrostbiteClient sender, int limit) {
            this.AppendSetting("vars.idleTimeout", limit.ToString());
        }

        void Client_LevelVariablesList(FrostbiteClient sender, LevelVariable lvRequestedContext, List<LevelVariable> lstReturnedValues) {
            foreach (LevelVariable variable in lstReturnedValues) {
                if (variable.Context.ContextTarget.Length > 0) {
                    this.AppendSetting(String.Format("levelVars.set {0} {1} {2}", variable.Context.ContextType.ToString().ToLower(), variable.Context.ContextTarget, variable.VariableName), variable.RawValue);
                }
                else {
                    this.AppendSetting(String.Format("levelVars.set {0} {1}", variable.Context.ContextType.ToString().ToLower(), variable.VariableName), variable.RawValue);
                }
            }
        }

        void Client_ListPlaylists(FrostbiteClient sender, List<string> lstPlaylists) {
            foreach (string playList in lstPlaylists) {

                this.Client.Game.SendLevelVarsListPacket(new LevelVariableContext(LevelVariableContextType.GameMode, playList));
                //this.Client.SendRequest("levelVars.list", "gamemode", playList);

                this.Client.Game.SendAdminSupportedMapsPacket(playList);
                //this.Client.SendRequest("admin.supportedMaps", playList);
            }
        }

        void Client_SupportedMaps(FrostbiteClient sender, string strPlaylist, List<string> lstSupportedMaps) {
            foreach (string map in lstSupportedMaps) {
                this.Client.Game.SendLevelVarsListPacket(new LevelVariableContext(LevelVariableContextType.Level, map));
                //this.Client.SendRequest("levelVars.list", "level", map);
            }
        }

        private string SettingToSafeString(string[] valueList) {

            string[] formattedValueList = new string[valueList.Length];

            for (int i = 0; i < valueList.Length; i++) {

                int iValue = 0;
                bool blValue = false;

                if (int.TryParse(valueList[i], out iValue) == true) {
                    formattedValueList[i] = valueList[i];
                }
                else if (bool.TryParse(valueList[i], out blValue) == true) {
                    formattedValueList[i] = Packet.bltos(blValue);
                }
                else {
                    formattedValueList[i] = String.Format("\"{0}\"", valueList[i]);
                }
            }

            return String.Join(" ", formattedValueList);
        }

        protected void AppendSetting(string settingName, params string[] settingValue) {
            if (this.m_dicSettings.ContainsKey(settingName) == true) {
                this.m_dicSettings[settingName] = settingValue;
                
                // Full update

                StringBuilder rewriteConfig = new StringBuilder();

                rewriteConfig.Append(this.HeaderText);
                //this.txtConfig.Text = this.HeaderText;

                foreach (KeyValuePair<string, string[]> kvpSetting in this.m_dicSettings) {
                    rewriteConfig.AppendFormat("{0} {1}{2}", kvpSetting.Key, this.SettingToSafeString(kvpSetting.Value), Environment.NewLine);
                    //this.txtConfig.AppendText(String.Format());
                }

                this.txtConfig.Text = rewriteConfig.ToString();
            }
            else {

                this.m_dicSettings.Add(settingName, settingValue);

                if (this.txtConfig.Text.Length == 0) {
                    this.txtConfig.Text = this.HeaderText;
                }

                // Append it, we have not seen it yet..
                this.txtConfig.AppendText(String.Format("{0} {1}{2}", settingName, this.SettingToSafeString(settingValue), Environment.NewLine));
            }
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e) {
            try {
                Clipboard.SetDataObject(this.txtConfig.Text, true, 5, 10);
            }
            catch (Exception) {
                // Nope, another thread is accessing the clipboard..
            }
        }

    }
}