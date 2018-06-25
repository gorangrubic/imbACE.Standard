// --------------------------------------------------------------------------------------------------------------------
// <copyright file="menuRenderFlag.cs" company="imbVeles" >
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
using System;

namespace imbACE.Core.commands.menu.render
{
    /// <summary>
    /// Flagovi - menuRenderFlag
    /// </summary>
    [Flags]
    public enum menuRenderFlag
    {
        none = 0,

        /// <summary>
        /// Izvrsice ucitavanje samo ako isti URL / komanda nije poslednja koja je izvrsena
        /// </summary>
        loadOnlyIfNotLoaded = 1,

        onlyNumber = 2,

        numberOrKey = 4,
        numberAndKey = 8,

        /// <summary>
        /// Da li prikazuje selectionBox ispred menija
        /// </summary>
        showSelectionBox = 16,

        /// <summary>
        /// da li prikazuje ako je defailt
        /// </summary>
        showIfDefault = 32,

        /// <summary>
        /// pokazuje napomene u liniji
        /// </summary>
        showInlineRemarks = 64,

        /// <summary>
        /// pokazuje napomenu za selektovan item
        /// </summary>
        showSelectedRemarks = 128,

        fullWidthLineUnder = 256,
        fullWidthLineAbove = 512,

        inlineItems = 1024,
        listItems = 2048,
    }
}