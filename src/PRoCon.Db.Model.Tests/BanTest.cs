namespace PRoCon.Db.Tests.Domain
{
    using Db.Domain;
    using NHibernate;
    using NUnit.Framework;

    [TestFixture]
    public class BanTest : PRoConDbEntityTestBase<Ban>
    {
        private Player testPlayer;

        public override Ban CreateEntity()
        {
            ISession session = Session;
            var country = new Country
                              {
                                  Name = "Germany"
                              };
            session.Save(country);

            testPlayer = new Player
                             {
                                 Name = "MyTestPlayer",
                                 ClanTag = "Test",
                             };
            session.Save(testPlayer);
            session.Flush();

            return new Ban
                       {
                           Player = testPlayer,
                           Duration = "4Ever",
                           Reason = "just4fun"
                       };
        }

        public override void AssertEntity(Ban entity)
        {
            Assert.AreEqual(testPlayer, entity.Player);
            Assert.AreEqual("4Ever", entity.Duration);
            Assert.AreEqual("just4fun", entity.Reason);
        }

        public override void UpdateEntity(Ban entity)
        {
            entity.Duration = "untilNow";
            entity.Reason = "not available";
        }

        protected override void AssertUpdatedEntity(Ban entity)
        {
            Assert.AreEqual(testPlayer, entity.Player);
            Assert.AreEqual("untilNow", entity.Duration);
            Assert.AreEqual("not available", entity.Reason);
        }
    }
}