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

namespace PRoCon.Db.Tests.Domain
{
    using System;
    using Db.Domain;
    using NUnit.Framework;

    [TestFixture]
    public class PlayerStatisticTest : PRoConDbEntityTestBase<PlayerStatistic>
    {
        private Player player;

        [Test]
        public void TestKillDeathRatio ()
        {
            var stat = new PlayerStatistic();

            stat.KillCount = 10;
            stat.DeathCount = 2;
            Assert.AreEqual(5D, stat.KillDeathRatio);

            stat.DeathCount = 0;
            Assert.AreEqual(10D, stat.KillDeathRatio);
        }

        public override PlayerStatistic CreateEntity ()
        {
            var session = this.Provider.Session;
            this.player = new Player
                         {
                             Name = "TestPlayer",
                         };
            session.Save(this.player);
            session.Flush();


            return new PlayerStatistic
                       {
                           Player = this.player,
                           DeathCount = 0,
                           KillCount = 10,
                           Score = 950
                       };
        }

        public override void AssertEntity (PlayerStatistic entity)
        {
            Assert.AreEqual(this.player, entity.Player);
            Assert.AreEqual(0, entity.DeathCount);
            Assert.AreEqual(10, entity.KillCount);
            Assert.AreEqual(950, entity.Score);
        }

        public override void UpdateEntity (PlayerStatistic entity)
        {
            entity.Score = 1000;
            entity.KillCount = 11;
        }

        protected override void AssertUpdatedEntity (PlayerStatistic entity)
        {
            Assert.AreEqual(this.player, entity.Player);
            Assert.AreEqual(0, entity.DeathCount);
            Assert.AreEqual(11, entity.KillCount);
            Assert.AreEqual(1000, entity.Score);
        }
    }
}