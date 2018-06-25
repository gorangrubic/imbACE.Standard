namespace imbACE.Network.web.result
{
    #region imbVeles using

    using imbACE.Network.web.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting;
    using imbSCI.Data.data;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Net;
    using System.Text;
    using System.Xml.Serialization;

    // using Newtonsoft.Json;

    #endregion imbVeles using

    /// <summary>
    /// imbVeles http/web/file response objekat
    /// </summary>
    public class webResponse : imbBindable, IAppendDataFields
    {
        /// <summary>
        /// Appends its data points into new or existing property collection -- automatically called by webResultBase
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public PropertyCollection AppendDataFields(PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();
            this.buildPropertyCollection(false, false, "res", data);

            StringBuilder report = new StringBuilder();
            foreach (webHeader wh in headers)
            {
                report.AppendLine("> " + wh.requestHeader + "=" + wh.value);
            }
            data[templateFieldWebRequest.res_headerReport] = report;
            report.Clear();
            foreach (var wh in cookies)
            {
                report.AppendLine("> *" + wh.Name + "*");
                report.AppendLine("> > ''" + wh.Value + "''");
                report.AppendLine("> > __" + wh.Comment + "__");
                report.AppendLine("> > T:" + wh.TimeStamp + "");
                report.AppendLine("> > P:" + wh.Port + "");
            }
            data[templateFieldWebRequest.res_cookiesReport] = report;

            //data[templateFieldWebRequest.doc_documentType] = contentType;

            // data[templateFieldWebRequest.target_description] = description;
            // data[templateFieldWebRequest.target_id] = id;
            // data[templateFieldWebRequest.target_url] = url;
            return data;
        }

        #region --- encoding ------- Enkodiranje koje je korišćeno

        private String _encoding;

        /// <summary>
        /// Enkodiranje koje je korišćeno
        /// </summary>
        public String encoding
        {
            get { return _encoding; }
            set
            {
                _encoding = value;
                OnPropertyChanged("encoding");
            }
        }

        #endregion --- encoding ------- Enkodiranje koje je korišćeno

        #region --- server ------- server koji je poslao odgovor

        private String _server;

        /// <summary>
        /// server koji je poslao odgovor
        /// </summary>
        public String server
        {
            get { return _server; }
            set
            {
                _server = value;
                OnPropertyChanged("server");
            }
        }

        #endregion --- server ------- server koji je poslao odgovor

        #region --- cookies ------- lista kukija

        private List<Cookie> _cookies = new List<Cookie>();

        /// <summary>
        /// lista kukija
        /// </summary>
        //[JsonIgnore]
        [XmlIgnore]
        public List<Cookie> cookies
        {
            get { return _cookies; }
            set
            {
                _cookies = value;
                OnPropertyChanged("cookies");
            }
        }

        #endregion --- cookies ------- lista kukija

        #region --- headers ------- Hederi koji su došli uz odgovor

        private List<webHeader> _headers = new List<webHeader>();

        /// <summary>
        /// Hederi koji su došli uz odgovor
        /// </summary>
        //[JsonIgnore]
        public List<webHeader> headers
        {
            get { return _headers; }
            set
            {
                _headers = value;
                OnPropertyChanged("headers");
            }
        }

        #endregion --- headers ------- Hederi koji su došli uz odgovor

        #region --- type ------- Tip web response objekta

        private responseType _type;

        /// <summary>
        /// Tip web response objekta
        /// </summary>
        public responseType type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("type");
            }
        }

        #endregion --- type ------- Tip web response objekta

        #region --- contentType ------- Tip sadržaja koji je učitan

        private String _contentType;

        /// <summary>
        /// Tip sadržaja koji je učitan
        /// </summary>
        public String contentType
        {
            get { return _contentType; }
            set
            {
                _contentType = value;
                OnPropertyChanged("contentType");
            }
        }

        #endregion --- contentType ------- Tip sadržaja koji je učitan

        #region --- responseUrl ------- Adresa sa koje je stvarno došao odgovor

        private String _responseUrl;

        /// <summary>
        /// Adresa sa koje je stvarno došao odgovor
        /// </summary>
        public String responseUrl
        {
            get { return _responseUrl; }
            set
            {
                _responseUrl = value;
                OnPropertyChanged("responseUrl");
            }
        }

        #endregion --- responseUrl ------- Adresa sa koje je stvarno došao odgovor

        #region --- responseDomain ------- Domen sa koga je stigao odgovor

        private String _responseDomain = "";

        /// <summary>
        /// Domen sa koga je stigao odgovor
        /// </summary>
        public String responseDomain
        {
            get { return _responseDomain; }
            set
            {
                _responseDomain = value;
                OnPropertyChanged("responseDomain");
            }
        }

        #endregion --- responseDomain ------- Domen sa koga je stigao odgovor

        #region --- method ------- Metod kojim je napravljen upit

        private String _method;

        /// <summary>
        /// Metod kojim je napravljen upit
        /// </summary>
        public String method
        {
            get { return _method; }
            set
            {
                _method = value;
                OnPropertyChanged("method");
            }
        }

        #endregion --- method ------- Metod kojim je napravljen upit

        #region --- statusCode ------- statusni kod

        private String _statusCode;

        /// <summary>
        /// statusni kod
        /// </summary>
        public String statusCode
        {
            get { return _statusCode; }
            set
            {
                _statusCode = value;
                OnPropertyChanged("statusCode");
            }
        }

        #endregion --- statusCode ------- statusni kod

        #region --- statusDesc ------- opis statusa

        private String _statusDesc;

        /// <summary>
        /// opis statusa
        /// </summary>
        public String statusDesc
        {
            get { return _statusDesc; }
            set
            {
                _statusDesc = value;
                OnPropertyChanged("statusDesc");
            }
        }

        #endregion --- statusDesc ------- opis statusa

        #region --- charset ------- Headeri u odgovoru

        private String _charset;

        /// <summary>
        /// Headeri u odgovoru
        /// </summary>
        public String charset
        {
            get { return _charset; }
            set
            {
                _charset = value;
                OnPropertyChanged("charset");
            }
        }

        #endregion --- charset ------- Headeri u odgovoru

        public webResponse()
        {
            type = responseType.WebResponse;
        }

        /// <summary>
        /// Formira webResponse na osnovu prosledjeno web response-a
        /// </summary>
        /// <param name="response"></param>
        public webResponse(WebResponse response)
        {
            String __typeString = "null";
            responseType __type = responseType.nullResponse;
            _response = response;
            if (response != null)
            {
                __typeString = response.GetType().Name;
                __type = responseType.HttpWebResponse;

                if (!Enum.TryParse<responseType>(__typeString, out __type)) __type = responseType.WebResponse;
            }

            type = __type;
            headers = new List<webHeader>();

            if (response != null)
            {
                foreach (string key in response.Headers.AllKeys)
                {
                    headers.Add(new webHeader(key, response.Headers[key]));
                }

                responseUrl = response.ResponseUri.OriginalString;
                contentType = response.ContentType;

                switch (type)
                {
                    case responseType.PackWebResponse:
                        break;

                    case responseType.FtpWebResponse:
                        break;

                    case responseType.FileWebResponse:
                        FileWebResponse fileRes = response as FileWebResponse;
                        break;

                    case responseType.HttpWebResponse:
                        HttpWebResponse httpRes = response as HttpWebResponse;
                        charset = httpRes.CharacterSet;
                        encoding = httpRes.ContentEncoding;
                        statusCode = httpRes.StatusCode.ToString();
                        statusDesc = httpRes.StatusDescription;
                        cookies = new List<Cookie>();
                        foreach (Cookie ck in httpRes.Cookies) cookies.Add(ck);
                        method = httpRes.Method;
                        server = httpRes.Server;
                        responseDomain = httpRes.ResponseUri.DnsSafeHost;
                        break;
                }
            }
        }

        #region --- _response ------- response objekat koji je dobio pri instanciranju

        private WebResponse __response;

        /// <summary>
        /// response objekat koji je dobio pri instanciranju
        /// </summary>

        [XmlIgnore]
        public WebResponse _response
        {
            get { return __response; }
            set
            {
                __response = value;
                OnPropertyChanged("_response");
            }
        }

        #endregion --- _response ------- response objekat koji je dobio pri instanciranju
    }
}