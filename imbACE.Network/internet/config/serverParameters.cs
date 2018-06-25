// --------------------------------------------------------------------------------------------------------------------
// <copyright file="serverParameters.cs" company="imbVeles" >
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
    using imbACE.Core.core;
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
    /// Run-time configuration, loaded from external file
    /// </summary>
    public class serverParameters : aceSettingsBase
    {
        /// <summary>
        ///
        /// </summary>
        public string instanceName { get; set; }

        /// <summary>
        /// The default port to use if -1 is supplied during instance creation
        /// </summary>
        public int port { get; set; } = 6897;

        /// <summary>
        /// Session timeout
        /// </summary>
        public int sessionTimeoutMinutes { get; set; } = 30;

        private string _filepathToServe = "/var/www/html";

        /// <summary> Local filepath to serve </summary>
        public string filepathToServe
        {
            get
            {
                return _filepathToServe;
            }
            set
            {
                _filepathToServe = value;
                OnPropertyChanged("filepathToServe");
            }
        }
    }
}