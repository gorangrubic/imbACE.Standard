// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textSectionLineStyle.cs" company="imbVeles" >
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
namespace imbACE.Services.textBlocks.core
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.enums.platform;
    using imbACE.Services.platform.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// Stil za liniju teksta unutar sectiona
    /// </summary>
    public class textSectionLineStyle : INotifyPropertyChanged
    {
        #region --- foreColor ------- boja slova

        private platformColorName _foreColor = platformColorName.White;

        /// <summary>
        /// boja slova
        /// </summary>
        public platformColorName foreColor
        {
            get
            {
                return _foreColor;
            }
            set
            {
                _foreColor = value;
                OnPropertyChanged("foreColor");
            }
        }

        #endregion --- foreColor ------- boja slova

        #region --- backColor ------- pozadinska boja

        private platformColorName _backColor = platformColorName.Black;

        /// <summary>
        /// pozadinska boja
        /// </summary>
        public platformColorName backColor
        {
            get
            {
                return _backColor;
            }
            set
            {
                _backColor = value;
                OnPropertyChanged("backColor");
            }
        }

        #endregion --- backColor ------- pozadinska boja

        /// <summary>
        /// Primenjuje stil na prosledjen objekat
        /// </summary>
        /// <param name="target"></param>
        public void deploy(IAcceptsTextSectionStyle target)
        {
            target.leftFieldWidth = fieldWidth[printHorizontal.left];

            target.rightFieldWidth = fieldWidth[printHorizontal.right];

            target.fieldFormats.Clear();

            foreach (var frm in fieldFormats)
            {
                target.fieldFormats[frm.Key] = frm.Value;
            }

            target.doInverseColors = doInverseColors;

            target.backgroundDecoration = backgroundDeco;
            target.marginDecoration = marginDecoration;

            //if (target.foreColor != platformColorName.none)
            //{
            //    target.foreColor = foreColor;
            //}
            //if (target.backColor != platformColorName.none)
            //{
            //    target.backColor = backColor;
            //}
        }

        public textSectionLineStyle()
        {
            setDefailts();
        }

        protected void setDefailts()
        {
            foreach (printHorizontal sname in Enum.GetValues(typeof(printHorizontal)))
            {
                fieldWidth[sname] = 10;
                fieldFormats[sname] = "[{0}]";
            }
        }

        private Dictionary<printHorizontal, int> _fieldWidth = new Dictionary<printHorizontal, int>();

        public Dictionary<printHorizontal, int> fieldWidth
        {
            get
            {
                return _fieldWidth;
            }
            set
            {
                // Boolean chg = (_fieldFormats != value);
                _fieldWidth = value;
                OnPropertyChanged("fieldFormats");
                // if (chg) {}
            }
        }

        #region -----------  fieldFormats  -------  [formati za polja]

        private Dictionary<printHorizontal, string> _fieldFormats = new Dictionary<printHorizontal, string>();

        /// <summary>
        /// formati za polja
        /// </summary>
        // [XmlIgnore]
        [Category("textSectionLineStyle")]
        [DisplayName("fieldFormats")]
        [Description("formati za polja")]
        public Dictionary<printHorizontal, string> fieldFormats
        {
            get
            {
                return _fieldFormats;
            }
            set
            {
                // Boolean chg = (_fieldFormats != value);
                _fieldFormats = value;
                OnPropertyChanged("fieldFormats");
                // if (chg) {}
            }
        }

        #endregion -----------  fieldFormats  -------  [formati za polja]

        #region -----------  backgroundDeco  -------  [dekoracija za pozadinu]

        private String _backgroundDeco = " "; // = new String();

        /// <summary>
        /// dekoracija za pozadinu
        /// </summary>
        // [XmlIgnore]
        [Category("textSectionLineStyle")]
        [DisplayName("backgroundDeco")]
        [Description("dekoracija za pozadinu")]
        public String backgroundDeco
        {
            get
            {
                return _backgroundDeco;
            }
            set
            {
                // Boolean chg = (_backgroundDeco != value);
                _backgroundDeco = value;
                OnPropertyChanged("backgroundDeco");
                // if (chg) {}
            }
        }

        #endregion -----------  backgroundDeco  -------  [dekoracija za pozadinu]

        #region -----------  marginDecoration  -------  [dekoracija prostora koji odvaja margina]

        private String _marginDecoration = " "; // = new String();

        /// <summary>
        /// dekoracija prostora koji odvaja margina
        /// </summary>
        // [XmlIgnore]
        [Category("textSectionLineStyle")]
        [DisplayName("marginDecoration")]
        [Description("dekoracija prostora koji odvaja margina")]
        public String marginDecoration
        {
            get
            {
                return _marginDecoration;
            }
            set
            {
                // Boolean chg = (_marginDecoration != value);
                _marginDecoration = value;
                OnPropertyChanged("marginDecoration");
                // if (chg) {}
            }
        }

        #endregion -----------  marginDecoration  -------  [dekoracija prostora koji odvaja margina]

        #region ----------- Boolean [ doInverseColors ] -------  [Da li konzola treba da invertuje boje kada prikazuje ovu liniju]

        private Boolean _doInverseColors = false;

        /// <summary>
        /// Da li konzola treba da invertuje boje kada prikazuje ovu liniju
        /// </summary>
        [Category("Switches")]
        [DisplayName("doInverseColors")]
        [Description("Da li konzola treba da invertuje boje kada prikazuje ovu liniju")]
        public Boolean doInverseColors
        {
            get { return _doInverseColors; }
            set { _doInverseColors = value; OnPropertyChanged("doInverseColors"); }
        }

        #endregion ----------- Boolean [ doInverseColors ] -------  [Da li konzola treba da invertuje boje kada prikazuje ovu liniju]

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}