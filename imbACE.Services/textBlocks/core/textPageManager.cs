// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textPageManager.cs" company="imbVeles" >
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
using imbACE.Core.core.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace imbACE.Services.textBlocks.core
{
    using imbACE.Services.textBlocks.core.proto;
    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Core.reporting.zone;
    using System.Collections;
    using System.ComponentModel;

    public class textPageManager<T> : textFormatSetupSize, INotifyPropertyChanged where T : class
    {
        protected Int32 columnWidth = 0;
        protected Int32 itemHeight = 1;
        protected Int32 columnCount = 1;

        public Boolean isDisabled = false;

        public textPageManager()//, __height, __leftRightMargin, __topBottomMargin, __leftRightPadding, __topBottomPadding)
        {
            isDisabled = true;
            allItems = new List<T>();
        }

        public Boolean isReady
        {
            get
            {
                if (isDisabled) return false;
                if (allItems == null) return false;
                return true;
            }
        }

        public void selectPageByItem(T item)
        {
            if (!isReady) return;
            Int32 i = allItems.IndexOf(item);
            Int32 p = i % pageCapacaty;
            currentPage = p;
        }

        public textPageManager(Int32 __height, Int32 __columnWidth = 100, Int32 _columnCount = 1)//, __height, __leftRightMargin, __topBottomMargin, __leftRightPadding, __topBottomPadding)
        {
            isDisabled = false;
            height = __height;
            width = __columnWidth;
            columnCount = _columnCount;
            columnWidth = width / columnCount;
            currentPage = 1;
        }

        public textPageManager(ITextLayoutContentProvider host)
        {
            Int32 __top = host.getAttachmentHeight(false);
            Int32 __bottom = host.getAttachmentHeight(true);
            //height = host.height - (__bottom + __top+2);
            height = host.height - host.margin.top;
        }

        /// <summary>
        /// Pomera seleektovan item na prethodno mesto
        /// </summary>
        public void selectPrev()
        {
            if (!isReady) return;
            if (currentPage > 1) currentPage--;
        }

        public void selectLast()
        {
            if (!isReady) return;
            currentPage = totalPages;
        }

        public void selectFirst()
        {
            if (!isReady) return;
            currentPage = 1;
        }

        /// <summary>
        /// Pomera selektovan item za sledece mesto
        /// </summary>
        public void selectNext()
        {
            if (!isReady) return;
            if (currentPage < totalPages) currentPage++;
        }

        public String getPageString()
        {
            if (!isReady) return "";
            return String.Format("{0}/{1}", currentPage, totalPages);
        }

        public Int32 currentPageStartIndex
        {
            get
            {
                if (!isReady) return 0;
                return Math.Min(currentPage * pageCapacaty, allItems.Count);
            }
        }

        public Int32 currentPageEndIndex
        {
            get { return Math.Min((currentPage + 1) * pageCapacaty, allItems.Count); }
        }

        /// <summary>
        /// Vraca elemente koji su trenutno na stranici
        /// </summary>
        /// <returns></returns>
        public IList<T> getPageElements(IList<T> items)
        {
            allItems = items;

            if (!isReady) return items;
            if (isDisabled) return items;

            var output = allItems.Skip(currentPageStartIndex).Take(Math.Min(pageCapacaty, allItems.Count - currentPageStartIndex)).ToList();
            //   allItems.GetRange(currentPageStartIndex, currentPageEndIndex - currentPageStartIndex);
            return output;
        }

        public Int32 pageCapacaty
        {
            get
            {
                if (!isReady) return height;
                return (height / itemHeight) * columnCount;
            }
        }

        protected IList<T> allItems;

        public void refresh(IList<T> items)
        {
            allItems = items;
        }

        #region --- currentPage ------- b

        private Int32 _currentPage;

        /// <summary>
        /// trenutni broj stranice
        /// </summary>
        public Int32 currentPage
        {
            get
            {
                // if (!isReady) return 1;
                if (_currentPage > _totalPages)
                {
                    _currentPage = _totalPages;
                }
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                OnPropertyChanged("currentPage");
            }
        }

        #endregion --- currentPage ------- b

        #region --- totalPages ------- koliko ukupno ima strana

        private Int32 _totalPages;

        /// <summary>
        /// koliko ukupno ima strana
        /// </summary>
        public Int32 totalPages
        {
            get
            {
                // if (!isReady) return 1;
                Double count = allItems.Count;
                _totalPages = (int)Math.Ceiling(count / pageCapacaty);
                return _totalPages;
            }
        }

        #endregion --- totalPages ------- koliko ukupno ima strana

        //public IEnumerable selectPage()

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}