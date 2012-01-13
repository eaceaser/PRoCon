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
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Xml;
using System.Diagnostics;
using MaxMind;

namespace PRoCon.Core {
    using Core;
    using Core.Accounts;
    using Core.Variables;
    using Core.Localization;
    using Core.Options;
    using Core.AutoUpdates;
    using Core.Players.Items;
    using Core.Battlemap;
    using Core.HttpServer;
    using Core.Remote;
    using Core.Packages;
    using Core.Events;
    // This, renamed or whatever, will eventually be the core app.
    // Will contain what frmMain.cs contains/does at the moment.

    public class PRoConApplication {
        
        public delegate void CurrentLanguageHandler(CLocalization language);
        public event CurrentLanguageHandler CurrentLanguageChanged;

        public delegate void ShowNotificationHandler(int timeout, string title, string text, bool isError);
        public event ShowNotificationHandler ShowNotification;

        public event HttpWebServer.StateChangeHandler HttpServerOnline;
        public event HttpWebServer.StateChangeHandler HttpServerOffline;

        public delegate void EmptyParameterHandler(PRoConApplication instance);
        public event EmptyParameterHandler BeginRssUpdate;
        public event EmptyParameterHandler RssUpdateError;

        public delegate void RssHandler(PRoConApplication instance, XmlDocument rss);
        public event RssHandler RssUpdateSuccess;

        private CountryLookup m_clIpToCountry;

        public bool ConsoleMode { get; set; }

        public AccountDictionary AccountsList {
            get;
            private set;
        }

        public LocalizationDictionary Languages {
            get;
            private set;
        }

        public HttpWebServer HttpWebServer {
            get;
            private set;
        }

        private CLocalization m_clocCurrentLanguage;
        public CLocalization CurrentLanguage {
            get {
                return this.m_clocCurrentLanguage;
            }
            set {
                if (value != null) {
                    this.m_clocCurrentLanguage = value;

                    if (this.CurrentLanguageChanged != null) {
                        FrostbiteConnection.RaiseEvent(this.CurrentLanguageChanged.GetInvocationList(), value);
                    }

                    this.SaveMainConfig();
                }
            }
        }

        public ConnectionDictionary Connections {
            get;
            private set;
        }

        public OptionsSettings OptionsSettings {
            get;
            private set;
        }

        public bool LoadingAccountsFile {
            get;
            private set;
        }

        public bool LoadingMainConfig {
            get;
            set;
        }

        public AutoUpdater AutoUpdater {
            get;
            private set;
        }

        public string CustomTitle {
            get;
            private set;
        }

        public int MaxGspServers {
            get;
            private set;
        }

        public bool BlockUpdateChecks {
            get;
            private set;
        }

        public System.Windows.Forms.FormWindowState SavedWindowState {
            get;
            set;
        }

        public Rectangle SavedWindowBounds {
            get;
            set;
        }

        public PackageManager PackageManager { get; private set; }

        #region Regex

        // Moved here in 0.6.0.0 because each connection would compile these and they took up
        // a surprising amount of memory once combined.
        public Regex RegexMatchPunkbusterPlist { get; private set; }
        public Regex RegexMatchPunkbusterGuidComputed { get; private set; }
        public Regex RegexMatchPunkbusterBanlist { get; private set; }
        public Regex RegexMatchPunkbusterUnban { get; private set; }
        public Regex RegexMatchPunkbusterBanAdded { get; private set; }

        public Regex RegexMatchPunkbusterBeginPlist { get; private set; }
        public Regex RegexMatchPunkbusterEndPlist { get; private set; }
        
        #endregion

        private bool m_isCheckerRunning;
        private Thread m_thChecker;

        private void GetGspSettings() {

            bool isEnabled = true;
            int iValue = int.MaxValue;

            if (File.Exists("PRoCon.xml") == true) {

                XmlDocument doc = new XmlDocument();
                doc.Load("PRoCon.xml");

                XmlNodeList OptionsList = doc.GetElementsByTagName("options");
                if (OptionsList.Count > 0) {
                    XmlNodeList BlockUpdateChecksList = ((XmlElement)OptionsList[0]).GetElementsByTagName("blockupdatechecks");
                    if (BlockUpdateChecksList.Count > 0) {
                        if (bool.TryParse(BlockUpdateChecksList[0].InnerText, out isEnabled) == true) {
                            this.BlockUpdateChecks = isEnabled;
                        }
                    }

                    XmlNodeList NameList = ((XmlElement)OptionsList[0]).GetElementsByTagName("name");
                    if (NameList.Count > 0) {
                        this.CustomTitle = NameList[0].InnerText;
                    }

                    XmlNodeList MaxServersList = ((XmlElement)OptionsList[0]).GetElementsByTagName("maxservers");
                    if (MaxServersList.Count > 0) {
                        if (int.TryParse(MaxServersList[0].InnerText, out iValue) == true) {
                            this.MaxGspServers = iValue;
                        }
                    }
                }
            }
        }

        public static bool IsProcessOpen() {

            int processCount = 0;

            Process currentProcess = Process.GetCurrentProcess();
            foreach (Process instance in Process.GetProcessesByName(currentProcess.ProcessName)) {

                if (String.Compare(instance.MainModule.FileName, currentProcess.MainModule.FileName) == 0) {

                    processCount++;

                    if (processCount > 1) {
                        break;
                    }
                }

            }

            return (processCount > 1);
        }

