// --------------------------------------------------------------------------------------------------------------------
// <copyright file="smartMenuSection.cs" company="imbVeles" >
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
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.commands.menu.render;
    using imbACE.Core.extensions;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.input;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.textBlocks.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.enums;

    /// <summary>
    /// Osnovni menu
    /// </summary>
    public class smartMenuSection : textInputMenuBase
    {
        public smartMenuSection(aceMenu __menu, int _height, int __width, int __leftRightMargin = 0, int __leftRightPadding = 0)
            : base(__menu, _height, __width, __leftRightMargin, __leftRightPadding)
        {
            doShowTitle = true;
            doShowRemarks = true;
            doShowInstructions = true;
            renderView = textInputMenuRenderView.listItemSelectable;
        }

        public smartMenuSection(aceMenu __menu, IHasCursor forLayout, textInputMenuRenderView __renderView) : base(__menu, forLayout.height, forLayout.width, 0, 0)
        {
            margin = forLayout.margin;
            padding = forLayout.padding;

            renderView = __renderView;
        }

        /// <summary>
        /// primenjuje rezultat iscitavanja jedne iteracije
        /// </summary>
        /// <returns></returns>
        public override textInputResult applyReading(IPlatform platform, textInputResult __currentOutput)
        {
            //ConsoleKeyInfo key = Console.ReadKey();
            //currentOutput.consoleKey = key;

            switch (currentOutput.consoleKey.Key)
            {
                case ConsoleKey.Enter:

                    currentOutput.doKeepReading = false;
                    break;

                case ConsoleKey.Escape:
                    menu.selected = null;

                    currentOutput.doKeepReading = false;
                    break;

                case ConsoleKey.PageUp:
                    pageManager.selectNext();
                    break;

                case ConsoleKey.PageDown:
                    pageManager.selectPrev();

                    break;

                case ConsoleKey.UpArrow:
                    menu.selectPrev();
                    pageManager.selectPageByItem(menu.selected);
                    break;

                case ConsoleKey.DownArrow:
                    menu.selectNext();
                    pageManager.selectPageByItem(menu.selected);
                    break;

                case ConsoleKey.LeftArrow:
                    menu.selectPrev();
                    pageManager.selectPageByItem(menu.selected);
                    break;

                case ConsoleKey.RightArrow:
                    menu.selectNext();
                    pageManager.selectPageByItem(menu.selected);
                    break;

                default:
                    aceMenuItem menuItem = menu.getItem(currentOutput.consoleKey.KeyChar.ToString(), -2, true);
                    if (menuItem == null)
                    {
                        switch (exitPolicy)
                        {
                            case textInputExitPolicy.onValidKey:
                                break;

                            case textInputExitPolicy.onValidValueOrKey:
                                break;

                            case textInputExitPolicy.onAnyKey:
                                currentOutput.doKeepReading = false;
                                break;
                        }
                    }
                    else
                    {
                        switch (exitPolicy)
                        {
                            case textInputExitPolicy.onValidKey:
                                currentOutput.doKeepReading = false;
                                break;

                            case textInputExitPolicy.onValidValueOrKey:
                                currentOutput.doKeepReading = false;
                                break;
                        }
                    }
                    break;
            }

            currentOutput.result = menu.selected;

            return currentOutput;
        }

        /// <summary>
        /// generise sadrzaj
        /// </summary>
        public override void resetContent()
        {
            base.resetContent();

            cursor.moveToCorner(textCursorZoneCorner.UpLeft);

            if (doShowTitle && !String.IsNullOrEmpty(menu.menuTitle))
            {
                setStyle(textSectionLineStyleName.heading);
                writeField(menu.menuTitle, printHorizontal.middle);
                setStyle(textSectionLineStyleName.content);
                //cursor.nextLine();
                insertSplitLine();
            }

            if (doShowRemarks && !String.IsNullOrEmpty(menu.menuDescription))
            {
                String remark = menu.menuDescription;

                insertLine(menu.menuDescription);
            }

            if (flags == null) flags = menuRenderFlag.inlineItems;

            Boolean doShowCurrentValueLine = false;
            switch (renderView)
            {
                case textInputMenuRenderView.listKeyItem:
                    flags |= menuRenderFlag.listItems;
                    flags |= menuRenderFlag.showSelectionBox;
                    flags |= menuRenderFlag.showIfDefault;
                    instructions = "Use letter key to select <ENTER> to confirm (" + menu.getSelectedKey() + ")";
                    break;

                case textInputMenuRenderView.listItemSelectable:
                    flags |= menuRenderFlag.listItems;
                    flags |= menuRenderFlag.showSelectionBox;
                    instructions = "Move with arrows, select with <ENTER>: "; //" to confirm (" + menu.selected.itemName + ")";
                    if (menu.selected != null)
                    {
                        if (menu.isDisabled(menu.selected))
                        {
                            instructions = instructions.add(menu.selected.itemName.add("is disabled"));
                        }
                        else
                        {
                            instructions = instructions.add(menu.selected.itemName);
                        }
                    }
                    break;

                case textInputMenuRenderView.inlineKeyListGroup:
                    flags |= menuRenderFlag.numberOrKey;
                    flags |= menuRenderFlag.inlineItems;
                    flags |= menuRenderFlag.showIfDefault;
                    instructions = "Arrows or key to select, ENTER to confirm.";
                    break;

                case textInputMenuRenderView.inlineItemsHidden:
                    instructions = "Use <UP>/<DOWN> arrows iterate available values. <ENTER> to confirm. <ESC> to go back.";
                    doShowCurrentValueLine = true;
                    break;
            }

            this.renderMenuItems(menu, flags);

            insertSplitLine("-");

            if (!pageManager.isDisabled)
            {
                writeLine("[PgUp][PgDn] Page change | Page: " + pageManager.getPageString(), -1, false, 5);
            }

            if (!String.IsNullOrEmpty(instructions))
            {
                //layoutFooterMessage = instructions;

                //instructionFooterLine = instructions;
                writeLine(instructions, -1, false, 2);
            }

            if (doShowValueRemarks)
            {
                if (menu.selected != null)
                {
                    if (menu.isDisabled(menu.selected))
                    {
                        writeLine(menu.selected.itemRemarkDisabled, -1, false, 2);
                    }
                    else
                    {
                        writeLine(menu.selected.itemRemarkEnabled, -1, false, 2);
                    }

                    if (!String.IsNullOrEmpty(menu.selected.helpLine))
                    {
                        if (menu.selected.itemRemarkEnabled != menu.selected.helpLine)
                        {
                            writeLine(menu.selected.helpLine);
                        }
                    }
                }
            }

            if (doShowCurrentValueLine)
            {
                //layoutStatusMessage = "Selected: "+ menu.getSelectedKey();

                // write("Selected: ", 0, false);
                //cursor.placeChild(valueZone);
                //valueZone.resetContent();
                //valueZone.cursor.moveToCorner(textCursorZoneCorner.UpLeft);
                //valueZone.write(valueZone.cursor, );
            }
            // insertSplitLine("-");

            cutSectionAtCursor();
        }
    }
}