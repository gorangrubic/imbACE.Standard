// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textSectionBase.cs" company="imbVeles" >
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
    using imbACE.Core.extensions;
    using imbACE.Services.platform.core;
    using imbACE.Services.terminal.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.zone;
    using System;
    using System.Collections.Generic;

    public abstract class textSectionBase : textLineContentBase, ITextLayoutContentProvider, IHasCursor
    {
        // private textCursor _cursor;

        protected textSectionBase(int __height, int __width, int __leftRightMargin = 0, int __leftRightPadding = 0) : base(" ", " ", __width, __leftRightMargin, __leftRightPadding)
        {
            height = __height;
            //setStyle(textSectionLineStyleName.content);
            cursor = new textCursor(this, textCursorMode.fixedZone, textCursorZone.innerZone);
            //init();
        }

        /// <summary>
        /// Sadrzaj za jednolinijski blok ili linija na koju trenutno pokazuje kursor
        /// </summary>
        protected override string contentLine
        {
            get
            {
                if (contentLines.Count > cursor.y)
                {
                    return contentLines[cursor.y];
                }
                else
                {
                    return "";
                }

                //throw new NotImplementedException();
            }
            set
            {
                if (contentLines.Count > cursor.y)
                {
                    contentLines[cursor.y] = value;
                }
                else
                {
                }
                //throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Brise sav zadrzaj i renderuje pozadinu
        /// </summary>
        public override void resetContent()
        {
            _cleanContent();
        }

        protected void _cleanContent()
        {
            cursor.moveToCorner(textCursorZoneCorner.UpLeft);
            styles[textSectionLineStyleName.content].deploy(this);

            var bg = writeBackground(contentLines, true);
        }

        public override string[] getContent()
        {
            String[] output = contentLines.GetRange(0, Math.Min(height, contentLines.Count)).ToArray();

            return output;
        }

        protected void insertEmptyLine(Int32 toInnerLine = -1, Boolean keepCursorAtNewLine = true)
        {
            if (toInnerLine != -1)
            {
                cursor.moveLineTo(toInnerLine);
            }
            String ln = "";

            if (cursor.y > innerTopPosition)
            {
                ln = writeInlineBackground(ln, marginDecoration, backgroundDecoration);
            }
            else if (cursor.y > innerBoxedTopPosition)
            {
                ln = writeInlineBackground(ln, marginDecoration, marginDecoration);
            }
            else
            {
                ln = " ".Repeat(width);
            }

            contentLines.insert(ln, width, cursor.y);
            // Insert(cursor.y, ln);
            if (!keepCursorAtNewLine) cursor.nextLine();
        }

        /// <summary>
        /// Osnovni metod koji se stalno koristi za ispisivanje
        /// </summary>
        /// <param name="content"></param>
        /// <param name="toInnerLine"></param>
        /// <param name="doMoveToNextLine"></param>
        /// <param name="limitLineCount"></param>
        /// <param name="inserNewLine"></param>
        /// <returns></returns>
        protected Int32 writeOrInsertLine(String content, Int32 toInnerLine, Boolean doMoveToNextLine, Int32 limitLineCount, Boolean inserNewLine)
        {
            Int32 c = 0;
            if (!String.IsNullOrEmpty(content))
            {
                List<string> cls = content.wrapLineBySpace(innerWidth);
                if (toInnerLine != -1)
                {
                    cursor.moveLineTo(toInnerLine);
                }

                foreach (String ln in cls)
                {
                    if (inserNewLine)
                    {
                        insertEmptyLine();
                    }
                    write(ln, 0, true, ln.Length, true);
                    c++;
                    if (limitLineCount > -1)
                    {
                        if (c > limitLineCount)
                        {
                            break;
                        }
                    }
                }
            }
            if (doMoveToNextLine) cursor.nextLine();
            return c;
        }

        protected String write(String content, Int32 atX = 0, Boolean doMoveToNextLine = true, Int32 limitWriteLen = -1, Boolean saveToSelf = false)
        {
            cursor.moveXTo(atX);
            if (limitWriteLen == -1)
            {
                limitWriteLen = innerWidth;
            }
            else
            {
                limitWriteLen = Math.Min(limitWriteLen, innerWidth);
            }
            String output = contentLine.overwrite(cursor.x, limitWriteLen, content);
            if (saveToSelf) contentLine = output;
            if (doMoveToNextLine) cursor.nextLine();
            return output;
        }

        protected virtual textContentLines writeClear(textContentLines __content)
        {
            if (__content == null) __content = new textContentLines(this);
            textContentLines output = __content;

            output.Clear();
            String marginLine = marginDecoration.Repeat(width);
            for (Int32 a = 0; a < height; a++)
            {
                output.Add(marginLine);
            }
            return output;
        }

        /// <summary>
        /// Renderuje pozadinu
        /// </summary>
        /// <param name="__content"></param>
        /// <param name="marginDeco"></param>
        /// <param name="backgroundDeco"></param>
        /// <returns></returns>
        protected virtual textContentLines writeBackground(textContentLines __content, Boolean saveToSelf)
        {
            if (__content == null)
            {
                __content = new textContentLines(this);
            }
            textContentLines output = __content;

            output = writeClear(output);

            /// renderovanje pozadine
            for (Int32 a = innerBoxedTopPosition; a < innerBoxedBottomPosition; a++)
            {
                output[a] = writeInlineBackground(output[a], marginDecoration, backgroundDecoration);
            }

            if (saveToSelf) contentLines = output;
            return output;
        }
    }
}