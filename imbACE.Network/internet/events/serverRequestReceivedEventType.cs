// --------------------------------------------------------------------------------------------------------------------
// <copyright file="serverRequestReceivedEventType.cs" company="imbVeles" >
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

    /// <summary>
    /// Tip dogadjaja - za objekat: serverRequestReceived
    /// </summary>
    public enum serverRequestReceivedEventType
    {
        unknown,
        error,

        /// <summary>
        /// Trys to find file in the serving path
        /// </summary>
        getFile,

        /// <summary>
        /// The CRM action: it expects action, module, record and other parameters in get
        /// </summary>
        crmAction,

        /// <summary>
        /// The ALC approved the username
        /// </summary>
        startSession,

        /// <summary>
        /// The access denied for the username, password and/or IP address
        /// </summary>
        accessDenied,

        /// <summary>
        /// The access is granted
        /// </summary>
        authorisedRequest,

        /// <summary>
        /// The access was granted - client calls for session termination
        /// </summary>
        terminateSession,
    }
}