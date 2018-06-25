// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textSectionLineStyles.cs" company="imbVeles" >
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
namespace imbACE.Services.textBlocks.core
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.enums.platform;
    using imbACE.Services.platform.core;
    using imbACE.Services.textBlocks.enums;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Kolekcija stilova -- automatski generise standardne stilove
    /// </summary>
    public class textSectionLineStyles : Dictionary<textSectionLineStyleName, textSectionLineStyle>
    {
        public textSectionLineStyles()
        {
            generateDefaultStyles();
        }

        protected void generateDefaultStyles()
        {
            foreach (textSectionLineStyleName sname in Enum.GetValues(typeof(textSectionLineStyleName)))
            {
                Add(sname, new textSectionLineStyle());
            }

            this[textSectionLineStyleName.heading].backgroundDeco = "=";
            this[textSectionLineStyleName.heading].marginDecoration = "-";
            this[textSectionLineStyleName.heading].fieldFormats[printHorizontal.left] = ": {0} :";
            this[textSectionLineStyleName.heading].fieldFormats[printHorizontal.middle] = ": {0} :";
            this[textSectionLineStyleName.heading].doInverseColors = true;
            this[textSectionLineStyleName.heading].backColor = platformColorName.DarkBlue;
            this[textSectionLineStyleName.heading].foreColor = platformColorName.Cyan;

            var name = textSectionLineStyleName.content;
            this[name].backgroundDeco = " ";
            this[name].marginDecoration = " ";
            this[name].fieldFormats[printHorizontal.left] = "{0}";
            this[name].fieldWidth[printHorizontal.right] = 0;
            this[name].foreColor = platformColorName.White;
            this[name].backColor = platformColorName.DarkGray;

            name = textSectionLineStyleName.itemlinst;
            this[name].backgroundDeco = "-";
            this[name].marginDecoration = " ";
            this[name].fieldFormats[printHorizontal.left] = "[{0}] ";
            this[name].fieldWidth[printHorizontal.left] = 5;
            this[name].fieldFormats[printHorizontal.middle] = " {0} ";
            this[name].fieldFormats[printHorizontal.right] = "{0}";
            this[name].fieldWidth[printHorizontal.right] = 20;
            this[name].foreColor = platformColorName.Red;
            this[name].backColor = platformColorName.Gray;

            name = textSectionLineStyleName.layout;
            this[name].backgroundDeco = "=";
            this[name].marginDecoration = " ";
            this[name].fieldFormats[printHorizontal.left] = " {0} ";
            this[name].fieldWidth[printHorizontal.left] = 10;
            this[name].fieldFormats[printHorizontal.middle] = " {0} ";
            this[name].fieldFormats[printHorizontal.right] = " {0} ";
            this[name].fieldWidth[printHorizontal.right] = 10;
            this[name].doInverseColors = true;
            this[name].foreColor = platformColorName.Gray;
            this[name].backColor = platformColorName.DarkGray;

            name = textSectionLineStyleName.footer;
            this[name].backgroundDeco = "=";
            this[name].marginDecoration = "-";
            this[name].fieldFormats[printHorizontal.left] = "[{0}] ";
            this[name].fieldWidth[printHorizontal.left] = 10;
            this[name].fieldFormats[printHorizontal.middle] = " {0} ";
            this[name].fieldFormats[printHorizontal.right] = " [{0}]";
            this[name].fieldWidth[printHorizontal.right] = 10;
            this[name].doInverseColors = true;
            this[name].foreColor = platformColorName.White;
            this[name].backColor = platformColorName.DarkGray;
        }
    }
}