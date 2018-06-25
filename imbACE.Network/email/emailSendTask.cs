// --------------------------------------------------------------------------------------------------------------------
// <copyright file="emailSendTask.cs" company="imbVeles" >
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
// Project: imbACE.Network
// Author: Goran Grubic
// ------------------------------------------------------------------------------------------------------------------
// Project web site: http://blog.veles.rs
// GitHub: http://github.com/gorangrubic
// Mendeley profile: http://www.mendeley.com/profiles/goran-grubi2/
// ORCID ID: http://orcid.org/0000-0003-2673-9471
// Email: hardy@veles.rs
// </summary>
// ------------------------------------------------------------------------------------------------------------------
namespace imbACE.Network.email
{
    #region imbVeles using

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
    using imbSCI.Reporting.includes;
    using imbSCI.Reporting.interfaces;
    using System;
    using System.ComponentModel;
    using System.Net.Mail;
    using System.Xml.Serialization;

    #endregion imbVeles using

    /// <summary>
    /// 2014c> zadatak slanja e-maila, koji se prati preko baze podataka
    /// </summary>
    public class emailSendTask : imbBindable
    {
        public emailSendTask() : base()
        {
        }

        /// <summary>
        /// Pravi poruku, adresira, kaci attachmente
        /// </summary>
        /// <param name="attachments">attachmenti koji treba da se okace</param>
        /// <returns>formiran MailMessage objekat - spreman za slanje</returns>
        public MailMessage createMailObject(reportIncludeFileCollection attachments = null)
        {
            MailMessage output = new MailMessage(message.from, message.address, message.subject, message.content);

            if (attachments != null)
            {
                foreach (System.Collections.Generic.KeyValuePair<string, reportIncludeFile> file in attachments)
                {
                    Attachment attachment = new Attachment(file.Value.sourceFilePath);
                    output.Attachments.Add(attachment);
                }
            }

            return output;
        }

        #region --- subject ------- naslov emaila

        private string _subject = "";

        /// <summary>
        /// naslov emaila
        /// </summary>
        public string subject
        {
            get { return _subject; }
            set
            {
                _subject = value;
                OnPropertyChanged("subject");
            }
        }

        #endregion --- subject ------- naslov emaila

        #region -----------  result  -------  [Rezultat slanja emaila]

        private emailSendTaskResult _result = emailSendTaskResult.unknown; // = new emailSendTaskResult();

        /// <summary>
        /// Rezultat slanja emaila
        /// </summary>
        // [XmlIgnore]
        [Category("emailSendTask")]
        [DisplayName("result")]
        [Description("Rezultat slanja emaila")]
        public emailSendTaskResult result
        {
            get { return _result; }
            set
            {
                // Boolean chg = (_result != value);
                _result = value;
                OnPropertyChanged("result");
                // if (chg) {}
            }
        }

        #endregion -----------  result  -------  [Rezultat slanja emaila]

        #region -----------  triggerDate  -------  [Datum od kada moze da se salje]

        private DateTime _triggerDate = new DateTime(); // = new DateTime();

        /// <summary>
        /// Datum od kada moze da se salje
        /// </summary>
        // [XmlIgnore]
        [Category("emailSendTask")]
        [DisplayName("triggerDate")]
        [Description("Datum od kada moze da se salje")]
        public DateTime triggerDate
        {
            get { return _triggerDate; }
            set
            {
                // Boolean chg = (_triggerDate != value);
                _triggerDate = value;
                OnPropertyChanged("triggerDate");
                // if (chg) {}
            }
        }

        #endregion -----------  triggerDate  -------  [Datum od kada moze da se salje]

        #region -----------  emailMessage  -------  [poruka koja treba da se posalje]

        private emailMessage _message = new emailMessage(); // = new emailMessage();

        /// <summary>
        /// poruka koja treba da se posalje
        /// </summary>
        // [XmlIgnore]
        [Category("emailSendTask")]
        [DisplayName("emailMessage")]
        [Description("poruka koja treba da se posalje")]
        public emailMessage message
        {
            get { return _message; }
            set
            {
                // Boolean chg = (_emailMessage != value);
                _message = value;
                OnPropertyChanged("message");
                // if (chg) {}
            }
        }

        #endregion -----------  emailMessage  -------  [poruka koja treba da se posalje]

        #region -----------  signature  -------  [Naziv ]

        private string _signature; // = new String();

        /// <summary>
        /// Naziv preseta
        /// </summary>
        [XmlIgnore]
        [Category("emailSendTask")]
        [DisplayName("Name")]
        [Description("Naziv preseta")]
        public string signature
        {
            get { return _signature; }
            set
            {
                _signature = value;
                OnPropertyChanged("signature");
            }
        }

        #endregion -----------  signature  -------  [Naziv ]

        #region --- status ------- status zadatka

        private emailSendTaskStatus _status = emailSendTaskStatus.waiting;

        /// <summary>
        /// status zadatka
        /// </summary>
        public emailSendTaskStatus status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged("status");
            }
        }

        #endregion --- status ------- status zadatka

        #region --- lastRunStamp ------- Potpis poslednjeg izvrsavanja: datum_vreme + oznaka testa

        private string _lastRunStamp;

        /// <summary>
        /// Potpis poslednjeg izvrsavanja: datum_vreme + oznaka testa
        /// </summary>
        [Category("emailSendTask")]
        [DisplayName("RunStamp")]
        [Description("Referenca prema testiranju na koje se ovaj odgovor odnosi")]
        public string lastRunStamp
        {
            get { return _lastRunStamp; }
            set
            {
                _lastRunStamp = value;
                OnPropertyChanged("lastRunStamp");
            }
        }

        #endregion --- lastRunStamp ------- Potpis poslednjeg izvrsavanja: datum_vreme + oznaka testa
    }
}