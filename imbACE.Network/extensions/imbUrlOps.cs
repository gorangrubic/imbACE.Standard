// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbUrlOps.cs" company="imbVeles" >
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
// Project: imbACE.Network
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------

namespace imbACE.Network.extensions
{
    #region imbVeles using

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
    using imbSCI.Data.enums;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex;
    using imbSCI.Reporting;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.interfaces;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    #endregion imbVeles using

    /// <summary>
    /// Skup alatki za rad sa URL adresama
    /// </summary>
    public static class imbUrlOps
    {
        public static Regex urlWordMatch = new Regex("([A-Za-z0-9\\.\\-]+)");

        public static Regex filenameMatch = new Regex("([A-Za-z0-9\\-]+)\\.([a-z]{2,4})");

        /// <summary>
        /// Extracts clean string non-unique tokens from specified url. From http://www.koplas.co.rs/index.php?page=home5&neki_tamo=203#top it will extract: index, page, home5, neki, tamo, 203, top
        /// </summary>
        /// <param name="urls">The urls.</param>
        /// <param name="removeFileExtension">if set to <c>true</c> [remove file extension].</param>
        /// <param name="removeShema">if set to <c>true</c> [remove shema].</param>
        /// <param name="removeDomain">if set to <c>true</c> [remove domain].</param>
        /// <returns></returns>
        public static List<string> getStringTokensFromUrls(this IEnumerable<string> urls, bool removeFileExtension = true, bool removeShema = true, bool removeDomain = true)
        {
            List<string> output = new List<string>();
            foreach (string mc in urls)
            {
                output.AddRange(mc.getStringTokensFromUrl(removeFileExtension, removeShema, removeDomain));
            }
            return output;
        }

        /// <summary>
        /// Extracts clean string tokens from specified url. From http://www.koplas.co.rs/index.php?page=home5&neki_tamo=203#top it will extract: index, page, home5, neki, tamo, 203, top
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="removeFileExtension">if set to <c>true</c> [remove file extension].</param>
        /// <param name="removeShema">if set to <c>true</c> [remove shema].</param>
        /// <param name="removeDomain">if set to <c>true</c> [remove domain].</param>
        /// <returns></returns>
        public static List<string> getStringTokensFromUrl(this string url, bool removeFileExtension = true, bool removeShema = true, bool removeDomain = true)
        {
            string source = url;
            List<string> output = new List<string>();
            if (imbSciStringExtensions.isNullOrEmptyString(url)) return output;
            if (removeShema) source = source.removeUrlShema();
            if (removeDomain)
            {
                string domain = source.getDomainNameFromUrl(false, false);
                source = imbSciStringExtensions.removeStartsWith(source, domain);
            }

            if (imbSciStringExtensions.isNullOrEmptyString(source)) return output;
            foreach (Match mc in urlWordMatch.Matches(source))
            {
                string vl = mc.Value;

                if (removeFileExtension)
                {
                    if (filenameMatch.IsMatch(vl))
                    {
                        Match fml = filenameMatch.Match(vl);
                        vl = fml.Groups[1].Value;
                    }
                }

                output.Add(vl);
            }
            return output;
        }

        public const string urlShemaSufix = @"://";

        /// <summary>
        /// 2014 maj> creates URI object from string path
        /// </summary>
        /// <param name="uriPath"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static Uri toUri(this string uriPath, UriKind kind = UriKind.RelativeOrAbsolute)
        {
            Uri output = null;
            Uri.TryCreate(uriPath, kind, out output);
            return output;
        }

