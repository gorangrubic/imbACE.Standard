// --------------------------------------------------------------------------------------------------------------------
// <copyright file="aceEventGeneralArgs.cs" company="imbVeles" >
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
using imbSCI.Core.attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace imbACE.Core.events
{
    //public delegate void aceEventGeneralHandler(Object sender, aceEventGeneralArgs args);

    /// <summary>
    /// General imbACE Event Arguments
    /// </summary>
    /// <remarks>
    /// <para>Try to avoid any automatic and assumption-based value attribution to the Event Arguments information</para>
    /// <para>Set only data that are: </para>
    /// <list type="Bullet">
    ///     <listheader>
    ///         <term>Guidelines</term>
    ///     </listheader>
    ///     <item>
    ///         <term>In case of exception or crash: populate <see cref="Properties"/> with any useful Exception and/or contextual information for easier debug</term>
    ///     </item>
    ///     <item>
    ///         <term>Set <see cref="type"/> and/or <see cref="Origin"/> only if it can be useful for event interpretation</term>
    ///     </item>
    ///     <item>
    ///         <term>Implement <see cref="isCanceled"/> in case the event is yet to happen (e.g. <see cref="aceEventType.closing"/>, <see cref="aceEventType.saving"/>)</term>
    ///     </item>
    /// </list>
    /// <example>
    ///     <para>Recommended application of the imbACE EventGeneralArgs</para>
    ///     <code>
    ///     // Event receiver
    ///     public class anAceObjectListener {
    ///
    ///         public anAceObject createObject() {
    ///
    ///             var a = new anAceObject();
    ///             a.EventOpDone += c_operation;
    ///             return a;
    ///         }
    ///
    ///         public void c_operationDone(object sender, aceEventGeneralArgs e)
    ///         {
    ///             // code that reacts to the event
    ///             Environment.Exit(0);
    ///         }
    ///
    ///     }
    ///
    ///     // Event sender
    ///     public class anAceObject {
    ///
    ///         protected String operationName {get;set;} = "doSomething";
    ///
    ///         // Event handler
    ///         public event EventHandler&lt;aceEventGeneralArgs&gt; EventOpDone;
    ///
    ///         //
    ///         protected virtual void OnOperationDone(aceEventGeneralArgs e) {
    ///
    ///             e.Properties["OperationName"]=operationName;
    ///
    ///             if (e.type == aceEventType.unknown) e.type = aceEventType.finished;
    ///
    ///             EventOpDone(this, e);
    ///         }
    ///
    ///         // "Closed" scenario - when we know exactly what happen
    ///         protected virtual void OnStarted() {
    ///
    ///             var e = aceEventGeneralArgs(aceEventType.started);
    ///             EventOpDone(this, e);
    ///
    ///         }
    ///
    ///     }
    ///     </code>
    /// </example>
    /// </remarks>
    public class aceEventGeneralArgs : EventArgs
    {
        /// <summary> if true, it will tell to the event handling party to try canceling / preventing, if event is preventable </summary>
        [Category("Flag")]
        [DisplayName("isCanceled")]
        [imb(imbAttributeName.measure_letter, "C")]
        [Description("if true, it will tell to the event handling party to try canceling / preventing, if event is preventable")]
        public Boolean isCanceled { get; set; } = false;

        /// <summary> Textual message attached to the event </summary>
        [Category("Label")]
        [DisplayName("Message")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Textual message attached to the event")] // [imb(imbAttributeName.reporting_escapeoff)]
        public String Message { get; set; } = default(String);

        /// <summary> An object that is relevant to the event (not sender) - this property is not serialized </summary>
        [Category("Label")]
        [DisplayName("RelatedObject")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("An object that is relevant to the event (not sender) - this property is not serialized")] // [imb(imbAttributeName.reporting_escapeoff)]
        [XmlIgnore]
        public Object RelatedObject { get; set; } = default(Object);

        /// <summary> Filesystem, network or application resources path - pointing to the position of event origin </summary>
        [Category("Label")]
        [DisplayName("Path")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Filesystem, network or application resources path - pointing to the position of event origin")] // [imb(imbAttributeName.reporting_escapeoff)]
        public String Path { get; set; } = default(String);

        /// <summary> Type of the event </summary>
        [Category("Label")]
        [DisplayName("Type")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Type of the event")] // [imb(imbAttributeName.reporting_escapeoff)]
        public aceEventType type { get; set; } = aceEventType.unknown;

        /// <summary> Indication on the event origin (optional)  </summary>
        [Category("Label")]
        [DisplayName("Origin")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Indication on the event origin (optional) ")] // [imb(imbAttributeName.reporting_escapeoff)]
        public aceEventOrigin Origin { get; set; } = aceEventOrigin.unknown;

        /// <summary> Additional meta data on the event </summary>
        [Category("Label")]
        [DisplayName("Properties")] //[imb(imbAttributeName.measure_letter, "")]
        [Description("Additional meta data on the event")] // [imb(imbAttributeName.reporting_escapeoff)]
        public PropertyCollection Properties { get; set; } = new PropertyCollection();
    }
}