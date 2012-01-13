using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using PRoCon;

namespace PRoCon.Forms {
    using Core;
    using Core.Remote;
    
    public partial class BasicConsole : Form {

        public delegate PRoConApplication WindowLoadedHandler(bool execute);
        public event WindowLoadedHandler WindowLoaded;

        private PRoConApplication m_praApplication;

        public BasicConsole() {
            InitializeComponent();
        }

        private void BasicConsole_Load(object sender, EventArgs e) {

            this.m_praApplication = this.WindowLoaded(false);
            this.m_praApplication.Connections.ConnectionAdded += new ConnectionDictionary.ConnectionAlteredHandler(Connections_ConnectionAdded);
            this.m_praApplication.Execute();

            if (this.m_praApplication.CustomTitle.Length > 0) {
                this.Text = this.m_praApplication.CustomTitle;
            }
        }

        private void Connections_ConnectionAdded(PRoConClient item) {
            item.GameTypeDiscovered += new PRoConClient.EmptyParamterHandler(item_GameTypeDiscovered);
            
        }

        private void item_GameTypeDiscovered(PRoConClient sender) {
            sender.ConnectionClosed += new PRoConClient.EmptyParamterHandler(sender_ConnectionClosed);
            sender.ConnectionFailure += new PRoConClient.FailureHandler(sender_ConnectionFailure);
            sender.ConnectSuccess += new PRoConClient.EmptyParamterHandler(sender_ConnectSuccess);
            sender.Login += new PRoConClient.EmptyParamterHandler(sender_Login);
            sender.LoginAttempt += new PRoConClient.EmptyParamterHandler(sender_LoginAttempt);
            sender.LoginFailure += new PRoConClient.AuthenticationFailureHandler(sender_LoginFailure);
            sender.Logout += new PRoConClient.EmptyParamterHandler(sender_Logout);
        }

        private void UpdateConnectionsLabel() {

            StringBuilder builder = new StringBuilder();

            foreach (PRoConClient client in this.m_praApplication.Connections) {

                if (client.State == ConnectionState.Connected && client.IsLoggedIn == true) {
                    builder.AppendFormat("{0,15}: {1}\r\n", "LoggedIn", client.HostNamePort);
                }
                else if (client.State == ConnectionState.Connected) {
                    builder.AppendFormat("{0,15}: {1}\r\n", "Connected", client.HostNamePort);
                }
                else if (client.State == ConnectionState.Connecting) {
                    builder.AppendFormat("{0,15}: {1}\r\n", "Connecting", client.HostNamePort);
                }
                else if (client.State == ConnectionState.Error) {
                    builder.AppendFormat("{0,15}: {1}\r\n", "Connection Error", client.HostNamePort);
                }
                else {
                    builder.AppendFormat("{0,15}: {1}\r\n", "Disconnected", client.HostNamePort);
                }
            }

            this.label1.Text = builder.ToString();
        }

        void sender_Logout(PRoConClient sender) {
            this.UpdateConnectionsLabel();
        }

        void sender_LoginAttempt(PRoConClient sender) {
            this.UpdateConnectionsLabel();
        }

        void sender_Login(PRoConClient sender) {
            this.UpdateConnectionsLabel();
        }

        void sender_ConnectSuccess(PRoConClient sender) {
            this.UpdateConnectionsLabel();
        }

        void sender_ConnectionFailure(PRoConClient sender, Exception exception) {
            this.UpdateConnectionsLabel();
        }

        void sender_ConnectionClosed(PRoConClient sender) {
            this.UpdateConnectionsLabel();
        }

        void sender_LoginFailure(PRoConClient sender, string strError) {
            this.UpdateConnectionsLabel();
        }
                
        private void BasicConsole_FormClosing(object sender, FormClosingEventArgs e) {
            this.m_praApplication.Shutdown();
        }
    }
}
