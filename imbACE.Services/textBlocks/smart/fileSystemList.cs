// --------------------------------------------------------------------------------------------------------------------
// <copyright file="fileSystemList.cs" company="imbVeles" >
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
namespace imbACE.Services.textBlocks.smart
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.extensions;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;

    /// <summary>
    /// lista sistemskih elemenata
    /// </summary>
    public class fileSystemList : INotifyPropertyChanged
    {
        public fileSystemList(String path, dialogSelectFileMode mode, String extension = "")
        {
            if (String.IsNullOrEmpty(path))
            {
                var pt = Directory.GetCurrentDirectory();
                path = Environment.CurrentDirectory;
            }

            rootDirectory = new DirectoryInfo(path);

            if (rootDirectory.Exists)
            {
                selfPopulate(mode, extension);
            }
        }

        public List<DirectoryInfo> directories = new List<DirectoryInfo>();
        public List<FileInfo> files = new List<FileInfo>();

        #region --- pattern ------- pattern za pretragu

        private String _pattern;

        /// <summary>
        /// pattern za pretragu
        /// </summary>
        public String pattern
        {
            get
            {
                return _pattern;
            }
            set
            {
                _pattern = value;
                OnPropertyChanged("pattern");
            }
        }

        #endregion --- pattern ------- pattern za pretragu

        protected void selfPopulate(dialogSelectFileMode mode, String extension)
        {
            pattern = "*".add(extension, ".");

            directories.Clear();
            files.Clear();
            if (rootDirectory != null) directories.Add(rootDirectory);
            if (rootDirectory.Parent != null) directories.Add(rootDirectory.Parent);

            switch (mode)
            {
                case dialogSelectFileMode.selectPath:
                    foreach (var d in rootDirectory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                    {
                        directories.Add(d);
                    }
                    break;

                case dialogSelectFileMode.selectFileToOpen:
                case dialogSelectFileMode.selectFileToSave:
                    foreach (var d in rootDirectory.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                    {
                        directories.Add(d);
                    }
                    foreach (var d in rootDirectory.EnumerateFiles(pattern, SearchOption.TopDirectoryOnly))
                    {
                        files.Add(d);
                    }
                    break;
            }
        }

        #region --- rootDirectory ------- direktorijum o dakle ide dalje

        private DirectoryInfo _rootDirectory;

        /// <summary>
        /// direktorijum o dakle ide dalje
        /// </summary>
        public DirectoryInfo rootDirectory
        {
            get
            {
                return _rootDirectory;
            }
            set
            {
                _rootDirectory = value;
                OnPropertyChanged("rootDirectory");
            }
        }

        #endregion --- rootDirectory ------- direktorijum o dakle ide dalje

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}