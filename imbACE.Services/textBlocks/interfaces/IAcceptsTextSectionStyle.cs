// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAcceptsTextSectionStyle.cs" company="imbVeles" >
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
namespace imbACE.Services.textBlocks.interfaces
{
    using imbACE.Core.core.exceptions;
    using imbACE.Services.textBlocks.core;
    using imbACE.Services.textBlocks.enums;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;

    public interface IAcceptsTextSectionStyle : ITextContentBasic
    {
        /// <summary>
        /// sirina levog polja
        /// </summary>
        Int32 leftFieldWidth { get; set; }

        /// <summary>
        /// sirina desnog polja
        /// </summary>
        Int32 rightFieldWidth { get; set; }

        Dictionary<printHorizontal, String> fieldFormats { get; set; }

        /// <summary>
        /// uzorak stringa koji se koristi za pozadinsku dekoraciju
        /// </summary>
        String backgroundDecoration { get; set; }

        /// <summary>
        /// dekoracijaMargine
        /// </summary>
        String marginDecoration { get; set; }

        /// <summary>
        /// koji pod stil se primenjuje
        /// </summary>
        Boolean doInverseColors { get; set; }
    }
}