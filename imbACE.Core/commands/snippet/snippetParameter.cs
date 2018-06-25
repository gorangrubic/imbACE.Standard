// --------------------------------------------------------------------------------------------------------------------
// <copyright file="snippetParameter.cs" company="imbVeles" >
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

namespace imbACE.Core.commands.snippet
{
    #region imbVeles using

    using System;

    #endregion imbVeles using

    /// <summary>
    /// Snippet parametar
    /// </summary>
    [imb(imbAttributeName.collectionPrimaryKey, "ID")]
    public class snippetParameter : imbBindable
    {
        #region --- ID ------- jedinstvena identifikacija

        private String _ID;

        /// <summary>
        /// jedinstvena identifikacija
        /// </summary>
        [imb(imbAttributeName.xmlMapXpath)]
        public String ID
        {
            get { return _ID; }
            set
            {
                _ID = value;
                OnPropertyChanged("ID");
            }
        }

        #endregion --- ID ------- jedinstvena identifikacija

        #region --- ToolTip ------- Tool tip poruka

        private String _ToolTip;

        /// <summary>
        /// Tool tip poruka
        /// </summary>
        [imb(imbAttributeName.xmlMapXpath)]
        public String ToolTip
        {
            get { return _ToolTip; }
            set
            {
                _ToolTip = value;
                OnPropertyChanged("ToolTip");
            }
        }

        #endregion --- ToolTip ------- Tool tip poruka

        #region --- Default ------- Podrazumevana vrednost

        private String _Default;

        /// <summary>
        /// Podrazumevana vrednost
        /// </summary>
        [imb(imbAttributeName.xmlMapXpath)]
        public String Default
        {
            get { return _Default; }
            set
            {
                _Default = value;
                OnPropertyChanged("Default");
            }
        }

        #endregion --- Default ------- Podrazumevana vrednost

        #region --- Function ------- funkcija koja dinamicki daje vrednost

        private String _Function;

        /// <summary>
        /// funkcija koja dinamicki daje vrednost
        /// </summary>
        [imb(imbAttributeName.xmlMapXpath)]
        public String Function
        {
            get { return _Function; }
            set
            {
                _Function = value;
                OnPropertyChanged("Function");
            }
        }

        #endregion --- Function ------- funkcija koja dinamicki daje vrednost
    }
}