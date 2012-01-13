using System;
using System.Collections.Generic;
using System.Text;

// This class will move to .Core once ProConClient is in .Core.
namespace PRoCon.Core.Consoles {
    using Core;
    using Core.Logging;
    using Core.Plugin;
    using Core.Remote;
    public class PluginConsole : Loggable {

        public event WriteConsoleHandler WriteConsole;

        private PRoConClient m_prcClient;

        public Queue<LogEntry> LogEntries {
            get;
            private set;
        }

        public PluginConsole(PRoConClient prcClient)
            : base() {

            this.m_prcClient = prcClient;

            this.LogEntries = new Queue<LogEntry>();

            this.FileHostNamePort = this.m_prcClient.FileHostNamePort;
            this.LoggingStartedPrefix = "Plugin logging started";
            this.LoggingStoppedPrefix = "Plugin logging stopped";
            this.FileNameSuffix = "plugin";

            this.m_prcClient.CompilingPlugins += new PRoConClient.EmptyParamterHandler(m_prcClient_CompilingPlugins);
            this.m_prcClient.RecompilingPlugins += new PRoConClient.EmptyParamterHandler(m_prcClient_RecompilingPlugins);
        }

        private void m_prcClient_RecompilingPlugins(PRoConClient sender) {
            this.m_prcClient.PluginsManager.PluginOutput += new PluginManager.PluginOutputHandler(Plugins_PluginOutput);
        }

        private void m_prcClient_CompilingPlugins(PRoConClient sender) {
            this.m_prcClient.PluginsManager.PluginOutput += new PluginManager.PluginOutputHandler(Plugins_PluginOutput);
        }

        private void Plugins_PluginOutput(string strOutput) {
            this.Write(strOutput);
        }

        public void Write(string strFormat, params string[] a_objArguments) {
            try {
                DateTime dtLoggedTime = DateTime.UtcNow.ToUniversalTime().AddHours(m_prcClient.Game.UTCoffset).ToLocalTime();
                string strText = String.Format(strFormat, a_objArguments);

                this.WriteLogLine(String.Format("[{0}] {1}", dtLoggedTime.ToString("HH:mm:ss"), strText));

                if (this.WriteConsole != null) {
                    FrostbiteConnection.RaiseEvent(this.WriteConsole.GetInvocationList(), dtLoggedTime, strText);
                }

                this.LogEntries.Enqueue(new LogEntry(dtLoggedTime, strText));

                while (this.LogEntries.Count > 100) {
                    this.LogEntries.Dequeue();
                }
            }
            catch (Exception) { }
        }
    }
}
