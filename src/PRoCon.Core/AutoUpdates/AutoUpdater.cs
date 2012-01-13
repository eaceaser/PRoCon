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
using System.IO;

using Ionic.Zip;
using System.Security.Cryptography;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace PRoCon.Core.AutoUpdates {
    using Core.Remote;

    public class AutoUpdater {

        public delegate void DownloadUnzipCompleteHandler();
        public event DownloadUnzipCompleteHandler DownloadUnzipComplete;

        public delegate void CheckingUpdatesHandler();
        public event CheckingUpdatesHandler CheckingUpdates;
        public event CheckingUpdatesHandler NoVersionAvailable;

        public delegate void UpdateDownloadingHandler(CDownloadFile cdfDownloading);
        public event UpdateDownloadingHandler UpdateDownloading;

        public delegate void CustomDownloadErrorHandler(string strError);
        public event CustomDownloadErrorHandler CustomDownloadError;

        private readonly object m_objDownloadingLocalizations = new object();
        private List<CDownloadFile> m_lstDownloadingLocalizations;

        private CDownloadFile m_cdfPRoConUpdate;

        // Now only used if they manually check for an update and it comes back false.
        //private bool m_blPopupVersionResults = false;

        private PRoConApplication m_praApplication;

        public CDownloadFile VersionChecker {
            get;
            private set;
        }

        public AutoUpdater(PRoConApplication praApplication, string[] args) {
            AutoUpdater.m_strArgs = args;
            this.m_praApplication = praApplication;

            this.VersionChecker = new CDownloadFile("http://www.phogue.net/procon/version3.php");
            this.VersionChecker.DownloadComplete += new CDownloadFile.DownloadFileEventDelegate(VersionChecker_DownloadComplete);
            this.m_lstDownloadingLocalizations = new List<CDownloadFile>();
        }

        public void CheckVersion() {
            if (this.m_praApplication.BlockUpdateChecks == false) {

                if (this.CheckingUpdates != null) {
                    FrostbiteConnection.RaiseEvent(this.CheckingUpdates.GetInvocationList());
                }

                this.VersionChecker.BeginDownload();
            }
        }

        private void DownloadLocalizationFile(string strDownloadSource, string strLocalizationFilename) {
            lock (this.m_objDownloadingLocalizations) {
                CDownloadFile cdfUpdatedLocalization = new CDownloadFile(strDownloadSource, strLocalizationFilename);
                cdfUpdatedLocalization.DownloadComplete += new CDownloadFile.DownloadFileEventDelegate(m_cdfUpdatedLocalization_DownloadComplete);
                this.m_lstDownloadingLocalizations.Add(cdfUpdatedLocalization);

                cdfUpdatedLocalization.BeginDownload();
            }
        }

        private void m_cdfUpdatedLocalization_DownloadComplete(CDownloadFile cdfSender) {

            string strLocalizationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Localization");

            try {
                if (Directory.Exists(strLocalizationFolder) == false) {
                    Directory.CreateDirectory(strLocalizationFolder);
                }

                using (ZipFile zip = ZipFile.Read(cdfSender.CompleteFileData)) {
                    zip.ExtractAll(strLocalizationFolder, ExtractExistingFileAction.OverwriteSilently);
                }

                this.m_praApplication.LoadLocalizationFiles();
                //this.Invoke(new ReloadLocalizationsDelegate(this.m_frmOptions.LoadLocalizationFiles));
            }
            catch (Exception) { }
        }

        private string MD5File(string strFileName) {
            StringBuilder sbStringifyHash = new StringBuilder();

            if (File.Exists(strFileName) == true) {
                MD5 md5Hasher = MD5.Create();

                byte[] a_bHash = md5Hasher.ComputeHash(File.ReadAllBytes(strFileName));

                for (int x = 0; x < a_bHash.Length; x++) {
                    sbStringifyHash.Append(a_bHash[x].ToString("x2"));
                }
            }

            return sbStringifyHash.ToString();
        }

        private string MD5Data(byte[] a_bData) {
            StringBuilder sbStringifyHash = new StringBuilder();

            MD5 md5Hasher = MD5.Create();

            byte[] a_bHash = md5Hasher.ComputeHash(a_bData);

            for (int x = 0; x < a_bHash.Length; x++) {
                sbStringifyHash.Append(a_bHash[x].ToString("x2"));
            }

            return sbStringifyHash.ToString();
        }

        private void VersionChecker_DownloadComplete(CDownloadFile cdfSender) {
 
            string[] a_strVersionData = System.Text.Encoding.UTF8.GetString(cdfSender.CompleteFileData).Split('\n');

            if (a_strVersionData.Length >= 4 && (this.m_cdfPRoConUpdate == null || this.m_cdfPRoConUpdate.FileDownloading == false)) {

                bool blContinueFileDownload = true;

                try {
                    if (Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updates")) == true) {
                        AssemblyName proconAssemblyName = AssemblyName.GetAssemblyName(String.Format("{0}{1}{2}{3}{4}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, "Updates", Path.DirectorySeparatorChar, "PRoCon.exe"));

                        // If an update has already been downloaded but not installed..
                        if (new Version(a_strVersionData[0]).CompareTo(proconAssemblyName.Version) >= 0) {
                            blContinueFileDownload = false;
                        }
                    }
                }
                catch (Exception e) { }

                if (blContinueFileDownload == true) {

                    if (new Version(a_strVersionData[0]).CompareTo(Assembly.GetExecutingAssembly().GetName().Version) > 0) {
                        // Download file, alert or auto apply once complete with release notes.
                        this.m_cdfPRoConUpdate = new CDownloadFile(a_strVersionData[2], a_strVersionData[3]);
                        this.m_cdfPRoConUpdate.DownloadComplete += new CDownloadFile.DownloadFileEventDelegate(cdfPRoConUpdate_DownloadComplete);

                        if (this.UpdateDownloading != null) {
                            FrostbiteConnection.RaiseEvent(this.UpdateDownloading.GetInvocationList(), this.m_cdfPRoConUpdate);
                        }

                        this.m_cdfPRoConUpdate.BeginDownload();
                    }
                    else {

                        if (this.NoVersionAvailable != null) {
                            FrostbiteConnection.RaiseEvent(this.NoVersionAvailable.GetInvocationList());
                        }

                        lock (this.m_objDownloadingLocalizations) {
                            foreach (CDownloadFile cdfFile in this.m_lstDownloadingLocalizations) {
                                cdfFile.EndDownload();
                            }

                            this.m_lstDownloadingLocalizations.Clear();
                        }

                        //ProConClient prcAnyClient = null;
                        //if (this.Connections.Count > 0) {
                        //    prcAnyClient = this.Connections[0];
                        //}


                        for (int i = 4; i < a_strVersionData.Length; i++) {
                            //string[] a_strVersionMd5s = a_strVersionData[i].Split(new char[] { ' ' });
                            List<string> lstExtensibilityVersion = Packet.Wordify(a_strVersionData[i]);

                            if (lstExtensibilityVersion.Count >= 4 && String.Compare(lstExtensibilityVersion[0], "localization", true) == 0) {

                                try {
                                    if (File.Exists(String.Format("{0}Localization{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, lstExtensibilityVersion[2])) == true) {
                                        if (String.Compare(lstExtensibilityVersion[1], this.MD5File(String.Format("{0}Localization{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, lstExtensibilityVersion[2])), true) != 0) {
                                            // Download new localization file and tell options to reload it once completed.
                                            this.DownloadLocalizationFile(lstExtensibilityVersion[3], lstExtensibilityVersion[2]);
                                            Thread.Sleep(100); // I don't know how many languages there may be later so sleep on it to prevent spam.
                                        }
                                    }
                                    else {
                                        // Download new localization file and tell options to load it once completed.
                                        this.DownloadLocalizationFile(lstExtensibilityVersion[3], lstExtensibilityVersion[2]);
                                        Thread.Sleep(100);
                                    }
                                }
                                catch (Exception) { }
                            }

                        }
                    }
                }
                else {
                    this.DownloadedUnzippedComplete();
                }
            }
        }

        private void cdfPRoConUpdate_DownloadComplete(CDownloadFile cdfSender) {

            if (String.Compare(this.MD5Data(cdfSender.CompleteFileData), (string)cdfSender.AdditionalData, true) == 0) {

                string strUpdatesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updates");

                try {
                    if (Directory.Exists(strUpdatesFolder) == false) {
                        Directory.CreateDirectory(strUpdatesFolder);
                    }

                    using (ZipFile zip = ZipFile.Read(cdfSender.CompleteFileData)) {
                        zip.ExtractAll(strUpdatesFolder, ExtractExistingFileAction.OverwriteSilently);
                    }

                    this.DownloadedUnzippedComplete();
                }
                catch (Exception e) {
                    if (this.CustomDownloadError != null) {
                        FrostbiteConnection.RaiseEvent(this.CustomDownloadError.GetInvocationList(), e.Message);
                    }

                    //this.Invoke(new DownloadErrorDelegate(DownloadError_Callback), e.Message);
                }
            }
            else {
                if (this.CustomDownloadError != null) {
                    FrostbiteConnection.RaiseEvent(this.CustomDownloadError.GetInvocationList(), "Downloaded file failed checksum, please try again or download direct from http://phogue.net");
                }

                //this.Invoke(new DownloadErrorDelegate(DownloadError_Callback), "Downloaded file failed checksum, please try again or download direct from http://phogue.net");
            }
        }

        private void DownloadedUnzippedComplete() {

            if (this.m_praApplication.OptionsSettings.AutoApplyUpdates == true) {
                AutoUpdater.BeginUpdateProcess(this.m_praApplication);
            }
            else {
                if (this.DownloadUnzipComplete != null) {
                    FrostbiteConnection.RaiseEvent(this.DownloadUnzipComplete.GetInvocationList());
                }
            }
        }

        public void Shutdown() {
            lock (this.m_objDownloadingLocalizations) {
                if (this.m_cdfPRoConUpdate != null) this.m_cdfPRoConUpdate.EndDownload();
                if (this.VersionChecker != null) this.VersionChecker.EndDownload();

                foreach (CDownloadFile cdfFile in this.m_lstDownloadingLocalizations) {
                    cdfFile.EndDownload();
                }

                this.m_lstDownloadingLocalizations.Clear();
            }
        }

        public static string[] m_strArgs;

        /*
        public static void BeginUpdateProcess() {

            // Check if the autoupdater needs updating.. if not then we'll forget about it.

            string strUpdatesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updates");

            if (Directory.Exists(strUpdatesFolder) == true) {

                int iDeleteChecker = 0;

                string strCurrentProconUpdaterPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PRoConUpdater.exe");
                // Overwrite proconupdater.exe

                if (File.Exists(Path.Combine(strUpdatesFolder, "PRoConUpdater.exe")) == true) {

                    do {
                        try {
                            File.Copy(Path.Combine(strUpdatesFolder, "PRoConUpdater.exe"), strCurrentProconUpdaterPath, true);

                            File.Delete(Path.Combine(strUpdatesFolder, "PRoConUpdater.exe"));
                        }
                        catch (Exception) { }

                        Thread.Sleep(100);
                        iDeleteChecker++;
                    } while (File.Exists(Path.Combine(strUpdatesFolder, "PRoConUpdater.exe")) == true && iDeleteChecker < 10);
                }

                if (File.Exists(strCurrentProconUpdaterPath) == true) {

                    if (AutoUpdater.m_strArgs.Length == 0) {
                        System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "PRoConUpdater.exe");
                    }
                    else {
                        System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "PRoConUpdater.exe", String.Join(" ", AutoUpdater.m_strArgs));
                    }

                    Application.Exit();
                }
            }
        }
        */

        public static void BeginUpdateProcess(PRoConApplication praApplication) {

            // Check if the autoupdater needs updating.. if not then we'll forget about it.

            string strUpdatesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Updates");

            if (Directory.Exists(strUpdatesFolder) == true) {
                AssemblyName proconUpdaterAssemblyName = null;
                AssemblyName proconUpdaterUpdatesDirAssemblyName = null;

                if (File.Exists(String.Format("{0}{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, "PRoConUpdater.exe")) == true) {
                    proconUpdaterAssemblyName = AssemblyName.GetAssemblyName(String.Format("{0}{1}{2}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, "PRoConUpdater.exe"));
                }
                if (File.Exists(String.Format("{0}{1}{2}{3}{4}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, "Updates", Path.DirectorySeparatorChar, "PRoConUpdater.exe")) == true) {
                    proconUpdaterUpdatesDirAssemblyName = AssemblyName.GetAssemblyName(String.Format("{0}{1}{2}{3}{4}", AppDomain.CurrentDomain.BaseDirectory, Path.DirectorySeparatorChar, "Updates", Path.DirectorySeparatorChar, "PRoConUpdater.exe"));
                }

                // If the old updater is.. old =)
                if ((proconUpdaterAssemblyName == null && proconUpdaterUpdatesDirAssemblyName != null) || (proconUpdaterAssemblyName != null && proconUpdaterUpdatesDirAssemblyName != null && proconUpdaterUpdatesDirAssemblyName.Version.CompareTo(proconUpdaterAssemblyName.Version) > 0)) {
                    //int iDeleteChecker = 0;

                    string strCurrentProconUpdaterPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PRoConUpdater.exe");
                    // Overwrite proconupdater.exe

                    //do {
                    //    //if (iDeleteChecker > 0) {
                    //    //    MessageBox.Show("Please close the PRoConUpdater to continue the update process..");
                    //    //}

                    //    try {
                    //        //File.Delete(strCurrentProconUpdaterPath);
                    //    }
                    //    catch (Exception) { }

                    //    Thread.Sleep(100);
                    //    iDeleteChecker++;
                    //} while (File.Exists(strCurrentProconUpdaterPath) == true && iDeleteChecker < 5);

                    try {
                        try {
                            File.Copy(Path.Combine(strUpdatesFolder, "PRoConUpdater.exe"), strCurrentProconUpdaterPath, true);
                            File.Delete(Path.Combine(strUpdatesFolder, "PRoConUpdater.exe"));
                        }
                        catch (Exception) { }

                        if (AutoUpdater.m_strArgs != null && AutoUpdater.m_strArgs.Length == 0) {
                            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "PRoConUpdater.exe");
                        }
                        else {
                            System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "PRoConUpdater.exe", String.Join(" ", AutoUpdater.m_strArgs));
                        }

                        if (praApplication != null) {
                            praApplication.Shutdown();
                        }

                        Application.Exit();
                    }
                    catch (Exception) { }

                }
                else {
                    // Same or newer version, we're running with the same autoupdater.exe
                    if (AutoUpdater.m_strArgs != null && AutoUpdater.m_strArgs.Length == 0) {
                        System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "PRoConUpdater.exe");
                    }
                    else {
                        System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "PRoConUpdater.exe", String.Join(" ", AutoUpdater.m_strArgs));
                    }

                    if (praApplication != null) {
                        praApplication.Shutdown();
                    }

                    //System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "PRoConUpdater.exe");
                    Application.Exit();
                }
            }
        }

        
    }
}
