// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cacheResponseForType_2.cs" company="imbVeles" >
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
    using imbACE.Core.core.exceptions;
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.files;
    using imbSCI.Core.files.folders;
    using imbSCI.Core.reporting;
    using imbSCI.Data.data;
    using System;
    using System.Data;
    using System.IO;

    /// <summary>
    /// General cache response container
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="imbSCI.Core.core.imbBindable" />
    public class cacheResponseForType : imbBindable, IAppendDataFields
    {
        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public PropertyCollectionExtended AppendDataFields(PropertyCollectionExtended data = null)
        {
            if (data == null) data = new PropertyCollectionExtended();

            data.Add("cachename", instanceID, "Instance ID", "ID for " + instanceType.Name + " instance");
            data.Add("cachetype", instanceTypeName, "Type", "Class name of cached instance");
            data.Add("cacheinstanceready", haveInstance, "Instance", "True if instance is ready (loaded or created)");

            data.Add("cachedirectory", directory, "Directory", "Reporistory of cache system associated to the cache");
            data.Add("cachefilepath", filepath, "File path", "Full file path for cache content save and/or load operation.");
            data.Add("cacheexists", cacheFileExists, "File exists", "If the cache file was detected");
            data.Add("cacheage", cacheAge, "File age", "Cache file age in hours");
            data.Add("cacheisloaded", cacheInstanceLoaded, "Cache loaded", "If the cache was loaded from file");

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
        /// Initializes a new instance of the
        /// </summary>
        /// <param name="__instanceID">The instance identifier.</param>
        /// <param name="__directory">The directory.</param>
        public cacheResponseForType(String __instanceID, folderNode __directory, Boolean __createNewOnNotFound = false, Type __instanceType = null)
        {
            instanceID = __instanceID;
            directory = __directory;
            createNewOnNotFound = __createNewOnNotFound;
            instanceType = __instanceType;
        }

        private Boolean _createNewOnNotFound;

        /// <summary>
        ///
        /// </summary>
        protected Boolean createNewOnNotFound
        {
            get { return _createNewOnNotFound; }
            set { _createNewOnNotFound = value; }
        }

        /// <summary>
        ///
        /// </summary>
        public String instanceTypeName
        {
            get
            {
                if (instanceType != null) return instanceType.Name;
                return "unknown";
            }
        }

        private Type _instanceType;

        /// <summary>
        ///
        /// </summary>
        public Type instanceType
        {
            get { return _instanceType; }
            protected set { _instanceType = value; }
        }

        /// <summary>
        /// Gets the cache from harddisk if the file is younger than <c>limithours</c>, otherwise deletes all
        /// </summary>
        /// <param name="limithours">The limithours.</param>
        public void getCacheFromHarddisk<T>(Int32 limithours = 24, String customSubFolder = "") where T : new()
        {
            try
            {
                filepath = cacheSystem.getCacheFilepath(instanceID, directory, customSubFolder);

                instanceType = typeof(T);

                if (File.Exists(filepath))
                {
                    cacheAge = DateTime.Now.Subtract(File.GetCreationTime(filepath));

                    if (cacheAge.TotalHours > limithours)
                    {
                        try
                        {
                            File.Delete(filepath);
                        }
                        catch (Exception ex)
                        {
                            aceLog.loger.Append("getCacheFromHarddisk[" + instanceID + "] " + filepath + " ex:" + ex.Message);
                        }
                    }
                    else
                    {
                        _cacheFileExists = true;
                    }
                }

                if (cacheFileExists)
                {
                    String content = openBase.openFileToString(filepath, true, false);
                    instance = (T)objectSerialization.ObjectFromXML<T>(content);
                    cacheInstanceLoaded = true;
                }

                if (instance == null)
                {
                    if (createNewOnNotFound)
                    {
                        instance = new T();
                    }
                }
            }
            catch (Exception ex)
            {
                aceLog.log("Cache [" + instanceID + "] broken: " + ex.Message);
                File.Delete(filepath);
                instance = new T();
            }
        }

        private TimeSpan _cacheAge = new TimeSpan();

        /// <summary>
        /// How old is the cache file
        /// </summary>
        public TimeSpan cacheAge
        {
            get { return _cacheAge; }
            protected set { _cacheAge = value; }
        }

        private folderNode _directory;

        /// <summary>
        ///
        /// </summary>
        public folderNode directory
        {
            get { return _directory; }
            protected set { _directory = value; }
        }

        private Object _cacheInstance = null;

        /// <summary>
        /// instance to be retrieved
        /// </summary>
        public Object instance
        {
            get { return _cacheInstance; }
            protected set { _cacheInstance = value; }
        }

        private String _filepath = "";

        /// <summary>
        ///
        /// </summary>
        public String filepath
        {
            get { return _filepath; }
            protected set { _filepath = value; }
        }

        private Boolean _cacheInstanceLoaded;

        /// <summary>
        ///
        /// </summary>
        public Boolean cacheInstanceLoaded
        {
            get { return _cacheInstanceLoaded; }
            protected set { _cacheInstanceLoaded = value; }
        }

        /// <summary>
        /// if the cache was loaded
        /// </summary>
        public Boolean haveInstance
        {
            get
            {
                if (instance == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private Boolean _cacheFileExists;

        /// <summary>
        /// if the cache file exists
        /// </summary>
        public Boolean cacheFileExists
        {
            get { return _cacheFileExists; }
        }

        private String _instanceID = "";

        /// <summary>
        ///
        /// </summary>
        public String instanceID
        {
            get { return _instanceID; }
            protected set { _instanceID = value; }
        }
    }
}