        public PRoConApplication(bool consoleMode, string[] args) {

            this.LoadingMainConfig = true;
            this.LoadingAccountsFile = true;

            this.BlockUpdateChecks = false;
            this.MaxGspServers = int.MaxValue;
            this.CustomTitle = String.Empty;

            int iValue;
            if (args != null && args.Length >= 2) {
                for (int i = 0; i < args.Length; i = i + 2) {
                    if (String.Compare("-name", args[i], true) == 0) {
                        this.CustomTitle = args[i + 1];
                    }
                    else if (String.Compare("-maxservers", args[i], true) == 0 && int.TryParse(args[i + 1], out iValue) == true) {
                        this.MaxGspServers = iValue;
                    }
                    else if (String.Compare("-blockupdatechecks", args[i], true) == 0 && int.TryParse(args[i + 1], out iValue) == true) {
                        this.BlockUpdateChecks = (iValue == 1);
                    }
                }
            }

            this.GetGspSettings();

            this.PackageManager = new PackageManager();

            this.Connections = new ConnectionDictionary();
            this.Connections.ConnectionAdded += new ConnectionDictionary.ConnectionAlteredHandler(Connections_ConnectionAdded);
            this.Connections.ConnectionRemoved += new ConnectionDictionary.ConnectionAlteredHandler(Connections_ConnectionRemoved);

            this.OptionsSettings = new OptionsSettings(this);
            this.OptionsSettings.ChatLoggingChanged += new OptionsSettings.OptionsEnabledHandler(OptionsSettings_ChatLoggingChanged);
            this.OptionsSettings.ConsoleLoggingChanged += new OptionsSettings.OptionsEnabledHandler(OptionsSettings_ConsoleLoggingChanged);
            this.OptionsSettings.EventsLoggingChanged += new OptionsSettings.OptionsEnabledHandler(OptionsSettings_EventsLoggingChanged);
            this.OptionsSettings.PluginsLoggingChanged += new Options.OptionsSettings.OptionsEnabledHandler(OptionsSettings_PluginsLoggingChanged);

            this.Languages = new LocalizationDictionary();
            if ((this.ConsoleMode = consoleMode) == false) {
                this.LoadLocalizationFiles();
            }

            if (this.Languages.Contains("au.loc") == true) {
                this.CurrentLanguage = this.Languages["au.loc"];
            }
            else {
                this.CurrentLanguage = new CLocalization();
            }

            this.AutoUpdater = new AutoUpdater(this, args);

            this.AccountsList = new AccountDictionary();
            this.AccountsList.AccountAdded += new AccountDictionary.AccountAlteredHandler(AccountsList_AccountAdded);
            this.AccountsList.AccountRemoved += new AccountDictionary.AccountAlteredHandler(AccountsList_AccountRemoved);
            // TODO: Password change -> Save

            this.m_clIpToCountry = new CountryLookup(AppDomain.CurrentDomain.BaseDirectory + "GeoIP.dat");

            this.SavedWindowBounds = new Rectangle();

            this.RegexMatchPunkbusterPlist = new Regex(@":[ ]+?(?<slotid>[0-9]+)[ ]+?(?<guid>[A-Fa-f0-9]+)\(.*?\)[ ]+?(?<ip>[0-9\.:]+).*?\(.*?\)[ ]+?""(?<name>.*?)\""", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            this.RegexMatchPunkbusterGuidComputed = new Regex(@": Player Guid Computed[ ]+?(?<guid>[A-Fa-f0-9]+)\(.*?\)[ ]+?\(slot #(?<slotid>[0-9]+)\)[ ]+?(?<ip>[0-9\.:]+)[ ]+?(?<name>.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            this.RegexMatchPunkbusterBanlist = new Regex(@":[ ]+?(?<banid>[0-9]+)[ ]+?(?<guid>[A-Fa-f0-9]+)[ ]+?{(?<remaining>[0-9\-]+)/(?<banlength>[0-9\-]+)}[ ]+?""(?<name>.+?)""[ ]+?""(?<ip>.+?)""[ ]+?(?<reason>.*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            this.RegexMatchPunkbusterUnban = new Regex(@":[ ]+?Guid[ ]+?(?<guid>[A-Fa-f0-9]+)[ ]+?has been Unbanned", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            this.RegexMatchPunkbusterBanAdded = new Regex(@": Ban Added to Ban List", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            this.RegexMatchPunkbusterBeginPlist = new Regex(@":[ ]+?Player List: ", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            this.RegexMatchPunkbusterEndPlist = new Regex(@":[ ]+?End of Player List \((?<players>[0-9]+) Players\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            //this.CleanPlugins();
            
            // Create the initial web server object
            this.ExecutePRoConCommand(this, new List<string>() { "procon.private.httpWebServer.enable", "false", "27360", "0.0.0.0" }, 0);

            //this.Execute();
        }

        public void Execute() {
            // Load all of the accounts.
            this.UpdateRss();

            this.ExecuteMainConfig("accounts.cfg");
            this.LoadingAccountsFile = false;

            this.ExecuteMainConfig("procon.cfg");
            this.LoadingMainConfig = false;

            this.m_isCheckerRunning = true;
            this.m_thChecker = new Thread(this.ReconnectVersionChecker);
            this.m_thChecker.Start();
        }

        private void HttpWebServer_ProcessRequest(HttpWebServerRequest sender) {

            string[] directories = sender.Data.RequestPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
            HttpWebServerResponseData response = new HttpWebServerResponseData(String.Empty);

            if (directories.Length == 0) {

                switch (sender.Data.RequestFile.ToLower()) {
                    case "connections":
                        response.Document = this.Connections.ToJsonString();
                        response.Cache.CacheType = PRoCon.Core.HttpServer.Cache.HttpWebServerCacheType.NoCache;
                        break;
                    default:
                        response.StatusCode = "404 Not Found";
                        break;
                }
            }
            else if (directories.Length == 1) {

                if (this.Connections.Contains(directories[0]) == true) {

                    switch (sender.Data.RequestFile.ToLower()) {
                        case "players":

                            response.Document = this.Connections[directories[0]].PlayerList.ToJsonString();
                            response.Cache.CacheType = PRoCon.Core.HttpServer.Cache.HttpWebServerCacheType.NoCache;
                            break;
                        case "chat":

                            int historyLength = 0;
                            DateTime newerThan = DateTime.Now;

                            if (int.TryParse(sender.Data.Query.Get("history"), out historyLength) == true) {
                                response.Document = this.Connections[directories[0]].ChatConsole.ToJsonString(historyLength);
                                response.Cache.CacheType = PRoCon.Core.HttpServer.Cache.HttpWebServerCacheType.NoCache;
                            }
                            else if (DateTime.TryParse(sender.Data.Query.Get("newer_than"), out newerThan) == true) {
                                response.Document = this.Connections[directories[0]].ChatConsole.ToJsonString(newerThan.ToLocalTime());
                            }
                            else {
                                response.StatusCode = "400 Bad Request";
                            }

                            break;
                        default:

                            response.StatusCode = "404 Not Found";
                            break;
                    }
                }

            }
            else if (directories.Length >= 3) {

                // /HostNameIp/plugins/PluginClassName/
                if (this.Connections.Contains(directories[0]) == true && String.Compare("plugins", directories[1]) == 0) {

                    if (this.Connections[directories[0]].PluginsManager.Plugins.EnabledClassNames.Contains(directories[2]) == true) {
                        HttpWebServerResponseData pluginRespose = (HttpWebServerResponseData)this.Connections[directories[0]].PluginsManager.InvokeOnEnabled(directories[2], "OnHttpRequest", sender.Data);

                        if (pluginRespose != null) {
                            response = pluginRespose;
                        }
                    }
                    else {
                        response.StatusCode = "404 Not Found";
                    }
                }
                else {
                    response.StatusCode = "404 Not Found";
                }
            }
            else {
                response.StatusCode = "404 Not Found";
            }

            sender.Respond(response);
        }

        private void Connections_ConnectionAdded(PRoConClient item) {
            this.SaveMainConfig();
            item.AutomaticallyConnectChanged += new PRoConClient.AutomaticallyConnectHandler(item_AutomaticallyConnectChanged);
        }

        private void Connections_ConnectionRemoved(PRoConClient item) {
            item.AutomaticallyConnectChanged -= new PRoConClient.AutomaticallyConnectHandler(item_AutomaticallyConnectChanged);
            this.SaveMainConfig();
            item.ForceDisconnect();
            item.Destroy();
        }

        private void item_AutomaticallyConnectChanged(PRoConClient sender, bool isEnabled) {
            this.SaveMainConfig();
        }

        void OptionsSettings_PluginsLoggingChanged(bool blEnabled) {
            foreach (PRoConClient prcClient in this.Connections) {
                if (prcClient.PluginConsole != null) {
                    prcClient.PluginConsole.Logging = blEnabled;
                }
            }
        }

        void OptionsSettings_EventsLoggingChanged(bool blEnabled) {
            foreach (PRoConClient prcClient in this.Connections) {
                if (prcClient.EventsLogging != null) {
                    prcClient.EventsLogging.Logging = blEnabled;
                }
            }
        }

        void OptionsSettings_ConsoleLoggingChanged(bool blEnabled) {
            foreach (PRoConClient prcClient in this.Connections) {
                if (prcClient.Console != null) {
                    prcClient.Console.Logging = blEnabled;
                }
            }
        }

        private void OptionsSettings_ChatLoggingChanged(bool blEnabled) {
            foreach (PRoConClient prcClient in this.Connections) {
                if (prcClient.ChatConsole != null) {
                    prcClient.ChatConsole.Logging = blEnabled;
                }
            }
        }

        public PRoConClient AddConnection(string strHost, UInt16 iu16Port, string strUsername, string strPassword) {
            PRoConClient prcNewClient = null;

            if (this.Connections.Contains(strHost + ":" + iu16Port.ToString()) == false && this.Connections.Count < this.MaxGspServers) {
                prcNewClient = new PRoConClient(this, strHost, iu16Port, strUsername, strPassword);

                this.Connections.Add(prcNewClient);

                this.SaveMainConfig();
            }

            return prcNewClient;
        }

        #region Loading/Saving Configs and Commands

        public void LoadLocalizationFiles() {

            lock (new object()) {

                string strCurrentLanguagePath = String.Empty;

                if (this.CurrentLanguage != null) {
                    strCurrentLanguagePath = this.CurrentLanguage.FilePath;
                }

                this.Languages.Clear();

                try {

                    DirectoryInfo diLocalizationDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "Localization" + Path.DirectorySeparatorChar);
                    FileInfo[] a_fiLocalizations = diLocalizationDir.GetFiles("*.loc");

                    foreach (FileInfo fiLocalization in a_fiLocalizations) {

                        CLocalization clocLoadedLanguage = this.Languages.LoadLocalizationFile(fiLocalization.FullName, fiLocalization.Name);

                        //CLocalization clocLoadedLanguage = new CLocalization(fiLocalization.Name);

                        //if (this.Languages.Contains(clocLoadedLanguage.FileName) == false) {
                        //    this.Languages.Add(clocLoadedLanguage);
                        
                            if (String.Compare(clocLoadedLanguage.FilePath, strCurrentLanguagePath, true) == 0) {
                                this.CurrentLanguage = clocLoadedLanguage;
                            }
                        //}
                    }
                }
                catch (Exception e) {
                    FrostbiteConnection.LogError(String.Empty, String.Empty, e);
                }
            }
        }

        private void ExecuteMainConfig(string strConfigFile) {

            if (File.Exists(String.Format(@"{0}Configs{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, strConfigFile)) == true) {

                string[] a_strLines = File.ReadAllLines(String.Format(@"{0}Configs{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, strConfigFile)); // , Encoding.Unicode

                foreach (string strLine in a_strLines) {

                    List<string> lstWords = Packet.Wordify(strLine);

                    if (lstWords.Count >= 1 && Regex.Match(strLine, "^[ ]*//.*").Success == false) {
                        this.ExecutePRoConCommand(this, lstWords, 0);
                    }
                }
            }
        }

        public void SaveMainConfig() {

            if (this.LoadingMainConfig == false && this.CurrentLanguage != null && this.OptionsSettings != null && this.Connections != null && this.HttpWebServer != null) {
                FileStream stmProconConfigFile = null;

                try {

                    if (Directory.Exists(String.Format("{0}Configs{1}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar)) == false) {
                        Directory.CreateDirectory(String.Format("{0}Configs{1}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar));
                    }

                    stmProconConfigFile = new FileStream(String.Format("{0}Configs{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, "procon.cfg"), FileMode.Create);

                    if (stmProconConfigFile != null) {
                        StreamWriter stwConfig = new StreamWriter(stmProconConfigFile, Encoding.Unicode);

                        stwConfig.WriteLine("/////////////////////////////////////////////");
                        stwConfig.WriteLine("// This config will be overwritten by procon.");
                        stwConfig.WriteLine("/////////////////////////////////////////////");

                        //foreach (string[] a_strUsernamePassword in this.m_frmManageAccounts.UserList) {
                        //    stwConfig.WriteLine("procon.public.accounts.create \"{0}\" \"{1}\"", a_strUsernamePassword[0], a_strUsernamePassword[1]);
                        //}

                        stwConfig.WriteLine("procon.private.window.position {0} {1} {2} {3} {4}", this.SavedWindowState, this.SavedWindowBounds.X, this.SavedWindowBounds.Y, this.SavedWindowBounds.Width, this.SavedWindowBounds.Height);
                        //stwConfig.WriteLine("procon.private.window.splitterPosition {0}", this.spltTreeServers.SplitterDistance);

                        stwConfig.WriteLine("procon.private.options.setLanguage \"{0}\"", this.CurrentLanguage.FileName);
                        stwConfig.WriteLine("procon.private.options.chatLogging {0}", this.OptionsSettings.ChatLogging);
                        stwConfig.WriteLine("procon.private.options.consoleLogging {0}", this.OptionsSettings.ConsoleLogging);
                        stwConfig.WriteLine("procon.private.options.eventsLogging {0}", this.OptionsSettings.EventsLogging);
                        stwConfig.WriteLine("procon.private.options.pluginLogging {0}", this.OptionsSettings.PluginLogging);
                        stwConfig.WriteLine("procon.private.options.autoCheckDownloadUpdates {0}", this.OptionsSettings.AutoCheckDownloadUpdates);
                        stwConfig.WriteLine("procon.private.options.autoApplyUpdates {0}", this.OptionsSettings.AutoApplyUpdates);
                        stwConfig.WriteLine("procon.private.options.showtrayicon {0}", this.OptionsSettings.ShowTrayIcon);
                        stwConfig.WriteLine("procon.private.options.minimizetotray {0}", this.OptionsSettings.MinimizeToTray);
                        stwConfig.WriteLine("procon.private.options.closetotray {0}", this.OptionsSettings.CloseToTray);

                        stwConfig.WriteLine("procon.private.options.runPluginsInSandbox {0}", this.OptionsSettings.RunPluginsInTrustedSandbox);
                        stwConfig.WriteLine("procon.private.options.allowAllODBCConnections {0}", this.OptionsSettings.AllowAllODBCConnections);

                        stwConfig.WriteLine("procon.private.options.adminMoveMessage {0}", this.OptionsSettings.AdminMoveMessage);
                        stwConfig.WriteLine("procon.private.options.chatDisplayAdminName {0}", this.OptionsSettings.ChatDisplayAdminName);

                        stwConfig.WriteLine("procon.private.options.layerHideLocalPlugins {0}", this.OptionsSettings.LayerHideLocalPlugins);
                        stwConfig.WriteLine("procon.private.options.layerHideLocalAccounts {0}", this.OptionsSettings.LayerHideLocalAccounts);

                        stwConfig.WriteLine("procon.private.options.ShowRoundTimerConstantly {0}", this.OptionsSettings.ShowRoundTimerConstantly);

                        if (this.HttpWebServer != null) {
                            stwConfig.WriteLine("procon.private.httpWebServer.enable {0} {1} \"{2}\"", this.HttpWebServer.IsOnline, this.HttpWebServer.ListeningPort, this.HttpWebServer.BindingAddress);
                        }

                        stwConfig.Write("procon.private.options.trustedHostDomainsPorts");
                        foreach (TrustedHostWebsitePort trusted in this.OptionsSettings.TrustedHostsWebsitesPorts) {
                            stwConfig.Write(" {0} {1}", trusted.HostWebsite, trusted.Port);
                        }
                        stwConfig.WriteLine(String.Empty);
                        //stwConfig.WriteLine("procon.private.options.trustedHostDomainsPorts {0}", String.Join(" ", this.m_frmOptions.TrustedHostDomainsPorts.ToArray()));

                        foreach (PRoConClient prcClient in this.Connections) {

                            string strAddServerCommand = String.Format("procon.private.servers.add \"{0}\" {1}", prcClient.HostName, prcClient.Port);

                            if (prcClient.Password.Length > 0) {
                                strAddServerCommand = String.Format("{0} \"{1}\"", strAddServerCommand, prcClient.Password);

                                if (prcClient.Username.Length > 0) {
                                    strAddServerCommand = String.Format("{0} \"{1}\"", strAddServerCommand, prcClient.Username);
                                }
                            }

                            if (prcClient.CurrentServerInfo != null) {
                                stwConfig.WriteLine("procon.private.servers.name \"{0}\" {1} \"{2}\"", prcClient.HostName, prcClient.Port, prcClient.CurrentServerInfo.ServerName);
                            }

                            stwConfig.WriteLine(strAddServerCommand);

                            if (prcClient.AutomaticallyConnect == true) {
                                stwConfig.WriteLine("procon.private.servers.autoconnect \"{0}\" {1}", prcClient.HostName, prcClient.Port);
                            }
                        }

                        stwConfig.Close();
                    }
                }
                catch (Exception e) {
                    FrostbiteConnection.LogError("SaveMainConfig", String.Empty, e);
                }
                finally {
                    if (stmProconConfigFile != null) {
                        stmProconConfigFile.Close();
                    }
                }
            }
        }

        public void ExecutePRoConCommand(object objSender, List<string> lstWords, int iRecursion) {

            if (lstWords.Count >= 4 && String.Compare(lstWords[0], "procon.protected.weapons.add", true) == 0 && objSender is PRoConClient) {

                if (((PRoConClient)objSender).Weapons.Contains(lstWords[2]) == false &&
                    Enum.IsDefined(typeof(Kits), lstWords[1]) == true &&
                    Enum.IsDefined(typeof(WeaponSlots), lstWords[3]) == true &&
                    Enum.IsDefined(typeof(DamageTypes), lstWords[4]) == true) {
                    //this.SavedWindowState = (System.Windows.Forms.FormWindowState)Enum.Parse(typeof(System.Windows.Forms.FormWindowState), lstWords[1]);

                    ((PRoConClient)objSender).Weapons.Add(
                            new Weapon(
                                (Kits)Enum.Parse(typeof(Kits), lstWords[1]),
                                lstWords[2],
                                (WeaponSlots)Enum.Parse(typeof(WeaponSlots), lstWords[3]),
                                (DamageTypes)Enum.Parse(typeof(DamageTypes), lstWords[4])
                            )
                        );
                }
            }
            else if (lstWords.Count >= 1 && String.Compare(lstWords[0], "procon.protected.weapons.clear", true) == 0 && objSender is PRoConClient) {
                ((PRoConClient)objSender).Weapons.Clear();
            }
            else if (lstWords.Count >= 1 && String.Compare(lstWords[0], "procon.protected.zones.clear", true) == 0 && objSender is PRoConClient) {
                ((PRoConClient)objSender).MapGeometry.MapZones.Clear();
            }
            else if (lstWords.Count >= 4 && String.Compare(lstWords[0], "procon.protected.zones.add", true) == 0 && objSender is PRoConClient) {

                List<Point3D> points = new List<Point3D>();
                int iPoints = 0;

                if (int.TryParse(lstWords[4], out iPoints) == true) {

                    for (int i = 0, iOffset = 5; i < iPoints && iOffset + 3 <= lstWords.Count; i++) {
                        points.Add(new Point3D(lstWords[iOffset++], lstWords[iOffset++], lstWords[iOffset++]));
                    }
                }

                if (((PRoConClient)objSender).MapGeometry.MapZones.Contains(lstWords[1]) == false) {
                    ((PRoConClient)objSender).MapGeometry.MapZones.Add(new MapZoneDrawing(lstWords[1], lstWords[2], lstWords[3], points.ToArray(), true));
                }
            }


            else if (lstWords.Count >= 3 && String.Compare(lstWords[0], "procon.protected.specialization.add", true) == 0 && objSender is PRoConClient) {

                if (((PRoConClient)objSender).Specializations.Contains(lstWords[2]) == false &&
                    Enum.IsDefined(typeof(SpecializationSlots), lstWords[1]) == true) {

                    ((PRoConClient)objSender).Specializations.Add(new Specialization((SpecializationSlots)Enum.Parse(typeof(SpecializationSlots), lstWords[1]), lstWords[2]));
                }

            }
            else if (lstWords.Count >= 1 && String.Compare(lstWords[0], "procon.protected.specialization.clear", true) == 0 && objSender is PRoConClient) {
                ((PRoConClient)objSender).Specializations.Clear();
            }
            else if (lstWords.Count >= 5 && String.Compare(lstWords[0], "procon.protected.teamnames.add", true) == 0 && objSender is PRoConClient) {

                int iTeamID = 0;
                if (int.TryParse(lstWords[2], out iTeamID) == true) {
                    ((PRoConClient)objSender).ProconProtectedTeamNamesAdd(lstWords[1], iTeamID, lstWords[3], lstWords[4]);
                }
            }
            else if (lstWords.Count >= 6 && String.Compare(lstWords[0], "procon.protected.maps.add", true) == 0 && objSender is PRoConClient) {
                int iDefaultSquadID = 0;
                if (int.TryParse(lstWords[5], out iDefaultSquadID) == true) {
                    ((PRoConClient)objSender).ProconProtectedMapsAdd(lstWords[1], lstWords[2], lstWords[3], lstWords[4], iDefaultSquadID);
                }
            }
            else if (lstWords.Count >= 4 && String.Compare(lstWords[0], "procon.protected.plugins.setVariable", true) == 0 && objSender is PRoConClient) {

                string strUnescapedNewlines = lstWords[3].Replace(@"\n", "\n");
                strUnescapedNewlines = strUnescapedNewlines.Replace(@"\r", "\r");

                ((PRoConClient)objSender).ProconProtectedPluginSetVariable(lstWords[1], lstWords[2], strUnescapedNewlines);
            }
            else if (lstWords.Count >= 3 && String.Compare(lstWords[0], "procon.protected.vars.set", true) == 0 && objSender is PRoConClient) {
                ((PRoConClient)objSender).Variables.SetVariable(lstWords[1], lstWords[2]);
            }
            else if (lstWords.Count >= 3 && String.Compare(lstWords[0], "procon.protected.layer.setPrivileges", true) == 0 && objSender is PRoConClient) {

                CPrivileges sprPrivs = new CPrivileges();
                UInt32 ui32Privileges = 0;

                if (UInt32.TryParse(lstWords[2], out ui32Privileges) == true) {
                    sprPrivs.PrivilegesFlags = ui32Privileges;
                    if (this.AccountsList.Contains(lstWords[1]) == true) {
                        ((PRoConClient)objSender).ProconProtectedLayerSetPrivileges(this.AccountsList[lstWords[1]], sprPrivs);
                    }
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.send", true) == 0 && objSender is PRoConClient) {
                lstWords.RemoveAt(0);

                // Block them from changing the admin password to X
                if (String.Compare(lstWords[0], "vars.adminPassword", true) != 0) {
                     ((PRoConClient)objSender).SendRequest(lstWords);
                }
            }
            else if (lstWords.Count >= 3 && String.Compare(lstWords[0], "procon.public.accounts.create", true) == 0) {
                this.AccountsList.CreateAccount(lstWords[1], lstWords[2]);
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.public.accounts.delete", true) == 0) {
                this.AccountsList.DeleteAccount(lstWords[1]);
            }
            else if (lstWords.Count >= 3 && String.Compare(lstWords[0], "procon.public.accounts.setPassword", true) == 0) {
                this.AccountsList.ChangePassword(lstWords[1], lstWords[2]);
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.config.exec", true) == 0 && objSender is PRoConClient) {
                if (iRecursion < 5) {
                    ((PRoConClient)objSender).ExecuteConnectionConfig(lstWords[1], iRecursion, lstWords.Count > 2 ? lstWords.GetRange(2, lstWords.Count - 2) : null);
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.pluginconsole.write", true) == 0 && objSender is PRoConClient) {
                ((PRoConClient)objSender).PluginConsole.Write(lstWords[1]);
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.console.write", true) == 0 && objSender is PRoConClient) {
                ((PRoConClient)objSender).Console.Write(lstWords[1]);
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.chat.write", true) == 0 && objSender is PRoConClient) {
                ((PRoConClient)objSender).ChatConsole.WriteViaCommand(lstWords[1]);
            }
            else if (lstWords.Count >= 5 && String.Compare(lstWords[0], "procon.protected.events.write", true) == 0 && objSender is PRoConClient) {
                
                // EventType etType, CapturableEvents ceEvent, string strEventText, DateTime dtLoggedTime, string instigatingAdmin

                if (Enum.IsDefined(typeof(EventType), lstWords[1]) == true && Enum.IsDefined(typeof(CapturableEvents), lstWords[2]) == true) {
                    
                    EventType type = (EventType)Enum.Parse(typeof(EventType), lstWords[1]);
                    CapturableEvents cappedEventType = (CapturableEvents)Enum.Parse(typeof(CapturableEvents), lstWords[2]);

                    CapturedEvent cappedEvent = new CapturedEvent(type, cappedEventType, lstWords[3], DateTime.Now, lstWords[4]);
                    
                    ((PRoConClient)objSender).EventsLogging.ProcessEvent(cappedEvent);
                }
            } 

            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.layer.enable", true) == 0 && objSender is PRoConClient) {
                // procon.protected.layer.enable <true> <port>
                bool blEnabled = false;
                UInt16 ui16Port = 0;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {

                    if (lstWords.Count >= 5) {
                        UInt16.TryParse(lstWords[2], out ui16Port);
                        ((PRoConClient)objSender).ProconProtectedLayerEnable(blEnabled, ui16Port, lstWords[3], lstWords[4]);
                    }
                    else {
                        ((PRoConClient)objSender).ProconProtectedLayerEnable(blEnabled, 27260, "0.0.0.0", "PRoCon[%servername%]");
                    }
                }
            }
            else if (lstWords.Count >= 1 && String.Compare(lstWords[0], "procon.protected.teamnames.clear", true) == 0 && objSender is PRoConClient) {
                ((PRoConClient)objSender).ProconProtectedTeamNamesClear();
            }
            else if (lstWords.Count >= 1 && String.Compare(lstWords[0], "procon.protected.maps.clear", true) == 0 && objSender is PRoConClient) {
                ((PRoConClient)objSender).ProconProtectedMapsClear();
            }
            else if (lstWords.Count >= 1 && String.Compare(lstWords[0], "procon.protected.reasons.clear", true) == 0 && objSender is PRoConClient) {
                ((PRoConClient)objSender).ProconProtectedReasonsClear();
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.reasons.add", true) == 0 && objSender is PRoConClient) {
                ((PRoConClient)objSender).ProconProtectedReasonsAdd(lstWords[1]);
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.serverversions.clear", true) == 0 && objSender is PRoConClient)
            {
                ((PRoConClient)objSender).ProconProtectedServerVersionsClear(lstWords[1]);
            }
            else if (lstWords.Count >= 4 && String.Compare(lstWords[0], "procon.protected.serverversions.add", true) == 0 && objSender is PRoConClient)
            {
                ((PRoConClient)objSender).ProconProtectedServerVersionsAdd(lstWords[1], lstWords[2], lstWords[3]);
            }
            else if (lstWords.Count >= 3 && String.Compare(lstWords[0], "procon.protected.plugins.enable", true) == 0 && objSender is PRoConClient)
            {

                bool blEnabled = false;

                if (bool.TryParse(lstWords[2], out blEnabled) == true) {
                    ((PRoConClient)objSender).ProconProtectedPluginEnable(lstWords[1], blEnabled);
                }
            }
            else if (lstWords.Count >= 3 && String.Compare(lstWords[0], "procon.private.servers.add", true) == 0 && objSender == this) {
                // add IP port [password [username]]
                // procon.private.servers.add "127.0.0.1" 27260 "Password" "Phogue"
                UInt16 ui16Port = 0;
                if (UInt16.TryParse(lstWords[2], out ui16Port) == true) {
                    if (lstWords.Count == 3) {
                        this.AddConnection(lstWords[1], ui16Port, String.Empty, String.Empty);
                    }
                    else if (lstWords.Count == 4) {
                        this.AddConnection(lstWords[1], ui16Port, String.Empty, lstWords[3]);
                    }
                    else if (lstWords.Count == 5) {
                        this.AddConnection(lstWords[1], ui16Port, lstWords[4], lstWords[3]);
                    }
                }
            }
            else if (lstWords.Count >= 3 && String.Compare(lstWords[0], "procon.private.servers.connect", true) == 0 && objSender == this) {

                if (this.Connections.Contains(lstWords[1] + ":" + lstWords[2]) == true) {
                    this.Connections[lstWords[1] + ":" + lstWords[2]].ProconPrivateServerConnect();
                }
            }
            else if (lstWords.Count >= 3 && String.Compare(lstWords[0], "procon.private.servers.autoconnect", true) == 0 && objSender == this) {
                if (this.Connections.Contains(lstWords[1] + ":" + lstWords[2]) == true) {

                    this.Connections[lstWords[1] + ":" + lstWords[2]].AutomaticallyConnect = true;

                    // Originally leaving it for the reconnect thread to pickup but needed a quicker effect.
                    this.Connections[lstWords[1] + ":" + lstWords[2]].ProconPrivateServerConnect();
                }
            }

            /*
            else if (lstWords.Count >= 3 && String.Compare(lstWords[0], "procon.private.servers.name", true) == 0 && objSender == this) {
                this.uscServerPlayerTreeviewListing.SetServerName(lstWords[1] + ":" + lstWords[2], lstWords[3]);
            }

            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.window.splitterPosition", true) == 0 && objSender == this) {
                int iPositionVar = 0;

                if (int.TryParse(lstWords[1], out iPositionVar) == true) {
                    if (iPositionVar >= this.spltTreeServers.Panel1MinSize && iPositionVar <= this.spltTreeServers.Width - this.spltTreeServers.Panel2MinSize) {
                        this.spltTreeServers.SplitterDistance = iPositionVar;
                    }
                }
            }
            */

            else if (lstWords.Count >= 6 && String.Compare(lstWords[0], "procon.private.window.position", true) == 0 && objSender == this) {

                Rectangle recWindowBounds = new Rectangle(0, 0, 1024, 768);
                int iPositionVar = 0;

                if (Enum.IsDefined(typeof(System.Windows.Forms.FormWindowState), lstWords[1]) == true) {
                    this.SavedWindowState = (System.Windows.Forms.FormWindowState)Enum.Parse(typeof(System.Windows.Forms.FormWindowState), lstWords[1]);

                    if (int.TryParse(lstWords[2], out iPositionVar) == true) {
                        if (iPositionVar >= 0) {
                            recWindowBounds.X = iPositionVar;
                        }
                    }

                    if (int.TryParse(lstWords[3], out iPositionVar) == true) {
                        if (iPositionVar >= 0) {
                            recWindowBounds.Y = iPositionVar;
                        }
                    }

                    if (int.TryParse(lstWords[4], out iPositionVar) == true) {
                        recWindowBounds.Width = iPositionVar;
                    }

                    if (int.TryParse(lstWords[5], out iPositionVar) == true) {
                        recWindowBounds.Height = iPositionVar;
                    }

                    this.SavedWindowBounds = recWindowBounds;
                }
                
            }







            // procon.private.httpWebServer.enable true 27360 "0.0.0.0"
            else if (lstWords.Count >= 4 && String.Compare(lstWords[0], "procon.private.httpWebServer.enable", true) == 0 && objSender == this) {

                bool blEnabled = false;
                string bindingAddress = "0.0.0.0";
                UInt16 ui16Port = 27360;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {

                    if (this.HttpWebServer != null) {
                        this.HttpWebServer.Shutdown();

                        this.HttpWebServer.ProcessRequest -= new HttpWebServer.ProcessResponseHandler(HttpWebServer_ProcessRequest);
                        this.HttpWebServer.HttpServerOnline -= new HttpWebServer.StateChangeHandler(HttpWebServer_HttpServerOnline);
                        this.HttpWebServer.HttpServerOffline -= new HttpWebServer.StateChangeHandler(HttpWebServer_HttpServerOffline);
                    }

                    bindingAddress = lstWords[3];
                    if (UInt16.TryParse(lstWords[2], out ui16Port) == false) {
                        ui16Port = 27360;
                    }

                    this.HttpWebServer = new HttpWebServer(bindingAddress, ui16Port);
                    this.HttpWebServer.ProcessRequest += new HttpWebServer.ProcessResponseHandler(HttpWebServer_ProcessRequest);
                    this.HttpWebServer.HttpServerOnline += new HttpWebServer.StateChangeHandler(HttpWebServer_HttpServerOnline);
                    this.HttpWebServer.HttpServerOffline += new HttpWebServer.StateChangeHandler(HttpWebServer_HttpServerOffline);

                    if (blEnabled == true) {
                        this.HttpWebServer.Start();
                    }
                }
            }

            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.setLanguage", true) == 0 && objSender == this) {

                // if it does not exist but they have explicity asked for it, see if we can load it up
                // this could not be loaded because it is running in lean mode.
                if (this.Languages.Contains(lstWords[1]) == false) {
                    this.Languages.LoadLocalizationFile(AppDomain.CurrentDomain.BaseDirectory + "Localization" + Path.DirectorySeparatorChar + lstWords[1], lstWords[1]);
                }

                if (this.Languages.Contains(lstWords[1]) == true) {
                    this.CurrentLanguage = this.Languages[lstWords[1]];
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.autoCheckDownloadUpdates", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.AutoCheckDownloadUpdates = blEnabled;

                    // Force an update check right now..
                    if (this.OptionsSettings.AutoCheckDownloadUpdates == true) {
                        //this.CheckVersion();
                        //this.VersionCheck("http://www.phogue.net/procon/version.php");
                    }
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.autoApplyUpdates", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.AutoApplyUpdates = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.consoleLogging", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.ConsoleLogging = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.eventsLogging", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.EventsLogging = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.chatLogging", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.ChatLogging = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.pluginLogging", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.PluginLogging = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.showtrayicon", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.ShowTrayIcon = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.minimizetotray", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.MinimizeToTray = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.closetotray", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.CloseToTray = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.runPluginsInSandbox", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.RunPluginsInTrustedSandbox = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.adminMoveMessage", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.AdminMoveMessage = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.chatDisplayAdminName", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.ChatDisplayAdminName = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.layerHideLocalPlugins", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.LayerHideLocalPlugins = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.layerHideLocalAccounts", true) == 0 && objSender == this) {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.LayerHideLocalAccounts = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.ShowRoundTimerConstantly", true) == 0 && objSender == this)
            {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true)
                {
                    this.OptionsSettings.ShowRoundTimerConstantly = blEnabled;
                }
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.private.options.allowAllODBCConnections", true) == 0 && objSender == this)
            {
                bool blEnabled = false;

                if (bool.TryParse(lstWords[1], out blEnabled) == true) {
                    this.OptionsSettings.AllowAllODBCConnections = blEnabled;
                }
            }
            else if (lstWords.Count >= 1 && String.Compare(lstWords[0], "procon.private.options.trustedHostDomainsPorts", true) == 0 && objSender == this) {

                lstWords.RemoveAt(0);

                UInt16 ui16Port = 0;
                for (int i = 0; i + 1 < lstWords.Count; i = i + 2) {
                    if (UInt16.TryParse(lstWords[i + 1], out ui16Port) == true) {
                        this.OptionsSettings.TrustedHostsWebsitesPorts.Add(new TrustedHostWebsitePort(lstWords[i], ui16Port));
                    }
                }
            }
            else if (lstWords.Count >= 3 && String.Compare(lstWords[0], "procon.protected.notification.write", true) == 0 && objSender is PRoConClient) {

                bool blError = false;

                if (lstWords.Count >= 4 && bool.TryParse(lstWords[3], out blError) == true) {
                    if (this.ShowNotification != null) {
                        FrostbiteConnection.RaiseEvent(this.ShowNotification.GetInvocationList(), 2000, lstWords[1], lstWords[2], blError);
                    }
                }
                else {
                    if (this.ShowNotification != null) {
                        FrostbiteConnection.RaiseEvent(this.ShowNotification.GetInvocationList(), 2000, lstWords[1], lstWords[2], false);
                    }
                }
            }

            else if (lstWords.Count >= 3 && String.Compare(lstWords[0], "procon.protected.playsound", true) == 0 && objSender is PRoConClient) {

                int iRepeat = 0;

                string blah = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media"), lstWords[1]);

                if (int.TryParse(lstWords[2], out iRepeat) == true && iRepeat > 0 && File.Exists(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media"), lstWords[1])) == true) {

                    //this.Invoke(new DispatchProconProtectedPlaySound(this.PlaySound), new object[] { lstWords[1], iRepeat });

                    ((PRoConClient)objSender).PlaySound(lstWords[1], iRepeat);
                }
            }
            else if (lstWords.Count >= 1 && String.Compare(lstWords[0], "procon.protected.stopsound", true) == 0 && objSender is PRoConClient) {
                //this.Invoke(new DispatchProconProtectedStopSound(this.StopSound), new object[] { default(SPlaySound) });
                ((PRoConClient)objSender).StopSound(default(PRoConClient.SPlaySound));
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.events.captures", true) == 0 && objSender is PRoConClient) {
                lstWords.RemoveAt(0);
                ((PRoConClient)objSender).EventsLogging.Settings = lstWords;
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.playerlist.settings", true) == 0 && objSender is PRoConClient) {
                lstWords.RemoveAt(0);
                ((PRoConClient)objSender).PlayerListSettings.Settings = lstWords;
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.chat.settings", true) == 0 && objSender is PRoConClient) {
                lstWords.RemoveAt(0);
                ((PRoConClient)objSender).ChatConsole.Settings = lstWords;
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.lists.settings", true) == 0 && objSender is PRoConClient) {
                lstWords.RemoveAt(0);
                ((PRoConClient)objSender).ListSettings.Settings = lstWords;
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.console.settings", true) == 0 && objSender is PRoConClient) {
                lstWords.RemoveAt(0);
                ((PRoConClient)objSender).Console.Settings = lstWords;
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.timezone_UTCoffset", true) == 0 && objSender is PRoConClient)
            {
                double UTCoffset;
                if (double.TryParse(lstWords[1], out UTCoffset) == true) {
                    ((PRoConClient)objSender).Game.UTCoffset = UTCoffset;
                } else {
                    ((PRoConClient)objSender).Game.UTCoffset = 0 ;
                }
            }
            else if (lstWords.Count >= 1 && String.Compare(lstWords[0], "procon.protected.tasks.clear", true) == 0 && objSender is PRoConClient)
            {
                ((PRoConClient)objSender).ProconProtectedTasksClear();
            }
            else if (lstWords.Count >= 2 && String.Compare(lstWords[0], "procon.protected.tasks.remove", true) == 0 && objSender is PRoConClient) {
                ((PRoConClient)objSender).ProconProtectedTasksRemove(lstWords[1]);
            }
            else if (lstWords.Count >= 1 && String.Compare(lstWords[0], "procon.protected.tasks.list", true) == 0 && objSender is PRoConClient) {

                ((PRoConClient)objSender).ProconProtectedTasksList();
            }
            else if (lstWords.Count >= 4 && String.Compare(lstWords[0], "procon.protected.tasks.add", true) == 0 && objSender is PRoConClient) {

                int iDelay = 0, iInterval = 1, iRepeat = -1;
                string strTaskName = String.Empty;

                if (int.TryParse(lstWords[1], out iDelay) == true && int.TryParse(lstWords[2], out iInterval) == true && int.TryParse(lstWords[3], out iRepeat) == true) {

                    lstWords.RemoveRange(0, 4);
                    ((PRoConClient)objSender).ProconProtectedTasksAdd(String.Empty, lstWords, iDelay, iInterval, iRepeat);
                }
                else if (lstWords.Count >= 5 && int.TryParse(lstWords[2], out iDelay) == true && int.TryParse(lstWords[3], out iInterval) == true && int.TryParse(lstWords[4], out iRepeat) == true) {
                    strTaskName = lstWords[1];
                    lstWords.RemoveRange(0, 5);
                    ((PRoConClient)objSender).ProconProtectedTasksAdd(strTaskName, lstWords, iDelay, iInterval, iRepeat);
                }
            }
            else if (lstWords.Count >= 1 && String.Compare(lstWords[0], "procon.protected.vars.list", true) == 0 && objSender is PRoConClient) {

                ((PRoConClient)objSender).Console.Write("Local Variables: [Variable] [Value]");

                foreach (Variable kvpVariable in ((PRoConClient)objSender).Variables) {
                    ((PRoConClient)objSender).Console.Write(String.Format("{0} \"{1}\"", kvpVariable.Name, kvpVariable.Value));
                }

                ((PRoConClient)objSender).Console.Write(String.Format("End of Local Variables List ({0} Variables)", ((PRoConClient)objSender).Variables.Count));
            }
            else if (lstWords.Count >= 1 && String.Compare(lstWords[0], "procon.protected.sv_vars.list", true) == 0 && objSender is PRoConClient) {

                ((PRoConClient)objSender).Console.Write("Server Variables: [Variable] [Value]");

                foreach (Variable kvpVariable in ((PRoConClient)objSender).SV_Variables) {
                    ((PRoConClient)objSender).Console.Write(String.Format("{0} \"{1}\"", kvpVariable.Name, kvpVariable.Value));
                }

                ((PRoConClient)objSender).Console.Write(String.Format("End of Server Variables List ({0} Variables)", ((PRoConClient)objSender).SV_Variables.Count));
            }
            else if (lstWords.Count >= 3 && String.Compare(lstWords[0], "procon.protected.plugins.call", true) == 0 && objSender is PRoConClient) {

                if (((PRoConClient)objSender).PluginsManager != null) {
                    if (((PRoConClient)objSender).PluginsManager.Plugins.LoadedClassNames.Contains(lstWords[1]) == true) {

                        string[] strParams = null;

                        if (lstWords.Count - 3 > 0) {
                            strParams = new string[lstWords.Count - 3];
                            lstWords.CopyTo(3, strParams, 0, lstWords.Count - 3);
                        }

                        ((PRoConClient)objSender).PluginsManager.InvokeOnEnabled(lstWords[1], lstWords[2], strParams);
                    }
                }
            }
            else if (lstWords.Count >= 6 && String.Compare(lstWords[0], "procon.private.tcadmin.enableLayer", true) == 0 && objSender == this) {

                if (this.Connections.Contains(String.Format("{0}:{1}", lstWords[1], lstWords[2])) == true) {
                    UInt16 ui16Port = 0;
                    UInt16.TryParse(lstWords[4], out ui16Port);

                    this.Connections[String.Format("{0}:{1}", lstWords[1], lstWords[2])].ProconProtectedLayerEnable(true, ui16Port, lstWords[3], lstWords[5]);
                }
            }
            else if (lstWords.Count >= 5 && String.Compare(lstWords[0], "procon.private.tcadmin.setPrivileges", true) == 0 && objSender == this) {

                if (this.Connections.Contains(String.Format("{0}:{1}", lstWords[1], lstWords[2])) == true) {

                    CPrivileges sprPrivs = new CPrivileges();
                    UInt32 ui32Privileges = 0;

                    if (UInt32.TryParse(lstWords[4], out ui32Privileges) == true && this.AccountsList.Contains(lstWords[3]) == true) { 
                        sprPrivs.PrivilegesFlags = ui32Privileges;
                        this.Connections[String.Format("{0}:{1}", lstWords[1], lstWords[2])].ProconProtectedLayerSetPrivileges(this.AccountsList[lstWords[3]], sprPrivs);
                    }
                }
            }
        }

        private void HttpWebServer_HttpServerOffline(HttpWebServer sender) {
            if (this.HttpServerOffline != null) {
                FrostbiteConnection.RaiseEvent(this.HttpServerOffline.GetInvocationList(), sender);
            }
        }

        private void HttpWebServer_HttpServerOnline(HttpWebServer sender) {
            if (this.HttpServerOnline != null) {
                FrostbiteConnection.RaiseEvent(this.HttpServerOnline.GetInvocationList(), sender);
            }
        }

        #endregion

        #region RSS Feed

        public void UpdateRss() {
            
            // Begin RSS Update
            if (this.BeginRssUpdate != null) {
                FrostbiteConnection.RaiseEvent(this.BeginRssUpdate.GetInvocationList(), this);
            }

            CDownloadFile downloadRssFeed = new CDownloadFile("http://phogue.net/feed/");
            downloadRssFeed.DownloadComplete += new CDownloadFile.DownloadFileEventDelegate(downloadRssFeed_DownloadComplete);
            downloadRssFeed.DownloadError += new CDownloadFile.DownloadFileEventDelegate(downloadRssFeed_DownloadError);
            downloadRssFeed.BeginDownload();
        }

        private void downloadRssFeed_DownloadComplete(CDownloadFile cdfSender) {

            string xmlDocumentText = Encoding.UTF8.GetString(cdfSender.CompleteFileData);

            XmlDocument rssDocument = new XmlDocument();

            try {
                rssDocument.LoadXml(xmlDocumentText);

                if (this.PackageManager != null) {
                    this.PackageManager.LoadRemotePackages(rssDocument);
                }

                if (this.RssUpdateSuccess != null) {
                    FrostbiteConnection.RaiseEvent(this.RssUpdateSuccess.GetInvocationList(), this, rssDocument);
                }
            }
            catch (Exception) { }

        }

        private void downloadRssFeed_DownloadError(CDownloadFile cdfSender) {

            // RSS Error
            if (this.RssUpdateError != null) {
                FrostbiteConnection.RaiseEvent(this.RssUpdateError.GetInvocationList(), this);
            }

        }

        #endregion

        #region IP to Country

        private readonly object m_objIpToCountryLocker = new object();

        public string GetCountryCode(string strIP) {

            string strReturnCode = String.Empty;

            lock (this.m_objIpToCountryLocker) {

                string[] a_strSplitIP = strIP.Split(new char[] { ':' });

                if (a_strSplitIP.Length >= 1) {
                    strReturnCode = this.m_clIpToCountry.lookupCountryCode(a_strSplitIP[0]).ToLower();
                }
            }

            return strReturnCode;
        }

        public string GetCountryName(string strIP) {
            string strReturnName = String.Empty;

            lock (this.m_objIpToCountryLocker) {

                string[] a_strSplitIP = strIP.Split(new char[] { ':' });

                if (a_strSplitIP.Length >= 1) {
                    strReturnName = this.m_clIpToCountry.lookupCountryName(a_strSplitIP[0]);
                }
            }

            return strReturnName;
        }

        #endregion

        #region Accounts

        public void SaveAccountsConfig() {
            if (this.LoadingAccountsFile == false && this.AccountsList != null) {
                FileStream stmProconConfigFile = null;

                try {

                    if (Directory.Exists(String.Format("{0}Configs{1}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar)) == false) {
                        Directory.CreateDirectory(String.Format("{0}Configs{1}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar));
                    }

                    stmProconConfigFile = new FileStream(String.Format("{0}Configs{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, "accounts.cfg"), FileMode.Create);

                    if (stmProconConfigFile != null) {
                        StreamWriter stwConfig = new StreamWriter(stmProconConfigFile, Encoding.Unicode);

                        stwConfig.WriteLine("/////////////////////////////////////////////");
                        stwConfig.WriteLine("// This config will be overwritten by procon.");
                        stwConfig.WriteLine("/////////////////////////////////////////////");

                        foreach (Account accAccount in this.AccountsList) {
                            stwConfig.WriteLine("procon.public.accounts.create \"{0}\" \"{1}\"", accAccount.Name, accAccount.Password);
                        }

                        stwConfig.Flush();
                        stwConfig.Close();
                    }
                }
                catch (Exception e) {
                    FrostbiteConnection.LogError("SaveAccountsConfig", String.Empty, e);
                }
                finally {
                    if (stmProconConfigFile != null) {
                        stmProconConfigFile.Close();
                    }
                }
            }
        }

        private void AccountsList_AccountRemoved(Account item) {
            item.AccountPasswordChanged -= new Account.AccountPasswordChangedHandler(accAccount_AccountPasswordChanged);

            this.SaveAccountsConfig();
        }

        private void AccountsList_AccountAdded(Account item) {
            item.AccountPasswordChanged += new Account.AccountPasswordChangedHandler(accAccount_AccountPasswordChanged);

            this.SaveAccountsConfig();
        }

        private void accAccount_AccountPasswordChanged(Account item) {
            this.SaveAccountsConfig();
        }

        #endregion

        #region Reconnection and Version Timer

        private int m_iVersionTicks = 0;
        private DateTime m_dtDayCheck = DateTime.Now;

        private void ReconnectVersionChecker() {

            while (this.m_isCheckerRunning == true) {
                Thread.Sleep(20000);

                // Loop through each connection
                foreach (PRoConClient prcClient in this.Connections) {

                    // If an error occurs
                    if (prcClient.State == ConnectionState.Error
                    || (prcClient.State != ConnectionState.Connected && prcClient.AutomaticallyConnect == true)
                    || (this.ConsoleMode == true && prcClient.State != ConnectionState.Connected)) {
                        prcClient.Connect();
                    }

                    //if (((prcClient.ConnectionError == true && prcClient.IsConnected == false) || (prcClient.IsConnected == false && prcClient.ManuallyDisconnected == false)) && prcClient.AutomaticallyConnect == true) {
                        
                    //}
                }

                // If it's ticked over to a new day..
                if (this.m_dtDayCheck.Day != DateTime.Now.Day) {

                    foreach (PRoConClient prcClient in this.Connections) {
                        if (prcClient.ChatConsole != null) {
                            prcClient.ChatConsole.Logging = false;
                            prcClient.ChatConsole.Logging = this.OptionsSettings.ChatLogging;
                        }

                        if (prcClient.EventsLogging != null) {
                            prcClient.EventsLogging.Logging = false;
                            prcClient.EventsLogging.Logging = this.OptionsSettings.EventsLogging;
                        }

                        if (prcClient.Console != null) {
                            prcClient.Console.Logging = false;
                            prcClient.Console.Logging = this.OptionsSettings.ConsoleLogging;
                        }
                            
                        if (prcClient.PluginConsole != null) {
                            prcClient.PluginConsole.Logging = false;
                            prcClient.PluginConsole.Logging = this.OptionsSettings.PluginLogging;
                        }
                    }
                }

                this.m_dtDayCheck = DateTime.Now;

                // If it's been 30 mins (this ticks every 20 seconds) and we're checking for updates..

                if (this.m_iVersionTicks >= 90 && this.OptionsSettings.AutoCheckDownloadUpdates == true) {

                    this.AutoUpdater.CheckVersion();

                    this.m_iVersionTicks = 0;
                }

                this.m_iVersionTicks++;
            }
        }

        #endregion

        public void Shutdown() {

            this.m_isCheckerRunning = false;

            // This is mostly for debug so I don't need to wait 20 seconds to exit
            try {
                this.m_thChecker.Abort();
            }
            catch (Exception) { }

            this.SaveAccountsConfig();
            this.SaveMainConfig();

            this.AutoUpdater.Shutdown();

            foreach (PRoConClient pcClient in this.Connections) {
                pcClient.StopSound(default(PRoConClient.SPlaySound));
                pcClient.ForceDisconnect();
                pcClient.Destroy();
            }

            if (this.HttpWebServer != null) {
                this.HttpWebServer.Shutdown();
                this.HttpWebServer = null;
            }
        }
    }
}
