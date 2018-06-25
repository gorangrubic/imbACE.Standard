namespace imbACE.Network.web.cache
{
    using System.IO;
    using imbACE.Core.core.diagnostic;
    using imbACE.Network.web.result;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.math;
    using imbSCI.Data;
    using imbSCI.Data.data;

    #region imbVeles using

    using System;

    // using Newtonsoft.Json;
    using imbSCI.Data.enums;
    using System.Text;
    using imbACE.Network.extensions;
    using imbACE.Core.application;
    using imbSCI.Core.files.folders;

    #endregion imbVeles using

    /// <summary>
    /// Cache response for http response and web content
    /// </summary>
    /// <seealso cref="imbCore.core.imbBindable" />
    public class cacheResponse : imbBindable
    {
        public cacheResponse()
        {
        }

        #region --- content ------- sadrzaj

        private String _content;

        /// <summary>
        /// sadrzaj
        /// </summary>
        public String content
        {
            get { return _content; }
            set
            {
                _content = value;
                OnPropertyChanged("content");
            }
        }

        #endregion --- content ------- sadrzaj

        #region --- httpContent ------- httpVezani sadrzaj

        private webResponse _httpContent;

        /// <summary>
        /// httpVezani sadrzaj
        /// </summary>
        public webResponse httpContent
        {
            get { return _httpContent; }
            set
            {
                _httpContent = value;
                OnPropertyChanged("httpContent");
            }
        }

        #endregion --- httpContent ------- httpVezani sadrzaj

        private Boolean _cacheFound;

        /// <summary>
        /// Da li postoji kesiran sadrzaj
        /// </summary>
        public Boolean cacheFound
        {
            get { return _cacheFound; }
            set
            {
                _cacheFound = value;
                OnPropertyChanged("cacheFound");
            }
        }
    }
}