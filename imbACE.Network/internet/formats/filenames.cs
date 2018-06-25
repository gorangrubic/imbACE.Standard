// --------------------------------------------------------------------------------------------------------------------
// <copyright file="filenames.cs" company="imbVeles" >
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
namespace imbACE.Network.internet.formats
{
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
    using System.Collections.Generic;

    public static class filenames
    {
        private static List<string> _indexAllFilenames;

        /// <summary>
        /// List of all known index filenames, not only HTML
        /// </summary>
        public static List<string> indexAllFilenames
        {
            get
            {
                if (_indexAllFilenames == null)
                {
                    _indexAllFilenames = new List<string>();
                    _indexAllFilenames.AddRange(new string[] {"index.pl",
                                                     "index.html",
                                                     "index.htm",
                                                     "index.shtml",
                                                     "index.php",
                                                     "index.php5",
                                                     "index.php4",
                                                     "index.php3",
                                                     "index.cgi",
                                                     "default.html",
                                                     "default.htm",
                                                     "home.html",
                                                     "home.htm",
                                                     "Index.html",
                                                     "Index.htm",
                                                     "Index.shtml",
                                                     "Index.php",
                                                     "Index.cgi",
                                                     "Default.html",
                                                     "Default.htm",
                                                     "Home.html",
                                                     "Home.htm",
                                                     "placeholder.html" });
                }
                return _indexHtmlFilenames;
            }
        }

        private static List<string> _indexHtmlFilenames;

        /// <summary>
        /// static and autoinitiated object
        /// </summary>
        public static List<string> indexHtmlFilenames
        {
            get
            {
                if (_indexHtmlFilenames == null)
                {
                    _indexHtmlFilenames = new List<string>();
                    _indexHtmlFilenames.AddRange(new string[] { "index.html", "index.htm", "default.html", "default.htm" });
                }
                return _indexHtmlFilenames;
            }
        }
    }
}