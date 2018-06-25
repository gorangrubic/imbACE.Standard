// --------------------------------------------------------------------------------------------------------------------
// <copyright file="globalResourceManager.cs" company="imbVeles" >
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
namespace imbACE.Core.extensions.io
{
    using System;
    using System.IO;
    using System.Windows;

#if net45
    using System.Windows.Resources;
#endif

    public static class globalResourceManager
    {
        /// <summary>
        /// Loads content file to string
        /// </summary>
        /// <param name="pathToContent"></param>
        /// <returns></returns>
        public static String getContentFileToString(String pathToContent)
        {
#if net45
            Uri uri = new Uri(pathToContent, UriKind.Relative);
            StreamResourceInfo sri = Application.GetContentStream(uri);
            StreamReader sr = new StreamReader(sri.Stream);
            String output = sr.ReadToEnd();
            return output;
#else
            return File.ReadAllText(pathToContent);
#endif
        }
    }
}