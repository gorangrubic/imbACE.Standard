// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileWatcher.cs" company="imbVeles" >
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
using imbACE.Core;
using imbSCI.Core;
using imbSCI.Core.attributes;
using imbSCI.Core.enums;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.interfaces;
using imbSCI.Data;
using imbSCI.Data.collection;
using imbSCI.Data.data;
using imbSCI.Data.interfaces;
using imbSCI.DataComplex;

namespace imbACE.Core.ataman.fileWatch
{
    using imbACE.Core.core.diagnostic;
    using imbSCI.Core.extensions.io;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading;

    /// <summary>
    /// File watcher is a continuous type of ataman watcher, it collects file system events (according to specified rules) and reports them trough <see cref="eventFlush"/> call
    /// </summary>
    public class fileWatcher //:INotifyPropertyChanged
    {
        /// <summary>
        /// Creates a file watcher instance
        /// </summary>
        /// <param name="path"></param>
        /// <param name="searchFilter"></param>
        /// <returns></returns>
        public fileWatcher(fileWatcherType __type, String __path, String __searchFilter)
        {
            _watchType = __type;

            switch (_watchType)
            {
                case fileWatcherType.filesNewAndChangeInDirectory:
                    _path = __path.getResolvedPath(false);
                    _searchFilter = __searchFilter.getFileSearchFilter();
                    if (!Directory.Exists(path))
                    {
                        _errorMessage = "Path (" + path + ") directory creation/selection failed";
                        _watchType = fileWatcherType.initFailed;
                    }
                    else
                    {
                        _fileSystemWatcher = new FileSystemWatcher(path, searchFilter);
                        _fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.FileName;
                        _fileSystemWatcher.Changed += _fileSystemWatcher_newEvent;
                        //_fileSystemWatcher.Created += _fileSystemWatcher_newEvent;
                        //_fileSystemWatcher.Renamed += _fileSystemWatcher_newEvent;
                        //_fileSystemWatcher.Deleted += _fileSystemWatcher_newEvent;
                        _fileSystemWatcher.EnableRaisingEvents = true;
                    }
                    break;

                case fileWatcherType.fileActivity:
                    _path = __path.getResolvedPath(true);
                    // doraditi
                    throw new NotImplementedException();
                    break;
            }
        }

        /// <summary>
        /// (re)starts fileWatcher operation reseting state according to __eventMode
        /// </summary>
        /// <param name="__eventMode">fileWatcher event flushing mode to operate on</param>
        /// <param name="__onFileWatcherEvent">Event handler that should be called for flushing</param>
        public void start(fileWatcherEventMode __eventMode, fileWatcherEvent __onFileWatcherEvent = null)
        {
            eventMode = __eventMode;

            _flush(null);

            isOperationDisabled = false;
            switch (eventMode)
            {
                case fileWatcherEventMode.directEventCall:
                    break;

                case fileWatcherEventMode.externalEventListFlush:
                    break;

                case fileWatcherEventMode.handlerFlush_internalTimer:

                    flushTimer.Change(eventFlushInterval, eventFlushInterval);

                    break;

                case fileWatcherEventMode.handlerFlush_externalTimer:
                    stopwatch = new imbStopwatch("Event accumulation stop watch", false);

                    break;
            }

            if (__onFileWatcherEvent != null)
            {
                onFileWatcherEvent = __onFileWatcherEvent;
            }
        }

        /// <summary>
        /// Stops fileWatcher operation - may be used for temporaral or definite purposes
        /// </summary>
        public void stop()
        {
            isOperationDisabled = true;
        }

        /// <summary>
        /// Continues fileWatcher operation from current state, using current settings
        /// </summary>
        public void run()
        {
            isOperationDisabled = false;
        }

        /// <summary>
        /// Okida eksterni tajmer
        /// </summary>
        /// <param name="state"></param>
        public void externalTimerTick(Object state = null)
        {
            if (eventMode != fileWatcherEventMode.handlerFlush_externalTimer)
            {
                throw new fileWatcherException("Event mode not supported for this operation", this);
            }

            timerCheck(state);
        }

        /// <summary>
        /// Izvrsava preuzimanje nakupljenih eventova - samo za mode  fileWatcherEventMode.externalEventListFlush
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public fileWatcherEventCollection externalEventsFlush(Object state = null)
        {
            if (eventMode != fileWatcherEventMode.externalEventListFlush)
            {
                throw new fileWatcherException("Event mode not supported for this operation", this);
            }

            return _flush(state);
        }

        #region STATE ----------------------

