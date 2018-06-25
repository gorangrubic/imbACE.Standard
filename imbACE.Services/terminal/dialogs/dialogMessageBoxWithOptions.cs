// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dialogMessageBoxWithOptions.cs" company="imbVeles" >
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
namespace imbACE.Services.terminal.dialogs
{
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.core.exceptions;
    using imbACE.Core.extensions;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.dialogs.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.smart;
    using System;
    using System.Collections.Generic;

    public class dialogMessageBoxWithOptions<T> : dialogScreenBase
    {
        /// <summary>
        /// Dijalog kojim se edituju propertiji nekog objekta. Podrzava paginaciju
        /// </summary>
        /// <param name="platform">Platforma na kojoj se prikazuje dijalog</param>
        /// <param name="targetObject">Objekat koi se edituje</param>
        public dialogMessageBoxWithOptions(IPlatform platform, String title, String description, IEnumerable<T> options)
            : base(platform)
        {
            layoutTitleMessage = title;
            layoutStatusMessage = description;

            init(platform);
            // Definicija izgleda ovog dijaloga
            dialogFormatSettings format = new dialogFormatSettings(dialogStyle.redDialog, dialogSize.messageBox);

            // Primenjuje pravila formatiranja
            format.apply(this, platform);
            backgroundDecoration = "=";
            writeBackground(null, true);

            //var titleDiv = new smartMessageSection(title, description, height, width, 2, 2);

            //titleDiv.setStyle(textSectionLineStyleName.heading);

            var menu = new aceMenu();
            menu.setItems(options);

            var menuDiv = new smartMenuSection(menu, this, textInputMenuRenderView.inlineKeyListGroup);

            menuDiv.doShowValueRemarks = false;
            menuDiv.doShowTitle = false;
            menuDiv.doShowRemarks = false;
            header.setAttachment(menuDiv);

            //   titleDiv.setAttachment(menuDiv);

            // addLayer(titleDiv, layerBlending.transparent, 100);
        }
    }
}