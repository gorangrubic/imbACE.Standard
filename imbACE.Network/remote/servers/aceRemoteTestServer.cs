// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceRemoteTestServer.cs" company="imbVeles" >
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
namespace imbACE.Network.remote.servers
{
    using imbACE.Core;
    using imbACE.Network.remote.core;
    using imbACE.Network.remote.events;
    using imbACE.Network.remote.instance;
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
    using System;
    using System.Threading;

    /// <summary>
    /// Test application and demonstration class
    /// </summary>
    /// <seealso cref="imbACE.Network.remote.instance.aceRemoteServer" />
    public class aceRemoteTestServer : aceRemoteServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="aceRemoteTestServer"/> class.
        /// </summary>
        /// <param name="__settings"></param>
        public aceRemoteTestServer(aceRemoteSettings __settings = null) : base(__settings)
        {
            onServerReceived += new aceRemoteInstanceBaseEvent(received);

            logSettings();

            startSending();

            startListening();
        }

        /// <summary>
        /// Reaction on something received
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="args">The <see cref="aceRemoteInstanceBaseEventArgs"/> instance containing the event data.</param>
        public void received(aceRemoteInstanceBase instance, aceRemoteInstanceBaseEventArgs args)
        {
            string ip = args.socket.RemoteEndPoint.ToString();
            log("Received [" + ip.ToString() + "]: " + args.message);

            string msg = args.message.Trim();

            if (msg.Length > 4)
            {
                string cmd = msg.Substring(0, 3).ToUpper();
                string par = msg.Substring(4).Trim();

                switch (cmd)
                {
                    case "RND":
                        if (string.IsNullOrWhiteSpace(par)) par = "5";
                        int _rndTimes = int.Parse(par);
                        Random random = new Random();
                        for (int ri = 0; ri < _rndTimes; ri++)
                        {
                            string rnd = imbSCI.Core.extensions.text.imbStringGenerators.getRandomString(32); //aceCommonExtensions.getRandomString(32);

                            int rnt = random.Next(10) * 200;

                            sendStack.Enqueue(new aceRemoteSendTask(rnd, rnt, args.socket));
                        }
                        break;

                    case "ECH":
                        if (string.IsNullOrWhiteSpace(par)) par = "[echo]";
                        sendStack.Enqueue(new aceRemoteSendTask(par, 0, args.socket));
                        break;

                    case "PNG":
                        if (string.IsNullOrWhiteSpace(par)) par = "5";

                        int _pngTimes = int.Parse(par);
                        for (int pi = 0; pi < _pngTimes; pi++)
                        {
                            sendStack.Enqueue(new aceRemoteSendTask("ping [" + pi.ToString("D3") + "/" + _pngTimes + "]", 200, args.socket));
                        }
                        break;

                    default:
                        log("Unknown command [" + msg + "]");
                        break;
                }
            }
        }
    }
}