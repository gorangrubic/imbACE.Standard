using imbACE.Core.application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbACE.Core
{
    /// <summary>
    /// Static reference to the application instance
    /// </summary>
    public static class appManager
    {
        public static IAceApplicationBase Application { get; set; } = null;

        /// <summary>
        /// Gets a value indicating whether this instance is ready.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is ready; otherwise, <c>false</c>.
        /// </value>
        public static Boolean IsReady
        {
            get
            {
                return Application != null;
            }
        }

        /// <summary>
        /// Gets the application information.
        /// </summary>
        /// <value>
        /// The application information.
        /// </value>
        public static aceApplicationInfo AppInfo
        {
            get
            {
                if (Application != null) return Application.appAboutInfo;
                return null;
            }
        }

        /// <summary>
        /// Gets the application settings.
        /// </summary>
        /// <value>
        /// The application settings.
        /// </value>
        public static aceApplicationSettings AppSettings
        {
            get
            {
                if (Application != null) return Application.settings;
                return null;
            }
        }
    }
}