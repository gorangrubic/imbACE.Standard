namespace imbACE.Network.web.result
{
    using HtmlAgilityPack;
    using imbACE.Network.web.cache;
    using imbACE.Network.web.request;
    using System;
    using System.Xml.Serialization;

    /// <summary>
    /// Osnovna verzija Rezultata
    /// </summary>
    public class webResult : webResultBase
    {
        public webResult()
        {
        }

        public webResult(webRequest __request)
        {
            request = __request;
        }

        /// <summary>
        /// Building webResult from <see cref="cacheResponse"/>
        /// </summary>
        /// <param name="__response">The response.</param>
        public webResult(cacheResponse __response)
        {
            _byteSize = System.Text.ASCIIEncoding.Unicode.GetByteCount(__response.content);
            document.deploySource(__response.content, enums.webRequestActionType.localFile);
            response = __response.httpContent;
            request = new webRequestFile(response.responseUrl, enums.webRequestActionType.localFile);
        }

        public string httpStatus
        {
            get
            {
                if (response == null) return "unknown";
                return response.statusCode;
            }
        }

        [XmlIgnore]
        public HtmlDocument HtmlDocument
        {
            get
            {
                var output = (HtmlDocument)document.getDocument<HtmlDocument>();
                return output;
            }
        }

        [XmlIgnore]
        public String sourceCode
        {
            get
            {
                if (document == null) return "";
                return document.source;
            }
        }

        /// <summary>
        /// Releases the document from memory
        /// </summary>
        public void releaseDocumentFromMemory()
        {
            document.releaseDocumentFromMemory();
            dataObject = null;
        }

        private Int32 _byteSize = 0;

        /// <summary>
        /// Byte size of the document source code
        /// </summary>
        public Int32 byteSize
        {
            get
            {
                if (_byteSize == 0)
                {
                    if (document != null)
                    {
                        if (document.source != null)
                        {
                            _byteSize = System.Text.ASCIIEncoding.Unicode.GetByteCount(sourceCode);
                        }
                    }
                }
                if (_byteSize == 0)
                {
                }
                return _byteSize;
            }
            set { _byteSize = value; }
        }
    }
}