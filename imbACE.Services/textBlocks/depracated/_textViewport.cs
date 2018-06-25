// --------------------------------------------------------------------------------------------------------------------
// <copyright file="_textViewport.cs" company="imbVeles" >
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
namespace imbACE.Services.textBlocks.depracated
{
    using imbACE.Core.core.exceptions;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public class _textViewport : _textContent
    {
        public _textViewport()
        {
            autoSetup();
        }

        public _textViewport(Int32 headerHeight, Int32 footerHeight)
        {
            y = headerHeight + 1;
            autoSetup();
            height -= footerHeight;

            autoPopulate();
            mode = viewportMode.showTopTrimBelow;
        }

        #region --- mode ------- Mod

        private viewportMode _mode = viewportMode.showTopTrimBelow;

        /// <summary>
        /// Mod
        /// </summary>
        public viewportMode mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
                OnPropertyChanged("mode");
            }
        }

        #endregion --- mode ------- Mod

        /// <summary>
        /// Vraca prikaz sadrzaja
        /// </summary>
        /// <returns></returns>
        public List<String> getView()
        {
            //textContent output = new textContent(width, height);
            List<string> scl = new List<string>();
            List<String> output = new List<string>();

            switch (mode)
            {
                case viewportMode.scrolled:
                    scl = GetRange(yScroll, Math.Max(height, Count));
                    break;

                case viewportMode.showAll:
                    scl.AddRange(this);
                    break;

                case viewportMode.showBottomTrimAbove:
                    if (Count < height)
                    {
                        scl = GetRange(0, Count);
                    }
                    else
                    {
                        Int32 _start = Count - height;
                        Int32 _count = Math.Min(height, (Count - _start));
                        scl = GetRange(_start, _count);
                    }

                    break;

                case viewportMode.showTopTrimBelow:
                    if (Count < height)
                    {
                        scl = GetRange(0, Count);
                    }
                    else
                    {
                        Int32 _start = 0;
                        Int32 _count = Math.Min(height, Count);
                        scl = GetRange(_start, _count);
                    }
                    break;
            }

            switch (mode)
            {
                case viewportMode.scrolled:
                    foreach (String ln in scl)
                    {
                        String lni = ln.Substring(xScroll, Math.Min(ln.Length, width));
                        output.Add(lni);
                    }
                    break;

                case viewportMode.showTopTrimBelow:
                case viewportMode.showBottomTrimAbove:
                    foreach (String ln in scl)
                    {
                        String lni = ln.Substring(0, Math.Min(ln.Length, width));
                        output.Add(lni);
                    }
                    break;

                default:
                    foreach (String ln in scl)
                    {
                        output.Add(ln);
                    }
                    break;
            }

            return output;
        }

        protected void autoSetup()
        {
            width = Console.WindowWidth;
            if (showHorizontalScroll)
            {
                width -= 2;
            }
            height = Console.WindowHeight;
            if (showVerticalScroll)
            {
                height -= 2;
            }
            height -= y;
        }

        /*

        #region --- width ------- sirina viewporta u kolonama

        private Int32 _width;
        /// <summary>
        /// sirina viewporta u kolonama
        /// </summary>
        public Int32 width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                OnPropertyChanged("width");
            }
        }

        #endregion --- width ------- sirina viewporta u kolonama

        #region --- height ------- visina viewporta u redovima

        private Int32 _height;
        /// <summary>
        /// visina viewporta u redovima
        /// </summary>
        public Int32 height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                OnPropertyChanged("height");
            }
        }

        #endregion --- height ------- visina viewporta u redovima

        */

        #region --- y ------- vertikalna pozicija viewporta

        private Int32 _y = 0;

        /// <summary>
        /// vertikalna pozicija viewporta
        /// </summary>
        public Int32 y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                OnPropertyChanged("y");
            }
        }

        #endregion --- y ------- vertikalna pozicija viewporta

        #region --- x ------- horizontalna pozicija viewporta

        private Int32 _x = 0;

        /// <summary>
        /// horizontalna pozicija viewporta
        /// </summary>
        public Int32 x
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                OnPropertyChanged("x");
            }
        }

        #endregion --- x ------- horizontalna pozicija viewporta

        #region --- doEnableScroll ------- da li moze scroll

        private Boolean _doEnableScroll = true;

        /// <summary>
        /// da li moze scroll
        /// </summary>
        public Boolean doEnableScroll
        {
            get
            {
                return _doEnableScroll;
            }
            set
            {
                _doEnableScroll = value;
                OnPropertyChanged("doEnableScroll");
            }
        }

        #endregion --- doEnableScroll ------- da li moze scroll

        #region --- xScroll ------- horizontalna scroll pozicija

        private Int32 _xScroll = 0;

        /// <summary>
        /// horizontalna scroll pozicija
        /// </summary>
        public Int32 xScroll
        {
            get
            {
                return _xScroll;
            }
            set
            {
                _xScroll = value;
                OnPropertyChanged("xScroll");
            }
        }

        #endregion --- xScroll ------- horizontalna scroll pozicija

        #region --- yScroll ------- vertikalna scroll pozicija

        private Int32 _yScroll = 0;

        /// <summary>
        /// vertikalna scroll pozicija
        /// </summary>
        public Int32 yScroll
        {
            get
            {
                return _yScroll;
            }
            set
            {
                _yScroll = value;
                OnPropertyChanged("yScroll");
            }
        }

        #endregion --- yScroll ------- vertikalna scroll pozicija

        #region --- showVerticalScroll ------- Da li da prikazuje vertikalni skrol

        private Boolean _showVerticalScroll = true;

        /// <summary>
        /// Da li da prikazuje vertikalni skrol
        /// </summary>
        public Boolean showVerticalScroll
        {
            get
            {
                return _showVerticalScroll;
            }
            set
            {
                _showVerticalScroll = value;
                OnPropertyChanged("showVerticalScroll");
            }
        }

        #endregion --- showVerticalScroll ------- Da li da prikazuje vertikalni skrol

        #region --- showHorizontalScroll ------- da li da pokazuje horizontalni skrol

        private Boolean _showHorizontalScroll = false;

        /// <summary>
        /// da li da pokazuje horizontalni skrol
        /// </summary>
        public Boolean showHorizontalScroll
        {
            get
            {
                return _showHorizontalScroll;
            }
            set
            {
                _showHorizontalScroll = value;
                OnPropertyChanged("showHorizontalScroll");
            }
        }

        #endregion --- showHorizontalScroll ------- da li da pokazuje horizontalni skrol

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}