// --------------------------------------------------------------------------------------------------------------------
// <copyright file="serverSessionControl.cs" company="imbVeles" >
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
    using imbACE.Network.core;
    using imbACE.Network.internet.config;
    using imbACE.Network.internet.core;
    using imbSCI.Core;
    using imbSCI.Core.attributes;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.math;
    using imbSCI.Data;
    using imbSCI.Data.collection;
    using imbSCI.Data.data;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex;
    using imbSCI.Reporting;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// Session control mechanism
    /// </summary>
    /// <remarks>
    /// <para>Primarly desinged for network servers, but might be usefull for other applications due <see cref="serverSession.data"/> and <see cref="serverSession.dataSet"/> properties</para>
    /// </remarks>
    public class serverSessionControl : serverComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="serverSessionControl"/> class.
        /// </summary>
        /// <param name="serverInstanceName">Name of the server instance.</param>
        public serverSessionControl(IAceHttpServer __server) : base(__server, "Session control for " + __server.instanceName, __server.instanceName)
        {
            started = DateTime.Now;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public virtual void Update()
        {
        }

        public virtual int Clear()
        {
            int output = items.Count();
            items.Clear();
            return output;
        }

        public virtual int Count() => items.Count();

        /// <summary> </summary>
        public string serverName { get; protected set; } = "";

        /// <summary>Time of instance creation</summary>
        public DateTime started { get; protected set; }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <param name="sessionID">The session identifier.</param>
        /// <returns></returns>
        public virtual serverSession GetSession(string sessionID)
        {
            if (items.ContainsKey(sessionID))
            {
                return items[sessionID];
            }
            return null;
        }

        /// <summary>
        /// Gets the new session.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="PHPSESSID">The phpsessid.</param>
        /// <returns></returns>
        public virtual serverSession GetNewSession(aceServerUser user, string PHPSESSID = "")
        {
            string newUID = GetNewUID(user.username + PHPSESSID);
            serverSession output = new serverSession(newUID);
            items.Add(newUID, output);
            return output;
        }

        /// <summary>
        /// The limit new uid creation retries inside <see cref="GetNewUID(string)"/>
        /// </summary>
        public const int LIMIT_NEWUID_RETRY = 1000;

        /// <summary>
        /// Gets the new uid based on <c>customString</c> and internal algorithm
        /// </summary>
        /// <param name="customString">The custom string.</param>
        /// <returns>Unique ID</returns>
        /// <exception cref="aceServerSessionControlException">Failed to get new UID after r:[" + r.ToString() + "] retries. - null</exception>
        internal string GetNewUID(string customString = "")
        {
            string uid = "";

            TimeSpan sinceStart = DateTime.Now.Subtract(started);

            if (customString.isNullOrEmpty())
            {
                customString = imbStringGenerators.getRandomString(8);
            }

            customString = items.Count.ToString("D3") + customString;

            int r = 0;
            do
            {
                if (r > LIMIT_NEWUID_RETRY)
                {
                    throw new aceServerSessionControlException("Failed to get new UID after r:[" + r.ToString() + "] retries.", null, serverName);
                }
                uid = customString + sinceStart.TotalMilliseconds.ToString();
                uid = md5.GetMd5Hash(uid);
                Thread.Sleep(10 + items.Count);
                r++;
            } while (items.ContainsKey(uid));

            return uid;
        }

        /// <summary>
        /// Active and terminated sessions
        /// </summary>
        protected Dictionary<string, serverSession> items { get; set; } = new Dictionary<string, serverSession>();
    }
}