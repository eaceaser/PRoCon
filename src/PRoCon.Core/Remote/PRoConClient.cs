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
using System.Text;
using System.IO;
using System.Net;
using System.Reflection;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Media;
using System.Timers;

using System.Security;
using System.Security.Permissions;

namespace PRoCon.Core.Remote {
    using Core;
    using Core.Players;
    using Core.Players.Items;
    using Core.Consoles;
    using Core.Variables;
    using Core.Lists;
    using Core.Remote.Layer;
    using Core.Accounts;
    using Core.Events;
    using Core.Settings;
    using Core.Maps;
    using Core.Battlemap;
    using Core.TextChatModeration;
    using Core.Plugin;

    public class PRoConClient {

        private readonly object m_objConfigSavingLocker = new object();

        private string m_strProconEventsUid;

        public PRoConApplication Parent { get; private set; }

        private bool m_gameModModified = false;
        private bool m_blLoadingSavingConnectionConfig = false;

        System.Timers.Timer m_taskTimer;
        //private Thread m_thTasks;
        //private bool m_isTasksRunning;

        private FrostbiteConnection m_connection;

        #region Event handlers

        public delegate void EmptyParamterHandler(PRoConClient sender);
        public event EmptyParamterHandler ConnectAttempt;
        public event EmptyParamterHandler ConnectSuccess;
        public event EmptyParamterHandler ConnectionClosed;
        
        public event EmptyParamterHandler LoginAttempt;
        public event EmptyParamterHandler Login;
        public event EmptyParamterHandler Logout;

        public event EmptyParamterHandler GameTypeDiscovered;

        public delegate void FailureHandler(PRoConClient sender, Exception exception);
        public event FailureHandler ConnectionFailure;

        public delegate void SocketExceptionHandler(PRoConClient sender, SocketException se);
        public event SocketExceptionHandler SocketException;

        public delegate void PassLayerEventHandler(PRoConClient sender, Packet packet);
        public event PassLayerEventHandler PassLayerEvent;

        public delegate void AuthenticationFailureHandler(PRoConClient sender, string strError);
        public event AuthenticationFailureHandler LoginFailure;

        public delegate void PunkbusterPlayerInfoHandler(PRoConClient sender, CPunkbusterInfo pbInfo);
        public event PunkbusterPlayerInfoHandler PunkbusterPlayerInfo;
        public event EmptyParamterHandler PunkbusterBeginPlayerInfo;
        public event EmptyParamterHandler PunkbusterEndPlayerInfo;

        public delegate void PunkbusterBanHandler(PRoConClient sender, CBanInfo cbiUnbannedPlayer);
        public event PunkbusterBanHandler PunkbusterPlayerBanned;
        public event PunkbusterBanHandler PunkbusterPlayerUnbanned;

        public delegate void FullBanListListHandler(PRoConClient sender, List<CBanInfo> lstBans);
        public event FullBanListListHandler FullBanListList;

        public delegate void FullTextChatModerationListListHandler(PRoConClient sender, TextChatModerationDictionary moderationList);
        public event FullTextChatModerationListListHandler FullTextChatModerationListList;

        // These events are in PRoConClient to determine if it's a recompile or not.
        public event EmptyParamterHandler CompilingPlugins;
        public event EmptyParamterHandler RecompilingPlugins;
        public event EmptyParamterHandler PluginsCompiled;

        public delegate void ProconAdminSayingHandler(PRoConClient sender, string strAdminStack, string strMessage, CPlayerSubset spsAudience);
        public event ProconAdminSayingHandler ProconAdminSaying;

        public delegate void ProconAdminYellingHandler(PRoConClient sender, string strAdminStack, string strMessage, int iMessageDuration, CPlayerSubset spsAudience);
        public event ProconAdminYellingHandler ProconAdminYelling;

        public delegate void ProconPrivilegesHandler(PRoConClient sender, CPrivileges spPrivs);
        public event ProconPrivilegesHandler ProconPrivileges;

        public delegate void ProconVersionHandler(PRoConClient sender, Version version);
        public event ProconVersionHandler ProconVersion;

        public delegate void ReceiveProconVariableHandler(PRoConClient sender, string strVariable, string strValue);
        public event ReceiveProconVariableHandler ReceiveProconVariable;

        // List/Edit/Create/Delete Accounts through the Parent Layer Control.
        public delegate void RemoteAccountLoginStatusHandler(PRoConClient sender, string accountName, bool isOnline);
        public event RemoteAccountLoginStatusHandler RemoteAccountLoggedIn;

        public event EmptyParamterHandler RemoteAccountChangePassword;

        public delegate void RemoteAccountAlteredHandler(PRoConClient sender, string accountName, CPrivileges accountPrivileges);
        public event RemoteAccountAlteredHandler RemoteAccountAltered;

        public delegate void RemoteAccountHandler(PRoConClient sender, string accountName);
        public event RemoteAccountHandler RemoteAccountCreated;
        public event RemoteAccountHandler RemoteAccountDeleted;

        // List/Enable/Disable remote plugins via PLC.
        public delegate void RemoteEnabledPluginsHandler(PRoConClient sender, List<string> enabledPluginClasses);
        public event RemoteEnabledPluginsHandler RemoteEnabledPlugins;

        public delegate void RemoteLoadedPluginsHandler(PRoConClient sender, Dictionary<string, PluginDetails> loadedPlugins);
        public event RemoteLoadedPluginsHandler RemoteLoadedPlugins;

        public delegate void RemotePluginLoadedHandler(PRoConClient sender, PluginDetails spdDetails);
        public event RemotePluginLoadedHandler RemotePluginLoaded;

        public delegate void RemotePluginEnabledHandler(PRoConClient sender, string strClassName, bool isEnabled);
        public event RemotePluginEnabledHandler RemotePluginEnabled;

        public delegate void RemotePluginVariablesHandler(PRoConClient sender, string strClassName, List<CPluginVariable> lstVariables);
        public event RemotePluginVariablesHandler RemotePluginVariables;

        public delegate void AutomaticallyConnectHandler(PRoConClient sender, bool isEnabled);
        public event AutomaticallyConnectHandler AutomaticallyConnectChanged;

        public new delegate void PlayerSpawnedHandler(PRoConClient sender, string soldierName, Inventory spawnedInventory);
        public new event PlayerSpawnedHandler PlayerSpawned;

        public new delegate void PlayerKilledHandler(PRoConClient sender, Kill kKillerVictimDetails);
        public new event PlayerKilledHandler PlayerKilled;

        public delegate void MapZoneEditedHandler(PRoConClient sender, MapZoneDrawing zone);
        public event MapZoneEditedHandler MapZoneCreated;
        public event MapZoneEditedHandler MapZoneDeleted;
        public event MapZoneEditedHandler MapZoneModified;

        public delegate void ListMapZonesHandler(PRoConClient sender, List<MapZoneDrawing> zones);
        public event ListMapZonesHandler ListMapZones;

        public delegate void ReadRemoteConsoleHandler(PRoConClient sender, DateTime loggedTime, string text);
        public event ReadRemoteConsoleHandler ReadRemotePluginConsole;
        public event ReadRemoteConsoleHandler ReadRemoteChatConsole;

        #region Packages
        
        public delegate void RemotePackagesInstallHandler(PRoConClient sender, string uid);
        public event RemotePackagesInstallHandler PackageDownloading;
        public event RemotePackagesInstallHandler PackageDownloaded;
        public event RemotePackagesInstallHandler PackageInstalling;

        public delegate void RemotePackagesInstallErrorHandler(PRoConClient sender, string uid, string error);
        public event RemotePackagesInstallErrorHandler PackageDownloadError;

        public delegate void RemotePackagesInstalledHandler(PRoConClient sender, string uid, bool restart);
        public event RemotePackagesInstalledHandler PackageInstalled;

        #endregion

        #endregion

        #region Attributes

        public FrostbiteClient Game {
            get;
            private set;
        }

        public string InstigatingAccountName { get; private set; }

        public string Password {
            get;
            set;
        }

        public bool IsPRoConConnection {
            get;
            private set;
        }

        public EventCaptures EventsLogging {
            get;
            private set;
        }

        public PluginManager PluginsManager {
            get;
            private set;
        }

        public PluginConsole PluginConsole {
            get;
            private set;
        }

        public ConnectionConsole Console {
            get;
            private set;
        }

        public PunkbusterConsole PunkbusterConsole {
            get;
            private set;
        }

        public ChatConsole ChatConsole {
            get;
            private set;
        }

        public PlayerDictionary PlayerList {
            get;
            private set;
        }

        public PlayerListSettings PlayerListSettings {
            get;
            private set;
        }

        public ListsSettings ListSettings {
            get;
            private set;
        }

        public ServerSettings ServerSettings {
            get;
            private set;
        }

        public CServerInfo CurrentServerInfo {
            get;
            private set;
        }

        public List<CTeamName> TeamNameList {
            get;
            private set;
        }

        public NotificationList<CMap> MapListPool {
            get;
            private set;
        }

        public NotificationList<string> ReservedSlotList {
            get;
            private set;
        }

        public CLocalization Language {
            get {
                return this.Parent.CurrentLanguage;
            }
        }

        public CPrivileges Privileges {
            get;
            private set;
        }

        public Version ConnectedLayerVersion { get; private set; }

        public PRoConLayer Layer {
            get;
            private set;
        }

        public NotificationList<string> Reasons {
            get;
            private set;
        }

        public VariableDictionary Variables {
            get;
            private set;
        }

        public List<CBanInfo> FullVanillaBanList {
            get;
            private set;
        }


        public TextChatModerationDictionary FullTextChatModerationList {
            get;
            private set;
        }

        public MapGeometry MapGeometry {
            get;
            private set;
        }

        private bool m_blAutomaticallyConnect = false;
        public bool AutomaticallyConnect {
            get {
                return this.m_blAutomaticallyConnect;
            }
            set {
                this.m_blAutomaticallyConnect = value;

                if (this.AutomaticallyConnectChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.AutomaticallyConnectChanged.GetInvocationList(), this, value);
                }
            }
        }

        #region Connection State

        private bool IsConnected {
            get {
                bool isConnected = false;

                if (this.Game != null && this.Game.Connection != null) {
                    isConnected = this.Game.Connection.IsConnected;
                }
                else if (this.m_connection != null) {
                    isConnected = this.m_connection.IsConnected;
                }

                return isConnected;
            }
        }

        private bool IsConnecting {
            get {
                bool isConnecting = false;

                if (this.Game != null && this.Game.Connection != null) {
                    isConnecting = this.Game.Connection.IsConnecting;
                }
                else if (this.m_connection != null) {
                    isConnecting = this.m_connection.IsConnecting;
                }

                return isConnecting;
            }
        }

        private ConnectionState m_currentState;
        public ConnectionState State {
            get {
                ConnectionState currentState = this.m_currentState;

                if (this.IsConnected == true) {
                    currentState = ConnectionState.Connected;
                }
                else if (this.IsConnecting == true) {
                    currentState = ConnectionState.Connecting;
                }

                return currentState;
            }
            set {
                this.m_currentState = value;
            }
        }

        public bool IsLoggedIn {
            get {
                bool isLoggedIn = false;

                if (this.Game != null) {
                    isLoggedIn = this.Game.IsLoggedIn;
                }

                return isLoggedIn;
            }
        }

        #endregion

        /*
        public bool ManuallyDisconnected {
            get;
            private set;
        }
        */
        private List<Task> Tasks {
            get;
            set;
        }

        // Variables received by the server.
        //private Dictionary<string, string> m_dicSvLayerVariables;
        public VariableDictionary SV_Variables {
            get;
            private set;
        }

        public WeaponDictionary Weapons {
            get;
            private set;
        }

        public SpecializationDictionary Specializations {
            get;
            private set;
        }

        public string HostName {
            get;
            private set;
        }

        public ushort Port {
            get;
            private set;
        }

        private string m_strUsername = String.Empty;
        public string Username {
            get {
                return this.m_strUsername;
            }
            set {
                if (this.Game != null && this.Game.IsLoggedIn == false) {
                    this.m_strUsername = value;
                }
            }
        }

        public string HostNamePort {
            get { return this.HostName + ":" + this.Port.ToString(); }
        }

        public string FileHostNamePort {
            get { return this.HostName + "_" + this.Port.ToString(); }
        }

        /*
        public bool ConnectionError {
            get;
            private set;
        }
        */

        public string GameType {
            get {
                string gameType = String.Empty;

                if (this.Game != null) {
                    gameType = this.Game.GameType;
                }

                return gameType;
            }
        }

        public string VersionNumber {
            get;
            private set;
        }

        #endregion

        #region Layer Packet Passing

        internal struct SOriginalForwardedPacket {
            public UInt32 m_ui32OriginalSequence;
            public PRoConLayerClient m_sender;
            public List<string> m_lstWords;
        }

        // <UInt32, - Sequence number sent to the BFBC2 server
        // Packet> - Original sequence number to be used when passing it back to the PRoCon layer.
        private Dictionary<UInt32, SOriginalForwardedPacket> m_dicForwardedPackets;

        private Dictionary<string, string> m_dicUsernamesToUids;

        #endregion

        public PRoConClient(PRoConApplication praApplication, string hostName, ushort port, string username, string password) {

            this.HostName = hostName;
            this.Port = port;
            this.Password = password;
            this.m_strUsername = username;
            //this.m_uscParent = frmParent;
            this.Parent = praApplication;

            this.m_strProconEventsUid = String.Empty;

            m_dicForwardedPackets = new Dictionary<UInt32, SOriginalForwardedPacket>();

            this.Tasks = new List<Task>();
            this.VersionNumber = String.Empty;

            this.Layer = new PRoConLayer();

            this.m_dicUsernamesToUids = new Dictionary<string, string>() { { "SYSOP", "" } };
            this.InstigatingAccountName = String.Empty;

        }
        
        private void AssignEventHandlers() {

            if (this.Game != null) {

                this.Game.Login += new FrostbiteClient.EmptyParamterHandler(Game_Login);
                this.Game.LoginFailure += new FrostbiteClient.AuthenticationFailureHandler(Game_LoginFailure);
                this.Game.Logout += new FrostbiteClient.EmptyParamterHandler(Game_Logout);

                this.Game.ListPlayers += this.OnListPlayers;
                this.Game.PlayerLeft += this.OnPlayerLeft;
                this.Game.PunkbusterMessage += this.OnPunkbusterMessage;

                this.Game.ServerInfo += this.OnServerInfo;

                this.Game.BanListList += new FrostbiteClient.BanListListHandler(PRoConClient_BanListList);
                this.Game.TextChatModerationListAddPlayer += new FrostbiteClient.TextChatModerationListAddPlayerHandler(Game_TextChatModerationListAddPlayer);
                this.Game.TextChatModerationListRemovePlayer += new FrostbiteClient.TextChatModerationListRemovePlayerHandler(Game_TextChatModerationListRemovePlayer);
                this.Game.TextChatModerationListClear += new FrostbiteClient.EmptyParamterHandler(Game_TextChatModerationListClear);
                this.Game.TextChatModerationListList += new FrostbiteClient.TextChatModerationListListHandler(Game_TextChatModerationListList);
                this.Game.PlayerLimit += this.OnPlayerLimit;

                this.Game.ReservedSlotsList += new FrostbiteClient.ReservedSlotsListHandler(PRoConClient_ReservedSlotsList);
                this.Game.ReservedSlotsPlayerAdded += new FrostbiteClient.ReservedSlotsPlayerHandler(PRoConClient_ReservedSlotsPlayerAdded);
                this.Game.ReservedSlotsPlayerRemoved += new FrostbiteClient.ReservedSlotsPlayerHandler(PRoConClient_ReservedSlotsPlayerRemoved);

                this.Game.ResponseError += new FrostbiteClient.ResponseErrorHandler(PRoConClient_ResponseError);

                this.PluginsCompiled += new EmptyParamterHandler(ProConClient_PluginsCompiled);

                this.Game.PlayerSpawned += new FrostbiteClient.PlayerSpawnedHandler(PRoConClient_PlayerSpawned);
                this.Game.PlayerKilled += new FrostbiteClient.PlayerKilledHandler(PRoConClient_PlayerKilled);

            }
        }

