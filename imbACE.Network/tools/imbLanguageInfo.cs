namespace imbACE.Network.tools
{
    using imbSCI.Data.data;

    #region imbVELES USING

    using System;
    using System.ComponentModel;

    #endregion imbVELES USING

    /// <summary>
    /// Basic Project Module [imbLanguageInfo] - imbVeles v4.0 lightClassTemplate
    /// </summary>
    /// <remarks>
    /// Resurs koji koristi nove tehnike npr. nestedModules
    /// </remarks>
    public class imbLanguageInfo : imbBindable
    {
        #region <------ OSNOVNE METODE I KONSTRUKTORI --- >

        public imbLanguageInfo()
        {
        }

        #endregion <------ OSNOVNE METODE I KONSTRUKTORI --- >

        #region -----------  name  -------  [Ime jezika na engleskom]

        private String _name;

        /// <summary>
        /// Ime jezika na engleskom
        /// </summary>
        // [XmlIgnore]
        [Category("imbLanguageInfo")]
        [DisplayName("name")]
        [Description("Ime jezika na engleskom")]
        public String name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("name");
            }
        }

        #endregion -----------  name  -------  [Ime jezika na engleskom]

        #region -----------  isoCode2L  -------  [Oznaka jezika po dvoslovnom ISO standardu ]

        private String _isoCode2L;

        /// <summary>
        /// Oznaka jezika po dvoslovnom ISO standardu
        /// </summary>
        // [XmlIgnore]
        //[imbSql(sqlEntityPropMode.unique)]
        [Category("imbLanguageInfo")]
        [DisplayName("isoCode2L")]
        [Description("Oznaka jezika po dvoslovnom ISO standardu ")]
        public String isoCode2L
        {
            get { return _isoCode2L; }
            set
            {
                _isoCode2L = value;
                OnPropertyChanged("isoCode2L");
            }
        }

        #endregion -----------  isoCode2L  -------  [Oznaka jezika po dvoslovnom ISO standardu ]
    }
}