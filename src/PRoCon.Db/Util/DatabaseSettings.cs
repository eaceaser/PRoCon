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
    /// <summary>
    /// this class describes the database settings neccessary for a database connection
    /// </summary>
    public abstract class DatabaseSettings
    {
        public DatabaseType DatabaseType { get; set; }
        public string FileName { get; set; }
        public string Password { get; set; }
        public abstract string DriverClass { get; }
        public abstract string Dialect { get; }
        public abstract string ConnectionString { get; }
    }
}