        #region Saving/Loading Settings

        // Set to true just before the config is open so events thrown during the loading process
        // don't trigger a save.
        public void SaveConnectionConfig() {

            if (this.m_blLoadingSavingConnectionConfig == false && this.Layer != null && this.Layer.AccountPrivileges != null && this.PluginsManager != null && this.MapGeometry != null && this.MapGeometry.MapZones != null) {

                lock (this.m_objConfigSavingLocker) {

                    FileStream stmConnectionConfigFile = null;

                    try {
                        if (Directory.Exists(String.Format("{0}Configs{1}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar)) == false) {
                            Directory.CreateDirectory(String.Format("{0}Configs{1}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar));
                        }

                        string strSaveFile = String.Format(@"{0}Configs{1}{2}.cfg", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, this.FileHostNamePort);

                        stmConnectionConfigFile = new FileStream(strSaveFile + ".temp", FileMode.Create);

                        if (stmConnectionConfigFile != null) {
                            StreamWriter stwConfig = new StreamWriter(stmConnectionConfigFile, Encoding.Unicode);

                            stwConfig.WriteLine("/////////////////////////////////////////////");
                            stwConfig.WriteLine("// This config will be overwritten by procon.");
                            stwConfig.WriteLine("/////////////////////////////////////////////");

                            foreach (AccountPrivilege apPrivs in this.Layer.AccountPrivileges) {
                                stwConfig.WriteLine("procon.protected.layer.setPrivileges \"{0}\" {1}", apPrivs.Owner.Name, apPrivs.Privileges.PrivilegesFlags);
                            }

                            stwConfig.WriteLine("procon.protected.layer.enable {0} {1} \"{2}\" \"{3}\"", this.Layer.LayerEnabled, this.Layer.ListeningPort, this.Layer.BindingAddress, this.Layer.LayerNameFormat);

                            stwConfig.WriteLine("procon.protected.playerlist.settings " + String.Join(" ", this.PlayerListSettings.Settings.ToArray()));
                            stwConfig.WriteLine("procon.protected.chat.settings " + String.Join(" ", this.ChatConsole.Settings.ToArray()));
                            stwConfig.WriteLine("procon.protected.events.captures " + String.Join(" ", this.EventsLogging.Settings.ToArray()));
                            stwConfig.WriteLine("procon.protected.lists.settings " + String.Join(" ", this.ListSettings.Settings.ToArray()));
                            stwConfig.WriteLine("procon.protected.console.settings " + String.Join(" ", this.Console.Settings.ToArray()));
                            stwConfig.WriteLine("procon.protected.timezone_UTCoffset " + this.Game.UTCoffset);

                            foreach (MapZoneDrawing zone in this.MapGeometry.MapZones) {
                                stwConfig.WriteLine("procon.protected.zones.add \"{0}\" \"{1}\" \"{2}\" {3} {4}", zone.UID, zone.LevelFileName, zone.Tags, zone.ZonePolygon.Length, String.Join(" ", Point3D.ToStringList(zone.ZonePolygon).ToArray()));
                            }

                            foreach (string strClassName in new List<string>(this.PluginsManager.Plugins.LoadedClassNames)) {

                                stwConfig.WriteLine("procon.protected.plugins.enable \"{0}\" {1}", strClassName, this.PluginsManager.Plugins.EnabledClassNames.Contains(strClassName));

                                PluginDetails spdUpdatedDetails = this.PluginsManager.GetPluginDetails(strClassName);

                                foreach (CPluginVariable cpvVariable in spdUpdatedDetails.PluginVariables) {
                                    string strEscapedNewlines = CPluginVariable.Decode(cpvVariable.Value);
                                    strEscapedNewlines = strEscapedNewlines.Replace("\n", @"\n");
                                    strEscapedNewlines = strEscapedNewlines.Replace("\r", @"\r");
                                    strEscapedNewlines = strEscapedNewlines.Replace("\"", @"\""");

                                    stwConfig.WriteLine("procon.protected.plugins.setVariable \"{0}\" \"{1}\" \"{2}\"", strClassName, cpvVariable.Name, strEscapedNewlines);
                                }
                            }

                            // Now resave all cached plugin settings (plugin settings of of plugins that failed to load)
                            foreach (Plugin plugin in this.PluginsManager.Plugins) {
                                if (plugin.IsLoaded == false) {
                                    foreach (KeyValuePair<string, string> CachedPluginVariable in plugin.CacheFailCompiledPluginVariables) {
                                        stwConfig.WriteLine("procon.protected.plugins.setVariable \"{0}\" \"{1}\" \"{2}\"", plugin.ClassName, CachedPluginVariable.Key, CachedPluginVariable.Value);
                                    }
                                }
                            }
                            /*
                            foreach (KeyValuePair<string, Dictionary<string, string>> CachedPluginSettings in this.PluginsManager.CacheFailCompiledPluginVariables) {
                                foreach (KeyValuePair<string, string> CachedPluginVariable in CachedPluginSettings.Value) {
                                    stwConfig.WriteLine("procon.protected.plugins.setVariable \"{0}\" \"{1}\" \"{2}\"", CachedPluginSettings.Key, CachedPluginVariable.Key, CachedPluginVariable.Value);
                                }
                            }
                            */
                            stwConfig.Flush();
                            stwConfig.Close();

                            File.Copy(strSaveFile + ".temp", strSaveFile, true);
                            File.Delete(strSaveFile + ".temp");
                        }
                    }
                    catch (Exception e) {
                        FrostbiteConnection.LogError("SaveConnectionConfig", String.Empty, e);
                    }
                    finally {
                        if (stmConnectionConfigFile != null) {
                            stmConnectionConfigFile.Close();
                            stmConnectionConfigFile.Dispose();
                        }
                    }
                }
            }
        }

        public void ExecuteConnectionConfig(string strConfigFile, int iRecursion, List<string> lstArguments) {

            //FileStream stmConfigFile = null;
            try {

                if (File.Exists(String.Format("{0}Configs{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, strConfigFile)) == true) {

                    //stmConfigFile = new FileStream(String.Format(@"{0}Configs\{1}", AppDomain.CurrentDomain.BaseDirectory, strConfigFile), FileMode.Open);

                    string[] a_strConfigData = File.ReadAllLines(String.Format(@"{0}Configs{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, strConfigFile));

                    if (a_strConfigData != null) {

                        foreach (string strLine in a_strConfigData) {
                            if (strLine.Length > 0 && Regex.Match(strLine, "^[ ]+//.*").Success == false) { // AND not a comment..

                                if (lstArguments != null) {

                                    string strReplacedLine = strLine;

                                    for (int i = 0; i < lstArguments.Count; i++) {
                                        strReplacedLine = strReplacedLine.Replace(String.Format("%arg:{0}%", i), lstArguments[i]);
                                    }

                                    List<string> lstWordifiedCommand = Packet.Wordify(strReplacedLine);
                                    // procon.protected.config.demand 48543 CanKickPlayers procon.protected.send admin.say "You're not allowed to kick" player phogue
                                    if (lstWordifiedCommand.Count >= 3 && String.Compare(lstWordifiedCommand[0], "procon.protected.config.demand", true) == 0) {

                                        UInt32 ui32PrivilegesFlags = 0;

                                        if (UInt32.TryParse(lstWordifiedCommand[1], out ui32PrivilegesFlags) == true) {
                                            CPrivileges cpConfigPrivs = new CPrivileges(ui32PrivilegesFlags);
                                            bool blHasPrivileges = false;

                                            try {
                                                System.Reflection.PropertyInfo[] a_piAllProperties = cpConfigPrivs.GetType().GetProperties();

                                                foreach (System.Reflection.PropertyInfo pInfo in a_piAllProperties) {
                                                    if (String.Compare(pInfo.GetGetMethod().Name, "get_" + lstWordifiedCommand[2]) == 0) {
                                                        blHasPrivileges = (bool)pInfo.GetValue(cpConfigPrivs, null);
                                                        break;
                                                    }
                                                }

                                                if (blHasPrivileges == false) {

                                                    // If they have asked for a command on failure..
                                                    if (lstWordifiedCommand.Count > 3) {
                                                        this.Parent.ExecutePRoConCommand(this, lstWordifiedCommand.GetRange(3, lstWordifiedCommand.Count - 3), iRecursion++);
                                                    }

                                                    // Cancel execution of the config file, they don't have the demanded privileges.
                                                    break;
                                                }
                                            }
                                            catch (Exception e) {
                                                FrostbiteConnection.LogError("Parsing a config.", String.Empty, e);
                                                break;
                                            }
                                        }
                                        else {
                                            // Cancel execution of the config file, wrong format for demand.
                                            break;
                                        }
                                    }
                                    else {
                                        this.Parent.ExecutePRoConCommand(this, lstWordifiedCommand, iRecursion++);
                                    }
                                }
                                else {
                                    this.Parent.ExecutePRoConCommand(this, Packet.Wordify(strLine), iRecursion++);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e) {
                FrostbiteConnection.LogError("ExecuteConnectionConfig", String.Empty, e);
            }
        }

        #endregion

        #region Plugin setup & events

        public void CompilePlugins(System.Security.PermissionSet prmPluginSandbox) {

            Dictionary<string, List<CPluginVariable>> dicClassSavedVariables = new Dictionary<string, List<CPluginVariable>>();
            List<string> lstEnabledPlugins = null;

            // If it's a recompile save all the current variables.
            if (this.PluginsManager != null) {
                foreach (String strClassName in this.PluginsManager.Plugins.LoadedClassNames) {
                    if (dicClassSavedVariables.ContainsKey(strClassName) == false) {
                        dicClassSavedVariables.Add(strClassName, this.PluginsManager.GetPluginVariables(strClassName));
                        //dicClassSavedVariables.Add(strClassName, ProConClient.GetSvVariables(this.m_cpPlugins.InvokeOnLoaded(strClassName, "GetPluginVariables", null)));
                    }
                }

                lstEnabledPlugins = this.PluginsManager.Plugins.EnabledClassNames;

                this.PluginsManager.Unload();

                this.PluginsManager = new PluginManager(this);
                if (this.RecompilingPlugins != null) {
                    FrostbiteConnection.RaiseEvent(this.RecompilingPlugins.GetInvocationList(), this);
                }
            }
            else {
                this.PluginsManager = new PluginManager(this);
                if (this.CompilingPlugins != null) {
                    FrostbiteConnection.RaiseEvent(this.CompilingPlugins.GetInvocationList(), this);
                }
            }

            this.PluginsManager.CompilePlugins(prmPluginSandbox);

            if (this.PluginsCompiled != null) {
                FrostbiteConnection.RaiseEvent(this.PluginsCompiled.GetInvocationList(), this);
            }

            // Reload all the variables if it's a recompile
            foreach (KeyValuePair<string, List<CPluginVariable>> kvpPluginVariables in dicClassSavedVariables) {
                foreach (CPluginVariable cpvVariable in kvpPluginVariables.Value) {
                    this.PluginsManager.SetPluginVariable(kvpPluginVariables.Key, cpvVariable.Name, cpvVariable.Value);
                    //this.Plugins.InvokeOnLoaded(kvpPluginVariables.Key, "SetPluginVariable", new object[] { cpvVariable.Name, cpvVariable.Value });
                }
            }

            if (lstEnabledPlugins != null) {
                foreach (string strEnabledClass in lstEnabledPlugins) {
                    this.PluginsManager.EnablePlugin(strEnabledClass);
                }
            }
        }

        public List<CMap> GetMapDefines() {
            return new List<CMap>(this.MapListPool);
        }

        public bool TryGetLocalized(string strLanguageCode, out string strLocalizedText, string strVariable, string[] a_strArguements) {

            bool isSuccess = false;
            strLocalizedText = String.Empty;

            if (strLanguageCode == null) {
                isSuccess = this.Language.TryGetLocalized(out strLocalizedText, strVariable, a_strArguements);
            }
            else {
                string strCountryCode = String.Empty;

                foreach (CLocalization LanguageFile in this.Parent.Languages) {
                    if (LanguageFile.TryGetLocalized(out strCountryCode, "file.countrycode") == true && String.Compare(strCountryCode, strLanguageCode, true) == 0) {
                        isSuccess = this.Language.TryGetLocalized(out strLocalizedText, strVariable, a_strArguements);
                        break;
                    }
                }
            }

            return isSuccess;
        }

        public string GetVariable(string strVariable) {
            return this.Variables.GetVariable<string>(strVariable, String.Empty);
        }

        public string GetSvVariable(string strVariable) {
            return this.SV_Variables.GetVariable<string>(strVariable, String.Empty);
        }

        public CPrivileges GetAccountPrivileges(string strAccountName) {

            CPrivileges spReturn = default(CPrivileges);

            if (this.Layer.AccountPrivileges.Contains(strAccountName) == true) {
                spReturn = this.Layer.AccountPrivileges[strAccountName].Privileges;
            }

            return spReturn;
        }

        public void ExecuteCommand(List<string> lstCommand) {
            if (lstCommand != null) {
                this.Parent.ExecutePRoConCommand(this, lstCommand, 0);
            }
        }

        public WeaponDictionary GetWeaponDefines() {
            return (WeaponDictionary)this.Weapons.Clone();
        }

        public SpecializationDictionary GetSpecializationDefines() {
            return (SpecializationDictionary)this.Specializations.Clone();
        }

        #endregion

        private void Connection_ConnectionClosed(FrostbiteConnection sender) {
            //sender.PacketReceived -= new FrostbiteConnection.PacketDispatchHandler(Connection_PacketRecieved);

            if (this.ConnectionClosed != null) {
                FrostbiteConnection.RaiseEvent(this.ConnectionClosed.GetInvocationList(), this);
            }
        }

        private void Connection_ConnectionFailure(FrostbiteConnection sender, Exception exception) {
            this.State = ConnectionState.Error;

            //sender.PacketReceived -= new FrostbiteConnection.PacketDispatchHandler(Connection_PacketRecieved);

            if (this.ConnectionFailure != null) {
                FrostbiteConnection.RaiseEvent(this.ConnectionFailure.GetInvocationList(), this, exception);
            }
        }

        private void Connection_SocketException(FrostbiteConnection sender, SocketException se) {
            this.State = ConnectionState.Error;

            //sender.PacketReceived -= new FrostbiteConnection.PacketDispatchHandler(Connection_PacketRecieved);

            if (this.SocketException != null) {
                FrostbiteConnection.RaiseEvent(this.SocketException.GetInvocationList(), this, se);
            }
        }

        private void InitialSetup() {
            if (this.Game != null) {
                this.m_blLoadingSavingConnectionConfig = true;

                this.AssignEventHandlers();

                // Assume full access until we're told otherwise.
                this.Layer.Initialize(this.Parent, this);

                // I may move these events to within Layer, depends on the end of the restructure.
                this.Layer.LayerOnline += new PRoConLayer.LayerEmptyParameterHandler(Layer_LayerOnline);
                this.Layer.LayerOffline += new PRoConLayer.LayerEmptyParameterHandler(Layer_LayerOffline);
                this.Layer.AccountPrivileges.AccountPrivilegeAdded += new AccountPrivilegeDictionary.AccountPrivilegeAlteredHandler(AccountPrivileges_AccountPrivilegeAdded);
                this.Layer.AccountPrivileges.AccountPrivilegeRemoved += new AccountPrivilegeDictionary.AccountPrivilegeAlteredHandler(AccountPrivileges_AccountPrivilegeRemoved);

                foreach (AccountPrivilege apPriv in this.Layer.AccountPrivileges) {
                    apPriv.AccountPrivilegesChanged += new AccountPrivilege.AccountPrivilegesChangedHandler(item_AccountPrivilegesChanged);
                }

                this.Privileges = new CPrivileges(CPrivileges.FullPrivilegesFlags);
                this.EventsLogging = new EventCaptures(this);
                this.Console = new ConnectionConsole(this);
                this.PunkbusterConsole = new PunkbusterConsole(this);
                this.ChatConsole = new ChatConsole(this);
                this.PluginConsole = new PluginConsole(this);
                this.MapGeometry = new MapGeometry(this);
                this.MapGeometry.MapZones.MapZoneAdded += new MapZoneDictionary.MapZoneAlteredHandler(MapZones_MapZoneAdded);
                this.MapGeometry.MapZones.MapZoneChanged += new MapZoneDictionary.MapZoneAlteredHandler(MapZones_MapZoneChanged);
                this.MapGeometry.MapZones.MapZoneRemoved += new MapZoneDictionary.MapZoneAlteredHandler(MapZones_MapZoneRemoved);

                if (this.CurrentServerInfo == null) {
                    this.CurrentServerInfo = new CServerInfo();
                }

                this.ListSettings = new ListsSettings(this);
                this.ServerSettings = new ServerSettings(this);
                this.PlayerListSettings = new PlayerListSettings();
                this.PlayerList = new PlayerDictionary();
                this.TeamNameList = new List<CTeamName>();
                this.MapListPool = new NotificationList<CMap>();
                this.ReservedSlotList = new NotificationList<string>();
                this.Variables = new VariableDictionary();
                this.SV_Variables = new VariableDictionary();
                this.Reasons = new NotificationList<string>();
                this.FullVanillaBanList = new List<CBanInfo>();
                this.FullTextChatModerationList = new TextChatModerationDictionary();
                this.Weapons = new WeaponDictionary();
                this.Specializations = new SpecializationDictionary();

                if (Regex.Match(this.HostName, @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$").Success == true) {
                    this.Variables.SetVariable("SERVER_COUNTRY", this.Parent.GetCountryName(this.HostName));
                    this.Variables.SetVariable("SERVER_COUNTRY_CODE", this.Parent.GetCountryCode(this.HostName));
                }
                else {
                    IPAddress ipServer = FrostbiteConnection.ResolveHostName(this.HostName);
                    this.Variables.SetVariable("SERVER_COUNTRY", this.Parent.GetCountryName(ipServer.ToString()));
                    this.Variables.SetVariable("SERVER_COUNTRY_CODE", this.Parent.GetCountryCode(ipServer.ToString()));
                }

                this.Console.Logging = this.Parent.OptionsSettings.ConsoleLogging;
                this.EventsLogging.Logging = this.Parent.OptionsSettings.EventsLogging;
                this.ChatConsole.Logging = this.Parent.OptionsSettings.ChatLogging;
                this.PluginConsole.Logging = this.Parent.OptionsSettings.PluginLogging;

                //this.m_blLoadingSavingConnectionConfig = true;

                if (this.CurrentServerInfo.GameMod == GameMods.None) {
                    this.ExecuteConnectionConfig(this.Game.GameType + ".def", 0, null);
                }
                else {
                    this.ExecuteConnectionConfig(this.Game.GameType + "." + this.CurrentServerInfo.GameMod + ".def", 0, null);
                }
                
                this.ExecuteConnectionConfig("reasons.cfg", 0, null);
                
                lock (this.Parent) {
                    this.CompilePlugins(this.Parent.OptionsSettings.PluginPermissions);
                }

                this.ExecuteConnectionConfig(this.FileHostNamePort + ".cfg", 0, null);


                //this.m_blLoadingSavingConnectionConfig = false;

                // this.ManuallyDisconnected = true;

                // this.ConnectionError = false;

                this.BeginLoginSequence();

                this.m_blLoadingSavingConnectionConfig = false;
            }
        }

        private void BeginLoginSequence() {
            if (this.Username.Length > 0) {
                this.SendProconLoginUsernamePacket(this.Username);
            }
            else {
                this.Game.SendLoginHashedPacket(this.Password);
            }
        }

        private void Connection_PacketRecieved(FrostbiteConnection sender, bool isHandled, Packet packetBeforeDispatch) {

            if (packetBeforeDispatch.OriginatedFromServer == false) {

                Packet request = sender.GetRequestPacket(packetBeforeDispatch);

                if (request != null && String.Compare(request.Words[0], "serverInfo", true) == 0) {

                    this.CurrentServerInfo = new CServerInfo(
                        new List<string>() {
                        "ServerName",
                        "PlayerCount",
                        "MaxPlayerCount",
                        "GameMode",
                        "Map",
                        "CurrentRound",
                        "TotalRounds",
                        "TeamScores",
                        "ConnectionState",
                        "Ranked",
                        "PunkBuster",
                        "Passworded",
                        "ServerUptime",
                        "RoundTime",
                        "GameMod", // Note: if another variable is affixed to both games this method
                        "Mappack",  // will need to be split into MoHClient and BFBC2Client.
                        "ExternalGameIpandPort",
                        "PunkBusterVersion",
                        "JoinQueueEnabled",
                        "ServerRegion"
                        }, packetBeforeDispatch.Words.GetRange(1, packetBeforeDispatch.Words.Count - 1)
                    );
                }
                else if (request != null && String.Compare(request.Words[0], "version", true) == 0) {

                    if (packetBeforeDispatch.Words.Count >= 1 && String.Compare(packetBeforeDispatch.Words[0], "LogInRequired", true) == 0) {

                        // 0.6.0.0 -> 0.5.4.9 connections.  A login would have been required in previous
                        // version for a "version" packet to be sent.

                        this.Game = new BFBC2Client((FrostbiteConnection)sender);
                        this.m_connection = null;

                        if (this.Game != null) {
                            this.InitialSetup();

                            if (this.GameTypeDiscovered != null) {
                                FrostbiteConnection.RaiseEvent(this.GameTypeDiscovered.GetInvocationList(), this);
                            }
                        }

                    }
                    else if (packetBeforeDispatch.Words.Count >= 3) {
                        if (this.Game == null) {

                            if (String.Compare(packetBeforeDispatch.Words[1], "BFBC2", true) == 0) {
                                this.Game = new BFBC2Client((FrostbiteConnection)sender);
                                this.m_connection = null;
                            }
                            else if (String.Compare(packetBeforeDispatch.Words[1], "MOH", true) == 0) {
                                this.Game = new MoHClient((FrostbiteConnection)sender);
                                this.m_connection = null;
                            }

                            if (this.Game != null) {

                                this.VersionNumber = packetBeforeDispatch.Words[2];

                                this.InitialSetup();

                                if (this.GameTypeDiscovered != null) {
                                    FrostbiteConnection.RaiseEvent(this.GameTypeDiscovered.GetInvocationList(), this);
                                }
                            }
                        }
                        else if (this.Game.Connection != null) {
                            this.BeginLoginSequence();
                        }
                    }
                    sender.PacketReceived -= new FrostbiteConnection.PacketDispatchHandler(Connection_PacketRecieved);
                }
            }
        }

        private void Connection_ConnectSuccess(FrostbiteConnection sender) {

            if (this.ConnectSuccess != null) {
                FrostbiteConnection.RaiseEvent(this.ConnectSuccess.GetInvocationList(), this);
            }

            sender.PacketReceived -= new FrostbiteConnection.PacketDispatchHandler(Connection_PacketRecieved);
            sender.PacketReceived += new FrostbiteConnection.PacketDispatchHandler(Connection_PacketRecieved);
        }

        private void m_connection_ConnectionReady(FrostbiteConnection sender) {
            // Sleep the thread so the server has enough time to setup the listener.
            // This is generally only problematic when connecting to local layers.
            Thread.Sleep(50);

            sender.SendQueued(new Packet(false, false, sender.AcquireSequenceNumber, "serverInfo"));
            sender.SendQueued(new Packet(false, false, sender.AcquireSequenceNumber, "version"));
        }

        private void Connection_ConnectAttempt(FrostbiteConnection sender) {
            if (this.ConnectAttempt != null) {
                FrostbiteConnection.RaiseEvent(this.ConnectAttempt.GetInvocationList(), this);
            }
        }

        private void Game_LoginFailure(FrostbiteClient sender, string strError) {
            if (this.LoginFailure != null) {
                this.State = ConnectionState.Error;

                FrostbiteConnection.RaiseEvent(this.LoginFailure.GetInvocationList(), this, strError);
            }
        }

        private void Game_Logout(FrostbiteClient sender) {
            if (this.Logout != null) {
                FrostbiteConnection.RaiseEvent(this.Logout.GetInvocationList(), this);
            }
        }

        private void Game_Login(FrostbiteClient sender) {

            // Now abuse the hell out the queue..
            // TO DO: Make virtual helpers for all of these.
            // ... Load up for each game type?  Some of these sent commands may not even be relevant to MoH.
            /*
            this.SendRequest(new List<string>() { "serverInfo" });
            this.SendRequest(new List<string>() { "admin.listPlayers", "all" });

            //this.SendRequest(new List<string>() { "version" });
            this.SendRequest(new List<string>() { "admin.getPlaylist" });

            this.SendRequest(new List<string>() { "reservedSlots.list" });
            this.SendRequest(new List<string>() { "mapList.list", "rounds" });
            this.SendRequest(new List<string>() { "vars.gamePassword" });
            this.SendRequest(new List<string>() { "vars.gamePassword" });
            this.SendRequest(new List<string>() { "vars.punkBuster" });
            this.SendRequest(new List<string>() { "vars.hardCore" });
            this.SendRequest(new List<string>() { "vars.ranked" });
            this.SendRequest(new List<string>() { "vars.rankLimit" });
            this.SendRequest(new List<string>() { "vars.teamBalance" });
            this.SendRequest(new List<string>() { "vars.friendlyFire" });

            this.SendRequest(new List<string>() { "vars.currentPlayerLimit" });
            this.SendRequest(new List<string>() { "vars.maxPlayerLimit" });

            this.SendRequest(new List<string>() { "vars.bannerUrl" });
            this.SendRequest(new List<string>() { "vars.serverDescription" });
            this.SendRequest(new List<string>() { "vars.killCam" });
            this.SendRequest(new List<string>() { "vars.miniMap" });
            this.SendRequest(new List<string>() { "vars.crossHair" });
            this.SendRequest(new List<string>() { "vars.3dSpotting" });
            this.SendRequest(new List<string>() { "vars.miniMapSpotting" });
            this.SendRequest(new List<string>() { "vars.thirdPersonVehicleCameras" });
            this.SendRequest(new List<string>() { "banList.list" });

            this.SendRequest(new List<string>() { "vars.serverName" });

            this.SendRequest(new List<string>() { "vars.idleTimeout" });
            this.SendRequest(new List<string>() { "vars.profanityFilter" });
            this.SendRequest(new List<string>() { "vars.teamKillCountForKick" });
            this.SendRequest(new List<string>() { "vars.teamKillValueForKick" });
            this.SendRequest(new List<string>() { "vars.teamKillValueIncrease" });
            this.SendRequest(new List<string>() { "vars.teamKillValueDecreasePerSecond" });

            this.SendRequest(new List<string>() { "punkBuster.pb_sv_command", "pb_sv_plist" });
            */

            if (this.m_gameModModified == true) {
                if (this.CurrentServerInfo.GameMod == GameMods.None) {
                    this.ExecuteConnectionConfig(this.Game.GameType + ".def", 0, null);
                }
                else {
                    this.ExecuteConnectionConfig(this.Game.GameType + "." + this.CurrentServerInfo.GameMod + ".def", 0, null);
                }

                lock (this.Parent) {
                    this.CompilePlugins(this.Parent.OptionsSettings.PluginPermissions);
                }
            }

            if (this.IsPRoConConnection == true) {
                this.SendRequest(new List<string>() { "procon.privileges" });
                this.SendRequest(new List<string>() { "procon.registerUid", "true", FrostbiteClient.GeneratePasswordHash(Encoding.ASCII.GetBytes(DateTime.Now.ToString("HH:mm:ss ff")), this.m_strUsername) });
                this.SendRequest(new List<string>() { "procon.version" });
            }

            this.Game.FetchStartupVariables();

            // Occurs when they disconnect then reconnect a connection.
            if (this.PluginsManager == null) {
                this.CompilePlugins(this.Parent.OptionsSettings.PluginPermissions);
            }

            // This saves about 1.7 mb's per connection.  I'd prefer the plugins never compiled though if its connecting to a layer.
            //if (this.IsPRoConConnection == true) { this.PluginsManager.Unload(); GC.Collect();  }

            this.ExecuteConnectionConfig("connection_onlogin.cfg", 0, null);

            if (this.Login != null) {
                FrostbiteConnection.RaiseEvent(this.Login.GetInvocationList(), this);
            }

            // Tasks

            if (this.m_taskTimer == null) {
                this.m_taskTimer = new System.Timers.Timer(1000);
                this.m_taskTimer.Elapsed += new ElapsedEventHandler(m_taskTimer_Elapsed);
                this.m_taskTimer.Start();
            }
            /*
            if (this.m_thTasks == null) {
                this.m_thTasks = new Thread(this.TaskExecuterThread);
                this.m_isTasksRunning = true;
                this.m_thTasks.Start();
            }
            */
        }
        
        public void Shutdown() {
            if (this.Game != null) {
                this.Game.Shutdown();
            }
            else if (this.m_connection != null) {
                this.m_connection.Shutdown();
            }
            else if (this.ConnectionClosed != null) {
                FrostbiteConnection.RaiseEvent(this.ConnectionClosed.GetInvocationList(), this);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////

        /*
        public void Connect() {
            // this.ManuallyDisconnected = false;


            this.AttemptConnection();
        }*/

        public void Connect() {
            if (this.State != ConnectionState.Connecting && this.State != ConnectionState.Connected || (this.State == ConnectionState.Connected && this.IsLoggedIn == false)) {

                if (this.m_taskTimer != null) {
                    this.m_taskTimer.Stop();
                    this.m_taskTimer = null;
                }
                /*
                if (this.m_thTasks != null) {

                    try {
                        this.m_thTasks.Abort();
                    }
                    catch (Exception) { }

                    this.m_thTasks = null;
                }
                */
                if (this.Game == null) {
                    if (this.m_connection == null) {
                        this.m_connection = new FrostbiteConnection(this.HostName, this.Port);
                        this.m_connection.ConnectAttempt += new FrostbiteConnection.EmptyParamterHandler(Connection_ConnectAttempt);
                        this.m_connection.ConnectionReady += new FrostbiteConnection.EmptyParamterHandler(m_connection_ConnectionReady);
                        this.m_connection.ConnectSuccess += new FrostbiteConnection.EmptyParamterHandler(Connection_ConnectSuccess);
                        //this.m_connection.PacketReceived += new FrostbiteConnection.PacketDispatchHandler(Connection_PacketRecieved);
                        this.m_connection.SocketException += new FrostbiteConnection.SocketExceptionHandler(Connection_SocketException);
                        this.m_connection.ConnectionFailure += new FrostbiteConnection.FailureHandler(Connection_ConnectionFailure);
                        this.m_connection.ConnectionClosed += new FrostbiteConnection.EmptyParamterHandler(Connection_ConnectionClosed);
                        this.m_connection.BeforePacketDispatch += new FrostbiteConnection.PrePacketDispatchedHandler(Connection_BeforePacketDispatch);
                    }
                    
                    this.m_connection.AttemptConnection();
                }
                else if (this.Game.Connection != null && this.Game.Connection.IsConnected == false) {
                    this.Game.Connection.AttemptConnection();
                }
            }
        }
        
        #region Layer Events

        private void Layer_LayerOffline() {
            this.SaveConnectionConfig();
        }

        private void Layer_LayerOnline() {
            this.SaveConnectionConfig();
        }

        private void AccountPrivileges_AccountPrivilegeRemoved(AccountPrivilege item) {
            item.AccountPrivilegesChanged -= new AccountPrivilege.AccountPrivilegesChangedHandler(item_AccountPrivilegesChanged);

            this.SaveConnectionConfig();
        }

        private void AccountPrivileges_AccountPrivilegeAdded(AccountPrivilege item) {
            item.AccountPrivilegesChanged += new AccountPrivilege.AccountPrivilegesChangedHandler(item_AccountPrivilegesChanged);

            this.SaveConnectionConfig();
        }

        private void item_AccountPrivilegesChanged(AccountPrivilege item) {
            this.SaveConnectionConfig();
        }

        #endregion

        private void ProConClient_PluginsCompiled(PRoConClient sender) {
            if (this.PluginsManager != null) {
                this.PluginsManager.PluginVariableAltered += new PluginManager.PluginVariableAlteredHandler(Plugins_PluginVariableAltered);
                this.PluginsManager.PluginEnabled += new PluginManager.PluginEmptyParameterHandler(Plugins_PluginEnabled);
                this.PluginsManager.PluginDisabled += new PluginManager.PluginEmptyParameterHandler(Plugins_PluginDisabled);
            }
        }

        void Plugins_PluginDisabled(string strClassName) {
            this.SaveConnectionConfig();
        }

        void Plugins_PluginEnabled(string strClassName) {
            this.SaveConnectionConfig();
        }

        private void Plugins_PluginVariableAltered(PluginDetails spdNewDetails) {
            this.SaveConnectionConfig();
        }

        private void Connection_BeforePacketDispatch(FrostbiteConnection sender, Packet packetBeforeDispatch, out bool isProcessed) {
 
            bool blCancelPacket = false;
            bool blCancelUpdateEvent = false;

            this.InstigatingAccountName = String.Empty;

            // IF it's a response to a packet we sent..
            if (packetBeforeDispatch.OriginatedFromServer == false && packetBeforeDispatch.IsResponse == true) {

                blCancelPacket = this.HandleResponsePacket(packetBeforeDispatch, blCancelUpdateEvent, blCancelPacket);
            }
            // ELSE IF it's an event initiated by the server (OnJoin, OnLeave, OnChat etc)
            else if (packetBeforeDispatch.OriginatedFromServer == true && packetBeforeDispatch.IsResponse == false) {

                blCancelPacket = this.HandleEventPacket(packetBeforeDispatch, blCancelPacket);
            }

            isProcessed = blCancelPacket;
        }

        private bool HandleEventPacket(Packet cpBeforePacketDispatch, bool blCancelPacket) {

            if (cpBeforePacketDispatch.Words.Count >= 1 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.shutdown", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                this.State = ConnectionState.Error;
                this.Connection_ConnectionFailure(this.Game.Connection, new Exception("The PRoCon layer has been shutdown by the host"));

                this.Shutdown();
                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 2 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.account.onLogin", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                // Also able to get their privs as well if needed?
                //this.m_uscParent.OnRemoteAccountLoggedIn(cpBeforePacketDispatch.Words[1], true);

                if (this.RemoteAccountLoggedIn != null) {
                    FrostbiteConnection.RaiseEvent(this.RemoteAccountLoggedIn.GetInvocationList(), this, cpBeforePacketDispatch.Words[1], true);
                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 2 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.account.onLogout", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                if (this.RemoteAccountLoggedIn != null) {
                    FrostbiteConnection.RaiseEvent(this.RemoteAccountLoggedIn.GetInvocationList(), this, cpBeforePacketDispatch.Words[1], false);
                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 3 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.account.onUidRegistered", true) == 0) {

                if (this.m_dicUsernamesToUids.ContainsKey(cpBeforePacketDispatch.Words[2]) == true) {
                    this.m_dicUsernamesToUids[cpBeforePacketDispatch.Words[2]] = cpBeforePacketDispatch.Words[1];
                }
                else {
                    this.m_dicUsernamesToUids.Add(cpBeforePacketDispatch.Words[2], cpBeforePacketDispatch.Words[1]);
                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 2 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.account.onCreated", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                if (this.RemoteAccountCreated != null) {
                    FrostbiteConnection.RaiseEvent(this.RemoteAccountCreated.GetInvocationList(), this, cpBeforePacketDispatch.Words[1]);
                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 2 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.account.onDeleted", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                if (this.RemoteAccountDeleted != null) {
                    FrostbiteConnection.RaiseEvent(this.RemoteAccountDeleted.GetInvocationList(), this, cpBeforePacketDispatch.Words[1]);
                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 3 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.account.onAltered", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                UInt32 ui32Privileges = 0;
                if (UInt32.TryParse(cpBeforePacketDispatch.Words[2], out ui32Privileges) == true) {
                    CPrivileges spPrivs = new CPrivileges();
                    spPrivs.PrivilegesFlags = ui32Privileges;

                    if (this.ProconPrivileges != null && String.Compare(cpBeforePacketDispatch.Words[1], this.Username) == 0) {

                        this.Privileges = spPrivs;

                        FrostbiteConnection.RaiseEvent(this.ProconPrivileges.GetInvocationList(), this, spPrivs);
                    }

                    if (this.RemoteAccountAltered != null) {
                        FrostbiteConnection.RaiseEvent(this.RemoteAccountAltered.GetInvocationList(), this, cpBeforePacketDispatch.Words[1], spPrivs);
                    }

                }

                blCancelPacket = true;
            }

            else if (cpBeforePacketDispatch.Words.Count >= 2 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.packages.onDownloading", true) == 0) {

                if (this.PackageDownloading != null) {
                    FrostbiteConnection.RaiseEvent(this.PackageDownloading.GetInvocationList(), this, cpBeforePacketDispatch.Words[1]);
                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 2 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.packages.onDownloaded", true) == 0) {

                if (this.PackageDownloaded != null) {
                    FrostbiteConnection.RaiseEvent(this.PackageDownloaded.GetInvocationList(), this, cpBeforePacketDispatch.Words[1]);
                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 2 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.packages.onInstalling", true) == 0) {

                if (this.PackageInstalling != null) {
                    FrostbiteConnection.RaiseEvent(this.PackageInstalling.GetInvocationList(), this, cpBeforePacketDispatch.Words[1]);
                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 3 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.packages.onDownloadError", true) == 0) {

                if (this.PackageDownloadError != null) {
                    FrostbiteConnection.RaiseEvent(this.PackageDownloadError.GetInvocationList(), this, cpBeforePacketDispatch.Words[1], cpBeforePacketDispatch.Words[2]);
                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 3 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.packages.onInstalled", true) == 0) {

                bool restartRequired = false;

                if (bool.TryParse(cpBeforePacketDispatch.Words[2], out restartRequired) == true) {
                    if (this.PackageInstalled != null) {
                        FrostbiteConnection.RaiseEvent(this.PackageInstalled.GetInvocationList(), this, cpBeforePacketDispatch.Words[1], restartRequired);
                    }
                }

                blCancelPacket = true;
            }


            else if (cpBeforePacketDispatch.Words.Count >= 3 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.chat.onConsole", true) == 0) {

                long logTime = 0L;

                if (long.TryParse(cpBeforePacketDispatch.Words[1], out logTime) == true) {
                    if (this.ReadRemoteChatConsole != null) {
                        FrostbiteConnection.RaiseEvent(this.ReadRemoteChatConsole.GetInvocationList(), this, DateTime.FromBinary(logTime), cpBeforePacketDispatch.Words[2]);
                    }
                }
            }
            else if (cpBeforePacketDispatch.Words.Count >= 3 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.plugin.onConsole", true) == 0) {

                long logTime = 0L;

                if (long.TryParse(cpBeforePacketDispatch.Words[1], out logTime) == true) {
                    if (this.ReadRemotePluginConsole != null) {
                        FrostbiteConnection.RaiseEvent(this.ReadRemotePluginConsole.GetInvocationList(), this, DateTime.FromBinary(logTime), cpBeforePacketDispatch.Words[2]);
                    }
                }
            }
            else if (cpBeforePacketDispatch.Words.Count >= 3 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.plugin.onVariablesAltered", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                int i = 1;
                List<CPluginVariable> lstVariables = new List<CPluginVariable>();
                string strClassName = cpBeforePacketDispatch.Words[i++];

                int iTotalVariables = 0;
                if (int.TryParse(cpBeforePacketDispatch.Words[i++], out iTotalVariables) == true && i + (iTotalVariables * 3) <= cpBeforePacketDispatch.Words.Count) {
                    for (int x = 0; x < (iTotalVariables * 3); x += 3) {
                        lstVariables.Add(new CPluginVariable(cpBeforePacketDispatch.Words[i++], cpBeforePacketDispatch.Words[i++], cpBeforePacketDispatch.Words[i++]));
                    }
                }

                if (this.RemotePluginVariables != null) {
                    FrostbiteConnection.RaiseEvent(this.RemotePluginVariables.GetInvocationList(), this, strClassName, lstVariables);
                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 1 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.plugin.onLoaded", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                int i = 1;
                if (i + 6 <= cpBeforePacketDispatch.Words.Count) {
                    PluginDetails spdLoaded = new PluginDetails();

                    spdLoaded.ClassName = cpBeforePacketDispatch.Words[i++];
                    spdLoaded.Name = cpBeforePacketDispatch.Words[i++];
                    spdLoaded.Author = cpBeforePacketDispatch.Words[i++];
                    spdLoaded.Website = cpBeforePacketDispatch.Words[i++];
                    spdLoaded.Version = cpBeforePacketDispatch.Words[i++];
                    spdLoaded.Description = cpBeforePacketDispatch.Words[i++];

                    spdLoaded.DisplayPluginVariables = new List<CPluginVariable>();
                    spdLoaded.PluginVariables = new List<CPluginVariable>(); // Not used here.
                    int iTotalVariables = 0;
                    if (int.TryParse(cpBeforePacketDispatch.Words[i++], out iTotalVariables) == true && i + (iTotalVariables * 3) <= cpBeforePacketDispatch.Words.Count) {
                        for (int x = 0; x < (iTotalVariables * 3); x += 3) {
                            spdLoaded.DisplayPluginVariables.Add(new CPluginVariable(cpBeforePacketDispatch.Words[i++], cpBeforePacketDispatch.Words[i++], cpBeforePacketDispatch.Words[i++]));
                        }
                    }

                    if (this.RemotePluginLoaded != null) {
                        FrostbiteConnection.RaiseEvent(this.RemotePluginLoaded.GetInvocationList(), this, spdLoaded);
                    }

                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 3 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.plugin.onEnabled", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                bool blEnabled = false;

                if (bool.TryParse(cpBeforePacketDispatch.Words[2], out blEnabled) == true && this.RemotePluginEnabled != null) {

                    FrostbiteConnection.RaiseEvent(this.RemotePluginEnabled.GetInvocationList(), this, cpBeforePacketDispatch.Words[1], blEnabled);
                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 4 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.admin.onSay", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                if (this.ProconAdminSaying != null) {
                    FrostbiteConnection.RaiseEvent(this.ProconAdminSaying.GetInvocationList(), this, cpBeforePacketDispatch.Words[1], cpBeforePacketDispatch.Words[2], new CPlayerSubset(cpBeforePacketDispatch.Words.GetRange(3, cpBeforePacketDispatch.Words.Count - 3)));
                }

                if (this.PassLayerEvent != null) {
                    FrostbiteConnection.RaiseEvent(this.PassLayerEvent.GetInvocationList(), this, cpBeforePacketDispatch);
                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 5 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.admin.onYell", true) == 0) {
               // this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                int iDisplayDuration = 0;

                if (int.TryParse(cpBeforePacketDispatch.Words[3], out iDisplayDuration) == true) {

                    if (this.ProconAdminYelling != null) {
                        FrostbiteConnection.RaiseEvent(this.ProconAdminYelling.GetInvocationList(), this, cpBeforePacketDispatch.Words[1], cpBeforePacketDispatch.Words[2], iDisplayDuration, new CPlayerSubset(cpBeforePacketDispatch.Words.GetRange(4, cpBeforePacketDispatch.Words.Count - 4)));
                    }

                    if (this.PassLayerEvent != null) {
                        FrostbiteConnection.RaiseEvent(this.PassLayerEvent.GetInvocationList(), this, cpBeforePacketDispatch);
                    }
                }

                blCancelPacket = true;
            }
            else if (cpBeforePacketDispatch.Words.Count >= 3 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.updated", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                foreach (KeyValuePair<string, string> registerUid in this.m_dicUsernamesToUids) {
                    if (String.Compare(cpBeforePacketDispatch.Words[1], registerUid.Value) == 0) {
                        this.InstigatingAccountName = registerUid.Key;
                        break;
                    }
                }

                // Only parse it if the UID is different from this connections registered UID.
                if (String.Compare(cpBeforePacketDispatch.Words[1], this.m_strProconEventsUid) != 0) {
                    List<string> lstAssumedRequestPacket = new List<string>(cpBeforePacketDispatch.Words);
                    lstAssumedRequestPacket.RemoveRange(0, 2);

                    Packet cpAssumedRequestPacket = new Packet(false, false, 0, lstAssumedRequestPacket);
                    Packet cpAssumedResponsePacket = new Packet(false, true, 0, new List<string>() { "OK" });

                    this.Game.DispatchResponsePacket(this.Game.Connection, cpAssumedResponsePacket, cpAssumedRequestPacket);

                    cpAssumedRequestPacket = null;
                    cpAssumedResponsePacket = null;

                    if (this.PassLayerEvent != null) {
                        FrostbiteConnection.RaiseEvent(this.PassLayerEvent.GetInvocationList(), this, cpBeforePacketDispatch);
                    }
                }

                blCancelPacket = true;
            }

            else if (cpBeforePacketDispatch.Words.Count >= 2 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.battlemap.onZoneRemoved", true) == 0) {
                if (this.MapZoneDeleted != null) {
                    FrostbiteConnection.RaiseEvent(this.MapZoneDeleted.GetInvocationList(), this, new MapZoneDrawing(cpBeforePacketDispatch.Words[1], String.Empty, String.Empty, null, true));
                }
            }

            else if (cpBeforePacketDispatch.Words.Count >= 4 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.battlemap.onZoneCreated", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                int iPoints = 0;

                if (int.TryParse(cpBeforePacketDispatch.Words[3], out iPoints) == true) {

                    Point3D[] points = new Point3D[iPoints];

                    for (int i = 0; i < iPoints; i++) {
                        points[i] = new Point3D(cpBeforePacketDispatch.Words[3 + i * 3 + 1], cpBeforePacketDispatch.Words[3 + i * 3 + 2], cpBeforePacketDispatch.Words[3 + i * 3 + 3]);
                    }

                    if (this.MapZoneCreated != null) {
                        FrostbiteConnection.RaiseEvent(this.MapZoneCreated.GetInvocationList(), this, new MapZoneDrawing(cpBeforePacketDispatch.Words[1], cpBeforePacketDispatch.Words[2], String.Empty, points, true));
                    }
                }

                blCancelPacket = true;
            }

            else if (cpBeforePacketDispatch.Words.Count >= 3 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.battlemap.onZoneModified", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                int iPoints = 0;

                if (int.TryParse(cpBeforePacketDispatch.Words[3], out iPoints) == true) {

                    Point3D[] points = new Point3D[iPoints];

                    for (int i = 0; i < iPoints; i++) {
                        points[i] = new Point3D(cpBeforePacketDispatch.Words[3 + i * 3 + 1], cpBeforePacketDispatch.Words[3 + i * 3 + 2], cpBeforePacketDispatch.Words[3 + i * 3 + 3]);
                    }

                    if (this.MapZoneModified != null) {
                        FrostbiteConnection.RaiseEvent(this.MapZoneModified.GetInvocationList(), this, new MapZoneDrawing(cpBeforePacketDispatch.Words[1], String.Empty, cpBeforePacketDispatch.Words[2], points, true));
                    }
                }

                blCancelPacket = true;
            }


            else if (cpBeforePacketDispatch.Words.Count >= 3 && String.Compare(cpBeforePacketDispatch.Words[0], "procon.vars.onAltered", true) == 0) {
                //this.SendPacket(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "OK" }));

                this.SV_Variables.SetVariable(cpBeforePacketDispatch.Words[1], cpBeforePacketDispatch.Words[2]);

                if (this.ReceiveProconVariable != null) {
                    FrostbiteConnection.RaiseEvent(this.ReceiveProconVariable.GetInvocationList(), this, cpBeforePacketDispatch.Words[1], cpBeforePacketDispatch.Words[2]);
                }

                blCancelPacket = true;
            }

            else { //  if (blCommandConnection == false) Pass everything else onto any connected clients..
                if (this.PassLayerEvent != null) {
                    FrostbiteConnection.RaiseEvent(this.PassLayerEvent.GetInvocationList(), this, cpBeforePacketDispatch);
                }
            }

            return blCancelPacket;
        }

        private bool HandleResponsePacket(Packet cpBeforePacketDispatch, bool blCancelUpdateEvent, bool blCancelPacket) {

            if (this.Game != null) {

                Packet cpRequestPacket = this.Game.Connection.GetRequestPacket(cpBeforePacketDispatch);

                if (cpRequestPacket != null) {

                    if (cpBeforePacketDispatch.Words.Count >= 1 && String.Compare(cpBeforePacketDispatch.Words[0], "InvalidUsername", true) == 0) {

                        if (this.LoginFailure != null) {
                            FrostbiteConnection.RaiseEvent(this.LoginFailure.GetInvocationList(), this, cpBeforePacketDispatch.Words[0]);
                        }

                        this.Shutdown();
                        this.State = ConnectionState.Error;

                        blCancelPacket = true;
                    }
                    else if (cpRequestPacket.Words.Count >= 1 && String.Compare(cpRequestPacket.Words[0], "procon.version", true) == 0 && cpBeforePacketDispatch.Words.Count >= 2) {

                        try {
                            this.ConnectedLayerVersion = new Version(cpBeforePacketDispatch.Words[1]);

                            if (this.ProconVersion != null) {
                                FrostbiteConnection.RaiseEvent(this.ProconVersion.GetInvocationList(), this, this.ConnectedLayerVersion);
                            }
                        }
                        catch (Exception) { }

                        blCancelPacket = true;
                    }
                    else if (cpRequestPacket.Words.Count >= 1 && String.Compare(cpRequestPacket.Words[0], "procon.privileges", true) == 0 && cpBeforePacketDispatch.Words.Count >= 2) {

                        UInt32 ui32Privileges = 0;
                        if (UInt32.TryParse(cpBeforePacketDispatch.Words[1], out ui32Privileges) == true) {
                            CPrivileges spPrivs = new CPrivileges();
                            spPrivs.PrivilegesFlags = ui32Privileges;

                            this.Privileges = spPrivs;

                            if (this.ProconPrivileges != null) {
                                FrostbiteConnection.RaiseEvent(this.ProconPrivileges.GetInvocationList(), this, spPrivs);
                            }
                        }

                        blCancelPacket = true;
                    }
                    else if (cpRequestPacket.Words.Count >= 2 && String.Compare(cpRequestPacket.Words[0], "procon.registerUid", true) == 0 && cpBeforePacketDispatch.Words.Count >= 1) {

                        if (String.Compare(cpBeforePacketDispatch.Words[0], "OK", true) == 0 && cpRequestPacket.Words.Count >= 3) {
                            this.m_strProconEventsUid = cpRequestPacket.Words[2];
                        }
                        else if (String.Compare(cpBeforePacketDispatch.Words[0], "ProconUidConflict", true) == 0) {
                            // Conflict in our UID, just hash and send another one.
                            // Then go to vegas.
                            this.SendRequest(new List<string>() { "procon.registerUid", "true", FrostbiteClient.GeneratePasswordHash(Encoding.ASCII.GetBytes(DateTime.Now.ToString("HH:mm:ss ff")), this.m_strUsername) });
                        }

                        blCancelPacket = true;
                    }
                    else if (cpRequestPacket.Words.Count >= 1 && String.Compare(cpRequestPacket.Words[0], "procon.account.listAccounts", true) == 0) {

                        UInt32 ui32Privileges = 0;

                        for (int i = 1; i < cpBeforePacketDispatch.Words.Count; i += 2) {
                            if (UInt32.TryParse(cpBeforePacketDispatch.Words[i + 1], out ui32Privileges) == true) {
                                CPrivileges spPrivs = new CPrivileges();
                                spPrivs.PrivilegesFlags = ui32Privileges;

                                if (this.RemoteAccountCreated != null && this.RemoteAccountAltered != null) {
                                    FrostbiteConnection.RaiseEvent(this.RemoteAccountCreated.GetInvocationList(), this, cpBeforePacketDispatch.Words[i]);
                                    FrostbiteConnection.RaiseEvent(this.RemoteAccountAltered.GetInvocationList(), this, cpBeforePacketDispatch.Words[i], spPrivs);
                                }
                            }
                        }

                        blCancelPacket = true;
                    }
                    else if (cpRequestPacket.Words.Count >= 1 && String.Compare(cpRequestPacket.Words[0], "procon.battlemap.listZones", true) == 0 && cpBeforePacketDispatch.Words.Count >= 2 && String.Compare(cpBeforePacketDispatch.Words[0], "OK", true) == 0) {

                        List<MapZoneDrawing> zones = new List<MapZoneDrawing>();

                        int iZones = 0;
                        int iOffset = 1;

                        if (int.TryParse(cpBeforePacketDispatch.Words[iOffset++], out iZones) == true) {

                            for (int iZoneCount = 0; iZoneCount < iZones; iZoneCount++) {

                                string uid = String.Empty;
                                string level = String.Empty;
                                string tags = String.Empty;
                                List<Point3D> points = new List<Point3D>();

                                if (iOffset + 4 < cpBeforePacketDispatch.Words.Count) {

                                    uid = cpBeforePacketDispatch.Words[iOffset++];
                                    level = cpBeforePacketDispatch.Words[iOffset++];
                                    tags = cpBeforePacketDispatch.Words[iOffset++];

                                    int iZonePoints = 0;
                                    if (int.TryParse(cpBeforePacketDispatch.Words[iOffset++], out iZonePoints) == true && iOffset + iZonePoints * 3 <= cpBeforePacketDispatch.Words.Count) {

                                        for (int iZonePointCount = 0; iZonePointCount < iZonePoints && iOffset + 3 <= cpBeforePacketDispatch.Words.Count; iZonePointCount++) {
                                            points.Add(new Point3D(cpBeforePacketDispatch.Words[iOffset++], cpBeforePacketDispatch.Words[iOffset++], cpBeforePacketDispatch.Words[iOffset++]));
                                        }

                                    }
                                }

                                zones.Add(new MapZoneDrawing(uid, level, tags, points.ToArray(), true));
                            }
                        }

                        if (this.ListMapZones != null) {
                            FrostbiteConnection.RaiseEvent(this.ListMapZones.GetInvocationList(), this, zones);
                        }

                        blCancelPacket = true;
                    }


                    else if (cpRequestPacket.Words.Count >= 1 && String.Compare(cpRequestPacket.Words[0], "procon.account.setPassword", true) == 0 && cpBeforePacketDispatch.Words.Count >= 1 && String.Compare(cpBeforePacketDispatch.Words[0], "OK", true) == 0) {

                        if (this.RemoteAccountChangePassword != null) {
                            FrostbiteConnection.RaiseEvent(this.RemoteAccountChangePassword.GetInvocationList(), this);
                        }

                        blCancelPacket = true;
                    }
                    else if (cpRequestPacket.Words.Count >= 1 && String.Compare(cpRequestPacket.Words[0], "procon.account.listLoggedIn", true) == 0) {

                        bool containsUids = (cpRequestPacket.Words.Count >= 2 && String.Compare(cpRequestPacket.Words[1], "uids") == 0);

                        if (this.RemoteAccountLoggedIn != null) {
                            for (int i = 1; i < cpBeforePacketDispatch.Words.Count; i++) {
                                FrostbiteConnection.RaiseEvent(this.RemoteAccountLoggedIn.GetInvocationList(), this, cpBeforePacketDispatch.Words[i], true);

                                if (containsUids == true && i + 1 < cpBeforePacketDispatch.Words.Count) {

                                    if (this.m_dicUsernamesToUids.ContainsKey(cpBeforePacketDispatch.Words[i]) == true) {
                                        this.m_dicUsernamesToUids[cpBeforePacketDispatch.Words[i]] = cpBeforePacketDispatch.Words[i + 1];
                                    }
                                    else {
                                        this.m_dicUsernamesToUids.Add(cpBeforePacketDispatch.Words[i], cpBeforePacketDispatch.Words[i + 1]);
                                    }

                                    i++;
                                }
                            }
                        }

                        blCancelPacket = true;
                    }

                    else if (cpRequestPacket.Words.Count >= 1 && String.Compare(cpRequestPacket.Words[0], "procon.plugin.listEnabled", true) == 0) {

                        if (this.RemoteEnabledPlugins != null) {
                            List<string> lstEnabledPlugins = new List<string>(cpBeforePacketDispatch.Words);
                            lstEnabledPlugins.RemoveAt(0);

                            FrostbiteConnection.RaiseEvent(this.RemoteEnabledPlugins.GetInvocationList(), this, lstEnabledPlugins);
                        }

                        blCancelPacket = true;
                    }
                    else if (cpRequestPacket.Words.Count >= 1 && String.Compare(cpRequestPacket.Words[0], "procon.vars", true) == 0 && cpBeforePacketDispatch.Words.Count >= 3) {
                        this.SV_Variables.SetVariable(cpBeforePacketDispatch.Words[1], cpBeforePacketDispatch.Words[2]);

                        if (this.ReceiveProconVariable != null) {
                            FrostbiteConnection.RaiseEvent(this.ReceiveProconVariable.GetInvocationList(), this, cpBeforePacketDispatch.Words[1], cpBeforePacketDispatch.Words[2]);
                        }
                        // Dispatch to plugins.
                    }
                    else if (cpRequestPacket.Words.Count >= 1 && String.Compare(cpRequestPacket.Words[0], "procon.plugin.listLoaded", true) == 0) {

                        if (this.RemoteLoadedPlugins != null) {

                            int i = 0;
                            if (cpBeforePacketDispatch.Words.Count >= 1 && String.Compare(cpBeforePacketDispatch.Words[i++], "OK", true) == 0) {

                                Dictionary<string, PluginDetails> dicLoadedPlugins = new Dictionary<string, PluginDetails>();

                                while (i + 6 <= cpBeforePacketDispatch.Words.Count) {
                                    PluginDetails spdLoaded = new PluginDetails();

                                    spdLoaded.ClassName = cpBeforePacketDispatch.Words[i++];
                                    spdLoaded.Name = cpBeforePacketDispatch.Words[i++];
                                    spdLoaded.Author = cpBeforePacketDispatch.Words[i++];
                                    spdLoaded.Website = cpBeforePacketDispatch.Words[i++];
                                    spdLoaded.Version = cpBeforePacketDispatch.Words[i++];
                                    spdLoaded.Description = cpBeforePacketDispatch.Words[i++];

                                    spdLoaded.DisplayPluginVariables = new List<CPluginVariable>();
                                    spdLoaded.PluginVariables = new List<CPluginVariable>(); // Not used here.
                                    int iTotalVariables = 0;
                                    if (int.TryParse(cpBeforePacketDispatch.Words[i++], out iTotalVariables) == true && i + (iTotalVariables * 3) <= cpBeforePacketDispatch.Words.Count) {
                                        for (int x = 0; x < (iTotalVariables * 3); x += 3) {
                                            spdLoaded.DisplayPluginVariables.Add(new CPluginVariable(cpBeforePacketDispatch.Words[i++], cpBeforePacketDispatch.Words[i++], cpBeforePacketDispatch.Words[i++]));
                                        }
                                    }

                                    if (dicLoadedPlugins.ContainsKey(spdLoaded.ClassName) == false) {
                                        dicLoadedPlugins.Add(spdLoaded.ClassName, spdLoaded);
                                    }
                                }

                                FrostbiteConnection.RaiseEvent(this.RemoteLoadedPlugins.GetInvocationList(), this, dicLoadedPlugins);
                            }

                        }

                        blCancelPacket = true;
                    }
                    else if (cpRequestPacket.Words.Count >= 2 && String.Compare(cpRequestPacket.Words[0], "login.hashed", true) == 0 && cpBeforePacketDispatch.Words.Count >= 1 && String.Compare(cpBeforePacketDispatch.Words[0], "InsufficientPrivileges", true) == 0) {

                        if (this.LoginFailure != null) {
                            FrostbiteConnection.RaiseEvent(this.LoginFailure.GetInvocationList(), this, cpBeforePacketDispatch.Words[0]);
                        }

                        this.Shutdown();
                        this.State = ConnectionState.Error;

                        blCancelPacket = true;
                    }
                    else if (cpRequestPacket.Words.Count >= 2 && String.Compare(cpRequestPacket.Words[0], "procon.login.username", true) == 0 && cpBeforePacketDispatch.Words.Count >= 1 && (String.Compare(cpBeforePacketDispatch.Words[0], "OK", true) == 0 || String.Compare(cpBeforePacketDispatch.Words[0], "UnknownCommand", true) == 0)) {
                        //this.send(new Packet(true, true, cpBeforePacketDispatch.SequenceNumber, new List<string>() { "procon.login.requestUsername", this.m_strUsername }));

                        // This is the first command we would recieve so now we know we're connected through a PRoCon layer.
                        if (this.LoginAttempt != null) {
                            FrostbiteConnection.RaiseEvent(this.LoginAttempt.GetInvocationList(), this);
                        }

                        this.Game.SendLoginHashedPacket(this.Password);

                        if (String.Compare(cpBeforePacketDispatch.Words[0], "OK", true) == 0) {
                            this.IsPRoConConnection = true;
                        }
                        else {
                            this.IsPRoConConnection = false;
                        }

                        blCancelPacket = true;
                    }
                    else if (cpRequestPacket.Words.Count >= 2 && String.Compare(cpRequestPacket.Words[0], "procon.login.username", true) == 0 && cpBeforePacketDispatch.Words.Count >= 1 && String.Compare(cpBeforePacketDispatch.Words[0], "InsufficientPrivileges", true) == 0) {
                        // The servers just told us off, try and login normally.
                        if (this.LoginFailure != null) {
                            FrostbiteConnection.RaiseEvent(this.LoginFailure.GetInvocationList(), this, cpBeforePacketDispatch.Words[0]);
                        }

                        this.Shutdown();
                        this.State = ConnectionState.Error;

                        blCancelPacket = true;
                    }
                    else if (cpRequestPacket.Words.Count >= 3 && String.Compare(cpRequestPacket.Words[0], "admin.say", true) == 0 && this.m_dicForwardedPackets.ContainsKey(cpBeforePacketDispatch.SequenceNumber) == true) {
                        if (this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_lstWords.Count >= 4 && String.Compare(this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_lstWords[0], "procon.admin.say", true) == 0) {
                            this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_lstWords[0] = "procon.admin.onSay";

                            if (this.IsPRoConConnection == false) {
                                List<string> lstWords = this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_lstWords;

                                if (this.ProconAdminSaying != null) {
                                    FrostbiteConnection.RaiseEvent(this.ProconAdminSaying.GetInvocationList(), this, lstWords[1], lstWords[2], new CPlayerSubset(lstWords.GetRange(3, lstWords.Count - 3)));
                                }
                            }

                            if (this.PassLayerEvent != null) {
                                FrostbiteConnection.RaiseEvent(this.PassLayerEvent.GetInvocationList(), this, new Packet(true, false, cpRequestPacket.SequenceNumber, this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_lstWords));
                            }

                            // Send to all logged in layer clients
                            this.m_dicForwardedPackets.Remove(cpBeforePacketDispatch.SequenceNumber);
                            blCancelPacket = true;
                            blCancelUpdateEvent = true;
                        }
                    }
                    else if (cpRequestPacket.Words.Count >= 4 && String.Compare(cpRequestPacket.Words[0], "admin.yell", true) == 0 && this.m_dicForwardedPackets.ContainsKey(cpBeforePacketDispatch.SequenceNumber) == true) {
                        if (this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_lstWords.Count >= 5 && String.Compare(this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_lstWords[0], "procon.admin.yell", true) == 0) {
                            this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_lstWords[0] = "procon.admin.onYell";

                            // If we're at the top of the tree, simulate the event coming from a layer above.
                            if (this.IsPRoConConnection == false) {
                                List<string> lstWords = this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_lstWords;

                                int iDisplayDuration = 0;

                                if (int.TryParse(lstWords[3], out iDisplayDuration) == true) {

                                    if (this.ProconAdminYelling != null) {
                                        FrostbiteConnection.RaiseEvent(this.ProconAdminYelling.GetInvocationList(), this, lstWords[1], lstWords[2], iDisplayDuration, new CPlayerSubset(lstWords.GetRange(4, lstWords.Count - 4)));
                                    }
                                }
                            }

                            // Send to all logged in layer clients
                            if (this.PassLayerEvent != null) {
                                FrostbiteConnection.RaiseEvent(this.PassLayerEvent.GetInvocationList(), this, new Packet(true, false, cpRequestPacket.SequenceNumber, this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_lstWords));
                            }

                            this.m_dicForwardedPackets.Remove(cpBeforePacketDispatch.SequenceNumber);
                            blCancelPacket = true;
                            blCancelUpdateEvent = true;
                        }
                    }

                    if (blCancelUpdateEvent == false) {
                        string strProconEventsUid = String.Empty;

                        // If a layer client sent this packet..
                        if (this.m_dicForwardedPackets.ContainsKey(cpBeforePacketDispatch.SequenceNumber) == true) {
                            if (this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_sender != null) {
                                ((PRoConLayerClient)this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_sender).OnServerForwardedResponse(new Packet(false, true, this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_ui32OriginalSequence, new List<string>(cpBeforePacketDispatch.Words)));

                                strProconEventsUid = ((PRoConLayerClient)this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_sender).ProconEventsUid;

                                this.InstigatingAccountName = this.m_dicForwardedPackets[cpBeforePacketDispatch.SequenceNumber].m_sender.Username;
                            }

                            // Unregister the sequence and packet.
                            this.m_dicForwardedPackets.Remove(cpBeforePacketDispatch.SequenceNumber);
                        }

                        // IF the command was not a request for a list (it's a GET operation only,
                        // as in it only lists or retrieves information and will never be set.)

                        if (this.Game != null && this.Game.GetPacketsPattern.IsMatch(cpRequestPacket.ToString()) == false) { // && cpBeforePacketDispatch.Words.Count >= 1 && String.Compare(cpBeforePacketDispatch.Words[0], "OK") == 0) {
                            List<string> lstProconUpdatedWords = new List<string>(cpRequestPacket.Words);
                            lstProconUpdatedWords.Insert(0, "procon.updated");
                            lstProconUpdatedWords.Insert(1, strProconEventsUid);
                            // Now we pass on the packet to all the clients as an event so they can remain in sync.

                            if (this.PassLayerEvent != null) {
                                FrostbiteConnection.RaiseEvent(this.PassLayerEvent.GetInvocationList(), this, new Packet(true, false, cpRequestPacket.SequenceNumber, lstProconUpdatedWords));
                            }

                        }
                    }
                }
            }

            return blCancelPacket;
        }

        #region General methods

        public int GetLocalizedTeamNameCount(string strMapFilename) {

            int iTeamCount = 0;

            foreach (CTeamName ctnTeamName in this.TeamNameList) {
                if (String.Compare(ctnTeamName.MapFilename, strMapFilename, true) == 0) {
                    iTeamCount++;
                }
            }

            return iTeamCount;
        }

        public string GetLocalizedTeamName(int iTeamID, string strMapFilename) {

            string strReturnName = String.Empty;

            foreach (CTeamName ctnTeamName in this.TeamNameList) {
                if (String.Compare(ctnTeamName.MapFilename, strMapFilename, true) == 0 && ctnTeamName.TeamID == iTeamID) {
                    strReturnName = this.Language.GetLocalized(ctnTeamName.LocalizationKey, null);
                    break;
                }
            }

            return strReturnName;
        }

        public string GetFriendlyGamemodeByMap(string strMapFileName) {

            string strFriendlyName = String.Empty;

            if (this.MapListPool != null) {
                foreach (CMap cmMap in this.MapListPool) {
                    if (string.Compare(cmMap.FileName, strMapFileName, true) == 0) {
                        strFriendlyName = cmMap.GameMode;
                        break;
                    }
                }
            }

            return strFriendlyName;
        }

        public string GetFriendlyGamemode(string strPlaylistName) {

            string strFriendlyName = String.Empty;

            if (this.MapListPool != null) {
                foreach (CMap cmMap in this.MapListPool) {
                    if (string.Compare(cmMap.PlayList, strPlaylistName, true) == 0) {
                        strFriendlyName = cmMap.GameMode;
                        break;
                    }
                }
            }

            return strFriendlyName;
        }

        public string GetFriendlyMapname(string strMapFileName) {

            string strFriendlyName = String.Empty;

            if (this.MapListPool != null) {
                foreach (CMap cmMap in this.MapListPool) {
                    if (string.Compare(cmMap.FileName, strMapFileName, true) == 0) {
                        strFriendlyName = cmMap.PublicLevelName;
                        break;
                    }
                }
            }

            return strFriendlyName;
        }

        public string GetPlaylistByMapname(string strMapFileName) {

            string strFriendlyName = String.Empty;

            foreach (CMap cmMap in this.MapListPool) {
                if (string.Compare(cmMap.FileName, strMapFileName, true) == 0) {
                    strFriendlyName = cmMap.PlayList;
                    break;
                }
            }

            return strFriendlyName;
        }

        public int GetDefaultSquadIDByMapname(string strMapFileName) {
            int iDefaultSquadID = 0;

            foreach (CMap cmMap in this.MapListPool) {
                if (string.Compare(cmMap.FileName, strMapFileName, true) == 0) {
                    iDefaultSquadID = cmMap.DefaultSquadID;
                    break;
                }
            }

            return iDefaultSquadID;
        }

        /// <summary>
        /// Return an arbitrary list of CMap objects in this.MapListPool
        /// with unique .Gamemode
        /// </summary>
        /// <returns></returns>
        public List<CMap> GetGamemodeList() {

            List<CMap> returnList = new List<CMap>();

            foreach (CMap map in this.MapListPool) {

                bool isGamemodeAdded = false;

                foreach (CMap gamemodeMap in returnList) {
                    if (String.Compare(map.GameMode, gamemodeMap.GameMode, true) == 0) {
                        isGamemodeAdded = true;
                    }
                }

                if (isGamemodeAdded == false) {
                    returnList.Add(map);
                }

            }

            return returnList;
        }

        #endregion

        #region Playing Sounds

        private SoundPlayer m_spPlayer = new SoundPlayer();

        public struct SPlaySound {
            public string m_strSoundFile;
            public int m_iRepeat;
        }

        private Thread m_thSound;
        private Thread m_thStopSound;
        private bool m_blPlaySound;

        public void PlaySound(string strSoundFile, int iRepeat) {

            SPlaySound spsSound = new SPlaySound();
            spsSound.m_iRepeat = iRepeat;
            spsSound.m_strSoundFile = strSoundFile;
            //spsSound.m_spPlayer = this.m_spPlayer;

            if (this.m_thSound != null) {
                this.StopSound(spsSound);
            }
            else {
                this.m_thSound = new Thread(new ParameterizedThreadStart(PlaySound));
                this.m_blPlaySound = true;
                this.m_thSound.Start(spsSound);
            }
        }

        public void StopSound(SPlaySound spsSound) {

            this.m_blPlaySound = false;
            //this.m_spPlayer.Stop();

            if (this.m_thSound != null) {
                this.m_thStopSound = new Thread(new ParameterizedThreadStart(StopSound));
                this.m_thStopSound.Start(spsSound);
            }
        }

        private void StopSound(object obj) {
            try {
                if (this.m_thSound != null) {
                    //this.m_spPlayer.Stop();
                    this.m_blPlaySound = false;
                    this.m_thSound.Join();

                    if (obj != null) {
                        this.m_thSound = new Thread(new ParameterizedThreadStart(PlaySound));
                        this.m_blPlaySound = true;
                        this.m_thSound.Start((SPlaySound)obj);
                    }
                    else {
                        this.m_thSound = null;
                    }
                }
            }
            catch (Exception e) {

            }
        }

        private void PlaySound(object obj) {
            SPlaySound spsSound = (SPlaySound)obj;

            try {

                using (BinaryReader brFormatCheck = new BinaryReader(File.Open(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media"), spsSound.m_strSoundFile), FileMode.Open))) {

                    brFormatCheck.BaseStream.Position = 20;
                    Int16 i16Format = brFormatCheck.ReadInt16();

                    if (i16Format == 1 || i16Format == 2) {

                        brFormatCheck.Close();

                        // Load it in this thread in case the file is big.
                        this.m_spPlayer.SoundLocation = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media"), spsSound.m_strSoundFile);

                        for (int i = 0; i < spsSound.m_iRepeat && this.m_blPlaySound == true; i++) {
                            this.m_spPlayer.PlaySync();
                        }
                    }
                    else {
                        brFormatCheck.Close();
                    }
                }
            }
            catch (Exception e) {

            }
        }

        #endregion

        // TO DO: With events most of these can be removed.
        #region Internal Commands

        /*
        public void ProconProtectedWeaponsClear() {
            this.Weapons.Clear();
        }

        public void ProconProtectedWeaponsAdd(Kits restriction, string name, WeaponSlots slot, DamageTypes damage) {
            this.Weapons.Add(new Weapon(restriction, name, slot, damage);
        }

        public void ProconProtectedSpecializationClear() {
            this.Specializations.Clear();
        }

        public void ProconProtectedSpecializationAdd(SpecializationSlots slot, string name) {
            this.Specializations.Add(new Specialization(slot, name));
        }
        */

        public void ProconProtectedTeamNamesClear() {
            this.TeamNameList.Clear();

            foreach (CMap cmMap in this.MapListPool) {
                cmMap.TeamNames.Clear();
            }
        }

        public void ProconProtectedTeamNamesAdd(string strFileName, int iTeamID, string strLocalizationKey, string strImageKey) {
            foreach (CMap cmMap in this.MapListPool) {
                if (String.Compare(cmMap.FileName, strFileName, true) == 0) {
                    cmMap.TeamNames.Add(new CTeamName(strFileName, iTeamID, strLocalizationKey, strImageKey));
                }
            }

            this.TeamNameList.Add(new CTeamName(strFileName, iTeamID, strLocalizationKey, strImageKey));
        }

        public void ProconProtectedMapsClear() {
            this.MapListPool.Clear();
        }

        public void ProconProtectedMapsAdd(string strPlaylist, string strFileName, string strGamemode, string strPublicLevelName, int iDefaultSquadID) {
            this.MapListPool.Add(new CMap(strPlaylist, strFileName, strGamemode, strPublicLevelName, iDefaultSquadID));
        }

        public void ProconProtectedReasonsClear() {
            this.Reasons.Clear();
        }

        public void ProconProtectedReasonsAdd(string strReason) {
            this.Reasons.Add(strReason);
        }

        public void ProconProtectedServerVersionsClear(string strType)
        {
            if (strType == "BFBC2")
            {
                this.Game.VersionNumberToFriendlyName.Clear();
            }
            if (strType == "MoH")
            {
                this.Game.VersionNumberToFriendlyName.Clear();
            }
        }

        public void ProconProtectedServerVersionsAdd(string strType, string strVerNr, string strVerRel)
        {
            if (strType == "BFBC2")
            {
                this.Game.VersionNumberToFriendlyName.Add(strVerNr, strVerRel);
            }
            if (strType == "MoH")
            {
                this.Game.VersionNumberToFriendlyName.Add(strVerNr, strVerRel);
            }
        }

        public void ProconProtectedPluginEnable(string strClassName, bool blEnabled)
        {

            if (this.PluginsManager != null && this.PluginsManager.Plugins.LoadedClassNames.Contains(strClassName) == true) {
                if (blEnabled == true) {
                    this.PluginsManager.EnablePlugin(strClassName);
                }
                else {
                    this.PluginsManager.DisablePlugin(strClassName);
                }
            }
        }


        public void ProconProtectedPluginSetVariable(string strClassName, string strVariable, string strValue) {
            if (this.PluginsManager != null) {
                this.PluginsManager.SetPluginVariable(strClassName, strVariable, strValue);
            }
        }

        public void ProconProtectedLayerSetPrivileges(Account account, CPrivileges sprvPrivileges) {

            if (this.Layer.AccountPrivileges.Contains(account.Name) == true) {
                this.Layer.AccountPrivileges[account.Name].SetPrivileges(sprvPrivileges);
            }
            else {

                this.Layer.AccountPrivileges.Add(new AccountPrivilege(account, sprvPrivileges));
                
            }
        }

        public void ProconProtectedLayerEnable(bool blEnabled, UInt16 ui16Port, string strBindingAddress, string strLayerName) {

            if (this.Layer != null) {

                this.Layer.LayerEnabled = blEnabled;
                //if (this.Layer.LayerEnabled == true) {
                this.Layer.ListeningPort = ui16Port;
                this.Layer.BindingAddress = strBindingAddress;
                this.Layer.LayerNameFormat = strLayerName;
                //}

                // Start it up if we've logged into the bfbc2 server..
                if (this.Game != null && this.Game.IsLoggedIn == true) {

                    if (this.Layer.LayerEnabled == true && this.Layer.IsLayerOnline == false) {
                        this.Layer.StartLayerListener();
                    }
                    else if (this.Layer.LayerEnabled == false && this.Layer.IsLayerOnline == true) {
                        this.Layer.ShutdownLayerListener();
                    }
                }
            }
            else {
                // Magic!!!
            }
        }

        public void ProconPrivateServerConnect() {
            this.Connect();
        }

        #region Tasks

        // Quick and dirty thread safe tasks.
        // private readonly object m_taskLocker = new object();

        public void ProconProtectedTasksList() {
            this.Console.Write("Running Tasks: [Name] [Delay] [Interval] [Repeat] [Command]");

            //lock (this.m_taskLocker) {
                foreach (Task ctTask in new List<Task>(this.Tasks)) {
                    this.Console.Write(ctTask.ToString());
                }
            //}

            this.Console.Write(String.Format("End of Tasks List ({0} Tasks)", this.Tasks.Count));
        }

        public void ProconProtectedTasksAdd(string strTaskname, List<string> lstCommandWords, int iDelay, int iInterval, int iRepeat) {
            if (iDelay >= 0 && iInterval > 0 && iRepeat != 0) {
                //lock (this.m_taskLocker) {
                    this.Tasks.Add(new Task(strTaskname, lstCommandWords, iDelay, iInterval, iRepeat));
                //}
            }
        }

        public void ProconProtectedTasksClear() {
            //lock (this.m_taskLocker) {
                this.Tasks.Clear();
            //}
        }

        public void ProconProtectedTasksRemove(string strTaskName) {
            //lock (this.m_taskLocker) {
                for (int i = 0; i < this.Tasks.Count; i++) {
                    if (String.Compare(this.Tasks[i].TaskName, strTaskName) == 0) {
                        this.Tasks.RemoveAt(i);
                        i--;
                    }
                }
            //}
        }

        private void m_taskTimer_Elapsed(object sender, ElapsedEventArgs e) {
            try {
                foreach (Task ctExecute in new List<Task>(this.Tasks)) {
                    if (this.Game != null && this.Game.IsLoggedIn == true && ctExecute.ExecuteCommand == true) {
                        this.Parent.ExecutePRoConCommand(this, ctExecute.Command, 0);
                    }
                }

                this.Tasks.RemoveAll(RepeatTaskDisabled);
            }
            catch (Exception ex) { // Debug for 0.3.1.0 to make sure this bug is gone.
                FrostbiteConnection.LogError("TaskExecuterThread", String.Empty, ex);
            }
        }

        /*
        private void TaskExecuterThread() {

            while (this.m_isTasksRunning == true) {

                try {
                    lock (this.m_taskLocker) {
                        foreach (Task ctExecute in new List<Task>(this.Tasks)) {
                            if (this.Game != null && this.Game.IsLoggedIn == true && ctExecute.ExecuteCommand == true) {
                                this.Parent.ExecutePRoConCommand(this, ctExecute.Command, 0);
                            }
                        }

                        this.Tasks.RemoveAll(RepeatTaskDisabled);
                    }
                }
                catch (Exception ex) { // Debug for 0.3.1.0 to make sure this bug is gone.
                    FrostbiteConnection.LogError("TaskExecuterThread", String.Empty, ex);
                }

                Thread.Sleep(1000);
            }
        }
        */
        private static bool RepeatTaskDisabled(Task ctRemoveAll) {
            return ctRemoveAll.RemoveTask;
        }

        #endregion














































        #endregion

        #region Send Packet Helpers

        public void SendResponse(params string[] words) {
            this.SendResponse(new List<string>(words));
        }

        public void SendResponse(List<string> lstWords) {
            if (lstWords.Count > 0) {

                if (this.Game != null && this.Game.Connection != null) {
                    this.Game.Connection.SendQueued(new Packet(true, true, this.Game.Connection.AcquireSequenceNumber, lstWords));
                }
            }
        }

        private void SendRequest(params string[] words) {
            this.SendRequest(new List<string>(words));
        }

        public void SendRequest(List<string> lstWords) {
            if (this.IsLoggedIn == true && lstWords.Count > 0) {

                if (lstWords.Count >= 4 && String.Compare(lstWords[0], "procon.admin.yell", true) == 0) {
                    this.SendProconAdminYell(lstWords[1], lstWords[2], lstWords[3], lstWords.Count > 4 ? lstWords[4] : String.Empty);
                }
                else {
                    if (this.Game != null && this.Game.Connection != null) {
                        this.Game.Connection.SendQueued(new Packet(false, false, this.Game.Connection.AcquireSequenceNumber, lstWords));
                    }
                }
            }
            else {
                int i = 0;
            }
        }

        public void SendPacket(Packet packet) {
            this.Game.Connection.SendQueued(packet);
        }

        #region Procon Extensions

        public void SendProconAdminSay(string strText, string strPlayerSubset, string strTarget) {

            if (strTarget.Length > 0) {
                this.SendProconLayerPacket(null, new Packet(false, false, this.Game.Connection.AcquireSequenceNumber, new List<string>() { "procon.admin.say", String.Empty, strText, strPlayerSubset, strTarget }));
            }
            else {
                this.SendProconLayerPacket(null, new Packet(false, false, this.Game.Connection.AcquireSequenceNumber, new List<string>() { "procon.admin.say", String.Empty, strText, strPlayerSubset }));
            }
        }

        public void SendProconAdminYell(string strText, string strDisplayTime, string strPlayerSubset, string strTarget) {

            if (strTarget.Length > 0) {
                this.SendProconLayerPacket(null, new Packet(false, false, this.Game.Connection.AcquireSequenceNumber, new List<string>() { "procon.admin.yell", String.Empty, strText, strDisplayTime, strPlayerSubset, strTarget }));
            }
            else {
                this.SendProconLayerPacket(null, new Packet(false, false, this.Game.Connection.AcquireSequenceNumber, new List<string>() { "procon.admin.yell", String.Empty, strText, strDisplayTime, strPlayerSubset }));
            }
        }

        public void SendProconLayerPacket(PRoConLayerClient sender, Packet cpPassOn) {

            lock (new object()) {
                UInt32 ui32MainConnSequence = this.Game.Connection.AcquireSequenceNumber;

                if (this.m_dicForwardedPackets.ContainsKey(ui32MainConnSequence) == false) {

                    SOriginalForwardedPacket spopForwardedPacket = new SOriginalForwardedPacket();
                    spopForwardedPacket.m_ui32OriginalSequence = cpPassOn.SequenceNumber;
                    spopForwardedPacket.m_sender = sender;
                    spopForwardedPacket.m_lstWords = new List<string>(cpPassOn.Words);

                    // Register the packet as forwared. 
                    this.m_dicForwardedPackets.Add(ui32MainConnSequence, spopForwardedPacket);

                    if (cpPassOn.Words.Count >= 5 && String.Compare(cpPassOn.Words[0], "procon.admin.yell") == 0) {

                        if (this.IsPRoConConnection == false) {
                            // Just yell it, we'll capture it and process the return in OnBeforePacketRecv
                            cpPassOn.Words.RemoveAt(1);
                            cpPassOn.Words[0] = "admin.yell";
                        }
                        // Else forward the packet as is so the layer above can append its username.

                    }
                    else if (cpPassOn.Words.Count >= 4 && String.Compare(cpPassOn.Words[0], "procon.admin.say") == 0) {

                        if (this.IsPRoConConnection == false) {
                            // Just yell it, we'll capture it and process the return in OnBeforePacketRecv
                            cpPassOn.Words.RemoveAt(1);
                            cpPassOn.Words[0] = "admin.say";
                        }
                        // Else forward the packet as is so the layer above can append its username.
                    }

                    // Now forward the packet.
                    this.SendPacket(new Packet(false, false, ui32MainConnSequence, cpPassOn.Words));
                }
            }
        }

        public virtual void SendProconLoginUsernamePacket(string username) {

            // By pass all usual login checks.
            if (this.Game != null && this.Game.Connection != null) {
                this.Game.Connection.SendQueued(new Packet(false, false, this.Game.Connection.AcquireSequenceNumber, new List<string>() { "procon.login.username", username }));
            }
        }

        public virtual void SendProconBattlemapListZonesPacket() {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.battlemap.listZones");
            }
        }

        public virtual void SendGetProconVarsPacket(string variable) {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.vars", variable);
            }
        }

        public virtual void SendProconPluginSetVariablePacket(string strClassName, string strVariable, string strValue) {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.plugin.setVariable", strClassName, strVariable, strValue);
            }
        }

        public virtual void SendProconPluginEnablePacket(string strClassName, bool blEnabled) {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.plugin.enable", strClassName, Packet.bltos(blEnabled));
            }
        }

        #region Map Zones

        public virtual void SendProconBattlemapModifyZonePointsPacket(string uid, Point3D[] zonePoints) {
            if (this.IsLoggedIn == true) {
                List<string> list = new List<string>() { "procon.battlemap.modifyZonePoints", uid };
                list.Add(zonePoints.Length.ToString());
                list.AddRange(Point3D.ToStringList(zonePoints));

                this.SendRequest(list);
            }
        }

        public virtual void SendProconBattlemapDeleteZonePacket(string uid) {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.battlemap.deleteZone", uid);
                
            }
        }

        public virtual void SendProconBattlemapCreateZonePacket(string mapFileName, Point3D[] zonePoints) {
            if (this.IsLoggedIn == true) {

                List<string> list = new List<string>() { "procon.battlemap.createZone", mapFileName };
                list.Add(zonePoints.Length.ToString());
                list.AddRange(Point3D.ToStringList(zonePoints));

                this.SendRequest(list);
            }
        }

        public virtual void SendProconBattlemapModifyZoneTagsPacket(string uid, string tagList) {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.battlemap.modifyZoneTags", uid, tagList);

            }
        }

        #endregion

        #region Layer

        public virtual void SendProconLayerSetPrivilegesPacket(string username, UInt32 privileges) {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.layer.setPrivileges", username, privileges.ToString());
            }
        }

        #endregion

        #region Accounts

        public virtual void SendProconAccountListAccountsPacket() {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.account.listAccounts");
            }
        }

        public virtual void SendProconAccountListLoggedInPacket() {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.account.listLoggedIn", "uids");
            }
        }

        public virtual void SendProconAccountSetPasswordPacket(string username, string password) {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.account.setPassword", username, password);
            }
        }

        public virtual void SendProconAccountCreatePacket(string username, string password) {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.account.create", username, password);
            }
        }

        public virtual void SendProconAccountDeletePacket(string username) {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.account.delete", username);
            }
        }

        #endregion

        #region Plugins

        public virtual void SendProconPluginListLoadedPacket() {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.plugin.listLoaded");
            }
        }

        public virtual void SendProconPluginListEnabledPacket() {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.plugin.listEnabled");
            }
        }

        #endregion

        #region Packages

        public virtual void SendProconPackagesInstallPacket(string uid, string version, string md5) {
            if (this.IsLoggedIn == true) {
                this.SendRequest("procon.packages.install", uid, version, md5);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Game events

        private void PRoConClient_PlayerKilled(FrostbiteClient sender, string strKiller, string strVictim, string strDamageType, bool blHeadshot, Point3D pntKiller, Point3D pntVictim) {
            if (this.PlayerKilled != null) {

                CPlayerInfo cpKiller = null, cpVictim = null;

                if (this.PlayerList.Contains(strKiller) == true) {
                    cpKiller = this.PlayerList[strKiller];
                }
                else {
                    cpKiller = new CPlayerInfo(strKiller, String.Empty, 0, 0);
                }

                if (this.PlayerList.Contains(strVictim) == true) {
                    cpVictim = this.PlayerList[strVictim];
                }
                else {
                    cpVictim = new CPlayerInfo(strVictim, String.Empty, 0, 0);
                }

                FrostbiteConnection.RaiseEvent(this.PlayerKilled.GetInvocationList(), this, new Kill(cpKiller, cpVictim, strDamageType, blHeadshot, pntKiller, pntVictim));
            }
        }

        private void PRoConClient_PlayerSpawned(FrostbiteClient sender, string soldierName, string strKit, List<string> lstWeapons, List<string> lstSpecializations) {
            if (this.PlayerSpawned != null && Enum.IsDefined(typeof(Kits), strKit) == true) {
                Inventory inv = new Inventory((Kits)Enum.Parse(typeof(Kits), strKit));

                foreach (string strWeapon in lstWeapons) {
                    if (this.Weapons.Contains(strWeapon) == true) {
                        inv.Weapons.Add(this.Weapons[strWeapon]);
                    }
                }

                foreach (string strSpecialization in lstSpecializations) {
                    if (this.Specializations.Contains(strSpecialization) == true) {
                        inv.Specializations.Add(this.Specializations[strSpecialization]);
                    }
                }

                FrostbiteConnection.RaiseEvent(this.PlayerSpawned.GetInvocationList(), this, soldierName, inv);
            }
        }

        private void PRoConClient_ReservedSlotsPlayerRemoved(FrostbiteClient sender, string strSoldierName) {
            if (this.ReservedSlotList.Contains(strSoldierName) == true) {
                this.ReservedSlotList.Remove(strSoldierName);
            }
        }

        private void PRoConClient_ReservedSlotsPlayerAdded(FrostbiteClient sender, string strSoldierName) {
            if (this.ReservedSlotList.Contains(strSoldierName) == false) {
                this.ReservedSlotList.Add(strSoldierName);
            }
        }

        private void PRoConClient_ReservedSlotsList(FrostbiteClient sender, List<string> soldierNames) {
            foreach (string strSoldierName in soldierNames) {
                if (this.ReservedSlotList.Contains(strSoldierName) == false) {
                    this.ReservedSlotList.Add(strSoldierName);
                }
            }

            foreach (string strSoldierName in this.ReservedSlotList) {
                if (soldierNames.Contains(strSoldierName) == false) {
                    this.ReservedSlotList.Remove(strSoldierName);
                }
            }
        }

        protected void OnServerInfo(FrostbiteClient sender, CServerInfo csiServerInfo) {

            GameMods oldGameMod = this.CurrentServerInfo != null ? this.CurrentServerInfo.GameMod : GameMods.None;

            if (this.CurrentServerInfo != null && oldGameMod != csiServerInfo.GameMod) {
                this.m_gameModModified = true;
            }
            else {
                this.m_gameModModified = false;
            }

            this.CurrentServerInfo = csiServerInfo;
        }

        protected void OnLogout() {
            this.Shutdown();
        }

        protected void OnPlayerLeft(FrostbiteClient sender, string strSoldierName, CPlayerInfo cpiPlayer) {
            if (this.PlayerList.Contains(strSoldierName) == true) {
                this.PlayerList.Remove(strSoldierName);
            }
        }

        protected void OnPunkbusterMessage(FrostbiteClient sender, string strPunkbusterMessage) {

            strPunkbusterMessage = strPunkbusterMessage.TrimEnd('\r', '\n');

            // PunkBuster Server: ([0-9]+)[ ]? ([A-Za-z0-9]+)\(.*?\) ([0-9\.:]+).*?\(.*?\) "(.*?)"
            // PunkBuster Server: 1  2c90591ce08a5f799622705d7ba1155c(-) 192.168.1.3:52460 OK   1 3.0 0 (W) "(U3)Phogue"
            //Match mMatch = Regex.Match(strPunkbusterMessage, @":[ ]+?(?<slotid>[0-9]+)[ ]+?(?<guid>[A-Za-z0-9]+)\(.*?\)[ ]+?(?<ip>[0-9\.:]+).*?\(.*?\)[ ]+?""(?<name>.*?)\""", RegexOptions.IgnoreCase);
            Match mMatch = this.Parent.RegexMatchPunkbusterPlist.Match(strPunkbusterMessage);
            // If it is a punkbuster pb_plist update
            if (mMatch.Success == true && mMatch.Groups.Count >= 5) {

                CPunkbusterInfo newPbInfo = new CPunkbusterInfo(mMatch.Groups["slotid"].Value, mMatch.Groups["name"].Value, mMatch.Groups["guid"].Value, mMatch.Groups["ip"].Value, this.Parent.GetCountryName(mMatch.Groups["ip"].Value), this.Parent.GetCountryCode(mMatch.Groups["ip"].Value));

                if (this.PunkbusterPlayerInfo != null) {
                    FrostbiteConnection.RaiseEvent(this.PunkbusterPlayerInfo.GetInvocationList(), this, newPbInfo);
                }
            }

            mMatch = this.Parent.RegexMatchPunkbusterBeginPlist.Match(strPunkbusterMessage);
            if (mMatch.Success == true && this.PunkbusterBeginPlayerInfo != null) {
                FrostbiteConnection.RaiseEvent(this.PunkbusterBeginPlayerInfo.GetInvocationList(), this);
            }

            mMatch = this.Parent.RegexMatchPunkbusterEndPlist.Match(strPunkbusterMessage);
            if (mMatch.Success == true && this.PunkbusterEndPlayerInfo != null) {
                FrostbiteConnection.RaiseEvent(this.PunkbusterEndPlayerInfo.GetInvocationList(), this);
            }

            // PunkBuster Server: Player Guid Computed ([A-Za-z0-9]+)\(.*?\) \(slot #([0-9]+)\) ([0-9\.:]+) (.*)
            // PunkBuster Server: Player Guid Computed 2c90591ce08a5f799622705d7ba1155c(-) (slot #1) 192.168.1.3:52581 (U3)Phogue
            //mMatch = Regex.Match(strPunkbusterMessage, @": Player Guid Computed[ ]+?(?<guid>[A-Za-z0-9]+)\(.*?\)[ ]+?\(slot #(?<slotid>[0-9]+)\)[ ]+?(?<ip>[0-9\.:]+)[ ]+?(?<name>.*)", RegexOptions.IgnoreCase);
            mMatch = this.Parent.RegexMatchPunkbusterGuidComputed.Match(strPunkbusterMessage);
            // If it is a new connection, technically its a resolved guid type command but stil..
            if (mMatch.Success == true && mMatch.Groups.Count >= 5) {

                CPunkbusterInfo newPbInfo = new CPunkbusterInfo(mMatch.Groups["slotid"].Value, mMatch.Groups["name"].Value, mMatch.Groups["guid"].Value, mMatch.Groups["ip"].Value, this.Parent.GetCountryName(mMatch.Groups["ip"].Value), this.Parent.GetCountryCode(mMatch.Groups["ip"].Value));

                if (this.PunkbusterPlayerInfo != null) {
                    FrostbiteConnection.RaiseEvent(this.PunkbusterPlayerInfo.GetInvocationList(), this, newPbInfo);
                }
            }

            //mMatch = Regex.Match(strPunkbusterMessage, @":[ ]+?(?<banid>[0-9]+)[ ]+?(?<guid>[A-Za-z0-9]+)[ ]+?{(?<remaining>[0-9\-]+)/(?<banlength>[0-9\-]+)}[ ]+?""(?<name>.+?)""[ ]+?""(?<ip>.+?)""[ ]+?(?<reason>.*)", RegexOptions.IgnoreCase);
            mMatch = this.Parent.RegexMatchPunkbusterBanlist.Match(strPunkbusterMessage);

            if (mMatch.Success == true && mMatch.Groups.Count >= 5) {

                //IPAddress ipOut;
                string strIP = String.Empty;
                string[] a_strIP;

                if (mMatch.Groups["ip"].Value.Length > 0 && (a_strIP = mMatch.Groups["ip"].Value.Split(':')).Length > 0) {
                    strIP = a_strIP[0];
                }

                CBanInfo newPbBanInfo = new CBanInfo(mMatch.Groups["name"].Value, mMatch.Groups["guid"].Value, mMatch.Groups["ip"].Value, new TimeoutSubset(mMatch.Groups["banlength"].Value, mMatch.Groups["remaining"].Value), mMatch.Groups["reason"].Value);

                if (this.PunkbusterPlayerBanned != null) {
                    FrostbiteConnection.RaiseEvent(this.PunkbusterPlayerBanned.GetInvocationList(), this, newPbBanInfo);
                }
            }

            //mMatch = Regex.Match(strPunkbusterMessage, @":[ ]+?Guid[ ]+?(?<guid>[A-Za-z0-9]+)[ ]+?has been Unbanned", RegexOptions.IgnoreCase);
            mMatch = this.Parent.RegexMatchPunkbusterUnban.Match(strPunkbusterMessage);
            // If it is a new connection, technically its a resolved guid type command but stil..
            if (mMatch.Success == true && mMatch.Groups.Count >= 2) {
                CBanInfo cbiUnbannedPlayer = new CBanInfo(String.Empty, mMatch.Groups["guid"].Value, String.Empty, new TimeoutSubset(TimeoutSubset.TimeoutSubsetType.None), String.Empty);

                if (this.PunkbusterPlayerUnbanned != null) {
                    FrostbiteConnection.RaiseEvent(this.PunkbusterPlayerUnbanned.GetInvocationList(), this, cbiUnbannedPlayer);
                }
            }

            //mMatch = Regex.Match(strPunkbusterMessage, @": Ban Added to Ban List", RegexOptions.IgnoreCase);
            mMatch = this.Parent.RegexMatchPunkbusterBanAdded.Match(strPunkbusterMessage);
            if (mMatch.Success == true && mMatch.Groups.Count >= 5) {
                this.SendRequest(new List<string>() { "punkBuster.pb_sv_command", this.Variables.GetVariable<string>("PUNKBUSTER_BANLIST_REFRESH", "pb_sv_banlist BC2! ") });
            }
        }

        protected void OnListPlayers(FrostbiteClient sender, List<CPlayerInfo> lstPlayers, CPlayerSubset cpsSubset) {
            if (cpsSubset.Subset == CPlayerSubset.PlayerSubsetType.All) {

                // Add or update players.
                foreach (CPlayerInfo cpiPlayer in lstPlayers) {
                    if (this.PlayerList.Contains(cpiPlayer.SoldierName) == true) {
                        this.PlayerList[this.PlayerList.IndexOf(this.PlayerList[cpiPlayer.SoldierName])] = cpiPlayer;
                    }
                    else {
                        this.PlayerList.Add(cpiPlayer);
                    }
                }

                PlayerDictionary recievedPlayerList = new PlayerDictionary(lstPlayers);
                foreach (CPlayerInfo storedPlayer in new List<CPlayerInfo>(this.PlayerList)) {

                    // If the stored player is not in the list we recieved
                    if (recievedPlayerList.Contains(storedPlayer.SoldierName) == false) {
                        // They have left the server, remove them from the master stored list.
                        this.PlayerList.Remove(storedPlayer.SoldierName);
                    }
                }

            }
        }

        #region Text Chat Moderation

        // FullTextChatModerationList
        private void Game_TextChatModerationListList(FrostbiteClient sender, int startOffset, List<TextChatModerationEntry> textChatModerationList) {
            if (startOffset == 0) {
                this.FullTextChatModerationList.Clear();
            }

            if (textChatModerationList.Count > 0) {
                this.FullTextChatModerationList.AddRange(textChatModerationList);

                this.Game.SendTextChatModerationListListPacket(startOffset + 100);
                //this.SendRequest(new List<string>() { "banList.list", (startOffset + 100).ToString() });
            }
            else {
                // We have recieved the whole banlist in 100 ban increments.. throw event.
                if (this.FullTextChatModerationListList != null) {
                    FrostbiteConnection.RaiseEvent(this.FullTextChatModerationListList.GetInvocationList(), this, this.FullTextChatModerationList);
                }
            }
        }

        private void Game_TextChatModerationListClear(FrostbiteClient sender) {
            this.FullTextChatModerationList.Clear();
        }

        private void Game_TextChatModerationListRemovePlayer(FrostbiteClient sender, TextChatModerationEntry playerEntry) {
            this.FullTextChatModerationList.RemoveEntry(playerEntry);
        }

        private void Game_TextChatModerationListAddPlayer(FrostbiteClient sender, TextChatModerationEntry playerEntry) {
            this.FullTextChatModerationList.AddEntry(playerEntry);
        }

        #endregion

        private void PRoConClient_BanListList(FrostbiteClient sender, int iStartOffset, List<CBanInfo> lstBans) {
            if (iStartOffset == 0) {
                this.FullVanillaBanList.Clear();
            }

            if (lstBans.Count > 0) {
                this.FullVanillaBanList.AddRange(lstBans);

                this.Game.SendBanListListPacket(iStartOffset + 100);
                //this.SendRequest(new List<string>() { "banList.list", (iStartOffset + 100).ToString() });
            }
            else {
                // We have recieved the whole banlist in 100 ban increments.. throw event.
                if (this.FullBanListList != null) {
                    FrostbiteConnection.RaiseEvent(this.FullBanListList.GetInvocationList(), this, this.FullVanillaBanList);
                }
            }
        }

        void PRoConClient_ResponseError(FrostbiteClient sender, Packet originalRequest, string errorMessage) {

            // Banlist backwards compatability with R11 (Will attempt to get "banList.list 100" which will throw this error)
            if (originalRequest.Words.Count > 0 && String.Compare(originalRequest.Words[0], "banList.list", true) == 0 && String.Compare(errorMessage, "InvalidArguments", true) == 0) {
                if (this.FullBanListList != null) {
                    FrostbiteConnection.RaiseEvent(this.FullBanListList.GetInvocationList(), this, this.FullVanillaBanList);
                }
            }
        }

        protected void OnPlayerLimit(FrostbiteClient sender, int iPlayerLimit) {
            this.SendRequest(new List<string>() { "vars.currentPlayerLimit" });
            this.SendRequest(new List<string>() { "vars.maxPlayerLimit" });
            this.SendRequest(new List<string>() { "serverInfo" });
        }

        #endregion

        #region Mapzones

        private void MapZones_MapZoneRemoved(MapZoneDrawing item) {
            this.SaveConnectionConfig();
        }

        private void MapZones_MapZoneChanged(MapZoneDrawing item) {
            this.SaveConnectionConfig();
        }

        private void MapZones_MapZoneAdded(MapZoneDrawing item) {
            this.SaveConnectionConfig();
        }

        #endregion

        public void Disconnect() {

            this.State = ConnectionState.Disconnected;
            this.ForceDisconnect();
        }

        public void ForceDisconnect() {

            this.SaveConnectionConfig();

            this.m_blLoadingSavingConnectionConfig = true;

            if (this.Console != null) this.Console.Logging = false;
            if (this.EventsLogging != null) this.EventsLogging.Logging = false;
            if (this.PunkbusterConsole != null) this.PunkbusterConsole.Logging = false;
            if (this.PluginConsole != null) this.PluginConsole.Logging = false;
            if (this.ChatConsole != null) this.ChatConsole.Logging = false;

            this.SendRequest(new List<string>() { "quit" });

            if (this.m_taskTimer != null) {
                this.m_taskTimer.Stop();
            }

            /*
            this.m_isTasksRunning = false;
            if (this.m_thTasks != null) {

                try {
                    this.m_thTasks.Abort();
                }
                catch (Exception) { }

                this.m_thTasks = null;
            }
            */
            if (this.Layer != null) {
                this.Layer.ShutdownLayerListener();
            }

            this.Shutdown();

            this.m_blLoadingSavingConnectionConfig = false;
        }

        public void Destroy() {

            //this.SaveConnectionConfig();

            this.m_blLoadingSavingConnectionConfig = true;

            lock (this.m_objConfigSavingLocker) {
                if (this.PluginsManager != null) {
                    this.PluginsManager.Unload();
                    this.PluginsManager = null;
                }
            }

            this.m_blLoadingSavingConnectionConfig = false;
        }

    }
}
