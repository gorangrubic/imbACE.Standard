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
namespace $rootnamespace$
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using imbACE.Services.console;

    public class $safeitemname$Console : aceAdvancedConsole<$safeitemname$State, $safeitemname$Workspace>
    {
        public override string consoleTitle { get { return "$safeitemname$ Console"; } }

        public $safeitemname$Console() : base()
        {


        }

       
        /// <summary>
        /// Gets the workspace.
        /// </summary>
        /// <value>
        /// The workspace.
        /// </value>
        public override $safeitemname$Workspace workspace {
            get
            {
                if (_workspace == null)
                {
                    _workspace = new $safeitemname$Workspace(this);
                }
                return _workspace;
            }
        }

        public override void onStartUp()
        {
            //
        }

        protected override void doCustomSpecialCall(aceCommandActiveInput input)
        {
            
        }
    }

}