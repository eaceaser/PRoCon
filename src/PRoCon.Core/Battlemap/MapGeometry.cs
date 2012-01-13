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
using System.Drawing;

namespace PRoCon.Core.Battlemap {
    using Core.Players;
    using Core.Remote;

    public class MapGeometry {

        private PRoConClient m_prcClient;
        private string m_currentMapFileName;

        // public delegate void MapLoadedHandler(string strMapName);
        // public event MapLoadedHandler MapLoaded;

        public delegate void MapZoneTrespassedHandler(CPlayerInfo cpiSoldier, ZoneAction action, MapZone sender, Point3D pntTresspassLocation, float flTresspassPercentage, object trespassState);
        public event MapZoneTrespassedHandler MapZoneTrespassed;

        //public string MapPath {
        //    get;
        //    private set;
        //}

        //public MapImagePackDictionary MapImagePacks {
        //    get;
        //    private set;
        //}

        //public MapImagePack LoadedMapImagePack {
        //    get;
        //    private set;
        //}

        public MapZoneDictionary MapZones {
            get;
            private set;
        }

        public MapGeometry(PRoConClient prcClient) {
            // this.MapImagePacks = new MapImagePackDictionary();
            // this.MapPath = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media"), "Maps");

            //this.LoadMapImagePacks();

            this.MapZones = new MapZoneDictionary();

            //if (this.MapImagePacks.Count > 0) {
            //    this.LoadedMapImagePack = this.MapImagePacks[0];
            //}

            if ((this.m_prcClient = prcClient) != null) {
                this.m_prcClient.Game.ServerInfo += new FrostbiteClient.ServerInfoHandler(m_prcClient_ServerInfo);
                this.m_prcClient.Game.LoadingLevel += new FrostbiteClient.LoadingLevelHandler(m_prcClient_LoadingLevel);
                this.m_prcClient.PlayerKilled += new PRoConClient.PlayerKilledHandler(m_prcClient_PlayerKilled);

            //    this.m_prcClient.LoadingLevel += new FrostbiteConnection.LoadingLevelHandler(m_prcClient_LoadingLevel);
            }

        }

        private void m_prcClient_PlayerKilled(PRoConClient sender, Kill kKillerVictimDetails) {

            float flTrespassArea = 0.0F;

            foreach (MapZoneDrawing zone in new List<MapZoneDrawing>(this.MapZones)) {

                if (String.Compare(this.m_currentMapFileName, zone.LevelFileName, true) == 0) {

                    if ((flTrespassArea = zone.TrespassArea(kKillerVictimDetails.KillerLocation, 14.14F)) > 0.0F) {
                        if (this.MapZoneTrespassed != null) {
                            FrostbiteConnection.RaiseEvent(this.MapZoneTrespassed.GetInvocationList(), kKillerVictimDetails.Killer, ZoneAction.Kill, new MapZone(zone.UID, zone.LevelFileName, zone.Tags.ToString(), zone.ZonePolygon, true), kKillerVictimDetails.KillerLocation, flTrespassArea, kKillerVictimDetails);
                        }
                    }

                    if ((flTrespassArea = zone.TrespassArea(kKillerVictimDetails.VictimLocation, 14.14F)) > 0.0F) {
                        if (this.MapZoneTrespassed != null) {
                            FrostbiteConnection.RaiseEvent(this.MapZoneTrespassed.GetInvocationList(), kKillerVictimDetails.Victim, ZoneAction.Death, new MapZone(zone.UID, zone.LevelFileName, zone.Tags.ToString(), zone.ZonePolygon, true), kKillerVictimDetails.VictimLocation, flTrespassArea, kKillerVictimDetails);
                        }
                    }
                }
            }
        }

        private void m_prcClient_LoadingLevel(FrostbiteClient sender, string mapFileName, int roundsPlayed, int roundsTotal) {
            this.m_currentMapFileName = mapFileName;
        }

        private void m_prcClient_ServerInfo(FrostbiteClient sender, CServerInfo csiServerInfo) {

            this.m_currentMapFileName = csiServerInfo.Map;

            /*
            if (this.LoadedMapImagePack != null) {

                if (this.LoadedMapImagePack.LoadedMapFileName.Length == 0 && String.Compare(this.LoadedMapImagePack.LoadedMapFileName, csiServerInfo.Map, true) != 0) {
                    // Load new map.
                    //this.CurrentMapFilename = csiServerInfo.Map.ToLower();

                    if (this.LoadedMapImagePack != null) {
                        this.LoadedMapImagePack.LoadMap(csiServerInfo.Map.ToLower());

                        if (this.MapLoaded != null) {
                            FrostbiteConnection.RaiseEvent(this.MapLoaded.GetInvocationList(), csiServerInfo.Map.ToLower());
                        }
                    }

                }
            }
            */
            //this.CurrentMapFilename = csiServerInfo.Map.ToLower();
        }

        /*
        private void LoadMapImagePacks() {

            this.MapImagePacks.Clear();

            if (Directory.Exists(this.MapPath) == true) {
                string[] a_strMapImagePacks = Directory.GetDirectories(this.MapPath);

                foreach (string strMapImagePack in a_strMapImagePacks) {
                    if (File.Exists(Path.Combine(Path.Combine(this.MapPath, strMapImagePack), "data.map")) == true) {
                        this.MapImagePacks.Add(new MapImagePack(Path.Combine(this.MapPath, strMapImagePack), new CLocalization(Path.Combine(Path.Combine(this.MapPath, strMapImagePack), "data.map"), "data.map")));
                    }
                }
            }
        }
        

        public void SetMapPack(MapImagePack mpiPack) {

            string strCurrentMapFileName = String.Empty;

            if (this.LoadedMapImagePack != null) {
                strCurrentMapFileName = this.LoadedMapImagePack.LoadedMapFileName;
            }

            this.LoadedMapImagePack = mpiPack;

            if (strCurrentMapFileName.Length > 0) {
                this.LoadedMapImagePack.LoadMap(strCurrentMapFileName);

                if (this.MapLoaded != null) {
                    FrostbiteConnection.RaiseEvent(this.MapLoaded.GetInvocationList(), strCurrentMapFileName);
                }
            }
        }
         * */
    }
}
