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
    using System.Collections.Generic;
    using Db.Domain;
    using NHibernate;
    using NUnit.Framework;

    [TestFixture]
    public class PlayerTest : PRoConDbEntityTestBase<Player>
    {
        private string guid;

        public static void AssertPlayer (long playerId, string playerName, string playerClan, string guid,
                                         Player playerToTest)
        {
            Assert.AreEqual(playerId, playerToTest.Id);
            Assert.AreEqual(playerName, playerToTest.Name);
            Assert.AreEqual(playerClan, playerToTest.ClanTag);
            Assert.AreEqual(guid, playerToTest.Guid);
        }

        [Test]
        public void TestCRUD ()
        {
            ISession session = this.Provider.Session;
            string guid = Guid.NewGuid().ToString();
            var player = new Player
                             {
                                 Name = "TestPlayer",
                                 ClanTag = "[Test]",
                                 Guid = guid
                             };
            session.Save(player);
            session.Flush();
            session.Evict(player);


            IList<Player> list = session.CreateCriteria(typeof (Player)).List<Player>();
            Assert.IsNotNull(list);
            Assert.AreEqual(1, list.Count);
            Player playerToTest = list[0];

            AssertPlayer(1, "TestPlayer", "[Test]", guid, playerToTest);

            player = playerToTest;

            player.ClanTag = "[New]";
            session.Save(player);
            session.Flush();
            session.Evict(player);

            list = session.CreateCriteria(typeof (Player)).List<Player>();
            Assert.IsNotNull(list);
            Assert.AreEqual(1, list.Count);

            AssertPlayer(1, "TestPlayer", "[New]", guid, list[0]);

            session.Delete(list[0]);
            session.Flush();

            list = session.CreateCriteria(typeof (Player)).List<Player>();
            Assert.IsNotNull(list);
            Assert.AreEqual(0, list.Count);
        }

        public override Player CreateEntity ()
        {
            this.guid = Guid.NewGuid().ToString();
            return new Player
                       {
                           Name = "TestPlayer",
                           ClanTag = "[Test]",
                           Guid = this.guid
                       };
        }

        public override void AssertEntity (Player entity)
        {
            Assert.AreEqual(1, entity.Id);
            Assert.AreEqual("TestPlayer", entity.Name);
            Assert.AreEqual("[Test]", entity.ClanTag);
            Assert.AreEqual(this.guid, entity.Guid);
        }

        public override void UpdateEntity (Player entity)
        {
            entity.ClanTag = "[blubb]";

        }

        protected override void AssertUpdatedEntity (Player entity)
        {
            Assert.AreEqual(1, entity.Id);
            Assert.AreEqual("TestPlayer", entity.Name);
            Assert.AreEqual("[blubb]", entity.ClanTag);
            Assert.AreEqual(this.guid, entity.Guid);
        }
    }
}