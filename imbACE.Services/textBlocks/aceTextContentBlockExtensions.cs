// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceTextContentBlockExtensions.cs" company="imbVeles" >
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
    using imbACE.Core.extensions;
    using imbACE.Services.textBlocks.enums;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using System;

    public static class aceTextContentBlockExtensions
    {
        public static String toKeyString(this ConsoleKeyInfo keyInfo)
        {
            String output = "";
            if (keyInfo.Modifiers != 0)
            {
                output = keyInfo.Modifiers.ToString().add(keyInfo.Key.ToString(), "+");
            }
            else
            {
                output = keyInfo.Key.ToString();
            }
            return output.ToLower().Trim();
        }

        public static Boolean isKeyMatch(this ConsoleKeyInfo keyInfo, String keyToMatch)
        {
            String output = keyInfo.toKeyString().ToLower().Trim();
            return output == keyToMatch.ToLower().Trim();
        }

        /// <summary>
        /// Osigurava trimovanje tacno na zadatu duzinu
        /// </summary>
        /// <param name="__textToInsert">Tekst koji treba da se trimuje</param>
        /// <param name="widthLimit"></param>
        /// <param name="horizontal"></param>
        /// <returns></returns>
        public static String trimToWidth(this String __textToInsert, Int32 widthLimit, printHorizontal horizontal)
        {
            String output = __textToInsert;
            if (__textToInsert.Length > widthLimit)
            {
                switch (horizontal)
                {
                    case printHorizontal.left:
                        output = output.Substring(0, widthLimit);
                        //output = output.overwrite(innerLeftPosition, insertLen, __textToInsert);
                        break;

                    case printHorizontal.middle:
                        Int32 diff = (__textToInsert.Length - widthLimit) / 2;
                        Int32 len = (__textToInsert.Length - widthLimit);
                        if (len > widthLimit)
                        {
                            len -= widthLimit - len;
                        }
                        output = output.Substring(diff, len);

                        break;

                    case printHorizontal.right:
                        Int32 len2 = (__textToInsert.Length - widthLimit);
                        output = output.Substring(len2);
                        break;

                    default:
                        break;
                }
            }
            else
            {
                return output;
            }
            return output;
        }
    }
}