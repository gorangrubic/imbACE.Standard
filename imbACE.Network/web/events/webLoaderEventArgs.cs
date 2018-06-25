namespace imbACE.Network.web.events
{
    #region imbVeles using

    using System;

    #endregion imbVeles using

    /// <summary>
    /// Objekat koji opisuje dogadjaj koji se desio objektu: webLoader
    /// </summary>
    public class webLoaderEventArgs : EventArgs
    {
        private webLoaderEventType _type = webLoaderEventType.unknown;

        public webLoaderEventArgs()
        {
        }

        public String message { get; set; }

        public webLoaderEventArgs(webLoaderEventType __type, String __message = "")

        {
            message = __message;
            _type = __type;
        }

        /// <summary>
        /// Tip dogadjaja - ne moze da se prepravlja kasnije
        /// </summary>
        public webLoaderEventType type { get; set; }
    }
}