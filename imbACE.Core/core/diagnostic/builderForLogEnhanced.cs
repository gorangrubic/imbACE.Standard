// --------------------------------------------------------------------------------------------------------------------
// <copyright file="builderForLogEnhanced.cs" company="imbVeles" >
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

namespace imbACE.Core.core.diagnostic
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.extensions.io;
    using imbSCI.Core.collection;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Core.reporting.render.core;
    using System.Data;
    using System.Diagnostics;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    public class builderForLogEnhanced : builderForLog
    {
        private Object AppendExceptionLock = new object();

        /// <summary>
        /// Starts a phase in logging process
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="message">The message.</param>
        public builderForLog logStartPhase(String title, String message)
        {
            if (logEndPhase() == null)
            {
                stopwatch.Start();
            }
            tagBlock tb = open("phase", title, message);

            aceLog.logAutoSavePermission(this, lastAutoSave, true);

            //if (immediateSaveOn)
            //{
            //    saveOverwrite();
            //}

            return this;
        }

        /// <summary>
        /// Closes a phase in the logging proces
        /// </summary>
        /// <returns></returns>
        public builderForLog logEndPhase()
        {
            tagBlock tb = close("phase");

            if (tb != null)
            {
                AppendPair(tb.name, "finished after: " + stopwatch.getIntervalString());
                AppendHorizontalLine();
            }
            else
            {
                return null;
            }

            if (immediateSaveOn)
            {
                saveOverwrite();
            }
            return this;
        }

        private imbStopwatch _stopwatch = new imbStopwatch("Execution phase", false);

        /// <summary>
        /// Stop watch used internally
        /// </summary>
        protected imbStopwatch stopwatch
        {
            get { return _stopwatch; }
            set { _stopwatch = value; }
        }

        /// <summary>
        /// Appends the exception.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="ex">The ex.</param>
        /// <param name="exceptionLevel">The exception level.</param>
        public void AppendException(String title, Exception ex, Int32 exceptionLevel = 0)
        {
            lock (AppendExceptionLock)
            {
                throw new aceGeneralException("Exception: " + ex.Message, ex, this, title);

                open("Exception", title);

                if (ex == null)
                {
                    AppendLine("Exception: [null]");
                    return;
                }

                ex.describe(this, "");
                callerInfo ci = null;
                StackFrame[] frames = null;

                if (ex is aceGeneralException)
                {
                    aceGeneralException axe = (aceGeneralException)ex;
                    ci = axe.callInfo;
                }
                else
                {
                    StackTrace st = new StackTrace(ex, true);
                    StackFrame sf = st.GetFrame(0);
                    if (sf == null)
                    {
                        aceGeneralException axe = new aceGeneralException("WrapperException for " + ex.GetType().Name, ex, this, title);
                        ci = axe.callInfo;
                    }
                    else
                    {
                        ci = callerInfo.getCallerInfo(sf, true);
                    }

                    frames = st.GetFrames();
                }

                if (exceptionLevel == 0)
                {
                    AppendTable(ci.AppendDataFields());
                }

                AppendTable(ci.AppendDataFieldsOfMethod());

                open("parents", "Parent StackFrames");

                List<String> sfi_strings = new List<string>();
                Int32 c = 0;
                if (frames != null)
                {
                    foreach (StackFrame sfi in frames)
                    {
                        if (c != 0)
                        {
                            open("frame", "StackFrame(" + c.ToString() + ")");

                            callerInfo sfi_ci = callerInfo.getCallerInfo(sfi, true);

                            AppendTable(sfi_ci.AppendDataFieldsOfMethod());
                            close();
                        }
                        c++;

                        //sfi.GetFileLineNumb
                    }
                }
                close();

                if (ex.InnerException != null)
                {
                    AppendException("InnerException [level:" + exceptionLevel + "]", ex.InnerException, exceptionLevel + 1);
                }

                close();
            }
        }

        private Object openLock = new object();

        /// <summary>
        /// Opens the specified tag.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        /// \ingroup_disabled renderapi_append
        public override tagBlock open(String tag, String title = "", String description = "")
        {
            lock (openLock)
            {
                tagBlock tb = base.open(tag, title, description);
                tb.meta = new imbStopwatch("Execution", true);

                return tb;
            }
        }

        private Object closeLock = new object();

        /// <summary>
        /// Dodaje HTML zatvaranje taga -- zatvara poslednji koji je otvoren
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        /// <remarks>
        /// Ako je prosledjeni tag none onda zatvara poslednji tag koji je otvoren.
        /// </remarks>
        /// \ingroup_disabled renderapi_append
        public override tagBlock close(string tag = "none")
        {
            lock (closeLock)
            {
                tagBlock tb = base.close(tag);
                if (tb != null)
                {
                    imbStopwatch stw = tb.meta as imbStopwatch;
                    if (stw != null)
                    {
                        log("*" + tb.name + "* " + stw.getIntervalString(true));
                        stw.Stop();
                    }
                }

                return tb;
            }
        }
    }
}