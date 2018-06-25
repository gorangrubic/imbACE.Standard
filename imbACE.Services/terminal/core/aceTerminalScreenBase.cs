// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceTerminalScreenBase.cs" company="imbVeles" >
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
namespace imbACE.Services.terminal.core
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.extensions;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.textBlocks;
    using imbACE.Services.textBlocks.depracated;
    using imbACE.Services.textBlocks.input;
    using System;

    /// <summary>
    /// Osnova mehanizma za renderovanje konzolarnog ekrana
    /// </summary>
    public abstract class aceTerminalScreenBase : aceOperationSetExecutorBase, IRenderReadExecute, ItextLayout, IAceTerminalScreen
    {
        protected aceTerminalScreenBase(String __title)
        {
            title = __title;
        }

        private String _title = "";

        /// <summary>
        /// Screen title
        /// </summary>
        public String title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        private textLayout _layout;

        /// <summary>
        /// Screen layout
        /// </summary>
        public textLayout layout
        {
            get
            {
                return _layout;
            }
            set
            {
                _layout = value;
                OnPropertyChanged("layout");
            }
        }

        /// <summary>
        /// #0 Called when the screen is constructed
        /// </summary>
        /// <param name="platform"> </param>
        public abstract void init(IPlatform platform);

        /// <summary>
        /// Refresh call for dynamic part of content or applicative logic
        /// </summary>
        public abstract void refresh();

        /// <summary>
        /// Reads the user input
        /// </summary>
        /// <param name="__results">The results.</param>
        /// <returns></returns>
        public abstract inputResultCollection read(inputResultCollection __results);

        /// <summary>
        /// Executes the screen-specific logic
        /// </summary>
        /// <param name="__inputs">The inputs.</param>
        /// <returns></returns>
        public abstract inputResultCollection execute(inputResultCollection __inputs);

        /// <summary>
        /// #1 Renders the content
        /// </summary>
        /// <param name="platform">The platform on which the rendering should be performed</param>
        /// <param name="doClearScreen">if set to <c>true</c> it will clear screen.</param>
        public abstract void render(IPlatform platform, Boolean doClearScreen = true);

        public string layoutFooterMessage
        {
            get { return layout.layoutFooterMessage; }
            set { layout.layoutFooterMessage = value; }
        }

        public string layoutTitleMessage
        {
            get { return layout.layoutTitleMessage; }
            set { layout.layoutTitleMessage = value; }
        }

        public string layoutStatusMessage
        {
            get { return layout.layoutStatusMessage; }
            set { layout.layoutStatusMessage = value; }
        }
    }
}