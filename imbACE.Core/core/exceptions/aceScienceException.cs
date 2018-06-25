// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceScienceException.cs" company="imbVeles" >
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbACE.Core.core.exceptions
{
    using imbACE.Core.collection;
    using imbACE.Core.core.exceptions;
    using imbACE.Core.extensions;
    using System.Collections;
    using System.Data;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Exception about problem with logic, common sense, scientific method and/or ethics
    /// </summary>
    /// <seealso cref="imbACE.Core.core.exceptions.aceGeneralException" />
    public class aceScienceException : aceGeneralException
    {
        public aceScienceException(String message, Exception innerEx, Object instance, String title, Object alsoRelatedInstance = null) : base(message, innerEx, instance, title)
        {
        }
    }
}