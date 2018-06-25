// --------------------------------------------------------------------------------------------------------------------
// <copyright file="performanceResourcesTake.cs" company="imbVeles" >
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
    using imbSCI.Data.interfaces;
    using System;
    using System.ComponentModel;

    public class performanceResourcesTake : performanceTake, IPerformanceTake
    {
        public performanceResourcesTake()
        {
        }

        [Category("Memory")]
        [Description("MiB of RAM available to the application")]
        [DisplayName("Available")]
        [imb(imbAttributeName.measure_setUnit, "MiB")]
        [imb(imbAttributeName.measure_letter, "m_free")]
        [imb(imbAttributeName.reporting_valueformat, "#,###.##")]
        public Double availableMemory { get; set; }

        [Category("Memory")]
        [Description("MiB of physical memory installed on the system")]
        [DisplayName("Total")]
        [imb(imbAttributeName.measure_setUnit, "MiB")]
        [imb(imbAttributeName.measure_letter, "m_sys")]
        [imb(imbAttributeName.reporting_valueformat, "#,###.##")]
        public Double totalMemory { get; set; }

        [Category("Memory")]
        [Description("Bytes of physical memory allocated")]
        [DisplayName("Physical")]
        [imb(imbAttributeName.measure_setUnit, "MiB")]
        [imb(imbAttributeName.measure_letter, "MEM_ram")]
        [imb(imbAttributeName.reporting_valueformat, "#,###.##")]
        public Double physicalMemory { get; set; }

        [Category("Memory")]
        [Description("Bytes of virtual memory allocated")]
        [DisplayName("Virtual")]
        [imb(imbAttributeName.measure_setUnit, "MiB")]
        [imb(imbAttributeName.measure_letter, "MEM_vir")]
        [imb(imbAttributeName.reporting_valueformat, "#,###.##")]
        public Double virtualMemory { get; set; }

        [Category("Memory")]
        [Description("Bytes of paged memory allocated")]
        [DisplayName("Paged")]
        [imb(imbAttributeName.measure_setUnit, "MiB")]
        [imb(imbAttributeName.measure_letter, "MEM_page")]
        [imb(imbAttributeName.reporting_valueformat, "#,###.##")]
        public Double pagedMemory { get; set; }

        [Category("Hard disk")]
        [Description("HDD read bytes")]
        [DisplayName("Disk read")]
        [imb(imbAttributeName.measure_setUnit, "MiB")]
        [imb(imbAttributeName.measure_letter, "HDD_rd")]
        [imb(imbAttributeName.reporting_valueformat, "#,###.##")]
        public Double diskRead { get; set; }

        [Category("Hard disk")]
        [Description("HDD write bytes")]
        [DisplayName("Disk write")]
        [imb(imbAttributeName.measure_setUnit, "MiB")]
        [imb(imbAttributeName.measure_letter, "HDD_wr")]
        [imb(imbAttributeName.reporting_valueformat, "#,###.##")]
        public Double diskWrite { get; set; }

        [Category("CPU")]
        [Description("Processor utilization by the crawler process from 0% to 800% (8 cores)")]
        [DisplayName("CPU8")]
        [imb(imbAttributeName.measure_setUnit, "%")]
        [imb(imbAttributeName.measure_letter, "CPU_8c")]
        [imb(imbAttributeName.reporting_valueformat, "P2")]
        public Double cpuRateOfProcess { get; set; }

        [Category("Crawl")]
        [Description("Number of domain level crawls running at moment of sampling")]
        [DisplayName("Running")]
        [imb(imbAttributeName.measure_setUnit, "n of DLC threads")]
        [imb(imbAttributeName.measure_letter, "TC_i")]
        public Int32 dlcRunning { get; set; }

        [Category("Crawl")]
        [Description("Number of domain level crawls waiting in the Crawl Job")]
        [DisplayName("Waiting")]
        [imb(imbAttributeName.measure_setUnit, "n of DLC threads")]
        [imb(imbAttributeName.measure_letter, "TC_i")]
        public Double dlcWaiting { get; set; }

        [Category("Crawl")]
        [Description("Number of domain level crawls canceled at moment of sampling")]
        [DisplayName("Canceled")]
        [imb(imbAttributeName.measure_setUnit, "n of DLC threads")]
        [imb(imbAttributeName.measure_letter, "TC_i")]
        public Double dlcCanceled { get; set; }

        [Category("Data Load")]
        [Description("Requests made to the Loader component, cumulative")]
        [DisplayName("Load Requests (sum)")]
        [imb(imbAttributeName.measure_setUnit, "n of requests")]
        [imb(imbAttributeName.measure_letter, "PLr_sum")]
        public Int32 pageLoadsRealTotal { get; set; }

        [Category("Data Load")]
        [Description("Requests made to the Loader component")]
        [DisplayName("Load Requests")]
        [imb(imbAttributeName.measure_setUnit, "n of requests")]
        [imb(imbAttributeName.measure_letter, "PLr_i")]
        public Int32 pageLoadsRealSample { get; set; }

        [Category("Data Load")]
        [Description("Content Loaded cumulative, bytes of processed HTML source code")]
        [DisplayName("Data load (sum)")]
        [imb(imbAttributeName.measure_setUnit, "MiB")]
        [imb(imbAttributeName.measure_letter, "DL_sum")]
        [imb(imbAttributeName.reporting_valueformat, "#,###.##")]
        public Double bytesLoadedTotal { get; set; }

        [Category("Data Load")]
        [Description("Content Loaded, bytes of processed HTML source code, sample take")]
        [DisplayName("Data load")]
        [imb(imbAttributeName.measure_setUnit, "MiB")]
        [imb(imbAttributeName.measure_letter, "DL_i")]
        [imb(imbAttributeName.reporting_valueformat, "#,###.##")]
        public Double bytesLoadedSample { get; set; }
    }
}