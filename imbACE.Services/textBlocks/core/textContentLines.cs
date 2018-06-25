// --------------------------------------------------------------------------------------------------------------------
// <copyright file="textContentLines.cs" company="imbVeles" >
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
    using imbACE.Core.extensions;

    using imbACE.Core.extensions;

    using imbACE.Services.textBlocks.interfaces;
    using imbSCI.Core.extensions.text;
    using imbSCI.Data;

    public class textContentLines : List<String>
    {
        //protected Int32 maxWith = 5;

        #region --- maxWidth ------- maksimalna sirina

        private Int32 _maxWidth = 80;

        /// <summary>
        /// maksimalna sirina
        /// </summary>
        protected Int32 maxWidth
        {
            get
            {
                return _maxWidth;
            }
            set
            {
                _maxWidth = value;
                //OnPropertyChanged("maxWidth");
            }
        }

        #endregion --- maxWidth ------- maksimalna sirina

        //protected String emptyLine = "!";

        #region --- emptyLine ------- dimenzija linije

        private String _emptyLineChar = "!";
        private String _emptyLine = "";

        /// <summary>
        /// dimenzija linije
        /// </summary>
        protected String emptyLine
        {
            get
            {
                if (maxWidth > _emptyLine.Length)
                {
                    _emptyLine = _emptyLineChar.Repeat(maxWidth);
                }
                return _emptyLine;
            }
        }

        #endregion --- emptyLine ------- dimenzija linije

        public String this[Int32 key]
        {
            get
            {
                do
                {
                    Add(emptyLine);
                } while (key >= Count);
                if (key < 0)
                {
                    return "------- negative key ------ ";
                }
                return base[key];
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    maxWidth = Math.Max(value.Length, maxWidth);
                }

                do
                {
                    Add(emptyLine);
                } while (key >= Count);

                if (key < 0)
                {
                }
                else
                {
                    base[key] = value;
                }
            }
        }

        public String select(Int32 y, Int32 x, Int32 length)
        {
            if (length < 0) length = 0;

            String ln = this[y].toWidthMinimum(x + length, " ");
            return ln.Substring(x, length);
        }

        public textContentLines()
        {
        }

        public textContentLines(ITextLayoutContentProvider parent)
        {
            String deco = " ";
            if (!String.IsNullOrEmpty(parent.marginDecoration))
            {
                deco = parent.marginDecoration;
            }
            String ln = deco.Repeat(parent.width);

            for (int i = 0; i < parent.height; i++)
            {
                Add(ln);
            }
        }

        public String toString(Boolean showLineNumber = false, String newLineSep = null)
        {
            String output = "";
            if (newLineSep == null) newLineSep = Environment.NewLine;
            Int32 c = 0;
            foreach (string s in this)
            {
                String so = "";
                if (showLineNumber)
                {
                    so = String.Format("{0,-4} : ", c);
                }

                output = output.add(so, newLineSep);
                c++;
            }
            return output;
        }

        public new void Insert(Int32 index, String newItem)
        {
            throw new aceGeneralException("NE TREBA OVO DA POZIVAS!!!");
        }

        public List<string> insert(IEnumerable<String> content, Int32 innerWidth, Int32 toInnerLine = -1)
        {
            Int32 c = 0;
            Int32 yd = toInnerLine;
            if (yd == -1) yd = Count;
            Int32 ys = yd;
            List<string> clso = new List<string>();
            foreach (String cl in content)
            {
                clso.AddRange(insert(cl, innerWidth, yd));
                if (yd != -1) yd = ys + clso.Count();
            }

            return clso;
        }

        /// <summary>
        /// Glavni metod za ubacivanje sadrzaja - vraca linije koje je napravio wordwrapom
        /// </summary>
        /// <param name="content"></param>
        /// <param name="innerWidth"></param>
        /// <param name="toInnerLine"></param>
        /// <returns></returns>
        public List<string> insert(String content, Int32 innerWidth, Int32 toInnerLine = -1)
        {
            Int32 c = 0;
            Int32 yc = toInnerLine;

            List<string> cls = new List<string>();
            if (!String.IsNullOrEmpty(content))
            {
                if (content.Length <= innerWidth)
                {
                    cls.Add(content);
                }
                else
                {
                    cls = content.wrapLineBySpace(innerWidth);
                }

                foreach (String ln in cls)
                {
                    if (toInnerLine == -1)
                    {
                        yc = -1;
                    }
                    else
                    {
                        yc = toInnerLine + c;
                    }
                    if (yc == -1) yc = Count;
                    if (yc >= Count)
                    {
                        Add(ln);
                    }
                    else
                    {
                        base[yc] = ln;

                        //base.Insert(yc, ln);
                    }
                    //if (yc == -1)
                    //{
                    //    //cursor.moveLineTo(toInnerLine);
                    //} else
                    //{
                    //}
                    c++;
                }
            }

            return cls;
        }

        public void paint(ITextLayoutContentProvider parent, String deco = "")
        {
            Clear();
            if (!String.IsNullOrEmpty(deco))
            {
                deco = parent.marginDecoration;
            }
            String ln = deco.Repeat(parent.width);

            for (int i = 0; i < parent.height; i++)
            {
                Add(ln);
            }
        }
    }
}