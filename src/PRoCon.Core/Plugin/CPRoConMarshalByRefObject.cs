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
using System.Text;

namespace PRoCon.Core.Plugin {
    using System.ComponentModel;
    using System.Reflection;
    using Core.Plugin.Commands;
    using Core.Players.Items;
    using Core.Battlemap;
    using Core.Remote;

    public class CPRoConMarshalByRefObject : MarshalByRefObject {
        public delegate void ExecuteCommandHandler(List<string> lstCommand);
        public delegate CPrivileges GetAccountPrivilegesHandler(string strAccountName);
        public delegate string GetVariableHandler(string strVariable);
        public delegate bool TryGetLocalizedHandler(string strLanguageCode, out string strLocalizedText, string strVariable, string[] a_strArguements);
        public delegate void RegisterCommandHandler(MatchCommand mtcCommand);
        public delegate void UnregisterCommandHandler(MatchCommand mtcCommand);
        public delegate List<MatchCommand> GetRegisteredCommandsHandler();
        public delegate List<CMap> GetMapDefinesHandler();
        public delegate WeaponDictionary GetWeaponDefinesHandler();
        public delegate SpecializationDictionary GetSpecializationDefinesHandler();
        public delegate List<string> GetLoggedInAccountUsernamesHandler();
        public delegate void RegisterEventsHandler(string className, List<string> lstCommand);

        private ExecuteCommandHandler m_delExecuteCommand;
        private GetAccountPrivilegesHandler m_delGetAccountPrivileges;
        private GetVariableHandler m_delGetVariable;
        private GetVariableHandler m_delGetSvVariable;
        private GetMapDefinesHandler m_delGetMapDefines;
        private TryGetLocalizedHandler m_delTryGetLocalized;
        private RegisterCommandHandler m_delRegisterCommand;
        private UnregisterCommandHandler m_delUnregisterCommand;
        private GetRegisteredCommandsHandler m_delGetRegisteredCommands;

        private GetWeaponDefinesHandler m_delGetWeaponDefines;
        private GetSpecializationDefinesHandler m_delGetSpecializationDefines;

        private GetLoggedInAccountUsernamesHandler m_delGetLoggedInAccountUsernames;
        private RegisterEventsHandler m_delRegisterEvents;

        public void RegisterCallbacks(ExecuteCommandHandler delExecuteCommand, GetAccountPrivilegesHandler delGetAccountPrivileges, GetVariableHandler delGetVariable, GetVariableHandler delGetSvVariable, GetMapDefinesHandler delGetMapDefines, TryGetLocalizedHandler delTryGetLocalized, RegisterCommandHandler delRegisterCommand, UnregisterCommandHandler delUnregisterCommand, GetRegisteredCommandsHandler delGetRegisteredCommands, GetWeaponDefinesHandler delGetWeaponDefines, GetSpecializationDefinesHandler delGetSpecializationDefines, GetLoggedInAccountUsernamesHandler delGetLoggedInAccountUsernames, RegisterEventsHandler delRegisterEvents) {
            this.m_delExecuteCommand = delExecuteCommand;
            this.m_delGetAccountPrivileges = delGetAccountPrivileges;
            this.m_delGetVariable = delGetVariable;
            this.m_delGetSvVariable = delGetSvVariable;
            this.m_delGetMapDefines = delGetMapDefines;
            this.m_delTryGetLocalized = delTryGetLocalized;

            this.m_delRegisterCommand = delRegisterCommand;
            this.m_delUnregisterCommand = delUnregisterCommand;
            this.m_delGetRegisteredCommands = delGetRegisteredCommands;

            this.m_delGetWeaponDefines = delGetWeaponDefines;
            this.m_delGetSpecializationDefines = delGetSpecializationDefines;

            this.m_delGetLoggedInAccountUsernames = delGetLoggedInAccountUsernames;
            this.m_delRegisterEvents = delRegisterEvents;

            this.SetupInternalDictionaries();
        }

        public override object InitializeLifetimeService() {
            return null;
        }

        public object Invoke(string strMethodName, object[] a_objParameters) {
            object objReturn = null;

            try {
                if (this.GetType().GetMember(strMethodName).Length > 0) {
                    objReturn = this.GetType().InvokeMember(strMethodName, BindingFlags.InvokeMethod, null, this, a_objParameters);
                }
            }
            catch (Exception) { }

            return objReturn;
        }

        public string GetLocalizedByLanguage(string strLanguageCode, string strDefaultText, string strVariable, params string[] a_strArguements) {
            string strReturn = strDefaultText;

            if (this.m_delTryGetLocalized != null) {
                if (this.m_delTryGetLocalized(strLanguageCode, out strReturn, strVariable, a_strArguements) == false) {
                    strReturn = strDefaultText;
                }
            }

            return strReturn;
        }

