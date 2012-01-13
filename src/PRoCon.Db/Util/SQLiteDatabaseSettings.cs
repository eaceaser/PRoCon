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

namespace PRoCon.Db.Util
{
    using System.Data.SQLite;
    using NHibernate.Dialect;
    using NHibernate.Driver;

    public class SQLiteDatabaseSettings : DatabaseSettings
    {
        private string fileName;

        public new string FileName
        {
            get
            {
                if (string.IsNullOrEmpty(this.fileName))
                {
                    return ":memory:";
                }
                return this.fileName;
            }
            set { this.fileName = value; }
        }

        public override string DriverClass
        {
            get { return typeof(SQLite20Driver).FullName; }
        }

        public override string Dialect
        {
            get { return typeof(SQLiteDialect).FullName; }
        }

        public override string ConnectionString
        {
            get
            {
                var connectionStringBuilder = new SQLiteConnectionStringBuilder();
                connectionStringBuilder.DataSource = this.FileName;
                connectionStringBuilder.Password = this.Password;
                return connectionStringBuilder.ToString();
            }
        }
    }
}