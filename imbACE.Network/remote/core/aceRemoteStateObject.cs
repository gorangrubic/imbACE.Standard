// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceRemoteStateObject.cs" company="imbVeles" >
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
    using System.Text;

    /// <summary>
    /// StateObject -- for inter-thread communication
    /// </summary>
    public class aceRemoteStateObject : aceRemoteBindable
    {
        public aceRemoteStateObject(int __buffersize)
        {
            buffer = new byte[__buffersize];
        }

        #region --- workSocket ------- trenutni socket objekat

        private Socket _workSocket;

        /// <summary>
        /// trenutni socket objekat
        /// </summary>
        public Socket workSocket
        {
            get
            {
                return _workSocket;
            }
            set
            {
                _workSocket = value;
                OnPropertyChanged("workSocket");
            }
        }

        #endregion --- workSocket ------- trenutni socket objekat

        #region --- buffer ------- bafer za komunikaciju

        private byte[] _buffer;

        /// <summary>
        /// bafer za komunikaciju
        /// </summary>
        public byte[] buffer
        {
            get
            {
                return _buffer;
            }
            set
            {
                _buffer = value;
                OnPropertyChanged("buffer");
            }
        }

        #endregion --- buffer ------- bafer za komunikaciju

        #region --- sb ------- string builder

        private StringBuilder _sb = new StringBuilder();

        /// <summary>
        /// string builder
        /// </summary>
        public StringBuilder sb
        {
            get
            {
                return _sb;
            }
            set
            {
                _sb = value;
                OnPropertyChanged("sb");
            }
        }

        #endregion --- sb ------- string builder
    }
}