namespace imbACE.Network.web.core
{
    #region imbVeles using

    using System;
    using System.Collections.Generic;

    #endregion imbVeles using

    /// <summary>
    /// 2013c: Kolekcija domenskih elemenata - koristi se za poddomene itd
    /// </summary>
    public class domainElementCollection : Dictionary<String, domainElement>
    {
        public domainElement Add(String url, domainElementPosition position = domainElementPosition.sub)
        {
            domainElement tmp = new domainElement(url, position);
            Add(url, tmp);
            return tmp;
        }
    }
}