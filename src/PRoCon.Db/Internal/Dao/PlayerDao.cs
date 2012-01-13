namespace PRoCon.Db.Internal.Dao
{
    using Domain;
    using NHibernate;
    using NHibernate.Criterion;

    /// <summary>
    /// DataAccess for player information
    /// </summary>
    public class PlayerDao
    {
        private readonly ISession session;

        /// <summary>
        /// creates the instance
        /// </summary>
        /// <param name="session"></param>
        public PlayerDao(ISession session)
        {
            this.session = session;
        }

        /// <summary>
        /// saves the player information to the database
        /// </summary>
        /// <param name="player"></param>
        public void Save(Player player)
        {
            using (var trx = this.session.BeginTransaction())
            {
                session.Save(player);
                trx.Commit();
            }
        }

        /// <summary>
        /// deletes the player information from the database
        /// </summary>
        /// <param name="player"></param>
        public void Delete(Player player)
        {
            using(var trx = session.BeginTransaction() )
            {
                session.Delete(player);
                trx.Commit();
            }
        }

        /// <summary>
        /// searches the database for a player with the name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Player Get(string name)
        {
            using (var trx = session.BeginTransaction())
            {
                var player = session.CreateCriteria(typeof (Player))
                    .Add(Restrictions.Eq("Name", name))
                    .SetMaxResults(1)
                    .UniqueResult<Player>();
                trx.Commit();
                return player;
            }
        }
    }
}