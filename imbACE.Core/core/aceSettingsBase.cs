// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceSettingsBase.cs" company="imbVeles" >
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
namespace imbACE.Core.core
{
    using extensions.io;
    using imbACE.Core.extensions;
    using imbSCI.Data.data;
    using System;
    using System.IO;
    using System.Threading;
    using System.Xml.Serialization;

    /// <summary>
    /// Base class for ace settings holder
    /// </summary>
    /// <seealso cref="imbBindable" />
    public abstract class aceSettingsBase : imbBindable
    {
        protected aceSettingsBase()
        {
            wasLoaded = false;
        }

        private Boolean _wasLoaded = false;

        /// <summary>
        ///
        /// </summary>
        [XmlIgnore]
        public Boolean wasLoaded
        {
            get { return _wasLoaded; }
            set { _wasLoaded = value; }
        }
    }
}