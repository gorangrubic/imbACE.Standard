// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cacheSimpleBase.cs" company="imbVeles" >
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
namespace imbACE.Core.cache
{
    using imbACE.Core.core;
    using imbSCI.Core.collection;
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.files.folders;
    using imbSCI.Core.reporting;
    using imbSCI.Data;
    using imbSCI.Data.data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;

    public abstract class cacheSimpleBase : imbBindable, IAppendDataFields
    {
        private static builderForLog _log = new builderForLog();

        /// <summary>
        ///
        /// </summary>
        public static builderForLog log
        {
            get
            {
                return _log;
            }
            set
            {
                _log = value;
            }
        }

        public abstract String getInstanceID(Object instance);

        private settingsEntriesForObject _typeInfo;

        /// <summary>
        ///
        /// </summary>
        protected settingsEntriesForObject typeInfo
        {
            get { return _typeInfo; }
            set { _typeInfo = value; }
        }

        private Boolean _cacheSaveDisabled;

        /// <summary>
        ///
        /// </summary>
        public Boolean cacheSaveDisabled
        {
            get { return _cacheSaveDisabled; }
            set { _cacheSaveDisabled = value; }
        }

        private Boolean _cacheLoadDisabled;

        /// <summary>
        ///
        /// </summary>
        public Boolean cacheLoadDisabled
        {
            get { return _cacheLoadDisabled; }
            set { _cacheLoadDisabled = value; }
        }

        /// <summary>
        /// Removes all cache files within directory, returns number of files removed
        /// </summary>
        /// <returns></returns>
        public void removeCacheAll()
        {
            customSubdirectory.deleteFiles(); //cacheSystem.removeCacheAll(directory, customSubdirectory);
        }

        /// <summary>
        /// Removes the cache for instance - if exists. TRUE if cache file existed
        /// </summary>
        /// <param name="instanceID">The instance identifier.</param>
        /// <returns></returns>
        public Boolean removeCache(String instanceID)
        {
            instanceID = instanceID.ensureEndsWith(typeInfo.GetSignature());

            return cacheSystem.removeCache(instanceID, directory, customSubdirectory);
        }

        private Int32 _hourslimit;

        /// <summary>
        ///
        /// </summary>
        protected Int32 hourslimit
        {
            get { return _hourslimit; }
            set { _hourslimit = value; }
        }

        protected cacheSimpleBase(folderNode __directory, Int32 __hoursLimit, String __subFolder)
        {
            directory = __directory;
            hourslimit = __hoursLimit;
            customSubdirectory = __subFolder;
        }

        private folderNode _directory;

        /// <summary>
        ///
        /// </summary>
        protected folderNode directory
        {
            get { return _directory; }
            set { _directory = value; }
        }

        private String _customSubdirectory;

        /// <summary>
        ///
        /// </summary>
        protected String customSubdirectory
        {
            get { return _customSubdirectory; }
            set { _customSubdirectory = value; }
        }

        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public virtual PropertyCollectionExtended AppendDataFields(PropertyCollectionExtended data = null)
        {
            if (data == null) data = new PropertyCollectionExtended();

            String path = cacheSystem.getCacheFilepath("", directory, customSubdirectory);

            List<String> files = Directory.GetFiles(path).ToList();

            data.Add("cmng_directory", directory, "Directory", "Reporistory of cache system associated with the cache manager");
            data.Add("cmng_subdirectory", customSubdirectory, "Subdirectory", "Subdirectory within cache system repository");
            data.Add("cmng_path", path, "Path", "Cache repository file system path");
            data.Add("cmng_files", files.Count, "Files", "Number of cache files detected in the cache directory");
            data.Add("cmng_cachesize", path.getDirectorySize().getMByteCountFormated(), "Size", "Memory allocated for all cache files within the path, in megabytes", "bytes");
            data.Add("cmng_cacheLoadDisabled", cacheLoadDisabled, "Load disabled", "If cache load disabled the cache manager will ignore cache files on cacheLoad() calls.");
            data.Add("cmng_cacheSaveDisabled", cacheSaveDisabled, "Save disabled", "If cache save disabled the cache manager will ignore cacheSave() calls.");
            data.Add("cmng_subdirectory", customSubdirectory, "Subdirectory", "Subdirectory within cache system repository");
            data.Add("cmng_hours", hourslimit, "Age limit", "Age limit for cache file to be accepted, in hours", "h");

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
    }
}