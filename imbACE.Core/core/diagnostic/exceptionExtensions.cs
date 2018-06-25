// --------------------------------------------------------------------------------------------------------------------
// <copyright file="exceptionExtensions.cs" company="imbVeles" >
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
    using imbACE.Core.enums;
    using imbACE.Core.extensions.io;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.reporting.render;
    using imbSCI.Core.reporting.render.builders;
    using imbSCI.Data;
    using imbSCI.Data.enums;
    using imbSCI.Data.enums.appends;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Extensions for reporting on Exceptions
    /// </summary>
    public static class exceptionExtensions
    {
        /// <summary>
        /// Returns TRUE if the <c>policy</c> instructs throwing exception
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns></returns>
        public static Boolean doThrow(this onErrorPolicy policy)
        {
            if (policy == onErrorPolicy.onErrorThrowException) return true;
            return false;
        }

        /// <summary>
        /// Gets the caller information report.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        public static String getCallerInfoReport(this callerInfo output)
        {
            builderForMarkdown sb = new builderForMarkdown();

            sb.Append("Caller info report:", appendType.heading_3, true);

            sb.nextTabLevel();
            // sb.AppendLine("Caller method: " + labelFormat);

            sb.AppendPair("Call in code: ", output.sufix);

            sb.AppendPair("Method name: ", output.className);

            sb.AppendPair("Source file: ", output.Filepath);

            sb.Append("Source call line: ", appendType.bold, true);

            sb.Append(output.sourceCodeLine, appendType.source, true);

            sb.Append("Source call line segment: ", appendType.regular, true);
            sb.Append(output.sourceCodeInvokePart, appendType.source, true);

            sb.Append("Caller method local variables:", appendType.heading_4);
            sb.nextTabLevel();

            output.localVariables.ForEach(
                x => sb.Append(
                    String.Format("{0}:{1} {2}", x.LocalIndex, x.LocalType.Name, x.toStringSafe()), appendType.quotation)
                    );
            sb.prevTabLevel();

            sb.AppendPair("Caller source line properties:", output.sourceCodeParameterCalls.toCsvInLine());

            sb.prevTabLevel();

            sb.prevTabLevel();

            return sb.ToString();
        }

        /// <summary>
        /// Prepoznaje gresku u serijalizaciji
        /// </summary>
        public static String ERROR_serialization_Interface =
            @"Cannot serialize member (.*) of type (.*) because it is (.*).";

        public static String ERROR_stackTrackOnlyImb = "at imb(.*)";
        private static Regex _regex_ourStack;

        /// <summary>
        /// imbControl property regex_ourStack tipa Regex
        /// </summary>
        internal static Regex regex_ourStack
        {
            get
            {
                if (_regex_ourStack == null) _regex_ourStack = new Regex(ERROR_stackTrackOnlyImb);

                return _regex_ourStack;
            }
            set { _regex_ourStack = value; }
        }

        /// <summary>
        /// Google search about the exception
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="ci">The ci.</param>
        public static void googleException(this Exception ex, callerInfo ci)
        {
            if (ex == null) return;
            String urlFormat = @"https://www.google.com/?q={0}&safe=off";
            String search = ci.methodBase.DeclaringType.Name + "." + ci.methodName + "() " +
                            ex.GetType().FullName + " " + ex.Message;
            search = Uri.EscapeDataString(search);
            externalTool.chrome.run(String.Format(urlFormat, search));
        }

        /// <summary>
        /// Writes a summary report into ITextRender
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="sb">The sb.</param>
        /// <param name="summaryTitle">The summary title.</param>
        /// <returns></returns>
        public static String reportSummary(this Exception ex, ITextRender sb = null, String summaryTitle = "")
        {
            if (sb == null)
            {
                sb = new builderForMarkdown();
            }
            if (String.IsNullOrEmpty(summaryTitle)) summaryTitle = "Exception" + ex.GetType().Name;

            //sb.open(htmlTagName.div, htmlClassForReport.noteContainer, htmlIdForReport.innerException);

            sb.AppendPair(summaryTitle + " message: ", ex.Message, true, "");

            sb.Append(summaryTitle + " stack: ", appendType.heading_3, true);
            //, htmlTagName.p, , "", false);

            ex.StackTrace.cleanStackTrace(sb);

            callerInfo ci = new callerInfo();

            sb.AppendLine(ex.Source);

            sb.AppendLine(ex.StackTrace);

            // sb.close();

            return sb.ToString();
        }

        /// <summary>
        /// Pravi stack trace izvestaj
        /// </summary>
        /// <param name="stackTrace"></param>
        /// <param name="sb"></param>
        /// <returns></returns>
        internal static String cleanStackTrace(this String stackTrace, ITextRender sb)
        {
            if (sb == null)
            {
                sb = new builderForMarkdown();
            }
            if (String.IsNullOrEmpty(stackTrace)) return "";

            Match m = regex_ourStack.Match(stackTrace);

            if (m != null)
            {
                string cs = m.Value.toStringSafe().Trim("{}".ToCharArray());
                cs.imbRemoveDouble();

                List<string> parts = imbSciStringExtensions.SplitOnce(cs, " in ");

                if (parts.Count > 1)
                {
                    sb.AppendPair("Location: ", parts.imbFirstSafe());

                    String filePart = parts.toCsvInLine();

                    parts = imbSciStringExtensions.SplitOnce(filePart, ":line ");

                    sb.AppendPair("File: ", parts.imbFirstSafe());

                    //sb.Append("File :", htmlTagName.span, htmlClassForReport.itemName);

                    sb.AppendLink(parts.imbFirstSafe(), parts.imbFirstSafe(), "Source file", appendLinkType.link);

                    sb.AppendPair("Line: ", parts.imbLastSafe()); //, htmlTagName.span, htmlClassForReport.itemName.ToString(), htmlClassForReport.filePath.ToString(), true, htmlTagName.p);

                    sb.close();
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Primary method for exception description
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="sb"></param>
        public static String describe(this Exception ex, ITextRender sb = null, String title = "")
        {
            if (sb == null)
            {
                sb = new builderForMarkdown();
            }

            if (String.IsNullOrEmpty(title)) title = "Exception " + ex.GetType().Name;
            //sb.nextTabLevel();
            try
            {
                sb.AppendHeading(title + " : " + ex.GetType().Name);
                sb.AppendPair("Message: ", ex.Message, true);
                sb.AppendPair("Target site: ", ex.TargetSite.ToString(), true);
                sb.AppendPair("Source: ", ex.Source, true);

                ex.StackTrace.cleanStackTrace(sb);

                //  sb.AppendLine("", -1);

                var exi = ex.getLastInner();
                if (ex != exi)
                {
                    exi.describe(sb, "Last inner exception");
                }
            }
            catch (Exception ex2)
            {
                sb.AppendLine("EX2: " + ex2.Message);
                //throw ex2;
            }

            //r sb.prevTabLevel();
            return sb.ToString();
        }

        /// <summary>
        /// Retrieves the last exception nested as InnerException
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>InnerException found</returns>
        public static Exception getLastInner(this Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }
    }
}