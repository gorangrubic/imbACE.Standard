namespace imbACE.Network.web.result
{
    #region imbVeles using

    using imbACE.Network.web.request;
    using imbSCI.Core.extensions.data;
    using imbSCI.Core.reporting;
    using imbSCI.Data.data;
    using System;
    using System.Data;
    using System.Xml.Serialization;

    #endregion imbVeles using

    /// <summary>
    /// Osnova klase sa rezultatom webRequest-a
    /// </summary>
    public abstract class webResultBase : imbBindable, IAppendDataFields
    {
        /// <summary>
        /// Appends its data points into new or existing property collection
        /// </summary>
        /// <param name="data">Property collection to add data into</param>
        /// <returns>Updated or newly created property collection</returns>
        public PropertyCollection AppendDataFields(PropertyCollection data = null)
        {
            if (data == null) data = new PropertyCollection();
            this.buildPropertyCollection(false, false, "doc", data);

            document.AppendDataFields(data);
            response.AppendDataFields(data);
            request.AppendDataFields(data);

            // data[target.doc_name] = name;
            // data[target.doc_description] = description;
            // data[target.doc_id] = id;
            // data[target.doc_url] = url;

            // throw new NotImplementedException();

            return data;
        }

        #region --- dataObject ------- Opcioni objekat za smestanje podataka

        private Object _dataObject;

        /// <summary>
        /// Opcioni objekat za smestanje podataka
        /// </summary>
        public Object dataObject
        {
            get { return _dataObject; }
            set
            {
                _dataObject = value;
                OnPropertyChanged("dataObject");
            }
        }

        #endregion --- dataObject ------- Opcioni objekat za smestanje podataka

        #region --- document ------- Web dokument koji je ucitan

        private webDocument _document = new webDocument();

        /// <summary>
        /// Web dokument koji je ucitan
        /// </summary>
        public webDocument document
        {
            get { return _document; }
            set
            {
                _document = value;
                OnPropertyChanged("document");
            }
        }

        #endregion --- document ------- Web dokument koji je ucitan

        #region --- response ------- Web odgovor koji je dobijen

        private webResponse _response;

        /// <summary>
        /// Web odgovor koji je dobijen
        /// </summary>
        public webResponse response
        {
            get { return _response; }
            set
            {
                _response = value;
                OnPropertyChanged("response");
            }
        }

        #endregion --- response ------- Web odgovor koji je dobijen

        #region --- request ------- Instanca prema requestu koji je obradio ovaj odgovor

        private webRequest _request;

        /// <summary>
        /// Instanca prema requestu koji je obradio ovaj odgovor
        /// </summary>
        [XmlIgnore]
        public webRequest request
        {
            get { return _request; }
            set
            {
                _request = value;
                OnPropertyChanged("request");
            }
        }

        #endregion --- request ------- Instanca prema requestu koji je obradio ovaj odgovor
    }
}