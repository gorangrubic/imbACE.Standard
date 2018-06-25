namespace imbACE.Network.web.result
{
    using imbSCI.Data.data;

    // using Newtonsoft.Json;

    #region imbVeles using

    using System;

    #endregion imbVeles using

    /// <summary>
    /// imbVeles Web Header
    /// </summary>

    public class webHeader : imbBindable
    {
        public webHeader()
        {
        }

        public webHeader(String input, String __value)
        {
            requestHeader = input;
            value = __value;
        }

        #region --- requestHeader ------- tip hedera

        private String _requestHeader;

        /// <summary>
        /// tip hedera
        /// </summary>

        public String requestHeader
        {
            get { return _requestHeader; }
            set
            {
                _requestHeader = value;
                OnPropertyChanged("requestHeader");
            }
        }

        #endregion --- requestHeader ------- tip hedera

        #region --- value ------- Podatak koji je upisan

        private String _value;

        /// <summary>
        /// Podatak koji je upisan
        /// </summary>

        public String value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("value");
            }
        }

        #endregion --- value ------- Podatak koji je upisan
    }
}