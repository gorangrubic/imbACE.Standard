// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceAdvancedConsoleWorkspace.cs" company="imbVeles" >
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
using imbACE.Core;
using imbSCI.Core;
using imbSCI.Core.attributes;
using imbSCI.Core.enums;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.interfaces;
using imbSCI.Data;
using imbSCI.Data.collection;
using imbSCI.Data.data;
using imbSCI.Data.interfaces;
using imbSCI.DataComplex;
using imbSCI.Reporting;
using imbSCI.Reporting.enums;
using imbSCI.Reporting.interfaces;

namespace imbACE.Services.console
{
    using imbACE.Core.commands.menu;
    using imbACE.Core.core;
    using imbACE.Core.core.exceptions;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.table;
    using imbSCI.Core.files;
    using imbSCI.Core.files.folders;
    using imbSCI.Core.files.unit;
    using imbSCI.Core.reporting;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.reporting;
    using imbSCI.DataComplex.extensions.data.formats;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Workspace helper object for aceAdvanced console
    /// </summary>
    /// <seealso cref="aceBindable" />
    public abstract class aceAdvancedConsoleWorkspace : aceBindable
    {
        private IAceAdvancedConsole _console;

        /// <summary> </summary>
        public IAceAdvancedConsole console
        {
            get
            {
                return _console;
            }
            protected set
            {
                _console = value;
                OnPropertyChanged("console");
            }
        }

        protected aceAdvancedConsoleWorkspace()
        {
        }

        /// <summary>
        /// It will run <see cref="deployWorkspace"/> after setting <see cref="console"/> reference
        /// </summary>
        /// <param name="__console">The console.</param>
        public aceAdvancedConsoleWorkspace(IAceAdvancedConsole __console)
        {
            console = __console;
            deployWorkspace();
        }

        private String _projectRootPath; // = "";

                                         /// <summary>
                                         /// Path to project's root folder - without \\ trail
                                         /// </summary>
        public String projectRootPath
        {
            get
            {
                //if (_projectRootPath.isNullOrEmpty())
                //{
                //}
                if (console != null)
                {
                    _projectRootPath = console.state.projects_path + "\\" + console.state.currentProjectName;
                }

                return _projectRootPath;
            }
        }

        /// <summary>
        /// Gets called during workspace construction, the method should initiate any additional subdirectories that console's project uses
        /// </summary>
        /// <remarks>
        /// <example>
        /// Place inside initiation of additional directories, required for your console's project class (inheriting: <see cref="aceAdvancedConsoleStateBase"/>)
        /// <code>
        /// <![CDATA[
        /// folderName = folder.Add(nameof(folderName), "Caption", "Description");
        /// ]]>
        /// </code>
        /// </example>
        /// </remarks>
        public abstract void setAdditionalWorkspaceFolders();

        /// <summary>
        /// Gets the new name of the project.
        /// </summary>
        /// <param name="proposal">The proposal.</param>
        /// <returns></returns>
        public String getNewProjectName(String proposal)
        {
            String path = _projectRootPath = console.state.projects_path + "\\" + proposal;
            String solution = proposal;
            Int32 i = 1;

            while (Directory.Exists(path))
            {
                solution = proposal + i.ToString("D3");
                i++;
                path = _projectRootPath = console.state.projects_path + "\\" + solution;
            }

            return solution;
        }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="sufix">The sufix.</param>
        /// <returns></returns>
        protected String getFilename(IObjectWithName instance, String sufix = "")
        {
            if (sufix.isNullOrEmpty()) sufix = instance.GetType().Name;
            return getFilename(instance.name, sufix);
        }

        /// <summary>
        /// Gets the filename.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="sufix">The sufix.</param>
        /// <returns></returns>
        protected String getFilename(String instance, String sufix = "")
        {
            String path = instance.add(sufix, "_").add("xml", ".");
            return folder[aceCCFolders.metadata].pathFor(path);
        }

        /// <summary>
        /// Saves the meta.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void saveMeta(IObjectWithName instance)
        {
            instance.saveObjectToXML(getFilename(instance));
        }

        public void Poke()
        {
        }

        public void saveData(DataSet instance, dataTableExportEnum descriptiveCopy = dataTableExportEnum.csv)
        {
            String filename = folder[aceCCFolders.metadata].pathFor(instance.FilenameForDataset());
            instance.saveObjectToXML(filename.ensureEndsWith(".xml"));

            if (descriptiveCopy != dataTableExportEnum.none)
            {
                instance.serializeDataSet(instance.FilenameForDataset(), folder[aceCCFolders.metadata], descriptiveCopy, console.application.appAboutInfo);
            }
        }

