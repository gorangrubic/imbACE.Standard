// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceLog.cs" company="imbVeles" >
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
    using imbSCI.Core;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.format;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.config;
    using imbSCI.Core.reporting.render.converters;
    using imbSCI.Core.reporting.render.core;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using imbSCI.Data.enums.fields;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// ACE main log system
    /// </summary>
    /// <seealso cref="ITextRender" />
    public class aceLog : ILogBuilder
    {
        public static void appShutdown()
        {
            aceLog.loger.AppendLine("-- Application shutdown -- " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            saveAllLogs(true);
        }

        private static Boolean isInitiated = false;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static aceLogToConsoleControl consoleControl
        {
            get
            {
                if (!isInitiated)
                {
                    isInitiated = true;
                    screenOutputControl.logToConsoleControl.setAsOutput(aceLog.loger.logBuilder, nameof(aceLog));
                }
                return screenOutputControl.logToConsoleControl;
            }
        }

        private static Object saveAllLogsLock = new object();

        /// <summary>
        /// Saves all globally registered<see cref= "imbACE.Core.core.builderForLog" /> instances.If no exception thrown it returns<c>true</c>. Otherwise<c>false</c>. During the proces list of log files created is written in <see cref = "Console" />
        /// </ summary >
        /// < param name= "autosaveOverride" >if set to <c>true</c> [autosave override].</param>
        /// <returns></returns>
        /// <remarks>
        /// Intent: For application crash or normal termination log save.
        /// </remarks>
        /// <example>
        /// Example of proper
        /// <code>
        /// static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        /// {
        ///     aceCommonServices.aceCommons.reportUnhandledException(e.ExceptionObject as Exception, mainComponent);
        ///     aceLog.saveAllLogs(true);
        /// }
        /// </code></example>
        public static Boolean saveAllLogs(Boolean autosaveOverride)
        {
            lock (saveAllLogsLock)
            {
                Double ts = DateTime.Now.Subtract(aceLog.loger.lastSaveAllLogsCall).TotalSeconds;

                if (ts < 10)
                {
                    Console.WriteLine(" Execution deniad due time lock.  aceLog.saveAllLogs() called again in less then 10 seconds time span. Possible StackOverFlow code loop.");
                    return false;
                }
                try
                {
                    if (imbACECoreConfig.settings.doAllowLogAutosave || autosaveOverride)
                    {
                        foreach (KeyValuePair<Object, String> pair in aceLog.logBuilderRegistry)
                        {
                            //FileInfo fi = pair.Value.getWritableFile(getWritableFileMode.autoRenameExistingOnOtherDate);
                            String strContent = "";
                            Object content = aceLog.logBuilderRegistry.getLogContent(pair.Value);
                            if (content is String)
                            {
                                strContent = content.toStringSafe();
                                if (!strContent.isNullOrEmpty())
                                {
                                    strContent.saveStringToFile(pair.Value, getWritableFileMode.autoRenameExistingOnOtherDate, Encoding.UTF8);
                                }
                                else
                                {
                                    //aceLog.log("Content for " + pair.Value + " is empty, nothing saved");
                                }
                            }
                            else
                            {
                                aceLog.log("Content for " + pair.Value + " isn't string but [" + content.GetType().Name + "] -> nothing saved");
                            }

                            // Console.WriteLine("Log file saved to: " + pair.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
                aceLog.loger.lastSaveAllLogsCall = DateTime.Now;
            }
            return true;
        }

        private DateTime _lastSaveAllLogsCall = DateTime.Now;

        /// <summary>
        ///
        /// </summary>
        public DateTime lastSaveAllLogsCall
        {
            get { return _lastSaveAllLogsCall; }
            set { _lastSaveAllLogsCall = value; }
        }

        private static Object logAutoSavePermissionLock = new object();

        public void AppendException(string v, ArgumentException argEx)
        {
            //log(argEx)
            throw new NotImplementedException();
        }

        //private static Object logAutoSavePresourceLock = new Object();

        /// <summary>
        /// Gets if  automatic save is allowed for the instance
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="lastAutoSave">The last automatic save.</param>
        /// <returns></returns>
        public static Boolean logAutoSavePermission(builderForLog source, DateTime lastAutoSave, Boolean saveFromAceLog = false)
        {
            Boolean output = false;
            if (!imbACECoreConfig.settings.doAllowLogAutosave) return false;

            if (!source.immediateSaveOn) return false;
            if (source.outputPath.isNullOrEmpty()) return false;

            if (imbACECoreConfig.settings.autosaveTimeSpanOn)
            {
                if (DateTime.Now.Subtract(lastAutoSave).TotalSeconds > imbACECoreConfig.settings.autosaveTimeSpanInSec)
                {
                    output = true;
                    source.lastAutoSave = DateTime.Now;
                }
            }

            FileInfo fi = null;
            if (output && imbACECoreConfig.settings.doAutoFlushLogPage)
            {
                lock (logAutoSavePermissionLock)
                {
                    if (source.Length.getFileSizeScaled(imbStringFormats.fileSizeUnit.Mb) > imbACECoreConfig.settings.autoFlushLogPageKByteLimit)
                    {
                        String logOutput = source.ContentToString(false);
                        fi = fi.FullName.saveStringToFile(logOutput, getWritableFileMode.autoRenameExistingToNextPage, Encoding.UTF8);
                        output = false;
                        if (fi != null)
                        {
                            source.Clear();
                        }
                    }
                }
            }
            else if (output)
            {
                lock (logAutoSavePermissionLock)
                {
                    String logOutput = source.ContentToString(false);
                    fi = fi.FullName.saveStringToFile(logOutput, getWritableFileMode.autoRenameExistingToNextPage, Encoding.UTF8);
                    output = false;
                    if (fi != null)
                    {
                        source.Clear();
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Logs the specified message via the main <see cref="loger"/>. If the <c>instance specified creates <c>ToStringDump</c> limited output</c>
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="instance">The instance.</param>
        public static void log(String message, Object instance = null, bool highlight = false)
        {
            if (highlight) loger.logBuilder.consoleAltColorToggle();
            loger.logBuilder.log(message);
            if (instance != null)
            {
                String msg = " -- log instance: " + instance.toStringSafe();
                // msg = msg + instance.ToStringDump(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty, 10);
                loger.AppendCode(msg);
            }
            if (highlight) loger.logBuilder.consoleAltColorToggle();
        }

        #region --- log ------- The main log mechanism for aceCommonTypes

        private static aceLog _loger;

        /// <summary>
        /// The main log mechanism for aceCommonTypes
        /// </summary>
        public static aceLog loger
        {
            get
            {
                if (_loger == null)
                {
                    _loger = new aceLog();
                    _loger.AppendLine("# Log started [" + loger.GetType().Name + "]");
                }
                return _loger;
            }
        }

        #endregion --- log ------- The main log mechanism for aceCommonTypes

        private static aceLogRegistry _logBuilderRegistry;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static aceLogRegistry logBuilderRegistry
        {
            get
            {
                if (_logBuilderRegistry == null)
                {
                    _logBuilderRegistry = new aceLogRegistry();
                    _logBuilderRegistry.Add(logOutputSpecial.aceSubsystem, aceLog.loger);
                }
                return _logBuilderRegistry;
            }
        }

        /// <summary>
        /// Waits the specified n of seconds. If thick meessage undefined it writes: 1... 2... 3... and so on.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        /// <param name="thickMessage">The thick message.</param>
        public static void wait(Int32 seconds = 5, String thickMessage = "")
        {
            log("Waiting [" + seconds + "] seconds before continue: ");
            while (seconds > 0)
            {
                seconds--;
                Thread.Sleep(1000);
                if (!thickMessage.isNullOrEmpty())
                {
                    loger.Append(thickMessage);
                }
                else
                {
                    loger.Append(seconds + "s ... ");
                }
            }
        }

        private builderForLog _externalLogBuilder;

        /// <summary>
        /// Extern log builder instance that will override its own builder
        /// </summary>
        public builderForLog externalLogBuilder
        {
            get { return _externalLogBuilder; }
            set { _externalLogBuilder = value; }
        }

        public builderForLog GetMainLogBuilder()
        {
            return logBuilder;
        }

        private builderForLog _logBuilder = new builderForLog();

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        protected builderForLog logBuilder
        {
            get
            {
                if (_externalLogBuilder != null)
                {
                    return _externalLogBuilder;
                }

                if (_logBuilder == null)
                {
                    _logBuilder = new builderForLog();
                }
                return _logBuilder;
            }
        }

        #region --- logBuilder ------- static and autoinitiated object

        public cursor c
        {
            get
            {
                return ((ITextRender)logBuilder).c;
            }
        }

        public IList content
        {
            get
            {
                return ((ITextRender)logBuilder).content;
            }
        }

        public DirectoryInfo directoryScope
        {
            get
            {
                return ((ITextRender)logBuilder).directoryScope;
            }

            set
            {
                ((ITextRender)logBuilder).directoryScope = value;
            }
        }

        public string linePrefix
        {
            get
            {
                return ((ITextRender)logBuilder).linePrefix;
            }

            set
            {
                ((ITextRender)logBuilder).linePrefix = value;
            }
        }

        public builderSettings settings
        {
            get
            {
                return ((ITextRender)logBuilder).settings;
            }
        }

        public string tabInsert
        {
            get
            {
                return ((ITextRender)logBuilder).tabInsert;
            }
        }

        public int tabLevel
        {
            get
            {
                return ((ITextRender)logBuilder).tabLevel;
            }

            set
            {
                ((ITextRender)logBuilder).tabLevel = value;
            }
        }

        public cursorZone zone
        {
            get
            {
                return ((ITextRender)logBuilder).zone;
            }
        }

        public string logContent
        {
            get
            {
                return ((ILogBuilder)logBuilder).logContent;
            }
        }

        public converterBase converter
        {
            get
            {
                return ((ILogBuilder)logBuilder).converter;
            }
        }

        public long lastLength
        {
            get
            {
                return ((ILogBuilder)logBuilder).lastLength;
            }
        }

        public long Length
        {
            get
            {
                return ((ILogBuilder)logBuilder).Length;
            }
        }

        public bool VAR_AllowAutoOutputToConsole => ((ILogBuilder)logBuilder).VAR_AllowAutoOutputToConsole;

        public bool VAR_AllowInstanceToOutputToConsole { get => ((ILogBuilder)logBuilder).VAR_AllowInstanceToOutputToConsole; set => ((ILogBuilder)logBuilder).VAR_AllowInstanceToOutputToConsole = value; }

        public object addDocument(string name, bool scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameExistingOnOtherDate, reportOutputFormatName format = reportOutputFormatName.none)
        {
            return ((ITextRender)logBuilder).addDocument(name, scopeToNew, mode, format);
        }

        public object addPage(string name, bool scopeToNew = true, getWritableFileMode mode = getWritableFileMode.autoRenameThis, reportOutputFormatName format = reportOutputFormatName.none)
        {
            return ((ITextRender)logBuilder).addPage(name, scopeToNew, mode, format);
        }

        public void Append(string content, appendType type = appendType.none, bool breakLine = false)
        {
            ((ITextRender)logBuilder).Append(content, type, breakLine);
        }

        public object AppendCite(string content)
        {
            return ((ITextRender)logBuilder).AppendCite(content);
        }

        public object AppendCode(string content)
        {
            return ((ITextRender)logBuilder).AppendCode(content);
        }

        public object AppendCode(string content, string codetypename)
        {
            return ((ITextRender)logBuilder).AppendCode(content, codetypename);
        }

        public object AppendComment(string content)
        {
            return ((ITextRender)logBuilder).AppendComment(content);
        }

        public void AppendDirect(string content)
        {
            ((ITextRender)logBuilder).AppendDirect(content);
        }

        public void AppendFile(string sourcepath, string outputpath, bool isDataTemplate = false)
        {
            ((ITextRender)logBuilder).AppendFile(sourcepath, outputpath, isDataTemplate);
        }

        public object AppendFrame(string content, int width, int height, string title = "", string footnote = "", IEnumerable<string> paragraphs = null)
        {
            return ((ITextRender)logBuilder).AppendFrame(content, width, height, title, footnote, paragraphs);
        }

        public void AppendFromFile(string sourcepath, templateFieldSubcontent datakey = templateFieldSubcontent.none, bool isLocalSource = false)
        {
            ((ITextRender)logBuilder).AppendFromFile(sourcepath, datakey, isLocalSource);
        }

        public object AppendFunction(string functionCode, bool breakLine = false)
        {
            return ((ITextRender)logBuilder).AppendFunction(functionCode, breakLine);
        }

        public object AppendHeading(string content, int level = 1)
        {
            return ((ITextRender)logBuilder).AppendHeading(content, level);
        }

        public void AppendHorizontalLine()
        {
            ((ITextRender)logBuilder).AppendHorizontalLine();
        }

        public void AppendImage(string imageSrc, string imageAltText, string imageRef)
        {
            ((ITextRender)logBuilder).AppendImage(imageSrc, imageAltText, imageRef);
        }

        public void AppendLabel(string content, bool isBreakLine = true, object comp_style = null)
        {
            ((ITextRender)logBuilder).AppendLabel(content, isBreakLine, comp_style);
        }

        public void AppendLine(string content)
        {
            ((ITextRender)logBuilder).AppendLine(content);
        }

        public void AppendLink(string url, string name, string caption = "", appendLinkType linkType = appendLinkType.link)
        {
            ((ITextRender)logBuilder).AppendLink(url, name, caption, linkType);
        }

        public void AppendList(IEnumerable<object> content, bool isOrderedList = false)
        {
            ((ITextRender)logBuilder).AppendList(content, isOrderedList);
        }

        public void AppendMath(string mathFormula, string mathFormat = "asciimath")
        {
            ((ITextRender)logBuilder).AppendMath(mathFormula, mathFormat);
        }

        public void AppendPair(string key, object value, bool breakLine = true, string between = " = ")
        {
            ((ITextRender)logBuilder).AppendPair(key, value, breakLine, between);
        }

        public object AppendPairs(PropertyCollection data, bool isHorizontal = false, string between = "")
        {
            return ((ITextRender)logBuilder).AppendPairs(data, isHorizontal, between);
        }

        public void AppendPanel(string content, string comp_heading = "", string comp_description = "", object comp_style = null)
        {
            ((ITextRender)logBuilder).AppendPanel(content, comp_heading, comp_description, comp_style);
        }

        public object AppendParagraph(string content, bool fullWidth = false)
        {
            return ((ITextRender)logBuilder).AppendParagraph(content, fullWidth);
        }

        public void AppendPlaceholder(object fieldName, bool breakLine = false)
        {
            ((ITextRender)logBuilder).AppendPlaceholder(fieldName, breakLine);
        }

        public object AppendQuote(string content)
        {
            return ((ITextRender)logBuilder).AppendQuote(content);
        }

        public object AppendSection(string content, string title, string footnote = null, IEnumerable<string> paragraphs = null)
        {
            return ((ITextRender)logBuilder).AppendSection(content, title, footnote, paragraphs);
        }

        public void AppendTable(DataTable table, Boolean doThrowException = true)
        {
            ((ITextRender)logBuilder).AppendTable(table, doThrowException);
        }

        public void AppendToFile(string outputpath, string content)
        {
            ((ITextRender)logBuilder).AppendToFile(outputpath, content);
        }

        public void Clear()
        {
            ((ITextRender)logBuilder).Clear();
        }

        public tagBlock close(string tag = "none")
        {
            return ((ITextRender)logBuilder).close(tag);
        }

        public void closeAll()
        {
            ((ITextRender)logBuilder).closeAll();
        }

        public string ContentToString(bool doFlush = false, reportOutputFormatName format = reportOutputFormatName.none)
        {
            return ((ITextRender)logBuilder).ContentToString(doFlush, format);
        }

        public PropertyCollection getContentBlocks(bool includeMain, reportOutputFormatName format = reportOutputFormatName.none)
        {
            return ((ITextRender)logBuilder).getContentBlocks(includeMain, format);
        }

        public object getDocument()
        {
            return ((ITextRender)logBuilder).getDocument();
        }

        public FileInfo loadDocument(string filepath, string name = "", reportOutputFormatName format = reportOutputFormatName.none)
        {
            return ((ITextRender)logBuilder).loadDocument(filepath, name, format);
        }

        public object loadPage(string filepath, string name = "", reportOutputFormatName format = reportOutputFormatName.none)
        {
            return ((ITextRender)logBuilder).loadPage(filepath, name, format);
        }

        public void nextTabLevel()
        {
            ((ITextRender)logBuilder).nextTabLevel();
        }

        public tagBlock open(string tag, string title = "", string description = "")
        {
            return ((ITextRender)logBuilder).open(tag, title, description);
        }

        public void prevTabLevel()
        {
            ((ITextRender)logBuilder).prevTabLevel();
        }

        public void rootTabLevel()
        {
            ((ITextRender)logBuilder).rootTabLevel();
        }

        public void saveDocument(FileInfo fi)
        {
            ((ITextRender)logBuilder).saveDocument(fi);
        }

        public FileInfo saveDocument(string name, getWritableFileMode mode, reportOutputFormatName format = reportOutputFormatName.none)
        {
            return ((ITextRender)logBuilder).saveDocument(name, mode, format);
        }

        public FileInfo savePage(string name, reportOutputFormatName format = reportOutputFormatName.none)
        {
            return ((ITextRender)logBuilder).savePage(name, format);
        }

        public string SubcontentClose()
        {
            return ((ITextRender)logBuilder).SubcontentClose();
        }

        public void SubcontentStart(templateFieldSubcontent key, bool cleanPriorContent = false)
        {
            ((ITextRender)logBuilder).SubcontentStart(key, cleanPriorContent);
        }

        public ILogBuilder logStartPhase(string title, string message)
        {
            return ((ILogBuilder)logBuilder).logStartPhase(title, message);
        }

        public ILogBuilder logEndPhase()
        {
            return ((ILogBuilder)logBuilder).logEndPhase();
        }

        //public void AppendException(string title, Exception ex, int exceptionLevel = 0)
        //{
        //    ((ILogBuilder)logBuilder).AppendException(title, ex, exceptionLevel);
        //}

        public void save(String path)
        {
            ((ILogBuilder)logBuilder).save(path);
        }

        void IAceLogable.log(string message)
        {
            ((ILogBuilder)logBuilder).log(message);
        }

        void ILogable.log(string message)
        {
            ((ILogBuilder)logBuilder).log(message);
        }

        //public void AppendCheckList(checkList list, bool isVertical, checkListItemValue filter = checkListItemValue.none)
        //{
        //    ((ILogBuilder)logBuilder).AppendCheckList(list, isVertical, filter);
        //}

        public void consoleAltColorToggle(bool setExact = false, int altChange = -1)
        {
            ((ILogBuilder)logBuilder).consoleAltColorToggle(setExact, altChange);
        }

        public string GetContent(long fromLength = long.MinValue, long toLength = long.MinValue)
        {
            return ((ILogBuilder)logBuilder).GetContent(fromLength, toLength);
        }

        public string getLastLine(bool removeIt = false)
        {
            return ((ILogBuilder)logBuilder).getLastLine(removeIt);
        }

        PropertyCollection ITextRender.getContentBlocks(bool includeMain, reportOutputFormatName format)
        {
            return ((ILogBuilder)loger).getContentBlocks(includeMain, format);
        }

        public void AppendLine()
        {
            ((ILogBuilder)loger).AppendLine();
        }

        //public object AppendPairs(PropertyCollection data, bool isHorizontal = false, string between = "")
        //{
        //    return ((ILogBuilder)loger).AppendPairs(data, isHorizontal, between);
        //}

        //public void AppendTable(DataTable table, bool doThrowException = true)
        //{
        //    ((ILogBuilder)loger).AppendTable(table, doThrowException);
        //}

        #endregion --- logBuilder ------- static and autoinitiated object
    }
}