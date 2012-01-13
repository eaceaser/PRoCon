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
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace PRoCon.Core.Remote {
    public class FrostbiteConnection {

        private TcpClient m_tcpClient;
        private NetworkStream m_nwsStream;

        // Maximum amount of data to accept before scrapping the whole lot and trying again.
        // Test maximizing this to see if plugin descriptions are causing some problems.
        private static readonly UInt32 MAX_GARBAGE_BYTES = 4194304;
        private static readonly UInt16 BUFFER_SIZE = 16384;

        protected Dictionary<UInt32, Packet> m_dicSentPackets;

        private byte[] a_bReceivedBuffer;
        private byte[] a_bPacketStream;

        private Queue<Packet> m_quePackets;// = new Queue<Packet>();

        private UInt32 m_ui32SequenceNumber;
        public UInt32 AcquireSequenceNumber {
            get {
                lock (new object()) {
                    return ++this.m_ui32SequenceNumber;
                }
            }
        }

        public string Hostname {
            get;
            private set;
        }

        public UInt16 Port {
            get;
            private set;
        }

        public bool IsConnected {
            get {
                return this.m_tcpClient == null ? false : this.m_tcpClient.Connected;
            }
        }

        public bool IsConnecting {
            get {
                return this.m_tcpClient == null ? false : true ^ this.m_tcpClient.Connected;
            }
        }

        #region Events

        public delegate void PrePacketDispatchedHandler(FrostbiteConnection sender, Packet packetBeforeDispatch, out bool isProcessed);
        public event PrePacketDispatchedHandler BeforePacketDispatch;
        public event PrePacketDispatchedHandler BeforePacketSend;

        public delegate void PacketDispatchHandler(FrostbiteConnection sender, bool isHandled, Packet packet);
        public event PacketDispatchHandler PacketSent;
        public event PacketDispatchHandler PacketReceived;

        public delegate void SocketExceptionHandler(FrostbiteConnection sender, SocketException se);
        public event SocketExceptionHandler SocketException;

        public delegate void FailureHandler(FrostbiteConnection sender, Exception exception);
        public event FailureHandler ConnectionFailure;

        public delegate void PacketQueuedHandler(FrostbiteConnection sender, Packet cpPacket, int iThreadId);
        public event PacketQueuedHandler PacketQueued;
        public event PacketQueuedHandler PacketDequeued;

        public delegate void EmptyParamterHandler(FrostbiteConnection sender);
        public event EmptyParamterHandler ConnectAttempt;
        public event EmptyParamterHandler ConnectSuccess;
        public event EmptyParamterHandler ConnectionClosed;
        public event EmptyParamterHandler ConnectionReady;

        #endregion

        public FrostbiteConnection(string hostname, UInt16 port) {
            this.ClearConnection();

            this.Hostname = hostname;
            this.Port = port;
        }

        private void ClearConnection() {
            this.m_ui32SequenceNumber = 0;

            this.m_dicSentPackets = new Dictionary<UInt32, Packet>();
            this.m_quePackets = new Queue<Packet>();
            
            this.a_bReceivedBuffer = new byte[FrostbiteConnection.BUFFER_SIZE];
            this.a_bPacketStream = null;
        }

        // TO DO: Move out of FrostbiteClient.
        public static void RaiseEvent(Delegate[] a_delInvokes, params object[] a_objArguments) {
            for (int i = 0; i < a_delInvokes.Length; i++) {

                try {
                    if (a_delInvokes[i].Target is Form) {
                        if (((Form)a_delInvokes[i].Target).Disposing == false && ((Form)a_delInvokes[i].Target).IsDisposed == false && ((Form)a_delInvokes[i].Target).IsHandleCreated == true) {
                            try {
                                ((Control)a_delInvokes[i].Target).Invoke(a_delInvokes[i], a_objArguments);
                            }
                            catch (InvalidOperationException) { }
                        }
                    }
                    else if (a_delInvokes[i].Target is Control) {
                        if (((Control)a_delInvokes[i].Target).Disposing == false && ((Control)a_delInvokes[i].Target).IsDisposed == false) {//
                            try {
                                ((Control)a_delInvokes[i].Target).Invoke(a_delInvokes[i], a_objArguments);
                            }
                            catch (InvalidOperationException) { }
                        }
                    }
                    else {
                        //a_delInvokes[i].Method.Invoke(a_delInvokes[i].Target, a_objArguments);
                        a_delInvokes[i].DynamicInvoke(a_objArguments);
                    }
                }
                catch (Exception e) {

                    string strParams = String.Empty;

                    if (a_objArguments != null) {
                        foreach (object objParam in a_objArguments) {
                            strParams += ("---" + objParam.ToString());
                        }
                    }

                    FrostbiteConnection.LogError(String.Format("{0} ::: {1} ::: {2}", a_delInvokes[i].Target.ToString(), a_delInvokes[i].Method.ToString(), strParams), String.Empty, e);
                }
            }
        }

        public static void LogError(string strPacket, string strAdditional, Exception e) {
            try {
                string strOutput = "=======================================" + Environment.NewLine + Environment.NewLine;

                StackTrace stTracer = new StackTrace(e, true);
                strOutput += "Exception caught at: " + Environment.NewLine;
                strOutput += String.Format("{0}{1}", stTracer.GetFrame((stTracer.FrameCount - 1)).GetFileName(), Environment.NewLine);
                strOutput += String.Format("Line {0}{1}", stTracer.GetFrame((stTracer.FrameCount - 1)).GetFileLineNumber(), Environment.NewLine);
                strOutput += String.Format("Method {0}{1}", stTracer.GetFrame((stTracer.FrameCount - 1)).GetMethod().Name, Environment.NewLine);

                strOutput += "DateTime: " + DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString() + Environment.NewLine;
                strOutput += "Version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + Environment.NewLine;

                strOutput += "Packet: " + Environment.NewLine;
                strOutput += strPacket + Environment.NewLine;

                strOutput += "Additional: " + Environment.NewLine;
                strOutput += strAdditional + Environment.NewLine;

                strOutput += Environment.NewLine;
                strOutput += e.Message + Environment.NewLine;

                strOutput += Environment.NewLine;
                strOutput += stTracer.ToString();

                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "DEBUG.txt")) {
                    using (StreamWriter sw = File.CreateText(AppDomain.CurrentDomain.BaseDirectory + "DEBUG.txt")) {
                        sw.Write(strOutput);
                    }
                }
                else {
                    using (StreamWriter sw = File.AppendText(AppDomain.CurrentDomain.BaseDirectory + "DEBUG.txt")) {
                        sw.Write(strOutput);
                    }
                }
            }
            catch (Exception ex) {
                // It'd be to ironic to happen, surely?
            }
        }

        private bool QueueUnqueuePacket(bool blSendingPacket, Packet cpPacket, out Packet cpNextPacket) {
            cpNextPacket = null;
            bool blResponse = false;

            lock (new object()) {

                if (blSendingPacket == true) {
                    if (this.m_dicSentPackets.Count > 0) {
                        this.m_quePackets.Enqueue(cpPacket);
                        if (this.PacketQueued != null) {
                            FrostbiteConnection.RaiseEvent(this.PacketQueued.GetInvocationList(), this, cpPacket, Thread.CurrentThread.ManagedThreadId);
                            //this.PacketQueued(cpPacket, Thread.CurrentThread.ManagedThreadId);
                        }
                        blResponse = true;
                    }
                    else {
                        if (this.m_dicSentPackets.Count == 0 && this.m_quePackets.Count > 0) {
                            // TODO: I've seen it slip in here once, but that was when I had
                            // combined the events and commands streams.  Have not seen it since, but need to make sure.

                            //throw new Exception();
                        }
                        else {
                            // No packets waiting for response, free to send the new packet.
                            blResponse = false;
                        }
                    }
                }
                else {
                    // Else it's being called from recv and cpPacket holds the processed RequestPacket.

                    // Remove the packet 
                    if (cpPacket != null) {
                        if (this.m_dicSentPackets.ContainsKey(cpPacket.SequenceNumber) == true) {
                            this.m_dicSentPackets.Remove(cpPacket.SequenceNumber);
                        }
                    }

                    if (this.m_quePackets.Count > 0) {
                        cpNextPacket = this.m_quePackets.Dequeue();
                        if (this.PacketDequeued != null) {
                            FrostbiteConnection.RaiseEvent(this.PacketDequeued.GetInvocationList(), this, cpNextPacket, Thread.CurrentThread.ManagedThreadId);
                            //this.PacketDequeued(cpNextPacket, Thread.CurrentThread.ManagedThreadId);
                        }
                        blResponse = true;
                    }
                    else {
                        blResponse = false;
                    }

                }

                return blResponse;
            }
        }

        private void SendAsyncCallback(IAsyncResult ar) {

            Packet pSentPacket = (Packet)ar.AsyncState;

            try {
                if (this.m_nwsStream != null) {
                    this.m_nwsStream.EndWrite(ar);
                    if (this.PacketSent != null) {
                        //spcData.rconParent.PacketSent(spcData.cpSentPacket, true);
                        FrostbiteConnection.RaiseEvent(this.PacketSent.GetInvocationList(), this, false, pSentPacket);
                    }
                }
            }
            catch (SocketException se) {
                this.Shutdown(se);
            }
            catch (Exception e) {
                this.Shutdown(e);
            }
        }

        // Send straight away ignoring the queue
        private void SendAsync(Packet cpPacket) {
            try {

                bool isProcessed = false;

                if (this.BeforePacketSend != null) {
                    this.BeforePacketSend(this, cpPacket, out isProcessed);
                }

                if (isProcessed == false && this.m_nwsStream != null) {

                    byte[] a_bBytePacket = cpPacket.EncodePacket();

                    if (cpPacket.OriginatedFromServer == false && cpPacket.IsResponse == false && this.m_dicSentPackets.ContainsKey(cpPacket.SequenceNumber) == false) {
                        this.m_dicSentPackets.Add(cpPacket.SequenceNumber, cpPacket);
                    }

                    this.m_nwsStream.BeginWrite(a_bBytePacket, 0, a_bBytePacket.Length, this.SendAsyncCallback, cpPacket);

                }
            }
            catch (SocketException se) {
                this.Shutdown(se);
            }
            catch (Exception e) {
                this.Shutdown(e);
            }
        }

        // Queue for sending.
        public void SendQueued(Packet cpPacket) {
            // QueueUnqueuePacket
            Packet cpNullPacket = null;

            if (cpPacket.OriginatedFromServer == true && cpPacket.IsResponse == true) {
                this.SendAsync(cpPacket);
            }
            else {
                // Null return because we're not popping a packet, just checking to see if this one needs to be queued.
                if (this.QueueUnqueuePacket(true, cpPacket, out cpNullPacket) == false) {
                    // No need to queue, queue is empty.  Send away..
                    this.SendAsync(cpPacket);
                }
            }
        }

        public Packet GetRequestPacket(Packet cpRecievedPacket) {

            Packet cpRequestPacket = null;

            if (this.m_dicSentPackets.ContainsKey(cpRecievedPacket.SequenceNumber) == true) {
                cpRequestPacket = this.m_dicSentPackets[cpRecievedPacket.SequenceNumber];
            }

            return cpRequestPacket;
        }

        private void ReceiveCallback(IAsyncResult ar) {

            try {
                int iBytesRead = this.m_nwsStream.EndRead(ar);

                if (iBytesRead > 0) {

                    // Create or resize our packet stream to hold the new data.
                    if (this.a_bPacketStream == null) {
                        this.a_bPacketStream = new byte[iBytesRead];
                    }
                    else {
                        Array.Resize(ref this.a_bPacketStream, this.a_bPacketStream.Length + iBytesRead);
                    }

                    Array.Copy(this.a_bReceivedBuffer, 0, this.a_bPacketStream, this.a_bPacketStream.Length - iBytesRead, iBytesRead);

                    UInt32 ui32PacketSize = Packet.DecodePacketSize(this.a_bPacketStream);

                    while (this.a_bPacketStream.Length >= ui32PacketSize
                        && this.a_bPacketStream.Length > Packet.INT_PACKET_HEADER_SIZE) {

                        // Copy the complete packet from the beginning of the stream.
                        byte[] a_bCompletePacket = new byte[ui32PacketSize];
                        Array.Copy(this.a_bPacketStream, a_bCompletePacket, ui32PacketSize);

                        Packet cpCompletePacket = new Packet(a_bCompletePacket);
                        //cbfConnection.m_ui32SequenceNumber = Math.Max(cbfConnection.m_ui32SequenceNumber, cpCompletePacket.SequenceNumber) + 1;

                        // Dispatch the completed packet.
                        try {
                            bool isProcessed = false;

                            if (this.BeforePacketDispatch != null) {
                                this.BeforePacketDispatch(this, cpCompletePacket, out isProcessed);
                            }

                            if (this.PacketReceived != null) {
                                FrostbiteConnection.RaiseEvent(this.PacketReceived.GetInvocationList(), this, isProcessed, cpCompletePacket);
                            }

                            if (cpCompletePacket.OriginatedFromServer == true && cpCompletePacket.IsResponse == false) {
                                this.SendAsync(new Packet(true, true, cpCompletePacket.SequenceNumber, "OK"));
                            }

                            Packet cpNextPacket = null;
                            if (this.QueueUnqueuePacket(false, cpCompletePacket, out cpNextPacket) == true) {
                                this.SendAsync(cpNextPacket);
                            }
                        }
                        catch (Exception e) {

                            Packet cpRequest = this.GetRequestPacket(cpCompletePacket);

                            if (cpRequest != null) {
                                LogError(cpCompletePacket.ToDebugString(), cpRequest.ToDebugString(), e);
                            }
                            else {
                                LogError(cpCompletePacket.ToDebugString(), String.Empty, e);
                            }

                            // Now try to recover..
                            Packet cpNextPacket = null;
                            if (this.QueueUnqueuePacket(false, cpCompletePacket, out cpNextPacket) == true) {
                                this.SendAsync(cpNextPacket);
                            }
                        }

                        // Now remove the completed packet from the beginning of the stream
                        byte[] a_bUpdatedSteam = new byte[this.a_bPacketStream.Length - ui32PacketSize];
                        Array.Copy(this.a_bPacketStream, ui32PacketSize, a_bUpdatedSteam, 0, this.a_bPacketStream.Length - ui32PacketSize);
                        this.a_bPacketStream = a_bUpdatedSteam;

                        ui32PacketSize = Packet.DecodePacketSize(this.a_bPacketStream);
                    }

                    // If we've recieved the maxmimum garbage, scrap it all and shutdown the connection.
                    // We went really wrong somewhere =)
                    if (this.a_bReceivedBuffer.Length >= FrostbiteConnection.MAX_GARBAGE_BYTES) {
                        this.a_bReceivedBuffer = null; // GC.collect()
                        this.Shutdown(new Exception("Exceeded maximum garbage packet"));
                    }
                }

                if (iBytesRead == 0) {
                    this.Shutdown();
                    return;
                }

                if (this.m_nwsStream != null) {

                    IAsyncResult result = this.m_nwsStream.BeginRead(this.a_bReceivedBuffer, 0, this.a_bReceivedBuffer.Length, this.ReceiveCallback, this);

                    if (result.AsyncWaitHandle.WaitOne(180000, false) == false) {
                        //if (this.ConnectionFailure != null) {
                        //    FrostbiteConnection.RaiseEvent(this.ConnectionFailure.GetInvocationList(), this, new Exception("Events connection has timed out after two minutes without data."));
                        //}

                        this.Shutdown(new Exception("Events connection has timed out after two minutes without data."));
                    }
                }
            }
            catch (SocketException se) {
                this.Shutdown(se);
            }
            catch (Exception e) {
                this.Shutdown(e);
            }
        }

        private void ConnectedCallback(IAsyncResult ar) {

            try {
                this.m_tcpClient.EndConnect(ar);
                this.m_tcpClient.NoDelay = true;

                if (this.ConnectSuccess != null) {
                    FrostbiteConnection.RaiseEvent(this.ConnectSuccess.GetInvocationList(), this);
                }

                this.m_nwsStream = this.m_tcpClient.GetStream();
                this.m_nwsStream.BeginRead(this.a_bReceivedBuffer, 0, this.a_bReceivedBuffer.Length, this.ReceiveCallback, this);

                if (this.ConnectionReady != null) {
                    FrostbiteConnection.RaiseEvent(this.ConnectionReady.GetInvocationList(), this);
                }
            }
            catch (SocketException se) {
                this.Shutdown(se);
            }
            catch (Exception e) {
                this.Shutdown(e);
            }
        }

        public static IPAddress ResolveHostName(string hostName) {
            IPAddress ipReturn = IPAddress.None;

            if (IPAddress.TryParse(hostName, out ipReturn) == false) {

                ipReturn = IPAddress.None;

                try {
                    IPHostEntry iphHost = Dns.GetHostEntry(hostName);

                    if (iphHost.AddressList.Length > 0) {
                        ipReturn = iphHost.AddressList[0];
                    }
                    // ELSE return IPAddress.None..
                }
                catch (Exception) { } // Returns IPAddress.None..
            }

            return ipReturn;
        }

        public void AttemptConnection() {
            try {

                this.m_quePackets.Clear();
                this.m_dicSentPackets.Clear();
                this.m_ui32SequenceNumber = 0;

                this.m_tcpClient = new TcpClient();
                this.m_tcpClient.NoDelay = true;
                this.m_tcpClient.BeginConnect(this.Hostname, this.Port, this.ConnectedCallback, this);

                if (this.ConnectAttempt != null) {
                    FrostbiteConnection.RaiseEvent(this.ConnectAttempt.GetInvocationList(), this);
                }
            }
            catch (SocketException se) {
                this.Shutdown(se);
            }
            catch (Exception e) {
                this.Shutdown(e);
            }
        }

        public void Shutdown(Exception e) {
            if (this.m_tcpClient != null) {
                this.ShutdownConnection();

                if (this.ConnectionClosed != null) {
                    FrostbiteConnection.RaiseEvent(this.ConnectionClosed.GetInvocationList(), this);
                }

                if (this.ConnectionFailure != null) {
                    FrostbiteConnection.RaiseEvent(this.ConnectionFailure.GetInvocationList(), this, e);
                }
            }
        }

        public void Shutdown(SocketException se) {
            if (this.m_tcpClient != null) {
                this.ShutdownConnection();

                if (this.ConnectionClosed != null) {
                    FrostbiteConnection.RaiseEvent(this.ConnectionClosed.GetInvocationList(), this);
                }

                if (this.SocketException != null) {
                    FrostbiteConnection.RaiseEvent(this.SocketException.GetInvocationList(), this, se);
                }
            }
        }

        public void Shutdown() {
            if (this.m_tcpClient != null) {
                this.ShutdownConnection();

                if (this.ConnectionClosed != null) {
                    FrostbiteConnection.RaiseEvent(this.ConnectionClosed.GetInvocationList(), this);
                }
            }
        }

        private void ShutdownConnection() {

            lock (new object()) {
                if (this.m_tcpClient != null) {

                    try {

                        this.ClearConnection();

                        if (this.m_nwsStream != null) {
                            this.m_nwsStream.Close();
                            this.m_nwsStream.Dispose();
                            this.m_nwsStream = null;
                        }

                        if (this.m_tcpClient != null) {
                            this.m_tcpClient.Close();
                            this.m_tcpClient = null;
                        }
                    }
                    catch (SocketException se) {
                        if (this.SocketException != null) {
                            FrostbiteConnection.RaiseEvent(this.SocketException.GetInvocationList(), this, se);
                            //this.SocketException(se);
                        }
                    }
                    catch (Exception) { }
                }
            }
        }
    }
}
