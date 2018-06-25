// --------------------------------------------------------------------------------------------------------------------
// <copyright file="soundExtensions.cs" company="imbVeles" >
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

namespace imbACE.Core.sound
{
    using imbACE.Core.sound.beep;
    using imbACE.Core.sound.sequence;
    using System;
    using System.Collections.Generic;

    public static class soundExtensions
    {
        /// <summary>
        /// Vraca frekvenciju za dati ton
        /// </summary>
        /// <param name="tone"></param>
        /// <returns></returns>
        public static Int32 toFrequency(this beepTone tone)
        {
            return beepTones[tone];
        }

        public static beepSequence toSequence(this beepRoleType role)
        {
            return beepLibrary[role];
        }

        public static Int32 toMilisecond(this beepDuration note, Int32 bpm)
        {
            Int32 fullNote = 60000 / bpm;
            switch (note)
            {
                default:
                case beepDuration.full:
                    return fullNote;
                    break;

                case beepDuration.half:
                    return fullNote / 2;
                    break;

                case beepDuration.oneThird:
                    return fullNote / 4;
                    break;

                case beepDuration.threeThirds:
                    return 3 * fullNote / 4;
            }
        }

        #region --- beepLibrary ------- biblioteka podrazumevanih tonova

        private static Dictionary<beepRoleType, beepSequence> _beepLibrary;

        /// <summary>
        /// biblioteka podrazumevanih tonova
        /// </summary>
        public static Dictionary<beepRoleType, beepSequence> beepLibrary
        {
            get
            {
                if (_beepLibrary == null)
                {
                    _beepLibrary = new Dictionary<beepRoleType, beepSequence>();
                    _beepLibrary.Add(beepRoleType.done, new beepSequence(beepTone.A1, beepTone.D1, beepTone.G2));
                    _beepLibrary.Add(beepRoleType.error, new beepSequence(beepTone.G1, beepTone.none, beepTone.G1, beepTone.none, beepTone.G1));
                    _beepLibrary.Add(beepRoleType.message, new beepSequence(beepTone.A1, beepTone.none, beepTone.G1));
                }
                return _beepLibrary;
            }
        }

        #endregion --- beepLibrary ------- biblioteka podrazumevanih tonova

        #region --- beepTones ------- kolekcija svih tonova

        private static Dictionary<beepTone, Int32> _beepTones;

        /// <summary>
        /// kolekcija svih tonova
        /// </summary>
        public static Dictionary<beepTone, Int32> beepTones
        {
            get
            {
                if (_beepTones == null)
                {
                    _beepTones = new Dictionary<beepTone, Int32>();
                    _beepTones.Add(beepTone.none, 0);
                    _beepTones.Add(beepTone.C0, 262);
                    _beepTones.Add(beepTone.Cis0, 277);
                    _beepTones.Add(beepTone.D0, 294);
                    _beepTones.Add(beepTone.Dis0, 311);
                    _beepTones.Add(beepTone.E0, 330);
                    _beepTones.Add(beepTone.F0, 349);
                    _beepTones.Add(beepTone.Fis0, 370);
                    _beepTones.Add(beepTone.G0, 392);
                    _beepTones.Add(beepTone.Gis0, 415);
                    _beepTones.Add(beepTone.A0, 440);
                    _beepTones.Add(beepTone.Ais0, 466);
                    _beepTones.Add(beepTone.B0, 494);
                    _beepTones.Add(beepTone.C1, 523);
                    _beepTones.Add(beepTone.Cis1, 554);
                    _beepTones.Add(beepTone.D1, 587);
                    _beepTones.Add(beepTone.Dis1, 622);
                    _beepTones.Add(beepTone.E1, 659);
                    _beepTones.Add(beepTone.F1, 698);
                    _beepTones.Add(beepTone.Fis1, 740);
                    _beepTones.Add(beepTone.G1, 784);
                    _beepTones.Add(beepTone.Gis1, 831);
                    _beepTones.Add(beepTone.A1, 880);
                    _beepTones.Add(beepTone.Ais1, 932);
                    _beepTones.Add(beepTone.B1, 988);
                    _beepTones.Add(beepTone.C2, 785);
                    _beepTones.Add(beepTone.Cis2, 832);
                    _beepTones.Add(beepTone.D2, 881);
                    _beepTones.Add(beepTone.Dis2, 933);
                    _beepTones.Add(beepTone.E2, 989);
                    _beepTones.Add(beepTone.F2, 1048);
                    _beepTones.Add(beepTone.Fis2, 1110);
                    _beepTones.Add(beepTone.G2, 1177);
                    _beepTones.Add(beepTone.Gis2, 1246);
                    _beepTones.Add(beepTone.A2, 1320);
                    _beepTones.Add(beepTone.Ais2, 1399);
                    _beepTones.Add(beepTone.B2, 1482);
                    _beepTones.Add(beepTone.C3, 1046);
                    _beepTones.Add(beepTone.Cis3, 1109);
                    _beepTones.Add(beepTone.D3, 1175);
                    _beepTones.Add(beepTone.Dis3, 1244);
                    _beepTones.Add(beepTone.E3, 1318);
                    _beepTones.Add(beepTone.F3, 1397);
                    _beepTones.Add(beepTone.Fis3, 1480);
                    _beepTones.Add(beepTone.G3, 1569);
                    _beepTones.Add(beepTone.Gis3, 1661);
                    _beepTones.Add(beepTone.A3, 1760);
                    _beepTones.Add(beepTone.Ais3, 1865);
                    _beepTones.Add(beepTone.B3, 1976);
                }
                return _beepTones;
            }
        }

        #endregion --- beepTones ------- kolekcija svih tonova
    }
}