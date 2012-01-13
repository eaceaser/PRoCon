using System;
using System.Collections.Generic;
using System.Text;

namespace PRoCon.Db.Internal
{
    using Dao;
    using Domain;
    using NHibernate;
    using NHibernate.Criterion;

    public class PRoConDatabaseAccess : IPRoConDatabaseAccess
    {
        private ISession session;

        internal PRoConDatabaseAccess(ISession session)
        {
            this.session = session;
        }

        public PlayerDao GetPlayerAccess()
        {
            return new PlayerDao(this.session);
        }
    }
}
