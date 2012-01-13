using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Db.Tests.Domain
{
    using Db.Domain;
    using NUnit.Framework;

    [TestFixture]
    public class KillTest : PRoConDbEntityTestBase<Kill>
    {
        private Player killer;
        private Player killedPlayer;
        private Server server;
        private DateTime timstamp;

        public override Kill CreateEntity()
        {
            killer = new Player() {Name = "TestKiller"};
            Session.Save(killer);
            
            killedPlayer = new Player(){Name = "TestVictim"};
            Session.Save(killedPlayer);

            server = new Server() { Address = "myTestServer.com" };
            Session.Save(server);
            Session.Flush();

            timstamp = DateTime.Now;
            return new Kill()
                       {
                           Killer = killer,
                           PlayerKilled = killedPlayer,
                           Server = server,
                           Timestamp = timstamp,
                           Weapon = "M2"
                       };
        }

        public override void AssertEntity(Kill entity)
        {
            Assert.AreEqual(killer, entity.Killer);
            Assert.AreEqual(killedPlayer, entity.PlayerKilled);
            Assert.AreEqual(server, entity.Server);
            Assert.AreEqual(timstamp.ToString("dd.MM.yyyy hh:mm:ss"), entity.Timestamp.ToString("dd.MM.yyyy hh:mm:ss"));
            Assert.AreEqual("M2", entity.Weapon);
        }

        public override void UpdateEntity(Kill entity)
        {
            entity.Weapon = "None";
        }

        protected override void AssertUpdatedEntity(Kill entity)
        {
            Assert.AreEqual(killer, entity.Killer);
            Assert.AreEqual(killedPlayer, entity.PlayerKilled);
            Assert.AreEqual(server, entity.Server);
            Assert.AreEqual(timstamp.ToString("dd.MM.yyyy hh:mm:ss"), entity.Timestamp.ToString("dd.MM.yyyy hh:mm:ss"));
            Assert.AreEqual("None", entity.Weapon);
        }
    }
}
