namespace imbACE.Network.web.events
{
    /// <summary>
    /// Tip dogadjaja - za objekat: webRequestBlock
    /// </summary>
    public enum webRequestBlockEventType
    {
        unknown,
        error,

        executedAllError,
        executedAllOk,
        executedWithErrors,

        progressReport,
        aborted,
    }
}