// --------------------------------------------------------------------------------------------------------------------
// <copyright file="inputResultCollection.cs" company="imbVeles" >
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
// Project: imbACE.Services
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbACE.Services.textBlocks.input
{
    using imbACE.Core.commands.menu.core;
    using imbACE.Core.core.exceptions;
    using imbACE.Core.enums.platform;
    using imbACE.Core.operations;
    using imbACE.Services.platform.core;
    using imbACE.Services.platform.input;
    using imbACE.Services.platform.interfaces;
    using imbACE.Services.terminal.dialogs;
    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Core.reporting.zone;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public class inputResultCollection : List<textInputResult>, INotifyPropertyChanged
    {
        #region --- resultInFocus ------- rezultat koji je trenutno fokusiran

        private textInputResult _resultInFocus = null;

        /// <summary>
        /// rezultat koji je trenutno fokusiran
        /// </summary>
        public textInputResult resultInFocus
        {
            get
            {
                if (_resultInFocus == null)
                {
                    if (this.Any())
                    {
                        _resultInFocus = this[0];
                    }
                }
                return _resultInFocus;
            }
            set
            {
                _resultInFocus = value;
                OnPropertyChanged("resultInFocus");
            }
        }

        #endregion --- resultInFocus ------- rezultat koji je trenutno fokusiran

        public textInputResult getLastResult()
        {
            var ir = this[Count - 1];

            return ir;
        }

        /// <summary>
        /// If menu is created by executor methods it will trigger selected method />.
        /// </summary>
        /// <returns></returns>
        public String doMenuItems()
        {
            String output = "";

            textInputResult res = getLastResult();

            aceMenuItem ami = res.result as aceMenuItem;
            if (ami != null)
            {
                output = ami.executeMeta();
            }

            return output;
        }

        #region --- platform ------- referenca prema platformi

        private IPlatform _platform;

        /// <summary>
        /// referenca prema platformi
        /// </summary>
        public IPlatform platform
        {
            get
            {
                return _platform;
            }
            set
            {
                _platform = value;
                OnPropertyChanged("platform");
            }
        }

        #endregion --- platform ------- referenca prema platformi

        public Boolean doKeepReading()
        {
            foreach (var ir in this)
            {
                if (ir.doKeepReading == false)
                {
                    return false;
                }
            }
            return true;
        }

        public void AddUniqueSection(textInputResult input)
        {
            var min = getBySection(input.section);
            if (min == null)
            {
                Add(input);
            }
            else
            {
                this.Remove(min);
                Add(input);
            }
        }

        public textInputResult getBySection(ITextLayoutContentProvider section)
        {
            textInputResult output = null;
            if (section != null)
            {
                foreach (var ir in this)
                {
                    if (ir.section == section)
                    {
                        resultInFocus = ir;
                        return ir;
                    }
                }
            }

            output = new textInputResult(platform, inputReadMode.unknown, new selectZone(0, 0, 0, 0));
            output.section = section;
            Add(output);

            return output;
        }

        public T getResultObject<T>(T defValue)
        {
            T output = defValue;

            if (resultInFocus.result is aceMenuItem)
            {
                var dmi = resultInFocus.result as aceMenuItem;
                if (dmi.metaObject is T)
                {
                    output = (T)dmi.metaObject;
                }
                else
                {
                }
            }
            else
            {
                if (resultInFocus.result is T)
                {
                    output = (T)resultInFocus.result;
                }
            }

            return output;
            //var dmie = (dialogMenuItem)dmi.metaObject;
        }

        /// <summary>
        /// Vraca TRUE ako je pronasao rezultat koji je povezan sa KeyValue, rezultat je posotavljen u fokus
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public Boolean doIfKey(Object keyValue)
        {
            Boolean output = false;
            foreach (var ir in this)
            {
                if (ir.consoleKey != null)
                {
                    if (ir.consoleKey.isKeyMatch(keyValue.ToString()))
                    {
                        resultInFocus = ir;
                        return true;
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// Vraca TRUE ako je pronadjen rezultat sa ovim menuitemom, rezultat se postavlja u fokus
        /// </summary>
        /// <param name="__criteriaItem"></param>
        /// <returns></returns>
        public Boolean doIfMenuItems(IEnumerable<aceMenuItem> __criteriaItems)
        {
            Boolean output = false;
            foreach (var ir in this)
            {
                if (__criteriaItems.Contains(ir.result))
                {
                    resultInFocus = ir;
                    return true;
                }
            }

            return output;
        }

        /// <summary>
        /// Vraca TRUE ako je pronadjen rezultat sa ovim menuitemom, rezultat se postavlja u fokus
        /// </summary>
        /// <param name="__criteriaItem"></param>
        /// <returns></returns>
        public Boolean doIfMenuItem(aceMenuItem __criteriaItem)
        {
            Boolean output = false;
            foreach (var ir in this)
            {
                if (ir.result == __criteriaItem)
                {
                    resultInFocus = ir;
                    return true;
                }
            }

            return output;
        }

        public aceMenuItem getMenuItemBySection(ITextLayoutContentProvider section)
        {
            aceMenuItem output = null;
            foreach (var ir in this)
            {
                if (ir.section == section)
                {
                    return ir.result as aceMenuItem;
                }
            }
            return output;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}