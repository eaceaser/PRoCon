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

//using PRoCon.Plugin;
using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using System.IO;
using System.Text.RegularExpressions;

using System.Security;
using System.Security.Permissions;
using System.Runtime.InteropServices;

using System.Threading;
using System.Security.Policy;

//using System.Windows.Forms;

namespace PRoCon.Core.Plugin {
    using Core;
    using Core.Players;
    //using Core.Plugin;
    using Core.Accounts;
    using Core.Maps;
    using Core.Plugin.Commands;
    using Core.Remote;
    using Core.Remote.Layer;

    public class PluginManager {
        
        public static string PLUGINS_DIRECTORY_NAME = "Plugins";

        #region Private member attributes

        private readonly object m_objMatchedInGameCommandsLocker = new object();

        private AppDomain m_appDomainSandbox;
        private CPRoConPluginCallbacks m_cpPluginCallbacks;

        private PRoConClient m_client;

        //private Dictionary<string, IPRoConPluginInterface> m_dicLoadedPlugins;
        //private Dictionary<string, IPRoConPluginInterface> m_dicEnabledPlugins;

        //public Dictionary<string, Dictionary<string, string>> CacheFailCompiledPluginVariables { get; private set; }

        #endregion

        #region Events and Delegates

        public delegate void PluginOutputHandler(string strOutput);
        public event PluginOutputHandler PluginOutput;

        public delegate void PluginEmptyParameterHandler(string strClassName);
        public event PluginEmptyParameterHandler PluginLoaded;
        public event PluginEmptyParameterHandler PluginEnabled;
        public event PluginEmptyParameterHandler PluginDisabled;

        public delegate void PluginVariableAlteredHandler(PluginDetails spdNewDetails);
        public event PluginVariableAlteredHandler PluginVariableAltered;

        #endregion

        #region Properties

        public PluginDictionary Plugins { get; private set; }

        //public List<string> EnabledClassNames {
        //    get {
        //        return this.m_plugins.EnabledClassNames;
        //    }
        //}
        
        //public List<string> LoadedClassNames { get; private set; }

        // TO DO: Move to seperate command control class with events captured by PluginManager.
        public Dictionary<string, MatchCommand> MatchedInGameCommands { get; private set; }
        private ConfirmationDictionary CommandsNeedingConfirmation { get; set; }

