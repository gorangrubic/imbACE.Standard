// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceEventType.cs" company="imbVeles" >
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbACE.Core.events
{
    /// <summary>
    /// General descriptive enumeration for aceEvents (imbACE.Application Framework low-level layer at imbACE.Core)
    /// </summary>
    public enum aceEventType
    {
        /// <summary>
        /// The none: nothing happened
        /// </summary>
        none,

        /// <summary>
        /// The unknown: something happen but who knows what and to whom
        /// </summary>
        unknown,

        /// <summary>
        /// The universal: universal event, interpreted outside this library scope
        /// </summary>
        Universal,

        /// <summary>
        /// The loaded: something is just loaded
        /// </summary>
        Loaded,

        /// <summary>
        /// The opened: something was opened
        /// </summary>
        Opened,

        /// <summary>
        /// The updated: something was just updated
        /// </summary>
        Updated,

        /// <summary>
        /// The deployed: something has just deployed (installed to parent object or instance created
        /// </summary>
        Deployed,

        /// <summary>
        /// The created: something was just created
        /// </summary>
        Created,

        /// <summary>
        /// The ready: something is ready for next step in the process or to be used by user
        /// </summary>
        Ready,

        /// <summary>
        /// The saving: something is preparing to be saved
        /// </summary>
        Saving,

        /// <summary>
        /// The error: something had non-critical error
        /// </summary>
        Error,

        /// <summary>
        /// The exception: something had universaly handled exception
        /// </summary>
        Exception,

        /// <summary>
        /// The crashed: a process or object construction has crashed beyond usability
        /// </summary>
        Crashed,

        /// <summary>
        /// The failed: something failed in task it has received to do
        /// </summary>
        Failed,

        /// <summary>
        /// The started: something just started to perform given task
        /// </summary>
        Started,

        /// <summary>
        /// The paused: something has been temporary paused in task that was given
        /// </summary>
        Paused,

        /// <summary>
        /// The finished: something just finished with earlier given task
        /// </summary>
        Finished,

        /// <summary>
        /// The closing: something is about to close/shutdown
        /// </summary>
        Closing,

        /// <summary>
        /// The closed: something just closed/shotdown
        /// </summary>
        Closed,
    }
}