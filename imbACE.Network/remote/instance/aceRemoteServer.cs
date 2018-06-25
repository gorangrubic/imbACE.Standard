// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceRemoteServer.cs" company="imbVeles" >
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
namespace imbACE.Network.remote.instance
{
    using imbACE.Core;
    using imbACE.Network.remote.core;
    using imbACE.Network.remote.events;
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
    using System.ComponentModel;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="imbACE.Network.remote.instance.aceRemoteClient" />
    public class aceRemoteServer : aceRemoteClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="aceRemoteServer"/> class.
        /// </summary>
        /// <param name="__settings"></param>
        public aceRemoteServer(aceRemoteSettings __settings) : base(__settings)
        {
            settingsLoadOrDefault();

            settings.serverIP = Dns.GetHostName();
        }

        /// <summary>
        /// All done
        /// </summary>
        protected ManualResetEvent allDone = new ManualResetEvent(false);

        protected ManualResetEvent readDone = new ManualResetEvent(false);

        /// <summary>
        /// Kada server primi konekciju
        /// </summary>
        public aceRemoteInstanceBaseEvent onServerReceived;

        /// <summary>
        /// Sends the back string.
        /// </summary>
        /// <param name="handler">The handler.</param>
        /// <param name="_data">The data.</param>
        public void sendBackString(Socket handler, string _data)
        {
            byte[] byteData = Encoding.ASCII.GetBytes(_data);
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(sendBackCallback), handler);
        }

        /// <summary>
        /// Sends the back callback.
        /// </summary>
        /// <param name="ar">The ar.</param>
        public void sendBackCallback(IAsyncResult ar)
        {
            Socket handler = (Socket)ar.AsyncState;

            int bytesSent = handler.EndSend(ar);
            log("Bytes sent: " + bytesSent.ToString());
        }

        #region ----------- Boolean [ doLoopListening ] -------  [Da li da loopuje slusanje veze]

        private bool _doLoopListening = true;

        /// <summary>
        /// Is listening turned on
        /// </summary>
        [Category("Switches")]
        [DisplayName("doLoopListening")]
        [Description("Da li da loopuje slusanje veze")]
        public bool doLoopListening
        {
            get { return _doLoopListening; }
            set { _doLoopListening = value; OnPropertyChanged("doLoopListening"); }
        }

        #endregion ----------- Boolean [ doLoopListening ] -------  [Da li da loopuje slusanje veze]

        /// <summary>
        /// Starts the listening.
        /// </summary>
        public void startListening()
        {
            try
            {
                ipHostInfo = Dns.Resolve(settings.serverIP);
                ipAddress = ipHostInfo.AddressList[0];
                remoteEP = new IPEndPoint(ipAddress, settings.serverPort);

                log("Server IP: " + ipAddress.ToString());

                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Bind(remoteEP);
                client.Listen(100);

                while (doLoopListening)
                {
                    allDone.Reset();

                    log("Waiting for a connection ...");

                    aceRemoteStateObject state = new aceRemoteStateObject(settings.bufferSize);

                    client.BeginAccept(new AsyncCallback(acceptCallback), state);

                    // ovde je obrada primljene poruke
                    // String input = state.sb.ToString();

                    //if (onServerReceived != null) onServerReceived(this, new aceRemoteInstanceBaseEventArgs(aceRemoteInstanceBaseEventType.serverReceive, input));

                    allDone.WaitOne();
                }
            }
            catch (Exception e)
            {
                log("Listing failed : " + e.Message);
                return;
            }
        }

        /// <summary>
        /// Accepts the callback.
        /// </summary>
        /// <param name="ar">The ar.</param>
        public void acceptCallback(IAsyncResult ar)
        {
            allDone.Set();

            aceRemoteStateObject state = (aceRemoteStateObject)ar.AsyncState;
            Socket handler = client.EndAccept(ar);
            state.workSocket = handler;

            log("Connected [" + handler.LocalEndPoint.ToString() + "]::[" + handler.RemoteEndPoint.ToString() + "]");// handler.RemoteEndPoint.ToString();

            handler.BeginReceive(state.buffer, 0, settings.bufferSize, 0, new AsyncCallback(readCallback), state);
        }

        public void readCallback(IAsyncResult ar)
        {
            string content = "";

            aceRemoteStateObject state = (aceRemoteStateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                content = state.sb.ToString();

                int endLine = content.IndexOf(settings.endMessageSufix);
                if (endLine > -1)
                {
                    string[] lns = content.Split(settings.endMessageSufix.ToCharArray(),
                                            StringSplitOptions.RemoveEmptyEntries);

                    foreach (string ln in lns)
                    {
                        if (onServerReceived != null) onServerReceived(this, new aceRemoteInstanceBaseEventArgs(aceRemoteInstanceBaseEventType.serverReceive, ln, handler));
                    }

                    //String receivedLine = content.Substring(0, endLine);

                    //state.sb.Remove(0, endLine);
                    //String input = state.sb.ToString();

                    state.sb.Clear();
                    //log("Received: " + content);

                    // handler.Close();
                    // allDone.Set();
                    // all data received
                }
                else
                {
                }

                handler.BeginReceive(state.buffer, 0, settings.bufferSize, 0, new AsyncCallback(readCallback), state);
            }
            else
            {
                //if (state.sb.Length > 1)
                //{
                //    content = state.sb.ToString();
                //    if (onServerReceived != null) onServerReceived(this, new aceRemoteInstanceBaseEventArgs(aceRemoteInstanceBaseEventType.serverReceive, content));

                //}
                // handler.Close();
            }
        }
    }
}