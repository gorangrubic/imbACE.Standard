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
    /// 
    /// </summary>
    /// <seealso cref="imbACE.Generator.codeGenerator.definitions.codeGeneratorTaskBase" />
    /// <seealso cref="imbSCI.Data.interfaces.IObjectWithName" />
    public class packageOptionTask : codeGeneratorTaskBase, IObjectWithName
    {
        /// <summary>
        /// Gets or sets the targets.
        /// </summary>
        /// <value>
        /// The targets.
        /// </value>
        public List<String> targets { get; set; } = new List<string>();


        private String _name = "System.Drawing.Primitives";
        /// <summary> Full namespace name</summary>
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

        public String HintPath { get; set; }

        private String _Version = "";
        /// <summary> </summary>
        public String Version
        {
            get
            {
                return _Version;
            }
            set
            {
                _Version = value;
                OnPropertyChanged("Version");
            }
        }


    }

}