// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbStopwatch.cs" company="imbVeles" >
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
    #region imbVeles using

    using imbSCI.Core.extensions.text;
    using System;
    using System.Diagnostics;

    #endregion imbVeles using

    /// <summary>
    /// Stoperica koja vraca intervale od poziva do poziva. If autorun is TRUE it will automatically start upon constructor executed
    /// </summary>
    public class imbStopwatch : Stopwatch
    {
        public static imbStopwatch mainStopWatch = new imbStopwatch("Diagnostic", false);
        public String message = "";

        private Double msecLast = 0;
        private Double msecSpan = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="imbStopwatch"/> class.
        /// </summary>
        /// <param name="_message">The optional message that may be used externaly</param>
        /// <param name="_autorun">if set to <c>true</c> [autorun].</param>
        public imbStopwatch(String _message = "", Boolean _autorun = true)
        {
            message = _message;
            if (_autorun) Restart();
        }

        /// <summary>
        /// Saopstava poruku vezanu za glavni stop watch
        /// </summary>
        /// <param name="_message"></param>
        public static void mainStart(String _message = "")
        {
            if (!String.IsNullOrEmpty(_message)) mainStopWatch.message = _message;
            mainStopWatch.Restart();
        }

        public static void mainReport(Boolean stopIt = true)
        {
            mainStopWatch.reportToLog(stopIt);
        }

        /// <summary>
        /// samostalno pravi log
        /// </summary>
        /// <returns></returns>
        public void reportToLog(Boolean stopIt = true)
        {
            //logSystem.log("Stopwatch: (" + getIntervalSecond() + ")" + message, logType.Execution);
            if (stopIt) Stop();
        }

        /// <summary>
        /// Vraca interval koji je prosao od proslog poziva u Decimal vrednosti
        /// </summary>
        /// <returns>Vreme u sekundama</returns>
        public String getIntervalString(Boolean sinceStart = false)
        {
            if (sinceStart)
            {
                return imbStringFormats.getTimeSecString(ElapsedMilliseconds);
            }
            msecSpan = ElapsedMilliseconds - msecLast;
            msecLast = ElapsedMilliseconds;
            return imbStringFormats.getTimeSecString(msecSpan);
        }

        /// <summary>
        /// Vraca interval koji je prosao od proslog poziva u Decimal vrednosti
        /// </summary>
        /// <returns>Vreme u sekundama</returns>
        public Decimal getIntervalSecond(Boolean sinceStart = false)
        {
            if (sinceStart)
            {
                return imbStringFormats.getSeconds(ElapsedMilliseconds, 2);
            }

            msecSpan = ElapsedMilliseconds - msecLast;
            msecLast = ElapsedMilliseconds;

            return imbStringFormats.getSeconds(msecSpan, 2);
        }
    }
}