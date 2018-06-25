// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceMenuItemCollection.cs" company="imbVeles" >
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
// Project: imbACE.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
using imbACE.Core;
using imbSCI.Core;
using imbSCI.Core.attributes;
using imbSCI.Core.enums;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.interfaces;
using imbSCI.Data;
using imbSCI.Data.collection;
using imbSCI.Data.data;
using imbSCI.Data.interfaces;
using imbSCI.DataComplex;
using imbSCI.Reporting;
using imbSCI.Reporting.enums;
using imbSCI.Reporting.interfaces;

namespace imbACE.Core.commands.menu.core
{
    using imbACE.Core.operations;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.files.job;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Reflection;

    /// <summary>
    /// Collection of menu items
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    /// <example>
    /// <c>menu.setItems(this, application.mainComponent);</c>
    /// </example>
    /// <seealso cref="imbACE.Core.collection.aceCollection{imbACE.Services.menu.core.aceMenuItem}" />
    /// <seealso cref="aceOperationSetExecutorBase"/>
    public class aceMenuItemCollection : aceCollection<aceMenuItem>
    {
        #region -----------  defaultOption  -------  [Opcija koja ce biti podrazumevana]

        private Int32 _defaultOption = 0; // = new Int32();

        /// <summary>
        /// Opcija koja ce biti podrazumevana
        /// </summary>
        // [XmlIgnore]
        [Category("aceTerminalMenu")]
        [DisplayName("defaultOption")]
        [Description("Opcija koja ce biti podrazumevana")]
        public Int32 defaultOption
        {
            get
            {
                return _defaultOption;
            }
            set
            {
                // Boolean chg = (_defaultOption != value);
                _defaultOption = value;
                OnPropertyChanged("defaultOption");
                // if (chg) {}
            }
        }

        #endregion -----------  defaultOption  -------  [Opcija koja ce biti podrazumevana]

        #region SET ITEM

        #region --- currentItemGroup ------- Bindable property

        private aceMenuItemGroup _currentItemGroup = aceMenuItemGroup.mainItems;

        /// <summary>
        /// Bindable property
        /// </summary>
        public aceMenuItemGroup currentItemGroup
        {
            get
            {
                return _currentItemGroup;
            }
            set
            {
                _currentItemGroup = value;
                OnPropertyChanged("currentItemGroup");
            }
        }

        #endregion --- currentItemGroup ------- Bindable property

        /// <summary>
        /// Postavlja novi item u menu
        /// </summary>
        /// <param name="itemTitle"></param>
        /// <param name="itemDisabledRemarks"></param>
        /// <param name="itemKey"></param>
        /// <param name="isDefault"></param>
        public aceMenuItem setItem(String itemTitle, String itemDisabledRemarks = "", String itemKey = "", Boolean isDefault = false)
        {
            aceMenuItem item = new aceMenuItem();
            item.itemName = itemTitle;
            item.itemRemarkDisabled = itemDisabledRemarks;
            item.key = itemKey;
            item.group = currentItemGroup;
            setItem(item);
            if (isDefault) defaultOption = item.index;
            return item;
        }

        /// <summary>
        /// Adds externally created aceMenuItem object
        /// </summary>
        /// <param name="_item">aceMenuItem object that was created outside</param>
        public void setItem(aceMenuItem _item)
        {
            if (Items.Contains(_item))
            {
                return;
            }
            if (_item.group == aceMenuItemGroup.none) _item.group = currentItemGroup;
            Add(_item);
            _item.index = IndexOf(_item);
        }