        /// <summary>
        /// Saves the data.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="descriptiveCopy">The descriptive copy.</param>
        public void saveData(DataTable instance, dataTableExportEnum descriptiveCopy = dataTableExportEnum.csv)
        {
            String filename = folder[aceCCFolders.metadata].pathFor(getFilename(instance.TableName, "table"));
            instance.saveObjectToXML(filename);

            if (descriptiveCopy != dataTableExportEnum.none)
            {
                instance.serializeDataTable(descriptiveCopy, instance.TableName, folder[aceCCFolders.metadata], console.application.appAboutInfo);
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public DataTable loadData(String tableName)
        {
            String filename = folder[aceCCFolders.metadata].pathFor(getFilename(tableName, "table"));
            return objectSerialization.loadObjectFromXML<DataTable>(filename);
        }

        /// <summary>
        /// Loads the meta.
        /// </summary>
        /// <param name="instanceName">Name of the instance.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IObjectWithName loadMeta(String instanceName, Type type)
        {
            String path = getFilename(instanceName, type.Name);
            return objectSerialization.loadObjectFromXML(path, type) as IObjectWithName;
        }

        public const string OUTPUT_PREFIX = "out_";

        /// <summary>
        /// Gets the new output.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="extension">The extension.</param>
        /// <returns></returns>
        public fileunit getNewOutput(String name = "", String extension = "txt")
        {
            name = Path.GetFileNameWithoutExtension(name);
            String proposal = folder[aceCCFolders.output].pathFor(OUTPUT_PREFIX.add(name, "_").add(extension, "."));

            FileInfo info = proposal.getWritableFile(getWritableFileMode.overwrite);

            return new fileunit(info.FullName);
        }

        public fileunit toNewOutput(String content, String name, String extension = "txt")
        {
            fileunit funit = getNewOutput(name, extension);
            funit.setContent(content);
            return funit;
        }

        public fileunit toNewOutput(List<String> content, String name, String extension = "txt")
        {
            fileunit funit = getNewOutput(name, extension);
            funit.setContentLines(content);
            return funit;
        }

        /// <summary>
        /// Saves the output.
        /// </summary>
        /// <param name="file">The file.</param>
        public void saveOutput(fileunit file)
        {
            String filename = Path.GetFileName(file.path);
            String path = folder[aceCCFolders.output].pathFor(filename);
            file.getContent().saveStringToFile(path, getWritableFileMode.autoRenameExistingOnOtherDate);
        }

        public fileunit loadInput(String filename)
        {
            String path = folder[aceCCFolders.input].pathFor(filename);
            var unit = new fileunit(path);
            loadedInputs.AddUnique(unit.info.FullName);
            return unit;
        }

        public fileunit loadInputNext(String pattern = "*.*", Int32 skip = 0)
        {
            DirectoryInfo di = folder[aceCCFolders.input];
            FileInfo[] fis = di.GetFiles(pattern);
            foreach (var fi in fis)
            {
                if (!loadedInputs.Contains(fi.FullName))
                {
                    if (skip > 0)
                    {
                        skip--;
                    }
                    else
                    {
                        return loadInput(fi.Name);
                    }
                }
            }
            return null;
        }

        public void resetLoadedInputs()
        {
            loadedInputs.Clear();
        }

        private List<String> _loadedInputs = new List<String>();

        /// <summary> </summary>
        public List<String> loadedInputs
        {
            get
            {
                return _loadedInputs;
            }
            protected set
            {
                _loadedInputs = value;
                OnPropertyChanged("loadedInputs");
            }
        }

        public List<String> getInputList(String pattern = "*.*", Boolean getAbsolutePaths = false)
        {
            DirectoryInfo di = folder[aceCCFolders.input];
            FileInfo[] fis = di.GetFiles(pattern);
            List<String> output = new List<string>();
            foreach (var fi in fis)
            {
                if (getAbsolutePaths)
                {
                    output.Add(fi.FullName);
                }
                else
                {
                    output.Add(fi.Name);
                }
            }

            return output;
        }

        public void saveLog(ILogBuilder loger, String name = "log")
        {
            String path = folder[aceCCFolders.logs].pathFor(name.add("md", "."));
            loger.ToString().saveStringToFile(path, getWritableFileMode.autoRenameExistingOnOtherDate);
        }

        public void saveOpLog(aceOperationArgs args)
        {
            String output = "Time: " + DateTime.Now.ToString();
            output = output.addLine(args.paramSet.ToString());
            output = output.addLine("-- RESPONSE --");
            output = output.addLine(console.output.getLastLine());
            output = output.addLine("-- RESPONSE --");
            output = output.addLine(console.response.getLastLine());

            String path = folder[aceCCFolders.logs].pathFor("Operation_" + args.method.Name + "_log".add("md", "."));
            output.saveStringToFile(path, getWritableFileMode.autoRenameExistingOnOtherDate);
        }

        /// <summary>
        /// Saves the script under specified filename (without extension, it will put the proper one automatically)
        /// </summary>
        /// <param name="scriptname">The scriptname.</param>
        /// <param name="content">The content.</param>
        public void saveScript(String scriptname, List<String> content)
        {
            String path = folder[aceCCFolders.scripts].pathFor(scriptname.ensureEndsWith(".ace"));
            String dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dir);
            saveBase.saveToFile(path, content);
        }

        /// <summary>
        /// Gets the script file information.
        /// </summary>
        /// <param name="scriptname">The scriptname.</param>
        /// <param name="autocreate">if set to <c>true</c> [autocreate].</param>
        /// <returns></returns>
        public FileInfo getScriptFileInfo(String scriptname, Boolean autocreate = true)
        {
            String filename = scriptname;
            filename = Path.GetFileNameWithoutExtension(filename);
            filename = filename.ensureEndsWith(".ace");
            String path = folder[aceCCFolders.scripts].pathFor(filename);

            if (!File.Exists(path))
            {
                if (autocreate)
                {
                    String dir = Path.GetDirectoryName(path);
                    Directory.CreateDirectory(dir);
                    saveScript(scriptname, new List<String>() { "// This is automatically created script", "// " + DateTime.Now.ToString(), "help" });
                }
            }

            return new FileInfo(path);
        }

        public aceConsoleScript loadScript(String scriptname)
        {
            String path = folder[aceCCFolders.scripts].pathFor(scriptname.ensureEndsWith(".ace"));

            if (!File.Exists(path))
            {
                String dir = Path.GetDirectoryName(path);
                Directory.CreateDirectory(dir);
                saveScript(scriptname, new List<String>() { "// This is automatically created script", "// " + DateTime.Now.ToString(), "help" });
            }
            return new aceConsoleScript(path, true); //openBase.openFile(path);
        }

        public List<String> getScriptList()
        {
            List<String> output = new List<string>();
            DirectoryInfo di = folder[aceCCFolders.scripts];

            var scriptFiles = di.GetFiles("*.ace");
            foreach (FileInfo sc in scriptFiles)
            {
                output.Add(sc.Name);
            }

            return output;
        }

        /// <summary>
        /// If override, call .base(false) at the end of your code. To add new folders better idea is to override <see cref="setAdditionalWorkspaceFolders"/>
        /// </summary>
        public virtual void deployWorkspace(Boolean resetFolder = true)
        {
            if (console == null) throw new aceGeneralException("AdvancedConsole not connected to the workspace", null, this, "Workspace not ready");

            Directory.CreateDirectory(console.state.projects_path);
            if (folder == null) resetFolder = true;

            if (resetFolder) folder = new folderStructure(projectRootPath, console.state.currentProjectName + " folder", "The root folder of " + console.consoleTitle + " project named " + console.state.currentProjectName + ".");

            folder.Add(aceCCFolders.cache, "Cache", "Temporal data and text cache storage");
            folder.Add(aceCCFolders.input, "Input", "Files and documents used as input for the console operations.");
            folder.Add(aceCCFolders.output, "Output", "Files and documents created by the console operations.");
            folder.Add(aceCCFolders.logs, "Logs", "Diagnostic records made during the console operations");
            folder.Add(aceCCFolders.metadata, "Metadata", "Serialized objects created and used by the console operations.");
            folder.Add(aceCCFolders.scripts, "Scripts", "Scripts that are executable on startup or via direct call to the console.");

            setAdditionalWorkspaceFolders();

            folder.generateReadmeFiles(console.application.appAboutInfo);
        }

        private folderStructure _folder;

        /// <summary> </summary>
        public folderStructure folder
        {
            get
            {
                return _folder;
            }
            protected set
            {
                _folder = value;
                OnPropertyChanged("folder");
            }
        }
    }
}