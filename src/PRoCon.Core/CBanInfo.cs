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

namespace PRoCon.Core {
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class CBanInfo {
        public CBanInfo(string strIdType, string strId) {
            this.IdType = strIdType;
            if (String.Compare(strIdType, "name") == 0) {
                this.SoldierName = strId;
            }
            else if (String.Compare(strIdType, "ip") == 0) {
                this.IpAddress = strId;
            }
            else if (String.Compare(strIdType, "guid") == 0) {
                this.Guid = strId;
            }

            //this.m_ui32BanLength = 0;
            //this.m_ui32Time = 0;
            this.Reason = String.Empty;
            this.BanLength = new TimeoutSubset(TimeoutSubset.TimeoutSubsetType.None);
        }

        public CBanInfo(List<string> lstBanWords) {
            // Id-type, id, ban-type, time and reason
            // Used to pull data from a banList.list command which is always 5 words.
            if (lstBanWords.Count == 5) {

                this.IdType = lstBanWords[0];
                this.BanLength = new TimeoutSubset(lstBanWords.GetRange(2, 2));

                if (String.Compare(lstBanWords[0], "name") == 0) {
                    this.SoldierName = lstBanWords[1];
                }
                else if (String.Compare(lstBanWords[0], "ip") == 0) {
                    this.IpAddress = lstBanWords[1];
                }
                else if (String.Compare(lstBanWords[0], "guid") == 0) {
                    this.Guid = lstBanWords[1];
                }

                this.Reason = lstBanWords[4];
            }
        }

        // Only used for a pbguid
        public CBanInfo(string strSoldierName, string strGUID, string strIP, TimeoutSubset ctsBanLength,
                       string strReason) {
            this.SoldierName = strSoldierName;
            this.Guid = strGUID;
            this.IpAddress = strIP;
            this.IdType = "pbguid";

            this.BanLength = ctsBanLength;
            this.Reason = strReason;
        }

        // Ban added
        public CBanInfo(string strBanType, string strId, TimeoutSubset ctsBanLength, string strReason) {
            this.IdType = strBanType;
            this.BanLength = ctsBanLength;
            this.Reason = strReason;

            if (String.Compare(this.IdType, "name") == 0) {
                this.SoldierName = strId;
            }
            else if (String.Compare(this.IdType, "ip") == 0) {
                this.IpAddress = strId;
            }
            else if (String.Compare(this.IdType, "guid") == 0) {
                this.Guid = strId;
            }
        }

        public string SoldierName { get; private set; }

        public string Guid { get; private set; }

        public string IpAddress { get; private set; }

        public string IdType { get; private set; }

        public string Reason { get; private set; }

        public TimeoutSubset BanLength { get; private set; }

        public static List<CBanInfo> GetVanillaBanlist(List<string> lstWords) {

            List<CBanInfo> lstBans = new List<CBanInfo>();
            int iBans = 0;

            if (lstWords.Count >= 1 && int.TryParse(lstWords[0], out iBans) == true) {
                lstWords.RemoveAt(0);
                for (int i = 0; i < iBans; i++) {
                    lstBans.Add(new CBanInfo(lstWords.GetRange(i * 5, 5)));
                }
            }

            return lstBans;
        }
    }
}