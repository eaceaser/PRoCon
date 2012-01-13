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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace PRoCon {
    using Core;
    using Core.Remote;
    using PRoCon.Forms;

    public partial class uscConsolePanel : UserControl {

        private frmMain m_frmMainWindow;
        private uscServerConnection m_uscParent;

        private CLocalization m_clocLanguage;

        private PRoConClient m_prcClient;

        //private Dictionary<string, Color> m_dicConsoleTextColours;

        private LinkedList<string> m_llCommandsHistory;
        private LinkedListNode<string> m_llCommandsHistoryCurrentNode;

        private LinkedList<string> m_llPunkbusterCommandsHistory;
        private LinkedListNode<string> m_llPunkbusterCommandsHistoryCurrentNode;

        public delegate void SendCommandDelegate(string strCommand);
        public event SendCommandDelegate SendCommand;

        public delegate void SendListCommandDelegate(List<string> lstCommand);
        public event SendListCommandDelegate SendListCommand;

        //public delegate void DebugDelegate(bool blDebugEnabled);
        //public event DebugDelegate EnableConsoleDebug;

        private Regex m_regRemoveCaretCodes;

        /*
        public bool ShowConsoleDebug {
            get {
                return this.chkDebug.Checked;
            }
        }

        public bool ShowConsoleEvents {
            get {
                return this.chkEvents.Checked;
            }
        }
        */

        /*
        public List<string> SetConsoleSettings {
            set {
                bool blChecked = true;

                if (value.Count >= 1 && bool.TryParse(value[0], out blChecked) == true) {
                    this.chkEnableOutput.Checked = blChecked;
                }

                if (value.Count >= 2 && bool.TryParse(value[1], out blChecked) == true) {
                    this.chkEvents.Checked = blChecked;
                }

                if (value.Count >= 3 && bool.TryParse(value[2], out blChecked) == true) {
                    this.chkDebug.Checked = blChecked;
                }

                if (value.Count >= 4 && bool.TryParse(value[3], out blChecked) == true) {
                    this.chkEnablePunkbusterOutput.Checked = blChecked;
                }
            }
        }

        public string ConsoleSettings {
            get {
                return String.Format("{0} {1} {2} {3}", this.chkEnableOutput.Checked, this.chkEvents.Checked, this.chkDebug.Checked, this.chkEnablePunkbusterOutput.Checked);
            }
        }
        */
        public uscConsolePanel() {
            InitializeComponent();

            this.m_clocLanguage = null;

            this.m_llCommandsHistory = new LinkedList<string>();
            this.m_llPunkbusterCommandsHistory = new LinkedList<string>();

            this.m_regRemoveCaretCodes = new Regex(@"\^[0-9]|\^b|\^i|\^n", RegexOptions.Compiled);
        }

        private void uscConsolePanel_Load(object sender, EventArgs e) {
            if (this.m_prcClient != null && this.m_prcClient.Console != null) {
                this.m_prcClient.Console.DisplayConnection = this.m_prcClient.Console.DisplayConnection;
                this.m_prcClient.Console.DisplayPunkbuster = this.m_prcClient.Console.DisplayPunkbuster;
                this.m_prcClient.Console.LogDebugDetails = this.m_prcClient.Console.LogDebugDetails;
                this.m_prcClient.Console.LogEventsConnection = this.m_prcClient.Console.LogEventsConnection;
                this.m_prcClient.Console.ConScrolling = this.m_prcClient.Console.ConScrolling;
                this.m_prcClient.Console.PBScrolling = this.m_prcClient.Console.PBScrolling;
            }
        }

        public void Initialize(frmMain frmMainWindow, uscServerConnection uscParent) {
            this.m_uscParent = uscParent;
            this.m_frmMainWindow = frmMainWindow;

            this.tbcConsoles.ImageList = this.m_frmMainWindow.iglIcons;
            this.tabConsole.ImageKey = "application_xp_terminal.png";
            this.tabPunkbuster.ImageKey = "punkbuster.png";

        }

        public void SetConnection(PRoConClient prcClient) {
            if ((this.m_prcClient = prcClient) != null) {
                if (this.m_prcClient.Game != null) {
                    this.m_prcClient_GameTypeDiscovered(prcClient);
                }
                else {
                    this.m_prcClient.GameTypeDiscovered += new PRoConClient.EmptyParamterHandler(m_prcClient_GameTypeDiscovered);
                }
            }
        }

        private void m_prcClient_GameTypeDiscovered(PRoConClient sender) {
            this.m_prcClient.Console.WriteConsole += new PRoCon.Core.Logging.Loggable.WriteConsoleHandler(Console_WriteConsole);
            this.m_prcClient.Console.LogEventsConnectionChanged += new PRoCon.Core.Consoles.ConnectionConsole.IsEnabledHandler(Console_LogEventsConnectionChanged);
            this.m_prcClient.Console.DisplayConnectionChanged += new PRoCon.Core.Consoles.ConnectionConsole.IsEnabledHandler(Console_DisplayConnectionChanged);
            this.m_prcClient.Console.DisplayPunkbusterChanged += new PRoCon.Core.Consoles.ConnectionConsole.IsEnabledHandler(Console_DisplayPunkbusterChanged);

            this.m_prcClient.Console.LogDebugDetailsChanged += new PRoCon.Core.Consoles.ConnectionConsole.IsEnabledHandler(Console_LogDebugDetailsChanged);

            this.m_prcClient.PunkbusterConsole.WriteConsole += new PRoCon.Core.Logging.Loggable.WriteConsoleHandler(PunkbusterConsole_WriteConsole);

            this.m_prcClient.Console.ConScrollingChanged += new PRoCon.Core.Consoles.ConnectionConsole.IsEnabledHandler(Console_ConEnableScrollingChanged);
            this.m_prcClient.Console.PBScrollingChanged += new PRoCon.Core.Consoles.ConnectionConsole.IsEnabledHandler(Console_PBEnableScrollingChanged);

            this.m_prcClient.Game.Help += new FrostbiteClient.HelpHandler(Game_Help);
        }

        void Game_Help(FrostbiteClient sender, List<string> lstCommands) {
            this.txtConsoleCommand.AutoCompleteCustomSource.Clear();
            this.txtConsoleCommand.AutoCompleteCustomSource.AddRange(lstCommands.ToArray());
        }

        private void RefreshSentRecvCounters() {
            this.lblOutputKiBs.Text = String.Format("D: {0:0.00} KiB's, U: {1:0.00} KiB's", (float)this.m_prcClient.Console.BytesRecieved / 1024F, (float)this.m_prcClient.Console.BytesSent / 1024F);
        }

        public void SetColour(string strVariable, string strValue) {
            this.rtbConsoleBox.SetColour(strVariable, strValue);
        }

        public void SetLocalization(CLocalization clocLanguage) {
            this.m_clocLanguage = clocLanguage;

            this.tabConsole.Text = this.m_clocLanguage.GetLocalized("uscServerConnection.tabConsole", null);
            this.btnConsoleSend.Text = this.m_clocLanguage.GetLocalized("uscConsolePanel.btnConsoleSend", null);
            this.chkEnableOutput.Text = this.m_clocLanguage.GetLocalized("uscConsolePanel.chkEnableOutput", null);
            this.chkEvents.Text = this.m_clocLanguage.GetLocalized("uscConsolePanel.chkEvents", null);
            this.chkDebug.Text = this.m_clocLanguage.GetLocalized("uscConsolePanel.chkDebug", null);

            this.btnPunkbusterSend.Text = this.m_clocLanguage.GetLocalized("uscConsolePanel.btnConsoleSend", null);
            this.chkEnablePunkbusterOutput.Text = this.m_clocLanguage.GetLocalized("uscConsolePanel.chkEnableOutput", null);
            this.chkConEnableScrolling.Text = this.m_clocLanguage.GetLocalized("uscChatPanel.chkDisplayScrolling", null);
            this.chkPBEnableScrolling.Text = this.m_clocLanguage.GetLocalized("uscChatPanel.chkDisplayScrolling", null);
        }

        public event uscServerConnection.OnTabChangeDelegate OnTabChange;

        private void tbcConsoles_SelectedIndexChanged(object sender, EventArgs e) {
            if (this.OnTabChange != null) {
                Stack<string> stkTabIndexes = new Stack<string>();
                stkTabIndexes.Push(tbcConsoles.SelectedTab.Name);

                this.OnTabChange(this, stkTabIndexes);
            }
        }

        public void SetTabIndexes(Stack<string> stkTabIndexes) {
            if (tbcConsoles.TabPages.ContainsKey(stkTabIndexes.Peek()) == true) {
                this.tbcConsoles.SelectedTab = tbcConsoles.TabPages[stkTabIndexes.Pop()];
            }
        }

        private void Console_WriteConsole(DateTime dtLoggedTime, string strLoggedText) {

            this.RefreshSentRecvCounters();

            if (this.chkEnableOutput.Checked == true) {

                string strFormattedConsoleOutput = String.Format("[{0}] {1}{2}", dtLoggedTime.ToString("HH:mm:ss"), strLoggedText, Environment.NewLine);

                this.rtbConsoleBox.AppendText(strFormattedConsoleOutput);
                //this.rtbConsoleBox.AppendText(String.Format("[{0}] {1}\n", DateTime.Now.ToString("HH:mm:ss"), strConsoleOutput));

                // We only pass the length of the original text to exclude the time from being formatted.
                //this.ColourizeConsoleOutput(strConsoleOutput.Length);

                if (this.rtbConsoleBox.Focused == false) {
                    if (this.m_prcClient.Console.ConScrolling == true)
                    {
                        this.rtbConsoleBox.ScrollToCaret();
                    }
                }

                this.rtbConsoleBox.ReadOnly = false;

                int iMaxConsoleLines = this.m_prcClient.Variables.GetVariable<int>("MAX_CONSOLE_LINES", 75);
                int iConsoleBoxLines = this.rtbConsoleBox.Lines.Length;

                if ((iConsoleBoxLines > iMaxConsoleLines && this.rtbConsoleBox.Focused == false) || iConsoleBoxLines > 3000) {
                    
                    for (int i = 0; i < iConsoleBoxLines - iMaxConsoleLines; i++) {

                        //while (this.rtbPunkbusterBox.Lines.Length > iMaxPunkbusterLines) {

                        this.rtbConsoleBox.Select(0, this.rtbConsoleBox.Lines[0].Length + 1);
                        this.rtbConsoleBox.SelectedText = String.Empty;
                    }
                }
                this.rtbConsoleBox.ReadOnly = true;
            }
        }

        private void PunkbusterConsole_WriteConsole(DateTime dtLoggedTime, string strLoggedText) {
            
            if (this.chkEnablePunkbusterOutput.Checked == true) {

                string strFormattedConsoleOutput = String.Format("{0}{1}", strLoggedText, Environment.NewLine);

                this.rtbPunkbusterBox.AppendText(strFormattedConsoleOutput);
                //this.rtbConsoleBox.AppendText(String.Format("[{0}] {1}\n", DateTime.Now.ToString("HH:mm:ss"), strConsoleOutput));

                // We only pass the length of the original text to exclude the time from being formatted.
                //this.ColourizeConsoleOutput(strConsoleOutput.Length);

                if (this.rtbPunkbusterBox.Focused == false) {
                    if (this.m_prcClient.Console.PBScrolling == true)
                    {
                        this.rtbPunkbusterBox.ScrollToCaret();
                    }
                }
                this.rtbPunkbusterBox.ReadOnly = false;

                int iMaxPunkbusterLines = this.m_prcClient.Variables.GetVariable<int>("MAX_PUNKBUSTERCONSOLE_LINES", 200);
                int iPunkbusterBoxLines = this.rtbPunkbusterBox.Lines.Length;

                if ((iPunkbusterBoxLines > iMaxPunkbusterLines && this.rtbPunkbusterBox.Focused == false) || iPunkbusterBoxLines > 3000) {
                    for (int i = 0; i < iPunkbusterBoxLines - iMaxPunkbusterLines; i++) {

                        //while (this.rtbPunkbusterBox.Lines.Length > iMaxPunkbusterLines) {

                        this.rtbPunkbusterBox.Select(0, this.rtbPunkbusterBox.Lines[0].Length + 1);
                        this.rtbPunkbusterBox.SelectedText = String.Empty;
                    }
                }
                this.rtbPunkbusterBox.ReadOnly = true;
            }
        }

        /*
        public void PunkbusterWrite(string strConsoleOutput) {

            //string strFormattedConsoleOutput = String.Format("[{0}] {1}\r\n", DateTime.Now.ToString("HH:mm:ss"), strConsoleOutput);

            string strFormattedConsoleOutput = String.Format("{0}{1}", strConsoleOutput, Environment.NewLine);

            if (this.chkEnablePunkbusterOutput.Checked == true) {

                this.rtbPunkbusterBox.AppendText(strFormattedConsoleOutput);
                //this.rtbConsoleBox.AppendText(String.Format("[{0}] {1}\n", DateTime.Now.ToString("HH:mm:ss"), strConsoleOutput));

                // We only pass the length of the original text to exclude the time from being formatted.
                //this.ColourizeConsoleOutput(strConsoleOutput.Length);

                this.rtbPunkbusterBox.ScrollToCaret();
                this.rtbPunkbusterBox.ReadOnly = false;

                int iMaxPunkbusterLines = this.m_uscParent.GetVariableInt("MAX_PUNKBUSTERCONSOLE_LINES", 200);
                int iPunkbusterBoxLines = this.rtbPunkbusterBox.Lines.Length;

                if (iPunkbusterBoxLines > iMaxPunkbusterLines) {
                    for (int i = 0; i < iPunkbusterBoxLines - iMaxPunkbusterLines; i++) {

                        //while (this.rtbPunkbusterBox.Lines.Length > iMaxPunkbusterLines) {

                        this.rtbPunkbusterBox.Select(0, this.rtbPunkbusterBox.Lines[0].Length + 1);
                        this.rtbPunkbusterBox.SelectedText = String.Empty;
                    }
                }
                this.rtbPunkbusterBox.ReadOnly = true;
            }
        }
        */

        private void btnPunkbusterSend_Click(object sender, EventArgs e) {
            if (this.SendListCommand != null) {
                this.m_llPunkbusterCommandsHistory.AddFirst(this.txtPunkbusterCommand.Text);
                if (this.m_llPunkbusterCommandsHistory.Count > 20) {
                    this.m_llPunkbusterCommandsHistory.RemoveLast();
                }
                this.m_llPunkbusterCommandsHistoryCurrentNode = null;

                this.SendListCommand(new List<string>() { "punkBuster.pb_sv_command", this.txtPunkbusterCommand.Text });

                this.txtPunkbusterCommand.Clear();
                this.txtPunkbusterCommand.Focus();
            }
        }

        private void txtPunkbusterCommand_KeyDown(object sender, KeyEventArgs e) {

            if (this.SendListCommand != null && e.KeyData == Keys.Enter) {

                this.SendListCommand(new List<string>() { "punkBuster.pb_sv_command", this.txtPunkbusterCommand.Text });

                this.m_llPunkbusterCommandsHistory.AddFirst(this.txtPunkbusterCommand.Text);
                if (this.m_llPunkbusterCommandsHistory.Count > 20) {
                    this.m_llPunkbusterCommandsHistory.RemoveLast();
                }
                this.m_llPunkbusterCommandsHistoryCurrentNode = null;

                this.txtPunkbusterCommand.Clear();
                this.txtPunkbusterCommand.Focus();
                e.SuppressKeyPress = true;
            }

            if (e.KeyData == Keys.Up) {
                e.SuppressKeyPress = true;

                if (this.m_llPunkbusterCommandsHistoryCurrentNode == null && this.m_llPunkbusterCommandsHistory.First != null) {
                    this.m_llPunkbusterCommandsHistoryCurrentNode = this.m_llPunkbusterCommandsHistory.First;
                    this.txtPunkbusterCommand.Text = this.m_llPunkbusterCommandsHistoryCurrentNode.Value;

                    this.txtPunkbusterCommand.Select(this.txtPunkbusterCommand.Text.Length, 0);
                }
                else if (this.m_llPunkbusterCommandsHistoryCurrentNode != null && this.m_llPunkbusterCommandsHistoryCurrentNode.Next != null) {
                    this.m_llPunkbusterCommandsHistoryCurrentNode = this.m_llPunkbusterCommandsHistoryCurrentNode.Next;
                    this.txtPunkbusterCommand.Text = this.m_llPunkbusterCommandsHistoryCurrentNode.Value;

                    this.txtPunkbusterCommand.Select(this.txtConsoleCommand.Text.Length, 0);
                }
            }
            else if (e.KeyData == Keys.Down) {

                if (this.m_llPunkbusterCommandsHistoryCurrentNode != null && this.m_llPunkbusterCommandsHistoryCurrentNode.Previous != null) {
                    this.m_llPunkbusterCommandsHistoryCurrentNode = this.m_llPunkbusterCommandsHistoryCurrentNode.Previous;
                    this.txtPunkbusterCommand.Text = this.m_llPunkbusterCommandsHistoryCurrentNode.Value;

                    this.txtPunkbusterCommand.Select(this.txtPunkbusterCommand.Text.Length, 0);
                }

                e.SuppressKeyPress = true;
            }

        }


        private void btnConsoleSend_Click(object sender, EventArgs e) {
            if (this.SendCommand != null) {
                this.m_llCommandsHistory.AddFirst(this.txtConsoleCommand.Text);
                if (this.m_llCommandsHistory.Count > 20) {
                    this.m_llCommandsHistory.RemoveLast();
                }
                this.m_llCommandsHistoryCurrentNode = null;

                this.SendCommand(this.txtConsoleCommand.Text);

                this.txtConsoleCommand.Clear();
                this.txtConsoleCommand.Focus();
            }
        }

        private void txtConsoleCommand_KeyDown(object sender, KeyEventArgs e) {

            if (this.SendCommand != null && e.KeyData == Keys.Enter) {
            
                this.SendCommand(this.txtConsoleCommand.Text);

                this.m_llCommandsHistory.AddFirst(this.txtConsoleCommand.Text);
                if (this.m_llCommandsHistory.Count > 20) {
                    this.m_llCommandsHistory.RemoveLast();
                }
                this.m_llCommandsHistoryCurrentNode = null;

                this.txtConsoleCommand.Clear();
                this.txtConsoleCommand.Focus();
                e.SuppressKeyPress = true;
            }

            if (e.KeyData == Keys.Up) {
                e.SuppressKeyPress = true;

                if (this.m_llCommandsHistoryCurrentNode == null && this.m_llCommandsHistory.First != null) {
                    this.m_llCommandsHistoryCurrentNode = this.m_llCommandsHistory.First;
                    this.txtConsoleCommand.Text = this.m_llCommandsHistoryCurrentNode.Value;

                    this.txtConsoleCommand.Select(this.txtConsoleCommand.Text.Length, 0);
                }
                else if (this.m_llCommandsHistoryCurrentNode != null && this.m_llCommandsHistoryCurrentNode.Next != null) {
                    this.m_llCommandsHistoryCurrentNode = this.m_llCommandsHistoryCurrentNode.Next;
                    this.txtConsoleCommand.Text = this.m_llCommandsHistoryCurrentNode.Value;

                    this.txtConsoleCommand.Select(this.txtConsoleCommand.Text.Length, 0);
                }
            }
            else if (e.KeyData == Keys.Down) {

                if (this.m_llCommandsHistoryCurrentNode != null && this.m_llCommandsHistoryCurrentNode.Previous != null) {
                    this.m_llCommandsHistoryCurrentNode = this.m_llCommandsHistoryCurrentNode.Previous;
                    this.txtConsoleCommand.Text = this.m_llCommandsHistoryCurrentNode.Value;

                    this.txtConsoleCommand.Select(this.txtConsoleCommand.Text.Length, 0);
                }

                e.SuppressKeyPress = true;
            }
        }

        private void chkEnableOutput_CheckedChanged(object sender, EventArgs e) {
            if (this.m_prcClient != null) {
                this.m_prcClient.Console.DisplayConnection = this.chkEnableOutput.Checked;
            }
        }

        private void Console_DisplayConnectionChanged(bool isEnabled) {
            this.chkEnableOutput.Checked = isEnabled;
        }


        private void chkEnablePunkbusterOutput_CheckedChanged(object sender, EventArgs e) {
            if (this.m_prcClient != null) {
                this.m_prcClient.Console.DisplayPunkbuster = this.chkEnablePunkbusterOutput.Checked;
            }
        }

        void Console_DisplayPunkbusterChanged(bool isEnabled) {
            this.chkEnablePunkbusterOutput.Checked = isEnabled;
        }

        private void chkConEnableScrolling_CheckedChanged(object sender, EventArgs e)
        {
            if (this.m_prcClient != null)
            {
                this.m_prcClient.Console.ConScrolling = this.chkConEnableScrolling.Checked;
            }
        }

        void Console_ConEnableScrollingChanged(bool isEnabled)
        {
            this.chkConEnableScrolling.Checked = isEnabled;
        }

        private void chkPBEnableScrolling_CheckedChanged(object sender, EventArgs e)
        {
            if (this.m_prcClient != null)
            {
                this.m_prcClient.Console.PBScrolling = this.chkPBEnableScrolling.Checked;
            }
        }

        void Console_PBEnableScrollingChanged(bool isEnabled)
        {
            this.chkPBEnableScrolling.Checked = isEnabled;
        }

        private void chkDebug_CheckedChanged(object sender, EventArgs e) {
            if (this.m_prcClient != null) {
                this.m_prcClient.Console.LogDebugDetails = this.chkDebug.Checked;
            }
        }

        private void Console_LogDebugDetailsChanged(bool isEnabled) {
            this.chkDebug.Checked = isEnabled;
        }

        private void chkEvents_CheckedChanged(object sender, EventArgs e) {
            if (this.m_prcClient != null) {
                this.m_prcClient.Console.LogEventsConnection = this.chkEvents.Checked;
            }
        }

        private void Console_LogEventsConnectionChanged(bool isEnabled) {
            this.chkEvents.Checked = isEnabled;
        }
    }
}
