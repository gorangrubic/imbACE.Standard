// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceTerminalInput.cs" company="imbVeles" >
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
using imbACE.Core.core.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbACE.Services.terminal
{
    using imbACE.Core.data;
    using imbACE.Core.interfaces;
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.math.range;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.colors;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data;
    using System.Collections;
    using System.ComponentModel;
    using System.Reflection;
    using System.Threading;

    /// <summary>
    /// Klasa za prikupljanje ulaza preko terminala (command line aplikacija)
    /// </summary>
    public static class aceTerminalInput
    {
        /// <summary>
        /// VRaca True ako treba ponovo da se prikaze
        /// </summary>
        /// <param name="console"></param>
        /// <param name="collection"></param>
        /// <param name="collectionName"></param>
        /// <param name="itemNameProperty"></param>
        /// <param name="addItem"></param>
        /// <param name="deleteItem"></param>
        /// <param name="editItem"></param>
        /// <param name="editCollectionSettings"></param>
        /// <returns></returns>
        public static Boolean editCollection(aceTerminal console, IList collection, String collectionName, String itemNameProperty, Boolean addItem, Boolean deleteItem, Boolean editItem, Boolean editCollectionSettings)
        {
            /*
            aceTerminalMenu menu = new aceTerminalMenu();
            menu.menuTitle = "Configure & edit [" + collectionName + "] ";

            Type genType = null;
            String genTypeName = "";
            Int32 c = 0;
            PropertyInfo pi = null;
            foreach (Object colItem in collection)
            {
                if (pi == null) pi = colItem.GetType().GetProperty(itemNameProperty);
                String itoName = "item["+c.ToString()+"]->"+itemNameProperty+"="+ pi.GetValue(colItem, new object[0]).ToString();
                var ito = menu.setItem(itoName, "", "");
                ito.index = c;
                ito.metaObject = colItem;
                ito.metaStringData = itoName;
                c++;
            }

            if (pi != null)
            {
                genType = pi.DeclaringType;
                genTypeName = genType.Name;
            } else
            {
                Type[] gt = collection.GetType().GetGenericArguments();

                if (gt.Any())
                {
                    genType = gt[0];
                    genTypeName = genType.Name;
                }
            }
            menu.menuDescription = "Contains (" + collection.Count + ") items ";
            if (genTypeName != "") menu.menuDescription += " of type " + genTypeName;

            if (editCollectionSettings) menu.setItem(collectionName + " settings", "", "S", true);

            if (addItem) menu.setItem("Add new item", "", "A");
            menu.setItem("Quit", "", "Q");

            aceMenuItem selected = menu.showMenu(console, aceMenuItemRendering.numberOrKey, true, true);
            menu.metaInt = -1;
            Object selectedItem = null;
            switch (selected.key)
            {
                case "S":

                    aceTerminalInput.editSettings(collection, "settings", collectionName);
                    break;

                case "A":
                    Object newItem = genType.TypeInitializer.Invoke(new object[0]);
                    collection.Add(newItem);
                    menu.metaInt = collection.IndexOf(newItem);
                    selectedItem = newItem;
                    break;

                case "Q":
                    return false;
                    break;

                default:
                    if (selected.metaObject != null)
                    {
                        menu.metaInt = collection.IndexOf(selected.metaObject);
                        selectedItem = selected.metaObject;
                    }
                    break;
            }

            if (menu.metaInt > 0)
            {
                aceTerminalMenu itemMenu = new aceTerminalMenu();
                itemMenu.menuTitle = "Selected item [" + menu.metaInt + "] ";

                if (pi == null) pi = selectedItem.GetType().GetProperty(itemNameProperty);

                if (pi != null)
                {
                     itemMenu.menuDescription = "[" + menu.metaInt + "]:" + pi.GetValue(selectedItem, new object[0]).ToString();
                } else
                {
                    itemMenu.menuDescription = "[" + menu.metaInt + "] ";
                }

                if (deleteItem) itemMenu.setItem("Delete item", "", "D");
                if (editItem) itemMenu.setItem("Edit item", "", "E", true);
                itemMenu.setItem("Close item", "", "Q");

                var selectedItemMenu = itemMenu.showMenu(console, aceMenuItemRendering.numberOrKey, true, true);
                switch (selectedItemMenu.key)
                {
                    case "E":
                        aceTerminalInput.editSettings(selectedItem, menu.metaInt.ToString(), collectionName);
                        return true;
                        break;

                    case "D":
                        if (collection.Contains(selectedItem))
                        {
                            collection.Remove(selectedItem);
                        }
                        return true;
                        break;

                    default:
                        return true;
                        break;
                }
            }
            */
            return false;
        }

        public static Object AskFor(settingsPropertyEntry prop, Object propValue)
        {
            Object propNewValue = null;

            String prompt = "";

            if (!prop.prompt.isNullOrEmptyString())
            {
                prompt = prop.prompt;
            }

            if (prop.type.IsEnum)
            {
                propNewValue = askForOption(prompt.or("Select value from list below"), propValue);
            }
            else
            {
                switch (prop.type.Name.ToLower())
                {
                    case "string":
                        propNewValue = askForStringInline(prompt.or("Insert new value"), propValue as String);
                        break;

                    case "decimal":
                        String tmpString = askForStringInline(prompt.or("Insert new value"), propValue.ToString());
                        Decimal decimalResult;
                        if (Decimal.TryParse(tmpString, out decimalResult))
                        {
                            propNewValue = decimalResult;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input - default value set: " + propValue.ToString());
                            propNewValue = propValue;
                        }
                        break;

                    case "int32":
                        String tmpStringInt = askForStringInline(prompt.or("Insert new value"), propValue.ToString());
                        Int32 intResult;
                        if (Int32.TryParse(tmpStringInt, out intResult))
                        {
                            propNewValue = intResult;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input - default value set: " + propValue.ToString());
                            propNewValue = propValue;
                        }
                        break;

                    case "bool":
                    case "boolean":
                        propNewValue = askYesNo(prompt.or("Select new value"), (Boolean)propValue);
                        break;
                }
            }
            return propNewValue;
        }

        /// <summary>
        /// Podesava vrednosti celog objekta - bez nasledjenih
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <param name="parentName">Name of the parent.</param>
        /// <param name="loger">The loger.</param>
        public static void editSettings(Object settings, String instanceName = "", String parentName = "", ILogBuilder loger = null)
        {
            Type settingsType = settings.GetType();
            PropertyInfo[] props = settingsType.GetProperties();

            List<PropertyInfo> propList = new List<PropertyInfo>();
            foreach (PropertyInfo pro in props)
            {
                if (pro.CanWrite)
                {
                    if (pro.PropertyType.IsEnum)
                    {
                        propList.Add(pro);
                    }
                    else if (pro.PropertyType.IsPrimitive)
                    {
                        propList.Add(pro);
                    }
                    else if (pro.PropertyType == typeof(String))
                    {
                        propList.Add(pro);
                    }
                    else
                    {
                        if (pro.DeclaringType == settingsType)
                        {
                            if (pro.Name != "Item")
                            {
                                propList.Add(pro);
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }

            props = propList.ToArray();

            String titleLine = "";
            if (String.IsNullOrEmpty(instanceName))
            {
                titleLine = "Variables [" + props.Length + "] in an instance of [" + settingsType.Name + "].";
            }
            else
            {
                if (String.IsNullOrEmpty(parentName))
                {
                    titleLine = "Variables [" + props.Length + "] in [" + parentName + "->" + instanceName + "] of [" + settingsType.Name + "].";
                }
                else
                {
                    titleLine = "Variables [" + props.Length + "] in [" + instanceName + "] of [" + settingsType.Name + "].";
                }
            }
            Console.WriteLine(titleLine);

            Console.WriteLine();

            builderForMarkdown input_builderForMarkdown = (builderForMarkdown)loger;

            if (input_builderForMarkdown != null)
            {
                input_builderForMarkdown.rootTabLevel();

                input_builderForMarkdown.AppendHeading(titleLine, 1);

                input_builderForMarkdown.AppendTableRow(acePaletteVariationRole.header, "Caption", "Property", "Value", "New value", "Description");
            }

            foreach (PropertyInfo prop in props)
            {
                List<String> msgOut = new List<string>();
                DescriptionAttribute descAttribute;
                DisplayNameAttribute displayNameAttribute;

                String description = "";
                String displayName = "";

                Object[] propAttributes = prop.GetCustomAttributes(false);

                foreach (Object propAtt in propAttributes)
                {
                    descAttribute = propAtt as DescriptionAttribute;
                    if (descAttribute != null)
                    {
                        description = descAttribute.Description;
                    }

                    displayNameAttribute = propAtt as DisplayNameAttribute;
                    if (displayNameAttribute != null)
                    {
                        displayName = displayNameAttribute.DisplayName;
                    }
                }

                Object propValue = null;

                propValue = prop.GetValue(settings, null);
                Object propNewValue = new Object();

                Console.WriteLine();

                Console.WriteLine("" + displayName + " (" + prop.Name + ") = " + propValue.ToString());
                Console.WriteLine("--  " + description);

                String oldValue = propValue.ToString();

                propNewValue = null;

                if (prop.PropertyType.IsEnum)
                {
                    propNewValue = askForOption("Select value from list below", propValue);
                }
                else
                {
                    switch (prop.PropertyType.Name.ToLower())
                    {
                        case "string":
                            propNewValue = askForStringInline("Insert new value", propValue as String);
                            break;

                        case "decimal":
                            String tmpString = askForStringInline("Insert new value", propValue.ToString());
                            Decimal decimalResult;
                            if (Decimal.TryParse(tmpString, out decimalResult))
                            {
                                propNewValue = decimalResult;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input - default value set: " + propValue.ToString());
                                propNewValue = propValue;
                            }
                            break;

                        case "int32":
                            String tmpStringInt = askForStringInline("Insert new value", propValue.ToString());
                            Int32 intResult;
                            if (Int32.TryParse(tmpStringInt, out intResult))
                            {
                                propNewValue = intResult;
                            }
                            else
                            {
                                Console.WriteLine("Invalid input - default value set: " + propValue.ToString());
                                propNewValue = propValue;
                            }
                            break;

                        case "bool":
                        case "boolean":
                            propNewValue = askYesNo("Select new value", (Boolean)propValue);
                            break;
                    }
                }
                if (propNewValue != null)
                {
                    prop.SetValue(settings, propNewValue, new object[0]);
                    Console.WriteLine("Value set: " + propNewValue.ToString());
                }
                else
                {
                    Console.WriteLine("Skipping parameter: " + prop.Name + ":" + prop.PropertyType.Name);
                }

                input_builderForMarkdown.AppendTableRow(acePaletteVariationRole.none, displayName, prop.Name, oldValue, propNewValue.ToString(), description);

                Console.WriteLine();
            }
        }

        /// <summary>
        /// Asks for enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="question">The question.</param>
        /// <param name="defans">The defans.</param>
        /// <param name="loger">The loger.</param>
        /// <returns></returns>
        public static T askForEnum<T>(String question, T defans, ILogBuilder loger = null)
        {
            return (T)askForOption(question, defans, loger);
        }

        public static Object askForOption(String question, Object defans, IList options, ILogBuilder loger = null)
        {
            Object output = defans;

            String helpLine = "Leave blank for default value (" + defans.ToString() + ")";

            Console.WriteLine(question);
            Console.WriteLine(helpLine);
            List<String> msgOut = new List<string>();

            Int32 c = 0;
            foreach (String op in options)
            {
                String msg = " [" + c.ToString() + "] " + op + "";
                if (op == defans.ToString())
                {
                    msg += " (default)";
                }
                Console.WriteLine(msg);
                msgOut.Add(msg);
                c++;
            }

            String input = Console.ReadLine();

            input = input.Trim();

            if (String.IsNullOrEmpty(input))
            {
                return defans;
            }

            Int32 ninput = 0;

            if (Int32.TryParse(input, out ninput))
            {
                output = options[ninput];
            }
            else
            {
                Console.WriteLine("--- invalid input ---");
                return askForOption(question, defans, options, loger);
            }

            if (loger != null)
            {
                loger.AppendSection(helpLine, question, "User response: " + output, msgOut);
            }

            return output;
        }

        /// <summary>
        /// Asks for option.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="defans">The defans.</param>
        /// <returns></returns>
        public static Object askForOption(String question, Object defans, ILogBuilder loger = null)
        {
            Object output = defans;

            List<String> options = new List<string>();
            options.AddRange(Enum.GetNames(output.GetType()));

            output = askForOption(question, defans, options, loger);

            //output = Enum.Parse(output.GetType(), options[ninput]);

            output = Enum.Parse(defans.GetType(), output.toStringSafe());

            return output;
        }

        /*
        /// <summary>
        /// Pita za vrednost opcije
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="question"></param>
        /// <param name="defans"></param>
        /// <returns></returns>
        public static T askForOption<T>(String question, T defans) where T:class
        {
            T output = defans;
        }*/

        /// <summary>
        /// Asks for string input - in inline mode
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="defans">Default answer</param>
        /// <param name="loger">The loger.</param>
        /// <returns></returns>
        public static String askForStringInline(String question, String defans, ILogBuilder loger = null)
        {
            Console.WriteLine();

            Console.CursorVisible = true;

            String line = question;
            if (!String.IsNullOrEmpty(defans))
            {
                line += " (enter for [" + defans + "])";
            }
            line += " : ";
            Console.Write(line);
            String output = Console.ReadLine();
            if (String.IsNullOrEmpty(output))
            {
                output = defans;
            }

            if (loger != null)
            {
                loger.AppendSection(line, question, "User response: " + output);
            }

            Console.CursorVisible = false;
            return output;
        }

        /// <summary>
        /// Asks user to enter typed (numeric) value
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="defans">The defans.</param>
        /// <param name="loger">The loger.</param>
        /// <param name="range">The range.</param>
        /// <param name="defFormat">The definition format.</param>
        /// <param name="autoretry">if set to <c>true</c> [autoretry].</param>
        /// <param name="retryLimit">The retry limit.</param>
        /// <returns></returns>
        public static T askForTypedValue<T>(String question, T defans, ILogBuilder loger = null, rangeValueBase<T> range = null, String defFormat = "F2", Boolean autoretry = true, Int32 retryLimit = 5) where T : IComparable
        {
            Int32 retry = 0;
            while (autoretry)
            {
                try
                {
                    String q = question;
                    if (range != null)
                    {
                        q = q.add(" (range: " + range.min.ToString() + " - " + range.max.ToString() + ") ");
                    }
                    if (retry > 0) { q = q.add(" - retry: " + retry); }

                    T l = imbACE.Services.terminal.aceTerminalInput.askForString(q, defans.ToString(), loger).imbConvertValueSafeTyped<T>();
                    autoretry = false;
                    return l;
                }
                catch (Exception ex)
                {
                    retry++;
                    imbACE.Services.aceCommons.log(ex.Message);
                    doBeepViaConsole(2400, 500, 1);
                    if (retry >= retryLimit)
                    {
                        autoretry = false;
                    }
                }
            }
            return defans;
        }

        /// <summary>
        /// Asks for string.
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="defans">The defans.</param>
        /// <param name="loger">The loger.</param>
        /// <returns></returns>
        public static String askForString(String question, String defans, ILogBuilder loger = null)
        {
            String output = defans;
            Console.CursorVisible = true;
            String helpLine = "[enter] to leave current";

            Console.WriteLine(question);

            Console.WriteLine(helpLine);
            Console.WriteLine();

            String input = Console.ReadLine();

            if (loger != null)
            {
                loger.AppendSection(helpLine, question, "User response: " + input);
            }

            if (String.IsNullOrWhiteSpace(input))
            {
                return defans;
            }
            else
            {
                return input;
            }
            Console.CursorVisible = false;
            return output;
        }

        /// <summary>
        /// Postavlja Y/N pitanje korisniku. Ako pritisne neorgovarajuc taster - postavice pitanje ponovo
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="defans">if set to <c>true</c> [defans].</param>
        /// <returns>TRUE on yes</returns>
        public static Boolean askYesNo(String question, Boolean defans = true, ILogBuilder loger = null)
        {
            Boolean output = defans; // = new Boolean();

            String helpLine = "[Y] yes    [N] no      [enter] default";
            if (defans)
            {
                helpLine += " (default is Y)";
            }
            else
            {
                helpLine += " (default is N)";
            }

            Console.WriteLine(question);

            Console.WriteLine(helpLine);
            Console.WriteLine();

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Console.WriteLine(keyInfo.Key.ToString());

            if (loger != null)
            {
                loger.AppendSection(helpLine, question, "User response: " + keyInfo.Key.ToString());
            }
            switch (keyInfo.Key)
            {
                case ConsoleKey.Y:
                    return true;
                    break;

                case ConsoleKey.Enter:
                    return defans;
                    break;

                case ConsoleKey.N:
                    return false;
                    break;

                default:
                    return askYesNo(question, defans);
                    break;
            }

            return output;
        }

        /// <summary>
        /// Does the beep via <see cref="Console.Beep(int, int)"/> returns true if OS supported it, <c>false</c> if the OS has no beep support (i.e. WinXP and Vista 64bit)
        /// </summary>
        /// <param name="hz">The hz.</param>
        /// <param name="ms">The ms.</param>
        /// <returns></returns>
        public static Boolean doBeepViaConsole(Int32 hz = 800, Int32 ms = 200, Int32 repeat = 1)
        {
            try
            {
                for (int i = 0; i < repeat; i++)
                {
                    Console.Beep(hz, ms);
                    if (repeat > 1)
                    {
                        Thread.Sleep(ms);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("-- beep not supported by OS: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Asks user to confirm action by pressing any button except Escape.
        /// </summary>
        /// <param name="question">The question/message to show</param>
        /// <param name="defAns">Boolean result for user inaction, it will return opposite value if something was pressed.</param>
        /// <param name="timeout">In seconds, time given for reaction.</param>
        /// <param name="doBeep">if set to <c>true</c> it will beep in last 3 seconds.</param>
        /// <param name="doBeepInLast">if <c>doBeep</c> is true then it will beep in last specified seconds</param>
        /// <returns>
        /// Returns <c>defAns</c> if no action by user, returns !<c>defAns</c> in other case
        /// </returns>
        public static Boolean askPressAnyKeyInTime(String question, Boolean defAns = false, Int32 timeout = 10, Boolean doBeep = false, Int32 doBeepInLast = 3)
        {
            Boolean output = defAns; // = new Boolean();

            String helpLine = "-- press any key in [" + timeout + "] seconds, or [Escape] to cancel"; //[Y] yes    [N] no      [enter] default";

            Console.WriteLine(question);

            Console.WriteLine(helpLine);
            Console.WriteLine();

            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            DateTime startTime = DateTime.Now;
            DateTime lastReminder = DateTime.Now;
            Int32 timeleft = timeout;

            while (Console.KeyAvailable == false)
            {
                TimeSpan time = DateTime.Now.Subtract(lastReminder);

                if (time.TotalMilliseconds > 999)
                {
                    lastReminder = DateTime.Now;
                    Console.Write(timeleft + "... ");
                    timeleft--;
                    if (doBeep && (timeleft < (doBeepInLast + 1)))
                    {
                        doBeepViaConsole();
                    }
                }
                Thread.Sleep(200); // Loop until input is entered.
                if (timeout != -1)
                {
                    if (timeleft == 0)
                    {
                        Console.WriteLine(" -- timeout -- ");
                        return defAns;
                    }
                }
                else
                {
                    timeleft = 1;
                }
            }

            cki = Console.ReadKey(true);

            if (cki.Key != ConsoleKey.Escape)
            {
                Console.WriteLine(" -- confirmed by pressing [" + cki.Key.ToString() + "] -- ");
                return !defAns;
            }
            else
            {
                Console.WriteLine(" -- canceled by pressing [" + cki.Key.ToString() + "] -- ");
                return defAns;
            }

            return output;
        }

        /// <summary>
        /// User is prompted to press a key in defined time gap. On timeout, default option is selected
        /// </summary>
        /// <param name="question">The question.</param>
        /// <param name="defAns">The definition ans.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="doBeep">if set to <c>true</c> [do beep].</param>
        /// <param name="doBeepInLast">The do beep in last.</param>
        /// <returns></returns>
        public static ConsoleKey askAnyKeyInTime(String question, ConsoleKey defAns = ConsoleKey.Enter, Int32 timeout = 10, Boolean doBeep = false, Int32 doBeepInLast = 3)
        {
            ConsoleKey output = defAns; // = new Boolean();

            String helpLine = "-- press key in [" + timeout + "] seconds or default option [" + output.ToString() + "] will be selected"; //[Y] yes    [N] no      [enter] default";

            Console.WriteLine(question);

            Console.WriteLine(helpLine);
            Console.WriteLine();

            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            DateTime startTime = DateTime.Now;
            DateTime lastReminder = DateTime.Now;
            Int32 timeleft = timeout;

            while (Console.KeyAvailable == false)
            {
                TimeSpan time = DateTime.Now.Subtract(lastReminder);

                if (time.TotalMilliseconds > 999)
                {
                    lastReminder = DateTime.Now;
                    Console.Write(timeleft + "... ");
                    timeleft--;
                    if (doBeep && (timeleft < (doBeepInLast + 1)))
                    {
                        doBeepViaConsole();
                    }
                }
                Thread.Sleep(200); // Loop until input is entered.
                if (timeout != -1)
                {
                    if (timeleft == 0)
                    {
                        Console.WriteLine(" -- timeout -- ");
                        return defAns;
                    }
                }
                else
                {
                    timeleft = 1;
                }
            }

            cki = Console.ReadKey(true);

            return output;
        }
    }
}