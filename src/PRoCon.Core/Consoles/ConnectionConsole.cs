using System;
using System.Collections.Generic;
using System.Text;

// This class will move to .Core once ProConClient is in .Core.
namespace PRoCon.Core.Consoles {
    using Core;
    using Core.Logging;
    using Core.Remote;
    public class ConnectionConsole : Loggable {

        public event Loggable.WriteConsoleHandler WriteConsole;

        public delegate void IsEnabledHandler(bool isEnabled);
        public event IsEnabledHandler LogEventsConnectionChanged;
        public event IsEnabledHandler LogDebugDetailsChanged;
        public event IsEnabledHandler DisplayConnectionChanged;
        public event IsEnabledHandler DisplayPunkbusterChanged;
        public event IsEnabledHandler ConScrollingChanged;
        public event IsEnabledHandler PBScrollingChanged;

        private PRoConClient m_prcClient;

        public UInt32 BytesRecieved {
            get;
            private set;
        }

        public UInt32 BytesSent {
            get;
            private set;
        }

        private bool m_blLogEventsConnection;
        public bool LogEventsConnection {
            get {
                return this.m_blLogEventsConnection;
            }
            set {
                if (this.LogEventsConnectionChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.LogEventsConnectionChanged.GetInvocationList(), value);
                }

                this.m_blLogEventsConnection = value;
            }
        }

