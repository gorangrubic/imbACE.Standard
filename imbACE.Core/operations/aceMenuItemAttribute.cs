// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceMenuItemAttribute.cs" company="imbVeles" >
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

namespace imbACE.Core.operations
{
    using System;

    /// <summary>
    /// Operation Annotation attribute used to define how: Command Console, Screen, Console Plugin etc. operation methods are presented in auto-generated user manual, menu TUI, command line args help and ACE Script help
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class |
                    AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property |
                    AttributeTargets.Enum | AttributeTargets.GenericParameter | AttributeTargets.All,
        AllowMultiple = true)]
    public class aceMenuItemAttribute : Attribute
    {
        /// <summary>
        /// Postavlja atribut zajedno sa porukom
        /// </summary>
        /// <param name="_name">Ime podešavanja na koje se odnosi poruka</param>
        /// <param name="_msg">Tekst poruke za podešavanje</param>
        public aceMenuItemAttribute(aceMenuItemAttributeRole __role, String __setting)
        {
            role = __role;
            setting = __setting;
        }

        public String setting = "";

        public aceMenuItemAttributeRole role = aceMenuItemAttributeRole.Category;
    }
}