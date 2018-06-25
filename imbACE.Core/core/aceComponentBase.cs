// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceComponentBase.cs" company="imbVeles" >
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

namespace imbACE.Core.core
{
    using imbSCI.Core.data;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.files;
    using imbSCI.Core.files.folders;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data.enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.IO;

    /// <summary>
    /// Osnova za kompaktne izvrsne komponente --- deprecated
    /// </summary>
    public abstract class aceComponentBase<T> : aceBindable

    {
        /// <summary>
        /// Sets the default folder structure
        /// </summary>
        //  public abstract void setFolderStructure();
        //{
        //    //folderStructure = new folderStructure(Directory.GetCurrentDirectory(), name, )
        //    //folderStructure.Add(aceFolderType.etc, "etc", "Content: configuration and temporary files managed by the software. Intended use: not for manual modification.");
        //    //folderStructure.Add(aceFolderType.tmp, "etc/tmp", "Content: temporary files managed by the software. Intended use: not for manual modification.");
        //    //folderStructure.Add(aceFolderType.var, "var", "Content: logs, crash reports and other diagnostic reports, backups... Intended use: troubleshooting and recover ops.");
        //    //folderStructure.Add(aceFolderType.lib, "lib", "Content: preset library and resources. Intended use: modification only for experts");
        //    //folderStructure.Add(aceFolderType.bin, "bin", "Content: included third-party software tools.");
        //    //folderStructure.Add(aceFolderType.usr, "usr", "Content: user projects/jobs.");
        //}

        public aceComponentBase()
        {
            // setFolderStructure();
            // makeFolderStructure();
            settingsLoadOrDefault();
        }

        /// <summary>
        /// Makes the folder structure - and generates readme_structure / directory files
        /// </summary>
        protected void makeFolderStructure()
        {
            if (folderStructure != null)
            {
                // List<aceFolderInfo> nw = folderStructure;
                // nw = nw.getFlatList<aceFolderInfo>();

                generateFolderReadme(folderStructure, null).saveStringToFile("readme_structure.md");

                String readme_filename = "readme_directory.md";

                foreach (var ni in folderStructure)
                {
                    String path = ni.Value.pathFor(readme_filename);

                    FileInfo fi = path.getWritableFile(getWritableFileMode.overwrite);

                    String content = generateFolderReadme(null, ni.Value);
                    content.saveStringToFile(fi.FullName, getWritableFileMode.overwrite);
                    saveBase.saveToFile(fi.FullName, content);
                    //DirectoryInfo di =
                }

                //  nw.ForEach(x => log("Folder ["+x.name+"] created for ["+name+"] "));
            }
        }

        private folderStructure _folderStructure;

        /// <summary> </summary>
        public folderStructure folderStructure
        {
            get
            {
                return _folderStructure;
            }
            set
            {
                _folderStructure = value;
                OnPropertyChanged("folderStructure");
            }
        }

        //        public aceFolderStructure folderStructure;

        #region -----------  terminal  -------  [instanca terminala koji koristi za logovanje]

        private ILogBuilder _terminal;// = new aceTerminal();

        /// <summary>
        /// instanca terminala koji koristi za logovanje
        /// </summary>
        // [XmlIgnore]
        [Category("aceComponentBase")]
        [DisplayName("terminal")]
        [Description("instanca terminala koji koristi za logovanje")]
        public ILogBuilder terminal
        {
            get
            {
                if (_terminal == null)
                {
                    return aceLog.loger; // aceCommons.terminal;
                }
                return _terminal;
            }
        }

        #endregion -----------  terminal  -------  [instanca terminala koji koristi za logovanje]

        private aceAuthorNotation _notation = new aceAuthorNotation();

        /// <summary>
        ///
        /// </summary>
        public aceAuthorNotation notation
        {
            get { return _notation; }
            protected set { _notation = value; }
        }

        /// <summary>
        /// Snima vrednosti podesavanja
        /// </summary>
        public void saveSettings()
        {
            if (settings != null)
            {
                objectSerialization.saveObjectToXML(path_with_filename_settings, settings);
            }
        }

        #region -----------  settings  -------  [Podesavanja komponente]

        private T _settings = default(T);

        /// <summary>
        /// Podesavanja komponente
        /// </summary>
        // [XmlIgnore]
        [Category("aceComponentBase")]
        [DisplayName("settings")]
        [Description("Podesavanja komponente")]
        public T settings
        {
            get
            {
                return _settings;
            }
            set
            {
                // Boolean chg = (_settings != value);
                _settings = value;
                OnPropertyChanged("settings");
                // if (chg) {}
            }
        }

