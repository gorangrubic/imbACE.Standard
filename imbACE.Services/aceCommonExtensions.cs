// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceCommonExtensions.cs" company="imbVeles" >
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
namespace imbACE.Services
{
    using imbACE.Core.core.exceptions;
    using System;

    public static class aceCommonExtensions
    {
        public static void log(this String message)
        {
            aceCommons.log(message);
        }

        /// <summary>
        /// Generise random string zadate duzine
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string getRandomString(Int32 length = 32)
        {
            String output = "";
            int randNumber;
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                if (random.Next(1, 3) == 1)
                    randNumber = random.Next(97, 123); //char {a-z}
                else
                    randNumber = random.Next(48, 58); //int {0-9}

                output = output + (char)randNumber;
            }

            return output;
        }
    }
}