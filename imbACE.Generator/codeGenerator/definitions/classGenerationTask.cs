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

    /// <summary>
    /// Description of a class generation task
    /// </summary>
    /// <seealso cref="imbSCI.Data.data.imbBindable" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithName" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithDescription" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithNameAndDescription" />
    public class classGenerationTask : codeGeneratorTaskBase, IObjectWithName, IObjectWithDescription, IObjectWithNameAndDescription
    {
        public classGenerationTask()
        {

        }

        public String description { get; set; } = "";

        public String SourceFileName { get; set; } = "";

        public String TargetFileName { get; set; } = "$fileinputname$";

        private String _name = "imbACE Advanced console bundle";
        /// <summary> Human readable - Display name for a class to be generated</summary>
        public String name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }
    }

}