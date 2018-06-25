// --------------------------------------------------------------------------------------------------------------------
// <copyright file="builderForLog.cs" company="imbVeles" >
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
namespace imbACE.Core.core
{
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Core.reporting.render.core;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Special builder for log files. Has special call: log(). Immediate save as option
    /// </summary>
    /// <seealso cref="builderForMarkdown" />
    /// <seealso cref="ITextRender" />
    public class builderForLog : builderForMarkdown, ITextRender, ILogBuilder, IAceLogable, IAppendDataFieldsExtended
    {
        public override bool VAR_AllowAutoOutputToConsole
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Appends its data points into new or existing property collection: log statistics
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public PropertyCollectionExtended AppendDataFields(PropertyCollectionExtended data = null)
        {
            if (data == null) data = new PropertyCollectionExtended();

            String log = ContentToString();

            Int32 logEntries = contentElements.Count;

            var logLines = imbSciStringExtensions.SplitOnce(log, Environment.NewLine);
            Int32 logLinesCount = logLines.Count;
            String logMemSize = logContent.getStringMemByteSize();

            List<logContentExceptionsFlags> logExceptions = logContent.getEnumsDetectedInString<logContentExceptionsFlags>();

            data.Add("log_entries", logEntries, "Calls", "Count of log calls");
            data.Add("log_lines", logLinesCount, "Lines", "String line breaks count");
            data.Add("log_mem", logMemSize, "Memory", "Allocated memory by log string encoded as UTF8");
            data.Add("log_exc", logExceptions.toStringSafe(), "Exceptions", "Detected exceptions in log content");
            data.Add("log_out", outputPath, "File", "Output path associated with this log builder instance");
            data.Add("log_auto", immediateSaveOn, "Autosave", "If live autosave mode is enabled for this log builder)");

            return data;
        }

        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>
        /// Updated or newly created property collection
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        PropertyCollection IAppendDataFields.AppendDataFields(PropertyCollection data)
        {
            return AppendDataFields(data as PropertyCollectionExtended);
        }

        /// <summary>
        /// Registers the log builder.
        /// </summary>
        /// <param name="logid">The logid.</param>
        /// <returns></returns>
        public Boolean registerLogBuilder(logOutputSpecial logid, Boolean replace = false)
        {
            Object reg = aceLog.logBuilderRegistry.Add(logid, this, replace);
            if (reg is String)
            {
                outputPath = reg as String;
            }
            if (imbSciStringExtensions.isNullOrEmptyString(outputPath)) return false;
            return true;
        }

        /// <summary>
        /// Automatically registers the builder for log
        /// </summary>
        /// <param name="logid">The logid.</param>
        public builderForLog(logOutputSpecial logid) : base()
        {
            immediateSaveOn = false;
            registerLogBuilder(logid);
        }

        /// <summary>
        /// Builder without attached files
        /// </summary>
        public builderForLog() : base()
        {
            immediateSaveOn = false;
        }

        /// <summary>
        /// New instance of <see cref="builderForLog"/>
        /// </summary>
        /// <param name="__filename">The log output filename with extension .md. It will automatically set .md extension</param>
        /// <param name="autoSave">if set to <c>true</c> if will do automatic save on each log call.</param>
        public builderForLog(String __filename, Boolean autoSave, getWritableFileMode mode = getWritableFileMode.autoRenameExistingOnOtherDate) : base()
        {
            immediateSaveOn = autoSave;

            filename = imbSciStringExtensions.ensureEndsWith(__filename, ".md");

            if (autoSave)
            {
                String fileheader = "*AUTO SAVE LOG FILE*";
                FileInfo fi = outputPath.getWritableFile(mode); //fileheader.saveStringToFile(outputPath, getWritableFileMode.autoRenameExistingOnOtherDate);

                outputPath = fi.FullName;
            }
        }

        public override void prepareBuilder()
        {
            base.prepareBuilder();
            //formats.defaultFormat = reportOutputFormatName.textLog;

            outputPath = imbSciStringExtensions.add(outputSubPath, filename, "\\");

            //new reportOutputSupport(filename, reportOutputFormat.textLog, reportOutputFormat.)
        }

        /// <summary>
        /// The output path
        /// </summary>
        //protected String outputPath = "";

        private String _outputPath = "";

        /// <summary>
        ///
        /// </summary>
        public String outputPath
        {
            get { return _outputPath; }
            set { _outputPath = value; }
        }

