// --------------------------------------------------------------------------------------------------------------------
// <copyright file="consolePlatformExtensions.cs" company="imbVeles" >
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
    using imbACE.Services.platform.core;
    using imbACE.Services.terminal;
    using imbACE.Services.textBlocks.core;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data.data.text;
    using imbSCI.Data.enums;
    using System.Text.RegularExpressions;

    public static class consolePlatformExtensions
    {
        //private static regexMarkerCollection<consoleStyleMarkerEnum> _consoleStyleMarkers;/
        //public static regexMarkerCollection<consoleStyleMarkerEnum> consoleStyleMarkers

        public static void consoleWriteLine(String line, Boolean breakLine = true)
        {
            regexMarkerResultCollection<consoleStyleMarkerEnum> result = consolePlatformExtensions.consoleStyleMarkers.process(line);
            ConsoleColor lastColor = Console.ForegroundColor;
            ConsoleColor lastBg = Console.BackgroundColor;

            foreach (regexMarkerResult res in result.GetByOrder())
            {
                if ((consoleStyleMarkerEnum)res.marker == result.defaultMarker)
                {
                    Console.ForegroundColor = lastColor;
                    Console.BackgroundColor = lastBg;
                }
                else
                {
                    regexMarkerForConsole regRule = consolePlatformExtensions.consoleStyleMarkers[(consoleStyleMarkerEnum)res.marker] as regexMarkerForConsole;

                    Console.ForegroundColor = regRule.foreground;
                    Console.BackgroundColor = regRule.background;
                }
                Console.Write(res.content);
            }
            if (breakLine) Console.WriteLine();

            Console.ForegroundColor = lastColor;
            Console.BackgroundColor = lastBg;
        }

        private static Object consoleStyleMarkersLock = new Object();

        private static regexMarkerCollectionForConsole _consoleStyleMarkers;

        public static regexMarkerCollectionForConsole consoleStyleMarkers
        {
            get
            {
                if (_consoleStyleMarkers == null)
                {
                    lock (consoleStyleMarkersLock)
                    {
                        if (_consoleStyleMarkers == null)
                        {
                            _consoleStyleMarkers = new regexMarkerCollectionForConsole();

                            _consoleStyleMarkers.Add(new regexMarkerForConsole(@"\*\*([\w\s\.\-\:\,\%\?\!\:\=\$]*)\*\*", consoleStyleMarkerEnum.doubleBolder, ConsoleColor.Red, ConsoleColor.Black));
                            _consoleStyleMarkers.Add(new regexMarkerForConsole(@"_([\w\s\.\-\:\,\%\?\!\:\=\$]*)_", consoleStyleMarkerEnum.highlight, ConsoleColor.Cyan, ConsoleColor.DarkGray));
                            _consoleStyleMarkers.Add(new regexMarkerForConsole(@"`([\w\s\.\-\:\,\%\?\!\:\=\$]*)`", consoleStyleMarkerEnum.darker, ConsoleColor.Gray, ConsoleColor.DarkGray));
                            _consoleStyleMarkers.Add(new regexMarkerForConsole(@"\*([\w\s\.\-\:\,\%\?\!\:\=\$]*)\*", consoleStyleMarkerEnum.bolder, ConsoleColor.DarkGray, ConsoleColor.White));
                        }
                    }
                }
                return _consoleStyleMarkers;
            }
        }

        public static List<String> GetBuffer()
        {
            List<String> output = new List<string>();
            Console.CursorTop = 0;
            for (int i = 0; i < Console.BufferHeight; i++)
            {
                String ln = "";
                Console.CursorTop = i;
                for (int x = 0; x < Console.BufferWidth; x++)
                {
                    //Console.CursorLeft = x;
                    ln += Console.Read();
                }
                output.Add(ln);
            }
            return output;
        }

        public static selectRange ClearAndPrint(this cursorZone zone, cursor c, String content)
        {
            selectRange before = null;

            before = zone.ClearZoneAndSet();

            Console.CursorLeft = zone.x;
            Console.CursorTop = zone.innerHeight;

            consoleWriteLine(content);

            Console.CursorLeft = zone.innerLeftPosition + c.x;
            Console.CursorTop = zone.innerHeight + c.y;

            return before;
        }

        /// <summary>
        /// Clears the zone and sets cursor at innerHeight position -- and returns cursor position before the call
        /// </summary>
        /// <param name="zone">The zone.</param>
        /// <returns></returns>
        public static selectRange ClearZoneAndSet(this cursorZone zone)
        {
            selectRange output = new selectRange(Console.CursorLeft, Console.CursorTop);

            selectRangeArea range = zone.selectRangeArea(textCursorZone.innerBoxedZone);

            Console.CursorLeft = zone.x;
            Console.CursorTop = zone.y;

            range.PrintRange(" ");

            return output;
        }

        public static void PrintRange(this selectRange range, String pattern = " ")
        {
            Console.CursorLeft = range.x;
            Console.CursorTop = range.y;

            for (int i = 0; i < range.y; i++)
            {
                Console.WriteLine(pattern.toWidthExact(range.x), pattern);
            }
        }

        public static Int32 localX(this selectZone zone)
        {
            return Console.CursorLeft - zone.x;
        }

        public static Int32 localY(this selectZone zone)
        {
            return Console.CursorTop - zone.y;
        }

        public static void toLocalX(this selectZone zone, Int32 x)
        {
            //x = zone.x + x;
            Console.CursorLeft = zone.x + x;
            zone.cursorToZone();
        }

        public static void toLocalY(this selectZone zone, Int32 y)
        {
            Console.CursorTop = zone.y + y;
            zone.cursorToZone();
            //return Console.CursorTop - zone.y;
            //Console.CursorTop = Console.CursorTop + zone.y;
        }

        public static void to(this selectZone zone, Int32 x = 0, Int32 y = 0)
        {
            Console.CursorTop = zone.y + y;
            Console.CursorLeft = zone.x + x;
            zone.cursorToZone();
            //return Console.CursorTop - zone.y;
            //Console.CursorTop = Console.CursorTop + zone.y;
        }

        public static void moveX(this selectZone zone, Int32 step = 1)
        {
            Console.CursorLeft = Console.CursorLeft + step;
            zone.cursorToZone();
        }

        public static void moveY(this selectZone zone, Int32 step = 1)
        {
            Console.CursorTop = Console.CursorTop + step;
            zone.cursorToZone();
        }

        /// <summary>
        /// Postavlja kursor unutar zone, vraca TRUE ako je morao da intervenise
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        public static Boolean cursorToZone(this selectZone zone)
        {
            Boolean output = false;
            if (Console.CursorLeft < zone.x)
            {
                Console.CursorLeft = zone.x;
                output = true;
            }

            if (Console.CursorLeft > zone.x + zone.weight)
            {
                Console.CursorLeft = zone.x + zone.weight;
                output = true;
            }

            if (Console.CursorTop < zone.y)
            {
                Console.CursorTop = zone.y;
                output = true;
            }

            if (Console.CursorTop > zone.y + zone.height)
            {
                Console.CursorTop = zone.y + zone.height;
                output = true;
            }
            return output;
        }

        public static string outputToZone(this selectZone zone, String output)
        {
            String cv = "";
            cv += output;
            cv = String.Format("{0,-" + zone.weight + "}", cv);
            return cv;
        }

        public enum readLineValidationMode
        {
            stringMode,

            /// <summary>
            /// Bilo koja celobrojna vrednost
            /// </summary>
            ordinalNumber,

            /// <summary>
            /// Bilo koja decimalna vrednost
            /// </summary>
            decimalNumber,
        }

        public static void reprint(List<char> chars, selectZone zone)
        {
            Int32 cl = Console.CursorLeft;
            Int32 ct = Console.CursorTop;

            Console.CursorLeft = zone.x;
            Console.CursorTop = zone.y;

            for (Int32 a = 0; a < zone.weight; a++)
            {
                Char c = ' ';
                if (chars.Count > a)
                {
                    c = chars[a];
                }
                Console.Write(c);
            }

            Console.CursorLeft = cl;
            Console.CursorTop = ct;
        }

        /// <summary>
        /// Cita liniju u skladu sa dodeljenom zonom delovanja
        /// </summary>
        /// <param name="val">Vrednost koja se edituje</param>
        /// <param name="zone">Zona editovanja</param>
        /// <param name="valType">Tip vrednosti</param>
        /// <returns>Nova vrednost ako ENTER ili stara vrednost ako ESC</returns>
        public static Object ReadLine(Object val, selectZone zone, Type valType)
        {
            String Default = val.toStringSafe("");

            readLineValidationMode valMode = readLineValidationMode.stringMode;
            if (valType == typeof(Int32))
            {
                valMode = readLineValidationMode.ordinalNumber;
                Default = val.ToString();
            }
            if (valType == typeof(Double))
            {
                valMode = readLineValidationMode.decimalNumber;
                Default = String.Format("N2", val.imbToNumber(typeof(Double)));
            }

            zone.to();

            //int pos = Console.CursorLeft;
            String cv = zone.outputToZone(Default);
            List<char> chars = new List<char>();
            if (string.IsNullOrEmpty(cv) == false)
            {
                chars.AddRange(cv.ToCharArray());
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.CursorVisible = true;

            reprint(chars, zone);
            //Console.Write(cv);

            //Console.CursorLeft = zone.x;
            //Console.CursorTop = zone.y;

            //Console.SetCursorPosition(zone.x, zone.y);

            ConsoleKeyInfo info;

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    info = Console.ReadKey();
                    Boolean allowChar = false;

                    switch (info.Key)
                    {
                        case ConsoleKey.Escape:
                            return val;
                            break;

                        case ConsoleKey.Enter:
                            String result = "";
                            foreach (char c in chars)
                            {
                                result = result + c;
                            }
                            result = result.Trim();
                            val = result.imbConvertValueSafe(valType);
                            return val;
                            break;

                        case ConsoleKey.Delete:
                            chars.RemoveAt(zone.localX());
                            //Console.CursorLeft -= 1;
                            //Console.Write(' ');
                            //Console.CursorLeft -= 1;

                            //chars.Add(' ');
                            break;

                        case ConsoleKey.End:
                            zone.toLocalX(1000);
                            // Console.CursorLeft = zone.x + zone.weight;
                            break;

                        case ConsoleKey.Home:
                            zone.toLocalX(0);
                            break;

                        case ConsoleKey.Backspace:
                            if (chars.Count > zone.localX())
                            {
                                chars.RemoveAt(zone.localX());
                                //Console.CursorLeft -= 1;
                                //Console.Write(' ');
                                //Console.CursorLeft += 1;

                                //chars.Add(' ');
                            }
                            //Console.CursorLeft -= 1;
                            break;

                        case ConsoleKey.LeftArrow:
                            zone.moveX(-2);
                            break;

                        case ConsoleKey.RightArrow:
                            //Console.CursorLeft += 1;
                            break;

                        case ConsoleKey.UpArrow:
                            //Console.CursorTop -= 1;
                            break;

                        case ConsoleKey.DownArrow:

                            //Console.CursorTop += 1;
                            break;

                        default:
                            switch (valMode)
                            {
                                case readLineValidationMode.stringMode:
                                    if (char.IsLetterOrDigit(info.KeyChar))
                                    {
                                        allowChar = true;
                                    }
                                    if (zone.localX() != 0)
                                    {
                                        allowChar = true;
                                    }
                                    else
                                    {
                                        if (char.IsSeparator(info.KeyChar))
                                        {
                                            allowChar = true;
                                        }
                                    }
                                    break;

                                case readLineValidationMode.decimalNumber:
                                    if (char.IsSymbol(info.KeyChar) && zone.localX() != 0)
                                    {
                                        allowChar = true;
                                    }
                                    else if (char.IsNumber(info.KeyChar))
                                    {
                                        allowChar = true;
                                    }
                                    else if (char.IsPunctuation(info.KeyChar))
                                    {
                                        allowChar = true;
                                    }
                                    break;

                                case readLineValidationMode.ordinalNumber:
                                    if (char.IsSymbol(info.KeyChar) && zone.localX() != 0)
                                    {
                                        allowChar = true;
                                    }
                                    else if (char.IsNumber(info.KeyChar))
                                    {
                                        allowChar = true;
                                    }
                                    break;
                            }
                            break;
                    }

                    zone.cursorToZone();

                    if (allowChar)
                    {
                        // Console.Write(info.KeyChar);
                        chars[zone.localX() - 1] = info.KeyChar;
                    }

                    reprint(chars, zone);
                }

                /*

                if (info.Key == ConsoleKey.Backspace && Console.CursorLeft > )
                {
                    chars.RemoveAt(chars.Count - 1);
                    Console.CursorLeft -= 1;
                    Console.Write(' ');
                    Console.CursorLeft -= 1;
                }
                else if (info.Key == ConsoleKey.Enter) { Console.Write(Environment.NewLine); break; }
                //Here you need create own checking of symbols
                else if (char.IsLetterOrDigit(info.KeyChar))
                {
                    Console.Write(info.KeyChar);
                    chars.Add(info.KeyChar);
                }*/
            }
            //return new string(chars.ToArray());
        }
    }
}