        /// <summary>
        /// Populates menu with provided <c>executor</c>
        /// </summary>
        /// <param name="executor">The executor: Console or Screen that performs operations</param>
        /// <param name="component">The specific component that is related to the execution of this menu</param>
        public void setItems(aceOperationSetExecutorBase executor, IAceComponent component = null, BindingFlags binding = BindingFlags.Public | BindingFlags.Instance)
        {
            var executorType = executor.GetType();
            MethodInfo[] __methods = executorType.GetMethods(binding);
            foreach (var __m in __methods)
            {
                if (__m.Name.StartsWith(aceMenuItemMeta.METHOD_PREFIX))
                {
                    aceMenuItem item = new aceMenuItem();
                    var args = new aceOperationArgs(executor, this, item, __m, component);
                    String displayName = __m.Name.removeStartsWith(aceMenuItemMeta.METHOD_PREFIX);

                    item.itemName = displayName.removeStartsWith(args.methodInfo.categoryName);

                    item.metaObject = args;
                    item.index = Count;
                    //item.group = ;
                    Add(item);
                }
            }
        }

        //public void setItemFromMethods(aceOperationSetExecutorBase executor, IAceComponent component=null)
        //{
        //    Type executorType = executor.GetType();
        //    MethodInfo[] __methods = executorType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        //    foreach (var __m in __methods)
        //    {
        //        if (__m.Name.StartsWith(aceMenuItemMeta.METHOD_PREFIX))
        //        {
        //            aceMenuItem item = new aceMenuItem();
        //            var args = new aceOperationArgs(executor, this, item, __m, component);
        //            item.itemName = __m.Name.removeStartsWith(aceMenuItemMeta.METHOD_PREFIX);

        //            item.metaObject = args;
        //            item.index = Count;
        //            item.group = currentItemGroup;
        //            Add(item);

        //        }
        //    }

        //}

        /// <summary>
        /// Populates menu with provided collection of strings
        /// </summary>
        /// <param name="_input"></param>
        public void setItems(IEnumerable<String> _input)
        {
            foreach (String _item in _input)
            {
                aceMenuItem item = new aceMenuItem();
                item.index = Count;
                //item.key = dictKey;
                item.itemName = _item;
                item.group = currentItemGroup;
                Add(item);
            }
        }

        /// <summary>
        /// Sets items by Enum <c>defOption</c> type values and sets <c>defOption</c> as default selection
        /// </summary>
        /// <param name="defOption">The definition option.</param>
        public void setItems(Enum defOption)
        {
            Type tp = defOption.GetType();
            var all = Enum.GetValues(tp);
            foreach (Enum _item in all)
            {
                aceMenuItemMeta itemMeta = new aceMenuItemMeta(_item as Enum);

                aceMenuItem item = new aceMenuItem(itemMeta, _item);
                setItem(item);
                if (defOption == _item)
                {
                    selected = item;
                }
            }
        }

        /// <summary>
        /// Populates menu with IEnumerable Enum values. It automatically takes aceMenuItem attribute values from Enum declaration
        /// </summary>
        /// <typeparam name="T">Enum with proper aceMenuItem attributes set</typeparam>
        /// <param name="_input">Array of Enum values</param>
        public void setItems<T>(IEnumerable<T> _input)
        {
            Type tp = typeof(T);

            foreach (T _item in _input)
            {
                if (tp.isEnum())
                {
                    aceMenuItemMeta itemMeta = new aceMenuItemMeta(_item as Enum);

                    aceMenuItem item = new aceMenuItem(itemMeta, _item);
                    setItem(item);
                }
                else if (tp.isText())
                {
                    String it = _item as String;
                    aceMenuItemMeta itemMeta = new aceMenuItemMeta();

                    aceMenuItem item = new aceMenuItem(itemMeta, _item);
                    item.itemName = it;

                    setItem(item);
                }
                else
                {
                    String it = _item.toStringSafe();
                    aceMenuItemMeta itemMeta = new aceMenuItemMeta();

                    aceMenuItem item = new aceMenuItem(itemMeta, _item);
                    item.itemName = it;

                    setItem(item);
                }
            }
        }

        public void setItemsFromDictionary(Dictionary<String, String> _input)
        {
            foreach (String dictKey in _input.Keys)
            {
                aceMenuItem item = new aceMenuItem();
                item.index = Count;
                item.key = dictKey;
                item.itemName = _input[dictKey];

                item.group = currentItemGroup;
                Add(item);
            }
        }

        #endregion SET ITEM

