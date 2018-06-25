// --------------------------------------------------------------------------------------------------------------------
// <copyright file="emailServer.cs" company="imbVeles" >
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
    using imbSCI.Core.reporting;
    using imbSCI.Data;
    using imbSCI.Data.collection;
    using imbSCI.Data.data;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex;
    using imbSCI.Reporting;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.interfaces;
    using System;
    using System.ComponentModel;
    using System.Net;
    using System.Net.Mail;
    using System.Xml.Serialization;

    #endregion imbVeles using

    /// <summary>
    ///
    /// </summary>
    public class emailServer : imbBindable
    {
        private string _loginPassword; // = new String();
        private string _smtpHostUrl = "smtp.gmail.com"; // = new String();
        private SmtpClient _smtpServer;
        private int _smtpServerPort = 587; // = new Int32();
        private bool _useSSL = true;

        #region --- login ------- podaci o logovanju na server

        private NetworkCredential _login;

        /// <summary>
        /// podaci o logovanju na server
        /// </summary>
        public NetworkCredential login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged("login");
            }
        }

        #endregion --- login ------- podaci o logovanju na server

        public emailServer()
        {
            //_emailClientModule = emailClientModule1;
        }

        #region ----------- Boolean [ isConnectedToSMTP ] -------  [Da li je konektovan na SMTP]

        private bool _isConnectedToSMTP = false;

        /// <summary>
        /// Da li je konektovan na SMTP
        /// </summary>
        [Category("Switches")]
        [DisplayName("isConnectedToSMTP")]
        [Description("Da li je konektovan na SMTP")]
        public bool isConnectedToSMTP
        {
            get { return _isConnectedToSMTP; }
            set
            {
                _isConnectedToSMTP = value;
                OnPropertyChanged("isConnectedToSMTP");
            }
        }

        #endregion ----------- Boolean [ isConnectedToSMTP ] -------  [Da li je konektovan na SMTP]

        #region --- isLogedInAndReady ------- da li je spreman za upotrebu?

        private bool _isLogedInAndReady;

        /// <summary>
        /// da li je spreman za upotrebu?
        /// </summary>
        public bool isLogedInAndReady
        {
            get { return _isLogedInAndReady; }
            set
            {
                _isLogedInAndReady = value;
                OnPropertyChanged("isLogedInAndReady");
            }
        }

        #endregion --- isLogedInAndReady ------- da li je spreman za upotrebu?

        /// <summary>
        /// Port kojim pristupa serveru
        /// </summary>
        // [XmlIgnore]
        [Category("emailClientModule")]
        [DisplayName("smtpServerPort")]
        [Description("Port kojim pristupa serveru")]
        public int smtpServerPort
        {
            get { return _smtpServerPort; }
            set
            {
                // Boolean chg = (_smtpServerPort != value);
                _smtpServerPort = value;
                OnPropertyChanged("smtpServerPort");
                // if (chg) {}
            }
        }

        /// <summary>
        /// adresa na kojoj se nalazi server
        /// </summary>
        // [XmlIgnore]
        [Category("emailClientModule")]
        [DisplayName("smtpHostUrl")]
        [Description("adresa na kojoj se nalazi server")]
        public string smtpHostUrl
        {
            get { return _smtpHostUrl; }
            set
            {
                // Boolean chg = (_smtpHostUrl != value);
                _smtpHostUrl = value;
                OnPropertyChanged("smtpHostUrl");
                // if (chg) {}
            }
        }

        /// <summary>
        /// da li da koristi SSL konekciju
        /// </summary>
        [Category("Switches")]
        [DisplayName("useSSL")]
        [Description("da li da koristi SSL konekciju")]
        public bool useSSL
        {
            get { return _useSSL; }
            set
            {
                _useSSL = value;
                OnPropertyChanged("useSSL");
            }
        }

        /// <summary>
        /// Password za login - ako je prazan onda ce otvoriti input box -- privremena varijabla, ne snima se
        /// </summary>
        [XmlIgnore]
        [Category("emailClientModule")]
        [DisplayName("loginPassword")]
        [Description("Password za login - ako je prazan onda ce otvoriti input box")]
        public string loginPassword
        {
            get { return _loginPassword; }
            set
            {
                // Boolean chg = (_loginPassword != value);
                _loginPassword = value;
                OnPropertyChanged("loginPassword");
                // if (chg) {}
            }
        }

        /// <summary>
        /// instanca SMTP servera koji se koristi za slanje poste
        /// </summary>
        [XmlIgnore]
        public SmtpClient smtpServer
        {
            get { return _smtpServer; }
            set
            {
                _smtpServer = value;
                OnPropertyChanged("smtpServer");
            }
        }

        #region -----------  loginUsername  -------  [snimljen username za logovanje na server]

        private string _loginUsername = ""; // = new String();

        /// <summary>
        /// snimljen username za logovanje na server
        /// </summary>
        // [XmlIgnore]
        [Category("emailServer")]
        [DisplayName("loginUsername")]
        [Description("snimljen username za logovanje na server")]
        public string loginUsername
        {
            get { return _loginUsername; }
            set
            {
                // Boolean chg = (_loginUsername != value);
                _loginUsername = value;
                OnPropertyChanged("loginUsername");
                // if (chg) {}
            }
        }

        #endregion -----------  loginUsername  -------  [snimljen username za logovanje na server]

        /// <summary>
        /// Uspostavlja vezu za Smtp serverom
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="__smtpHostUrl"></param>
        /// <param name="__useSSL"></param>
        /// <param name="__smtpServerPort"></param>
        /// <returns></returns>
        public bool connectSmtp(string username, string password, string __smtpHostUrl = "", bool __useSSL = true,
                                   int __smtpServerPort = -1)
        {
            if (isLogedInAndReady)
            {
                //logSystem.log(
                //    "emailServer::smtp is already connected (" + smtpHostUrl +
                //    ") -- to reconnect, close the connection first", logType.Notification);
                return false;
            }

            if (!string.IsNullOrEmpty(__smtpHostUrl)) smtpHostUrl = __smtpHostUrl;

            useSSL = __useSSL;

            if (__smtpServerPort > -1) smtpServerPort = __smtpServerPort;

            smtpServer = new SmtpClient(smtpHostUrl, smtpServerPort);
            smtpServer.UseDefaultCredentials = false;
            smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpServer.Timeout = 25000;
            smtpServer.EnableSsl = useSSL;

            isConnectedToSMTP = (smtpServer != null);

            if (!isConnectedToSMTP) return false;

            login = new System.Net.NetworkCredential(username, password);

            smtpServer.Credentials = login;

            isLogedInAndReady = true;
            return true;
        }

        /// <summary>
        /// Disconnecting from the SMTP server
        /// </summary>
        public void disconnectSmtp()
        {
            smtpServer.Credentials = null;
            smtpServer = null;

            isConnectedToSMTP = false;
            isLogedInAndReady = false;
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="loger">The loger.</param>
        /// <returns></returns>
        public Boolean Send(emailMessage message, ILogBuilder loger)
        {
            try
            {
                var msg = message.createMailObject();

                smtpServer.Send(msg);

                loger.log(message.address + " sent");
                return true;
            }
            catch (Exception ex)
            {
                loger.log(message.address + " failed [" + ex.Message + "]");
                return false;
            }
        }

        /// <summary>
        /// Salje i vraca TRUE ako je uspeo
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool Send(MailMessage msg)
        {
            if (aceCommonNetworkSystem.main.doEnableEmailSending) return false;
            try
            {
                smtpServer.Send(msg);
                return true;
            }
            catch (Exception ex)
            {
                //   this.note(ex, "Send failed for: " + msg.Subject);
            }
            return false;
        }
    }
}