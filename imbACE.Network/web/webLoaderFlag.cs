using System;

namespace imbACE.Network.web
{
    /// <summary>
    /// Neke dodatne opcije za ucitavanje
    /// </summary>
    [Flags]
    public enum webLoaderFlag
    {
        none = 0,

        /// <summary>
        /// Izvrsice ucitavanje samo ako isti URL / komanda nije poslednja koja je izvrsena
        /// </summary>
        loadOnlyIfNotLoaded = 1,
    }
}