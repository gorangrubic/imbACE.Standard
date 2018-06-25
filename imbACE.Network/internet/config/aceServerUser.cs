// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceServerUser.cs" company="imbVeles" >
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
    using System.Collections.Generic;

    /// <summary>
    /// Definition of a php user
    /// </summary>
    public class aceServerUser
    {
        public aceServerUser()
        {
        }

        private static aceServerUser _serverSidePhp;

        /// <summary>
        /// Default user instance - for server side PHP call
        /// </summary>
        public static aceServerUser serverSidePhp
        {
            get
            {
                if (_serverSidePhp == null)
                {
                    _serverSidePhp = new aceServerUser();
                    _serverSidePhp.userAccessLevel = 100;
                }
                return _serverSidePhp;
            }
        }

        /// <summary>0 means it is not authorised</summary>
        public int userAccessLevel { get; protected set; } = 0;

        /// <summary> </summary>
        public uint uid { get; internal set; } = 10;

        /// <summary> </summary>
        public string username { get; internal set; } = "php";

        private List<string> _ipAddress;

        /// <summary> </summary>
        public List<string> ipAddress
        {
            get
            {
                if (_ipAddress == null) _ipAddress = new List<string>();
                _ipAddress.Add("127.0.0.1");
                return _ipAddress;
            }
            internal set
            {
                _ipAddress = value;
            }
        }

        /// <summary>Password  MD5 hash</summary>
        public string md5pass { get; internal set; } = "";
    }
}