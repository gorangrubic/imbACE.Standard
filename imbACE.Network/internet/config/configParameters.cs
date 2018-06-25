// --------------------------------------------------------------------------------------------------------------------
// <copyright file="configParameters.cs" company="imbVeles" >
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

    /// <summary>
    /// Design-time configuration
    /// </summary>
    public class configParameters
    {
        /// <summary>
        /// The default server settings
        /// </summary>
        private serverParameters _settings = new serverParameters();

        /// <summary> </summary>
        public serverParameters settings
        {
            get
            {
                return _settings;
            }
            protected set
            {
                _settings = value;
            }
        }

        /// <summary>
        /// DEFAULT init flags
        /// </summary>
        /// <value>
        /// The initialize flags.
        /// </value>
        public aceServerInitFlags initFlags { get; protected set; } = aceServerInitFlags.enforceLocalhostOnly;

        /// <summary> relative path to user list - relative from server execution</summary>
        public string pathToUserlist { get; protected set; } = "config/user.acejson";

        /// <summary> relative path to <see cref="serverParameters"/> xml file - relative from server execution</summary>
        public string pathToConfig { get; protected set; } = "config/server.acejson";

        /// <summary>HTTP header terminator</summary>
        public string headerTerminator { get; protected set; } = "\r\n\r\n";

        private static configParameters _main;

        /// <summary>
        /// Current and active server instance configuration
        /// </summary>
        public static configParameters main
        {
            get
            {
                if (_main == null)
                {
                    _main = new configParameters();
                }
                return _main;
            }
        }

        /// <summary> </summary>
        public int defSuspendInSeconds { get; protected set; } = 0;

        /// <summary> </summary>
        public int defRestartInSeconds { get; protected set; } = 0;

        /// <summary>Minimal port number for safe operation</summary>
        public int minPortNumber { get; protected set; } = 1024;

        /// <summary>Maximal port number for safe operation</summary>
        public int maxPortNumber { get; protected set; } = 49151;
    }
}