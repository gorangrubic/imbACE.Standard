// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textInputBase.cs" company="imbVeles" >
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

namespace imbACE.Services.platform.input
{
    using imbACE.Core.enums.platform;
    using imbACE.Core.operations;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.core;
    using imbACE.Services.textBlocks;
    using imbACE.Services.textBlocks.input;
    using imbSCI.Core.reporting.zone;
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Osnova za sve klase koje mogu da citaju rezultat
    /// </summary>
    public abstract class textInputBase : textSection, IInputSection, IRead, IApplyReading
    {
        protected textInputBase(int __height, int __width, int __leftRightMargin = 0, int __leftRightPadding = 0) : base(__height, __width, __leftRightMargin, __leftRightPadding)
        {
            init();
        }

        /// <summary>
        /// Vrsi resetovanje/pripremu inputa za novo koriscenje, obrisace currentOutput
        /// </summary>
        public virtual void init()
        {
            currentOutput = new textInputResult();
            currentOutput.doKeepReading = true;
        }

        #region -----------  currentOutput  -------  [Trenutni rezultat]

        private textInputResult _currentOutput = new textInputResult();

        /// <summary>
        /// Trenutni rezultat
        /// </summary>
        // [XmlIgnore]
        [Category("textInputBase")]
        [DisplayName("currentOutput")]
        [Description("Trenutni rezultat")]
        public textInputResult currentOutput
        {
            get
            {
                return _currentOutput;
            }
            set
            {
                // Boolean chg = (_currentOutput != value);
                _currentOutput = value;
                OnPropertyChanged("currentOutput");
                // if (chg) {}
            }
        }

        #endregion -----------  currentOutput  -------  [Trenutni rezultat]

        #region ----------- Boolean [ doShowTitle ] -------  [Da li prikazuje naslov inputa]

        private Boolean _doShowTitle = false;

        /// <summary>
        /// Da li prikazuje naslov inputa
        /// </summary>
        [Category("Switches")]
        [DisplayName("doShowTitle")]
        [Description("Da li prikazuje naslov inputa")]
        public Boolean doShowTitle
        {
            get { return _doShowTitle; }
            set { _doShowTitle = value; OnPropertyChanged("doShowTitle"); }
        }

        #endregion ----------- Boolean [ doShowTitle ] -------  [Da li prikazuje naslov inputa]

        #region ----------- Boolean [ doShowRemarks ] -------  [Da li da prikazuje komentar - trenutna vrednost / selektovana opcija]

        private Boolean _doShowRemarks = false;

        /// <summary>
        /// Da li da prikazuje komentar - trenutna vrednost / selektovana opcija
        /// </summary>
        [Category("Switches")]
        [DisplayName("doShowRemarks")]
        [Description("Da li da prikazuje komentar - trenutna vrednost / selektovana opcija")]
        public Boolean doShowRemarks
        {
            get { return _doShowRemarks; }
            set { _doShowRemarks = value; OnPropertyChanged("doShowRemarks"); }
        }

        #endregion ----------- Boolean [ doShowRemarks ] -------  [Da li da prikazuje komentar - trenutna vrednost / selektovana opcija]

        #region ----------- Boolean [ doShowValueRemats ] -------  [Da li prikazuje komentar na trenutnu vrednost / selektovanu opcijua]

        private Boolean _doShowValueRemarks = false;

        /// <summary>
        /// Da li prikazuje komentar na trenutnu vrednost / selektovanu opcijua
        /// </summary>
        [Category("Switches")]
        [DisplayName("doShowValueRemats")]
        [Description("Da li prikazuje komentar na trenutnu vrednost / selektovanu opcijua")]
        public Boolean doShowValueRemarks
        {
            get { return _doShowValueRemarks; }
            set { _doShowValueRemarks = value; OnPropertyChanged("doShowValueRemarks"); }
        }

        #endregion ----------- Boolean [ doShowValueRemats ] -------  [Da li prikazuje komentar na trenutnu vrednost / selektovanu opcijua]

        #region ----------- Boolean [ doShowInstructions ] -------  [Da li da prikazuje instrukcije> npr. izaberi opciju strelicama, potvrdi enter]

        private Boolean _doShowInstructions = true;

        /// <summary>
        /// Da li da prikazuje instrukcije> npr. izaberi opciju strelicama, potvrdi enter
        /// </summary>
        [Category("Switches")]
        [DisplayName("doShowInstructions")]
        [Description("Da li da prikazuje instrukcije> npr. izaberi opciju strelicama, potvrdi enter")]
        public Boolean doShowInstructions
        {
            get { return _doShowInstructions; }
            set { _doShowInstructions = value; OnPropertyChanged("doShowInstructions"); }
        }

        #endregion ----------- Boolean [ doShowInstructions ] -------  [Da li da prikazuje instrukcije> npr. izaberi opciju strelicama, potvrdi enter]

        /// <summary>
        /// #2 Očitava ulaz
        /// </summary>
        public virtual inputResultCollection read(inputResultCollection __results)
        {
            if (__results == null) __results = new inputResultCollection();

            var min = __results.getBySection(this);
            var rd = read(__results.platform, min);
            __results.AddUniqueSection(rd);

            return __results;
        }

        /// <summary>
        /// iscitava jednu iteraciju
        /// </summary>
        /// <returns></returns>
        public virtual textInputResult read(IPlatform platform, textInputResult __currentOutput = null)
        {
            if (__currentOutput != null) currentOutput = __currentOutput;
            if (currentOutput == null) currentOutput = new textInputResult();

            currentOutput = platform.read(currentOutput, inputReadMode.readKey, new selectZone(cursor.x, cursor.y, width, 1));

            //if (top_attachment is IApplyReading)
            //{
            //    IApplyReading r = top_attachment as IApplyReading;
            //    r.applyReading(platform, currentOutput);
            //}

            currentOutput = applyReading(platform, currentOutput);

            if (bottom_attachment is IApplyReading)
            {
                IApplyReading r = bottom_attachment as IApplyReading;
                r.applyReading(platform, currentOutput);
            }

            currentOutput.section = this;

            return currentOutput;
        }

        #region -----------  exitPolicy  -------  [nacin zatvaranja inputa]

        private textInputExitPolicy _exitPolicy = textInputExitPolicy.onValidKey; // = new textInputExitPolicy();

        /// <summary>
        /// nacin zatvaranja inputa
        /// </summary>
        // [XmlIgnore]
        [Category("textInputBase")]
        [DisplayName("exitPolicy")]
        [Description("nacin zatvaranja inputa")]
        public textInputExitPolicy exitPolicy
        {
            get
            {
                return _exitPolicy;
            }
            set
            {
                // Boolean chg = (_exitPolicy != value);
                _exitPolicy = value;
                OnPropertyChanged("exitPolicy");
                // if (chg) {}
            }
        }

        #endregion -----------  exitPolicy  -------  [nacin zatvaranja inputa]

        /// <summary>
        /// 2 Očitava ulaz
        /// </summary>
        //public abstract inputResultCollection read(inputResultCollection __results);

        /// <summary>
        /// Primena procitanog unosa
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="__currentOutput"></param>
        /// <returns></returns>
        public abstract textInputResult applyReading(IPlatform platform, textInputResult __currentOutput);
    }
}