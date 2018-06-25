// --------------------------------------------------------------------------------------------------------------------
// <copyright file="smartScreenFileSelector.cs" company="imbVeles" >
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

namespace imbACE.Services.terminal.smartScreen
{
    using imbACE.Core.enums.platform;
    using imbACE.Core.operations;
    using imbACE.Services.application;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.input;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.core;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.smart;
    using System.IO;

    public class smartScreenFileSelector<T> : aceTerminalScreenBase<T> where T : aceTerminalApplication
    {
        #region --- filePathSection ------- sekcija kojom se bira file path

        private smartFilePathSelector _filePathSection;

        /// <summary>
        /// sekcija kojom se bira file path
        /// </summary>
        public smartFilePathSelector filePathSection
        {
            get
            {
                return _filePathSection;
            }
            set
            {
                _filePathSection = value;
                OnPropertyChanged("filePathSection");
            }
        }

        #endregion --- filePathSection ------- sekcija kojom se bira file path

        public textInputResult getFilePath()
        {
            textInputResult output = new textInputResult(inputReadMode.readLine);

            FileSystemInfo fl = filePathSection.currentOutput.result as FileSystemInfo;
            output.result = fl;
            output.meta.Add(fl.FullName);
            output.meta.Add(fl.Extension);
            output.meta.Add(filePathSection);
            //output.result = filePathSection.

            return filePathSection.currentOutput; //fl.FullName;
        }

        public smartScreenFileSelector(T __app, string __title, String __path, dialogSelectFileMode __mode, String __extension)
            : base(__app, __title)
        {
            filePathSection = new smartFilePathSelector(__path, __mode, __extension, application.platform.height,
                                                         application.platform.width, 0, 0);

            init(__app.platform);
        }

        /// <summary>
        /// #0 Izvrsava se prvi put - kada se instancira. Customized sekvenca inicijalizacije
        /// </summary>
        /// <param name="platform"> </param>
        public override void init(IPlatform platform)
        {
            filePathSection.refresh();

            layout.addLayer(filePathSection, textBlocks.enums.layerBlending.transparent, 100);
        }

        /// <summary>
        /// Obnavlja dinamicki deo sadrzaja
        /// </summary>
        public override void refresh()
        {
            filePathSection.refresh();
        }

        /// <summary>
        /// #2 Očitava ulaz -- reseno na nivou aceTErminalScreenBase
        /// </summary>
        public override inputResultCollection read(inputResultCollection __results)
        {
            if (__results == null) __results = new inputResultCollection();

            var min = __results.getBySection(filePathSection);

            if (filePathSection == null)
            {
            }
            else
            {
                var rd = filePathSection.read(__results.platform, min);
                __results.AddUniqueSection(rd);
            }

            //filePathSection.read(__results.platform);

            return __results;
        }

        /// <summary>
        /// #3 Vrsi rad nakon sto je obradjen ulaz
        /// </summary>
        public override inputResultCollection execute(inputResultCollection __inputs)
        {
            application.goBack();

            return __inputs;
        }
    }
}