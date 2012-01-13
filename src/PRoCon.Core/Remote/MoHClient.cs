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
    public class MoHClient : FrostbiteClient {

        public override string GameType {
            get {
                return "MOH";
            }
        }

        public override bool HasSquads {
            get {
                return false;
            }
        }

        public override bool HasOpenMaplist {
            get {
                // true, does not lock maplist to a playlist
                return true;
            }
        }

        public MoHClient(FrostbiteConnection connection)
            : base(connection) {
            
            this.VersionNumberToFriendlyName = new Dictionary<string, string>();
            /*{
                { "577887", "R2" },
                { "582779", "R3" },
                { "586627", "R5" },
                { "586981", "R6" },
                { "587960", "R7" },
                { "592364", "R8" },
                { "615937", "R9" },
            };*/

            this.m_responseDelegates.Add("vars.clanTeams", this.DispatchVarsClanTeamsResponse);
            this.m_responseDelegates.Add("vars.noCrosshairs", this.DispatchVarsNoCrosshairsResponse);
            this.m_responseDelegates.Add("vars.realisticHealth", this.DispatchVarsRealisticHealthResponse);
            this.m_responseDelegates.Add("vars.noUnlocks", this.DispatchVarsNoUnlocksResponse);
            this.m_responseDelegates.Add("vars.skillLimit", this.DispatchVarsSkillLimitResponse);
            this.m_responseDelegates.Add("vars.noAmmoPickups", this.DispatchVarsNoAmmoPickupsResponse);
            this.m_responseDelegates.Add("vars.tdmScoreCounterMaxScore", this.DispatchVarsTdmScoreCounterMaxScoreResponse);
            
            // Preround vars
            this.m_responseDelegates.Add("vars.preRoundLimit", this.DispatchVarsPreRoundLimitResponse);
            this.m_responseDelegates.Add("admin.roundStartTimerEnabled", this.DispatchAdminRoundStartTimerEnabledResponse);
            this.m_responseDelegates.Add("vars.roundStartTimerDelay", this.DispatchVarsRoundStartTimerDelayResponse);
            this.m_responseDelegates.Add("vars.roundStartTimerPlayersLimit", this.DispatchVarsRoundStartTimerPlayersLimitResponse);
             
            // New map functions?
            this.m_responseDelegates.Add("admin.stopPreRound", this.DispatchAdminStopPreRoundResponse);

            // Note: These delegates point to methods in FrostbiteClient.
            this.m_responseDelegates.Add("reservedSpectateSlots.configFile", this.DispatchReservedSlotsConfigFileResponse);
            this.m_responseDelegates.Add("reservedSpectateSlots.load", this.DispatchReservedSlotsLoadResponse);
            this.m_responseDelegates.Add("reservedSpectateSlots.save", this.DispatchReservedSlotsSaveResponse);
            this.m_responseDelegates.Add("reservedSpectateSlots.addPlayer", this.DispatchReservedSlotsAddPlayerResponse);
            this.m_responseDelegates.Add("reservedSpectateSlots.removePlayer", this.DispatchReservedSlotsRemovePlayerResponse);
            this.m_responseDelegates.Add("reservedSpectateSlots.clear", this.DispatchReservedSlotsClearResponse);
            this.m_responseDelegates.Add("reservedSpectateSlots.list", this.DispatchReservedSlotsListResponse);

            this.GetPacketsPattern = new Regex(this.GetPacketsPattern.ToString() + "|^reservedSpectateSlots.list|^admin.roundStartTimerEnabled$|^admin.tdmScoreCounterMaxScore$", RegexOptions.Compiled);
        }

        public override void FetchStartupVariables() {
            base.FetchStartupVariables();

            this.SendGetVarsClanTeamsPacket();
            this.SendGetVarsNoCrosshairsPacket();
            this.SendGetVarsRealisticHealthPacket();
            this.SendGetVarsNoUnlocksPacket();
            this.SendGetVarsTdmScoreCounterMaxScorePacket();
            this.SendGetVarsNoAmmoPickupsPacket();
            this.SendGetVarsSkillLimitPacket();

            this.SendGetAdminRoundStartTimerEnabledPacket();
            this.SendGetVarsRoundStartTimerPlayersLimitPacket();
            this.SendGetVarsRoundStartTimerDelayPacket();
            this.SendGetVarsPreRoundLimitPacket();

            // vars.skillLimit
            // vars.preRoundLimit
        }

        #region Overridden Events

        public override event FrostbiteClient.PlayerTeamChangeHandler PlayerChangedTeam;
        public override event FrostbiteClient.PlayerMovedByAdminHandler PlayerMovedByAdmin;

        // Vars
        public override event FrostbiteClient.IsEnabledHandler ClanTeams;
        public override event FrostbiteClient.UpperLowerLimitHandler SkillLimit;
        public override event FrostbiteClient.IsEnabledHandler NoAmmoPickups;
        public override event FrostbiteClient.IsEnabledHandler NoCrosshairs;
        public override event FrostbiteClient.IsEnabledHandler NoSpotting;
        public override event FrostbiteClient.IsEnabledHandler NoUnlocks;
        public override event FrostbiteClient.IsEnabledHandler RealisticHealth;
        public override event FrostbiteClient.LimitHandler TdmScoreCounterMaxScore;

        // Preround
        public override event FrostbiteClient.EmptyParamterHandler StopPreRound;
        public override event FrostbiteClient.UpperLowerLimitHandler PreRoundLimit;
        public override event FrostbiteClient.IsEnabledHandler RoundStartTimer;
        public override event FrostbiteClient.LimitHandler RoundStartTimerDelay;
        public override event FrostbiteClient.LimitHandler RoundStartTimerPlayerLimit;
        
        public override event FrostbiteClient.PlayerSpawnedHandler PlayerSpawned;

        #endregion
        
        #region Packet Helpers

        public override void SendAdminMovePlayerPacket(string soldierName, int destinationTeamId, int destinationSquadId, bool forceKill) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("admin.movePlayer", soldierName, destinationTeamId.ToString(), Packet.bltos(forceKill));
            }
        }

        #region Reserved Slot List

        public override void SendReservedSlotsListPacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("reservedSpectateSlots.list");
            }
        }

        public override void SendReservedSlotsAddPlayerPacket(string soldierName) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("reservedSpectateSlots.addPlayer", soldierName);
            }
        }

        public override void SendReservedSlotsRemovePlayerPacket(string soldierName) {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("reservedSpectateSlots.removePlayer", soldierName);
            }
        }

        public override void SendReservedSlotsSavePacket() {
            if (this.IsLoggedIn == true) {
                this.BuildSendPacket("reservedSpectateSlots.save");
            }
        }
        #endregion

        #endregion
        
        #region Implemented/Overridden Response Handlers

        protected virtual void DispatchAdminStopPreRoundResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.StopPreRound != null) {
                    FrostbiteConnection.RaiseEvent(this.StopPreRound.GetInvocationList(), this);
                }
            }
        }

        #region Vars
        
        protected virtual void DispatchVarsClanTeamsResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.ClanTeams != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.ClanTeams.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.ClanTeams.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsNoCrosshairsResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.NoCrosshairs != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.NoCrosshairs.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.NoCrosshairs.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsNoSpottingResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.NoSpotting != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.NoSpotting.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.NoSpotting.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsNoUnlocksResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.NoUnlocks != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.NoUnlocks.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.NoUnlocks.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsRealisticHealthResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.RealisticHealth != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.RealisticHealth.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.RealisticHealth.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsSkillLimitResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.SkillLimit != null) {
                    if (cpRecievedPacket.Words.Count == 3) {
                        FrostbiteConnection.RaiseEvent(this.SkillLimit.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]), Convert.ToInt32(cpRecievedPacket.Words[2]));
                    }
                    else if (cpRequestPacket.Words.Count >= 3) {
                        FrostbiteConnection.RaiseEvent(this.SkillLimit.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]), Convert.ToInt32(cpRequestPacket.Words[2]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsNoAmmoPickupsResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.NoAmmoPickups != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.NoAmmoPickups.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.NoAmmoPickups.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }


        protected virtual void DispatchVarsTdmScoreCounterMaxScoreResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.TdmScoreCounterMaxScore != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.TdmScoreCounterMaxScore.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.TdmScoreCounterMaxScore.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                    }
                }
            }
        }


        #region Preround

        protected virtual void DispatchVarsPreRoundLimitResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.PreRoundLimit != null) {
                    if (cpRecievedPacket.Words.Count == 3) {
                        FrostbiteConnection.RaiseEvent(this.PreRoundLimit.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]), Convert.ToInt32(cpRecievedPacket.Words[2]));
                    }
                    else if (cpRequestPacket.Words.Count >= 3) {
                        FrostbiteConnection.RaiseEvent(this.PreRoundLimit.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]), Convert.ToInt32(cpRequestPacket.Words[2]));
                    }
                }
            }
        }
        
        protected virtual void DispatchAdminRoundStartTimerEnabledResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.RoundStartTimer != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.RoundStartTimer.GetInvocationList(), this, Convert.ToBoolean(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.RoundStartTimer.GetInvocationList(), this, Convert.ToBoolean(cpRequestPacket.Words[1]));
                    }
                }
            }
        }
        
        protected virtual void DispatchVarsRoundStartTimerDelayResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.RoundStartTimerDelay != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.RoundStartTimerDelay.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.RoundStartTimerDelay.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                    }
                }
            }
        }

        protected virtual void DispatchVarsRoundStartTimerPlayersLimitResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.RoundStartTimerPlayerLimit != null) {
                    if (cpRecievedPacket.Words.Count == 2) {
                        FrostbiteConnection.RaiseEvent(this.RoundStartTimerPlayerLimit.GetInvocationList(), this, Convert.ToInt32(cpRecievedPacket.Words[1]));
                    }
                    else if (cpRequestPacket.Words.Count >= 2) {
                        FrostbiteConnection.RaiseEvent(this.RoundStartTimerPlayerLimit.GetInvocationList(), this, Convert.ToInt32(cpRequestPacket.Words[1]));
                    }
                }
            }
        }


        #endregion

        #endregion

        //public override void DispatchResponsePacket(FrostbiteConnection connection, Packet cpRecievedPacket, Packet cpRequestPacket) {
        //    base.DispatchResponsePacket(connection, cpRecievedPacket, cpRequestPacket);
        //}

        #endregion
        
        #region Overridden Request Handlers

        protected override void DispatchPlayerOnSpawnRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 7) {
                if (this.PlayerSpawned != null) {
                    FrostbiteConnection.RaiseEvent(this.PlayerSpawned.GetInvocationList(), this, cpRequestPacket.Words[1], cpRequestPacket.Words[2], cpRequestPacket.Words.GetRange(3, 1), cpRequestPacket.Words.GetRange(4, 3)); // new Inventory(cpRequestPacket.Words[3], cpRequestPacket.Words[4], cpRequestPacket.Words[5], cpRequestPacket.Words[6], cpRequestPacket.Words[7], cpRequestPacket.Words[8]));
                }
            }
        }

        protected override void DispatchPlayerOnTeamChangeRequest(FrostbiteConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 3) {
                int iTeamID = 0;

                if (int.TryParse(cpRequestPacket.Words[2], out iTeamID) == true) {
                    if (this.PlayerChangedTeam != null) {
                        FrostbiteConnection.RaiseEvent(this.PlayerChangedTeam.GetInvocationList(), this, cpRequestPacket.Words[1], iTeamID, 0);
                    }
                }
            }
        }

        protected override void DispatchAdminMovePlayerResponse(FrostbiteConnection sender, Packet cpRecievedPacket, Packet cpRequestPacket) {

            if (this.PlayerMovedByAdmin != null) {

                int desinationTeamId;
                bool forceKilled;

                if (cpRequestPacket.Words.Count >= 4) {

                    if (int.TryParse(cpRequestPacket.Words[2], out desinationTeamId) == true &&
                        bool.TryParse(cpRequestPacket.Words[3], out forceKilled) == true) {

                        FrostbiteConnection.RaiseEvent(this.PlayerMovedByAdmin.GetInvocationList(), this, cpRequestPacket.Words[1], desinationTeamId, 0, forceKilled);
                    }
                }
            }
        }
        
        //public override void DispatchRequestPacket(FrostbiteConnection connection, Packet cpRequestPacket) {
        //    base.DispatchRequestPacket(connection, cpRequestPacket);
        //}

        #endregion

    }
}
