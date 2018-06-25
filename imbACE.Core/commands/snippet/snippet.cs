// --------------------------------------------------------------------------------------------------------------------
// <copyright file="snippet.cs" company="imbVeles" >
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

namespace imbACE.Core.commands.snippet
{
    #region imbVeles using

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    //using aceCommonTypes.extensions;

    #endregion imbVeles using

    /// <summary>
    /// C# Code snippet
    /// </summary>
    public class snippet : imbBindable
    {
        #region snippetType enum

        public enum snippetType
        {
            expansion,
            wrapper
        }

        #endregion snippetType enum

        #region --- type ------- tip snipeta

        private snippetType _type = snippetType.expansion;

        /// <summary>
        /// tip snipeta
        /// </summary>
        public snippetType type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("type");
            }
        }

        #endregion --- type ------- tip snipeta

        #region --- filepath ------- potpuna putanja prema snippetu

        private String _filepath;

        /// <summary>
        /// potpuna putanja prema snippetu
        /// </summary>
        public String filepath
        {
            get { return _filepath; }
            set
            {
                _filepath = value;
                OnPropertyChanged("filepath");
            }
        }

        #endregion --- filepath ------- potpuna putanja prema snippetu

        #region --- snippetTypeStrings ------- String lista snippet tipova

        private List<String> _snippetTypeStrings;

        /// <summary>
        /// String lista snippet tipova
        /// </summary>
        [imb(imbAttributeName.xmlMapXpath, "//h:SnippetType")]
        public List<String> snippetTypeStrings
        {
            get { return _snippetTypeStrings; }
            set
            {
                _snippetTypeStrings = value;
                OnPropertyChanged("snippetTypeStrings");
            }
        }

        #endregion --- snippetTypeStrings ------- String lista snippet tipova

        #region --- name ------- naziv snippeta

        private String _name;

        /// <summary>
        /// naziv snippeta
        /// </summary>
        [imb(imbAttributeName.xmlMapXpath, @"//h:Title")]
        public String name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        #endregion --- name ------- naziv snippeta

        #region --- shortcut ------- precica koja ga aktivira

        private String _shortcut;

        /// <summary>
        /// precica koja ga aktivira
        /// </summary>
        [imb(imbAttributeName.xmlMapXpath, @"//h:Shortcut")]
        public String shortcut
        {
            get { return _shortcut; }
            set
            {
                _shortcut = value;
                OnPropertyChanged("shortcut");
            }
        }

        #endregion --- shortcut ------- precica koja ga aktivira

        #region --- description ------- Opis snippeta

        private String _description;

        /// <summary>
        /// Opis snippeta
        /// </summary>
        [imb(imbAttributeName.xmlMapXpath, @"//h:Description")]
        public String description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }

        #endregion --- description ------- Opis snippeta

        #region --- code ------- kod koji se ubacuje

        private String _code;

        /// <summary>
        /// kod koji se ubacuje
        /// </summary>
        [imb(imbAttributeName.xmlMapXpath, @"//h:Code")]
        public String code
        {
            get { return _code; }
            set
            {
                _code = value;
                OnPropertyChanged("code");
            }
        }

        #endregion --- code ------- kod koji se ubacuje

        #region --- literals ------- Kolekcija literala - parametara

        private aceCollection<snippetParameter> _literals; // = new aceCollection<snippetParameter>();

        /// <summary>
        /// Kolekcija literala - parametara
        /// </summary>
        [imb(imbAttributeName.xmlMapXpath, @"//h:Literal")]
        public aceCollection<snippetParameter> literals
        {
            get { return _literals; }
            set
            {
                _literals = value;
                OnPropertyChanged("literals");
            }
        }

        #endregion --- literals ------- Kolekcija literala - parametara

        /// <summary>
        /// default object konstruktor
        /// </summary>
        public snippet()
        {
        }

        public Boolean deploy(FileInfo fi)
        {
            filepath = fi.FullName;

            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(filepath);

            throw new NotImplementedException();

            //  XPathDocument document = new XPathDocument(fi.FullName);

            //imbNamespaceSetup ins = new imbNamespaceSetup(xdoc, "h");

            //  XPathNavigator navigator = xdoc.CreateNavigator();

            //imbTypeInfo iTI = typeof (snippet).getTypology();

            //snippet _snip = imbXmlEntityTools.setObject(xdoc, this, iTI) as snippet;

            //xdoc.setObject(this);

            return false;//_snip is snippet;

            //    XPathNavigator nav = xdoc.CreateNavigator();
            //imbXPathQuery imbq = new imbXPathQuery("/");
        }
    }
}