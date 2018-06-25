// --------------------------------------------------------------------------------------------------------------------
// <copyright file="commandExpression.cs" company="imbVeles" >
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
    using System.Linq;

    public class commandExpression : aceBindable
    {
        #region -----------  isValidExpression  -------  [Da li je u pitanju validni komandni izraz]

        private Boolean _isValidExpression = false; // = new Boolean();

        /// <summary>
        /// Da li je u pitanju validni komandni izraz
        /// </summary>
        // [XmlIgnore]
        [Category("commandExpression")]
        [DisplayName("isValidExpression")]
        [Description("Da li je u pitanju validni komandni izraz")]
        public Boolean isValidExpression
        {
            get
            {
                return _isValidExpression;
            }
            set
            {
                // Boolean chg = (_isValidExpression != value);
                _isValidExpression = value;
                OnPropertyChanged("isValidExpression");
                // if (chg) {}
            }
        }

        #endregion -----------  isValidExpression  -------  [Da li je u pitanju validni komandni izraz]

        public commandExpression(String __identifier)
        {
            identifier = __identifier.Trim();
        }

        public commandExpression(String inputline, commandExpressionFormat inputformat)
        {
            inputline = inputline.Trim();
            var commandStart = inputline.IndexOf(inputformat.commandPrefixString);
            if (commandStart > -1)
            {
                inputline = inputline.Substring(commandStart);
            }

            String[] parts = inputline.Split(inputformat.commandParameterSpliter.ToCharArray(),
                                             StringSplitOptions.RemoveEmptyEntries);
            if (!parts.Any()) parts = new string[] { inputline };

            identifier = parts[0];

            for (int i = 1; i < parts.Length; i++)
            {
                parameters[i - 1] = parts[i];
            }

            if (String.IsNullOrEmpty(identifier))
            {
                isValidExpression = false;
            }
            else
            {
                isValidExpression = true;
            }
        }

        #region -----------  parameters  -------  [redom parametri]

        private String[] _parameters = new String[] { };

        // = new List<String>();
        /// <summary>
        /// redom parametri
        /// </summary>
        // [XmlIgnore]
        [Category("commandExpression")]
        [DisplayName("parameters")]
        [Description("redom parametri")]
        public String[] parameters
        {
            get
            {
                return _parameters;
            }
            set
            {
                // Boolean chg = (_parameters != value);
                _parameters = value;
                OnPropertyChanged("parameters");
                // if (chg) {}
            }
        }

        #endregion -----------  parameters  -------  [redom parametri]

        #region -----------  identifier  -------  [identificator komande]

        private String _identifier; // = new String();

        /// <summary>
        /// identificator komande
        /// </summary>
        // [XmlIgnore]
        [Category("commandExpression")]
        [DisplayName("identifier")]
        [Description("identificator komande")]
        public String identifier
        {
            get
            {
                return _identifier;
            }
            set
            {
                // Boolean chg = (_identifier != value);
                _identifier = value;
                OnPropertyChanged("identifier");
                // if (chg) {}
            }
        }

        #endregion -----------  identifier  -------  [identificator komande]
    }
}