// --------------------------------------------------------------------------------------------------------------------
// <copyright file="serverComponentBase.cs" company="imbVeles" >
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

namespace imbACE.Network.internet.core
{
    using imbACE.Core;
    using imbACE.Network.core;
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
    ///
    /// </summary>
    /// <seealso cref="imbACE.Network.internet.core.IAceServerComponent" />
    public abstract class serverComponentBase : IAceServerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="serverComponentBase"/> class.
        /// </summary>
        /// <param name="__serverInstance">The server instance.</param>
        /// <param name="__componentName">Name of the component.</param>
        /// <param name="__instanceName">Name of the instance.</param>
        /// <exception cref="aceServerException">Server instance is null and instance name is empty/null - null - null - Component constructor exception</exception>
        public serverComponentBase(IAceHttpServer __serverInstance, string __componentName = "", string __instanceName = "")
        {
            if (__serverInstance == null)
            {
                if (__instanceName.isNullOrEmpty()) throw new aceServerException("Server instance is null and instance name is empty/null", null, null, "Component constructor exception");
                instanceName = __instanceName;
            }
            else
            {
                serverInstance = __serverInstance;
            }

            if (componentName.isNullOrEmpty())
            {
                componentName = GetType().Name;
            }
        }

        /// <summary> </summary>
        public string componentName { get; protected set; }

        /// <summary>
        /// Server instance name
        /// </summary>
        public string instanceName { get; private set; }

        /// <summary>
        /// Reference to <see cref="IAceHttpServer"/>
        /// </summary>
        public IAceHttpServer serverInstance { get; private set; }
    }
}