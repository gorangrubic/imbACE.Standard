using System;
using System.Collections.Generic;
using System.Linq;

namespace imbACE.Network.email
{
    using imbACE.Core;
    using imbACE.Core.core.diagnostic;
    using imbSCI.Core;
    using imbSCI.Core.attributes;
    using imbSCI.Core.enums;
    using imbSCI.Core.extensions.text;
    using imbSCI.Core.extensions.typeworks;
    using imbSCI.Core.files.folders;
    using imbSCI.Core.interfaces;
    using imbSCI.Core.reporting;
    using imbSCI.Core.reporting.template;
    using imbSCI.Data;
    using imbSCI.Data.collection;
    using imbSCI.Data.data;
    using imbSCI.Data.interfaces;
    using imbSCI.DataComplex;
    using imbSCI.DataComplex.extensions.data.formats;
    using imbSCI.Reporting;
    using imbSCI.Reporting.enums;
    using imbSCI.Reporting.includes;
    using imbSCI.Reporting.interfaces;
    using System.Data;
    using System.Xml.Serialization;

    /// <summary>
    /// Class that builds the email collections using templates and DataTable source
    /// </summary>
    public class emailMessageFactory
    {
        public emailMessageFactory()
        {
        }

        /// <summary>
        /// Deploys the specified loger.
        /// </summary>
        /// <param name="loger">The loger.</param>
        /// <param name="workspace">The workspace.</param>
        public void deploy(ILogBuilder loger, folderNode workspace)
        {
            if (!dataSourcePath.isNullOrEmpty())
            {
                //dataSource = csvFileExtensions.fromCsvFileToTable(dataSourcePath, dataSource, true);

                dataSource = imbDataTableExtensions.deserializeDataTable(dataSourcePath, imbSCI.Data.enums.reporting.dataTableExportEnum.excel, workspace);
            }

            if (!folderWithAttachmentsName.isNullOrEmpty())
            {
                folderWithAttachments = workspace.Add(folderWithAttachmentsName, "Attachments", "Folder with attachment files, to be send via email");
            }
        }

        /// <summary>
        /// Creates message collections
        /// </summary>
        /// <param name="loger">The loger.</param>
        /// <param name="workspace">The workspace.</param>
        /// <param name="showMessage">if set to <c>true</c> [show message].</param>
        /// <returns></returns>
        public List<emailMessageCollection> createCollections(ILogBuilder loger, folderNode workspace, Boolean showMessage = true)
        {
            List<emailMessageCollection> output = new List<emailMessageCollection>();

            stringTemplate subject = new stringTemplate(subjectTemplate);

            stringTemplate content = new stringTemplate(contentTemplate);

            stringTemplate address = new stringTemplate(addressTemplate);

            emailMessageCollection messageCollection = new emailMessageCollection();
            messageCollection.name = "block01";
            output.Add(messageCollection);

            foreach (DataRow row in dataSource.Rows)
            {
                emailMessage message = new emailMessage();
                message.from = fromAddress;
                message.content = content.applyToContent(row);
                message.subject = subject.applyToContent(row);
                message.address = address.applyToContent(row);

                if (showMessage)
                {
                    loger.AppendLine("Subject: " + message.subject);
                    loger.AppendLine("Content: " + message.content);
                }

                messageCollection.Add(message);
                if (messageCollection.Count >= collectionSize)
                {
                    messageCollection.Save(workspace.pathFor(messageCollection.name + ".xml", imbSCI.Data.enums.getWritableFileMode.overwrite, "Block of messages", true));
                    messageCollection = new emailMessageCollection();
                    messageCollection.name = "block" + output.Count.ToString("D2");
                    output.Add(messageCollection);
                    loger.log("Message block created: " + messageCollection.name);
                }
            }

            messageCollection.Save(workspace.pathFor(messageCollection.name + ".xml", imbSCI.Data.enums.getWritableFileMode.overwrite, "Block of messages", true));

            return output;
        }

        public String fromAddress { get; set; } = "";

        /// <summary>
        /// Gets or sets the data source.
        /// </summary>
        /// <value>
        /// The data source.
        /// </value>
        [XmlIgnore]
        public DataTable dataSource { get; set; }

        /// <summary>
        /// Gets or sets the data source path.
        /// </summary>
        /// <value>
        /// The data source path.
        /// </value>
        public String dataSourcePath { get; set; }

        /// <summary>
        /// Gets or sets the content template.
        /// </summary>
        /// <value>
        /// The content template.
        /// </value>
        public string contentTemplate { get; set; } = "";

        /// <summary>
        /// Gets or sets the subject template.
        /// </summary>
        /// <value>
        /// The subject template.
        /// </value>
        public string subjectTemplate { get; set; } = "";

        /// <summary>
        /// Gets or sets the address template.
        /// </summary>
        /// <value>
        /// The address template.
        /// </value>
        [XmlAttribute()]
        public String addressTemplate { get; set; } = "";

        /// <summary>
        /// Size of one collection (batch)
        /// </summary>
        /// <value>
        /// The size of the collection.
        /// </value>
        [XmlAttribute()]
        public Int32 collectionSize { get; set; } = 175;

        [XmlAttribute()]
        public String folderWithAttachmentsName { get; set; } = "";

        [XmlIgnore]
        public folderNode folderWithAttachments { get; set; }
    }
}