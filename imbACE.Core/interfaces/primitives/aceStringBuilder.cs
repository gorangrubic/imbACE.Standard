// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceStringBuilder.cs" company="imbVeles" >
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
    using System;
    using System.Text;

    /// <summary>
    /// StringBuilder with string return (2017:ne koristi se:samo ideja)
    /// </summary>
    public class aceStringBuilder
    {
        private StringBuilder sb = new StringBuilder();

        private StringBuilder current = new StringBuilder();

        private String last = "";

        public String Append(Object input)
        {
            if (input is String)
            {
                return Append((String)input);
            }
            else
            {
                return Append(input.ToString());
            }
        }

        public String Append(String input)
        {
            current.Append(input);
            sb.Append(input);
            return input;
        }

        public String AppendLine(String input)
        {
            last = current.ToString();
            current.Clear();
            current.Append(input);
            sb.AppendLine(input);
            return last;
        }

        public void Clear()
        {
            sb.Clear();
        }

        public String ToString(Boolean Flush = false)
        {
            String output = sb.ToString();
            if (Flush) sb.Clear();
            return output;
        }
    }
}