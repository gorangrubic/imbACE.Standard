namespace imbACE.Network.web.context
{
    #region imbVeles using

    using imbACE.Network.web.result;
    using imbSCI.Data.data;

    #endregion imbVeles using

    public class webResultContext : imbBindable
    {
        private webLoader _loader;
        private webResultBase _result;

        #region IContextForWebResult Members

        public webResultBase result
        {
            get { return _result; }
            set { _result = value; }
        }

        public webLoader loader
        {
            get { return _loader; }
            set { _loader = value; }
        }

        #endregion IContextForWebResult Members
    }
}