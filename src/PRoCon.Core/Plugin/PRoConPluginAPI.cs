/*  Copyright 2010 Geoffrey 'Phogue' Green

    http://www.phogue.net
 
    This file is part of PRoCon Frostbite.

    PRoCon Frostbite is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    PRoCon Frostbite is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY { } without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with PRoCon Frostbite.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Core.Plugin {
    using Core.Players;
    using Core.Players.Items;
    using Core.Plugin.Commands;
    using Core.Battlemap;
    using Core.Maps;
    using Core.HttpServer;
    using Core.TextChatModeration;

    public class PRoConPluginAPI : CPRoConMarshalByRefObject {

        #region Properties

        /// <summary>
        /// You need to allow "OnPunkbusterPlayerInfo" and "OnPlayerLeft" for the pb list
        /// to be properly updated
        /// </summary>
        protected Dictionary<string, CPunkbusterInfo> PunkbusterPlayerInfoList;

        /// <summary>
        /// You need to allow "OnListPlayers", "OnPlayerJoin" and "OnPlayerLeft"
        /// </summary>
        protected Dictionary<string, CPlayerInfo> FrostbitePlayerInfoList;

        #endregion

        public PRoConPluginAPI() {
            this.PunkbusterPlayerInfoList = new Dictionary<string, CPunkbusterInfo>();
            this.FrostbitePlayerInfoList = new Dictionary<string, CPlayerInfo>();
        }

        public virtual void OnConnectionClosed() { }

        #region Helper Methods

        /// <summary>
        /// t-master's enum-to-string method
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public string CreateEnumString(Type enumeration) {
            return string.Format("enum.{0}_{1}({2})", GetType().Name, enumeration.Name, string.Join("|", Enum.GetNames(enumeration)));
        }
        
        #endregion

        // Commands sent by the client with a "OK" response received.
        #region Game Server Responses (Responses to commands the client sent)

        #region Global/Login

        public virtual void OnLogin() { }
        public virtual void OnLogout() { }
        public virtual void OnQuit() { }
        public virtual void OnVersion(string serverType, string version) { }
        public virtual void OnHelp(List<string> commands) { }

        public virtual void OnRunScript(string scriptFileName) { }
        public virtual void OnRunScriptError(string scriptFileName, int lineError, string errorDescription) { }

        public virtual void OnServerInfo(CServerInfo serverInfo) { }
        public virtual void OnResponseError(List<string> requestWords, string error) { }

        public virtual void OnYelling(string message, int messageDuration, CPlayerSubset subset) { }
        public virtual void OnSaying(string message, CPlayerSubset subset) { }

        #endregion

        #region Map Functions

        public virtual void OnRestartLevel() { }
        public virtual void OnSupportedMaps(string playlist, List<string> lstSupportedMaps) { }
        public virtual void OnListPlaylists(List<string> playlists) { }

        public virtual void OnListPlayers(List<CPlayerInfo> players, CPlayerSubset subset) {
            if (subset.Subset == CPlayerSubset.PlayerSubsetType.All) {
                foreach (CPlayerInfo player in players) {
                    if (this.FrostbitePlayerInfoList.ContainsKey(player.SoldierName) == true) {
                        this.FrostbitePlayerInfoList[player.SoldierName] = player;
                    }
                    else {
                        this.FrostbitePlayerInfoList.Add(player.SoldierName, player);
                    }
                }
            }
        }

        /// <summary>
        /// Called when procon recieves "OK" from admin.endRound X
        /// 
        /// See OnRoundOver(int iWinningTeamID) for the event sent by the server.
        /// </summary>
        /// <param name="iWinningTeamID"></param>
        public virtual void OnEndRound(int iWinningTeamID) { }
        public virtual void OnRunNextLevel() { }
        public virtual void OnCurrentLevel(string mapFileName) { }

        #region BFBC2

        public virtual void OnPlaylistSet(string playlist) { }

        #endregion

        #endregion

        #region Banlist

        public virtual void OnBanAdded(CBanInfo ban) { }
        public virtual void OnBanRemoved(CBanInfo ban) { }
        public virtual void OnBanListClear() { }
        public virtual void OnBanListSave() { }
        public virtual void OnBanListLoad() { }
        public virtual void OnBanList(List<CBanInfo> banList) { }

        #endregion

        #region Text Chat Moderation

        public virtual void OnTextChatModerationAddPlayer(TextChatModerationEntry playerEntry) { }
        public virtual void OnTextChatModerationRemovePlayer(TextChatModerationEntry playerEntry) { }
        public virtual void OnTextChatModerationClear() { }
        public virtual void OnTextChatModerationSave() { }
        public virtual void OnTextChatModerationLoad() { }
        public virtual void OnTextChatModerationList(TextChatModerationDictionary moderationList) { }

        #endregion
        
        #region Maplist

        public virtual void OnMaplistConfigFile(string configFileName) { }
        public virtual void OnMaplistLoad() { }
        public virtual void OnMaplistSave() { }
        /// <summary>
        /// Includes a list of maps/rounds from "mapList.list rounds"
        /// </summary>
        /// <param name="lstMaplist"></param>
        public virtual void OnMaplistList(List<MaplistEntry> lstMaplist) { }
        public virtual void OnMaplistCleared() { }
        public virtual void OnMaplistMapAppended(string mapFileName) { }
        public virtual void OnMaplistNextLevelIndex(int mapIndex) { }
        public virtual void OnMaplistMapRemoved(int mapIndex) { }
        public virtual void OnMaplistMapInserted(int mapIndex, string mapFileName) { }

        #endregion

        #region Variables

        #region Details

        public virtual void OnServerName(string serverName) { }
        public virtual void OnServerDescription(string serverDescription) { }
        public virtual void OnBannerURL(string url) { }

        #endregion

        #region Configuration

        public virtual void OnGamePassword(string gamePassword) { }
        public virtual void OnPunkbuster(bool isEnabled) { }
        public virtual void OnRanked(bool isEnabled) { }
        public virtual void OnRankLimit(int iRankLimit) { }
        public virtual void OnPlayerLimit(int limit) { }
        public virtual void OnMaxPlayerLimit(int limit) { }
        public virtual void OnCurrentPlayerLimit(int limit) { }
        public virtual void OnIdleTimeout(int limit) { }
        public virtual void OnProfanityFilter(bool isEnabled) { }

        #endregion

        #region Gameplay

        public virtual void OnFriendlyFire(bool isEnabled) { }
        public virtual void OnHardcore(bool isEnabled) { }

        #region BFBC2

        public virtual void OnTeamBalance(bool isEnabled) { }
        public virtual void OnKillCam(bool isEnabled) { }
        public virtual void OnMiniMap(bool isEnabled) { }
        public virtual void OnCrossHair(bool isEnabled) { }
        public virtual void On3dSpotting(bool isEnabled) { }
        public virtual void OnMiniMapSpotting(bool isEnabled) { }
        public virtual void OnThirdPersonVehicleCameras(bool isEnabled) { }

        #endregion

        #endregion

        #region Team Kill

        public virtual void OnTeamKillCountForKick(int limit) { }
        public virtual void OnTeamKillValueIncrease(int limit) { }
        public virtual void OnTeamKillValueDecreasePerSecond(int limit) { }
        public virtual void OnTeamKillValueForKick(int limit) { }

        #endregion

        #region Level Variables

        public virtual void OnLevelVariablesList(LevelVariable requestedContext, List<LevelVariable> returnedValues) { }
        public virtual void OnLevelVariablesEvaluate(LevelVariable requestedContext, LevelVariable returnedValue) { }
        public virtual void OnLevelVariablesClear(LevelVariable requestedContext) { }
        public virtual void OnLevelVariablesSet(LevelVariable requestedContext) { }
        public virtual void OnLevelVariablesGet(LevelVariable requestedContext, LevelVariable returnedValue) { }

        #endregion

        #region Text Chat Moderation Settings

        public virtual void OnTextChatModerationMode(ServerModerationModeType mode) { }
        public virtual void OnTextChatSpamTriggerCount(int limit) { }
        public virtual void OnTextChatSpamDetectionTime(int limit) { }
        public virtual void OnTextChatSpamCoolDownTime(int limit) { }

        #endregion

        #region Reserved/Specate Slots
        // Note: This covers MoH's reserved spectate slots as well.

        public virtual void OnReservedSlotsConfigFile(string configFileName) { }
        public virtual void OnReservedSlotsLoad() { }
        public virtual void OnReservedSlotsSave() { }
        public virtual void OnReservedSlotsPlayerAdded(string soldierName) { }
        public virtual void OnReservedSlotsPlayerRemoved(string soldierName) { }
        public virtual void OnReservedSlotsCleared() { }
        public virtual void OnReservedSlotsList(List<string> soldierNames) { }

        #endregion

        #endregion

        #region Player Actions

        public virtual void OnPlayerKilledByAdmin(string soldierName) { }
        public virtual void OnPlayerKickedByAdmin(string soldierName, string reason) { }
        public virtual void OnPlayerMovedByAdmin(string soldierName, int destinationTeamId, int destinationSquadId, bool forceKilled) { }

        #endregion

        #endregion
        
        // These events are sent from the server without any initial request from the client.
        #region Game Server Requests (Events)

        #region Players

        public virtual void OnPlayerJoin(string soldierName) {
            if (this.FrostbitePlayerInfoList.ContainsKey(soldierName) == false) {
                this.FrostbitePlayerInfoList.Add(soldierName, new CPlayerInfo(soldierName, "", 0, 24));
            }
        }

        public virtual void OnPlayerLeft(CPlayerInfo playerInfo) {
            if (this.PunkbusterPlayerInfoList.ContainsKey(playerInfo.SoldierName) == true) {
                this.PunkbusterPlayerInfoList.Remove(playerInfo.SoldierName);
            }

            if (this.FrostbitePlayerInfoList.ContainsKey(playerInfo.SoldierName) == true) {
                this.FrostbitePlayerInfoList.Remove(playerInfo.SoldierName);
            }
        }

        public virtual void OnPlayerAuthenticated(string soldierName, string guid) { }
        public virtual void OnPlayerKilled(Kill kKillerVictimDetails) { }
        public virtual void OnPlayerKicked(string soldierName, string reason) { }
        public virtual void OnPlayerSpawned(string soldierName, Inventory spawnedInventory) { }

        public virtual void OnPlayerTeamChange(string soldierName, int teamId, int squadId) { }
        public virtual void OnPlayerSquadChange(string soldierName, int teamId, int squadId) { }

        #endregion

        #region Chat

        public virtual void OnGlobalChat(string speaker, string message) { }
        public virtual void OnTeamChat(string speaker, string message, int teamId) { }
        public virtual void OnSquadChat(string speaker, string message, int teamId, int squadId) { }

        #endregion

        #region Round Over Events

        /// <summary>
        /// Replacement for IPRoConPluginInterface2.OnRoundOverPlayers(List<string> players) { }
        /// It was a typo =\
        /// </summary>
        /// <param name="players"></param>
        public virtual void OnRoundOverPlayers(List<CPlayerInfo> players) { }
        public virtual void OnRoundOverTeamScores(List<TeamScore> teamScores) { }
        public virtual void OnRoundOver(int winningTeamId) { }

        #endregion

        #region Levels

        /// <summary>
        /// Updated version of LoadingLevel that contains the amount of rounds played.
        /// Thanks to Archsted on the forums for pointing this out.
        /// </summary>
        /// <param name="mapFileName"></param>
        /// <param name="roundsPlayed"></param>
        /// <param name="roundsTotal"></param>
        public virtual void OnLoadingLevel(string mapFileName, int roundsPlayed, int roundsTotal) { }
        public virtual void OnLevelStarted() { }

        #endregion

        #region Punkbuster

        /// <summary>
        /// Raw punkbuster message from the server
        /// </summary>
        /// <param name="punkbusterMessage"></param>
        public virtual void OnPunkbusterMessage(string punkbusterMessage) { }

        /// <summary>
        /// A ban taken from a punkbuster message and converted to a CBanInfo object
        /// </summary>
        /// <param name="ban"></param>
        public virtual void OnPunkbusterBanInfo(CBanInfo ban) { }

        /// <summary>
        /// Fired when unbanning a guid with pb_sv_unbanguid
        /// </summary>
        /// <param name="unban"></param>
        public virtual void OnPunkbusterUnbanInfo(CBanInfo unban) { }

        public virtual void OnPunkbusterBeginPlayerInfo() { }

        public virtual void OnPunkbusterEndPlayerInfo() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="playerInfo"></param>
        public virtual void OnPunkbusterPlayerInfo(CPunkbusterInfo playerInfo) {
            if (playerInfo != null) {
                if (this.PunkbusterPlayerInfoList.ContainsKey(playerInfo.SoldierName) == false) {
                    this.PunkbusterPlayerInfoList.Add(playerInfo.SoldierName, playerInfo);
                }
                else {
                    this.PunkbusterPlayerInfoList[playerInfo.SoldierName] = playerInfo;
                }
            }
        }

        #endregion

        #endregion

        #region Internal Procon Events

        #region Accounts

        public virtual void OnAccountCreated(string username) { }
        public virtual void OnAccountDeleted(string username) { }
        public virtual void OnAccountPrivilegesUpdate(string username, CPrivileges privileges) { }

        public virtual void OnAccountLogin(string accountName, string ip, CPrivileges privileges) { }
        public virtual void OnAccountLogout(string accountName, string ip, CPrivileges privileges) { }


        #endregion

        #region Command Registration

        /// <summary>
        /// Fires when any registered command has been matched against a players text in game.
        /// This method is called regardless of it being registered to your classname.
        /// Called *after* a confirmation.  If this method is called you can assume the 
        /// speaker has met the required privliege and has confirmed the command as correct.
        /// </summary>
        /// <param name="speaker">The player that issued the command</param>
        /// <param name="strText">The text that was matched to the MatchCommand object</param>
        /// <param name="mtcCommand">The registered command object</param>
        /// <param name="capCommand">The captured command details</param>
        /// <param name="subMatchedScope">The scope the message was sent by the player (squad chat, team chat etc)</param>
        /// Note: This method was not included, instead you delegate a method when creating a MatchCommand object.
        public virtual void OnAnyMatchRegisteredCommand(string speaker, string text, MatchCommand matchedCommand, CapturedCommand capturedCommand, CPlayerSubset matchedScope) { }

        /// <summary>
        /// Fires whenever a command is registered to procon from a plugin.
        /// Care should be taken not to enter an endless loop with this function by setting a command
        /// inside of the event.
        /// </summary>
        /// <param name="mtcCommand">The registered command object</param>
        public virtual void OnRegisteredCommand(MatchCommand command) { }

        /// <summary>
        /// Fires whenever a command is unregisted from procon from any plugin.
        /// </summary>
        /// <param name="mtcCommand"></param>
        public virtual void OnUnregisteredCommand(MatchCommand command) { }

        #endregion

        #region Battlemap Events

        /// <summary>
        /// Fires when a player takes [ZoneAction] and [flTresspassPercentage] > 0.0F
        /// </summary>
        /// <param name="cpiSoldier">The PlayerInfo object procon has on the player.</param>
        /// <param name="action">The action the player has taken on the zone</param>
        /// <param name="sender">The mapzone object that has fired the event</param>
        /// <param name="pntTresspassLocation">The location, reported by the game, that the action has taken place</param>
        /// <param name="flTresspassPercentage">The percentage (0.0F to 1.0F) that the circle created by the error radius (default 14m) that
        /// this player has tresspased on the zone at point [pntTresspassLocation].</param>
        /// <param name="trespassState">Additional information about the event.  If the ZoneAction is Kill/Death then this object is type "Kill".</param>
        public virtual void OnZoneTrespass(CPlayerInfo playerInfo, ZoneAction action, MapZone sender, Point3D tresspassLocation, float tresspassPercentage, object trespassState) { }

        #endregion

        #region HTTP Server

        /// <summary>
        /// If the http server interface is on all requests to the plugin will
        /// be directed to this method.  The response will be sent back.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>By default the response will be a blank "200 OK" document</returns>
        public virtual HttpWebServerResponseData OnHttpRequest(HttpWebServerRequestData data) {
            return null;
        }

        #endregion

        #endregion

        #region Layer Procon Events

        #region Variables

        public virtual void OnReceiveProconVariable(string variableName, string value) { }

        #endregion

        #endregion
    }
}
