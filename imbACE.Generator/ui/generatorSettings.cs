using System;
using System.Collections.Generic;
using System.Text;

namespace imbACE.Generator.ui
{

    public class generatorSettings
    {

        public Int32 EntryHeight { get; set; } = 24;
        public Int32 LabelHeight { get; set; } = 30;

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public generatorOptions options { get; set; } = generatorOptions.detailPreset;
    }

}