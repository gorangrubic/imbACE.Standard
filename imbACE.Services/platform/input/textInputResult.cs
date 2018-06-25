// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textInputResult.cs" company="imbVeles" >
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
// Project: imbACE.Services
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
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

namespace imbACE.Services.platform.input
{
    using imbACE.Core.enums.platform;
    using imbACE.Core.operations;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Core.reporting.zone;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Klasa koja sadr�i odgovor na text input
    /// </summary>
    public class textInputResult : INotifyPropertyChanged
    {
        private List<object> _meta = new List<object>();

        /// <summary>
        /// Kolekcija razlicitih meta objekata
        /// </summary>
        public List<object> meta
        {
            get
            {
                return _meta;
            }
            set
            {
                _meta = value;
                OnPropertyChanged("meta");
            }
        }

        #region --- consoleKey ------- referenca prema konzolnom tasteru koji je pritisnut

        private ConsoleKeyInfo _consoleKey;

        /// <summary>
        /// referenca prema konzolnom tasteru koji je pritisnut
        /// </summary>
        public ConsoleKeyInfo consoleKey
        {
            get
            {
                return _consoleKey;
            }
            set
            {
                _consoleKey = value;
                OnPropertyChanged("consoleKey");
            }
        }

        #endregion --- consoleKey ------- referenca prema konzolnom tasteru koji je pritisnut

        #region --- result ------- Primarni rezultat inputa

        private Object _result;

        /// <summary>
        /// Primarni rezultat inputa
        /// </summary>
        public Object result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
                OnPropertyChanged("result");
            }
        }

        #endregion --- result ------- Primarni rezultat inputa

        public textInputResult(inputReadMode __readMode = inputReadMode.unknown)
        {
            readMode = __readMode;
        }

        public textInputResult(IPlatform __platform, inputReadMode __readMode, selectZone __readZone)
        {
            platform = __platform;
            readMode = __readMode;
            readZone = __readZone;
        }

        public IPlatform platform;
        public inputReadMode readMode = inputReadMode.unknown;
        public selectZone readZone;

        #region --- section ------- section koji je inicirao ovo citanje

        private ITextLayoutContentProvider _section;

        /// <summary>
        /// section koji je inicirao ovo citanje
        /// </summary>
        public ITextLayoutContentProvider section
        {
            get
            {
                return _section;
            }
            set
            {
                _section = value;
                OnPropertyChanged("section");
            }
        }

        #endregion --- section ------- section koji je inicirao ovo citanje

        #region --- doKeepReading ------- da li da nastavlja sa citanjem [true] ili je zavrsio [false]

        private Boolean _doKeepReading = true;

        /// <summary>
        /// da li da nastavlja sa citanjem [true] ili je zavrsio [false]
        /// </summary>
        public Boolean doKeepReading
        {
            get
            {
                return _doKeepReading;
            }
            set
            {
                _doKeepReading = value;
                OnPropertyChanged("doKeepReading");
            }
        }

        #endregion --- doKeepReading ------- da li da nastavlja sa citanjem [true] ili je zavrsio [false]

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}