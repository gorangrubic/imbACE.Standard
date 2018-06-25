// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbTimeStampFormat.cs" company="imbVeles" >
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
namespace imbACE.Core.core.diagnostic
{
    /// <summary>
    /// Format za kreiranje vremenskog potpisa
    /// </summary>
    public enum imbTimeStampFormat
    {
        /// <summary>
        /// Standard koji koristi YahooAPI
        /// </summary>
        totalSeconds,

        /// <summary>
        /// Standard koji koristi Alexa API. primer> 2016-07-22T14:01:43.357Z
        /// </summary>
        iso8601,

        /// <summary>
        /// Format koji obezbedjuje unikatni fajl name
        /// </summary>
        imbBackup,

        /// <summary>
        /// Format koji se koristi za formiranje cache fajla
        /// sadrzi MMdd
        /// </summary>
        imbCacheStamp,

        /// <summary>
        /// Precizna beleska o vremenu izvrsavanja: yy_MM_dd_HH:mm:ss:ffffff
        /// </summary>
        imbExecutionTimeStamp,

        /// <summary>
        /// vreme koje je prodeklo od startovanja aplikacije - > HH_mm_ss_ff - filesafe. primer: h0_m0_s11_ms287
        /// </summary>
        imbSinceApplicationStart,

        /// <summary>
        /// vreme koje je prodeklo od startovanja aplikacije -> manje precizno HHmmss . primer: h0m0s11
        /// </summary>
        imbSinceApplicationStartRough,

        /// <summary>
        /// Standardni format> MM-dd-yyyy HH:mm
        /// </summary>
        imbDatabase,

        /// <summary>
        /// Format koji sme da se koristi u nazivu tabele : dd_mm_yyyy
        /// </summary>
        imbDatabaseTableName,

        /// <summary>
        /// Vreme pokretanja aplikacije
        /// </summary>
        imbApplicationStartTime,

        /// <summary>
        /// potpis za filename sa exception output-om - da bi bilo lakse za pracenje
        /// </summary>
        imbExceptionStamp,

        /// <summary>
        /// Nema timestampa - vraca prazan string
        /// </summary>
        none,
    }
}