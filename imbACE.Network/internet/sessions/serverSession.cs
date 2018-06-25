// --------------------------------------------------------------------------------------------------------------------
// <copyright file="serverSession.cs" company="imbVeles" >
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
namespace imbACE.Network.internet.sessions
{
    using imbACE.Core;
    using imbACE.Network.internet.config;
    using imbSCI.Core;
    using imbSCI.Core.attributes;
    using imbSCI.Core.collection;
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
    using System.Data;

    /// <summary>
    /// Instance of a session
    /// </summary>
    public class serverSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="serverSession"/> class.
        /// </summary>
        /// <param name="uid">The uid.</param>
        public serverSession(string uid)
        {
            started = DateTime.Now;
            sessionID = uid;
        }

        /// <summary> </summary>
        public DateTime started { get; protected set; }

        /// <summary>
        /// The username on null
        /// </summary>
        public const string usernameOnNull = "[not authorized]";

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string username
        {
            get
            {
                if (authorizedUser == null) return usernameOnNull;
                return authorizedUser.username;
            }
        }

        /// <summary>Instance of <c>user</c></summary>
        public aceServerUser authorizedUser { get; set; }

        /// <summary> </summary>
        public PropertyCollectionExtended data { get; protected set; } = new PropertyCollectionExtended();

        /// <summary> </summary>
        public DataSet dataSet { get; protected set; } = new DataSet();

        /// <summary> </summary>
        public string sessionID { get; protected set; }

        /// <summary> </summary>
        public string phpSessionID { get; protected set; }

        /// <summary> </summary>
        public bool isActive { get; set; } = true;
    }
}