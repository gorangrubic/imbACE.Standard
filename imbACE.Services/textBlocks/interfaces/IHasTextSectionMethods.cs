// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHasTextSectionMethods.cs" company="imbVeles" >
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
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using System;

    public interface IHasTextSectionMethods : IHasCursor, ITextContentBasic
    {
        /// <summary>
        /// Upisuje jedno polje u dati tab, omogucava custom format za polje
        /// </summary>
        /// </summary>
        /// <param name="content"></param>
        /// <param name="tab"></param>
        /// <param name="toInnerLine"></param>
        /// <param name="customFormat"></param>
        /// <param name="customBackground"></param>
        /// <returns></returns>
        String writeField(String content, printHorizontal tab, Int32 toInnerLine = -1, String customFormat = "", String customBackground = "");

        /// <summary>
        /// Upisuje liniju ili linije (automatski wrappuje)
        /// </summary>
        /// <param name="content"></param>
        /// <param name="toInnerLine"></param>
        /// <param name="doMoveToNextLine">Da li nakon ispisivanja pomera kursor na sledeci red</param>
        /// <param name="limitLineCount">Koliko linija najvise moze da ubaci na osnovu datog contenta</param>
        /// <returns>Koliko je linija ispisao nakon obrade content parametra</returns>
        Int32 writeLine(String content, Int32 toInnerLine = -1, Boolean doMoveToNextLine = true, Int32 limitLineCount = 5);

        /// <summary>
        /// Ubacuje liniju ili linije
        /// </summary>
        /// <param name="content"></param>
        /// <param name="toInnerLine"></param>
        /// <param name="doMoveToNextLine"></param>
        /// <param name="limitLineCount"></param>
        /// <returns></returns>
        Int32 insertLine(String content, Int32 toInnerLine = -1, Boolean doMoveToNextLine = true, Int32 limitLineCount = 5);

        /// <summary>
        /// Ubacuje spliter liniju i pomera kursor na sledeci red
        /// </summary>
        /// <param name="__backgroundDeco"></param>
        /// <param name="__marginDeco"></param>
        /// <param name="toInnerLine"></param>
        void insertSplitLine(String __backgroundDeco = "-", String __marginDeco = "", Int32 toInnerLine = -1);

        void setStyle(textSectionLineStyleName __styleName);

        /// <summary>
        /// Podesava formatiranje polja
        /// </summary>
        /// <param name="defaultFormat">Format koji se primenjuje u svaki insert</param>
        /// <param name="__rightField">Desna kolona</param>
        /// <param name="__leftField">Leva kolona</param>
        void setupFieldFormat(String defaultFormat, Int32 __rightField = 0, Int32 __leftField = 0);
    }
}