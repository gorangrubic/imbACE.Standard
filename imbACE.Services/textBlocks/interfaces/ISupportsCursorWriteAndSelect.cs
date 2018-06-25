// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISupportsCursorWriteAndSelect.cs" company="imbVeles" >
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
    using System;

    public interface ISupportsCursorWriteAndSelect
    {
        /// <summary>
        /// Od trenutne pozicije kursora vraca substring date duzine. Ako je length = -1 onda do desnog kraja dozvoljene zone.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="copyCompleteLine">Da li da iskopira celu liniju</param>
        /// <returns></returns>
        String select(textCursor cursor, Int32 length = -1, Boolean copyCompleteLine = false);

        /// <summary>
        /// upisuje prosledjen unos, primenjuje limit ako je dat - ako nije> limit je u skladu za zonom
        /// </summary>
        /// <param name="input"></param>
        /// <param name="limit"></param>
        /// <param name="writeCompleteLine"></param>
        void write(textCursor cursor, String input, Int32 limit = -1, Boolean writeCompleteLine = false);
    }
}