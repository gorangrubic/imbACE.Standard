// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textLineContent.cs" company="imbVeles" >
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
using imbACE.Core.core.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbACE.Services.textBlocks
{
    using imbACE.Services.textBlocks.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Data.enums;

    /// <summary>
    /// Klasa za templejtiranje jedne linije --- nije predvidjeno za nasledjivanje
    /// </summary>
    public abstract class textLineContent : textLineContentBase, ITextLayoutContentProvider
    {
        ///// <summary>
        ///// Brise sav zadrzaj i renderuje pozadinu
        ///// </summary>
        //public override void resetContent()
        //{
        //    contentLine = writeInlineBackground("");

        //}

        public String renderLeftAndMiddle(String __content, String leftFieldContent, String middleFieldContent)
        {
            Int32 __rightFieldWidth = rightFieldWidth;
            rightFieldWidth = 0;
            String output = __content;

            output = writeInlineBackground(output, "", backgroundDecoration);

            output = insertField(output, leftFieldContent, printHorizontal.left);
            output = insertField(output, middleFieldContent, printHorizontal.middle);

            rightFieldWidth = __rightFieldWidth;
            contentLine = output;
            return output;
        }

        public String renderLeftMiddleRight(String __content, String leftFieldContent, String middleFieldContent, String rightFieldContent)
        {
            String output = __content;

            output = writeInlineBackground(output, "", backgroundDecoration);

            output = insertField(output, leftFieldContent, printHorizontal.left);
            output = insertField(output, middleFieldContent, printHorizontal.middle);
            output = insertField(output, rightFieldContent, printHorizontal.right);
            contentLine = output;
            return output;
        }

        public String renderMiddle(String __content, String middleFieldContent)
        {
            String output = __content;
            Int32 __rightFieldWidth = rightFieldWidth;
            rightFieldWidth = 0;
            Int32 __leftFieldWidth = leftFieldWidth;
            leftFieldWidth = 0;

            output = insertField(output, middleFieldContent, printHorizontal.middle);

            rightFieldWidth = __rightFieldWidth;
            leftFieldWidth = __leftFieldWidth;
            contentLine = output;
            return output;
        }

        public String renderMiddleRight(String __content, String rightFieldContent, String middleFieldContent)
        {
            Int32 __leftFieldWidth = leftFieldWidth;
            leftFieldWidth = 0;
            String output = __content;
            output = insertField(output, middleFieldContent, printHorizontal.middle);
            output = insertField(output, rightFieldContent, printHorizontal.right);
            leftFieldWidth = __leftFieldWidth;
            contentLine = output;
            return output;
        }

        /// <summary>
        /// Deklarisanje linijskog templejta
        /// </summary>
        /// <param name="__bgDecoration"></param>
        /// <param name="__width"></param>
        /// <param name="__leftRightMargin"></param>
        /// <param name="__leftRightPadding"></param>
        public textLineContent(string __mrDecoration, string __bgDecoration, int __width, int __leftRightMargin, int __leftRightPadding)
            : base(__mrDecoration, __bgDecoration, __width, __leftRightMargin, __leftRightPadding)
        {
            height = 1;
            contentLines = new textContentLines(this);
        }

        #region --- contentLine ------- Bindable property

        private String _contentLine = "";

        public override string[] getContent()
        {
            string[] output = { contentLine };
            return output;
        }

        /// <summary>
        /// Sadrzaj za jednolinijski blok ili linija na koju trenutno pokazuje kursor
        /// </summary>
        protected override String contentLine
        {
            get
            {
                return contentLines[0];
            }
            set
            {
                contentLines[0] = value;
                OnPropertyChanged("contentLine");
            }
        }

        #endregion --- contentLine ------- Bindable property
    }
}