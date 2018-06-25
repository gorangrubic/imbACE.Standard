// --------------------------------------------------------------------------------------------------------------------
// <copyright file="systemStatusData.cs" company="imbVeles" >
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

    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;

    #endregion imbVeles using

    public struct systemStatusData
    {
        private String _memoryUsage;
        private String _threads;
        private String _timeRunning;
        public ObservableCollection<Process> ps;
        public DateTime startedDateTime;
        public TimeSpan timeRunningTimeSpan;

        public String timeRunning
        {
            get { return _timeRunning; }
            set { _timeRunning = value; }
        }

        public String memoryUsage
        {
            get { return _memoryUsage; }
            set { _memoryUsage = value; }
        }

        public String threads
        {
            get { return _threads; }
            set { _threads = value; }
        }
    }
}