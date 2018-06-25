// --------------------------------------------------------------------------------------------------------------------
// <copyright file="performanceTake.cs" company="imbVeles" >
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
namespace imbACE.Core.data.measurement
{
    using imbSCI.Core.attributes;
    using imbSCI.Core.extensions.data;
    using imbSCI.Data.interfaces;
    using System;
    using System.ComponentModel;

    public class performanceTake : IPerformanceTake
    {
        public performanceTake()
        {
            samplingTime = DateTime.Now;
        }

        public performanceTake(DateTime __samplingTime, Double __secondsSinceLastTake, Double __reading)
        {
            samplingTime = __samplingTime;
            secondsSinceLastTake = __secondsSinceLastTake;
            reading = __reading;
        }

        public Int32 id { get; set; } = -1;

        public String idk
        {
            get
            {
                if (_idk.isNullOrEmpty()) _idk = id.ToString();
                return _idk;
            }
            set
            {
                _idk = value;
                if (id == -1)
                {
                    if (_idk.isNullOrEmpty()) _idk = "1";
                    id = Int32.Parse(_idk);
                }
            }
        }

        private DateTime _samplingTime;

        /// <summary>
        /// Time of take creation
        /// </summary>
        [Category("Sample")]
        [DisplayName("Time")]
        [Description("Sampling time")]
        [imb(imbAttributeName.reporting_valueformat, "T")]
        public DateTime samplingTime
        {
            get { return _samplingTime; }
            set { _samplingTime = value; }
        }

        /// <summary> Multiplier to convert reading into n per minute value. mK = 60 / secondsSinceLastTake </summary>
        [Category("Factor")]
        [DisplayName("PerMinuteFactor")]
        [imb(imbAttributeName.measure_letter, "mK")]
        [imb(imbAttributeName.measure_setUnit, "1/min")]
        [imb(imbAttributeName.reporting_valueformat, "#.000")]
        [Description("Multiplier to convert reading into n per minute value. mK = 60 / secondsSinceLastTake")] // [imb(imbAttributeName.measure_important)][imb(imbAttributeName.reporting_valueformat, "")][imb(imbAttributeName.reporting_escapeoff)]
        public Double PerMinuteFactor { get; set; } = 1;

        private Double _secondsSinceLastTake;

        /// <summary>
        ///
        /// </summary>
        [Category("Sample")]
        [DisplayName("Period")]
        [Description("Since last sample take, seconds")]
        [imb(imbAttributeName.reporting_valueformat, "#.00")]
        [imb(imbAttributeName.measure_setUnit, "s")]
        public Double secondsSinceLastTake
        {
            get { return _secondsSinceLastTake; }
            set { _secondsSinceLastTake = value; }
        }

        private Double _reading;
        private String _idk;

        /// <summary>
        /// Primary reading value
        /// </summary>
        [Category("Sample")]
        [DisplayName("Reading")]
        [Description("Primary measurement")]
        public Double reading
        {
            get { return _reading; }
            set { _reading = value; }
        }
    }
}