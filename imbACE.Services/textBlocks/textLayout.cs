// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textLayout.cs" company="imbVeles" >
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
namespace imbACE.Services.textBlocks
{
    using imbACE.Core.core.exceptions;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.core;
    using imbACE.Services.textBlocks.core;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.interfaces;
    using imbACE.Services.textBlocks.smart;
    using imbSCI.Core.reporting.zone;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    //public enum textLayoutRenderNewFrameMode
    //{
    //    clean,
    //    overwrite,
    //}

    /// <summary>
    /// Text Layout - Composite content layer that accepts content from multiple layers. It has coordinate area (H/W, padding and margin), Footer message, Title message and Status message
    /// </summary>
    public class textLayout : textSectionBase, IRender, ItextLayout
    {
        private static int compareZ(ITextLayoutContentProvider x, ITextLayoutContentProvider y)
        {
            return x.ZLayerOrder.CompareTo(y.ZLayerOrder);
        }

        public textLayout(IPlatform platform, String bgdeco = "/") : base(platform.height, platform.width, platform.margin.left, platform.padding.left)
        {
            backgroundDecoration = bgdeco;
            init(platform);
        }

        #region --- layers ------- kolekcija sectiona

        private List<ITextLayoutContentProvider> _layers = new List<ITextLayoutContentProvider>();

        /// <summary>
        /// Collection of layers/sections
        /// </summary>
        public List<ITextLayoutContentProvider> layers
        {
            get
            {
                return _layers;
            }
            set
            {
                _layers = value;
                OnPropertyChanged("layers");
            }
        }

        #endregion --- layers ------- kolekcija sectiona

        private List<ITextLayoutContentProvider> preRenderPass(List<ITextLayoutContentProvider> input)
        {
            List<ITextLayoutContentProvider> output = new List<ITextLayoutContentProvider>();

            Int32 delta = input.Count;
            Int32 last = output.Count;
            while (delta > 0)
            {
                foreach (ITextLayoutContentProvider layer in input)
                {
                    if (!output.Contains(layer))
                    {
                        output.Add(layer);
                    }
                    var ta = layer.getAttachment(false);
                    if (ta != null)
                    {
                        if (!output.Contains(ta))
                        {
                            output.Add(ta);
                        }
                    }
                    var ba = layer.getAttachment(true);
                    if (ba != null)
                    {
                        if (!output.Contains(ba))
                        {
                            output.Add(ba);
                        }
                    }
                }
                delta = output.Count - input.Count;
                input = output.ToList();
            }

            return output;
        }

        private void preRender()
        {
            List<ITextLayoutContentProvider> allLayers = preRenderPass(layers.ToList());

            foreach (ITextLayoutContentProvider layer in allLayers)
            {
                addLayer(layer);
                //if (layer is IInputSection)
                //{
                //   // layer.resetContent();
                //    IInputSection ilayer = layer as IInputSection;
                //    if (ilayer.layoutFooterMessage.Length > 0) layoutFooterMessage = ilayer.layoutFooterMessage;
                //    if (ilayer.layoutStatusMessage.Length > 0) layoutStatusMessage = ilayer.layoutStatusMessage;
                //    if (ilayer.layoutTitleMessage.Length > 0) layoutTitleMessage = ilayer.layoutTitleMessage;
                //}
            }
            layers.Sort(compareZ);
        }

        public override void resetContent()
        {
            base.resetContent();
            contentLines.paint(this, backgroundDecoration);
            blending = layerBlending.background;
            ZLayerOrder = 0;
        }

        /// <summary>
        /// Renderuje lejere u sopstveni sadrzaj
        /// </summary>
        public void render()
        {
            resetContent();
            preRender();

            foreach (ITextLayoutContentProvider layer in layers)
            {
            }

            foreach (ITextLayoutContentProvider layer in layers)
            {
                layer.resetContent();
            }

            foreach (ITextLayoutContentProvider layer in layers)
            {
                layer.refreshAttachmentPosition();
                render(layer, this);
            }
        }

        /// <summary>
        /// Pakuje sadržaj po lejerima u IHasCursor target objekat koristeći njegov kursor
        /// </summary>
        /// <param name="target">Objekat čijim kursorom pakuje sadržaj</param>
        private void render(ITextLayoutContentProvider source, IHasCursor target)
        {
            textCursor cursor;
            cursor = target.cursor;
            cursor.switchToZone(textCursorZone.innerZone);

            if (source != null)
            {
                List<String> lns = source.getContent().ToList();

                switch (source.blending)
                {
                    case layerBlending.background:
                        cursor.switchToZone(textCursorZone.outterZone);
                        cursor.moveToCorner(textCursorZoneCorner.UpLeft);
                        foreach (String ln in lns)
                        {
                            target.write(cursor, ln, -1, true);
                            //target.write(ln, cursor.x, true, layer.target.width, true);
                        }
                        break;

                    case layerBlending.semitransparent:
                        lns = lns.GetRange(source.innerBoxedTopPosition, source.innerBoxedHeight);

                        cursor.switchToZone(textCursorZone.innerBoxedZone);
                        cursor.moveToCorner(textCursorZoneCorner.UpLeft);
                        foreach (String ln in lns)
                        {
                            target.write(cursor, ln, -1, true);
                            // target.write(cursor, ln, -1, true); //ln, 0, true, source.width, true);
                        }
                        break;

                    case layerBlending.transparent:
                        // lns = lns.GetRange(source.innerBoxedTopPosition, source.innerBoxedHeight);

                        cursor.switchToZone(textCursorZone.innerBoxedZone);
                        cursor.moveToCorner(textCursorZoneCorner.UpLeft);
                        for (Int32 i = source.innerBoxedTopPosition; i < source.innerBoxedBottomPosition; i++)
                        {
                            String cur = source.select(cursor, target.innerWidth, false);
                            target.write(cursor, cur);
                        }

                        break;

                    case layerBlending.hidden:

                        break;
                }
            }

            cursor.switchToMainZone();
        }

