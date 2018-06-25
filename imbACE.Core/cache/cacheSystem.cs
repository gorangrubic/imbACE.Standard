// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cacheSystem.cs" company="imbVeles" >
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
    using imbACE.Core.core.diagnostic;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.files;
    using imbSCI.Core.files.folders;
    using imbSCI.Core.math;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    ///
    /// </summary>
    public static class cacheSystem
    {
        public static String defaultCacheDirectoryPath = "datacache";

        public static Int32 refreshDays_limit = 5;
        private static object imbUrlOps;

        /// <summary>
        /// Caches the age limit.
        /// </summary>
        /// <returns></returns>
        public static Int32 cacheAgeLimit()
        {
            return (refreshDays_limit * 24);
        }

        /// <summary>
        /// Removes cache from specified directory
        /// </summary>
        /// <param name="directory">The directory.</param>
        /// <param name="customSubDirectory">The custom sub directory.</param>
        /// <returns></returns>
        public static Int32 removeCacheAll(folderNode directory, String customSubDirectory = "")
        {
            Int32 output = 0;
            String filepath = cacheSystem.getCacheFilepath("", directory, customSubDirectory);
            foreach (String fi in Directory.EnumerateFiles(filepath))
            {
                File.Delete(fi);
                output++;
            }

            //   platform.cache.cacheManagerBase.log.log("Cache cleared from [" + directory + "] sub[" + customSubDirectory + "] - files deleted [" + output + "]");

            return output;
        }

        /// <summary>
        /// Removes chache file for <c>instanceID</c>, returns <c>TRUE</c> if file existed, <c>FALSE</c> if file never existed
        /// </summary>
        /// <param name="instanceID">The instance identifier.</param>
        /// <param name="directory">The directory.</param>
        /// <returns></returns>
        public static Boolean removeCache(String instanceID, folderNode directory, String customSubDirectory = "")
        {
            String filepath = cacheSystem.getCacheFilepath(instanceID, directory, customSubDirectory);
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Saves the cache.
        /// </summary>
        /// <param name="instanceID">The instance identifier.</param>
        /// <param name="source">The source.</param>
        /// <param name="directory">The directory.</param>
        public static String saveObjectCache(String instanceID, Object source, folderNode directory, String customSubDirectory = "")
        {
            try
            {
                String pth = getCacheFilepath(instanceID, directory, customSubDirectory);

                String json_content = objectSerialization.ObjectToXML(source);

                FileInfo fc = json_content.saveStringToFile(pth, getWritableFileMode.overwrite, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                // aceLog.log(ex, "cacheSystem (" + instanceID + ") saveObjectCache()", logType.CriticalWarning);
            }

            return "";
        }

        /// <summary>
        /// Loads the cache file for the <c>instanceID</c> if file jounger than <c>limithours</c>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instanceID">The instance identifier.</param>
        /// <param name="limithours">The limithours.</param>
        /// <param name="directory">The directory.</param>
        /// <returns></returns>
        public static cacheResponseForType loadObjectCache<T>(String instanceID, Int32 limithours, folderNode directory) where T : class, new()
        {
            cacheResponseForType<T> output = new cacheResponseForType<T>(instanceID, directory);
            try
            {
                output.getCacheFromHarddisk<T>(limithours);
            }
            catch (Exception ex)
            {
                // aceLog.log(ex, "cacheSystem (" + instanceID + ") loadObjectCache()", logType.CriticalWarning);
                throw;
            }
            return output;
        }

        /// <summary>
        /// Gets the full filepath via MD5 algorithm based on instanceID
        /// </summary>
        /// <param name="instanceID">The instance identifier.</param>
        /// <param name="directory">The directory.</param>
        /// <returns></returns>
        public static String getCacheFilepath(String instanceID, folderNode directory, String customSubDirectory = "")
        {
            String filepath = "";
            if (!instanceID.isNullOrEmpty())
            {
                filepath = md5.GetMd5Hash(instanceID).add("json", ".");
            }

            String directory_path = directory.path;

            if (!customSubDirectory.isNullOrEmpty())
            {
                directory_path = directory_path.add(customSubDirectory, "\\");
            }

            DirectoryInfo di = Directory.CreateDirectory(directory_path);

            filepath = di.FullName.add(filepath, "\\");

            return filepath;
        }
    }
}