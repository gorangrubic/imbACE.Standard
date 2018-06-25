using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbACE.Core.commands
{
    /// <summary>
    /// Defines how executing script line should be rendered at output
    /// </summary>
    public enum scriptLineShowMode
    {
        /// <summary>
        /// The none: nothing is displayed, except comments
        /// </summary>
        none,

        /// <summary>
        /// The undefined: it is not set, using default
        /// </summary>
        undefined,

        /// <summary>
        /// The only code line: only script code line currently executed
        /// </summary>
        onlyCodeLine,

        /// <summary>
        /// The code number and code line: script code line with line number prefix
        /// </summary>
        codeNumberAndCodeLine,

        /// <summary>
        /// The full prefix and code line currently executing
        /// </summary>
        fullPrefixAndCodeLine,
    }
}