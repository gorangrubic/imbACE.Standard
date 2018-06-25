// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceServerInitFlags.cs" company="imbVeles" >
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
namespace imbACE.Network.internet.config
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

    /// <summary>
    /// Server init options
    /// </summary>
    [Flags]
    public enum aceServerInitFlags
    {
        none = 0,

        /// <summary>
        /// To enforce Access Level Control (authorization)
        /// </summary>
        enforceALC = 1,

        /// <summary>
        /// Should it check IP allow list in user definition
        /// </summary>
        /// <remarks>
        /// When <c>enforceACL</c> is on it checks user allowed IP list
        /// </remarks>
        enforceIPDoubleCheck = 2,

        /// <summary>
        /// It uses IP allowed list from <see cref="configParameters"/>
        /// </summary>
        enforceIPGlobalCheck = 4,

        /// <summary>
        /// It will ignore both <see cref="enforceIPGlobalCheck"/> and <see cref="enforceIPDoubleCheck"/> and allow only local host to send requests.
        /// </summary>
        enforceLocalhostOnly = 8,

        /// <summary>
        /// Allow access all ip addressed with the same network prefix
        /// </summary>
        enforceLocalNetworkNodeOnly = 16,
    }
}