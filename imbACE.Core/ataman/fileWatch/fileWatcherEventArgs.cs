// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileWatcherEventArgs.cs" company="imbVeles" >
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

//using imbSCI.DataComplex;
//using imbSCI.Reporting;
//using imbSCI.Reporting.enums;
//using imbSCI.Reporting.interfaces;

namespace imbACE.Core.ataman.fileWatch
{
    using imbACE.Core.core.diagnostic;
    using System;
    using System.IO;

    /// <summary>
    /// Objekat koji opisuje dogadjaj koji se desio objektu: fileWatcher
    /// </summary>
    public class fileWatcherEventArgs : EventArgs
    {
        //public fileWatcherEventArgs()
        //{
        //}

        public fileWatcherEventArgs(FileSystemEventArgs e, fileWatcher sender, Int32 __id)
        {
            _watcher = sender;
            _id = __id;
            deployArgs(e);
        }

        private void deployArgs(FileSystemEventArgs e)
        {
            _time = DateTime.Now;

            _filesystemArgs = e;

            _isFile = Path.HasExtension(e.FullPath);
            if (_isFile)
            {
                _filename = Path.GetFileName(e.FullPath);
            }
            else
            {
                _filename = Path.GetDirectoryName(e.FullPath);
            }
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Deleted:
                    if (isFile)
                    {
                        type = fileWatcherEventType.fileDeleted;
                    }
                    break;

                case WatcherChangeTypes.Created:
                    if (isFile)
                    {
                        type = fileWatcherEventType.fileCreated;
                    }
                    break;

                case WatcherChangeTypes.Changed:
                    if (isFile)
                    {
                        type = fileWatcherEventType.fileChanged;
                    }
                    break;

                case WatcherChangeTypes.Renamed:
                    if (isFile)
                    {
                        type = fileWatcherEventType.fileRenamed;
                    }
                    break;
            }

            _uid =
                timeStamp.getTimeStamp(imbTimeStampFormat.imbSinceApplicationStartRough).add(
                    filename.imbGetAbbrevation(5, true));

            _message = "";

            _message = _message.add("[" + type.ToString() + "]:[" + id.ToString("D3") + "]");

            if (isFile)
            {
                _message = _message.add("File");
            }
            else
            {
                _message = _message.add("Directory");
            }
            _message = _message.add(String.Format("[{0}] {1} [{2}].", filename, e.ChangeType, uid));

            _message = _message.add(time.ToShortTimeString());
        }

        /// <summary>
        /// Obelezava event ta je obradjen i oslobadja sve resurse
        /// </summary>
        public void processAndDispose()
        {
            _isProcessed = true;
        }

        #region --- time ------- Vreme kada se desio dogadjaj

        private DateTime _time;

        /// <summary>
        /// Vreme kada se desio dogadjaj
        /// </summary>
        public DateTime time
        {
            get
            {
                return _time;
            }
        }

        #endregion --- time ------- Vreme kada se desio dogadjaj

        /// <summary>
        /// Unique ID string
        /// </summary>
        private String _uid = "";

        /// <summary>
        /// Unique ID string
        /// </summary>
        public String uid
        {
            get
            {
                return _uid;
            }
        }

        #region --- message ------- verbal message about the event

        private String _message;

        /// <summary>
        /// verbal message about the event
        /// </summary>
        public String message
        {
            get
            {
                return _message;
            }
        }

        #endregion --- message ------- verbal message about the event

        /// <summary>
        /// filename sa ekstenzijom bez putanje
        /// </summary>
        private String _filename = "";

        /// <summary>
        /// filename sa ekstenzijom bez putanje
        /// </summary>
        public String filename
        {
            get
            {
                return _filename;
            }
        }

        #region --- id ------- Ordinal event id - for file watcher

        private Int32 _id = 0;

        /// <summary>
        /// Ordinal event id - for file watcher
        /// </summary>
        public Int32 id
        {
            get
            {
                return _id;
            }
        }

        #endregion --- id ------- Ordinal event id - for file watcher

        #region --- watcher ------- referenca prema watcher objektu koji je primetio promenu

        private fileWatcher _watcher;

        /// <summary>
        /// referenca prema watcher objektu koji je primetio promenu
        /// </summary>
        public fileWatcher watcher
        {
            get
            {
                return _watcher;
            }
        }

        #endregion --- watcher ------- referenca prema watcher objektu koji je primetio promenu

        #region --- filesystemArgs ------- izvorni argumenti filesystemwatchera

        private FileSystemEventArgs _filesystemArgs;

        /// <summary>
        /// izvorni argumenti filesystemwatchera
        /// </summary>
        public FileSystemEventArgs filesystemArgs
        {
            get
            {
                return _filesystemArgs;
            }
        }

        #endregion --- filesystemArgs ------- izvorni argumenti filesystemwatchera

        #region ----------- Boolean [ isFile ] -------  [da li putanja ukazuje na fajl]

        private Boolean _isFile = false;

        /// <summary>
        /// da li putanja ukazuje na fajl
        /// </summary>
        public Boolean isFile
        {
            get { return _isFile; }
        }

        #endregion ----------- Boolean [ isFile ] -------  [da li putanja ukazuje na fajl]

        #region --- isProcessed ------- da li je vec obradjen ovaj event -

        private Boolean _isProcessed = false;

        /// <summary>
        /// da li je vec obradjen ovaj event -
        /// </summary>
        public Boolean isProcessed
        {
            get
            {
                return _isProcessed;
            }
        }

        #endregion --- isProcessed ------- da li je vec obradjen ovaj event -

        /// <summary>
        /// Vrsta promene koja je nastala
        /// </summary>
        public WatcherChangeTypes changeType
        {
            get { return _filesystemArgs.ChangeType; }
        }

        /// <summary>
        /// Putanja ka fajlu/direktorijumu koji je predmet dogadjaja
        /// </summary>
        public String path
        {
            get { return _filesystemArgs.FullPath; }
        }

        #region --- type ------- event type

        private fileWatcherEventType _type;

        /// <summary>
        /// event type -
        /// </summary>
        public fileWatcherEventType type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        #endregion --- type ------- event type
    }
}