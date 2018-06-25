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
    /// Platform target definition for Visual Studio project file (.csproj)
    /// </summary>
    /// <seealso cref="imbSCI.Data.data.imbBindable" />
    public class projectTargetTask : codeGeneratorTaskBase, IObjectWithName
    {
        /// <summary>
        /// Initializes an empty instance of the <see cref="projectTargetTask"/> class.
        /// </summary>
        public projectTargetTask()
        {

        }

        /// <summary>
        /// XML node with compiler constant / flag 
        /// </summary>
        /// <returns></returns>
        public String GetDefineConstantsXML()
        {

            String output = "<PropertyGroup Condition=\" '$(TargetFramework)' == 'net40' \"><DefineConstants >";
            foreach (var f in compilerFlagName)
            {
                output = output.add(f, ";");
            }
            output = output + "</DefineConstants>";
            return output;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="projectTargetTask"/> class.
        /// </summary>
        /// <param name="_targetName">Target framework name - compatibile with Visual Studio property PropertyGroup->TargetFrameworks e.g. netstandard2.0 or net45</param>
        /// <param name="_name">Display name, e.g. .NET Standard 2.0</param>
        /// <param name="_flagConstant">Compiler constants to be defined in the Visual Studio project file. e.g. "NETSTANDARD2_0" etc.</param>
        public projectTargetTask(String _targetName, String _name, params String[] _flagConstant)
        {
            targetName = _targetName;
            name = _name;
            compilerFlagName = new List<string>();
            compilerFlagName.AddRange(_flagConstant);
        }


        public static projectTargetTask GetPreset_NetStandard2()
        {
            return new projectTargetTask("netstandard2.0", ".NET Standard 2.0", "NETSTANDARD2_0");
        }

        public static projectTargetTask GetPreset_Net40()
        {
            return new projectTargetTask("net40", ".NET Framework 4.0", "net40");
        }

        public static projectTargetTask GetPreset_Net45()
        {
            return new projectTargetTask("net45", ".NET Framework 4.5", "net45");
        }


        private String _targetName = "netstandard2.0";
        /// <summary> Target framework name - compatibile with Visual Studio property PropertyGroup->TargetFrameworks </summary>
        public String targetName
        {
            get
            {
                return _targetName;
            }
            set
            {
                _targetName = value;
                OnPropertyChanged("targetName");
            }
        }


        private List<String> _compilerFlagName = new List<string>() { "NETSTANDARD2_0", "NETSTANDARD", "NETCORE" };
        /// <summary> Compiler Flags to be defined </summary>
        public List<String> compilerFlagName
        {
            get
            {
                return _compilerFlagName;
            }
            set
            {
                _compilerFlagName = value;
                OnPropertyChanged("compilerFlagName");
            }
        }



        private String _name = ".NET Standard 2.0";
        /// <summary> Human - Display name of the target </summary>
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