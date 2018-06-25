// --------------------------------------------------------------------------------------------------------------------
// <copyright file="screenOptionFlags.cs" company="imbVeles" >
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
namespace imbACE.Services.terminal.core
{
    using imbACE.Core.enums.render;
    using System;

    ///// <summary>
    ///// Kolekcija flagova
    ///// </summary>
    //public class screenOptionFlags : flags<screenOptionFlag>
    //{
    //    public screenOptionFlags()
    //    {
    //    }

    //    /// <summary>
    //    /// Popunjava flagove odgovarajucim objektima u prosledjenom nizu
    //    /// </summary>
    //    /// <param name="resources">skup bilo kojih objekata - odabrace screenOptionFlag i IEnumerable tipa screenOptionFlag</param>
    //    public screenOptionFlags(Object[] resources)
    //    {
    //        populateWith(resources);
    //    }

    //    /// <summary>
    //    /// Kreira kolekciju podrazumevanih flagova
    //    /// </summary>
    //    /// <returns></returns>
    //    public static screenOptionFlags getDefaultFlags()
    //    {
    //        screenOptionFlags output = new screenOptionFlags();
    //        // ovde ubaciti podrazumevane flagove
    //        //output.Add(screenOptionFlag.useExternalLibrary);
    //        foreach (Enum en in Enum.GetValues(typeof(screenOptionFlag)))
    //        {
    //            output.Add((screenOptionFlag)en);
    //        }

    //        return output;
    //    }

    //    public static implicit operator screenOptionFlags(screenOptionFlag flag)
    //    {
    //        screenOptionFlags output = new screenOptionFlags();
    //        output.Add(flag);
    //        return output;
    //    }

    //    /// <summary>
    //    /// Implicitna konverzija iz niza u flag objekat
    //    /// </summary>
    //    /// <param name="newItem"></param>
    //    /// <returns></returns>
    //    public static implicit operator screenOptionFlags(screenOptionFlag[] flagArray)
    //    {
    //        screenOptionFlags output = new screenOptionFlags();
    //        if (flagArray == null)
    //        {
    //            return getDefaultFlags();
    //        }
    //        output.AddRange(flagArray);
    //        return output;
    //    }
    //}
}