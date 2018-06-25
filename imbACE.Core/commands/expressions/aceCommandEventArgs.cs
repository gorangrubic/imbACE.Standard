// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceCommandEventArgs.cs" company="imbVeles" >
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

namespace imbACE.Core.commands.expressions
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Objekat koji opisuje dogadjaj koji se desio objektu: aceCommand
    /// </summary>
    public class aceCommandEventArgs : dataBindableBase
    {
        public aceCommandEventArgs()
        {
        }

        #region -----------  parameter  -------  [proizvoljni parametar poziva komande]

        private String _parameter = ""; // = new String();

        /// <summary>
        /// proizvoljni parametar poziva komande
        /// </summary>
        // [XmlIgnore]
        [Category("aceCommandEventArgs")]
        [DisplayName("parameter")]
        [Description("proizvoljni parametar poziva komande")]
        public String parameter
        {
            get
            {
                return _parameter;
            }
            set
            {
                // Boolean chg = (_parameter != value);
                _parameter = value;
                OnPropertyChanged("parameter");
                // if (chg) {}
            }
        }

        #endregion -----------  parameter  -------  [proizvoljni parametar poziva komande]

        public aceCommandEventArgs(aceCommandEventType __type, Object __caller, IAceCommand __command, String __parameter = "")
        {
            type = __type;
            parameter = __parameter;
            logableCaller = __caller as IAceLogable;
            command = __command;
        }

        #region -----------  command  -------  [komanda koja je izazvala dogadjaj]

        private IAceCommand _command; // = new IAceCommand();

        /// <summary>
        /// komanda koja je izazvala dogadjaj
        /// </summary>
        // [XmlIgnore]
        [Category("aceCommandEventArgs")]
        [DisplayName("command")]
        [Description("komanda koja je izazvala dogadjaj")]
        public IAceCommand command
        {
            get
            {
                return _command;
            }
            set
            {
                // Boolean chg = (_command != value);
                _command = value;
                OnPropertyChanged("command");
                // if (chg) {}
            }
        }

        #endregion -----------  command  -------  [komanda koja je izazvala dogadjaj]

        #region -----------  caller  -------  [Objekat koji je pozvao komandu]

        private Object _caller; // = new Object();

        /// <summary>
        /// Objekat koji je pozvao komandu
        /// </summary>
        // [XmlIgnore]
        [Category("aceCommandEventArgs")]
        [DisplayName("caller")]
        [Description("Objekat koji je pozvao komandu")]
        public Object caller
        {
            get
            {
                return _caller;
            }
            set
            {
                // Boolean chg = (_caller != value);
                _caller = value;
                OnPropertyChanged("caller");
                // if (chg) {}
            }
        }

        #endregion -----------  caller  -------  [Objekat koji je pozvao komandu]

        #region -----------  logableCaller  -------  [objekat koji je pozvao komandu - a podrzava logovanje]

        private IAceLogable _logableCaller; // = new IAceLogable();

        /// <summary>
        /// objekat koji je pozvao komandu - a podrzava logovanje
        /// </summary>
        // [XmlIgnore]
        [Category("aceCommandEventArgs")]
        [DisplayName("logableCaller")]
        [Description("objekat koji je pozvao komandu - a podrzava logovanje")]
        public IAceLogable logableCaller
        {
            get
            {
                return _logableCaller;
            }
            set
            {
                // Boolean chg = (_logableCaller != value);
                _logableCaller = value;
                OnPropertyChanged("logableCaller");
                // if (chg) {}
            }
        }

        #endregion -----------  logableCaller  -------  [objekat koji je pozvao komandu - a podrzava logovanje]

        #region -----------  type  -------  [tip poziva komande]

        private aceCommandEventType _type; // = new aceCommandEventType();

        /// <summary>
        /// tip poziva komande
        /// </summary>
        // [XmlIgnore]
        [Category("aceCommandEventArgs")]
        [DisplayName("type")]
        [Description("tip poziva komande")]
        public aceCommandEventType type
        {
            get
            {
                return _type;
            }
            set
            {
                // Boolean chg = (_type != value);
                _type = value;
                OnPropertyChanged("type");
                // if (chg) {}
            }
        }

        #endregion -----------  type  -------  [tip poziva komande]
    }
}