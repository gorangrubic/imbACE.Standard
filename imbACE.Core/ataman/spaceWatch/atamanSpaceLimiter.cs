// --------------------------------------------------------------------------------------------------------------------
// <copyright file="atamanSpaceLimiter.cs" company="imbVeles" >
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
using System.Linq;
using System.Text;

namespace imbACE.Core.ataman.spaceWatch
{
    /// <summary>
    /// Checks total harddrive allocation of associated directory and performes auto-cleaning of old files upon the limit is reached
    /// </summary>
    public class atamanSpaceLimiter
    {
        /// <summary> Number of megabytes allowed for assigned directory to allocate </summary>
        [Category("Limit")]
        [DisplayName("LimitMB")]
        [imb(imbAttributeName.measure_letter, "")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [Description("Number of megabytes allowed for assigned directory to allocate")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 LimitMB { get; set; } = default(Int32);

        /// <summary> Number of megabytes to leave undeleted (the newest content) </summary>
        [Category("Count")]
        [DisplayName("CleaningLimitMB")]
        [imb(imbAttributeName.measure_letter, "")]
        [imb(imbAttributeName.measure_setUnit, "n")]
        [Description("Number of megabytes to leave undeleted (the newest content)")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")]
        public Int32 CleaningLimitMB { get; set; } = default(Int32);
    }
}