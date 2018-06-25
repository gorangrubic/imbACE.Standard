// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceTerminalMenuScreenBase.cs" company="imbVeles" >
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
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.core.exceptions;
    using imbACE.Services.application;
    using imbACE.Services.platform.core;
    using imbACE.Services.terminal.core;
    using imbACE.Services.textBlocks;
    using imbACE.Services.textBlocks.depracated;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.smart;
    using System;

    public abstract class aceTerminalMenuScreenBase<T> : aceTerminalScreenBase<T> where T : aceTerminalApplication
    {
        private aceMenu _menu;

        /// <summary>
        /// Data structure describing the menu
        /// </summary>
        protected aceMenu menu
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

        private smartMenuSection _menuSection;

        /// <summary>
        /// Section displaying the menu
        /// </summary>
        public smartMenuSection menuSection
        {
            get
            {
                return _menuSection;
            }
            set
            {
                _menuSection = value;
                OnPropertyChanged("menuSection");
            }
        }

        public override inputResultCollection read(inputResultCollection __results)
        {
            if (__results == null) __results = new inputResultCollection();

            var min = __results.getBySection(menuSection);

            if (menuSection == null)
            {
            }
            else
            {
                var rd = menuSection.read(__results.platform, min);
                __results.AddUniqueSection(rd);
            }

            return __results;
        }

        /// <summary>
        /// Screen with menu
        /// </summary>
        /// <param name="terminalApplication"></param>
        /// <param name="__title"></param>
        protected aceTerminalMenuScreenBase(T terminalApplication, string __title) : base(terminalApplication, __title)
        {
        }
    }
}