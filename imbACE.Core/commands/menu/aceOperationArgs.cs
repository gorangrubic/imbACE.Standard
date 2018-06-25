// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceOperationArgs.cs" company="imbVeles" >
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

namespace imbACE.Core.commands.menu
{
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.core;
    using imbACE.Core.operations;
    using imbSCI.Core.data;
    using imbSCI.Core.files.job;
    using imbSCI.Core.syntax.param;
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Legacy abstractor for ace operation method parameter values
    /// </summary>
    public class aceOperationArgs : aceBindable, ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="aceOperationArgs"/> class.
        /// </summary>
        /// <param name="__executor">The executor.</param>
        /// <param name="__menu">The menu.</param>
        /// <param name="__item">The item.</param>
        /// <param name="__method">The method.</param>
        /// <param name="__component">The component.</param>
        public aceOperationArgs(aceOperationSetExecutorBase __executor, aceMenuItemCollection __menu, aceMenuItem __item, MethodInfo __method, IAceComponent __component = null)
        {
            _executor = __executor;
            _menu = __menu;
            _item = __item;
            _method = __method;
            _component = __component;
            _itemMetaData = new aceMenuItemMeta(_method);

            _methodInfo = new settingsMemberInfoEntry(_method);

            _item.deployMeta(_itemMetaData, this);
            _paramSet = itemMetaData.cmdParams;
        }

        public Object[] getInvokeArray()
        {
            if (method.GetParameters().Length == 0)
            {
                return null;
            }

            if (isMethodWithParameters)
            {
                return paramSet.GetValues();
            }
            else
            {
                return new Object[] { this };
            }
        }

        /// <summary>
        /// If the method has its own parameters and not using <see cref="aceOperationArgs"/>
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is method with parameters; otherwise, <c>false</c>.
        /// </value>
        public Boolean isMethodWithParameters
        {
            get
            {
                return _itemMetaData.isMethodWithParameters;
            }
        }

        /// <summary>
        /// Clones the object and sets values to the parameter set
        /// </summary>
        /// <param name="paramNames">The parameter names.</param>
        /// <param name="paramValues">The parameter values.</param>
        /// <returns></returns>
        public aceOperationArgs CloneFor(List<String> paramNames, List<String> paramValues)
        {
            aceOperationArgs output = this.Clone() as aceOperationArgs;

            output.paramSet.setValues(paramNames, paramValues);

            return output;
        }

        /// <summary>
        /// params
        /// </summary>
        private typedParamCollection _paramSet;

        /// <summary>
        /// params
        /// </summary>
        public typedParamCollection paramSet
        {
            get
            {
                return _paramSet;
            }
        }

        /// <summary>
        /// Gets the specified param value.
        /// </summary>
        /// <typeparam name = "T" ></ typeparam >
        /// < param name="paramname">The paramname.</param>
        /// <returns></returns>
        public Int32 GetInt32(String paramname)
        {
            Int32 output = 0;

            output = (Int32)paramSet[paramname];

            return output;
        }

        public Boolean GetBool(String paramname)
        {
            Boolean output = false;

            output = (Boolean)paramSet[paramname];

            return output;
        }

        public T Get<T>(String paramname)
        {
            T output = default(T);
            if (typeof(T).IsEnum)
            {
                output = (T)Enum.Parse(typeof(T), paramSet[paramname].ToString());
            }
            else
            {
                Object vl = paramSet[paramname];

                output = vl.imbConvertValueSafeTyped<T>();
            }
            return output;
        }

        /// <summary>
        /// Assigned method to invoke
        /// </summary>
        private MethodInfo _method;

        /// <summary>
        /// Assigned method to invoke
        /// </summary>
        public MethodInfo method
        {
            get
            {
                return _method;
            }
        }

        /// <summary>
        /// referenca prema komponenti koja je relevantna za ovaj menu
        /// </summary>
        private IAceComponent _component;

        /// <summary>
        /// referenca prema komponenti koja je relevantna za ovaj menu
        /// </summary>
        public IAceComponent component
        {
            get
            {
                return _component;
            }
        }

        /// <summary>
        /// Method member info - expanded
        /// </summary>
        private settingsMemberInfoEntry _methodInfo;

        /// <summary>
        /// Method member info - expanded
        /// </summary>
        public settingsMemberInfoEntry methodInfo
        {
            get
            {
                return _methodInfo;
            }
        }

        /// <summary>
        /// menu reference
        /// </summary>
        private aceMenuItemCollection _menu;

        /// <summary>
        /// menu reference
        /// </summary>
        public aceMenuItemCollection menu
        {
            get
            {
                return _menu;
            }
        }

        /// <summary>
        /// assigned menu item
        /// </summary>
        private aceMenuItem _item = null;

        /// <summary>
        /// assigned menu item
        /// </summary>
        public aceMenuItem item
        {
            get
            {
                return _item;
            }
        }

        /// <summary>
        /// Meta data for item
        /// </summary>
        private aceMenuItemMeta _itemMetaData;

        /// <summary>
        /// Meta data for item
        /// </summary>
        public aceMenuItemMeta itemMetaData
        {
            get
            {
                return _itemMetaData;
            }
        }

        /// <summary>
        /// executor
        /// </summary>
        private aceOperationSetExecutorBase _executor = null;

        /// <summary>
        /// executor
        /// </summary>
        public aceOperationSetExecutorBase executor
        {
            get { return _executor; }
        }

        /// <summary>
        /// propertyS shema with context
        /// </summary>
        private settingsEntriesForObject _shema;

        /// <summary>
        /// propertyS shema with context
        /// </summary>
        public settingsEntriesForObject shema
        {
            get
            {
                return _shema;
            }
        }

        public object Clone()
        {
            aceOperationArgs output = new aceOperationArgs(executor, menu, item, method, component);
            return output;
        }
    }
}