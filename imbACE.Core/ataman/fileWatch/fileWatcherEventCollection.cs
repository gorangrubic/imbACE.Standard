// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileWatcherEventCollection.cs" company="imbVeles" >
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
using imbSCI.Reporting;
using imbSCI.Reporting.enums;
using imbSCI.Reporting.interfaces;

namespace imbACE.Core.ataman.fileWatch
{
    using imbSCI.Core.extensions.data;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// List of cached events
    /// </summary>
    public class fileWatcherEventCollection : List<fileWatcherEventArgs>
    {
        public fileWatcherEventCollection()
        {
        }

        public fileWatcherEventCollection(fileWatcherEventArgs first)
        {
            Add(first);
        }

        public Boolean AddEvent(fileWatcherEventArgs newEvent, Boolean refuseRepeatedUID = true)
        {
            if (refuseRepeatedUID && (newEvent.uid == lastUID))
            {
                return false;
            }
            _lastUID = newEvent.uid;
            Add(newEvent);
            return true;
        }

        /// <summary>
        /// Collects all messages from collection
        /// </summary>
        /// <returns></returns>
        public List<string> getMessages()
        {
            List<string> output = new List<string>();

            if (Count > 1) output.Add("fileWatcherEventCollection(" + Count.ToString() + ")");

            foreach (fileWatcherEventArgs tmp in this)
            {
                output.Add(tmp.message);
            }
            return output;
        }

        /// <summary>
        /// last UID added
        /// </summary>
        private String _lastUID = "";

        /// <summary>
        /// last UID added
        /// </summary>
        public String lastUID
        {
            get
            {
                return _lastUID;
            }
        }

        /// <summary>
        /// The First or only event in collection
        /// </summary>
        private fileWatcherEventArgs _TheEvent = null;

        /// <summary>
        /// The First or only event in collection
        /// </summary>
        public fileWatcherEventArgs TheEvent
        {
            get { return this.imbFirstSafe(); }
        }
    }
}