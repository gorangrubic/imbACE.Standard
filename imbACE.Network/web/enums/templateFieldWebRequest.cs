namespace imbACE.Network.web.enums
{
    /// <summary>
    /// Reporting template names
    /// </summary>
    public enum templateFieldWebRequest
    {
        req_httpStatus,
        req_server,
        req_type,
        req_method,

        res_responseUrl,
        res_statusCode,
        res_statusDesc,
        res_contentType,
        res_headers,
        res_type,
        res_charset,
        res_server,
        res_encoding,

        /// <summary>
        /// String report on headers loaded with result
        /// </summary>
        res_headerReport,

        /// <summary>
        /// Cookies loaded with result
        /// </summary>
        res_cookiesReport,

        doc_documentType,
        doc_nsPrefix,
        doc_hasDocument,

        /// <summary>
        /// Original source
        /// </summary>
        doc_source,

        /// <summary>
        /// Normalized document source
        /// </summary>
        doc_sourceNormalized,

        head_requestHeader,
        head_value,
    }
}