// --------------------------------------------------------------------------------------------------------------------
// <copyright file="imbConsoleLog.cs" company="imbVeles" >
//
// Copyright (C) 2017 imbVeles
//
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
// Project: imbACE.Core
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------

namespace imbACE.Core.core.diagnostic
{
    #region imbVeles using

    using System;

    #endregion imbVeles using

    /// <summary>
    /// Complex log entry
    /// </summary>
    public class imbConsoleLog
    {
        //public String messageText = "";

        /// <summary>
        /// Cela linija loga u String varijanti
        /// </summary>
        public String fullTextLine = "";

        /// <summary>
        /// Specijalna log instrukcija -- za thread safe izvrsavanje
        /// </summary>
        public imbConsoleLogInstruction instruction = imbConsoleLogInstruction.none;

        public int level = 3;

        public String levelName = "";
        public String message = "";
        public String stampPart = "";

        #region --- instructionObject ------- Objekat koji je prosledjen radi izvrsenja instrukcije

        private Object _instructionObject;

        /// <summary>
        /// Objekat koji je prosledjen radi izvrsenja instrukcije
        /// </summary>
        public Object instructionObject
        {
            get { return _instructionObject; }
            set { _instructionObject = value; }
        }

        #endregion --- instructionObject ------- Objekat koji je prosledjen radi izvrsenja instrukcije

        public imbConsoleLog(String _message, logType _level)
        {
            message = _message;
            level = (int)_level;
        }

        public imbConsoleLog(String _stampPart, String _levelName, String _message, String _fullTextLine, int _level)
        {
            stampPart = _stampPart;
            levelName = _levelName;
            message = _message;

            fullTextLine = _fullTextLine;

            level = _level;
        }
    }
}