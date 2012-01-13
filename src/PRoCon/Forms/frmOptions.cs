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
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Security;
using System.Security.Permissions;

namespace PRoCon.Forms {
    using Core;
    using Core.Options;
    using Controls.ControlsEx;
    public partial class frmOptions : Form {

        //private Font m_fntComboBoxFont = null;
        private frmMain m_frmParent = null;

        //public delegate void ChangeLanguageDelegate(CLocalization locNewLanguage);
        //public event ChangeLanguageDelegate ChangeLanguage;

        //public delegate void OptionsEnabledDelegate(bool blEnabled);
        //public event OptionsEnabledDelegate ConsoleLoggingChecked;
        //public event OptionsEnabledDelegate EventsLoggingChecked;
        //public event OptionsEnabledDelegate ChatLoggingChecked;

        //public event OptionsEnabledDelegate ShowTrayIconChecked;

        //private string m_strSetLanguageFileName = String.Empty;

        public static int INT_OPTIONS_PREFERENCES_SHOWWINDOW_TASKBARANDTRAY = 0;
        public static int INT_OPTIONS_PREFERENCES_SHOWWINDOW_TASKBARONLY = 1;

        private PRoConApplication m_praApplication;

        public frmOptions(PRoConApplication praApplication, frmMain frmParent) {
            this.m_isLoadingForm = true;

            InitializeComponent();

            this.m_praApplication = praApplication;
            this.m_praApplication.CurrentLanguageChanged += new PRoConApplication.CurrentLanguageHandler(m_praApplication_CurrentLanguageChanged);
            
            this.m_praApplication.OptionsSettings.AutoApplyUpdatesChanged += new PRoCon.Core.Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_AutoApplyUpdatesChanged);
            this.m_praApplication.OptionsSettings.AutoCheckDownloadUpdatesChanged += new PRoCon.Core.Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_AutoCheckDownloadUpdatesChanged);
            this.m_praApplication.OptionsSettings.ChatLoggingChanged += new PRoCon.Core.Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_ChatLoggingChanged);
            this.m_praApplication.OptionsSettings.PluginsLoggingChanged += new OptionsSettings.OptionsEnabledHandler(OptionsSettings_PluginsLoggingChanged);
            this.m_praApplication.OptionsSettings.EventsLoggingChanged += new PRoCon.Core.Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_EventsLoggingChanged);
            this.m_praApplication.OptionsSettings.ConsoleLoggingChanged += new PRoCon.Core.Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_ConsoleLoggingChanged);

            this.m_praApplication.OptionsSettings.ShowTrayIconChanged += new PRoCon.Core.Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_ShowTrayIconChanged);
            this.m_praApplication.OptionsSettings.CloseToTrayChanged += new PRoCon.Core.Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_CloseToTrayChanged);
            this.m_praApplication.OptionsSettings.MinimizeToTrayChanged += new PRoCon.Core.Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_MinimizeToTrayChanged);

            this.m_praApplication.OptionsSettings.RunPluginsInTrustedSandboxChanged += new PRoCon.Core.Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_RunPluginsInTrustedSandboxChanged);
            this.m_praApplication.OptionsSettings.AllowAllODBCConnectionsChanged += new PRoCon.Core.Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_AllowAllODBCConnectionsChanged);
            this.m_praApplication.OptionsSettings.TrustedHostsWebsitesPorts.ItemAdded += new NotificationList<PRoCon.Core.Options.TrustedHostWebsitePort>.ItemModifiedHandler(TrustedHostsWebsitesPorts_ItemAdded);
            this.m_praApplication.OptionsSettings.TrustedHostsWebsitesPorts.ItemRemoved += new NotificationList<PRoCon.Core.Options.TrustedHostWebsitePort>.ItemModifiedHandler(TrustedHostsWebsitesPorts_ItemRemoved);

            this.m_praApplication.HttpServerOffline += new PRoCon.Core.HttpServer.HttpWebServer.StateChangeHandler(m_praApplication_HttpServerOffline);
            this.m_praApplication.HttpServerOnline += new PRoCon.Core.HttpServer.HttpWebServer.StateChangeHandler(m_praApplication_HttpServerOnline);

            this.m_praApplication.OptionsSettings.AdminMoveMessageChanged += new PRoCon.Core.Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_AdminMoveMessageChanged);
            this.m_praApplication.OptionsSettings.ChatDisplayAdminNameChanged += new PRoCon.Core.Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_ChatDisplayAdminNameChanged);

            this.m_praApplication.OptionsSettings.LayerHideLocalAccountsChanged += new OptionsSettings.OptionsEnabledHandler(OptionsSettings_LayerHideLocalAccountsChanged);
            this.m_praApplication.OptionsSettings.LayerHideLocalPluginsChanged += new OptionsSettings.OptionsEnabledHandler(OptionsSettings_LayerHideLocalPluginsChanged);

            this.m_praApplication.OptionsSettings.ShowRoundTimerConstantlyChanged += new PRoCon.Core.Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_ShowRoundTimerConstantlyChanged);

            //m_fntComboBoxFont = new Font("Calibri", 10);
            this.m_frmParent = frmParent;
            this.picHttpServerServerStatus.Image = this.m_frmParent.picLayerOffline.Image;

            cboBasicsLanguagePicker.DropDownStyle = ComboBoxStyle.DropDownList;
            cboBasicsLanguagePicker.DrawMode = DrawMode.OwnerDrawVariable;

            //this.LoadLocalizationFiles();

            this.btnPluginsRemoveTrustedHostDomain.ImageList = this.btnPluginsAddTrustedHostDomain.ImageList = this.m_frmParent.iglIcons;

            this.btnPluginsAddTrustedHostDomain.ImageKey = "add.png";
            this.btnPluginsRemoveTrustedHostDomain.ImageKey = "cross.png";

            this.cboBasicsShowWindow.SelectedIndex = frmOptions.INT_OPTIONS_PREFERENCES_SHOWWINDOW_TASKBARANDTRAY;
            this.cboPluginsSandboxOptions.SelectedIndex = 0;
        }
        
        private void m_praApplication_CurrentLanguageChanged(CLocalization language) {
            cboBasicsLanguagePicker.SelectedItem = language;
        }

        private bool m_isLoadingForm;
        private void frmOptions_Load(object sender, EventArgs e) {

            if (this.m_praApplication != null) {

                this.cboBasicsLanguagePicker.Items.Clear();

                foreach (CLocalization clocLanguage in this.m_praApplication.Languages) {
                    this.cboBasicsLanguagePicker.Items.Add(clocLanguage);
                }

                this.cboBasicsLanguagePicker.SelectedItem = this.m_praApplication.CurrentLanguage;

                this.m_praApplication.OptionsSettings.AutoCheckDownloadUpdates = this.m_praApplication.OptionsSettings.AutoCheckDownloadUpdates;
                this.m_praApplication.OptionsSettings.AutoApplyUpdates = this.m_praApplication.OptionsSettings.AutoApplyUpdates;

                this.m_praApplication.OptionsSettings.ConsoleLogging = this.m_praApplication.OptionsSettings.ConsoleLogging;
                this.m_praApplication.OptionsSettings.ChatLogging = this.m_praApplication.OptionsSettings.ChatLogging;
                this.m_praApplication.OptionsSettings.EventsLogging = this.m_praApplication.OptionsSettings.EventsLogging;
                this.m_praApplication.OptionsSettings.PluginLogging = this.m_praApplication.OptionsSettings.PluginLogging;

                this.m_praApplication.OptionsSettings.ShowTrayIcon = this.m_praApplication.OptionsSettings.ShowTrayIcon;
                this.m_praApplication.OptionsSettings.CloseToTray = this.m_praApplication.OptionsSettings.CloseToTray;
                this.m_praApplication.OptionsSettings.MinimizeToTray = this.m_praApplication.OptionsSettings.MinimizeToTray;

                this.m_praApplication.OptionsSettings.RunPluginsInTrustedSandbox = this.m_praApplication.OptionsSettings.RunPluginsInTrustedSandbox;
                this.m_praApplication.OptionsSettings.AllowAllODBCConnections = this.m_praApplication.OptionsSettings.AllowAllODBCConnections;

                this.m_praApplication.OptionsSettings.AdminMoveMessage = this.m_praApplication.OptionsSettings.AdminMoveMessage;
                this.m_praApplication.OptionsSettings.ChatDisplayAdminName = this.m_praApplication.OptionsSettings.ChatDisplayAdminName;

                this.m_praApplication.OptionsSettings.LayerHideLocalAccounts = this.m_praApplication.OptionsSettings.LayerHideLocalAccounts;
                this.m_praApplication.OptionsSettings.LayerHideLocalPlugins = this.m_praApplication.OptionsSettings.LayerHideLocalPlugins;

                this.m_praApplication.OptionsSettings.ShowRoundTimerConstantly = this.m_praApplication.OptionsSettings.ShowRoundTimerConstantly;

                this.lsvTrustedHostDomainPorts.Items.Clear();
                foreach (TrustedHostWebsitePort trusted in this.m_praApplication.OptionsSettings.TrustedHostsWebsitesPorts) {
                    this.TrustedHostsWebsitesPorts_ItemAdded(0, trusted);
                }

                if (this.m_praApplication.HttpWebServer != null && this.m_praApplication.HttpWebServer.IsOnline == true) {
                    this.m_praApplication_HttpServerOnline(this.m_praApplication.HttpWebServer);
                }

                this.m_isLoadingForm = false;
            }
        }

        public void SetLocalization(CLocalization clocLanguage) {
            this.Text = clocLanguage.GetLocalized("frmOptions.Title");

            this.btnClose.Text = clocLanguage.GetLocalized("global.close");

            this.tabBasics.Text = clocLanguage.GetLocalized("frmOptions.tabBasics");
            this.lblBasicsLanguage.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.lblBasicsLanguage");
            this.btnBasicsSetLanguage.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.btnBasicsSetLanguage");
            this.lblBasicsAuthor.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.lblBasicsAuthor");
            this.lblBasicsLogging.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.lblBasicsLogging");
            this.chkBasicsEnableChatLogging.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.chkBasicsEnableChatLogging");
            this.chkBasicsEnableConsoleLogging.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.chkBasicsEnableConsoleLogging");
            this.chkBasicsEnableEventsLogging.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.chkBasicsEnableEventsLogging");
            this.chkBasicsEnablePluginLogging.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.chkBasicsEnablePluginLogging");

            this.lblBasicsPrivacy.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.lblBasicsPrivacy");
            this.chkBasicsAutoCheckDownloadForUpdates.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.chkBasicsAutoCheckDownloadForUpdates");
            this.chkBasicsAutoApplyUpdates.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.chkBasicsAutoApplyUpdates");

            this.lblBasicPreferences.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.lblBasicPreferences");
            this.lblBasicsShowWindow.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.lblBasicsShowWindow");
            this.cboBasicsShowWindow.Items[0] = clocLanguage.GetLocalized("frmOptions.tabBasics.cboBasicsShowWindow.TaskTray");
            this.cboBasicsShowWindow.Items[1] = clocLanguage.GetLocalized("frmOptions.tabBasics.cboBasicsShowWindow.Task");
            this.chkBasicsCloseToTray.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.chkBasicsCloseToTray");
            this.chkBasicsMinimizeToTray.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.chkBasicsMinimizeToTray");

            this.tabPlugins.Text = clocLanguage.GetLocalized("frmOptions.tabPlugins");

            this.lblPluginsSecurity.Text = clocLanguage.GetLocalized("frmOptions.tabPlugins.lblPluginsSecurity");
            this.cboPluginsSandboxOptions.Items[0] = clocLanguage.GetLocalized("frmOptions.tabPlugins.cboSandboxOptions.Sandbox");
            this.cboPluginsSandboxOptions.Items[1] = clocLanguage.GetLocalized("frmOptions.tabPlugins.cboSandboxOptions.Trusted");

            this.lblPluginsOutgoingConnections.Text = clocLanguage.GetLocalized("frmOptions.tabPlugins.lblPluginsOutgoingConnections");
            this.lblPluginsTrustedHostDomain.Text = clocLanguage.GetLocalized("frmOptions.tabPlugins.lblPluginsTrustedHostDomain");
            this.colTrustedDomainsHost.Text = clocLanguage.GetLocalized("frmOptions.tabPlugins.lblPluginsTrustedHostDomain");
            this.lblPluginsPort.Text = clocLanguage.GetLocalized("frmOptions.tabPlugins.lblPluginsPort");
            this.colTrustedPort.Text = clocLanguage.GetLocalized("frmOptions.tabPlugins.lblPluginsPort");
            this.lblPluginsDatabases.Text = clocLanguage.GetLocalized("frmOptions.tabPlugins.lblPluginsDatabases");
            this.chkAllowODBCConnections.Text = clocLanguage.GetLocalized("frmOptions.tabPlugins.chkAllowODBCConnections");

            this.lblPluginsChangesAfterRestart.Text = clocLanguage.GetLocalized("frmOptions.tabPlugins.lblPluginsChangesAfterRestart");

            this.lblHttpServerTitle.Text = clocLanguage.GetLocalized("frmOptions.lblHttpServerTitle");

            this.lblHttpServerBindingIP.Text = clocLanguage.GetLocalized("frmOptions.lblHttpServerBindingIP");
            this.lblHttpServerStartPort.Text = clocLanguage.GetLocalized("frmOptions.lblHttpServerStartPort");
            this.lblBindingExplanation.Text = clocLanguage.GetLocalized("frmOptions.lblBindingExplanation"); 
            
            this.lnkStartStopHttpServer.Text = clocLanguage.GetLocalized("frmOptions.lnkStartStopHttpServer.Start");
            this.lnkHttpServerForwardedTest.Text = clocLanguage.GetLocalized("frmOptions.lnkHttpServerForwardedTest");

            this.lnkHttpServerForwardedTest.Text = clocLanguage.GetLocalized("frmOptions.lnkHttpServerForwardedTest");
            this.lblHttpServerForwardedTestStatus.Text = clocLanguage.GetLocalized("frmOptions.lblHttpServerForwardedTestStatus.Unknown");

            this.tabAdv.Text = clocLanguage.GetLocalized("frmOptions.tabAdvanced");

            this.lblAdvPlayerTab.Text = clocLanguage.GetLocalized("frmOptions.tabAdvanced.lblAdvPlayerTab");
            this.lblAdvChatTab.Text = clocLanguage.GetLocalized("frmOptions.tabAdvanced.lblAdvChatTab");
            this.chkAdvEnableAdminMoveMsg.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.chkAdvEnableAdminMoveMsg");
            this.chkAdvEnableChatAdminName.Text = clocLanguage.GetLocalized("frmOptions.tabBasics.chkAdvEnableChatAdminName");

            this.lblAdvLayerTabs.Text = clocLanguage.GetLocalized("frmOptions.tabAdvanced.lblAdvLayerTabs");
            this.chkAdvHideLocalPluginsTab.Text = clocLanguage.GetLocalized("frmOptions.tabAdvanced.lblAdvLayerTabs.chkAdvHideLocalPluginsTab");
            this.chkAdvHideLocalAccountsTab.Text = clocLanguage.GetLocalized("frmOptions.tabAdvanced.lblAdvLayerTabs.chkAdvHideLocalAccountsTab");
            this.lblAdvLayerTabsChangeNotice.Text = clocLanguage.GetLocalized("frmOptions.tabAdvanced.lblAdvLayerTabs.lblAdvLayerTabsChangeNotice");

            this.lblAdvConVisuals.Text = clocLanguage.GetLocalized("frmOptions.tabAdvanced.lblAdvConVisuals");
            this.chkAdvShowRoundTimerConstantly.Text = clocLanguage.GetLocalized("frmOptions.tabAdvanced.lblAdvConVisuals.chkAdvShowRoundTimerConstantly");

            //this.m_strSetLanguageFileName = clocLanguage.FileName;
        }

        private void cboBasicsLanguagePicker_SelectedIndexChanged(object sender, EventArgs e) {
            if (cboBasicsLanguagePicker.SelectedItem != null) {

                this.lnkBasicsAuthor.Text = ((CLocalization)cboBasicsLanguagePicker.SelectedItem).GetLocalized("file.author", null);
                //this.lnkBasicsAuthor.LinkArea = new LinkArea(0, this.lnkBasicsAuthor.Text.Length);
                this.lnkBasicsAuthor.Tag = ((CLocalization)cboBasicsLanguagePicker.SelectedItem).GetLocalized("file.authorwebsite", null);
            }
        }

        private void lnkBasicsAuthor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (this.lnkBasicsAuthor.Tag != null) {

                string strLink = this.lnkBasicsAuthor.Tag.ToString();

                if (Regex.Match(strLink, "^http://.*?$").Success == false) {
                    strLink = "http://" + strLink;
                }

                System.Diagnostics.Process.Start(strLink);
            }
        }

        private void btnSetLanguage_Click(object sender, EventArgs e) {
            this.m_praApplication.CurrentLanguage = (CLocalization)cboBasicsLanguagePicker.SelectedItem;
        }

        private void cboBasicsLanguagePicker_DrawItem(object sender, DrawItemEventArgs e) {
            if (e.Index != -1) {

                CLocalization clocDraw = ((CLocalization)cboBasicsLanguagePicker.Items[e.Index]);
                System.Drawing.Image imgFlag = null;

                e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);

                if (this.m_frmParent.iglFlags.Images.ContainsKey(clocDraw.GetLocalized("file.flag", null) + ".png") == true) {

                    imgFlag = this.m_frmParent.iglFlags.Images[clocDraw.GetLocalized("file.flag", null) + ".png"];

                    e.Graphics.DrawImage(imgFlag, e.Bounds.Left + 2, e.Bounds.Top + 3, imgFlag.Width, imgFlag.Height);
                }

                e.Graphics.DrawString(clocDraw.GetLocalized("file.Language", null), this.Font, SystemBrushes.WindowText, e.Bounds.Left + 21, e.Bounds.Top);
            }
        }

        private void cboSandboxOptions_DrawItem(object sender, DrawItemEventArgs e) {
            if (e.Index == 0) {
                e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
                e.Graphics.DrawString((string)this.cboPluginsSandboxOptions.Items[e.Index], new Font("Segoe UI", 9, FontStyle.Bold), SystemBrushes.WindowText, e.Bounds.Left + 5, e.Bounds.Top);
            }
            else if (e.Index == 1) {
                e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);
                e.Graphics.DrawString((string)this.cboPluginsSandboxOptions.Items[e.Index], this.Font, SystemBrushes.WindowText, e.Bounds.Left + 5, e.Bounds.Top);
            }
        }

        private void txtPluginsTrustedPort_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b');
        }

        private void txtPluginsTrustedHostDomain_TextChanged(object sender, EventArgs e) {
            this.btnPluginsAddTrustedHostDomain.Enabled = (this.txtPluginsTrustedHostDomain.Text.Length > 0 && this.txtPluginsTrustedPort.Text.Length > 0);
        }

        private void txtPluginsTrustedPort_TextChanged(object sender, EventArgs e) {
            this.btnPluginsAddTrustedHostDomain.Enabled = (this.txtPluginsTrustedHostDomain.Text.Length > 0 && this.txtPluginsTrustedPort.Text.Length > 0);
        }

        void TrustedHostsWebsitesPorts_ItemAdded(int iIndex, PRoCon.Core.Options.TrustedHostWebsitePort item) {
            ListViewItem lviNewDomainHost = new ListViewItem(item.HostWebsite);
            lviNewDomainHost.Tag = item;

            lviNewDomainHost.SubItems.Add(new ListViewItem.ListViewSubItem(lviNewDomainHost, item.Port.ToString()));

            this.lsvTrustedHostDomainPorts.Items.Add(lviNewDomainHost);

            this.txtPluginsTrustedHostDomain.Clear();
            this.txtPluginsTrustedPort.Clear();
            this.txtPluginsTrustedHostDomain.Focus();
        }

        void TrustedHostsWebsitesPorts_ItemRemoved(int iIndex, PRoCon.Core.Options.TrustedHostWebsitePort item) {
            
            for (int i = 0; i < this.lsvTrustedHostDomainPorts.Items.Count; i++) {
                if (this.lsvTrustedHostDomainPorts.Items[i].Tag == item) {
                    this.lsvTrustedHostDomainPorts.Items.RemoveAt(i);
                    i--;
                }
            }
        }

        private void btnPluginsAddTrustedHostDomain_Click(object sender, EventArgs e) {

            ushort iPort = 0;

            if (ushort.TryParse(this.txtPluginsTrustedPort.Text, out iPort) == true) {

                this.m_praApplication.OptionsSettings.TrustedHostsWebsitesPorts.Add(new TrustedHostWebsitePort(this.txtPluginsTrustedHostDomain.Text, iPort));

                /*
                ListViewItem lviNewDomainHost = new ListViewItem(this.txtPluginsTrustedHostDomain.Text);
                lviNewDomainHost.Tag = iPort;

                lviNewDomainHost.SubItems.Add(new ListViewItem.ListViewSubItem(lviNewDomainHost, this.txtPluginsTrustedPort.Text));

                this.lsvTrustedHostDomainPorts.Items.Add(lviNewDomainHost);

                this.txtPluginsTrustedHostDomain.Clear();
                this.txtPluginsTrustedPort.Clear();
                this.txtPluginsTrustedHostDomain.Focus();
                */
            }
        }

        private void lsvTrustedHostDomainPorts_SelectedIndexChanged(object sender, EventArgs e) {
            this.btnPluginsRemoveTrustedHostDomain.Enabled = (this.lsvTrustedHostDomainPorts.SelectedItems.Count > 0);
        }

        private void btnPluginsRemoveTrustedHostDomain_Click(object sender, EventArgs e) {
            if (this.lsvTrustedHostDomainPorts.SelectedItems.Count > 0) {

                if (this.lsvTrustedHostDomainPorts.SelectedItems[0].Tag != null) {
                    this.m_praApplication.OptionsSettings.TrustedHostsWebsitesPorts.Remove((TrustedHostWebsitePort)this.lsvTrustedHostDomainPorts.SelectedItems[0].Tag);
                }

                /*
                int iReselectIndex = this.lsvTrustedHostDomainPorts.Items.IndexOf(this.lsvTrustedHostDomainPorts.SelectedItems[0]);
                
                this.lsvTrustedHostDomainPorts.Items.Remove(this.lsvTrustedHostDomainPorts.SelectedItems[0]);

                if (iReselectIndex < this.lsvTrustedHostDomainPorts.Items.Count) {
                    this.lsvTrustedHostDomainPorts.Items[iReselectIndex].Selected = true;
                }
                else if (--iReselectIndex >= 0 && this.lsvTrustedHostDomainPorts.Items.Count != 0) {
                    this.lsvTrustedHostDomainPorts.Items[iReselectIndex].Selected = true;
                }
                */
            }
        }

        void OptionsSettings_AllowAllODBCConnectionsChanged(bool blEnabled) {
            this.chkAllowODBCConnections.Checked = blEnabled;
        }

        private void chkAllowODBCConnections_CheckedChanged(object sender, EventArgs e) {
            this.m_praApplication.OptionsSettings.AllowAllODBCConnections = this.chkAllowODBCConnections.Checked;
        }

        #region Privacy

        #region Auto apply updates

        void OptionsSettings_AutoApplyUpdatesChanged(bool blEnabled) {
            this.chkBasicsAutoApplyUpdates.Checked = blEnabled;
        }
        
        private void chkBasicsAutoApplyUpdates_CheckedChanged(object sender, EventArgs e) {
            this.m_praApplication.OptionsSettings.AutoApplyUpdates = this.chkBasicsAutoApplyUpdates.Checked;
        }

        #endregion

        #region Auto check and download

        void OptionsSettings_AutoCheckDownloadUpdatesChanged(bool blEnabled) {
            this.chkBasicsAutoCheckDownloadForUpdates.Checked = blEnabled;
        }

        private void chkBasicsAutoCheckDownloadForUpdates_CheckedChanged(object sender, EventArgs e) {
            this.m_praApplication.OptionsSettings.AutoCheckDownloadUpdates = this.chkBasicsAutoCheckDownloadForUpdates.Checked;
        }

        #endregion

        #endregion

        #region Logging

        #region Chat

        void OptionsSettings_ChatLoggingChanged(bool blEnabled) {
            this.chkBasicsEnableChatLogging.Checked = blEnabled;
        }

        private void chkBasicsEnableChatLogging_CheckedChanged(object sender, EventArgs e) {
            this.m_praApplication.OptionsSettings.ChatLogging = this.chkBasicsEnableChatLogging.Checked;
        }

        #endregion

        #region Events

        void OptionsSettings_EventsLoggingChanged(bool blEnabled) {
            this.chkBasicsEnableEventsLogging.Checked = blEnabled;
        }

        private void chkBasicsEnableEventsLogging_CheckedChanged(object sender, EventArgs e) {
            this.m_praApplication.OptionsSettings.EventsLogging = this.chkBasicsEnableEventsLogging.Checked;
        }

        #endregion

        #region Plugins

        void OptionsSettings_PluginsLoggingChanged(bool blEnabled) {
            this.chkBasicsEnablePluginLogging.Checked = blEnabled;
        }

        private void chkBasicsEnablePluginLogging_CheckedChanged(object sender, EventArgs e) {
            this.m_praApplication.OptionsSettings.PluginLogging = this.chkBasicsEnablePluginLogging.Checked;
        }

        #endregion

        #region Console

        void OptionsSettings_ConsoleLoggingChanged(bool blEnabled) {
            this.chkBasicsEnableConsoleLogging.Checked = blEnabled;
        }
        
        private void chkBasicsEnableConsoleLogging_CheckedChanged(object sender, EventArgs e) {
            this.m_praApplication.OptionsSettings.ConsoleLogging = this.chkBasicsEnableConsoleLogging.Checked;
        }

        #endregion

        #endregion

        void OptionsSettings_ShowTrayIconChanged(bool blEnabled) {
            if (blEnabled == true) {
                this.chkBasicsCloseToTray.Enabled = true;
                this.chkBasicsMinimizeToTray.Enabled = true;

                this.cboBasicsShowWindow.SelectedIndex = 0;
            }
            else {
                this.chkBasicsCloseToTray.Checked = false;
                this.chkBasicsMinimizeToTray.Checked = false;
                this.chkBasicsCloseToTray.Enabled = false;
                this.chkBasicsMinimizeToTray.Enabled = false;

                this.cboBasicsShowWindow.SelectedIndex = 1;
            }
        }

        private void cboBasicsShowWindow_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.m_isLoadingForm == false) {
                if (this.cboBasicsShowWindow.SelectedIndex == frmOptions.INT_OPTIONS_PREFERENCES_SHOWWINDOW_TASKBARANDTRAY) {
                    this.m_praApplication.OptionsSettings.ShowTrayIcon = true;

                }
                else if (this.cboBasicsShowWindow.SelectedIndex == frmOptions.INT_OPTIONS_PREFERENCES_SHOWWINDOW_TASKBARONLY) {
                    this.m_praApplication.OptionsSettings.ShowTrayIcon = false;
                }
            }
        }

        void OptionsSettings_CloseToTrayChanged(bool blEnabled) {
            this.chkBasicsCloseToTray.Checked = blEnabled;
        }

        private void chkBasicsCloseToTray_CheckedChanged(object sender, EventArgs e) {
            this.m_praApplication.OptionsSettings.CloseToTray = this.chkBasicsCloseToTray.Checked;
        }


        void OptionsSettings_MinimizeToTrayChanged(bool blEnabled) {
            this.chkBasicsMinimizeToTray.Checked = blEnabled;
        }

        private void chkBasicsMinimizeToTray_CheckedChanged(object sender, EventArgs e) {
            this.m_praApplication.OptionsSettings.MinimizeToTray = this.chkBasicsMinimizeToTray.Checked;
        }



        void OptionsSettings_RunPluginsInTrustedSandboxChanged(bool blEnabled) {
            this.pnlSandboxOptions.Enabled = blEnabled;

            if (blEnabled == true) {
                this.cboPluginsSandboxOptions.SelectedIndex = 0;
            }
            else {
                this.cboPluginsSandboxOptions.SelectedIndex = 1;
            }

        }

        private void cboSandboxOptions_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.m_isLoadingForm == false) {
                this.m_praApplication.OptionsSettings.RunPluginsInTrustedSandbox = (this.cboPluginsSandboxOptions.SelectedIndex == 0);
            }
            //this.pnlSandboxOptions.Enabled = (this.cboPluginsSandboxOptions.SelectedIndex == 0);
        }

        private void lnkStartStopHttpServer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            this.picHttpServerServerStatus.Image = this.m_frmParent.picAjaxStyleLoading.Image;

            if (this.m_praApplication.HttpWebServer == null || (this.m_praApplication.HttpWebServer != null && this.m_praApplication.HttpWebServer.IsOnline == false)) {
                this.m_praApplication.ExecutePRoConCommand(this.m_praApplication, new List<string>() { "procon.private.httpWebServer.enable", "true", this.txtHttpServerStartPort.Text, this.txtHttpServerBindingAddress.Text }, 0);
            }
            else {
                this.m_praApplication.ExecutePRoConCommand(this.m_praApplication, new List<string>() { "procon.private.httpWebServer.enable", "false", this.txtHttpServerStartPort.Text, this.txtHttpServerBindingAddress.Text }, 0);
            }
        }

        private void m_praApplication_HttpServerOnline(PRoCon.Core.HttpServer.HttpWebServer sender) {
            this.txtHttpServerStartPort.Text = sender.ListeningPort.ToString();
            this.txtPluginsTrustedHostDomain.Text = sender.BindingAddress;

            this.pnlHttpServerSettings.Enabled = false;

            this.lnkHttpServerForwardedTest.Text = this.m_praApplication.CurrentLanguage.GetLocalized("frmOptions.lnkHttpServerForwardedTest");
            this.lblHttpServerForwardedTestStatus.Text = this.m_praApplication.CurrentLanguage.GetLocalized("frmOptions.lblHttpServerForwardedTestStatus.Unknown");
            this.picHttpServerForwardedTestStatus.Image = this.m_frmParent.picPortCheckerUnknown.Image;
            this.lnkHttpServerExampleLink.Visible = false;

            this.pnlHttpServerTester.Visible = true;

            this.lnkStartStopHttpServer.Text = this.m_praApplication.CurrentLanguage.GetLocalized("frmOptions.lnkStartStopHttpServer.Stop");
            this.lblHttpServerServerStatus.Text = this.m_praApplication.CurrentLanguage.GetLocalized("frmOptions.lblHttpServerServerStatus.Online", sender.ListeningPort.ToString());
            this.lblHttpServerServerStatus.ForeColor = Color.ForestGreen;

            this.picHttpServerServerStatus.Image = this.m_frmParent.picLayerOnline.Image;
        }

        private void m_praApplication_HttpServerOffline(PRoCon.Core.HttpServer.HttpWebServer sender) {

            this.pnlHttpServerSettings.Enabled = true;
            this.pnlHttpServerTester.Visible = false;

            this.lnkStartStopHttpServer.Text = this.m_praApplication.CurrentLanguage.GetLocalized("frmOptions.lnkStartStopHttpServer.Start");
            this.lblHttpServerServerStatus.Text = this.m_praApplication.CurrentLanguage.GetLocalized("frmOptions.lblHttpServerServerStatus.Offline");
            this.lblHttpServerServerStatus.ForeColor = Color.Maroon;

            this.picHttpServerServerStatus.Image = this.m_frmParent.picLayerOffline.Image;
        }

        private void lnkHttpServerForwardedTest_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {

            CDownloadFile portTest = new CDownloadFile("http://www.phogue.net/procon/testport.php?port=" + this.txtHttpServerStartPort.Text);
            portTest.DownloadComplete += new CDownloadFile.DownloadFileEventDelegate(portTest_DownloadComplete);
            portTest.DownloadError += new CDownloadFile.DownloadFileEventDelegate(portTest_DownloadError);

            this.picHttpServerForwardedTestStatus.Image = this.m_frmParent.picAjaxStyleLoading.Image;
            //this.tmrPortCheckTester.Enabled = true;
            this.lnkHttpServerForwardedTest.Enabled = false;
            this.lblHttpServerForwardedTestStatus.Text = this.m_praApplication.CurrentLanguage.GetLocalized("frmOptions.lblHttpServerForwardedTestStatus.Running", this.txtHttpServerStartPort.Text);
            this.lblHttpServerForwardedTestStatus.ForeColor = Color.Black;

            portTest.BeginDownload();
        }

        private void portTest_DownloadError(CDownloadFile cdfSender) {
            this.picHttpServerForwardedTestStatus.Image = this.m_frmParent.picPortCheckerClosed.Image;

            this.lblHttpServerForwardedTestStatus.Text = this.m_praApplication.CurrentLanguage.GetLocalized("frmOptions.lblHttpServerForwardedTestStatus.Closed", this.txtHttpServerStartPort.Text);
            this.lblHttpServerForwardedTestStatus.ForeColor = Color.Maroon;

            this.lnkHttpServerForwardedTest.Enabled = true;
            this.lnkHttpServerExampleLink.Visible = false;
        }

        private void portTest_DownloadComplete(CDownloadFile cdfSender) {

            // Do not environment this \n.  It's from the php script and will always be just \n
            string[] a_strResponses = Encoding.UTF8.GetString(cdfSender.CompleteFileData).Split('\n');

            if (a_strResponses.Length >= 1) {
                if (a_strResponses[0].CompareTo("open") == 0) {
                    
                    this.picHttpServerForwardedTestStatus.Image = this.m_frmParent.picPortCheckerOpen.Image;

                    if (a_strResponses.Length >= 2) {
                        this.lblHttpServerForwardedTestStatus.Text = this.m_praApplication.CurrentLanguage.GetLocalized("frmOptions.lblHttpServerForwardedTestStatus.Open", this.txtHttpServerStartPort.Text, a_strResponses[1]);

                        this.lnkHttpServerExampleLink.Text = String.Format("http://{0}:{1}/connections", a_strResponses[1], this.txtHttpServerStartPort.Text);
                        this.lnkHttpServerExampleLink.Visible = true;
                    }
                    else {
                        this.lblHttpServerForwardedTestStatus.Text = this.m_praApplication.CurrentLanguage.GetLocalized("frmOptions.lblHttpServerForwardedTestStatus.Open", this.txtHttpServerStartPort.Text, "");
                    }
                    this.lblHttpServerForwardedTestStatus.ForeColor = Color.ForestGreen;

                    this.lnkHttpServerForwardedTest.Enabled = true;
                }
                else if (a_strResponses[0].CompareTo("closed") == 0 || a_strResponses[0].CompareTo("error") == 0 || a_strResponses[0].CompareTo("denied") == 0) {
                    this.picHttpServerForwardedTestStatus.Image = this.m_frmParent.picPortCheckerClosed.Image;

                    this.lblHttpServerForwardedTestStatus.Text = this.m_praApplication.CurrentLanguage.GetLocalized("frmOptions.lblHttpServerForwardedTestStatus.Closed", this.txtHttpServerStartPort.Text);
                    this.lblHttpServerForwardedTestStatus.ForeColor = Color.Maroon;

                    this.lnkHttpServerForwardedTest.Enabled = true;
                    this.lnkHttpServerExampleLink.Visible = false;
                }
            }
        }

        private void lnkHttpServerExampleLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start(this.lnkHttpServerExampleLink.Text);
        }

        # region Advanced

        void OptionsSettings_AdminMoveMessageChanged(bool blEnabled)
        {
            this.chkAdvEnableAdminMoveMsg.Checked = blEnabled;
        }

        private void chkAdvEnableAdminMoveMsg_CheckedChanged(object sender, EventArgs e)
        {
            this.m_praApplication.OptionsSettings.AdminMoveMessage = this.chkAdvEnableAdminMoveMsg.Checked;
        }

        void OptionsSettings_ChatDisplayAdminNameChanged(bool blEnabled)
        {
            this.chkAdvEnableChatAdminName.Checked = blEnabled;
        }

        private void chkAdvEnableChatAdminName_CheckedChanged(object sender, EventArgs e)
        {
            this.m_praApplication.OptionsSettings.ChatDisplayAdminName = this.chkAdvEnableChatAdminName.Checked;
        }

        void OptionsSettings_LayerHideLocalPluginsChanged(bool blEnabled) {
            this.chkAdvHideLocalPluginsTab.Checked = blEnabled;
        }

        private void chkAdvHideLocalPluginsTab_CheckedChanged(object sender, EventArgs e) {
            this.m_praApplication.OptionsSettings.LayerHideLocalPlugins = this.chkAdvHideLocalPluginsTab.Checked;
        }


        void OptionsSettings_LayerHideLocalAccountsChanged(bool blEnabled) {
            this.chkAdvHideLocalAccountsTab.Checked = blEnabled;
        }

        private void chkAdvHideLocalAccountsTab_CheckedChanged(object sender, EventArgs e) {
            this.m_praApplication.OptionsSettings.LayerHideLocalAccounts = this.chkAdvHideLocalAccountsTab.Checked;
        }
        
        void OptionsSettings_ShowRoundTimerConstantlyChanged(bool blEnabled)
        {
            this.chkAdvShowRoundTimerConstantly.Checked = blEnabled;
        }

        private void chkAdvShowRoundTimerConstantly_CheckedChanged(object sender, EventArgs e)
        {
            this.m_praApplication.OptionsSettings.ShowRoundTimerConstantly = this.chkAdvShowRoundTimerConstantly.Checked;
        }

        # endregion



    }
}
