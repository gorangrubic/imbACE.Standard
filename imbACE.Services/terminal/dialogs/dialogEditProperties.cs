// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dialogEditProperties.cs" company="imbVeles" >
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
    using imbACE.Core.core.exceptions;
    using imbACE.Core.data;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.dialogs.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.smart;
    using System;

    /// <summary>
    /// Dijalog kojim se edituju propertiji
    /// </summary>
    public class dialogEditProperties : dialogScreenBase
    {
        /// <summary>
        /// Otvara privremenu instancu dijaloga i sinhrono ceka rezultat
        /// </summary>
        /// <param name="platform">Platforma na kojoj se prikazuje dijalog</param>
        /// <param name="targetObject">Objekat koi se edituje</param>
        /// <param name="description">The description.</param>
        /// <param name="hostTitle">Naslov host-&gt;property objekta</param>
        /// <returns>
        /// Kolekcija rezultata
        /// </returns>
        public static Object open(IPlatform platform, Object targetObject, String description, String hostTitle)
        {
            dialogEditProperties dialog = new dialogEditProperties(platform, targetObject, hostTitle, description);

            var format = new dialogFormatSettings(dialogStyle.redDialog, dialogSize.mediumBox);
            var result = dialog.open(platform, format);
            return dialog.editor.getObject();
        }

        /// <summary>
        /// Dijalog kojim se edituju propertiji nekog objekta. Podrzava paginaciju
        /// </summary>
        /// <param name="platform">Platforma na kojoj se prikazuje dijalog</param>
        /// <param name="targetObject">Objekat koi se edituje</param>
        public dialogEditProperties(IPlatform platform, Object targetObject, String TitleMessage, String StatusMessage)
            : base(platform)
        {
            layoutTitleMessage = TitleMessage;
            layoutStatusMessage = StatusMessage;
            init(platform);

            // Definicija izgleda ovog dijaloga
            dialogFormatSettings format = new dialogFormatSettings(dialogStyle.greenDialog, dialogSize.fullScreenBox);

            // Primenjuje pravila formatiranja
            format.apply(this, platform);
            backgroundDecoration = "=";
            writeBackground(null, true);

            editor = new smartMenuPropertyEditor(height - header.height, width, 0, 0);
            //editor.pageManager = new textBlocks.core.textPageManager<menu.core.aceMenuItem>()
            editor.layoutTitleMessage = TitleMessage;
            editor.layoutStatusMessage = StatusMessage;
            editor.setObject(targetObject);

            header.setAttachment(editor);
            //addLayer(editor, layerBlending.transparent, 100);
        }

        public smartMenuPropertyEditor editor;
    }
}