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

// TO DO: Cleanup..
using System;
using System.Collections.Generic;
using System.Windows.Forms;

using System.Collections;
using System.Globalization;

using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Security;
using System.Security.Permissions;
using System.Runtime.InteropServices;

using System.Threading;
using System.Security.Policy;
using System.Net;
using System.Web;
using System.Xml;

using System.Security.Cryptography;
using PRoCon.Core.Events;

using System.ComponentModel;

namespace PRoCon {
    using Core.AutoUpdates;
    using Forms;
    using Core;
    using Core.Plugin.Commands;
    using Core.Battlemap;
    using Core.Remote;
    
    public static class Program {


        public static PRoConApplication m_application;
        public static string[] m_Args;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {

            Program.m_Args = args;

            if (PRoConApplication.IsProcessOpen() == false) {

                if (Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updates")) == true) {
                    AutoUpdater.m_strArgs = args;
                    AutoUpdater.BeginUpdateProcess(null);
                }
                else {
                    //PRoConApplication paProcon = null;

                    try {

                        bool blBasicConsole = false;
                        bool blGspUpdater = false;

                        int iValue;
                        if (args != null && args.Length >= 2) {
                            for (int i = 0; i < args.Length; i = i + 2) {
                                if (String.Compare("-console", args[i], true) == 0 && int.TryParse(args[i + 1], out iValue) == true && iValue == 1) {
                                    blBasicConsole = true;
                                }
                                if (String.Compare("-gspupdater", args[i], true) == 0 && int.TryParse(args[i + 1], out iValue) == true && iValue == 1) {
                                    blGspUpdater = true;
                                }
                            }
                        }

                        //Thread.Sleep(5000);
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);

                        if (blGspUpdater == true) {
                            Application.Run(new GspUpdater());
                        }
                        else {

                            if (blBasicConsole == true) {
                                BasicConsole basicWindow = new BasicConsole();
                                basicWindow.WindowLoaded += new BasicConsole.WindowLoadedHandler(procon_WindowLoaded);

                                Application.Run(basicWindow);

                            }
                            else {
                                frmMain mainWindow = new frmMain(args);
                                mainWindow.WindowLoaded += new frmMain.WindowLoadedHandler(procon_WindowLoaded);
                                Application.Run(mainWindow);
                            }

                        }
                    }
                    catch (Exception e) {

                        FrostbiteConnection.LogError("Application error", String.Empty, e);
                        MessageBox.Show("PRoCon ran into a critical error, but hopefully it logged that error in DEBUG.txt.  Please post/pm/email this to phogue at www.phogue.net.");
                    }
                    finally {
                        if (Program.m_application != null) {
                            Program.m_application.Shutdown();
                        }
                    }
                }
            }
            else {
                // Possible prevention of a cpu consumption bug I can see at the time of writing.
                // TCAdmin: Start procon.exe
                // procon.exe has an update to install
                // procon.exe loads proconupdater.exe
                // procon.exe unloads
                // proconupdater.exe begins update
                // TCAdmin detects procon.exe shutdown - attempts to reload
                Thread.Sleep(50);
            }
        }
        
        static PRoConApplication procon_WindowLoaded(bool execute) {
            Program.m_application = new PRoConApplication(false, Program.m_Args);

            if (execute == true) {
                Program.m_application.Execute();
            }

            return Program.m_application;
        }

    }

}
