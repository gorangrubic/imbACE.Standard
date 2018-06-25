// --------------------------------------------------------------------------------------------------------------------
// <copyright file="renderTask.cs" company="imbVeles" >
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

namespace imbACE.Core.commands.menu.render
{
    using imbACE.Core.enums.render;
    using imbACE.Core.operations;
    using System;
    using System.ComponentModel;

    public class renderTask : INotifyPropertyChanged
    {
        public renderTask(menuRendererTask __task, menuRenderFlag __flags)
        {
            task = __task;
            renderFlags = __flags;
        }

        public renderTask(aceMenuItemGroup __group, menuRenderFlag __flags)
        {
            task = menuRendererTask.menuItemGroup;
            group = __group;
            renderFlags = __flags;
        }

        #region --- task ------- Bindable property

        private menuRendererTask _task = menuRendererTask.none;

        /// <summary>
        /// Bindable property
        /// </summary>
        public menuRendererTask task
        {
            get
            {
                return _task;
            }
            set
            {
                _task = value;
                OnPropertyChanged("task");
            }
        }

        #endregion --- task ------- Bindable property

        #region --- renderFlags ------- Bindable property

        private menuRenderFlag _renderFlags = menuRenderFlag.listItems;

        /// <summary>
        /// Bindable property
        /// </summary>
        public menuRenderFlag renderFlags
        {
            get
            {
                return _renderFlags;
            }
            set
            {
                _renderFlags = value;
                OnPropertyChanged("renderFlags");
            }
        }

        #endregion --- renderFlags ------- Bindable property

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

        #region --- priority ------- Bindable property

        private Int32 _priority = 100;

        /// <summary>
        /// Bindable property
        /// </summary>
        public Int32 priority
        {
            get
            {
                return _priority;
            }
            set
            {
                _priority = value;
                OnPropertyChanged("priority");
            }
        }

        #endregion --- priority ------- Bindable property

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}