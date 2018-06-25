namespace imbACE.Core.cache
{
    using System;

    public interface ICacheEnabled
    {
        /// <summary>
        /// Saves the cache.
        /// </summary>
        /// <returns></returns>
        String saveCache();

        /// <summary>
        /// Loads cache for <c>instanceID</c> into this object
        /// </summary>
        /// <param name="instanceID">The instance identifier.</param>
        /// <returns></returns>
        cacheResponseForType loadCache(String instanceID);

        /// <summary>
        /// Gets the cache instance identifier.
        /// </summary>
        /// <value>
        /// The cache instance identifier.
        /// </value>
        String cacheInstanceID { get; }
    }
}