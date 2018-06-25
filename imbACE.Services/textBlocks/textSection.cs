// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textSection.cs" company="imbVeles" >
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
namespace imbACE.Services.textBlocks
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.extensions;
    using imbACE.Services.platform.core;
    using imbACE.Services.terminal.core;
    using imbACE.Services.textBlocks.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System;

    /// <summary>
    /// Potpuno aktivno formatiran blok sadrzaja
    /// </summary>
    /// <remarks>
    /// Koristiti ovu klasu za generisanje sadrzaja
    /// </remarks>
    public class textSection : textSectionBase, IHasTextSectionMethods
    {
        public void setAndWriteValueLine(String valueString, String contentPrefix = "Value: ")
        {
            //write(cursor, contentPrefix, -1, false);
            String format = contentPrefix + "[{0,-";
            Int32 formatLenReduct = contentPrefix.Length + 2;

            Int32 vw = innerRightPosition - formatLenReduct - cursor.x;
            format = format.add(vw.ToString() + "}]", "-");
            format = format.Replace("--", "-");

            //write(contentPrefix, cursor.x, false, -
            String ln = String.Format(format, valueString);
            write(cursor, ln, -1, false);
            cursor.moveXTo(innerLeftPosition);

            cursor.valueReadZone = cursor.getSelectZone(1, -1, selectZoneOption.moveCursorToBottomEndOfZone, selectZoneOption.takeBracetDefinedArea);

            cursor.nextLine();
        }

        /// <summary>
        /// Upisuje u trenunu liniju podatke iz polja
        /// </summary>
        /// <param name="leftContent"></param>
        /// <param name="toInnerLine"></param>
        /// <param name="midContent"></param>
        /// <param name="rightContent"></param>
        /// <returns></returns>
        public String writeFields(String leftContent, Int32 toInnerLine = -1, String midContent = "", String rightContent = "")
        {
            if (toInnerLine != -1)
            {
                cursor.moveLineTo(toInnerLine);
            }

            if (String.IsNullOrEmpty(rightContent))
            {
                rightFieldWidth = 0;
            }

            if (String.IsNullOrEmpty(midContent))
            {
                leftFieldWidth = innerWidth;
            }

            contentLine = insertField(contentLine, leftContent, printHorizontal.left);
            contentLine = insertField(contentLine, midContent, printHorizontal.middle);
            contentLine = insertField(contentLine, rightContent, printHorizontal.right);
            styles[currentStyle].deploy(this);
            return contentLine;
        }

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
        public String writeField(String content, printHorizontal tab, Int32 toInnerLine = -1, String customFormat = "", String customBackground = "")
        {
            if (toInnerLine != -1)
            {
                cursor.moveLineTo(toInnerLine);
            }

            contentLine = insertField(contentLine, content, tab, customFormat, customBackground);
            return contentLine;
        }

        /// <summary>
        /// Upisuje liniju ili linije (automatski wrappuje)
        /// </summary>
        /// <param name="content"></param>
        /// <param name="toInnerLine"></param>
        /// <param name="doMoveToNextLine">Da li nakon ispisivanja pomera kursor na sledeci red</param>
        /// <param name="limitLineCount">Koliko linija najvise moze da ubaci na osnovu datog contenta</param>
        /// <returns>Koliko je linija ispisao nakon obrade content parametra</returns>
        public Int32 writeLine(String content, Int32 toInnerLine = -1, Boolean doMoveToNextLine = true, Int32 limitLineCount = 5)
        {
            return writeOrInsertLine(content, toInnerLine, doMoveToNextLine, limitLineCount, false);
        }

        /// <summary>
        /// Ubacuje liniju ili linije
        /// </summary>
        /// <param name="content"></param>
        /// <param name="toInnerLine"></param>
        /// <param name="doMoveToNextLine"></param>
        /// <param name="limitLineCount"></param>
        /// <returns></returns>
        public Int32 insertLine(String content, Int32 toInnerLine = -1, Boolean doMoveToNextLine = true, Int32 limitLineCount = 5)
        {
            return writeOrInsertLine(content, toInnerLine, doMoveToNextLine, limitLineCount, true);
        }

        /// <summary>
        /// Ubacuje spliter liniju i pomera kursor na sledeci red
        /// </summary>
        /// <param name="__backgroundDeco"></param>
        /// <param name="__marginDeco"></param>
        /// <param name="toInnerLine"></param>
        public void insertSplitLine(String __backgroundDeco = "-", String __marginDeco = "", Int32 toInnerLine = -1)
        {
            string content = __backgroundDeco.Repeat(innerWidth); //writeInlineBackground("", __marginDeco, __backgroundDeco);
            //write(cursor, content, innerWidth, false);
            writeLine(content, toInnerLine, false, 1);
        }

        /// <summary>
        /// skracuje section na mestu kursora
        /// </summary>
        public void cutSectionAtCursor()
        {
            Int32 bottom = cursor.y + padding.bottom + margin.bottom;
            height = bottom;
        }

        /// <summary>
        /// Instancira sekciju i renderuje background u skladu sa podesavanjima
        /// </summary>
        /// <param name="__height"></param>
        /// <param name="__width"></param>
        /// <param name="__leftRightMargin"></param>
        /// <param name="__leftRightPadding"></param>
        public textSection(int __height, int __width, int __leftRightMargin, int __leftRightPadding)
            : base(__height, __width, __leftRightMargin, __leftRightPadding)
        {
            height = __height;
            cursor = new textCursor(this, textCursorMode.fixedZone, textCursorZone.innerZone);
            cursor.moveToCorner(textCursorZoneCorner.UpLeft);
            //cursor.toTopLeftCorner();
            styles[textSectionLineStyleName.content].deploy(this);

            var bg = writeBackground(contentLines, true);
        }
    }
}