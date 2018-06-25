namespace imbACE.Network.web.core
{
    #region imbVeles using

    using imbACE.Network.tools;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    #endregion imbVeles using

    /// <summary>
    /// Skup parametara
    /// </summary>
    public class requestParameterCollection : Dictionary<String, requestParameter>
    {
        #region requestQueryStringType enum

        public enum requestQueryStringType
        {
            template,
            withValues,
        }

        #endregion requestQueryStringType enum

        /// <summary>
        /// Popunjava kolekciju parametara na osnovu URL-a. Moze biti ceo URL ili samo query deo URL-a. Moze biti i template
        /// </summary>
        /// <param name="input">Ulazni URL string - moze ceo, moze i samo query deo, moze da ima anchor itd.</param>
        /// <returns>Broj parametara koji su ucitani u kolekciju</returns>
        public Int32 learnFromUrl(String input)
        {
            Int32 output = 0;
            if (String.IsNullOrEmpty(input)) return -1;

            input = input.Replace(Environment.NewLine, "");

            try
            {
                // Precistavanje ulaznog stringa
                Int32 iQStart = input.IndexOf(imbNetworkTools.URL_QUERYSTART);
                if (iQStart > 0) input = input.Substring(iQStart);

                Int32 iAnchorStart = input.IndexOf(imbNetworkTools.URL_ANCHORSTART);
                if (iAnchorStart > 0) input = input.Substring(0, iAnchorStart);

                input = input.Trim(imbNetworkTools.URL_QUERYSTART.ToArray());

                String[] par = input.Split(imbNetworkTools.URL_PARAMSEPARATOR.ToArray(),
                                           StringSplitOptions.RemoveEmptyEntries);
                foreach (String pl in par)
                {
                    requestParameter rp = new requestParameter(pl);

                    Add(rp.Name, rp);
                    output++;
                }
            }
            catch (Exception ex)
            {
                output = -1;
            }

            return output;
        }

        /// <summary>
        /// Vraca QUERY deo sa parametrima requesta - ako nema vrednosti onda ne prikazuje taj item
        /// </summary>
        /// <returns></returns>
        public String makeGetQueryLine(requestQueryStringType qType = requestQueryStringType.template,
                                       Dictionary<String, Object> externalVariables = null)
        {
            if (Count == 0) return "";

            String output = imbNetworkTools.URL_QUERYSTART;

            foreach (requestParameter rp in this.Values)
            {
                if (rp.useIt)
                {
                    String valueToPut = "";
                    if (externalVariables != null)
                    {
                        valueToPut = externalVariables[rp.Name].ToString();
                    }
                    else
                    {
                        valueToPut = rp.Value;
                    }
                    if (!String.IsNullOrEmpty(valueToPut))
                    {
                        output += rp.Name + imbNetworkTools.URL_PARAMOPERATOR + valueToPut;
                        if (rp.Name != this.Values.Last().Name) output += imbNetworkTools.URL_PARAMSEPARATOR;
                    }
                }
            }

            return output;
        }
    }
}