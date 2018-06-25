// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textCursor.cs" company="imbVeles" >
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
    using imbACE.Core.collection;
    using imbACE.Core.core.exceptions;
    using imbACE.Core.extensions;
    using imbACE.Services.platform.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.enums;
    using System;
    using System.ComponentModel;
    using System.Linq;

    /// <summary>
    /// Kursor
    /// </summary>
    public class textCursor : INotifyPropertyChanged
    {
        protected ISupportsBasicCursor target = null;

        protected textCursorMode mode = textCursorMode.fixedZone;

        #region --- currentZone -------

        private textCursorZone _currentZone = textCursorZone.innerZone;

        /// <summary>
        ///
        /// </summary>
        protected textCursorZone currentZone
        {
            get
            {
                return _currentZone;
            }
            set
            {
                if (mainZone == textCursorZone.unknownZone)
                {
                    if (_currentZone != textCursorZone.unknownZone)
                    {
                        //if (value != currentZone)
                        mainZone = _currentZone;
                    }
                    else
                    {
                    }
                }
                _currentZone = value;
                OnPropertyChanged("currentZone");
            }
        }

        #endregion --- currentZone -------

        #region --- valueReadZone ------- snimljena pozicija na ekranu odakle treba kasnije da se iscitava unos sa tastature

        private selectZone _valueReadZone;

        /// <summary>
        /// snimljena pozicija na ekranu odakle treba kasnije da se iscitava unos sa tastature
        /// </summary>
        public selectZone valueReadZone
        {
            get
            {
                if (!_valueReadZone.isDefined) _valueReadZone = autoValueReadZone();
                return _valueReadZone;
            }
            set
            {
                _valueReadZone = value;
                OnPropertyChanged("valueReadZone");
            }
        }

        #endregion --- valueReadZone ------- snimljena pozicija na ekranu odakle treba kasnije da se iscitava unos sa tastature

        /// <summary>
        /// Automatski pravi value read zonu na mestu gde je stao kursor
        /// </summary>
        /// <returns></returns>
        protected selectZone autoValueReadZone()
        {
            var sr = selectToCorner(textCursorZoneCorner.Right);
            selectZone output = new selectZone(x, y, sr.x, 1);
            return output;
        }

        /// <summary>
        /// Pravi selectZone na osnovu sadrzaja trenutne linije. Trazi [ i ] kao granicnike zone. Ako ih ima vise, upotrebice prvi par.
        /// </summary>
        /// <remarks>
        /// Ako u trenutnoj liniji:
        /// - ima vise [ ] parova: upotrebice prvi
        /// - je sve prazno ili popunjeno razmacima: upotrebice celu liniju kao zonu
        /// - ako je od x-a do desnog inner x-a prazno
        /// </remarks>
        /// <returns>selectZone objekat koji treba upotrebiti kao inputReadZone ili za nesto drugo</returns>
        public selectZone getSelectZone(Int32 zoneHeight = -1, Int32 zoneWidth = -1, params selectZoneOption[] option)
        {
            var optionList = option.ToList();
            selectZone output = new selectZone();
            output.y = y;

            if (zoneHeight == -1)
            {
                zoneHeight = 1;
                output.height = zoneHeight;
            }

            selectZoneOption takeOption = option.takeFirstFromList(selectZoneOption.takeDefinedWidth,
                                                      selectZoneOption.takeFromPositionToRightEnd,
                                                      selectZoneOption.takeBracetDefinedArea,
                                                      selectZoneOption.takeCompleteLine);

            if (takeOption == selectZoneOption.takeDefinedWidth)
            {
                if (zoneWidth == -1)
                {
                    takeOption = selectZoneOption.takeFromPositionToRightEnd;
                }
            }

            if (takeOption == selectZoneOption.takeBracetDefinedArea)
            {
                if (target is ISupportsCursorWriteAndSelect)
                {
                    ISupportsCursorWriteAndSelect target2 = target as ISupportsCursorWriteAndSelect;
                    String lastLine = target2.select(this, -1, true);
                    Int32 si = lastLine.IndexOf("[");

                    if (si == -1)
                    {
                        prevLine();
                        lastLine = target2.select(this, -1, true);
                        if (lastLine.IndexOf("[") == -1)
                        {
                            nextLine();
                        }
                        else
                        {
                            si = lastLine.IndexOf("[");
                        }
                    }

                    Int32 ln = lastLine.IndexOf("]") - si;
                    if (ln < 0)
                    {
                        takeOption = selectZoneOption.takeFromPositionToRightEnd;
                    }
                    else
                    {
                        output.y = y;
                        output.x = si;
                        output.weight = ln;
                    }
                }
            }

            switch (takeOption)
            {
                case selectZoneOption.takeFromPositionToRightEnd:
                    output.x = x;
                    output.weight = selectToCorner(textCursorZoneCorner.Right).x;
                    break;

                case selectZoneOption.takeCompleteLine:
                    output.x = target.innerLeftPosition;
                    output.weight = target.innerWidth;
                    break;
            }

            var moveOption = option.takeFirstFromList(selectZoneOption.none,
                                                      selectZoneOption.moveCursorToBottomEndOfZone,
                                                      selectZoneOption.moveCursorToBeginningOfZone);

            switch (moveOption)
            {
                case selectZoneOption.moveCursorToBeginningOfZone:
                    moveLineTo(output.y);
                    moveXTo(output.x);
                    break;

                case selectZoneOption.moveCursorToBottomEndOfZone:
                    moveLineTo(output.y + output.height);
                    break;
            }

            return output;
        }

        public textCursor(ISupportsBasicCursor __target, textCursorMode __mode, textCursorZone __zone)
        {
            target = __target;
            mode = __mode;
            currentZone = __zone;
        }

        /// <summary>
        /// Prebacuje kursor u datu zonu
        /// </summary>
        /// <param name="__zone"></param>
        /// <param name="__corner"></param>
        public void switchToZone(textCursorZone __zone, textCursorZoneCorner __corner = textCursorZoneCorner.default_corner)
        {
            currentZone = __zone;
            moveToCorner(__corner);
        }

        /// <summary>
        /// Vraca kursor na njegovu glavnu zonu
        /// </summary>
        public void switchToMainZone()
        {
            if (mainZone == textCursorZone.unknownZone)
            {
                Exception ex =
                    new aceGeneralException(
                        "Main zone je ostao nepoznat - nikada nije doslo do dodeljivanja nove vrednosti currentZone propertiju!");
                throw ex;
            }
            currentZone = mainZone;
        }

        /// <summary>
        /// zona koja je "home" za ovaj kursor - odnosno, zona za koju je ovaj kursor primarno namenjen i u koju se vraca nakon privremenih premestanja u druge zone metodom switchToZone()
        /// </summary>
        protected textCursorZone mainZone = textCursorZone.innerZone;

        /// <summary>
        /// Postavlja child objekat na poziciju kursora
        /// </summary>
        /// <param name="child"></param>
        public void placeChild(ISupportsTextCursor child)
        {
            child.margin.left = x;
            child.margin.top = y;

            Int32 dMLeft = x - child.innerBoxedLeftPosition;
            Int32 dMTop = y - child.innerBoxedTopPosition;

            child.width = target.width; // Math.Min(child.width + dMLeft, target.width - x);
            child.height = target.height; // Math.Min(child.height + dMTop, target.height - y);
        }

        /// <summary>
        /// Vraca dimenzije trenutne zone
        /// </summary>
        /// <returns></returns>
        public selectRange selectZone()
        {
            Int32 __x = x;
            Int32 __y = y;
            moveToCorner(textCursorZoneCorner.UpLeft);
            var res = selectToCorner(textCursorZoneCorner.DownRight);
            x = __x;
            y = __y;

            return res;
        }

        /// <summary>
        /// Vraca rastojanje izmedju trenutne pozicije kursora i datog kraja/coska
        /// </summary>
        /// <param name="__corner"></param>
        /// <returns></returns>
        public selectRange selectToCorner(textCursorZoneCorner __corner = textCursorZoneCorner.default_corner)
        {
            Int32 __x = x;
            Int32 __y = y;
            moveToCorner(__corner);
            Int32 _dX = x - __x;
            Int32 _dY = y - __y;
            selectRange res = new selectRange(_dX, _dY);
            x = __x;
            y = __y;

            return res;
        }

        /// <summary>
        /// Pomera kursor u dati ugao
        /// </summary>
        /// <param name="__corner"></param>
        public void moveToCorner(textCursorZoneCorner __corner = textCursorZoneCorner.default_corner)
        {
            switch (__corner)
            {
                case textCursorZoneCorner.Left:
                    x = 0;
                    break;

                case textCursorZoneCorner.Right:
                    x = target.width;
                    break;

                case textCursorZoneCorner.Top:
                    y = 0;
                    break;

                case textCursorZoneCorner.Bottom:
                    y = target.height;
                    break;

                case textCursorZoneCorner.UpLeft:
                    y = 0;
                    x = 0;
                    break;

                case textCursorZoneCorner.UpRight:
                    y = 0;
                    x = target.width;
                    break;

                case textCursorZoneCorner.DownLeft:
                    y = target.height;
                    x = 0;
                    break;

                case textCursorZoneCorner.DownRight:
                    y = target.height;
                    x = target.width;
                    break;
            }
            checkPositions();
        }

        /// <summary>
        /// proverava poziciju i primenjuje ogranicenje
        /// </summary>
        /// <returns></returns>
        protected Boolean checkPositions()
        {
            Boolean correctionMade = false;
            switch (currentZone)
            {
                case textCursorZone.innerZone:
                    if (y > target.innerBottomPosition)
                    {
                        y = target.innerBottomPosition;
                        correctionMade = true;
                    }
                    if (y < target.innerTopPosition)
                    {
                        y = target.innerTopPosition;
                        correctionMade = true;
                    }
                    if (x > target.innerRightPosition)
                    {
                        x = target.innerRightPosition;
                        correctionMade = true;
                    }
                    if (x < target.innerLeftPosition)
                    {
                        x = target.innerLeftPosition;
                        correctionMade = true;
                    }
                    break;

                case textCursorZone.innerBoxedZone:
                    if (y > target.innerBoxedBottomPosition)
                    {
                        y = target.innerBoxedBottomPosition;
                        correctionMade = true;
                    }
                    if (y < target.innerBoxedTopPosition)
                    {
                        y = target.innerBoxedTopPosition;
                        correctionMade = true;
                    }
                    if (x > target.innerBoxedRightPosition)
                    {
                        x = target.innerBoxedRightPosition;
                        correctionMade = true;
                    }
                    if (x < target.innerBoxedLeftPosition)
                    {
                        x = target.innerBoxedLeftPosition;
                        correctionMade = true;
                    }
                    break;

                case textCursorZone.outterZone:
                    if (y > target.height)
                    {
                        y = target.height;
                        correctionMade = true;
                    }
                    if (y < 0)
                    {
                        y = 0;
                        correctionMade = true;
                    }
                    if (x > target.width)
                    {
                        x = target.width;
                        correctionMade = true;
                    }
                    if (x < 0)
                    {
                        x = 0;
                        correctionMade = true;
                    }
                    break;
            }
            return correctionMade;
        }

        public void toTopLeftCorner()
        {
            x = -1;
            y = -1;
            checkPositions();
        }

        /// <summary>
        /// Pomera kursor na lokalnu liniju
        /// </summary>
        /// <param name="zoneLineNumber"></param>
        /// <returns></returns>
        public Boolean moveLineTo(Int32 zoneLineNumber)
        {
            Int32 tY = zoneLineNumber;
            switch (currentZone)
            {
                case textCursorZone.innerZone:
                    tY += target.innerTopPosition;
                    break;

                case textCursorZone.innerBoxedZone:
                    tY += target.innerBoxedTopPosition;
                    break;

                case textCursorZone.outterZone:
                    tY += 0;
                    break;
            }

            y = tY;
            return checkPositions();
        }

        public Boolean moveXTo(Int32 zoneXNumber)
        {
            Int32 tX = zoneXNumber;
            switch (currentZone)
            {
                case textCursorZone.innerZone:
                    tX += target.innerLeftPosition;
                    break;

                case textCursorZone.innerBoxedZone:
                    tX += target.innerBoxedLeftPosition;
                    break;

                case textCursorZone.outterZone:
                    tX += 0;
                    break;
            }

            x = tX;
            return checkPositions();
        }

        /// <summary>
        /// Prelazi u sledeci red i vraca se na X=0
        /// </summary>
        /// <returns></returns>
        public Boolean enter()
        {
            moveToCorner(textCursorZoneCorner.Left);
            return nextLine();
        }

        public Boolean nextLine(Int32 step = 1)
        {
            y = y + step;
            return checkPositions();
        }

        public Boolean prevLine(Int32 step = -1)
        {
            y = y + step;
            return checkPositions();
        }

        public Boolean tabNext()
        {
            if (x < tabPosition(printHorizontal.left))
            {
                x = tabPosition(printHorizontal.left);
            }
            else if (x < tabPosition(printHorizontal.middle))
            {
                x = tabPosition(printHorizontal.middle);
            }
            else if (x < tabPosition(printHorizontal.right))
            {
                x = tabPosition(printHorizontal.right);
            }
            return checkPositions();
        }

        public Int32 tabPosition(printHorizontal field)
        {
            ISupportsTextCursor target_e = target as ISupportsTextCursor;
            switch (field)
            {
                case printHorizontal.left:
                    return target.innerLeftPosition;

                    break;

                case printHorizontal.middle:
                    if (target_e != null)
                    {
                        return target_e.leftFieldWidth + target.innerLeftPosition;
                    }
                    break;

                case printHorizontal.right:
                    if (target_e != null)
                    {
                        return target.innerRightPosition - target_e.rightFieldWidth;
                    }
                    break;
            }

            return 0;
        }

        public Boolean tab(printHorizontal field)
        {
            x = tabPosition(field);
            return checkPositions();
        }

        #region --- x ------- X pozicija kursora

        private Int32 _x = 0;

        /// <summary>
        /// X pozicija kursora
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

        #endregion --- x ------- X pozicija kursora

        #region --- y ------- pozicija po liniji

        private Int32 _y;

        /// <summary>
        /// pozicija po liniji
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

        #endregion --- y ------- pozicija po liniji

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}