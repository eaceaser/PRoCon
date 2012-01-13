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

namespace PRoCon.Core.Players.Items {
    [Serializable]
    public class Weapon {

        public Kits KitRestriction {
            get;
            private set;
        }

        public string Name {
            get;
            private set;
        }

        public WeaponSlots Slot {
            get;
            private set;
        }

        public DamageTypes Damage {
            get;
            private set;
        }

        public Weapon(Kits restriction, string name, WeaponSlots slot, DamageTypes damage) {
            this.KitRestriction = restriction;
            this.Name = name;
            this.Slot = slot;
            this.Damage = damage;
        }
    }
}
