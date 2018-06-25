// --------------------------------------------------------------------------------------------------------------------
// <copyright file="_textContent.cs" company="imbVeles" >
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
    using imbACE.Services.textBlocks.enums;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class _textContent : _textBlock
    {
        public _textContent()
        {
        }

        public _textContent(Int32 _w, Int32 _h)
        {
            width = _w;
            height = _h;
            autoPopulate();
        }

        #region --- margin ------- Bindable property

        private fourSideSetting _margin = new fourSideSetting(5, 5);

        /// <summary>
        /// Bindable property
        /// </summary>
        public fourSideSetting margin
        {
            get
            {
                return _margin;
            }
            set
            {
                _margin = value;
                OnPropertyChanged("margin");
            }
        }

        #endregion --- margin ------- Bindable property

        public String emptyLine
        {
            get
            {
                String format = "{0," + width.ToString() + "}";
                String _emptyLine = String.Format(format, " ");
                return _emptyLine;
            }
        }

        protected void autoPopulate()
        {
            Clear();

            for (Int32 a = 0; a < height; a++)
            {
                String format = "{0," + width.ToString() + "}";
                String _emptyLine = String.Format(format, " ");
                Add(_emptyLine);
            }
        }

        protected void setToEmptyLine(Int32 _start = 0, Int32 _to = -1)
        {
            if (_to == -1) _to = height;
            String _el = emptyLine;
            if (_start < 0) return;
            for (Int32 a = _start; a < _to; a++)
            {
                this[a] = _el;
            }
        }

        protected void insertLines(IEnumerable<String> lines, Int32 _start = 0, Int32 _len = -1)
        {
            if (_len == -1) _len = lines.Count();
            if (_len > (height - _start)) _len = height - _start;

            Int32 _to = height - _len;

            Int32 c = 0;
            foreach (String ln in lines)
            {
                Int32 ind = _start + c;

                Add(ln, ind);
                c++;
                if (c > _len) break;
                if (ind > _to) break;
            }
        }

        public void fitToViewport(_textViewport viewport, Int32 extraLns = 5)
        {
            width = viewport.width - margin.leftAndRight;
            height = viewport.height - margin.topAndBottom - extraLns;
            //setToEmptyLine(0, margin.top);
        }

        public void setContent(_textBlock content, printHorizontal horizontal, printVertical vertical, Boolean doInsertEmptyLines = true)
        {
            String lineFormat = "";
            Int32 dW = width - content.width;
            Int32 dWl = dW / 2;
            Int32 dWr = dW - dWl;

            Int32 dH = height - content.height;

            List<String> inserts = content.getContent(width, horizontal);

            switch (vertical)
            {
                case printVertical.bottom:
                    dH = height - content.height;
                    if (doInsertEmptyLines) setToEmptyLine(0, dH);
                    insertLines(inserts, dH);
                    break;

                case printVertical.center:
                    dH = height - content.height;
                    dH = dH / 2;
                    if (doInsertEmptyLines) setToEmptyLine(0, dH);
                    insertLines(inserts, dH);
                    if (doInsertEmptyLines) setToEmptyLine(dH);
                    break;

                case printVertical.top:
                    dH = height - content.height;
                    insertLines(inserts, 0);
                    if (doInsertEmptyLines) setToEmptyLine(dH);
                    break;
            }
        }
    }
}