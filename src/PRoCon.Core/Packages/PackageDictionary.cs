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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PRoCon.Core.Packages {

    public class PackageDictionary : KeyedCollection<string, Package> {

        public PackageDictionary() {

        }

        protected override string GetKeyForItem(Package item) {
            return item.Uid;
        }

        public void AddPackage(Package package) {
            if (this.Contains(package.Uid) == false) {
                this.Add(package);
            }
            else {
                // Update the item, it might be a new release.
                this.SetItem(this.IndexOf(this[package.Uid]), package);
            }
        }

        public bool IsNewer(Package package) {

            bool isNewer = false;

            if (this.Contains(package.Uid) == true) {
                // if 1.0.0.0 < 2.0.0.0
                isNewer = (this[package.Uid].Version < package.Version);
            }
            else {
                // Not found, yes it is newer.
                isNewer = true;
            }

            return isNewer;
        }

    }
}
