// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceServerRequestStdParams.cs" company="imbVeles" >
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
// Project: imbACE.Network
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbACE.Network.internet.events
{
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
    using System;

    [Flags]
    public enum aceServerRequestStdParams
    {
        none = 0,

        /// <summary>
        /// The CRM action parameter
        /// </summary>
        action = 1,

        /// <summary>
        /// The CRM module name
        /// </summary>
        module = 2,

        /// <summary>
        /// The parent tab (containing CRM module link)
        /// </summary>
        parentTab = 4,

        /// <summary>
        /// The record UID
        /// </summary>
        record = 8,

        /// <summary>
        /// The log
        /// </summary>
        log = 16,

        find = 32,
        create = 64,
        scan = 128,

        filepath = 256,

        phpsessid = 512,
        acesessid = 1024,

        username = 2024,
        password = 4048,
    }
}