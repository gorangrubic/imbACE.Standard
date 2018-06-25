// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceSettingsStandaloneBase.cs" company="imbVeles" >
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
using System;
using System.Collections.Generic;
using System.Linq;

namespace imbACE.Core.core
{
    using extensions.io;
    using imbACE.Core.extensions;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.files;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Settings container with Load() and Save() standalone implementation.
    /// </summary>
    /// <seealso cref="imbACE.Core.core.aceSettingsBase" />
    public abstract class aceSettingsStandaloneBase : aceSettingsBase
    {
        //protected aceSettingsStandaloneBase(String  __settings_filepath="")
        //{
        //    if (__settings_filepath == "")
        //    {
        //        __settings_filepath = "configuration\\";
        //    }
        //    settings_filepath = __settings_filepath;
        //}

        private String _settings_filepath;

        /// <summary> </summary>
        [XmlIgnore]
        public abstract String settings_filepath { get; }

        /// <summary>
        /// Gets the path with full filename
        /// </summary>
        /// <value>
        /// The path with full filepath and file name
        /// </value>
        [XmlIgnore]
        protected String path_with_filename_settings
        {
            get
            {
                String pt = settings_filepath
;
                if (!Path.HasExtension(pt))
                {
                    pt = imbSciStringExtensions.add(pt, "config_" + GetType().Name.getFilename() + ".xml", "\\");
                }
                return pt;
            }
        }

        /// <summary>
        /// Empty method used just to trigger autoload if required
        /// </summary>
        public void Poke()
        {
        }

        /// <summary>
        /// Loads and auto saves
        /// </summary>
        public void Load()
        {
            aceSettingsStandaloneBase settings;
            if (File.Exists(path_with_filename_settings))
            {
                settings = objectSerialization.loadObjectFromXML(path_with_filename_settings, GetType()) as aceSettingsStandaloneBase;
                if (settings != null) settings.wasLoaded = true;
                this.setObjectBySource(settings);
            }
            else
            {
                settings = this;
            }
            Save();
            //objectSerialization.saveObjectToXML(path_with_filename_settings, settings);
        }

        /// <summary>
        /// Saves the current settings. If filepath specified it will use it to save into. This will not change default load location.
        /// </summary>
        public void Save(String filepath = "")
        {
            FileInfo fi = null;

            if (filepath.isNullOrEmpty())
            {
                fi = path_with_filename_settings.getWritableFile(getWritableFileMode.newOrExisting);
            }
            else
            {
                fi = filepath.getWritableFile(getWritableFileMode.overwrite);
            }

            objectSerialization.saveObjectToXML(this, fi.FullName);
        }
    }
}