namespace imbACE.Network.tools
{
    #region imbVELES USING

    using System;
    using System.Collections.Generic;

    #endregion imbVELES USING

    /// <summary>
    /// imbEngine koji izvrsava odredjenu logiku koristeci adekvatan imbProjectResource derivat
    /// </summary>
    public static class imbCountryInfoEngine
    {
        /// <summary>
        /// Country Name Versus ImbCountryInfoEntry
        /// </summary>
        public static Dictionary<string, imbCountryInfoEntry> countryBase =
            new Dictionary<string, imbCountryInfoEntry>();

        /// <summary>
        /// Country code versus imbCountryInfoEntry
        /// </summary>
        public static Dictionary<string, imbCountryInfoEntry> countryBasePerCode =
            new Dictionary<string, imbCountryInfoEntry>();

        public static String getAllCountriesCode()
        {
            String output = "";
            foreach (String key in countryBasePerCode.Keys)
            {
                output += key.ToUpper() + "|" + countryBasePerCode[key].countryName + ";";
            }

            return output;
        }

        #region <---------------- ENGINE METHODS ---------------->

        /// <summary>
        /// Izvrsava predmet Enginea
        /// </summary>
        /// <param name="itemToExecute"></param>
        public static imbCountryInfoEntry findCountry(String needle)
        {
            needle = needle.ToUpper().Trim();

            //needle = imbData.imbFilterModuleWorks.imbFilterModuleEngine.filterString(needle, imbXCommand_FilterType.trimElements, "oL", "w");

            imbCountryInfoEntry output = null;

            output = countryBase[needle];
            if (output == null)
            {
                output = countryBasePerCode[needle];
            }
            return output;
        }

        #endregion <---------------- ENGINE METHODS ---------------->

        #region <---------------- ENGINE INITIATION ---------------->

        /// <summary>
        /// Da li je vec iniciran engine
        /// </summary>
        public static Boolean isReady = false;

        /// <summary>
        /// pozivati pri svakom pozivanju enginea
        /// </summary>
        public static void checkEngine()
        {
            if (!isReady)
            {
                initiate();
            }
        }

        /// <summary>
        /// Inicira engine ako do sada nije iniciran
        /// </summary>
        public static void initiate()
        {
            countryBase = new Dictionary<string, imbCountryInfoEntry>();
            countryBasePerCode = new Dictionary<string, imbCountryInfoEntry>();

            // belezi da je spreman
            isReady = true;
        }

        /// <summary>
        /// Ucitava podatke o drzavi na osnovu domain managera
        /// </summary>
        /// <param name="myDomainManager"></param>
        public static void initFromDomainManager()
        {
            initiate();
            try
            {
                /*
                foreach (imbTopLevelDomain tmpDomain in imbDomainManager.allDomainsUnique)
                {
                    if (!(countryBase.ContainsKey(tmpDomain.countryName)))
                    {
                        imbCountryInfoEntry tmpEntry = new imbCountryInfoEntry();
                        tmpEntry.learnFromTLD(tmpDomain);
                        countryBase.Add(tmpDomain.countryName.ToUpper(), tmpEntry);
                        countryBasePerCode.Add(tmpDomain.countryCode.ToUpper(), tmpEntry);
                    }
                }
                 * */
            }
            catch //(Exception ex)
            {
            }
        }

        #endregion <---------------- ENGINE INITIATION ---------------->
    }
}