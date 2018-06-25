// --------------------------------------------------------------------------------------------------------------------
// <copyright file="freeRunnerModule.cs" company="imbVeles" >
//
// Copyright (C) 2017 imbVeles
//
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Project: imbACE.Services
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbACE.Services.process
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Xml.Serialization;
    using imbSCI.Core.attributes;
    using imbSCI.Data.data;
    using imbSCI.Data.enums;

    #region imbVeles using

    using System;
    using imbACE.Core.core;
    using imbACE.Core.core.diagnostic;
    using imbSCI.Core.extensions.data;

    #endregion imbVeles using

    /// <summary>
    /// ContextualModule [freeRunnerModule] -
    /// </summary>
    /// <remarks>
    /// </remarks>
    [imb(imbAttributeName.menuIcon, "bigb01")]
    [imb(imbAttributeName.menuHelp, "Starting custom process")]
    [imb(imbAttributeName.menuGroupPath, "Basic")]
    //[imb(imbAttributeName.diagnosticMode, "")] // AKTIVIRA dijagnostiku
    public class freeRunnerModule : dataBindableBase
    {
        #region Event Handlers: ProcessFeedback - Aktivira se kada dodje do bilo koje promene u statusu procesa - tj kada dodje ErrorOutput ili StandardOutput

        /// <summary>
        /// Event invoker za ProcessFeedback
        /// </summary>
        /// <remarks>
        /// Aktivira se kada dodje do bilo koje promene u statusu procesa - tj kada dodje ErrorOutput ili StandardOutput
        /// Ovaj method pozvati kada treba da se pokrene izvrsavanje dogadjaja
        /// </remarks>
        /// <example>
        /// Primer poziva ka izvrsenju eventa - bez argumenata
        /// <code>
        /// freeRunnerModule.callProcessFeedback();
        /// </code>
        /// </example>
        /// <example>
        /// Primer poziva ka izvrsenju eventa - sa argumentom
        /// <code>
        /// freeRunnerModule.callProcessFeedback(this, null);
        /// </code>
        /// </example>
        /// <param name="sender">Objekat koji je pozvao izvrsavanje</param>
        /// <param name="e">Argumenti dogadjaja</param>
        public void callProcessFeedback(String logMessage = "", logType msgType = logType.ExecutionError)
        {
            if (onProcessFeedback != null) onProcessFeedback(new imbConsoleLog(logMessage, msgType));
        }

        /// <summary>
        /// Event handler za ProcessFeedback
        /// </summary>
        /// <remarks>
        /// Aktivira se kada dodje do bilo koje promene u statusu procesa - tj kada dodje ErrorOutput ili StandardOutput
        /// Na ovaj property vezati sve dogadjaje koji treba da se izvrse
        /// </remarks>
        /// <example>
        /// Primer vezivanja hendlera:
        /// <code>
        /// freeRunnerModule.onProcessFeedback += new RoutedEventHandler(nazivMetoda);
        /// </code>
        /// </example>
        public event logEvent onProcessFeedback;

        #endregion Event Handlers: ProcessFeedback - Aktivira se kada dodje do bilo koje promene u statusu procesa - tj kada dodje ErrorOutput ili StandardOutput

        #region ------- operationMenuSystem

        /// <summary>
        /// ENUM za sve dostupne operacije
        /// </summary>
        [imb(imbAttributeName.menuIcon, "bigb01")]
        [imb(imbAttributeName.menuCommandTitle, "Free Runner")]
        public enum freeRunnerOperation
        {
            [imb(imbAttributeName.menuIcon, "bigb01")] //filename icon slicice, ne treba .png
            [imb(imbAttributeName.menuPriority, "50")] [imb(imbAttributeName.menuCommandTitle, "Run process")] [imb(imbAttributeName.menuHelp, "Runs a process from the settings")] run,

            [imb(imbAttributeName.menuIcon, "big32")] //filename icon slicice, ne treba .png
            [imb(imbAttributeName.menuPriority, "45")] [imb(imbAttributeName.menuCommandTitle, "Terminate the process")] [imb(imbAttributeName.menuHelp, "Terminates the active process")] killActiveProcess,

            [imb(imbAttributeName.menuIcon, "big08")] //filename icon slicice, ne treba .png
            [imb(imbAttributeName.menuPriority, "43")] [imb(imbAttributeName.menuCommandTitle, "Check the process")] [imb(imbAttributeName.menuHelp, "Terminates the active process")] checkActiveProcess,

            [imb(imbAttributeName.menuIcon, "big5b")] //filename icon slicice, ne treba .png
            [imb(imbAttributeName.menuPriority, "41")] [imb(imbAttributeName.menuCommandTitle, "Take selected process")] [imb(imbAttributeName.menuHelp, "Takes the process selected in monitoring grid - if any")] takeSelectedProcess,

            [imb(imbAttributeName.menuIcon, "big31")] //filename icon slicice, ne treba .png
            [imb(imbAttributeName.menuPriority, "42")] [imb(imbAttributeName.menuCommandTitle, "Browse")] [imb(imbAttributeName.menuHelp, "Browse executable file")] browseForFile,

            [imb(imbAttributeName.menuIcon, "bigb01")] //filename icon slicice, ne treba .png
            [imb(imbAttributeName.menuPriority, "40")] [imb(imbAttributeName.menuCommandTitle, "Run external tool")] [imb(imbAttributeName.menuHelp, "Runs external tool with its default settings")] externalToolToFreerunMode,

            [imb(imbAttributeName.menuIcon, "big09")] //filename icon slicice, ne treba .png
            [imb(imbAttributeName.menuPriority, "30")] [imb(imbAttributeName.menuCommandTitle, "Get C# line")] [imb(imbAttributeName.menuHelp, "Creates C# equivalend application call")] getCSharpLine,
        }

        /// <summary>
        /// Izvrsava operaciju nad ITEM-om
        /// </summary>
        /// <param name="toExecute">Operacija koja treba da se izvrsi</param>
        /// <param name="__item">Objekat koji je prosledjen prilikom poziva: moze biti editor ili objekat - ali treba da bude trenutno selektovani objekat</param>
        /// <param name="__collection">Kolekcija ciji je item clan</param>
        /// <param name="parentObject">Parent objekat u odnosu na selektovan item</param>
        public static void freeRunnerOperationExecute(freeRunnerOperation toExecute, freeRunnerModule __item,
                                                      ObservableCollection<freeRunnerModule> __collection,
                                                      Object parentObject)
        {
            freeRunnerModule _item = __item as freeRunnerModule;

            if (_item == null) _item = parentObject as freeRunnerModule;

            if (_item == null)
            {
                return;
            }
            String signature = "";
            String signatureTwo = "";
            String workdirectory = "";

            switch (toExecute)
            {
                case freeRunnerOperation.run:
                    _item.lastResponse = "";

                    if (_item.autoKillExistingProcess)
                    {
                        _item.freeRunnerOperationExecute(freeRunnerOperation.killActiveProcess);
                    }

                    switch (_item.toolToRun)
                    {
                        case externalTool.freeRunner:
                            _item.activeProcess = run.startApplication(_item.path, _item.arguments,
                                                                       _item.useSchellExecute, _item.homeDirectory);
                            break;

                        default:
                            _item.activeProcess = run.startApplication(_item.toolToRun, _item.arguments);
                            break;
                    }
                    if (_item.activeProcess != null)
                    {
                        _item.activeProcess.Exited += _item.activeProcess_Exited;
                        if (!_item.useSchellExecute)
                        {
                            switch (_item.StandardMode)
                            {
                                case freeRunnerLoging.disable:
                                    break;

                                default:
                                    try
                                    {
                                        _item.activeProcess.OutputDataReceived += _item.activeProcess_OutputDataReceived;
                                        _item.activeProcess.BeginOutputReadLine();
                                    }
                                    catch (Exception ex)
                                    {
                                        _item.lastResponse = ex.Message;
                                    }
                                    break;
                            }

                            switch (_item.ErrorMode)
                            {
                                case freeRunnerLoging.disable:
                                    break;

                                default:
                                    try
                                    {
                                        _item.activeProcess.ErrorDataReceived += _item.activeProcess_ErrorDataReceived;
                                        _item.activeProcess.BeginErrorReadLine();
                                    }
                                    catch (Exception ex)
                                    {
                                        _item.lastResponse = ex.Message;
                                    }
                                    break;
                            }

                            //_item.lastResponse = _item.activeProcess.StandardOutput
                        }
                        else
                        {
                            _item.lastResponse = "(turn off useSchellExecute to get StardardOutput results)";
                        }
                        signature = _item.activeProcess.ProcessName + " (" + _item.activeProcess.Id + ") ";
                        signatureTwo = " [" + _item.toolToRun.ToString() + "] path: " + _item.path + " with arguments: " +
                                       _item.arguments + " workdirectory: " + workdirectory;

                        logSystem.log("Process started: " + signature + signatureTwo, logType.Done);
                    }
                    else
                    {
                        logSystem.log("Process start failed " + signatureTwo, logType.ExecutionError);
                    }
                    break;

                case freeRunnerOperation.killActiveProcess:
                    if (_item.activeProcess != null)
                    {
                        signature = _item.activeProcess.ProcessName + " (" + _item.activeProcess.Id + ")";
                        if (_item.activeProcess.HasExited)
                        {
                            logSystem.log("Process already exited " + signature, logType.Done);
                        }
                        else
                        {
                            _item.activeProcess.Kill();
                            logSystem.log("Process killed " + signature, logType.Done);
                        }
                        _item.activeProcess.Dispose();
                    }
                    else
                    {
                        logSystem.log("No active process to kill.", logType.ExecutionError);
                    }
                    break;

                case freeRunnerOperation.takeSelectedProcess:
                    if (_item.selectedProcess != null)
                    {
                        _item.activeProcess = _item.selectedProcess;
                    }
                    break;

                case freeRunnerOperation.externalToolToFreerunMode:

                    _item.activeProcess = run.startApplication(_item.toolToRun);

                    break;

                case freeRunnerOperation.checkActiveProcess:

                    break;

                case freeRunnerOperation.getCSharpLine:
                    _item.lastResponse = "run.startApplication(\"" + _item.path + "\", \"" + _item.arguments + "\", " +
                                         _item.useSchellExecute.ToString() + ", \"" + _item.homeDirectory + "\");";

                    clipboard.setObject(_item.lastResponse);
                    break;

                case freeRunnerOperation.browseForFile:

                    throw new NotImplementedException();

                    //String selected = @select.selectFile("", fileDialog.open,
                    //                                     "Executable files (*.exe;*.jar;*.bat;*.dll;*.air;*.msi)|*.exe;*.jar;*.bat;*.dll;*.air;*.msi|All files (*.*)|*.*");
                    // if (!String.IsNullOrEmpty(selected)) _item.path = selected;
                    break;

                default:
                    logSystem.log("Operation execution not supported yed> " + toExecute.ToString(),
                                  logType.ExecutionError, true);
                    break;
            }

            __item.refresh();
        }

        private void activeProcess_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            dataReceivedHandler(e.Data, true);
        }

        public void activeProcess_Exited(object sender, EventArgs e)
        {
            refresh();
            //logSystem.log("Not implemented :: " + this.GetType().Name + " :: ", logType.FatalError);
        }

        private void activeProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            dataReceivedHandler(e.Data, false);
        }

        private void dataReceivedHandler(String message, Boolean isError)
        {
            freeRunnerLoging logMode = StandardMode;
            logType type = logType.Notification;

            if (isError)
            {
                logMode = ErrorMode;
                type = logType.ExecutionError;
            }

            switch (logMode)
            {
                case freeRunnerLoging.collectAndSendToLogSystem:
                    lastResponse += message + Environment.NewLine;
                    logSystem.log(message, type);
                    break;

                case freeRunnerLoging.collectToResponse:
                    lastResponse += message + Environment.NewLine;
                    break;

                case freeRunnerLoging.sendToLogSystem:
                    lastResponse = message;
                    logSystem.log(message, type);
                    break;

                case freeRunnerLoging.keepLastResponse:
                    lastResponse = message;
                    break;

                default:
                    break;
            }
            callProcessFeedback(message, type);
            refresh();
        }

        public void refresh()
        {
            if (activeProcess == null)
            {
                hasProcess = false;
            }
            else
            {
                try
                {
                    if (activeProcess.HasExited)
                    {
                        hasProcess = false;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(processNameFilter)) processNameFilter = activeProcess.ProcessName;
                        hasProcess = true;
                    }
                }
                catch (Exception ex)
                {
                    hasProcess = false;
                    activeProcess = null;
                }
            }

            // skeniranje procesa
            if (doProcessScan)
            {
                String filterName = "";

                if (doShowProcessWithSameName)
                {
                    filterName = processNameFilter;
                }
                try
                {
                    processes.Clear();
                    processes.AddRange(imbSystemInfo.getProcessList(filterName, showOnlyNewProcesses, creationTime));
                }
                catch (Exception ex)
                {
                }
            }
        }

        /// <summary>
        /// Izvrsava operaciju za ovu instancu
        /// </summary>
        public void freeRunnerOperationExecute(freeRunnerOperation toExecute)
        {
            freeRunnerModule.freeRunnerOperationExecute(toExecute, this, null, this);
        }

        #region -----------  useCustomWorkdirectory  -------  [Da li koristi custom work directory]

        /// <summary>
        /// Da li koristi custom work directory
        /// </summary>
        [XmlIgnore]
        public Boolean useCustomWorkdirectory
        {
            get { return ((!usePathFolderForHomeDirectory) && (!useSchellExecute)); }
        }

        #endregion -----------  useCustomWorkdirectory  -------  [Da li koristi custom work directory]

        #region -----------  usePathFolderForHomeDirectory  -------  [Da li da koristi path folder za radni direktorijum?]

        private Boolean _usePathFolderForHomeDirectory; // = new Boolean();

        /// <summary>
        /// Da li da koristi path folder za radni direktorijum?
        /// </summary>
        // [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("usePathFolderForHomeDirectory")]
        [Description("Da li da koristi path folder za radni direktorijum?")]
        public Boolean usePathFolderForHomeDirectory
        {
            get { return _usePathFolderForHomeDirectory; }
            set
            {
                // Boolean chg = (_usePathFolderForHomeDirectory != value);
                _usePathFolderForHomeDirectory = value;
                OnPropertyChanged("usePathFolderForHomeDirectory");
                OnPropertyChanged("useCustomWorkdirectory");
                OnPropertyChanged("homeDirectory");
                // if (chg) {}
            }
        }

        #endregion -----------  usePathFolderForHomeDirectory  -------  [Da li da koristi path folder za radni direktorijum?]

        #endregion ------- operationMenuSystem

        #region -----------  hasProcess  -------  [Proverava da li ima aktivan process]

        private Boolean _hasProcess; // = new Boolean();

        /// <summary>
        /// Proverava da li ima aktivan process
        /// </summary>
        [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("hasProcess")]
        [Description("Proverava da li ima aktivan process")]
        public Boolean hasProcess
        {
            get { return _hasProcess; }
            set
            {
                // Boolean chg = (_hasProcess != value);
                _hasProcess = value;
                OnPropertyChanged("hasProcess");
                // if (chg) {}
            }
        }

        #endregion -----------  hasProcess  -------  [Proverava da li ima aktivan process]

        #region -----------  homeDirectory  -------  [Path for home directory]

        private String _homeDirectory; // = new String();

        /// <summary>
        /// Path for home directory
        /// </summary>
        // [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("homeDirectory")]
        [Description("Path for home directory")]
        public String homeDirectory
        {
            get
            {
                if (String.IsNullOrEmpty(_homeDirectory)) _homeDirectory = Directory.GetCurrentDirectory();  // imbCoreApplicationSettings.installedPath;

                if (useCustomWorkdirectory)
                {
                    return _homeDirectory;
                }

                if (usePathFolderForHomeDirectory)
                {
                    _homeDirectory = Path.GetDirectoryName(path);
                    return _homeDirectory;
                }

                return _homeDirectory;
            }
            set
            {
                // Boolean chg = (_homeDirectory != value);
                if (useCustomWorkdirectory)
                {
                    _homeDirectory = value;
                    OnPropertyChanged("homeDirectory");
                }

                // if (chg) {}
            }
        }

        #endregion -----------  homeDirectory  -------  [Path for home directory]

        #region -----------  activeProcess  -------  [Aktivni proces]

        private Process _activeProcess; // = new String();

        /// <summary>
        /// Aktivni proces
        /// </summary>
        [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("activeProcess")]
        [Description("Aktivni proces")]
        public Process activeProcess
        {
            get { return _activeProcess; }
            set
            {
                // Boolean chg = (_activeProcess != value);
                _activeProcess = value;
                OnPropertyChanged("activeProcess");
                // if (chg) {}
            }
        }

        #endregion -----------  activeProcess  -------  [Aktivni proces]

        #region -----------  lastResponse  -------  [Poslednji odgovor usled izvrsavanja]

        private String _lastResponse; // = new String();

        /// <summary>
        /// Poslednji odgovor usled izvrsavanja
        /// </summary>
        [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("lastResponse")]
        [Description("Poslednji odgovor usled izvrsavanja")]
        [imb(imbAttributeName.diagnosticValue, "Diagnostic mode!")]
        public String lastResponse
        {
            get { return _lastResponse; }
            set
            {
                // Boolean chg = (_lastResponse != value);
                _lastResponse = value;
                OnPropertyChanged("lastResponse");
                // if (chg) {}
            }
        }

        #endregion -----------  lastResponse  -------  [Poslednji odgovor usled izvrsavanja]

        #region -----------  toolToRun  -------  [Kojim alatom da poziva izvrsenje]

        private externalTool _toolToRun = externalTool.freeRunner; // = new externalTool();

        /// <summary>
        /// Kojim alatom da poziva izvrsenje
        /// </summary>
        // [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("toolToRun")]
        [Description("Kojim alatom da poziva izvrsenje")]
        public externalTool toolToRun
        {
            get { return _toolToRun; }
            set
            {
                // Boolean chg = (_toolToRun != value);
                _toolToRun = value;
                OnPropertyChanged("toolToRun");
                // if (chg) {}
            }
        }

        #endregion -----------  toolToRun  -------  [Kojim alatom da poziva izvrsenje]

        #region -----------  useSchellExecute  -------  [Da li da prosledi schell execute]

        private Boolean _useSchellExecute; // = new Boolean();

        /// <summary>
        /// Da li da prosledi schell execute
        /// </summary>
        // [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("useSchellExecute")]
        [Description("Da li da prosledi schell execute")]
        [imb(imbAttributeName.diagnosticValue, "True")]
        public Boolean useSchellExecute
        {
            get { return _useSchellExecute; }
            set
            {
                // Boolean chg = (_useSchellExecute != value);
                _useSchellExecute = value;
                OnPropertyChanged("useSchellExecute");
                OnPropertyChanged("useCustomWorkdirectory");
                OnPropertyChanged("homeDirectory");
                // if (chg) {}
            }
        }

        #endregion -----------  useSchellExecute  -------  [Da li da prosledi schell execute]

        #region -----------  arguments  -------  [Arguments to pass]

        private String _arguments; // = new String();

        /// <summary>
        /// Arguments to pass
        /// </summary>
        // [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("arguments")]
        [Description("Arguments to pass")]
        [imb(imbAttributeName.diagnosticValue, @"-jar f:\fuseki\fuseki-server.jar --update --mem /ds")]
        public String arguments
        {
            get { return _arguments; }
            set
            {
                // Boolean chg = (_arguments != value);
                _arguments = value;
                OnPropertyChanged("arguments");
                // if (chg) {}
            }
        }

        #endregion -----------  arguments  -------  [Arguments to pass]

        #region -----------  path  -------  [Path to run]

        private String _path; // = new String();

        /// <summary>
        /// Path to run
        /// </summary>
        // [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("path")]
        [Description("Path to run")]
        [imb(imbAttributeName.diagnosticValue, "java.exe")]
        public String path
        {
            get { return _path; }
            set
            {
                // Boolean chg = (_path != value);
                _path = value;
                OnPropertyChanged("path");
                // if (chg) {}
            }
        }

        #endregion -----------  path  -------  [Path to run]

        #region -----------  autoKillExistingProcess  -------  [Da li automatski ukida prethodni process]

        private Boolean _autoKillExistingProcess = true; // = new Boolean();

        /// <summary>
        /// Da li automatski ukida prethodni process
        /// </summary>
        // [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("autoKillExistingProcess")]
        [Description("Da li automatski ukida prethodni process")]
        public Boolean autoKillExistingProcess
        {
            get { return _autoKillExistingProcess; }
            set
            {
                // Boolean chg = (_autoKillExistingProcess != value);
                _autoKillExistingProcess = value;
                OnPropertyChanged("autoKillExistingProcess");
                // if (chg) {}
            }
        }

        #endregion -----------  autoKillExistingProcess  -------  [Da li automatski ukida prethodni process]

        #region -----------  StandardMode  -------  [Nacin na koji upravlja standard outputom]

        private freeRunnerLoging _standardMode = freeRunnerLoging.collectToResponse; // = new freeRunnerLoging();

        /// <summary>
        /// Nacin na koji upravlja standard outputom
        /// </summary>
        // [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("StandardMode")]
        [Description("Nacin na koji upravlja standard outputom")]
        public freeRunnerLoging StandardMode
        {
            get { return _standardMode; }
            set
            {
                // Boolean chg = (_standardMode != value);
                _standardMode = value;
                OnPropertyChanged("StandardMode");
                // if (chg) {}
            }
        }

        #endregion -----------  StandardMode  -------  [Nacin na koji upravlja standard outputom]

        #region -----------  ErrorMode  -------  [Nacin na koji upravlja error outputom]

        private freeRunnerLoging _ErrorMode = freeRunnerLoging.collectAndSendToLogSystem; // = new freeRunnerLoging();

        /// <summary>
        /// Nacin na koji upravlja error outputom
        /// </summary>
        // [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("ErrorMode")]
        [Description("Nacin na koji upravlja error outputom")]
        public freeRunnerLoging ErrorMode
        {
            get { return _ErrorMode; }
            set
            {
                // Boolean chg = (_ErrorMode != value);
                _ErrorMode = value;
                OnPropertyChanged("ErrorMode");
                // if (chg) {}
            }
        }

        #endregion -----------  ErrorMode  -------  [Nacin na koji upravlja error outputom]

        #region -----------  creationTime  -------  [Vreme kada je instanciran ovaj modul - treba zbog pracenja procesa]

        private DateTime _creationTime = DateTime.Now;

        /// <summary>
        /// Vreme kada je instanciran ovaj modul - treba zbog pracenja procesa
        /// </summary>
        [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("creationTime")]
        [Description("Vreme kada je instanciran ovaj modul - treba zbog pracenja procesa")]
        public DateTime creationTime
        {
            get { return _creationTime; }
            set
            {
                // Boolean chg = (_creationTime != value);
                _creationTime = value;
                OnPropertyChanged("creationTime");
                // if (chg) {}
            }
        }

        #endregion -----------  creationTime  -------  [Vreme kada je instanciran ovaj modul - treba zbog pracenja procesa]

        #region -----------  showOnlyNewProcesses  -------  [Proces monitoring pokazuje samo nove procese]

        private Boolean _showOnlyNewProcesses = true; // = new Boolean();

        /// <summary>
        /// Proces monitoring pokazuje samo nove procese
        /// </summary>
        // [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("showOnlyNewProcesses")]
        [Description("Proces monitoring pokazuje samo nove procese")]
        public Boolean showOnlyNewProcesses
        {
            get { return _showOnlyNewProcesses; }
            set
            {
                // Boolean chg = (_showOnlyNewProcesses != value);
                _showOnlyNewProcesses = value;
                OnPropertyChanged("showOnlyNewProcesses");
                // if (chg) {}
            }
        }

        #endregion -----------  showOnlyNewProcesses  -------  [Proces monitoring pokazuje samo nove procese]

        #region -----------  processes  -------  [Lista svih procesa koje vidi]

        private ObservableCollection<Process> _processes = new ObservableCollection<Process>();

        /// <summary>
        /// Lista svih procesa koje vidi
        /// </summary>
        [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("processes")]
        [Description("Lista svih procesa koje vidi")]
        public ObservableCollection<Process> processes
        {
            get { return _processes; }
            set
            {
                // Boolean chg = (_processes != value);
                _processes = value;
                OnPropertyChanged("processes");
                // if (chg) {}
            }
        }

        #endregion -----------  processes  -------  [Lista svih procesa koje vidi]

        #region -----------  selectedProcess  -------  [Process koji je selektovan u datagridu]

        private Process _selectedProcess; // = new Process();

        /// <summary>
        /// Process koji je selektovan u datagridu
        /// </summary>
        [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("selectedProcess")]
        [Description("Process koji je selektovan u datagridu")]
        public Process selectedProcess
        {
            get { return _selectedProcess; }
            set
            {
                // Boolean chg = (_selectedProcess != value);
                _selectedProcess = value;
                OnPropertyChanged("selectedProcess");
                // if (chg) {}
            }
        }

        #endregion -----------  selectedProcess  -------  [Process koji je selektovan u datagridu]

        #region -----------  processNameFilter  -------  [Filter za pretragu procesa]

        private String _processNameFilter; // = new String();

        /// <summary>
        /// Filter za pretragu procesa
        /// </summary>
        // [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("processNameFilter")]
        [Description("Filter za pretragu procesa")]
        public String processNameFilter
        {
            get { return _processNameFilter; }
            set
            {
                // Boolean chg = (_processNameFilter != value);
                _processNameFilter = value;
                OnPropertyChanged("processNameFilter");
                // if (chg) {}
            }
        }

        #endregion -----------  processNameFilter  -------  [Filter za pretragu procesa]

        #region -----------  doProcessScan  -------  [Da li sprovodi skeniranje procesa]

        private Boolean _doProcessScan = false; // = new Boolean();

        /// <summary>
        /// Da li sprovodi skeniranje procesa
        /// </summary>
        // [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("doProcessScan")]
        [Description("Da li sprovodi skeniranje procesa")]
        public Boolean doProcessScan
        {
            get { return _doProcessScan; }
            set
            {
                // Boolean chg = (_doProcessScan != value);
                _doProcessScan = value;
                OnPropertyChanged("doProcessScan");
                // if (chg) {}
            }
        }

        #endregion -----------  doProcessScan  -------  [Da li sprovodi skeniranje procesa]

        #region -----------  doShowProcessWithSameName  -------  [Da li da prikazuje procese sa istim imenom - iako su kreirani ranije]

        private Boolean _doShowProcessWithSameName = true; // = new Boolean();

        /// <summary>
        /// Da li da prikazuje procese sa istim imenom - iako su kreirani ranije
        /// </summary>
        // [XmlIgnore]
        [Category("freeRunnerModule")]
        [DisplayName("doShowProcessWithSameName")]
        [Description("Da li da prikazuje procese sa istim imenom - iako su kreirani ranije")]
        public Boolean doShowProcessWithSameName
        {
            get { return _doShowProcessWithSameName; }
            set
            {
                // Boolean chg = (_doShowProcessWithSameName != value);
                _doShowProcessWithSameName = value;
                OnPropertyChanged("doShowProcessWithSameName");
                // if (chg) {}
            }
        }

        #endregion -----------  doShowProcessWithSameName  -------  [Da li da prikazuje procese sa istim imenom - iako su kreirani ranije]
    }
}