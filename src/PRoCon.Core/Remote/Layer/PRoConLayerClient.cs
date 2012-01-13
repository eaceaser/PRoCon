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
using System.Net.Sockets;
using System.Threading;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace PRoCon.Core.Remote.Layer {
    using Core;
    using Core.Plugin;
    using Core.Accounts;
    using Core.Battlemap;
    using Core.Remote;
    using Core.Packages;

    public class PRoConLayerClient {

        #region Constants

        public static string RESPONSE_OK = "OK";
        
        public static string RESPONSE_INVALID_PASSWORD_HASH = "InvalidPasswordHash";
        public static string RESPONSE_INVALID_PASSWORD = "InvalidPassword";
        public static string RESPONSE_INVALID_USERNAME = "InvalidUsername";
        public static string RESPONSE_LOGIN_REQUIRED = "LogInRequired";
        public static string RESPONSE_INSUFFICIENT_PRIVILEGES = "InsufficientPrivileges";
        public static string RESPONSE_INVALID_ARGUMENTS = "InvalidArguments";
        public static string RESPONSE_UNKNOWN_COMMAND = "UnknownCommand";

        // Packages
        public static string RESPONSE_PACKAGE_ALREADYINSTALLED = "AlreadyInstalled";

        #endregion

        //private uscServerConnection m_uscConnectionPanel;
        //private uscAccountsPanel m_uscParent;
        public delegate void LayerClientHandler(PRoConLayerClient sender);
        public event LayerClientHandler ClientShutdown;
        public event LayerClientHandler Login;
        public event LayerClientHandler Logout;
        public event LayerClientHandler Quit;
        public event LayerClientHandler UidRegistered;

        private PRoConApplication m_praApplication;
        private PRoConClient m_prcClient;

        protected delegate void RequestPacketHandler(FrostbiteLayerClient sender, Packet packet);
        protected Dictionary<string, RequestPacketHandler> m_requestDelegates;

        public FrostbiteLayerClient Game {
            get;
            private set;
        }

        public bool IsLoggedIn { get; private set; }

        private bool m_blEventsEnabled = false;
        public bool EventsClient {
            get {
                return this.m_blEventsEnabled;
            }
        }

        // If = "" = events disabled, if != "" then procon events enabled.
        //private string m_strProconEventsUid = String.Empty;
        public string ProconEventsUid { get;  private set; }

        //public bool IsLoggedIn {
        //    get {
        //        return this.IsLoggedIn;
        //    }
        //}

        public string Username {
            get {
                return this.m_strUsername;
            }
        }

        public CPrivileges Privileges {
            get {
                return this.m_sprvPrivileges;
            }
        }

        public bool GzipCompression { get; private set; }

        private string m_strSalt = DateTime.Now.ToString("HH:mm:ss ff");
        private CPrivileges m_sprvPrivileges = new CPrivileges();
        private string m_strUsername = String.Empty;

        public PRoConLayerClient(FrostbiteLayerConnection newConnection, PRoConApplication praApplication, PRoConClient prcClient) {

            this.IsLoggedIn = false;
            this.GzipCompression = false;

            this.ProconEventsUid = String.Empty;

            if (prcClient.Game != null) {

                if (prcClient.Game is BFBC2Client) {
                    this.Game = new BFBC2LayerClient(newConnection);
                }
                else if (prcClient.Game is MoHClient) {
                    this.Game = new MoHLayerClient(newConnection);
                }

                this.m_requestDelegates = new Dictionary<string, RequestPacketHandler>() {
                    { "procon.login.username", this.DispatchProconLoginUsernameRequest  },
                    { "procon.registerUid", this.DispatchProconRegisterUidRequest  },
                    { "procon.version", this.DispatchProconVersionRequest  },
                    { "procon.vars", this.DispatchProconVarsRequest  },
                    { "procon.privileges", this.DispatchProconPrivilegesRequest  },
                    { "procon.compression", this.DispatchProconCompressionRequest  },

                    { "procon.account.listAccounts", this.DispatchProconAccountListAccountsRequest  },
                    { "procon.account.listLoggedIn", this.DispatchProconAccountListLoggedInRequest  },
                    { "procon.account.create", this.DispatchProconAccountCreateRequest  },
                    { "procon.account.delete", this.DispatchProconAccountDeleteRequest  },
                    { "procon.account.setPassword", this.DispatchProconAccountSetPasswordRequest  },

                    { "procon.battlemap.deleteZone", this.DispatchProconBattlemapDeleteZoneRequest  },
                    { "procon.battlemap.createZone", this.DispatchProconBattlemapCreateZoneRequest  },
                    { "procon.battlemap.modifyZoneTags", this.DispatchProconBattlemapModifyZoneTagsRequest  },
                    { "procon.battlemap.modifyZonePoints", this.DispatchProconBattlemapModifyZonePointsRequest  },
                    { "procon.battlemap.listZones", this.DispatchProconBattlemapListZonesRequest  },

                    { "procon.layer.setPrivileges", this.DispatchProconLayerSetPrivilegesRequest  },

                    { "procon.plugin.listLoaded", this.DispatchProconPluginListLoadedRequest  },
                    { "procon.plugin.listEnabled", this.DispatchProconPluginListEnabledRequest  },
                    { "procon.plugin.enable", this.DispatchProconPluginEnableRequest  },
                    { "procon.plugin.setVariable", this.DispatchProconPluginSetVariableRequest  },

                    { "procon.exec", this.DispatchProconExecRequest },
                    { "procon.packages.install", this.DispatchProconPackagesInstallRequest },

                    { "procon.admin.say", this.DispatchProconAdminSayRequest },
                    { "procon.admin.yell", this.DispatchProconAdminYellRequest },
                };

                if ((this.m_praApplication = praApplication) != null && (this.m_prcClient = prcClient) != null) {
                    this.RegisterEvents();
                }
            }
        }

        private void RegisterEvents() {
            if (this.Game != null) {

                this.Game.ConnectionClosed += new FrostbiteLayerClient.EmptyParameterHandler(Game_ConnectionClosed);

                this.Game.RequestPacketUnknownRecieved += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketUnknownRecieved);
                this.Game.RequestLoginHashed += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestLoginHashed);
                this.Game.RequestLoginHashedPassword += new FrostbiteLayerClient.RequestLoginHashedPasswordHandler(Game_RequestLoginHashedPassword);
                this.Game.RequestLoginPlainText += new FrostbiteLayerClient.RequestLoginPlainTextHandler(Game_RequestLoginPlainText);
                this.Game.RequestLogout += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestLogout);
                this.Game.RequestQuit += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestQuit);
                this.Game.RequestHelp += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestHelp);
                this.Game.RequestPacketAdminShutdown += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAdminShutdown);

                this.Game.RequestEventsEnabled += new FrostbiteLayerClient.RequestEventsEnabledHandler(Game_RequestEventsEnabled);

                this.Game.RequestPacketSecureSafeListedRecieved += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketSecureSafeListedRecieved);
                this.Game.RequestPacketUnsecureSafeListedRecieved += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketUnsecureSafeListedRecieved);

                this.Game.RequestPacketPunkbusterRecieved += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketPunkbusterRecieved);
                this.Game.RequestPacketUseMapFunctionRecieved += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketUseMapFunctionRecieved);
                this.Game.RequestPacketAlterMaplistRecieved += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAlterMaplistRecieved);
                this.Game.RequestPacketAdminPlayerMoveRecieved += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAdminPlayerMoveRecieved);
                this.Game.RequestPacketAdminPlayerKillRecieved += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAdminPlayerKillRecieved);
                this.Game.RequestPacketAdminKickPlayerRecieved += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAdminKickPlayerRecieved);
                this.Game.RequestBanListAddRecieved += new FrostbiteLayerClient.RequestBanListAddHandler(Game_RequestBanListAddRecieved);
                this.Game.RequestPacketAlterBanListRecieved += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAlterBanListRecieved);
                this.Game.RequestPacketAlterReservedSlotsListRecieved += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAlterReservedSlotsListRecieved);
                this.Game.RequestPacketAlterTextMonderationListRecieved += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAlterTextMonderationListRecieved);
                this.Game.RequestPacketVarsRecieved += new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketVarsRecieved);
            }

            this.ClientShutdown += new LayerClientHandler(CPRoConLayerClient_LayerClientShutdown);

            this.m_praApplication.AccountsList.AccountAdded += new AccountDictionary.AccountAlteredHandler(AccountsList_AccountAdded);
            this.m_praApplication.AccountsList.AccountRemoved += new AccountDictionary.AccountAlteredHandler(AccountsList_AccountRemoved);

            foreach (Account acAccount in this.m_praApplication.AccountsList) {
                this.m_prcClient.Layer.AccountPrivileges[acAccount.Name].AccountPrivilegesChanged += new AccountPrivilege.AccountPrivilegesChangedHandler(CPRoConLayerClient_AccountPrivilegesChanged);
            }

            this.m_prcClient.RecompilingPlugins += new PRoConClient.EmptyParamterHandler(m_prcClient_CompilingPlugins);
            this.m_prcClient.CompilingPlugins += new PRoConClient.EmptyParamterHandler(m_prcClient_CompilingPlugins);

            this.m_prcClient.PassLayerEvent += new PRoConClient.PassLayerEventHandler(m_prcClient_PassLayerEvent);

            if (this.m_prcClient.PluginsManager != null) {
                this.m_prcClient.PluginsManager.PluginLoaded += new PluginManager.PluginEmptyParameterHandler(Plugins_PluginLoaded);
                this.m_prcClient.PluginsManager.PluginEnabled += new PluginManager.PluginEmptyParameterHandler(Plugins_PluginEnabled);
                this.m_prcClient.PluginsManager.PluginDisabled += new PluginManager.PluginEmptyParameterHandler(Plugins_PluginDisabled);
                this.m_prcClient.PluginsManager.PluginVariableAltered += new PluginManager.PluginVariableAlteredHandler(Plugins_PluginVariableAltered);
            }

            this.m_prcClient.MapGeometry.MapZones.MapZoneAdded += new PRoCon.Core.Battlemap.MapZoneDictionary.MapZoneAlteredHandler(MapZones_MapZoneAdded);
            this.m_prcClient.MapGeometry.MapZones.MapZoneChanged += new PRoCon.Core.Battlemap.MapZoneDictionary.MapZoneAlteredHandler(MapZones_MapZoneChanged);
            this.m_prcClient.MapGeometry.MapZones.MapZoneRemoved += new PRoCon.Core.Battlemap.MapZoneDictionary.MapZoneAlteredHandler(MapZones_MapZoneRemoved);

            this.m_prcClient.PluginConsole.WriteConsole += new PRoCon.Core.Logging.Loggable.WriteConsoleHandler(PluginConsole_WriteConsole);
            this.m_prcClient.ChatConsole.WriteConsoleViaCommand += new PRoCon.Core.Logging.Loggable.WriteConsoleHandler(ChatConsole_WriteConsoleViaCommand);

            this.m_prcClient.Variables.VariableAdded += new PRoCon.Core.Variables.VariableDictionary.PlayerAlteredHandler(Variables_VariableAdded);
            this.m_prcClient.Variables.VariableUpdated += new PRoCon.Core.Variables.VariableDictionary.PlayerAlteredHandler(Variables_VariableUpdated);

            // Packages
            this.m_praApplication.PackageManager.PackageAwaitingRestart += new PackageManager.PackageEventHandler(PackageManager_PackageAwaitingRestart);
            this.m_praApplication.PackageManager.PackageBeginningDownload += new PackageManager.PackageEventHandler(PackageManager_PackageBeginningDownload);
            this.m_praApplication.PackageManager.PackageDownloaded += new PackageManager.PackageEventHandler(PackageManager_PackageDownloaded);
            this.m_praApplication.PackageManager.PackageDownloadFail += new PackageManager.PackageEventHandler(PackageManager_PackageDownloadFail);
            this.m_praApplication.PackageManager.PackageInstalling += new PackageManager.PackageEventHandler(PackageManager_PackageInstalling);
        }

        private void UnregisterEvents() {

            if (this.Game != null) {

                this.Game.ConnectionClosed -= new FrostbiteLayerClient.EmptyParameterHandler(Game_ConnectionClosed);

                this.Game.RequestPacketUnknownRecieved -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketUnknownRecieved);
                this.Game.RequestLoginHashed -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestLoginHashed);
                this.Game.RequestLoginHashedPassword -= new FrostbiteLayerClient.RequestLoginHashedPasswordHandler(Game_RequestLoginHashedPassword);
                this.Game.RequestLoginPlainText -= new FrostbiteLayerClient.RequestLoginPlainTextHandler(Game_RequestLoginPlainText);
                this.Game.RequestLogout -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestLogout);
                this.Game.RequestQuit -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestQuit);
                this.Game.RequestHelp -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestHelp);
                this.Game.RequestPacketAdminShutdown -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAdminShutdown);

                this.Game.RequestEventsEnabled -= new FrostbiteLayerClient.RequestEventsEnabledHandler(Game_RequestEventsEnabled);

                this.Game.RequestPacketSecureSafeListedRecieved -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketSecureSafeListedRecieved);
                this.Game.RequestPacketUnsecureSafeListedRecieved -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketUnsecureSafeListedRecieved);

                this.Game.RequestPacketPunkbusterRecieved -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketPunkbusterRecieved);
                this.Game.RequestPacketUseMapFunctionRecieved -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketUseMapFunctionRecieved);
                this.Game.RequestPacketAlterMaplistRecieved -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAlterMaplistRecieved);
                this.Game.RequestPacketAdminPlayerMoveRecieved -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAdminPlayerMoveRecieved);
                this.Game.RequestPacketAdminPlayerKillRecieved -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAdminPlayerKillRecieved);
                this.Game.RequestPacketAdminKickPlayerRecieved -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAdminKickPlayerRecieved);
                this.Game.RequestBanListAddRecieved -= new FrostbiteLayerClient.RequestBanListAddHandler(Game_RequestBanListAddRecieved);
                this.Game.RequestPacketAlterBanListRecieved -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAlterBanListRecieved);
                this.Game.RequestPacketAlterReservedSlotsListRecieved -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAlterReservedSlotsListRecieved);
                this.Game.RequestPacketAlterTextMonderationListRecieved -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketAlterTextMonderationListRecieved);
                this.Game.RequestPacketVarsRecieved -= new FrostbiteLayerClient.RequestPacketDispatchHandler(Game_RequestPacketVarsRecieved);
            }

            this.ClientShutdown -= new LayerClientHandler(CPRoConLayerClient_LayerClientShutdown);

            this.m_praApplication.AccountsList.AccountAdded -= new AccountDictionary.AccountAlteredHandler(AccountsList_AccountAdded);
            this.m_praApplication.AccountsList.AccountRemoved -= new AccountDictionary.AccountAlteredHandler(AccountsList_AccountRemoved);

            foreach (Account acAccount in this.m_praApplication.AccountsList) {
                this.m_prcClient.Layer.AccountPrivileges[acAccount.Name].AccountPrivilegesChanged -= new AccountPrivilege.AccountPrivilegesChangedHandler(CPRoConLayerClient_AccountPrivilegesChanged);
            }

            this.m_prcClient.RecompilingPlugins -= new PRoConClient.EmptyParamterHandler(m_prcClient_CompilingPlugins);
            this.m_prcClient.CompilingPlugins -= new PRoConClient.EmptyParamterHandler(m_prcClient_CompilingPlugins);

            this.m_prcClient.PassLayerEvent -= new PRoConClient.PassLayerEventHandler(m_prcClient_PassLayerEvent);

            if (this.m_prcClient.PluginsManager != null) {
                this.m_prcClient.PluginsManager.PluginLoaded -= new PluginManager.PluginEmptyParameterHandler(Plugins_PluginLoaded);
                this.m_prcClient.PluginsManager.PluginEnabled -= new PluginManager.PluginEmptyParameterHandler(Plugins_PluginEnabled);
                this.m_prcClient.PluginsManager.PluginDisabled -= new PluginManager.PluginEmptyParameterHandler(Plugins_PluginDisabled);
                this.m_prcClient.PluginsManager.PluginVariableAltered -= new PluginManager.PluginVariableAlteredHandler(Plugins_PluginVariableAltered);
            }

            this.m_prcClient.MapGeometry.MapZones.MapZoneAdded -= new PRoCon.Core.Battlemap.MapZoneDictionary.MapZoneAlteredHandler(MapZones_MapZoneAdded);
            this.m_prcClient.MapGeometry.MapZones.MapZoneChanged -= new PRoCon.Core.Battlemap.MapZoneDictionary.MapZoneAlteredHandler(MapZones_MapZoneChanged);
            this.m_prcClient.MapGeometry.MapZones.MapZoneRemoved -= new PRoCon.Core.Battlemap.MapZoneDictionary.MapZoneAlteredHandler(MapZones_MapZoneRemoved);

            this.m_prcClient.PluginConsole.WriteConsole -= new PRoCon.Core.Logging.Loggable.WriteConsoleHandler(PluginConsole_WriteConsole);
            this.m_prcClient.ChatConsole.WriteConsoleViaCommand -= new PRoCon.Core.Logging.Loggable.WriteConsoleHandler(ChatConsole_WriteConsoleViaCommand);

            this.m_prcClient.Variables.VariableAdded -= new PRoCon.Core.Variables.VariableDictionary.PlayerAlteredHandler(Variables_VariableAdded);
            this.m_prcClient.Variables.VariableUpdated -= new PRoCon.Core.Variables.VariableDictionary.PlayerAlteredHandler(Variables_VariableUpdated);

            // Packages
            this.m_praApplication.PackageManager.PackageAwaitingRestart -= new PackageManager.PackageEventHandler(PackageManager_PackageAwaitingRestart);
            this.m_praApplication.PackageManager.PackageBeginningDownload -= new PackageManager.PackageEventHandler(PackageManager_PackageBeginningDownload);
            this.m_praApplication.PackageManager.PackageDownloaded -= new PackageManager.PackageEventHandler(PackageManager_PackageDownloaded);
            this.m_praApplication.PackageManager.PackageDownloadFail -= new PackageManager.PackageEventHandler(PackageManager_PackageDownloadFail);
            this.m_praApplication.PackageManager.PackageInstalling -= new PackageManager.PackageEventHandler(PackageManager_PackageInstalling);


        }

        private string m_strClientIPPort = String.Empty;
        public string IPPort {
            get {
                string strClientIPPort = this.m_strClientIPPort;

                // However if the connection is open just get it straight from the horses mouth.
                if (this.Game != null) {
                    strClientIPPort = this.Game.IPPort;
                }

                return strClientIPPort;
            }
        }

        #region Account Authentication

        protected string GeneratePasswordHash(byte[] a_bSalt, string strData) {
            MD5 md5Hasher = MD5.Create();

            byte[] a_bCombined = new byte[a_bSalt.Length + strData.Length];
            a_bSalt.CopyTo(a_bCombined, 0);
            Encoding.Default.GetBytes(strData).CopyTo(a_bCombined, a_bSalt.Length);

            byte[] a_bHash = md5Hasher.ComputeHash(a_bCombined);

            StringBuilder sbStringifyHash = new StringBuilder();
            for (int i = 0; i < a_bHash.Length; i++) {
                sbStringifyHash.Append(a_bHash[i].ToString("X2"));
            }

            return sbStringifyHash.ToString();
        }

        protected byte[] HashToByteArray(string strHexString) {
            byte[] a_bReturn = new byte[strHexString.Length / 2];

            for (int i = 0; i < a_bReturn.Length; i++) {
                a_bReturn[i] = Convert.ToByte(strHexString.Substring(i * 2, 2), 16);
            }

            return a_bReturn;
        }

        private bool AuthenticatePlaintextAccount(string strUsername, string strPassword) {

            if (String.Compare(this.GetAccountPassword(strUsername), String.Empty) != 0) {
                return (String.Compare(this.GetAccountPassword(strUsername), strPassword) == 0);
            }
            else {
                return false;
            }
        }

        private bool AuthenticateHashedAccount(string strUsername, string strHashedPassword) {
            if (String.Compare(this.GetAccountPassword(strUsername), String.Empty) != 0) {
                return (String.Compare(GeneratePasswordHash(HashToByteArray(this.m_strSalt), this.GetAccountPassword(strUsername)), strHashedPassword) == 0);
            }
            else {
                return false;
            }
        }

        private string GenerateSalt() {
            Random random = new Random();
            return (this.m_strSalt = this.GeneratePasswordHash(Encoding.ASCII.GetBytes(DateTime.Now.ToString("HH:mm:ss ff") + Convert.ToString(random.NextDouble() * Double.MaxValue)), this.m_strUsername));
        }

        #endregion

        #region Packet Forwarding

        // What we got back from the BFBC2 server..
        UInt32 m_ui32ServerInfoSequenceNumber = 0;
        public void OnServerForwardedResponse(Packet cpPacket) {

            if (this.Game != null) {

                if (this.m_ui32ServerInfoSequenceNumber == cpPacket.SequenceNumber && cpPacket.Words.Count >= 2) {
                    cpPacket.Words[1] = this.m_prcClient.Layer.LayerNameFormat.Replace("%servername%", cpPacket.Words[1]);
                }

                this.Game.SendResponse(cpPacket, cpPacket.Words);

            }

            /*
            if (this.m_connection != null) {

                if (this.m_ui32ServerInfoSequenceNumber == cpPacket.SequenceNumber && cpPacket.Words.Count >= 2) {
                    cpPacket.Words[1] = this.m_prcClient.Layer.LayerNameFormat.Replace("%servername%", cpPacket.Words[1]);
                }

                this.m_connection.SendAsync(cpPacket);
            }
            */
        }

        private void m_prcClient_PassLayerEvent(PRoConClient sender, Packet packet) {

            if (this.Game != null) {
                this.Game.SendPacket(packet);
            }
            /*
            if (this.m_connection != null && this.m_blEventsEnabled == true) {
                this.m_connection.SendAsync(packet);
            }*/
        }

        #endregion

        #region Packet Handling

        #region Extended Protocol Handling

        private void DispatchProconLoginUsernameRequest(FrostbiteLayerClient sender, Packet packet) {
            this.m_strUsername = packet.Words[1];

            // We send back any errors in the login process after they attempt to login.
            if (this.m_praApplication.AccountsList.Contains(this.m_strUsername) == true) {
                this.m_sprvPrivileges = this.GetAccountPrivileges(this.m_strUsername);

                this.m_sprvPrivileges.SetLowestPrivileges(this.m_prcClient.Privileges);

                if (this.m_sprvPrivileges.CanLogin == true) {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_USERNAME);
            }
        }

        private void DispatchProconRegisterUidRequest(FrostbiteLayerClient sender, Packet packet) {
            
            if (this.IsLoggedIn == true) {
            
                bool blEnabled = true;

                if (bool.TryParse(packet.Words[1], out blEnabled) == true) {

                    if (blEnabled == false) {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                        this.ProconEventsUid = String.Empty;
                    }
                    else if (packet.Words.Count >= 3) {

                        if (this.m_prcClient.Layer.LayerClients.isUidUnique(packet.Words[2]) == true) {
                            sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                            this.ProconEventsUid = packet.Words[2];

                            if (this.UidRegistered != null) {
                                FrostbiteConnection.RaiseEvent(this.UidRegistered.GetInvocationList(), this);
                            }

                        }
                        else {
                            sender.SendResponse(packet, "ProconUidConflict");
                        }
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconVersionRequest(FrostbiteLayerClient sender, Packet packet) {
            sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK, Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        private void DispatchProconVarsRequest(FrostbiteLayerClient sender, Packet packet) {

            if (this.IsLoggedIn == true) {

                if (packet.Words.Count == 2) {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK, packet.Words[1], this.m_prcClient.Variables.GetVariable<string>(packet.Words[1], ""));
                }
                else if (packet.Words.Count > 2) {

                    if (this.m_sprvPrivileges.CanIssueLimitedProconCommands == true) {

                        this.m_prcClient.Variables.SetVariable(packet.Words[1], packet.Words[2]);

                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK, packet.Words[1], this.m_prcClient.Variables.GetVariable<string>(packet.Words[1], ""));
                    }
                    else {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconPrivilegesRequest(FrostbiteLayerClient sender, Packet packet) {

            if (this.IsLoggedIn == true) {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK, this.m_sprvPrivileges.PrivilegesFlags.ToString());
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconCompressionRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {

                bool enableCompress = false;

                if (packet.Words.Count == 2 && bool.TryParse(packet.Words[1], out enableCompress) == true) {
                    this.GzipCompression = enableCompress;
                    
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconPackagesInstallRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (packet.Words.Count == 4) {
                    if (this.m_sprvPrivileges.CanIssueAllProconCommands == true) {

                        // Register the package as psuedo-known
                        this.m_praApplication.PackageManager.RemotePackages.AddPackage(new Package(packet.Words[1], packet.Words[2], packet.Words[3]));

                        if (this.m_praApplication.PackageManager.CanDownloadPackage(packet.Words[1]) == true) {
                            sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                            this.m_praApplication.PackageManager.DownloadInstallPackage(packet.Words[1], false);
                        }
                        else {
                            sender.SendResponse(packet, PRoConLayerClient.RESPONSE_PACKAGE_ALREADYINSTALLED);
                        }
                    }
                    else {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconExecRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanIssueAllProconCommands == true) {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                    packet.Words.RemoveAt(0);
                    this.m_praApplication.ExecutePRoConCommand(this.m_prcClient, packet.Words, 0);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        #region Accounts

        private void DispatchProconAccountListAccountsRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanIssueLimitedProconCommands == true) {

                    List<string> lstAccounts = new List<string>();
                    lstAccounts.Add(PRoConLayerClient.RESPONSE_OK);

                    foreach (string strAccountName in this.m_praApplication.AccountsList.ListAccountNames()) {
                        if (this.m_prcClient.Layer.AccountPrivileges.Contains(strAccountName) == true) {
                            lstAccounts.Add(strAccountName);
                            lstAccounts.Add(this.m_prcClient.Layer.AccountPrivileges[strAccountName].Privileges.PrivilegesFlags.ToString());
                        }
                    }

                    sender.SendResponse(packet, lstAccounts);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconAccountListLoggedInRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.m_sprvPrivileges.CanIssueLimitedProconCommands == true) {

                List<string> lstLoggedInAccounts = this.m_prcClient.Layer.GetLoggedInAccounts((packet.Words.Count >= 2 && String.Compare(packet.Words[1], "uids") == 0));

                //List<string> lstLoggedInAccounts = this.m_prcClient.Layer.GetLoggedInAccounts();
                lstLoggedInAccounts.Insert(0, PRoConLayerClient.RESPONSE_OK);

                sender.SendResponse(packet, lstLoggedInAccounts);
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
            }
        }

        private void DispatchProconAccountCreateRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanIssueLimitedProconCommands == true) {

                    if (this.m_praApplication.AccountsList.Contains(packet.Words[1]) == false) {
                        if (packet.Words[2].Length > 0) {
                            sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);
                            this.m_praApplication.AccountsList.CreateAccount(packet.Words[1], packet.Words[2]);
                            //this.m_uscParent.LayerCreateAccount(
                        }
                        else {
                            sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                        }
                    }
                    else {
                        sender.SendResponse(packet, "AccountAlreadyExists");
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconAccountDeleteRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanIssueLimitedProconCommands == true) {
                    if (packet.Words.Count >= 2) {

                        if (this.m_praApplication.AccountsList.Contains(packet.Words[1]) == true) {
                            sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                            this.m_praApplication.AccountsList.Remove(packet.Words[1]);
                            //this.m_uscParent.LayerDeleteAccount(cpPacket.Words[1]);
                        }
                        else {
                            sender.SendResponse(packet, "AccountDoesNotExists");
                        }
                    }
                    else {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconAccountSetPasswordRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanIssueLimitedProconCommands == true) {

                    if (packet.Words.Count >= 3 && packet.Words[2].Length > 0) {

                        if (this.m_praApplication.AccountsList.Contains(packet.Words[1]) == true) {
                            sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                            this.m_praApplication.AccountsList[packet.Words[1]].Password = packet.Words[2];
                        }
                        else {
                            sender.SendResponse(packet, "AccountDoesNotExists");
                        }
                    }
                    else {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        #endregion

        #region Battlemap

        private void DispatchProconBattlemapDeleteZoneRequest(FrostbiteLayerClient sender, Packet packet) {

            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanEditMapZones == true) {
                    if (this.m_prcClient.MapGeometry.MapZones.Contains(packet.Words[1]) == true) {
                        this.m_prcClient.MapGeometry.MapZones.Remove(packet.Words[1]);
                    }

                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconBattlemapCreateZoneRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanEditMapZones == true) {

                    if (packet.Words.Count >= 3) {

                        int iPoints = 0;

                        if (int.TryParse(packet.Words[2], out iPoints) == true) {

                            Point3D[] points = new Point3D[iPoints];

                            for (int i = 0; i < iPoints && i + 3 < packet.Words.Count; i++) {
                                points[i] = new Point3D(packet.Words[2 + i * 3 + 1], packet.Words[2 + i * 3 + 2], packet.Words[2 + i * 3 + 3]);
                            }

                            this.m_prcClient.MapGeometry.MapZones.CreateMapZone(packet.Words[1], points);
                        }

                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);
                    }
                    else {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconBattlemapModifyZoneTagsRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanEditMapZones == true) {

                    if (packet.Words.Count >= 3) {

                        if (this.m_prcClient.MapGeometry.MapZones.Contains(packet.Words[1]) == true) {
                            this.m_prcClient.MapGeometry.MapZones[packet.Words[1]].Tags.FromString(packet.Words[2]);
                        }

                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);
                    }
                    else {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconBattlemapModifyZonePointsRequest(FrostbiteLayerClient sender, Packet packet) {
            
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanEditMapZones == true) {

                    if (packet.Words.Count >= 3) {

                        int iPoints = 0;

                        if (int.TryParse(packet.Words[2], out iPoints) == true) {

                            Point3D[] points = new Point3D[iPoints];

                            for (int i = 0; i < iPoints && i + 3 < packet.Words.Count; i++) {
                                points[i] = new Point3D(packet.Words[2 + i * 3 + 1], packet.Words[2 + i * 3 + 2], packet.Words[2 + i * 3 + 3]);
                            }

                            if (this.m_prcClient.MapGeometry.MapZones.Contains(packet.Words[1]) == true) {
                                this.m_prcClient.MapGeometry.MapZones.ModifyMapZonePoints(packet.Words[1], points);
                            }
                        }

                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);
                    }
                    else {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconBattlemapListZonesRequest(FrostbiteLayerClient sender, Packet packet) {

            if (this.IsLoggedIn == true) {

                List<string> listPacket = new List<string>() { PRoConLayerClient.RESPONSE_OK };

                listPacket.Add(this.m_prcClient.MapGeometry.MapZones.Count.ToString());

                foreach (MapZoneDrawing zone in this.m_prcClient.MapGeometry.MapZones) {

                    listPacket.Add(zone.UID);
                    listPacket.Add(zone.LevelFileName);
                    listPacket.Add(zone.Tags.ToString());

                    listPacket.Add(zone.ZonePolygon.Length.ToString());
                    listPacket.AddRange(Point3D.ToStringList(zone.ZonePolygon));
                }

                sender.SendResponse(packet, listPacket);
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        #endregion

        #region Layer

        private void DispatchProconLayerSetPrivilegesRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanIssueLimitedProconCommands == true) {

                    UInt32 ui32Privileges = 0;

                    if (packet.Words.Count >= 3 && UInt32.TryParse(packet.Words[2], out ui32Privileges) == true) {

                        if (this.m_praApplication.AccountsList.Contains(packet.Words[1]) == true) {

                            CPrivileges sprvPrivs = new CPrivileges();

                            sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                            sprvPrivs.PrivilegesFlags = ui32Privileges;
                            this.m_prcClient.Layer.AccountPrivileges[packet.Words[1]].SetPrivileges(sprvPrivs);
                        }
                        else {
                            sender.SendResponse(packet, "AccountDoesNotExists");
                        }
                    }
                    else {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        #endregion

        #region Plugin

        private void DispatchProconPluginListLoadedRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanIssueLimitedProconPluginCommands == true) {

                    if (packet.Words.Count == 1) {
                        List<string> lstLoadedPlugins = this.GetListLoadedPlugins();

                        lstLoadedPlugins.Insert(0, PRoConLayerClient.RESPONSE_OK);

                        sender.SendResponse(packet, lstLoadedPlugins);
                    }
                    else {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconPluginListEnabledRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanIssueLimitedProconPluginCommands == true) {
                    List<string> lstEnabledPlugins = this.m_prcClient.PluginsManager.Plugins.EnabledClassNames;
                    lstEnabledPlugins.Insert(0, PRoConLayerClient.RESPONSE_OK);

                    sender.SendResponse(packet, lstEnabledPlugins);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconPluginEnableRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanIssueLimitedProconPluginCommands == true) {
                    bool blEnabled = false;

                    if (packet.Words.Count >= 3 && bool.TryParse(packet.Words[2], out blEnabled) == true) {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                        if (blEnabled == true) {
                            this.m_prcClient.PluginsManager.EnablePlugin(packet.Words[1]);
                        }
                        else {
                            this.m_prcClient.PluginsManager.DisablePlugin(packet.Words[1]);
                        }
                    }
                    else {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconPluginSetVariableRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanIssueLimitedProconPluginCommands == true) {

                    if (packet.Words.Count >= 4) {

                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                        this.m_prcClient.PluginsManager.SetPluginVariable(packet.Words[1], packet.Words[2], packet.Words[3]);
                    }
                    else {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        #endregion

        #region Communication

        private void DispatchProconAdminSayRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                
                if (packet.Words.Count >= 4) {

                    // Append the admin to the adminstack and send it on its way..
                    if (packet.Words[1].Length > 0) {
                        packet.Words[1] = String.Format("{0}|{1}", packet.Words[1], CPluginVariable.Encode(this.m_strUsername));
                    }
                    else {
                        packet.Words[1] = CPluginVariable.Encode(this.m_strUsername);
                    }

                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void DispatchProconAdminYellRequest(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                
                if (packet.Words.Count >= 5) {
                    // Append the admin to the adminstack and send it on its way..
                    if (packet.Words[1].Length > 0) {
                        packet.Words[1] = String.Format("{0}|{1}", packet.Words[1], CPluginVariable.Encode(this.m_strUsername));
                    }
                    else {
                        packet.Words[1] = CPluginVariable.Encode(this.m_strUsername);
                    }

                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        #endregion

        private void Game_RequestPacketUnknownRecieved(FrostbiteLayerClient sender, Packet packet) {

            if (packet.Words.Count >= 1) {
                if (this.m_requestDelegates.ContainsKey(packet.Words[0]) == true) {
                    this.m_requestDelegates[packet.Words[0]](sender, packet);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_UNKNOWN_COMMAND);
                }
            }
        }

        #endregion

        #region Overridden Protocol Handling

        private void Game_RequestLoginHashed(FrostbiteLayerClient sender, Packet packet) {
            sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK, this.GenerateSalt());
        }

        private void Game_RequestLoginHashedPassword(FrostbiteLayerClient sender, Packet packet, string hashedPassword) {

            if (this.m_praApplication.AccountsList.Contains(this.m_strUsername) == false) {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_USERNAME);
            }
            else {
                if (this.AuthenticateHashedAccount(this.m_strUsername, hashedPassword) == true) {

                    this.m_sprvPrivileges = this.GetAccountPrivileges(this.m_strUsername);
                    this.m_sprvPrivileges.SetLowestPrivileges(this.m_prcClient.Privileges);

                    if (this.m_sprvPrivileges.CanLogin == true) {
                        this.IsLoggedIn = true;
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                        if (this.Login != null) {
                            FrostbiteConnection.RaiseEvent(this.Login.GetInvocationList(), this);
                        }
                    }
                    else {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_PASSWORD_HASH);
                }
            }
        }

        private void Game_RequestLoginPlainText(FrostbiteLayerClient sender, Packet packet, string password) {

            if (this.m_praApplication.AccountsList.Contains(this.m_strUsername) == false) {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_USERNAME);
            }
            else {

                if (this.AuthenticatePlaintextAccount(this.m_strUsername, password) == true) {

                    this.IsLoggedIn = true;
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                    if (this.Login != null) {
                        FrostbiteConnection.RaiseEvent(this.Login.GetInvocationList(), this);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_PASSWORD);
                }
            } 
        }

        private void Game_RequestLogout(FrostbiteLayerClient sender, Packet packet) {
            sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);
            
            this.IsLoggedIn = false;

            if (this.Logout != null) {
                FrostbiteConnection.RaiseEvent(this.Logout.GetInvocationList(), this);
            }
        }

        private void Game_RequestQuit(FrostbiteLayerClient sender, Packet packet) {
            sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

            if (this.Logout != null) {
                FrostbiteConnection.RaiseEvent(this.Logout.GetInvocationList(), this);
            }

            if (this.Quit != null) {
                FrostbiteConnection.RaiseEvent(this.Quit.GetInvocationList(), this);
            }

            this.Shutdown();
        }

        private void Game_RequestEventsEnabled(FrostbiteLayerClient sender, Packet packet, bool eventsEnabled) {
            if (this.IsLoggedIn == true) {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_OK);

                this.m_blEventsEnabled = eventsEnabled;
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void Game_RequestHelp(FrostbiteLayerClient sender, Packet packet) {
            // TO DO: Edit on way back with additional commands IF NOT PRESENT.
            this.m_prcClient.SendProconLayerPacket(this, packet);
        }

        private void Game_RequestPacketAdminShutdown(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanShutdownServer == true) {
                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        #endregion

        #region Game Protocol Handling

        private void Game_RequestPacketSecureSafeListedRecieved(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                this.m_prcClient.SendProconLayerPacket(this, packet);
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void Game_RequestPacketUnsecureSafeListedRecieved(FrostbiteLayerClient sender, Packet packet) {
            
            if (packet.Words.Count >= 1 && String.Compare(packet.Words[0], "serverInfo", true) == 0) {
                this.m_ui32ServerInfoSequenceNumber = packet.SequenceNumber;
            }
            
            this.m_prcClient.SendProconLayerPacket(this, packet);
        }

        private void Game_RequestPacketPunkbusterRecieved(FrostbiteLayerClient sender, Packet packet) {
 	        if (this.IsLoggedIn == true) {

                if (packet.Words.Count >= 2) {
                    
                    bool blCommandProcessed = false;
                    
                    if (this.m_sprvPrivileges.CannotIssuePunkbusterCommands == true) {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);

                        blCommandProcessed = true;
                    }
                    else {
                        Match mtcMatch = Regex.Match(packet.Words[1], "^(?=(?<pb_sv_command>pb_sv_plist))|(?=(?<pb_sv_command>pb_sv_ban))|(?=(?<pb_sv_command>pb_sv_banguid))|(?=(?<pb_sv_command>pb_sv_banlist))|(?=(?<pb_sv_command>pb_sv_getss))|(?=(?<pb_sv_command>pb_sv_kick)[ ]+?.*?[ ]+?(?<pb_sv_kick_time>[0-9]+)[ ]+)|(?=(?<pb_sv_command>pb_sv_unban))|(?=(?<pb_sv_command>pb_sv_unbanguid))|(?=(?<pb_sv_command>pb_sv_reban))", RegexOptions.IgnoreCase);

                        // IF they tried to issue a pb_sv_command that isn't on the safe list AND they don't have full access.
                        if (mtcMatch.Success == false && this.m_sprvPrivileges.CanIssueAllPunkbusterCommands == false) {
                            sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                            blCommandProcessed = true;
                        }
                        else {

                            if (this.m_sprvPrivileges.CanPermanentlyBanPlayers == false && (String.Compare(mtcMatch.Groups["pb_sv_command"].Value, "pb_sv_ban", true) == 0 || String.Compare(mtcMatch.Groups["pb_sv_command"].Value, "pb_sv_banguid", true) == 0 || String.Compare(mtcMatch.Groups["pb_sv_command"].Value, "pb_sv_reban", true) == 0)) {
                                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                                blCommandProcessed = true;
                            }
                            else if (this.m_sprvPrivileges.CanEditBanList == false && (String.Compare(mtcMatch.Groups["pb_sv_command"].Value, "pb_sv_unban", true) == 0 || String.Compare(mtcMatch.Groups["pb_sv_command"].Value, "pb_sv_unbanguid", true) == 0)) {
                                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                                blCommandProcessed = true;
                            }
                            else if (String.Compare(mtcMatch.Groups["pb_sv_command"].Value, "pb_sv_kick", true) == 0) {

                                int iBanLength = 0;

                                // NOTE* Punkbuster uses minutes not seconds.
                                if (int.TryParse(mtcMatch.Groups["pb_sv_kick_time"].Value, out iBanLength) == true) {

                                    // If they cannot punish players at all..
                                    if (this.m_sprvPrivileges.CannotPunishPlayers == true) {
                                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                                        blCommandProcessed = true;
                                    }
                                    // If they can temporary ban but not permanently ban BUT the banlength is over an hour (default)
                                    else if (this.m_sprvPrivileges.CanTemporaryBanPlayers == true && this.m_sprvPrivileges.CanPermanentlyBanPlayers == false && iBanLength > (this.m_prcClient.Variables.GetVariable<int>("TEMP_BAN_CEILING", 3600) / 60)) {
                                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                                        blCommandProcessed = true;
                                    }
                                    // If they can kick but not temp or perm ban players AND the banlength is over 0 (no ban time)
                                    else if (this.m_sprvPrivileges.CanKickPlayers == true && this.m_sprvPrivileges.CanTemporaryBanPlayers == false && this.m_sprvPrivileges.CanPermanentlyBanPlayers == false && iBanLength > 0) {
                                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                                        blCommandProcessed = true;
                                    }
                                    // ELSE they have punkbuster access and full ban privs.. issue the command.
                                }
                                else { // Would rather stop it here than pass it on
                                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);

                                    blCommandProcessed = true;
                                }
                            }
                            // ELSE they have permission to issue this command (full or partial)
                        }
                    }

                    // Was not denied above, send it on to the game server.
                    if (blCommandProcessed == false) {
                        this.m_prcClient.SendProconLayerPacket(this, packet);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INVALID_ARGUMENTS);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void Game_RequestPacketUseMapFunctionRecieved(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanUseMapFunctions == true) {
                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void Game_RequestPacketAlterMaplistRecieved(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanEditMapList == true) {
                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void Game_RequestPacketAdminPlayerMoveRecieved(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanMovePlayers == true) {
                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void Game_RequestPacketAdminPlayerKillRecieved(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanKillPlayers == true) {
                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void Game_RequestPacketAdminKickPlayerRecieved(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanKickPlayers == true) {
                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void Game_RequestBanListAddRecieved(FrostbiteLayerClient sender, Packet packet, CBanInfo newBan) {
            if (this.IsLoggedIn == true) {

                if (newBan.BanLength.Subset == TimeoutSubset.TimeoutSubsetType.Permanent && this.m_sprvPrivileges.CanPermanentlyBanPlayers == true) {
                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else if (newBan.BanLength.Subset == TimeoutSubset.TimeoutSubsetType.Round && this.m_sprvPrivileges.CanTemporaryBanPlayers == true) {
                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else if (newBan.BanLength.Subset == TimeoutSubset.TimeoutSubsetType.Seconds && this.m_sprvPrivileges.CanPermanentlyBanPlayers == true) {
                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else if (newBan.BanLength.Subset == TimeoutSubset.TimeoutSubsetType.Seconds && this.m_sprvPrivileges.CanTemporaryBanPlayers == true) {
                    
                    if (newBan.BanLength.Seconds <= this.m_prcClient.Variables.GetVariable<int>("TEMP_BAN_CEILING", 3600)) {
                        this.m_prcClient.SendProconLayerPacket(this, packet);
                    }
                    else {
                        sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                    }
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void Game_RequestPacketAlterBanListRecieved(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanEditBanList == true) {
                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void Game_RequestPacketAlterTextMonderationListRecieved(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanEditTextChatModerationList == true) {
                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void Game_RequestPacketAlterReservedSlotsListRecieved(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanEditReservedSlotsList == true) {
                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        private void Game_RequestPacketVarsRecieved(FrostbiteLayerClient sender, Packet packet) {
            if (this.IsLoggedIn == true) {
                if (this.m_sprvPrivileges.CanAlterServerSettings == true) {
                    this.m_prcClient.SendProconLayerPacket(this, packet);
                }
                else {
                    sender.SendResponse(packet, PRoConLayerClient.RESPONSE_INSUFFICIENT_PRIVILEGES);
                }
            }
            else {
                sender.SendResponse(packet, PRoConLayerClient.RESPONSE_LOGIN_REQUIRED);
            }
        }

        #endregion

        #endregion

        #region Accounts


        private string GetAccountPassword(string strUsername) {

            string strReturnPassword = String.Empty;

            if (this.m_praApplication.AccountsList.Contains(strUsername) == true) {
                strReturnPassword = this.m_praApplication.AccountsList[strUsername].Password;
            }

            if (String.IsNullOrEmpty(strUsername) == true) {
                strReturnPassword = this.m_prcClient.Variables.GetVariable<string>("GUEST_PASSWORD", "");
            }

            return strReturnPassword;
        }

        private CPrivileges GetAccountPrivileges(string strUsername) {

            CPrivileges sprReturn = new CPrivileges();
            sprReturn.PrivilegesFlags = 0;

            if (this.m_prcClient.Layer.AccountPrivileges.Contains(strUsername) == true) {
                sprReturn = this.m_prcClient.Layer.AccountPrivileges[strUsername].Privileges;
                //sprReturn = this.m_praApplication.AccountsList[strUsername].AccountPrivileges[this.m_prcClient.HostNamePort].Privileges;
            }

            if (String.IsNullOrEmpty(strUsername) == true && this.m_prcClient.Variables.IsVariableNullOrEmpty("GUEST_PRIVILEGES") == false) {
                sprReturn.PrivilegesFlags = this.m_prcClient.Variables.GetVariable<UInt32>("GUEST_PRIVILEGES", 0);
            }

            return sprReturn;
        }

        // TO DO: Implement event once available
        public void OnAccountLogin(string strUsername, CPrivileges sprvPrivileges) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                this.Game.SendRequest("procon.account.onLogin", strUsername, sprvPrivileges.PrivilegesFlags.ToString());
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, new List<string>() { "procon.account.onLogin", strUsername, sprvPrivileges.PrivilegesFlags.ToString() }));
            }
        }

        // TO DO: Implement event once available
        public void OnAccountLogout(string strUsername) {
            if (this.IsLoggedIn == true && String.Compare(strUsername, this.m_strUsername) != 0 && this.m_blEventsEnabled == true && this.Game != null) {
                this.Game.SendRequest("procon.account.onLogout", strUsername);
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, new List<string>() { "procon.account.onLogout", strUsername }));
            }
        }

        private void CPRoConLayerClient_AccountPrivilegesChanged(AccountPrivilege item) {

            CPrivileges cpPrivs = new CPrivileges(item.Privileges.PrivilegesFlags);

            cpPrivs.SetLowestPrivileges(this.m_prcClient.Privileges);

            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                this.Game.SendRequest("procon.account.onAltered", item.Owner.Name, cpPrivs.PrivilegesFlags.ToString());
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, new List<string>() { "procon.account.onAltered", item.Owner.Name, cpPrivs.PrivilegesFlags.ToString() }));
            }

            if (String.Compare(this.m_strUsername, item.Owner.Name) == 0) {
                this.m_sprvPrivileges = cpPrivs;
            }
        }

        private void AccountsList_AccountRemoved(Account item) {

            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                this.Game.SendRequest("procon.account.onDeleted", item.Name);
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, new List<string>() { "procon.account.onDeleted", item.Name }));
            }
        }

        private void AccountsList_AccountAdded(Account item) {

            this.m_prcClient.Layer.AccountPrivileges[item.Name].AccountPrivilegesChanged += new AccountPrivilege.AccountPrivilegesChangedHandler(CPRoConLayerClient_AccountPrivilegesChanged);

            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                this.Game.SendRequest("procon.account.onCreated", item.Name);
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, new List<string>() { "procon.account.onCreated", item.Name }));
            }
        }

        public void OnRegisteredUid(string uid, string strUsername) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                this.Game.SendRequest("procon.account.onUidRegistered", uid, strUsername);
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, new List<string>() { "procon.account.onLogin", strUsername, sprvPrivileges.PrivilegesFlags.ToString() }));
            }
        }

        #endregion

        #region Plugins

        private List<string> GetListLoadedPlugins() {
            List<string> lstReturn = new List<string>();

            foreach (string strPluginClassName in this.m_prcClient.PluginsManager.Plugins.LoadedClassNames) {
                // Get some updated plugin details..
                PluginDetails spDetails = this.m_prcClient.PluginsManager.GetPluginDetails(strPluginClassName);

                lstReturn.Add(spDetails.ClassName);

                lstReturn.Add(spDetails.Name);
                lstReturn.Add(spDetails.Author);
                lstReturn.Add(spDetails.Website);
                lstReturn.Add(spDetails.Version);
                if (this.GzipCompression == true) {
                    lstReturn.Add(Packet.Compress(spDetails.Description));
                }
                else {
                    lstReturn.Add(spDetails.Description);
                }
                lstReturn.Add(spDetails.DisplayPluginVariables.Count.ToString());

                foreach (CPluginVariable cpvVariable in spDetails.DisplayPluginVariables) {
                    lstReturn.Add(cpvVariable.Name);
                    lstReturn.Add(cpvVariable.Type);
                    lstReturn.Add(cpvVariable.Value);
                }
            }

            return lstReturn;
        }

        private void m_prcClient_CompilingPlugins(PRoConClient sender) {
            this.m_prcClient.PluginsManager.PluginLoaded += new PluginManager.PluginEmptyParameterHandler(Plugins_PluginLoaded);
            this.m_prcClient.PluginsManager.PluginEnabled += new PluginManager.PluginEmptyParameterHandler(Plugins_PluginEnabled);
            this.m_prcClient.PluginsManager.PluginDisabled += new PluginManager.PluginEmptyParameterHandler(Plugins_PluginDisabled);
            this.m_prcClient.PluginsManager.PluginVariableAltered += new PluginManager.PluginVariableAlteredHandler(Plugins_PluginVariableAltered);
        }

        private void Plugins_PluginLoaded(string strClassName) {
            PluginDetails spdDetails = this.m_prcClient.PluginsManager.GetPluginDetails(strClassName);

            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {

                List<string> lstOnPluginLoaded = new List<string>() { "procon.plugin.onLoaded", spdDetails.ClassName, spdDetails.Name, spdDetails.Author, spdDetails.Website, spdDetails.Version, spdDetails.Description, spdDetails.DisplayPluginVariables.Count.ToString() };

                foreach (CPluginVariable cpvVariable in spdDetails.DisplayPluginVariables) {
                    lstOnPluginLoaded.AddRange(new List<string> { cpvVariable.Name, cpvVariable.Type, cpvVariable.Value });
                }

                this.Game.SendRequest(lstOnPluginLoaded);
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, lstOnPluginLoaded));
            }
        }

        private void Plugins_PluginVariableAltered(PluginDetails spdNewDetails) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {

                List<string> lstWords = new List<string>() { "procon.plugin.onVariablesAltered", spdNewDetails.ClassName, (spdNewDetails.DisplayPluginVariables.Count).ToString() };

                foreach (CPluginVariable cpvVariable in spdNewDetails.DisplayPluginVariables) {
                    lstWords.AddRange(new string[] { cpvVariable.Name, cpvVariable.Type, cpvVariable.Value });
                }

                this.Game.SendRequest(lstWords);
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, lstWords));
            }
        }

        private void Plugins_PluginEnabled(string strClassName) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                this.Game.SendRequest("procon.plugin.onEnabled", strClassName, Packet.bltos(true));
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, new List<string>() { "procon.plugin.onEnabled", strClassName, bool.TrueString }));
            }
        }

        private void Plugins_PluginDisabled(string strClassName) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                this.Game.SendRequest("procon.plugin.onEnabled", strClassName, Packet.bltos(false));
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, new List<string>() { "procon.plugin.onEnabled", strClassName, bool.FalseString }));
            }
        }

        public void PluginConsole_WriteConsole(DateTime dtLoggedTime, string strLoggedText) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                this.Game.SendRequest("procon.plugin.onConsole", dtLoggedTime.ToBinary().ToString(), strLoggedText);
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, new List<string>() { "procon.plugin.onConsole", dtLoggedTime.ToBinary().ToString(), strLoggedText }));
            }
        }

        #endregion

        #region Chat

        public void ChatConsole_WriteConsoleViaCommand(DateTime dtLoggedTime, string strLoggedText) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                this.Game.SendRequest("procon.chat.onConsole", dtLoggedTime.ToBinary().ToString(), strLoggedText);
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, new List<string>() { "procon.chat.onConsole", dtLoggedTime.ToBinary().ToString(), strLoggedText }));
            }
        }

        #endregion

        #region Map Zones

        private void MapZones_MapZoneRemoved(PRoCon.Core.Battlemap.MapZoneDrawing item) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                this.Game.SendRequest("procon.battlemap.onZoneRemoved", item.UID);
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, new List<string>() { "procon.battlemap.onZoneRemoved", item.UID }));
            }
        }

        private void MapZones_MapZoneChanged(PRoCon.Core.Battlemap.MapZoneDrawing item) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                List<string> packet = new List<string>() { "procon.battlemap.onZoneModified", item.UID, item.Tags.ToString(), item.ZonePolygon.Length.ToString() };

                packet.AddRange(Point3D.ToStringList(item.ZonePolygon));

                this.Game.SendRequest(packet);
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, packet));
            }
        }

        private void MapZones_MapZoneAdded(PRoCon.Core.Battlemap.MapZoneDrawing item) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                List<string> packet = new List<string>() { "procon.battlemap.onZoneCreated", item.UID, item.LevelFileName, item.ZonePolygon.Length.ToString() };

                packet.AddRange(Point3D.ToStringList(item.ZonePolygon));

                this.Game.SendRequest(packet);
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, packet));
            }
        }

        #endregion

        #region Variables

        private void Variables_VariableUpdated(PRoCon.Core.Variables.Variable item) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                this.Game.SendRequest("procon.vars.onAltered", item.Name, item.Value);
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, new List<string>() { "procon.vars.onAltered", item.Name, item.Value }));
            }
        }

        private void Variables_VariableAdded(PRoCon.Core.Variables.Variable item) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true && this.Game != null) {
                this.Game.SendRequest("procon.vars.onAltered", item.Name, item.Value);
                //this.send(new Packet(true, false, this.AcquireSequenceNumber, new List<string>() { "procon.vars.onAltered", item.Name, item.Value }));
            }
        }

        #endregion

        #region Packages

        private void PackageManager_PackageBeginningDownload(PackageManager sender, Package target) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true) {
                this.Game.SendRequest("procon.packages.onDownloading", target.Uid);
            }
        }

        private void PackageManager_PackageDownloaded(PackageManager sender, Package target) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true) {
                this.Game.SendRequest("procon.packages.onDownloaded", target.Uid);
            }
        }

        private void PackageManager_PackageDownloadFail(PackageManager sender, Package target) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true) {
                this.Game.SendRequest("procon.packages.onDownloadError", target.Uid, target.Error);
            }
        }
        
        private void PackageManager_PackageInstalling(PackageManager sender, Package target) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true) {
                this.Game.SendRequest("procon.packages.onInstalling", target.Uid);
            }
        }

        private void PackageManager_PackageAwaitingRestart(PackageManager sender, Package target) {
            if (this.IsLoggedIn == true && this.m_blEventsEnabled == true) {
                this.Game.SendRequest("procon.packages.onInstalled", target.Uid, Packet.bltos(true));
            }
        }

        #endregion

        private void Game_ConnectionClosed(FrostbiteLayerClient sender) {
            if (this.ClientShutdown != null) {
                FrostbiteConnection.RaiseEvent(this.ClientShutdown.GetInvocationList(), this);
            }
        }

        private void CPRoConLayerClient_LayerClientShutdown(PRoConLayerClient sender) {
            this.UnregisterEvents();
        }

        // TODO: Change to event once this.m_prcClient.Layer has shutdown event..
        public void OnShutdown() {
            if (this.Game != null) {
                this.Game.SendRequest("procon.shutdown");
            }
        }

        public void Shutdown() {
            if (this.Game != null) {
                this.Game.Shutdown();
            }

            
        }
    }
}
