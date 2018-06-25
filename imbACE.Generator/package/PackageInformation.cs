using imbSCI.Data.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace imbACE.Generator.package
{
    public class PackageInformation : imbBindable
    {






        private String _repository_name = ""; // = new String();
        /// <summary>
        /// Name of the GitHub repository
        /// </summary>
        [Category("PackageInformation")]
        [DisplayName("repository_name")]
        [Description("Name of the GitHub repository")]
        public String repository_name
        {
            get
            {
                return _repository_name;
            }
            set
            {
                _repository_name = value;
                OnPropertyChanged("repository_name");
            }
        }


    }
}
