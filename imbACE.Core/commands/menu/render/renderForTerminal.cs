// --------------------------------------------------------------------------------------------------------------------
// <copyright file="renderForTerminal.cs" company="imbVeles" >
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
using imbACE.Core;
using imbSCI.Core;
using imbSCI.Core.attributes;
using imbSCI.Core.enums;
using imbSCI.Core.extensions.text;
using imbSCI.Core.extensions.typeworks;
using imbSCI.Core.interfaces;
using imbSCI.Data;
using imbSCI.Data.collection;
using imbSCI.Data.data;
using imbSCI.Data.interfaces;
using imbSCI.DataComplex;
using imbSCI.Reporting;
using imbSCI.Reporting.enums;
using imbSCI.Reporting.interfaces;

namespace imbACE.Core.commands.menu.render
{
    using System;

    public static class renderForTerminal
    {
        #region --- formatNumberBox ------- Formatiranje za number/key box

        private static String _formatNumberBox = "[{0,-3}]";

        /// <summary>
        /// Formatiranje za number/key box
        /// </summary>
        public static String formatNumberBox
        {
            get
            {
                return _formatNumberBox;
            }
            set
            {
                _formatNumberBox = value;
            }
        }

        #endregion --- formatNumberBox ------- Formatiranje za number/key box

        #region --- formatForItem ------- formatiranje za item

        private static String _formatForItem = "{0} {1,-15} {2,-20}";

        /// <summary>
        /// formatiranje za item
        /// </summary>
        public static String formatForItem
        {
            get
            {
                return _formatForItem;
            }
            set
            {
                _formatForItem = value;
                //OnPropertyChanged("formatForItem");
            }
        }

        #endregion --- formatForItem ------- formatiranje za item

        #region --- formatForSelection ------- format za prikaz selekcije

        private static String _formatForSelection = "[{0,1}] ";

        /// <summary>
        /// format za prikaz selekcije
        /// </summary>
        public static String formatForSelection
        {
            get
            {
                return _formatForSelection;
            }
            set
            {
                _formatForSelection = value;
                //OnPropertyChanged("formatForSelection");
            }
        }

        #endregion --- formatForSelection ------- format za prikaz selekcije

        #region --- formatNumberOrKey ------- numberOrKey box format

        private static String _formatNumberOrKey = "[{0,-3} or {1,-3}]";

        /// <summary>
        /// numberOrKey box format
        /// </summary>
        public static String formatNumberOrKey
        {
            get
            {
                return _formatNumberOrKey;
            }
            set
            {
                _formatNumberOrKey = value;
                //  OnPropertyChanged("formatNumberOrKey");
            }
        }

        #endregion --- formatNumberOrKey ------- numberOrKey box format

        public static void renderItems()
        {
        }

        /*
        public void renderTask(renderTask task, aceMenu menu, textContent output)
        {
            if (task.renderFlags.Contains(menuRenderFlag.fullWidthLineAbove))
            {
                output.appendFullLine("-");
            }

            switch (task.task)
            {
                case menuRendererTask.menuTitle:
                    output.appendLine(menu.menuTitle, "[ {0} ]", printHorizontal.left, "=");
                    break;

                case menuRendererTask.menuDescription:
                    output.appendLine(menu.menuDescription, " {0} ", printHorizontal.left, ":");
                    break;

                case menuRendererTask.menuDefaultValue:
                    break;

                case menuRendererTask.menuItemGroup:
                    renderMenuItems(output, menu, task.renderFlags, task.group);
                    break;

                case menuRendererTask.menuSelectedValue:
                    break;

                case menuRendererTask.menuInputLine:
                    break;

                default:
                case menuRendererTask.none:
                    break;
            }

            if (task.renderFlags.Contains(menuRenderFlag.fullWidthLineUnder))
            {
                output.appendFullLine("-");
            }
        }
        */
    }
}