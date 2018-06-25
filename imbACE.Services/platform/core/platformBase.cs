// --------------------------------------------------------------------------------------------------------------------
// <copyright file="platformBase.cs" company="imbVeles" >
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
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbACE.Services.platform.core
{
    using imbACE.Core.enums.platform;
    using imbACE.Core.sound;
    using imbACE.Core.sound.beep;
    using imbACE.Core.sound.sequence;
    using imbACE.Services.platform.input;
    using imbACE.Services.terminal.core;
    using imbACE.Services.textBlocks;
    using imbACE.Services.textBlocks.core;
    using imbACE.Services.textBlocks.core.proto;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Core.reporting.zone;

    /// <summary>
    /// Osnovna klasa koja definise UI platformu
    /// </summary>
    public abstract class platformBase : textFormatSetupBase
    {
        public abstract void deploySize();

        public platformBase() : base(0, 0, 0)
        {
            deploySize();
        }

        //public abstract Int32 x { get; set;}
        //public abstract Int32 y { get; set;}

        ///// <summary>
        ///// maksimalna spoljna sirina formata (innerWidth+padding+margin = Windows.width)
        ///// </summary>
        //int ISupportsTextCursor.width { get; set; }

        ///// <summary>
        ///// maksimalna spoljna visina formata (innerHeight+padding+margin=Windows.Height)
        ///// </summary>
        //int ISupportsTextCursor.height { get; set; }
        //public abstract Int32 width { get;  }
        //public abstract Int32 height { get; }

        /// <summary>
        /// Prikazuje prosledjeni sadrzaj na datim koordinatama
        /// </summary>
        /// <param name="content">Jednolinijski sadrzaj</param>
        /// <param name="x">X koordinata, ako je izostavljena onda je 0</param>
        /// <param name="y">Ako ostane -1 onda pise u trenutnom redu</param>
        public abstract void render(String content, Int32 x = 0, Int32 y = -1);

        /// <summary>
        /// Brise ceo do sadasnji izlaz
        /// </summary>
        public abstract void clear();

        /// <summary>
        /// Pusta zvuk
        /// </summary>
        /// <param name="bRoleType"></param>
        public virtual void beep(beepRoleType role)
        {
            beepSequence seq = role.toSequence();
            foreach (var s in seq)
            {
                if (s.frequency > 0)
                {
                    Int32 dur = s.duration.toMilisecond(seq.tempo);
                    Int32 frq = s.frequency;
                    beep(frq, dur);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="frequency">u hz</param>
        /// <param name="duration">u mili sekundama</param>
        public abstract void beep(Int32 frequency, Int32 duration);

        /// <summary>
        /// Postavlja naslov
        /// </summary>
        /// <param name="title">Naslov koji treba da se ispise</param>
        public abstract void title(String title);

        /// <summary>
        /// Postavlja boje za dalji izlaz
        /// </summary>
        /// <param name="foreColor">Prednja boja. Ako je .none onda resetuje boje</param>
        /// <param name="backColor">Boja pozadine</param>
        /// <param name="doInvert"> </param>
       // public abstract void setColors(platformColorName foreColor, platformColorName backColor, Boolean doInvert=false);

        /// <summary>
        /// Cita odredjenu zonu
        /// </summary>
        /// <param name="mode">Vrste citanja</param>
        /// <param name="x">-1 koristi trenutni</param>
        /// <param name="y">-1 koristi trenutni</param>
        /// <param name="w">vise od 0 znaci citanje 1D ili 2D</param>
        /// <param name="h">vise od 0 znaci citanje 2D bloka</param>
        /// <returns>Popunjen input result</returns>
        public abstract textInputResult read(textInputResult currentOutput, inputReadMode mode, selectZone zone, object startValue);

        ///// <summary>
        ///// Citanje pomocu zone objekta
        ///// </summary>
        ///// <param name="mode">Vrsta citanja</param>
        ///// <param name="zone">Zona</param>
        ///// <returns></returns>
        //public abstract textInputResult read(textInputResult currentOutput, inputReadMode mode, selectZone zone);

        /// <summary>
        /// Vraca dimenzije trenutnog prozora
        /// </summary>
        /// <returns></returns>
        public abstract selectRange getWindowSize();

        /// <summary>
        /// Pomera kursor na zadatu koordinatu
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public abstract void moveCursor(Int32 x = 0, Int32 y = -1);
    }
}