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

    [Class(NameType =typeof(Server), Table = "server")]
    public class Server
    {
        [Id(Name = "Id", Column = "id")]
        [Generator(1, Class = "native")]
        public virtual long Id { get; set; }

        /// <summary>
        /// the server address with ip-address and name
        /// </summary>
        [Property(Column = "address", UniqueKey = "uq_server_01")]
        public virtual string Address { get; set; }

        /// <summary>
        /// the current visible name
        /// </summary>
        [Property(Column = "name")]
        public virtual string Name { get; set; }

        [Property(Column = "version")]
        public virtual string Version { get; set; }

        public override bool Equals(object obj)
        {
            var entity = obj as Server;
            if (entity != null)
            {
                return new EqualsBuilder()
                    .Append(this.Name, entity.Name)
                    .Append(this.Address, entity.Address)
                    .IsEqual;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return new HashCodeBuilder()
                .Append(this.Address)
                .Append(this.Name)
                .ToHashCode();
        }
    }
}