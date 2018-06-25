// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceRemoteReceiveMessage.cs" company="imbVeles" >
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
namespace imbACE.Network.remote.core
{
    using imbACE.Core;
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
    using System.Net.Sockets;

    /// <summary>
    /// Primljena aceRemote poruka
    /// </summary>
    public class aceRemoteReceiveMessage : aceRemoteBindable
    {
        public aceRemoteReceiveMessage(string __message, Socket __socket)
        {
            message = __message;
            socket = __socket;
        }

        #region --- message ------- poruka koja se salje

        private string _message;

        /// <summary>
        /// poruka koja se salje
        /// </summary>
        public string message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged("message");
            }
        }

        #endregion --- message ------- poruka koja se salje

        #region --- socket ------- dodeljen socket

        private Socket _socket;

        /// <summary>
        /// dodeljen socket
        /// </summary>
        public Socket socket
        {
            get
            {
                return _socket;
            }
            set
            {
                _socket = value;
                OnPropertyChanged("socket");
            }
        }

        #endregion --- socket ------- dodeljen socket
    }
}