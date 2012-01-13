using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace PRoCon.Core.Remote.Layer {
    using Core;
    using Core.Accounts;
    using Core.Remote;

    public class PRoConLayer {

        public delegate void LayerEmptyParameterHandler();
        public event LayerEmptyParameterHandler LayerOnline;
        public event LayerEmptyParameterHandler LayerOffline;

        public delegate void LayerSocketErrorHandler(SocketException se);
        public event LayerSocketErrorHandler LayerSocketError;

        public delegate void LayerAccountHandler(PRoConLayerClient client);
        public event LayerAccountHandler ClientConnected;
        //public event LayerAccountHandler LayerAccountLogout;

        private TcpListener m_tclLayerListener;

        private PRoConApplication m_praApplication;
        private PRoConClient m_prcClient;

        public AccountPrivilegeDictionary AccountPrivileges {
            get;
            private set;
        }

        public LayerClientDictionary LayerClients {
            get;
            private set;
        }

        public string BindingAddress {
            get;
            set;
        }

        public UInt16 ListeningPort {
            get;
            set;
        }

        public string LayerNameFormat {
            get;
            set;
        }

        public bool LayerEnabled {
            get;
            set;
        }

        public bool IsLayerOnline {
            get {
                return (this.m_tclLayerListener != null);
            }
        }

        public PRoConLayer() {
            this.AccountPrivileges = new AccountPrivilegeDictionary();

            this.ListeningPort = 27260;
            this.BindingAddress = "0.0.0.0";
            this.LayerNameFormat = "PRoCon[%servername%]";
            this.m_tclLayerListener = null;
            this.LayerClients = new LayerClientDictionary();
            
            this.LayerEnabled = false;

        }

        public void Initialize(PRoConApplication praApplication, PRoConClient prcClient) {
            this.m_praApplication = praApplication;
            foreach (Account accAccount in this.m_praApplication.AccountsList) {

                if (this.AccountPrivileges.Contains(accAccount.Name) == false) {
                    AccountPrivilege apPrivs = new AccountPrivilege(accAccount, new CPrivileges());
                    apPrivs.AccountPrivilegesChanged += new AccountPrivilege.AccountPrivilegesChangedHandler(apPrivs_AccountPrivilegesChanged);
                    this.AccountPrivileges.Add(apPrivs);
                }
                else {
                    this.AccountPrivileges[accAccount.Name].AccountPrivilegesChanged += new AccountPrivilege.AccountPrivilegesChangedHandler(apPrivs_AccountPrivilegesChanged);
                }

            }
            this.m_praApplication.AccountsList.AccountAdded += new AccountDictionary.AccountAlteredHandler(AccountsList_AccountAdded);
            this.m_praApplication.AccountsList.AccountRemoved += new AccountDictionary.AccountAlteredHandler(AccountsList_AccountRemoved);

            this.m_prcClient = prcClient;

            this.m_prcClient.SocketException += new PRoConClient.SocketExceptionHandler(m_prcClient_SocketException);
            this.m_prcClient.ConnectionFailure += new PRoConClient.FailureHandler(m_prcClient_ConnectionFailure);
            this.m_prcClient.ConnectionClosed += new PRoConClient.EmptyParamterHandler(m_prcClient_ConnectionClosed);
            this.m_prcClient.Game.Login += new FrostbiteClient.EmptyParamterHandler(m_prcClient_CommandLogin);

            this.ClientConnected += new LayerAccountHandler(PRoConLayer_ClientConnected);

            if (this.LayerEnabled == true && this.IsLayerOnline == false) {
                this.StartLayerListener();
            }
        }

        private void AccountsList_AccountRemoved(Account item) {
            item.AccountPasswordChanged -= new Account.AccountPasswordChangedHandler(item_AccountPasswordChanged);
            this.AccountPrivileges[item.Name].AccountPrivilegesChanged -= new AccountPrivilege.AccountPrivilegesChangedHandler(apPrivs_AccountPrivilegesChanged);
            
            this.AccountPrivileges.Remove(item.Name);

            this.ForcefullyDisconnectAccount(item.Name);
        }

        private void AccountsList_AccountAdded(Account item) {
            AccountPrivilege apPrivs = new AccountPrivilege(item, new CPrivileges());

            this.AccountPrivileges.Add(apPrivs);

            item.AccountPasswordChanged += new Account.AccountPasswordChangedHandler(item_AccountPasswordChanged);
            apPrivs.AccountPrivilegesChanged += new AccountPrivilege.AccountPrivilegesChangedHandler(apPrivs_AccountPrivilegesChanged);
        }

        private void item_AccountPasswordChanged(Account item) {
            this.ForcefullyDisconnectAccount(item.Name);
        }

        private void apPrivs_AccountPrivilegesChanged(AccountPrivilege item) {
            if (item.Privileges.CanLogin == false) {
                this.ForcefullyDisconnectAccount(item.Owner.Name);
            }
        }

        private void m_prcClient_CommandLogin(FrostbiteClient sender) {
            // Start the layer if it's been enabled to on startup.
            if (this.LayerEnabled == true && this.IsLayerOnline == false) {
                this.StartLayerListener();
            }
        }

        private void m_prcClient_ConnectionClosed(PRoConClient sender) {
            this.ShutdownLayerListener();
        }

        private void m_prcClient_ConnectionFailure(PRoConClient sender, Exception exception) {
            this.ShutdownLayerListener();
        }

        private void m_prcClient_SocketException(PRoConClient sender, SocketException se) {
            this.ShutdownLayerListener();
        }

        private void PRoConLayer_ClientConnected(PRoConLayerClient client) {
            client.Login += new PRoConLayerClient.LayerClientHandler(client_LayerClientLogin);
            client.Logout += new PRoConLayerClient.LayerClientHandler(client_LayerClientLogout);
            client.Quit += new PRoConLayerClient.LayerClientHandler(client_LayerClientQuit);
            client.ClientShutdown += new PRoConLayerClient.LayerClientHandler(client_LayerClientShutdown);
            client.UidRegistered += new PRoConLayerClient.LayerClientHandler(client_UidRegistered);
        }
        
        private void client_LayerClientShutdown(PRoConLayerClient sender) {
            sender.Login -= new PRoConLayerClient.LayerClientHandler(client_LayerClientLogin);
            sender.Logout -= new PRoConLayerClient.LayerClientHandler(client_LayerClientLogout);
            sender.Quit -= new PRoConLayerClient.LayerClientHandler(client_LayerClientQuit);
            sender.ClientShutdown -= new PRoConLayerClient.LayerClientHandler(client_LayerClientShutdown);
            sender.UidRegistered -= new PRoConLayerClient.LayerClientHandler(client_UidRegistered);

            this.LayerClients.Remove(sender);

            this.SendAccountLogout(sender.Username);
        }

        private void SendAccountLogout(string username) {
            foreach (PRoConLayerClient clcClient in new List<PRoConLayerClient>(this.LayerClients)) {
                clcClient.OnAccountLogout(username);
            }
        }

        private void client_LayerClientQuit(PRoConLayerClient sender) {
            if (this.LayerClients.Contains(sender.IPPort) == true) {
                this.LayerClients.Remove(sender.IPPort);
                this.SendAccountLogout(sender.Username);
            }
        }

        private void client_LayerClientLogout(PRoConLayerClient sender) {
            this.SendAccountLogout(sender.Username);
        }

        private void client_LayerClientLogin(PRoConLayerClient sender) {
            if (this.LayerClients.Contains(sender.Username) == true) {
                // List a logged in account
            }

            foreach (PRoConLayerClient clcClient in new List<PRoConLayerClient>(this.LayerClients)) {
                clcClient.OnAccountLogin(sender.Username, sender.Privileges);
            }
        }

        private void client_UidRegistered(PRoConLayerClient sender) {
            foreach (PRoConLayerClient clcClient in new List<PRoConLayerClient>(this.LayerClients)) {
                clcClient.OnRegisteredUid(sender.ProconEventsUid, sender.Username);
            }
        }

        private void ForcefullyDisconnectAccount(string strAccountName) {
            List<PRoConLayerClient> lstShutDownClients = new List<PRoConLayerClient>();

            foreach (PRoConLayerClient plcConnection in new List<PRoConLayerClient>(this.LayerClients)) {
                if (String.Compare(plcConnection.Username, strAccountName) == 0) {
                    lstShutDownClients.Add(plcConnection);
                }
            }

            foreach (PRoConLayerClient cplcShutdown in lstShutDownClients) {
                cplcShutdown.Shutdown();
            }

            this.SendAccountLogout(strAccountName);
        }

        public List<string> GetLoggedInAccounts() {
            List<string> lstLoggedInAccounts = new List<string>();

            foreach (PRoConLayerClient plcConnection in new List<PRoConLayerClient>(this.LayerClients)) {
                if (lstLoggedInAccounts.Contains(plcConnection.Username) == false) {
                    lstLoggedInAccounts.Add(plcConnection.Username);
                }
            }

            return lstLoggedInAccounts;
        }

        public List<string> GetLoggedInAccounts(bool listUids) {
            List<string> lstLoggedInAccounts = new List<string>();

            foreach (PRoConLayerClient plcConnection in new List<PRoConLayerClient>(this.LayerClients)) {
                if (lstLoggedInAccounts.Contains(plcConnection.Username) == false) {
                    lstLoggedInAccounts.Add(plcConnection.Username);

                    if (listUids == true) {
                        lstLoggedInAccounts.Add(plcConnection.ProconEventsUid);
                    }
                }
            }

            return lstLoggedInAccounts;
        }

        /*
        private List<string> LayerGetAccounts() {

            List<string> lstReturnWords = new List<string>();

            foreach (AccountPrivilege apAccount in this.AccountPrivileges) {
                lstReturnWords.Add(apAccount.Owner.Name);
                lstReturnWords.Add(Convert.ToString(apAccount.Privileges.PrivilegesFlags));
            }

            return lstReturnWords;
        }
        */

        private AsyncCallback m_asyncAcceptCallback = new AsyncCallback(PRoConLayer.ListenIncommingLayerConnections);
        private static void ListenIncommingLayerConnections(IAsyncResult ar) {

            PRoConLayer plLayer = (PRoConLayer)ar.AsyncState;

            if (plLayer.m_tclLayerListener != null) {

                try {
                    TcpClient tcpNewConnection = plLayer.m_tclLayerListener.EndAcceptTcpClient(ar);

                    PRoConLayerClient cplcNewConnection = new PRoConLayerClient(new FrostbiteLayerConnection(tcpNewConnection), plLayer.m_praApplication, plLayer.m_prcClient);
                    
                    plLayer.LayerClients.Add(cplcNewConnection);

                    if (plLayer.ClientConnected != null) {
                        FrostbiteConnection.RaiseEvent(plLayer.ClientConnected.GetInvocationList(), cplcNewConnection);
                    }

                    plLayer.m_tclLayerListener.BeginAcceptTcpClient(plLayer.m_asyncAcceptCallback, plLayer);
                }
                catch (SocketException skeError) {

                    if (plLayer.LayerSocketError != null) {
                        FrostbiteConnection.RaiseEvent(plLayer.LayerSocketError.GetInvocationList(), skeError);
                    }

                    plLayer.ShutdownLayerListener();

                    //cbfAccountsPanel.OnLayerServerSocketError(skeError);
                }
                catch (Exception) {

                }
            }
        }

        private IPAddress ResolveHostName(string strHostName) {
            IPAddress ipReturn = IPAddress.None;

            if (IPAddress.TryParse(strHostName, out ipReturn) == false) {

                ipReturn = IPAddress.None;

                try {
                    IPHostEntry iphHost = Dns.GetHostEntry(strHostName);

                    if (iphHost.AddressList.Length > 0) {
                        ipReturn = iphHost.AddressList[0];
                    }
                    // ELSE return IPAddress.None..
                }
                catch (Exception) { } // Returns IPAddress.None..
            }

            return ipReturn;
        }

        public void StartLayerListener() {

            try {
                IPAddress ipBinding = this.ResolveHostName(this.BindingAddress);

                this.m_tclLayerListener = new TcpListener(ipBinding, this.ListeningPort);

                this.m_tclLayerListener.Start();

                if (this.LayerOnline != null) {
                    FrostbiteConnection.RaiseEvent(this.LayerOnline.GetInvocationList());
                }

                //this.OnLayerServerOnline();

                this.m_tclLayerListener.BeginAcceptTcpClient(this.m_asyncAcceptCallback, this);
            }
            catch (SocketException skeError) {

                if (this.LayerSocketError != null) {
                    FrostbiteConnection.RaiseEvent(this.LayerSocketError.GetInvocationList(), skeError);
                }

                this.ShutdownLayerListener();
                //this.OnLayerServerSocketError(skeError);
            }
        }

        public void ShutdownLayerListener() {

            if (this.m_tclLayerListener != null) {

                try {

                    PRoConLayerClient[] cplcShutdownClients = new PRoConLayerClient[this.LayerClients.Count];
                    this.LayerClients.CopyTo(cplcShutdownClients, 0);

                    foreach (PRoConLayerClient cplcShutdownClient in cplcShutdownClients) {

                        cplcShutdownClient.OnShutdown();
                        cplcShutdownClient.Shutdown();
                    }

                    //if (this.m_tclLayerListener != null) {
                        this.m_tclLayerListener.Stop();
                        this.m_tclLayerListener = null;
                    //}
                }
                catch (Exception) { }

                if (this.LayerOffline != null) {
                    FrostbiteConnection.RaiseEvent(this.LayerOffline.GetInvocationList());
                }
            }
            //this.OnLayerServerOffline();
        }
        
    }
         
}
