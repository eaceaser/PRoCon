using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Core.Remote.Layer {
    public class FrostbiteLayerClient {

        private FrostbiteLayerConnection m_connection;

        #region Packet Management

        protected delegate void RequestPacketHandler(FrostbiteLayerConnection sender, Packet cpRecievedPacket);
        //protected delegate void ResponsePacketHandler(FrostbiteLayerConnection sender, Packet cpRecievedPacket);
        //protected Dictionary<string, ResponsePacketHandler> m_responseDelegates = new Dictionary<string, ResponsePacketHandler>();
        protected Dictionary<string, RequestPacketHandler> m_requestDelegates = new Dictionary<string, RequestPacketHandler>();

        #endregion

        #region Events

        public delegate void EmptyParameterHandler(FrostbiteLayerClient sender);
        public event EmptyParameterHandler ConnectionClosed;

        public delegate void RequestPacketDispatchHandler(FrostbiteLayerClient sender, Packet packet);
        public virtual event RequestPacketDispatchHandler RequestPacketUnsecureSafeListedRecieved; // Harmless packets that do not require login.
        public virtual event RequestPacketDispatchHandler RequestPacketSecureSafeListedRecieved; // Harmless recieved packets for logged in users

        public virtual event RequestPacketDispatchHandler RequestPacketPunkbusterRecieved;

        public virtual event RequestPacketDispatchHandler RequestPacketAlterTextMonderationListRecieved;
        public virtual event RequestPacketDispatchHandler RequestPacketAlterBanListRecieved;
        public virtual event RequestPacketDispatchHandler RequestPacketAlterReservedSlotsListRecieved;
        public virtual event RequestPacketDispatchHandler RequestPacketAlterMaplistRecieved;

        public virtual event RequestPacketDispatchHandler RequestPacketUseMapFunctionRecieved;

        public virtual event RequestPacketDispatchHandler RequestPacketVarsRecieved;

        public virtual event RequestPacketDispatchHandler RequestPacketAdminShutdown;

        public virtual event RequestPacketDispatchHandler RequestPacketAdminPlayerKillRecieved;
        public virtual event RequestPacketDispatchHandler RequestPacketAdminKickPlayerRecieved;
        public virtual event RequestPacketDispatchHandler RequestPacketAdminPlayerMoveRecieved;

        public virtual event RequestPacketDispatchHandler RequestPacketUnknownRecieved;

        public delegate void RequestBanListAddHandler(FrostbiteLayerClient sender, Packet packet, CBanInfo newBan);
        public virtual event RequestBanListAddHandler RequestBanListAddRecieved;

        public delegate void RequestLoginPlainTextHandler(FrostbiteLayerClient sender, Packet packet, string password);
        public virtual event RequestLoginPlainTextHandler RequestLoginPlainText;

        public virtual event RequestPacketDispatchHandler RequestQuit;
        public virtual event RequestPacketDispatchHandler RequestLogout;
        public virtual event RequestPacketDispatchHandler RequestHelp;
        public virtual event RequestPacketDispatchHandler RequestLoginHashed;

        public delegate void RequestEventsEnabledHandler(FrostbiteLayerClient sender, Packet packet, bool eventsEnabled);
        public virtual event RequestEventsEnabledHandler RequestEventsEnabled;

        public delegate void RequestLoginHashedPasswordHandler(FrostbiteLayerClient sender, Packet packet, string hashedPassword);
        public virtual event RequestLoginHashedPasswordHandler RequestLoginHashedPassword;

        #endregion

        public string IPPort {
            get {
                string returnIpPort = String.Empty;

                if (this.m_connection != null) {
                    returnIpPort = this.m_connection.IPPort;
                }

                return returnIpPort;
            }
        }

        public FrostbiteLayerClient(FrostbiteLayerConnection connection) {
            this.m_connection = connection;

            this.m_requestDelegates = new Dictionary<string, RequestPacketHandler>() {
                { "login.plainText", this.DispatchLoginPlainTextRequest },
                { "login.hashed", this.DispatchLoginHashedRequest },
                { "logout", this.DispatchLogoutRequest },
                { "quit", this.DispatchQuitRequest },
                { "version", this.DispatchUnsecureSafeListedRequest },
                { "eventsEnabled", this.DispatchEventsEnabledRequest },
                { "help", this.DispatchHelpRequest },

                { "admin.runScript", this.DispatchSecureSafeListedRequest },
                { "punkBuster.pb_sv_command", this.DispatchPunkbusterRequest },
                { "serverInfo", this.DispatchUnsecureSafeListedRequest },
                { "admin.say", this.DispatchSecureSafeListedRequest },
                { "admin.yell", this.DispatchSecureSafeListedRequest },
                
                { "admin.runNextLevel", this.DispatchUseMapFunctionRequest },
                { "admin.currentLevel", this.DispatchSecureSafeListedRequest },
                { "admin.restartMap", this.DispatchUseMapFunctionRequest },
                { "admin.supportedMaps", this.DispatchSecureSafeListedRequest },
                { "admin.setPlaylist", this.DispatchAlterMaplistRequest },
                { "admin.getPlaylist", this.DispatchSecureSafeListedRequest },
                { "admin.getPlaylists", this.DispatchSecureSafeListedRequest },
                { "admin.listPlayers", this.DispatchSecureSafeListedRequest },
                { "listPlayers", this.DispatchSecureSafeListedRequest },
                { "admin.endRound", this.DispatchUseMapFunctionRequest },

                { "admin.runNextRound", this.DispatchUseMapFunctionRequest },
                { "admin.restartRound", this.DispatchUseMapFunctionRequest },

                { "banList.add", this.DispatchBanListAddRequest },
                { "banList.remove", this.DispatchAlterBanListRequest },
                { "banList.clear", this.DispatchAlterBanListRequest },
                { "banList.save", this.DispatchAlterBanListRequest },
                { "banList.load", this.DispatchAlterBanListRequest },
                { "banList.list", this.DispatchSecureSafeListedRequest },
                
                { "textChatModerationList.addPlayer", this.DispatchAlterTextMonderationListRequest },
                { "textChatModerationList.removePlayer", this.DispatchAlterTextMonderationListRequest },
                { "textChatModerationList.clear", this.DispatchAlterTextMonderationListRequest },
                { "textChatModerationList.save", this.DispatchAlterTextMonderationListRequest },
                { "textChatModerationList.load", this.DispatchAlterTextMonderationListRequest },
                { "textChatModerationList.list", this.DispatchSecureSafeListedRequest },

                #region Maplist

                { "mapList.configFile", this.DispatchAlterMaplistRequest },
                { "mapList.load", this.DispatchAlterMaplistRequest },
                { "mapList.save", this.DispatchAlterMaplistRequest },
                { "mapList.list", this.DispatchSecureSafeListedRequest },
                { "mapList.clear", this.DispatchAlterMaplistRequest },
                { "mapList.append", this.DispatchAlterMaplistRequest },
                { "mapList.nextLevelIndex", this.DispatchUseMapFunctionRequest },
                { "mapList.remove", this.DispatchAlterMaplistRequest },
                { "mapList.insert", this.DispatchAlterMaplistRequest },

                #endregion

                #region Configuration

                { "vars.adminPassword", this.DispatchVarsAdminPasswordRequest },
                { "vars.gamePassword", this.DispatchVarsRequest },
                { "vars.punkBuster", this.DispatchVarsRequest },
                { "vars.ranked", this.DispatchVarsRequest },
                { "vars.rankLimit", this.DispatchVarsRequest },
                { "vars.profanityFilter", this.DispatchVarsRequest },
                { "vars.idleTimeout", this.DispatchVarsRequest },
                { "vars.playerLimit", this.DispatchVarsRequest },
                { "vars.currentPlayerLimit", this.DispatchVarsRequest },
                { "vars.maxPlayerLimit", this.DispatchVarsRequest },

                #endregion

                #region Details

                { "vars.serverName", this.DispatchVarsRequest },
                { "vars.bannerUrl", this.DispatchVarsRequest },
                { "vars.serverDescription", this.DispatchVarsRequest },

                #endregion

                #region Gameplay

                { "vars.hardCore", this.DispatchVarsRequest },
                { "vars.friendlyFire", this.DispatchVarsRequest },

                #endregion

                #region Team Killing

                { "vars.teamKillCountForKick", this.DispatchVarsRequest },
                { "vars.teamKillValueForKick", this.DispatchVarsRequest },
                { "vars.teamKillValueIncrease", this.DispatchVarsRequest },
                { "vars.teamKillValueDecreasePerSecond", this.DispatchVarsRequest },

                #endregion

                #region Text Chat Moderation

                { "vars.textChatModerationMode", this.DispatchVarsRequest },
                { "vars.textChatSpamTriggerCount", this.DispatchVarsRequest },
                { "vars.textChatSpamDetectionTime", this.DispatchVarsRequest },
                { "vars.textChatSpamCoolDownTime", this.DispatchVarsRequest },

                #endregion

                #region Level Variables

                { "levelVars.set", this.DispatchVarsRequest },
                { "levelVars.get", this.DispatchVarsRequest },
                { "levelVars.evaluate", this.DispatchVarsRequest },
                { "levelVars.clear", this.DispatchVarsRequest },
                { "levelVars.list", this.DispatchSecureSafeListedRequest },

                #endregion

                { "admin.kickPlayer", this.DispatchAdminKickPlayerRequest },
                { "admin.killPlayer", this.DispatchAdminKillPlayerRequest },
                { "admin.movePlayer", this.DispatchAdminMovePlayerRequest },

                { "admin.shutDown", this.DispatchAdminShutDownRequest },
            };

            this.m_connection.PacketReceived += new FrostbiteLayerConnection.PacketDispatchHandler(m_connection_PacketReceived);
            this.m_connection.ConnectionClosed += new FrostbiteLayerConnection.EmptyParameterHandler(m_connection_ConnectionClosed);
        }


        protected virtual void DispatchPunkbusterRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestPacketPunkbusterRecieved != null) {
                FrostbiteConnection.RaiseEvent(this.RequestPacketPunkbusterRecieved.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        protected virtual void DispatchUseMapFunctionRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestPacketUseMapFunctionRecieved != null) {
                FrostbiteConnection.RaiseEvent(this.RequestPacketUseMapFunctionRecieved.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        protected virtual void DispatchBanListAddRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {

            if (cpRecievedPacket.Words.Count >= 4) {

                CBanInfo newBan = new CBanInfo(cpRecievedPacket.Words[1], cpRecievedPacket.Words[2], new TimeoutSubset(cpRecievedPacket.Words.GetRange(3, TimeoutSubset.RequiredLength(cpRecievedPacket.Words[3]))), cpRecievedPacket.Words.Count >= (4 + TimeoutSubset.RequiredLength(cpRecievedPacket.Words[3])) ? cpRecievedPacket.Words[(3 + TimeoutSubset.RequiredLength(cpRecievedPacket.Words[3]))] : "");

                if (this.RequestBanListAddRecieved != null) {
                    FrostbiteConnection.RaiseEvent(this.RequestBanListAddRecieved.GetInvocationList(), this, cpRecievedPacket, newBan);
                }
            }
        }

        protected virtual void DispatchAlterTextMonderationListRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestPacketAlterTextMonderationListRecieved != null) {
                FrostbiteConnection.RaiseEvent(this.RequestPacketAlterTextMonderationListRecieved.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        protected virtual void DispatchAlterBanListRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestPacketAlterBanListRecieved != null) {
                FrostbiteConnection.RaiseEvent(this.RequestPacketAlterBanListRecieved.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        protected virtual void DispatchAlterReservedSlotsListRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestPacketAlterReservedSlotsListRecieved != null) {
                FrostbiteConnection.RaiseEvent(this.RequestPacketAlterReservedSlotsListRecieved.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        protected virtual void DispatchAlterMaplistRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestPacketAlterMaplistRecieved != null) {
                FrostbiteConnection.RaiseEvent(this.RequestPacketAlterMaplistRecieved.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        protected virtual void DispatchVarsAdminPasswordRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            this.SendResponse(cpRecievedPacket, "UnknownCommand");
        }

        protected virtual void DispatchUnsecureSafeListedRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestPacketUnsecureSafeListedRecieved != null) {
                FrostbiteConnection.RaiseEvent(this.RequestPacketUnsecureSafeListedRecieved.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        protected virtual void DispatchVarsRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {

            if (cpRecievedPacket.Words.Count == 1) {
                if (this.RequestPacketSecureSafeListedRecieved != null) {
                    FrostbiteConnection.RaiseEvent(this.RequestPacketSecureSafeListedRecieved.GetInvocationList(), this, cpRecievedPacket);
                }
            }
            else {
                if (this.RequestPacketVarsRecieved != null) {
                    FrostbiteConnection.RaiseEvent(this.RequestPacketVarsRecieved.GetInvocationList(), this, cpRecievedPacket);
                }
            }
        }

        protected virtual void DispatchSecureSafeListedRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestPacketSecureSafeListedRecieved != null) {
                FrostbiteConnection.RaiseEvent(this.RequestPacketSecureSafeListedRecieved.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        #region Player Events

        protected virtual void DispatchAdminKickPlayerRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestPacketAdminKickPlayerRecieved != null) {
                FrostbiteConnection.RaiseEvent(this.RequestPacketAdminKickPlayerRecieved.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        protected virtual void DispatchAdminMovePlayerRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {

            if (this.RequestPacketAdminPlayerMoveRecieved != null) {
                FrostbiteConnection.RaiseEvent(this.RequestPacketAdminPlayerMoveRecieved.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        protected virtual void DispatchAdminKillPlayerRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestPacketAdminPlayerKillRecieved != null) {
                FrostbiteConnection.RaiseEvent(this.RequestPacketAdminPlayerKillRecieved.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        #endregion

        #region Basic Universal Commands

        protected virtual void DispatchLoginPlainTextRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {

            if (cpRecievedPacket.Words.Count >= 2) {
                if (this.RequestLoginPlainText != null) {
                    FrostbiteConnection.RaiseEvent(this.RequestLoginPlainText.GetInvocationList(), this, cpRecievedPacket, cpRecievedPacket.Words[1]);
                }
            }
            else {
                this.SendResponse(cpRecievedPacket, "InvalidArguments");
            }
        }

        protected virtual void DispatchLoginHashedRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (cpRecievedPacket.Words.Count == 1) {
                if (this.RequestLoginHashed != null) {
                    FrostbiteConnection.RaiseEvent(this.RequestLoginHashed.GetInvocationList(), this, cpRecievedPacket);
                }
            }
            else if (cpRecievedPacket.Words.Count >= 2) {
                if (this.RequestLoginHashedPassword != null) {
                    FrostbiteConnection.RaiseEvent(this.RequestLoginHashedPassword.GetInvocationList(), this, cpRecievedPacket, cpRecievedPacket.Words[1]);
                }
            }
            else {
                this.SendResponse(cpRecievedPacket, "InvalidArguments");
            }
        }

        protected virtual void DispatchLogoutRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestLogout != null) {
                FrostbiteConnection.RaiseEvent(this.RequestLogout.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        protected virtual void DispatchQuitRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestQuit != null) {
                FrostbiteConnection.RaiseEvent(this.RequestQuit.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        protected virtual void DispatchEventsEnabledRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestEventsEnabled != null) {
                bool blEnabled = true;

                if (cpRecievedPacket.Words.Count == 2 && bool.TryParse(cpRecievedPacket.Words[1], out blEnabled) == true) {
                    FrostbiteConnection.RaiseEvent(this.RequestEventsEnabled.GetInvocationList(), this, cpRecievedPacket, blEnabled);
                }
                else {
                    this.SendResponse(cpRecievedPacket, "InvalidArguments");
                }
            }
        }

        protected virtual void DispatchHelpRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestHelp != null) {
                FrostbiteConnection.RaiseEvent(this.RequestHelp.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        protected virtual void DispatchAdminShutDownRequest(FrostbiteLayerConnection sender, Packet cpRecievedPacket) {
            if (this.RequestPacketAdminShutdown != null) {
                FrostbiteConnection.RaiseEvent(this.RequestPacketAdminShutdown.GetInvocationList(), this, cpRecievedPacket);
            }
        }

        #endregion

        public virtual void DispatchRequestPacket(FrostbiteLayerConnection sender, Packet cpRequestPacket) {
            if (cpRequestPacket.Words.Count >= 1) {
                if (this.m_requestDelegates.ContainsKey(cpRequestPacket.Words[0]) == true) {
                    this.m_requestDelegates[cpRequestPacket.Words[0]](sender, cpRequestPacket);
                }
                else {
                    if (this.RequestPacketUnknownRecieved != null) {
                        FrostbiteConnection.RaiseEvent(this.RequestPacketUnknownRecieved.GetInvocationList(), this, cpRequestPacket);
                    }
                }
            }
        }

        private void m_connection_PacketReceived(FrostbiteLayerConnection sender, Packet packet) {

            if (packet.OriginatedFromServer == false && packet.IsResponse == false) {

                this.DispatchRequestPacket(sender, packet);
            }
            //else if (packet.OriginatedFromServer == true && packet.IsResponse == true) {
            //  Response to an event we sent.  We just accept these without processing them.  Should always be "OK".  
            //}
        }

        private void m_connection_ConnectionClosed(FrostbiteLayerConnection sender) {
            if (this.ConnectionClosed != null) {
                FrostbiteConnection.RaiseEvent(this.ConnectionClosed.GetInvocationList(), this);
            }
        }

        public void SendPacket(Packet packet) {
            if (this.m_connection != null) {
                this.m_connection.SendAsync(packet);
            }
        }

        public void SendRequest(List<string> words) {
            if (this.m_connection != null) {
                this.m_connection.SendAsync(new Packet(true, false, this.m_connection.AcquireSequenceNumber, words));
            }
        }

        public void SendRequest(params string[] words) {
            this.SendRequest(new List<string>(words));
        }

        public void SendResponse(Packet packet, List<string> words) {

            if (this.m_connection != null) {
                this.m_connection.SendAsync(new Packet(false, true, packet.SequenceNumber, words));
            }
        }

        public void SendResponse(Packet packet, params string[] words) {
            this.SendResponse(packet, new List<string>(words));
        }

        public void Shutdown() {

            if (this.m_connection != null) {
                this.m_connection.Shutdown();
                this.m_connection = null;
            }
        }
    }
}
