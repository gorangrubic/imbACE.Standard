namespace imbACE.Network.tools
{
    using imbSCI.Data.data;

    #region imbVELES USING

    using System;
    using System.ComponentModel;

    #endregion imbVELES USING

    /// <summary>
    /// Klasa koja opisuje razlicite tipove veb servisa
    /// </summary>
    public class imbServiceType : imbBindable
    {
        #region -----------  title  -------  [Naziv vrste servisa]

        private String _title;

        /// <summary>
        /// Naziv vrste servisa
        /// </summary>
        // [XmlIgnore]
        //[imbSql(sqlEntityPropMode.unique)]
        [Category("imbServiceType")]
        [DisplayName("title")]
        [Description("Naziv vrste servisa")]
        public String title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("title");
            }
        }

        #endregion -----------  title  -------  [Naziv vrste servisa]

        #region -----------  description  -------  [Opis servisa]

        private String _description;

        /// <summary>
        /// Opis servisa
        /// </summary>
        // [XmlIgnore]
        [Category("imbServiceType")]
        [DisplayName("description")]
        [Description("Opis servisa")]
        public String description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }

        #endregion -----------  description  -------  [Opis servisa]
    }
}