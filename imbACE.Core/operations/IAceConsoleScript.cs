// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAceConsoleScript.cs" company="imbVeles" >
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

namespace imbACE.Core.operations
{
    using imbACE.Core.commands;
    using imbSCI.Core.files.search;
    using imbSCI.Core.reporting;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;

    public interface IAceConsoleScript : IEnumerable<string>
    {
        /// <summary> True if the script is ready for execution </summary>
        Boolean isReady { get; }

        /// <summary> </summary>
        DateTime executionStart { get; }

        /// <summary> </summary>
        DateTime executionFinish { get; }

        DateTime lastWrite { get; }
        bool contentChanged { get; }
        List<string> contentLines { get; }
        string path { get; }
        FileInfo info { get; }
        bool HasChanges { get; }
        List<string> Changes { get; }

        /// <summary>
        /// Calls the script execution abort. It can't abort already called command but will abort on the next.
        /// </summary>
        void AbortExecution();

        /// <summary>
        /// Gets the script in another <see cref="commandLineFormat"/>
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="format">The format.</param>
        /// <param name="newPath">The new path.</param>
        /// <returns></returns>
        IAceConsoleScript GetScriptInForm(IAceCommandConsole console, commandLineFormat format, String newPath = null);

        IEnumerator<string> GetEnumerator();

        fileTextOperater getOperater(bool useMemMap);

        string getContent(bool ignoreCache);

        List<string> getContentLines(bool ignoreCache);

        void setContent(string content);

        void setContentLines(IList<string> input);

        void Append(string line, bool callSave);

        void Append(IEnumerable<string> line, bool callSave);

        void AppendUnique(IEnumerable<string> line, bool callSave);

        void SaveAs(string newPath, getWritableFileMode mode, ILogBuilder loger);

        void Save(ILogBuilder loger);

        long getByteSize();

        long getLineCount();

        List<string> GetChanges(bool andAccept);

        event PropertyChangedEventHandler PropertyChanged;
    }
}