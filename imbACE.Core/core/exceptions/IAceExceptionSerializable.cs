// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAceExceptionSerializable.cs" company="imbVeles" >
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
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    public interface IAceExceptionSerializable : ILogSerializable
    {
        String Title { get; set; }
        String Message { get; set; }
        String StackTrace { get; set; }
        DateTime time { get; set; }
        String RelInstanceClassName { get; set; }
        String RelInstanceParentClassName { get; set; }
        String SouceCodeLine { get; set; }
        String SourceCodeFile { get; set; }
        String DataDump { get; set; }
        //void SetFromAceException(aceGeneralException axe);
    }
}