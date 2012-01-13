using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Db.Tests.Domain
{
    using Db.Domain;
    using NUnit.Framework;

    [TestFixture]
    public class ServerTest : PRoConDbEntityTestBase<Server>
    {
        public override Server CreateEntity()
        {
            return new Server
                       {
                           Address = "myTestServer.com",
                           Name = "My little TestServer",
                           Version = "5.100.1.0"
                       };
        }

        public override void AssertEntity(Server entity)
        {
            Assert.AreEqual("myTestServer.com", entity.Address);
            Assert.AreEqual("My little TestServer", entity.Name);
            Assert.AreEqual("5.100.1.0", entity.Version);
        }

        public override void UpdateEntity(Server entity)
        {
            entity.Address = "myNewTestServer.com";
            entity.Name = "My New TestServer";
            entity.Version = "5.100.1.0";
        }

        protected override void AssertUpdatedEntity(Server entity)
        {
            Assert.AreEqual("myNewTestServer.com", entity.Address);
            Assert.AreEqual("My New TestServer", entity.Name);
            Assert.AreEqual("5.100.1.0", entity.Version);
        }
    }
}
