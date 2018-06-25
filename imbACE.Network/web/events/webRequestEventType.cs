namespace imbACE.Network.web.events
{
    /// <summary>
    /// Tip dogadjaja - za objekat: webRequest - sluzi i za status
    /// </summary>
    public enum webRequestEventType
    {
        unknown,
        error,
        errorTimeout,
        errorContent,

        scheduled,
        executing,

        loaded,

        waitingCooloff,
        waitingForRetry,
        waitingContentCriteria,

        finalization,
        executedOk,

        callRetry,
        callProxyChange,
    }
}