        private Int32 ordinalIndex = 0;

        private fileWatcherEventCollection _events = new fileWatcherEventCollection();

        /// <summary>
        /// lista dogadjaja
        /// </summary>
        public fileWatcherEventCollection events
        {
            get
            {
                return _events;
            }
        }

        private String _errorMessage = "";

        /// <summary>
        /// Kada je zaustavljen onda je ovde true
        /// </summary>
        private Boolean isOperationDisabled = false;

        #endregion STATE ----------------------

        #region CONFIG ----------------------

        /// <summary>
        /// fileWatcher mode of operation
        /// </summary>
        private fileWatcherEventMode eventMode = fileWatcherEventMode.disabled;

        private Int32 eventFlushInterval = 0;

        private fileWatcherType _watchType = fileWatcherType.filesNewAndChangeInDirectory;

        private String _path;

        /// <summary>
        /// putanja koju posmatra
        /// </summary>
        public String path
        {
            get
            {
                return _path;
            }
        }

        /// <summary>
        /// Search filter for Folder monitoring
        /// </summary>
        private String _searchFilter = "*.*";

        /// <summary>
        /// Search filter for Folder monitoring
        /// </summary>
        public String searchFilter
        {
            get
            {
                return _searchFilter;
            }
        }

        #endregion CONFIG ----------------------

        #region TOOLS / OBJECTS ----------------------

        private imbStopwatch stopwatch;

        protected fileWatcherEvent onFileWatcherEvent;

        public FileSystemWatcher _fileSystemWatcher;

        protected Timer flushTimer = null;

        #endregion TOOLS / OBJECTS ----------------------

        #region Internal methods ----------------------

        /// <summary>
        /// Processing .NET API fileSystemWatcher event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _fileSystemWatcher_newEvent(object sender, FileSystemEventArgs e)
        {
            if (aceSystemAtaman.doPauseAllFileWatchers) return;

            fileWatcherEventArgs args = new fileWatcherEventArgs(e, this, ordinalIndex);

            if (events.AddEvent(args, true))
            {
                ordinalIndex++;

                switch (eventMode)
                {
                    case fileWatcherEventMode.directEventCall:
                        onFileWatcherEvent(this, new fileWatcherEventCollection(args));
                        break;

                    case fileWatcherEventMode.externalEventListFlush:
                        //
                        break;

                    case fileWatcherEventMode.handlerFlush_internalTimer:

                        flushTimer.Change(eventFlushInterval, eventFlushInterval);

                        break;

                    case fileWatcherEventMode.handlerFlush_externalTimer:
                        // ceka eksterni tajmer
                        break;
                }
            }
        }

        /// <summary>
        /// Izvrsava kada je tajmer (interni ili externi) dostigao potrebnu vrednost
        /// </summary>
        /// <param name="state"></param>
        private void timerCheck(Object state)
        {
            if (isOperationDisabled) return;

            switch (eventMode)
            {
                case fileWatcherEventMode.directEventCall:
                case fileWatcherEventMode.externalEventListFlush:
                case fileWatcherEventMode.handlerFlush_internalTimer:
                    break;

                case fileWatcherEventMode.handlerFlush_externalTimer:
                    if (!stopwatch.IsRunning)
                    {
                        stopwatch.Restart();
                    }

                    if (stopwatch.ElapsedMilliseconds > eventFlushInterval)
                    {
                        timerTick(state);
                        stopwatch.Restart();
                    }
                    break;
            }
        }

        /// <summary>
        /// Izvrsava kada je tajmer (interni ili externi) dostigao potrebnu vrednost
        /// </summary>
        /// <param name="state"></param>
        private void timerTick(Object state)
        {
            //if (onFileWatcherEvent != null)
            //{
            //    eventFlush();
            //}
        }

        private fileWatcherEventCollection _flush(Object state = null)
        {
            fileWatcherEventCollection _output = _events;

            _events = new fileWatcherEventCollection();

            return _output;
        }

        #endregion Internal methods ----------------------

        /// <summary>
        /// <para>Returns the event list or triggers the event handlers - but only when the flush time has passed, otherwise returns null and takes no action</para>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<fileWatcherEventArgs> eventFlush()
        {
            return null;
        }

        internal fileWatcher()
        {
            stopwatch = new imbStopwatch("Events flusher", false);
            eventFlushInterval = aceSystemAtaman.defaultEventsFlushInterval;
            flushTimer = new Timer(new TimerCallback(timerTick));

            //_fileSystemWatcher = new FileSystemWatcher(
        }
    }
}