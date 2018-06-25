// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceServerUserALC.cs" company="imbVeles" >
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
    using imbACE.Network.core;
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
    using System.Collections.Generic;

    /// <summary>
    /// aceServer :: collection of users and Access Level Controler
    /// </summary>
    public class aceServerUserALC : serverComponentBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="aceServerUserALC"/> class.
        /// </summary>
        /// <param name="__serverInstance">The server instance.</param>
        /// <param name="__componentName">Name of the component.</param>
        /// <param name="__instanceName">Name of the instance.</param>
        public aceServerUserALC(IAceHttpServer __serverInstance) : base(__serverInstance, "ALC for " + __serverInstance.instanceName, __serverInstance.instanceName)
        {
        }

        /// <summary>
        /// Returns null if username and password are not matched
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>null or aceServerUser instance</returns>
        public aceServerUser GetAccessLevel(string username, string password)
        {
            if (items.ContainsKey(username))
            {
                string md5pass = md5.GetMd5Hash(password);
                aceServerUser usr = this[username];
                if (usr.md5pass == md5pass)
                {
                    return usr;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the <see cref="aceServerUser"/> with the specified username.
        /// </summary>
        /// <value>
        /// The <see cref="aceServerUser"/>.
        /// </value>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public aceServerUser this[string username]
        {
            get
            {
                if (items.ContainsKey(username))
                {
                    return items[username];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Sets the user supplied. Use this during init sequence or during user list loading
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="md5pass">The md5 encripted password</param>
        /// <param name="uid">The uid.</param>
        /// <param name="uLevel">User access level</param>
        /// <param name="allowIP">The list of IP addresses allowed to login using this username/password.</param>
        /// <returns></returns>
        /// <exception cref="imbACE.Network.core.aceServerException">Username [" + username + "] is already taken. - null - SetUser failed</exception>
        public aceServerUser SetUser(string username, string md5pass, uint uid, int uLevel = 1, string[] allowIP = null)
        {
            aceServerUser item = new aceServerUser();
            item.username = username;
            item.md5pass = md5pass;
            item.uid = uid;
            if (allowIP != null) item.ipAddress.AddRange(allowIP);
            if (items.ContainsKey(username)) throw new aceServerException("Username [" + username + "] is already taken.", null, serverInstance, "SetUser failed");
            items.Add(username, item);

            return item;
        }

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, aceServerUser> items { get; set; } = new Dictionary<string, aceServerUser>();
    }
}