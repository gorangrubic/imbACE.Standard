// --------------------------------------------------------------------------------------------------------------------
// <copyright file="callerInfo.cs" company="imbVeles" >
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

    using imbACE.Core.core.exceptions;
    using imbACE.Core.extensions.io;
    using imbSCI.Core.collection;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.io;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    #endregion imbVeles using

    /// <summary>
    /// 2014 Svi moguci podaci pozivu
    /// </summary>
    public class callerInfo : IAppendDataFields
    {
        /// <summary>
        /// Pronalazi prvi frame koji nije na spisku za preskakanje
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static StackFrame getExecutionFrame(object parent, IEnumerable<Type> __toSkip = null)
        {
            List<Type> toSkip = new List<Type>();
            toSkip.AddRange(__toSkip);

            if (__toSkip.isNullOrEmpty())
            {
                //toSkip = typesToSkipStack.ToList();
                //if (toSkip.Contains(parent.GetType())) toSkip.Remove(parent.GetType());
            }
            else
            {
                toSkip = __toSkip.ToList();
                // toSkip.AddRangeUnique(typesToSkipStack);
            }

            StackTrace st = new StackTrace(true);
            Int32 fi = 1;
            StackFrame sf = st.GetFrame(fi);
            Boolean go = true;
            while (go)
            {
                if (!toSkip.Contains(sf.GetMethod().DeclaringType))
                {
                    go = false;
                    break;
                }
                else
                {
                    if (fi < st.FrameCount)
                    {
                        sf = st.GetFrame(fi);
                        fi++;
                    }
                    else
                    {
                        go = false;
                        break;
                    }
                }
            }

            return sf;
        }

        public static StackFrame getExecutionFrame(List<Type> __toSkip = null)
        {
            if (__toSkip == null) __toSkip = new List<Type>();

            // __toSkip.AddRangeUnique(typesToSkipStack);
            return getExecutionFrame(null, __toSkip);
        }

        /// <summary>
        /// Pronalazi prvi frame koji je pozvan ovim metodom
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        public static StackFrame getExecutionFrameByMethod(MethodBase mb)
        {
            StackTrace st = new StackTrace(true);
            Int32 fi = 0;
            StackFrame sf = st.GetFrame(fi);
            Boolean go = true;

            while (go)
            {
                MethodBase cmb = sf.GetMethod();
                if (cmb == mb)
                {
                    return sf;
                }
                if (fi < st.FrameCount)
                {
                    sf = st.GetFrame(fi);
                    fi++;
                }
                else
                {
                    go = false;
                    break;
                }
            }

            return null;
        }

        public PropertyCollectionExtended AppendDataFieldsOfMethod(PropertyCollectionExtended data = null)
        {
            if (data == null) data = new PropertyCollectionExtended();

            data.Add("devnote_method", methodName, "Method", "Name of the method called");
            data.Add("", Filepath, "File path", "");
            data.Add("", sourceCodeLine, "Source code line", "").relevance(dataPointImportance.important);
            data.Add("", line, "Line", "");
            data.Add("", column, "Column", "");

            return data;
        }

        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public PropertyCollectionExtended AppendDataFields(PropertyCollectionExtended data = null)
        {
            if (data == null) data = new PropertyCollectionExtended();

            try
            {
                //  data.Add("", sufix, "Call in code", "");
                data.Add("devnote_callertype", callerType.Name, "Caller", "Caller type");
                data.Add("", methodName, "Method", "").relevance(dataPointImportance.important);
                //data.Add("", className, "Class name", "");

                data.Add("", methodBody.InitLocals, "Local init", "Local variables were initialized");
                data.Add("", methodBody.MaxStackSize, "Stack size", "Maximum number of items on the operant stack");

                if (methodInfo != null)
                {
                    if (methodInfo.ReturnType != null)
                    {
                        data.Add("", methodInfo.ReturnType.Name, "Return", "Type of method return value");
                    }
                }
                foreach (LocalVariableInfo lvar in localVariables)
                {
                    String varname_sufix = lvar.LocalIndex.ToString("D3");
                    data.Add(varname_sufix, "Local var: " + varname_sufix, lvar.toStringSafe("[unknown]"), lvar.LocalType.FullName);

                    //                data.Add("", lvar.toStringSafe(), "Value "+ varname_sufix, "");
                }
                Int32 c = 0;

                foreach (ParameterInfo lvar in parameters)
                {
                    String varname_sufix = lvar.Position.ToString("D3");

                    String desc = lvar.Attributes.ToStringEnumSmart(); //lvar.Attributes.getEnumListFromFlags<PropertyAttributes>().ToStringEnumSmart()

                    String vl = "";

                    if (lvar.Position < sourceCodeParameterCalls.Count)
                    {
                        vl = sourceCodeParameterCalls[lvar.Position];
                    }

                    data.Add(varname_sufix + " " + lvar.Name, vl, lvar.GetType().Name, "Value: " + lvar.toStringSafe("[null]") + " : " + desc); // ToString());

                    //  data.Add("var" + varname_sufix, lvar.toStringSafe("[null]"), lvar.toStringSafe("[unknown]"), lvar.LocalType.Name);

                    //                data.Add("", lvar.toStringSafe(), "Value "+ varname_sufix, "");
                }
            }
            catch (Exception ex)
            {
                throw new aceGeneralException("callerInfo.AppendDataFields() exception: " + ex.Message, ex, this, "Caller info exception");
            }

            return data;
        }

        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>
        /// Updated or newly created property collection
        /// </returns>
        /// <exception cref="NotImplementedException"></exception>
        PropertyCollection IAppendDataFields.AppendDataFields(PropertyCollection data)
        {
            return AppendDataFields(data as PropertyCollectionExtended);
        }

        public static Regex methodParameters = new Regex(@"\((.*[^()])\)");
        public static Regex methodParametersEach = new Regex(@"(?:[_\(\)\.\[\]\w\-\+\\\*\~]+)", RegexOptions.Compiled);
        public static String labelFormat = "{0}.{1}({2})";
        public static String sufixFormat = "{0}:ln{1}cl{2}";
        public Type callerType;
        public Type callerTypeInfo;
        public String className;
        public int column;
        public string Filepath { get; set; }

        /// <summary>
        /// skraceni prikaz
        /// </summary>
        public String label;

        public int line;
        public List<LocalVariableInfo> localVariables = new List<LocalVariableInfo>();

        public MethodBase methodBase;

        public MethodBody methodBody;
        public MethodInfo methodInfo;

        public MethodBase methodCurrent;
        public String methodName;

        public ParameterInfo[] parameters;
        public List<String> sourceCodeFile = new List<string>();
        public String sourceCodeInvokeParamsPart;
        public String sourceCodeInvokePart;
        public String sourceCodeLine;

        public List<String> sourceCodeParameterCalls = new List<string>();

        /// <summary>
        /// dodatne informacije
        /// </summary>
        public String sufix;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sf"></param>
        /// <param name="getCode">Kompleksnija operacija - daje vise informacija</param>
        /// <returns></returns>
        public static callerInfo getCallerInfo(StackFrame sf, Boolean getCode = true)
        {
            callerInfo output = new callerInfo();

            if (sf == null)
            {
                Exception ex = new aceGeneralException("Exception had no *StackFrame* --- don-t use explicit throwing after the Exception was caught");
                throw ex;
            }
            else
            {
            }

            MethodBase mb = sf.GetMethod();

            //MethodBodyReader.GetInstructions(output.methodBase);

            output.methodBase = mb;
            output.methodBody = mb.GetMethodBody();
            output.parameters = mb.GetParameters();

            output.methodInfo = output.methodBase as MethodInfo;

            output.callerType = mb.DeclaringType;
            output.callerTypeInfo = output.callerType;

            output.methodName = output.callerType.FullName + "." + mb.Name + "()";

            if (output.callerTypeInfo == null)
            {
                output.className = "callerTypeInfo is null";
            }
            else
            {
                output.className = output.callerType.FullName; // + "." + mb.Name + "()";
            }
            output.line = sf.GetFileLineNumber();
            output.column = sf.GetFileColumnNumber();
            output.Filepath = sf.GetFileName();

            if (output.methodBody != null) output.localVariables = output.methodBody.LocalVariables.ToList();

            if (getCode)
            {
                if (output.callerType != null)
                {
                    output.methodInfo = output.callerType.GetMethod(mb.Name, new[] { output.callerType }); //  output.callerTypeInfo.findMethod(mb); //.Name, output.parameters);

                    if (!String.IsNullOrEmpty(output.Filepath))
                    {
                        output.sourceCodeFile = output.Filepath.openFileToList(false, Encoding.UTF8);// open.openFile(output.filepath);
                        output.sourceCodeLine = output.sourceCodeFile[output.line - 1];

                        output.sourceCodeInvokePart = output.sourceCodeLine.Substring(output.column);
                        String ln = methodParameters.Match(output.sourceCodeInvokePart).Value;
                        output.sourceCodeInvokeParamsPart = ln;
                        var propCalls = methodParametersEach.Matches(ln);
                        output.sourceCodeParameterCalls = new List<String>();

                        foreach (Match m in propCalls)
                        {
                            output.sourceCodeParameterCalls.Add(m.Value.Trim("()".ToCharArray()));
                        }
                        //if (output.sourceCodeParameterCalls.Any()) output.sourceCodeParameterCalls.RemoveAt(0);
                    }
                    else
                    {
                        output.sourceCodeLine = " { unknown file location }";
                    }
                }
            }

            if (output.callerType != null)
            {
                output.label = String.Format(labelFormat, output.callerType.FullName, output.methodName,
                                         output.sourceCodeInvokeParamsPart);
            }
            else
            {
                output.label = "Output caller type info is null";
            }
            output.sufix = String.Format(sufixFormat, output.Filepath, output.line, output.column);

            return output;
        }
    }
}