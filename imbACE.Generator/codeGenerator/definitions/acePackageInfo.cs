using System;
using System.Linq;
using System.Collections.Generic;
using imbACE.Core.application;
using imbSCI.Core.collection;
using imbSCI.Core.collection.checkLists;
using imbSCI.Core.collection.checkLists;
using imbSCI.Core.extensions.data;
using imbSCI.Core.extensions.text;
using imbSCI.Data;
using imbSCI.Data.data;
using imbSCI.Data.interfaces;
using System.ComponentModel;
using System.Text;

namespace imbACE.Generator.codeGenerator.definitions
{

    /// <summary>
    /// Extends imbACE application information with data entries relevant for NuGet package meta data
    /// </summary>
    /// <seealso cref="imbACE.Core.application.aceApplicationInfo" />
    public class acePackageInfo : aceApplicationInfo
    {


        private String _ReleaseNotes = default(String); // = new String();
                                                        /// <summary>
                                                        /// Text of release notes, attached to NuGet package 
                                                        /// </summary>
        [Category("acePackageInfo")]
        [DisplayName("ReleaseNotes")]
        [Description("Text of release notes, attached to NuGet package ")]
        public String ReleaseNotes
        {
            get
            {
                return _ReleaseNotes;
            }
            set
            {
                _ReleaseNotes = value;
                OnPropertyChanged("ReleaseNotes");
            }
        }

    }

}