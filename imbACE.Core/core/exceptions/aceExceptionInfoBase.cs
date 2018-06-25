// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceExceptionInfoBase.cs" company="imbVeles" >
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
namespace imbACE.Core.core.exceptions
{
    using extensions.io;
    using imbACE.Core.collection;
    using imbACE.Core.core.exceptions;
    using imbACE.Core.extensions;
    using imbACE.Core.interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Data model
    /// </summary>
    /// <seealso cref="imbACE.Core.core.exceptions.IAceExceptionSerializable" />
    public abstract class aceExceptionInfoBase : IAceExceptionSerializable
    {
        protected aceExceptionInfoBase()
        {
        }

        public string Message { get; set; }

        public string RelInstanceClassName { get; set; }

        public string RelInstanceParentClassName { get; set; }

        public string StackTrace { get; set; }

        public DateTime time { get; set; }

        public string Title { get; set; }

        public string SouceCodeLine { get; set; }

        public string SourceCodeFile { get; set; }

        public string DataDump { get; set; }

        public abstract String SaveXML(String path = null);

        /// <summary>
        /// Gets the filename with .log extension
        /// </summary>
        /// <returns></returns>
        public abstract String GetFilename();
    }
}