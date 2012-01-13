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
using System.IO;
using System.Text.RegularExpressions;

namespace PRoCon.Core {
    using Core.Remote;
    public class CLocalization {

        // VariableName=LocalizedString
        private Dictionary<string, string> m_dicLocalizedStrings;

        private string m_strLocalizationFileName;
        private string m_strLocalizationFilePath;

        public string FilePath {
            get { return this.m_strLocalizationFilePath; }
        }

        public string FileName {
            get { return this.m_strLocalizationFileName; }
        }

        public CLocalization() {
            this.m_strLocalizationFileName = String.Empty;
            this.m_strLocalizationFilePath = String.Empty;
            this.m_dicLocalizedStrings = new Dictionary<string, string>();
        }

        // string strLocalizationFilePath,
        public CLocalization(string strLocalizationFilePath, string strLocalizationFileName) {

            this.m_strLocalizationFileName = strLocalizationFileName;
            this.m_strLocalizationFilePath = strLocalizationFilePath;
            this.m_dicLocalizedStrings = new Dictionary<string, string>();

            try {
                string strFullLocalizationFile = Encoding.Unicode.GetString(File.ReadAllBytes(this.m_strLocalizationFilePath));

                MatchCollection mtcAllVariables = Regex.Matches(strFullLocalizationFile, "^(.*?)=(.*?)[\\r]?$", RegexOptions.Multiline);
                
                foreach (Match mtVariable in mtcAllVariables) {

                    if (this.m_dicLocalizedStrings.ContainsKey(mtVariable.Groups[1].Value) == false) {
                        this.m_dicLocalizedStrings.Add(mtVariable.Groups[1].Value, mtVariable.Groups[2].Value);
                    }
                    else {
                        this.m_dicLocalizedStrings[mtVariable.Groups[1].Value] = mtVariable.Groups[2].Value;
                    }
                }

            }
            catch (Exception e) {
                FrostbiteConnection.LogError("CLocalization", String.Empty, e);
                // TO DO: Nice error message for loading localization file error.
            }
        }

        public bool LocalizedExists(string strVariable) {
            return this.m_dicLocalizedStrings.ContainsKey(strVariable);
        }

        public bool TryGetLocalized(out string strLocalizedText, string strVariable, params object[] a_strArguements) {

            bool blFoundLocalized = false;
            strLocalizedText = String.Empty;

            if (this.m_dicLocalizedStrings.ContainsKey(strVariable) == true) {
                if (a_strArguements == null) {
                    strLocalizedText = this.m_dicLocalizedStrings[strVariable];
                    blFoundLocalized = true;
                }
                else {
                    try {
                        strLocalizedText = String.Format(this.m_dicLocalizedStrings[strVariable], a_strArguements);
                        blFoundLocalized = true;
                    }
                    catch (Exception) {
                        blFoundLocalized = false;
                    }
                }
            }

            return blFoundLocalized;
        }

        public string GetDefaultLocalized(string defaultText, string variable, params object[] arguements) {
            string returnText = defaultText;

            if (this.TryGetLocalized(out returnText, variable, arguements) == false) {
                returnText = defaultText;
            }

            return returnText;
        }

        public string GetLocalized(string strVariable, params string[] a_strArguements) {
            string strReturn = String.Empty;

            if (this.m_dicLocalizedStrings.ContainsKey(strVariable) == true) {

                if (a_strArguements == null) {
                    strReturn = this.m_dicLocalizedStrings[strVariable];
                }
                else {
                    try {
                        strReturn = String.Format(this.m_dicLocalizedStrings[strVariable], a_strArguements);
                    }
                    catch (FormatException) {
                        strReturn = "{FE: " + this.m_dicLocalizedStrings[strVariable] + "}";
                    }
                    catch (Exception) {
                        // So people can debug their localized file.
                        strReturn = this.m_dicLocalizedStrings[strVariable];
                    }
                }
            }
            else {
                strReturn = "{MISSING: " + strVariable + "}";
            }

            return strReturn;
        }

        public void SetLocalized(string strVariable, string strValue) {
            
            try {
                
                string strFullFileContents;
                
                using (StreamReader streamReader = new StreamReader(this.m_strLocalizationFilePath, Encoding.Unicode)) {
                    strFullFileContents = streamReader.ReadToEnd();
                }

                strFullFileContents = Regex.Replace(strFullFileContents, String.Format("^{0}=(.*?)[\\r]?$", strVariable), String.Format("{0}={1}", strVariable, strValue), RegexOptions.Multiline);

                using (StreamWriter streamWriter = new StreamWriter(this.m_strLocalizationFilePath, false, Encoding.Unicode)) {
                    streamWriter.Write(strFullFileContents);
                }

                if (this.m_dicLocalizedStrings.ContainsKey(strVariable) == true) {
                    this.m_dicLocalizedStrings[strVariable] = strValue;
                }

            }
            catch (Exception) {

            }
        }
        /*
        public string GetLocalized(string strVariable, string[] a_strArguements) {
            string strReturn = String.Empty;

            if (this.m_dicLocalizedStrings.ContainsKey(strVariable) == true) {

                if (a_strArguements == null) {
                    strReturn = this.m_dicLocalizedStrings[strVariable];
                }
                else {
                    try {
                        strReturn = String.Format(this.m_dicLocalizedStrings[strVariable], a_strArguements);
                    }
                    catch (FormatException) {
                        strReturn = "{FE: " + this.m_dicLocalizedStrings[strVariable] + "}";
                    }
                    catch (Exception) {
                        // So people can debug their localized file.
                        strReturn = this.m_dicLocalizedStrings[strVariable];
                    }
                }
            }
            else {
                strReturn = "{MISSING: " + strVariable + "}";
            }

            return strReturn;
        }
        */
    }
}
