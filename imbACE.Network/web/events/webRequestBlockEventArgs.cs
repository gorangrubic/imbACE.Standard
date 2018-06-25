namespace imbACE.Network.web.events
{
    #region imbVeles using

    using System;

    #endregion imbVeles using

    /// <summary>
    /// Objekat koji opisuje dogadjaj koji se desio objektu: webRequestBlock
    /// </summary>
    public class webRequestBlockEventArgs : EventArgs
    {
        private webRequestBlockEventType _type = webRequestBlockEventType.unknown;

        public webRequestBlockEventArgs()
        {
        }

        public String message { get; set; }

        public webRequestBlockEventArgs(webRequestBlockEventType __type, String __message = "")

        {
            _type = __type;
            message = __message;
        }

        /// <summary>
        /// Tip dogadjaja - ne moze da se prepravlja kasnije
        /// </summary>
        public webRequestBlockEventType type { get; set; }
    }
}