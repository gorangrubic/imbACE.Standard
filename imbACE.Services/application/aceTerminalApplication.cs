// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceTerminalApplication.cs" company="imbVeles" >
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

namespace imbACE.Services.application
{
    using imbACE.Core.events;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.core;
    using imbACE.Services.textBlocks;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.smart;
    using imbSCI.Core.reporting.zone;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Text User Interface application base class
    /// </summary>
    /// <remarks>
    /// <para>To change currently displayed screen, just set desired screen to the <see cref="current"/> property.</para>
    /// <para>Go back mechanism is already implemented, however if you want to trigger previous screen back call <see cref="goBack"/> method, do not replace <see cref="current"/> on your own as it will lead to screen loop.</para>
    /// </remarks>
    /// <seealso cref="imbACE.Services.application.aceTerminalApplicationBase" />
    /// <seealso cref="imbACE.Services.terminal.core.IRenderReadExecute" />
    public abstract class aceTerminalApplication : aceTerminalApplicationBase
    {
        protected aceTerminalApplication() : base()
        {
        }

        /// <summary>
        /// Method called by each screen, on its opening
        /// </summary>
        /// <returns></returns>
        public virtual textLayout initCommonLayout()
        {
            textLayout layout = new textLayout(platform);

            layout.addLayer(footerLine, layerBlending.transparent, 10);
            //layout.addLayer(statusLine, layerBlending.transparent, 15);
            //layout.addLayer(headerLine, layerBlending.transparent, 20);

            return layout;
        }

        /// <summary>
        /// Gets or sets the screen welcome.
        /// </summary>
        /// <value>
        /// The screen welcome.
        /// </value>
        public IAceTerminalScreen screenWelcome { get; protected set; }

        /// <summary>
        /// Gets or sets the screen main.
        /// </summary>
        /// <value>
        /// The screen main.
        /// </value>
        public IAceTerminalScreen screenMain { get; protected set; }

        public smartInfoLineSection headerLine { get; set; }
        public smartInfoLineSection statusLine { get; set; }
        public smartInfoLineSection footerLine { get; set; }

        /// <summary>
        /// Goes to main page.
        /// </summary>
        public virtual void goToMainPage()
        {
            current = screenMain;
        }

        /// <summary>
        /// Goes to welcome page.
        /// </summary>
        public virtual void goToWelcomePage()
        {
            current = screenWelcome;
        }

        /// <summary>
        /// Goes back for one screen-step. If there are any screen in the <see cref="screenHistoryStack"/>
        /// </summary>
        public void goBack()
        {
            if (isGoBackEnabled)
            {
                callEventScreenClosed(_current);

                _current = screenHistoryStack.Pop();

                callEventScreenOpened(_current);
            }
        }

        /// <summary>
        /// Exits the application
        /// </summary>
        public override void doQuit()
        {
            doKeepRunning = false;
            current = null;
        }

        /// <summary>Event handler for <see cref="aceEventType.Opened"/>" at <see cref="aceEventOrigin.Screen"/>, to hook reaction method on to. </summary>;
        public event EventHandler<aceTerminalApplicationEventArgs> onEventScreenOpened;

        /// <summary>
        /// The event Opened at Screen caster, with optional pre-created arguments.
        /// </summary>
        /// <param name="openedScreen">The opened screen.</param>
        /// <remarks>
        /// Invokes <see cref="onEventScreenOpened" />, with <see cref="aceEventType.Opened" /> and <see cref="aceEventOrigin.Screen" />
        /// </remarks>
        protected virtual void callEventScreenOpened(IAceTerminalScreen openedScreen)
        {
            var e = new aceTerminalApplicationEventArgs();
            if (e.type == aceEventType.unknown) e.type = aceEventType.Opened;
            if (e.Origin == aceEventOrigin.unknown) e.Origin = aceEventOrigin.Screen;
            if (e.RelatedObject == null) e.RelatedObject = openedScreen;
            e.Message = "";
            if (onEventScreenOpened != null) onEventScreenOpened(this, e);
        }

        /// <summary>Event handler for <see cref="aceEventType.Closed"/>" at <see cref="aceEventOrigin.Screen"/>, to hook reaction method on to. </summary>;
        public event EventHandler<aceTerminalApplicationEventArgs> onEventScreenClosed;

