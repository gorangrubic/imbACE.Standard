// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbNLPsettings.cs" company="imbVeles" >
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

namespace imbACE.Core.interfaces.primitives
{
    #region imbVeles using

    using imbACE.Core.enums;
    using imbSCI.Data.data;
    using System;
    using System.ComponentModel;

    #endregion imbVeles using

    /// <summary>
    /// Podeševanja imbNLP filtera
    /// </summary>
    public class imbNLPsettings : imbBindable
    {
        #region -----------  splitLevel  -------  [Podrazumevani split nivo]

        private defaultSplitingLevel _splitLevel = defaultSplitingLevel.tokenBased; // = new defaultSplitingLevel();

        /// <summary>
        /// Podrazumevani split nivo
        /// </summary>
        // [XmlIgnore]
        [Category("Tokenize")]
        [DisplayName("splitLevel")]
        [Description("Podrazumevani split nivo")]
        public defaultSplitingLevel splitLevel
        {
            get { return _splitLevel; }
            set
            {
                _splitLevel = value;
                OnPropertyChanged("splitLevel");
            }
        }

        #endregion -----------  splitLevel  -------  [Podrazumevani split nivo]

        #region -----------  minLength  -------  [Minimalna sirina tokena - ako je 0 onda se ne primenjuje]

        private Int32 _minLength = 2;

        /// <summary>
        /// Minimalna sirina tokena - ako je 0 onda se ne primenjuje
        /// </summary>
        // [XmlIgnore]
        [Category("Filter")]
        [DisplayName("minLength")]
        [Description("Minimalna sirina tokena - ako je 0 onda se ne primenjuje")]
        public Int32 minLength
        {
            get { return _minLength; }
            set
            {
                _minLength = value;
                OnPropertyChanged("minLength");
            }
        }

        #endregion -----------  minLength  -------  [Minimalna sirina tokena - ako je 0 onda se ne primenjuje]

        #region -----------  toLowerCase  -------  [Svi tokeni da idu u lowerCase]

        private Boolean _toLowerCase = true;

        /// <summary>
        /// Svi tokeni da idu u lowerCase
        /// </summary>
        // [XmlIgnore]
        [Category("Filter")]
        [DisplayName("toLowerCase")]
        [Description("Svi tokeni da idu u lowerCase")]
        public Boolean toLowerCase
        {
            get { return _toLowerCase; }
            set
            {
                _toLowerCase = value;
                OnPropertyChanged("toLowerCase");
            }
        }

        #endregion -----------  toLowerCase  -------  [Svi tokeni da idu u lowerCase]

        #region -----------  trimTokens  -------  [Da trimuje space oko tokena]

        private Boolean _trimTokens = true;

        /// <summary>
        /// Da trimuje space oko tokena
        /// </summary>
        // [XmlIgnore]
        [Category("Filter")]
        [DisplayName("trimTokens")]
        [Description("Da trimuje space oko tokena")]
        public Boolean trimTokens
        {
            get { return _trimTokens; }
            set
            {
                _trimTokens = value;
                OnPropertyChanged("trimTokens");
            }
        }

        #endregion -----------  trimTokens  -------  [Da trimuje space oko tokena]

        #region -----------  onlyUnique  -------  [Samo jedinstveni tokeni]

        private Boolean _onlyUnique = true;

        /// <summary>
        /// Samo jedinstveni tokeni
        /// </summary>
        // [XmlIgnore]
        [Category("Filter")]
        [DisplayName("onlyUnique")]
        [Description("Samo jedinstveni tokeni")]
        public Boolean onlyUnique
        {
            get { return _onlyUnique; }
            set
            {
                _onlyUnique = value;
                OnPropertyChanged("onlyUnique");
            }
        }

        #endregion -----------  onlyUnique  -------  [Samo jedinstveni tokeni]
    }
}