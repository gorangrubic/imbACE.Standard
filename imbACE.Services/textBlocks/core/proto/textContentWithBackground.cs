// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textContentWithBackground.cs" company="imbVeles" >
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
namespace imbACE.Services.textBlocks.core.proto
{
    using imbACE.Core.core.exceptions;
    using System;

    public abstract class textContentWithBackground : textContentBasicBlock
    {
        #region --- backgroundDecoration ------- uzorak stringa koji se koristi za pozadinsku dekoraciju

        private String _backgroundDecoration = "=";

        protected textContentWithBackground(String __mgDecoration, String __bgDecoration, int __width, int __height, int __leftRightMargin, int __topBottomMargin, int __leftRightPadding, int __topBottomPadding)
            : base(__width, __height, __leftRightMargin, __topBottomMargin, __leftRightPadding, __topBottomPadding)
        {
            if (!String.IsNullOrEmpty(__bgDecoration))
            {
                _backgroundDecoration = __bgDecoration;
            }
            if (!String.IsNullOrEmpty(__mgDecoration))
            {
                marginDecoration = __mgDecoration;
            }
        }

        protected textContentWithBackground(String __mgDecoration, String __bgDecoration, int __width, int __leftRightMargin, int __leftRightPadding)
            : base(__width, __leftRightMargin, __leftRightPadding)
        {
            if (!String.IsNullOrEmpty(__bgDecoration))
            {
                _backgroundDecoration = __bgDecoration;
            }

            if (!String.IsNullOrEmpty(__mgDecoration))
            {
                marginDecoration = __mgDecoration;
            }
        }

        /// <summary>
        /// uzorak stringa koji se koristi za pozadinsku dekoraciju
        /// </summary>
        public String backgroundDecoration
        {
            get
            {
                return _backgroundDecoration;
            }
            set
            {
                _backgroundDecoration = value;
                OnPropertyChanged("backgroundDecoration");
            }
        }

        #endregion --- backgroundDecoration ------- uzorak stringa koji se koristi za pozadinsku dekoraciju

        #region --- marginDecoration ------- dekoracijaMargine

        private String _marginDecoration = "";

        /// <summary>
        /// dekoracijaMargine
        /// </summary>
        public String marginDecoration
        {
            get
            {
                return _marginDecoration;
            }
            set
            {
                _marginDecoration = value;
                OnPropertyChanged("marginDecoration");
            }
        }

        #endregion --- marginDecoration ------- dekoracijaMargine
    }
}