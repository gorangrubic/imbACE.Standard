namespace imbACE.Network.web
{
    using imbACE.Network.web.request;
    using imbACE.Network.web.result;

    public class webContentAndRequest
    {
        private webRequest _request;

        /// <summary>
        ///
        /// </summary>
        public webRequest request
        {
            get { return _request; }
            set { _request = value; }
        }

        private webResult _result;

        /// <summary>
        ///
        /// </summary>
        public webResult result
        {
            get { return _result; }
            set { _result = value; }
        }
    }
}