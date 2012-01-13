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

namespace PRoCon.Db
{
    using System.Data;
    using Domain;
    using Internal;
    using NHibernate;
    using NHibernate.ByteCode.Castle;
    using NHibernate.Cfg;
    using NHibernate.Connection;
    using NHibernate.Mapping.Attributes;
    using Util;

    /// <summary>
    /// this class is the entry class for the basic procon database connection
    /// </summary>
    public class PRoConDatabaseProvider
    {
        private ISessionFactory activeSessionFactory;
        private Configuration databaseConfiguration;

        #region Properties

        /// <summary>
        /// gets the ADO.NET connection
        /// </summary>
        public IDbConnection Connection
        {
            get { return this.activeSessionFactory.OpenSession().Connection; }
        }

        #endregion

        public PRoConDatabaseProvider (DatabaseSettings settings)
        {
            this.databaseConfiguration = new Configuration();
            this.databaseConfiguration.Properties.Add("connection.provider_class",
                                                      typeof (DriverConnectionProvider).FullName);
            this.databaseConfiguration.Properties.Add("connection.driver_class", settings.DriverClass);
            this.databaseConfiguration.Properties.Add("connection.connection_string", settings.ConnectionString);
            this.databaseConfiguration.Properties.Add("dialect", settings.Dialect);
            this.databaseConfiguration.Properties.Add("proxyfactory.factory_class",
                                                      typeof (ProxyFactoryFactory).AssemblyQualifiedName);

            this.databaseConfiguration.AddInputStream(
                HbmSerializer.Default.Serialize(typeof (Player).Assembly));

            this.activeSessionFactory = this.databaseConfiguration.BuildSessionFactory();
        }

        public Configuration Configuration
        {
            get { return this.databaseConfiguration; }
        }

        public ISession Session
        {
            get { return this.activeSessionFactory.OpenSession(); }
        }

        public void EnableDebug ()
        {
            this.databaseConfiguration.Properties.Add("show_sql", "true");
            this.activeSessionFactory = this.databaseConfiguration.BuildSessionFactory();
        }

        public IPRoConDatabaseAccess BuildDataAccess()
        {
            return new PRoConDatabaseAccess(this.activeSessionFactory.OpenSession());
        }
    }
}