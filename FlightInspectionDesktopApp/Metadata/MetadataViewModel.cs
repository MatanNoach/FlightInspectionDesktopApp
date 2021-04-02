using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Metadata
{
    class MetadataViewModel : INotifyPropertyChanged
    {
        private MetadataModel model;

        public MetadataViewModel(MetadataModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM" + e.PropertyName);
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public double VMAltitude { get { return model.Altitude; } }
        public double VMAirSpeed { get { return model.AirSpeed; } }
        public double VMHeading { get { return model.Heading; } }
        public double VMPitch { get { return model.Pitch; } }
        public double VMRoll { get { return model.Roll; } }
        public double VMSideSlip { get { return model.SideSlip; } }
    }
}
