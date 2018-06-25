// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textInputMenuBase.cs" company="imbVeles" >
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
using System.Linq;
using System.Text;

namespace imbACE.Services.textBlocks.input
{
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.commands.menu.render;
    using imbACE.Core.extensions;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.input;
    using imbACE.Services.textBlocks.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Data;

    /// <summary>
    /// Klasa koja daje osnovu za sve menu-like input sistem
    /// </summary>
    public abstract class textInputMenuBase : textInputBase
    {
        #region --- pageManager ------- upravlja

        private textPageManager<aceMenuItem> _pageManager = new textPageManager<aceMenuItem>();

        /// <summary>
        /// upravlja
        /// </summary>
        public textPageManager<aceMenuItem> pageManager
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

        #endregion --- pageManager ------- upravlja

        #region --- formatKeyBox ------- formatiranje key boxa

        private String _formatKeyBox = "[{0,-3}]";

        /// <summary>
        /// formatiranje key boxa
        /// </summary>
        protected String formatKeyBox
        {
            get
            {
                return _formatKeyBox;
            }
            set
            {
                _formatKeyBox = value;
                OnPropertyChanged("formatKeyBox");
            }
        }

        #endregion --- formatKeyBox ------- formatiranje key boxa

        protected void updateFormats()
        {
            if (menu != null)
            {
                Int32 len = menu.getMaxKeyLength();
                formatKeyBox = "[{0,-" + len.ToString() + "}]";
            }
        }

        protected void renderMenuItems(aceMenuItemCollection items, menuRenderFlag renderFlags, aceMenuItemGroup group = aceMenuItemGroup.none)
        {
            if (items == null)
            {
                //aceCommons.terminal.log("------ renderMenuItems called -"
                //return;
            }
            String inlineOutput = "";

            var itemsOnPage = pageManager.getPageElements(items);

            foreach (aceMenuItem item in itemsOnPage)
            {
                if ((group == aceMenuItemGroup.none) || item.group == group)
                {
                    Boolean isDisabled = items.isDisabled(item);
                    Boolean isSelected = items.isSelected(item);
                    Boolean isDefault = items.isDefault(item);
                    String itemString = renderItem(item, renderFlags, isDisabled, isSelected, isDefault);

                    if (renderFlags.HasFlag(menuRenderFlag.listItems))
                    {
                        writeLine(itemString, -1, false, 1);

                        //output.Add(itemString);
                    }
                    else
                    {
                        inlineOutput = inlineOutput.add(itemString, " ");
                    }
                    //output.appendLine(renderItem(item, renderFlags, isDisabled, isSelected, isDefault));
                }
            }

            if (renderFlags.HasFlag(menuRenderFlag.inlineItems))
            {
                insertLine(inlineOutput);
            }

            //return output;
        }

        protected string renderSelectBox(aceMenuItem item, bool isDisabled, bool isSelected, bool isDefault)
        {
            String keyPrefix = "";
            if (isSelected)
            {
                if (isDisabled)
                {
                    keyPrefix = "[=] ";
                }
                else
                {
                    keyPrefix = "[+] ";
                }
            }
            else
            {
                if (isDisabled)
                {
                    keyPrefix = "[-] ";
                }
                else
                {
                    keyPrefix = "[ ] ";
                }
            }
            return keyPrefix;
        }

