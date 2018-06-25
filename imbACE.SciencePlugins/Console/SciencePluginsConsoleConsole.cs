// --------------------------------------------------------------------------------------------------------------------
// This program is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see http://www.gnu.org/licenses/. 
// </copyright>
// <summary>
// Project: 
// Author: 
// ------------------------------------------------------------------------------------------------------------------
// Created with imbVeles / imbACE framework
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbACE.SciencePlugins.Console
{
    using System;
    using System.Collections.Generic;
    using imbACE.Core.operations;
    using System.ComponentModel.DataAnnotations;

    using System.Text;

    using imbACE.Services.console;

    public class SciencePluginsConsoleConsole : aceAdvancedConsole<SciencePluginsConsoleState, SciencePluginsConsoleWorkspace>
    {
        public override string consoleTitle { get { return "SciencePluginsConsole Console"; } }

        public SciencePluginsConsoleConsole() : base()
        {

            
        }

       
        /// <summary>
        /// Gets the workspace.
        /// </summary>
        /// <value>
        /// The workspace.
        /// </value>
        public override SciencePluginsConsoleWorkspace workspace {
            get
            {
                if (_workspace == null)
                {
                    _workspace = new SciencePluginsConsoleWorkspace(this);
                }
                return _workspace;
            }
        }

        /// <summary>
        /// Called once console is started
        /// </summary>
        public override void onStartUp()
        {
            //
            base.onStartUp();
        }

        /// <summary>
        /// Currently not used
        /// </summary>
        /// <param name="input">The console input</param>
        protected override void doCustomSpecialCall(aceCommandActiveInput input)
        {
            
        }
    }

}