        private bool m_blLogDebugDetails;
        public bool LogDebugDetails {
            get {
                return this.m_blLogDebugDetails;
            }
            set {
                if (this.LogDebugDetailsChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.LogDebugDetailsChanged.GetInvocationList(), value);
                }

                this.m_blLogDebugDetails = value;
            }
        }

        private bool m_blDisplayConnection;
        public bool DisplayConnection {
            get {
                return this.m_blDisplayConnection;
            }
            set {
                if (this.DisplayConnectionChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.DisplayConnectionChanged.GetInvocationList(), value);
                }

                this.m_blDisplayConnection = value;
            }
        }

        private bool m_blDisplayPunkbuster;
        public bool DisplayPunkbuster {
            get {
                return this.m_blDisplayPunkbuster;
            }
            set {
                if (this.DisplayPunkbusterChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.DisplayPunkbusterChanged.GetInvocationList(), value);
                }

                this.m_blDisplayPunkbuster = value;
            }
        }

        private bool m_blConScrolling;
        public bool ConScrolling
        {
            get
            {
                return this.m_blConScrolling;
            }
            set
            {
                if (this.ConScrollingChanged != null)
                {
                    FrostbiteConnection.RaiseEvent(this.ConScrollingChanged.GetInvocationList(), value);
                }

                this.m_blConScrolling = value;
            }
        }

        private bool m_blPBScrolling;
        public bool PBScrolling
        {
            get
            {
                return this.m_blPBScrolling;
            }
            set
            {
                if (this.PBScrollingChanged != null)
                {
                    FrostbiteConnection.RaiseEvent(this.PBScrollingChanged.GetInvocationList(), value);
                }

                this.m_blPBScrolling = value;
            }
        }

        public List<string> Settings
        {
            set {
                bool isEnabled = true;

                if (value.Count >= 1 && bool.TryParse(value[0], out isEnabled) == true) {
                    this.DisplayConnection = isEnabled;
                }

                if (value.Count >= 2 && bool.TryParse(value[1], out isEnabled) == true) {
                    this.LogEventsConnection = isEnabled;
                }

                if (value.Count >= 3 && bool.TryParse(value[2], out isEnabled) == true) {
                    this.LogDebugDetails = isEnabled;
                }

                if (value.Count >= 4 && bool.TryParse(value[3], out isEnabled) == true) {
                    this.DisplayPunkbuster = isEnabled;
                }

                if (value.Count >= 5 && bool.TryParse(value[4], out isEnabled) == true)
                {
                    this.ConScrolling = isEnabled;
                }

                if (value.Count >= 6 && bool.TryParse(value[5], out isEnabled) == true)
                {
                    this.PBScrolling = isEnabled;
                }
            }
            get {
                return new List<string>() { this.DisplayConnection.ToString(), this.LogEventsConnection.ToString(), this.LogDebugDetails.ToString(), this.DisplayPunkbuster.ToString(), this.ConScrolling.ToString(), this.PBScrolling.ToString() };
            }
        }

        public ConnectionConsole(PRoConClient prcClient)
            : base() {

            this.m_prcClient = prcClient;

            this.FileHostNamePort = this.m_prcClient.FileHostNamePort;
            this.LoggingStartedPrefix = "Console logging started";
            this.LoggingStoppedPrefix = "Console logging stopped";
            this.FileNameSuffix = "console";

            this.LogDebugDetails = false;
            this.LogEventsConnection = false;
            this.DisplayConnection = true;
            this.DisplayPunkbuster = true;
            this.ConScrolling = true;
            this.PBScrolling = true;

            this.m_prcClient.Game.Connection.PacketQueued += new FrostbiteConnection.PacketQueuedHandler(m_prcClient_PacketQueued);
            this.m_prcClient.Game.Connection.PacketDequeued += new FrostbiteConnection.PacketQueuedHandler(m_prcClient_PacketDequeued);
            this.m_prcClient.Game.Connection.PacketSent += new FrostbiteConnection.PacketDispatchHandler(m_prcClient_PacketSent);
            this.m_prcClient.Game.Connection.PacketReceived += new FrostbiteConnection.PacketDispatchHandler(m_prcClient_PacketRecieved);

            this.m_prcClient.ConnectAttempt += new PRoConClient.EmptyParamterHandler(m_prcClient_CommandConnectAttempt);
            this.m_prcClient.ConnectSuccess += new PRoConClient.EmptyParamterHandler(m_prcClient_CommandConnectSuccess);

            //this.m_prcClient.EventsConnectAttempt += new FrostbiteConnection.EmptyParamterHandler(m_prcClient_EventsConnectAttempt);
            //this.m_prcClient.EventsConnectSuccess += new FrostbiteConnection.EmptyParamterHandler(m_prcClient_EventsConnectSuccess);

            this.m_prcClient.ConnectionFailure += new PRoConClient.FailureHandler(m_prcClient_ConnectionFailure);
            this.m_prcClient.ConnectionClosed += new PRoConClient.EmptyParamterHandler(m_prcClient_ConnectionClosed);

            this.m_prcClient.LoginAttempt += new PRoConClient.EmptyParamterHandler(m_prcClient_CommandLoginAttempt);
            this.m_prcClient.Login += new PRoConClient.EmptyParamterHandler(m_prcClient_CommandLogin);
            this.m_prcClient.LoginFailure += new PRoConClient.AuthenticationFailureHandler(m_prcClient_CommandLoginFailure);
            this.m_prcClient.Logout += new PRoConClient.EmptyParamterHandler(m_prcClient_CommandLogout);

            //this.m_prcClient.EventsLogin += new FrostbiteConnection.EmptyParamterHandler(m_prcClient_EventsLogin);
            //this.m_prcClient.EventsLoginAttempt += new FrostbiteConnection.EmptyParamterHandler(m_prcClient_EventsLoginAttempt);
        }

        void m_prcClient_PacketRecieved(FrostbiteConnection sender, bool isHandled, Packet packetBeforeDispatch) {

            Packet cpRequestPacket = this.m_prcClient.Game.Connection.GetRequestPacket(packetBeforeDispatch);

            if (packetBeforeDispatch.OriginatedFromServer == false && packetBeforeDispatch.IsResponse == true) {

                if (this.LogDebugDetails == true && cpRequestPacket != null) {

                    if (cpRequestPacket.OriginatedFromServer == false) {
                        this.Write(this.GetDebugPacket("^6Client", "^4", packetBeforeDispatch, cpRequestPacket));
                        //this.Write("^b^6{0,18}^0 {1}\tS:{2}\t^4{3}\t^0(RE: ^2{4}^0)", "Command:", this.GetRequestResponseColour(packetBeforeDispatch), packetBeforeDispatch.SequenceNumber, packetBeforeDispatch.ToDebugString().TrimEnd('\r', '\n').Replace("{", "{{").Replace("}", "}}"), cpRequestPacket.ToString().Replace("{", "{{").Replace("}", "}}"));
                    }
                    else {
                        if (this.LogEventsConnection == true) {
                            this.Write(this.GetDebugPacket("^8Server", "^4", packetBeforeDispatch, cpRequestPacket));
                            //this.Write("^b^8{0,18}^0 {1}\tS:{2}\t^4{3}\t^0(RE: ^2{4}^0)", "Event:", this.GetRequestResponseColour(packetBeforeDispatch), packetBeforeDispatch.SequenceNumber, packetBeforeDispatch.ToDebugString().TrimEnd('\r', '\n').Replace("{", "{{").Replace("}", "}}"), cpRequestPacket.ToString().Replace("{", "{{").Replace("}", "}}"));
                        }
                    }
                }
                else {
                    if ((cpRequestPacket != null && cpRequestPacket.OriginatedFromServer == false) || this.LogEventsConnection == true) {
                        this.Write("^b^4{0}", packetBeforeDispatch.ToString().TrimEnd('\r', '\n').Replace("{", "{{").Replace("}", "}}"));
                    }
                }
            }
            // ELSE IF it's an event initiated by the server (OnJoin, OnLeave, OnChat etc)
            else if (packetBeforeDispatch.OriginatedFromServer == true && packetBeforeDispatch.IsResponse == false) {

                if (this.LogDebugDetails == true) {

                    if (cpRequestPacket != null && cpRequestPacket.OriginatedFromServer == false) {
                        this.Write(this.GetDebugPacket("^6Client", "^4", packetBeforeDispatch, null));
                        //this.Write("^b^6{0,18}^0 {1}\tS:{2}\t^4{3}", "Command:", this.GetRequestResponseColour(packetBeforeDispatch), packetBeforeDispatch.SequenceNumber, packetBeforeDispatch.ToDebugString().Replace("{", "{{").Replace("}", "}}"));
                    }
                    else {
                        if (this.LogEventsConnection == true) {
                            this.Write(this.GetDebugPacket("^8Server", "^4", packetBeforeDispatch, null));
                            //this.Write("^b^8{0,18}^0 {1}\tS:{2}\t^4{3}", "Event:", this.GetRequestResponseColour(packetBeforeDispatch), packetBeforeDispatch.SequenceNumber, packetBeforeDispatch.ToDebugString().Replace("{", "{{").Replace("}", "}}"));
                        }
                    }
                }
                else {
                    if ((cpRequestPacket != null && cpRequestPacket.OriginatedFromServer == false) || this.LogEventsConnection == true) {
                        this.Write("^b^4{0}", packetBeforeDispatch.ToString().TrimEnd('\r', '\n').Replace("{", "{{").Replace("}", "}}"));
                    }
                }

            }

            this.BytesRecieved += packetBeforeDispatch.PacketSize;
        }

        //private void m_prcClient_EventsLoginAttempt() {
        //    this.Write(this.m_prcClient.Language.GetLocalized("uscServerConnection.OnLoginEventsAttempt"));
        //}

        //private void m_prcClient_EventsLogin() {
        //    this.Write("^b^3" + this.m_prcClient.Language.GetLocalized("uscServerConnection.OnLoginEventsSuccess"));
        //}

        private void m_prcClient_CommandLogout(PRoConClient sender) {
            this.Write(this.m_prcClient.Language.GetLocalized("uscServerConnection.OnLogoutSuccess"));
        }

        private void m_prcClient_CommandLoginFailure(PRoConClient sender, string strError) {
            this.Write("^1" + this.m_prcClient.Language.GetLocalized("uscServerConnection.OnLoginAuthenticationFailure"));
        }

        private void m_prcClient_CommandLogin(PRoConClient sender) {
            this.Write("^b^3" + this.m_prcClient.Language.GetLocalized("uscServerConnection.OnLoginSuccess"));
        }

        private void m_prcClient_CommandLoginAttempt(PRoConClient sender) {
            this.Write(this.m_prcClient.Language.GetLocalized("uscServerConnection.OnLoginAttempt"));
        }

        private void m_prcClient_ConnectionFailure(PRoConClient sender, Exception exception) {
            this.Write("^b^1" + this.m_prcClient.Language.GetLocalized("uscServerConnection.OnServerConnectionFailure", exception.Message));
        }

        private void m_prcClient_ConnectionClosed(PRoConClient sender) {
            this.Write(this.m_prcClient.Language.GetLocalized("uscServerConnection.OnServerConnectionClosed", this.m_prcClient.HostNamePort));
        }

        //private void m_prcClient_EventsConnectSuccess() {
        //    this.Write("^b^3" + this.m_prcClient.Language.GetLocalized("uscServerConnection.OnServerEventsConnectionSuccess", this.m_prcClient.HostNamePort));
        //}

        //private void m_prcClient_EventsConnectAttempt() {
        //    this.Write(this.m_prcClient.Language.GetLocalized("uscServerConnection.OnServerEventsConnectionAttempt", this.m_prcClient.HostNamePort));
        //}

        private void m_prcClient_CommandConnectAttempt(PRoConClient sender) {
            this.Write(this.m_prcClient.Language.GetLocalized("uscServerConnection.OnServerCommandConnectionAttempt", this.m_prcClient.HostNamePort));
        }

        private void m_prcClient_CommandConnectSuccess(PRoConClient sender) {
            this.Write("^b^3" + this.m_prcClient.Language.GetLocalized("uscServerConnection.OnServerCommandConnectionSuccess", this.m_prcClient.HostNamePort));
        }

        private void m_prcClient_PacketSent(FrostbiteConnection sender, bool isHandled, Packet packetBeforeDispatch) {

            if (this.LogDebugDetails == true) {

                if (packetBeforeDispatch.OriginatedFromServer == false) {
                    this.Write(this.GetDebugPacket("^6Client", "^2", packetBeforeDispatch, null));

                    //this.Write("^b^6{0,18}^0 {1}\tS:{2}\t^2{3}", "Command:", this.GetRequestResponseColour(packetBeforeDispatch), packetBeforeDispatch.SequenceNumber, packetBeforeDispatch.ToDebugString().TrimEnd('\r', '\n'));
                }
                else {
                    if (this.LogEventsConnection == true) {
                        this.Write(this.GetDebugPacket("^8Server", "^2", packetBeforeDispatch, null));

                        //this.Write("^b^8{0,18}^0 {1}\tS:{2}\t^2{3}", "Event:", this.GetRequestResponseColour(packetBeforeDispatch), packetBeforeDispatch.SequenceNumber, packetBeforeDispatch.ToDebugString().TrimEnd('\r', '\n'));
                    }
                }

            }
            else {
                if (packetBeforeDispatch.OriginatedFromServer == false || this.LogEventsConnection == true) {
                    this.Write("^b^2{0}", packetBeforeDispatch.ToString().TrimEnd('\r', '\n'));
                }
            }

            this.BytesSent += packetBeforeDispatch.PacketSize;
        }

        private void m_prcClient_PacketDequeued(FrostbiteConnection sender, Packet cpPacket, int iThreadId) {
            if (this.LogDebugDetails == true) {
                this.Write(this.GetDebugPacket("^7Dequeued", "^2", cpPacket, null));

                //this.Write("^b^7Dequeued({0}):\t^0 {1}\tS:{2}\t^2{3}", iThreadId, this.GetRequestResponseColour(cpPacket), cpPacket.SequenceNumber, cpPacket.ToDebugString().TrimEnd('\r', '\n'));
            }
        }

        private void m_prcClient_PacketQueued(FrostbiteConnection sender, Packet cpPacket, int iThreadId) {
            if (this.LogDebugDetails == true) {

                this.Write(this.GetDebugPacket("^7Queued", "^2", cpPacket, null));

                //this.Write("^b^7Queued({0}):\t^0 {1}\tS:{2}\t^2{3}", iThreadId, this.GetRequestResponseColour(cpPacket), cpPacket.SequenceNumber, cpPacket.ToDebugString().TrimEnd('\r', '\n'));
            }
        }

        private string GetDebugPacket(string connectionPrefix, string packetColour, Packet packet, Packet requestPacket) {

            string debugString = String.Empty;

            debugString = string.Format("{0,10}: {1,-12} S: {2,-6} {3}{4}", connectionPrefix, this.GetRequestResponseColour(packet), packet.SequenceNumber, packetColour, packet.ToDebugString().Replace("\r", "").Replace("\n", ""));

            if (requestPacket != null) {
                debugString = String.Format("{0} ^0(RE: ^2{1}^0)", debugString, requestPacket.ToDebugString().TrimEnd('\r', '\n'));
            }

            debugString = debugString.Replace("{", "{{").Replace("}", "}}");

            return debugString;
        }

        private string GetRequestResponseColour(Packet packet) {

            string strReturn = String.Empty;

            if (packet.IsResponse == true) {
                strReturn = "^2response^0";
            }
            else {
                strReturn = "^1request^0";
            }

            return strReturn;
        }

        /*
        private string GetTruthColour(bool blValue) {

            string strReturn = String.Empty;

            if (blValue == true) {
                strReturn = "^2true^0";
            }
            else {
                strReturn = "^1false^0";
            }

            return strReturn;
        }*/

        public void Write(string strFormat, params object[] a_objArguments) {

            DateTime dtLoggedTime = DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime();
            string strText = String.Format(strFormat, a_objArguments);

            this.WriteLogLine(String.Format("[{0}] {1}", dtLoggedTime.ToString("HH:mm:ss"), strText.Replace("{", "{{").Replace("}", "}}")));

            if (this.WriteConsole != null) {
                FrostbiteConnection.RaiseEvent(this.WriteConsole.GetInvocationList(), dtLoggedTime, strText);
            }
        }

    }
}
