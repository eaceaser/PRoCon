using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Permissions;
using System.IO;
using System.Text.RegularExpressions;

// This can be moved into .Core once the contents of PRoCon.Plugin.* have been moved to .Core.
namespace PRoCon.Core.Options {
    using Core.Remote;
    public class OptionsSettings {

        private PRoConApplication m_praApplication;

        public delegate void OptionsEnabledHandler(bool blEnabled);
        public event OptionsEnabledHandler ConsoleLoggingChanged;
        public event OptionsEnabledHandler EventsLoggingChanged;
        public event OptionsEnabledHandler PluginsLoggingChanged;
        public event OptionsEnabledHandler ChatLoggingChanged;
        public event OptionsEnabledHandler AutoCheckDownloadUpdatesChanged;
        public event OptionsEnabledHandler AutoApplyUpdatesChanged;
        public event OptionsEnabledHandler ShowTrayIconChanged;
        public event OptionsEnabledHandler CloseToTrayChanged;
        public event OptionsEnabledHandler MinimizeToTrayChanged;

        public event OptionsEnabledHandler RunPluginsInTrustedSandboxChanged;
        public event OptionsEnabledHandler AllowAllODBCConnectionsChanged;

        public event OptionsEnabledHandler AdminMoveMessageChanged;
        public event OptionsEnabledHandler ChatDisplayAdminNameChanged;

        public event OptionsEnabledHandler LayerHideLocalPluginsChanged;
        public event OptionsEnabledHandler LayerHideLocalAccountsChanged;

        public event OptionsEnabledHandler ShowRoundTimerConstantlyChanged;