        ///// <summary>
        ///// Name of active phase
        ///// </summary>
        //protected String phaseName = "";

        //private Int32 _phaseCount = 0;
        ///// <summary>
        /////
        ///// </summary>
        //public Int32 phaseCount
        //{
        //    get { return _phaseCount; }
        //    set { _phaseCount = value; }
        //}

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
                //   tb.meta = new imbStopwatch("Execution", true);

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
                    //imbStopwatch stw = tb.meta as imbStopwatch;
                    //if (stw != null)
                    //{
                    //    log("*" + tb.name + "* " + stw.getIntervalString(true));
                    //    stw.Stop();
                    //}
                }

                return tb;
            }
        }

        /// <summary>
        /// Log with Formatted message string
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="fin">The fin.</param>
        public void logF(String message, params Object[] fin)
        {
            String msg = String.Format(message, fin.getFlatArray<Object>());
            log(msg);
        }

        public static Int32 autoFlushLength = 1000;
        public static Boolean autoFlushDisabled = false;

        /// <summary>
        /// Logs the specified message. Adds short time string in front of message
        /// </summary>
        /// <param name="message">The message.</param>
        public void log(String message)
        {
            String msg = "" + DateTime.Now.ToShortTimeString() + " \t " + message;
            AppendLine(msg);

            aceLog.logAutoSavePermission(this, lastAutoSave, true);

            if (!autoFlushDisabled)
            {
                if (sb.Length > autoFlushLength)
                {
                    sb.Clear();
                    //aceLog.log(">> log builder auto-cleared");
                }
            }

            //if (immediateSaveOn)
            //{
            //    aceLog.logAutoSavePermission(this, lastAutoSave, true);
            //    //saveOverwrite();
            //}
        }

        protected StreamWriter sw;

        protected void saveOverwrite()
        {
            //content
            if (aceLog.logAutoSavePermission(this, lastAutoSave))
            {
                sb.ToString().saveStringToFile(outputPath, getWritableFileMode.overwrite, Encoding.UTF8);
            }
        }

        public long getByteSize()
        {
            return sb.Length;
        }

        private DateTime _lastAutoSave = DateTime.Now;

        /// <summary>
        ///
        /// </summary>
        public DateTime lastAutoSave
        {
            get { return _lastAutoSave; }
            set { _lastAutoSave = value; }
        }

        protected void saveAppend()
        {
            aceLog.logAutoSavePermission(this, lastAutoSave, true);

            //if (aceLog.logAutoSavePermission(this, lastAutoSave))
            //{
            //    sb.ToString().saveStringToFile(outputPath, getWritableFileMode.overwrite, Encoding.UTF8);

            //}

            //String ll = getLastLine();

            //sw.WriteLine(ll);
        }

        ILogBuilder ILogBuilder.logStartPhase(string title, string message)
        {
            open("Phase", title, message);
            return this;
        }

        ILogBuilder ILogBuilder.logEndPhase()
        {
            close("Phase");
            return this;
        }

        public void save(string destination_path = "")
        {
            outputPath = destination_path;

            //String pt = outputSubPath.add(filename, "\\");
            aceLog.logAutoSavePermission(this, lastAutoSave, true);
            //if (autosaveEnabled) sb.ToString().saveStringToFile(pt, getWritableFileMode.autoRenameExistingOnOtherDate, Encoding.UTF8);
        }

        private String _outputSubPath = "diagnostic";

        /// <summary>
        ///
        /// </summary>
        protected String outputSubPath
        {
            get { return _outputSubPath; }
            set { _outputSubPath = value; }
        }

        private Boolean _immediateSaveOn;

        /// <summary>
        /// Loger will automatically save its content after each new line
        /// </summary>
        public Boolean immediateSaveOn
        {
            get { return _immediateSaveOn; }
            set { _immediateSaveOn = value; }
        }

        private String _filename = "log.md";

        /// <summary>
        /// Filename used to stora log on hdd
        /// </summary>
        public String filename
        {
            get { return _filename; }
            set { _filename = value; }
        }

        /// <summary>
        /// Gets the content of the log.
        /// </summary>
        /// <value>
        /// The content of the log.
        /// </value>
        public string logContent
        {
            get
            {
                return ContentToString(false, reportOutputFormatName.textMdFile);
            }
        }
    }
}