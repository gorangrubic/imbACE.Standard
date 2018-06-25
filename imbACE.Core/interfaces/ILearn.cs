// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILearn.cs" company="imbVeles" >
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
using imbACE.Core.core.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbACE.Core.interfaces
{
    /// <summary>
    /// Has ability to copy values of <c>source</c> properties.
    /// </summary>
    /// <remarks>
    /// Unlike <c>IClonable</c> it keep all references to this object untouched.
    /// </remarks>
    public interface ILearn
    {
        /// <summary>
        /// In default implementation learns all  public properties from the specified source.
        /// </summary>
        /// <typeparam name="T">source object to learn from. It's exception safer to use the source object of the same or derived class.</typeparam>
        /// <param name="source">The source.</param>
        /// <remarks>Uses <see cref="imbTypeObjectOperations.setObjectBySource(object, object, string[])"/> for default implementation. </remarks>
        void Learn(Object source);
    }
}