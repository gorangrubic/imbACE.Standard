// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbSystemInfo.cs" company="imbVeles" >
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

    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    #endregion imbVeles using

    /// <summary>
    /// Klasa za pristup sistemskim informacijama
    /// </summary>
    public class imbSystemInfo
    {
        public static String toStringReport(systemStatusData input)
        {
            builderForText sb = new builderForText();

            sb.AppendLine("Starting time: " + input.startedDateTime.ToShortTimeString());
            sb.AppendLine("Running time: " +
                          String.Format("{0}:{1}:{2}", input.timeRunningTimeSpan.Hours,
                                        input.timeRunningTimeSpan.Minutes, input.timeRunningTimeSpan.Seconds));
            sb.AppendLine("Memory usage: " + input.memoryUsage);
            sb.AppendLine("Threads count: " + input.threads);
            sb.AppendLine("Process list: ");
            foreach (Process p in input.ps)
            {
                try
                {
                    sb.AppendLine(p.ProcessName + " [" + p.Id + "] " + p.MainModule.FileName);
                }
                catch (Exception ex)
                {
                    sb.AppendLine(p.ProcessName + " [" + p.Id + "] not accessible :: " + ex.Message);
                }
            }
            sb.prevTabLevel();
            return sb.ToString();
        }

        public static String getSystemStatusReport()
        {
            systemStatusData rep = getSystemStatusData();
            return toStringReport(rep);
        }

        public static systemStatusData getSystemStatusData()
        {
            systemStatusData output = new systemStatusData();
            output.startedDateTime = Process.GetCurrentProcess().StartTime;
            output.timeRunningTimeSpan = DateTime.UtcNow - output.startedDateTime;

            output.memoryUsage = getMemory();

            output.ps = getProcessList("", true, output.startedDateTime, true);

            output.threads = getThreads();

            return output;
        }

        /// <summary>
        /// Vraca listu procesa prema datim uslovima
        /// </summary>
        /// <param name="nameFilter"></param>
        /// <param name="showOnlyNewProcesses"></param>
        /// <param name="startedSince"></param>
        /// <returns></returns>
        public static ObservableCollection<Process> getProcessList(String nameFilter, Boolean showOnlyNewProcesses,
                                                                   DateTime startedSince,
                                                                   Boolean hideSystemProcesses = true)
        {
            ObservableCollection<Process> output = new ObservableCollection<Process>();

            Process[] input = Process.GetProcesses();

            foreach (Process pr in input)
            {
                Boolean takeThis = false;
                if (!String.IsNullOrEmpty(nameFilter))
                {
                    if (pr.ProcessName.Contains(nameFilter)) takeThis = true;
                }
                else
                {
                    takeThis = true;
                }
                if (hideSystemProcesses)
                {
                    if (pr.SessionId == 0)
                    {
                        takeThis = false;
                    }
                }
                if (takeThis)
                {
                    if (showOnlyNewProcesses)
                    {
                        try
                        {
                            if (pr.StartTime < startedSince)
                            {
                                takeThis = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            takeThis = false;
                            // logSystem.log("Time catch failed for:" + pr.ProcessName, logType.ExecutionError);
                        }
                    }
                }
                if (takeThis) output.Add(pr);
            }

            return output;
        }

        public static String getProcessStartTime()
        {
            return System.Diagnostics.Process.GetCurrentProcess().StartTime.ToShortDateString() + " " + System.Diagnostics.Process.GetCurrentProcess().StartTime.ToShortTimeString();
        }

        public static String getProcessRunTimeSpan()
        {
            return imbSciStringExtensions.add(DateTime.Now.Subtract(System.Diagnostics.Process.GetCurrentProcess().StartTime).TotalMinutes.ToString("#0.00"), " mins");
        }

        /// <summary>
        /// Koliko se memorije trenutno korist
        /// </summary>
        /// <returns></returns>
        public static String getMemory()
        {
            //return getNumber(AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize);

            return getNumber(GC.GetTotalMemory(true));
        }

        public static String getThreads()
        {
            //  AppDomain.CurrentDomain.MonitoringTotalProcessorTime
            return getNumber(Process.GetCurrentProcess().Threads.Count, 1, "P");
        }

        /// <summary>
        /// Pretvara prosledjen broj odgovarajuci format
        /// </summary>
        /// <param name="input">Broj koji treba da se preformatira</param>
        /// <param name="factor">Broj sa kojim deli ulaz - za Mb je 100000</param>
        /// <param name="sufix">Sufix koji treba da se doda na kraju</param>
        /// <returns>Sredjen prikaz broja</returns>
        public static String getNumber(long input, long factor = 1000000, String sufix = "Mb")
        {
            input = input / factor;
            Decimal output = Math.Round(Convert.ToDecimal(input), 2);
            return output.ToString() + sufix;
        }
    }
}