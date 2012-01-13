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

    [Class(NameType = typeof(Kick), Table = "player_kick")]
    public class Kick
    {
        [Id(Name = "Id", TypeType = typeof(long), Column = "id")]
        [Generator(Class = "native")]
        public virtual long Id { get; set; }

        [ManyToOne(ClassType = typeof(Player), Column = "player_id")]
        public virtual Player Player { get; set; }

        [ManyToOne(ClassType = typeof(Server), Column = "server_id")]
        public virtual Server Server { get; set; }

        [Property(Column = "kicked")]
        public virtual DateTime KickedTimestamp { get; set; }

        [Property(Column = "reason")]
        public virtual string Reason { get; set; }
    }
}