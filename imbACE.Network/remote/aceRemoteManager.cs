// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceRemoteManager.cs" company="imbVeles" >
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
namespace imbACE.Network.remote
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

    /// <summary>
    /// aceRemote MANAGER
    /// </summary>
    public static class aceRemoteManager
    {
        /*
        public static void reportUnhandledException(Exception e, aceRemoteInstanceBase client) {
            String msg = "Unhandled exception :: " + e.GetType().Name + Environment.NewLine;
            msg += e.Message + Environment.NewLine;

            msg += e.TargetSite + Environment.NewLine;

            msg += e.Source + Environment.NewLine;

            msg += e.StackTrace + Environment.NewLine;

            if (client != null)
            {
                msg += "Instance: " + client.GetType().Name + " " + Environment.NewLine;
                client.log(msg);
            } else
            {
                msg += "Instance: null" + Environment.NewLine;
                File.WriteAllText("_crash_report.txt", msg);
            }
        }

        public static void saveSettings(String filepath, aceRemoteSettings settings)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(aceRemoteSettings));
            TextWriter writer = new StreamWriter(filepath);
            serializer.Serialize(writer, settings);
            writer.Close();
        }

        public static aceRemoteSettings loadSettings(String filepath)
        {
            if (!File.Exists(filepath))
            {
                return new aceRemoteSettings();
            }

            XmlSerializer deserializer = new XmlSerializer(typeof(aceRemoteSettings));
            TextReader reader = new StreamReader(filepath);
            object obj = deserializer.Deserialize(reader);
            aceRemoteSettings output = (aceRemoteSettings)obj;
            reader.Close();
            return output;
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
        */
    }
}