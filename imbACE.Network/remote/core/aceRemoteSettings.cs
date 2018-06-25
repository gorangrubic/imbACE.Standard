// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceRemoteSettings.cs" company="imbVeles" >
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
    using imbACE.Core.core;
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

    /// <summary>
    /// aceRemote settings
    /// </summary>
    public class aceRemoteSettings : aceSettingsBase
    {
        #region --- bufferSize ------- Number of bytes reserved for buffer size

        private int _bufferSize = 256;

        /// <summary>
        /// Number of bytes reserved for buffer size
        /// </summary>
        public int bufferSize
        {
            get
            {
                return _bufferSize;
            }
            set
            {
                _bufferSize = value;
                OnPropertyChanged("bufferSize");
            }
        }

        #endregion --- bufferSize ------- Number of bytes reserved for buffer size

        #region --- serverIP ------- IP Adresa servera sa kojim komunicira

        private string _serverIP = "192.168.1.65";

        /// <summary>
        /// IP Adresa servera sa kojim komunicira
        /// </summary>
        public string serverIP
        {
            get
            {
                return _serverIP;
            }
            set
            {
                _serverIP = value;
                OnPropertyChanged("serverIP");
            }
        }

        #endregion --- serverIP ------- IP Adresa servera sa kojim komunicira

        #region --- serverPort ------- PORT koji slusa server

        private int _serverPort = 11000;

        /// <summary>
        /// PORT koji slusa server
        /// </summary>
        public int serverPort
        {
            get
            {
                return _serverPort;
            }
            set
            {
                _serverPort = value;
                OnPropertyChanged("serverPort");
            }
        }

        #endregion --- serverPort ------- PORT koji slusa server

        #region --- endMessageSufix ------- sufix koji se dodaje na kraj poruke kako bi oznacio jednu transakciju

        private string _endMessageSufix = ";";

        /// <summary>
        /// sufix koji se dodaje na kraj poruke kako bi oznacio jednu transakciju
        /// </summary>
        public string endMessageSufix
        {
            get
            {
                return _endMessageSufix;
            }
            set
            {
                _endMessageSufix = value;
                OnPropertyChanged("endMessageSufix");
            }
        }

        #endregion --- endMessageSufix ------- sufix koji se dodaje na kraj poruke kako bi oznacio jednu transakciju
    }
}