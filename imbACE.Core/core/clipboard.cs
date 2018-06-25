// --------------------------------------------------------------------------------------------------------------------
// <copyright file="clipboard.cs" company="imbVeles" >
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
namespace imbACE.Core.core
{
    using extensions.io;
    using imbACE.Core.extensions;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.files;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Xml.Serialization;

    /// <summary>
    /// Clipboard operations
    /// </summary>
    public static class clipboard
    {
        private static Object clipboardInstance { get; set; } = null;

        public static String clipboardGetText()
        {
            clipboardGetter getter = new clipboardGetter();
            getter.Go();
            return getter.data;
        }

        /// <summary>
        /// Sets the text.
        /// </summary>
        /// <param name="input">The input.</param>
        public static void clipboardSetText(this String input)
        {
            new clipboardSetter(input).Go();
        }

        private static Object SetGetInstanceLock = new Object();

        public static void setObject(Object lastResponse)
        {
            lock (SetGetInstanceLock)
            {
                if (lastResponse is String)
                {
                    clipboardInstance = null;
                    clipboardSetText(lastResponse.toStringSafe());
                }
                else
                {
                    clipboardInstance = lastResponse;
                    String serialized = objectSerialization.ObjectToXML(lastResponse);
                }
            }
            throw new NotImplementedException();
        }

        public static Object getObject()
        {
            if (clipboardInstance == null)
            {
                throw new NotImplementedException();
            }

            return clipboardInstance;
        }
    }
}