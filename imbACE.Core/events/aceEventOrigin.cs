// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceEventOrigin.cs" company="imbVeles" >
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
using imbSCI.Core.attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbACE.Core.events
{
    public enum aceEventOrigin
    {
        /// <summary>
        /// The none: there is no origin set
        /// </summary>
        none,

        /// <summary>
        /// The unknown: unknown origin of the event
        /// </summary>
        unknown,

        /// <summary>
        /// The framework: imbACI.Core and lower in the architecture
        /// </summary>
        Framework,

        /// <summary>
        /// The application: imbACE.Application had a major event
        /// </summary>
        Application,

        /// <summary>
        /// The settings: settings data object had an event
        /// </summary>
        Settings,

        /// <summary>
        /// The user: user done something
        /// </summary>
        User,

        /// <summary>
        /// The remote: remote server or client done something
        /// </summary>
        Remote,

        /// <summary>
        /// The console: command console had its own specific event
        /// </summary>
        Console,

        /// <summary>
        /// The screen: TUI screen
        /// </summary>
        Screen,

        /// <summary>
        /// The plugin: a plugin had event
        /// </summary>
        Plugin,

        /// <summary>
        /// The script: ACE or S# script had some event
        /// </summary>
        Script,

        /// <summary>
        /// The project: imbACE.Application project had an event
        /// </summary>
        Project,

        /// <summary>
        /// The component: a component had an event
        /// </summary>
        Component,

        /// <summary>
        /// The data object: data structure, collection and such kind of staff
        /// </summary>
        DataObject,

        /// <summary>
        /// The work object: active/procedural/algorithm - component/object had an event
        /// </summary>
        WorkObject,

        /// <summary>
        /// The filesystem: filesystem related event
        /// </summary>
        Filesystem,

        /// <summary>
        /// The ataman: ataman reporting
        /// </summary>
        Ataman,

        /// <summary>
        /// The task: a job task had event
        /// </summary>
        Task,

        /// <summary>
        /// The job: collection of tasks had an event
        /// </summary>
        Job,

        /// <summary>
        /// The web request: web request had an event
        /// </summary>
        WebRequest,

        /// <summary>
        /// The cache: cache system had event
        /// </summary>
        Cache,

        /// <summary>
        /// The operation system: operation system had event
        /// </summary>
        OperationSystem,

        /// <summary>
        /// The client: ACE client had an event
        /// </summary>
        Client,

        /// <summary>
        /// The server: ACE server had an event
        /// </summary>
        Server,

        /// <summary>
        /// The criterion: some criterion is just met
        /// </summary>
        Criterion,

        /// <summary>
        /// The scheduler: scheduler triggered an event
        /// </summary>
        Scheduler,

        /// <summary>
        /// The iteration: inside a iterative process had event
        /// </summary>
        Iteration,
    }
}