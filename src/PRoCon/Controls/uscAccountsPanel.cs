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

using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace PRoCon {
    using Core;
    using Core.Accounts;
    using Core.Plugin;
    using Core.Remote.Layer;
    using Core.Remote;
    using PRoCon.Forms;

    public partial class uscAccountsPanel : UserControl {

        private frmMain m_frmMain;
        private uscServerConnection m_uscConnectionPanel;
        private CLocalization m_clocLanguage;

        private PRoConApplication m_praApplication;
        private PRoConClient m_prcClient;

        //private CPrivileges m_spPrivileges;

        // This variable is only used by this panel to show an example of the Layer Name.
        private string m_strServerName;
        public string ServerName {
            set { 
                this.m_strServerName = value;
                this.lblExampleLayerName.Text = this.txtLayerName.Text.Replace("%servername%", this.m_strServerName);
            }
        }

        public uscAccountsPanel() {
            InitializeComponent();

            //this.m_uscParentPanel = null;
            this.m_frmMain = null;
            this.m_clocLanguage = null;

            //this.m_spPrivileges = new CPrivileges();
            //this.m_spPrivileges.PrivilegesFlags = CPrivileges.FullPrivilegesFlags;

            this.uscPrivileges.OnUpdatePrivileges += new uscPrivilegesSelection.OnUpdatePrivilegesDelegate(uscPrivileges_OnUpdatePrivileges);
            this.uscPrivileges.OnCancelPrivileges += new uscPrivilegesSelection.OnCancelPrivilegesDelegate(uscPrivileges_OnCancelPrivileges);
        }

        private void uscAccountsPanel_Load(object sender, EventArgs e) {

            if (this.m_praApplication != null && this.m_prcClient != null) {
                foreach (Account accLoadedAccount in this.m_praApplication.AccountsList) {
                    this.AccountsList_AccountAdded(accLoadedAccount);

                    if (this.m_prcClient.Layer.AccountPrivileges.Contains(accLoadedAccount.Name) == true) {
                        this.uscAccountsPanel_AccountPrivilegesChanged(this.m_prcClient.Layer.AccountPrivileges[accLoadedAccount.Name]);
                    }
                }

                this.txtLayerName.Text = this.m_prcClient.Layer.LayerNameFormat;
                this.txtLayerBindingAddress.Text = this.m_prcClient.Layer.BindingAddress;
                this.txtLayerStartPort.Text = this.m_prcClient.Layer.ListeningPort.ToString();

                if (this.m_prcClient.Layer.IsLayerOnline == true) {
                    this.Layer_LayerOnline();
                }
            }
        }

        public void Initalize(frmMain frmMain, uscServerConnection uscConnectionPanel) {
            this.m_frmMain = frmMain;
            this.m_uscConnectionPanel = uscConnectionPanel;
            //this.m_uscParentPanel = uscParentPanel;

            this.picLayerForwardedTestStatus.Image = this.m_frmMain.picPortCheckerUnknown.Image; //  .iglPRoConLayerIcons.Images[uscServerConnection.INT_ICON_LAYERSERVER_PORTCHECK_UNKNOWN];
            this.picLayerServerStatus.Image = this.m_frmMain.picLayerOffline.Image; // .iglPRoConLayerIcons.Images[uscServerConnection.INT_ICON_LAYERSERVER_OFFLINE];

            this.pnlMainLayerServer.Dock = DockStyle.Fill;
            this.pnlStartPRoConLayer.Dock = DockStyle.Fill;
            this.pnlAccountPrivileges.Dock = DockStyle.Fill;

            this.lsvLayerAccounts.SmallImageList = this.m_frmMain.iglIcons;
        }

        public void SetConnection(PRoConApplication praApplication, PRoConClient prcClient) {
            if ((this.m_praApplication = praApplication) != null && (this.m_prcClient = prcClient) != null) {
                //this.m_prcClient.ProconPrivileges += new PRoConClient.ProconPrivilegesHandler(m_prcClient_ProconPrivileges);

                if (this.m_prcClient.Game != null) {
                    this.m_prcClient_GameTypeDiscovered(prcClient);
                }
                else {
                    this.m_prcClient.GameTypeDiscovered += new PRoConClient.EmptyParamterHandler(m_prcClient_GameTypeDiscovered);
                }
            }
        }

        void m_prcClient_GameTypeDiscovered(PRoConClient sender) {

            this.m_praApplication.AccountsList.AccountAdded += new PRoCon.Core.Accounts.AccountDictionary.AccountAlteredHandler(AccountsList_AccountAdded);
            this.m_praApplication.AccountsList.AccountRemoved += new PRoCon.Core.Accounts.AccountDictionary.AccountAlteredHandler(AccountsList_AccountRemoved);

            foreach (Account acAccount in this.m_praApplication.AccountsList) {
                acAccount.AccountPasswordChanged += new Account.AccountPasswordChangedHandler(acAccount_AccountPasswordChanged);

                if (this.m_prcClient.Layer.AccountPrivileges.Contains(acAccount.Name) == true) {
                    this.m_prcClient.Layer.AccountPrivileges[acAccount.Name].AccountPrivilegesChanged += new AccountPrivilege.AccountPrivilegesChangedHandler(uscAccountsPanel_AccountPrivilegesChanged);
                }
            }

            this.m_prcClient.Layer.LayerOnline += new PRoConLayer.LayerEmptyParameterHandler(Layer_LayerOnline);
            this.m_prcClient.Layer.LayerOffline += new PRoConLayer.LayerEmptyParameterHandler(Layer_LayerOffline);
            this.m_prcClient.Layer.LayerSocketError += new PRoConLayer.LayerSocketErrorHandler(Layer_LayerSocketError);

            this.m_prcClient.Layer.ClientConnected += new PRoConLayer.LayerAccountHandler(Layer_ClientConnected);
        }

        void Layer_ClientConnected(PRoConLayerClient client) {
            client.Login += new PRoConLayerClient.LayerClientHandler(client_LayerClientLogin);
            client.Logout += new PRoConLayerClient.LayerClientHandler(client_LayerClientLogout);
        }

        //public void m_prcClient_ProconPrivileges(CPrivileges spPrivs) {
        //    this.m_spPrivileges = spPrivs;
        //}

        public void SetLocalization(CLocalization clocLanguage) {

            if ((this.m_clocLanguage = clocLanguage) != null) {

                //this.tbpPRoConLayer.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.Title", null);

                this.lblLayerServerSetupTitle.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerServerSetupTitle", null);
                this.lnkStartStopLayer.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lnkStartStopLayer.Start", null);
                //this.lnkStartStopLayer.LinkArea = new LinkArea(0, this.lnkStartStopLayer.Text.Length);
                this.lblLayerServerStatus.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerServerStatus.Offline", null);

                this.lnkLayerForwardedTest.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lnkLayerForwardedTest", null);
                //this.lnkLayerForwardedTest.LinkArea = new LinkArea(0, this.lnkLayerForwardedTest.Text.Length);
                this.lblLayerForwardedTestStatus.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerForwardedTestStatus.Unknown", null);


                this.lnkWhatIsPRoConLayer.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lnkWhatIsPRoConLayer", null);
                //this.lnkWhatIsPRoConLayer.LinkArea = new LinkArea(0, this.lnkWhatIsPRoConLayer.Text.Length);

                this.lblLayerAssignAccountPrivilegesTitle.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerAssignAccountPrivilegesTitle", null);

                this.colAccounts.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lstLayerAccounts.colAccounts", null);
                this.colPrivileges.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lstLayerAccounts.colPrivileges", null);
                this.colRConAccess.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lstLayerAccounts.colRConAccess", null);
                this.colIPPort.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lstLayerAccounts.colIPPort", null);

                this.lnkManageAccounts.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lnkManageAccounts", null);
                //this.lnkManageAccounts.LinkArea = new LinkArea(0, this.lnkManageAccounts.Text.Length);

                this.lblLayerStartTitle.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerStartTitle", null);
                this.lblLayerStartPort.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerStartPort", null);
                this.lblLayerBindingIP.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerBindingIP", null);
                this.lblBindingExplanation.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblBindingExplanation", null);
                this.lblLayerName.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerName", null);
                this.btnInsertName.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.btnInsertName", null);
                this.lblExampleLayerName.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblExampleLayerName", new string[] { this.txtLayerName.Text.Replace("%servername%", this.m_strServerName) });
                this.btnLayerStart.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.btnLayerStart", null);
                this.btnCancelLayerStart.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.btnCancelLayerStart", null);

                this.uscPrivileges.SetLocalization(this.m_clocLanguage);

                this.RefreshLayerPrivilegesPanel();

            }
        }

        public delegate void ManageAccountsRequestDelegate(object sender, EventArgs e);
        public event ManageAccountsRequestDelegate ManageAccountsRequest;

        private UInt16 m_ui16LayerListenerPort = 27260;

        private TcpListener m_tclLayerListener = null;

        private bool m_blEditingPrivileges = false;
        private void ShowLayerPanel(Panel pnlShow) {
            this.pnlMainLayerServer.Hide();
            this.pnlStartPRoConLayer.Hide();
            this.pnlAccountPrivileges.Hide();

            this.m_blEditingPrivileges = false;

            if (pnlShow == this.pnlMainLayerServer) {
                this.lsvLayerAccounts.SelectedItems.Clear();
            }
            else if (pnlShow == this.pnlAccountPrivileges) {
                this.m_blEditingPrivileges = true;

                // Should be but still..
                if (this.lsvLayerAccounts.SelectedItems.Count > 0) {
                    this.uscPrivileges.AccountName = this.lsvLayerAccounts.SelectedItems[0].Text;
                }

                if (this.lsvLayerAccounts.SelectedItems.Count > 0) {
                    CPrivileges spPrivs = (CPrivileges)this.lsvLayerAccounts.SelectedItems[0].SubItems[1].Tag;

                    this.uscPrivileges.Privileges = spPrivs;
                }
                else {
                    this.uscPrivileges.Privileges = new CPrivileges();
                }
            }

            pnlShow.Show();
        }

        private void RefreshLayerPrivilegesPanel() {
            foreach (ListViewItem lviItem in this.lsvLayerAccounts.Items) {
                if (lviItem.SubItems[1].Tag != null && this.m_clocLanguage != null && this.m_prcClient != null) {

                    if (this.m_prcClient.Layer.AccountPrivileges.Contains(lviItem.Text) == true) {
                        CPrivileges spDetails = this.m_prcClient.Layer.AccountPrivileges[lviItem.Text].Privileges;
                    
                        if (spDetails.HasNoRconAccess == true) {
                            lviItem.SubItems["rconaccess"].Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lstLayerAccounts.Privileges.None", null);
                        }
                        else if (spDetails.HasLimitedRconAccess == true) {
                            lviItem.SubItems["rconaccess"].Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lstLayerAccounts.Privileges.Limited", null);
                        }
                        else {
                            lviItem.SubItems["rconaccess"].Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lstLayerAccounts.Privileges.Full", null);
                        }

                        if (spDetails.HasNoLocalAccess == true) {
                            lviItem.SubItems["localaccess"].Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lstLayerAccounts.Privileges.None", null);
                        }
                        else if (spDetails.HasLimitedLocalAccess == true) {
                            lviItem.SubItems["localaccess"].Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lstLayerAccounts.Privileges.Limited", null);
                        }
                        else {
                            lviItem.SubItems["localaccess"].Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lstLayerAccounts.Privileges.Full", null);
                        }
                    }
                }
            }

            if (this.m_blEditingPrivileges == true) {
                this.ShowLayerPanel(this.pnlAccountPrivileges);
            }

            foreach (ColumnHeader ch in this.lsvLayerAccounts.Columns) {
                ch.Width = -2;
            }
        }


        void uscAccountsPanel_AccountPrivilegesChanged(AccountPrivilege item) {

            if (this.lsvLayerAccounts.Items.ContainsKey(item.Owner.Name) == true) {
                ListViewItem lviAccount = this.lsvLayerAccounts.Items[item.Owner.Name];

                lviAccount.SubItems["rconaccess"].Tag = item.Privileges;

                this.RefreshLayerPrivilegesPanel();
            }
        }

        void acAccount_AccountPasswordChanged(Account item) {

            if (this.lsvLayerAccounts.Items.ContainsKey(item.Name) == true) {
                this.lsvLayerAccounts.Items[item.Name].Tag = item.Password;
            }
        }

        void AccountsList_AccountRemoved(Account item) {
            item.AccountPasswordChanged -= new Account.AccountPasswordChangedHandler(acAccount_AccountPasswordChanged);

            if (this.lsvLayerAccounts.Items.ContainsKey(item.Name) == true) {

                //foreach (KeyValuePair<string, CPRoConLayerClient> kvpConnection in this.m_dicLayerClients) {
                //    kvpConnection.Value.OnAccountDeleted(e.AccountName);
                //}

                this.lsvLayerAccounts.Items.Remove(this.lsvLayerAccounts.Items[item.Name]);
            }
        }

        void AccountsList_AccountAdded(Account item) {
            item.AccountPasswordChanged += new Account.AccountPasswordChangedHandler(acAccount_AccountPasswordChanged);

            if (this.lsvLayerAccounts.Items.ContainsKey(item.Name) == false) {

                ListViewItem lviNewAccount = new ListViewItem(item.Name);
                lviNewAccount.Name = item.Name;
                lviNewAccount.Tag = item.Password;
                lviNewAccount.ImageKey = "status_offline.png";

                ListViewItem.ListViewSubItem lsviNewSubitem = new ListViewItem.ListViewSubItem();
                //lsviNewSubitem.Text = "none";
                lsviNewSubitem.Name = "rconaccess";
                lsviNewSubitem.Tag = new CPrivileges();
                lviNewAccount.SubItems.Add(lsviNewSubitem);

                lsviNewSubitem = new ListViewItem.ListViewSubItem();
                //lsviNewSubitem.Text = "none";
                lsviNewSubitem.Name = "localaccess";
                lviNewAccount.SubItems.Add(lsviNewSubitem);

                lsviNewSubitem = new ListViewItem.ListViewSubItem();
                lsviNewSubitem.Text = String.Empty;
                lsviNewSubitem.Name = "ip";
                lviNewAccount.SubItems.Add(lsviNewSubitem);

                this.lsvLayerAccounts.Items.Add(lviNewAccount);

                //foreach (KeyValuePair<string, CPRoConLayerClient> kvpConnection in this.m_dicLayerClients) {
                //    kvpConnection.Value.OnAccountCreated(e.AccountName);
                //}

                this.RefreshLayerPrivilegesPanel();
            }
        }
        
        //public event DispatchEventDelegate LayerServerManuallyStopped;
        private void lnkStartStopLayer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {

            if (this.m_prcClient.Layer.IsLayerOnline == false) {
                this.ShowLayerPanel(this.pnlStartPRoConLayer);
            }
            else {
                this.m_prcClient.Layer.LayerEnabled = false;
                //this.m_blLayerEnabled = false;

                this.m_prcClient.Layer.ShutdownLayerListener();

                //if (this.LayerServerManuallyStopped != null) {
                //    this.LayerServerManuallyStopped();
                //}
            }
        }

        //public event DispatchEventDelegate LayerServerManuallyStarted;
        private void btnLayerStart_Click(object sender, EventArgs e) {
            this.ShowLayerPanel(this.pnlMainLayerServer);

            this.m_prcClient.Layer.LayerEnabled = true;
            this.m_prcClient.Layer.BindingAddress = this.txtLayerBindingAddress.Text;
            this.m_prcClient.Layer.ListeningPort = Convert.ToUInt16(this.txtLayerStartPort.Text);
            this.m_prcClient.Layer.LayerNameFormat = this.txtLayerName.Text;
            //this.m_ui16LayerListenerPort = Convert.ToUInt16(this.txtLayerStartPort.Text);
            this.m_prcClient.Layer.StartLayerListener();

            //if (this.LayerServerManuallyStarted != null) {
            //    this.LayerServerManuallyStarted();
            //}
        }

        private void btnCancelLayerStart_Click(object sender, EventArgs e) {
            this.ShowLayerPanel(this.pnlMainLayerServer);
        }

        private void lstLayerAccounts_SelectedIndexChanged(object sender, EventArgs e) {

            if (this.lsvLayerAccounts.SelectedItems.Count > 0) {

                this.ShowLayerPanel(this.pnlAccountPrivileges);
            }
        }

        void uscPrivileges_OnCancelPrivileges() {
            this.ShowLayerPanel(this.pnlMainLayerServer);
        }

        void uscPrivileges_OnUpdatePrivileges(string strAccountName, CPrivileges spUpdatedPrivs) {
            //this.SetLayerAccountPrivileges(strAccountName, spUpdatedPrivs);

            if (this.m_prcClient.Layer.AccountPrivileges.Contains(strAccountName) == true) {
                this.m_prcClient.Layer.AccountPrivileges[strAccountName].SetPrivileges(spUpdatedPrivs);
            }

            this.ShowLayerPanel(this.pnlMainLayerServer);

            this.RefreshLayerPrivilegesPanel();
        }

        private void lnkManageAccounts_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            this.ManageAccountsRequest(this, new EventArgs());
        }

        private void lnkLayerForwardedTest_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            this.PortCheck("http://www.phogue.net/procon/testport.php?port=" + this.txtLayerStartPort.Text);
        }

        #region Layer Events and Helper Methods

        public delegate void UpdatePortCheckerFeedbackDelegate(string strCheckerFeedback);
        private void OnPortCheckerFeedback(string strCheckerFeedback) {
            this.Invoke(new UpdatePortCheckerFeedbackDelegate(OnPortCheckerFeedback_Callback), new object[] { strCheckerFeedback });
        }
        private void OnPortCheckerFeedback_Callback(string strCheckerFeedback) {

            // Do not environment this \n.  It's from the php script and will always be just \n
            string[] a_strResponses = strCheckerFeedback.Split('\n');

            if (a_strResponses.Length >= 1) {
                if (a_strResponses[0].CompareTo("open") == 0) {
                    //this.picLayerForwardedTestStatus.Image = this.m_frmParent.iglPRoConLayerIcons.Images[uscServerConnection.INT_ICON_LAYERSERVER_PORTCHECK_OPEN];
                    this.picLayerForwardedTestStatus.Image = this.m_frmMain.picPortCheckerOpen.Image;
                    if (a_strResponses.Length >= 2) {
                        this.lblLayerForwardedTestStatus.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerForwardedTestStatus.Open", new string[] { this.m_prcClient.Layer.ListeningPort.ToString(), a_strResponses[1] });
                    }
                    else {
                        this.lblLayerForwardedTestStatus.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerForwardedTestStatus.Open", new string[] { this.m_prcClient.Layer.ListeningPort.ToString(), String.Empty });
                    }
                    this.lblLayerForwardedTestStatus.ForeColor = Color.ForestGreen;
                    //this.tmrPortCheckTester.Enabled = false;
                    this.lnkLayerForwardedTest.Enabled = true;
                }
                else if (a_strResponses[0].CompareTo("closed") == 0 || strCheckerFeedback.CompareTo("error") == 0 || a_strResponses[0].CompareTo("denied") == 0) {
                    this.picLayerForwardedTestStatus.Image = this.m_frmMain.picPortCheckerClosed.Image;
                    //this.picLayerForwardedTestStatus.Image = this.m_frmParent.iglPRoConLayerIcons.Images[uscServerConnection.INT_ICON_LAYERSERVER_PORTCHECK_CLOSED];
                    this.lblLayerForwardedTestStatus.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerForwardedTestStatus.Closed", new string[] { this.m_prcClient.Layer.ListeningPort.ToString() });
                    this.lblLayerForwardedTestStatus.ForeColor = Color.Maroon;
                    //this.tmrPortCheckTester.Enabled = false;
                    this.lnkLayerForwardedTest.Enabled = true;
                }
                else if (a_strResponses[0].CompareTo("checking") == 0) {
                    this.picLayerForwardedTestStatus.Image = this.m_frmMain.picAjaxStyleLoading.Image;
                    //this.tmrPortCheckTester.Enabled = true;
                    this.lnkLayerForwardedTest.Enabled = false;
                    this.lblLayerForwardedTestStatus.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerForwardedTestStatus.Running", new string[] { this.m_prcClient.Layer.ListeningPort.ToString() });
                    this.lblLayerForwardedTestStatus.ForeColor = Color.Black;
                }
            }
        }

        private void Layer_LayerOnline() {
            //this.picLayerServerStatus.Image = this.m_frmParent.iglPRoConLayerIcons.Images[uscServerConnection.INT_ICON_LAYERSERVER_ONLINE];
            this.picLayerServerStatus.Image = this.m_frmMain.picLayerOnline.Image;

            this.lblLayerServerStatus.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerServerStatus.Online", new string[] { this.m_prcClient.Layer.ListeningPort.ToString() });
            this.lblLayerServerStatus.ForeColor = Color.ForestGreen;

            this.lnkStartStopLayer.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lnkStartStopLayer.Stop", null);
            //this.lnkStartStopLayer.LinkArea = new LinkArea(0, this.lnkStartStopLayer.Text.Length);

            this.pnlLayerServerTester.Visible = true;
            //this.picLayerForwardedTestStatus.Image = this.m_frmParent.iglPRoConLayerIcons.Images[uscServerConnection.INT_ICON_LAYERSERVER_PORTCHECK_UNKNOWN];
            this.picLayerForwardedTestStatus.Image = this.m_frmMain.picPortCheckerUnknown.Image;
            this.lblLayerForwardedTestStatus.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerForwardedTestStatus.Unknown", null);

        }

        private void Layer_LayerOffline() {
            this.picLayerServerStatus.Image = this.m_frmMain.picLayerOffline.Image;//this.m_frmParent.iglPRoConLayerIcons.Images[uscServerConnection.INT_ICON_LAYERSERVER_OFFLINE];

            this.lblLayerServerStatus.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerServerStatus.Offline", null);
            this.lblLayerServerStatus.ForeColor = Color.Maroon;

            this.lnkStartStopLayer.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lnkStartStopLayer.Start", null);
            //this.lnkStartStopLayer.LinkArea = new LinkArea(0, this.lnkStartStopLayer.Text.Length);

            this.pnlLayerServerTester.Visible = false;
        }

        private void Layer_LayerSocketError(SocketException se) {
            //this.ShutdownLayerListener();

            this.picLayerServerStatus.Image = this.m_frmMain.picLayerOffline.Image;
            //this.picLayerServerStatus.Image = this.m_frmParent.iglPRoConLayerIcons.Images[uscServerConnection.INT_ICON_LAYERSERVER_OFFLINE];

            this.lblLayerServerStatus.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lblLayerServerStatus.Error", new string[] { se.Message });
            this.lblLayerServerStatus.ForeColor = Color.Maroon;

            this.lnkStartStopLayer.Text = this.m_clocLanguage.GetLocalized("uscAccountsPanel.lnkStartStopLayer.Start", null);
            //this.lnkStartStopLayer.LinkArea = new LinkArea(0, this.lnkStartStopLayer.Text.Length);

            this.pnlLayerServerTester.Visible = false;
        }

        void client_LayerClientLogin(PRoConLayerClient sender) {

            if (this.lsvLayerAccounts.Items.ContainsKey(sender.Username) == true) {
                // TO DO: Change Icon

                // TODO: Fix
                //this.m_uscConnectionPanel.ThrowEvent(this, uscEventsPanel.CapturableEvents.AccountLogin, new string[] { strUsername, sender.ClientIPPort });

                if (this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Tag == null) {
                    this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Tag = new List<string>() { sender.IPPort };
                }
                else {
                    ((List<string>)this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Tag).Add(sender.IPPort);
                }

                this.lsvLayerAccounts.Items[sender.Username].ImageKey = "status_online.png";

                this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Text = String.Format("({0} CMD/EVNT) ", Math.Floor(((List<string>)this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Tag).Count / 2.0));

                for (int i = 0; i < ((List<string>)this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Tag).Count; i++) {
                    if (i > 0) {
                        this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Text += ", ";
                    }

                    this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Text += ((List<string>)this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Tag)[i];
                }

                //this.lsvLayerAccounts.Items[strUsername].SubItems["ip"].Text = sender.ClientIPPort;
            }
        }

        private void client_LayerClientLogout(PRoConLayerClient sender) {

            if (this.lsvLayerAccounts.Items.ContainsKey(sender.Username) == true) {
                // TO DO: Change Icon
                // TODO: Fix
                //this.m_uscConnectionPanel.ThrowEvent(this, uscEventsPanel.CapturableEvents.AccountLogout, new string[] { strUsername });

                if (this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Tag != null) {

                    ((List<string>)this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Tag).Remove(sender.IPPort);

                    if (Math.Floor(((List<string>)this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Tag).Count / 2.0) > 0) {
                        this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Text = String.Format("({0} CMD/EVNT) ", Math.Floor(((List<string>)this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Tag).Count / 2.0));
                    }
                    else {
                        this.lsvLayerAccounts.Items[sender.Username].ImageKey = "status_offline.png";
                        this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Text = String.Empty;
                    }

                    for (int i = 0; i < ((List<string>)this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Tag).Count; i++) {
                        if (i > 0) {
                            this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Text += ", ";
                        }

                        this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Text += ((List<string>)this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Tag)[i];
                    }
                }
                else {
                    this.lsvLayerAccounts.Items[sender.Username].SubItems["ip"].Text = String.Empty;
                }
            }
        }

        #endregion

        #region Port forward checker

        private struct SAsyncRequestState {
            public HttpWebRequest WebRequest;
            public uscAccountsPanel uscAccounts;
        }

        private void PortCheckTimeoutCallback(object objState, bool blTimedOut) {
            if (blTimedOut) {
                HttpWebRequest request = ((SAsyncRequestState)objState).WebRequest;
                if (request != null) {
                    //    Console.WriteLine("[" + DateTime.Now.ToString("HH:mm:ss ff") + "] PostCallback -> http://phogue.net Timedout.");
                    request.Abort();
                    ((SAsyncRequestState)objState).uscAccounts.OnPortCheckerFeedback("error");
                }
            }
        }

        private static void PortCheckResponseCallback(IAsyncResult asynchronousResult) {
            try {

                WebResponse wbResponse = ((SAsyncRequestState)asynchronousResult.AsyncState).WebRequest.EndGetResponse(asynchronousResult);

                StreamReader stmReadCheck = new StreamReader(wbResponse.GetResponseStream(), Encoding.UTF8);

                string strResponse = stmReadCheck.ReadToEnd();

                ((SAsyncRequestState)asynchronousResult.AsyncState).uscAccounts.OnPortCheckerFeedback(strResponse);

                stmReadCheck.Close();
                wbResponse.Close();
            }
            catch (WebException) {
                ((SAsyncRequestState)asynchronousResult.AsyncState).uscAccounts.OnPortCheckerFeedback("error");
            }
        }

        private static void PortCheckPostCallback(IAsyncResult asynchronousResult) {

            try {

                HttpWebRequest request = ((SAsyncRequestState)asynchronousResult.AsyncState).WebRequest;

                // End the operation.
                Stream postStream = request.EndGetRequestStream(asynchronousResult);

                // Convert the string into a byte array.
                //byte[] byteArray = Encoding.UTF8.GetBytes(((SAsyncRequestState)asynchronousResult.AsyncState).strPostData);
                // Write to the request stream.
                //postStream.Write(byteArray, 0, ((SAsyncRequestState)asynchronousResult.AsyncState).strPostData.Length);
                postStream.Close();

                request.BeginGetResponse(new AsyncCallback(PortCheckResponseCallback), asynchronousResult.AsyncState);
            }
            catch (WebException) {
                ((SAsyncRequestState)asynchronousResult.AsyncState).uscAccounts.OnPortCheckerFeedback("error");
            }
        }

        public void PortCheck(string strUrl) {

            this.OnPortCheckerFeedback("checking");

            // Open a connection
            HttpWebRequest WebRequestObject = (HttpWebRequest)HttpWebRequest.Create(strUrl);
            //SAsyncRequestState arsPostData = new SAsyncRequestState();

            // You can also specify additional header values like 
            // the user agent or the referer:
            WebRequestObject.UserAgent = ".NET Framework/2.0";
            WebRequestObject.Referer = "http://prconserver.phogue.net";
            //WebRequestObject.KeepAlive = false;

            // Set values for the request back 
            WebRequestObject.Method = "POST";
            WebRequestObject.ContentType = "application/x-www-form-urlencoded";
            WebRequestObject.Proxy = null;

            //arsPostData.WebRequest = WebRequestObject;

            SAsyncRequestState sarState = new SAsyncRequestState();
            sarState.uscAccounts = this;
            sarState.WebRequest = WebRequestObject;

            IAsyncResult iarResult = WebRequestObject.BeginGetRequestStream(new AsyncCallback(PortCheckPostCallback), sarState);

            // Set a timeout on our Request for a stream.  Timeout in 20 seconds.
            ThreadPool.RegisterWaitForSingleObject(iarResult.AsyncWaitHandle, new WaitOrTimerCallback(PortCheckTimeoutCallback), sarState, 15000, true);
        }

        #endregion

        private void txtLayerName_TextChanged(object sender, EventArgs e) {
            this.lblExampleLayerName.Text = this.txtLayerName.Text.Replace("%servername%", this.m_strServerName);
        }

        private void btnInsertName_Click(object sender, EventArgs e) {

            int iInsertPosition = this.txtLayerName.SelectionStart;

            this.txtLayerName.Text = this.txtLayerName.Text.Remove(iInsertPosition, this.txtLayerName.SelectionLength);
            this.txtLayerName.Text = this.txtLayerName.Text.Insert(iInsertPosition, "%servername%");
        }

        private void lnkWhatIsPRoConLayer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start("http://phogue.net/procon/whatis.php");
        }

        private void txtLayerStartPort_KeyPress(object sender, KeyPressEventArgs e) {
            e.Handled = (!char.IsDigit(e.KeyChar) && e.KeyChar != '\b');
        }

    }
}
