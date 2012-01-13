﻿// Copyright 2010 Geoffrey 'Phogue' Green
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

namespace PRoCon.Controls.Battlemap {
    // Only used to store icons and fade times of a kill.
    // This class will be removed once I move the kills over to the new MapObjects.
    public class KillDisplayDetails {

        public float Opacity {
            get;
            set;
        }

        public bool IsMouseOver {
            get;
            set;
        }

        public DateTime TimeOfFadeoutStart {
            get;
            set;
        }

        public KillDisplayDetails(DateTime dtFadeoutStart) {
            this.TimeOfFadeoutStart = dtFadeoutStart;
            this.Opacity = 0.0F;
        }
    }
}
