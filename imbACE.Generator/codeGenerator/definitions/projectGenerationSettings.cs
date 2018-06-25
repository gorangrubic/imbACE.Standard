using imbACE.Core.application;
using imbSCI.Core.collection;
using imbSCI.Core.collection.checkLists;
using imbSCI.Core.collection.checkLists;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.text;
using imbSCI.Data;
using imbSCI.Data.data;
using imbSCI.Data.interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace imbACE.Generator.codeGenerator.definitions
{




    /// <summary>
    /// Settings consumed by imbACE project templates
    /// </summary>
    public abstract class projectGenerationSettings
    {


        private acePackageInfo _applicationInfo = new acePackageInfo();
        /// <summary> </summary>
        public acePackageInfo applicationInfo
        {
            get
            {
                return _applicationInfo;
            }
            set
            {
                _applicationInfo = value;

            }
        }

        public Dictionary<string, string> DeployTemplateData(Dictionary<string, string> replacementsDictionary = null)
        {
            Dictionary<string, string> output = replacementsDictionary;
            if (output == null) output = new Dictionary<string, string>();

            PropertyCollectionExtended applicationInfo_pce = applicationInfo.buildPCE(true);
            foreach (PropertyEntry pe in applicationInfo_pce)
            {
                output.Add("app_" + pe.keyName, pe[PropertyEntryColumn.entry_value].toStringSafe());
            }

            return output;

            // applicationInfo_pce.GetStringDictionary();
        }


        public List<classGenerationTask> itemsToGenerate { get; set; } = new List<classGenerationTask>();
        public List<packageOptionTask> packages { get; set; } = new List<packageOptionTask>();

        public List<projectTargetTask> targets { get; set; } = new List<projectTargetTask>();

        protected projectGenerationSettings()
        {
            //itemsToGenerate = new instanceCheckList<classGenerationTask>(new List<classGenerationTask>());
            //packages = new instanceCheckList<packageOptionTask>(new List<packageOptionTask>());
            //targets = new instanceCheckList<projectTargetTask>(new List<projectTargetTask>());
        }

    }

}