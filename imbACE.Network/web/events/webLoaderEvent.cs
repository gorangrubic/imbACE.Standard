namespace imbACE.Network.web.events
{
    #region imbVeles using

    using imbACE.Network.web.core;

    #endregion imbVeles using

    /// <summary>
    /// Delegat eventa
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void webLoaderEvent(webLoaderBase sender, webLoaderEventArgs args);
}