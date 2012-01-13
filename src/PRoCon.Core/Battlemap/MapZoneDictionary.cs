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
using System.Collections.ObjectModel;
using System.Text;

namespace PRoCon.Core.Battlemap {
    using Core.Remote;

    [Serializable]
    public class MapZoneDictionary : KeyedCollection<string, MapZoneDrawing> {

        public delegate void MapZoneAlteredHandler(MapZoneDrawing item);
        public event MapZoneAlteredHandler MapZoneAdded;
        public event MapZoneAlteredHandler MapZoneChanged;
        public event MapZoneAlteredHandler MapZoneRemoved;

        protected override string GetKeyForItem(MapZoneDrawing item) {
            return item.UID;
        }

        protected override void InsertItem(int index, MapZoneDrawing item) {
            base.InsertItem(index, item);
            item.TagsEdited += new MapZoneDrawing.TagsEditedHandler(item_TagsEdited);

            if (this.MapZoneAdded != null) {
                FrostbiteConnection.RaiseEvent(this.MapZoneAdded.GetInvocationList(), item);
            }
        }

        private void item_TagsEdited(MapZoneDrawing sender) {
            if (this.MapZoneChanged != null) {
                FrostbiteConnection.RaiseEvent(this.MapZoneChanged.GetInvocationList(), sender);
            }
        }

        protected override void RemoveItem(int index) {

            MapZoneDrawing apRemoved = this[index];
            apRemoved.TagsEdited -= new MapZoneDrawing.TagsEditedHandler(item_TagsEdited);

            base.RemoveItem(index);

            if (this.MapZoneRemoved != null) {
                FrostbiteConnection.RaiseEvent(this.MapZoneRemoved.GetInvocationList(), apRemoved);
            }
        }

        protected override void SetItem(int index, MapZoneDrawing item) {
            if (this.MapZoneChanged != null) {
                FrostbiteConnection.RaiseEvent(this.MapZoneChanged.GetInvocationList(), item);
            }

            base.SetItem(index, item);
            item.TagsEdited += new MapZoneDrawing.TagsEditedHandler(item_TagsEdited);
        }

        public void CreateMapZone(string mapFileName, Point3D[] points) {

            Random random = new Random();
            string strUid = String.Empty;

            do {
                strUid = String.Format("{0}{1}", mapFileName, random.Next());
                strUid = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(strUid));
            } while (this.Contains(strUid) == true);

            this.Add(new MapZoneDrawing(strUid, mapFileName, "", points, true));
        }

        public void ModifyMapZonePoints(string strUid, Point3D[] points) {

            if (this.Contains(strUid) == true) {
                // this[strUid].LevelFileName = mapFileName;
                this[strUid].ZonePolygon = points;

                if (this.MapZoneChanged != null) {
                    FrostbiteConnection.RaiseEvent(this.MapZoneChanged.GetInvocationList(), this[strUid]);
                }
            }

        }
    }
}
