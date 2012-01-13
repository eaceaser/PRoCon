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
using System.Collections.ObjectModel;
using System.Text;

namespace PRoCon.Core.Players {
    using Core.Remote;

    public class PlayerDictionary : KeyedCollection<string, CPlayerInfo> {

        public delegate void PlayerAlteredHandler(CPlayerInfo item);
        public event PlayerAlteredHandler PlayerAdded;
        public event PlayerAlteredHandler PlayerUpdated;
        public event PlayerAlteredHandler PlayerRemoved;

        public PlayerDictionary() {

        }

        public PlayerDictionary(IEnumerable<CPlayerInfo> playerList) {
            foreach (CPlayerInfo cpi in playerList) {
                this.Add(cpi);
            }
        }

        protected override string GetKeyForItem(CPlayerInfo item) {
            return item.SoldierName;
        }

        protected override void InsertItem(int index, CPlayerInfo item) {
            if (this.PlayerAdded != null) {
                FrostbiteConnection.RaiseEvent(this.PlayerAdded.GetInvocationList(), item);
            }

            base.InsertItem(index, item);
        }

        protected override void RemoveItem(int index) {

            if (this.PlayerUpdated != null) {
                FrostbiteConnection.RaiseEvent(this.PlayerUpdated.GetInvocationList(), this[index]);
            }

            base.RemoveItem(index);
        }

        protected override void SetItem(int index, CPlayerInfo item) {
            if (this.PlayerRemoved != null) {
                FrostbiteConnection.RaiseEvent(this.PlayerRemoved.GetInvocationList(), item);
            }

            base.SetItem(index, item);
        }

        public string ToJsonString() {

            Hashtable players = new Hashtable();

            ArrayList playersList = new ArrayList();
            foreach (CPlayerInfo playerInfo in this) {

                Hashtable player = new Hashtable();

                player.Add("clan_tag", playerInfo.ClanTag);
                player.Add("deaths", playerInfo.Deaths);
                player.Add("guid", playerInfo.GUID);
                player.Add("kills", playerInfo.Kills);

                player.Add("ping", playerInfo.Ping);
                player.Add("score", playerInfo.Score);
                player.Add("name", playerInfo.SoldierName);
                player.Add("squad_id", playerInfo.SquadID);
                player.Add("team_id", playerInfo.TeamID);

                //players.Add(playerInfo.SoldierName, player);

                playersList.Add(player);
            }

            players.Add("players", playersList);

            return JSON.JsonEncode(players);
        }
    }

}