        public string GetLocalized(string strDefaultText, string strVariable, params string[] a_strArguements) {
            return this.GetLocalizedByLanguage(null, strDefaultText, strVariable, a_strArguements);
        }

        public void ExecuteCommand(params string[] a_strCommandWords) {
            if (this.m_delExecuteCommand != null) {
                this.m_delExecuteCommand(new List<string>(a_strCommandWords));
            }
        }

        /// <summary>
        /// There is no need to register the methods found in IPRoConPluginInterface.
        /// 
        /// They will always be called no matter if you register them or not.
        /// You will need to register all methods you need within PRoConPluginAPI though
        /// if you choose to RegisterEvents.
        /// 
        /// By default if you load up a plugin all of the events will be fired, but this is 
        /// the most cpu/time consuming task that procon has is calling empty methods within plugins.
        /// This optimizes your plugin by telling procon to only call your plugin with the registered events.
        /// 
        /// You can see the potential gains by enabling all of your plugins and restarting procon.
        /// See how slowly the console ticks passed compared to having no plugins enabled and logging in.
        /// </summary>
        /// <param name="className">The name of your plugins class</param>
        /// <param name="a_events">A list of methods to be called within your plugin by procon
        /// Example: ("OnListPlayers", "OnPlayerLeft") - Only these two methods will be called within
        /// your plugin.</param>
        public void RegisterEvents(string className, params string[] a_events) {
            if (this.m_delRegisterEvents != null) {
                this.m_delRegisterEvents(className, new List<string>(a_events));
            }
        }

        public void RegisterCommand(MatchCommand mtcCommand) {
            if (this.m_delRegisterCommand != null) {
                this.m_delRegisterCommand(mtcCommand);
            }
        }

        public void UnregisterCommand(MatchCommand mtcCommand) {
            if (this.m_delUnregisterCommand != null) {
                this.m_delUnregisterCommand(mtcCommand);
            }
        }

        public List<MatchCommand> GetRegisteredCommands() {
            List<MatchCommand> lstReturn = default(List<MatchCommand>);

            if (this.m_delGetRegisteredCommands != null) {
                lstReturn = this.m_delGetRegisteredCommands();
            }

            return lstReturn;
        }

        public List<CMap> GetMapDefines() {
            List<CMap> lstReturn = default(List<CMap>);

            if (this.m_delGetAccountPrivileges != null) {
                lstReturn = this.m_delGetMapDefines();
            }

            return lstReturn;
        }

        public CPrivileges GetAccountPrivileges(string strAccountName) {
            CPrivileges spReturn = default(CPrivileges);

            if (this.m_delGetAccountPrivileges != null) {
                spReturn = this.m_delGetAccountPrivileges(strAccountName);
            }

            return spReturn;
        }

        public T GetVariable<T>(string strVariable, T tDefault) {
            T tReturn = tDefault;

            if (this.m_delGetVariable != null) {
                string strValue = this.m_delGetVariable(strVariable);

                TypeConverter tycPossible = TypeDescriptor.GetConverter(typeof(T));
                if (strValue.Length > 0 && tycPossible.CanConvertFrom(typeof(string)) == true) {
                    tReturn = (T)tycPossible.ConvertFrom(strValue);
                }
                else {
                    tReturn = tDefault;
                }
            }

            return tReturn;
        }

        public T GetSvVariable<T>(string strVariable, T tDefault) {
            T tReturn = tDefault;

            if (this.m_delGetSvVariable != null) {
                string strValue = this.m_delGetSvVariable(strVariable);

                TypeConverter tycPossible = TypeDescriptor.GetConverter(typeof(T));
                if (strValue.Length > 0 && tycPossible.CanConvertFrom(typeof(string)) == true) {
                    tReturn = (T)tycPossible.ConvertFrom(strValue);
                }
                else {
                    tReturn = tDefault;
                }
            }

            return tReturn;
        }

        public WeaponDictionary GetWeaponDefines() {
            WeaponDictionary dicReturn = default(WeaponDictionary);

            if (this.m_delGetWeaponDefines != null) {
                dicReturn = this.m_delGetWeaponDefines();
            }

            return dicReturn;
        }

        public SpecializationDictionary GetSpecializationDefines() {
            SpecializationDictionary dicReturn = default(SpecializationDictionary);

            if (this.m_delGetSpecializationDefines != null) {
                dicReturn = this.m_delGetSpecializationDefines();
            }

            return dicReturn;
        }

