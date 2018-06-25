// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceApplicationInfo.cs" company="imbVeles" >
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

namespace imbACE.Core.application
{
    using imbSCI.Core.data;
    using System;

    /// <summary>
    /// Information about application that is immutable - defined at Design Time
    /// </summary>
    /// <seealso cref="imbSCI.Core.data.aceAuthorNotation" />
    public class aceApplicationInfo : aceAuthorNotation
    {
        public aceApplicationInfo()
        {
        }

        public const String FOLDERNAME_CACHE = "cache";
        public const String FOLDERNAME_REPORTS = "reports";
        public const String FOLDERNAME_RESOURCES = "resources";
        public const String FOLDERNAME_CONFIG = "config";
        public const String FOLDERNAME_PLUGINS = "plugin";
        public const String FOLDERNAME_PROJECTS = "projects";
        public const String FOLDERNAME_LOGS = "logs";

        /// <summary>
        /// Message to be shown at startup
        /// </summary>
        /// <value>
        /// The welcome message.
        /// </value>
        public String welcomeMessage { get; set; }

        /// <summary>
        /// naziv aplikacije
        /// </summary>
        public String applicationName { get { return software; } }

        /// <summary>
        /// verzija aplikacije
        /// </summary>
        public String applicationVersion { get; set; }
    }
}