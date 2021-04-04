using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Altimeter
{
    class AltimeterViewModel : INotifyPropertyChanged
    {
        private AltimeterModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// AltimeterViewModel constructor.
        /// </summary>
        /// <param name="model">MetadataModel</param>
        public AltimeterViewModel(AltimeterModel model)
        {
            this.model = model;
            // when a property in MetadataModel changes, indicate it changed in MetadataViewModel as well
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM" + e.PropertyName);
            };
        }

        /// <summary>
        /// Evokes all subscribed methods of PropertyChanged.
        /// </summary>
        /// <param name="propName">name of the property that's been changed</param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public double VMAltimeter { get { return model.Altimeter; } }
    }
}