        public List<string> GetLoggedInAccountUsernames() {
            List<string> loggedInList = null;

            if (this.m_delGetLoggedInAccountUsernames != null) {
                loggedInList = this.m_delGetLoggedInAccountUsernames();
            }

            return loggedInList;
        }

        /// <summary>
        /// Provides a way of creating and populating lists inline with the C# 2.0 compiler.
        /// The below examples would be identical in a 3.5 C# compiler but the first would throw
        /// compile errors in 2.0
        /// 
        /// List<string> lstHelloWorld = new List<string>() { "Hello", "World!" };
        /// List<string> lstHelloWorld = this.Listify<string>("Hello", "World!");
        /// </summary>
        /// <typeparam name="T">The type of object to create a List of</typeparam>
        /// <param name="newList"></param>
        /// <returns>A list populated with the array of params</returns>
        public List<T> Listify<T>(params T[] newList) {
            return new List<T>(newList);
        }

        // Converts "hello world! \"this is a string\" 5 6 7
        // to a list with "hello", "world!", "this is a string", "5", "6", "7"
        //
        // Converts "procon.private.servers.add \"1.2.3.4\" 48889 \"password\""
        // to a list with "procon.private.servers.add", "1.2.3.4", "48889", "password"
        public List<string> Wordify(string strCommand) {
            return Packet.Wordify(strCommand);
        }

        // Takes a string and splits it on words based on characters 
        // string testString = "this is a string with some words in it";
        // WordWrap(testString, 10) == List<string>() { "this is a", "string", "with some", "words in", "it" }
        //
        // Useful if you want to output a long string to the game and want all of the data outputed without
        // losing any data.
        // 
        // See the @help function in the basic in game information plugin for an example.
        public List<string> WordWrap(string strText, int iColumn) {

            List<string> lstReturn = new List<string>(strText.Split(' '));

            for (int i = 0; i < lstReturn.Count - 1; i++) {
                if (lstReturn[i].Length + lstReturn[i + 1].Length + 1 <= iColumn) {
                    lstReturn[i] = String.Format("{0} {1}", lstReturn[i], lstReturn[i + 1]);
                    lstReturn.RemoveAt(i + 1);
                    i--;
                }
            }

            return lstReturn;
        }

        public void UnregisterZoneTags(params string[] tags) {

            string tagList = GetVariable<string>("ZONE_TAG_LIST", String.Empty);
            ZoneTagList tagsList = new ZoneTagList(tagList);

            foreach (string tag in tags) {
                if (tagsList.Contains(tag) == true) {
                    tagsList.Remove(tag);
                }
            }

            this.ExecuteCommand("procon.protected.vars.set", "ZONE_TAG_LIST", tagsList.ToString());
        }

        public void RegisterZoneTags(params string[] tags) {

            string tagList = GetVariable<string>("ZONE_TAG_LIST", String.Empty);

            ZoneTagList tagsList = new ZoneTagList(tagList);
            foreach (string tag in tags) {
                if (tagsList.Contains(tag) == false) {
                    tagsList.Add(tag);
                }
            }

            this.ExecuteCommand("procon.protected.vars.set", "ZONE_TAG_LIST", tagsList.ToString());
        }

        protected Dictionary<string, Weapon> WeaponDictionaryByLocalizedName {
            get;
            private set;
        }

        protected Dictionary<string, Specialization> SpecializationDictionaryByLocalizedName {
            get;
            private set;
        }

