// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceTerminalWelcomeScreen.cs" company="imbVeles" >
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
namespace imbACE.Services.terminal.smartScreen
{
    using imbACE.Core;
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.core.exceptions;
    using imbACE.Core.enums.platform;
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
    using System;
    using System.ComponentModel.DataAnnotations;

    //using imbACE.Core.zone;

    /// <summary>
    /// Preset class with Welcome Screen
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class aceTerminalWelcomeScreen<T> : aceTerminalMenuScreenBase<T> where T : aceTerminalApplication
    {
        #region --- message ------- sadrzaj poruke koja se prikazuje

        private String _message = "";

        /// <summary>
        /// sadrzaj poruke koja se prikazuje
        /// </summary>
        protected String message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                OnPropertyChanged("message");
            }
        }

        #endregion --- message ------- sadrzaj poruke koja se prikazuje

        #region --- messageTitle ------- sadrzaj naslova

        private String _messageTitle = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        protected String messageTitle
        {
            get
            {
                return _messageTitle;
            }
            set
            {
                _messageTitle = value;
                OnPropertyChanged("messageTitle");
            }
        }

        #endregion --- messageTitle ------- sadrzaj naslova

        #region --- messageSection ------- section sa porukom

        private smartMessageSection _messageSection;

        /// <summary>
        /// section sa porukom
        /// </summary>
        public smartMessageSection messageSection
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

        public aceTerminalWelcomeScreen(T _app)
            : base(_app, " ")
        {
            message = _app.appAboutInfo.welcomeMessage;
            messageTitle = _app.appAboutInfo.applicationName;
            init(_app.platform);
        }

        public aceTerminalWelcomeScreen(T _app, String welcomeMessage, String welcomeTitle, String __title = "Introduction") : base(_app, __title)
        {
            message = welcomeMessage;
            messageTitle = welcomeTitle;

            /// pozvati na kraju
            init(_app.platform);
        }

        /// <summary>
        /// #0 Called when the screen is constructed
        /// </summary>
        /// <param name="platform"></param>
        public override void init(IPlatform platform)
        {
            menu = new aceMenu();
            menu.menuTitle = "Welcome menu";
            menu.setItems(this);

            messageSection = new smartMessageSection(messageTitle, message, platform.height, platform.width, 2, 2);
            messageSection.height = 17;
            messageSection.margin.top = 3;
            messageSection.margin.bottom = 1;
            messageSection.padding.bottom = 1;
            messageSection.doInverseColors = false;
            messageSection.backColor = platformColorName.Black;
            messageSection.foreColor = platformColorName.Gray;

            menuSection = new smartMenuSection(menu, platform.height, platform.width, 2, 1);
            menuSection.renderView = textInputMenuRenderView.inlineKeyListGroup;
            menuSection.doShowTitle = false;
            menuSection.doInverseColors = false;
            menuSection.padding.top = 1;
            menuSection.padding.bottom = 1;
            menuSection.backColor = platformColorName.White;
            menuSection.foreColor = platformColorName.Blue;

            menuSection.setStyle(textSectionLineStyleName.itemlinst);

            messageSection.setAttachment(menuSection);

            layout.addLayer(messageSection, layerBlending.transparent, 50);
        }

        //protected aceMenuItem menuItemContinue = new aceMenuItem("Continue", "C", "", "", null);
        //protected aceMenuItem menuItemAbout = new aceMenuItem("About", "A", "", "", null);
        //protected aceMenuItem menuItemQuit = new aceMenuItem("Quit", "Q", "", "", null);

        [Display(GroupName = "run", Name = "Continue", ShortName = "C", Description = "Go to the main screen")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "It calls the main screen of the application")]
        /// <summary>Go to the main screen</summary>
        /// <remarks><para>It calls the main screen of the application</para></remarks>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_runContinue()
        {
            application.goToMainPage();
        }

        [Display(GroupName = "run", Name = "About", ShortName = "A", Description = "Show About information")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "Opens the About dialog")]
        /// <summary>Show About information</summary>
        /// <remarks><para>Opens the About dialog</para></remarks>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_runAbout()
        {
            // your code
        }

        [Display(GroupName = "run", Name = "Quit", ShortName = "ESC", Description = "Quits the application")]
        [aceMenuItem(aceMenuItemAttributeRole.ExpandedHelp, "It will quit the application")]
        /// <summary>Quits the application</summary>
        /// <remarks><para>It will quit the application</para></remarks>
        /// <seealso cref="aceOperationSetExecutorBase"/>
        public void aceOperation_runQuit()
        {
            application.doQuit();
        }

        /// <summary>
        /// Refresh call for dynamic part of content or applicative logic
        /// </summary>
        public override void refresh()
        {
        }

        /// <summary>
        /// Executes the screen-specific logic
        /// </summary>
        /// <param name="__inputs">The inputs.</param>
        /// <returns></returns>
        public override inputResultCollection execute(inputResultCollection __inputs)
        {
            var menuResult = __inputs.getBySection(menuSection);
            var selectedItem = menuResult.result as aceMenuItem;
            selectedItem.executeMeta();

            return __inputs;
        }
    }
}