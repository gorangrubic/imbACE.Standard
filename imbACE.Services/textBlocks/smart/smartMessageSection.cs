// --------------------------------------------------------------------------------------------------------------------
// <copyright file="smartMessageSection.cs" company="imbVeles" >
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
    using imbACE.Services.textBlocks.core;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;

    /// <summary>
    /// Prikazuje poruku sa naslovom
    /// </summary>
    public class smartMessageSection : textSection
    {
        #region --- doInsertSplitLineAtEnd ------- da li da ubacuje split line na kraju

        private Boolean _doInsertSplitLineAtEnd = false;

        /// <summary>
        /// da li da ubacuje split line na kraju
        /// </summary>
        public Boolean doInsertSplitLineAtEnd
        {
            get
            {
                return _doInsertSplitLineAtEnd;
            }
            set
            {
                _doInsertSplitLineAtEnd = value;
                OnPropertyChanged("doInsertSplitLineAtEnd");
            }
        }

        #endregion --- doInsertSplitLineAtEnd ------- da li da ubacuje split line na kraju

        protected String title = "";
        protected String message = "";
        protected String rightCorner = " [ESC:X]";

        public smartMessageSection(String messageTitle, String messageContent, int __height, int __width, int __leftRightMargin, int __leftRightPadding) : base(__height, __width, __leftRightMargin, __leftRightPadding)
        {
            title = messageTitle;
            message = messageContent;
            padding.top = 0;
            padding.bottom = 0;
            // setupFieldFormat("{0}", 20, 50);
            rightCorner = "";
        }

        public smartMessageSection(String messageTitle, String messageContent, textLayout layout)
            : base(layout.height, layout.width, layout.margin.left, layout.padding.left)
        {
            title = messageTitle;
            message = messageContent;
            padding.top = 0;
            padding.bottom = 0;
            // setupFieldFormat("{0}", 20, 50);
            resetContent();
        }

        public void setContent(String messageTitle, String messageContent)
        {
            _cleanContent();
            cursor.moveToCorner(textCursorZoneCorner.UpLeft);

            title = messageTitle;
            message = messageContent;
            // cursor.moveToCorner(textCursorZoneCorner.UpLeft);

            if (!String.IsNullOrEmpty(title))
            {
                //writeField(title, enums.printHorizontal.left);
                //writeField(rightCorner, enums.printHorizontal.right);
                //cursor.enter();
                // insertLine(title, 0, false, 1);

                String ln = title + " ";
                Int32 w = innerWidth - rightCorner.Length - 2;
                ln = ln.toWidthExact(w, "=");
                ln = ln.add(rightCorner);
                insertLine(ln, 0, false, 1);
                insertSplitLine();
            }

            if (!String.IsNullOrEmpty(message))
            {
                insertLine(message, -1, false, height - 4);
            }

            if (doInsertSplitLineAtEnd) if (!String.IsNullOrEmpty(title)) insertSplitLine();

            cutSectionAtCursor();
        }

        public override void resetContent()
        {
            base.resetContent();

            setContent(title, message);
        }
    }
}