// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceRemoteTestClient.cs" company="imbVeles" >
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
namespace imbACE.Network.remote.clients
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
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Testing client class
    /// </summary>
    /// <seealso cref="imbACE.Network.remote.instance.aceRemoteClient" />
    public class aceRemoteTestClient : aceRemoteClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="aceRemoteTestClient"/> class.
        /// </summary>
        /// <param name="__settings"></param>
        public aceRemoteTestClient(aceRemoteSettings __settings) : base(__settings)
        {
            settingsLoadOrDefault();

            logSettings();

            onClientReceived += new aceRemoteInstanceBaseEvent(received);
            onClientSent += new aceRemoteInstanceBaseEvent(sending);

            List<string> scriptLines = new List<string>();

            string scriptPath = "_testClientScript.txt";
            if (!File.Exists(scriptPath))
            {
                scriptLines.Add("PNG 5;");
                scriptLines.Add("ECH echooo;");
                scriptLines.Add("RND 3;");

                File.WriteAllLines(scriptPath, scriptLines);
            }
            else
            {
                scriptLines.AddRange(File.ReadAllLines(scriptPath));
            }

            if (connect())
            {
                startSending();
                startProcessing();

                foreach (string ln in scriptLines)
                {
                    // Console.ReadLine();
                    string command = commandCheck(ln);
                    log("Command added to: " + sendToStack(command, 50, client).ToString());
                }

                /*
                while (true)
                {
                    Console.WriteLine("Enter line to send :: ");
                    String command = Console.ReadLine();
                    command = commandCheck(command);
                    log("Command added to: " + sendToStack(command, 50, client).ToString());
                }*/
            }
            else
            {
            }
        }

        public void received(aceRemoteInstanceBase instance, aceRemoteInstanceBaseEventArgs args)
        {
            log("-- received[" + args.socket.RemoteEndPoint.ToString() + "] :: " + args.message);
        }

        public void sending(aceRemoteInstanceBase instance, aceRemoteInstanceBaseEventArgs args)
        {
            log("-- sending[" + args.socket.RemoteEndPoint.ToString() + "] :: " + args.message);
        }
    }
}