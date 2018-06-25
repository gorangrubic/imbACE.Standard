// --------------------------------------------------------------------------------------------------------------------
// <copyright file="externalToolExtensions.cs" company="imbVeles" >
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
// Project: imbACE.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbACE.Core.extensions.io
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.enums;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    ///
    /// </summary>
    public static class externalToolExtensions
    {
        //public static Dictionary<externalTool, String> externalToolsPath = new Dictionary<externalTool, string>();

        #region --- externalToolsPath ------- External tool vs path collection

        private static Dictionary<externalTool, string> _externalToolsPath;

        /// <summary>
        /// External tool vs path collection
        /// </summary>
        public static Dictionary<externalTool, string> externalToolsPath
        {
            get
            {
                if (_externalToolsPath == null)
                {
                    _externalToolsPath = new Dictionary<externalTool, string>();
                    externalToolsPath.Clear();
                    externalToolsPath.Add(externalTool.chrome,
                        @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe");
                    externalToolsPath.Add(externalTool.dreamweaver,
                        @"C:\Program Files (x86)\Adobe\Adobe Dreamweaver CS4\Dreamweaver.exe");
                    externalToolsPath.Add(externalTool.explorer,
                        @"C:\Program Files (x86)\Internet Explorer\iexplore.exe");
                    externalToolsPath.Add(externalTool.wamp, @"c:\wamp\wampmanager.exe");
                    externalToolsPath.Add(externalTool.firefox, @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe");
                    externalToolsPath.Add(externalTool.notepadpp, @"C:\Program Files (x86)\Notepad++\notepad++.exe");

                    externalToolsPath.Add(externalTool.graphVizDot, @"E:\imbVelesFramework\IncludedBin\GraphVizExe\dot.exe");
                    externalToolsPath.Add(externalTool.fusekiServer, @"C:\wamp\bin\fuseki\fuseki-server.bat");
                    externalToolsPath.Add(externalTool.fusekiServerJava, @"C:\wamp\bin\fuseki\fuseki-server.jar");
                }
                return _externalToolsPath;
            }
        }

        #endregion --- externalToolsPath ------- External tool vs path collection

        /// <summary>
        /// The active tools
        /// </summary>
        public static Dictionary<externalTool, Process> activeTools = new Dictionary<externalTool, Process>();

        /// <summary>
        /// Pokrece fajl/aplikaciju
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_lineArguments"></param>
        private static Process runProcess(String _path, String _lineArguments = "", Boolean UseSchellExecute = true, String workingDirectory = "")
        {
            if (_path == "") return null;

            if (!File.Exists(_path))
            {
                throw new aceGeneralException("There is no tool on path> " + _path);
            }

            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = UseSchellExecute;
            myProcess.StartInfo.FileName = _path;

            if (!UseSchellExecute)
            {
                myProcess.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();

                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo.RedirectStandardError = true;
            }

            if (!String.IsNullOrEmpty(_lineArguments)) myProcess.StartInfo.Arguments = _lineArguments;
            try
            {
                myProcess.Start();
            }
            catch (Exception ex)
            {
                throw new aceGeneralException("Failed to start process> " + ex.Message, ex);
            }

            return myProcess;
        }

        public static Boolean doKillExisting = true;

        /// <summary>
        /// Opens file specified with an <see cref="externalTool"/>
        /// </summary>
        /// <param name="fi">The fi.</param>
        /// <param name="whatApplication">The what application.</param>
        /// <returns></returns>
        public static Process run(this FileInfo fi, externalTool whatApplication)
        {
            return run(whatApplication, fi.FullName);
        }

        /// <summary>
        /// Pokrece eksterni alat i u njemu otvara prosledjenu putanju
        /// </summary>
        /// <param name="filePathOrUrl"></param>
        /// <param name="whatApplication"></param>
        public static Process run(this externalTool whatApplication, String filePathOrUrl = "")
        {
            Process myProcess = null;
            String path = "";
            try
            {
                path = externalToolsPath[whatApplication];
            }
            catch
            {
            }

            switch (whatApplication)
            {
                case externalTool.firefox:
                    //  filePathOrUrl = filePathOrUrl.Replace("/", "\\");

                    myProcess = runProcess(path, " -url " + filePathOrUrl);
                    break;

                case externalTool.explorer:
                    filePathOrUrl = filePathOrUrl.Replace("/", "\\");
                    myProcess = runProcess(path, " -url " + filePathOrUrl);
                    break;

                case externalTool.chrome:
                    filePathOrUrl = filePathOrUrl.Replace("/", "\\");
                    myProcess = runProcess(path, " \"" + filePathOrUrl + "\" --single-process ");
                    break;

                case externalTool.notepadpp:
                    myProcess = runProcess(path, " -multiInst " + filePathOrUrl);
                    break;

                case externalTool.projectFolder:
                    // startApplication("explorer.exe", Path.GetPathRoot(imbCoreManager.projectSource.projectPath));
                    break;

                case externalTool.runtimeFolder:
                    myProcess = runProcess("explorer.exe",
                        Path.GetPathRoot(Directory.GetCurrentDirectory()));
                    break;
            }

            if (doKillExisting)
            {
                if (activeTools.ContainsKey(whatApplication))
                {
                    try
                    {
                        activeTools[whatApplication].Kill();
                        activeTools.Remove(whatApplication);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                activeTools.Add(whatApplication, myProcess);
            }
            return myProcess;
        }
    }
}