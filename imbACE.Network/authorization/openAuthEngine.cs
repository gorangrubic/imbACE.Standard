// --------------------------------------------------------------------------------------------------------------------
// <copyright file="openAuthEngine.cs" company="imbVeles" >
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
// Project: imbACE.Network
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------

namespace imbACE.Network.authorization
{
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

    #region imbVeles using

    using System;

    using System.Net;

    #endregion imbVeles using

    /// <summary>
    /// PREISPITATI UPOTREBU
    /// </summary>
    public class openAuthEngine
    {
        #region oauth_headers enum

        public enum oauth_headers
        {
            oauth_version,
            oauth_nonce,
            oauth_timestamp,
            oauth_consumer_key,
            oauth_signature_method,
            oauth_signature,
            oauth_callback,
        }

        #endregion oauth_headers enum

        public static void addOAuthHeader(WebHeaderCollection targetCollection, oauth_headers oauthType,
                                          string value = "", string yahooUrl = "")
        {
            if (value == null)
            {
                switch (oauthType)
                {
                    case oauth_headers.oauth_version:
                        value = "1.0";
                        break;

                    case oauth_headers.oauth_timestamp:
                        DateTime nula = new DateTime(1970, 1, 1);

                        TimeSpan vreme = DateTime.Now.Subtract(nula);
                        int ts = Math.Abs((int)vreme.TotalSeconds);

                        value = ts.ToString();

                        break;

                    case oauth_headers.oauth_signature_method:
                        //  value = imbSettingsManager.current.yahooSignature;

                        break;

                    case oauth_headers.oauth_nonce:
                        value = imbStringGenerators.getRandomString(32);
                        break;
                }
            }
            targetCollection.Add(oauthType.ToString(), value);
        }
    }
}