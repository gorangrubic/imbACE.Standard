// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPlatformBase.cs" company="imbVeles" >
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
using imbACE.Core;
using imbSCI.Core;
using imbSCI.Core.attributes;
using imbSCI.Core.enums;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.interfaces;
using imbSCI.Data;
using imbSCI.Data.collection;
using imbSCI.Data.data;
using imbSCI.Data.interfaces;
using imbSCI.DataComplex;
using imbSCI.Reporting;
using imbSCI.Reporting.enums;
using imbSCI.Reporting.interfaces;

namespace imbACE.Services.platform.interfaces
{
    using imbACE.Core.enums.platform;
    using imbACE.Core.sound.beep;
    using imbACE.Services.platform.input;
    using imbSCI.Core.reporting.zone;

    using imbSCI.Core.reporting.zone;

    using System;
    using System.Collections.Generic;

    public interface IPlatformBase : ISupportsBasicCursor
    {
        /// <summary>
        /// Prikazuje prosledjeni sadrzaj na datim koordinatama
        /// </summary>
        /// <param name="content">Jednolinijski sadrzaj</param>
        /// <param name="x">X koordinata, ako je izostavljena onda je 0</param>
        /// <param name="y">Ako ostane -1 onda pise u trenutnom redu</param>
        void render(String content, Int32 x = 0, Int32 y = -1);

        void render(IEnumerable<String> content, Int32 x = 0, Int32 y = -1);

        /// <summary>
        /// Sets the size of the window.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="doAutoDeploySize">if set to <c>true</c> [do automatic deploy size].</param>
        void setWindowSize(windowSize size, Boolean doAutoDeploySize = true);

        /// <summary>
        /// Sets the size of the window.
        /// </summary>
        /// <param name="preset">The preset.</param>
        /// <param name="doAutoDeploySize">if set to <c>true</c> [do automatic deploy size].</param>
        void setWindowSize(cursorZoneSpatialPreset preset, Boolean doAutoDeploySize = true);

        /// <summary>
        /// Brise ceo do sadasnji izlaz
        /// </summary>
        void clear();

        /// <summary>
        /// Pusta zvuk
        /// </summary>
        /// <param name="bRoleType"></param>
        void beep(beepRoleType bRoleType);

        void beep(Int32 frequency, Int32 duration);

        /// <summary>
        /// Postavlja naslov
        /// </summary>
        /// <param name="title">Naslov koji treba da se ispise</param>
        void title(String title);

        /// <summary>
        /// Postavlja boje za dalji izlaz
        /// </summary>
        /// <param name="foreColor">Prednja boja. Ako je .none onda resetuje boje</param>
        /// <param name="backColor">Boja pozadine</param>
        /// <param name="doInvert"> </param>
        void setColors(platformColorName foreColor, platformColorName backColor, Boolean doInvert = false);

        /// <summary>
        /// Cita odredjenu zonu
        /// </summary>
        /// <param name="mode">Vrste citanja</param>
        /// <param name="x">-1 koristi trenutni</param>
        /// <param name="y">-1 koristi trenutni</param>
        /// <param name="w">vise od 0 znaci citanje 1D ili 2D</param>
        /// <param name="h">vise od 0 znaci citanje 2D bloka</param>
        /// <returns>Popunjen input result</returns>
        ///textInputResult read(textInputResult currentOutput, inputReadMode mode, Int32 x=-1, Int32 y=-1, Int32 w = 0, Int32 h = 0);

        /// <summary>
        /// Citanje pomocu zone objekta
        /// </summary>
        /// <param name="mode">Vrsta citanja</param>
        /// <param name="zone">Zona</param>
        /// <returns></returns>
        textInputResult read(textInputResult currentOutput, inputReadMode mode, selectZone zone, Object startValue = null);

        /// <summary>
        /// Vraca dimenzije trenutnog prozora
        /// </summary>
        /// <returns></returns>
        selectRange getWindowSize();

        /// <summary>
        /// Pomera kursor na zadatu koordinatu
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void moveCursor(Int32 x = 0, Int32 y = -1);
    }
}