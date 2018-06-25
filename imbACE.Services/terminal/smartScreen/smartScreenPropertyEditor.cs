// --------------------------------------------------------------------------------------------------------------------
// <copyright file="smartScreenPropertyEditor.cs" company="imbVeles" >
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

namespace imbACE.Services.terminal.smartScreen
{
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.data;
    using imbACE.Core.enums.platform;
    using imbACE.Core.extensions;
    using imbACE.Core.operations;
    using imbACE.Services.application;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.core;
    using imbACE.Services.terminal.screen;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.smart;
    using imbSCI.Core.reporting.zone;

    //using imbACE.Core.zone;

    public class smartScreenPropertyEditor<T> : aceTerminalScreenBase<T> where T : aceTerminalApplication
    {
        public smartScreenPropertyEditor(T __application, string __title) : base(__application, __title)
        {
            init(application.platform);
        }

        #region --- menu ------- osnovni menu

        private aceMenu _commands;

        /// <summary>
        /// osnovni menu
        /// </summary>
        protected aceMenu commands
        {
            get
            {
                return _commands;
            }
            set
            {
                _commands = value;
                OnPropertyChanged("menu");
            }
        }

        #endregion --- menu ------- osnovni menu

        #region --- menuSection ------- sekcija kojom prikazuje menu

        private smartMenuPropertyEditor _menuSection;

        /// <summary>
        /// sekcija kojom prikazuje menu
        /// </summary>
        protected smartMenuPropertyEditor menuSection
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

        #endregion --- menuSection ------- sekcija kojom prikazuje menu

        #region --- messageSection ------- section sa porukom

        private smartMessageSection _messageSection;

        /// <summary>
        /// section sa porukom
        /// </summary>
        protected smartMessageSection messageSection
        {
            get
            {
                return _messageSection;
            }
            set
            {
                _messageSection = value;
                OnPropertyChanged("messageSection");
            }
        }

        #endregion --- messageSection ------- section sa porukom

        public void setObject(Object target)
        {
            menuSection.setObject(target);
            refresh();
        }

        public Object getObject()
        {
            return menuSection.getObject();
        }

        /// <summary>
        /// #0 Izvrsava se prvi put - kada se instancira. Customized sekvenca inicijalizacije
        /// </summary>
        /// <param name="platform"> </param>
        public override void init(IPlatform platform)
        {
            String msg = "Status information";

            messageSection = new smartMessageSection("", "", platform.height, platform.width, 0, 2);
            messageSection.margin.top = 0;

            messageSection.setStyle(textSectionLineStyleName.heading);
            messageSection.foreColor = platformColorName.Blue;
            messageSection.backColor = platformColorName.White;
            //  messageSection.doInverseColors = true;
            messageSection.padding.top = 1;
            messageSection.padding.bottom = 1;
            messageSection.blending = layerBlending.hidden;
            messageSection.doInsertSplitLineAtEnd = false;

            //menu = new aceMenu();

            menuSection = new smartMenuPropertyEditor(23, platform.width, 1, 2);
            commands = menuSection.menu;

            commands.doSkipDisabled = false;

            menuSection.renderView = textInputMenuRenderView.listItemSelectable;
            menuSection.doShowValueRemarks = true;
            menuSection.doShowInstructions = true;
            menuSection.doInverseColors = false;
            menuSection.doShowRemarks = true;
            menuSection.exitPolicy = textInputExitPolicy.onValidKey;

            menuSection.margin.top = 0;
            menuSection.margin.bottom = 0;
            menuSection.padding.bottom = 1;
            menuSection.setStyle(textSectionLineStyleName.itemlinst);

            menuSection.setAttachment(messageSection);

            layout.addLayer(menuSection, layerBlending.transparent, 80);

            refresh();
        }

        /// <summary>
        /// #3 Vrsi rad nakon sto je obradjen ulaz
        /// </summary>
        public override inputResultCollection execute(inputResultCollection __inputs)
        {
            var results = __inputs;

            return results;
        }

        /// <summary>
        /// #2 Očitava ulaz
        /// </summary>
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
        /// Obnavlja dinamicki deo sadrzaja
        /// </summary>
        public override void refresh()
        {
        }
    }
}