        public string PluginBaseDirectory {
            get {
                return Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PluginManager.PLUGINS_DIRECTORY_NAME), this.m_client.GameType);
            }
        }

        #endregion

        public PluginManager(PRoConClient cpcClient) {

            m_appDomainSandbox = null;
            this.Plugins = new PluginDictionary();
            //this.m_dicLoadedPlugins = new Dictionary<string, IPRoConPluginInterface>();
            //this.m_dicEnabledPlugins = new Dictionary<string, IPRoConPluginInterface>();

            //this.CacheFailCompiledPluginVariables = new Dictionary<string, Dictionary<string, string>>();


            this.m_client = cpcClient;
            //this.LoadedClassNames = new List<string>();
            this.MatchedInGameCommands = new Dictionary<string, MatchCommand>();
            this.CommandsNeedingConfirmation = new ConfirmationDictionary();

            this.AssignEventHandler();
        }

        // TO DO: Move to seperate command control class with events captured by PluginManager.
        public void RegisterCommand(MatchCommand mtcCommand) {

            lock (this.m_objMatchedInGameCommandsLocker) {

                if (mtcCommand.RegisteredClassname.Length > 0 && mtcCommand.RegisteredMethodName.Length > 0 && mtcCommand.Command.Length > 0) {

                    if (this.MatchedInGameCommands.ContainsKey(mtcCommand.ToString()) == true) {

                        if (String.Compare(this.MatchedInGameCommands[mtcCommand.ToString()].RegisteredClassname, mtcCommand.RegisteredClassname) != 0) {
                            this.WritePluginConsole("^1^bIdentical command registration on class {0} overwriting class {1} command {2}", mtcCommand.RegisteredClassname, this.MatchedInGameCommands[mtcCommand.ToString()].RegisteredClassname, this.MatchedInGameCommands[mtcCommand.ToString()].ToString());
                        }

                        this.MatchedInGameCommands[mtcCommand.ToString()] = mtcCommand;
                    }
                    else {
                        this.MatchedInGameCommands.Add(mtcCommand.ToString(), mtcCommand);

                        this.InvokeOnAllEnabled("OnRegisteredCommand", mtcCommand);
                    }
                }
            }
        }

        public void UnregisterCommand(MatchCommand mtcCommand) {
            lock (this.m_objMatchedInGameCommandsLocker) {
                if (this.MatchedInGameCommands.ContainsKey(mtcCommand.ToString()) == true) {
                    this.MatchedInGameCommands.Remove(mtcCommand.ToString());

                    this.InvokeOnAllEnabled("OnUnregisteredCommand", mtcCommand);
                }
            }
        }

        public List<MatchCommand> GetRegisteredCommands() {
            return new List<MatchCommand>(this.MatchedInGameCommands.Values);
        }

        private void WritePluginConsole(string strFormat, params object[] a_objArguments) {
            if (this.PluginOutput != null) {
                FrostbiteConnection.RaiseEvent(this.PluginOutput.GetInvocationList(), String.Format(strFormat, a_objArguments));
            }
        }

        public void EnablePlugin(string className) {

            if (this.Plugins.IsLoaded(className) == true && this.Plugins.IsEnabled(className) == false) {

                this.Plugins[className].IsEnabled = true;

                try {
                    this.Plugins[className].Type.Invoke("OnPluginEnable");

                    if (this.PluginEnabled != null) {
                        FrostbiteConnection.RaiseEvent(this.PluginEnabled.GetInvocationList(), className);
                    }
                }
                catch (Exception e) {
                    this.WritePluginConsole("{0}.EnablePlugin(): {1}", className, e.Message);
                }
            }

            /*

            // If it's loaded
            if (this.m_dicLoadedPlugins.ContainsKey(strClassName) == true && this.m_dicEnabledPlugins.ContainsKey(strClassName) == false) {
                // Move to enabled
                this.m_dicEnabledPlugins.Add(strClassName, this.m_dicLoadedPlugins[strClassName]);

                try {
                    if (this.m_dicEnabledPlugins.ContainsKey(strClassName) == true) {
                        this.m_dicEnabledPlugins[strClassName].Invoke("OnPluginEnable");

                        if (this.PluginEnabled != null) {
                            FrostbiteConnection.RaiseEvent(this.PluginEnabled.GetInvocationList(), strClassName);
                        }
                    }
                }
                catch (Exception e) {
                    if (this.PluginOutput != null) {

                    }

                    this.WritePluginConsole("{0}.EnablePlugin(): {1}", strClassName, e.Message);
                }
            }
             * */
        }

        public void DisablePlugin(string className) {

            if (this.Plugins.IsLoaded(className) == true && this.Plugins.IsEnabled(className) == true) {

                this.Plugins[className].IsEnabled = false;

                try {
                    this.Plugins[className].Type.Invoke("OnPluginDisable");

                    if (this.PluginDisabled != null) {
                        FrostbiteConnection.RaiseEvent(this.PluginDisabled.GetInvocationList(), className);
                    }
                }
                catch (Exception e) {
                    this.WritePluginConsole("{0}.DisablePlugin(): {1}", className, e.Message);
                }
            }

            /*
            if (this.m_dicEnabledPlugins.ContainsKey(strClassName) == true) {

                try {
                    if (this.m_dicEnabledPlugins.ContainsKey(strClassName) == true) {
                        this.m_dicEnabledPlugins[strClassName].Invoke("OnPluginDisable");
                    }
                }
                catch (Exception e) {
                    this.WritePluginConsole("{0}.DisablePlugin(): {1}", strClassName, e.Message);
                }

                this.m_dicEnabledPlugins.Remove(strClassName);
                
                if (this.PluginDisabled != null) {
                    FrostbiteConnection.RaiseEvent(this.PluginDisabled.GetInvocationList(), strClassName);
                }
            } */
        }

        public PluginDetails GetPluginDetails(string strClassName) {
            PluginDetails spdReturnDetails = new PluginDetails();

            spdReturnDetails.ClassName = strClassName;
            spdReturnDetails.Name = this.InvokeOnLoaded_String(strClassName, "GetPluginName");
            spdReturnDetails.Author = this.InvokeOnLoaded_String(strClassName, "GetPluginAuthor");
            spdReturnDetails.Version = this.InvokeOnLoaded_String(strClassName, "GetPluginVersion");
            spdReturnDetails.Website = this.InvokeOnLoaded_String(strClassName, "GetPluginWebsite");
            spdReturnDetails.Description = this.InvokeOnLoaded_String(strClassName, "GetPluginDescription");

            spdReturnDetails.DisplayPluginVariables = this.InvokeOnLoaded_CPluginVariables(strClassName, "GetDisplayPluginVariables");
            spdReturnDetails.PluginVariables = this.InvokeOnLoaded_CPluginVariables(strClassName, "GetPluginVariables");

            return spdReturnDetails;
        }

        public void SetPluginVariable(string strClassName, string strVariable, string strValue) {
            
            // FailCompiledPlugins

            if (this.Plugins.Contains(strClassName) == true && this.Plugins[strClassName].IsLoaded == true) {

                this.InvokeOnLoaded(strClassName, "SetPluginVariable", new object[] { strVariable, strValue });

                if (this.PluginVariableAltered != null) {
                    FrostbiteConnection.RaiseEvent(this.PluginVariableAltered.GetInvocationList(), this.GetPluginDetails(strClassName));
                }
            }
            else if (this.Plugins.IsLoaded(strClassName) == false) {

                this.Plugins.SetCachedPluginVariable(strClassName, strVariable, strValue);

                /*
                if (this.CacheFailCompiledPluginVariables[strClassName].ContainsKey(strVariable) == true) {
                    this.CacheFailCompiledPluginVariables[strClassName][strVariable] = strValue;
                }
                else {
                    this.CacheFailCompiledPluginVariables[strClassName].Add(strVariable, strValue);
                }
                */
            }
        }

        public void InvokeOnLoaded(string strClassName, string strMethod, params object[] a_objParameters) {
            try {
                if (this.Plugins.Contains(strClassName) == true && this.Plugins[strClassName].IsLoaded == true) {
                    this.Plugins[strClassName].Type.Invoke(strMethod, a_objParameters);
                }
            }
            catch (Exception e) {
                this.WritePluginConsole("{0}.{1}(): {2}", strClassName, strMethod, e.Message);
            }
        }

        public string InvokeOnLoaded_String(string strClassName, string strMethod, params object[] a_objParameters) {

            string strReturn = String.Empty;

            try {
                if (this.Plugins.Contains(strClassName) == true && this.Plugins[strClassName].IsLoaded == true) {
                    strReturn = (string)this.Plugins[strClassName].Type.Invoke(strMethod, a_objParameters);
                }
            }
            catch (Exception e) {
                this.WritePluginConsole("{0}.{1}(): {2}", strClassName, strMethod, e.Message);
            }

            return strReturn;
        }

        public List<CPluginVariable> GetPluginVariables(string strClassName) {
            return this.InvokeOnLoaded_CPluginVariables(strClassName, "GetPluginVariables");
        }

        private List<CPluginVariable> InvokeOnLoaded_CPluginVariables(string strClassName, string strMethod, params object[] a_objParameters) {

            List<CPluginVariable> lstReturn = null;

            try {
                if (this.Plugins.Contains(strClassName) == true && this.Plugins[strClassName].IsLoaded == true) {
                    lstReturn = (List<CPluginVariable>)this.Plugins[strClassName].Type.Invoke(strMethod, a_objParameters);
                }
            }
            catch (Exception e) {
                this.WritePluginConsole("{0}.{1}(): {2}", strClassName, strMethod, e.Message);
            }

            // If a problem occured return an empty list.
            if (lstReturn == null) {
                lstReturn = new List<CPluginVariable>();
            }

            return lstReturn;
        }

        public object InvokeOnEnabled(string strClassName, string strMethod, params object[] a_objParameters) {

            object returnObject = null;

            try {
                if (this.Plugins.Contains(strClassName) == true && this.Plugins[strClassName].IsEnabled == true) {
                    returnObject = this.Plugins[strClassName].Type.Invoke(strMethod, a_objParameters);
                }
            }
            catch (Exception e) {
                this.WritePluginConsole("{0}.{1}(): {2}", strClassName, strMethod, e.Message);
            }

            return returnObject;
        }

        public void InvokeOnAllLoaded(string strMethod, params object[] a_objParameters) {

            foreach (Plugin plugin in this.Plugins) {
                if (plugin.IsLoaded == true) {
                    try {
                        plugin.ConditionalInvoke(strMethod, a_objParameters);
                    }
                    catch (Exception e) {
                        this.WritePluginConsole("{0}.{1}(): {2}", plugin.ClassName, strMethod, e.Message);
                    }
                }
            }
        }

        public void InvokeOnAllEnabled(string strMethod, params object[] a_objParameters) {

            foreach (Plugin plugin in this.Plugins) {
                if (plugin.IsEnabled == true) {
                    try {
                        plugin.ConditionalInvoke(strMethod, a_objParameters);
                    }
                    catch (Exception e) {
                        this.WritePluginConsole("{0}.{1}(): {2}", plugin.ClassName, strMethod, e.Message);
                    }
                }
            }
        }

        private void PreparePluginsDirectory() {
            
            try {
                if (Directory.Exists(this.PluginBaseDirectory) == false) {
                    Directory.CreateDirectory(this.PluginBaseDirectory);
                }
                
                File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PRoCon.Core.dll"), Path.Combine(this.PluginBaseDirectory, "PRoCon.Core.dll"), true);
                File.Copy(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PRoCon.Core.pdb"), Path.Combine(this.PluginBaseDirectory, "PRoCon.Core.pdb"), true);
            }
            catch (Exception e) { }
        }

        private void MoveLegacyPlugins() {

            try {

                string legacyPluginDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PluginManager.PLUGINS_DIRECTORY_NAME);
                string legacyPluginDestinationDirectory = Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PluginManager.PLUGINS_DIRECTORY_NAME), "BFBC2");

                DirectoryInfo legacyPluginsDirectoryInfo = new DirectoryInfo(legacyPluginDirectory);
                FileInfo[] a_legacyPluginsInfo = legacyPluginsDirectoryInfo.GetFiles("*.cs");

                foreach (FileInfo legacyPlugin in a_legacyPluginsInfo) {

                    try {
                        File.Move(legacyPlugin.FullName, Path.Combine(legacyPluginDestinationDirectory, legacyPlugin.Name));
                    }
                    catch (Exception e) {
                        this.WritePluginConsole("^1PluginManager.MoveLegacyPlugins(): Move: \"{0}\"; Keeping /Plugins/BFBC2/ version.  Warning: {1};", legacyPlugin.Name, e.Message);
                    }
                }
            }
            catch (Exception e) {
                this.WritePluginConsole("^1PluginManager.MoveLegacyPlugins(): Error: {0};", e.Message);
            }
        }

        private CompilerParameters GenerateCompilerParameters() {

            CompilerParameters parameters = new CompilerParameters();

            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Data.dll");
            parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            parameters.ReferencedAssemblies.Add("System.Xml.dll");
            parameters.ReferencedAssemblies.Add("PRoCon.Core.dll");
            parameters.GenerateInMemory = false;
            parameters.IncludeDebugInformation = false;
            parameters.OutputAssembly = "Default.dll";

            return parameters;
        }

        private void PrintPluginResults(FileInfo pluginFile, CompilerResults pluginResults) {

            // Produce compiler errors (if any)
            if (pluginResults.Errors.HasErrors == true && pluginResults.Errors[0].ErrorNumber.CompareTo("CS0016") != 0) {
                this.WritePluginConsole("Compiling {0}... ^1Errors^0 or ^3Warnings", pluginFile.Name);

                foreach (CompilerError errComp in pluginResults.Errors) {
                    if (errComp.ErrorNumber.CompareTo("CS0016") != 0 && errComp.IsWarning == false) {
                        this.WritePluginConsole("\t^1{0} (Line: {1}, C: {2}) {3}: {4}", pluginFile.Name, errComp.Line, errComp.Column, errComp.ErrorNumber, errComp.ErrorText);
                    }
                }
            }
            else {
                this.WritePluginConsole("Compiling {0}... ^2Done", pluginFile.Name);
            }
        }

        private string PrecompileDirectives(string source) {

            MatchCollection matches;
            int replacementDepth = 0;

            do {
                matches = Regex.Matches(source, "#include \"(?<file>.*?)\"");

                foreach (Match match in matches) {
                    try {
                        string fileContents = File.ReadAllText(Path.Combine(this.PluginBaseDirectory, match.Groups["file"].Value.Replace("%GameType%", this.m_client.GameType)));

                        source = source.Replace(match.Value, fileContents);
                    }
                    catch (Exception e) {
                        this.WritePluginConsole("^PluginManager.PrecompileDirectives(): #include File Error: {0};", e.Message);
                    }
                }

                replacementDepth++;
            } while (matches.Count > 0 && replacementDepth <= 5);

            if (replacementDepth > 5) {
                this.WritePluginConsole("^PluginManager.PrecompileDirectives(): #include Recursion Error: Invalid depth of {0}", replacementDepth);
            }


            return source;
        }

        private void CompilePlugin(FileInfo pluginFile, string pluginClassName, CodeDomProvider pluginsCodeDomProvider, CompilerParameters parameters) {

            try {
                string outputAssembly = Path.Combine(this.PluginBaseDirectory, pluginClassName + ".dll");

                if (File.Exists(outputAssembly) == false) {

                    string fullPluginSource = File.ReadAllText(pluginFile.FullName);

                    parameters.OutputAssembly = outputAssembly;

                    fullPluginSource = this.PrecompileDirectives(fullPluginSource);

                    fullPluginSource = fullPluginSource.Replace("using PRoCon.Plugin;", "using PRoCon.Core.Plugin;");

                    if (fullPluginSource.Contains("using PRoCon.Core;") == false) {
                        fullPluginSource = fullPluginSource.Insert(fullPluginSource.IndexOf("using PRoCon.Core.Plugin;"), "\r\nusing PRoCon.Core;\r\n");
                    }

                    if (fullPluginSource.Contains("using PRoCon.Core.Players;") == false) {
                        fullPluginSource = fullPluginSource.Insert(fullPluginSource.IndexOf("using PRoCon.Core.Plugin;"), "\r\nusing PRoCon.Core.Players;\r\n");
                    }

                    this.PrintPluginResults(pluginFile, pluginsCodeDomProvider.CompileAssemblyFromSource(parameters, fullPluginSource));
                }
                else {
                    this.WritePluginConsole("Compiling {0}... Skipping", pluginFile.Name);
                }
            }
            catch (Exception) { }
        }

        private void LoadPlugin(string pluginClassName, CPRoConPluginLoaderFactory pluginFactory) {

            string outputAssembly = Path.Combine(this.PluginBaseDirectory, pluginClassName + ".dll");

            if (File.Exists(outputAssembly) == true) {

                IPRoConPluginInterface loRemote = (IPRoConPluginInterface)pluginFactory.Create(outputAssembly, "PRoConEvents." + pluginClassName, null);

                // Indirectely invoke registercallbacks since the delegates cannot go in the interface.
                loRemote.Invoke("RegisterCallbacks", new object[] { new CPRoConMarshalByRefObject.ExecuteCommandHandler(this.m_cpPluginCallbacks.ExecuteCommand_Callback),
                                                                            new CPRoConMarshalByRefObject.GetAccountPrivilegesHandler(this.m_cpPluginCallbacks.GetAccountPrivileges_Callback),
                                                                            new CPRoConMarshalByRefObject.GetVariableHandler(this.m_cpPluginCallbacks.GetVariable_Callback),
                                                                            new CPRoConMarshalByRefObject.GetVariableHandler(this.m_cpPluginCallbacks.GetSvVariable_Callback),
                                                                            new CPRoConMarshalByRefObject.GetMapDefinesHandler(this.m_cpPluginCallbacks.GetMapDefines_Callback),
                                                                            new CPRoConMarshalByRefObject.TryGetLocalizedHandler(this.m_cpPluginCallbacks.TryGetLocalized_Callback),
                                                                            new CPRoConMarshalByRefObject.RegisterCommandHandler(this.m_cpPluginCallbacks.RegisterCommand_Callback),
                                                                            new CPRoConMarshalByRefObject.UnregisterCommandHandler(this.m_cpPluginCallbacks.UnregisterCommand_Callback),
                                                                            new CPRoConMarshalByRefObject.GetRegisteredCommandsHandler(this.m_cpPluginCallbacks.GetRegisteredCommands_Callback),
                                                                            new CPRoConMarshalByRefObject.GetWeaponDefinesHandler(this.m_cpPluginCallbacks.GetWeaponDefines_Callback),
                                                                            new CPRoConMarshalByRefObject.GetSpecializationDefinesHandler(this.m_cpPluginCallbacks.GetSpecializationDefines_Callback),
                                                                            new CPRoConMarshalByRefObject.GetLoggedInAccountUsernamesHandler(this.m_cpPluginCallbacks.GetLoggedInAccountUsernames_Callback),
                                                                            new CPRoConMarshalByRefObject.RegisterEventsHandler(this.m_cpPluginCallbacks.RegisterEvents_Callback)
                                                                          });

                this.Plugins.AddLoadedPlugin(pluginClassName, loRemote);

                /*
                if (this.m_dicLoadedPlugins.ContainsKey(pluginClassName) == false) {
                    this.m_dicLoadedPlugins.Add(pluginClassName, loRemote);
                }
                else {
                    this.m_dicLoadedPlugins[pluginClassName] = loRemote;
                }

                this.LoadedClassNames.Add(pluginClassName);
                */
                this.WritePluginConsole("Loading {0}... ^2Loaded", pluginClassName);

                this.InvokeOnLoaded(pluginClassName, "OnPluginLoaded", this.m_client.HostName, this.m_client.Port.ToString(), Assembly.GetExecutingAssembly().GetName().Version.ToString());

                if (this.PluginLoaded != null) {
                    FrostbiteConnection.RaiseEvent(this.PluginLoaded.GetInvocationList(), pluginClassName);
                }
            }
            //else {
            //    this.CacheFailCompiledPluginVariables.Add(pluginClassName, new Dictionary<string, string>());
            //}
        }

        public void RegisterPluginEvents(string className, List<string> events) {
            
            if (this.Plugins.IsLoaded(className) == true) {
                this.Plugins[className].RegisteredEvents = events;
            }
        }

        public void CompilePlugins(PermissionSet pluginSandboxPermissions) {

            try {
                this.WritePluginConsole("Preparing plugins directory..");
                this.PreparePluginsDirectory();
                this.CleanPlugins();

                this.WritePluginConsole("Moving legacy plugins..");
                this.MoveLegacyPlugins();

                this.WritePluginConsole("Creating compiler..");
                CodeDomProvider pluginsCodeDomProvider = CodeDomProvider.CreateProvider("CSharp");

                this.WritePluginConsole("Configuring compiler..");
                CompilerParameters parameters = GenerateCompilerParameters();
                AppDomainSetup domainSetup = new AppDomainSetup() { ApplicationBase = this.PluginBaseDirectory };

                this.WritePluginConsole("Building sandbox..");
                Evidence hostEvidence = new Evidence();
                hostEvidence.AddHost(new Zone(SecurityZone.MyComputer));

                this.m_appDomainSandbox = AppDomain.CreateDomain(this.m_client.HostName + this.m_client.Port + "Engine", hostEvidence, domainSetup, pluginSandboxPermissions);

                this.WritePluginConsole("Configuring sandbox..");
                // create the factory class in the secondary app-domain
                CPRoConPluginLoaderFactory pluginFactory = (CPRoConPluginLoaderFactory)this.m_appDomainSandbox.CreateInstance("PRoCon.Core", "PRoCon.Core.Plugin.CPRoConPluginLoaderFactory").Unwrap();
                this.m_cpPluginCallbacks = new CPRoConPluginCallbacks(this.m_client.ExecuteCommand, this.m_client.GetAccountPrivileges, this.m_client.GetVariable, this.m_client.GetSvVariable, this.m_client.GetMapDefines, this.m_client.TryGetLocalized, this.RegisterCommand, this.UnregisterCommand, this.GetRegisteredCommands, this.m_client.GetWeaponDefines, this.m_client.GetSpecializationDefines, this.m_client.Layer.GetLoggedInAccounts, this.RegisterPluginEvents);

                this.WritePluginConsole("Compiling and loading plugins..");


                DirectoryInfo pluginsDirectoryInfo = new DirectoryInfo(this.PluginBaseDirectory);
                FileInfo[] a_pluginsFileInfo = pluginsDirectoryInfo.GetFiles("*.cs");

                foreach (FileInfo pluginFile in a_pluginsFileInfo) {

                    string className = Regex.Replace(pluginFile.Name, "\\.cs$", "");

                    this.CompilePlugin(pluginFile, className, pluginsCodeDomProvider, parameters);

                    this.LoadPlugin(className, pluginFactory);
                }

                pluginsCodeDomProvider.Dispose();
            }
            catch (Exception e) {
                this.WritePluginConsole(e.Message);
            }
        }

        ~PluginManager() {
            this.m_appDomainSandbox = null;
            //this.m_dicEnabledPlugins = null;
            //this.m_dicLoadedPlugins = null;
            //this.LoadedClassNames = null;
            this.m_client = null;
        }

        private void CleanPlugins() {

            DirectoryInfo pluginsDirectoryInfo = new DirectoryInfo(this.PluginBaseDirectory);
            FileInfo[] a_pluginsFileInfo = pluginsDirectoryInfo.GetFiles("*.cs");

            foreach (FileInfo pluginFile in a_pluginsFileInfo) {

                string compiledPluginFileName = Path.Combine(this.PluginBaseDirectory, Regex.Replace(pluginFile.Name, "\\.cs$", "") + ".dll");

                if (File.Exists(compiledPluginFileName) == true) {
                    try {
                        File.Delete(compiledPluginFileName);
                    }
                    catch (Exception) { }
                }
            }

        }

        public void Unload() {

            this.UnassignEventHandler();

            try {
                if (this.m_appDomainSandbox != null) {
                    AppDomain.Unload(this.m_appDomainSandbox);
                }
            }
            catch (Exception e) {
                if (this.m_client != null) {
                    this.WritePluginConsole("^1{0}", e.Message);
                }
            }

            this.CleanPlugins();

            GC.Collect();
        }

        #region Event Assignments

        private void UnassignEventHandler() {

            this.m_client.Login -= new PRoConClient.EmptyParamterHandler(m_prcClient_CommandLogin);
            this.m_client.Logout -= new PRoConClient.EmptyParamterHandler(m_prcClient_CommandLogout);
            this.m_client.Game.Quit -= new FrostbiteClient.EmptyParamterHandler(m_prcClient_CommandQuit);

            this.m_client.ConnectionClosed -= new PRoConClient.EmptyParamterHandler(m_prcClient_ConnectionClosed);

            this.m_client.Game.PlayerJoin -= new FrostbiteClient.PlayerEventHandler(m_prcClient_PlayerJoin);
            this.m_client.Game.PlayerLeft -= new FrostbiteClient.PlayerLeaveHandler(m_prcClient_PlayerLeft);
            this.m_client.Game.PlayerAuthenticated -= new FrostbiteClient.PlayerAuthenticatedHandler(m_prcClient_PlayerAuthenticated);
            this.m_client.Game.PlayerKicked -= new FrostbiteClient.PlayerKickedHandler(m_prcClient_PlayerKicked);
            this.m_client.Game.PlayerChangedTeam -= new FrostbiteClient.PlayerTeamChangeHandler(m_prcClient_PlayerChangedTeam);
            this.m_client.Game.PlayerChangedSquad -= new FrostbiteClient.PlayerTeamChangeHandler(m_prcClient_PlayerChangedSquad);
            this.m_client.PlayerKilled -= new PRoConClient.PlayerKilledHandler(m_prcClient_PlayerKilled);

            this.m_client.Game.GlobalChat -= new FrostbiteClient.GlobalChatHandler(m_prcClient_GlobalChat);
            this.m_client.Game.TeamChat -= new FrostbiteClient.TeamChatHandler(m_prcClient_TeamChat);
            this.m_client.Game.SquadChat -= new FrostbiteClient.SquadChatHandler(m_prcClient_SquadChat);

            this.m_client.Game.ResponseError -= new FrostbiteClient.ResponseErrorHandler(m_prcClient_ResponseError);
            this.m_client.Game.Version -= new FrostbiteClient.VersionHandler(m_prcClient_Version);
            this.m_client.Game.Help -= new FrostbiteClient.HelpHandler(m_prcClient_Help);

            this.m_client.Game.RunScript -= new FrostbiteClient.RunScriptHandler(m_prcClient_RunScript);
            this.m_client.Game.RunScriptError -= new FrostbiteClient.RunScriptErrorHandler(m_prcClient_RunScriptError);

            this.m_client.Game.PunkbusterMessage -= new FrostbiteClient.PunkbusterMessageHandler(m_prcClient_PunkbusterMessage);
            this.m_client.Game.LoadingLevel -= new FrostbiteClient.LoadingLevelHandler(m_prcClient_LoadingLevel);
            this.m_client.Game.LevelStarted -= new FrostbiteClient.EmptyParamterHandler(m_prcClient_LevelStarted);

            this.m_client.Game.ServerInfo -= new FrostbiteClient.ServerInfoHandler(m_prcClient_ServerInfo);
            this.m_client.Game.Yelling -= new FrostbiteClient.YellingHandler(m_prcClient_Yelling);
            this.m_client.Game.Saying -= new FrostbiteClient.SayingHandler(m_prcClient_Saying);

            this.m_client.Game.RunNextRound -= new FrostbiteClient.EmptyParamterHandler(m_prcClient_RunNextLevel);
            this.m_client.Game.CurrentLevel -= new FrostbiteClient.CurrentLevelHandler(m_prcClient_CurrentLevel);
            this.m_client.Game.RestartRound -= new FrostbiteClient.EmptyParamterHandler(m_prcClient_RestartLevel);
            this.m_client.Game.SupportedMaps -= new FrostbiteClient.SupportedMapsHandler(m_prcClient_SupportedMaps);

            this.m_client.Game.PlaylistSet -= new FrostbiteClient.PlaylistSetHandler(m_prcClient_PlaylistSet);
            this.m_client.Game.ListPlaylists -= new FrostbiteClient.ListPlaylistsHandler(m_prcClient_ListPlaylists);

            this.m_client.Game.ListPlayers -= new FrostbiteClient.ListPlayersHandler(m_prcClient_ListPlayers);

            this.m_client.Game.BanListLoad -= new FrostbiteClient.EmptyParamterHandler(m_prcClient_BanListLoad);
            this.m_client.Game.BanListSave -= new FrostbiteClient.EmptyParamterHandler(m_prcClient_BanListSave);
            this.m_client.Game.BanListAdd -= new FrostbiteClient.BanListAddHandler(m_prcClient_BanListAdd);
            this.m_client.Game.BanListRemove -= new FrostbiteClient.BanListRemoveHandler(m_prcClient_BanListRemove);
            this.m_client.Game.BanListClear -= new FrostbiteClient.EmptyParamterHandler(m_prcClient_BanListClear);
            this.m_client.FullBanListList -= new PRoConClient.FullBanListListHandler(m_prcClient_BanListList);

            this.m_client.Game.ReservedSlotsConfigFile -= new FrostbiteClient.ReserverdSlotsConfigFileHandler(m_prcClient_ReservedSlotsConfigFile);
            this.m_client.Game.ReservedSlotsLoad -= new FrostbiteClient.EmptyParamterHandler(m_prcClient_ReservedSlotsLoad);
            this.m_client.Game.ReservedSlotsSave -= new FrostbiteClient.EmptyParamterHandler(m_prcClient_ReservedSlotsSave);
            this.m_client.Game.ReservedSlotsPlayerAdded -= new FrostbiteClient.ReservedSlotsPlayerHandler(m_prcClient_ReservedSlotsPlayerAdded);
            this.m_client.Game.ReservedSlotsPlayerRemoved -= new FrostbiteClient.ReservedSlotsPlayerHandler(m_prcClient_ReservedSlotsPlayerRemoved);
            this.m_client.Game.ReservedSlotsCleared -= new FrostbiteClient.EmptyParamterHandler(m_prcClient_ReservedSlotsCleared);
            this.m_client.Game.ReservedSlotsList -= new FrostbiteClient.ReservedSlotsListHandler(m_prcClient_ReservedSlotsList);

            this.m_client.Game.MapListConfigFile -= new FrostbiteClient.MapListConfigFileHandler(m_prcClient_MapListConfigFile);
            this.m_client.Game.MapListLoad -= new FrostbiteClient.EmptyParamterHandler(m_prcClient_MapListLoad);
            this.m_client.Game.MapListSave -= new FrostbiteClient.EmptyParamterHandler(m_prcClient_MapListSave);
            this.m_client.Game.MapListMapAppended -= new FrostbiteClient.MapListAppendedHandler(m_prcClient_MapListMapAppended);
            this.m_client.Game.MapListNextLevelIndex -= new FrostbiteClient.MapListLevelIndexHandler(m_prcClient_MapListNextLevelIndex);
            this.m_client.Game.MapListMapRemoved -= new FrostbiteClient.MapListLevelIndexHandler(m_prcClient_MapListMapRemoved);
            this.m_client.Game.MapListMapInserted -= new FrostbiteClient.MapListMapInsertedHandler(m_prcClient_MapListMapInserted);
            this.m_client.Game.MapListCleared -= new FrostbiteClient.EmptyParamterHandler(m_prcClient_MapListCleared);
            this.m_client.Game.MapListListed -= new FrostbiteClient.MapListListedHandler(m_prcClient_MapListListed);

            this.m_client.Game.TextChatModerationListAddPlayer -= new FrostbiteClient.TextChatModerationListAddPlayerHandler(Game_TextChatModerationListAddPlayer);
            this.m_client.Game.TextChatModerationListRemovePlayer -= new FrostbiteClient.TextChatModerationListRemovePlayerHandler(Game_TextChatModerationListRemovePlayer);
            this.m_client.Game.TextChatModerationListClear -= new FrostbiteClient.EmptyParamterHandler(Game_TextChatModerationListClear);
            this.m_client.Game.TextChatModerationListSave -= new FrostbiteClient.EmptyParamterHandler(Game_TextChatModerationListSave);
            this.m_client.Game.TextChatModerationListLoad -= new FrostbiteClient.EmptyParamterHandler(Game_TextChatModerationListLoad);
            this.m_client.FullTextChatModerationListList -= new PRoConClient.FullTextChatModerationListListHandler(m_client_FullTextChatModerationListList);

            this.m_client.Game.GamePassword -= new FrostbiteClient.PasswordHandler(m_prcClient_GamePassword);
            this.m_client.Game.Punkbuster -= new FrostbiteClient.IsEnabledHandler(m_prcClient_Punkbuster);
            this.m_client.Game.Hardcore -= new FrostbiteClient.IsEnabledHandler(m_prcClient_Hardcore);
            this.m_client.Game.Ranked -= new FrostbiteClient.IsEnabledHandler(m_prcClient_Ranked);
            this.m_client.Game.RankLimit -= new FrostbiteClient.LimitHandler(m_prcClient_RankLimit);
            this.m_client.Game.TeamBalance -= new FrostbiteClient.IsEnabledHandler(m_prcClient_TeamBalance);
            this.m_client.Game.FriendlyFire -= new FrostbiteClient.IsEnabledHandler(m_prcClient_FriendlyFire);
            this.m_client.Game.MaxPlayerLimit -= new FrostbiteClient.LimitHandler(m_prcClient_MaxPlayerLimit);
            this.m_client.Game.CurrentPlayerLimit -= new FrostbiteClient.LimitHandler(m_prcClient_CurrentPlayerLimit);
            this.m_client.Game.PlayerLimit -= new FrostbiteClient.LimitHandler(m_prcClient_PlayerLimit);
            this.m_client.Game.BannerUrl -= new FrostbiteClient.BannerUrlHandler(m_prcClient_BannerUrl);
            this.m_client.Game.ServerDescription -= new FrostbiteClient.ServerDescriptionHandler(m_prcClient_ServerDescription);
            this.m_client.Game.KillCam -= new FrostbiteClient.IsEnabledHandler(m_prcClient_KillCam);
            this.m_client.Game.MiniMap -= new FrostbiteClient.IsEnabledHandler(m_prcClient_MiniMap);
            this.m_client.Game.CrossHair -= new FrostbiteClient.IsEnabledHandler(m_prcClient_CrossHair);
            this.m_client.Game.ThreeDSpotting -= new FrostbiteClient.IsEnabledHandler(m_prcClient_ThreeDSpotting);
            this.m_client.Game.MiniMapSpotting -= new FrostbiteClient.IsEnabledHandler(m_prcClient_MiniMapSpotting);
            this.m_client.Game.ThirdPersonVehicleCameras -= new FrostbiteClient.IsEnabledHandler(m_prcClient_ThirdPersonVehicleCameras);

            this.m_client.Game.TextChatModerationMode -= new FrostbiteClient.TextChatModerationModeHandler(Game_TextChatModerationMode);
            this.m_client.Game.TextChatSpamCoolDownTime -= new FrostbiteClient.LimitHandler(Game_TextChatSpamCoolDownTime);
            this.m_client.Game.TextChatSpamDetectionTime -= new FrostbiteClient.LimitHandler(Game_TextChatSpamDetectionTime);
            this.m_client.Game.TextChatSpamTriggerCount -= new FrostbiteClient.LimitHandler(Game_TextChatSpamTriggerCount);

            // R13
            this.m_client.Game.ServerName -= new FrostbiteClient.ServerNameHandler(m_prcClient_ServerName);
            this.m_client.Game.TeamKillCountForKick -= new FrostbiteClient.LimitHandler(m_prcClient_TeamKillCountForKick);
            this.m_client.Game.TeamKillValueIncrease -= new FrostbiteClient.LimitHandler(m_prcClient_TeamKillValueIncrease);
            this.m_client.Game.TeamKillValueDecreasePerSecond -= new FrostbiteClient.LimitHandler(m_prcClient_TeamKillValueDecreasePerSecond);
            this.m_client.Game.TeamKillValueForKick -= new FrostbiteClient.LimitHandler(m_prcClient_TeamKillValueForKick);
            this.m_client.Game.IdleTimeout -= new FrostbiteClient.LimitHandler(m_prcClient_IdleTimeout);
            this.m_client.Game.ProfanityFilter -= new FrostbiteClient.IsEnabledHandler(m_prcClient_ProfanityFilter);

            this.m_client.PlayerSpawned -= new PRoConClient.PlayerSpawnedHandler(m_prcClient_PlayerSpawned);
            this.m_client.Game.RoundOver -= new FrostbiteClient.RoundOverHandler(m_prcClient_RoundOver);
            this.m_client.Game.RoundOverPlayers -= new FrostbiteClient.RoundOverPlayersHandler(m_prcClient_RoundOverPlayers);
            this.m_client.Game.RoundOverTeamScores -= new FrostbiteClient.RoundOverTeamScoresHandler(m_prcClient_RoundOverTeamScores);
            this.m_client.Game.EndRound -= new FrostbiteClient.EndRoundHandler(m_prcClient_EndRound);

            this.m_client.Game.LevelVariablesGet -= new FrostbiteClient.LevelVariableGetHandler(m_prcClient_LevelVariablesGet);
            this.m_client.Game.LevelVariablesSet -= new FrostbiteClient.LevelVariableHandler(m_prcClient_LevelVariablesSet);
            this.m_client.Game.LevelVariablesClear -= new FrostbiteClient.LevelVariableHandler(m_prcClient_LevelVariablesClear);
            this.m_client.Game.LevelVariablesEvaluate -= new FrostbiteClient.LevelVariableGetHandler(m_prcClient_LevelVariablesEvaluate);
            this.m_client.Game.LevelVariablesList -= new FrostbiteClient.LevelVariableListHandler(m_prcClient_LevelVariablesList);

            this.m_client.ReceiveProconVariable -= new PRoConClient.ReceiveProconVariableHandler(m_prcClient_ReceiveProconVariable);

            this.m_client.PunkbusterBeginPlayerInfo -= new PRoConClient.EmptyParamterHandler(m_client_PunkbusterBeginPlayerInfo);
            this.m_client.PunkbusterEndPlayerInfo -= new PRoConClient.EmptyParamterHandler(m_client_PunkbusterEndPlayerInfo);
            this.m_client.PunkbusterPlayerInfo -= new PRoConClient.PunkbusterPlayerInfoHandler(m_prcClient_PunkbusterPlayerInfo);
            this.m_client.PunkbusterPlayerBanned -= new PRoConClient.PunkbusterBanHandler(m_prcClient_PunkbusterPlayerBanned);
            this.m_client.PunkbusterPlayerUnbanned -= new PRoConClient.PunkbusterBanHandler(m_prcClient_PunkbusterPlayerUnbanned);

            this.m_client.Layer.AccountPrivileges.AccountPrivilegeAdded -= new AccountPrivilegeDictionary.AccountPrivilegeAlteredHandler(AccountPrivileges_AccountPrivilegeAdded);
            this.m_client.Layer.AccountPrivileges.AccountPrivilegeRemoved -= new AccountPrivilegeDictionary.AccountPrivilegeAlteredHandler(AccountPrivileges_AccountPrivilegeRemoved);

            this.m_client.MapGeometry.MapZoneTrespassed -= new PRoCon.Core.Battlemap.MapGeometry.MapZoneTrespassedHandler(MapGeometry_MapZoneTrespassed);

            #region Admin actions on players

            this.m_client.Game.PlayerKickedByAdmin -= new FrostbiteClient.PlayerKickedHandler(Game_PlayerKickedByAdmin);
            this.m_client.Game.PlayerKilledByAdmin -= new FrostbiteClient.PlayerKilledByAdminHandler(Game_PlayerKilledByAdmin);
            this.m_client.Game.PlayerMovedByAdmin -= new FrostbiteClient.PlayerMovedByAdminHandler(Game_PlayerMovedByAdmin);

            #endregion

            #region Layer Accounts

            foreach (PRoConLayerClient client in this.m_client.Layer.LayerClients) {
                client.Login -= new PRoConLayerClient.LayerClientHandler(client_LayerClientLogin);
                client.Logout -= new PRoConLayerClient.LayerClientHandler(client_LayerClientLogout);
            }
            this.m_client.Layer.ClientConnected -= new Remote.Layer.PRoConLayer.LayerAccountHandler(Layer_ClientConnected);

            #endregion
        }

        private void AssignEventHandler() {

            this.m_client.Login += new PRoConClient.EmptyParamterHandler(m_prcClient_CommandLogin);
            this.m_client.Logout += new PRoConClient.EmptyParamterHandler(m_prcClient_CommandLogout);
            this.m_client.Game.Quit += new FrostbiteClient.EmptyParamterHandler(m_prcClient_CommandQuit);

            this.m_client.ConnectionClosed += new PRoConClient.EmptyParamterHandler(m_prcClient_ConnectionClosed);

            this.m_client.Game.PlayerJoin += new FrostbiteClient.PlayerEventHandler(m_prcClient_PlayerJoin);
            this.m_client.Game.PlayerLeft += new FrostbiteClient.PlayerLeaveHandler(m_prcClient_PlayerLeft);
            this.m_client.Game.PlayerAuthenticated += new FrostbiteClient.PlayerAuthenticatedHandler(m_prcClient_PlayerAuthenticated);
            this.m_client.Game.PlayerKicked += new FrostbiteClient.PlayerKickedHandler(m_prcClient_PlayerKicked);
            this.m_client.Game.PlayerChangedTeam += new FrostbiteClient.PlayerTeamChangeHandler(m_prcClient_PlayerChangedTeam);
            this.m_client.Game.PlayerChangedSquad += new FrostbiteClient.PlayerTeamChangeHandler(m_prcClient_PlayerChangedSquad);
            this.m_client.PlayerKilled += new PRoConClient.PlayerKilledHandler(m_prcClient_PlayerKilled);

            this.m_client.Game.GlobalChat += new FrostbiteClient.GlobalChatHandler(m_prcClient_GlobalChat);
            this.m_client.Game.TeamChat += new FrostbiteClient.TeamChatHandler(m_prcClient_TeamChat);
            this.m_client.Game.SquadChat += new FrostbiteClient.SquadChatHandler(m_prcClient_SquadChat);

            this.m_client.Game.ResponseError += new FrostbiteClient.ResponseErrorHandler(m_prcClient_ResponseError);
            this.m_client.Game.Version += new FrostbiteClient.VersionHandler(m_prcClient_Version);
            this.m_client.Game.Help += new FrostbiteClient.HelpHandler(m_prcClient_Help);

            this.m_client.Game.RunScript += new FrostbiteClient.RunScriptHandler(m_prcClient_RunScript);
            this.m_client.Game.RunScriptError += new FrostbiteClient.RunScriptErrorHandler(m_prcClient_RunScriptError);

            this.m_client.Game.PunkbusterMessage += new FrostbiteClient.PunkbusterMessageHandler(m_prcClient_PunkbusterMessage);
            this.m_client.Game.LoadingLevel += new FrostbiteClient.LoadingLevelHandler(m_prcClient_LoadingLevel);
            this.m_client.Game.LevelStarted += new FrostbiteClient.EmptyParamterHandler(m_prcClient_LevelStarted);

            this.m_client.Game.ServerInfo += new FrostbiteClient.ServerInfoHandler(m_prcClient_ServerInfo);
            this.m_client.Game.Yelling += new FrostbiteClient.YellingHandler(m_prcClient_Yelling);
            this.m_client.Game.Saying += new FrostbiteClient.SayingHandler(m_prcClient_Saying);

            this.m_client.Game.RunNextRound += new FrostbiteClient.EmptyParamterHandler(m_prcClient_RunNextLevel);
            this.m_client.Game.CurrentLevel += new FrostbiteClient.CurrentLevelHandler(m_prcClient_CurrentLevel);
            this.m_client.Game.RestartRound += new FrostbiteClient.EmptyParamterHandler(m_prcClient_RestartLevel);
            this.m_client.Game.SupportedMaps += new FrostbiteClient.SupportedMapsHandler(m_prcClient_SupportedMaps);

            this.m_client.Game.PlaylistSet += new FrostbiteClient.PlaylistSetHandler(m_prcClient_PlaylistSet);
            this.m_client.Game.ListPlaylists += new FrostbiteClient.ListPlaylistsHandler(m_prcClient_ListPlaylists);

            this.m_client.Game.ListPlayers += new FrostbiteClient.ListPlayersHandler(m_prcClient_ListPlayers);

            this.m_client.Game.BanListLoad += new FrostbiteClient.EmptyParamterHandler(m_prcClient_BanListLoad);
            this.m_client.Game.BanListSave += new FrostbiteClient.EmptyParamterHandler(m_prcClient_BanListSave);
            this.m_client.Game.BanListAdd += new FrostbiteClient.BanListAddHandler(m_prcClient_BanListAdd);
            this.m_client.Game.BanListRemove += new FrostbiteClient.BanListRemoveHandler(m_prcClient_BanListRemove);
            this.m_client.Game.BanListClear += new FrostbiteClient.EmptyParamterHandler(m_prcClient_BanListClear);
            this.m_client.FullBanListList += new PRoConClient.FullBanListListHandler(m_prcClient_BanListList);

            this.m_client.Game.ReservedSlotsConfigFile += new FrostbiteClient.ReserverdSlotsConfigFileHandler(m_prcClient_ReservedSlotsConfigFile);
            this.m_client.Game.ReservedSlotsLoad += new FrostbiteClient.EmptyParamterHandler(m_prcClient_ReservedSlotsLoad);
            this.m_client.Game.ReservedSlotsSave += new FrostbiteClient.EmptyParamterHandler(m_prcClient_ReservedSlotsSave);
            this.m_client.Game.ReservedSlotsPlayerAdded += new FrostbiteClient.ReservedSlotsPlayerHandler(m_prcClient_ReservedSlotsPlayerAdded);
            this.m_client.Game.ReservedSlotsPlayerRemoved += new FrostbiteClient.ReservedSlotsPlayerHandler(m_prcClient_ReservedSlotsPlayerRemoved);
            this.m_client.Game.ReservedSlotsCleared += new FrostbiteClient.EmptyParamterHandler(m_prcClient_ReservedSlotsCleared);
            this.m_client.Game.ReservedSlotsList += new FrostbiteClient.ReservedSlotsListHandler(m_prcClient_ReservedSlotsList);

            this.m_client.Game.MapListConfigFile += new FrostbiteClient.MapListConfigFileHandler(m_prcClient_MapListConfigFile);
            this.m_client.Game.MapListLoad += new FrostbiteClient.EmptyParamterHandler(m_prcClient_MapListLoad);
            this.m_client.Game.MapListSave += new FrostbiteClient.EmptyParamterHandler(m_prcClient_MapListSave);
            this.m_client.Game.MapListMapAppended += new FrostbiteClient.MapListAppendedHandler(m_prcClient_MapListMapAppended);
            this.m_client.Game.MapListNextLevelIndex += new FrostbiteClient.MapListLevelIndexHandler(m_prcClient_MapListNextLevelIndex);
            this.m_client.Game.MapListMapRemoved += new FrostbiteClient.MapListLevelIndexHandler(m_prcClient_MapListMapRemoved);
            this.m_client.Game.MapListMapInserted += new FrostbiteClient.MapListMapInsertedHandler(m_prcClient_MapListMapInserted);
            this.m_client.Game.MapListCleared += new FrostbiteClient.EmptyParamterHandler(m_prcClient_MapListCleared);
            this.m_client.Game.MapListListed += new FrostbiteClient.MapListListedHandler(m_prcClient_MapListListed);

            this.m_client.Game.TextChatModerationListAddPlayer += new FrostbiteClient.TextChatModerationListAddPlayerHandler(Game_TextChatModerationListAddPlayer);
            this.m_client.Game.TextChatModerationListRemovePlayer += new FrostbiteClient.TextChatModerationListRemovePlayerHandler(Game_TextChatModerationListRemovePlayer);
            this.m_client.Game.TextChatModerationListClear += new FrostbiteClient.EmptyParamterHandler(Game_TextChatModerationListClear);
            this.m_client.Game.TextChatModerationListSave += new FrostbiteClient.EmptyParamterHandler(Game_TextChatModerationListSave);
            this.m_client.Game.TextChatModerationListLoad += new FrostbiteClient.EmptyParamterHandler(Game_TextChatModerationListLoad);
            this.m_client.FullTextChatModerationListList += new PRoConClient.FullTextChatModerationListListHandler(m_client_FullTextChatModerationListList);

            this.m_client.Game.GamePassword += new FrostbiteClient.PasswordHandler(m_prcClient_GamePassword);
            this.m_client.Game.Punkbuster += new FrostbiteClient.IsEnabledHandler(m_prcClient_Punkbuster);
            this.m_client.Game.Hardcore += new FrostbiteClient.IsEnabledHandler(m_prcClient_Hardcore);
            this.m_client.Game.Ranked += new FrostbiteClient.IsEnabledHandler(m_prcClient_Ranked);
            this.m_client.Game.RankLimit += new FrostbiteClient.LimitHandler(m_prcClient_RankLimit);
            this.m_client.Game.TeamBalance += new FrostbiteClient.IsEnabledHandler(m_prcClient_TeamBalance);
            this.m_client.Game.FriendlyFire += new FrostbiteClient.IsEnabledHandler(m_prcClient_FriendlyFire);
            this.m_client.Game.MaxPlayerLimit += new FrostbiteClient.LimitHandler(m_prcClient_MaxPlayerLimit);
            this.m_client.Game.CurrentPlayerLimit += new FrostbiteClient.LimitHandler(m_prcClient_CurrentPlayerLimit);
            this.m_client.Game.PlayerLimit += new FrostbiteClient.LimitHandler(m_prcClient_PlayerLimit);
            this.m_client.Game.BannerUrl += new FrostbiteClient.BannerUrlHandler(m_prcClient_BannerUrl);
            this.m_client.Game.ServerDescription += new FrostbiteClient.ServerDescriptionHandler(m_prcClient_ServerDescription);
            this.m_client.Game.KillCam += new FrostbiteClient.IsEnabledHandler(m_prcClient_KillCam);
            this.m_client.Game.MiniMap += new FrostbiteClient.IsEnabledHandler(m_prcClient_MiniMap);
            this.m_client.Game.CrossHair += new FrostbiteClient.IsEnabledHandler(m_prcClient_CrossHair);
            this.m_client.Game.ThreeDSpotting += new FrostbiteClient.IsEnabledHandler(m_prcClient_ThreeDSpotting);
            this.m_client.Game.MiniMapSpotting += new FrostbiteClient.IsEnabledHandler(m_prcClient_MiniMapSpotting);
            this.m_client.Game.ThirdPersonVehicleCameras += new FrostbiteClient.IsEnabledHandler(m_prcClient_ThirdPersonVehicleCameras);

            this.m_client.Game.TextChatModerationMode += new FrostbiteClient.TextChatModerationModeHandler(Game_TextChatModerationMode);
            this.m_client.Game.TextChatSpamCoolDownTime += new FrostbiteClient.LimitHandler(Game_TextChatSpamCoolDownTime);
            this.m_client.Game.TextChatSpamDetectionTime += new FrostbiteClient.LimitHandler(Game_TextChatSpamDetectionTime);
            this.m_client.Game.TextChatSpamTriggerCount += new FrostbiteClient.LimitHandler(Game_TextChatSpamTriggerCount);

            // R13
            this.m_client.Game.ServerName += new FrostbiteClient.ServerNameHandler(m_prcClient_ServerName);
            this.m_client.Game.TeamKillCountForKick += new FrostbiteClient.LimitHandler(m_prcClient_TeamKillCountForKick);
            this.m_client.Game.TeamKillValueIncrease += new FrostbiteClient.LimitHandler(m_prcClient_TeamKillValueIncrease);
            this.m_client.Game.TeamKillValueDecreasePerSecond += new FrostbiteClient.LimitHandler(m_prcClient_TeamKillValueDecreasePerSecond);
            this.m_client.Game.TeamKillValueForKick += new FrostbiteClient.LimitHandler(m_prcClient_TeamKillValueForKick);
            this.m_client.Game.IdleTimeout += new FrostbiteClient.LimitHandler(m_prcClient_IdleTimeout);
            this.m_client.Game.ProfanityFilter += new FrostbiteClient.IsEnabledHandler(m_prcClient_ProfanityFilter);

            this.m_client.PlayerSpawned += new PRoConClient.PlayerSpawnedHandler(m_prcClient_PlayerSpawned);
            this.m_client.Game.RoundOver += new FrostbiteClient.RoundOverHandler(m_prcClient_RoundOver);
            this.m_client.Game.RoundOverPlayers += new FrostbiteClient.RoundOverPlayersHandler(m_prcClient_RoundOverPlayers);
            this.m_client.Game.RoundOverTeamScores += new FrostbiteClient.RoundOverTeamScoresHandler(m_prcClient_RoundOverTeamScores);
            this.m_client.Game.EndRound += new FrostbiteClient.EndRoundHandler(m_prcClient_EndRound);

            this.m_client.Game.LevelVariablesGet += new FrostbiteClient.LevelVariableGetHandler(m_prcClient_LevelVariablesGet);
            this.m_client.Game.LevelVariablesSet += new FrostbiteClient.LevelVariableHandler(m_prcClient_LevelVariablesSet);
            this.m_client.Game.LevelVariablesClear += new FrostbiteClient.LevelVariableHandler(m_prcClient_LevelVariablesClear);
            this.m_client.Game.LevelVariablesEvaluate += new FrostbiteClient.LevelVariableGetHandler(m_prcClient_LevelVariablesEvaluate);
            this.m_client.Game.LevelVariablesList += new FrostbiteClient.LevelVariableListHandler(m_prcClient_LevelVariablesList);

            this.m_client.ReceiveProconVariable += new PRoConClient.ReceiveProconVariableHandler(m_prcClient_ReceiveProconVariable);

            this.m_client.PunkbusterBeginPlayerInfo += new PRoConClient.EmptyParamterHandler(m_client_PunkbusterBeginPlayerInfo);
            this.m_client.PunkbusterEndPlayerInfo += new PRoConClient.EmptyParamterHandler(m_client_PunkbusterEndPlayerInfo);
            this.m_client.PunkbusterPlayerInfo += new PRoConClient.PunkbusterPlayerInfoHandler(m_prcClient_PunkbusterPlayerInfo);
            this.m_client.PunkbusterPlayerBanned += new PRoConClient.PunkbusterBanHandler(m_prcClient_PunkbusterPlayerBanned);
            this.m_client.PunkbusterPlayerUnbanned += new PRoConClient.PunkbusterBanHandler(m_prcClient_PunkbusterPlayerUnbanned);

            this.m_client.Layer.AccountPrivileges.AccountPrivilegeAdded += new AccountPrivilegeDictionary.AccountPrivilegeAlteredHandler(AccountPrivileges_AccountPrivilegeAdded);
            this.m_client.Layer.AccountPrivileges.AccountPrivilegeRemoved += new AccountPrivilegeDictionary.AccountPrivilegeAlteredHandler(AccountPrivileges_AccountPrivilegeRemoved);

            this.m_client.MapGeometry.MapZoneTrespassed += new PRoCon.Core.Battlemap.MapGeometry.MapZoneTrespassedHandler(MapGeometry_MapZoneTrespassed);

            #region Admin actions on players

            this.m_client.Game.PlayerKickedByAdmin += new FrostbiteClient.PlayerKickedHandler(Game_PlayerKickedByAdmin);
            this.m_client.Game.PlayerKilledByAdmin += new FrostbiteClient.PlayerKilledByAdminHandler(Game_PlayerKilledByAdmin);
            this.m_client.Game.PlayerMovedByAdmin += new FrostbiteClient.PlayerMovedByAdminHandler(Game_PlayerMovedByAdmin);

            #endregion

            #region Layer Accounts

            foreach (PRoConLayerClient client in this.m_client.Layer.LayerClients) {
                client.Login += new PRoConLayerClient.LayerClientHandler(client_LayerClientLogin);
                client.Logout += new PRoConLayerClient.LayerClientHandler(client_LayerClientLogout);
            }
            this.m_client.Layer.ClientConnected += new Remote.Layer.PRoConLayer.LayerAccountHandler(Layer_ClientConnected);

            #endregion
        }

        #endregion

        #region Events

        #region Layer Accounts

        private void Layer_ClientConnected(Remote.Layer.PRoConLayerClient client) {
            client.Login += new Remote.Layer.PRoConLayerClient.LayerClientHandler(client_LayerClientLogin);
            client.Logout += new Remote.Layer.PRoConLayerClient.LayerClientHandler(client_LayerClientLogout);
        }

        private void client_LayerClientLogout(Remote.Layer.PRoConLayerClient sender) {
            this.InvokeOnAllEnabled("OnAccountLogout", sender.Username, sender.IPPort, sender.Privileges);
        }

        private void client_LayerClientLogin(Remote.Layer.PRoConLayerClient sender) {
            this.InvokeOnAllEnabled("OnAccountLogin", sender.Username, sender.IPPort, sender.Privileges);
        }

        #endregion

        private void AccountPrivileges_AccountPrivilegeRemoved(AccountPrivilege item) {
            this.InvokeOnAllLoaded("OnAccountDeleted", item.Owner.Name);

            item.AccountPrivilegesChanged -= new AccountPrivilege.AccountPrivilegesChangedHandler(item_AccountPrivilegesChanged);
        }

        private void AccountPrivileges_AccountPrivilegeAdded(AccountPrivilege item) {
            item.AccountPrivilegesChanged += new AccountPrivilege.AccountPrivilegesChangedHandler(item_AccountPrivilegesChanged);

            this.InvokeOnAllLoaded("OnAccountCreated", item.Owner.Name);
        }

        void item_AccountPrivilegesChanged(AccountPrivilege item) {
            this.InvokeOnAllLoaded("OnAccountPrivilegesUpdate", item.Owner.Name, item.Privileges);
        }

        void m_prcClient_PunkbusterPlayerUnbanned(PRoConClient sender, CBanInfo cbiUnbannedPlayer) {
            this.InvokeOnAllEnabled("OnPunkbusterUnbanInfo", cbiUnbannedPlayer);
        }

        void m_prcClient_PunkbusterPlayerBanned(PRoConClient sender, CBanInfo cbiBannedPlayer) {
            this.InvokeOnAllEnabled("OnPunkbusterBanInfo", cbiBannedPlayer);
        }

        private void m_prcClient_PunkbusterPlayerInfo(PRoConClient sender, CPunkbusterInfo pbInfo) {
            this.InvokeOnAllEnabled("OnPunkbusterPlayerInfo", pbInfo);
        }

        private void m_client_PunkbusterEndPlayerInfo(PRoConClient sender) {
            this.InvokeOnAllEnabled("OnPunkbusterBeginPlayerInfo");
        }
        
        private void m_client_PunkbusterBeginPlayerInfo(PRoConClient sender) {
            this.InvokeOnAllEnabled("OnPunkbusterEndPlayerInfo");
        }

        private void m_prcClient_ReceiveProconVariable(PRoConClient sender, string strVariable, string strValue) {
            this.InvokeOnAllEnabled("OnReceiveProconVariable", strVariable, strValue);
        }

        private void m_prcClient_CommandLogin(PRoConClient sender) {
            this.InvokeOnAllEnabled("OnLogin");
        }

        private void m_prcClient_CommandLogout(PRoConClient sender) {
            this.InvokeOnAllEnabled("OnLogout");
        }

        private void m_prcClient_CommandQuit(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnQuit");
        }

        private void m_prcClient_ConnectionClosed(PRoConClient sender) {
            this.InvokeOnAllEnabled("OnConnectionClosed");
        }

        private void m_prcClient_PlayerJoin(FrostbiteClient sender, string playerName) {
            this.InvokeOnAllEnabled("OnPlayerJoin", playerName);
        }

        private void m_prcClient_PlayerLeft(FrostbiteClient sender, string playerName, PRoCon.Core.Players.CPlayerInfo cpiPlayer) {
            if (cpiPlayer != null) {
                this.InvokeOnAllEnabled("OnPlayerLeft", cpiPlayer);
            }

            // DEPRECATED
            this.InvokeOnAllEnabled("OnPlayerLeft", playerName);
        }

        private void m_prcClient_PlayerAuthenticated(FrostbiteClient sender, string playerName, string playerGuid) {
            this.InvokeOnAllEnabled("OnPlayerAuthenticated", playerName, playerGuid);
        }

        private void m_prcClient_PlayerKicked(FrostbiteClient sender, string strSoldierName, string strReason) {
            this.InvokeOnAllEnabled("OnPlayerKicked", strSoldierName, strReason);
        }

        private void m_prcClient_PlayerChangedTeam(FrostbiteClient sender, string strSoldierName, int iTeamID, int iSquadID) {
            this.InvokeOnAllEnabled("OnPlayerTeamChange", strSoldierName, iTeamID, iSquadID);
        }

        private void m_prcClient_PlayerChangedSquad(FrostbiteClient sender, string strSoldierName, int iTeamID, int iSquadID) {
            this.InvokeOnAllEnabled("OnPlayerSquadChange", strSoldierName, iTeamID, iSquadID);
        }

        private void m_prcClient_PlayerKilled(PRoConClient sender, Kill kKillerVictimDetails) {
            this.InvokeOnAllEnabled("OnPlayerKilled", new object[] { kKillerVictimDetails });

            // DEPRECATED
            this.InvokeOnAllEnabled("OnPlayerKilled", new object[] { kKillerVictimDetails.Killer.SoldierName, kKillerVictimDetails.Victim.SoldierName });
        }

        /// <summary>
        /// This will check from the dictionary of registered commands to see if some text is matched
        /// against a registered command.  The return is prioritized for whatever command matches more
        /// arguments.
        /// </summary>
        /// <param name="playerName">Who executed the command</param>
        /// <param name="message">The message they sent</param>
        private bool CheckInGameCommands(string playerName, string message, out MatchCommand mtcMatchedCommand, out CapturedCommand capReturnCommand) {

            bool isMatch = false;
            capReturnCommand = null;
            mtcMatchedCommand = null;

            lock (this.m_objMatchedInGameCommandsLocker) {
                
                CapturedCommand capMatched = null;

                // If this player has a command stored that requires confirmation.
                if (this.CommandsNeedingConfirmation.Contains(playerName) == true) {
                    if ((capMatched = this.CommandsNeedingConfirmation[playerName].MatchedCommand.Requirements.ConfirmationCommand.Matches(message)) != null) {
                        //capReturnCommand = capMatched;
                        capReturnCommand = this.CommandsNeedingConfirmation[playerName].ConfirmationDetails;
                        mtcMatchedCommand = this.CommandsNeedingConfirmation[playerName].MatchedCommand;
                        capReturnCommand.IsConfirmed = true;
                        isMatch = true;
                    }
                }

                // If it was not a confirmation to a previously matched command.
                if (isMatch == false) {

                    foreach (KeyValuePair<string, MatchCommand> kvpCommand in this.MatchedInGameCommands) {

                        // Only care if the plugin is enabled.
                        if (this.Plugins.IsEnabled(kvpCommand.Value.RegisteredClassname) == true) {

                            capMatched = kvpCommand.Value.Matches(message);

                            if (capMatched != null) {

                                if (kvpCommand.Value.Requirements.HasValidPermissions(this.m_client.GetAccountPrivileges(playerName)) == true) {

                                    // if (this.ValidateRequirements(playerName, kvpCommand.Value.Requirements) == true) {

                                    // If it's the first match we've found
                                    if (capReturnCommand == null) {
                                        capReturnCommand = capMatched;
                                        mtcMatchedCommand = kvpCommand.Value;
                                        isMatch = true;
                                    }
                                    else if (capReturnCommand != null && capMatched.CompareTo(capReturnCommand) > 0) {
                                        // We've found a command with that is a closer match to its arguments
                                        capReturnCommand = capMatched;
                                        mtcMatchedCommand = kvpCommand.Value;
                                        isMatch = true;
                                    }
                                    
                                        /*
                                    // If we've found a better match than before (more arguments matched)
                                    else if (capReturnCommand != null && capMatched.MatchedArguments.Count > capReturnCommand.MatchedArguments.Count) {
                                        capReturnCommand = capMatched;
                                        mtcMatchedCommand = kvpCommand.Value;
                                        isMatch = true;
                                    }
                                    // If we've found another match, check if this one is "matchier" (has a lower score)
                                    else if (capReturnCommand != null && capMatched.MatchedArguments.Count == capReturnCommand.MatchedArguments.Count && capMatched.AggregateMatchScore < capReturnCommand.AggregateMatchScore) {
                                        // We've found a command with the same amount of matched data but the new command is closer to it's own dictionary.
                                        capReturnCommand = capMatched;
                                        mtcMatchedCommand = kvpCommand.Value;
                                        isMatch = true;
                                    }*/
                                }
                                else {
                                    this.m_client.Game.SendAdminSayPacket(kvpCommand.Value.Requirements.FailedRequirementsMessage, new CPlayerSubset(CPlayerSubset.PlayerSubsetType.Player, playerName));
                                    // this.m_prcClient.SendRequest(new List<string>() { "admin.say", kvpCommand.Value.Requirements.FailedRequirementsMessage, "player", playerName });
                                }
                            }
                        }
                    }
                }
            }

            return isMatch;
        }

        private void DispatchMatchedCommand(string playerName, string message, MatchCommand mtcCommand, CapturedCommand capCommand, CPlayerSubset subset) {

            bool isConfirmationRequired = false;

            if (capCommand.IsConfirmed == false) {
                foreach (MatchArgument mtcArgument in capCommand.MatchedArguments) {
                    if (mtcArgument.MatchScore > mtcCommand.Requirements.MinimumMatchSimilarity) {
                        isConfirmationRequired = true;
                        capCommand.IsConfirmed = false;
                        break;
                    }
                }
            }

            if (isConfirmationRequired == true && capCommand.IsConfirmed == false) {
                if (this.CommandsNeedingConfirmation.Contains(playerName) == true) {
                    this.CommandsNeedingConfirmation.Remove(playerName);
                }

                this.CommandsNeedingConfirmation.Add(new ConfirmationEntry(playerName, message, mtcCommand, capCommand, subset));

                this.m_client.Game.SendAdminSayPacket(String.Format("Did you mean {0}?", capCommand.ToString()), new CPlayerSubset(CPlayerSubset.PlayerSubsetType.Player, playerName));
                // this.m_prcClient.SendRequest(new List<string>() { "admin.say", String.Format("Did you mean {0}?", capCommand.ToString()), "player", playerName });
            }
            else {
                this.InvokeOnEnabled(mtcCommand.RegisteredClassname, mtcCommand.RegisteredMethodName, playerName, message, mtcCommand, capCommand, new CPlayerSubset(CPlayerSubset.PlayerSubsetType.All));

                this.InvokeOnAllEnabled("OnAnyMatchRegisteredCommand", playerName, message, mtcCommand, capCommand, new CPlayerSubset(CPlayerSubset.PlayerSubsetType.All));
            }
        }

        private void m_prcClient_GlobalChat(FrostbiteClient sender, string playerName, string message) {
            this.InvokeOnAllEnabled("OnGlobalChat", playerName, message);

            CapturedCommand capCommand = null;
            MatchCommand mtcCommand = null;

            if (String.Compare(playerName, "server", true) != 0 && this.CheckInGameCommands(playerName, message, out mtcCommand, out capCommand) == true) {
                this.DispatchMatchedCommand(playerName, message, mtcCommand, capCommand, new CPlayerSubset(CPlayerSubset.PlayerSubsetType.All));
                //this.InvokeOnEnabled(mtcCommand.RegisteredClassname, "OnMatchRegisteredCommand", playerName, message, mtcCommand, capCommand, new CPlayerSubset(CPlayerSubset.PlayerSubsetType.All));
            }
        }

        private void m_prcClient_TeamChat(FrostbiteClient sender, string playerName, string message, int teamId) {
            this.InvokeOnAllEnabled("OnTeamChat", playerName, message, teamId);

            CapturedCommand capCommand = null;
            MatchCommand mtcCommand = null;

            if (String.Compare(playerName, "server", true) != 0 && this.CheckInGameCommands(playerName, message, out mtcCommand, out capCommand) == true) {
                this.DispatchMatchedCommand(playerName, message, mtcCommand, capCommand, new CPlayerSubset(CPlayerSubset.PlayerSubsetType.Team, teamId));
                //this.InvokeOnEnabled(mtcCommand.RegisteredClassname, "OnMatchRegisteredCommand", playerName, message, mtcCommand, capCommand, new CPlayerSubset(CPlayerSubset.PlayerSubsetType.Team, teamId));
            }
        }

        private void m_prcClient_SquadChat(FrostbiteClient sender, string playerName, string message, int teamId, int squadId) {
            this.InvokeOnAllEnabled("OnSquadChat", playerName, message, teamId, squadId);

            CapturedCommand capCommand = null;
            MatchCommand mtcCommand = null;

            if (String.Compare(playerName, "server", true) != 0 && this.CheckInGameCommands(playerName, message, out mtcCommand, out capCommand) == true) {
                this.DispatchMatchedCommand(playerName, message, mtcCommand, capCommand, new CPlayerSubset(CPlayerSubset.PlayerSubsetType.Squad, teamId, squadId));
                //this.InvokeOnEnabled(mtcCommand.RegisteredClassname, "OnMatchRegisteredCommand", playerName, message, mtcCommand, capCommand, new CPlayerSubset(CPlayerSubset.PlayerSubsetType.Squad, teamId, squadId));
            }
        }

        private void m_prcClient_ResponseError(FrostbiteClient sender, Packet originalRequest, string errorMessage) {
            this.InvokeOnAllEnabled("OnResponseError", new List<string>(originalRequest.Words), errorMessage);
        }

        private void m_prcClient_Version(FrostbiteClient sender, string serverType, string serverVersion) {
            this.InvokeOnAllEnabled("OnVersion", serverType, serverVersion);
        }

        private void m_prcClient_Help(FrostbiteClient sender, List<string> lstCommands) {
            this.InvokeOnAllEnabled("OnHelp", lstCommands);
        }

        private void m_prcClient_RunScript(FrostbiteClient sender, string scriptFileName) {
            this.InvokeOnAllEnabled("OnRunScript", scriptFileName);
        }

        private void m_prcClient_RunScriptError(FrostbiteClient sender, string strScriptFileName, int iLineError, string strErrorDescription) {
            this.InvokeOnAllEnabled("OnRunScriptError", strScriptFileName, iLineError, strErrorDescription);
        }

        private void m_prcClient_PunkbusterMessage(FrostbiteClient sender, string punkbusterMessage) {
            this.InvokeOnAllEnabled("OnPunkbusterMessage", punkbusterMessage);
        }

        private void m_prcClient_LoadingLevel(FrostbiteClient sender, string mapFileName, int roundsPlayed, int roundsTotal) {

            this.InvokeOnAllEnabled("OnLoadingLevel", mapFileName, roundsPlayed, roundsTotal);

            // DEPRECATED
            this.InvokeOnAllEnabled("OnLoadingLevel", mapFileName);
        }

        private void m_prcClient_LevelStarted(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnLevelStarted");
        }

        private void m_prcClient_ServerInfo(FrostbiteClient sender, CServerInfo csiServerInfo) {
            this.InvokeOnAllEnabled("OnServerInfo", csiServerInfo);
        }

        private void m_prcClient_Yelling(FrostbiteClient sender, string strMessage, int iMessageDuration, List<string> lstSubsetWords) {
            this.InvokeOnAllEnabled("OnYelling", strMessage, iMessageDuration, new CPlayerSubset(lstSubsetWords));
        }

        private void m_prcClient_Saying(FrostbiteClient sender, string strMessage, List<string> lstSubsetWords) {
            this.InvokeOnAllEnabled("OnSaying", strMessage, new CPlayerSubset(lstSubsetWords));
        }

        private void m_prcClient_RunNextLevel(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnRunNextLevel");
        }

        private void m_prcClient_CurrentLevel(FrostbiteClient sender, string currentLevel) {
            this.InvokeOnAllEnabled("OnCurrentLevel", currentLevel);
        }

        private void m_prcClient_RestartLevel(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnRestartLevel");
        }

        private void m_prcClient_SupportedMaps(FrostbiteClient sender, string strPlaylist, List<string> lstSupportedMaps) {
            this.InvokeOnAllEnabled("OnSupportedMaps", strPlaylist, lstSupportedMaps);
        }

        private void m_prcClient_PlaylistSet(FrostbiteClient sender, string playlist) {
            this.InvokeOnAllEnabled("OnPlaylistSet", playlist);
        }

        private void m_prcClient_ListPlaylists(FrostbiteClient sender, List<string> lstPlaylists) {
            this.InvokeOnAllEnabled("OnListPlaylists", lstPlaylists);
        }

        private void m_prcClient_ListPlayers(FrostbiteClient sender, List<PRoCon.Core.Players.CPlayerInfo> lstPlayers, CPlayerSubset cpsSubset) {
            this.InvokeOnAllEnabled("OnListPlayers", lstPlayers, cpsSubset);
        }

        #region Banlist

        private void m_prcClient_BanListLoad(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnBanListLoad");
        }

        private void m_prcClient_BanListSave(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnBanListSave");
        }

        private void m_prcClient_BanListAdd(FrostbiteClient sender, CBanInfo cbiAddedBan) {
            this.InvokeOnAllEnabled("OnBanAdded", cbiAddedBan);
        }

        private void m_prcClient_BanListRemove(FrostbiteClient sender, CBanInfo cbiRemovedBan) {
            this.InvokeOnAllEnabled("OnBanRemoved", cbiRemovedBan);
        }

        private void m_prcClient_BanListClear(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnBanListClear");
        }

        private void m_prcClient_BanListList(PRoConClient sender, List<CBanInfo> lstBans) {
            this.InvokeOnAllEnabled("OnBanList", lstBans);
        }

        #endregion

        #region Text Chat Moderation

        private void m_client_FullTextChatModerationListList(PRoConClient sender, TextChatModeration.TextChatModerationDictionary moderationList) {
            this.InvokeOnAllEnabled("OnTextChatModerationList", moderationList);
        }

        private void Game_TextChatModerationListLoad(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnTextChatModerationLoad");
        }

        private void Game_TextChatModerationListClear(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnTextChatModerationClear");
        }

        private void Game_TextChatModerationListSave(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnTextChatModerationSave");
        }

        private void Game_TextChatModerationListRemovePlayer(FrostbiteClient sender, TextChatModeration.TextChatModerationEntry playerEntry) {
            this.InvokeOnAllEnabled("OnTextChatModerationRemovePlayer", playerEntry);
        }

        private void Game_TextChatModerationListAddPlayer(FrostbiteClient sender, TextChatModeration.TextChatModerationEntry playerEntry) {
            this.InvokeOnAllEnabled("OnTextChatModerationAddPlayer", playerEntry);
        }

        #endregion


        private void m_prcClient_ReservedSlotsConfigFile(FrostbiteClient sender, string configFilename) {
            this.InvokeOnAllEnabled("OnReservedSlotsConfigFile", configFilename);
        }

        private void m_prcClient_ReservedSlotsLoad(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnReservedSlotsLoad");
        }

        private void m_prcClient_ReservedSlotsSave(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnReservedSlotsSave");
        }

        private void m_prcClient_ReservedSlotsPlayerAdded(FrostbiteClient sender, string strSoldierName) {
            this.InvokeOnAllEnabled("OnReservedSlotsPlayerAdded", strSoldierName);
        }

        private void m_prcClient_ReservedSlotsPlayerRemoved(FrostbiteClient sender, string strSoldierName) {
            this.InvokeOnAllEnabled("OnReservedSlotsPlayerRemoved", strSoldierName);
        }

        private void m_prcClient_ReservedSlotsCleared(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnReservedSlotsCleared");
        }

        private void m_prcClient_ReservedSlotsList(FrostbiteClient sender, List<string> soldierNames) {
            this.InvokeOnAllEnabled("OnReservedSlotsList", soldierNames);
        }

        private void m_prcClient_MapListConfigFile(FrostbiteClient sender, string strConfigFilename) {
            this.InvokeOnAllEnabled("OnMaplistConfigFile", strConfigFilename);
        }

        private void m_prcClient_MapListLoad(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnMaplistLoad");
        }

        private void m_prcClient_MapListSave(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnMaplistSave");
        }

        private void m_prcClient_MapListMapAppended(FrostbiteClient sender, MaplistEntry mapEntry) {
            this.InvokeOnAllEnabled("OnMaplistMapAppended", mapEntry.MapFileName);
        }

        private void m_prcClient_MapListNextLevelIndex(FrostbiteClient sender, int mapIndex) {
            this.InvokeOnAllEnabled("OnMaplistNextLevelIndex", mapIndex);
        }

        private void m_prcClient_MapListMapRemoved(FrostbiteClient sender, int mapIndex) {
            this.InvokeOnAllEnabled("OnMaplistMapRemoved", mapIndex);
        }

        private void m_prcClient_MapListMapInserted(FrostbiteClient sender, int mapIndex, string mapFileName, int rounds) {
            this.InvokeOnAllEnabled("OnMaplistMapInserted", mapIndex, mapFileName, rounds);
        }

        private void m_prcClient_MapListCleared(FrostbiteClient sender) {
            this.InvokeOnAllEnabled("OnMaplistCleared");
        }

        private void m_prcClient_MapListListed(FrostbiteClient sender, List<MaplistEntry> lstMaplist) {

            this.InvokeOnAllEnabled("OnMaplistList", lstMaplist);

            // DEPRECATED
            List<string> lstMapFileNames = new List<string>();
            foreach (MaplistEntry mleEntry in lstMaplist) {
                lstMapFileNames.Add(mleEntry.MapFileName);
            }

            // DEPRECATED
            this.InvokeOnAllEnabled("OnMaplistList", lstMapFileNames);
        }

        private void m_prcClient_GamePassword(FrostbiteClient sender, string password) {
            this.InvokeOnAllEnabled("OnGamePassword", password);
        }

        private void m_prcClient_Punkbuster(FrostbiteClient sender, bool isEnabled) {
            this.InvokeOnAllEnabled("OnPunkbuster", isEnabled);
        }

        private void m_prcClient_Hardcore(FrostbiteClient sender, bool isEnabled) {
            this.InvokeOnAllEnabled("OnHardcore", isEnabled);
        }

        private void m_prcClient_Ranked(FrostbiteClient sender, bool isEnabled) {
            this.InvokeOnAllEnabled("OnRanked", isEnabled);
        }

        private void m_prcClient_RankLimit(FrostbiteClient sender, int limit) {
            this.InvokeOnAllEnabled("OnRankLimit", limit);
        }

        private void m_prcClient_TeamBalance(FrostbiteClient sender, bool isEnabled) {
            this.InvokeOnAllEnabled("OnTeamBalance", isEnabled);
        }

        private void m_prcClient_FriendlyFire(FrostbiteClient sender, bool isEnabled) {
            this.InvokeOnAllEnabled("OnFriendlyFire", isEnabled);
        }

        private void m_prcClient_MaxPlayerLimit(FrostbiteClient sender, int limit) {
            this.InvokeOnAllEnabled("OnMaxPlayerLimit", limit);
        }

        private void m_prcClient_CurrentPlayerLimit(FrostbiteClient sender, int limit) {
            this.InvokeOnAllEnabled("OnCurrentPlayerLimit", limit);
        }

        private void m_prcClient_PlayerLimit(FrostbiteClient sender, int limit) {
            this.InvokeOnAllEnabled("OnPlayerLimit", limit);
        }

        private void m_prcClient_BannerUrl(FrostbiteClient sender, string url) {
            this.InvokeOnAllEnabled("OnBannerURL", url);
        }

        private void m_prcClient_ServerDescription(FrostbiteClient sender, string serverDescription) {
            this.InvokeOnAllEnabled("OnServerDescription", serverDescription);
        }

        private void m_prcClient_KillCam(FrostbiteClient sender, bool isEnabled) {
            this.InvokeOnAllEnabled("OnKillCam", isEnabled);
        }

        private void m_prcClient_MiniMap(FrostbiteClient sender, bool isEnabled) {
            this.InvokeOnAllEnabled("OnMiniMap", isEnabled);
        }

        private void m_prcClient_CrossHair(FrostbiteClient sender, bool isEnabled) {
            this.InvokeOnAllEnabled("OnCrossHair", isEnabled);
        }

        private void m_prcClient_ThreeDSpotting(FrostbiteClient sender, bool isEnabled) {
            this.InvokeOnAllEnabled("On3dSpotting", isEnabled);
        }

        private void m_prcClient_MiniMapSpotting(FrostbiteClient sender, bool isEnabled) {
            this.InvokeOnAllEnabled("OnMiniMapSpotting", isEnabled);
        }

        private void m_prcClient_ThirdPersonVehicleCameras(FrostbiteClient sender, bool isEnabled) {
            this.InvokeOnAllEnabled("OnThirdPersonVehicleCameras", isEnabled);
        }

        private void m_prcClient_ProfanityFilter(FrostbiteClient sender, bool isEnabled) {
            this.InvokeOnAllEnabled("OnProfanityFilter", new object[] { isEnabled });
        }

        private void m_prcClient_IdleTimeout(FrostbiteClient sender, int limit) {
            this.InvokeOnAllEnabled("OnIdleTimeout", new object[] { limit });
        }

        private void m_prcClient_TeamKillValueForKick(FrostbiteClient sender, int limit) {
            this.InvokeOnAllEnabled("OnTeamKillValueForKick", new object[] { limit });
        }

        private void m_prcClient_TeamKillValueDecreasePerSecond(FrostbiteClient sender, int limit) {
            this.InvokeOnAllEnabled("OnTeamKillValueDecreasePerSecond", new object[] { limit });
        }

        private void m_prcClient_TeamKillValueIncrease(FrostbiteClient sender, int limit) {
            this.InvokeOnAllEnabled("OnTeamKillValueIncrease", new object[] { limit });
        }

        private void m_prcClient_TeamKillCountForKick(FrostbiteClient sender, int limit) {
            this.InvokeOnAllEnabled("OnTeamKillCountForKick", new object[] { limit });
        }

        #region Text Chat Moderation Settings

        private void Game_TextChatSpamTriggerCount(FrostbiteClient sender, int limit) {
            this.InvokeOnAllEnabled("OnTextChatSpamTriggerCount", limit);
        }

        private void Game_TextChatSpamDetectionTime(FrostbiteClient sender, int limit) {
            this.InvokeOnAllEnabled("OnTextChatSpamDetectionTime", limit);
        }

        private void Game_TextChatSpamCoolDownTime(FrostbiteClient sender, int limit) {
            this.InvokeOnAllEnabled("OnTextChatSpamCoolDownTime", limit);
        }

        private void Game_TextChatModerationMode(FrostbiteClient sender, TextChatModeration.ServerModerationModeType mode) {
            this.InvokeOnAllEnabled("OnTextChatModerationMode", mode);
        }

        #endregion

        private void m_prcClient_ServerName(FrostbiteClient sender, string strServerName) {
            this.InvokeOnAllEnabled("OnServerName", new object[] { strServerName });
        }

        private void m_prcClient_EndRound(FrostbiteClient sender, int iWinningTeamID) {
            this.InvokeOnAllEnabled("OnEndRound", new object[] { iWinningTeamID });
        }

        private void m_prcClient_RoundOverTeamScores(FrostbiteClient sender, List<TeamScore> lstTeamScores) {
            this.InvokeOnAllEnabled("OnRoundOverTeamScores", new object[] { lstTeamScores });
        }

        private void m_prcClient_RoundOverPlayers(FrostbiteClient sender, List<CPlayerInfo> lstPlayers) {
            this.InvokeOnAllEnabled("OnRoundOverPlayers", new object[] { lstPlayers });
        }

        private void m_prcClient_RoundOver(FrostbiteClient sender, int iWinningTeamID) {
            this.InvokeOnAllEnabled("OnRoundOver", new object[] { iWinningTeamID });
        }

        private void m_prcClient_PlayerSpawned(PRoConClient sender, string soldierName, Inventory spawnedInventory) {
            this.InvokeOnAllEnabled("OnPlayerSpawned", new object[] { soldierName, spawnedInventory });
        }

        private void m_prcClient_LevelVariablesList(FrostbiteClient sender, LevelVariable lvRequestedContext, List<LevelVariable> lstReturnedValues) {
            this.InvokeOnAllEnabled("OnLevelVariablesList", new object[] { lvRequestedContext, lstReturnedValues });
        }

        private void m_prcClient_LevelVariablesEvaluate(FrostbiteClient sender, LevelVariable lvRequestedContext, LevelVariable lvReturnedValue) {
            this.InvokeOnAllEnabled("OnLevelVariablesEvaluate", new object[] { lvRequestedContext, lvReturnedValue });
        }

        private void m_prcClient_LevelVariablesClear(FrostbiteClient sender, LevelVariable lvRequestedContext) {
            this.InvokeOnAllEnabled("OnLevelVariablesClear", new object[] { lvRequestedContext });
        }

        private void m_prcClient_LevelVariablesSet(FrostbiteClient sender, LevelVariable lvRequestedContext) {
            this.InvokeOnAllEnabled("OnLevelVariablesSet", new object[] { lvRequestedContext });
        }

        private void m_prcClient_LevelVariablesGet(FrostbiteClient sender, LevelVariable lvRequestedContext, LevelVariable lvReturnedValue) {
            this.InvokeOnAllEnabled("OnLevelVariablesGet", new object[] { lvRequestedContext, lvReturnedValue });
        }

        private void MapGeometry_MapZoneTrespassed(CPlayerInfo cpiSoldier, PRoCon.Core.Battlemap.ZoneAction action, PRoCon.Core.Battlemap.MapZone sender, Point3D pntTresspassLocation, float flTresspassPercentage, object trespassState) {
            this.InvokeOnAllEnabled("OnZoneTrespass", new object[] { cpiSoldier, action, sender, pntTresspassLocation, flTresspassPercentage, trespassState });
        }

        #region Admin actions on players

        private void Game_PlayerKilledByAdmin(FrostbiteClient sender, string soldierName) {
            this.InvokeOnAllEnabled("OnPlayerKilledByAdmin", soldierName);
        }

        private void Game_PlayerKickedByAdmin(FrostbiteClient sender, string strSoldierName, string strReason) {
            this.InvokeOnAllEnabled("OnPlayerKickedByAdmin", strSoldierName, strReason);
        }

        private void Game_PlayerMovedByAdmin(FrostbiteClient sender, string soldierName, int destinationTeamId, int destinationSquadId, bool forceKilled) {
            this.InvokeOnAllEnabled("OnPlayerMovedByAdmin", soldierName, destinationTeamId, destinationSquadId, forceKilled);
        }

        #endregion

        #endregion
    }
}
