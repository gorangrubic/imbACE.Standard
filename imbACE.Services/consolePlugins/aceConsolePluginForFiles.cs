// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceConsolePluginForFiles.cs" company="imbVeles" >
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

namespace imbACE.Services.consolePlugins
{
    using imbACE.Core.core;
    using imbACE.Core.operations;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.files.folders;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Base class for ACE Console plugin (<see cref="aceConsolePluginBase"/>) for files operation
    /// </summary>
    /// <seealso cref="aceConsolePluginBase" />
    public abstract class aceConsolePluginForFiles : aceConsolePluginBase
    {
        public aceConsolePluginForFiles(string __name, string __help = "", builderForLog __output = null) : base(__name, __help, __output)
        {
            folder = new DirectoryInfo(Directory.GetCurrentDirectory());
        }

        public aceConsolePluginForFiles(IAceOperationSetExecutor __parent, string __name, string __help = "") : base(__parent, __name, __help)
        {
            folder = new DirectoryInfo(Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// Currently selected folder
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        protected folderNode folder { get; set; }

        protected List<FileInfo> selectedFiles { get; set; } = new List<FileInfo>();

        [Display(Description = "Navigates the plugin to the specified path.", GroupName = "path", Name = "Select", ShortName = "cd",
                // Prompt = "Prompt message",
                Order = 10)]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "It will execute specified path and set resulting directory to folder property of the plugin")]
        /// <summary>Navigates the plugin to the specified path.</summary>
        /// <remarks><para>It will execute specified path and set resulting directory to folder property of the plugin</para></remarks>
        /// <param name="path">path to havigate from current directory location - supports typical DOS/Linux/WindowsCMD expressions for absolute and relative `cd` commands</param>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_pathSelect(
            [Description("path to havigate from current directory location - supports typical DOS/Linux/WindowsCMD expressions for absolute and relative `cd` commands")] String path = "..")
        {
            //response.log("Select with path=" + path + ", steps =" + steps + ", debug=" + debug + ".");

            DirectoryInfo di = folder;
            folder = di.CreateSubdirectory(path);
        }

        [Display(Description = "Showing content of the current location", GroupName = "path", Name = "List", ShortName = "dir",
                // Prompt = "Prompt message",
                Order = 10)]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "Lists all files and directories of the current folder, like ls or dir command")]
        /// <summary>"Showing content of the current location</summary>
        /// <remarks><para>Lists all files and directories of the current folder, like ls or dir command</para></remarks>
        /// <param name="compactView">if TRUE it will show file list in a compact manner</param><param name="steps">--</param><param name="debug">--</param>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_pathList(
            [Description("if TRUE it will show file list in a compact manner")] Boolean compactView = false)
        {
            DirectoryInfo di = folder;
            response.AppendLine("Path: _" + di.FullName + "_");

            if (di.Parent != null)
            {
                response.AppendLine("..");
            }
            var dirs = di.GetDirectories();
            foreach (var dir in dirs)
            {
                response.AppendLine("_/" + dir.Name + "_");
            }
            var files = di.GetFiles();
            foreach (var dir in files)
            {
                response.AppendLine("" + dir.Name + "");
            }
        }

        [Display(Description = "Sets the current list of selected files", GroupName = "select", Name = "Files",
                // Prompt = "Prompt message",
                Order = 10)]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "It will search for matches in current or other specified path and set the result into current list")]
        /// <summary>Sets the current list of selected files</summary>
        /// <remarks><para>It will search for matches in current or other specified path and set the result into current list</para></remarks>
        /// <param name="search">Search pattern to be used for file selection</param><param name="path">path to search with the pattern specified. By default uses the current folder location</param><param name="subdirs">if TRUE it will search subdirectories too</param>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_selectFiles(
            [Description("Search pattern to be used for file selection")] String search = "*.xml",
            [Description("path to search with the pattern specified. By default uses the current folder location")]
        String path="",
            [Description("if TRUE it will search subdirectories too")] Boolean subdirs = true)
        {
            if (path.isNullOrEmpty()) path = folder.path;
            DirectoryInfo di = new DirectoryInfo(path);
            SearchOption op = SearchOption.TopDirectoryOnly;
            if (subdirs) op = SearchOption.AllDirectories;

            var files = di.GetFiles(search, op);
            selectedFiles = new List<FileInfo>();
            selectedFiles.AddRange(files);
            foreach (FileInfo item in selectedFiles)
            {
                response.AppendLine(item.FullName);
            }
            output.log("Files selected [" + selectedFiles.Count() + "]");
        }
    }
}