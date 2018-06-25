// --------------------------------------------------------------------------------------------------------------------
// <copyright file="signature.cs" company="imbVeles" >
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
    #region imbVeles using

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
    using System;
    using System.Security.Cryptography;
    using System.Text;

    #endregion imbVeles using

    /// <summary>
    /// Authorization signature generators
    /// </summary>
    public static class signature
    {
        /// <summary>
        /// Generate signature according to specified parameters
        /// </summary>
        /// <param name="itemToExecute"></param>
        /// <param name="data"></param>
        /// <param name="urlToSign"></param>
        /// <returns></returns>
        public static string makeSignature(signatureType apiSignatureType, string apiSecretKey, string data,
                                           string urlToSign = "")
        {
            byte[] dataBuffer;
            byte[] hashBytes = null;

            switch (apiSignatureType)
            {
                case signatureType.PLAINTEXT:
                    return data;

                case signatureType.noSignature:
                    return "";

                case signatureType.HMACSHA256:
                    string str_to_sign = "GET\n" + urlToSign + "\n/\n" + data;

                    Console.WriteLine("String To Sign: \n" + str_to_sign);

                    Encoding ae = new UTF8Encoding();
                    HMACSHA256 signature = new HMACSHA256(ae.GetBytes(apiSecretKey));
                    string b64 = Convert.ToBase64String(signature.ComputeHash(ae.GetBytes(str_to_sign.ToCharArray())));
                    return b64;

                case signatureType.HMACSHA1:
                    HMACSHA1 hmacsha1 = new HMACSHA1();
                    hmacsha1.Key = Encoding.ASCII.GetBytes(apiSecretKey);
                    dataBuffer = Encoding.ASCII.GetBytes(data);
                    hashBytes = hmacsha1.ComputeHash(dataBuffer);
                    break;

                case signatureType.RSASHA1:
                    //logSystem.log("Not implemented ::  :: ", logType.FatalError);
                    break;

                default:
                    throw new ArgumentException("Unknown signature type", "signatureType");
            }

            return Convert.ToBase64String(hashBytes); // mozda posle bude trebalo da se napravi Base64 da bude promenjiv
        }
    }
}