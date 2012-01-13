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

namespace PRoCon.Db.Domain
{
    using System;
    using NHibernate.Mapping.Attributes;

    [Class (NameType = typeof (Kill), Table = "kill")]
    public class Kill
    {
        [Id (Name = "Id", TypeType = typeof (long), Column = "id")]
        [Generator (1, Class = "native")]
        public virtual long Id { get; set; }

        [ManyToOne(ClassType = typeof(Server), Column = "server_id")]
        public virtual Server Server { get; set; }

        [Property(Column = "kill_timestamp")]
        public virtual DateTime Timestamp { get; set; }

        [ManyToOne(ClassType = typeof(Player), Column = "killer_player_id")]
        public virtual Player Killer { get; set; }

        [ManyToOne(ClassType = typeof(Player), Column = "killed_player_id")]
        public virtual Player PlayerKilled { get; set; }

        [Property(Column = "weapon")]
        public virtual string Weapon { get; set; }
    }
}