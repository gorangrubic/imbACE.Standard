// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StaHelper.cs" company="imbVeles" >
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

namespace imbACE.Core.core
{
    using extensions.io;
    using imbACE.Core.extensions;
    using System.IO;
    using System.Threading;
    using System.Windows;
    using System.Xml.Serialization;

    /// <summary>
    /// Single Thread Helper. Initial source code author: Paul Alexander,  https://stackoverflow.com/users/76456/paul-alexander
    /// </summary>
    /// <remarks>
    /// StackOverFlow topic: https://stackoverflow.com/questions/899350/how-do-i-copy-the-contents-of-a-string-to-the-clipboard-in-c
    /// </remarks>
    public abstract class StaHelper
    {
        private readonly ManualResetEvent _complete = new ManualResetEvent(false);

        public void Go()
        {
            var thread = new Thread(new ThreadStart(DoWork))
            {
                IsBackground = true,
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }

        // Thread entry method
        private void DoWork()
        {
            try
            {
                _complete.Reset();
                Work();
            }
            catch (Exception ex)
            {
                if (DontRetryWorkOnFailed)
                    throw;
                else
                {
                    try
                    {
                        Thread.Sleep(1000);
                        Work();
                    }
                    catch
                    {
                        // ex from first exception
                        aceLog.log("StaHelper exception: " + ex.Message);
                    }
                }
            }
            finally
            {
                _complete.Set();
            }
        }

        public abstract bool DontRetryWorkOnFailed { get; }

        // Implemented in base class to do actual work.
        protected abstract void Work();
    }
}