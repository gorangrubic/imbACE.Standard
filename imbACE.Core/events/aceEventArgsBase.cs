// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceEventArgsBase.cs" company="imbVeles" >
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
using imbACE.Core.collection;
using imbACE.Core.core.exceptions;
using imbACE.Core.extensions;

using imbACE.Core.extensions;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace imbACE.Core.events
{
    using imbSCI.Data.data;

    /// <summary>
    /// 2013c: Osnovna klasa Event Args-ova koji se koriste u imbVeles sistemu
    /// </summary>
    public abstract class aceEventArgsBase : imbBindable
    {
        #region --- Ex ------- Beleska o exception

        private Exception _Ex;

        /// <summary>
        /// Beleska o exception
        /// </summary>
        public Exception Ex
        {
            get { return _Ex; }
            set
            {
                _Ex = value;
                OnPropertyChanged("Ex");
            }
        }

        #endregion --- Ex ------- Beleska o exception

        private String _eventTypeName = "unknown";
        // private logType _lastLogType = logType.Debug;

        private String _message = "";

        protected Object _type = null;

        protected aceEventArgsBase(Object __type = null, String __message = null, Exception __ex = null)
        {
            _type = __type;
            message = __message;
            Ex = __ex;
            //processEvent();
        }

        /// <summary>
        /// Da li je doslo do greske u izvrsavanju? - ovo i nije bas pouzdano
        /// </summary>
        public Boolean isError
        {
            get
            {
                if (Ex != null) return true;
                return false;
                //return lastLogType.ToString().ToLower().Contains("error");
            }
        }

        /// <summary>
        /// Tekstualna poruka koja opisuje dogadjaj
        /// </summary>
        public String message
        {
            get { return _message; }
            set
            {
                Boolean chg = (_message != value);
                _message = value;
                OnPropertyChanged("message");
            }
        }

        #region ILogStatusInformation Members

        //public logType lastLogType
        //{
        //    get { return _lastLogType; }
        //    set { _lastLogType = value; }
        //}

        public string lastLogMessage
        {
            get { return message; }
            set { message = value; }
        }

        #endregion ILogStatusInformation Members
    }
}