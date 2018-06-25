// --------------------------------------------------------------------------------------------------------------------
// <copyright file="logSystem.cs" company="imbVeles" >
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbACE.Core.core
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.config;
    using imbSCI.Core.reporting.render.converters;
    using imbSCI.Core.reporting.render.core;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using System.Collections;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Threading;

    public enum logType
    {
        Notification,
        ExecutionError,
        FatalError,
        Done
    }

    /// <summary>
    /// Compatibility layer --- for easier integration of several ealier source codes
    /// </summary>
    public static class logSystem
    {
        /// <summary>
        /// Avoid this method --- this is a compatibility layer, call <see cref="aceLog.log(string, object, bool)"/> instead. Forwards the call to <see cref="aceLog.log"/> and ignores <c>logResources</c>
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="logResources">These are ignored ---</param>
        public static void log(String message, params Object[] logResources)
        {
            aceLog.log(message);
        }
    }
}