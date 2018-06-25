// --------------------------------------------------------------------------------------------------------------------
// <copyright file="smartCollectionEditor.cs" company="imbVeles" >
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
namespace imbACE.Services.textBlocks.smart
{
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.data;
    using imbACE.Core.extensions;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.input;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.core;
    using imbACE.Services.textBlocks.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System;
    using System.Collections;

    public class smartCollectionEditor : textInputMenuBase, IRefresh, IRead
    {
        public smartCollectionEditor(settingsPropertyEntryWithContext __target, int _height, int __width, int __leftRightMargin = 0, int __leftRightPadding = 0) : base(new aceMenu(), _height, __width, __leftRightMargin, __leftRightPadding)
        {
            target = __target;
        }

        /// <summary>
        /// Primena procitanog unosa
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="__currentOutput"></param>
        /// <returns></returns>
        public override textInputResult applyReading(IPlatform platform, textInputResult __currentOutput)
        {
            return __currentOutput;
        }

        protected settingsPropertyEntryWithContext target;

        protected settingsEntriesForObject itemSettings;

        public void refresh()
        {
            IEnumerable items = target.value as IEnumerable;

            menu = new aceMenu();

            menu.menuTitle = "Inside " + target.pi.DeclaringType.Name + "->" + target.displayName + " collection";
            menu.menuDescription = "Collection type [" + target.pi.PropertyType.ToString() + "]";

            pageManager = new textPageManager<aceMenuItem>(10, 100, 1);

            itemSettings = null;

            Int32 c = 0;
            foreach (var t in items)
            {
                if (itemSettings == null) itemSettings = new settingsEntriesForObject(t);
                var mi = new aceMenuItem(t.ToString(), c.ToString(), t.GetType().Name, "", t);
                menu.setItem(mi);
            }
        }

        public override void resetContent()
        {
            base.resetContent();

            setupFieldFormat("{0}", 25, 25);
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

            Object spec = null;
            foreach (aceMenuItem it in menu)
            {
                var id = renderSelectBox(it, menu.isDisabled(it), menu.isSelected(it), menu.isDefault(it)).add(it.itemName);
                writeField(id, printHorizontal.left);

                //var spec = it.metaObject as settingsPropertyEntryWithContext;
                //it.metaObject a

                writeField(it.metaObject.toStringSafe(), printHorizontal.middle);

                //writeField(name, printHorizontal.right);
                cursor.nextLine();
            }

            insertSplitLine();

            if (menu.selected != null)
            {
                spec = menu.selected.metaObject;
                if (spec != null)
                {
                    insertLine("[ENTER] Edit | [DEL] Delete | [F5] Duplicate");
                }
            }

            insertLine("[UP][DOWN] Select | [INS] New item | [ESC] Done");

            //if (spec != null)
            //{
            //    var extraLine = "";
            //    if (spec.pi.PropertyType.isBoolean())
            //    {
            //        Boolean bvl = !spec.value.imbToBoolean();
            //        extraLine += "[SPACE] Toggle to " + bvl.ToString() + " [+] True [-] False";
            //    }
            //    else if (spec.pi.PropertyType.IsEnum)
            //    {
            //        extraLine += "[+][-] Select value";
            //    }
            //    else if (spec.pi.PropertyType.isText())
            //    {
            //        extraLine += "[SPACE] Edit value";
            //    }
            //    else if (spec.pi.PropertyType.isNumber())
            //    {
            //        extraLine += "[SPACE] Edit value [+][-] Change value | with [Alt] 5x | with [Ctrl] 10x";
            //    }
            //    else
            //    {
            //        extraLine += "[SPACE] Edit value";
            //    }
            //    insertLine(extraLine, -1, false, 2);
            //}

            //insertLine("On leaving this screen changes will be saved.");
            //insertLine("[F2] Load preset | [F5] Save preset | [F12] Reset to default");

            cutSectionAtCursor();
        }

        /// <summary>
        /// #2 Očitava ulaz
        /// </summary>
        public inputResultCollection read(inputResultCollection __results)
        {
            if (__results == null) __results = new inputResultCollection();

            var min = __results.getBySection(this);
            var rd = base.read(__results.platform, min);
            __results.AddUniqueSection(rd);

            return __results;
        }
    }
}