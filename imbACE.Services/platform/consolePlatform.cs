// --------------------------------------------------------------------------------------------------------------------
// <copyright file="consolePlatform.cs" company="imbVeles" >
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

namespace imbACE.Services.platform
{
    using imbACE.Core;
    using imbACE.Core.enums.platform;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.input;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.textBlocks.input;
    using imbSCI.Core.reporting.geometrics;
    using imbSCI.Core.reporting.zone;

    /// <summary>
    /// Platforma za konzolnu aplikaciju
    /// </summary>
    public class consolePlatform : platformBase, IPlatform, IPlatformBase
    {
        #region --- zone ------- referenca prema trenutnoj zoni delovanja

        private selectZone _zone;

        /// <summary>
        /// referenca prema trenutnoj zoni delovanja
        /// </summary>
        protected selectZone zone
        {
            get
            {
                if (!_zone.isDefined)
                {
                }
                return _zone;
            }
            set
            {
                _zone = value;
                OnPropertyChanged("zone");
            }
        }

        #endregion --- zone ------- referenca prema trenutnoj zoni delovanja

        public ConsoleColor convertColor(platformColorName color)
        {
            if (color == platformColorName.none)
            {
                return ConsoleColor.Black;
            }
            else
            {
            }

            String str = color.ToString();

            switch (color)
            {
                case platformColorName.Black:
                    break;
            }

            var cn = Enum.GetValues(typeof(ConsoleColor));
            foreach (ConsoleColor c in cn)
            {
                if (str.ToLower() == c.ToString().ToLower())
                {
                    return c;
                }
            }

            //ConsoleColor conColor = (ConsoleColor) Enum.Parse(typeof (ConsoleColor), color.ToString());

            return ConsoleColor.Magenta;
        }

        /// <summary>
        /// Primenjuje default podesavanja
        /// </summary>
        public override void deploySize()
        {
            height = Console.WindowHeight;
            width = Console.WindowWidth;
            padding = new fourSideSetting(0, 0);
            margin = new fourSideSetting(0, 0);
        }

        /// <summary>
        /// Sets the size of the window.
        /// </summary>
        /// <param name="preset">The preset.</param>
        /// <param name="doAutoDeploySize">if set to <c>true</c> [do automatic deploy size].</param>
        public void setWindowSize(cursorZoneSpatialPreset preset, Boolean doAutoDeploySize = true)
        {
            var spatials = preset.getPresetSpatialSettings();

            Console.SetWindowSize(spatials.width, spatials.height);

            if (doAutoDeploySize)
            {
                deploySize();
            }
        }

        /// <summary>
        /// Sets the size of the console application window.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="doAutoDeploySize">if set to <c>true</c> [do automatic deploy size].</param>
        public void setWindowSize(windowSize size, Boolean doAutoDeploySize = true)
        {
            Int32 w = 0;
            Int32 h = 0;

            switch (size)
            {
                case windowSize.fullscreenSize:

                    //Console.SetWindowPosition(0, 0);
                    w = Console.LargestWindowWidth;
                    h = Console.LargestWindowHeight;

                    // Console.SetWindowPosition(1, 1);
                    break;

                case windowSize.medium:
                    //Console.SetWindowSize(135, 80);
                    w = 135;
                    h = 60;
                    break;

                case windowSize.doubleSize:
                    //Console.SetWindowSize(170, 84);
                    w = 170;
                    h = 80;
                    break;

                case windowSize.defaultSize:
                    w = 85;
                    h = 43;
                    //Console.SetWindowSize(85, 43);
                    break;
            }

            if (w > Console.LargestWindowWidth) w = Console.LargestWindowWidth;
            if (h > Console.LargestWindowHeight) h = Console.LargestWindowHeight;

            Console.SetWindowSize(w, h);
            Console.SetWindowPosition(0, 0);

            if (doAutoDeploySize)
            {
                deploySize();
            }
        }

        /// <summary>
        /// Prikazuje prosledjeni sadrzaj na datim koordinatama
        /// </summary>
        /// <param name="content">Jednolinijski sadrzaj</param>
        /// <param name="x">X koordinata, ako je izostavljena onda je 0</param>
        /// <param name="y">Ako ostane -1 onda pise u trenutnom redu</param>
        public override void render(string content, int x = 0, int y = -1)
        {
            moveCursor(x, y);

            if (imbACECoreConfig.settings.doConsoleColorsByMarkupParshing)
            {
                consolePlatformExtensions.consoleWriteLine(content, false);
            }
            else
            {
                Console.Write(content);
            }
        }

        /// <summary>
        /// Renders the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void render(IEnumerable<string> content, int x = 0, int y = -1)
        {
            Int32 c = 0;
            Int32 yc = y;
            foreach (String cl in content)
            {
                if (y != -1)
                {
                    yc = y + c;
                }
                render(cl, x, yc);
                c++;
            }
            //  Console.Write(content);
        }

