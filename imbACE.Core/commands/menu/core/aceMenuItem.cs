// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceMenuItem.cs" company="imbVeles" >
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
using imbSCI.Core.collection;
using imbSCI.Core.data;
using imbSCI.Core.enums;
using imbSCI.Core.extensions;
using imbSCI.Core.extensions.data;
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
    using imbACE.Core.core;
    using imbACE.Core.operations;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Osnovna klasa za meni item
    /// </summary>
    public class aceMenuItem : aceBindable
    {
        //public MemberInfo memberInfo { get; set; }

        public aceMenuItem()
        {
        }

        public aceMenuItem(aceMenuItemMeta meta, Object __metaObject)
        {
            deployMeta(meta, __metaObject);
            //itemName = meta[aceMenuItemAttributeRole.DisplayName];
            //key = meta[aceMenuItemAttributeRole.Key];
            //itemRemarkEnabled = meta[aceMenuItemAttributeRole.EnabledRemarks];
            //itemRemarkDisabled = meta[aceMenuItemAttributeRole.DisabledRemarks];
            //helpLine = meta[aceMenuItemAttributeRole.Description];

            // metaObject = __metaObject;
            //metaStringData = meta[aceMenuItemAttributeRole.Meta];
        }

        /// <summary>
        /// Meta object
        /// </summary>
        private aceMenuItemMeta _itemMetaInfo = new aceMenuItemMeta();

        /// <summary>
        /// Meta object
        /// </summary>
        public aceMenuItemMeta itemMetaInfo
        {
            get
            {
                return _itemMetaInfo;
            }
        }

        internal void deployMeta(aceMenuItemMeta meta, Object __metaObject)
        {
            //itemName
            //meta.TryGetValue(, out itemName);
            itemName = meta.getEntrySafe(aceMenuItemAttributeRole.DisplayName, "");
            key = meta.getEntrySafe(aceMenuItemAttributeRole.Key, "");
            itemRemarkEnabled = meta.getEntrySafe(aceMenuItemAttributeRole.EnabledRemarks, "");
            itemRemarkDisabled = meta.getEntrySafe(aceMenuItemAttributeRole.DisabledRemarks, "");
            helpLine = meta.getEntrySafe(aceMenuItemAttributeRole.Description, "");

            metaStringData = meta.getEntrySafe(aceMenuItemAttributeRole.Meta, "");
            _itemMetaInfo = meta;
            metaObject = __metaObject;
        }

        /// <summary>
        /// Executes the meta object
        /// </summary>
        /// <seealso cref="aceOperationArgs"/>
        /// <returns></returns>
        public String executeMeta()
        {
            String output = "";

            if (metaObject is aceOperationArgs)
            {
                aceOperationArgs marg = metaObject as aceOperationArgs;

                /*
                if (inputLine != "")
                {
                    marg.paramSet.addFromString(inputLine);
                }*/

                Object[] array = marg.getInvokeArray();

                if (marg.method != null)
                {
                    marg.method.Invoke(marg.executor, array);
                }

                output = "";
            }
            else
            {
                output = String.Format("metaObject[{0}] type is not supported yet ", metaObject.GetType().Name);
            }

            return output;
        }

        public aceMenuItem(String __name, String __key, String __enRem, String __disRem = "", Object __meta = null)
        {
            itemName = __name;
            key = __key;
            itemRemarkEnabled = __enRem;
            helpLine = __enRem;
            itemRemarkDisabled = __disRem;
            metaObject = __meta;
            // itemMetaInfo = new aceMenuItemMeta();
        }

        public String keyOrIndex()
        {
            if (String.IsNullOrEmpty(key))
            {
                return index.ToString();
            }
            else
            {
                return key;
            }
        }

        #region --- group ------- Bindable property

        private aceMenuItemGroup _group = aceMenuItemGroup.none;

        /// <summary>
        /// Bindable property
        /// </summary>
        public aceMenuItemGroup group
        {
            get
            {
                return _group;
            }
            set
            {
                _group = value;
                OnPropertyChanged("group");
            }
        }

        #endregion --- group ------- Bindable property

        #region --- helpLine ------- tekst koji se vidi kada je menu item odabran

        private String _helpLine = "";

        /// <summary>
        /// tekst koji se vidi kada je menu item odabran
        /// </summary>
        public String helpLine
        {
            get
            {
                return _helpLine;
            }
            set
            {
                _helpLine = value;
                OnPropertyChanged("helpLine");
            }
        }

        #endregion --- helpLine ------- tekst koji se vidi kada je menu item odabran

        #region -----------  index  -------  [broj koji ga aktivira]

        private Int32 _index; // = new Int32();

        /// <summary>
        /// broj koji ga aktivira
        /// </summary>
        // [XmlIgnore]
        [Category("aceMenuItem")]
        [DisplayName("index")]
        [Description("broj koji ga aktivira")]
        public Int32 index
        {
            get
            {
                return _index;
            }
            set
            {
                // Boolean chg = (_index != value);
                _index = value;
                OnPropertyChanged("index");
                // if (chg) {}
            }
        }

        #endregion -----------  index  -------  [broj koji ga aktivira]

        #region -----------  key  -------  [Slovo ili kratak string koje ga aktivira]

        private String _key = ""; // = new String();

        /// <summary>
        /// Slovo ili kratak string koje ga aktivira
        /// </summary>
        // [XmlIgnore]
        [Category("aceMenuItem")]
        [DisplayName("key")]
        [Description("Slovo koje ga aktivira")]
        public String key
        {
            get
            {
                return _key;
            }
            set
            {
                // Boolean chg = (_key != value);
                _key = value;
                OnPropertyChanged("key");
                // if (chg) {}
            }
        }

        #endregion -----------  key  -------  [Slovo ili kratak string koje ga aktivira]

        #region -----------  itemName  -------  [Naziv itema koji se prikazuje u meniju]

        private String _itemName; // = new String();

        /// <summary>
        /// Naziv itema koji se prikazuje u meniju
        /// </summary>
        // [XmlIgnore]
        [Category("aceMenuItem")]
        [DisplayName("itemName")]
        [Description("Naziv itema koji se prikazuje u meniju")]
        public String itemName
        {
            get
            {
                return _itemName;
            }
            set
            {
                // Boolean chg = (_itemName != value);
                _itemName = value;
                OnPropertyChanged("itemName");
                // if (chg) {}
            }
        }

        #endregion -----------  itemName  -------  [Naziv itema koji se prikazuje u meniju]

        #region -----------  itemRemarkEnabled  -------  [Komentar koji se upisuje kada je item enabled]

        private String _itemRemarkEnabled = ""; // = new String();

        /// <summary>
        /// Komentar koji se upisuje kada je item enabled
        /// </summary>
        // [XmlIgnore]
        [Category("aceMenuItem")]
        [DisplayName("itemRemarkEnabled")]
        [Description("Komentar koji se upisuje kada je item enabled")]
        public String itemRemarkEnabled
        {
            get
            {
                return _itemRemarkEnabled;
            }
            set
            {
                // Boolean chg = (_itemRemarkEnabled != value);
                _itemRemarkEnabled = value;
                OnPropertyChanged("itemRemarkEnabled");
                // if (chg) {}
            }
        }

        #endregion -----------  itemRemarkEnabled  -------  [Komentar koji se upisuje kada je item enabled]

        #region -----------  itemRemarkDisabled  -------  [Komentar koji se upisuje kada je item disabled]

        private String _itemRemarkDisabled = ""; // = new String();

        /// <summary>
        /// Komentar koji se upisuje kada je item disabled
        /// </summary>
        // [XmlIgnore]
        [Category("aceMenuItem")]
        [DisplayName("itemRemarkDisabled")]
        [Description("Komentar koji se upisuje kada je item disabled")]
        public String itemRemarkDisabled
        {
            get
            {
                return _itemRemarkDisabled;
            }
            set
            {
                // Boolean chg = (_itemRemarkDisabled != value);
                _itemRemarkDisabled = value;
                OnPropertyChanged("itemRemarkDisabled");
                // if (chg) {}
            }
        }

        #endregion -----------  itemRemarkDisabled  -------  [Komentar koji se upisuje kada je item disabled]

        #region -----------  metaObject  -------  [Dodatni objekat]

        private Object _metaObject; // = new Object();

        /// <summary>
        /// Dodatni objekat
        /// </summary>
        // [XmlIgnore]
        [Category("aceMenuItem")]
        [DisplayName("metaObject")]
        [Description("Dodatni objekat")]
        public Object metaObject
        {
            get
            {
                return _metaObject;
            }
            set
            {
                // Boolean chg = (_metaObject != value);
                _metaObject = value;
                OnPropertyChanged("metaObject");
                // if (chg) {}
            }
        }

        #endregion -----------  metaObject  -------  [Dodatni objekat]

        #region -----------  metaStringData  -------  [Dodatni string data]

        private String _metaStringData; // = new String();

        /// <summary>
        /// Dodatni string data
        /// </summary>
        // [XmlIgnore]
        [Category("aceMenuItem")]
        [DisplayName("metaStringData")]
        [Description("Dodatni string data")]
        public String metaStringData
        {
            get
            {
                return _metaStringData;
            }
            set
            {
                // Boolean chg = (_metaStringData != value);
                _metaStringData = value;
                OnPropertyChanged("metaStringData");
                // if (chg) {}
            }
        }

        #endregion -----------  metaStringData  -------  [Dodatni string data]
    }
}