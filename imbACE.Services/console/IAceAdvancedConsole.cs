// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAceAdvancedConsole.cs" company="imbVeles" >
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

namespace imbACE.Services.console
{
    using imbACE.Core.application;
    using imbACE.Core.commands.menu;
    using imbACE.Core.core;
    using imbACE.Core.operations;
    using imbACE.Services.platform.interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Second tier interface to imbACE Command Console stack
    /// </summary>
    /// <seealso cref="IAceCommandConsole" />
    public interface IAceAdvancedConsole : IAceCommandConsole
    {
        /// <summary>
        /// Logs the specified message, using the other color if required
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="otherColor">if set to <c>true</c> [other color].</param>
        void log(String message, Boolean otherColor = false);

        /// <summary>
        /// Gets a value indicating whether this instance is ready.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ready; otherwise, <c>false</c>.
        /// </value>
        Boolean isReady { get; }

        ///// <summary> </summary>
        //String consoleTitle { get; }

        ///// <summary> </summary>
        //String consoleHelp { get; }

        /// <summary> </summary>
        IPlatform platform { get; }

        aceAdvancedConsoleStateBase state { get; }

        aceAdvancedConsoleWorkspace workspace { get; }

        /// <summary>
        ///
        /// </summary>
       // builderForLog output { get; set; }

        ///// <summary>
        /////
        ///// </summary>
        //builderForLog response { get; set; }

        ///// <summary> </summary>
        //List<String> helpHeader { get; }

        aceCommandConsoleIOEncode encode { get; }

        ///// <summary>
        /////
        ///// </summary>
        //String helpContent { get; set; }

        ///// <summary> </summary>
        //Boolean consoleIsRunning { get; }

        /// <summary>
        /// Executes the script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="delay">The delay.</param>
        aceConsoleScript executeScript(IAceConsoleScript script, Int32 delay = 10);

        aceConsoleScript scriptRunning { get; }

        IAceApplicationBase application { get; }

        /// <summary>
        /// Clear Screen
        /// </summary>
        void cls();

        void onStartUp();

        ///// <summary>
        ///// Starts the console
        ///// </summary>
        //void start(IAceApplicationBase _application);

        /// Kreira event koji obaveštava da je promenjen neki parametar
        /// </summary>
        /// <remarks>
        /// Neće biti kreiran event ako nije spremna aplikacija: imbSettingsManager.current.isReady
        /// </remarks>
        /// <param name="name"></param>
        void OnPropertyChanged(string name);

        // event PropertyChangedEventHandler PropertyChanged;
    }
}