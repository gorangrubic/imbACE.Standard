// --------------------------------------------------------------------------------------------------------------------
// <copyright file="smartPageViewer.cs" company="imbVeles" >
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
    using imbACE.Services.platform.input;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.core;
    using imbACE.Services.terminal.dialogs;
    using imbACE.Services.textBlocks.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using System.IO;

    /// <summary>
    /// Smart section
    /// </summary>
    public class smartPageViewer : textInputBase, IRefresh, IRead
    {
        #region --- pageManager ------- menadzer_stranica

        private textPageManager<String> _pageManager;

        /// <summary>
        /// menadzer_stranica
        /// </summary>
        public textPageManager<String> pageManager
        {
            get
            {
                return _pageManager;
            }
            set
            {
                _pageManager = value;
                OnPropertyChanged("pageManager");
            }
        }

        #endregion --- pageManager ------- menadzer_stranica

        #region --- content ------- linije sadrzaja

        private textContentLines _content;

        /// <summary>
        /// linije sadrzaja
        /// </summary>
        public textContentLines content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnPropertyChanged("content");
            }
        }

        #endregion --- content ------- linije sadrzaja

        public smartPageViewer(IEnumerable<String> __content, int _height, int __width, int __leftRightMargin = 0, int __leftRightPadding = 0)
            : base(_height, __width, __leftRightMargin, __leftRightPadding)
        {
            content = new textContentLines();
            content.insert(__content, innerWidth);

            //margin.top = 1;
            //margin.bottom = 2;
            //ageManager = new textPageManager<String>();
            refresh();
        }

        /// <summary>
        /// Izvrsava se svaki put kad treba azurirati strukturu sadrzaja prema DataModel izvoru
        /// </summary>
        public void refresh()
        {
            //Int32
            //Int32 page = height - (top_attachment.height - bottom_attachment.height);
            //top_attachment.height

            //pageManager = new textPageManager<String>(this);
        }

        /// <summary>
        /// Izvrsava se neposredno pre renderinga
        /// </summary>
        public override void resetContent()
        {
            base.resetContent();

            pageManager = new textPageManager<String>(this);
            pageManager.refresh(content);

            //setupFieldFormat("{0}", 10, 30);
            cursor.moveToCorner(textCursorZoneCorner.UpLeft);

            #region PAGINATED CONTENT DISPLAY

            var items = pageManager.getPageElements(content);
            Int32 c = 0;
            foreach (string it in items)
            {
                writeLine(it, -1, false);
                c++;
            }
            cursor.nextLine(pageManager.pageCapacaty - c);

            #endregion PAGINATED CONTENT DISPLAY

            // insertLine("[HOME] First page | [END] Last page");
            insertSplitLine();

            writeLine("[HOME][END][PgUp][PgDn] Page selection | Page ".add(pageManager.getPageString()), -1, false);

            cutSectionAtCursor();
        }

        /// <summary>
        /// Primena procitanog unosa
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="__currentOutput"></param>
        /// <returns></returns>
        public override textInputResult applyReading(IPlatform platform, textInputResult __currentOutput)
        {
            switch (currentOutput.consoleKey.Key)
            {
                case ConsoleKey.PageDown:
                    pageManager.selectNext();
                    //menu.selected = menu[pageManager.currentPageStartIndex];
                    break;

                case ConsoleKey.PageUp:
                    pageManager.selectPrev();
                    break;

                case ConsoleKey.Home:
                    pageManager.selectFirst();
                    break;

                case ConsoleKey.End:
                    pageManager.selectLast();
                    break;
            }
            return currentOutput;
        }
    }
}