// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbNLPbasic.cs" company="imbVeles" >
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
    #region imbVeles using

    using imbACE.Core.enums;
    using imbSCI.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion imbVeles using

    public static class imbNLPbasic
    {
        /// <summary>
        /// vraca zajednicki koren reci - jos nije implementirano
        /// </summary>
        public static String getRoot(List<String> input)
        {
            String output = ""; // = new String();

            if (input == null)
            {
                //  input = new List<String>();
            }

            return output;
        }

        /// <summary>
        /// Filtrira tokene prema zadatim pode�avanjima
        /// </summary>
        /// <param name="output"></param>
        /// <param name="_settings"></param>
        /// <returns></returns>
        public static String[] tokenFilter(String[] output, imbNLPsettings _settings)
        {
            return tokenFilter(output, _settings.trimTokens, _settings.minLength, _settings.onlyUnique,
                               _settings.toLowerCase);
        }

        private static int compareCount(IGrouping<String, String> x, IGrouping<String, String> y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal.
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater.
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    if (x.Count() > y.Count())
                    {
                        return 1;
                    }
                    else if (x.Count() == y.Count())
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }

        /// <summary>
        /// Mo�e da se koristi za bilo koji niz stringova
        /// </summary>
        /// <param name="output"></param>
        /// <param name="trimOutput"></param>
        /// <param name="tokenLenLimit"></param>
        /// <param name="onlyUnique"></param>
        /// <param name="toLoweCase"></param>
        /// <returns></returns>
        public static String[] tokenFilter(String[] output, Boolean trimOutput = true, Int32 tokenLenLimit = 0,
                                           Boolean onlyUnique = false, Boolean toLoweCase = false)
        {
            if (output.Count() == 0) return output;

            // String[] filtered;

            if (trimOutput)
            {
                for (Int32 i = 0; i < output.Length; i++)
                {
                    output[i] = output[i].Trim();
                }
                //output.ForEach(x => x = x.Trim());
            }

            if (tokenLenLimit > 0)
            {
                output = output.Where<String>(x => x.Length > tokenLenLimit).ToArray();
            }

            if (toLoweCase)
            {
                for (Int32 i = 0; i < output.Length; i++)
                {
                    output[i] = output[i].ToLower();
                }
            }

            if (onlyUnique)
            {
                List<String> unique = new List<String>();
                List<IGrouping<String, String>> groups = new List<IGrouping<string, string>>();

                groups.AddRange(output.GroupBy<string, String>(x => x));

                groups.Sort(compareCount);
                groups.Reverse();

                foreach (IGrouping<String, String> g in groups)
                {
                    unique.Add(g.Key);
                }

                output = unique.ToArray();
            }
            return output;
        }

        /// <summary>
        /// Univerzalni osnovni split mehanizam
        /// </summary>
        /// <param name="input"></param>
        /// <param name="splitLevel"></param>
        /// <returns></returns>
        public static String[] split(String input, defaultSplitingLevel splitLevel)
        {
            Char[] spliter = "".ToCharArray();
            switch (splitLevel)
            {
                case defaultSplitingLevel.lineBased:
                    spliter = Environment.NewLine.ToCharArray();
                    break;

                case defaultSplitingLevel.sentenceBased:
                    spliter = sentenceSeparators.ToArray();
                    break;

                case defaultSplitingLevel.tokenBased:
                    spliter = tokenSeparators.ToArray();
                    break;
            }
            String[] output = input.Split(spliter, StringSplitOptions.RemoveEmptyEntries);
            return output;
        }

        /// <summary>
        /// Univerzalni osnovni split mehanizam
        /// </summary>
        /// <param name="input"></param>
        /// <param name="splitLevel"></param>
        /// <returns></returns>
        public static String unsplit(String[] input, defaultSplitingLevel splitLevel)
        {
            String spliter = "";
            switch (splitLevel)
            {
                case defaultSplitingLevel.lineBased:
                    spliter = Environment.NewLine;
                    break;

                case defaultSplitingLevel.sentenceBased:
                    spliter = sentenceSeparators[0].ToString();
                    break;

                case defaultSplitingLevel.tokenBased:
                    spliter = tokenSeparators[0].ToString();
                    break;
            }
            String output = "";
            foreach (var st in input)
            {
                imbSciStringExtensions.add(output, st, spliter);
            }

            return output;
        }

        /// <summary>
        /// 2013s> osnovni tokenize mehanizam
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String[] tokenize(String input)
        {
            String[] output = input.Split(tokenSeparators.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            return output;
        }

        /// <summary>
        /// 2013a> osnovni sentencenize mehanizam
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static String[] sentencenize(String input)
        {
            String[] output = input.Split(sentenceSeparators.ToArray(), StringSplitOptions.RemoveEmptyEntries);
            return output;
        }

        #region --- tokenSeparators ------- svi karakteri koji mogu biti token separatori

        private static List<Char> _tokenSeparators;

        /// <summary>
        ///  svi karakteri koji mogu biti token separatori
        /// </summary>
        public static List<Char> tokenSeparators
        {
            get
            {
                if (_tokenSeparators == null)
                {
                    _tokenSeparators = new List<char>();
                    _tokenSeparators.AddRange(" .,!?-".ToCharArray());
                }
                return _tokenSeparators;
            }
            set { _tokenSeparators = value; }
        }

        #endregion --- tokenSeparators ------- svi karakteri koji mogu biti token separatori

        #region --- _sentenceSeparators ------- svi karakteri koji mogu biti token separatori

        private static List<Char> _sentenceSeparators;

        /// <summary>
        ///  svi karakteri koji mogu biti token separatori
        /// </summary>
        public static List<Char> sentenceSeparators
        {
            get
            {
                if (_sentenceSeparators == null)
                {
                    _sentenceSeparators = new List<char>();
                    _sentenceSeparators.AddRange(".!?".ToCharArray());
                }
                return _tokenSeparators;
            }
            set { _sentenceSeparators = value; }
        }

        #endregion --- _sentenceSeparators ------- svi karakteri koji mogu biti token separatori
    }
}