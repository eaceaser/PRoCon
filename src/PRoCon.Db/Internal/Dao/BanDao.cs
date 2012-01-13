namespace PRoCon.Db.Internal.Dao
{
    using Domain;
    using NHibernate;

    /// <summary>
    /// data access for ban data
    /// </summary>
    public class BanDao
    {
        private ISession session;

        public BanDao(ISession session)
        {
            this.session = session;
        }

        /// <summary>
        /// saves the ban data
        /// </summary>
        /// <param name="ban"></param>
        public void Save(Ban ban)
        {
            using (var trx = this.session.BeginTransaction())
            {
                this.session.Save(ban);
                trx.Commit();
            }
        }

        /// <summary>
        /// deletes the ban data
        /// </summary>
        /// <param name="ban"></param>
        public void Delete(Ban ban)
        {
            using (var trx = this.session.BeginTransaction())
            {
                this.session.Delete(ban);
                trx.Commit();
            }
        }
    }
}