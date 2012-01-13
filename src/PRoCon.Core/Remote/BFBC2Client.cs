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
using System.Text.RegularExpressions;

namespace PRoCon.Core.Remote {
    using Core.Players;
    using Core.Maps;

    public class BFBC2Client : FrostbiteClient {

        public override string GameType {
            get {
                return "BFBC2";
            }
        }

        public BFBC2Client(FrostbiteConnection connection)
            : base(connection) {

                this.VersionNumberToFriendlyName = new Dictionary<string, string>(); 
            /*{
              { "571287", "R21" },
              { "581637", "R22" }, 
              { "584642", "R23" },
              { "593485", "R24" },
              { "602833", "R25" },
              { "609063", "R26" },
              { "617877", "R27" },
              { "621775", "R28" }
            };*/

            #region Map list functions

            this.m_responseDelegates.Add("admin.getPlaylist", this.DispatchAdminGetPlaylistResponse);
            this.m_responseDelegates.Add("admin.setPlaylist", this.DispatchAdminSetPlaylistResponse);

            // Note: These delegates point to methods in FrostbiteClient.
            this.m_responseDelegates.Add("admin.runNextLevel", this.DispatchAdminRunNextRoundResponse);
            this.m_responseDelegates.Add("admin.currentLevel", this.DispatchAdminCurrentLevelResponse);

            #endregion

            this.m_responseDelegates.Add("vars.rankLimit", this.DispatchVarsRankLimitResponse);
            this.m_responseDelegates.Add("vars.teamBalance", this.DispatchVarsTeamBalanceResponse);
            this.m_responseDelegates.Add("vars.killCam", this.DispatchVarsKillCamResponse);
            this.m_responseDelegates.Add("vars.miniMap", this.DispatchVarsMiniMapResponse);
            this.m_responseDelegates.Add("vars.crossHair", this.DispatchVarsCrossHairResponse);
            this.m_responseDelegates.Add("vars.3dSpotting", this.DispatchVars3dSpottingResponse);
            this.m_responseDelegates.Add("vars.miniMapSpotting", this.DispatchVarsMiniMapSpottingResponse);
            this.m_responseDelegates.Add("vars.thirdPersonVehicleCameras", this.DispatchVarsThirdPersonVehicleCamerasResponse);

            // Note: These delegates point to methods in FrostbiteClient.
            this.m_responseDelegates.Add("reservedSlots.configFile", this.DispatchReservedSlotsConfigFileResponse);
            this.m_responseDelegates.Add("reservedSlots.load", this.DispatchReservedSlotsLoadResponse);
            this.m_responseDelegates.Add("reservedSlots.save", this.DispatchReservedSlotsSaveResponse);
            this.m_responseDelegates.Add("reservedSlots.addPlayer", this.DispatchReservedSlotsAddPlayerResponse);
            this.m_responseDelegates.Add("reservedSlots.removePlayer", this.DispatchReservedSlotsRemovePlayerResponse);
            this.m_responseDelegates.Add("reservedSlots.clear", this.DispatchReservedSlotsClearResponse);
            this.m_responseDelegates.Add("reservedSlots.list", this.DispatchReservedSlotsListResponse);

            this.GetPacketsPattern = new Regex(this.GetPacketsPattern.ToString() + "|^admin.getPlaylist|^reservedSlots.list", RegexOptions.Compiled);
        }

        public override void FetchStartupVariables() {
            base.FetchStartupVariables();

            this.SendAdminGetPlaylistPacket();

            this.SendGetVarsRankLimitPacket();
            this.SendGetVarsTeamBalancePacket();
            this.SendGetVarsKillCamPacket();
            this.SendGetVarsMiniMapPacket();
            this.SendGetVarsCrossHairPacket();
            this.SendGetVars3dSpottingPacket();
            this.SendGetVarsMiniMapSpottingPacket();
            this.SendGetVarsThirdPersonVehicleCamerasPacket();
        }

        #region Overridden Events

        public override event FrostbiteClient.LimitHandler RankLimit;
        public override event FrostbiteClient.IsEnabledHandler KillCam;
        public override event FrostbiteClient.IsEnabledHandler MiniMap;
        public override event FrostbiteClient.IsEnabledHandler CrossHair;
        public override event FrostbiteClient.IsEnabledHandler ThreeDSpotting;
        public override event FrostbiteClient.IsEnabledHandler MiniMapSpotting;
        public override event FrostbiteClient.IsEnabledHandler ThirdPersonVehicleCameras;
        public override event FrostbiteClient.IsEnabledHandler TeamBalance;

        public override event FrostbiteClient.PlaylistSetHandler PlaylistSet;

        public override event FrostbiteClient.PlayerSpawnedHandler PlayerSpawned;

        #endregion

        #region Overridden Response Handlers

        protected override void DispatchPlayerOnSpawnRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 9) {
                if (this.PlayerSpawned != null) {
                    FrostbiteConnection.RaiseEvent(this.PlayerSpawned.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2], cpRequestPacket.Words.GetRange(3, 3), cpRequestPacket.Words.GetRange(6, 3)); // new Inventory(cpRequestPacket.Words[3], cpRequestPacket.Words[4], cpRequestPacket.Words[5], cpRequestPacket.Words[6], cpRequestPacket.Words[7], cpRequestPacket.Words[8]));
                }
            }
        }

        protected virtual void DispatchAdminSetPlaylistResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 2) {
                if (this.PlaylistSet != null) {
                    FrostbiteConnection.RaiseEvent(this.PlaylistSet.GetInvocationList(), this, cpRequestPacket.Words[1]);
                }
            }
        }

        protected virtual void DispatchAdminGetPlaylistResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1 && cpRecievedPacket.Words.Count >= 2) {
                if (this.PlaylistSet != null) {
                    FrostbiteConnection.RaiseEvent(this.PlaylistSet.GetInvocationList(), this, cpRecievedPacket.Words[1]);
                }
            }
        }

        protected virtual void DispatchVarsRankLimitResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.RankLimit != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.RankLimit.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.RankLimit.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsTeamBalanceResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.TeamBalance != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.TeamBalance.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.TeamBalance.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }


        protected virtual void DispatchVarsKillCamResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.KillCam != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.KillCam.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.KillCam.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsMiniMapResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.MiniMap != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.MiniMap.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.MiniMap.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsCrossHairResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.CrossHair != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.CrossHair.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.CrossHair.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVars3dSpottingResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.ThreeDSpotting != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.ThreeDSpotting.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.ThreeDSpotting.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsMiniMapSpottingResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.MiniMapSpotting != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.MiniMapSpotting.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.MiniMapSpotting.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsThirdPersonVehicleCamerasResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.ThirdPersonVehicleCameras != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.ThirdPersonVehicleCameras.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.ThirdPersonVehicleCameras.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        //public override void DispatchResponsePacket(FrostbiteConnection connection, Packet cpRecievedPacket, Packet cpRequestPacket) {
        //    base.DispatchResponsePacket(connection, cpRecievedPacket, cpRequestPacket);

        //}

        #endregion

    }
}
