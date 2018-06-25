// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceRemoteClient.cs" company="imbVeles" >
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
    using System.Collections.Concurrent;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Client for remote communication
    /// </summary>
    /// <seealso cref="imbACE.Network.remote.instance.aceRemoteInstanceBase" />
    public class aceRemoteClient : aceRemoteInstanceBase
    {
        /// <summary>
        /// Kada server primi konekciju
        /// </summary>
        public aceRemoteInstanceBaseEvent onClientReceived;

        public aceRemoteInstanceBaseEvent onClientSent;

        #region SENDING STACK

        private ConcurrentQueue<aceRemoteSendTask> _sendStack = new ConcurrentQueue<aceRemoteSendTask>();

        /// <summary>
        /// stack zadataka za slanje
        /// </summary>
        public ConcurrentQueue<aceRemoteSendTask> sendStack
        {
            get
            {
                return _sendStack;
            }
            set
            {
                _sendStack = value;
                OnPropertyChanged("sendStack");
            }
        }

        /// <summary>
        /// Sends to stack.
        /// </summary>
        /// <param name="__message">The message.</param>
        /// <param name="__delay">The delay.</param>
        /// <param name="__socket">The socket.</param>
        /// <returns></returns>
        public int sendToStack(string __message, int __delay = 0, Socket __socket = null)
        {
            var tsk = new aceRemoteSendTask(__message, __delay, __socket);
            return sendToStack(tsk);
        }

        /// <summary>
        /// Sends to stack.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <returns></returns>
        public int sendToStack(aceRemoteSendTask task)
        {
            sendStack.Enqueue(task);
            return sendStack.Count;
        }

        /// <summary>
        /// Pokrece novi thread koji brine o slanju
        /// </summary>
        public void startSending()
        {
            log("Send thread:" + doSendingStackLoop.ToString());

            sendingThread = new Thread(sendingStackLoop);
            sendingThread.Start();
        }

        protected Thread sendingThread;
        protected bool doSendingStackLoop = true;

        protected void sendingStackLoop()
        {
            while (doSendingStackLoop)
            {
                if (!sendStack.IsEmpty)
                {
                    aceRemoteSendTask st;
                    bool poped = sendStack.TryDequeue(out st);
                    if (poped)
                    {
                        SocketError erc = SocketError.Success;

                        byte[] byteData = Encoding.ASCII.GetBytes(st.message);

                        int sbc = st.socket.Send(byteData, 0, byteData.Length, 0, out erc);
                        log("Sent(" + sbc.ToString() + ":bytes) [" + st.message + "] :: " + erc.ToString());

                        if (st.delayAfter > 0) Thread.Sleep(st.delayAfter);

                        poped = false;
                    }
                }
                Thread.Sleep(1);
            }
            Thread.CurrentThread.Suspend();
        }

        #endregion SENDING STACK

        #region read and process stack

        private ConcurrentQueue<aceRemoteReceiveMessage> _readStack = new ConcurrentQueue<aceRemoteReceiveMessage>();

        /// <summary>
        /// stack zadataka za slanje
        /// </summary>
        public ConcurrentQueue<aceRemoteReceiveMessage> readStack
        {
            get
            {
                return _readStack;
            }
            set
            {
                _readStack = value;
                OnPropertyChanged("sendStack");
            }
        }

        protected Thread processThread;

        protected bool doProcessStackLoop = true;

        /// <summary>
        /// Pokrece novi thread koji brine o slanju
        /// </summary>
        public void startProcessing()
        {
            log("Read/Process thread:" + doProcessStackLoop.ToString());

            processThread = new Thread(processStackLoop);
            processThread.Start();
        }

        protected void processStackLoop()
        {
            while (doProcessStackLoop)
            {
                byte[] rb = new byte[settings.bufferSize];
                SocketError erc = SocketError.Success;
                StringBuilder sb = new StringBuilder();
                bool doRepeat = true;
                Socket socket = client;
                while (doRepeat)
                {
                    try
                    {
                        int brc = socket.Receive(rb, 0, settings.bufferSize, 0, out erc);
                        if (brc > 0)
                        {
                            sb.Append(Encoding.ASCII.GetString(rb, 0, brc));
                            rb = new byte[settings.bufferSize];
                        }
                        else
                        {
                            doRepeat = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        log("Receive exception:" + ex.Message);
                    }
                }

                string inputData = sb.ToString().Trim();

                // deljenje u vise linija
                if (!string.IsNullOrEmpty(inputData))
                {
                    if (inputData.Contains(settings.endMessageSufix))
                    {
                        var lns = inputData.Split(settings.endMessageSufix.ToCharArray(),
                                                  StringSplitOptions.RemoveEmptyEntries);
                        foreach (string ln in lns)
                        {
                            readStack.Enqueue(new aceRemoteReceiveMessage(ln, socket));
                        }
                    }
                    else
                    {
                        readStack.Enqueue(new aceRemoteReceiveMessage(inputData, socket));
                    }
                }

                if (!readStack.IsEmpty)
                {
                    aceRemoteReceiveMessage st;
                    bool poped = readStack.TryDequeue(out st);
                    if (poped)
                    {
                        if (onClientReceived != null)
                        {
                            aceRemoteInstanceBaseEventArgs args = new aceRemoteInstanceBaseEventArgs(aceRemoteInstanceBaseEventType.clientReceive, st.message, st.socket);
                            onClientReceived(this, args);
                        }
                        else
                        {
                            log("Processing not attached :: " + st.message);
                        }

                        poped = false;
                    }
                }
                Thread.Sleep(1);
            }
        }

        #endregion read and process stack

        public string commandCheck(string lineInput)
        {
            lineInput = lineInput.Trim();
            if (!lineInput.EndsWith(settings.endMessageSufix)) lineInput += settings.endMessageSufix;

            return lineInput;
        }

        protected ManualResetEvent connectDone = new ManualResetEvent(false);
        protected ManualResetEvent sendDone = new ManualResetEvent(false);
        protected ManualResetEvent receiveDone = new ManualResetEvent(false);

        #region --- client ------- TCP klijent

        private Socket _client;

        /// <summary>
        /// TCP klijent
        /// </summary>
        public Socket client
        {
            get
            {
                return _client;
            }
            set
            {
                _client = value;
                OnPropertyChanged("client");
            }
        }

        #endregion --- client ------- TCP klijent

        #region --- ipHostInfo ------- informacije o serveru

        private IPHostEntry _ipHostInfo;

        /// <summary>
        /// informacije o serveru
        /// </summary>
        public IPHostEntry ipHostInfo
        {
            get
            {
                return _ipHostInfo;
            }
            set
            {
                _ipHostInfo = value;
                OnPropertyChanged("ipHostInfo");
            }
        }

        #endregion --- ipHostInfo ------- informacije o serveru

        #region --- ipAddress ------- Bindable property

        private IPAddress _ipAddress;

        /// <summary>
        /// Bindable property
        /// </summary>
        public IPAddress ipAddress
        {
            get
            {
                return _ipAddress;
            }
            set
            {
                _ipAddress = value;
                OnPropertyChanged("ipAddress");
            }
        }

        #endregion --- ipAddress ------- Bindable property

        #region --- remoteEP ------- end point

        private IPEndPoint _remoteEP;

        /// <summary>
        /// end point
        /// </summary>
        public IPEndPoint remoteEP
        {
            get
            {
                return _remoteEP;
            }
            set
            {
                _remoteEP = value;
                OnPropertyChanged("remoteEP");
            }
        }

        public override string path_settings
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion --- remoteEP ------- end point

        /// <summary>
        /// Instancira klijenta - i kopira podesavanja.
        /// </summary>
        /// <param name="__settings"></param>
        public aceRemoteClient(aceRemoteSettings __settings)
        {
            if (__settings != null)
            {
                settings = __settings;
            }
        }

        /// <summary>
        /// Povezuje se sa serverom
        /// </summary>
        /// <returns></returns>
        public bool connect()
        {
            try
            {
                ipHostInfo = Dns.Resolve(settings.serverIP);
                ipAddress = ipHostInfo.AddressList[0];
                remoteEP = new IPEndPoint(ipAddress, settings.serverPort);

                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.BeginConnect(remoteEP, new AsyncCallback(connectCallback), client);
                connectDone.WaitOne();
            }
            catch (Exception e)
            {
                log("Connect failed : " + e.Message);
                return false;
            }

            return true;
        }

        protected void connectCallback(IAsyncResult ar)
        {
            try
            {
                client.EndConnect(ar);
                connectDone.Set();

                log("Connected to [" + client.RemoteEndPoint.ToString() + "]");
            }
            catch (Exception e)
            {
                log("Connect exception (" + e.GetType().Name + ") : " + e.Message);
                return;
            }
        }

        #region RECEIVE

        /// <summary>
        /// Prijem poruke
        /// </summary>
        /// <returns></returns>
        public aceRemoteStateObject receive()
        {
            aceRemoteStateObject state = new aceRemoteStateObject(settings.bufferSize);
            state.workSocket = client;

            client.BeginReceive(state.buffer, 0, settings.bufferSize, 0, new AsyncCallback(receiveCallback), state);

            return state;
        }

        /// <summary>
        /// Prijem string poruke
        /// </summary>
        /// <returns></returns>
        public string receiveString()
        {
            aceRemoteStateObject state = receive();
            string received = state.sb.ToString();
            log("Data received: " + received);

            return received;
        }

        public void receiveCallback(IAsyncResult ar)
        {
            try
            {
                aceRemoteStateObject state = (aceRemoteStateObject)ar.AsyncState;
                Socket socket = state.workSocket;

                int bytesRead = socket.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    /*
                    socket.BeginReceive(state.buffer, 0, settings.bufferSize, 0, new AsyncCallback(receiveCallback),
                                        state);*/
                }
                else
                {
                    // receiveDone.Set();
                }

                if (socket.Available == 0)
                {
                    /* state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                     receiveDone.Set();
                     return;*/
                }
            }
            catch (Exception ex)
            {
                //receiveDone.Set();
            }
        }

        #endregion RECEIVE

        /// <summary>

        /// Salje string podatak, enkodiran kao ASCII
        /// </summary>
        /// <param name="_data"></param>
        public void sendString(string _data)
        {
            if (!_data.EndsWith(settings.endMessageSufix))
            {
                _data = _data + settings.endMessageSufix;
            }

            byte[] byteData = Encoding.ASCII.GetBytes(_data);

            log("Sending string[" + _data + "] (bytes:" + byteData.Length + ")");
            try
            {
                client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(sendCallback), client);
            }
            catch (Exception e)
            {
                log("Exception (" + e.GetType().Name + ") : " + e.Message);
                return;
            }
            sendDone.WaitOne();
            log("Send done!");
        }

        protected void sendCallback(IAsyncResult ar)
        {
            int bytesSent = client.EndSend(ar);
            sendDone.Set();
        }

        /// <summary>
        /// Zatvara konekciju sa serverom
        /// </summary>
        public void close()
        {
            if (client == null) return;
            if (client.Connected)
            {
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
        }
    }
}