        /// <summary>
        /// Sklanja celu kolekciju iz menija
        /// </summary>
        /// <param name="items"></param>
        public void Remove(IEnumerable<aceMenuItem> items)
        {
            foreach (var it in items)
            {
                if (Contains(it))
                {
                    Remove(it);
                }
            }
        }

        /// <summary>
        /// Vraca default item -
        /// </summary>
        /// <param name="defOption">ako je -1 onda koristi podesavanja iz menija, ako je -2 onda vraca null</param>
        /// <returns></returns>
        public aceMenuItem getDefaultItem(Int32 defOption = -1)
        {
            if (defOption == -2) return null;
            if (defOption == -1) defOption = defaultOption;
            if (defOption > -1)
            {
                if (Count > defOption)
                {
                    return this[defOption];
                }
            }

            return null;
        }

        /// <summary>
        /// Vraca manuItem u skladu sa ulaznim parametrom - koristi se kod otvorenog unosa kljuca
        /// </summary>
        /// <param name="input">String vrednost moze biti broj ili key string</param>
        /// <param name="defOption">Ako je input neprihvatljiv onda default option</param>
        /// <returns>vraca item</returns>
        public aceMenuItem getItem(String input, Int32 defOption = -1, Boolean setAsSelected = true, Boolean disableDefOption = false)
        {
            aceMenuItem output = null;

            if (String.IsNullOrEmpty(input))
            {
                output = getDefaultItem(defOption);
            }
            else
            {
                input = input.Trim().ToLower();

                foreach (aceMenuItem _item in this)
                {
                    if (!String.IsNullOrEmpty(_item.key))
                    {
                        if (_item.key.ToLower() == input)
                        {
                            output = _item;
                            break;
                        }
                    }
                    if (!String.IsNullOrEmpty(_item.itemName))
                    {
                        if (_item.itemName.ToLower() == input)
                        {
                            output = _item;
                            break;
                        }
                    }
                    if (_item.index.ToString() == input)
                    {
                        output = _item;
                        break;
                    }
                }
            }

            if (output == null)
            {
                // ako nista nije pronasao onda proverava i aliase
                foreach (aceMenuItem _item in this)
                {
                    if (_item.itemMetaInfo.aliasList.Contains(input))
                    {
                        output = _item;
                        break;
                    }
                }
            }

            if (output == null)
            {
                if (disableDefOption)
                {
                    return null;
                }

                output = getDefaultItem(defOption);
            }
            if (output != null)
            {
                if (setAsSelected) selected = output;
            }

            return output;
        }

        public void clearAll()
        {
            Clear();
            disabledItems.Clear();
        }

        #region SELECTION

        private Boolean preSelect()
        {
            if (Any())
            {
                if (selected == null)
                {
                    selected = getDefaultItem();
                    if (selected == null)
                    {
                        selected = this[0];
                    }
                }

                if (Count == disabledItems.Count)
                {
                    return false;
                }
            }
            return selected != null;
        }

        #region --- doSkipDisabled ------- disejblovani itemi ne mogu da budu selektovani

        private Boolean _doSkipDisabled = true;

        /// <summary>
        /// disejblovani itemi ne mogu da budu selektovani
        /// </summary>
        public Boolean doSkipDisabled
        {
            get
            {
                return _doSkipDisabled;
            }
            set
            {
                _doSkipDisabled = value;
                OnPropertyChanged("doSkipDisabled");
            }
        }

        #endregion --- doSkipDisabled ------- disejblovani itemi ne mogu da budu selektovani

        /// <summary>
        /// Pomera seleektovan item na prethodno mesto
        /// </summary>
        public void selectPrev()
        {
            if (preSelect())
            {
                selected = this.takeItemRelativeTo(selected, -1);
                if (isDisabled(selected) && doSkipDisabled) selectPrev();
            }
        }

        /// <summary>
        /// Pomera selektovan item za sledece mesto
        /// </summary>
        public void selectNext()
        {
            if (preSelect())
            {
                selected = this.takeItemRelativeTo(selected, 1);
                if (isDisabled(selected) && doSkipDisabled) selectNext();
            }
        }

