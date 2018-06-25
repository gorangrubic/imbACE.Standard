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
    /// Collection of
    /// </summary>
    /// <seealso cref="System.Collections.Generic.List{imbACE.Generator.codeGenerator.definitions.projectTargetTask}" />
    public static class CodeGenerationForProjectDefinition
    {

        //public projectTargetTaskCollection()
        //{

        //}

        //public projectTargetTaskCollection(IEnumerable<projectTargetTask> items)
        //{
        //    AddRange(items);
        //}




        /// <summary>
        /// Gets the list of target frameworks, as required by PropertyGroup->TargetFrameworks element of Visual Studio project definitions
        /// </summary>
        /// <returns></returns>
        public static String GetTargetFrameworksValue(this IEnumerable<projectTargetTask> list)
        {
            String output = "";
            foreach (var task in list)
            {
                output = output.add(task.targetName, ";");

            }
            return output;
        }
    }

}