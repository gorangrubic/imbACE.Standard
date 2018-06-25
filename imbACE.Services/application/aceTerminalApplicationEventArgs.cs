using System;
using System.Collections.Generic;
using System.Linq;

namespace imbACE.Services.application
{
    using imbACE.Core;
    using imbACE.Core.events;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.core;
    using imbACE.Services.textBlocks;
    using imbACE.Services.textBlocks.enums;
    using imbACE.Services.textBlocks.input;
    using imbACE.Services.textBlocks.smart;
    using imbSCI.Core;
    using imbSCI.Core.attributes;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting.zone;
    using imbSCI.Data;
    using imbSCI.Data.collection;
    using imbSCI.Data.data;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex;
    using imbSCI.Reporting;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.interfaces;
    using System.Collections;
    using System.Globalization;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Event arguments for the aceTerminalApplication
    /// </summary>
    /// <seealso cref="imbACE.Core.events.aceEventGeneralArgs" />
    public class aceTerminalApplicationEventArgs : aceEventGeneralArgs
    {
        public IAceTerminalScreen screen
        {
            get
            {
                return this.RelatedObject as IAceTerminalScreen;
            }
            set
            {
                this.RelatedObject = value;
            }
        }
    }
}