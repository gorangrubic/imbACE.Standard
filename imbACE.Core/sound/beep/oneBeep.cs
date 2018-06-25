// --------------------------------------------------------------------------------------------------------------------
// <copyright file="oneBeep.cs" company="imbVeles" >
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
// Project: imbACE.Core
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

namespace imbACE.Core.sound.beep
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Definition of one sound beep
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class oneBeep : INotifyPropertyChanged
    {
        public oneBeep(beepTone tone, beepDuration __duration = beepDuration.half)
        {
            frequency = tone.toFrequency();
            duration = __duration;

            // __duration.toMilisecond(tempo);
        }

        #region --- frequency ------- Frekvencija beepa

        private Int32 _frequency = 110;

        /// <summary>
        /// Frekvencija beepa
        /// </summary>
        public Int32 frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                _frequency = value;
                OnPropertyChanged("frequency");
            }
        }

        #endregion --- frequency ------- Frekvencija beepa

        #region --- duration ------- in miliseconds

        private Int32 _durationms;

        /// <summary>
        /// in miliseconds
        /// </summary>
        public Int32 durationms
        {
            get
            {
                return _durationms;
            }
            set
            {
                _durationms = value;
                OnPropertyChanged("durationms");
            }
        }

        #endregion --- duration ------- in miliseconds

        //#region --- duration ------- Bindable property

        private beepDuration _duration = beepDuration.half;

        /// <summary>
        /// Bindable property
        /// </summary>
        public beepDuration duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
                OnPropertyChanged("duration");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}