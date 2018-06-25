using imbSCI.Core.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace imbACE.Generator.ui
{
    [Flags]
    public enum universalViewModelOptions
    {

        none = 0,
        autoContextualHelpOnChange = 1,

    }


    /// <summary>
    /// Universal ViewModel class - wrapping the edited object
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class universalViewModel : INotifyPropertyChanged
    {



        private String _Title;
        /// <summary> Title of the view model </summary>
        public String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                OnPropertyChanged(nameof(Title));
            }
        }


        private String _Description;
        /// <summary> </summary>
        public String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
                OnPropertyChanged("Description");
            }
        }



        private String _ContextualHelp;
        /// <summary> Contextual help </summary>
        public String ContextualHelp
        {
            get
            {
                return _ContextualHelp;
            }
            set
            {
                _ContextualHelp = value;
                OnPropertyChanged("ContextualHelp");
            }
        }



        private Object _DataObject;
        /// <summary> data object that is being edited </summary>
        public Object DataObject
        {
            get
            {
                return _DataObject;
            }
            set
            {
                _DataObject = value;
                OnPropertyChanged("DataObject");
            }
        }



        /// <summary>
        /// Builds universal view model with data object information
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        public universalViewModel(Object dataObject)
        {
            Deploy(dataObject);

        }

        public universalViewModel()
        {

        }


        /// <summary>
        /// Gets or sets the data object information.
        /// </summary>
        /// <value>
        /// The data object information.
        /// </value>
        public settingsEntriesForObject DataObjectInfo { get; set; }

        /// <summary>
        /// Options controling how view model is updated
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public universalViewModelOptions options { get; set; } = universalViewModelOptions.none;

        /// <summary>
        /// Deploys the specified data object.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        public void Deploy(Object dataObject)
        {

            DataObjectInfo = new settingsEntriesForObject(dataObject, false);
            DataObject = dataObject;

            if (DataObject is INotifyPropertyChanged DataObjectPropertyChanged)
            {
                DataObjectPropertyChanged.PropertyChanged += DataObjectPropertyChanged_PropertyChanged;
            }

            Title = DataObjectInfo.DisplayName;
            Description = DataObjectInfo.Description;
        }

        private void DataObjectPropertyChanged_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (options.HasFlag(universalViewModelOptions.autoContextualHelpOnChange))
            {
                SetContextualHelp(e.PropertyName);
            }

        }




        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Sets the contextual help
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected void SetContextualHelp(String propertyName)
        {
            if (propertyName != nameof(ContextualHelp))
            {
                if (DataObjectInfo.spes.ContainsKey(propertyName))
                {
                    ContextualHelp = DataObjectInfo.spes[propertyName].description;
                }
            }
        }


        protected void OnPropertyChanged(String propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
