// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDynamicTreeSource.cs" company="imbVeles" >
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
namespace imbACE.Core.core.interfaces
{
    #region imbVeles using

    using imbACE.Core.core.exceptions;
    using imbACE.Core.interfaces;
    using imbSCI.Core.interfaces;
    using System;
    using System.Collections;

    #endregion imbVeles using

    public interface IDynamicTreeSource<T, I> : IDynamicTreeSource, IDynamicListSource<I>, IDynamicListSource
        where T : class
        where I : class
    {
        /// <summary>
        /// Pristup pod granama
        /// </summary>
        IDynamicListSource<T> subTrees { get; }

        T parent { get; }
    }

    /// <summary>
    /// Objekat koji moze biti izvor DynamicTree grane u OperationMenu -
    /// </summary>
    public interface IDynamicTreeSource : IDynamicTreeSourceBase
    {
        /// <summary>
        /// Pristup pod granama
        /// </summary>
        IDynamicListSource subTrees { get; }

        Object parent { get; }
    }

    public interface IDynamicGraphSource : IDynamicSource, IDisplayInfoExtended
    {
        IDynamicListSource subTrees { get; }
    }

    public interface IDynamicListSource : IEnumerable, IDynamicSource
    {
        /// <summary>
        /// Direct access to items in dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Object this[String key] { get; set; }
    }
}