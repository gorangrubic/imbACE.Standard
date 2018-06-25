using System;
using System.Collections.Generic;
using System.Text;

namespace imbACE.Generator.ui
{

    [Flags]
    public enum generatorOptions
    {

        none = 0,

        showTitle = 1,

        showDescription = 2,

        groupByLabel = 4,

        groupByGroupBox = 8,

        contextualHelpInLabel = 16,


        detailPreset = showTitle | showDescription | groupByGroupBox

    }

}