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
    using Net.Commons.Lang.Builder;
    using NHibernate.Mapping.Attributes;

    /// <summary>
    /// the domain object represents a player
    /// </summary>
    [Class(NameType = typeof(Player), Table = "player")]
    public class Player
    {
        [Id(Name="Id", Column = "id", TypeType = typeof(long), UnsavedValueObject = 0)]
        [Generator(1, Class = "native")]
        public virtual long Id { get; set; }

        [Property(Column = "name")]
        public virtual string Name { get; set; }

        [Property(Column = "guid")]
        public virtual string Guid { get; set; }

        [Property(Column = "clan_tag")]
        public virtual string ClanTag { get; set; }

        [ManyToOne(ClassType = typeof(Country), Column = "country_id")]
        public virtual Country Country { get; set; }

        public override bool Equals(object obj)
        {
            var entity = obj as Player;
            if (entity != null)
            {
                return new EqualsBuilder()
                    .Append(this.Name, entity.Name)
                    .Append(this.ClanTag, entity.ClanTag)
                    .Append(this.Country, entity.Country)
                    .Append(this.Guid, entity.Guid)
                    .IsEqual;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                .Append(this.Name)
                .Append(this.ClanTag)
                .Append(this.Country)
                .Append(this.Guid)
                .ToHashCode();
        }
    }
}