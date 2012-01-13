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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Security.Cryptography;
using Ionic.Zip;

namespace PRoCon.Core.Packages {
    public class Package {

        #region Properties

        public string Uid { get; private set; }
        public PackageType PackageType { get; private set; }
        public Version Version { get; private set; }

        public string Name { get; private set; }
        public string Image { get; private set; }
        public string ForumLink { get; private set; }
        public string Author { get; private set; }
        public string Website { get; private set; }
        public List<string> Tags { get; private set; }
        public string Description { get; private set; }

        public List<string> Files { get; private set; }

        // From RSS Only
        public int Downloads { get; private set; }
        public string Md5 { get; private set; }
        public DateTime LastModified { get; private set; }
        public int FileSize { get; private set; }

        // Install/Download status
        public string Error { get; private set; }

        #endregion

        #region Events

        public delegate void PackageEventHandler(Package sender);
        public event PackageEventHandler PackageBeginUnzip;
        public event PackageEventHandler PackageEndUnzip;

        public delegate void PackageCustomDownloadErrorHandler(Package sender, string error);
        public event PackageCustomDownloadErrorHandler PackageCustomDownloadError;

        public delegate void DownloadFileEventHandler(Package sender, CDownloadFile file);
        public event DownloadFileEventHandler DownloadComplete;
        public event DownloadFileEventHandler DownloadError;
        public event DownloadFileEventHandler DownloadProgressUpdate;

        #endregion

        private void SetNull() {
            this.Uid = String.Empty;
            this.PackageType = Packages.PackageType.None;
            this.Version = null;

            this.Name = String.Empty;
            this.Image = String.Empty;
            this.ForumLink = String.Empty;
            this.Author = String.Empty;
            this.Website = String.Empty;
            this.Tags = new List<string>();
            this.Description = String.Empty;

            this.Files = new List<string>();

            // From RSS Only
            this.Downloads = 0;
            this.Md5 = String.Empty;
            this.LastModified = DateTime.Now;
            this.FileSize = 0;

        }

        public Package(string xml) {
            this.SetNull();

            this.FromXml(xml);
        }

        public Package(string uid, string version, string md5) {
            this.SetNull();

            this.Uid = String.Empty;
            this.Version = new Version(version);
            this.Md5 = String.Empty;
        }

        /*
            <package>
	            <released>true</released>

	            <uid>BFBCSStatsphile</uid>
	            <version>1.0.0.0</version>
	            <type>plugin</type>

	            <name>BFBCS Statsphile In-Game API</name>
	            <image>http://phogue.net/procon/extensions/phogue/logo.png</image>

	            <forumlink><![CDATA[http://phogue.net/forum/viewtopic.php?f=18&t=794]]></forumlink>
	            <author>Phogue</author>
	            <website>http://phogue.net</website>
	            <tags>
		            <tag>stats</tag>
		            <tag>battlefield</tag>
		            <tag>bfbcs</tag>
	            </tags>
	            <description>Allows players to pull stats of themselves or other players in game with a !bfbcs command</description>

	            <files>
		            <file path="plugins/BFBCSStatsphile.cs"/>
	            </files>
            </package>
        */
        private void FromXml(string xml) {

            XmlDocument xmlPackage = new XmlDocument();

            try {
                xmlPackage.LoadXml(xml);

                this.Uid = this.SelectSingleNodeInnerText(xmlPackage, "package/uid");
                this.Version = new Version(this.SelectSingleNodeInnerText(xmlPackage, "package/version"));
                
                switch(this.SelectSingleNodeInnerText(xmlPackage, "package/type").ToLower()) {
                    case "plugin":
                        this.PackageType = Packages.PackageType.Plugin;
                        break;
                    case "application":
                        this.PackageType = Packages.PackageType.Application;
                        break;
                    case "language":
                        this.PackageType = Packages.PackageType.Language;
                        break;
                    case "mappack":
                        this.PackageType = Packages.PackageType.Mappack;
                        break;
                    case "config":
                        this.PackageType = Packages.PackageType.Config;
                        break;
                    default:
                        this.PackageType = Packages.PackageType.Plugin;
                        break;
                }

                this.Name = this.SelectSingleNodeInnerText(xmlPackage, "package/name");
                this.Image = this.SelectSingleNodeInnerText(xmlPackage, "package/image");
                this.ForumLink = this.SelectSingleNodeInnerText(xmlPackage, "package/forumlink");
                this.Author = this.SelectSingleNodeInnerText(xmlPackage, "package/author");
                this.Website = this.SelectSingleNodeInnerText(xmlPackage, "package/website");
                this.Description = this.SelectSingleNodeInnerText(xmlPackage, "package/description");

                foreach (XmlNode node in xmlPackage.SelectSingleNode("package/tags").ChildNodes) {
                    this.Tags.Add(node.InnerText);
                }

                foreach (XmlNode node in xmlPackage.SelectSingleNode("package/files").ChildNodes) {

                    if (node.Attributes["path"] != null) {
                        this.Files.Add(node.Attributes["path"].InnerText);
                    }
                }

                // Below may or may not appear depending on the source of the document.

                string downloadstring = this.SelectSingleNodeInnerText(xmlPackage, "package/downloads");
                int downloads = 0;
                if (int.TryParse(downloadstring, out downloads) == true) {
                    this.Downloads = downloads;
                }

                this.Md5 = this.SelectSingleNodeInnerText(xmlPackage, "package/md5");

                string lastmodifiedstring = this.SelectSingleNodeInnerText(xmlPackage, "package/lastmodified");
                long lastmodified = 0;
                if (long.TryParse(lastmodifiedstring, out lastmodified) == true) {
                    this.LastModified = (new DateTime(1970, 1, 1, 0, 0, 0)).AddSeconds(lastmodified);
                }

                string filesizestring = this.SelectSingleNodeInnerText(xmlPackage, "package/filesize");
                int filesize = 0;
                if (int.TryParse(filesizestring, out filesize) == true) {
                    this.FileSize = filesize;
                }
            }
            catch (Exception) { }
        }

