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

namespace PRoCon.Db.Tests
{
    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Tool.hbm2ddl;
    using NUnit.Framework;
    using Util;

    [TestFixture]
    public class PRoConDbTestBase
    {
        protected ISession Session;

        #region Setup/Teardown

        [SetUp]
        public void TestSetup ()
        {
            this.Provider = new PRoConDatabaseProvider(new SQLiteDatabaseSettings {FileName = "test.db"});
            this.Provider.EnableDebug();
            Configuration cfg = this.Provider.Configuration;
            new SchemaExport(cfg).Execute(true, true, false);
            this.Session = this.Provider.Session;
        }

        #endregion

        protected PRoConDatabaseProvider Provider;
    }
}