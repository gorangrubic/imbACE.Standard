// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITextLayoutContentProvider.cs" company="imbVeles" >
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
    using System;

    public interface ITextLayoutContentProvider : IAcceptsTextSectionStyle, ISupportsTextCursor, ITextContentBasic
    {
        /// <summary>
        /// Z redosled sloja u kome se nalazi ovaj blok - 0 se prvo renderuje, 100 poslednji
        /// </summary>
        Int32 ZLayerOrder { get; set; }

        /// <summary>
        /// vrsta blendovanja
        /// </summary>
      //  layerBlending blending { get; set; }

        /// <summary>
        /// Brise sav zadrzaj i renderuje pozadinu
        /// </summary>
        void resetContent();

        ///// <summary>
        /////
        ///// </summary>
        //void rebuildContent();

        //string[] getContent();

        ///// <summary>
        ///// maksimalna spoljna sirina formata (innerWidth+padding+margin = Windows.width)
        ///// </summary>
        //Int32 width { get; set; }

        ///// <summary>
        ///// maksimalna spoljna visina formata (innerHeight+padding+margin=Windows.Height)
        ///// </summary>
        //Int32 height { get; set; }

        /// <summary>
        /// Vraca section koji je "nakacen" na ovaj. Nakachen section se automatski RRE obradjuje pri pozivanju ovog sectiona
        /// </summary>
        /// <param name="isBottomAttachment"></param>
        /// <returns></returns>
        ITextLayoutContentProvider getAttachment(Boolean isBottomAttachment = true);

        /// <summary>
        /// Kaci prosledjeni section na ovaj - zajedno ce biti renderovani
        /// </summary>
        /// <param name="__sectionToAttach">Sekcija koja se kaci na ovu</param>
        /// <param name="isBottomAttachment">Ako je TRUE onda ga dodaje sa donje strane</param>
        /// <returns>Vraca sekciju koja je bila ranije attachovana, ako je NULL onda je prvi put da se nesto attachuje</returns>
        ITextLayoutContentProvider setAttachment(ITextLayoutContentProvider __sectionToAttach, Boolean isBottomAttachment = true);

        void refreshAttachmentPosition();

        Int32 getAttachmentTotalHeight(Boolean isBottomAttachment = true, Int32 h = 0);

        Int32 getAttachmentHeight(Boolean isBottomAttachment = true, Int32 h = 0);
    }
}