        public string SelectSingleNodeInnerText(XmlDocument document, string xpath) {

            string innerText = String.Empty;

            XmlNode node = document.SelectSingleNode(xpath);

            if (node != null) {
                innerText = node.InnerText;
            }

            return innerText;
        }

        #region Downloading Packages

        public void DownloadPackage(string extractPath) {
            CDownloadFile packageDownload = null;

            if (this.Uid.Length > 0 && this.Md5.Length > 0 && this.Version != null) {
                packageDownload = new CDownloadFile(String.Format("http://phogue.net/procon/packages/download.php?uid={0}&version={1}", this.Uid, this.Version), extractPath);

                packageDownload.DownloadComplete += new CDownloadFile.DownloadFileEventDelegate(packageDownload_DownloadComplete);
                packageDownload.DownloadError += new CDownloadFile.DownloadFileEventDelegate(packageDownload_DownloadError);
                packageDownload.DownloadProgressUpdate += new CDownloadFile.DownloadFileEventDelegate(packageDownload_DownloadProgressUpdate);

                packageDownload.BeginDownload();
            }
        }

        private void packageDownload_DownloadProgressUpdate(CDownloadFile sender) {
            if (this.DownloadProgressUpdate != null) {
                this.DownloadProgressUpdate(this, sender);
            }
        }

        private void packageDownload_DownloadError(CDownloadFile sender) {
            if (this.DownloadError != null) {
                this.Error = sender.Error;
                this.DownloadError(this, sender);
            }
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

        private void packageDownload_DownloadComplete(CDownloadFile sender) {
            if (this.DownloadComplete != null) {
                this.DownloadComplete(this, sender);
            }

            if (String.Compare(this.MD5Data(sender.CompleteFileData), this.Md5, true) == 0) {

                string strUpdatesFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, (string)sender.AdditionalData);

                try {
                    if (Directory.Exists(strUpdatesFolder) == false) {
                        Directory.CreateDirectory(strUpdatesFolder);
                    }

                    if (this.PackageBeginUnzip != null) {
                        this.PackageBeginUnzip(this);
                    }

                    using (ZipFile zip = ZipFile.Read(sender.CompleteFileData)) {
                        zip.ExtractAll(strUpdatesFolder, ExtractExistingFileAction.OverwriteSilently);
                    }

                    if (this.PackageEndUnzip != null) {
                        this.PackageEndUnzip(this);
                    }
                }
                catch (Exception e) {
                    if (this.PackageCustomDownloadError != null) {
                        this.Error = e.Message;
                        this.PackageCustomDownloadError(this, e.Message);
                    }
                }
            }
            else {
                this.Error = "Downloaded file failed checksum, please try again or download direct from http://phogue.net";
                if (this.PackageCustomDownloadError != null) {
                    this.PackageCustomDownloadError(this, "Downloaded file failed checksum, please try again or download direct from http://phogue.net");
                }
            }
        }

        #endregion

        // Stole this from a quick google search, but corrected the units
        // http://sharpertutorials.com/pretty-format-bytes-kb-mb-gb/
        private string FormatBytes(long bytes) {
            const int scale = 1024;

            string[] orders = new string[] { "TiB", "GiB", "MiB", "KiB", "Bytes" };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (var order in orders) {
                if (bytes > max) {
                    return string.Format("{0:##.##} {1}", Decimal.Divide(bytes, max), order);
                }

                max /= scale;
            }

            return "0 Bytes";
        }

        public Hashtable ToHashTable() {

            Hashtable hashPackage = new Hashtable();

            hashPackage.Add("uid", this.Uid);
            hashPackage.Add("type", this.PackageType.ToString());
            hashPackage.Add("type_loc", this.PackageType.ToString());
            hashPackage.Add("name", this.Name);
            hashPackage.Add("version", this.Version.ToString());
            hashPackage.Add("lastupdate", this.LastModified.ToShortDateString());
            hashPackage.Add("filesize", this.FormatBytes(this.FileSize));
            hashPackage.Add("downloads", this.Downloads.ToString());
            hashPackage.Add("imagelink", this.Image);
            hashPackage.Add("forumlink", this.ForumLink);
            hashPackage.Add("author", this.Author);
            hashPackage.Add("website", this.Website);
            hashPackage.Add("tags", String.Join(" ", this.Tags.ToArray()));
            hashPackage.Add("description", this.Description);

            return hashPackage;
        }
    }
}
