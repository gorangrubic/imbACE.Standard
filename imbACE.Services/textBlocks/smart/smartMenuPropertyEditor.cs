// --------------------------------------------------------------------------------------------------------------------
// <copyright file="smartMenuPropertyEditor.cs" company="imbVeles" >
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
    using imbACE.Core.data;
    using imbACE.Core.enums.platform;
    using imbACE.Core.extensions;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.input;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.dialogs;
    using imbACE.Services.textBlocks.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.math;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System.Collections;
    using System.ComponentModel;

    public class smartMenuPropertyEditor : textInputMenuBase
    {
        #region --- editorTarget ------- cilj editora

        private Object _editorTarget;

        /// <summary>
        /// cilj editora
        /// </summary>
        protected Object editorTarget
        {
            get
            {
                return _editorTarget;
            }
            set
            {
                _editorTarget = value;
                OnPropertyChanged("editorTarget");
            }
        }

        #endregion --- editorTarget ------- cilj editora

        #region --- settingSpecs ------- specifikacija settingsa koje treba da prikaze

        private settingsEntriesForObject _settingSpecs;

        /// <summary>
        /// specifikacija settingsa koje treba da prikaze
        /// </summary>
        protected settingsEntriesForObject settingSpecs
        {
            get
            {
                return _settingSpecs;
            }
            set
            {
                _settingSpecs = value;
                OnPropertyChanged("settingSpecs");
            }
        }

        #endregion --- settingSpecs ------- specifikacija settingsa koje treba da prikaze

        protected List<aceMenuItem> propertyMenuItems = new List<aceMenuItem>();

        public Object getObject()
        {
            return settingSpecs.valueToObject(editorTarget);
        }

        public void setObject(Object __editorTarget)
        {
            editorTarget = __editorTarget;
            settingSpecs = new settingsEntriesForObject(editorTarget, true);

            menu.Remove(propertyMenuItems);

            propertyMenuItems.Clear();
            if (settingSpecs != null)
            {
                foreach (var _spec in settingSpecs.spes)
                {
                    aceMenuItem specItem = new aceMenuItem(_spec.Value.displayName, "", _spec.Value.description, "",
                                                           _spec.Value);
                    specItem.group = aceMenuItemGroup.mainItems;
                    propertyMenuItems.Add(specItem);
                    menu.setItem(specItem);
                }
            }
            menu.selected = menu.getFirstSafe() as aceMenuItem;

            menu.menuTitle = settingSpecs.targetType.Name + " variables";
            pageManager = new textPageManager<aceMenuItem>(10);

            pageManager.refresh(menu);
        }

        public smartMenuPropertyEditor(int _height, int __width, int __leftRightMargin = 0, int __leftRightPadding = 0)
            : base(new aceMenu(), _height, __width, __leftRightMargin, __leftRightPadding)
        {
        }

        public smartMenuPropertyEditor(aceMenu __menu, int _height, int __width, int __leftRightMargin = 0, int __leftRightPadding = 0) : base(__menu, _height, __width, __leftRightMargin, __leftRightPadding)
        {
        }

        public override void resetContent()
        {
            base.resetContent();
            setupFieldFormat("{0}", 15, 30);
            cursor.moveToCorner(textCursorZoneCorner.UpLeft);

            if (!String.IsNullOrEmpty(menu.menuTitle))
            {
                // setStyle(textSectionLineStyleName.heading);
                writeField(menu.menuTitle, printHorizontal.middle);
                //setStyle(textSectionLineStyleName.content);

                if (!String.IsNullOrEmpty(menu.menuDescription))
                {
                    writeField(menu.menuDescription, printHorizontal.left);
                    insertSplitLine();
                }

                insertSplitLine();
            }

            settingsPropertyEntryWithContext spec = null;
            var items = pageManager.getPageElements(menu);
            Int32 c = 0;
            foreach (aceMenuItem it in items)
            {
                var id = renderSelectBox(it, menu.isDisabled(it), menu.isSelected(it), menu.isDefault(it)).add(it.itemName);
                writeField(id, printHorizontal.left);
                spec = it.metaObject as settingsPropertyEntryWithContext;
                //it.metaObject a

                Object vl = spec.value;
                String valString = "";

                var tname = spec.type.toStringTypeTitle();
                valString = spec.value.toStringSafe();

                //if (spec.pi.PropertyType.IsEnum)
                //{
                //    tname = "Enumeration";
                //    valString = vl.ToString();
                //} else if (spec.pi.PropertyType.isCompatibileWith(typeof(ICollection)))
                //{
                //    tname = "Collection";
                //    ICollection icl = vl as ICollection;

                //    valString = "[" + icl.Count.ToString() + "]";
                //} else if (spec.pi.PropertyType.isCompatibileWith(typeof(INotifyPropertyChanged)))
                //{
                //    valString = "[object]";
                //} else if (spec.pi.PropertyType.IsPrimitive)
                //{
                //    valString = vl.toStringSafe();
                //} else if (spec.pi.PropertyType.isText())
                //{
                //    valString = vl.toStringSafe();
                //} else
                //{
                //    valString = "(..)";
                //}

                if (String.IsNullOrEmpty(valString)) valString = vl.toStringSafe();
                writeField(valString, printHorizontal.middle);
                writeField(tname, printHorizontal.right);
                c++;
                cursor.nextLine();
            }

            cursor.nextLine(pageManager.pageCapacaty - c);

            insertSplitLine();

            if (menu.selected != null)
            {
                spec = menu.selected.metaObject as settingsPropertyEntryWithContext;
            }

            if (spec != null)
            {
                //String sl = spec.description;
                writeLine(spec.displayName.add(spec.description, " : "), -1, false, 5);
            }

            insertSplitLine();

            if (spec != null)
            {
                var extraLine = "";

                if (spec.type.isBoolean())
                {
                    Boolean bvl = !spec.value.imbToBoolean();
                    extraLine += "[SPACE] Toggle to " + bvl.ToString() + " [+] True [-] False";
                }
                else if (spec.type.IsEnum)
                {
                    extraLine += "[+][-] Select value";
                }
                else if (spec.type.isText())
                {
                    setAndWriteValueLine(spec.value as string);
                    insertSplitLine();
                    extraLine += "[ENTER] Edit value";
                }
                else if (spec.type.isNumber())
                {
                    setAndWriteValueLine(spec.value.ToString());
                    insertSplitLine();

                    extraLine += "[ENTER] Edit value [+][-] Change value | with [Alt] 5x | with [Ctrl] 10x";
                }
                else
                {
                    if (editorTarget is IList)
                    {
                        if (spec == null)
                        {
                            insertLine("[INS] New item");
                        }
                        else if (spec.index == -1)
                        {
                            insertLine("[INS] New item");
                        }
                        else
                        {
                            insertLine("[INS] New item | [ENTER] Edit | [DEL] Delete | [F5] Duplicate");
                        }
                    }
                    else
                    {
                        extraLine += "[ENTER] Open in editor";
                    }
                }

                if (extraLine.Length > 0) insertLine(extraLine, -1, false, 2);
            }
            else
            {
                if (editorTarget is IList)
                {
                    insertLine("[INS] New item");
                }
            }

            //            insertLine("On leaving this screen changes will be saved.", -1, false);

            writeLine("[UP][DOWN] Select | [ESC] Done | [BACKSPACE] Revert values", -1, false);

            // insertLine("[UP][DOWN] Select | [INS] New item | [ESC] Done");

            //  insertLine("[F2] Load preset | [F5] Save preset | [F12] Reset to default", -1, false);

            cutSectionAtCursor();

            // instructions = "Use letter key to select <ENTER> to confirm (" + menu.getSelectedKey() + ")";
        }

        protected void changeValue(settingsPropertyEntryWithContext spec, Boolean forward, Int32 step = 1)
        {
            if (spec.pi.PropertyType.isBoolean())
            {
                Boolean bvl = !spec.value.imbToBoolean();
                spec.value = bvl;
                //extraLine += "[SPACE] Toggle to " + bvl.ToString() + " [+] True [-] False";
            }
            else if (spec.type.IsEnum)
            {
                if (forward)
                {
                    spec.value = spec.acceptableValues.takeItemRelativeTo(spec.value, step);
                }
                else
                {
                    spec.value = spec.acceptableValues.takeItemRelativeTo(spec.value, -step);
                }
                // extraLine += "[+][-] Select value";
            }
            else if (spec.type.isText())
            {
                // extraLine += "[SPACE] Edit value";
            }
            else if (spec.type.isNumber())
            {
                if (!forward) step = -step;

                if (spec.type == typeof(Double))
                {
                    spec.value = spec.value.changeValueDouble(step);
                }
                else
                {
                    spec.value = spec.value.changeValueAsInt32(step);
                }
                // extraLine += "[SPACE] Edit value [+][-] Change value | with [Alt] 5x | with [Ctrl] 10x";
            }
            else
            {
                // extraLine += "[SPACE] Edit value";
            }
        }

        protected void changeValue(settingsPropertyEntryWithContext spec, ConsoleKeyInfo keyInfo)
        {
            Int32 step = 1;
            Boolean forward = true;
            switch (currentOutput.consoleKey.Modifiers)
            {
                case ConsoleModifiers.Alt:
                    step = 5;
                    break;

                case ConsoleModifiers.Control:
                    step = 10;
                    break;

                case ConsoleModifiers.Shift:
                    step = 25;
                    break;
            }

            switch (currentOutput.consoleKey.Key)
            {
                case ConsoleKey.Backspace:
                    break;

                case ConsoleKey.RightArrow:
                case ConsoleKey.OemPlus:
                    forward = true;
                    break;

                case ConsoleKey.LeftArrow:
                case ConsoleKey.OemMinus:
                    forward = false;
                    break;
            }

            changeValue(spec, forward, step);
        }

        /// <summary>
        /// Primena procitanog unosa
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="__currentOutput"></param>
        /// <returns></returns>
        public override textInputResult applyReading(IPlatform platform, textInputResult __currentOutput)
        {
            // var item = __currentOutput.result as aceMenuItem;
            var spec = menu.selected.metaObject as settingsPropertyEntryWithContext;

            Int32 step = 1;

            switch (currentOutput.consoleKey.Key)
            {
                case ConsoleKey.Enter:

                    if (spec.type.isSimpleInputEnough())
                    {
                        currentOutput.result = spec.value;

                        if (currentOutput.result == null)
                        {
                            currentOutput.result = spec.type.GetDefaultValue();
                        }

                        var rs = platform.read(currentOutput, inputReadMode.read, cursor.valueReadZone, currentOutput.result);

                        spec.value = rs.result;
                    }
                    else if (!spec.type.isToggleValue())
                    {
                        var result = dialogEditProperties.open(platform, spec.value, spec.description, layoutTitleMessage.add(spec.displayName, " > "));
                        spec.value = result;
                    }
                    else
                    {
                        changeValue(spec, currentOutput.consoleKey);
                    }

                    // closeAndSaveChanges();
                    //currentOutput.doKeepReading = false;
                    break;

                case ConsoleKey.Escape:
                    object output = getObject();
                    currentOutput.result = output;
                    //menu.selected = null;
                    //    closeAndDefault();
                    currentOutput.doKeepReading = false;
                    break;

                case ConsoleKey.Backspace:
                    setObject(editorTarget);

                    break;

                case ConsoleKey.F5:
                    break;

                case ConsoleKey.F2:
                    break;

                case ConsoleKey.F12:
                    break;

                case ConsoleKey.LeftArrow:
                case ConsoleKey.RightArrow:
                case ConsoleKey.OemPlus:
                case ConsoleKey.Spacebar:
                case ConsoleKey.OemMinus:
                    changeValue(spec, currentOutput.consoleKey);
                    break;

                case ConsoleKey.UpArrow:
                    menu.selectPrev();
                    break;

                case ConsoleKey.DownArrow:
                    menu.selectNext();
                    break;

                default:

                    break;
            }

            currentOutput.result = menu.selected;

            //  layoutFooterMessage = instructions;
            // layoutFooterMessage =

            //currentOutput.meta[0] = menu;
            //currentOutput.meta[1] = this;

            return currentOutput;
        }
    }
}