// Copyright 2010 Geoffrey 'Phogue' Green
// 
// http://www.phogue.net
//  
// This file is part of PRoCon Frostbite.
// 
// PRoCon Frostbite is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// PRoCon Frostbite is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with PRoCon Frostbite.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Xml;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

namespace PRoCon.Controls {
    using PRoCon.Core;
    using PRoCon.Core.Remote;
    using PRoCon.Core.Remote.Layer;
    using PRoCon.Core.Packages;
    
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class uscStartPage : uscPage {

        public delegate void ConnectionPageHandler(string hostNamePort);
        public event ConnectionPageHandler ConnectionPage;

        private PRoConApplication m_proconApplication;

        private CLocalization m_startPageTemplates;

        private RssUserSummaryItem m_rssUserSummaryItem;

        private bool m_isDocumentReady;
        private XmlDocument m_previousDocument;

        private CLocalization m_language;

        public uscStartPage(PRoConApplication proconApplication) {
            this.m_isDocumentReady = false;

            this.m_proconApplication = proconApplication;
            this.m_proconApplication.Connections.ConnectionAdded += new ConnectionDictionary.ConnectionAlteredHandler(Connections_ConnectionAdded);
            this.m_proconApplication.Connections.ConnectionRemoved += new ConnectionDictionary.ConnectionAlteredHandler(Connections_ConnectionRemoved);

            this.m_proconApplication.PackageManager.PackageDownloaded += new PackageManager.PackageEventHandler(PackageManager_PackageDownloaded);
            this.m_proconApplication.PackageManager.PackageAwaitingRestart += new PackageManager.PackageEventHandler(PackageManager_PackageAwaitingRestart);
            this.m_proconApplication.PackageManager.PackageDownloadFail += new PackageManager.PackageEventHandler(PackageManager_PackageDownloadFail);
            this.m_proconApplication.PackageManager.RemotePackagesUpdated += new PackageManager.PackageManagerEventHandler(PackageManager_RemotePackagesUpdated);
            this.m_proconApplication.PackageManager.PackageBeginningDownload += new PackageManager.PackageEventHandler(PackageManager_PackageBeginningDownload);
            this.m_proconApplication.PackageManager.PackageInstalling += new PackageManager.PackageEventHandler(PackageManager_PackageInstalling);

            this.m_proconApplication.BeginRssUpdate += new PRoConApplication.EmptyParameterHandler(m_proconApplication_BeginRssUpdate);
            this.m_proconApplication.RssUpdateError += new PRoConApplication.EmptyParameterHandler(m_proconApplication_RssUpdateError);
            this.m_proconApplication.RssUpdateSuccess += new PRoConApplication.RssHandler(m_proconApplication_RssUpdateSuccess);

            InitializeComponent();
        }

        public override void SetLocalization(CLocalization clocLanguage) {
            base.SetLocalization(clocLanguage);

            this.m_language = clocLanguage;

            if (this.m_isDocumentReady == true) {

                // My Connections
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblMyConnections", clocLanguage.GetDefaultLocalized("My Connections", "pStartPage-lblMyConnections") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblNoConnections", clocLanguage.GetDefaultLocalized("You do not have any connections.  Get started by creating a connection to your game server or layer", "pStartPage-lblNoConnections") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblCreateConnection", clocLanguage.GetDefaultLocalized("Create Connection", "pStartPage-lblCreateConnection") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblCreateConnectionLegend", clocLanguage.GetDefaultLocalized("Create Connection", "pStartPage-lblCreateConnectionLegend") });

                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblConnectionsHostnameIp", clocLanguage.GetDefaultLocalized("Hostname/IP", "pStartPage-lblConnectionsHostnameIp") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblConnectionsPort", clocLanguage.GetDefaultLocalized("Port", "pStartPage-lblConnectionsPort") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblConnectionsUsername", clocLanguage.GetDefaultLocalized("Username", "pStartPage-lblConnectionsUsername") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblConnectionsUsernameExplanation", clocLanguage.GetDefaultLocalized("You only require a username to login to a PRoCon layer", "pStartPage-lblConnectionsUsernameExplanation") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblConnectionsPassword", clocLanguage.GetDefaultLocalized("Password", "pStartPage-lblConnectionsPassword") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblCancelCreateConnection", clocLanguage.GetDefaultLocalized("Cancel", "pStartPage-lblCancelCreateConnection") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblConnectCreateConnection", clocLanguage.GetDefaultLocalized("Connect", "pStartPage-lblConnectCreateConnection") });

                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-tabPhogueNetNewsFeed", clocLanguage.GetDefaultLocalized("News Feed", "pStartPage-tabPhogueNetNewsFeed") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-tabPackages", clocLanguage.GetDefaultLocalized("Packages", "pStartPage-tabPackages") });

                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblDonate", clocLanguage.GetDefaultLocalized("Donate", "pStartPage-lblDonate") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblDonationAmount", clocLanguage.GetDefaultLocalized("Donation Amount", "pStartPage-lblDonationAmount") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblDonationAmount-Currency", clocLanguage.GetDefaultLocalized("Currency: USD", "pStartPage-lblDonationAmount-Currency") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblRecognize", clocLanguage.GetDefaultLocalized("I want my kudos!", "pStartPage-lblRecognize") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-chkRecognize-lblShowOnWall", clocLanguage.GetDefaultLocalized("Show on Wall", "pStartPage-chkRecognize-lblShowOnWall") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-chkRecognize-optDoNotShow", clocLanguage.GetDefaultLocalized("Do not show any information", "pStartPage-chkRecognize-optDoNotShow") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-chkRecognize-optAmountDetailsComments", clocLanguage.GetDefaultLocalized("Amount, Details &amp; Comments", "pStartPage-chkRecognize-optAmountDetailsComments") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-chkRecognize-optDetailsCommentsOnly", clocLanguage.GetDefaultLocalized("User Details &amp; Comments Only", "pStartPage-chkRecognize-optDetailsCommentsOnly") });

                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-chkRecognize-lblName", clocLanguage.GetDefaultLocalized("Name", "pStartPage-chkRecognize-lblName") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-chkRecognize-lblEmail", clocLanguage.GetDefaultLocalized("Email", "pStartPage-chkRecognize-lblEmail") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-chkRecognize-lblWebsite", clocLanguage.GetDefaultLocalized("Website", "pStartPage-chkRecognize-lblWebsite") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-chkRecognize-lblComments", clocLanguage.GetDefaultLocalized("Comments", "pStartPage-chkRecognize-lblComments") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-chkRecognize-lblRecognizeTime", clocLanguage.GetDefaultLocalized("Your message may take up to an hour to appear on the wall.", "pStartPage-chkRecognize-lblRecognizeTime") });

                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblMonthlySummary", clocLanguage.GetDefaultLocalized("Monthly Summary", "pStartPage-lblMonthlySummary") });
                this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblDonationWall", clocLanguage.GetDefaultLocalized("Donation Wall", "pStartPage-lblDonationWall") });

                this.webBrowser1.Document.InvokeScript("fnSetVariableLocalization", new string[] { "m_pStartPage_lblDeleteConnection", clocLanguage.GetDefaultLocalized("Are you sure you want <br/>to delete this connection?", "m_pStartPage_lblDeleteConnection") });

                ArrayList tableHeaders = new ArrayList();
                tableHeaders.Add("");
                tableHeaders.Add(clocLanguage.GetDefaultLocalized("UID", "pStartPage-tblPackages-thUid"));
                tableHeaders.Add(clocLanguage.GetDefaultLocalized("Type", "pStartPage-tblPackages-thType"));
                tableHeaders.Add(clocLanguage.GetDefaultLocalized("Name", "pStartPage-tblPackages-thName"));
                tableHeaders.Add(clocLanguage.GetDefaultLocalized("Version", "pStartPage-tblPackages-thVersion"));
                tableHeaders.Add(clocLanguage.GetDefaultLocalized("Last Update", "pStartPage-tblPackages-thLastUpdate"));
                tableHeaders.Add(clocLanguage.GetDefaultLocalized("Status", "pStartPage-tblPackages-thStatus"));
                tableHeaders.Add(clocLanguage.GetDefaultLocalized("Downloads", "pStartPage-tblPackages-thDownloads"));
                tableHeaders.Add("");
                tableHeaders.Add(clocLanguage.GetDefaultLocalized("Discuss/Feedback", "pStartPage-tblPackages-thDiscussFeedback"));
                tableHeaders.Add(clocLanguage.GetDefaultLocalized("Author", "pStartPage-tblPackages-thAuthor"));
                tableHeaders.Add("");
                tableHeaders.Add("");
                tableHeaders.Add(clocLanguage.GetDefaultLocalized("Description", "pStartPage-tblPackages-thDescription"));
                tableHeaders.Add("");
                tableHeaders.Add("");
                tableHeaders.Add(clocLanguage.GetDefaultLocalized("Layer Install Status", "pStartPage-tblPackages-thLayerInstallStatus"));
                tableHeaders.Add(clocLanguage.GetDefaultLocalized("Install Package", "pStartPage-tblPackages-thInstallPackage"));
                this.webBrowser1.Document.InvokeScript("fnSetTableHeadersLocalization", new string[] { "pStartPage-tblPackages", JSON.JsonEncode(tableHeaders) });
            }
        }

        private void UpdateConnections() {

            ArrayList connectionsArray = new ArrayList();

            int playerCount = 0, playerSlotsTotal = 0;

            if (this.m_startPageTemplates != null && this.m_proconApplication != null) {
                foreach (PRoConClient client in this.m_proconApplication.Connections) {

                    Hashtable connectionHtml = new Hashtable();

                    string replacedTemplate = String.Empty;

                    if (client.State == ConnectionState.Connected == true && client.IsLoggedIn == true) {
                        if (client.CurrentServerInfo != null) {
                            replacedTemplate = this.m_startPageTemplates.GetLocalized("connections.online");
                        }
                        else {
                            replacedTemplate = this.m_startPageTemplates.GetLocalized("connections.online.noInfo");
                        }
                    }
                    else if (client.State == ConnectionState.Connecting || (client.State == ConnectionState.Connected && client.IsLoggedIn == false)) {
                        replacedTemplate = this.m_startPageTemplates.GetLocalized("connections.connect-attempt.noInfo");
                    }
                    else if (client.State == ConnectionState.Error) {
                        if (client.CurrentServerInfo != null) {
                            replacedTemplate = this.m_startPageTemplates.GetLocalized("connections.error");
                        }
                        else {
                            replacedTemplate = this.m_startPageTemplates.GetLocalized("connections.error.noInfo");
                        }
                    }
                    else {
                        if (client.CurrentServerInfo != null) {
                            replacedTemplate = this.m_startPageTemplates.GetLocalized("connections.offline");
                        }
                        else {
                            replacedTemplate = this.m_startPageTemplates.GetLocalized("connections.offline.noInfo");
                        }
                    }

                    replacedTemplate = replacedTemplate.Replace("%connections.online.options%", this.m_startPageTemplates.GetLocalized("connections.online.options"));
                    replacedTemplate = replacedTemplate.Replace("%connections.offline.options%", this.m_startPageTemplates.GetLocalized("connections.offline.options"));

                    if (client.Language != null) {
                        replacedTemplate = replacedTemplate.Replace("%pStartPage-lblQuickConnect%", client.Language.GetDefaultLocalized("connect", "pStartPage-lblQuickConnect"));
                        replacedTemplate = replacedTemplate.Replace("%pStartPage-lblQuickDisconnect%", client.Language.GetDefaultLocalized("disconnect", "pStartPage-lblQuickDisconnect"));
                        replacedTemplate = replacedTemplate.Replace("%pStartPage-lblQuickDelete%", client.Language.GetDefaultLocalized("delete", "pStartPage-lblQuickDelete"));
                    }
                    else {
                        replacedTemplate = replacedTemplate.Replace("%pStartPage-lblQuickConnect%", "connect");
                        replacedTemplate = replacedTemplate.Replace("%pStartPage-lblQuickDisconnect%", "disconnect");
                        replacedTemplate = replacedTemplate.Replace("%pStartPage-lblQuickDelete%", "delete");
                    }



                    replacedTemplate = replacedTemplate.Replace("%server_hostnameport%", client.HostNamePort);
                    
                    if (client.CurrentServerInfo != null) {
                        replacedTemplate = replacedTemplate.Replace("%players%", client.CurrentServerInfo.PlayerCount.ToString());
                        replacedTemplate = replacedTemplate.Replace("%max_players%", client.CurrentServerInfo.MaxPlayerCount.ToString());
                        replacedTemplate = replacedTemplate.Replace("%server_name%", client.CurrentServerInfo.ServerName);

                        if (this.m_proconApplication != null && this.m_proconApplication.CurrentLanguage != null) {
                            replacedTemplate = replacedTemplate.Replace("%server_additonal%", this.m_startPageTemplates.GetLocalized("connections.online.additional", client.GetFriendlyGamemode(client.CurrentServerInfo.GameMode), client.GetFriendlyMapname(client.CurrentServerInfo.Map), client.CurrentServerInfo.CurrentRound.ToString(), client.CurrentServerInfo.TotalRounds.ToString()));
                        }

                        playerCount += client.CurrentServerInfo.PlayerCount;
                        playerSlotsTotal += client.CurrentServerInfo.MaxPlayerCount;
                    }

                    connectionHtml.Add("safehostport", Regex.Replace(client.FileHostNamePort, "[^0-9a-zA-Z]", ""));
                    connectionHtml.Add("html", replacedTemplate);

                    connectionsArray.Add(connectionHtml);
                }

                if (playerSlotsTotal > 0 && this.m_language != null && this.m_isDocumentReady == true) {
                    this.webBrowser1.Document.InvokeScript("fnSetLocalization", new string[] { "pStartPage-lblConnectionsSummary", this.m_language.GetDefaultLocalized(String.Format("{0} of {1} slots used", playerCount, playerSlotsTotal), "pStartPage-lblConnectionsSummary", playerCount, playerSlotsTotal) });
                }

                if (this.m_isDocumentReady == true) {

                    this.webBrowser1.Document.InvokeScript("fnUpdateConnectionsList", new string[] { JSON.JsonEncode(connectionsArray) });
                }
            }
        }
        
        private void Connections_ConnectionAdded(PRoConClient item) {
            item.ConnectionClosed += new PRoConClient.EmptyParamterHandler(item_ConnectionClosed);
            item.ConnectAttempt += new PRoConClient.EmptyParamterHandler(item_ConnectAttempt);
            item.Login += new PRoConClient.EmptyParamterHandler(item_Login);
            item.GameTypeDiscovered += new PRoConClient.EmptyParamterHandler(item_GameTypeDiscovered);

            this.UpdateConnections();
        }
        
        private void Connections_ConnectionRemoved(PRoConClient item) {
            item.ConnectionClosed -= new PRoConClient.EmptyParamterHandler(item_ConnectionClosed);
            item.Login -= new PRoConClient.EmptyParamterHandler(item_Login);
            item.GameTypeDiscovered -= new PRoConClient.EmptyParamterHandler(item_GameTypeDiscovered);

            if (item.Game != null) {
                item.Game.ServerInfo -= new FrostbiteClient.ServerInfoHandler(Game_ServerInfo);
                item.Game.ResponseError -= new FrostbiteClient.ResponseErrorHandler(Game_ResponseError);

                item.PackageDownloading -= new PRoConClient.RemotePackagesInstallHandler(item_PackageDownloading);
                item.PackageDownloaded -= new PRoConClient.RemotePackagesInstallHandler(item_PackageDownloaded);
                item.PackageDownloadError -= new PRoConClient.RemotePackagesInstallErrorHandler(item_PackageDownloadError);
                item.PackageInstalled -= new PRoConClient.RemotePackagesInstalledHandler(item_PackageInstalled);
                item.PackageInstalling -= new PRoConClient.RemotePackagesInstallHandler(item_PackageInstalling);
            }

            if (this.m_isDocumentReady == true) {
                this.webBrowser1.Document.InvokeScript("fnRemoveConnection", new String[] { Regex.Replace(item.FileHostNamePort, "[^0-9a-zA-Z]", "") });
            }

            //this.UpdateConnections();
        }

        private void item_GameTypeDiscovered(PRoConClient sender) {
            if (sender.Game != null) {
                sender.Game.ServerInfo += new FrostbiteClient.ServerInfoHandler(Game_ServerInfo);
                sender.Game.ResponseError += new FrostbiteClient.ResponseErrorHandler(Game_ResponseError);

                sender.PackageDownloading += new PRoConClient.RemotePackagesInstallHandler(item_PackageDownloading);
                sender.PackageDownloaded += new PRoConClient.RemotePackagesInstallHandler(item_PackageDownloaded);
                sender.PackageDownloadError += new PRoConClient.RemotePackagesInstallErrorHandler(item_PackageDownloadError);
                sender.PackageInstalled += new PRoConClient.RemotePackagesInstalledHandler(item_PackageInstalled);
                sender.PackageInstalling += new PRoConClient.RemotePackagesInstallHandler(item_PackageInstalling);
            }
        }

        private void item_ConnectAttempt(PRoConClient sender) {
            this.UpdateConnections();
        }

        private void item_Login(PRoConClient sender) {
            this.UpdateConnections();
        }

        private void item_ConnectionClosed(PRoConClient sender) {
            this.UpdateConnections();
        }

        private void Game_ServerInfo(FrostbiteClient sender, CServerInfo csiServerInfo) {
            this.UpdateConnections();
        }

        #region COM Calls

        public void HREF(string url) {
            if (Regex.Match(url, @"http\:\/\/.*").Success == false) {
                url = String.Format("http://{0}", url);
            }

            System.Diagnostics.Process.Start(url);
        }

        public void DocumentReady() {
            this.m_isDocumentReady = true;

            this.UpdateConnections();
            if (this.m_previousDocument != null) {
                this.ReplaceRssContent(this.m_previousDocument);

                this.webBrowser1.Document.InvokeScript("UpdatePackageList", new object[] { this.m_proconApplication.PackageManager.RemoteToJsonString() });
            }

            this.SetLocalization(this.m_proconApplication.CurrentLanguage);
        }

        public void CreateConnection(string hostName, string port, string username, string password) {

            ushort parsedPort = 0;

            if (ushort.TryParse(port, out parsedPort) == true) {
                PRoConClient newConnection = this.m_proconApplication.AddConnection(hostName, parsedPort, username, password);

                if (newConnection != null) {
                    newConnection.Connect();
                }
            }
        }

        public void GoToConnectionPage(string hostNamePort) {
            if (this.ConnectionPage != null) {
                this.ConnectionPage(hostNamePort);
            }
        }

        public void AttemptConnection(string hostNamePort) {
            if (this.m_proconApplication != null && this.m_proconApplication.Connections != null && this.m_proconApplication.Connections.Contains(hostNamePort) == true) {
                this.m_proconApplication.Connections[hostNamePort].Connect();
            }
        }

        public void DisconnectConnection(string hostNamePort) {
            if (this.m_proconApplication != null && this.m_proconApplication.Connections != null && this.m_proconApplication.Connections.Contains(hostNamePort) == true) {
                this.m_proconApplication.Connections[hostNamePort].Disconnect();
                this.m_proconApplication.Connections[hostNamePort].AutomaticallyConnect = false;
            }
        }

        public void DeleteConnection(string hostNamePort) {
            if (this.m_proconApplication != null && this.m_proconApplication.Connections != null && this.m_proconApplication.Connections.Contains(hostNamePort) == true) {
                this.m_proconApplication.Connections[hostNamePort].Disconnect();
                this.m_proconApplication.Connections.Remove(hostNamePort);
            }
        }

        public void InstallPackage(string uid) {

            if (this.m_proconApplication != null && this.m_proconApplication.PackageManager != null && this.m_proconApplication.PackageManager.RemotePackages.Contains(uid) == true) {
                this.m_proconApplication.PackageManager.DownloadInstallPackage(uid, true);

                Package package = this.m_proconApplication.PackageManager.RemotePackages[uid];

                // Only install plugin packages on layer
                if (package.PackageType == PackageType.Plugin) {
                    foreach (PRoConClient client in this.m_proconApplication.Connections) {
                        if (client.State == ConnectionState.Connected && client.IsLoggedIn == true && client.IsPRoConConnection == true) {
                            client.SendProconPackagesInstallPacket(package.Uid, package.Version.ToString(), package.Md5);
                        }
                    }
                }
            }
        }

        #endregion

        protected override void OnLoad(EventArgs e) {
            base.OnLoad(e);

            string startPagePath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media"), "UI");

            this.m_isDocumentReady = false;
            if (File.Exists(Path.Combine(startPagePath, "startPage.temp")) == true) {
                this.m_startPageTemplates = new CLocalization(Path.Combine(startPagePath, "startPage.temp"), "startPage.temp");
            }
            this.webBrowser1.AllowNavigation = false;
            this.webBrowser1.AllowWebBrowserDrop = false;
            this.webBrowser1.IsWebBrowserContextMenuEnabled = false;
            this.webBrowser1.ObjectForScripting = this;
            this.webBrowser1.ScriptErrorsSuppressed = true;

            if (this.webBrowser1.Document == null && File.Exists(Path.Combine(startPagePath, "startpage.html")) == true) {
                this.webBrowser1.Navigate(Path.Combine(startPagePath, "startpage.html"));
            }
        }

        #region RSS Feed

        private void ReplaceRssContent(XmlDocument rssDocument) {
            try {
                this.m_previousDocument = rssDocument;

                this.DispatchArticleFeed(rssDocument);
                this.DispatchUserSummaryFeed(rssDocument);
                this.DispatchDonationFeed(rssDocument);
                this.DispatchPromotions(rssDocument);
            }
            catch (Exception) { }
        }

        private void m_proconApplication_RssUpdateSuccess(PRoConApplication instance, XmlDocument rssDocument) {
            this.ReplaceRssContent(rssDocument);
        }

        private void m_proconApplication_RssUpdateError(PRoConApplication instance) {
            if (this.m_isDocumentReady == true) {
                this.webBrowser1.Document.InvokeScript("UpdateRssMonthlySummaryFeed", new String[] { "" });
                this.webBrowser1.Document.InvokeScript("UpdateRssDonationFeed", new String[] { "" });
                this.webBrowser1.Document.InvokeScript("UpdateRssFeed", new String[] { "" });
            }
        }

        private void m_proconApplication_BeginRssUpdate(PRoConApplication instance) {
            if (this.m_isDocumentReady == true) {
                this.webBrowser1.Document.InvokeScript("LoadingRssFeed");
            }
        }

        /*
        public XmlNodeList GetNodeChildren(XmlNodeList parentList, string name) {

            XmlNodeList returnNodeList = null;

            if (parentList != null) {
                for (int i = 0; i < parentList.Count; i++) {
                    if (String.Compare(parentList[i].Name, name) == 0) {

                        returnNodeList = parentList[i].ChildNodes;
                        break;
                    }
                }
            }

            return returnNodeList;
        }
        */

        internal class RssArticleItem {

            public string Title {
                get;
                internal set;
            }

            public string Link {
                get;
                internal set;
            }

            public DateTime PublishDate {
                get;
                internal set;
            }

            public string Content {
                get;
                internal set;
            }
        }

        internal class RssDonationItem {

            public string Currency {
                get;
                internal set;
            }

            public float Amount {
                get;
                internal set;
            }

            public DateTime PublishDate {
                get;
                internal set;
            }

            public string Name {
                get;
                internal set;
            }

            public string Link {
                get;
                internal set;
            }

            public string Comment {
                get;
                internal set;
            }
        }

        internal class RssUserSummaryItem {

            public int Count {
                get;
                internal set;
            }
        }

        private void DispatchPromotions(XmlDocument rssDocument) {

            try {

                ArrayList promotionsList = new ArrayList();

                foreach (XmlNode node in rssDocument.SelectNodes("/rss/procon/promotions/promotion")) {
                    Hashtable promotion = new Hashtable();
                    promotion.Add("image", node.SelectSingleNode("image").InnerText);
                    promotion.Add("link", node.SelectSingleNode("link").InnerText);
                    promotion.Add("name", node.SelectSingleNode("name").InnerText);
                    promotionsList.Add(promotion);
                }

                if (this.m_isDocumentReady == true) {
                    this.webBrowser1.Document.InvokeScript("UpdatePromotions", new String[] { JSON.JsonEncode(promotionsList) });
                }
            }
            catch (Exception) { }
        }

        private void DispatchUserSummaryFeed(XmlDocument rssDocument) {

            try {

                int userCount = 0;
                int.TryParse(rssDocument.SelectSingleNode("/rss/procon/summary/users").InnerText, out userCount);

                this.m_rssUserSummaryItem = new RssUserSummaryItem() { Count = userCount };
            }
            catch (Exception) { }
        }

        private void DispatchDonationFeed(XmlDocument rssDocument) {

            try {

                List<RssDonationItem> rssItems = new List<RssDonationItem>();

                foreach (XmlNode node in rssDocument.SelectNodes("rss/donations/donation")) {

                    DateTime date = DateTime.Now.AddMonths(-1);
                    DateTime.TryParse(node.SelectSingleNode("date").InnerText, out date);

                    float amount = 0.0F;
                    float.TryParse(node.SelectSingleNode("amount").InnerText, NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out amount);

                    rssItems.Add(new RssDonationItem() {
                        Currency = node.SelectSingleNode("currency").InnerText,
                        Amount = amount,
                        PublishDate = date,
                        Name = node.SelectSingleNode("name").InnerText,
                        Link = node.SelectSingleNode("link").InnerText,
                        Comment = node.SelectSingleNode("comment").InnerText
                    });
                }

                this.UpdateRssDonationFeed(rssItems);
            }
            catch (Exception) { }
        }

        private void UpdateRssDonationFeed(List<RssDonationItem> rss) {

            if (this.m_startPageTemplates != null) {

                string rssHtml = String.Empty;

                float totalAmount = 0.0F;

                foreach (RssDonationItem item in rss) {
                    string replacedTemplate = this.m_startPageTemplates.GetLocalized("donationWall.donation");

                    replacedTemplate = replacedTemplate.Replace("%donation_link%", item.Link);
                    replacedTemplate = replacedTemplate.Replace("%donation_name%", item.Name);
                    replacedTemplate = replacedTemplate.Replace("%donation_amount%", String.Format("{0:0.00}", item.Amount));
                    replacedTemplate = replacedTemplate.Replace("%donation_currency%", item.Currency);
                    replacedTemplate = replacedTemplate.Replace("%donation_date%", item.PublishDate.ToShortDateString());
                    replacedTemplate = replacedTemplate.Replace("%donation_comment%", item.Comment);

                    if (item.PublishDate >= DateTime.Now.AddMonths(-1)) {
                        totalAmount += item.Amount;
                    }

                    rssHtml += replacedTemplate;
                }

                if (this.m_rssUserSummaryItem != null) {

                    string replacedSummaryTemplate = this.m_startPageTemplates.GetLocalized("donationWall.summary");

                    if (this.m_language != null) {
                        string formattedUserCount = String.Format("{0:0,0}", this.m_rssUserSummaryItem.Count);
                        string formattedDonationsTotal = totalAmount.ToString("N");
                        string formattedDonationsPetCapita = String.Format("{0:0.0000}", totalAmount / (float)this.m_rssUserSummaryItem.Count);
                        
                        replacedSummaryTemplate = replacedSummaryTemplate.Replace("%pStartPage-lblMonthlySummary-users%", this.m_language.GetDefaultLocalized(formattedUserCount + " users", "pStartPage-lblMonthlySummary-users", formattedUserCount));
                        replacedSummaryTemplate = replacedSummaryTemplate.Replace("%pStartPage-lblMonthlySummary-donations%", this.m_language.GetDefaultLocalized(formattedDonationsTotal + " donated", "pStartPage-lblMonthlySummary-donations", formattedDonationsTotal));
                        replacedSummaryTemplate = replacedSummaryTemplate.Replace("%pStartPage-lblMonthlySummary-donationspercapita%", this.m_language.GetDefaultLocalized(formattedDonationsPetCapita + " donated per user", "pStartPage-lblMonthlySummary-donationspercapita", formattedDonationsPetCapita));
                    }

                    if (this.m_isDocumentReady == true) {
                        this.webBrowser1.Document.InvokeScript("UpdateRssMonthlySummaryFeed", new String[] { replacedSummaryTemplate });
                    }
                }

                if (this.m_isDocumentReady == true) {
                    this.webBrowser1.Document.InvokeScript("UpdateRssDonationFeed", new String[] { rssHtml });
                }
            }

        }

        private void DispatchArticleFeed(XmlDocument rssDocument) {

            try {

                List<RssArticleItem> rssItems = new List<RssArticleItem>();

                foreach (XmlNode node in rssDocument.SelectNodes("rss/channel/item")) {
                    
                    DateTime pubDate = DateTime.Now.AddMonths(-1);
                    DateTime.TryParse(node.SelectSingleNode("pubDate").InnerText, out pubDate);

                    rssItems.Add(new RssArticleItem() {
                        Title = node.SelectSingleNode("title").InnerText,
                        Link = node.SelectSingleNode("link").InnerText,
                        Content = node.SelectSingleNode("description").InnerText,
                        PublishDate = pubDate
                    });
                }

                this.UpdateRssArticleFeed(rssItems);
            }
            catch (Exception e) { }
        }

        private void UpdateRssArticleFeed(List<RssArticleItem> rss) {

            if (this.m_startPageTemplates != null) {

                string rssHtml = String.Empty;

                foreach (RssArticleItem item in rss) {

                    string replacedTemplate = this.m_startPageTemplates.GetLocalized("newsFeed.article");

                    replacedTemplate = replacedTemplate.Replace("%article_link%", item.Link);
                    replacedTemplate = replacedTemplate.Replace("%article_title%", item.Title);
                    replacedTemplate = replacedTemplate.Replace("%article_date%", item.PublishDate.ToShortDateString());
                    replacedTemplate = replacedTemplate.Replace("%article_content%", item.Content);

                    rssHtml += replacedTemplate;

                    //rssHtml += String.Format(@"<h2><a onclick=""window.external.HREF('{0}')"">{1}</a></h2>", item.Link, item.Title);

                    //item.Content = Regex.Replace(item.Content, @"[^\[]\.\.\.($|[^\]])", " [...]", RegexOptions.IgnoreCase);
                    //item.Content = item.Content.Replace("[...]", String.Format(@"<a onclick=""window.external.HREF('{0}')"">[...]</a>", item.Link));

                    //rssHtml += String.Format("<p><b>{0}</b></p><p>{1}</p>", item.PublishDate.ToShortDateString(), item.Content);
                }

                if (this.m_isDocumentReady == true) {
                    this.webBrowser1.Document.InvokeScript("UpdateRssFeed", new String[] { rssHtml });
                }
            }
        }

        private void tmrRssFeed_Tick(object sender, EventArgs e) {
            // It only ever needs re-updating to view news
            this.m_proconApplication.UpdateRss();
        }

        #endregion

        #region Package Management

        // Installed
        // New Update Available
        // Error downloading/unzipping

        // Local
        // (spin) Downloading -> (check) Downloaded
        // (spin) Installing -> (check) Installed
        // (orange) You must restart

        // Layer
        // (check) Issuing update requests
        // 127.0.0.1:44453 downloading package
        // (warning) 1.1.1.1:00482 package already installed
        // (check) 127.0.0.1:44453 Package downloaded - Restart required

        #region Local Package Management

        private void PackageManager_RemotePackagesUpdated(PackageManager sender) {
            this.webBrowser1.Document.InvokeScript("UpdatePackageList", new object[] { sender.RemoteToJsonString() });
        }

        private void PackageManager_PackageBeginningDownload(PackageManager sender, Package target) {
            this.webBrowser1.Document.InvokeScript("UpdatePackageLocalInstallStatus", new object[] { JSON.JsonEncode(target.ToHashTable()), PackageManager.PACKAGE_STATUSCODE_DOWNLOADBEGIN, "Downloading" });
        }

        private void PackageManager_PackageDownloadFail(PackageManager sender, Package target) {
            this.webBrowser1.Document.InvokeScript("UpdatePackageLocalInstallStatus", new object[] { JSON.JsonEncode(target.ToHashTable()), PackageManager.PACKAGE_STATUSCODE_DOWNLOADFAIL, "Download error" });
        }

        private void PackageManager_PackageDownloaded(PackageManager sender, Package target) {
            this.webBrowser1.Document.InvokeScript("UpdatePackageLocalInstallStatus", new object[] { JSON.JsonEncode(target.ToHashTable()), PackageManager.PACKAGE_STATUSCODE_DOWNLOADSUCCESS, "Downloaded" });
        }

        private void PackageManager_PackageInstalling(PackageManager sender, Package target) {
            this.webBrowser1.Document.InvokeScript("UpdatePackageLocalInstallStatus", new object[] { JSON.JsonEncode(target.ToHashTable()), PackageManager.PACKAGE_STATUSCODE_INSTALLBEGIN, "Installing" });
        }

        private void PackageManager_PackageAwaitingRestart(PackageManager sender, Package target) {
            this.webBrowser1.Document.InvokeScript("UpdatePackageLocalInstallStatus", new object[] { JSON.JsonEncode(target.ToHashTable()), PackageManager.PACKAGE_STATUSCODE_INSTALLQUEUED, "Restart" });
        }

        #endregion

        #region Remote Package Management

        private Package GetRemotePackage(string uid) {

            Package returnPackage = null;

            if (this.m_proconApplication.PackageManager.RemotePackages.Contains(uid) == true) {
                returnPackage = this.m_proconApplication.PackageManager.RemotePackages[uid];
            }

            return returnPackage;
        }

        private void AppendRemotePackageInstallStatus(string uid, string hostNamePort, int statusCode, string statusText) {

            Package package = this.GetRemotePackage(uid);

            if (package != null) {
                this.webBrowser1.Document.InvokeScript("AppendPackageRemoteInstallStatus", new object[] { JSON.JsonEncode(package.ToHashTable()), hostNamePort, statusCode, statusText });
            }
        }

        void Game_ResponseError(FrostbiteClient sender, Packet originalRequest, string errorMessage) {
            if (originalRequest.Words.Count >= 2 && String.Compare(originalRequest.Words[0], "procon.packages.install", true) == 0) {
                if (String.Compare(errorMessage, PRoConLayerClient.RESPONSE_PACKAGE_ALREADYINSTALLED, true) == 0) {
                    this.AppendRemotePackageInstallStatus(originalRequest.Words[1], sender.Connection.Hostname + ":" + sender.Connection.Port.ToString(), PackageManager.PACKAGE_STATUSCODE_INSTALLFAIL, "Layer already has package installed");
                }
                else if (String.Compare(errorMessage, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES, true) == 0) {
                    this.AppendRemotePackageInstallStatus(originalRequest.Words[1], sender.Connection.Hostname + ":" + sender.Connection.Port.ToString(), PackageManager.PACKAGE_STATUSCODE_INSTALLFAIL, "Insufficient privileges to install packages on parent layer");
                }
            }
        }

        private void item_PackageDownloadError(PRoConClient sender, string uid, string error) {
            this.AppendRemotePackageInstallStatus(uid, sender.HostNamePort, PackageManager.PACKAGE_STATUSCODE_DOWNLOADFAIL, "Download error: " + error);
        }

        private void item_PackageDownloading(PRoConClient sender, string uid) {
            this.AppendRemotePackageInstallStatus(uid, sender.HostNamePort, PackageManager.PACKAGE_STATUSCODE_DOWNLOADBEGIN, "Downloading...");
        }

        private void item_PackageDownloaded(PRoConClient sender, string uid) {
            this.AppendRemotePackageInstallStatus(uid, sender.HostNamePort, PackageManager.PACKAGE_STATUSCODE_DOWNLOADSUCCESS, "Successfully downloaded package");
        }

        private void item_PackageInstalling(PRoConClient sender, string uid) {
            this.AppendRemotePackageInstallStatus(uid, sender.HostNamePort, PackageManager.PACKAGE_STATUSCODE_INSTALLBEGIN, "Beginning installation...");
        }

        private void item_PackageInstalled(PRoConClient sender, string uid, bool restart) {
            if (restart == true) {
                this.AppendRemotePackageInstallStatus(uid, sender.HostNamePort, PackageManager.PACKAGE_STATUSCODE_INSTALLQUEUED, "Package installed, restart required to complete installation");
            }
            else {
                this.AppendRemotePackageInstallStatus(uid, sender.HostNamePort, PackageManager.PACKAGE_STATUSCODE_INSTALLSUCCESS, "Package installed and active");
            }
        }

        #endregion

        #endregion
    }
}
