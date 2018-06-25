// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceGeneralException.cs" company="imbVeles" >
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
using imbACE.Core.collection;
using imbACE.Core.core.exceptions;
using imbACE.Core.extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace imbACE.Core.core.exceptions
{
    using extensions.io;
    using imbACE.Core.core.diagnostic;
    using imbACE.Core.interfaces;
    using imbSCI.Core.collection;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data;
    using imbSCI.Data.collection;

    /// <summary>
    /// General exception in imbACE framework
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class aceGeneralException : Exception, ILogSerializableProvider<IAceExceptionSerializable>
    {
        public void SetLogSerializable(IAceExceptionSerializable output)
        {
            output.Title = title;
            output.time = DateTime.Now;
            output.Message = Message;
            output.StackTrace = StackTrace;
            output.RelInstanceClassName = callInfo.callerTypeInfo.Name;
            output.SouceCodeLine = callInfo.sourceCodeLine;
            output.SourceCodeFile = callInfo.Filepath;
            output.DataDump = info.ToString();
        }

        /// <summary>
        /// Gets the serializable object with exception information
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSerializable<T>() where T : IAceExceptionSerializable, new()
        {
            T output = new T();
            SetLogSerializable(output);
            return output;
        }

        public aceGeneralException add(String newLine)
        {
            Message += Environment.NewLine + newLine;
            return this;
        }

        protected static List<StackFrame> getFramesWithSource(StackTrace trace, Int32 skip)
        {
            List<StackFrame> output = new List<StackFrame>();
            var frames = trace.GetFrames();
            foreach (StackFrame fr in frames)
            {
                if (!fr.GetFileName().isNullOrEmpty())
                {
                    output.Add(fr);
                }
            }
            return output;
        }

        protected static StackFrame getFirstFrameWithSource(StackTrace trace, Int32 skip)
        {
            Int32 i = 0;
            var frames = trace.GetFrames();
            StackFrame lastFound = null;
            foreach (StackFrame fr in frames)
            {
                if (fr != null)
                {
                    if (fr.GetMethod()?.DeclaringType == typeof(aceGeneralException))
                    {
                    }
                    else
                    {
                        if (!fr.GetFileName().isNullOrEmpty())
                        {
                            if (i < skip)
                            {
                                i++;
                                lastFound = fr;
                            }
                            else
                            {
                                return fr;
                            }
                        }
                    }
                }
            }
            return lastFound;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="aceGeneralException"/> class.
        /// </summary>
        /// <param name="__message">The message.</param>
        /// <param name="__innerException">The inner exception.</param>
        public aceGeneralException(String __message = "", Exception __innerException = null, Object __callerInstane = null, String __title = "", Int32 stacks = 0) : base(__message, __innerException)
        {
            Int32 stackSkip = 1 + stacks;

            while (__innerException != null)
            {
                __innerException = __innerException.InnerException as aceGeneralException;
                stackSkip++;
            }

            _stackTrace = new StackTrace(true);
            //_stackFrame = _stackTrace.GetFrame(0);
            _stackFrame = getFirstFrameWithSource(_stackTrace, stackSkip);
            _message = __message;
            title = __title;

            if (_stackTrace == null)
            {
                _stackTraceText = Environment.StackTrace;
            }
            else
            {
                _stackTraceText = _stackTrace.ToString();
            }

            callInfo = callerInfo.getCallerInfo(_stackFrame, true);
            info = callInfo.AppendDataFields(info);
            info = callInfo.AppendDataFieldsOfMethod(info);

            if (InnerException != null)
            {
                if (imbSciStringExtensions.isNullOrEmptyString(__message))
                {
                    _message += Environment.NewLine + InnerException.Message;
                }
                if (imbSciStringExtensions.isNullOrEmptyString(title))
                {
                    title = "Exception: " + InnerException.GetType().Name + " in " + callInfo.className;
                }
            }
            var ex = InnerException;
            imbSCI.Core.reporting.render.builders.builderForMarkdown md = new builderForMarkdown();
            Int32 c = 1;

            while (ex != null)
            {
                ex.reportSummary(md, "Inner exception [" + c + "]");
                ex = ex.InnerException;
                c++;
            }
            _message = _message.addLine(md.ContentToString());

            HelpLink = Directory.GetCurrentDirectory() + "\\diagnostics\\index.html";
        }

        private String _title;

        /// <summary> </summary>
        public String title
        {
            get
            {
                return _title;
            }
            protected set
            {
                _title = value;
            }
        }

        private PropertyCollectionExtended _info = new PropertyCollectionExtended();

        /// <summary> </summary>
        public PropertyCollectionExtended info
        {
            get
            {
                return _info;
            }
            protected set
            {
                _info = value;
            }
        }

        private StackFrame _stackFrame;

        /// <summary> </summary>
        public StackFrame stackFrame
        {
            get
            {
                return _stackFrame;
            }

            protected set
            {
                _stackFrame = value;
                //OnPropertyChanged("stackFrame;");
            }
        }

        private StackTrace _stackTrace;

        /// <summary> </summary>
        public StackTrace stackTrace
        {
            get
            {
                return _stackTrace;
            }
            protected set
            {
                _stackTrace = value;
            }
        }

        private callerInfo _callInfo;

        /// <summary> </summary>
        public callerInfo callInfo
        {
            get
            {
                return _callInfo;
            }
            protected set
            {
                _callInfo = value;
            }
        }

        public override string HelpLink
        {
            get
            {
                return base.HelpLink;
            }

            set
            {
                base.HelpLink = value;
            }
        }

        private String _message = "";

        public new string Message
        {
            get
            {
                return _message;
                //return base.Message;
            }
            protected set
            {
                _message = value;
            }
        }

        public override string Source
        {
            get
            {
                return base.Source;
            }

            set
            {
                base.Source = value;
            }
        }

        public override string ToString()
        {
            return base.ToString();
        }

        private String _stackTraceText = "";

        public override string StackTrace
        {
            get
            {
                return _stackTraceText;
            }
        }

        public override IDictionary Data
        {
            get
            {
                return base.Data;
            }
        }
    }
}