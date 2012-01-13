using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Db.Internal
{
    using Domain;



    public class RankInfo : IRankInfo
    {
        public Player Player { get; private set; }
        public long Rank { get; private set; }

        internal RankInfo(Player player, long rank)
        {
            this.Player = player;
            this.Rank = rank;
        }
    }
}