        #endregion -----------  settings  -------  [Podesavanja komponente]

        /// <summary>
        /// Component name
        /// </summary>
        private String _name = "";

        /// <summary>
        /// Component name
        /// </summary>
        public String name
        {
            get
            {
                if (String.IsNullOrEmpty(_name))
                {
                    _name = this.GetType().Name;
                }
                return _name;
            }
        }

        /// <summary>
        /// Generates the folder readme.
        /// </summary>
        /// <param name="folders">The folders.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        protected String generateFolderReadme(IEnumerable<folderNode> folders, folderNode folder = null)
        {
            builderForMarkdown builder = new builderForMarkdown();

            if (folder != null)
            {
                builder.AppendHeading("Folder structure", 2);
                builder.AppendLine();

                builder.AppendHeading(folder.name, 3);
                builder.AppendLine(" > " + folder.path);
                builder.AppendLine(" > " + folder.description);
                builder.AppendLine();

                builder.AppendHorizontalLine();
            }
            else
            {
                builder.AppendHeading("Folder structure", 2);
                builder.AppendLine();

                builder.AppendParagraph("Application directory structure");
                builder.AppendHorizontalLine();

                foreach (var fold in folders)
                {
                    //    builder.nextTabLevel();
                    builder.AppendHeading(fold.name, 3);
                    builder.AppendLine(" > " + fold.path);
                    builder.AppendLine(" > " + fold.description);
                    builder.AppendLine();
                    //  builder.prevTabLevel();
                }
            }

            builder.AppendLine();
            builder.AppendHorizontalLine();

            PropertyCollection pc = notation.buildPropertyCollection<PropertyCollection>(false, false, "cite");

            builder.AppendPairs(pc, false);

            builder.AppendLine();
            builder.AppendHorizontalLine();

            builder.AppendLine("File generated: ".add(DateTime.Now.ToLongDateString(), " ").add(DateTime.Now.ToLongTimeString()));
            builder.AppendLine("Application: <<".add(name, " ").add(">>", " "));

            return builder.ContentToString(true);
        }

        /// <summary>
        /// Proverava da li su instancirana podesavanja -- ako nisu ucitava postojeca ili kreira default objekat
        /// </summary>
        protected void settingsLoadOrDefault()
        {
            //if (File.Exists(path_with_filename_settings))
            //{
            //    settings = objectSerialization.loadObjectFromXML<T>(path_with_filename_settings);
            //    if (settings != null) settings.wasLoaded = true;
            //} else
            //{
            //    settings = new T();
            //}

            objectSerialization.saveObjectToXML(path_with_filename_settings, settings);
        }

        protected String path_with_filename_settings
        {
            get
            {
                String pt = path_settings;
                if (!Path.HasExtension(pt))
                {
                    pt = pt.add("config.xml", "\\");
                }
                return pt;
            }
        }

        ///// <summary>
        ///// putanja prema podesavanjima. Ukoliko nije definisano - automatski postavlja vrednost
        ///// </summary>
        public abstract String path_settings { get; }

        //#region --- path_logoutput ------- putanja prema izlazu log-a
        //private String _path_logoutput;
        ///// <summary>
        ///// putanja prema izlazu log-a. Ukoliko nije definisano automatski postavlja vrednost
        ///// </summary>
        //public String path_logoutput
        //{
        //    get
        //    {
        //        //if (String.IsNullOrEmpty(_path_logoutput))
        //        //{
        //        //    _path_settings = folderStructure[aceFolderType.var].getPathFor(name.add("log.txt", "_"), false);

        //        //    //_path_logoutput = "_" + this.GetType().Name.ToLower() + "_logout.txt";
        //        //}
        //        return _path_logoutput;
        //    }
        //    //set
        //    //{
        //    //    _path_logoutput = value;
        //    //    OnPropertyChanged("path_logoutput");
        //    //}
        //}
        //#endregion

        ///// <summary>
        ///// Osnovna podesavanja
        ///// </summary>
        ///// <param name="_toConsole"></param>
        ///// <param name="_autosave"></param>
        ///// <param name="__filename"></param>
        //public void logSetup(Boolean _toConsole, Boolean _autosave, String __filename = null)
        //{
        //    terminal.doAutoSaveLog = _autosave;
        //    terminal.doToConsole = _toConsole;

        //    if (__filename != null) terminal.path_logoutput = __filename;
        //}

        public void log(string message)
        {
            //terminal. = path_logoutput;
            terminal.log(message);
        }

        public string logContent
        {
            get { return terminal.logContent; }
        }
    }
}