        private bool m_isConsoleLoggingEnabled;
        public bool ConsoleLogging {
            get {
                return this.m_isConsoleLoggingEnabled;
            }
            set {
                this.m_isConsoleLoggingEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.ConsoleLoggingChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.ConsoleLoggingChanged.GetInvocationList(), value);
                }
            }
        }

        private bool m_isEventsLoggingEnabled;
        public bool EventsLogging {
            get {
                return this.m_isEventsLoggingEnabled;
            }
            set {
                this.m_isEventsLoggingEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.EventsLoggingChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.EventsLoggingChanged.GetInvocationList(), value);
                }
            }
        }

        private bool m_isPluginLoggingEnabled;
        public bool PluginLogging {
            get {
                return this.m_isPluginLoggingEnabled;
            }
            set {
                this.m_isPluginLoggingEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.PluginsLoggingChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.PluginsLoggingChanged.GetInvocationList(), value);
                }
            }
        }

        private bool m_isChatLoggingEnabled;
        public bool ChatLogging {
            get {
                return this.m_isChatLoggingEnabled;
            }
            set {
                this.m_isChatLoggingEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.ChatLoggingChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.ChatLoggingChanged.GetInvocationList(), value);
                }
            }
        }
        
        private bool m_isAutoCheckDownloadUpdatesEnabled;
        public bool AutoCheckDownloadUpdates {
            get {
                return this.m_isAutoCheckDownloadUpdatesEnabled;
            }
            set {
                if (this.m_praApplication.BlockUpdateChecks == true) {
                    this.m_isAutoCheckDownloadUpdatesEnabled = false;
                }
                else {
                    this.m_isAutoCheckDownloadUpdatesEnabled = value;
                }

                this.m_praApplication.SaveMainConfig();

                if (this.AutoCheckDownloadUpdatesChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.AutoCheckDownloadUpdatesChanged.GetInvocationList(), value);
                }
            }
        }

        private bool m_isAutoApplyUpdatesEnabled;
        public bool AutoApplyUpdates {
            get {
                return this.m_isAutoApplyUpdatesEnabled;
            }
            set {
                this.m_isAutoApplyUpdatesEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.AutoApplyUpdatesChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.AutoApplyUpdatesChanged.GetInvocationList(), value);
                }
            }
        }

        private bool m_isTrayIconVisible;
        public bool ShowTrayIcon {
            get {
                return this.m_isTrayIconVisible;
            }
            set {
                this.m_isTrayIconVisible = value;
                this.m_praApplication.SaveMainConfig();

                if (this.ShowTrayIconChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.ShowTrayIconChanged.GetInvocationList(), value);
                }
            }
        }

        private bool m_isCloseToTrayEnabled;
        public bool CloseToTray {
            get {
                return this.m_isCloseToTrayEnabled;
            }
            set {
                this.m_isCloseToTrayEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.CloseToTrayChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.CloseToTrayChanged.GetInvocationList(), value);
                }
            }
        }

        private bool m_isMinimizeToTrayEnabled;
        public bool MinimizeToTray {
            get {
                return this.m_isMinimizeToTrayEnabled;
            }
            set {
                this.m_isMinimizeToTrayEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.MinimizeToTrayChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.MinimizeToTrayChanged.GetInvocationList(), value);
                }
            }
        }

        private bool m_isRunPluginsInTrustedSandboxEnabled;
        public bool RunPluginsInTrustedSandbox {
            get {
                return this.m_isRunPluginsInTrustedSandboxEnabled;
            }
            set {
                this.m_isRunPluginsInTrustedSandboxEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.RunPluginsInTrustedSandboxChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.RunPluginsInTrustedSandboxChanged.GetInvocationList(), value);
                }
            }
        }

        private bool m_isAdminMoveMessageEnabled = true;
        public bool AdminMoveMessage {
            get {
                return this.m_isAdminMoveMessageEnabled;
            }
            set {
                this.m_isAdminMoveMessageEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.AdminMoveMessageChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.AdminMoveMessageChanged.GetInvocationList(), value);
                }
            }
        }
        
        // ChatDisplayAdminNameChanged
        private bool m_isChatDisplayAdminNameEnabled = true;
        public bool ChatDisplayAdminName {
            get {
                return this.m_isChatDisplayAdminNameEnabled;
            }
            set {
                this.m_isChatDisplayAdminNameEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.ChatDisplayAdminNameChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.ChatDisplayAdminNameChanged.GetInvocationList(), value);
                }
            }
        }
        
        private bool m_isLayerHideLocalPluginsEnabled = true;
        public bool LayerHideLocalPlugins {
            get {
                return this.m_isLayerHideLocalPluginsEnabled;
            }
            set {
                this.m_isLayerHideLocalPluginsEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.LayerHideLocalPluginsChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.LayerHideLocalPluginsChanged.GetInvocationList(), value);
                }
            }
        }

        private bool m_isLayerHideLocalAccountsEnabled = true;
        public bool LayerHideLocalAccounts {
            get {
                return this.m_isLayerHideLocalAccountsEnabled;
            }
            set {
                this.m_isLayerHideLocalAccountsEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.LayerHideLocalAccountsChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.LayerHideLocalAccountsChanged.GetInvocationList(), value);
                }
            }
        }

        // ShowRoundTimerConstantly
        private bool m_isShowRoundTimerConstantlyEnabled;
        public bool ShowRoundTimerConstantly
        {
            get
            {
                return this.m_isShowRoundTimerConstantlyEnabled;
            }
            set
            {
                this.m_isShowRoundTimerConstantlyEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.ShowRoundTimerConstantlyChanged != null)
                {
                    FrostbiteConnection.RaiseEvent(this.ShowRoundTimerConstantlyChanged.GetInvocationList(), value);
                }
            }
        }

        private bool m_isAllowAllODBCConnectionsEnabled;
        public bool AllowAllODBCConnections {
            get {
                return this.m_isAllowAllODBCConnectionsEnabled;
            }
            set {
                this.m_isAllowAllODBCConnectionsEnabled = value;
                this.m_praApplication.SaveMainConfig();

                if (this.AllowAllODBCConnectionsChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.AllowAllODBCConnectionsChanged.GetInvocationList(), value);
                }
            }
        }

        public NotificationList<TrustedHostWebsitePort> TrustedHostsWebsitesPorts {
            get;
            private set;
        }


        public PermissionSet PluginPermissions {

            get {

                PermissionSet psetPluginPermissions;

                if (this.RunPluginsInTrustedSandbox == true) {

                    psetPluginPermissions = new PermissionSet(PermissionState.None);

                    try {

                        psetPluginPermissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
                        psetPluginPermissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.PathDiscovery, AppDomain.CurrentDomain.BaseDirectory));
                        psetPluginPermissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, AppDomain.CurrentDomain.BaseDirectory + "Plugins" + Path.DirectorySeparatorChar));
                        psetPluginPermissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, AppDomain.CurrentDomain.BaseDirectory + "Logs" + Path.DirectorySeparatorChar));
                        psetPluginPermissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, AppDomain.CurrentDomain.BaseDirectory + "Localization" + Path.DirectorySeparatorChar));
                        psetPluginPermissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.Read | FileIOPermissionAccess.PathDiscovery, AppDomain.CurrentDomain.BaseDirectory + "Configs" + Path.DirectorySeparatorChar));
                        psetPluginPermissions.AddPermission(new FileIOPermission(FileIOPermissionAccess.AllAccess, AppDomain.CurrentDomain.BaseDirectory + "Media" + Path.DirectorySeparatorChar));
                        psetPluginPermissions.AddPermission(new UIPermission(PermissionState.Unrestricted));
                        psetPluginPermissions.AddPermission(new System.Net.DnsPermission(PermissionState.Unrestricted));

                        if (this.AllowAllODBCConnections == true) {
                            psetPluginPermissions.AddPermission(new System.Data.Odbc.OdbcPermission(PermissionState.Unrestricted));
                        }

                        // TO DO: Fixup.
                        psetPluginPermissions.AddPermission(new ReflectionPermission(ReflectionPermissionFlag.RestrictedMemberAccess));
                    }
                    catch (Exception) {

                    }

                    foreach (TrustedHostWebsitePort trusted in this.TrustedHostsWebsitesPorts) {

                        try {
                            psetPluginPermissions.AddPermission(new System.Net.WebPermission(System.Net.NetworkAccess.Connect, new Regex(trusted.HostWebsite.Replace(".", @"\.") + ".*", RegexOptions.IgnoreCase)));
                            psetPluginPermissions.AddPermission(new System.Net.SocketPermission(System.Net.NetworkAccess.Connect, System.Net.TransportType.All, trusted.HostWebsite, trusted.Port));
                        }
                        catch (Exception) {

                        }
                    }
                }
                else {
                    psetPluginPermissions = new PermissionSet(PermissionState.Unrestricted);
                }

                return psetPluginPermissions;
            }
        }

        public OptionsSettings(PRoConApplication praApplication) {
            this.m_praApplication = praApplication;
            this.AutoCheckDownloadUpdates = true;

            this.LayerHideLocalAccounts = true;
            this.LayerHideLocalPlugins = true;

            this.ShowTrayIcon = true;

            this.TrustedHostsWebsitesPorts = new NotificationList<TrustedHostWebsitePort>();
        }
    }
}
