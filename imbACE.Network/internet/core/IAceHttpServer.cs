// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAceHttpServer.cs" company="imbVeles" >
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
    using imbACE.Network.internet.config;
    using imbACE.Network.internet.sessions;
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
    using System.Net;

    public interface IAceHttpServer
    {
        int port { get; }

        /// <summary>
        /// Suspends the specified in seconds.
        /// </summary>
        /// <param name="inSeconds">The in seconds - from this moment</param>
        /// <param name="restartInSeconds">The restart in seconds - from moment of suspension: in <c>inSeconds</c> + this parametere</param>
        void Suspend(int inSeconds = -1, int restartInSeconds = -1);

        /// <summary>
        /// Starts this server,
        /// </summary>
        void Start();

        /// <summary>
        /// Stop server and dispose all functions.
        /// </summary>
        void Stop();

        /// <summary>
        /// Continues this instance -- if it was suspended
        /// </summary>
        void Continue();

        /// <summary>
        /// Before the instance going to be started
        /// </summary>
        void beforeStarted();

        /// <summary>
        /// Gets the name of the instance.
        /// </summary>
        /// <value>
        /// The name of the instance.
        /// </value>
        string instanceName { get; }

        /// <summary>
        /// Gets the session control.
        /// </summary>
        /// <value>
        /// The session control.
        /// </value>
        serverSessionControl sessionControl { get; }

        /// <summary>ALC component of the server -- use it within <see cref="Process(HttpListenerContext)"/> override to check access rights</summary>
        aceServerUserALC accessControl { get; }
    }
}