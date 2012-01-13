namespace PRoCon.Db.Tests.Internal.Dao
{
    using System.Collections.Generic;
    using Db.Domain;
    using Db.Internal.Dao;
    using NUnit.Framework;

    [TestFixture]
    public class PlayerDaoTest : PRoConDbTestBase
    {
        #region Setup/Teardown

        [SetUp]
        public void LocalTestSetup()
        {
            this.TestSetup();
            dao = new PlayerDao(Session);
        }

        #endregion

        private PlayerDao dao;

        private Player GetSamplePlayer()
        {
            return new Player {Name = "MyTestPlayer", ClanTag = "A-Tag"};
        }

        [Test]
        public void Test_Get_WithName()
        {

            Player player = GetSamplePlayer();
            dao.Save(player);
            Session.Evict(player);


            Player checkPlayer = dao.Get("MyTestPlayer");
            Assert.IsNotNull(checkPlayer);
            Assert.AreEqual(player, checkPlayer);
        }

        [Test]
        public void Test_Save()
        {
            Player player = GetSamplePlayer();
            dao.Save(player);

            IList<Player> playerList = Session.CreateCriteria(typeof (Player)).List<Player>();
            Assert.AreEqual(1, playerList.Count);
            Assert.AreEqual(player, playerList[0]);
        }
    }
}