        internal void SetupInternalDictionaries() {

            string localizedName = String.Empty;

            WeaponDictionary weapons = this.GetWeaponDefines();
            this.WeaponDictionaryByLocalizedName = new Dictionary<string, Weapon>();

            foreach (Weapon weapon in weapons) {
                localizedName = this.GetLocalized(weapon.Name, String.Format("global.Weapons.{0}", weapon.Name.ToLower()));

                if (this.WeaponDictionaryByLocalizedName.ContainsKey(localizedName) == false) {
                    this.WeaponDictionaryByLocalizedName.Add(localizedName, weapon);
                }
            }

            SpecializationDictionary specializations = this.GetSpecializationDefines();
            this.SpecializationDictionaryByLocalizedName = new Dictionary<string, Specialization>();

            foreach (Specialization specialization in specializations) {
                localizedName = this.GetLocalized(specialization.Name, String.Format("global.Specialization.{0}", specialization.Name.ToLower()));

                if (this.SpecializationDictionaryByLocalizedName.ContainsKey(localizedName) == false) {
                    this.SpecializationDictionaryByLocalizedName.Add(localizedName, specialization);
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="damageType">
        /// DamageTypes.None = Full list of weapons
        /// DamageTypes.SniperRifle = List of sniper rifles
        /// </param>
        /// <returns></returns>
        protected List<string> GetWeaponList(DamageTypes damageType) {

            List<string> returnWeaponList = new List<string>();

            foreach (KeyValuePair<string, Weapon> weapon in this.WeaponDictionaryByLocalizedName) {

                if (damageType == DamageTypes.None || (damageType & weapon.Value.Damage) == weapon.Value.Damage) {
                    if (returnWeaponList.Contains(weapon.Key) == false) {
                        returnWeaponList.Add(weapon.Key);
                    }
                }
            }

            return returnWeaponList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variableName">The name to display in the plugin settings</param>
        /// <param name="assemblyName">A unique name for this enum list, should be unique.  Put your plugin class name in to avoid clashes.</param>
        /// <param name="value">The current value/selected value</param>
        /// <param name="slotType">DamageTypes.None, gets full list otherwise limits the returned list</param>
        /// <returns></returns>
        protected CPluginVariable GetWeaponListPluginVariable(string variableName, string assemblyName, string value, DamageTypes damageType) {
            return new CPluginVariable(variableName, String.Format("enum.{0}({1})", assemblyName, String.Join("|", this.GetWeaponList(damageType).ToArray())), value);
        }

        protected Weapon GetWeaponByLocalizedName(string localizedName) {
            Weapon returnWeapon = null;

            if (this.WeaponDictionaryByLocalizedName.ContainsKey(localizedName) == true) {
                returnWeapon = this.WeaponDictionaryByLocalizedName[localizedName];
            }

            return returnWeapon;
        }

        /// <summary>
        /// this.GetSpecializationList(SpecializationSlots.None) gets a full list of specs
        /// this.GetSpecializationList(SpecializationSlots.Kit) will get a list of specs based on Kits
        /// </summary>
        /// <param name="slotType"></param>
        /// <returns></returns>
        protected List<string> GetSpecializationList(SpecializationSlots slotType) {

            List<string> returnSpecializationList = new List<string>();

            foreach (KeyValuePair<string, Specialization> specialization in this.SpecializationDictionaryByLocalizedName) {

                if (slotType == SpecializationSlots.None || slotType == specialization.Value.Slot) {
                    if (returnSpecializationList.Contains(specialization.Key) == false) {
                        returnSpecializationList.Add(specialization.Key);
                    }
                }
            }

            return returnSpecializationList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variableName">The name to display in the plugin settings</param>
        /// <param name="assemblyName">A unique name for this enum list, should be unique.  Put your plugin class name in to avoid clashes.</param>
        /// <param name="value">The current value/selected value</param>
        /// <param name="slotType">SpecializationSlots.None, gets full list otherwise limits the returned list</param>
        /// <returns></returns>
        protected CPluginVariable GetSpecializationListPluginVariable(string variableName, string assemblyName, string value, SpecializationSlots slotType) {
            return new CPluginVariable(variableName, String.Format("enum.{0}({1})", assemblyName, String.Join("|", this.GetSpecializationList(slotType).ToArray())), value);
        }

        protected Specialization GetSpecializationByLocalizedName(string localizedName) {
            Specialization returnSpecialization = null;

            if (this.SpecializationDictionaryByLocalizedName.ContainsKey(localizedName) == true) {
                returnSpecialization = this.SpecializationDictionaryByLocalizedName[localizedName];
            }

            return returnSpecialization;
        }

        internal bool IsValidPlaylist(string validatePlayList, string[] requestedPlayLists) {
            bool isValid = false;

            if (requestedPlayLists == null) {
                isValid = true;
            }
            else if (requestedPlayLists != null) {

                if (requestedPlayLists.Length == 0) {
                    isValid = true;
                }
                else {
                    foreach (string playlist in requestedPlayLists) {
                        if (String.Compare(validatePlayList, playlist) == 0) {
                            isValid = true;
                            break;
                        }
                    }
                }
            }

            return isValid;
        }

        /// <summary>
        /// Gets a list of formatted maps from the Map Defines.
        /// </summary>
        /// <param name="format">
        /// {PublicLevelName}
        /// {GameMode}
        /// {FileName}
        /// </param>
        /// <param name="playList"></param>
        /// <returns></returns>
        protected List<string> GetMapList(string format, params string[] playList) {
            List<string> returnMapList = new List<string>();

            foreach (CMap map in this.GetMapDefines()) {

                if (this.IsValidPlaylist(map.PlayList, playList) == true) {

                    string formattedMap = format.Replace("{PublicLevelName}", map.PublicLevelName).Replace("{GameMode}", map.GameMode).Replace("{FileName}", map.FileName);

                    if (returnMapList.Contains(formattedMap) == false) {
                        returnMapList.Add(formattedMap);
                    }
                }
            }

            return returnMapList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variableName">The name to display in the plugin settings</param>
        /// <param name="assemblyName">A unique name for this enum list, should be unique.  Put your plugin class name in to avoid clashes.</param>
        /// <param name="value">The current value/selected value</param>
        /// <param name="format">The format of the team list.  See this.GetMapList for more information</param>
        /// <param name="playList">SQDM, SQRUSH, CONQUEST, RUSH</param>
        /// <returns></returns>
        protected CPluginVariable GetMapListPluginVariable(string variableName, string assemblyName, string value, string format, params string[] playList) {
            return new CPluginVariable(variableName, String.Format("enum.{0}({1})", assemblyName, String.Join("|", this.GetMapList(format, playList).ToArray())), value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format">See this.GetMapList(string, params string[]) for more information about the format variable</param>
        /// <param name="formattedMapName"></param>
        /// <returns></returns>
        protected CMap GetMapByFormattedName(string format, string formattedMapName) {

            CMap returnMap = null;

            foreach (CMap map in this.GetMapDefines()) {

                string formattedMap = format.Replace("{PublicLevelName}", map.PublicLevelName).Replace("{GameMode}", map.GameMode).Replace("{FileName}", map.FileName);

                if (String.Compare(formattedMap, formattedMapName) == 0) {
                    returnMap = map;
                    break;
                }
            }

            return returnMap;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format">
        /// {PublicLevelName}
        /// {GameMode}
        /// {FileName}
        /// {TeamName}
        /// </param>
        /// <param name="playList"></param>
        /// <returns></returns>
        protected List<string> GetTeamList(string format, params string[] playList) {
            List<string> returnMapList = new List<string>();

            foreach (CMap map in this.GetMapDefines()) {

                if (this.IsValidPlaylist(map.PlayList, playList) == true) {

                    foreach (CTeamName teamname in map.TeamNames) {
                        string formattedTeamName = format.Replace("{PublicLevelName}", map.PublicLevelName).Replace("{GameMode}", map.GameMode).Replace("{FileName}", map.FileName).Replace("{TeamName}", this.GetLocalized(teamname.LocalizationKey, teamname.LocalizationKey));

                        if (returnMapList.Contains(formattedTeamName) == false) {
                            returnMapList.Add(formattedTeamName);
                        }
                    }
                }
            }

            return returnMapList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variableName">The name to display in the plugin settings</param>
        /// <param name="assemblyName">A unique name for this enum list, should be unique.  Put your plugin class name in to avoid clashes.</param>
        /// <param name="value">The current value/selected value</param>
        /// <param name="format">The format of the team list.  See this.GetTeamList for more information </param>
        /// <param name="playList">SQDM, SQRUSH, CONQUEST, RUSH</param>
        /// <returns></returns>
        protected CPluginVariable GetTeamListPluginVariable(string variableName, string assemblyName, string value, string format, params string[] playList) {
            return new CPluginVariable(variableName, String.Format("enum.{0}({1})", assemblyName, String.Join("|", this.GetTeamList(format, playList).ToArray())), value);
        }

        protected CTeamName GetTeamNameByFormattedTeamName(string format, string formattedTeamName) {
            CTeamName returnTeamName = null;

            foreach (CMap map in this.GetMapDefines()) {

                foreach (CTeamName teamname in map.TeamNames) {
                    string formattedTeam = format.Replace("{PublicLevelName}", map.PublicLevelName).Replace("{GameMode}", map.GameMode).Replace("{FileName}", map.FileName).Replace("{TeamName}", this.GetLocalized(teamname.LocalizationKey, teamname.LocalizationKey));

                    if (String.Compare(formattedTeam, formattedTeamName) == 0) {
                        returnTeamName = teamname;
                        break;
                    }
                }

                if (returnTeamName != null) {
                    break;
                }
            }

            return returnTeamName;
        }

        // Get Gamemode List

        protected CMap GetMapByFilename(string strMapFileName) {
            CMap cmReturn = null;

            List<CMap> mapDefines = this.GetMapDefines();

            if (mapDefines != null) {
                foreach (CMap cmMap in mapDefines) {
                    if (String.Compare(cmMap.FileName, strMapFileName, true) == 0) {
                        cmReturn = cmMap;
                        break;
                    }
                }
            }

            return cmReturn;
        }
    }
}
