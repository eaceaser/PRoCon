using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Db.Tests.Domain
{
    using Db.Domain;
    using NUnit.Framework;

    [TestFixture]
    public class PlayerPlaytimeTest : PRoConDbEntityTestBase<PlayerPlaytime>
    {
        private Player player;
        private Server server;
        private DateTime startTime;
        private DateTime endTime;

        public override PlayerPlaytime CreateEntity()
        {
            player = new Player() {Name = "TestPlayer"};
            Session.Save(player);
            server = new Server() {Address = "myTestServer.com"};
            Session.Save(server);
            Session.Flush();

            startTime = DateTime.Now.AddHours(-5);
            endTime = DateTime.Now;

            return new PlayerPlaytime()
                       {
                           Player = player,
                           Server = server,
                           Start = startTime,
                           Quit = endTime
                       };
        }

        public override void AssertEntity(PlayerPlaytime entity)
        {
            Assert.AreEqual(player, entity.Player);
            Assert.AreEqual(server, entity.Server);
            Assert.AreEqual(startTime.ToString("dd.MM.yyyy hh:mm:ss"), entity.Start.ToString("dd.MM.yyyy hh:mm:ss"));
            Assert.AreEqual(endTime.ToString("dd.MM.yyyy hh:mm:ss"), entity.Quit.ToString("dd.MM.yyyy hh:mm:ss"));
        }

        public override void UpdateEntity(PlayerPlaytime entity)
        {
            
        }

        protected override void AssertUpdatedEntity(PlayerPlaytime entity)
        {
            Assert.AreEqual(player, entity.Player);
            Assert.AreEqual(server, entity.Server);
            Assert.AreEqual(startTime.ToString("dd.MM.yyyy hh:mm:ss"), entity.Start.ToString("dd.MM.yyyy hh:mm:ss"));
            Assert.AreEqual(endTime.ToString("dd.MM.yyyy hh:mm:ss"), entity.Quit.ToString("dd.MM.yyyy hh:mm:ss"));
        }
    }
}
