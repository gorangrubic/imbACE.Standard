// --------------------------------------------------------------------------------------------------------------------
// <copyright file="smartInfoLineSection.cs" company="imbVeles" >
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

namespace imbACE.Services.textBlocks.smart
{
    using imbACE.Core.extensions;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.textBlocks.core;
    using imbACE.Services.textBlocks.enums;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;

    public class smartInfoLineSection : textLineContent
    {
        #region --- data ------- Bindable property

        private Dictionary<printHorizontal, String> _data = new Dictionary<printHorizontal, string>();

        /// <summary>
        /// Bindable property
        /// </summary>
        public Dictionary<printHorizontal, String> data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
                OnPropertyChanged("data");
            }
        }

        #endregion --- data ------- Bindable property

        public void setData(String leftFieldData, String midFieldData, String rightFieldData)
        {
            data = new Dictionary<printHorizontal, string>();
            if (!String.IsNullOrEmpty(leftFieldData)) data.Add(printHorizontal.left, leftFieldData);
            if (!String.IsNullOrEmpty(midFieldData)) data.Add(printHorizontal.middle, midFieldData);
            if (!String.IsNullOrEmpty(rightFieldData)) data.Add(printHorizontal.right, rightFieldData);
        }

        public override void resetContent()
        {
            //contentLines = new core.textContentLines(this);
            //cursor.switchToZone(textCursorZone.outterZone);
            // cursor.moveToCorner(textCursorZoneCorner.UpLeft);

            setStyle(currentStyle);
            Int32 y = 0;
            contentLines[y] = marginDecoration.Repeat(margin.left) + backgroundDecoration.Repeat(innerBoxedWidth) +
                            marginDecoration.Repeat(margin.right);

            // contentLines[y] = writeInlineBackground("");
            foreach (var dp in data)
            {
                contentLines[y] = insertField(contentLines[y], dp.Value, dp.Key, fieldFormats[dp.Key], "=");
            }
        }

        public smartInfoLineSection(Int32 linePos, IPlatform platform, int __leftRightMargin = 0, int __leftRightPadding = 2)
            : base("-", "=", platform.width, __leftRightMargin, __leftRightPadding)
        {
            height = platform.height;
            if (linePos < 0) linePos = platform.height - linePos;

            if (linePos > 1)
            {
                margin.top = linePos - 1;
            }
            padding.top = 0;
            contentLines = new textContentLines(this);
            setStyle(textSectionLineStyleName.heading);
        }
    }
}