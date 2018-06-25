// --------------------------------------------------------------------------------------------------------------------
// <copyright file="commandExpressionFormat.cs" company="imbVeles" >
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
    using imbACE.Core.core;
    using System;
    using System.ComponentModel;

    public class commandExpressionFormat : aceBindable
    {
        #region -----------  commandPrefixString  -------  [String koji ide pre navodjenja komande]

        private String _commandPrefixString = "-"; // = new String();

        /// <summary>
        /// String koji ide pre navodjenja komande
        /// </summary>
        // [XmlIgnore]
        [Category("commandExpressionFormat")]
        [DisplayName("commandPrefixString")]
        [Description("String koji ide pre navodjenja komande")]
        public String commandPrefixString
        {
            get
            {
                return _commandPrefixString;
            }
            set
            {
                // Boolean chg = (_commandPrefixString != value);
                _commandPrefixString = value;
                OnPropertyChanged("commandPrefixString");
                // if (chg) {}
            }
        }

        #endregion -----------  commandPrefixString  -------  [String koji ide pre navodjenja komande]

        #region -----------  commandParameterSpliterRegex  -------  [regex for spliting params]

        private String _commandParameterSpliterRegex; // = new String();

        /// <summary>
        /// Description of $property$
        /// </summary>
        // [XmlIgnore]
        [Category("commandExpressionFormat")]
        [DisplayName("commandParameterSpliterRegex")]
        [Description("Description of $property$")]
        public String commandParameterSpliterRegex
        {
            get
            {
                return _commandParameterSpliterRegex;
            }
            set
            {
                // Boolean chg = (_commandParameterSpliterRegex != value);
                _commandParameterSpliterRegex = value;
                OnPropertyChanged("commandParameterSpliterRegex");
                // if (chg) {}
            }
        }

        #endregion -----------  commandParameterSpliterRegex  -------  [regex for spliting params]

        #region -----------  commandParameterSpliter  -------  [String spliter koji deli parametre]

        private String _commandParameterSpliter = " "; // = new String();

        /// <summary>
        /// String spliter koji deli parametre
        /// </summary>
        // [XmlIgnore]
        [Category("commandExpressionFormat")]
        [DisplayName("commandParameterSpliter")]
        [Description("String spliter koji deli parametre")]
        public String commandParameterSpliter
        {
            get
            {
                return _commandParameterSpliter;
            }
            set
            {
                // Boolean chg = (_commandParameterSpliter != value);
                _commandParameterSpliter = value;
                OnPropertyChanged("commandParameterSpliter");
                // if (chg) {}
            }
        }

        #endregion -----------  commandParameterSpliter  -------  [String spliter koji deli parametre]
    }
}