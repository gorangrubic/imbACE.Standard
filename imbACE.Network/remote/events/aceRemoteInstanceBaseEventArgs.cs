// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceRemoteInstanceBaseEventArgs.cs" company="imbVeles" >
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
namespace imbACE.Network.remote.events
{
    using imbACE.Core;
    using imbACE.Network.remote.core;
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
    /// Objekat koji opisuje dogadjaj koji se desio objektu: aceRemoteInstanceBase
    /// </summary>
    public class aceRemoteInstanceBaseEventArgs : aceRemoteBindable
    {
        public aceRemoteInstanceBaseEventArgs()
        {
        }

        public aceRemoteInstanceBaseEventArgs(aceRemoteInstanceBaseEventType __type, string __message = "", Socket __socket = null)
        {
            type = __type;
            message = __message;
            socket = __socket;
        }

        #region --- socket ------- relevantan socket

        private Socket _socket;

        /// <summary>
        /// relevantan socket
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

        #endregion --- socket ------- relevantan socket

        #region --- message ------- event msg

        private string _message;

        /// <summary>
        /// event msg
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

        #endregion --- message ------- event msg

        #region --- type ------- tip dogadjaja

        private aceRemoteInstanceBaseEventType _type;

        /// <summary>
        /// tip dogadjaja
        /// </summary>
        public aceRemoteInstanceBaseEventType type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                OnPropertyChanged("type");
            }
        }

        #endregion --- type ------- tip dogadjaja
    }
}