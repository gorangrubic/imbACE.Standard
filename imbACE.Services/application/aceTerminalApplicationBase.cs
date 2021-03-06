// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceTerminalApplicationBase.cs" company="imbVeles" >
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

namespace imbACE.Services.application
{
    using imbACE.Core.application;
    using imbACE.Core.core;
    using System;

    /// <summary>
    /// Aplikacija/GUI preko terminala
    /// </summary>
    public abstract class aceTerminalApplicationBase : aceApplicationBase, IDisposable
    {
        /// <summary>
        /// Azurira dinamicke komponente interfejsa
        /// </summary>
        public abstract void doUpdateInterface();

        /// <summary>
        /// Sadrzaj koji treba da se prikaze u levom cosku
        /// </summary>
        public String headerLineLeftContent { get; set; } = "";

        /// <summary>
        /// Sadrzaj koji treba da se prikaze u desnom cosku
        /// </summary>
        public String headerLineRightContent { get; set; } = "[ESC] Quit";

        /// <summary>
        /// Sadrzaj statusne linije - levo
        /// </summary>
        public String statusLineLeftContent { get; set; } = "";

        /// <summary>
        /// Sadrzaj statusne linije desno
        /// </summary>
        public String statusLineRightContent { get; set; } = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public String footerLineRightContent { get; set; } = "";

        /// <summary>
        /// Bindable property
        /// </summary>
        public String footerLineLeftContent { get; set; } = "";

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}