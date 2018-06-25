namespace imbACE.Network.web.context
{
    #region imbVeles using

    using imbACE.Network.web.core;
    using imbACE.Network.web.request;
    using imbSCI.Data.data;

    #endregion imbVeles using

    /// <summary>
    /// 2013c: LowLevel resurs
    /// </summary>
    public class webRequestContext : imbBindable
    {
        private webRequest _request;
        private webLoaderSettings _settings;

        public webRequestContext()
        {
        }

        public webRequestContext(webLoaderSettings __settings)
        {
            settings = __settings;
        }

        public webLoaderSettings settings
        {
            get { return _settings; }
            set { _settings = value; }
        }

        public webRequest request
        {
            get { return _request; }
            set { _request = value; }
        }
    }
}