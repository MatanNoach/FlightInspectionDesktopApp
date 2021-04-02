using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Metadata
{
    class MetadataViewModel : INotifyPropertyChanged
    {
        private MetadataModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// MetadataViewModel constructor.
        /// </summary>
        /// <param name="model">MetadataModel</param>
        public MetadataViewModel(MetadataModel model)
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

        // properties
        public double VMAltitude { get { return model.Altitude; } }
        public double VMAirSpeed { get { return model.AirSpeed; } }
        public double VMHeading { get { return model.Heading; } }
        public double VMPitch { get { return model.Pitch; } }
        public double VMRoll { get { return model.Roll; } }
        public double VMSideSlip { get { return model.SideSlip; } }
    }
}
