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
    using NHibernate.Mapping.Attributes;

    [Class(NameType = typeof(PlayerStatistic), Table = "player_statistic")]
    public class PlayerStatistic
    {
        [Id(Name = "Id", Column = "id")]
        [Generator(1, Class = "native")]
        public virtual long Id { get; set; }

        [ManyToOne(ClassType = typeof(Player), Fetch = FetchMode.Join, Column = "player_id")]
        public virtual Player Player { get; set; }

        [ManyToOne(ClassType = typeof(Server), Column = "server_id")]
        public virtual Server Server { get; set; }

        [Property(Column = "score")]
        public virtual int Score { get; set; }

        [Property(Column = "kills")]
        public virtual int KillCount { get; set; }
        
        [Property(Column = "deaths")]
        public virtual int DeathCount { get; set; }

        public virtual double KillDeathRatio
        {
            get
            {
                if (this.DeathCount != 0)
                {
                    return (double) this.KillCount / this.DeathCount;
                }
                return this.KillCount;
            }
        }
    }
}