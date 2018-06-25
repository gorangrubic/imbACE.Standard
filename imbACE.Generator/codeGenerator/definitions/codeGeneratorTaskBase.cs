using imbSCI.Core.collection.checkLists;
using imbSCI.Core.collection.checkLists;
using imbSCI.Data;
using imbSCI.Data.data;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace imbACE.Generator.codeGenerator.definitions
{

    public abstract class codeGeneratorTaskBase : imbBindable
    {

        public Boolean IsEnabled { get; set; } = true;

    }

}