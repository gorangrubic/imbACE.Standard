// --------------------------------------------------------------------------------------------------------------------
// <copyright file="enumerableExtensions.cs" company="imbVeles" >
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
namespace imbACE.Core.data
{
    using imbACE.Core.core.exceptions;
    using imbSCI.Core.extensions.data;
    using System;
    using System.Collections;

    public static class enumerableExtensions
    {
        /// <summary>
        /// Gets <see cref="aceSubEnum"/> enumerator that iterates trough values found on the path
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        /// <exception cref="aceGeneralException">The path can't be empty - null - null - enumerableExtensions</exception>
        public static aceSubEnum Get(this IEnumerable source, String path)
        {
            if (path.isNullOrEmpty())
            {
                throw new aceGeneralException("The path can't be empty", null, null, nameof(enumerableExtensions));
            }
            return new aceSubEnum(source, path);
        }
    }
}