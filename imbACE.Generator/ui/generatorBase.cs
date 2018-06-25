using System;
using System.Collections.Generic;
using System.Text;

namespace imbACE.Generator.ui
{

    public abstract class generatorBase
    {

        protected generatorBase()
        {

        }




        public generatorSettings settings { get; set; } = new generatorSettings();

        /// <summary>
        /// Builds the source from <see cref="universalViewModel"/>
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public abstract String BuildSourceFrom(universalViewModel model);




    }

}