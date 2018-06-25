namespace imbACE.Network.web.result
{
    #region imbVeles using

    using imbACE.Network.web.request;

    #endregion imbVeles using

    public class webResultFile : webResult
    {
        public webResultFile(webRequest __request) : base(__request)
        {
        }

        /// <summary>
        /// Pseudo property: dnsLookupata
        /// </summary>
        public byte[] loadedBytes
        {
            get { return dataObject as byte[]; }
            set { dataObject = value; }
        }
    }
}