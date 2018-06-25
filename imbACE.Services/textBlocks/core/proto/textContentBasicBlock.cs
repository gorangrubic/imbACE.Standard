// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textContentBasicBlock.cs" company="imbVeles" >
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
namespace imbACE.Services.textBlocks.core.proto
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.enums.platform;
    using imbACE.Core.extensions;
    using imbACE.Services.platform.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using System;

    public abstract class textContentBasicBlock : textFormatSetupBase, ITextContentBasic, ItextContentBasicBlock
    {
        //public abstract textContentLines contentLines { get; set; }

        #region --- ZLayerOrder ------- Z redosled sloja u kome se nalazi ovaj blok - 0 se prvo renderuje, 100 poslednji

        private Int32 _ZLayerOrder = 50;

        /// <summary>
        /// Z redosled sloja u kome se nalazi ovaj blok - 0 se prvo renderuje, 100 poslednji
        /// </summary>
        public Int32 ZLayerOrder
        {
            get
            {
                return _ZLayerOrder;
            }
            set
            {
                _ZLayerOrder = value;
                OnPropertyChanged("ZLayerOrder");
            }
        }

        #endregion --- ZLayerOrder ------- Z redosled sloja u kome se nalazi ovaj blok - 0 se prvo renderuje, 100 poslednji

        #region --- doInverseColors ------- da li da invertuje boje

        private Boolean _doInverseColors;

        /// <summary>
        /// da li da invertuje boje
        /// </summary>
        public Boolean doInverseColors
        {
            get
            {
                return _doInverseColors;
            }
            set
            {
                _doInverseColors = value;
                OnPropertyChanged("doInverseColors");
            }
        }

        #endregion --- doInverseColors ------- da li da invertuje boje

        #region --- blending ------- vrsta blendovanja

        private layerBlending _blending = layerBlending.transparent;

        /// <summary>
        /// vrsta blendovanja
        /// </summary>
        public layerBlending blending
        {
            get
            {
                return _blending;
            }
            set
            {
                _blending = value;
                OnPropertyChanged("blending");
            }
        }

        #endregion --- blending ------- vrsta blendovanja

        #region --- foreColor ------- boja slova

        private platformColorName _foreColor = platformColorName.none;

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

        private platformColorName _backColor = platformColorName.none;

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

        protected textContentBasicBlock(int __width, int __height, int __leftRightMargin, int __topBottomMargin, int __leftRightPadding, int __topBottomPadding) : base(__width, __height, __leftRightMargin, __topBottomMargin, __leftRightPadding, __topBottomPadding)
        {
            //     content = "";
        }

        protected textContentBasicBlock(int __width, int __leftRightMargin, int __leftRightPadding) : base(__width, __leftRightMargin, __leftRightPadding)
        {
            //   content = "";
        }

        /// <summary>
        /// Renderuje preko __content sadrzaja __textToInsert u skladu sa formatiranjem i printHorizontal podesavanjem.
        /// </summary>
        /// <param name="__content">Postojeci sadrzaj</param>
        /// <param name="__textToInsert">Sadrzaj koji treba da se insertuje</param>
        /// <param name="horizontal">TextAligment - mesto gde treba sadrzaj da se insertuje</param>
        /// <param name="__widthLimit">Ogranicenje duzine sadrzaja za ubacivanje. Ukoliko ostane -1 onda je limit innerWidth</param>
        /// <returns>Azurirani sadrzaj</returns>
        public virtual String writeInlineText(String __content, String __textToInsert, printHorizontal horizontal, Int32 __widthLimit = -1)
        {
            String output = __content;

            if (String.IsNullOrEmpty(output)) output = "";

            if (output.Length < width)
            {
                output = " ".Repeat(width);
            }

            if (__widthLimit == -1) __widthLimit = innerWidth;

            __textToInsert = __textToInsert.trimToWidth(__widthLimit, horizontal);

            Int32 insertLen = 0;
            switch (horizontal)
            {
                case printHorizontal.left:
                    insertLen = Math.Min(__textToInsert.Length, innerWidth);
                    output = output.overwrite(innerLeftPosition, insertLen, __textToInsert);
                    break;

                case printHorizontal.middle:
                    Int32 startPos = margin.left + padding.left + (innerWidth / 2);
                    startPos -= (__textToInsert.Length / 2);
                    insertLen = __textToInsert.Length;
                    output = output.overwrite(startPos, insertLen, __textToInsert);
                    break;

                case printHorizontal.right:
                    insertLen = __textToInsert.Length;

                    output = output.overwrite(innerRightPosition - insertLen, insertLen, __textToInsert);
                    break;

                default:
                    break;
            }
            return output;
        }

        /// <summary>
        /// Renderuje jednu liniju pozadine - preko prosledjenog sadrzaja
        /// </summary>
        /// <param name="__content"></param>
        /// <param name="marginDeco"></param>
        /// <param name="backgroundDeco"></param>
        /// <returns></returns>
        public virtual String writeInlineBackground(String __content, String marginDeco = "", String backgroundDeco = " ")
        {
            String output = __content;

            if (String.IsNullOrEmpty(output)) output = "";

            if (output.Length < width)
            {
                output = " ".Repeat(width);
            }

            if (!String.IsNullOrEmpty(marginDeco))
            {
                output = output.overwrite(0, margin.left, marginDeco);
                output = output.overwrite(margin.left + innerBoxedWidth, margin.right, marginDeco);
            }

            if (!String.IsNullOrEmpty(backgroundDeco))
            {
                output = output.overwrite(margin.left, innerWidth, backgroundDeco);
            }
            return output;
        }

        public abstract String[] getContent();

        /// <summary>
        /// Sadrzaj za jednolinijski blok ili linija na koju trenutno pokazuje kursor
        /// </summary>
        protected abstract String contentLine { get; set; }
    }
}