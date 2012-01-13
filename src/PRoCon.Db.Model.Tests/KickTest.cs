using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Db.Tests.Domain
{
    using Db.Domain;
    using NUnit.Framework;

    public class KickTest : PRoConDbEntityTestBase<Kick>
    {
        private DateTime kickedTime;
        private Server server;
        private Player player;

        public override Kick CreateEntity ()
        {
            var session = this.Provider.Session;
            this.player = new Player
                         {
                             Name = "TestPlayer"
                         };
            session.Save(this.player);
            session.Flush();

            this.server = new Server
                         {
                             Address = "myGameServer.de:898989"
                         };
            session.Save(this.server);
            session.Flush();

            this.kickedTime = DateTime.Now;
            return new Kick
                       {
                           KickedTimestamp = this.kickedTime,
                           Player = this.player,
                           Reason = "a reason description",
                           Server = this.server
                       };
        }

        public override void AssertEntity (Kick entity)
        {
            Assert.AreEqual(this.kickedTime.ToString("yyyyMMddhhmmss"), entity.KickedTimestamp.ToString("yyyyMMddhhmmss"));
            Assert.AreEqual(this.player, entity.Player);
            Assert.AreEqual(this.server, entity.Server);
            Assert.AreEqual("a reason description", entity.Reason);
        }

        public override void UpdateEntity (Kick entity)
        {
            entity.Reason = "another reason";
        }

        protected override void AssertUpdatedEntity (Kick entity)
        {
            Assert.AreEqual(this.kickedTime.ToString("yyyyMMddhhmmss"), entity.KickedTimestamp.ToString("yyyyMMddhhmmss"));
            Assert.AreEqual(this.player, entity.Player);
            Assert.AreEqual(this.server, entity.Server);
            Assert.AreEqual("another reason", entity.Reason);
        }
    }
}
