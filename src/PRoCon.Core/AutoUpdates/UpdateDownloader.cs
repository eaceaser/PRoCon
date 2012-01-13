using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using Ionic.Zip;
using System.Security.Cryptography;

namespace PRoCon.Core.AutoUpdates {
    using Core.Remote;

    public class UpdateDownloader {

        public delegate void DownloadUnzipCompleteHandler();
        public event DownloadUnzipCompleteHandler DownloadUnzipComplete;

        public delegate void UpdateDownloadingHandler(CDownloadFile cdfDownloading);
        public event UpdateDownloadingHandler UpdateDownloading;

        public delegate void CustomDownloadErrorHandler(string strError);
        public event CustomDownloadErrorHandler CustomDownloadError;

        private CDownloadFile m_cdfPRoConUpdate;

        private string m_updatesDirectoryName;

        public CDownloadFile VersionChecker {
            get;
            private set;
        }

        public UpdateDownloader(string updatesDirectoryName) {
            this.m_updatesDirectoryName = updatesDirectoryName;
            this.VersionChecker = new CDownloadFile("http://www.phogue.net/procon/version3.php");
            this.VersionChecker.DownloadComplete += new CDownloadFile.DownloadFileEventDelegate(VersionChecker_DownloadComplete);
        }

        public void DownloadLatest() {
            this.VersionChecker.BeginDownload();
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

                // Download file, alert or auto apply once complete with release notes.
                this.m_cdfPRoConUpdate = new CDownloadFile(a_strVersionData[2], a_strVersionData[3]);
                this.m_cdfPRoConUpdate.DownloadComplete += new CDownloadFile.DownloadFileEventDelegate(cdfPRoConUpdate_DownloadComplete);

                if (this.UpdateDownloading != null) {
                    FrostbiteConnection.RaiseEvent(this.UpdateDownloading.GetInvocationList(), this.m_cdfPRoConUpdate);
                }

                this.m_cdfPRoConUpdate.BeginDownload();
            }
        }

        private void cdfPRoConUpdate_DownloadComplete(CDownloadFile cdfSender) {

            if (String.Compare(this.MD5Data(cdfSender.CompleteFileData), (string)cdfSender.AdditionalData, true) == 0) {

                string strUpdatesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.m_updatesDirectoryName);

                try {
                    if (Directory.Exists(strUpdatesFolder) == false) {
                        Directory.CreateDirectory(strUpdatesFolder);
                    }

                    using (ZipFile zip = ZipFile.Read(cdfSender.CompleteFileData)) {
                        zip.ExtractAll(strUpdatesFolder, ExtractExistingFileAction.OverwriteSilently);
                    }

                    if (this.DownloadUnzipComplete != null) {
                        FrostbiteConnection.RaiseEvent(this.DownloadUnzipComplete.GetInvocationList());
                    }
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

    }
}
