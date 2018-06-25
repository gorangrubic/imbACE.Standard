// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textLineContentBase.cs" company="imbVeles" >
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
    using imbACE.Services.textBlocks.core.proto;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Osnova za line i section blokove sadrzaja
    /// </summary>
    public abstract class textLineContentBase : textContentWithBackground, ISupportsTextCursor, IAcceptsTextSectionStyle, ITextLayoutContentProvider, ItextLayout
    {
        #region --------------------- LAYOUT MESSAGES --------

        #region --- layoutFooterMessage ------- porukaZaFooter

        private String _layoutFooterMessage = "";

        /// <summary>
        /// porukaZaFooter
        /// </summary>
        public String layoutFooterMessage
        {
            get
            {
                return _layoutFooterMessage;
            }
            set
            {
                _layoutFooterMessage = value;
                OnPropertyChanged("layoutFooterMessage");
            }
        }

        #endregion --- layoutFooterMessage ------- porukaZaFooter

        #region --- layoutTitleMessage ------- poruka za naslov prozora

        private String _layoutTitleMessage = "";

        /// <summary>
        /// poruka za naslov prozora
        /// </summary>
        public String layoutTitleMessage
        {
            get
            {
                return _layoutTitleMessage;
            }
            set
            {
                _layoutTitleMessage = value;
                OnPropertyChanged("layoutTitleMessage");
            }
        }

        #endregion --- layoutTitleMessage ------- poruka za naslov prozora

        #region --- layoutStatusMessage ------- Poruka za statusni header

        private String _layoutStatusMessage = "";

        /// <summary>
        /// Poruka za statusni header
        /// </summary>
        public String layoutStatusMessage
        {
            get
            {
                return _layoutStatusMessage;
            }
            set
            {
                _layoutStatusMessage = value;
                OnPropertyChanged("layoutStatusMessage");
            }
        }

        #endregion --- layoutStatusMessage ------- Poruka za statusni header

        #endregion --------------------- LAYOUT MESSAGES --------

        private Int32 _leftFieldWidth = 10;
        private Int32 _rightFieldWidth = 10;
        private Dictionary<printHorizontal, string> _fieldFormat = new Dictionary<printHorizontal, string>();

        private textContentLines _contentLines;
        private textCursor _cursor;

        private textSectionLineStyles _styles = new textSectionLineStyles();

        protected textSectionLineStyleName currentStyle = textSectionLineStyleName.content;

        /// <summary>
        /// stilovi
        /// </summary>
        protected textSectionLineStyles styles
        {
            get
            {
                return _styles;
            }
            set
            {
                _styles = value;
                OnPropertyChanged("styles");
            }
        }

        public void setStyle(textSectionLineStyleName __styleName)
        {
            currentStyle = __styleName;
            styles[__styleName].deploy(this);
        }

        /// <summary>
        /// Sekcija koja je pridodata sa gornje strane
        /// </summary>
        protected ITextLayoutContentProvider top_attachment;

        /// <summary>
        /// Sekcija koja je pridodata sa donje strane
        /// </summary>
        protected ITextLayoutContentProvider bottom_attachment;

        /// <summary>
        /// Vraca section koji je "nakacen" na ovaj. Nakachen section se automatski RRE obradjuje pri pozivanju ovog sectiona
        /// </summary>
        /// <param name="isBottomAttachment"></param>
        /// <returns></returns>
        public ITextLayoutContentProvider getAttachment(Boolean isBottomAttachment = true)
        {
            if (isBottomAttachment)
            {
                return bottom_attachment;
            }
            else
            {
                return top_attachment;
            }
        }

        public Int32 getAttachmentHeight(Boolean isBottomAttachment = true, Int32 h = 0)
        {
            if (isBottomAttachment)
            {
                if (top_attachment != null)
                {
                    return top_attachment.getAttachmentTotalHeight(isBottomAttachment);
                }
            }
            else
            {
                if (bottom_attachment != null)
                {
                    return bottom_attachment.getAttachmentTotalHeight(isBottomAttachment);
                }
            }
            return 0;
        }

        public Int32 getAttachmentTotalHeight(Boolean isBottomAttachment = true, Int32 h = 0)
        {
            //h = h + innerBoxedHeight;
            if (isBottomAttachment)
            {
                if (top_attachment != null)
                {
                    return top_attachment.innerBoxedHeight;
                }
            }
            else
            {
                if (bottom_attachment != null)
                {
                    return bottom_attachment.innerBoxedHeight;
                }
            }
            return h;
        }

        public void refreshAttachmentPosition()
        {
            refreshAttachmentPosition(true);
            //refreshAttachmentPosition(false);
        }

        protected void refreshAttachmentPosition(Boolean isBottomAttachment)
        {
            var __sectionToAttach = bottom_attachment;

            if (isBottomAttachment)
            {
                __sectionToAttach = bottom_attachment;
                if (__sectionToAttach != null)
                {
                    __sectionToAttach.margin.top = outerBottomPosition;
                    __sectionToAttach.ZLayerOrder = ZLayerOrder + 5;
                }
            }
            else
            {
                __sectionToAttach = top_attachment;
                if (__sectionToAttach != null)
                {
                    __sectionToAttach.margin.top = margin.top - __sectionToAttach.innerBoxedHeight;
                    __sectionToAttach.ZLayerOrder = ZLayerOrder - 5;
                }
            }
        }

        /// <summary>
        /// Kaci prosledjeni section na ovaj - zajedno ce biti renderovani
        /// </summary>
        /// <param name="__sectionToAttach">Sekcija koja se kaci na ovu</param>
        /// <param name="isBottomAttachment">Ako je TRUE onda ga dodaje sa donje strane</param>
        /// <returns>Vraca sekciju koja je bila ranije attachovana, ako je NULL onda je prvi put da se nesto attachuje</returns>
        public ITextLayoutContentProvider setAttachment(ITextLayoutContentProvider __sectionToAttach, Boolean isBottomAttachment = true)
        {
            ITextLayoutContentProvider popout = null;
            if (isBottomAttachment)
            {
                popout = bottom_attachment;
                bottom_attachment = __sectionToAttach;
                __sectionToAttach.setAttachment(this, false);
                refreshAttachmentPosition(isBottomAttachment);

                //__sectionToAttach.margin.top = outerBottomPosition;
                //__sectionToAttach.ZLayerOrder = ZLayerOrder + 5;
                return popout;
            }
            else
            {
                top_attachment = __sectionToAttach;
                // refreshAttachmentPosition(isBottomAttachment);

                //if (margin.top > __sectionToAttach.innerBoxedHeight)
                //{
                //    popout = top_attachment;
                //    //__sectionToAttach.margin.top = margin.top - __sectionToAttach.innerBoxedHeight;

                //   // __sectionToAttach.setAttachment(this, false);
                //    //__sectionToAttach.ZLayerOrder = ZLayerOrder - 5;
                //    refreshAttachmentPosition(isBottomAttachment);
                //} else
                //{
                //    Exception ex = new aceGeneralException("Top attachment refused> no free space in section margin");
                //    throw ex;
                //}
                //refreshAttachmentPosition(isBottomAttachment);
                return popout;
            }

            return popout;
        }

        /// <summary>
        /// sadrzaj sekcije
        /// </summary>
        protected textContentLines contentLines
        {
            get
            {
                if (_contentLines == null) _contentLines = new textContentLines(this);
                return _contentLines;
            }
            set
            {
                _contentLines = value;
                OnPropertyChanged("contentLines");
            }
        }

        /// <summary>
        /// zaduzen kursor
        /// </summary>
        public textCursor cursor
        {
            get
            {
                return _cursor;
            }
            set
            {
                _cursor = value;
                OnPropertyChanged("cursor");
            }
        }

        protected textLineContentBase(string __mrDecoration, string __bgDecoration, int __width, int __leftRightMargin, int __leftRightPadding) : base(__mrDecoration, __bgDecoration, __width, __leftRightMargin, __leftRightPadding)
        {
            setupFieldFormat("{0}", 10, 10);

            cursor = new textCursor(this, textCursorMode.fixedZone, textCursorZone.innerZone);
        }

        /// <summary>
        /// Podesava formatiranje polja
        /// </summary>
        /// <param name="defaultFormat">Format koji se primenjuje u svaki insert</param>
        /// <param name="__rightField">Desna kolona</param>
        /// <param name="__leftField">Leva kolona</param>
        public void setupFieldFormat(String defaultFormat, Int32 __rightField = 0, Int32 __leftField = 0)
        {
            leftFieldWidth = __leftField;
            rightFieldWidth = __rightField;

            foreach (printHorizontal en in Enum.GetValues(typeof(printHorizontal)))
            {
                if (fieldFormats.ContainsKey(en))
                {
                    fieldFormats[en] = defaultFormat;
                }
                else
                {
                    fieldFormats.Add(en, defaultFormat);
                }
            }
        }

        /// <summary>
        /// Od trenutne pozicije kursora vraca substring date duzine. Ako je length = -1 onda do desnog kraja dozvoljene zone.
        /// </summary>
        /// <param name="length"></param>
        /// <param name="copyCompleteLine">Da li da iskopira celu liniju</param>
        /// <returns></returns>
        public string @select(textCursor cursor, int length = -1, bool copyCompleteLine = false)
        {
            Int32 start = cursor.x;

            if (copyCompleteLine)
            {
                cursor.moveToCorner(textCursorZoneCorner.Left);
                start = cursor.x;
                length = cursor.selectToCorner(textCursorZoneCorner.Right).x;
            }
            else
            {
                if (length == -1)
                {
                    length = cursor.selectToCorner(textCursorZoneCorner.Right).x;

                    //cursor.moveToCorner(textCursorZoneCorner.Right);
                }
            }
            String output = contentLines.select(cursor.y, start, length);
            /*
            var ln = contentLines[cursor.y];
            if (!String.IsNullOrEmpty(ln))
            {
                length = Math.Min(length - start, ln.Length - start);

                ln.Substring(start, length);
            } else
            {
            }
            // output = "[" + cursor.y + "] " + output; /// debug
            */
            return output;
        }

        /// <summary>
        /// upisuje prosledjen unos, primenjuje limit ako je dat - ako nije> limit je u skladu za zonom
        /// </summary>
        /// <param name="input"></param>
        /// <param name="limit"></param>
        /// <param name="writeCompleteLine"></param>
        public void write(textCursor cursor, string input, int limit = -1, bool writeCompleteLine = false)
        {
            Int32 start = cursor.x;
            Int32 length = input.Length;

            if (limit != -1)
            {
                length = Math.Min(length, limit);
            }

            if (writeCompleteLine)
            {
                cursor.moveToCorner(textCursorZoneCorner.Left);
                start = cursor.x;
                length = Math.Min(cursor.selectToCorner(textCursorZoneCorner.Right).x, length);
            }

            var ln = contentLines[cursor.y];

            //length = Math.Min(ln.Length - start, input.Length - start);

            String st = contentLines[cursor.y].Substring(0, start);
            String md = input.Substring(0, length);
            Int32 led = ln.Length - (st.Length + md.Length);

            String ed = ln.Substring(st.Length + md.Length, led);

            contentLines[cursor.y] = st + md + ed;

            //String output = ln.Substring(start, length);
            //return output;

            // contentLine = contentLine.Insert(start, input.Substring(0, length)).Substring(0, length);
        }

        /// <summary>
        /// sirina levog polja
        /// </summary>
        public Int32 leftFieldWidth
        {
            get
            {
                return _leftFieldWidth;
            }
            set
            {
                _leftFieldWidth = value;
                OnPropertyChanged("leftFieldWidth");
            }
        }

        /// <summary>
        /// sirina desnog polja
        /// </summary>
        public Int32 rightFieldWidth
        {
            get
            {
                return _rightFieldWidth;
            }
            set
            {
                _rightFieldWidth = value;
                OnPropertyChanged("rightFieldWidth");
            }
        }

        public Int32 middleFieldWidth
        {
            get
            {
                Int32 output = width - leftFieldWidth - rightFieldWidth;
                return output;
            }
        }

        /// <summary>
        /// format koji primenjuje pri insertovanju polja
        /// </summary>
        public Dictionary<printHorizontal, String> fieldFormats
        {
            get
            {
                return _fieldFormat;
            }
            set
            {
                _fieldFormat = value;
                OnPropertyChanged("fieldFormat");
            }
        }

        /// <summary>
        /// Ubacuje jedno polje u trenutnu liniju
        /// </summary>
        /// <param name="__content"></param>
        /// <param name="input"></param>
        /// <param name="field"></param>
        /// <param name="__fieldFormat"></param>
        /// <param name="__background"></param>
        /// <returns></returns>
        protected String insertField(String __content, String input, printHorizontal field, String __fieldFormat = "", String __background = "")
        {
            if (String.IsNullOrEmpty(input)) return __content;
            if (String.IsNullOrEmpty(__background)) __background = backgroundDecoration;
            if (String.IsNullOrEmpty(__content))
            {
                __content = marginDecoration.Repeat(margin.left) + backgroundDecoration.Repeat(innerBoxedWidth) +
                            marginDecoration.Repeat(margin.right);
                // __background.Repeat(
                //__content = writeInlineBackground(__content, "", __background);
            }

            if (String.IsNullOrEmpty(__fieldFormat))
            {
                __fieldFormat = fieldFormats[field];
            }

            var fieldFormatEmpty = String.Format(__fieldFormat, "");
            var fieldFormatEmptyLen = fieldFormatEmpty.Length;

            Int32 fieldWidth = 0;
            switch (field)
            {
                case printHorizontal.left:
                    fieldWidth = leftFieldWidth;
                    break;

                case printHorizontal.middle:
                    fieldWidth = middleFieldWidth;
                    break;

                case printHorizontal.right:
                    fieldWidth = rightFieldWidth;
                    break;

                default:
                    fieldWidth = innerWidth;
                    break;
            }

            if (fieldWidth == 0) return __content;

            input = input.trimToWidth(fieldWidth - fieldFormatEmptyLen, field);
            var toInsert = String.Format(__fieldFormat, input);

            __content = writeInlineText(__content, toInsert, field, fieldWidth);
            return __content;
        }

        /// <summary>
        /// Brise sav zadrzaj i renderuje pozadinu
        /// </summary>
        public abstract void resetContent();
    }
}