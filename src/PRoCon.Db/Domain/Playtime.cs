// Copyright 2010 Geoffrey 'Phogue' Green
// 
// http://www.phogue.net
//  
// This file is part of BFBC2 PRoCon.
//  
// BFBC2 PRoCon is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// BFBC2 PRoCon is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//  
// You should have received a copy of the GNU General Public License
// along with BFBC2 PRoCon.  If not, see <http://www.gnu.org/licenses/>.

namespace PRoCon.Db.Domain
{
    using System;

    public class Playtime
    {
        public virtual long Id { get; set; }
        public virtual Player Player { get; set; }
        public virtual DateTime Start { get; set; }
        public virtual DateTime Quit { get; set; }
    }
}