namespace imbACE.Network.tools
{
    using imbACE.Core.core;
    using imbSCI.Core.extensions.data;
    using System;
    using System.Linq;

    //public class domainAnalysis
    //{
    //}

    public class domainAnalysisEngine
    {
        //public static imbTopLevelDomain getDomainDescription(String domainNameOrUrl, Boolean inputIsFullUrl = true)
        //{
        //    String tmpTld = domainNameOrUrl;
        //    String domainName = tmpTld.getDomainNameFromUrl(false);

        //    List<String> parts = domainName.Split(".".ToArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
        //    parts.Reverse();

        //    if (parts.Count > 2)
        //    {
        //        tmpTld = parts[1] + "." + parts[0];
        //    }
        //    else if (parts.Count == 2)
        //    {
        //        tmpTld = parts[0];
        //    }
        //    else if (parts.Count == 1)
        //    {
        //        domainRootName = parts[0];
        //        isFound = false;
        //        tld = "";
        //        return;
        //    }

        //}

        /// <summary>
        /// Vraca TLD definiciju za upit: co.rs
        /// </summary>
        /// <param name="tldInput"></param>
        /// <returns></returns>
        public static imbTopLevelDomain getDomain(String tldInput)
        {
            imbTopLevelDomain tldObject = getExactDomain(tldInput);

            return tldObject;
        }

        public static Boolean isTLDMatch(imbTopLevelDomain tld, String input)
        {
            if (tld == null) return false;

            if (tld.domainName == input) return true;

            if (tld.subTLDs.Contains(input)) return true;

            //if (!tld.subDomainList.isNullOrEmptyString())
            //{
            //    if (tld.subDomainList.Contains(input))
            //    {
            //        return true;
            //    } else
            //    {
            //    }
            //}
            return false;
        }

        /// <summary>
        /// Gets the exact domain.
        /// </summary>
        /// <param name="tldInput">The TLD input.</param>
        /// <returns></returns>
        private static imbTopLevelDomain getExactDomain(String tldInput)
        {
            if (tldInput.isNullOrEmpty()) return null;
            tldInput = tldInput.Trim().ToLower().Trim();
            tldInput = tldInput.TrimStart(".".ToArray());

            imbTopLevelDomain tldObject = imbDomainManager.AllDomains.Find(x => isTLDMatch(x, tldInput)); // //(x => (x.domainName == tldInput) || (x.subDomainList.Contains(tldInput)));

            if (tldObject == null)
            {
                tldObject = systemKnowledge.topLevelDomains.GetFirstWhere(nameof(imbTopLevelDomain.domainName) + "='" + tldInput + "'");

                if (tldObject == null)
                {
                    tldObject = systemKnowledge.topLevelDomains.GetFirstWhere(nameof(imbTopLevelDomain.subDomainList) + " LIKE '%" + tldInput + "%'");
                    if (tldObject == null)
                    {
                    }
                }

                if (tldObject != null)
                {
                    imbDomainManager.AllDomains.Add(tldObject);
                    aceLog.log("TLD found [" + tldObject.domainName + "] for query [" + tldInput + "]");
                }
                else
                {
                    aceLog.log("TLD not found for query [" + tldInput + "]");
                }
            }
            return tldObject;
        }
    }
}