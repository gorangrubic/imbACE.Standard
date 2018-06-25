// --------------------------------------------------------------------------------------------------------------------
// <copyright file="dialogScreenBase.cs" company="imbVeles" >
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
namespace imbACE.Services.terminal.dialogs.core
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.enums.platform;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.core;
    using imbACE.Services.textBlocks;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.interfaces;
    using imbACE.Services.textBlocks.smart;
    using imbSCI.Core.reporting.zone;
    using System;
    using System.ComponentModel;

    //using imbACE.Core.zone;

    /// <summary>
    /// Polazna klasa za dve dijaloge
    /// </summary>
    public abstract class dialogScreenBase : textLayout, INotifyPropertyChanged
    {
        protected dialogScreenBase(IPlatform platform)
            : base(platform, " ")
        {
        }

        protected smartMessageSection header;

        protected void init(IPlatform platform)
        {
            header = new smartMessageSection(layoutTitleMessage, layoutStatusMessage, this);
            header.backgroundDecoration = "#";
            header.doInverseColors = false;
            header.insertSplitLine();
            header.setStyle(textSectionLineStyleName.heading);
            header.backColor = platformColorName.Blue;
            header.foreColor = platformColorName.White;

            addLayer(header, layerBlending.transparent, 50);
        }

        public override Boolean addLayer(ITextLayoutContentProvider __target, layerBlending __blending = layerBlending.notset, Int32 __ZOrder = -1)
        {
            //  __target.margin.top = __target.margin.top + this.margin.top;
            return base.addLayer(__target, __blending, __ZOrder);
        }

        /// <summary>
        /// Otvara dijalog i ceka dok se uspesno ne zatvori
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <returns></returns>
        public inputResultCollection open(IPlatform platform, dialogFormatSettings format)
        {
            var output = openSequence(platform, format);
            return output;
        }

        /// <summary>
        /// Open sequenca koja se izvrsava unutar open poziva
        /// </summary>
        /// <param name="platform"></param>
        /// <param name="format"> </param>
        protected inputResultCollection openSequence(IPlatform platform, dialogFormatSettings format)
        {
            Boolean doKeepReading = true;
            Boolean doKeepOpened = false;

            // format.apply(this, platform);
            inputResultCollection results = null;
            do
            {
                refresh();

                results = new inputResultCollection();
                results.platform = platform;

                while (doKeepReading)
                {
                    render(platform, true);
                    results = read(results);
                    doKeepReading = results.doKeepReading();
                }

                results = execute(results);
            } while (doKeepOpened);

            return results;
        }

        /// <summary>
        /// Obnavlja dinamicki deo sadrzaja - nije predvidjen za direktno pozivanje
        /// </summary>
        public void refresh()
        {
            header.foreColor = foreColor;
            header.backColor = backColor;

            foreach (var l in layers)
            {
                if (l is IRefresh)
                {
                    IRefresh ir = l as IRefresh;
                    ir.refresh();
                }
            }
        }

        /// <summary>
        /// #2 Očitava ulaz -- reseno na nivou aceTErminalScreenBase
        /// </summary>
        public inputResultCollection read(inputResultCollection __results)
        {
            if (__results == null) __results = new inputResultCollection();

            foreach (var l in layers)
            {
                if (l is IRead)
                {
                    IRead ir = l as IRead;

                    __results = ir.read(__results);

                    //  var rs = __results.getBySection(ir);
                    // rs = ir.read(__results.platform, rs);
                    //   __results.AddUniqueSection(rs);
                }
            }

            return __results;
        }

        /// <summary>
        /// #3 Vrsi rad nakon sto je obradjen ulaz
        /// </summary>
        public virtual inputResultCollection execute(inputResultCollection __inputs)
        {
            return __inputs;
        }
    }
}