        /// <summary>
        /// Pokusava da pogodi TopLevelDomen
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string guessTopLevelDomain(this Uri uri)
        {
            string output = uri.Host;
            string tLD = "";
            List<string> seg = output.Split(".".ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();

            if (seg.Count() > 1)
            {
                if (seg.Last().Length < 4)
                {
                    tLD = seg.Last();
                    seg.Remove(seg.Last());
                    if (seg.Count() > 1)
                    {
                        if (seg.Last().Length < 4)
                        {
                            tLD = tLD + "." + seg.Last();
                        }
                    }
                }
            }
            else
            {
                return "";
            }
            return tLD;
        }

        /// <summary>
        /// v5 > osigurava da URL pocne pravilnom shemom - ako je shema unknown onda nista
        /// </summary>
        /// <param name="url"></param>
        /// <param name="shema"></param>
        /// <returns></returns>
        public static string validateUrlShema(this string url, urlShema shema)
        {
            if (shema == urlShema.unknown) shema = urlShema.http;
            if (shema == urlShema.none) shema = urlShema.http;
            string sufix = "";
            if (shema != urlShema.none)
            {
                bool hasShema = url.Contains(urlShemaSufix);
                if (!url.StartsWith(shema.ToString(), StringComparison.CurrentCultureIgnoreCase))
                {
                    if (hasShema)
                    {
                        sufix = url.Substring(url.IndexOf(urlShemaSufix) + urlShemaSufix.Length);
                    }
                    sufix = shema.ToString() + urlShemaSufix;
                }
            }
            return sufix.add(url, "/");
        }

        /// <summary>
        /// sklanja shemu - kao sto je su: http://  ftp:// itd
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string removeUrlShema(this string url)
        {
            if (url.Contains(urlShemaSufix))
            {
                url = url.Substring(url.IndexOf(urlShemaSufix) + urlShemaSufix.Length);
            }
            return url;
        }

        /// <summary>
        /// HTTPSs to HTTP shema check and replacement
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string httpsToHttpShema(this string url)
        {
            if (url.StartsWith("https:")) // << ------------------------------------------------------------------------------------- https to http link transformation
            {
                url = imbSciStringExtensions.removeStartsWith(url, "https:");
                url = url.ensureStartsWith("http:");
            }
            return url;
        }

        /// <summary>
        /// Pokusava da instancira objekat Uri
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Uri validateUrlWithUri(this string url)
        {
            Uri output = null;
            Uri.TryCreate(url, UriKind.Absolute, out output);
            return output;
        }

        /// <summary>
        /// Iz URL-a izdvaja ime domena - sklanja UrlShema deo i www. deo. Opciono konvertuje . u -
        /// </summary>
        /// <param name="input"></param>
        /// <param name="convertDots"></param>
        /// <returns></returns>
        public static string getDomainNameFromUrl(this string input, bool convertDots, bool removeWWW = false)
        {
            string output = input;
            if (imbSciStringExtensions.isNullOrEmptyString(input)) return output;

            output = output.ToLower();
            output = output.removeUrlShema();
            if (removeWWW) output = output.Replace("www.", "");
            var mt = imbStringSelect._select_rootDomainNameWithoutRelPath.Match(output);
            if (mt.Success)
            {
                output = mt.Groups[1].Value;
            }
            /*
            mt = imbStringSelect._select_isLettersWithDotsFromStart.Match(output);
            if (mt.Success)
            {
                output = mt.Value;
            }
            */
            if (convertDots)
            {
                output = output.Replace(".", "-");
            }

            return output;
        }

        /// <summary>
        /// Vraca segment URL-a
        /// </summary>
        /// <param name="input"></param>
        /// <param name="targetSegment"></param>
        /// <param name="indInput"></param>
        /// <returns></returns>
        public static string getUrlSegment(this string input, urlSegment targetSegment, object indInput = null)
        {
            string output = "";
            Uri tmpUri = null;
            bool haveQuery = false;

            int index = 0;
            string paramName = "";

            if (indInput == null)
            {
                index = 0;
            }
            else
            {
                if (typeof(string) == indInput.GetType())
                {
                    paramName = indInput as string;
                }
            }

            haveQuery = (input.IndexOf('?') > -1);

            if (haveQuery)
            {
                string[] tmpArr = input.Split("?".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                if (tmpArr.Length > 1)
                {
                    string query = tmpArr[1];

                    switch (targetSegment)
                    {
                        case urlSegment.getQParamName:
                        case urlSegment.getQParamSegment:
                        case urlSegment.getQParamValue:
                            output = getQuerySegment(query, targetSegment, index, paramName);
                            break;
                    }
                }
            }

            switch (targetSegment)
            {
                case urlSegment.getFileName:
                    output = tmpUri.GetComponents(UriComponents.Path, UriFormat.SafeUnescaped);
                    break;

                case urlSegment.getFileExtension:
                    output = Path.GetExtension(input);
                    break;

                case urlSegment.getFileNameOnly:
                    output = Path.GetFileNameWithoutExtension(input);
                    break;

                case urlSegment.getPathBeforeFileName:
                    output = Path.GetDirectoryName(input);
                    break;

                default:
                    break;
            }
            return output;
        }

        /// <summary>
        /// izdvaja Query ili ostatak iz URL-a. Ako nema ? , onda gleda da li ima = i vraca sve. Ako nema ni = onda vraca prazan string
        /// </summary>
        /// <param name="urlInput"></param>
        /// <param name="getLeftSide"></param>
        /// <returns></returns>
        public static string getQueryFromUrl(string urlInput, bool getLeftSide = false)
        {
            int pos = urlInput.IndexOf("?") + 1;
            string query = "";
            string output = "";

            if (pos > 0)
            {
                query = urlInput.Substring(pos);
            }
            else
            {
                if (urlInput.IndexOf("=") == -1)
                {
                    return "";
                }
                else
                {
                    return urlInput;
                }
            }

            if (getLeftSide)
            {
                output = urlInput.Replace(query, "");
            }
            else
            {
                output = query;
            }

            return output;
        }

        /// <summary>
        /// Vraca dictionary sa ParamName vs Value - input je URL query
        /// </summary>
        /// <param name="query">URL string ili samo query</param>
        /// <returns></returns>
        public static Dictionary<string, string> getQueryPairs(string query)
        {
            int pos = query.IndexOf("?");
            if (pos > 0)
            {
                query = query.Substring(pos);
            }

            List<string> queryLines = new List<string>();
            queryLines.AddRange(query.Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

            Dictionary<string, string> queryPairs = new Dictionary<string, string>();

            foreach (string qline in queryLines)
            {
                string[] tmpArr = qline.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                int tmpIndex = queryLines.IndexOf(qline);

                if (tmpArr.Length > 0)
                {
                    queryPairs.Add(tmpArr[0], tmpArr[1]);
                }
            }
            return queryPairs;
        }

        /// <summary>
        /// Izdvaja querz segment
        /// </summary>
        /// <param name="query"></param>
        /// <param name="targetSegment"></param>
        /// <param name="index"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public static string getQuerySegment(string query, urlSegment targetSegment, int index, string paramName = "")
        {
            if (string.IsNullOrEmpty(query)) return "";

            List<string> queryLines = new List<string>();

            Dictionary<string, string> queryPairs = new Dictionary<string, string>();
            Dictionary<int, KeyValuePair<string, string>> queryIndexedPairs =
                new Dictionary<int, KeyValuePair<string, string>>();

            queryLines.AddRange(query.Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

            foreach (string qline in queryLines)
            {
                string[] tmpArr = qline.Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                int tmpIndex = queryLines.IndexOf(qline);

                if (tmpArr.Length > 0)
                {
                    queryPairs.Add(tmpArr[0], tmpArr[1]);
                    queryIndexedPairs.Add(tmpIndex, new KeyValuePair<string, string>(tmpArr[0], tmpArr[1]));
                }
            }

            string paramValue = "";

            if (!(paramName == ""))
            {
                if (queryPairs.ContainsKey(paramName))
                {
                    paramValue = queryPairs[paramName];
                }
            }

            if (index < 0) return "";

            if (index >= queryLines.Count) return "";

            if (queryIndexedPairs.ContainsKey(index))
            {
                switch (targetSegment)
                {
                    case urlSegment.getQParamName:
                        return queryIndexedPairs[index].Key;
                    // break;
                    case urlSegment.getQParamValue:
                        if (!(paramValue == "")) return paramValue;

                        return queryIndexedPairs[index].Value;
                    //  break;
                    case urlSegment.getQParamSegment:
                        return queryIndexedPairs[index].Key + "=" + queryIndexedPairs[index].Value;
                        //  break;
                }
            }

            return "";
        }

        //  public static Regex _getDomain = new Regex()

        /// <summary>
        /// Vrši standardizaciju URL-a
        /// (staro rešenje)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="addWww"></param>
        /// <param name="addLastBackSlash"></param>
        /// <returns></returns>
        public static string getStandardizedUrl(string url, urlShema targetShema)
        {
            string output = url.validateUrlShema(targetShema); // url.validateUrlShema(targetShema);
            Uri res = null;
            Uri.TryCreate(url, UriKind.Absolute, out res);

            if (res != null)
            {
                return res.AbsoluteUri;
            }
            else
            {
                return "";
            }
            /*
            UriBuilder ub = new UriBuilder(url);
            output = ub.Uri.AbsoluteUri;

            */

            /*
            url.removeStartsWith("//");
            url.removeEndsWith("//");

            output = output.Replace("http://", "");

            output = output.Replace("//", "/");

            if (addWww) output = output.Replace("www.", "");

            String prefix = "http://";
            if (addWww) prefix = prefix + "www.";

            output = prefix + output;

            if (addLastBackSlash)
            {
                if (!(output[output.Length - 1] == Convert.ToChar("/")))
                {
                    output += "/";
                }
            }*/

            return output;
        }

        /// <summary>
        /// Iz prosleđenog urla izdvaja nedozvoljene karaktere.
        ///
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>String koji predstavlja URL i može biti deo imena fajla</returns>
        public static string getCleanFileName(this string url, bool removeExtension = false)
        {
            string output = url.removeFromString("\"*:<>?\\/|{}".ToArray()); // url;
            output = output.Replace(" ", "_");

            if (url.Contains(".") && removeExtension)
            {
                string ext = Path.GetExtension(url);
                url = url.Replace(ext, "");
                url = url.Replace(".", "_");
            }
            output = output.Replace(Environment.NewLine, " ");
            return output;
            /*
            output = output.Replace("/", "");
            output = output.Replace(".", "");
          output = output.Replace(":", "");
            output = output.Replace("?", "");
            output = output.Replace("=", "");
            output = output.Replace("&", "");
            output = output.Replace("%", "");

            output = output.Replace(Environment.NewLine, " ");
            output = output.Replace(":", "");
            output = output.Replace(",", "");
            output = output.Replace("'", "");
            output = output.Replace("\"", "");
            output = output.Replace(";", " ");

            //val = val.Replace(Environment.NewLine, "");
            //val = val.Replace(":", "");
            //val = val.Replace(",", "");
            //val = val.Replace("'", "");
            //val = val.Replace("\"", "");
            //val = val.Replace(";", "");

            return output;*/
        }
    }
}