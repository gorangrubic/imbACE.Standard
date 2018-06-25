namespace imbACE.Network.web.enums
{
    /// <summary>
    /// Modovi učitavanja
    /// </summary>
    public enum webRequestActionType
    {
        /// <summary>
        /// Otvara URL i pravi Document dom pomocu HtmlAgility packa
        /// </summary>
        openUrl,

        XML,
        HTMLasXML,

        CheckUrlOnly,
        GetHeaders, // prvi ce samo upisati status u target, drugi ce zapisati sve zive podatke koji su dostupni
        Text,
        Download,

        FTPUpload,
        FTPDownload,

        ipResolve,
        dnsResolve, // prvi: pravi listu IP adresa, drugi: ubacuje listu imena

        // postSendAndReceive,// imbSmartLoader
        whoIs,

        //  directHttp,
        //htmlAgility,
        // smartAss,

        localFile,

        None
    }
}