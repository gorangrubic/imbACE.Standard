// --------------------------------------------------------------------------------------------------------------------
// <copyright file="commandBase.cs" company="imbVeles" >
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

    public abstract class commandBase : dataBindableBase, IAceCommand
    {
        #region -----------  commandShortName  -------  [character ili kratak string]

        private String _commandShortName; // = new String();

        /// <summary>
        /// character ili kratak string
        /// </summary>
        // [XmlIgnore]
        [Category("commandBase")]
        [DisplayName("commandShortName")]
        [Description("character ili kratak string")]
        public String commandShortName
        {
            get
            {
                return _commandShortName;
            }
            set
            {
                // Boolean chg = (_commandShortName != value);
                _commandShortName = value;
                OnPropertyChanged("commandShortName");
                // if (chg) {}
            }
        }

        #endregion -----------  commandShortName  -------  [character ili kratak string]

        #region -----------  commandFullname  -------  [Dugacak naziv komande - opisnog tipa]

        private String _commandFullname; // = new String();

        /// <summary>
        /// Dugacak naziv komande - opisnog tipa
        /// </summary>
        // [XmlIgnore]
        [Category("commandBase")]
        [DisplayName("commandFullname")]
        [Description("Dugacak naziv komande - opisnog tipa")]
        public String commandFullname
        {
            get
            {
                return _commandFullname;
            }
            set
            {
                // Boolean chg = (_commandFullname != value);
                _commandFullname = value;
                OnPropertyChanged("commandFullname");
                // if (chg) {}
            }
        }

        #endregion -----------  commandFullname  -------  [Dugacak naziv komande - opisnog tipa]

        #region -----------  description  -------  [Opis komande]

        private String _description = ""; // = new String();

        /// <summary>
        /// Opis komande
        /// </summary>
        // [XmlIgnore]
        [Category("commandBase")]
        [DisplayName("description")]
        [Description("Opis komande")]
        public String description
        {
            get
            {
                return _description;
            }
            set
            {
                // Boolean chg = (_description != value);
                _description = value;
                OnPropertyChanged("description");
                // if (chg) {}
            }
        }

        #endregion -----------  description  -------  [Opis komande]

        public event aceCommandEvent onCommandCalled;

        public void callCommand(aceCommandEventArgs _args)
        {
            onCommandCalled(this, _args);
        }
    }
}