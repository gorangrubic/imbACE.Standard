// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceLogRegistry.cs" company="imbVeles" >
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
// Project: imbACE.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbACE.Core.core
{
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.render;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Text;

    /// <summary>
    /// aceLog ILogBuilder registry collection
    /// </summary>
    public class aceLogRegistry : IEnumerable
    {
        /// <summary>
        /// Gets content of the log specified by <see cref="aceCommonTypes.enums.logOutputSpecial"/> or some other <c>Enum</c> key
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public String getLogContent(Enum key)
        {
            String output = "";
            if (items.ContainsKey(key))
            {
                Object input = items[key];

                return getLogContentFromObject(input);
            }
            return "No instace found under [" + key.ToString() + "] in aceLogRegistry.";
        }

        public String getLogContent(String key)
        {
            String output = "";
            if (items.ContainsValue(key))
            {
                Object input = items[key];

                return getLogContentFromObject(input);
            }
            if (reservedFilenameAsKey.ContainsKey(key))
            {
                Object input = reservedFilenameAsKey[key];

                return getLogContentFromObject(input);
            }
            return "No instace found under [" + key + "] in aceLogRegistry.";
        }

        /// <summary>
        /// Gets the log content from object.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        protected String getLogContentFromObject(Object input)
        {
            if (input == null)
            {
                return "Silent Exception : [null] object sent as input provider --> aceLog / aceLogRegistry";
            }

            if (input is ITextRender)
            {
                ITextRender input_ITextRender = (ITextRender)input;
                return input_ITextRender.ContentToString();
            }

            if (input is IAceLogable)
            {
                IAceLogable input_IAceLogable = (IAceLogable)input;
                return input_IAceLogable.logContent;
            }

            //if (input is IAutosaveEnabled)
            //{
            //    IAutosaveEnabled input_IAceLogable = (IAutosaveEnabled)input;
            //    return input_IAceLogable.logContent;
            //}

            if (input is StringBuilder)
            {
                StringBuilder input_StringBuilder = (StringBuilder)input;
                return input_StringBuilder.ToString();
            }

            if (input is String)
            {
                String input_String = (String)input;
                return input_String;
            }

            return "Silent Exception : [unsupported] object (" + input.GetType().Name + ") send as input provider --> aceLog / aceLogRegistry";
        }

        /// <summary>
        /// Gets the type of the log builder of.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T getLogBuilderOfType<T>(Enum key) where T : class, ILogBuilder
        {
            if (items.ContainsKey(key))
            {
                return (T)items[key];
            }
            return null;
        }

        /// <summary>
        /// Gets the log builder.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public ILogBuilder getLogBuilder(Enum key)
        {
            if (items.ContainsKey(key))
            {
                return items[key] as ILogBuilder;
            }
            if (items.ContainsKey(key.toStringSafe()))
            {
                return items[key.toStringSafe()] as ILogBuilder;
            }
            return null;
        }

        public Object AddOrReplace(Object key, Object item, Boolean replace = true)
        {
            return Add(key, item, replace);
        }

        /// <summary>
        /// Adds an autosave enabled <c>item</c>, under the <c>key</c> specified
        /// </summary>
        /// <param name="key">The key : allowed types are String and Enumerations.</param>
        /// <param name="item">The item.</param>
        /// <returns>
        /// <see cref="Boolean"/> false if <c>item</c> registration failed otherwise <see cref="String"/> with reserved file path
        /// </returns>
        public Object Add(Object key, Object item, Boolean replace = false)
        {
            if (key is Enum) key = key.ToString();
            if (!(key is String))
            {
                ArgumentException argEx = new ArgumentException("The key of type:[" + key.GetType().Name + "] is not allowed. Only String or Enumerations are valid", "item");
                throw argEx;
                return false;
            }
            if (item == null)
            {
                ArgumentNullException argEx = new ArgumentNullException("item", "Failed registration call: the instace is null");
                aceLog.loger.AppendException("Silent ArgumentNullException : aceLogRegistry.Add(" + key + ", hash:[" + item.GetHashCode() + "]:[" + item.GetType().Name + "]", argEx);

                throw argEx;

                return false;
            }

            Boolean keyExists = items.ContainsKey(key);
            Boolean itemExists = items.ContainsValue(item);
            Boolean ok = (!(keyExists || itemExists) || replace);

            if (!ok)
            {
                ArgumentException argEx = new ArgumentException("Redundant registration call: the same instance [" + keyExists.ToString() + "] and/or the key [" + itemExists.ToString() + "]->[" + key + "] already exists in the registry", "item");

                if (keyExists && !itemExists)
                {
                    // if key is already taken but new item is new --- this is for unwanted "overwrite" protection
                    aceLog.loger.AppendException("Silent ArgumentException : aceLogRegistry.Add(" + key + ", hash:[" + item.GetHashCode() + "]:[" + item.GetType().Name + "])", argEx);
                    //throw argEx;
                }
                else
                {
                    aceLog.loger.AppendException("Silent ArgumentException : aceLogRegistry.Add(" + key + ", hash:[" + item.GetHashCode() + "]:[" + item.GetType().Name + "])", argEx);
                }

                return false;
            }
            else
            {
                Tuple<String, Boolean, String> response = reserveFileName(key, item, replace);

                if (!response.Item2)
                {
                    //ArgumentException argEx = new ArgumentException("item", "The filepath [" + filename + "] registration for [" + key.toStringSafe() + "] failed as the path was already reserved for item [" + itemExists.ToString() + "]->[" + key + "] already exists in the registry");
                    ArgumentException argEx = new ArgumentException("The filepath registration under key [" + response.Item3 + "] failed as path [" + response.Item1 + "] was already reserved for item hash:[" + item.GetHashCode() + "]:[" + item.GetType().Name + "] registered:[" + itemExists.ToString() + "]->[" + key + "]", "item");
                    throw argEx;
                    return false;
                }
                if (replace)
                {
                    if (items.ContainsKey(key)) items.Remove(key);
                }

                items.Add(key, item);
            }
            return reservedFileNames[item];
        }

        private Dictionary<Object, String> _reservedFileNames = new Dictionary<Object, string>();

        /// <summary>
        /// Gets or sets the reserved file names.
        /// </summary>
        /// <value>
        /// The reserved file names.
        /// </value>
        public Dictionary<Object, String> reservedFileNames
        {
            get { return _reservedFileNames; }
            protected set { _reservedFileNames = value; }
        }

        private Dictionary<String, Object> _reservedFilenameAsKey; // = new Dictionary<String, Object>();

        /// <summary>
        /// Description of $property$
        /// </summary>
        [Category("aceLogRegistry")]
        [DisplayName("reservedFilenameAsKey")]
        [Description("Description of $property$")]
        public Dictionary<String, Object> reservedFilenameAsKey
        {
            get
            {
                if (_reservedFilenameAsKey == null)
                {
                    _reservedFilenameAsKey = new Dictionary<String, Object>();
                }
                return _reservedFilenameAsKey;
            }
            set
            {
                _reservedFilenameAsKey = value;
            }
        }

        /// <summary>
        /// Reserves the name of the file.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        protected Tuple<String, Boolean, String> reserveFileName(Object key, Object item, Boolean replace)
        {
            String output = "";
            Boolean ok = true;
            String fileprefix = "log_";
            String fileextension = "md";
            String filename = key.toStringSafe();
            String filepath = "diagnostic\\";
            String filedatesignature = DateTime.Now.ToString("yyyy-MM-dd");

            if (item is IAutosaveEnabled)
            {
                IAutosaveEnabled item_IAutosaveEnabled = (IAutosaveEnabled)item;
                fileextension = item_IAutosaveEnabled.VAR_FilenameExtension;
                fileprefix = item_IAutosaveEnabled.VAR_FilenamePrefix;
                filepath = item_IAutosaveEnabled.VAR_FolderPathForAutosave;

                if (!item_IAutosaveEnabled.VAR_RegisterForAutosave)
                {
                    ArgumentException argEx = new ArgumentException("item", "The item that implements IAutosaveEnabled hash:[" + item.GetHashCode() + "]:[" + item.GetType().Name + "] has " + nameof(item_IAutosaveEnabled.VAR_RegisterForAutosave) + " set to *false*. This is unwanted call to autosave registration in " + nameof(aceLog.logBuilderRegistry) + ". Change the value to *true*.");
                    throw argEx;
                }
            }

            DirectoryInfo di = Directory.CreateDirectory(filepath);
            String fn = fileprefix.add(filename, "_").add(filedatesignature, "_").add(fileextension, ".");

            String path = di.FullName.add(fn, "\\");
            FileInfo fi = path.getWritableFile(getWritableFileMode.autoRenameExistingToOldOnce);
            output = fi.FullName;
            if ((!reservedFileNames.ContainsKey(item)) && (!reservedFilenameAsKey.ContainsKey(fi.FullName)))
            {
                reservedFileNames.Add(item, fi.FullName);
                reservedFilenameAsKey.Add(fi.FullName, item);
            }
            else
            {
                if (replace)
                {
                    reservedFileNames[item] = fi.FullName;
                    reservedFilenameAsKey[fi.FullName] = item;
                }

                ok = replace;
            }
            return Tuple.Create<string, bool, string>(output, ok, key.toStringSafe());
        }

        ///// <summary>
        ///// Adds <c>ILogBuilder</c> under <c>key</c> specified. Returns <c>TRUE</c> if item is accepted
        ///// </summary>
        ///// <param name="key">The key.</param>
        /////
        ///// <param name="item">The item.</param>
        ///// <returns></returns>
        //public String Add(Enum key, ILogBuilder item)
        //{
        //    if (items.ContainsKey(key))
        //    {
        //        return "";
        //    }

        //    items.Add(key, item);
        //    return fi.FullName;
        //}

        /// <summary>
        /// Returns an enumerator that iterates through <see cref="reservedFileNames"/> Dictionary. Item type is <see cref="KeyValuePair{IAceLogable, String}"/> where <c>Value</c> is designated location for log content save
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)reservedFileNames).GetEnumerator();
        }

        private Dictionary<Object, Object> _items = new Dictionary<Object, Object>();

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        /// <value>
        /// The items.
        /// </value>
        protected Dictionary<Object, Object> items
        {
            get { return _items; }
            set { _items = value; }
        }
    }
}