// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceTerminalMenu.cs" company="imbVeles" >
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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbACE.Services.terminal
{
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.core.exceptions;
    using System;
    using System.ComponentModel;

    public class aceTerminalMenu : aceMenu
    {
        #region -----------  metaInt  -------  [Meta broj]

        private Int32 _metaInt = 0; // = new Int32();

        /// <summary>
        /// Meta broj
        /// </summary>
        // [XmlIgnore]
        [Category("aceTerminalMenu")]
        [DisplayName("metaInt")]
        [Description("Meta broj")]
        public Int32 metaInt
        {
            get
            {
                return _metaInt;
            }
            set
            {
                // Boolean chg = (_metaInt != value);
                _metaInt = value;
                //  OnPropertyChanged("metaInt");
                // if (chg) {}
            }
        }

        #endregion -----------  metaInt  -------  [Meta broj]

        /*

        /// <summary>
        /// Prikazuje MENU i prikuplja odgovor
        /// </summary>
        /// <param name="console"></param>
        /// <param name="itemRendering"></param>
        /// <param name="showTitle"></param>
        /// <param name="showDescription"></param>
        /// <param name="defaultOpt"></param>
        /// <param name="__disabled"></param>
        /// <returns>Vraca ITEM koji je odabran</returns>
        public aceMenuItem showMenu(aceTerminal console, aceMenuItemRendering itemRendering, Boolean clearAll, Boolean showDescription, Int32 defaultOpt = -1)
        {
            if (clearAll)
            {
                console.WriteLine("----------------------------------------------------------");
                // console.Clear();
            }

            console.WriteLine(menuTitle);

            if (showDescription)
            {
                console.WriteLine(menuDescription);
            }

            if (defaultOpt == -1) defaultOpt = defaultOption;

            Int32 c = 0;
            String formatNumber = "[{0,-1}]";
            String formatNumberOrKey = "[{0,-1} or {1,-1}]";
            String format = "";
            if (items.Count()>9)
            {
                formatNumber = "[{0,-2}]";
            }

            format = "{0,-8} {1,-15} {2,-20}";

            foreach (aceMenuItem item in items)
            {
                String msg = "";
                String key = c.ToString();
                String remark = "";
                Boolean isDefault = (c == defaultOpt);
                item.index = c;

                if (isDisabled(item))
                {
                    key =  String.Format(formatNumber, "-");
                    remark = item.itemRemarkDisabled;
                } else
                {
                    switch (itemRendering)
                    {
                        case aceMenuItemRendering.onlyNumber:
                            key = String.Format(formatNumber, c);
                            break;

                        default:
                        case aceMenuItemRendering.numberOrKey:
                            if (!String.IsNullOrEmpty(item.key))
                            {
                                key = String.Format(formatNumber, item.key);
                            }
                            else
                            {
                                key = String.Format(formatNumber, c);
                            }
                            break;

                        case aceMenuItemRendering.onlyKey:
                            if (!String.IsNullOrEmpty(item.key))
                            {
                                key = String.Format(formatNumber, item.key);
                            }
                            break;

                        case aceMenuItemRendering.numberAndKey:
                            key = String.Format(formatNumberOrKey, c, item.key);
                            break;
                    }
                    remark = item.itemRemarkEnabled;
                }

                if (String.IsNullOrEmpty(key))
                {
                }
                else
                {
                    if (isDefault) key += " *";

                    msg = String.Format(format, key, item.itemName, remark);

                    Console.WriteLine(msg);
                }
                c++;
            }
            aceMenuItem defMenuItem = null;
            if (defaultOption > -1) defMenuItem = items[defaultOption];
            String input = "";
            if (defMenuItem != null)
            {
                input = aceTerminalInput.askForStringInline("Select menu option by [key]: ", defMenuItem.key);
            } else
            {
                input = aceTerminalInput.askForStringInline("Select menu option by [key]: ", "");
            }

           // console.log("input[" + input + "]");

            aceMenuItem selected = getItem(input, defaultOpt);

            if (isDisabled(selected))
            {
                console.log("Disabled:" + selected.index + " [" + selected.itemName + "] ");
                return null;
            } else
            {
                return selected;
            }
            return null;

    */
    }
}