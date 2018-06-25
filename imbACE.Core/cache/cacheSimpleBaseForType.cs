// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cacheManagerBase.cs" company="imbVeles" >
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
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.files.folders;
    using imbSCI.Data;
    using System;

    public abstract class cacheSimpleBase<T> : cacheSimpleBase where T : new()
    {
        public cacheSimpleBase(folderNode __directory, int __hoursLimit, string __subFolder) : base(__directory, __hoursLimit, __subFolder)
        {
            typeInfo = new settingsEntriesForObject(typeof(T));
        }

        /// <summary>
        /// Loads the cached instance or creates new object
        /// </summary>
        /// <param name="instanceID">The instance identifier.</param>
        /// <returns></returns>
        public T loadObjectCacheOrNew(String instanceID)
        {
            cacheResponseForType<T> output = loadObjectCacheAndMeta(instanceID, true);

            output.getCacheFromHarddisk<T>(hourslimit, customSubdirectory);

            return (T)output.instance;
        }

        /// <summary>
        /// Loads the object cached values to target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="instanceID">The instance identifier.</param>
        /// <returns></returns>
        public cacheResponseForType<T> loadObjectCachedValuesToTarget(T target, String instanceID = "")
        {
            if (instanceID.isNullOrEmpty())
            {
                instanceID = getInstanceID(target);
            }

            cacheResponseForType<T> output = loadObjectCacheAndMeta(instanceID, false);
            if (output.cacheInstanceLoaded)
            {
                target.setObjectBySource(output.instance);
            }
            return output;
        }

        /// <summary>
        /// Loads the object cache and meta returns all meta information
        /// </summary>
        /// <param name="instanceID">The instance identifier.</param>
        /// <param name="createNewIfNotFound">if set to <c>true</c> [create new if not found].</param>
        /// <returns></returns>
        public cacheResponseForType<T> loadObjectCacheAndMeta(String instanceID, Boolean createNewIfNotFound = false)
        {
            instanceID = instanceID.ensureEndsWith(typeInfo.GetSignature());

            cacheResponseForType<T> output = new cacheResponseForType<T>(instanceID, directory, createNewIfNotFound);
            if (cacheLoadDisabled)
            {
                output.getCacheFromHarddisk<T>(0, "_loadDisabled for [" + typeof(T).Name + "]");
            }
            else
            {
                output.getCacheFromHarddisk<T>(hourslimit, customSubdirectory);
            }

            return output;
        }

        /// <summary>
        /// Saves the object cache.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">saveObjectCache(T instance) failed because automatic instanceID extraction failed - instanceID</exception>
        public String saveObjectCache(T instance)
        {
            String instanceID = getInstanceID(instance);
            return saveObjectCache(instanceID, instance);
        }

        /// <summary>
        /// Saves the object cache and returns cache file full path
        /// </summary>
        /// <param name="instanceID">The instance identifier.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public String saveObjectCache(String instanceID, T instance)
        {
            String output = "";
            if (instanceID.isNullOrEmpty())
            {
                instanceID = getInstanceID(instance);
                if (instanceID.isNullOrEmpty())
                {
                    throw new ArgumentException("saveObjectCache(T instance) failed because automatic instanceID extraction failed", nameof(instanceID));
                }
            }

            if (cacheSaveDisabled)
            {
                return "[cacheSaveDisabled]";
            }
            instanceID = instanceID.ensureEndsWith(typeInfo.GetSignature());
            try
            {
                output = cacheSystem.saveObjectCache(instanceID, instance, directory, customSubdirectory);
            }
            catch (Exception ex)
            {
                log.log("saveObjectCache exception [" + instanceID + "] : ");
                log.Append("saveObjectCache exception [" + instanceID + "] : " + ex.Message);
                throw;
            }

            return output;
        }
    }
}