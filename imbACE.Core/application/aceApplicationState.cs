// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceApplicationState.cs" company="imbVeles" >
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
using imbACE.Core.ataman;
using imbACE.Core.core;
using imbACE.Core.events;
using imbACE.Core.operations;
using imbACE.Core.plugins;
using imbSCI.Core.files.folders;
using imbSCI.Data.data;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbACE.Core.application
{
    /// <summary>
    /// Basic information on application state
    /// </summary>
    /// <seealso cref="imbSCI.Data.data.imbBindable" />
    public class aceApplicationState : imbBindable
    {
        public DateTime appStartTime { get; set; } = DateTime.Now;

        /// <summary> If true it will keep the process running </summary>
        [Category("Flag")]
        [DisplayName("isRunning")]
        [Description("If true it will keep the process running")]
        public Boolean isRunning { get; set; } = false;

        public CultureInfo culture { get; set; }
    }
}