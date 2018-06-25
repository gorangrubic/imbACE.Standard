// --------------------------------------------------------------------------------------------------------------------
// <copyright file="objectWithNameAndDescription.cs" company="imbVeles" >
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

namespace imbACE.Core.interfaces.primitives
{
    using imbSCI.Core.extensions.data;
    using imbSCI.Data.enums.fields;
    using imbSCI.Data.interfaces;
    using System;
    using System.Data;

    /// <summary>
    /// Dummy object holding two basic information entryies
    /// </summary>
    /// <seealso cref="IObjectWithNameAndDescription" />
    public class objectWithNameAndDescription : IObjectWithNameAndDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="objectWithNameAndDescription"/> class.
        /// </summary>
        /// <param name="table">The table to learn <c>name</c> and <c>desciption</c> from</param>
        public objectWithNameAndDescription(DataTable table)
        {
            name = table.ExtendedProperties.getProperString(templateFieldDataTable.data_tablename, templateFieldDataTable.shema_sourcename, templateFieldDataTable.data_tablenamedb, templateFieldBasic.page_title, templateFieldBasic.document_title, templateFieldBasic.documentset_title);
            description = table.ExtendedProperties.getProperString(templateFieldDataTable.data_tabledesc, templateFieldBasic.page_desc, templateFieldBasic.document_desc, templateFieldBasic.documentset_desc);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="objectWithNameAndDescription"/> class.
        /// </summary>
        /// <param name="__name">The name.</param>
        /// <param name="__description">The description.</param>
        public objectWithNameAndDescription(String __name, String __description)
        {
            name = __name;
            description = __description;
        }

        private String _name = "";

        /// <summary>
        /// Name for this instance
        /// </summary>
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        private String _description = "";

        /// <summary>
        /// Human-readable description of object instance
        /// </summary>
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}