namespace imbACE.Network.web.core
{
    using imbSCI.Core.attributes;
    using imbSCI.Data.data;

    #region imbVeles using

    using System;
    using System.Linq;

    #endregion imbVeles using

    /// <summary>
    /// Element naziva domena
    /// </summary>
    [imb(imbAttributeName.collectionPrimaryKey, "url")]
    public class domainElement : imbBindable
    {
        public domainElement()
        {
        }

        public domainElement(String __url, domainElementPosition __position = domainElementPosition.sub)
        {
            url = __url.Trim(".".ToArray());
            position = __position;
        }

        #region --- url ------- URL pojavni oblik elementa domena

        private String _url = "www";

        /// <summary>
        /// URL pojavni oblik elementa domena
        /// </summary>
        public String url
        {
            get { return _url; }
            set
            {
                _url = value;
                OnPropertyChanged("url");
            }
        }

        #endregion --- url ------- URL pojavni oblik elementa domena

        #region --- position ------- domainElementPosition

        private domainElementPosition _position = domainElementPosition.sub;

        /// <summary>
        /// domainElementPosition
        /// </summary>
        public domainElementPosition position
        {
            get { return _position; }
            set
            {
                _position = value;
                OnPropertyChanged("position");
            }
        }

        #endregion --- position ------- domainElementPosition
    }
}