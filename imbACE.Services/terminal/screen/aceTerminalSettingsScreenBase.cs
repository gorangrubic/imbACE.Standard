// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceTerminalSettingsScreenBase.cs" company="imbVeles" >
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
namespace imbACE.Services.terminal.screen
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.data;
    using imbACE.Core.extensions;
    using imbACE.Services.application;
    using imbACE.Services.terminal.core;
    using imbSCI.Core.data;
    using System;
    using System.Collections.Generic;

    public abstract class aceTerminalSettingsScreenBase<T> : aceTerminalMenuScreenBase<T> where T : aceTerminalApplication
    {
        protected aceTerminalSettingsScreenBase(T terminalApplication, String __title) : base(terminalApplication, __title)
        {
        }

        #region --- settingsEntries ------- Bindable property

        private settingsEntriesForObject _settingsEntries;

        /// <summary>
        /// Bindable property
        /// </summary>
        protected settingsEntriesForObject settingsEntries
        {
            get
            {
                return _settingsEntries;
            }
            set
            {
                _settingsEntries = value;
                OnPropertyChanged("settingsEntries");
            }
        }

        #endregion --- settingsEntries ------- Bindable property

        /*

        #region --- menuItemDefault ------- postavi default vrednosti

        private aceMenuItem _menuItemDefault;
        /// <summary>
        /// postavi default vrednosti
        /// </summary>
        public aceMenuItem menuItemDefault
        {
            get
            {
                return _menuItemDefault;
            }
            set
            {
                _menuItemDefault = value;
                OnPropertyChanged("menuItemDefault");
            }
        }

        #endregion --- menuItemDefault ------- postavi default vrednosti

        #region --- menuItemSave ------- save changes

        private aceMenuItem _menuItemSave;
        /// <summary>
        /// save changes
        /// </summary>
        public aceMenuItem menuItemSave
        {
            get
            {
                return _menuItemSave;
            }
            set
            {
                _menuItemSave = value;
                OnPropertyChanged("menuItemSave");
            }
        }

        #endregion --- menuItemSave ------- save changes

        #region --- menuItemCancel ------- cancel canges

        private aceMenuItem _menuItemCancel;
        /// <summary>
        /// cancel canges
        /// </summary>
        public aceMenuItem menuItemCancel
        {
            get
            {
                return _menuItemCancel;
            }
            set
            {
                _menuItemCancel = value;
                OnPropertyChanged("menuItemCancel");
            }
        }

        #endregion --- menuItemCancel ------- cancel canges

        #region --- targetObject ------- objekat koji se menja

        private Object _targetObject;
        /// <summary>
        /// objekat koji se menja
        /// </summary>
        public Object targetObject
        {
            get
            {
                return _targetObject;
            }
            set
            {
                _targetObject = value;
                OnPropertyChanged("targetObject");
            }
        }

        #endregion --- targetObject ------- objekat koji se menja

        public virtual void initMenu(object toSetup)
        {
            targetObject = toSetup;

            settingsEntries = new settingsEntriesForObject(toSetup);
            menu.clearAll();
            menu.currentItemGroup = aceMenuItemGroup.mainItems;

            Int32 c = 0;
            foreach (KeyValuePair<string, settingsPropertyEntryWithContext> sme in settingsEntries.spes)
            {
                var menuItem = new aceMenuItem();
                string itemName = sme.Value.displayName + " (" + sme.Value.pi.Name + ":" + sme.Value.pi.PropertyType.Name + ")";
                menuItem.itemName = itemName;
                menuItem.itemRemarkEnabled = sme.Value.value.ToString();
                menuItem.metaObject = sme.Value;
                menuItem.helpLine = sme.Value.description;
                menuItem.index = c;
                menu.Add(menuItem);
                c++;
            }

            menu.currentItemGroup = aceMenuItemGroup.bottomItems;

            menuItemSave = new aceMenuItem("Save", "S", "Save current settings", "Nothing changed");
            menuItemDefault = new aceMenuItem("Default", "D", "Save current settings", "Nothing changed");
            menuItemCancel = new aceMenuItem("Cancel", "C", "Quit current changes", "");

            menu.setItem(menuItemSave);
            menu.setItem(menuItemDefault);
            menu.setItem(menuItemCancel);
        }

        public override void render()
        {
        }

        public override void executeMenu(aceMenuItem item)
        {
            if (item == menuItemSave)
            {
                application.goBack();
                return;
            }

            if (item == menuItemCancel)
            {
                application.goBack();
                return;
            }

            if (item == menuItemDefault)
            {
                targetObject = targetObject.GetType().getInstance();
                initMenu(targetObject);
                return;
            }
            //KeyValuePair<string, settingsPropertyEntryWithContext> pair = (_imbOperationMenuI)
            settingsPropertyEntryWithContext sme = item.metaObject as settingsPropertyEntryWithContext;
            if (sme != null)
            {
            }
        }
        */
    }
}