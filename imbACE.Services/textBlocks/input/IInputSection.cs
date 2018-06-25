// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IInputSection.cs" company="imbVeles" >
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
namespace imbACE.Services.textBlocks.input
{
    using imbACE.Core.core.exceptions;
    using imbACE.Services.platform.input;
    using System;
    using System.ComponentModel;

    public interface IInputSection : ItextLayout
    {
        /// <summary>
        /// Trenutni rezultat
        /// </summary>
// [XmlIgnore]
        [Category("textInputBase")]
        [DisplayName("currentOutput")]
        [Description("Trenutni rezultat")]
        textInputResult currentOutput { get; set; }

        /// <summary>
        /// Da li prikazuje naslov inputa
        /// </summary>
        [Category("Switches")]
        [DisplayName("doShowTitle")]
        [Description("Da li prikazuje naslov inputa")]
        Boolean doShowTitle { get; set; }

        /// <summary>
        /// Da li da prikazuje komentar - trenutna vrednost / selektovana opcija
        /// </summary>
        [Category("Switches")]
        [DisplayName("doShowRemarks")]
        [Description("Da li da prikazuje komentar - trenutna vrednost / selektovana opcija")]
        Boolean doShowRemarks { get; set; }

        /// <summary>
        /// Da li prikazuje komentar na trenutnu vrednost / selektovanu opcijua
        /// </summary>
        [Category("Switches")]
        [DisplayName("doShowValueRemats")]
        [Description("Da li prikazuje komentar na trenutnu vrednost / selektovanu opcijua")]
        Boolean doShowValueRemarks { get; set; }

        /// <summary>
        /// Da li da prikazuje instrukcije> npr. izaberi opciju strelicama, potvrdi enter
        /// </summary>
        [Category("Switches")]
        [DisplayName("doShowInstructions")]
        [Description("Da li da prikazuje instrukcije> npr. izaberi opciju strelicama, potvrdi enter")]
        Boolean doShowInstructions { get; set; }

        /// <summary>
        /// nacin zatvaranja inputa
        /// </summary>
// [XmlIgnore]
        [Category("textInputBase")]
        [DisplayName("exitPolicy")]
        [Description("nacin zatvaranja inputa")]
        textInputExitPolicy exitPolicy { get; set; }
    }
}