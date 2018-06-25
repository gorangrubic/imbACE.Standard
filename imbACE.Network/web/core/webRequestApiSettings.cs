namespace imbACE.Network.web.core
{
    #region imbVeles using

    using imbACE.Core.core.diagnostic;
    using imbACE.Network.authorization;
    using imbACE.Network.web.enums;
    using imbSCI.Data.data;
    using System;
    using System.ComponentModel;
    using System.Xml.Serialization;

    #endregion imbVeles using

    /// <summary>
    /// Skup podesavanja za API request
    /// </summary>
    public class webRequestApiSettings : imbBindable
    {
        public const String propCategory = "API settings";

        private String _apiSignatureLine;

        #region imbObject Property <String> apiSignatureLine

        /// <summary>
        /// Linija sa potpisom - temp
        /// </summary>
        [XmlIgnore]
        [Category(propCategory)]
        [Description("Linija sa potpisom - temp")]
        public String apiSignatureLine
        {
            get { return _apiSignatureLine; }
            set
            {
                _apiSignatureLine = value;
                OnPropertyChanged("apiSignatureLine");
            }
        }

        #endregion imbObject Property <String> apiSignatureLine

        #region API RELATED

        private String _apiKey;

        private Int32 _apiPageNumber;
        private Int32 _apiResultLimit = 50;
        private Int32 _apiResultOfset;
        private String _apiSecretKey;

        private String _apiServiceURL;
        private signatureType _apiSignatureType;

        private imbTimeStampFormat _apiTimeStampFormat = imbTimeStampFormat.iso8601;
        private apiType _targetAPI;

        #region imbObject Property <Int32> apiResultLimit

        /// <summary>
        /// API result limit (like SQL limit)
        /// </summary>
        [Category(propCategory)]
        [Description("API result limit (like SQL limit)")]
        public Int32 apiResultLimit
        {
            get { return _apiResultLimit; }
            set
            {
                _apiResultLimit = value;
                OnPropertyChanged("apiResultLimit");
            }
        }

        #endregion imbObject Property <Int32> apiResultLimit

        #region imbObject Property <Int32> apiResultOfset

        /// <summary>
        /// API result ofset (like SQL LIMIT OFSET)
        /// </summary>
        [Category(propCategory)]
        [Description("API result ofset (like SQL LIMIT OFSET)")]
        public Int32 apiResultOfset
        {
            get { return _apiResultOfset; }
            set
            {
                _apiResultOfset = value;
                OnPropertyChanged("apiResultOfset");
            }
        }

        #endregion imbObject Property <Int32> apiResultOfset

        #region imbObject Property <imbTimeStampFormat> apiTimeStampFormat

        /// <summary>
        /// format koji se koristi za vremenski pecat
        /// </summary>
        [Category(propCategory)]
        [Description("format koji se koristi za vremenski pecat")]
        public imbTimeStampFormat apiTimeStampFormat
        {
            get { return _apiTimeStampFormat; }
            set
            {
                _apiTimeStampFormat = value;
                OnPropertyChanged("apiTimeStampFormat");
            }
        }

        #endregion imbObject Property <imbTimeStampFormat> apiTimeStampFormat

        #region imbObject Property <String> apiServiceURL

        /// <summary>
        /// API service URL - koristi se kao osnova za generisanje API request URL-a
        /// </summary>
        [Category(propCategory)]
        [Description("API service URL - koristi se kao osnova za generisanje API request URL-a")]
        public String apiServiceURL
        {
            get { return _apiServiceURL; }
            set
            {
                _apiServiceURL = value;
                OnPropertyChanged("apiServiceURL");
            }
        }

        #endregion imbObject Property <String> apiServiceURL

        #region imbObject Property <signatureType> apiSignatureType

        /// <summary>
        /// Tip potpisa za proces autorizacije
        /// </summary>
        [Category(propCategory)]
        [Description("Tip potpisa za proces autorizacije")]
        public signatureType apiSignatureType
        {
            get { return _apiSignatureType; }
            set
            {
                _apiSignatureType = value;
                OnPropertyChanged("apiSignatureType");
            }
        }

        #endregion imbObject Property <signatureType> apiSignatureType

        #region imbObject Property <Int32> apiPageNumber

        /// <summary>
        /// API call page number to load (option)
        /// </summary>
        [Category(propCategory)]
        [Description("API call page number to load (option)")]
        public Int32 apiPageNumber
        {
            get { return _apiPageNumber; }
            set
            {
                _apiPageNumber = value;
                OnPropertyChanged("apiPageNumber");
            }
        }

        #endregion imbObject Property <Int32> apiPageNumber

        #region imbObject Property <String> apiSecretKey

        /// <summary>
        /// API tajni kljuc za pristup
        /// </summary>
        [Category(propCategory)]
        [Description("API tajni kljuc za pristup")]
        public String apiSecretKey
        {
            get { return _apiSecretKey; }
            set
            {
                _apiSecretKey = value;
                OnPropertyChanged("apiSecretKey");
            }
        }

        #endregion imbObject Property <String> apiSecretKey

        #region imbObject Property <String> apiKey

        /// <summary>
        /// API kljuc za pristup
        /// </summary>
        [Category(propCategory)]
        [Description("API kljuc za pristup")]
        public String apiKey
        {
            get { return _apiKey; }
            set
            {
                _apiKey = value;
                OnPropertyChanged("apiKey");
            }
        }

        #endregion imbObject Property <String> apiKey

        #region imbObject Property <imbAPI> targetAPI

        /// <summary>
        /// API za koji je predvidjen request
        /// </summary>
        [Category(propCategory)]
        [Description("API za koji je predvidjen request")]
        public apiType targetAPI
        {
            get { return _targetAPI; }
            set
            {
                _targetAPI = value;
                OnPropertyChanged("targetAPI");
            }
        }

        #endregion imbObject Property <imbAPI> targetAPI

        #endregion API RELATED
    }
}