        #endregion SELECTION

        /// <summary>
        /// Najveca sirina key-a
        /// </summary>
        /// <returns></returns>
        public Int32 getMaxKeyLength()
        {
            Int32 len = 0;
            foreach (aceMenuItem _item in this)
            {
                len = Math.Max(len, _item.keyOrIndex().Length);
            }
            return len;
        }

        #region --- selected ------- selected item

        private aceMenuItem _selected = null;

        /// <summary>
        /// selected item
        /// </summary>
        public aceMenuItem selected
        {
            get
            {
                return _selected;
            }
            set
            {
                _selected = value;
                OnPropertyChanged("selected");
            }
        }

        #endregion --- selected ------- selected item

        public String getSelectedKey()
        {
            if (preSelect())
            {
                return selected.keyOrIndex();
            }
            return "";
        }

        public Boolean isDisabled(aceMenuItem item)
        {
            if (item == null) return true;
            if (disabledItems.Contains(item.itemName))
            {
                return true;
            }
            if (!String.IsNullOrEmpty(item.key))
            {
                if (disabledItems.Contains(item.key))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if given item is currently selected
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Boolean isSelected(aceMenuItem item)
        {
            return selected == item;
        }

        public Boolean isDefault(aceMenuItem item)
        {
            if (defaultOption == item.index)
            {
                return true;
            }
            return false;
        }

        #region DISABLED ITEMS

        #region -----------  disabledItems  -------  [namesAndKeysOfDisabledItems]

        private List<String> _disabledItems = new List<String>();

        /// <summary>
        /// namesAndKeysOfDisabledItems
        /// </summary>
        // [XmlIgnore]
        [Category("aceTerminalMenu")]
        [DisplayName("disabledItems")]
        [Description("namesAndKeysOfDisabledItems")]
        public List<String> disabledItems
        {
            get
            {
                return _disabledItems;
            }
            set
            {
                // Boolean chg = (_disabledItems != value);
                _disabledItems = value;
                OnPropertyChanged("disabledItems");
                // if (chg) {}
            }
        }

        #endregion -----------  disabledItems  -------  [namesAndKeysOfDisabledItems]

        /// <summary>
        /// postavlja za selektovanu opciju prvu stavku koja nije disejblovana
        /// </summary>
        /// <param name="items"></param>
        public void selectFirstEnabled(params aceMenuItem[] items)
        {
            foreach (var it in items)
            {
                if (!isDisabled(it))
                {
                    selected = it;
                    break;
                }
            }
        }

        public void setDisabled(Boolean isDisabled, Boolean keepDisabled, params aceMenuItem[] items)
        {
            foreach (var it in items)
            {
                if (isDisabled)
                {
                    if (!disabledItems.Contains(it.itemName))
                    {
                        disabledItems.Add(it.itemName);
                    }
                }
                else
                {
                    if (!keepDisabled)
                    {
                        if (disabledItems.Contains(it.itemName))
                        {
                            disabledItems.Remove(it.itemName);
                        }
                    }
                }
            }
        }

        public void setDisabled(IEnumerable<Int32> _disableList, Boolean enableOther = true)
        {
            if (enableOther)
            {
                disabledItems.Clear();
            }
            foreach (Int32 _item in _disableList)
            {
                if (Count > _item)
                {
                    String _itemName = this[_item].itemName; //.ToLower().Trim();
                    disabledItems.Add(_itemName);
                }
            }
        }

        public void setDisabled(IList<String> _disableList, Boolean enableOther = true)
        {
            if (enableOther)
            {
                disabledItems.Clear();
            }

            foreach (aceMenuItem _item in this)
            {
                if (_disableList.Contains(_item.itemName))
                {
                    disabledItems.Add(_item.itemName);
                    if (!String.IsNullOrEmpty(_item.key))
                    {
                        disabledItems.Add(_item.key);
                    }
                }
            }
        }

        #endregion DISABLED ITEMS
    }
}