// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dialogUniversalTool.cs" company="imbVeles" >
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
namespace imbACE.Services.terminal.dialogs
{
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.dialogs.core;
    using imbSCI.Core.extensions.data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Dialog imbTUI
    /// </summary>
    public static class dialogUniversalTool
    {
        /// <summary>
        /// Otvara privremenu instancu dijaloga i sinhrono ceka rezultat
        /// </summary>
        /// <param name="platform">Platforma na kojoj se prikazuje dijalog</param>
        /// <param name="targetObject">Objekat koi se edituje</param>
        /// <param name="spec">Property za koji se edituje ovaj objekat</param>
        /// <param name="hostTitle">Naslov host->property objekta</param>
        /// <returns>Kolekcija rezultata</returns>
        public static T openPageViewer<T>(IPlatform platform, IEnumerable<string> content, String title, String description, params T[] spec)
        {
            dialogPageViewerWithMenu<T> dialog = new dialogPageViewerWithMenu<T>(platform, content, title, description, spec);

            var format = new dialogFormatSettings(dialogStyle.redDialog, dialogSize.mediumBox);

            T defVal = (T)spec.ToList().getFirstSafe();

            var res = dialog.open(platform, format);

            T output = res.getResultObject<T>(defVal);

            return output;
        }

        /// <summary>
        /// Otvara privremenu instancu dijaloga i sinhrono ceka rezultat
        /// </summary>
        /// <param name="platform">Platforma na kojoj se prikazuje dijalog</param>
        /// <param name="targetObject">Objekat koi se edituje</param>
        /// <param name="spec">Property za koji se edituje ovaj objekat</param>
        /// <param name="hostTitle">Naslov host->property objekta</param>
        /// <param name="title">todo: describe title parameter on openMessageBox</param>
        /// <param name="description">todo: describe description parameter on openMessageBox</param>
        /// <returns>Kolekcija rezultata</returns>
        public static T openMessageBox<T>(IPlatform platform, String title, String description, params T[] spec)
        {
            var dialog = new dialogMessageBoxWithOptions<T>(platform, title, description, spec);

            T defVal = (T)spec.ToList().getFirstSafe();

            var format = new dialogFormatSettings(dialogStyle.redDialog, dialogSize.mediumBox);

            var res = dialog.open(platform, format);

            T output = res.getResultObject<T>(defVal);

            return output;
        }
    }
}