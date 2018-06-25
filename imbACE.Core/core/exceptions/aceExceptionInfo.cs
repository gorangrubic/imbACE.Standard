// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceExceptionInfo.cs" company="imbVeles" >
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
namespace imbACE.Core.core.exceptions
{
    using extensions.io;
    using imbACE.Core.collection;
    using imbACE.Core.core.exceptions;
    using imbACE.Core.extensions;
    using imbACE.Core.interfaces;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.files;
    using imbSCI.Data;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Additional information on exception -- used for XML Serialization
    /// </summary>
    /// <seealso cref="imbACE.Core.core.exceptions.aceExceptionInfoBase" />
    public class aceExceptionInfo : aceExceptionInfoBase
    {
        public aceExceptionInfo()
        {
        }

        public override string GetFilename()
        {
            throw new NotImplementedException();
        }

        public void SetFromAceException(aceGeneralException axe)
        {
            axe.SetLogSerializable(this);
        }

        /// <summary>
        /// Saves the XML into specified path or default diagnostic location
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Full filepath of the saved XML or empty string if failed</returns>
        public override String SaveXML(String path = null)
        {
            if (path.isNullOrEmpty())
            {
                path = "diagnostic";
            }

            path = imbSciStringExtensions.add(path, GetFilename(), "\\");

            String xml = objectSerialization.ObjectToXML(this);

            FileInfo fi = path.getWritableFile();

            if (fi.FullName.saveToFile(xml))
            {
                return fi.FullName;
            }
            else
            {
                return "";
            }
        }
    }
}