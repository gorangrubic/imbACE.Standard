// --------------------------------------------------------------------------------------------------------------------
// <copyright file="run.cs" company="imbVeles" >
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
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using imbSCI.Data.enums;

    #region imbVeles using

    using System;
    using imbACE.Services.platform.windows;
    using imbACE.Core.core.diagnostic;
    using imbSCI.Core.files;
    using imbACE.Core.core;

    #endregion imbVeles using

    /// <summary>
    /// Klasa za pokretanje eksternih aplikacija
    /// </summary>
    public static class run
    {
        ///// <summary>
        ///// Izvrsice fajl/pokrenuce program koji mu je zadat u putanji
        ///// </summary>
        ///// <param name="_path">Putanja prema programu / fajlu koji treba da pokrene</param>
        //public static Process startApplication(String _path, Boolean UseSchellExecute = true)
        //{
        //    if (_path == "") return null;
        //    Process myProcess = new Process();
        //    myProcess.StartInfo.UseShellExecute = true;
        //    myProcess.StartInfo.FileName = _path;
        //    myProcess.Start();
        //    displayManager.deployDisplayModeToProcess(imbSettingsManager.current.displayExternalTool, myProcess);
        //    return myProcess;
        //}

        public static Dictionary<externalTool, Process> activeTools = new Dictionary<externalTool, Process>();

        /// <summary>
        /// Vraca preporucenu ekstenziju za file koji se otvara alatom
        /// </summary>
        /// <param name="tool"></param>
        /// <returns></returns>
        public static String getOutputExtensionFor(this externalTool tool)
        {
            switch (tool)
            {
                case externalTool.chrome:
                case externalTool.explorer:
                case externalTool.firefox:
                    return "html";
                    break;

                default:
                    return "txt";
                    break;
            }
        }

        /// <summary>
        /// Pokrece fajl/aplikaciju
        /// </summary>
        /// <param name="_path"></param>
        /// <param name="_lineArguments"></param>
        public static Process startApplication(String _path, String _lineArguments = "", Boolean UseSchellExecute = true,
                                               String workingDirectory = "")
        {
            throw new NotImplementedException();

            if (_path == "") return null;
            Process myProcess = new Process();
            myProcess.StartInfo.UseShellExecute = UseSchellExecute;
            myProcess.StartInfo.FileName = _path;

            if (!UseSchellExecute)
            {
                if (String.IsNullOrEmpty(workingDirectory))
                {
                    myProcess.StartInfo.WorkingDirectory = Directory.GetCurrentDirectory();
                }
                else
                {
                    myProcess.StartInfo.WorkingDirectory = workingDirectory;
                }
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo.RedirectStandardError = true;
                //myProcess.StartInfo.RedirectStandardInput = true;
            }
            if (!String.IsNullOrEmpty(_lineArguments)) myProcess.StartInfo.Arguments = _lineArguments;
            try
            {
                myProcess.Start();
                // displayManager.deployDisplayModeToProcess(imbSettingsManager.current.displayExternalTool, myProcess);
            }
            catch (Exception ex)
            {
                logSystem.log("Failed to start process> " + ex.Message, logType.ExecutionError, false, null);
            }

            return myProcess;
        }

        /// <summary>
        /// Snima prosledjen sadrzaj i otvara ga u odabranoj aplikaciji
        /// </summary>
        /// <param name="whatApplication">Koja aplikacija</param>
        /// <param name="content">String sadrzaj koji treba da otvori</param>
        /// <param name="contentName">Filename ili cela putanja</param>
        /// <returns></returns>
        public static Process openWithApplication(externalTool whatApplication, String content,
                                                  String contentName = "tempfile")
        {
            String p = "";
            String e = whatApplication.getOutputExtensionFor();

            if (Path.IsPathRooted(contentName))
            {
                p = contentName;
            }
            else
            {
                //p = content   fileOps.getOutputPath(contentName, e, e,
                //                                                 imbTimeStampFormat.imbExceptionStamp, "",
                //                                                  imbCore.environment.files.enums.outputFolder.reports);
            }

            throw new NotImplementedException();

            //objectSerialization

            //save.saveToFile(p, content);
            return startApplication(whatApplication, p);
        }

        /// <summary>
        /// Pokrece eksterni alat i u njemu otvara prosledjenu putanju
        /// </summary>
        /// <param name="filePathOrUrl"></param>
        /// <param name="whatApplication"></param>
        public static Process startApplication(externalTool whatApplication, String filePathOrUrl = "")
        {
            Process myProcess = null;
            String path = "";
            try
            {
                // path = imbSettingsManager.current.externalToolsPath[whatApplication];
            }
            catch
            {
            }

            switch (whatApplication)
            {
                case externalTool.phpMyAdmin:
                    myProcess = startApplication(path, " \"http://localhost/phpmyadmin/\" --single-process ");
                    break;

                case externalTool.firefox:
                    //  filePathOrUrl = filePathOrUrl.Replace("/", "\\");

                    myProcess = startApplication(path, " -url " + filePathOrUrl);
                    break;

                case externalTool.explorer:
                    filePathOrUrl = filePathOrUrl.Replace("/", "\\");
                    myProcess = startApplication(path, " -url " + filePathOrUrl);
                    break;

                case externalTool.chrome:
                    filePathOrUrl = filePathOrUrl.Replace("/", "\\");
                    myProcess = startApplication(path, " \"" + filePathOrUrl + "\" --single-process ");
                    break;

                case externalTool.wampLocalhost:
                    myProcess = startApplication(path, " \"http://localhost/\" --single-process ");
                    break;

                case externalTool.notepadpp:
                    myProcess = startApplication(path, " -multiInst " + filePathOrUrl);
                    break;

                case externalTool.dreamweaver:
                    myProcess = startApplication(path, " \"" + filePathOrUrl + "\"");
                    break;

                case externalTool.wamp:
                    myProcess = startApplication(path);
                    break;

                case externalTool.projectFolder:
                    // startApplication("explorer.exe", Path.GetPathRoot(imbCoreManager.projectSource.projectPath));
                    break;

                case externalTool.runtimeFolder:
                    //myProcess = startApplication("explorer.exe",
                    //                             Path.GetPathRoot(imbCoreApplicationSettings.installedPath));
                    break;

                case externalTool.wampWWW:
                    myProcess = startApplication("explorer.exe", "C:\\wamp\\www");
                    break;

                case externalTool.fusekiServer:
                    //myProcess = startApplication(path, "");
                    myProcess = startApplication(path, "--update --mem /ds");
                    break;

                case externalTool.fusekiServerJava:
                    myProcess = startApplication("java", "-jar fuseki-server.jar");
                    break;

                case externalTool.folderExplorer:
                    myProcess = startApplication("explorer.exe", Path.GetPathRoot(filePathOrUrl));
                    break;

                case externalTool.fusekiLocalhost:
                    myProcess = startApplication(@"C:\Program Files (x86)\Mozilla Firefox\firefox.exe",
                                                 @" -url http://localhost:3030/");
                    break;
            }
            throw new NotImplementedException();
            //if (imbCoreApplicationSettings.doKillExisting)
            //{
            //    if (activeTools.ContainsKey(whatApplication))
            //    {
            //        activeTools[whatApplication].Kill();
            //        activeTools.Remove(whatApplication);
            //    }
            //    activeTools.Add(whatApplication, myProcess);
            //}
            return myProcess;
        }
    }
}