        /// <summary>
        /// #1 Generise sadrzaj
        /// </summary>
        private void render(ITextLayoutContentProvider source, IPlatform platform)
        {
            platform.setColors(source.foreColor, source.backColor, source.doInverseColors);

            List<String> lns = source.getContent().ToList();

            textCursor cursor = new textCursor(platform, textCursorMode.fixedZone, textCursorZone.outterZone);

            if (source is smartInfoLineSection)
            {
                string[] cnt = source.getContent();
                platform.render(cnt[0], 0, source.margin.top);
                return;
            }

            switch (source.blending)
            {
                case layerBlending.background:
                    cursor.switchToZone(textCursorZone.outterZone);
                    cursor.moveToCorner(textCursorZoneCorner.UpLeft);

                    foreach (String ln in lns)
                    {
                        platform.render(ln, 0, cursor.y);
                        cursor.nextLine();
                        //target.write(cursor, ln, -1, true);
                        //target.write(ln, cursor.x, true, layer.target.width, true);
                    }
                    break;

                case layerBlending.semitransparent:

                    cursor = new textCursor(source, textCursorMode.fixedZone, textCursorZone.outterZone);
                    cursor.switchToZone(textCursorZone.innerBoxedZone);
                    cursor.moveToCorner(textCursorZoneCorner.UpLeft);
                    for (Int32 i = 0; i < source.innerBoxedHeight; i++)
                    {
                        cursor.moveLineTo(i);
                        String cur = source.select(cursor, source.innerBoxedHeight, true);
                        platform.render(cur, 0, cursor.y);
                    }
                    //lns = lns.GetRange(source.innerBoxedTopPosition, source.innerBoxedHeight);
                    //cursor.moveLineTo(source.innerBoxedTopPosition);
                    //cursor.switchToZone(enums.textCursorZone.innerBoxedZone);
                    //cursor.moveToCorner(textCursorZoneCorner.UpLeft);
                    foreach (String ln in lns)
                    {
                        platform.render(ln, 0, cursor.y);
                        //target.write(cursor, ln, -1, true);
                        // target.write(cursor, ln, -1, true); //ln, 0, true, source.width, true);
                    }
                    break;

                case layerBlending.transparent:
                    // lns = lns.GetRange(source.innerBoxedTopPosition, source.innerBoxedHeight);
                    cursor = new textCursor(source, textCursorMode.fixedZone, textCursorZone.innerBoxedZone);
                    cursor.switchToZone(textCursorZone.innerBoxedZone);
                    cursor.moveToCorner(textCursorZoneCorner.UpLeft);
                    for (Int32 i = source.innerBoxedTopPosition; i < source.innerBoxedBottomPosition; i++)
                    {
                        cursor.y = i;
                        //cursor.x =
                        String cur = source.select(cursor, source.innerBoxedWidth, false);

                        platform.render(cur, cursor.x, cursor.y);
                    }

                    break;

                case layerBlending.hidden:

                    break;
            }

            platform.setColorsBack();

            //platform.setColors(platformColorName.none);
        }

        //}

        /// <summary>
        /// #1 Generise sadrzaj
        /// </summary>
        public void render(IPlatform platform, Boolean doClearScreen = true)
        {
            if (doClearScreen) platform.clear();
            resetContent();
            preRender();
            // render(this, platform);

            foreach (ITextLayoutContentProvider layer in layers)
            {
                layer.resetContent();
                layer.refreshAttachmentPosition();

                render(layer, platform);
            }
        }

        /// <summary>
        /// Dodaje novi layer u layout. Ako je __target objekat vec zakacen za neki layer odustaje i vraca FALSE
        /// </summary>
        /// <param name="__target">Objekat koji pruza sadrzaj</param>
        /// <param name="__blending">Nacin kopiranja sadrzaja</param>
        /// <param name="onTop">Ako je TRUE layer ce biti na vrhu (poslednji se renderuje), ako je FALSE layer ce biti prvi na dnu (prvi se renderuje)</param>
        /// <returns></returns>
        public virtual Boolean addLayer(ITextLayoutContentProvider __target, layerBlending __blending = layerBlending.notset, Int32 __ZOrder = -1)
        {
            if (__target == null)
            {
                return false;
            }
            if (__blending != layerBlending.notset)
            {
                __target.blending = __blending;
            }

            if (__ZOrder > -1)
            {
                __target.ZLayerOrder = __ZOrder;
            }

            if (layers.Contains(__target))
            {
                return false;
            }

            layers.Add(__target);

            return true;
        }

        /// <summary>
        /// #0 Izvrsava se prvi put - kada se instancira. Customized sekvenca inicijalizacije
        /// </summary>
        /// <param name="platform"> </param>
        public virtual void init(IPlatform platform)
        {
            width = platform.width;
            height = platform.height;
        }
    }
}