// --------------------------------------------------------------------------------------------------------------------
// <copyright file="timeStamp.cs" company="imbVeles" >
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

    using System;
    using System.Diagnostics;
    using System.Globalization;

    #endregion imbVeles using

    public static class timeStamp
    {
        public static Int32 exceptionCount = 0;
        private static Int32 _applicationRunStamp = 0;

        public static int applicationRunStamp
        {
            get
            {
                if (_applicationRunStamp == 0)
                {
                    Random rnd = new Random();
                    _applicationRunStamp = rnd.Next();
                }
                return _applicationRunStamp;
            }
            set { _applicationRunStamp = value; }
        }

        /// <summary>
        /// Generate the timestamp for the signature
        /// </summary>
        /// <returns></returns>
        public static String GenerateTimeStamp(this imbTimeStampFormat timeFormat)
        {
            return getTimeStamp(timeFormat);
        }

        public static String getTimeStamp(imbTimeStampFormat timeFormat = imbTimeStampFormat.totalSeconds)
        {
            String output = "";
            switch (timeFormat)
            {
                case imbTimeStampFormat.imbApplicationStartTime:
                    output = Process.GetCurrentProcess().StartTime.ToString("MM-dd-yyyy HH:mm",
                                                                            CultureInfo.InvariantCulture);

                    break;

                case imbTimeStampFormat.imbDatabase:
                    output = DateTime.Now.ToString("MM-dd-yyyy HH:mm", CultureInfo.InvariantCulture);

                    break;

                case imbTimeStampFormat.iso8601:
                    output = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture);

                    break;

                case imbTimeStampFormat.imbDatabaseTableName: // dd_mm_yyyy
                    output = DateTime.UtcNow.ToString("dd_mm_yyyy", CultureInfo.InvariantCulture);
                    break;

                case imbTimeStampFormat.totalSeconds:
                    TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    output = Convert.ToInt64(ts.TotalSeconds).ToString();
                    break;

                case imbTimeStampFormat.imbBackup:
                    output = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff", CultureInfo.InvariantCulture);
                    break;

                case imbTimeStampFormat.imbExecutionTimeStamp:
                    output = DateTime.Now.ToString("yy_MM_dd_HH:mm:ss:ffffff", CultureInfo.InvariantCulture);
                    break;

                case imbTimeStampFormat.imbSinceApplicationStart:
                    var format = "h{0}_m{1}_s{2}_ms{3}";
                    TimeSpan _tm = DateTime.Now - Process.GetCurrentProcess().StartTime;

                    output = String.Format(format, _tm.Hours.ToString(), _tm.Minutes.ToString(), _tm.Seconds.ToString(),
                                           _tm.Milliseconds.ToString());
                    break;

                case imbTimeStampFormat.imbSinceApplicationStartRough:
                    var format2 = "h{0}m{1}s{2}";
                    TimeSpan _tm2 = DateTime.Now - Process.GetCurrentProcess().StartTime;

                    output = String.Format(format2, _tm2.Hours.ToString(), _tm2.Minutes.ToString(), _tm2.Seconds.ToString());
                    break;

                case imbTimeStampFormat.imbExceptionStamp:
                    exceptionCount++;
                    output = "run" + applicationRunStamp.ToString("D7") + "-exc" + exceptionCount + "-";
                    //output += DateTime.UtcNow.ToString("MM-dd", CultureInfo.InvariantCulture);
                    break;

                case imbTimeStampFormat.imbCacheStamp:
                    output = DateTime.Now.ToString("MM", CultureInfo.InvariantCulture);
                    break;

                case imbTimeStampFormat.none:
                    output = "";
                    break;
            }
            return output;
            // Default implementation of UNIX time of the current UTC time
        }
    }
}