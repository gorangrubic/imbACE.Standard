// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textInputMenuRenderView.cs" company="imbVeles" >
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
    public enum textInputMenuRenderView
    {
        /// <summary>
        /// Lista mogucih opcija sa prikazom trenutne vrednosti na dnu u posebnom redu
        /// </summary>
        listKeyItem,

        /// <summary>
        /// List mogucih opcija sa selekt prefixom
        /// </summary>
        listItemSelectable,

        /// <summary>
        /// strelicama se menja selektovan item - nema spiska svih itema
        /// </summary>
        inlineItemsHidden,

        /// <summary>
        /// iteme pakuje u liniju i radi line wrap
        /// </summary>
        inlineKeyListGroup,
    }
}