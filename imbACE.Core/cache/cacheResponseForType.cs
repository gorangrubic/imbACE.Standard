// --------------------------------------------------------------------------------------------------------------------
// <copyright file="cacheResponseForType.cs" company="imbVeles" >
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
    using imbSCI.Core.files.folders;
    using System;

    /// <summary>
    /// Container class that holds response from cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="imbSCI.Core.core.imbBindable" />
    /// <seealso cref="imbACE.Core.reporting.IAppendDataFields" />
    public class cacheResponseForType<T> : cacheResponseForType where T : new()
    {
        public cacheResponseForType(String __instanceID, folderNode __directory, Boolean __createNewOnNotFound = false) : base(__instanceID, __directory, __createNewOnNotFound, typeof(T))
        {
        }

        /// <summary>
        /// Typed instance of item loaded from cache
        /// </summary>
        public T instanceTyped
        {
            get
            {
                return (T)instance;
            }
            set
            {
                instance = value;
            }
        }
    }
}