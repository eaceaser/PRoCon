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
    public partial class uscServerSettingsConfiguration : uscServerSettings {

        private int m_iPreviousSuccessPlayerLimit;
        private int m_iPreviousSuccessIdleTimeoutLimit;

        private string m_strPreviousSuccessAdminPassword;
        private string m_strPreviousSuccessGamePassword;

        public uscServerSettingsConfiguration() {
            InitializeComponent();

            //this.AsyncSettingControls.Add("vars.punkbuster", new AsyncStyleSetting(this.picSettingsPunkbuster, this.chkSettingsPunkbuster, new Control[] { this.chkSettingsPunkbuster }, false));
            //this.AsyncSettingControls.Add("vars.ranked", new AsyncStyleSetting(this.picSettingsRanked, this.chkSettingsRanked, new Control[] { this.chkSettingsRanked }, false));
            
            this.AsyncSettingControls.Add("vars.playerlimit", new AsyncStyleSetting(this.picSettingsPlayerLimit, this.numSettingsPlayerLimit, new Control[] { this.numSettingsPlayerLimit, this.lnkSettingsSetPlayerLimit }, true));
            this.AsyncSettingControls.Add("vars.profanityfilter", new AsyncStyleSetting(this.picSettingsProfanityFilter, this.chkSettingsProfanityFilter, new Control[] { this.chkSettingsProfanityFilter }, true));
            this.AsyncSettingControls.Add("vars.idletimeout 0", new AsyncStyleSetting(this.picSettingsIdleKickLimit, this.chkSettingsNoIdleKickLimit, new Control[] { this.chkSettingsNoIdleKickLimit }, true));
            this.AsyncSettingControls.Add("vars.idletimeout", new AsyncStyleSetting(this.picSettingsIdleKickLimit, this.numSettingsIdleKickLimit, new Control[] { this.numSettingsIdleKickLimit, this.lnkSettingsSetidleKickLimit }, true));

            this.AsyncSettingControls.Add("vars.gamepassword", new AsyncStyleSetting(this.picSettingsGamePassword, this.txtSettingsGamePassword, new Control[] { this.lblSettingsGamePassword, this.txtSettingsGamePassword, this.lnkSettingsSetGamePassword }, true));
            this.AsyncSettingControls.Add("vars.adminpassword", new AsyncStyleSetting(this.picSettingsAdminPassword, this.txtSettingsAdminPassword, new Control[] { this.lblSettingsAdminPassword, this.txtSettingsAdminPassword, this.lnkSettingsSetAdminPassword }, true));
 
            this.m_iPreviousSuccessPlayerLimit = 50;
            this.m_iPreviousSuccessIdleTimeoutLimit = 0;
            this.m_strPreviousSuccessAdminPassword = String.Empty;
            this.m_strPreviousSuccessGamePassword = String.Empty;
        }

        public override void SetLocalization(CLocalization clocLanguage) {
            base.SetLocalization(clocLanguage);

            this.chkSettingsPunkbuster.Text = this.Language.GetLocalized("uscServerSettingsPanel.chkSettingsPunkbuster");
            this.chkSettingsRanked.Text = this.Language.GetLocalized("uscServerSettingsPanel.chkSettingsRanked");

            this.lblSettingsPlayerLimit.Text = this.Language.GetLocalized("uscServerSettingsPanel.lblSettingsPlayerLimit");
            this.lnkSettingsSetPlayerLimit.Text = this.Language.GetLocalized("uscServerSettingsPanel.lnkSettingsSetPlayerLimit");

            this.lblSettingsGamePassword.Text = this.Language.GetLocalized("uscServerSettingsPanel.lblSettingsGamePassword");
            this.lnkSettingsSetGamePassword.Text = this.Language.GetLocalized("uscServerSettingsPanel.lnkSettingsSetGamePassword");
            this.lblSettingsAdminPassword.Text = this.Language.GetLocalized("uscServerSettingsPanel.lblSettingsAdminPassword");
            this.lnkSettingsSetAdminPassword.Text = this.Language.GetLocalized("uscServerSettingsPanel.lnkSettingsSetAdminPassword");

            this.chkSettingsProfanityFilter.Text = this.Language.GetLocalized("uscServerSettingsPanel.chkSettingsProfanityFilter");
            this.chkSettingsNoIdleKickLimit.Text = this.Language.GetLocalized("uscServerSettingsPanel.chkSettingsNoIdleKickLimit");
            this.lnkSettingsSetidleKickLimit.Text = this.Language.GetLocalized("uscServerSettingsPanel.lnkSettingsSetidleKickLimit");

            this.DisplayName = this.Language.GetLocalized("uscServerSettingsPanel.lblSettingsConfiguration");
        }

        public override void SetConnection(Core.Remote.PRoConClient prcClient) {
            base.SetConnection(prcClient);

            if (this.Client != null) {
                if (this.Client.Game != null) {
                    this.m_prcClient_GameTypeDiscovered(prcClient);
                }
                else {
                    this.Client.GameTypeDiscovered += new PRoConClient.EmptyParamterHandler(m_prcClient_GameTypeDiscovered);
                }
            }
        }

        private void m_prcClient_GameTypeDiscovered(PRoConClient sender) {

            this.Client.Game.Punkbuster += new FrostbiteClient.IsEnabledHandler(m_prcClient_Punkbuster);
            this.Client.Game.Ranked += new FrostbiteClient.IsEnabledHandler(m_prcClient_Ranked);

            this.Client.Game.GamePassword += new FrostbiteClient.PasswordHandler(m_prcClient_GamePassword);
            this.Client.Game.AdminPassword += new FrostbiteClient.PasswordHandler(m_prcClient_AdminPassword);

            this.Client.Game.PlayerLimit += new FrostbiteClient.LimitHandler(m_prcClient_PlayerLimit);
            this.Client.Game.MaxPlayerLimit += new FrostbiteClient.LimitHandler(m_prcClient_MaxPlayerLimit);
            this.Client.Game.CurrentPlayerLimit += new FrostbiteClient.LimitHandler(m_prcClient_CurrentPlayerLimit);
            this.Client.Game.ProfanityFilter += new FrostbiteClient.IsEnabledHandler(m_prcClient_ProfanityFilter);
            this.Client.Game.IdleTimeout += new FrostbiteClient.LimitHandler(m_prcClient_IdleTimeout);

            this.Client.Game.ServerInfo += new FrostbiteClient.ServerInfoHandler(m_prcClient_ServerInfo);
        }

        private void m_prcClient_ServerInfo(FrostbiteClient sender, CServerInfo csiServerInfo) {
            if (csiServerInfo.MaxPlayerCount > 0 && csiServerInfo.MaxPlayerCount < this.numSettingsPlayerLimit.Maximum) {
                this.numSettingsPlayerLimit.Value = (decimal)csiServerInfo.MaxPlayerCount;
            }
        }

        #region Passwords

        private void m_prcClient_GamePassword(FrostbiteClient sender, string password) {
            this.OnSettingResponse("vars.gamepassword", password, true);
            this.m_strPreviousSuccessGamePassword = password;
        }

        private void lnkSettingsSetGamePassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (this.Client != null && this.Client.Game != null) {
                this.txtSettingsGamePassword.Focus();
                this.WaitForSettingResponse("vars.gamepassword", this.m_strPreviousSuccessGamePassword);

                this.Client.Game.SendSetVarsGamePasswordPacket(this.txtSettingsGamePassword.Text);
            }
        }

        private void m_prcClient_AdminPassword(FrostbiteClient sender, string password) {
            this.OnSettingResponse("vars.adminpassword", password, true);
            this.m_strPreviousSuccessAdminPassword = password;
        }

        private void lnkSettingsSetAdminPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (this.Client != null && this.Client.Game != null) {
                this.txtSettingsAdminPassword.Focus();
                this.WaitForSettingResponse("vars.adminpassword", this.m_strPreviousSuccessAdminPassword);

                this.Client.Game.SendSetVarsAdminPasswordPacket(this.txtSettingsAdminPassword.Text);
            }
        }

        #endregion

        #region Punkbuster

        private void m_prcClient_Punkbuster(FrostbiteClient sender, bool isEnabled) {
            this.chkSettingsPunkbuster.Checked = isEnabled;
            //this.OnSettingResponse("vars.punkbuster", isEnabled, true);
        }

        /*
        private void chkSettingsPunkbuster_CheckedChanged(object sender, EventArgs e) {

            if (this.Client != null && this.Client.Game != null) {

                if (this.IgnoreEvents == false && this.AsyncSettingControls["vars.punkbuster"].IgnoreEvent == false) {
                    this.WaitForSettingResponse("vars.punkbuster", !this.chkSettingsPunkbuster.Checked);

                    this.Client.Game.SendSetVarsPunkBusterPacket(this.chkSettingsPunkbuster.Checked);
                    //this.SendCommand("vars.punkBuster", Packet.bltos(this.chkSettingsPunkbuster.Checked));
                }
            }
        }
        */

        #endregion

        #region Ranked

        private void m_prcClient_Ranked(FrostbiteClient sender, bool isEnabled) {
            this.chkSettingsRanked.Checked = isEnabled;
            //this.OnSettingResponse("vars.ranked", isEnabled, true);
        }

        /*
        private void chkSettingsRanked_CheckedChanged(object sender, EventArgs e) {
            if (this.Client != null && this.Client.Game != null) {
                if (this.IgnoreEvents == false && this.AsyncSettingControls["vars.ranked"].IgnoreEvent == false) {
                    this.WaitForSettingResponse("vars.ranked", !this.chkSettingsRanked.Checked);

                    this.Client.Game.SendSetVarsRankedPacket(this.chkSettingsPunkbuster.Checked);
                    //this.SendCommand("vars.ranked", Packet.bltos(this.chkSettingsRanked.Checked));
                }
            }
        }
        */

        #endregion

        #region Player Limit

        private void m_prcClient_CurrentPlayerLimit(FrostbiteClient sender, int limit) {
            if (limit > 0 && limit <= this.numSettingsPlayerLimit.Maximum) {
                this.numSettingsPlayerLimit.Value = (decimal)limit;
            }
        }

        private void m_prcClient_MaxPlayerLimit(FrostbiteClient sender, int limit) {
            this.numSettingsPlayerLimit.Maximum = (decimal)limit;
        }

        private void m_prcClient_PlayerLimit(FrostbiteClient sender, int limit) {
            this.OnSettingResponse("vars.playerlimit", (decimal)limit, true);
            this.m_iPreviousSuccessPlayerLimit = limit;
        }
        
        private void lnkSettingsSetPlayerLimt_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (this.Client != null && this.Client.Game != null) {
                this.numSettingsPlayerLimit.Focus();
                this.WaitForSettingResponse("vars.playerlimit", (decimal)this.m_iPreviousSuccessPlayerLimit);

                this.Client.Game.SendSetVarsPlayerLimitPacket((int)this.numSettingsPlayerLimit.Value);
                //this.SendCommand("vars.playerLimit", this.numSettingsPlayerLimit.Value.ToString());
            }
        }

        #endregion

        #region Idle Timeout

        private void m_prcClient_IdleTimeout(FrostbiteClient sender, int limit) {
            this.m_iPreviousSuccessIdleTimeoutLimit = limit;

            if (this.m_iPreviousSuccessIdleTimeoutLimit == 0) {
                this.OnSettingResponse("vars.idletimeout 0", true, true);
            }
            else {
                this.OnSettingResponse("vars.idletimeout", (decimal)this.m_iPreviousSuccessIdleTimeoutLimit, true);
            }
        }

        private void chkSettingsNoIdleKickLimit_CheckedChanged(object sender, EventArgs e) {
            this.pnlSettingsSetidleKickLimit.Enabled = !this.chkSettingsNoIdleKickLimit.Checked;
            this.pnlSettingsSetidleKickLimit.Visible = !this.chkSettingsNoIdleKickLimit.Checked;

            if (this.IgnoreEvents == false && this.AsyncSettingControls["vars.idletimeout 0"].IgnoreEvent == false) {
                if (this.chkSettingsNoIdleKickLimit.Checked == true) {
                    this.WaitForSettingResponse("vars.idletimeout 0", !this.chkSettingsNoIdleKickLimit.Checked);

                    this.Client.Game.SendSetVarsIdleTimeoutPacket(0);
                    //this.SendCommand("vars.idleTimeout", "0");
                }
            }
        }

        private void lnkSettingsSetidleKickLimit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            this.numSettingsIdleKickLimit.Focus();
            this.WaitForSettingResponse("vars.idletimeout", (decimal)this.m_iPreviousSuccessIdleTimeoutLimit);

            this.Client.Game.SendSetVarsIdleTimeoutPacket((int)this.numSettingsIdleKickLimit.Value);
            //this.SendCommand("vars.idleTimeout", this.numSettingsIdleKickLimit.Value.ToString());
        }

        #endregion

        #region Profanity Filter

        private void m_prcClient_ProfanityFilter(FrostbiteClient sender, bool isEnabled) {
            this.OnSettingResponse("vars.profanityfilter", isEnabled, true);
        }

        private void chkSettingsProfanityFilter_CheckedChanged(object sender, EventArgs e) {
            if (this.IgnoreEvents == false && this.AsyncSettingControls["vars.profanityfilter"].IgnoreEvent == false) {
                this.WaitForSettingResponse("vars.profanityfilter", !this.chkSettingsProfanityFilter.Checked);

                this.Client.Game.SendSetVarsProfanityFilterPacket(this.chkSettingsProfanityFilter.Checked);
                //this.SendCommand("vars.profanityFilter", Packet.bltos(this.chkSettingsProfanityFilter.Checked));
            }
        }

        #endregion
    }
}