        /// <summary>
        /// Generise izlaz za prosledjen item - ovu funkciju treba da koriste ostale funkcije viseg nivoa
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemRendering"></param>
        /// <param name="isDisabled"></param>
        /// <param name="isSelected"></param>
        /// <param name="isDefault"></param>
        /// <returns></returns>
        protected string renderItem(aceMenuItem item, menuRenderFlag renderFlags, bool isDisabled, bool isSelected, bool isDefault)
        {
            String formatForItem = "{0} {1} {2}";

            String msg = "";
            String key = item.keyOrIndex();

            String remark = "";

            String prefix = "";
            if (renderFlags.HasFlag(menuRenderFlag.showInlineRemarks))
            {
                if (isDisabled)
                {
                    remark = item.itemRemarkDisabled;
                }
                else
                {
                    remark = item.itemRemarkEnabled;
                }
            }
            String keyPrefix = ""; // renderSelectBox(item, isDisabled, isSelected, isDefault);

            if (renderFlags.HasFlag(menuRenderFlag.showSelectionBox))
            {
                keyPrefix = renderSelectBox(item, isDisabled, isSelected, isDefault);
            }
            else
            {
                if (renderFlags.HasFlag(menuRenderFlag.onlyNumber))
                {
                    key = String.Format(formatKeyBox, item.index);
                    if (isDisabled) key = String.Format(formatKeyBox, "-");
                }
                else if (renderFlags.HasFlag(menuRenderFlag.numberOrKey))
                {
                    key = String.Format(formatKeyBox, item.keyOrIndex());
                    if (isDisabled) key = String.Format(formatKeyBox, "-");
                }
                else if (renderFlags.HasFlag(menuRenderFlag.numberAndKey))
                {
                    key = String.Format(formatKeyBox, item.index.ToString(), item.key);
                    if (isDisabled) key = String.Format(formatKeyBox, "-");
                }
            }

            key = keyPrefix.add(key);

            if (renderFlags.HasFlag(menuRenderFlag.listItems))
            {
                formatForItem = "{0} {1,-15} {2,-20}";

                msg = String.Format(formatForItem, key, item.itemName.Replace("_", " "), remark);
                // target.insertLine(msg);
            }
            else if (renderFlags.HasFlag(menuRenderFlag.inlineItems))
            {
                if (!isSelected)
                {
                    formatForItem = "| {0} {1} |";
                }
                else
                {
                    formatForItem = "|={0}={1}=|";
                }
                msg = String.Format(formatForItem, key, item.itemName.Replace("_", " "), remark);
            }

            //msg = prefix + msg;

            return msg;
        }

        #region --- menu ------- referenca prema meniju

        private aceMenu _menu;

        /// <summary>
        /// referenca prema meniju
        /// </summary>
        public aceMenu menu
        {
            get
            {
                return _menu;
            }
            set
            {
                _menu = value;
                OnPropertyChanged("menu");
            }
        }

        #endregion --- menu ------- referenca prema meniju

        //public textInputMenuBase(aceMenu __menu, IPlatform platform, int __leftRightMargin = 0, int __leftRightPadding = 0): base(0, __width, __leftRightMargin, __leftRightPadding)
        //{
        //}

        protected textInputMenuBase(aceMenu __menu, int _height, int __width, int __leftRightMargin = 0, int __leftRightPadding = 0)
            : base(_height, __width, __leftRightMargin, __leftRightPadding)
        {
            if ((__menu == null) && (menu == null)) menu = new aceMenu();
            menu = __menu;
            updateFormats();
        }

        #region --- renderView ------- Bindable property

        private textInputMenuRenderView _renderView = textInputMenuRenderView.listItemSelectable;

        /// <summary>
        /// Bindable property
        /// </summary>
        public textInputMenuRenderView renderView
        {
            get
            {
                return _renderView;
            }
            set
            {
                _renderView = value;
                OnPropertyChanged("renderView");
            }
        }

        #endregion --- renderView ------- Bindable property

        /// <summary>
        /// Modifikuje pre svega visinu sekcije u skladu sa podesavanjima - poziva se pre rendera
        /// </summary>
        protected void adjustSize()
        {
            Int32 heighNow = height;
            Int32 targetHeight = 0;
            if (doShowTitle) targetHeight++;
            if (doShowRemarks) targetHeight += 2;
            if (doShowValueRemarks) targetHeight++;

            switch (renderView)
            {
                case textInputMenuRenderView.listKeyItem:
                    targetHeight += menu.Count;
                    targetHeight += 2;
                    break;

                case textInputMenuRenderView.listItemSelectable:
                    targetHeight += menu.Count;
                    break;

                case textInputMenuRenderView.inlineKeyListGroup:
                    targetHeight += 3;
                    break;
            }

            height = Math.Max(height, targetHeight);
        }

        protected String instructions = "";

        #region --- flags ------- menuRenderingFlags

        private menuRenderFlag _flags = menuRenderFlag.listItems;

        /// <summary>
        /// menuRenderingFlags
        /// </summary>
        public menuRenderFlag flags
        {
            get
            {
                return _flags;
            }
            set
            {
                _flags = value;
                OnPropertyChanged("flags");
            }
        }

        #endregion --- flags ------- menuRenderingFlags

        ///// <summary>
        ///// Primena procitanog unosa
        ///// </summary>
        ///// <param name="platform"></param>
        ///// <param name="__currentOutput"></param>
        ///// <returns></returns>
        //public abstract textInputResult applyReading(IPlatform platform, textInputResult __currentOutput);
    }
}