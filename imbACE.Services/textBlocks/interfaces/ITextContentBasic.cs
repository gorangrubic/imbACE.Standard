// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextContentBasic.cs" company="imbVeles" >
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
    using imbACE.Core.enums.platform;
    using imbACE.Services.platform.core;
    using imbACE.Services.textBlocks.enums;
    using System;

    public interface ITextContentBasic
    {
        String[] getContent();

        /// <summary>
        /// Z redosled sloja u kome se nalazi ovaj blok - 0 se prvo renderuje, 100 poslednji
        /// </summary>
        Int32 ZLayerOrder { get; set; }

        /// <summary>
        /// vrsta blendovanja
        /// </summary>
        layerBlending blending { get; set; }

        /// <summary>
        /// boja slova
        /// </summary>
        platformColorName foreColor { get; set; }

        /// <summary>
        /// pozadinska boja
        /// </summary>
        platformColorName backColor { get; set; }

        /// <summary>
        /// da li da invertuje boje
        /// </summary>
        Boolean doInverseColors { get; set; }
    }
}