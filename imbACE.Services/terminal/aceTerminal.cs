// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceTerminal.cs" company="imbVeles" >
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
// Project: imbACE.Services
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbACE.Services.terminal
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.extensions;
    using imbACE.Core.interfaces;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.textBlocks.core;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;

    /// <summary>
    /// Log output and Console application textual UI
    /// </summary>
    public class aceTerminal : textContentLines, IAceLogable, INotifyPropertyChanged
    {
        public aceTerminal(IPlatform __platform)
        {
            platform = __platform;
        }

        public String getLastLogLine(Int32 width = 60)
        {
            return this[0].toWidthMaximum(width);
        }

        #region --- platform ------- referenca prema platformi

        private IPlatform _platform;

        /// <summary>
        /// referenca prema platformi
        /// </summary>
        public IPlatform platform
        {
            get
            {
                return _platform;
            }
            set
            {
                _platform = value;
                OnPropertyChanged("platform");
            }
        }

        #endregion --- platform ------- referenca prema platformi

        /// <summary>
        /// Osnovna podesavanja
        /// </summary>
        /// <param name="_toConsole"></param>
        /// <param name="_autosave"></param>
        /// <param name="__filename"></param>
        public void logSetup(Boolean _toConsole, Boolean _autosave, String __filename = null)
        {
            doAutoSaveLog = _autosave;
            doToConsole = _toConsole;

            if (__filename != null) path_logoutput = __filename;
        }

        #region --- path_logoutput ------- putanja prema izlazu log-a

        private String _path_logoutput;

        /// <summary>
        /// putanja prema izlazu log-a. Ukoliko nije definisano automatski postavlja vrednost
        /// </summary>
        public String path_logoutput
        {
            get
            {
                if (String.IsNullOrEmpty(_path_logoutput))
                {
                    _path_logoutput = "_terminal_logout.txt";
                }
                return _path_logoutput;
            }
            set
            {
                _path_logoutput = value;
                OnPropertyChanged("path_logoutput");
            }
        }

        #endregion --- path_logoutput ------- putanja prema izlazu log-a

        public aceTerminalEvent onNewLog;

        #region --- doAutoSaveLog ------- da li da automatski snima log

        private Boolean _doAutoSaveLog = true;

        /// <summary>
        /// da li da automatski snima log
        /// </summary>
        public Boolean doAutoSaveLog
        {
            get
            {
                return _doAutoSaveLog;
            }
            set
            {
                _doAutoSaveLog = value;
                OnPropertyChanged("doAutoSaveLog");
            }
        }

        #endregion --- doAutoSaveLog ------- da li da automatski snima log

        #region ----------- Boolean [ doToConsole ] -------  [Da li sve da salje na konzolu]

        private Boolean _doToConsole = true;

        /// <summary>
        /// Da li sve da salje na konzolu --- ne vazi ako je postavljen onNewLog event
        /// </summary>
        [Category("Switches")]
        [DisplayName("doToConsole")]
        [Description("Da li sve da salje na konzolu")]
        public Boolean doToConsole
        {
            get { return _doToConsole; }
            set
            {
                _doToConsole = value;
                OnPropertyChanged("doToConsole");
            }
        }

        #endregion ----------- Boolean [ doToConsole ] -------  [Da li sve da salje na konzolu]

        /// <summary>
        /// Belezi log liniju
        /// </summary>
        /// <param name="_message"></param>
        public void log(String _message)
        {
            var lco = insert(_message, platform.width, 0);

            if (doToConsole)
            {
                Console.WriteLine(_message);
                //platform.render(lco);
            }

            //  String message = _message;
            //insert()

            //  _logContent = _logContent + Environment.NewLine + _message;

            if (onNewLog != null)
            {
                onNewLog(null, _message);
            }
            if (doAutoSaveLog)
            {
                saveBase.saveAtEndOfFile(path_logoutput, _message.toListWithThisOne());

                //File.AppendText(path_logoutput, _message);
                //File.WriteAllText(path_logoutput, lo);
            }
        }

        #region --- logContent ------- Sadrzaj log ova

        private String _logContent = "";

        /// <summary>
        /// Sadrzaj log ova
        /// </summary>
        public String logContent
        {
            get
            {
                _logContent = toString(false);
                return _logContent;
            }
        }

        #endregion --- logContent ------- Sadrzaj log ova

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}