// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceSubEnum.cs" company="imbVeles" >
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
namespace imbACE.Core.data
{
    using imbACE.Core.core.exceptions;
    using imbSCI.Core.extensions.typeworks;
    using System;
    using System.Collections;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Enumerator that evaluates the <see cref="path"/> against <see cref="IEnumerator.Current"/> object
    /// </summary>
    /// <seealso cref="System.Collections.IEnumerator" />
    public class aceSubEnum : IEnumerator
    {
        private PropertyInfo _targetPi;

        /// <summary> </summary>
        public PropertyInfo targetPi
        {
            get
            {
                return _targetPi;
            }
            protected set
            {
                _targetPi = value;
                //OnPropertyChanged("targetPi");
            }
        }

        private Type _sourceType;

        /// <summary> </summary>
        public Type sourceType
        {
            get
            {
                return _sourceType;
            }
            protected set
            {
                _sourceType = value;
                //OnPropertyChanged("sourceType");
            }
        }

        private IEnumerable _source; //= new IEnumerable();

        /// <summary> </summary>
        public IEnumerable source
        {
            get
            {
                return _source;
            }
            protected set
            {
                _source = value;
            }
        }

        private IEnumerator _sourceEnumerator;

        /// <summary> </summary>
        public IEnumerator sourceEnumerator
        {
            get
            {
                return _sourceEnumerator;
            }
            protected set
            {
                _sourceEnumerator = value;
            }
        }

        private String _path;

        /// <summary> </summary>
        public String path
        {
            get
            {
                return _path;
            }
            protected set
            {
                _path = value;
            }
        }

        public aceSubEnum(IEnumerable __source, String __path)
        {
            try
            {
                source = __source;
                path = __path;
                Type sourceCollectionType = __source.GetType();
                sourceType = sourceCollectionType.GetGenericArguments().FirstOrDefault();
                targetPi = sourceType.getProperty(__path);
            }
            catch (Exception ex)
            {
                var axe = new aceGeneralException("The source collection is not applicable", ex, this, "aceSubEnum()");
                throw axe;
            }
        }

        /// <summary>
        /// Returns the value extracted from the <see cref="sourceEnumerator"/>
        /// </summary>
        object IEnumerator.Current
        {
            get
            {
                if (sourceEnumerator.Current == null) return null;
                Object cur = sourceEnumerator.Current;
                Object current = cur.imbGetPropertySafe(targetPi, null);
                return current;
            }
        }

        public void Dispose()
        {
            source = null;
            sourceEnumerator = null;
        }

        public bool MoveNext()
        {
            return sourceEnumerator.MoveNext();
        }

        public void Reset()
        {
            sourceEnumerator.Reset();
        }
    }
}