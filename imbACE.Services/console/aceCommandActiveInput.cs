// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceCommandActiveInput.cs" company="imbVeles" >
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

namespace imbACE.Services.console
{
    using imbACE.Core.commands.tree;
    using imbACE.Services.platform;
    using imbSCI.Core.reporting.zone;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    /// <summary>
    /// ---- this is abanded idea
    /// </summary>
    public class aceCommandActiveInput
    {
        public commandTree comTree { get; set; }
        public String prefix { get; set; } = "_>_";
        public List<String> history { get; set; }
        public Boolean active { get; set; }
        private Int32 cursorSize = 1;

        protected cursorZone zone { get; set; }
        protected cursor c { get; set; }

        public String current { get; set; } = "";
        public String currentOutput { get; set; } = "";

        public ConsoleKey specialCall { get; set; } = ConsoleKey.NoName;

        public Int32 historyIndex = 0;

        public aceCommandActiveInput(commandTree __comTree, List<String> __history, String __current, String __prefix = "")
        {
            comTree = __comTree;
            prefix = __prefix.or("_>_");
            history = __history;
            historyIndex = history.Count();

            zone = new cursorZone(80, 1, 1);
            zone.padding.top = 1;
            zone.padding.bottom = 1;
            zone.margin.top = Console.WindowHeight - zone.height;
            zone.margin.left = prefix.Length;

            c = new cursor(zone, textCursorMode.fixedZone, textCursorZone.innerZone, "themeCursor");

            c.switchToZone(textCursorZone.innerZone);
            active = true;

            current = __current;
        }

        public void doRedraw()
        {
            zone.y = Console.WindowHeight - zone.height;

            currentOutput = prefix.toWidthExact(zone.innerLeftPosition, " ") + current.toWidthExact(zone.outerRightPosition, " ");

            zone.ClearAndPrint(c, currentOutput);

            cursorSize += 10;
            if (cursorSize > 100) cursorSize = 1;

            Console.CursorSize = cursorSize;
        }

        public void run()
        {
            Console.CursorVisible = true;

            //ConsoleKey output = ConsoleKey.NoName; // = new Boolean();

            zone = new cursorZone(Console.BufferWidth, 1, 0);

            //Console.WriteLine();

            DateTime startTime = DateTime.Now;
            DateTime lastReminder = DateTime.Now;

            doRedraw();

            while (active)
            {
                String input = "";

                Boolean redraw = false;

                if (Console.KeyAvailable == false)
                {
                    Thread.Sleep(100);
                    continue;
                }

                if (Console.KeyAvailable)
                {
                    redraw = true;
                    ConsoleKeyInfo cki = Console.ReadKey(false);

                    if (cki.Modifiers.HasFlag(ConsoleModifiers.Alt)) input = "ALT+";
                    if (cki.Modifiers.HasFlag(ConsoleModifiers.Control)) input = "CTRL+";
                    if (cki.Modifiers.HasFlag(ConsoleModifiers.Shift)) input = "SHIFT+";

                    input += cki.Key.ToString();

                    switch (cki.Key)
                    {
                        case ConsoleKey.F1:
                        case ConsoleKey.F2:
                        case ConsoleKey.F3:
                        case ConsoleKey.F4:
                        case ConsoleKey.F5:
                        case ConsoleKey.F6:
                        case ConsoleKey.F7:
                        case ConsoleKey.F8:
                        case ConsoleKey.F9:
                        case ConsoleKey.F10:
                        case ConsoleKey.F11:
                        case ConsoleKey.F12:
                        case ConsoleKey.Escape:
                            current = input + cki.Key.ToString();
                            specialCall = cki.Key;
                            active = false;
                            break;

                        case ConsoleKey.DownArrow:
                            if (historyIndex > 0) historyIndex--;

                            break;

                        case ConsoleKey.UpArrow:
                            if (historyIndex < history.Count()) historyIndex--;
                            break;

                        case ConsoleKey.Tab:
                            break;

                        case ConsoleKey.Enter:
                            active = false;
                            break;

                        case ConsoleKey.Home:
                            c.x = 0;
                            break;

                        case ConsoleKey.End:
                            c.moveTo(current.Length, c.y);
                            break;

                        case ConsoleKey.LeftArrow:
                            c.prev();
                            break;

                        case ConsoleKey.RightArrow:
                            c.next();

                            break;

                        case ConsoleKey.Delete:
                            if (c.x < current.Length)
                            {
                                current = current.Remove(c.x, 1);
                            }
                            break;

                        case ConsoleKey.Backspace:
                            c.prev(1);

                            if (c.x < current.Length)
                            {
                                current = current.Remove(c.x, 1);
                            }

                            break;

                        case ConsoleKey.Spacebar:
                        default:
                            if (cki.KeyChar != char.MinValue)
                            {
                                if (c.x < current.Length)
                                {
                                    current = current.Insert(c.x, cki.KeyChar.ToString());
                                }
                                else
                                {
                                    current = current + cki.KeyChar.ToString();
                                }
                                c.next(false);
                            }
                            break;
                    }
                }
                else
                {
                    if (startTime.Subtract(lastReminder).TotalSeconds > 5)
                    {
                        lastReminder = DateTime.Now;
                        redraw = true;
                    }
                    else
                    {
                        //Thread.Sleep(500);
                    }
                }

                if (redraw)
                {
                    doRedraw();
                    // Loop until input is entered.
                }
            }

            zone.ClearZoneAndSet();
            Console.CursorLeft = 0;
        }

        public String response { get; set; } = "";
        public Boolean hasResponse { get; set; } = false;
    }
}