        /// <summary>
        /// The event Closed at Screen caster, with optional pre-created arguments.
        /// </summary>
        /// <param name="closedScreen">The closed screen.</param>
        /// <remarks>
        /// Invokes <see cref="onEventScreenClosed" />, with <see cref="aceEventType.Closed" /> and <see cref="aceEventOrigin.Screen" />
        /// </remarks>
        protected virtual void callEventScreenClosed(IAceTerminalScreen closedScreen)
        {
            var e = new aceTerminalApplicationEventArgs();
            if (e.type == aceEventType.unknown) e.type = aceEventType.Closed;
            if (e.Origin == aceEventOrigin.unknown) e.Origin = aceEventOrigin.Screen;
            if (e.RelatedObject == null) e.RelatedObject = closedScreen;
            e.Message = "";
            if (onEventScreenClosed != null) onEventScreenClosed(this, e);
        }

        /// <summary>
        /// Record on screen changes, excluding the ones caused by <see cref="goBack"/> method call
        /// </summary>
        /// <value>
        /// The screen history stack.
        /// </value>
        protected Stack<IAceTerminalScreen> screenHistoryStack { get; set; } = new Stack<IAceTerminalScreen>();

        private IAceTerminalScreen _current;

        /// <summary>
        /// The current screen, implements <see cref="screenHistoryStack"/> management and calls <see cref="onScreenClosed(IAceTerminalScreen)"/> and <see cref="onScreenOpened(IAceTerminalScreen)"/> methods
        /// </summary>
        public IAceTerminalScreen current
        {
            get
            {
                return _current;
            }
            set
            {
                if (_current != null)
                {
                    if (screenHistoryStack.Count != 0)
                    {
                        callEventScreenClosed(screenHistoryStack.Peek());
                    }

                    screenHistoryStack.Push(_current);
                }
                _current = value;

                if (_current != null)
                {
                    callEventScreenOpened(_current);
                }
            }
        }

        /// <summary>
        /// If <c>true</c>, currently it is enabled to perform <see cref="goBack"/>
        /// </summary>
        public Boolean isGoBackEnabled
        {
            get
            {
                return (screenHistoryStack.Count != 0);
            }
        }

        /// <summary>
        /// Interface refresh loop
        /// </summary>
        /// <returns></returns>
        protected override bool doApplicationLoop()
        {
            if (current == null)
            {
                return false;
            }

            doUpdateInterface();

            Boolean doKeepReading = true;

            inputResultCollection results = new inputResultCollection();
            results.platform = platform;

            while (doKeepReading)
            {
                render(platform);
                results = read(results);
                doKeepReading = results.doKeepReading();
            }

            results = execute(results);

            //  current.loop(this);

            return doKeepRunning;
        }

        /// <summary>
        /// Reference to the currently active output platform
        /// </summary>
        public IPlatform platform
        {
            get { return aceCommons.platform; }
        }

        /// <summary>
        /// Called on the application start up, once settings are loaded and everything is ready to run
        /// </summary>
        protected override void StartUp()
        {
            headerLineLeftContent = appAboutInfo.applicationName + " " + appAboutInfo.applicationVersion;

            footerLine = new smartInfoLineSection(25, platform, 0, 2);
            footerLine.setStyle(textSectionLineStyleName.heading);

            current = screenWelcome;
        }

        /// <summary>
        /// #1 Renders the content
        /// </summary>
        /// <param name="platform">The platform on which the rendering should be performed</param>
        /// <param name="doClearScreen">if set to <c>true</c> it will clear screen.</param>
        protected virtual void render(IPlatform platform, Boolean doClearScreen = true)
        {
            doUpdateInterface();

            current.render(platform, doClearScreen);
        }

        /// <summary>
        /// #2 Reads the input
        /// </summary>
        /// <param name="__results">todo: describe __results parameter on read</param>
        protected virtual inputResultCollection read(inputResultCollection __results)
        {
            if (__results == null) __results = new inputResultCollection();

            __results.platform = platform;

            return current.read(__results);
        }

        /// <summary>
        /// #3 Forwards the input result to the <see cref="current"/> screen
        /// </summary>
        /// <param name="__inputs">todo: describe __inputs parameter on execute</param>
        protected virtual inputResultCollection execute(inputResultCollection __inputs)
        {
            __inputs.platform = platform;

            if (__inputs.doIfKey("Escape"))
            {
                goBack();
                return __inputs;
            }

            return current.execute(__inputs);
        }
    }
}