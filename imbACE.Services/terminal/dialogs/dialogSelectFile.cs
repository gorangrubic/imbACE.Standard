// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dialogSelectFile.cs" company="imbVeles" >
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

namespace imbACE.Services.terminal.dialogs
{
    using imbACE.Core.core;
    using imbACE.Core.extensions;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.dialogs.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.smart;
    using System.IO;

    /// <summary>
    /// Dialog for file selection
    /// </summary>
    /// <seealso cref="imbACE.Services.terminal.dialogs.core.dialogScreenBase" />
    public class dialogSelectFile : dialogScreenBase
    {
        public dialogSelectFile(IPlatform platform, String path, dialogSelectFileMode mode, String extension, String extraComment)
            : base(platform)
        {
            dialogFormatSettings format = new dialogFormatSettings(dialogStyle.blueDialog, dialogSize.fullScreenBox);

            format.apply(this, platform);

            selector = new smartFilePathSelector(path, mode, extension, this.height, this.width, 0, 3);
            selector.doShowTitle = false;

            switch (mode)
            {
                case dialogSelectFileMode.selectFileToOpen:
                    layoutTitleMessage = "Select file to open";
                    break;

                case dialogSelectFileMode.selectFileToSave:
                    layoutTitleMessage = "Select file to save";
                    break;

                case dialogSelectFileMode.selectPath:
                    layoutTitleMessage = "Select directory path";
                    break;
            }

            layoutStatusMessage = extraComment;

            addLayer(selector, layerBlending.transparent, 100);

            init(platform);
        }

        public smartFilePathSelector selector;

        public static inputResultCollection open(IPlatform platform, String __path, dialogSelectFileMode __mode, String __extension = ".*", String extraComment = "")
        {
            dialogSelectFile dialog = new dialogSelectFile(platform, __path, __mode, __extension, extraComment);

            var format = new dialogFormatSettings(dialogStyle.redDialog, dialogSize.mediumBox);

            return dialog.open(platform, format);
        }
    }
}