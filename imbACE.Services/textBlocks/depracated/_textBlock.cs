// --------------------------------------------------------------------------------------------------------------------
// <copyright file="_textBlock.cs" company="imbVeles" >
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
namespace imbACE.Services.textBlocks.depracated
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.extensions;
    using imbACE.Services.textBlocks.enums;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class _textBlock : List<string>
    {
        #region --- padding ------- Bindable property

        private fourSideSetting _padding = new fourSideSetting(2, 2);

        /// <summary>
        /// Bindable property
        /// </summary>
        public fourSideSetting padding
        {
            get
            {
                return _padding;
            }
            set
            {
                _padding = value;
                OnPropertyChanged("padding");
            }
        }

        #endregion --- padding ------- Bindable property

        public _textBlock()
        {
        }

        public _textBlock(String __content, Int32 __w)
        {
            doWrapLines = true;
            width = __w;
            addContent(__content, addContentMode.replaceAll);
        }

        public _textBlock(String __content)
        {
            doWrapLines = false;
            addContent(__content, addContentMode.appendOnBottom);
            width = maxWidth;
        }

        #region --- width ------- sirina viewporta u kolonama

        private Int32 _width;

        /// <summary>
        /// sirina viewporta u kolonama
        /// </summary>
        public Int32 width
        {
            get
            {
                return Math.Max(_width, _maxWidth);
                return _width;
            }
            set
            {
                _width = value;
                OnPropertyChanged("width");
            }
        }

        #endregion --- width ------- sirina viewporta u kolonama

        #region --- height ------- visina viewporta u redovima

        private Int32 _height;

        /// <summary>
        /// visina viewporta u redovima
        /// </summary>
        public Int32 height
        {
            get
            {
                if (Count > _height)
                {
                    return Math.Min(_height, Count);
                }
                else
                {
                    return Math.Max(_height, Count);
                }
            }
            set
            {
                _height = value;
                OnPropertyChanged("height");
            }
        }

        #endregion --- height ------- visina viewporta u redovima

        #region --- maxWidth ------- Ukupna maksimalna sirina

        private Int32 _maxWidth = 0;

        /// <summary>
        /// Ukupna maksimalna sirina
        /// </summary>
        public Int32 maxWidth
        {
            get
            {
                return _maxWidth;
            }
            set
            {
                _maxWidth = value;
            }
        }

        #endregion --- maxWidth ------- Ukupna maksimalna sirina

        /// <summary>
        /// Dodaje sadrzaj
        /// </summary>
        /// <param name="content"></param>
        /// <param name="mode"></param>
        public void addContent(String content, addContentMode mode = addContentMode.appendOnBottom)
        {
            string[] lns = content.Split(Environment.NewLine.ToCharArray());

            if (doWrapLines)
            {
                List<String> lnso = new List<string>();
                foreach (String ln in lns)
                {
                    String lni = ln;
                    //lni = " ".Repeat(padding.left) + ln +
                    while (lni.Length > width)
                    {
                        String lnis = lni.Substring(0, width - 2);
                        if (!lnis.EndsWith(" ")) lnis += "-";
                        lnso.Add(lnis);
                        lni = lni.Substring(width - 2);
                    }
                    lnso.Add(lni);
                }
                lns = lnso.ToArray();
            }

            switch (mode)
            {
                case addContentMode.replaceAll:
                    Clear();
                    foreach (String ln in lns)
                    {
                        Add(ln, -1);
                    }
                    break;

                case addContentMode.replaceOnTop:
                    Int32 dLC = lns.Length - Count;
                    if (dLC > 0)
                    {
                        for (Int32 a = 0; a <= dLC; a++)
                        {
                            Add("", -1);
                        }
                    }
                    Int32 c = 0;
                    foreach (String ln in lns)
                    {
                        this[c] = ln;
                        c++;
                    }
                    break;

                case addContentMode.appendOnBottom:
                    foreach (String ln in lns)
                    {
                        Add(ln, -1);
                    }
                    break;

                case addContentMode.appendOnTop:
                    foreach (String ln in lns)
                    {
                        Add(ln, 0);
                        //Insert(0, ln);
                    }
                    break;
            }
        }

        public void Clear()
        {
            base.Clear();
            maxWidth = 0;
        }

        protected void getMaxWidth()
        {
            foreach (String ln in this)
            {
                maxWidth = Math.Max(maxWidth, ln.Length);
            }
        }

        public String getLineFormat(printHorizontal hor, String deco = " ")
        {
            String format = "";
            format += deco.Repeat(padding.left);
            Int32 innerWidth = width - padding.leftAndRight;
            switch (hor)
            {
                case printHorizontal.left:
                    format += "{0," + (-innerWidth).ToString() + "}";
                    break;

                case printHorizontal.middle:
                case printHorizontal.right:
                    format += "{0," + (innerWidth).ToString() + "}";
                    break;

                case printHorizontal.hide:
                    format += deco.Repeat(innerWidth);
                    break;
            }
            format += deco.Repeat(padding.right);
            return format;
        }

        public String appendFullLine(String deco = "-")
        {
            String ln = getLineFormat(printHorizontal.hide, deco);
            Add(ln);
            return ln;
        }

        public Int32 appendLine(String content, String contentInnerFormat = "{0}", printHorizontal hor = printHorizontal.left, String deco = " ", Int32 index = -1)
        {
            String cl = String.Format(contentInnerFormat, content);

            Int32 innerWidth = width - padding.leftAndRight;

            List<string> lns = cl.wrapLine(innerWidth);

            String fr = getLineFormat(hor, deco);

            Int32 c = 0;
            foreach (String ln in lns)
            {
                String lno = String.Format(fr, ln);
                Int32 ind = index + c;
                if (index == -1) ind = -1;
                Add(lno, ind);
                c++;
            }
            return lns.Count;
        }

        public void paintAll(String deco = " ")
        {
            String paintLine = deco.Repeat(width - padding.leftAndRight);
            for (Int32 a = 0; a < height; a++)
            {
                appendLine(paintLine);
            }
        }

        public void appendLines(Int32 y, IEnumerable<String> content)
        {
            Int32 c = 0;
            if (y == -1) y = Count - 1;
            foreach (String cln in content)
            {
                Int32 i = y + c;
                if (i > height)
                {
                    break;
                }
                appendLine(cln, "{0}", printHorizontal.left, " ", i);
                c++;
            }
        }

        public String getFormated(String line, printHorizontal hor, String decoLeft = "", String decoRight = "")
        {
            String newLine = "";
            String format = getLineFormat(line.Length, hor);

            newLine = String.Format(format, line, decoLeft, decoRight);

            return newLine;
        }

        public void Add(String line, Int32 index = -1)
        {
            if (maxWidth == 0) maxWidth = line.Length;
            maxWidth = Math.Max(maxWidth, line.Length);
            if (index == -1)
            {
                base.Add(line);
            }
            else
            {
                Insert(index, line);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /*
        if (this[index].Length == maxWidth)
                {
                    this[index] = line;
                    getMaxWidth();
                } else
                {
                    this[index] = line;
                }
        */

        public void setContent(String content)
        {
            addContent(content, addContentMode.replaceAll);
        }

        public String getLineFormat(Int32 targetWidth, printHorizontal horizontal)
        {
            String lineFormat = "";
            Int32 dW = targetWidth - width;
            Int32 dWl = dW / 2;
            Int32 dWr = dW - 1 - dWl;

            if (dW > 0)
            {
                switch (horizontal)
                {
                    case printHorizontal.left:
                        lineFormat = "{0," + (-width).ToString() + "}{1,1}{2," + dWr.ToString() + "}";
                        break;

                    case printHorizontal.middle:
                        lineFormat = "{1," + dWl.ToString() + "}{0," + width.ToString() + "}{2," + dWr.ToString() + "}";
                        break;

                    case printHorizontal.right:
                        lineFormat = "{1," + dW.ToString() + "}{0," + width.ToString() + "}{2,0}";
                        break;
                }
            }
            return lineFormat;
        }

        public List<String> getContent(Int32 targetWidth, printHorizontal horizontal)
        {
            String lineFormat = getLineFormat(targetWidth, horizontal);
            List<String> inserts = new List<string>();
            foreach (String ln in this)
            {
                inserts.Add(String.Format(lineFormat, ln, "", ""));
            }

            return inserts;
        }

        #region --- doWrapLines ------- Da li lomi linije ukoliko su duze od sirine

        private Boolean _doWrapLines = true;

        /// <summary>
        /// Da li lomi linije ukoliko su duze od sirine
        /// </summary>
        public Boolean doWrapLines
        {
            get
            {
                return _doWrapLines;
            }
            set
            {
                _doWrapLines = value;
            }
        }

        #endregion --- doWrapLines ------- Da li lomi linije ukoliko su duze od sirine
    }
}