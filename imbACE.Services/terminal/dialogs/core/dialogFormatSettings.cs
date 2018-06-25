// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dialogFormatSettings.cs" company="imbVeles" >
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
namespace imbACE.Services.terminal.dialogs.core
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.enums.platform;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.textBlocks.enums;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Data.enums;
    using System;
    using System.ComponentModel;

    public class dialogFormatSettings : INotifyPropertyChanged
    {
        public dialogFormatSettings(dialogStyle __style, dialogSize __size)
        {
            __deploy(__style, __size);
        }

        public void apply(dialogScreenBase dialog, IPlatform platform)
        {
            dialog.padding = new fourSideSetting(2, 2);
            dialog.blending = layerBlending.transparent;
            dialog.width = platform.width;
            dialog.height = platform.height;

            fourSideSetting margin = new fourSideSetting(10);
            switch (size)
            {
                case dialogSize.messageBox:

                    margin.top = 5;
                    margin.left = 5;
                    margin.right = 5;
                    break;

                case dialogSize.mediumBox:
                    margin.top = 2;
                    //dialog.height = 16;
                    margin.left = 5;
                    margin.right = 5;
                    break;

                case dialogSize.fullScreenBox:
                    margin.top = 0;
                    //dialog.height = 22;
                    margin.left = 5;
                    margin.right = 5;
                    break;
            }

            switch (style)
            {
                case dialogStyle.blueDialog:
                    dialog.foreColor = platformColorName.Cyan;
                    dialog.backColor = platformColorName.DarkBlue;
                    dialog.marginDecoration = "#";
                    break;

                case dialogStyle.greenDialog:
                    dialog.foreColor = platformColorName.White;
                    dialog.backColor = platformColorName.DarkGreen;
                    dialog.marginDecoration = "#";
                    break;

                default:
                case dialogStyle.redDialog:
                    dialog.foreColor = platformColorName.White;
                    dialog.backColor = platformColorName.Red;
                    dialog.marginDecoration = "#";
                    break;
            }

            dialog.margin = margin;

            //dialog.margin = new imbACE.Core.primitives.fourSideSetting(margin, platform.height - dialog.height);
        }

        protected void __deploy(dialogStyle __style, dialogSize __size)
        {
            horizontalAligment = printHorizontal.middle;
            verticalAlligment = printVertical.center;
            style = __style;
            size = __size;
        }

        #region --- horizontalAligment ------- horizontalnoPozicioniranje

        private printHorizontal _horizontalAligment = printHorizontal.middle;

        /// <summary>
        /// horizontalnoPozicioniranje
        /// </summary>
        public printHorizontal horizontalAligment
        {
            get
            {
                return _horizontalAligment;
            }
            set
            {
                _horizontalAligment = value;
                OnPropertyChanged("horizontalAligment");
            }
        }

        #endregion --- horizontalAligment ------- horizontalnoPozicioniranje

        #region --- verticalAlligment ------- vertikalnoPozicioniranje

        private printVertical _verticalAlligment = printVertical.center;

        /// <summary>
        /// vertikalnoPozicioniranje
        /// </summary>
        public printVertical verticalAlligment
        {
            get
            {
                return _verticalAlligment;
            }
            set
            {
                _verticalAlligment = value;
                OnPropertyChanged("verticalAlligment");
            }
        }

        #endregion --- verticalAlligment ------- vertikalnoPozicioniranje

        #region --- style ------- stil dijaloga

        private dialogStyle _style = dialogStyle.greenDialog;

        /// <summary>
        /// stil dijaloga
        /// </summary>
        public dialogStyle style
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
                OnPropertyChanged("style");
            }
        }

        #endregion --- style ------- stil dijaloga

        #region --- size ------- velicina dijaloga

        private dialogSize _size;

        /// <summary>
        /// velicina dijaloga
        /// </summary>
        public dialogSize size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                OnPropertyChanged("size");
            }
        }

        #endregion --- size ------- velicina dijaloga

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}