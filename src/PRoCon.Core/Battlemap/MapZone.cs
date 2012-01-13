﻿/*  Copyright 2010 Geoffrey 'Phogue' Green

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

namespace PRoCon.Core.Battlemap {

    [Serializable]
    public class MapZone {

        public string LevelFileName {
            get;
            private set;
        }

        public string UID {
            get;
            set;
        }

        // TODO: Unused at the time of writing.  Zones are only Inclusive of the area defined.
        public bool ZoneInclusive {
            get;
            set;
        }

        public Point3D[] ZonePolygon {
            get;
            set;
        }

        public ZoneTagList Tags {
            get;
            private set;
        }

        public MapZone(string strLevelFileName) {
            this.LevelFileName = strLevelFileName;
            this.Tags = new ZoneTagList();
        }

        public MapZone(string strUid, string strLevelFileName, string strTagList, Point3D[] a_pntZonePolygon, bool blInclusive) {
            this.LevelFileName = strLevelFileName;
            this.UID = strUid;
            this.ZonePolygon = a_pntZonePolygon;
            this.ZoneInclusive = blInclusive;

            this.Tags = new ZoneTagList(strTagList);
            
        }
    }
}
