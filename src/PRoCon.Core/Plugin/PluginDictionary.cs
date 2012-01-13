/*  Copyright 2010 Geoffrey 'Phogue' Green

    http://www.phogue.net
 
    This file is part of PRoCon Frostbite.

    PRoCon Frostbite is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    PRoCon Frostbite is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with PRoCon Frostbite.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PRoCon.Core.Plugin {
    public class PluginDictionary : KeyedCollection<string, Plugin> {

        public List<string> EnabledClassNames {
            get {
                List<string> enabledClasses = new List<string>();

                foreach (Plugin plugin in this) {
                    if (plugin.IsEnabled == true) {
                        enabledClasses.Add(plugin.ClassName);
                    }
                }

                return enabledClasses;
            }
        }

        public List<string> LoadedClassNames {
            get {
                List<string> loadedClasses = new List<string>();

                foreach (Plugin plugin in this) {
                    if (plugin.IsLoaded == true) {
                        loadedClasses.Add(plugin.ClassName);
                    }
                }

                return loadedClasses;
            }
        }

        public bool IsEnabled(string className) {

            bool isEnabled = false;

            if (this.Contains(className) == true) {
                isEnabled = this[className].IsEnabled;
            }

            return isEnabled;
        }

        public bool IsLoaded(string className) {

            bool isLoaded = false;

            if (this.Contains(className) == true) {
                isLoaded = this[className].IsLoaded;
            }

            return isLoaded;
        }

        protected override string GetKeyForItem(Plugin item) {
            return item.ClassName;
        }

        public void AddLoadedPlugin(string className, IPRoConPluginInterface type) {
            if (this.Contains(className) == false) {
                this.Add(new Plugin(className, type));
            }
            else {
                this.SetItem(this.IndexOf(this[className]), new Plugin(className, type));
            }
        }

        public void SetCachedPluginVariable(string className, string variable, string value) {

            if (this.Contains(className) == false) {
                this.Add(new Plugin(className));
            }

            if (this[className].CacheFailCompiledPluginVariables.ContainsKey(variable) == true) {
                this[className].CacheFailCompiledPluginVariables[variable] = value;
            }
            else {
                this[className].CacheFailCompiledPluginVariables.Add(variable, value);
            }
        }
        
    }
}
