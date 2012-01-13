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
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace PRoCon.Core.Remote {
    using Core.Players;
    using Core.Maps;
    using Core.TextChatModeration;

    public class FrostbiteClient {

        public FrostbiteConnection Connection {
            get;
            private set;
        }

        public bool IsLoggedIn {
            get;
            protected set;
        }

        public string Password {
            get;
            private set;
        }

        public virtual string GameType {
            get {
                return String.Empty;
            }
        }

        public string VersionNumber {
            get;
            protected set;
        }

        public int VersionInteger {
            get;
            protected set;
        }

        public Dictionary<string, string> VersionNumberToFriendlyName {
            get;
            set;
        }

        public string FriendlyVersionNumber {
            get {
                if (this.VersionNumber != null && this.VersionNumberToFriendlyName != null && this.VersionNumberToFriendlyName.ContainsKey(this.VersionNumber) == true) {
                    return this.VersionNumberToFriendlyName[this.VersionNumber];
                }
                else if (this.VersionNumber == null) {
                    return String.Empty;
                }
                else {
                    return this.VersionNumber;
                }
            }
        }

        public double UTCoffset
        {
            get;
            set;
        }

        public virtual bool HasSquads {
            get {
                return true;
            }
        }

        public virtual bool HasOpenMaplist {
            get {
                return false;
            }
        }

        // Commands that simply retreive a list, but do not alter any settings
        public Regex GetPacketsPattern { get; protected set; }

        #region Delegates

        public delegate void EmptyParamterHandler(FrostbiteClient sender);

        public delegate void PasswordHandler(FrostbiteClient sender, string password);

        public delegate void PlayerEventHandler(FrostbiteClient sender, string playerName);
        public delegate void PlayerLeaveHandler(FrostbiteClient sender, string playerName, CPlayerInfo cpiPlayer);
        public delegate void PlayerAuthenticatedHandler(FrostbiteClient sender, string playerName, string playerGuid);
        public delegate void PlayerKickedHandler(FrostbiteClient sender, string strSoldierName, string strReason);
        public delegate void PacketDispatchHandler(FrostbiteClient sender, Packet packetBeforeDispatch, bool isCommandConnection);
        public delegate void PlayerTeamChangeHandler(FrostbiteClient sender, string strSoldierName, int iTeamID, int iSquadID);
        public delegate void PacketDispatchedHandler(FrostbiteClient sender, Packet packetBeforeDispatch, bool isCommandConnection, out bool isProcessed);
        public delegate void PlayerKilledByAdminHandler(FrostbiteClient sender, string soldierName);
        public delegate void PlayerKilledHandler(FrostbiteClient sender, string strKiller, string strVictim, string strDamageType, bool blHeadshot, Point3D pntKiller, Point3D pntVictim);
        public delegate void PlayerMovedByAdminHandler(FrostbiteClient sender, string soldierName, int destinationTeamId, int destinationSquadId, bool forceKilled);
        public delegate void RawChatHandler(FrostbiteClient sender, List<string> rawChat);
        public delegate void GlobalChatHandler(FrostbiteClient sender, string playerName, string message);
        public delegate void TeamChatHandler(FrostbiteClient sender, string playerName, string message, int teamId);
        public delegate void SquadChatHandler(FrostbiteClient sender, string playerName, string message, int teamId, int squadId);
        public delegate void PunkbusterMessageHandler(FrostbiteClient sender, string punkbusterMessage);
        public delegate void LoadingLevelHandler(FrostbiteClient sender, string mapFileName, int roundsPlayed, int roundsTotal);
        public delegate void ResponseErrorHandler(FrostbiteClient sender, Packet originalRequest, string errorMessage);
        public delegate void VersionHandler(FrostbiteClient sender, string serverType, string serverVersion);
        public delegate void HelpHandler(FrostbiteClient sender, List<string> lstCommands);
        public delegate void RunScriptHandler(FrostbiteClient sender, string scriptFileName);
        public delegate void RunScriptErrorHandler(FrostbiteClient sender, string strScriptFileName, int iLineError, string strErrorDescription);
        public delegate void SendPunkBusterMessageHandler(FrostbiteClient sender, string punkbusterMessage);
        public delegate void ServerInfoHandler(FrostbiteClient sender, CServerInfo csiServerInfo);

        public delegate void AuthenticationFailureHandler(FrostbiteClient sender, string strError);

        public delegate void YellingHandler(FrostbiteClient sender, string strMessage, int iMessageDuration, List<string> lstSubsetWords);
        public delegate void SayingHandler(FrostbiteClient sender, string strMessage, List<string> lstSubsetWords);
        public delegate void CurrentLevelHandler(FrostbiteClient sender, string currentLevel);
        public delegate void SupportedMapsHandler(FrostbiteClient sender, string strPlaylist, List<string> lstSupportedMaps);
        public delegate void BannerUrlHandler(FrostbiteClient sender, string url);
        public delegate void PlaylistSetHandler(FrostbiteClient sender, string playlist);
        public delegate void ListPlaylistsHandler(FrostbiteClient sender, List<string> lstPlaylists);

        public delegate void ListPlayersHandler(FrostbiteClient sender, List<CPlayerInfo> lstPlayers, CPlayerSubset cpsSubset);

        public delegate void BanListAddHandler(FrostbiteClient sender, CBanInfo cbiAddedBan);
        public delegate void BanListRemoveHandler(FrostbiteClient sender, CBanInfo cbiRemovedBan);
        public delegate void BanListListHandler(FrostbiteClient sender, int iStartOffset, List<CBanInfo> lstBans);
        public delegate void ReserverdSlotsConfigFileHandler(FrostbiteClient sender, string configFilename);
        public delegate void ReservedSlotsPlayerHandler(FrostbiteClient sender, string strSoldierName);
        public delegate void ReservedSlotsListHandler(FrostbiteClient sender, List<string> soldierNames);
        public delegate void MapListConfigFileHandler(FrostbiteClient sender, string strConfigFilename);
        public delegate void MapListAppendedHandler(FrostbiteClient sender, MaplistEntry mapEntry);
        public delegate void MapListLevelIndexHandler(FrostbiteClient sender, int mapIndex);
        public delegate void MapListMapInsertedHandler(FrostbiteClient sender, int mapIndex, string mapFileName, int rounds);
        public delegate void MapListListedHandler(FrostbiteClient sender, List<MaplistEntry> lstMapList);
        public delegate void IsEnabledHandler(FrostbiteClient sender, bool isEnabled);
        public delegate void LimitHandler(FrostbiteClient sender, int limit);
        public delegate void UpperLowerLimitHandler(FrostbiteClient sender, int upperLimit, int lowerLimit);
        public delegate void ServerDescriptionHandler(FrostbiteClient sender, string serverDescription);
        public delegate void ServerNameHandler(FrostbiteClient sender, string strServerName);
        public delegate void LevelVariableHandler(FrostbiteClient sender, LevelVariable lvRequestedContext);
        public delegate void LevelVariableListHandler(FrostbiteClient sender, LevelVariable lvRequestedContext, List<LevelVariable> lstReturnedValues);
        public delegate void LevelVariableGetHandler(FrostbiteClient sender, LevelVariable lvRequestedContext, LevelVariable lvReturnedValue);
        public delegate void PlayerSpawnedHandler(FrostbiteClient sender, string soldierName, string strKit, List<string> lstWeapons, List<string> lstSpecializations);
        public delegate void RoundOverHandler(FrostbiteClient sender, int iWinningTeamID);
        public delegate void RoundOverPlayersHandler(FrostbiteClient sender, List<CPlayerInfo> lstPlayers);
        public delegate void RoundOverTeamScoresHandler(FrostbiteClient sender, List<TeamScore> lstTeamScores);
        public delegate void EndRoundHandler(FrostbiteClient sender, int iWinningTeamID);
        public delegate void TextChatModerationModeHandler(FrostbiteClient sender, ServerModerationModeType mode);

        public delegate void TextChatModerationListAddPlayerHandler(FrostbiteClient sender, TextChatModerationEntry playerEntry);
        public delegate void TextChatModerationListRemovePlayerHandler(FrostbiteClient sender, TextChatModerationEntry playerEntry);
        public delegate void TextChatModerationListListHandler(FrostbiteClient sender, int startOffset, List<TextChatModerationEntry> textChatModerationList);

        #endregion

        #region Events

        public event EmptyParamterHandler Login;
        public event AuthenticationFailureHandler LoginFailure;
        public event EmptyParamterHandler Logout;
        public event EmptyParamterHandler Quit;

        public event VersionHandler Version;
        public event HelpHandler Help;

        public event IsEnabledHandler EventsEnabled;

        #endregion

        #region Packet Management

        protected delegate void RequestPacketHandler(FrostbiteConnection sender, Packet cpRequestPacket);
        protected delegate void ResponsePacketHandler(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket);
        protected Dictionary<string, ResponsePacketHandler> m_responseDelegates;
        protected Dictionary<string, RequestPacketHandler> m_requestDelegates;

        #endregion
        
        public FrostbiteClient(FrostbiteConnection connection) {
            this.Connection = connection;
            this.Connection.PacketReceived += new FrostbiteConnection.PacketDispatchHandler(Connection_PacketRecieved);
            // Register.

            this.Login += new EmptyParamterHandler(FrostbiteClient_Login);
            this.Version += new VersionHandler(FrostbiteClient_Version);

            this.m_responseDelegates = new Dictionary<string, ResponsePacketHandler>() {

                #region Global/Login

                { "login.plainText", this.DispatchLoginPlainTextResponse },
                { "login.hashed", this.DispatchLoginHashedResponse },
                { "logout", this.DispatchLogoutResponse },
                { "quit", this.DispatchQuitResponse },
                { "version", this.DispatchVersionResponse },
                { "eventsEnabled", this.DispatchEventsEnabledResponse },
                { "help", this.DispatchHelpResponse },

                { "admin.runScript", this.DispatchAdminRunScriptResponse },
                { "punkBuster.pb_sv_command", this.DispatchPunkbusterPbSvCommandResponse },
                { "serverInfo", this.DispatchServerInfoResponse },
                { "admin.say", this.DispatchAdminSayResponse },
                { "admin.yell", this.DispatchAdminYellResponse },
                
                #endregion

                #region Map list functions

                { "admin.restartMap", this.DispatchAdminRestartRoundResponse },
                { "admin.supportedMaps", this.DispatchAdminSupportedMapsResponse },
                { "admin.getPlaylists", this.DispatchAdminGetPlaylistsResponse },
                { "admin.listPlayers", this.DispatchAdminListPlayersResponse },
                { "admin.endRound", this.DispatchAdminEndRoundResponse },
                { "admin.runNextRound", this.DispatchAdminRunNextRoundResponse },
                { "admin.restartRound", this.DispatchAdminRestartRoundResponse },

                #endregion

                #region Banlist

                { "banList.add", this.DispatchBanListAddResponse },
                { "banList.remove", this.DispatchBanListRemoveResponse },
                { "banList.clear", this.DispatchBanListClearResponse },
                { "banList.save", this.DispatchBanListSaveResponse },
                { "banList.load", this.DispatchBanListLoadResponse },
                { "banList.list", this.DispatchBanListListResponse },
                
                #endregion

                #region Text Chat Moderation

                { "textChatModerationList.addPlayer", this.DispatchTextChatModerationAddPlayerResponse },
                { "textChatModerationList.removePlayer", this.DispatchTextChatModerationListRemovePlayerResponse },
                { "textChatModerationList.clear", this.DispatchTextChatModerationListClearResponse },
                { "textChatModerationList.save", this.DispatchTextChatModerationListSaveResponse },
                { "textChatModerationList.load", this.DispatchTextChatModerationListLoadResponse },
                { "textChatModerationList.list", this.DispatchTextChatModerationListListResponse },

                #endregion

                { "vars.textChatModerationMode", this.DispatchVarsTextChatModerationModeResponse },
                { "vars.textChatSpamTriggerCount", this.DispatchVarsTextChatSpamTriggerCountResponse },
                { "vars.textChatSpamDetectionTime", this.DispatchVarsTextChatSpamDetectionTimeResponse },
                { "vars.textChatSpamCoolDownTime", this.DispatchVarsTextChatSpamCoolDownTimeResponse },
                
                #region Maplist

                { "mapList.configFile", this.DispatchMapListConfigFileResponse },
                { "mapList.load", this.DispatchMapListLoadResponse },
                { "mapList.save", this.DispatchMapListSaveResponse },
                { "mapList.list", this.DispatchMapListListResponse },
                { "mapList.clear", this.DispatchMapListClearResponse },
                { "mapList.append", this.DispatchMapListAppendResponse },
                { "mapList.nextLevelIndex", this.DispatchMapListNextLevelIndexResponse },
                { "mapList.remove", this.DispatchMapListRemoveResponse },
                { "mapList.insert", this.DispatchMapListInsertResponse },

                #endregion

                // Details
                { "vars.serverName", this.DispatchVarsServerNameResponse },
                { "vars.serverDescription", this.DispatchVarsServerDescriptionResponse },
                { "vars.bannerUrl", this.DispatchVarsBannerUrlResponse },

                // Configuration
                { "vars.adminPassword", this.DispatchVarsAdminPasswordResponse },
                { "vars.gamePassword", this.DispatchVarsGamePasswordResponse },
                { "vars.punkBuster", this.DispatchVarsPunkbusterResponse },
                { "vars.ranked", this.DispatchVarsRankedResponse },
                { "vars.playerLimit", this.DispatchVarsPlayerLimitResponse },
                { "vars.currentPlayerLimit", this.DispatchVarsCurrentPlayerLimitResponse },
                { "vars.maxPlayerLimit", this.DispatchVarsMaxPlayerLimitResponse },
                { "vars.idleTimeout", this.DispatchVarsIdleTimeoutResponse },
                { "vars.profanityFilter", this.DispatchVarsProfanityFilterResponse },
                
                // Gameplay
                { "vars.friendlyFire", this.DispatchVarsFriendlyFireResponse },
                { "vars.hardCore", this.DispatchVarsHardCoreResponse },

                #region Team killing

                { "vars.teamKillCountForKick", this.DispatchVarsTeamKillCountForKickResponse },
                { "vars.teamKillValueForKick", this.DispatchVarsTeamKillValueForKickResponse },
                { "vars.teamKillValueIncrease", this.DispatchVarsTeamKillValueIncreaseResponse },
                { "vars.teamKillValueDecreasePerSecond", this.DispatchVarsTeamKillValueDecreasePerSecondResponse },

                #endregion

                #region Level vars

                { "levelVars.set", this.DispatchLevelVarsSetResponse },
                { "levelVars.get", this.DispatchLevelVarsGetResponse },
                { "levelVars.evaluate", this.DispatchLevelVarsEvaluateResponse },
                { "levelVars.clear", this.DispatchLevelVarsClearResponse },
                { "levelVars.list", this.DispatchLevelVarsListResponse },

                #endregion

                { "admin.kickPlayer", this.DispatchAdminKickPlayerResponse },
                { "admin.killPlayer", this.DispatchAdminKillPlayerResponse },
                { "admin.movePlayer", this.DispatchAdminMovePlayerResponse },

                { "admin.shutDown", this.DispatchAdminShutDownResponse },
            };

            this.m_requestDelegates = new Dictionary<string, RequestPacketHandler>() {

                { "player.onJoin", this.DispatchPlayerOnJoinRequest },
                { "player.onLeave", this.DispatchPlayerOnLeaveRequest },
                { "player.onAuthenticated", this.DispatchPlayerOnAuthenticatedRequest },
                { "player.onKill", this.DispatchPlayerOnKillRequest },
                { "player.onChat", this.DispatchPlayerOnChatRequest },
                { "player.onKicked", this.DispatchPlayerOnKickedRequest },
                { "player.onTeamChange", this.DispatchPlayerOnTeamChangeRequest },
                { "player.onSquadChange", this.DispatchPlayerOnSquadChangeRequest },
                { "player.onSpawn", this.DispatchPlayerOnSpawnRequest },

                { "server.onLoadingLevel", this.DispatchServerOnLoadingLevelRequest },
                { "server.onLevelStarted", this.DispatchServerOnLevelStartedRequest },
                { "server.onRoundOver", this.DispatchServerOnRoundOverRequest },
                { "server.onRoundOverPlayers", this.DispatchServerOnRoundOverPlayersRequest },
                { "server.onRoundOverTeamScores", this.DispatchServerOnRoundOverTeamScoresRequest },

                { "punkBuster.onMessage", this.DispatchPunkBusterOnMessageRequest },
            };

            this.GetPacketsPattern = new Regex(@"^punkBuster\.pb_sv_command|^version|^help|^serverInfo|^admin\.listPlayers|^listPlayers|^admin\.supportMaps|^admin\.getPlaylists|^admin\.currentLevel|^mapList\.nextLevelIndex|^mapList\.list|^textChatModerationList\.list|^banList\.list|^levelVars\.list|^levelVars\.evaluate|^levelVars\.get|^vars\.[a-zA-Z]*?$");
        }

        #region Initial List

        public virtual void FetchStartupVariables() {

            this.SendServerinfoPacket();
            this.SendVersionPacket();
            this.SendHelpPacket();

            // Lists
            this.SendAdminListPlayersPacket(new CPlayerSubset(CPlayerSubset.PlayerSubsetType.All));
            this.SendReservedSlotsListPacket();
            this.SendMapListListRoundsPacket();
            this.SendTextChatModerationListListPacket();
            this.SendBanListListPacket();

            // Vars
            // Vars - Details
            this.SendGetVarsGamePasswordPacket();
            //this.SendGetVarsAdminPasswordPacket();
            this.SendGetVarsRankedPacket();
            this.SendGetVarsPunkBusterPacket();

            this.SendGetVarsServerNamePacket();
            this.SendGetVarsBannerUrlPacket();
            this.SendGetVarsServerDescriptionPacket();

            // Vars - Gameplay
            this.SendGetVarsHardCorePacket();
            this.SendGetVarsFriendlyFirePacket();

            // Vars - Configuration
            this.SendGetVarsIdleTimeoutPacket();
            this.SendGetVarsProfanityFilterPacket();

            this.SendGetVarsMaxPlayerLimitPacket();
            this.SendGetVarsCurrentPlayerLimitPacket();
            this.SendGetVarsPlayerLimitPacket();

            // Team Kill settings
            this.SendGetVarsTeamKillCountForKickPacket();
            this.SendGetVarsTeamKillValueForKickPacket();
            this.SendGetVarsTeamKillValueIncreasePacket();
            this.SendGetVarsTeamKillValueDecreasePerSecondPacket();

            // Text Chat Moderation
            this.SendGetVarsTextChatModerationModePacket();
            this.SendGetVarsTextChatSpamCoolDownTimePacket();
            this.SendGetVarsTextChatSpamDetectionTimePacket();
            this.SendGetVarsTextChatSpamTriggerCountPacket();

            //this.SendRequest(new List<string>() { "punkBuster.pb_sv_command", "pb_sv_plist" });
        }

        #endregion

        #region Login

        public static string GeneratePasswordHash(byte[] a_bSalt, string strData) {
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

        public virtual void SendLoginHashedPacket(string password) {
            this.Password = password;

            this.Connection.SendQueued(new Packet(false, false, this.Connection.AcquireSequenceNumber, new List<string>() { "login.hashed" }));
        }

        private void FrostbiteClient_Login(object sender) {
            this.SendEventsEnabledPacket(true);
        }

        #endregion

        #region Packet Helpers

        protected void BuildSendPacket(params string[] a_words) {
            this.Connection.SendQueued(new Packet(false, false, this.Connection.AcquireSequenceNumber, new List<string>(a_words)));
        }

        public virtual void SendEventsEnabledPacket(bool isEventsEnabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("eventsEnabled", Packet.bltos(isEventsEnabled));
            }
        }

        public virtual void SendAdminSayPacket(string text, CPlayerSubset subset) {
            if (this.IsLoggedIn == true) {
                if (subset.Subset == CPlayerSubset.PlayerSubsetType.All) {
                    this.BuildSendPacket("admin.say", text, "all");
                }
                else if (subset.Subset == CPlayerSubset.PlayerSubsetType.Team) {
                    this.BuildSendPacket("admin.say", text, "team", subset.TeamID.ToString());
                }
                else if (subset.Subset == CPlayerSubset.PlayerSubsetType.Squad) {
                    this.BuildSendPacket("admin.say", text, "squad", subset.TeamID.ToString(), subset.SquadID.ToString());
                }
                else if (subset.Subset == CPlayerSubset.PlayerSubsetType.Player) {
                    this.BuildSendPacket("admin.say", text, "player", subset.SoldierName);
                }
            }
        }
        /*
        public virtual void SendLevelVarsListPacket(LevelVariableContext requestedContext) {

            if (this.IsLoggedIn == true) {
                if (requestedContext.ContextType == LevelVariableContextType.All) {
                    this.BuildSendPacket("levelVars.list", "all");
                }
                else if (requestedContext.ContextType == LevelVariableContextType.GameMode) {
                    this.BuildSendPacket("levelVars.list", "gamemode", requestedContext.ContextTarget);
                }
                else if (requestedContext.ContextType == LevelVariableContextType.Level) {
                    this.BuildSendPacket("levelVars.list", "level", requestedContext.ContextTarget);
                }
            }
        }
        */

        #region General

        public virtual void SendServerinfoPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("serverInfo");
            }
        }

        public virtual void SendVersionPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("version");
            }
        }

        public virtual void SendHelpPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("help");
            }
        }

        public virtual void SendAdminListPlayersPacket(CPlayerSubset subset) {
            if (this.IsLoggedIn == true) {
                if (subset.Subset == CPlayerSubset.PlayerSubsetType.All) {
                    this.BuildSendPacket("admin.listPlayers", "all");
                }
                else if (subset.Subset == CPlayerSubset.PlayerSubsetType.Player) {
                    this.BuildSendPacket("admin.listPlayers", "player", subset.SoldierName);
                }
                else if (subset.Subset == CPlayerSubset.PlayerSubsetType.Team) {
                    this.BuildSendPacket("admin.listPlayers", "team", subset.TeamID.ToString());
                }
                else if (subset.Subset == CPlayerSubset.PlayerSubsetType.Squad) {
                    this.BuildSendPacket("admin.listPlayers", "squad", subset.TeamID.ToString(), subset.SquadID.ToString());
                }
            }
        }

        #endregion

        #region Map Controls

        public virtual void SendAdminRestartRoundPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("admin.restartRound");
            }
        }

        public virtual void SendAdminRunNextRoundPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("admin.runNextRound");
            }
        }

        #endregion

        #region Maplist

        public virtual void SendMapListListRoundsPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("mapList.list", "rounds");
            }
        }

        public virtual void SendMapListClearPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("mapList.clear");
            }
        }

        public virtual void SendMapListSavePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("mapList.save");
            }
        }

        public virtual void SendMapListAppendPacket(string mapFileName, int rounds) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("mapList.append", mapFileName, rounds.ToString());
            }
        }

        public virtual void SendMapListRemovePacket(int index) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("mapList.remove", index.ToString());
            }
        }

        public virtual void SendMapListInsertPacket(int index, string mapFileName, int rounds) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("mapList.insert", index.ToString(), mapFileName, rounds.ToString());
            }
        }

        public virtual void SendMapListNextLevelIndexPacket(int index) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("mapList.nextLevelIndex", index.ToString());
            }
        }

        public virtual void SendAdminSetPlaylistPacket(string playList) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("admin.setPlaylist", playList);
            }
        }

        

        #endregion

        #region Ban List

        public virtual void SendBanListSavePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("banList.save");
            }
        }

        public virtual void SendBanListListPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("banList.list");
            }
        }

        public virtual void SendBanListClearPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("banList.clear");
            }
        }

        public virtual void SendBanListListPacket(int startIndex) {
            if (this.IsLoggedIn == true) {
                if (startIndex >= 0) {
                    this.BuildSendPacket("banList.list", startIndex.ToString());
                }
                else {
                    this.BuildSendPacket("banList.list");
                }
            }
        }

        #endregion

        #region Text Moderation List

        public virtual void SendTextChatModerationListListPacket() {
            this.SendTextChatModerationListListPacket(0);
        }

        public virtual void SendTextChatModerationListListPacket(int startIndex) {
            if (this.IsLoggedIn == true) {
                if (startIndex >= 0) {
                    this.BuildSendPacket("textChatModerationList.list", startIndex.ToString());
                }
                else {
                    this.BuildSendPacket("textChatModerationList.list");
                }
            }
        }

        public virtual void SendTextChatModerationListAddPacket(TextChatModerationEntry playerEntry) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("textChatModerationList.addPlayer", playerEntry.PlayerModerationLevel.ToString().ToLower(), playerEntry.SoldierName);
            }
        }

        public virtual void SendTextChatModerationListRemovePacket(string soldierName) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("textChatModerationList.removePlayer", soldierName);
            }
        }

        public virtual void SendTextChatModerationListSavePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("textChatModerationList.save");
            }
        }

        #endregion

        #region Level Variables

        public virtual void SendLevelVarsSetPacket(LevelVariableContext context, string variable, string value) {
            if (this.IsLoggedIn == true) {
                if (context.ContextTarget.Length > 0) {
                    this.BuildSendPacket("levelVars.set", context.ContextType.ToString().ToLower(), context.ContextTarget, variable, value);
                }
                else {
                    this.BuildSendPacket("levelVars.set", context.ContextType.ToString().ToLower(), variable, value);
                }
            }
        }

        public virtual void SendLevelVarsListPacket(LevelVariableContext context) {
            if (this.IsLoggedIn == true) {
                if (context.ContextTarget.Length > 0) {
                    this.BuildSendPacket("levelVars.list", context.ContextType.ToString().ToLower(), context.ContextTarget);
                }
                else {
                    this.BuildSendPacket("levelVars.list", context.ContextType.ToString().ToLower());
                }
            }
        }

        public virtual void SendLevelVarsClearPacket(LevelVariableContext context) {
            if (this.IsLoggedIn == true) {
                if (context.ContextTarget.Length > 0) {
                    this.BuildSendPacket("levelVars.clear", context.ContextType.ToString().ToLower(), context.ContextTarget);
                }
                else {
                    this.BuildSendPacket("levelVars.clear", context.ContextType.ToString().ToLower());
                }
            }
        }

        #endregion

        #region Reserved Slot List 

        public virtual void SendReservedSlotsLoadPacket()
        {
            if (this.IsLoggedIn == true)
            {
                this.BuildSendPacket("reservedSlots.load");
            }
        }

        public virtual void SendReservedSlotsListPacket()
        {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("reservedSlots.list");
            }
        }

        public virtual void SendReservedSlotsAddPlayerPacket(string soldierName) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("reservedSlots.addPlayer", soldierName);
            }
        }

        public virtual void SendReservedSlotsRemovePlayerPacket(string soldierName) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("reservedSlots.removePlayer", soldierName);
            }
        }

        public virtual void SendReservedSlotsSavePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("reservedSlots.save");
            }
        }

        #endregion

        #region Vars

        public virtual void SendAdminSupportedMapsPacket(string playList) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("admin.supportedMaps", playList);
            }
        }

        public virtual void SendSetVarsAdminPasswordPacket(string password) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.adminPassword", password);
            }
        }

        public virtual void SendGetVarsAdminPasswordPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.adminPassword");
            }
        }

        public virtual void SendSetVarsGamePasswordPacket(string password) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.gamePassword", password);
            }
        }

        public virtual void SendGetVarsGamePasswordPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.gamePassword");
            }
        }

        public virtual void SendSetVarsPunkBusterPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.punkBuster", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsPunkBusterPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.punkBuster");
            }
        }

        public virtual void SendSetVarsHardCorePacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.hardCore", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsHardCorePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.hardCore");
            }
        }

        public virtual void SendSetVarsRankedPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.ranked", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsRankedPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.ranked");
            }
        }

        public virtual void SendSetVarsFriendlyFirePacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.friendlyFire", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsFriendlyFirePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.friendlyFire");
            }
        }

        public virtual void SendSetVarsPlayerLimitPacket(int limit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.playerLimit", limit.ToString());
            }
        }

        public virtual void SendGetVarsPlayerLimitPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.playerLimit");
            }
        }

        public virtual void SendGetVarsCurrentPlayerLimitPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.currentPlayerLimit");
            }
        }

        public virtual void SendGetVarsMaxPlayerLimitPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.maxPlayerLimit");
            }
        }

        public virtual void SendSetVarsBannerUrlPacket(string url) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.bannerUrl", url);
            }
        }

        public virtual void SendGetVarsBannerUrlPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.bannerUrl");
            }
        }

        public virtual void SendSetVarsServerDescriptionPacket(string description) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.serverDescription", description);
            }
        }

        public virtual void SendGetVarsServerDescriptionPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.serverDescription");
            }
        }

        #region Game Specific

        #region BFBC2

        public virtual void SendSetVarsRankLimitPacket(int limit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.rankLimit", limit.ToString());
            }
        }

        public virtual void SendGetVarsRankLimitPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.rankLimit");
            }
        }

        public virtual void SendSetVarsTeamBalancePacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.teamBalance", Packet.bltos(enabled));
            }
        }


        public virtual void SendGetVarsTeamBalancePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.teamBalance");
            }
        }

        public virtual void SendSetVarsKillCamPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.killCam", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsKillCamPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.killCam");
            }
        }

        public virtual void SendSetVarsMiniMapPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.miniMap", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsMiniMapPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.miniMap");
            }
        }

        public virtual void SendSetVarsCrossHairPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.crossHair", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsCrossHairPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.crossHair");
            }
        }

        public virtual void SendSetVars3dSpottingPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.3dSpotting", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVars3dSpottingPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.3dSpotting");
            }
        }

        public virtual void SendSetVarsMiniMapSpottingPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.miniMapSpotting", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsMiniMapSpottingPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.miniMapSpotting");
            }
        }

        public virtual void SendSetVarsThirdPersonVehicleCamerasPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.thirdPersonVehicleCameras", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsThirdPersonVehicleCamerasPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.thirdPersonVehicleCameras");
            }
        }

        #endregion

        #region MoH

        #region Clan teams

        public virtual void SendSetVarsClanTeamsPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.clanTeams", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsClanTeamsPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.clanTeams");
            }
        }

        #endregion

        #region No Crosshairs

        public virtual void SendSetVarsNoCrosshairsPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.noCrosshairs", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsNoCrosshairsPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.noCrosshairs");
            }
        }

        #endregion

        #region Realistic Health

        public virtual void SendSetVarsRealisticHealthPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.realisticHealth", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsRealisticHealthPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.realisticHealth");
            }
        }

        #endregion

        #region No Unlocks

        public virtual void SendSetVarsNoUnlocksPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.noUnlocks", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsNoUnlocksPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.noUnlocks");
            }
        }

        #endregion

        #region No Ammo Pickups

        public virtual void SendSetVarsNoAmmoPickupsPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.noAmmoPickups", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsNoAmmoPickupsPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.noAmmoPickups");
            }
        }

        #endregion

        #region TDM Score Limit

        public virtual void SendSetVarsTdmScoreCounterMaxScorePacket(int limit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.tdmScoreCounterMaxScore", limit.ToString());
            }
        }

        public virtual void SendGetVarsTdmScoreCounterMaxScorePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.tdmScoreCounterMaxScore");
            }
        }

        #endregion

        #region Preround Limit

        public virtual void SendSetVarsPreRoundLimitPacket(int upperLimit, int lowerLimit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.preRoundLimit", upperLimit.ToString(), lowerLimit.ToString());
            }
        }

        public virtual void SendGetVarsPreRoundLimitPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.preRoundLimit");
            }
        }

        #endregion

        #region Skill Limit

        public virtual void SendSetVarsSkillLimitPacket(int upperLimit, int lowerLimit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.skillLimit", upperLimit.ToString(), lowerLimit.ToString());
            }
        }

        public virtual void SendGetVarsSkillLimitPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.skillLimit");
            }
        }

        #endregion

        #region Preround Timer Enabled

        public virtual void SendSetAdminRoundStartTimerEnabledPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("admin.roundStartTimerEnabled", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetAdminRoundStartTimerEnabledPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("admin.roundStartTimerEnabled");
            }
        }

        #endregion

        #region Preround Timer Delay

        public virtual void SendSetVarsRoundStartTimerDelayPacket(int limit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.roundStartTimerDelay", limit.ToString());
            }
        }

        public virtual void SendGetVarsRoundStartTimerDelayPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.roundStartTimerDelay");
            }
        }

        #endregion

        #region Preround Player Limit

        public virtual void SendSetVarsRoundStartTimerPlayersLimitPacket(int limit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.roundStartTimerPlayersLimit", limit.ToString());
            }
        }

        public virtual void SendGetVarsRoundStartTimerPlayersLimitPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.roundStartTimerPlayersLimit");
            }
        }

        #endregion


        #endregion

        #endregion

        public virtual void SendSetVarsServerNamePacket(string serverName) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.serverName", serverName);
            }
        }

        public virtual void SendGetVarsServerNamePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.serverName");
            }
        }

        public virtual void SendSetVarsIdleTimeoutPacket(int limit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.idleTimeout", limit.ToString());
            }
        }

        public virtual void SendGetVarsIdleTimeoutPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.idleTimeout");
            }
        }

        public virtual void SendSetVarsProfanityFilterPacket(bool enabled) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.profanityFilter", Packet.bltos(enabled));
            }
        }

        public virtual void SendGetVarsProfanityFilterPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.profanityFilter");
            }
        }

        public virtual void SendSetVarsTeamKillCountForKickPacket(int limit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.teamKillCountForKick", limit.ToString());
            }
        }

        public virtual void SendGetVarsTeamKillCountForKickPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.teamKillCountForKick");
            }
        }

        public virtual void SendSetVarsTeamKillValueForKickPacket(int limit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.teamKillValueForKick", limit.ToString());
            }
        }

        public virtual void SendGetVarsTeamKillValueForKickPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.teamKillValueForKick");
            }
        }

        public virtual void SendSetVarsTeamKillValueIncreasePacket(int limit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.teamKillValueIncrease", limit.ToString());
            }
        }

        public virtual void SendGetVarsTeamKillValueIncreasePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.teamKillValueIncrease");
            }
        }

        public virtual void SendSetVarsTeamKillValueDecreasePerSecondPacket(int limit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.teamKillValueDecreasePerSecond", limit.ToString());
            }
        }

        public virtual void SendGetVarsTeamKillValueDecreasePerSecondPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.teamKillValueDecreasePerSecond");
            }
        }

        #region Text Chat Moderation

        public virtual void SendSetVarsTextChatModerationModePacket(ServerModerationModeType mode) {
            if (this.IsLoggedIn == true && mode != ServerModerationModeType.None) {
                this.BuildSendPacket("vars.textChatModerationMode", mode.ToString().ToLower());
            }
        }

        public virtual void SendGetVarsTextChatModerationModePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.textChatModerationMode");
            }
        }

        public virtual void SendSetVarsTextChatSpamTriggerCountPacket(int limit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.textChatSpamTriggerCount", limit.ToString());
            }
        }

        public virtual void SendGetVarsTextChatSpamTriggerCountPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.textChatSpamTriggerCount");
            }
        }

        public virtual void SendSetVarsTextChatSpamDetectionTimePacket(int limit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.textChatSpamDetectionTime", limit.ToString());
            }
        }

        public virtual void SendGetVarsTextChatSpamDetectionTimePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.textChatSpamDetectionTime");
            }
        }

        public virtual void SendSetVarsTextChatSpamCoolDownTimePacket(int limit) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.textChatSpamCoolDownTime", limit.ToString());
            }
        }

        public virtual void SendGetVarsTextChatSpamCoolDownTimePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("vars.textChatSpamCoolDownTime");
            }
        }

        #endregion

        public virtual void SendAdminGetPlaylistPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("admin.getPlaylist");
            }
        }

        public virtual void SendAdminGetPlaylistsPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("admin.getPlaylists");
            }
        }
        
        #endregion

        public virtual void SendAdminMovePlayerPacket(string soldierName, int destinationTeamId, int destinationSquadId, bool forceKill) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("admin.movePlayer", soldierName, destinationTeamId.ToString(), destinationSquadId.ToString(), Packet.bltos(forceKill));
            }
        }

        #endregion

        private void FrostbiteClient_Version(FrostbiteClient sender, string serverType, string serverVersion) {
            this.VersionNumber = serverVersion;

            int versionInteger = 0;
            if (int.TryParse(serverVersion, out versionInteger) == true) {
                this.VersionInteger = versionInteger;
            }

        }

        private void Connection_PacketRecieved(object sender, bool isHandled, Packet packetBeforeDispatch) {

            if (isHandled == false) {

                if (packetBeforeDispatch.OriginatedFromServer == false && packetBeforeDispatch.IsResponse == true) {

                    Packet requestPacket = this.Connection.GetRequestPacket(packetBeforeDispatch);

                    if (requestPacket != null) {
                        this.DispatchResponsePacket((FrostbiteConnection)sender, (Packet)packetBeforeDispatch.Clone(), requestPacket);
                    }
                }
                else if (packetBeforeDispatch.OriginatedFromServer == true && packetBeforeDispatch.IsResponse == false) {
                    this.DispatchRequestPacket((FrostbiteConnection)sender, (Packet)packetBeforeDispatch.Clone());
                }
            }
        }

        #region Response Handlers

        #region Basic Universal Commands

        protected virtual void DispatchLoginPlainTextResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {

            if (cpRequestPacket.Words.Count >= 1) {
                this.IsLoggedIn = true;
                if (this.Login != null) {
                    FrostbiteConnection.RaiseEvent(this.Login.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchLoginHashedResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count == 1 && cpRecievedPacket.Words.Count >= 2) {
                sender.SendQueued(new Packet(false, false, sender.AcquireSequenceNumber, new List<string>() { "login.hashed", FrostbiteClient.GeneratePasswordHash(this.HashToByteArray(cpRecievedPacket.Words[1]), this.Password) }));
            }
            else if (cpRequestPacket.Words.Count >= 2) {

                this.IsLoggedIn = true;

                if (this.Login != null) {
                    FrostbiteConnection.RaiseEvent(this.Login.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchLogoutResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {

                this.IsLoggedIn = false;

                if (this.Logout != null) {
                    FrostbiteConnection.RaiseEvent(this.Logout.GetInvocationList(), this);
                }

                this.Shutdown();
            }
        }

        protected virtual void DispatchQuitResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {

                this.IsLoggedIn = false;

                if (this.Quit != null) {
                    FrostbiteConnection.RaiseEvent(this.Quit.GetInvocationList(), this);
                }

                this.Shutdown();
            }
        }

        protected virtual void DispatchVersionResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRecievedPacket.Words.Count >= 3) {

                if (this.Version != null) {
                    FrostbiteConnection.RaiseEvent(this.Version.GetInvocationList(), this, cpRecievedPacket.Words[1], cpRecievedPacket.Words[2]);
                }
            }
        }

        protected virtual void DispatchEventsEnabledResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.EventsEnabled != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.EventsEnabled.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.EventsEnabled.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchHelpResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRecievedPacket.Words.Count >= 2) {
                List<string> lstWords = cpRecievedPacket.Words;
                lstWords.RemoveAt(0);
                if (this.Help != null) {
                    FrostbiteConnection.RaiseEvent(this.Help.GetInvocationList(), this, lstWords);
                }
            }
        }

        protected virtual void DispatchAdminShutDownResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {

            if (this.ShutdownServer != null) {
                if (cpRequestPacket.Words.Count >= 2) {
                    FrostbiteConnection.RaiseEvent(this.ShutdownServer.GetInvocationList(), this);
                }
            }
        }

        #endregion

        #region General

        protected virtual void DispatchAdminRunScriptResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {
                if (this.RunScript != null) {
                    FrostbiteConnection.RaiseEvent(this.RunScript.GetInvocationList(), this, cpRequestPacket.Words[1]);
                }
            }
        }

        protected virtual void DispatchPunkbusterPbSvCommandResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {
                if (this.SendPunkbusterMessage != null) {
                    FrostbiteConnection.RaiseEvent(this.SendPunkbusterMessage.GetInvocationList(), this, cpRequestPacket.Words[1]);
                }
            }
        }

        protected virtual void DispatchServerInfoResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {

            if (this.ServerInfo != null) {
                CServerInfo newServerInfo = new CServerInfo(
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
                        "Mappack", // will need to be split into MoHClient and BFBC2Client.
                        "ExternalGameIpandPort",
                        "PunkBusterVersion",
                        "JoinQueueEnabled",
                        "ServerRegion"
                    }, cpRecievedPacket.Words.GetRange(1, cpRecievedPacket.Words.Count - 1)
                );

                FrostbiteConnection.RaiseEvent(this.ServerInfo.GetInvocationList(), this, newServerInfo);
            }

            /*
            if (cpRequestPacket.Words.Count >= 1) {

                int iCurrentPlayers = 0, iMaxPlayers = 32, iCurrentRound = 0, iTotalRounds = 0, iTeamScoreScope = 0;
                string sConnectionState = String.Empty;
                List<TeamScore> lstTeamScores;

                // R17
                if (cpRecievedPacket.Words.Count >= 11) {

                    int.TryParse(cpRecievedPacket.Words[2], out iCurrentPlayers);
                    int.TryParse(cpRecievedPacket.Words[3], out iMaxPlayers);
                    int.TryParse(cpRecievedPacket.Words[6], out iCurrentRound);
                    int.TryParse(cpRecievedPacket.Words[7], out iTotalRounds);

                    if (int.TryParse(cpRecievedPacket.Words[8], out iTeamScoreScope) == true) {
                        // include neutral
                        iTeamScoreScope = iTeamScoreScope + 1;
                        lstTeamScores = TeamScore.GetTeamScores(cpRecievedPacket.Words.GetRange(8, iTeamScoreScope));
                    }
                    else {
                        lstTeamScores = TeamScore.GetTeamScores(cpRecievedPacket.Words.GetRange(8, cpRecievedPacket.Words.Count - 8));
                    }
                    if (8 + iTeamScoreScope <= cpRecievedPacket.Words.Count) {
                        sConnectionState = cpRecievedPacket.Words[8 + iTeamScoreScope + 1];
                    }

                    if (this.ServerInfo != null) {
                        FrostbiteConnection.RaiseEvent(this.ServerInfo.GetInvocationList(), this, new CServerInfo(cpRecievedPacket.Words[1], cpRecievedPacket.Words[5], cpRecievedPacket.Words[4], iCurrentPlayers, iMaxPlayers, iCurrentRound, iTotalRounds, lstTeamScores, sConnectionState));
                    }
                }
            }
            */
        }

        protected virtual void DispatchAdminListPlayersResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {

                cpRecievedPacket.Words.RemoveAt(0);
                if (this.ListPlayers != null) {

                    List<CPlayerInfo> lstPlayers = CPlayerInfo.GetPlayerList(cpRecievedPacket.Words);
                    CPlayerSubset cpsSubset = new CPlayerSubset(cpRequestPacket.Words.GetRange(1, cpRequestPacket.Words.Count - 1));

                    FrostbiteConnection.RaiseEvent(this.ListPlayers.GetInvocationList(), this, lstPlayers, cpsSubset);
                }
            }
        }

        protected virtual void DispatchAdminSayResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 3) {
                if (this.Saying != null) {
                    FrostbiteConnection.RaiseEvent(this.Saying.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words.GetRange(2, cpRequestPacket.Words.Count - 2));
                }
            }
        }

        protected virtual void DispatchAdminYellResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 4) {
                if (this.Yelling != null) {
                    FrostbiteConnection.RaiseEvent(this.Yelling.GetInvocationList(), this, cpRequestPacket.Words[1], Convert.ToInt32(cpRequestPacket.Words[2]), cpRequestPacket.Words.GetRange(3, cpRequestPacket.Words.Count - 3));
                }
            }
        }

        #endregion

        #region Map Functions

        protected virtual void DispatchAdminRunNextRoundResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.RunNextRound != null) {
                    FrostbiteConnection.RaiseEvent(this.RunNextRound.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchAdminCurrentLevelResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1 && cpRecievedPacket.Words.Count >= 2) {
                if (this.CurrentLevel != null) {
                    FrostbiteConnection.RaiseEvent(this.CurrentLevel.GetInvocationList(), this, cpRecievedPacket.Words[1]);
                }
            }
        }

        protected virtual void DispatchAdminRestartRoundResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.RestartRound != null) {
                    FrostbiteConnection.RaiseEvent(this.RestartRound.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchAdminSupportedMapsResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2 && cpRecievedPacket.Words.Count > 1) {
                if (this.SupportedMaps != null) {
                    FrostbiteConnection.RaiseEvent(this.SupportedMaps.GetInvocationList(), this, cpRequestPacket.Words[1], cpRecievedPacket.Words.GetRange(1, cpRecievedPacket.Words.Count - 1));
                }
            }
        }
        
        protected virtual void DispatchAdminGetPlaylistsResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {

                cpRecievedPacket.Words.RemoveAt(0);
                if (this.ListPlaylists != null) {
                    FrostbiteConnection.RaiseEvent(this.ListPlaylists.GetInvocationList(), this, cpRecievedPacket.Words);
                }
            }
        }

        protected virtual void DispatchAdminEndRoundResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {
                if (this.EndRound != null) {
                    FrostbiteConnection.RaiseEvent(this.EndRound.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                }
            }
        }

        #endregion

        #region Text Chat Moderation

        protected virtual void DispatchTextChatModerationAddPlayerResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 3) {
                if (this.TextChatModerationListAddPlayer != null) {
                    FrostbiteConnection.RaiseEvent(this.TextChatModerationListAddPlayer.GetInvocationList(), this, new TextChatModerationEntry(TextChatModerationEntry.GetPlayerModerationLevelType(cpRequestPacket.Words[1]), cpRequestPacket.Words[2]));
                }
            }
        }

        protected virtual void DispatchTextChatModerationListRemovePlayerResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {
                if (this.TextChatModerationListRemovePlayer != null) {
                    FrostbiteConnection.RaiseEvent(this.TextChatModerationListRemovePlayer.GetInvocationList(), this, new TextChatModerationEntry( PlayerModerationLevelType.None, cpRequestPacket.Words[1]));
                }
            }
        }

        protected virtual void DispatchTextChatModerationListClearResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.TextChatModerationListClear != null) {
                    FrostbiteConnection.RaiseEvent(this.TextChatModerationListClear.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchTextChatModerationListSaveResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.TextChatModerationListSave != null) {
                    FrostbiteConnection.RaiseEvent(this.TextChatModerationListSave.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchTextChatModerationListLoadResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.TextChatModerationListLoad != null) {
                    FrostbiteConnection.RaiseEvent(this.TextChatModerationListLoad.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchTextChatModerationListListResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {

                int requestStartOffset = 0;

                if (cpRequestPacket.Words.Count >= 2) {
                    if (int.TryParse(cpRequestPacket.Words[1], out requestStartOffset) == false) {
                        requestStartOffset = 0;
                    }
                }

                if (this.TextChatModerationListList != null) {
                    cpRecievedPacket.Words.RemoveRange(0, 2);

                    List<TextChatModerationEntry> moderationList = new List<TextChatModerationEntry>();

                    for (int i = 0; i + 2 <= cpRecievedPacket.Words.Count; ) {
                        moderationList.Add(new TextChatModerationEntry(cpRecievedPacket.Words[i++], cpRecievedPacket.Words[i++]));
                    }

                    FrostbiteConnection.RaiseEvent(this.TextChatModerationListList.GetInvocationList(), this, requestStartOffset, moderationList);
                }
            }
        }

        #endregion

        #region Ban List

        protected virtual void DispatchBanListAddResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 4) {
                // banList.add <id-type: id-type> <id: string> <timeout: timeout> <reason: string>
                if (this.BanListAdd != null) {
                    FrostbiteConnection.RaiseEvent(this.BanListAdd.GetInvocationList(), this, new CBanInfo(cpRequestPacket.Words[1], cpRequestPacket.Words[2], new TimeoutSubset(cpRequestPacket.Words.GetRange(3, TimeoutSubset.RequiredLength(cpRequestPacket.Words[3]))), cpRequestPacket.Words.Count >= (4 + TimeoutSubset.RequiredLength(cpRequestPacket.Words[3])) ? cpRequestPacket.Words[(3 + TimeoutSubset.RequiredLength(cpRequestPacket.Words[3]))] : ""));
                }
            }
        }

        protected virtual void DispatchBanListRemoveResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 3) {
                if (this.BanListRemove != null) {
                    FrostbiteConnection.RaiseEvent(this.BanListRemove.GetInvocationList(), this, new CBanInfo(cpRequestPacket.Words[1], cpRequestPacket.Words[2]));
                }
            }
        }

        protected virtual void DispatchBanListClearResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.BanListClear != null) {
                    FrostbiteConnection.RaiseEvent(this.BanListClear.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchBanListSaveResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.BanListSave != null) {
                    FrostbiteConnection.RaiseEvent(this.BanListSave.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchBanListLoadResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.BanListLoad != null) {
                    FrostbiteConnection.RaiseEvent(this.BanListLoad.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchBanListListResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {

                int iRequestStartOffset = 0;

                if (cpRequestPacket.Words.Count >= 2) {
                    if (int.TryParse(cpRequestPacket.Words[1], out iRequestStartOffset) == false) {
                        iRequestStartOffset = 0;
                    }
                }

                if (this.BanListList != null) {
                    cpRecievedPacket.Words.RemoveAt(0);
                    FrostbiteConnection.RaiseEvent(this.BanListList.GetInvocationList(), this, iRequestStartOffset, CBanInfo.GetVanillaBanlist(cpRecievedPacket.Words));
                }
            }
        }

        #endregion

        #region Reserved Slots

        protected virtual void DispatchReservedSlotsConfigFileResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {

                if (this.ReservedSlotsConfigFile != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.ReservedSlotsConfigFile.GetInvocationList(), this, cpRecievedPacket.Words[1]);
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.ReservedSlotsConfigFile.GetInvocationList(), this, cpRequestPacket.Words[1]);
                    }
                }
            }
        }

        protected virtual void DispatchReservedSlotsLoadResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.ReservedSlotsLoad != null) {
                    FrostbiteConnection.RaiseEvent(this.ReservedSlotsLoad.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchReservedSlotsSaveResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.ReservedSlotsSave != null) {
                    FrostbiteConnection.RaiseEvent(this.ReservedSlotsSave.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchReservedSlotsAddPlayerResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {
                if (this.ReservedSlotsPlayerAdded != null) {
                    FrostbiteConnection.RaiseEvent(this.ReservedSlotsPlayerAdded.GetInvocationList(), this, cpRequestPacket.Words[1]);
                }
            }
        }

        protected virtual void DispatchReservedSlotsRemovePlayerResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {
                if (this.ReservedSlotsPlayerRemoved != null) {
                    FrostbiteConnection.RaiseEvent(this.ReservedSlotsPlayerRemoved.GetInvocationList(), this, cpRequestPacket.Words[1]);
                }
            }
        }

        protected virtual void DispatchReservedSlotsClearResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.ReservedSlotsCleared != null) {
                    FrostbiteConnection.RaiseEvent(this.ReservedSlotsCleared.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchReservedSlotsListResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count == 1) {
                cpRecievedPacket.Words.RemoveAt(0);
                if (this.ReservedSlotsList != null) {
                    FrostbiteConnection.RaiseEvent(this.ReservedSlotsList.GetInvocationList(), this, cpRecievedPacket.Words);
                }
            }
        }

        #endregion

        #region Map List

        protected virtual void DispatchMapListConfigFileResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {

                if (this.MapListConfigFile != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.MapListConfigFile.GetInvocationList(), this, cpRecievedPacket.Words[1]);
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.MapListConfigFile.GetInvocationList(), this, cpRequestPacket.Words[1]);
                    }
                }
            }
        }

        protected virtual void DispatchMapListLoadResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.MapListLoad != null) {
                    FrostbiteConnection.RaiseEvent(this.MapListLoad.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchMapListSaveResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.MapListSave != null) {
                    FrostbiteConnection.RaiseEvent(this.MapListSave.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchMapListListResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {

                cpRecievedPacket.Words.RemoveAt(0);

                List<MaplistEntry> lstMaplist = new List<MaplistEntry>();

                if (cpRequestPacket.Words.Count == 1) {
                    for (int i = 0; i < cpRecievedPacket.Words.Count; i++) {
                        lstMaplist.Add(new MaplistEntry(cpRecievedPacket.Words[i]));
                    }
                }
                else if (cpRequestPacket.Words.Count >= 2 && String.Compare(cpRequestPacket.Words[1], "rounds", true) == 0) {
                    int iRounds = 0;

                    for (int i = 0; i + 1 < cpRecievedPacket.Words.Count; i = i + 2) {
                        if (int.TryParse(cpRecievedPacket.Words[i + 1], out iRounds) == true) {
                            lstMaplist.Add(new MaplistEntry(cpRecievedPacket.Words[i], iRounds));
                        }
                    }
                }

                if (this.MapListListed != null) {
                    FrostbiteConnection.RaiseEvent(this.MapListListed.GetInvocationList(), this, lstMaplist);
                }
            }
        }

        protected virtual void DispatchMapListClearResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.MapListCleared != null) {
                    FrostbiteConnection.RaiseEvent(this.MapListCleared.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchMapListAppendResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {

                MaplistEntry mapEntry = null;

                int iRounds = 0;

                if (cpRequestPacket.Words.Count == 2) {
                    mapEntry = new MaplistEntry(cpRequestPacket.Words[1]);
                }
                else if (cpRequestPacket.Words.Count >= 3 && int.TryParse(cpRequestPacket.Words[2], out iRounds) == true) {
                    mapEntry = new MaplistEntry(cpRequestPacket.Words[1], iRounds);
                }

                if (this.MapListMapAppended != null) {
                    FrostbiteConnection.RaiseEvent(this.MapListMapAppended.GetInvocationList(), this, mapEntry);
                }
            }
        }

        protected virtual void DispatchMapListNextLevelIndexResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {

                int iMapIndex = 0;
                if (this.MapListNextLevelIndex != null) {
                    if ((cpRequestPacket.Words.Count >= 2 && int.TryParse(cpRequestPacket.Words[1], out iMapIndex) == true) || cpRecievedPacket.Words.Count >= 2 && int.TryParse(cpRecievedPacket.Words[1], out iMapIndex) == true) {
                        FrostbiteConnection.RaiseEvent(this.MapListNextLevelIndex.GetInvocationList(), this, iMapIndex);
                    }
                }
            }
        }

        protected virtual void DispatchMapListRemoveResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {

                int iMapIndex = 0;
                if (int.TryParse(cpRequestPacket.Words[1], out iMapIndex) == true) {
                    if (this.MapListMapRemoved != null) {
                        FrostbiteConnection.RaiseEvent(this.MapListMapRemoved.GetInvocationList(), this, iMapIndex);
                    }
                }
            }
        }

        protected virtual void DispatchMapListInsertResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 4) {

                int iMapIndex = 0, iRounds = 0;
                if (int.TryParse(cpRequestPacket.Words[1], out iMapIndex) == true && int.TryParse(cpRequestPacket.Words[3], out iRounds) == true) {
                    if (this.MapListMapInserted != null) {
                        FrostbiteConnection.RaiseEvent(this.MapListMapInserted.GetInvocationList(), this, iMapIndex, cpRequestPacket.Words[2], iRounds);
                    }
                }
            }
        }

        #endregion

        #region Vars

        protected virtual void DispatchVarsAdminPasswordResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {

                if (this.AdminPassword != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.AdminPassword.GetInvocationList(), this, cpRecievedPacket.Words[1]);
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.AdminPassword.GetInvocationList(), this, cpRequestPacket.Words[1]);
                    }
                }
            }
        }

        protected virtual void DispatchVarsGamePasswordResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.GamePassword != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.GamePassword.GetInvocationList(), this, cpRecievedPacket.Words[1]);
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.GamePassword.GetInvocationList(), this, cpRequestPacket.Words[1]);
                    }
                }
            }
        }

        protected virtual void DispatchVarsPunkbusterResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.Punkbuster != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.Punkbuster.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.Punkbuster.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsHardCoreResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.Hardcore != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.Hardcore.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.Hardcore.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsRankedResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.Ranked != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.Ranked.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.Ranked.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsFriendlyFireResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.FriendlyFire != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.FriendlyFire.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.FriendlyFire.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsPlayerLimitResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.PlayerLimit != null) {
                    if (cpRecievedPacket.Words.Count == 2 && cpRecievedPacket.Words[1].Length <= 3) {
                        FrostbiteConnection.RaiseEvent(this.PlayerLimit.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2 && cpRequestPacket.Words[1].Length <= 3) {
                        FrostbiteConnection.RaiseEvent(this.PlayerLimit.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsCurrentPlayerLimitResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (cpRecievedPacket.Words.Count == 2 && this.CurrentPlayerLimit != null) {
                    FrostbiteConnection.RaiseEvent(this.CurrentPlayerLimit.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                }
            }
        }

        protected virtual void DispatchVarsMaxPlayerLimitResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (cpRecievedPacket.Words.Count == 2 && this.MaxPlayerLimit != null) {
                    FrostbiteConnection.RaiseEvent(this.MaxPlayerLimit.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                }
            }
        }

        protected virtual void DispatchVarsBannerUrlResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.BannerUrl != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.BannerUrl.GetInvocationList(), this, cpRecievedPacket.Words[1]);
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.BannerUrl.GetInvocationList(), this, cpRequestPacket.Words[1]);
                    }
                }
            }
        }

        protected virtual void DispatchVarsServerDescriptionResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.ServerDescription != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.ServerDescription.GetInvocationList(), this, cpRecievedPacket.Words[1]);
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.ServerDescription.GetInvocationList(), this, cpRequestPacket.Words[1]);
                    }
                }
            }
        }

        protected virtual void DispatchVarsTeamKillCountForKickResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.TeamKillCountForKick != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.TeamKillCountForKick.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.TeamKillCountForKick.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsTeamKillValueForKickResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.TeamKillValueForKick != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.TeamKillValueForKick.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.TeamKillValueForKick.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsTeamKillValueIncreaseResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.TeamKillValueIncrease != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.TeamKillValueIncrease.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.TeamKillValueIncrease.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsTeamKillValueDecreasePerSecondResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.TeamKillValueDecreasePerSecond != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.TeamKillValueDecreasePerSecond.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.TeamKillValueDecreasePerSecond.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsIdleTimeoutResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.IdleTimeout != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.IdleTimeout.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.IdleTimeout.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsProfanityFilterResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.ProfanityFilter != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.ProfanityFilter.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.ProfanityFilter.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsServerNameResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.ServerName != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.ServerName.GetInvocationList(), this, cpRecievedPacket.Words[1]);
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.ServerName.GetInvocationList(), this, cpRequestPacket.Words[1]);
                    }
                }
            }
        }

        protected virtual void DispatchVarsTextChatSpamTriggerCountResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.TextChatSpamTriggerCount != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.TextChatSpamTriggerCount.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.TextChatSpamTriggerCount.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsTextChatSpamDetectionTimeResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.TextChatSpamDetectionTime != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.TextChatSpamDetectionTime.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.TextChatSpamDetectionTime.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsTextChatSpamCoolDownTimeResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.TextChatSpamCoolDownTime != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.TextChatSpamCoolDownTime.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.TextChatSpamCoolDownTime.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsTextChatModerationModeResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.TextChatModerationMode != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.TextChatModerationMode.GetInvocationList(), this, TextChatModerationEntry.GetServerModerationLevelType(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.TextChatModerationMode.GetInvocationList(), this, TextChatModerationEntry.GetServerModerationLevelType(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        #endregion

        #region Level Variables

        protected virtual void DispatchLevelVarsSetResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {

                if (this.LevelVariablesSet != null) {
                    FrostbiteConnection.RaiseEvent(this.LevelVariablesSet.GetInvocationList(), this, LevelVariable.ExtractContextVariable(false, cpRequestPacket.Words.GetRange(1, cpRequestPacket.Words.Count - 1)));
                }
            }
        }

        protected virtual void DispatchLevelVarsGetResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRecievedPacket.Words.Count >= 2 && cpRequestPacket.Words.Count >= 2) {
                LevelVariable request = LevelVariable.ExtractContextVariable(false, cpRequestPacket.Words.GetRange(1, cpRequestPacket.Words.Count - 1));

                request.RawValue = cpRecievedPacket.Words[1];

                if (this.LevelVariablesGet != null) {
                    FrostbiteConnection.RaiseEvent(this.LevelVariablesGet.GetInvocationList(), this, request);
                }
            }
        }

        protected virtual void DispatchLevelVarsEvaluateResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRecievedPacket.Words.Count >= 2 && cpRequestPacket.Words.Count >= 2) {

                LevelVariable request = new LevelVariable(new LevelVariableContext(String.Empty, String.Empty), cpRequestPacket.Words[1], cpRecievedPacket.Words[1]);

                if (this.LevelVariablesEvaluate != null) {
                    FrostbiteConnection.RaiseEvent(this.LevelVariablesEvaluate.GetInvocationList(), this, request);
                }
            }
        }

        protected virtual void DispatchLevelVarsClearResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {

                LevelVariable request = LevelVariable.ExtractContextVariable(false, cpRequestPacket.Words.GetRange(1, cpRequestPacket.Words.Count - 1));

                if (this.LevelVariablesClear != null) {
                    FrostbiteConnection.RaiseEvent(this.LevelVariablesClear.GetInvocationList(), this, request);
                }
            }
        }

        protected virtual void DispatchLevelVarsListResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRecievedPacket.Words.Count >= 2 && cpRequestPacket.Words.Count >= 2) {

                LevelVariable varRequestContext = LevelVariable.ExtractContextVariable(false, cpRequestPacket.Words.GetRange(1, cpRequestPacket.Words.Count - 1));

                List<LevelVariable> lstVariables = new List<LevelVariable>();

                int iMatchedVariables = 0;

                if (int.TryParse(cpRecievedPacket.Words[1], out iMatchedVariables) == true) {

                    for (int i = 0; i < iMatchedVariables && ((i + 1) * 4 + 2) <= cpRecievedPacket.Words.Count; i++) {

                        lstVariables.Add(LevelVariable.ExtractContextVariable(true, cpRecievedPacket.Words.GetRange(i * 4 + 2, 4)));
                    }

                    if (this.LevelVariablesList != null) {
                        FrostbiteConnection.RaiseEvent(this.LevelVariablesList.GetInvocationList(), this, varRequestContext, lstVariables);
                    }
                }
            }
        }

        #endregion

        #region Player Events

        protected virtual void DispatchAdminKickPlayerResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (this.PlayerKickedByAdmin != null) {
                if (cpRequestPacket.Words.Count >= 2) {
                    FrostbiteConnection.RaiseEvent(this.PlayerKickedByAdmin.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2]);
                }
                else if (cpRequestPacket.Words.Count >= 2) {
                    FrostbiteConnection.RaiseEvent(this.PlayerKickedByAdmin.GetInvocationList(), this, cpRequestPacket.Words[1], String.Empty);
                }
            }
        }

        protected virtual void DispatchAdminMovePlayerResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {

            if (this.PlayerMovedByAdmin != null) {

                int desinationTeamId, destinationSquadId;
                bool forceKilled;

                if (cpRequestPacket.Words.Count >= 5) {

                    if (int.TryParse(cpRequestPacket.Words[2], out desinationTeamId) == true &&
                        int.TryParse(cpRequestPacket.Words[3], out destinationSquadId) == true &&
                        bool.TryParse(cpRequestPacket.Words[4], out forceKilled) == true) {

                        FrostbiteConnection.RaiseEvent(this.PlayerMovedByAdmin.GetInvocationList(), this, cpRequestPacket.Words[1], desinationTeamId, destinationSquadId, forceKilled);
                    }
                }
            }
        }

        protected virtual void DispatchAdminKillPlayerResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {

            if (this.PlayerKilledByAdmin != null) {
                if (cpRequestPacket.Words.Count >= 2) {
                    FrostbiteConnection.RaiseEvent(this.PlayerKilledByAdmin.GetInvocationList(), this, cpRequestPacket.Words[1]);
                }
            }
        }

        #endregion

        // Command
        public virtual void DispatchResponsePacket(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {

            if (cpRecievedPacket.Words.Count >= 1 && String.Compare(cpRecievedPacket.Words[0], "OK", true) == 0) {

                if (this.m_responseDelegates.ContainsKey(cpRequestPacket.Words[0]) == true) {
                    this.m_responseDelegates[cpRequestPacket.Words[0]](sender, cpRecievedPacket, cpRequestPacket);
                }

            }
            else if (cpRecievedPacket.Words.Count >= 1 && (String.Compare(cpRecievedPacket.Words[0], "InvalidPassword", true) == 0 || String.Compare(cpRecievedPacket.Words[0], "InvalidPasswordHash", true) == 0) && cpRequestPacket.Words.Count >= 1 && (String.Compare(cpRequestPacket.Words[0], "login.hashed", true) == 0 || String.Compare(cpRequestPacket.Words[0], "login.plaintext", true) == 0)) {
                this.IsLoggedIn = false;

                if (this.LoginFailure != null) {
                    FrostbiteConnection.RaiseEvent(this.LoginFailure.GetInvocationList(), this, cpRecievedPacket.Words[0]);
                }

                this.Shutdown();
            }
            else if (cpRecievedPacket.Words.Count >= 3 && String.Compare(cpRecievedPacket.Words[0], "ScriptError", true) == 0 && cpRequestPacket.Words.Count >= 2) {
                if (this.RunScriptError != null) {
                    FrostbiteConnection.RaiseEvent(this.RunScriptError.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]), cpRecievedPacket.Words[2]);
                }
            }
            // Else it is an error..
            else {
                // InvalidArguments
                // TooLongMessage
                // InvalidDuration
                // InvalidFileName
                // InvalidLevelName
                // More...
                if (this.ResponseError != null) {
                    FrostbiteConnection.RaiseEvent(this.ResponseError.GetInvocationList(), this, cpRequestPacket, cpRecievedPacket.Words[0]);
                }
            }
        }

        #endregion

        #region Request Handlers

        #region Player Initiated Events

        protected virtual void DispatchPlayerOnJoinRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {

                if (this.PlayerJoin != null && cpRequestPacket.Words[1].Length > 0) {
                    FrostbiteConnection.RaiseEvent(this.PlayerJoin.GetInvocationList(), this, cpRequestPacket.Words[1]);
                }
            }
        }

        protected virtual void DispatchPlayerOnLeaveRequest(FrostbiteConnection sender, Packet cpRequestPacket) {

            // Backwards compatability
            //else if (cpRequestPacket.Words.Count == 2 && String.Compare(cpRequestPacket.Words[0], "player.onLeave", true) == 0) {
            //    if (this.PlayerLeft != null) {
            //        FrostbiteConnection.RaiseEvent(this.PlayerLeft.GetInvocationList(), this, cpRequestPacket.Words[1], null);
            //    }
            //}
            if (cpRequestPacket.Words.Count >= 3) {
                if (this.PlayerLeft != null) {
                    CPlayerInfo cpiPlayer = null;

                    List<CPlayerInfo> lstPlayers = CPlayerInfo.GetPlayerList(cpRequestPacket.Words.GetRange(2, cpRequestPacket.Words.Count - 2));

                    if (lstPlayers.Count > 0) {
                        cpiPlayer = lstPlayers[0];
                    }

                    FrostbiteConnection.RaiseEvent(this.PlayerLeft.GetInvocationList(), this, cpRequestPacket.Words[1], cpiPlayer);
                }
            }
        }

        protected virtual void DispatchPlayerOnAuthenticatedRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 3) {
                if (this.PlayerAuthenticated != null) {
                    FrostbiteConnection.RaiseEvent(this.PlayerAuthenticated.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2]);
                }
            }
        }

        protected virtual void DispatchPlayerOnKillRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            // Backwards compatability.
            //else if (cpRequestPacket.Words.Count == 3 && String.Compare(cpRequestPacket.Words[0], "player.onKill", true) == 0) {

            //    if (this.PlayerKilled != null) {
            //        FrostbiteConnection.RaiseEvent(this.PlayerKilled.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2], "", false, new Point3D(0, 0, 0), new Point3D(0, 0, 0));
            //    }
            //}
            if (cpRequestPacket.Words.Count >= 11) {

                bool blHeadshot = false;
                if (this.PlayerKilled != null) {

                    if (bool.TryParse(cpRequestPacket.Words[4], out blHeadshot) == true) {
                        FrostbiteConnection.RaiseEvent(this.PlayerKilled.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2], cpRequestPacket.Words[3], blHeadshot, new Point3D(cpRequestPacket.Words[5], cpRequestPacket.Words[7], cpRequestPacket.Words[6]), new Point3D(cpRequestPacket.Words[8], cpRequestPacket.Words[10], cpRequestPacket.Words[9]));
                    }
                }
            }
        }

        protected virtual void DispatchPlayerOnChatRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 3) {
                int iTeamID = 0, iSquadID = 0;

                if (cpRequestPacket.Words.Count == 3) { // < R9 Support.
                    if (this.GlobalChat != null) {
                        FrostbiteConnection.RaiseEvent(this.GlobalChat.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2]);
                    }
                    if (this.TeamChat != null) {
                        FrostbiteConnection.RaiseEvent(this.TeamChat.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2], 0);
                    }
                    if (this.SquadChat != null) {
                        FrostbiteConnection.RaiseEvent(this.SquadChat.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2], 0, 0);
                    }
                }
                else if (cpRequestPacket.Words.Count == 4 && String.Compare(cpRequestPacket.Words[3], "all", true) == 0) {
                    if (this.GlobalChat != null) {
                        FrostbiteConnection.RaiseEvent(this.GlobalChat.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2]);
                    }
                }
                else if (cpRequestPacket.Words.Count >= 5 && String.Compare(cpRequestPacket.Words[3], "team", true) == 0 && int.TryParse(cpRequestPacket.Words[4], out iTeamID) == true) {
                    if (this.TeamChat != null) {
                        FrostbiteConnection.RaiseEvent(this.TeamChat.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2], iTeamID);
                    }
                }
                else if (cpRequestPacket.Words.Count >= 6 && String.Compare(cpRequestPacket.Words[3], "squad", true) == 0 && int.TryParse(cpRequestPacket.Words[4], out iTeamID) == true && int.TryParse(cpRequestPacket.Words[5], out iSquadID) == true) {
                    if (this.SquadChat != null) {
                        FrostbiteConnection.RaiseEvent(this.SquadChat.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2], iTeamID, iSquadID);
                    }
                }

                cpRequestPacket.Words.RemoveAt(0);
                if (this.Chat != null) {
                    FrostbiteConnection.RaiseEvent(this.Chat.GetInvocationList(), this, cpRequestPacket.Words);
                }
            }
        }

        protected virtual void DispatchPlayerOnKickedRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 3) {
                if (this.PlayerKicked != null) {
                    FrostbiteConnection.RaiseEvent(this.PlayerKicked.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2]);
                }
            }
        }

        protected virtual void DispatchPlayerOnTeamChangeRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 4) {
                int iTeamID = 0, iSquadID = 0;

                // TO DO: Specs say TeamId and SquadId which is a little odd..
                if (int.TryParse(cpRequestPacket.Words[2], out iTeamID) == true && int.TryParse(cpRequestPacket.Words[3], out iSquadID) == true) {
                    if (this.PlayerChangedTeam != null) {
                        FrostbiteConnection.RaiseEvent(this.PlayerChangedTeam.GetInvocationList(), this, cpRequestPacket.Words[1], iTeamID, iSquadID);
                    }
                }
            }
        }

        protected virtual void DispatchPlayerOnSquadChangeRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 4) {
                int iTeamID = 0, iSquadID = 0;

                if (int.TryParse(cpRequestPacket.Words[2], out iTeamID) == true && int.TryParse(cpRequestPacket.Words[3], out iSquadID) == true) {
                    if (this.PlayerChangedSquad != null) {
                        FrostbiteConnection.RaiseEvent(this.PlayerChangedSquad.GetInvocationList(), this, cpRequestPacket.Words[1], iTeamID, iSquadID);
                    }
                }
            }
        }

        protected virtual void DispatchPlayerOnSpawnRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 9) {
                if (this.PlayerSpawned != null) {
                    FrostbiteConnection.RaiseEvent(this.PlayerSpawned.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2], cpRequestPacket.Words.GetRange(3, 3), cpRequestPacket.Words.GetRange(6, 3)); // new Inventory(cpRequestPacket.Words[3], cpRequestPacket.Words[4], cpRequestPacket.Words[5], cpRequestPacket.Words[6], cpRequestPacket.Words[7], cpRequestPacket.Words[8]));
                }
            }
        }

        #endregion

        #region Server Initiated Events

        protected virtual void DispatchServerOnLoadingLevelRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 4) {
                if (this.LoadingLevel != null) {
                    int iRoundsPlayed = 0, iRoundsTotal = 0;

                    if (int.TryParse(cpRequestPacket.Words[2], out iRoundsPlayed) == true && int.TryParse(cpRequestPacket.Words[3], out iRoundsTotal) == true) {
                        FrostbiteConnection.RaiseEvent(this.LoadingLevel.GetInvocationList(), this, cpRequestPacket.Words[1], iRoundsPlayed, iRoundsTotal);
                    }
                }
            }
        }

        protected virtual void DispatchServerOnLevelStartedRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.LevelStarted != null) {
                    FrostbiteConnection.RaiseEvent(this.LevelStarted.GetInvocationList(), this);
                }
            }
        }

        protected virtual void DispatchServerOnRoundOverRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {
                int iTeamID = 0;

                if (int.TryParse(cpRequestPacket.Words[1], out iTeamID) == true) {
                    if (this.RoundOver != null) {
                        FrostbiteConnection.RaiseEvent(this.RoundOver.GetInvocationList(), this, iTeamID);
                    }
                }
            }
        }

        protected virtual void DispatchServerOnRoundOverPlayersRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {

                cpRequestPacket.Words.RemoveAt(0);
                if (this.RoundOverPlayers != null) {
                    FrostbiteConnection.RaiseEvent(this.RoundOverPlayers.GetInvocationList(), this, CPlayerInfo.GetPlayerList(cpRequestPacket.Words));
                }

            }
        }

        protected virtual void DispatchServerOnRoundOverTeamScoresRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {

                cpRequestPacket.Words.RemoveAt(0);
                if (this.RoundOverTeamScores != null) {
                    FrostbiteConnection.RaiseEvent(this.RoundOverTeamScores.GetInvocationList(), this, TeamScore.GetTeamScores(cpRequestPacket.Words));
                }

            }
        }

        #endregion

        #region Punkbuster Initiated Events

        protected virtual void DispatchPunkBusterOnMessageRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {
                if (this.PunkbusterMessage != null) {
                    FrostbiteConnection.RaiseEvent(this.PunkbusterMessage.GetInvocationList(), this, cpRequestPacket.Words[1]);
                }
            }
        }

        #endregion

        // Events
        public virtual void DispatchRequestPacket(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {

                if (this.m_requestDelegates.ContainsKey(cpRequestPacket.Words[0]) == true) {
                    this.m_requestDelegates[cpRequestPacket.Words[0]](sender, cpRequestPacket);
                }

            }
        }

        #endregion

        #region Events

        #region Server Request

        #region Player

        public virtual event FrostbiteClient.PlayerEventHandler PlayerJoin;
        public virtual event FrostbiteClient.PlayerLeaveHandler PlayerLeft;
        public virtual event FrostbiteClient.PlayerAuthenticatedHandler PlayerAuthenticated;
        public virtual event FrostbiteClient.PlayerKickedHandler PlayerKicked;
        public virtual event FrostbiteClient.PlayerTeamChangeHandler PlayerChangedTeam;
        public virtual event FrostbiteClient.PlayerTeamChangeHandler PlayerChangedSquad;
        public virtual event FrostbiteClient.PlayerKilledHandler PlayerKilled;
        public virtual event FrostbiteClient.PlayerSpawnedHandler PlayerSpawned;

        #endregion

        #region Synthetic

        public virtual event FrostbiteClient.PlayerKickedHandler PlayerKickedByAdmin;
        public virtual event FrostbiteClient.PlayerKilledByAdminHandler PlayerKilledByAdmin;
        public virtual event FrostbiteClient.PlayerMovedByAdminHandler PlayerMovedByAdmin;

        #endregion

        #region Chat

        public virtual event FrostbiteClient.RawChatHandler Chat;
        public virtual event FrostbiteClient.GlobalChatHandler GlobalChat;
        public virtual event FrostbiteClient.TeamChatHandler TeamChat;
        public virtual event FrostbiteClient.SquadChatHandler SquadChat;

        #endregion

        #region Punkbuster

        public virtual event FrostbiteClient.PunkbusterMessageHandler PunkbusterMessage;

        #endregion

        #region Map/Round

        public virtual event FrostbiteClient.LoadingLevelHandler LoadingLevel;
        public virtual event FrostbiteClient.EmptyParamterHandler LevelStarted;
        public virtual event FrostbiteClient.RoundOverHandler RoundOver;
        public virtual event FrostbiteClient.RoundOverPlayersHandler RoundOverPlayers;
        public virtual event FrostbiteClient.RoundOverTeamScoresHandler RoundOverTeamScores;

        #endregion

        #endregion

        #region Client Responses

        #region Global

        public virtual event FrostbiteClient.ResponseErrorHandler ResponseError;
        public virtual event FrostbiteClient.RunScriptHandler RunScript;
        public virtual event FrostbiteClient.RunScriptErrorHandler RunScriptError;
        public virtual event FrostbiteClient.EmptyParamterHandler ShutdownServer;

        #endregion

        #region Punkbuster

        public virtual event FrostbiteClient.SendPunkBusterMessageHandler SendPunkbusterMessage;

        #endregion

        #region Query

        public virtual event FrostbiteClient.ServerInfoHandler ServerInfo;
        public virtual event FrostbiteClient.ListPlayersHandler ListPlayers;

        #endregion

        #region Communication

        public virtual event FrostbiteClient.YellingHandler Yelling;
        public virtual event FrostbiteClient.SayingHandler Saying;

        #endregion

        #region Map/Round

        public virtual event FrostbiteClient.EmptyParamterHandler RunNextRound; // Alias for runNextRound
        public virtual event FrostbiteClient.CurrentLevelHandler CurrentLevel;
        public virtual event FrostbiteClient.EmptyParamterHandler RestartRound; // Alias for restartRound
        public virtual event FrostbiteClient.SupportedMapsHandler SupportedMaps;
        public virtual event FrostbiteClient.EndRoundHandler EndRound;

        // Playlist
        public virtual event FrostbiteClient.ListPlaylistsHandler ListPlaylists;

        #region BFBC2 Specific

        public virtual event FrostbiteClient.PlaylistSetHandler PlaylistSet;

        #endregion

        #endregion

        #region Ban list

        public virtual event FrostbiteClient.EmptyParamterHandler BanListLoad;
        public virtual event FrostbiteClient.EmptyParamterHandler BanListSave;
        public virtual event FrostbiteClient.BanListAddHandler BanListAdd;
        public virtual event FrostbiteClient.BanListRemoveHandler BanListRemove;
        public virtual event FrostbiteClient.EmptyParamterHandler BanListClear;
        public virtual event FrostbiteClient.BanListListHandler BanListList;

        #endregion

        #region Text Chat Moderation

        public virtual event FrostbiteClient.EmptyParamterHandler TextChatModerationListLoad;
        public virtual event FrostbiteClient.EmptyParamterHandler TextChatModerationListSave;
        public virtual event FrostbiteClient.TextChatModerationListAddPlayerHandler TextChatModerationListAddPlayer;
        public virtual event FrostbiteClient.TextChatModerationListRemovePlayerHandler TextChatModerationListRemovePlayer;
        public virtual event FrostbiteClient.EmptyParamterHandler TextChatModerationListClear;
        public virtual event FrostbiteClient.TextChatModerationListListHandler TextChatModerationListList;
        
        #endregion

        #region Reserved Slots

        public virtual event FrostbiteClient.ReserverdSlotsConfigFileHandler ReservedSlotsConfigFile;
        public virtual event FrostbiteClient.EmptyParamterHandler ReservedSlotsLoad;
        public virtual event FrostbiteClient.EmptyParamterHandler ReservedSlotsSave;
        public virtual event FrostbiteClient.ReservedSlotsPlayerHandler ReservedSlotsPlayerAdded;
        public virtual event FrostbiteClient.ReservedSlotsPlayerHandler ReservedSlotsPlayerRemoved;
        public virtual event FrostbiteClient.EmptyParamterHandler ReservedSlotsCleared;
        public virtual event FrostbiteClient.ReservedSlotsListHandler ReservedSlotsList;

        #endregion

        #region Maplist

        public virtual event FrostbiteClient.MapListConfigFileHandler MapListConfigFile;
        public virtual event FrostbiteClient.EmptyParamterHandler MapListLoad;
        public virtual event FrostbiteClient.EmptyParamterHandler MapListSave;
        public virtual event FrostbiteClient.MapListAppendedHandler MapListMapAppended;
        public virtual event FrostbiteClient.MapListLevelIndexHandler MapListNextLevelIndex;
        public virtual event FrostbiteClient.MapListLevelIndexHandler MapListMapRemoved;
        public virtual event FrostbiteClient.MapListMapInsertedHandler MapListMapInserted;
        public virtual event FrostbiteClient.EmptyParamterHandler MapListCleared;
        public virtual event FrostbiteClient.MapListListedHandler MapListListed;

        #endregion

        #region Variables (fired if setting or getting is successful)

        #region Configuration

        public virtual event FrostbiteClient.PasswordHandler AdminPassword;
        public virtual event FrostbiteClient.PasswordHandler GamePassword;
        public virtual event FrostbiteClient.IsEnabledHandler Punkbuster;
        public virtual event FrostbiteClient.IsEnabledHandler Ranked;
        public virtual event FrostbiteClient.LimitHandler MaxPlayerLimit;
        public virtual event FrostbiteClient.LimitHandler CurrentPlayerLimit;
        public virtual event FrostbiteClient.LimitHandler PlayerLimit;
        public virtual event FrostbiteClient.LimitHandler IdleTimeout;
        public virtual event FrostbiteClient.IsEnabledHandler ProfanityFilter;

        #endregion

        #region Details

        public virtual event FrostbiteClient.ServerNameHandler ServerName;
        public virtual event FrostbiteClient.BannerUrlHandler BannerUrl;
        public virtual event FrostbiteClient.ServerDescriptionHandler ServerDescription;

        #endregion

        #region Gameplay

        public virtual event FrostbiteClient.IsEnabledHandler Hardcore;
        public virtual event FrostbiteClient.IsEnabledHandler FriendlyFire;

        #region BFBC2

        public virtual event FrostbiteClient.LimitHandler RankLimit;
        public virtual event FrostbiteClient.IsEnabledHandler KillCam;
        public virtual event FrostbiteClient.IsEnabledHandler MiniMap;
        public virtual event FrostbiteClient.IsEnabledHandler CrossHair;
        public virtual event FrostbiteClient.IsEnabledHandler ThreeDSpotting;
        public virtual event FrostbiteClient.IsEnabledHandler MiniMapSpotting;
        public virtual event FrostbiteClient.IsEnabledHandler ThirdPersonVehicleCameras;
        public virtual event FrostbiteClient.IsEnabledHandler TeamBalance;

        #endregion

        #region MoH

        public virtual event FrostbiteClient.IsEnabledHandler ClanTeams;
        public virtual event FrostbiteClient.UpperLowerLimitHandler SkillLimit;
        public virtual event FrostbiteClient.UpperLowerLimitHandler PreRoundLimit;
        public virtual event FrostbiteClient.IsEnabledHandler NoAmmoPickups;
        public virtual event FrostbiteClient.IsEnabledHandler NoCrosshairs;
        public virtual event FrostbiteClient.IsEnabledHandler NoSpotting;
        public virtual event FrostbiteClient.IsEnabledHandler NoUnlocks;
        public virtual event FrostbiteClient.IsEnabledHandler RealisticHealth;

        public virtual event FrostbiteClient.EmptyParamterHandler StopPreRound;

        public virtual event FrostbiteClient.IsEnabledHandler RoundStartTimer;
        public virtual event FrostbiteClient.LimitHandler TdmScoreCounterMaxScore;
        public virtual event FrostbiteClient.LimitHandler RoundStartTimerDelay;
        public virtual event FrostbiteClient.LimitHandler RoundStartTimerPlayerLimit;

        #endregion

        #endregion

        #region Text Chat Moderation

        public virtual event FrostbiteClient.TextChatModerationModeHandler TextChatModerationMode;
        public virtual event FrostbiteClient.LimitHandler TextChatSpamTriggerCount;
        public virtual event FrostbiteClient.LimitHandler TextChatSpamDetectionTime;
        public virtual event FrostbiteClient.LimitHandler TextChatSpamCoolDownTime;

        #endregion

        #region Team Killing

        public virtual event FrostbiteClient.LimitHandler TeamKillCountForKick;
        public virtual event FrostbiteClient.LimitHandler TeamKillValueForKick;
        public virtual event FrostbiteClient.LimitHandler TeamKillValueIncrease;
        public virtual event FrostbiteClient.LimitHandler TeamKillValueDecreasePerSecond;

        #endregion

        #region Level Variables

        public virtual event FrostbiteClient.LevelVariableHandler LevelVariablesSet;
        public virtual event FrostbiteClient.LevelVariableHandler LevelVariablesClear;
        public virtual event FrostbiteClient.LevelVariableGetHandler LevelVariablesGet;
        public virtual event FrostbiteClient.LevelVariableGetHandler LevelVariablesEvaluate;
        public virtual event FrostbiteClient.LevelVariableListHandler LevelVariablesList;

        #endregion

        #endregion

        #endregion

        #endregion

        public virtual void Shutdown() {
            if (this.Connection != null) this.Connection.Shutdown();
        }
    }
}
