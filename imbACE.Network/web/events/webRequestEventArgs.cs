namespace imbACE.Network.web.events
{
    #region imbVeles using

    using System;

    #endregion imbVeles using

    /// <summary>
    /// Objekat koji opisuje dogadjaj koji se desio objektu: webRequest
    /// </summary>
    public class webRequestEventArgs : EventArgs
    {
        public webRequestEventArgs()
        {
        }

        public String message { get; set; }

        public webRequestEventArgs(webRequestEventType __type, String __message = "")
        {
            type = __type;
            message = __message;
        }

        /// <summary>
        /// Tip dogadjaja - ne moze da se prepravlja kasnije
        /// </summary>
        public webRequestEventType type { get; set; }
    }
}