// --------------------------------------------------------------------------------------------------------------------
// <copyright file="platformColorName.cs" company="imbVeles" >
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

namespace imbACE.Core.enums.platform
{
    public enum platformColorName
    {
        none,

        /// <summary>
        /// The color black.
        /// </summary>

        Black,

        /// <summary>
        /// The color dark blue.
        /// </summary>

        DarkBlue,

        /// <summary>
        /// The color dark green.
        /// </summary>

        DarkGreen,

        /// <summary>
        /// The color dark cyan (dark blue-green).
        /// </summary>

        DarkCyan,

        /// <summary>
        /// The color dark red.
        /// </summary>

        DarkRed,

        /// <summary>
        /// The color dark magenta (dark purplish-red).
        /// </summary>

        DarkMagenta,

        /// <summary>
        /// The color dark yellow (ochre).
        /// </summary>

        DarkYellow,

        /// <summary>
        /// The color gray.
        /// </summary>

        Gray,

        /// <summary>
        /// The color dark gray.
        /// </summary>

        DarkGray,

        /// <summary>
        /// The color blue.
        /// </summary>

        Blue,

        /// <summary>
        /// The color green.
        /// </summary>

        Green,

        /// <summary>
        /// The color cyan (blue-green).
        /// </summary>

        Cyan,

        /// <summary>
        /// The color red.
        /// </summary>

        Red,

        /// <summary>
        /// The color magenta (purplish-red).
        /// </summary>

        Magenta,

        /// <summary>
        /// The color yellow.
        /// </summary>

        Yellow,

        /// <summary>
        /// The color white.
        /// </summary>

        White,
    }
}