using System;
using System.Collections.Generic;
using System.Linq;

namespace imbACE.Network.web.cache
{
    using imbACE.Core.application;
    using imbACE.Core.core.diagnostic;
    using imbACE.Network.extensions;
    using imbACE.Network.web.result;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.files;
    using imbSCI.Core.files.folders;
    using imbSCI.Core.math;
    using imbSCI.Data;
    using imbSCI.Data.data;

    // using Newtonsoft.Json;
    using imbSCI.Data.enums;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Web Cache System
    /// </summary>
    public static class webCacheSystem
    {
        private static Int32 _ageCount = 720;

        /// <summary>
        /// Cached file age limit in hours
        /// </summary>
        public static Int32 ageCount
        {
            get
            {
                return _ageCount;
            }
            set
            {
                _ageCount = value;
            }
        }

        private static folderNode _webCacheFolder = null;

        public static folderNode webCacheFolder
        {
            get
            {
                if (_webCacheFolder == null)
                {
                    _webCacheFolder = new folderNode(aceApplicationInfo.FOLDERNAME_CACHE + Path.DirectorySeparatorChar + "web", "WebCache", "Web cache repo");
                }

                return _webCacheFolder;
            }
        }

        /// <summary>
        /// Saves the cache.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="content">The content.</param>
        /// <param name="httpResponse">The HTTP response.</param>
        public static void saveCache(String url, String content, webResponse httpResponse)
        {
            String cacheFilePath = "";
            String cacheHttpPath = "";

            String pth = getCacheFilenameFromUrl(url); //makeCachePath(itemToExecute);
            String pthr = getCacheFilenameFromUrl(url, true);

            String json_content = objectSerialization.ObjectToXML(httpResponse); //objectSerialization.SerializeJson(httpResponse);

            FileInfo fc = content.saveStringToFile(pth, getWritableFileMode.overwrite, Encoding.UTF8);

            FileInfo js = json_content.saveStringToFile(pthr, getWritableFileMode.overwrite, Encoding.UTF8);
        }

        /// <summary>
        /// Determines whether the specified URL has cache.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        ///   <c>true</c> if the specified URL has cache; otherwise, <c>false</c>.
        /// </returns>
        public static Boolean hasCache(String url)
        {
            String cacheFilePath = "";
            String cacheHttpPath = "";

            cacheFilePath = getCacheFilenameFromUrl(url);
            cacheHttpPath = getCacheFilenameFromUrl(url, true);
            if (File.Exists(cacheFilePath) && File.Exists(cacheHttpPath))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Ućitava sadržaj iz Cache-a u zavisnosti od podešavanja i dostupnosti zadatog URL-a
        /// </summary>
        /// <param name="url">URL koji treba da učita</param>
        /// <returns>Vraća Cache Odgovor</returns>
        public static cacheResponse loadCache(String url)
        {
            cacheResponse output = new cacheResponse();
            String cacheFilePath = "";
            String cacheHttpPath = "";

            cacheFilePath = getCacheFilenameFromUrl(url);
            cacheHttpPath = getCacheFilenameFromUrl(url, true);

            if (File.Exists(cacheFilePath))
            {
                FileInfo fi = new FileInfo(cacheFilePath);
                TimeSpan fileAge = fi.CreationTime.Subtract(DateTime.Now);
                if (fileAge.TotalHours < ageCount)
                {
                    output.content = openBase.openFileToString(fi.FullName, true, false);
                    if (output.content.isNullOrEmpty())
                    {
                        output.cacheFound = false;
                    }
                    else
                    {
                        output.cacheFound = true;
                    }
                }
                else
                {
                    output.cacheFound = false;
                }
            }

            if (output.cacheFound)
            {
                if (File.Exists(cacheHttpPath))
                {
                    String httpRes = openBase.openFileToString(cacheHttpPath, true, false);
                    if (httpRes.isNullOrEmpty())
                    {
                        output.cacheFound = false;
                    }
                    else
                    {
                        output.httpContent = objectSerialization.loadObjectFromXML<webResponse>(httpRes);  //objectSerialization.DeserializeJson<webResponse>(httpRes);

                        output.cacheFound = true;
                    }
                }
                else
                {
                    output.cacheFound = false;
                }
            }

            return output;
        }

        /// <summary>
        /// Vraca standarni htmlFileCache
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static String getCacheFilenameFromUrl(String url, Boolean forHttpResponse = false)
        {
            String prefix = "web";
            String sufix = timeStamp.getTimeStamp(imbTimeStampFormat.imbCacheStamp);

            String extension = "html";
            if (forHttpResponse)
            {
                prefix = prefix + "_httpRespornce_";
                extension = "xml";
            }
            url = url.getFilename();
            url = url.removeUrlShema().getCleanFilePath().getFilename();
            url = md5.GetMd5Hash(url);

            String filename = prefix.add(url, "_");

            filename = filename.add(sufix, "_");

            filename = filename.add(extension, ".");

            String filepath = webCacheFolder.pathFor(filename);
            return filepath;
        }
    }
}