        // public void SetCursor()

        /// <summary>
        /// Brise ceo do sadasnji izlaz
        /// </summary>
        public override void clear()
        {
            Console.Clear();
            moveCursor(0, 0);
            //Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Beeps using <see cref="terminal.aceTerminalInput.doBeepViaConsole(int, int)"/> since it is OS safe
        /// </summary>
        /// <param name="frequency">u hz</param>
        /// <param name="duration">u mili sekundama</param>
        public override void beep(int frequency, int duration)
        {
            terminal.aceTerminalInput.doBeepViaConsole(frequency, duration);
            //Console.Beep(frequency, duration);
        }

        /// <summary>
        /// Postavlja naslov
        /// </summary>
        /// <param name="title">Naslov koji treba da se ispise</param>
        public override void title(string title)
        {
            Console.Title = title;
        }

        public void setColorsBack()
        {
            Console.ForegroundColor = lastForeColor;
            Console.BackgroundColor = lastBackColor;
            Console.ResetColor();
        }

        protected ConsoleColor lastForeColor;
        protected ConsoleColor lastBackColor;

        /// <summary>
        /// Postavlja boje za dalji izlaz
        /// </summary>
        /// <param name="foreColor">Prednja boja. Ako je .none onda resetuje boje</param>
        /// <param name="backColor">Boja pozadine</param>
        /// <param name="doInvert"> </param>
        public void setColors(platformColorName __foreColor, platformColorName __backColor, bool doInvert = false)
        {
            // __backColor = platformColorName.Blue;
            //__foreColor = platformColorName.Cyan;
            if (__backColor != platformColorName.DarkGray)
            {
            }

            if (__foreColor == platformColorName.none)
            {
                __foreColor = platformColorName.White;
                //
                //return;
            }

            if (__backColor == platformColorName.none)
            {
                __backColor = platformColorName.Black;
            }

            lastForeColor = Console.ForegroundColor;
            lastBackColor = Console.BackgroundColor;

            if (doInvert)
            {
                Console.ForegroundColor = convertColor(__backColor);
                Console.BackgroundColor = convertColor(__foreColor);
            }
            else
            {
                Console.ForegroundColor = convertColor(__foreColor);
                Console.BackgroundColor = convertColor(__backColor);
            }
        }

        ///// <summary>
        ///// Cita odredjenu zonu
        ///// </summary>
        ///// <param name="mode">Vrste citanja</param>
        ///// <param name="x">-1 koristi trenutni</param>
        ///// <param name="y">-1 koristi trenutni</param>
        ///// <param name="w">vise od 0 znaci citanje 1D ili 2D</param>
        ///// <param name="h">vise od 0 znaci citanje 2D bloka</param>
        ///// <returns>Popunjen input result</returns>
        //public override textInputResult read(textInputResult currentOutput, inputReadMode mode, int x = -1, int y = -1, int w = 0, int h = 0)
        //{
        //    selectZone zone = new selectZone(x, y, w, h);
        //    return read(currentOutput, mode, zone);
        //}

        /// <summary>
        /// Citanje pomocu zone objekta
        /// </summary>
        /// <param name="mode">Vrsta citanja</param>
        /// <param name="zone">Zona</param>
        /// <returns></returns>
        public override textInputResult read(textInputResult currentOutput, inputReadMode mode, selectZone zone, Object startValue = null)
        {
            if (currentOutput == null)
            {
                currentOutput = new textInputResult(this, mode, zone);
            }
            else
            {
                currentOutput.platform = this;
                currentOutput.readMode = mode;
                currentOutput.readZone = zone;
            }

            if (currentOutput.result == null)
            {
                currentOutput.result = startValue;
            }

            Console.CursorVisible = false;

            /// nije inplementirano zonsko citanje
            switch (mode)
            {
                case inputReadMode.readKey:
                    currentOutput.consoleKey = Console.ReadKey(true);

                    break;

                case inputReadMode.read:
                    Console.CursorVisible = true;
                    Console.SetCursorPosition(zone.x, zone.y);
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    currentOutput.result = consolePlatformExtensions.ReadLine(startValue, zone, currentOutput.result.GetType());

                    Console.CursorVisible = false;
                    Console.ResetColor();
                    break;

                case inputReadMode.readLine:
                    currentOutput.result = Console.ReadLine();
                    break;
            }

            return currentOutput;
        }

        /// <summary>
        /// Vraca dimenzije trenutnog prozora
        /// </summary>
        /// <returns></returns>
        public override selectRange getWindowSize()
        {
            selectRange output = new selectRange(Console.WindowWidth, Console.WindowHeight);
            return output;
        }

        /// <summary>
        /// Pomera kursor na zadatu koordinatu
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public override void moveCursor(int x = 0, int y = -1)
        {
            Console.CursorLeft = x;
            if (y > -1) Console.CursorTop = y;
        }
    }
}