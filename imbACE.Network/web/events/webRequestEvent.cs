namespace imbACE.Network.web.events
{
    #region imbVeles using

    using imbACE.Network.web.request;

    #endregion imbVeles using

    /// <summary>
    /// Delegat eventa
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void webRequestEvent(webRequest sender, webRequestEventArgs args);
}