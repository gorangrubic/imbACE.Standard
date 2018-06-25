// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceConsoleScript.cs" company="imbVeles" >
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
    using imbACE.Core.commands;
    using imbACE.Core.core;
    using imbACE.Core.operations;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.files.unit;
    using imbSCI.Data.enums;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    /// <summary>
    /// Sequence of commands to be executed by <see cref="aceCommandConsole"/>
    /// </summary>
    /// <seealso cref="imbSCI.Core.files.unit.fileunit" />
    /// <seealso cref="System.Collections.Generic.IEnumerable{System.String}" />
    public class aceConsoleScript : fileunit, IAceConsoleScript
    {
        private Boolean _isReady = true;

        /// <summary> True if the script is ready for execution </summary>
        public Boolean isReady
        {
            get
            {
                return _isReady;
            }
            protected set
            {
                _isReady = value;
                OnPropertyChanged("isReady");
            }
        }

        public static implicit operator aceConsoleScript(List<String> source)
        {
            aceConsoleScript output = new aceConsoleScript("script_from_source.ace", false);
            output.Append(source, false);
            return output;
        }

        private aceConsoleScript _parent;

        /// <summary> </summary>
        public aceConsoleScript parent
        {
            get
            {
                return _parent;
            }
            protected set
            {
                _parent = value;
                OnPropertyChanged("parent");
            }
        }

        /// <summary>
        /// Calls the script execution abort. It can't abort already called command but will abort on the next.
        /// </summary>
        public void AbortExecution()
        {
            scriptExecutionAborted = true;
        }

        private Boolean scriptExecutionAborted = false;

        public Int32 currentLine { get; protected set; }

        /// <summary>
        /// Executes the specified console.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="__parent">The parent.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="skipLines">Number of lines to skip</param>
        public void Execute(aceCommandConsole console, aceConsoleScript __parent, Int32 delay = 1, Int32 skipLines = -1)
        {
            parent = __parent;

            executionStart = DateTime.Now;
            currentLine = 0;
            foreach (String line in this)
            {
                if (currentLine > skipLines)
                {
                    if (line.isNullOrEmptyString() || line == Environment.NewLine)
                    {
                        // skipping empty line
                    }
                    else
                    {
                        if (scriptExecutionAborted)
                        {
                            scriptExecutionAborted = false;
                            break;
                        }

                        switch (imbACECoreConfig.settings.doShowScriptLines)
                        {
                            case scriptLineShowMode.none:
                                break;

                            case scriptLineShowMode.undefined:
                                break;

                            case scriptLineShowMode.onlyCodeLine:
                                console.output.AppendLine(line);
                                break;

                            case scriptLineShowMode.fullPrefixAndCodeLine:
                                console.output.log("Script [" + currentLine.ToString("D3") + "]: " + line);
                                break;

                            case scriptLineShowMode.codeNumberAndCodeLine:
                                console.output.AppendLine("[" + currentLine.ToString("D3") + "] _" + line + "_");
                                break;
                        }

                        console.executeCommand(line);
                        Thread.Sleep(delay);
                    }
                }

                currentLine++;
            }
            executionFinish = DateTime.Now;
        }

        IAceConsoleScript IAceConsoleScript.GetScriptInForm(IAceCommandConsole console, commandLineFormat format, string newPath) => GetScriptInForm(console, format, newPath);

        /// <summary>
        /// Gets the script in another <see cref="commandLineFormat"/>
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="format">The format.</param>
        /// <param name="newPath">The new path.</param>
        /// <returns></returns>
        public aceConsoleScript GetScriptInForm(IAceCommandConsole console, commandLineFormat format, String newPath = null)
        {
            String p = newPath;
            if (p.isNullOrEmpty()) p = path;

            //if (File.Exists(p))
            aceConsoleScript output = new aceConsoleScript(p, false);

            foreach (String st in this)
            {
                aceCommandEntry entry = new aceCommandEntry(console, st);
                try
                {
                    if (!entry.isSyntaxError)
                    {
                        output.Append(entry.GetScriptLine(format));
                    }
                    else
                    {
                        output.Append(commandLineEntry.commentPrefix + " line: " + entry.inputLine + " - error: " + entry.errorMessage);
                    }
                }
                catch (Exception ex)
                {
                    output.Append("// error in line transformation: " + entry.inputLine);
                    output.Append("// " + ex.toStringSafe());
                    output.Append("// " + ex.Message);
                }
            }
            if (!newPath.isNullOrEmpty()) output.SaveAs(newPath, getWritableFileMode.overwrite, console.output);
            return output;
        }

        private DateTime _executionStart = new DateTime();

        /// <summary> </summary>
        public DateTime executionStart
        {
            get
            {
                return _executionStart;
            }
            protected set
            {
                _executionStart = value;
                OnPropertyChanged("executionStart");
            }
        }

        private DateTime _executionFinish = new DateTime();

        /// <summary> </summary>
        public DateTime executionFinish
        {
            get
            {
                return _executionFinish;
            }
            protected set
            {
                _executionFinish = value;
                OnPropertyChanged("executionFinish");
            }
        }

        /// <summary>
        /// New script
        /// </summary>
        /// <param name="__path">The path.</param>
        /// <param name="doPreload"></param>
        public aceConsoleScript(string __path, bool doPreload = true) : base(__path, doPreload)
        {
        }

        public IEnumerator<string> GetEnumerator()
        {
            return contentLines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return contentLines.GetEnumerator();
        }

        /// <summary>
        /// Creates another instance of script, using this as template
        /// </summary>
        /// <param name="pars">The pars.</param>
        public aceConsoleScript DeployTemplate(string[] pars, String newScriptName = "")
        {
            String newPath = "";
            if (newScriptName.isNullOrEmpty())
            {
                String filename = info.Name.removeEndsWith(".ace");
                FileInfo newFileInfo = filename.getWritableFile(getWritableFileMode.autoRenameThis);
                newPath = newFileInfo.FullName;
            }
            else
            {
                newPath = info.Directory.FullName.add(newScriptName.ensureEndsWith(".ace"));
            }

            aceConsoleScript output = new aceConsoleScript(newPath, false);
            String scriptCode = "";

            foreach (String line in this)
            {
                scriptCode = scriptCode + line + Environment.NewLine;
            }

            try
            {
                scriptCode = String.Format(scriptCode, pars);
            }
            catch (Exception ex)
            {
                aceLog.log(ex.Message, null, true);
                output.isReady = false;
            }

            //List<String> scriptLines = new List<string>();
            //scriptLines.AddRange();

            foreach (String line in scriptCode.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                output.Append(line, false);
            }

            return output;
        }
    }
}