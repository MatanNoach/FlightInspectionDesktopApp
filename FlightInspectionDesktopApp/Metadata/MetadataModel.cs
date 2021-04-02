using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Metadata
{
    class MetadataModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static MetadataModel metadataModelIns;

        private MetadataModel() { }

        public static MetadataModel Instance
        {
            get
            {
                if (metadataModelIns == null)
                {
                    throw new Exception("DataModel was not created");
                }
                return metadataModelIns;
            }
        }

        public static void CreateModel()
        {
            if (metadataModelIns != null)
            {
                throw new Exception("DataModel is already created");
            }
            metadataModelIns = new MetadataModel();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private double altitude;
        private double airSpeed;
        private double heading;
        private double pitch;
        private double roll;
        private double sideSlip;

        public double Altitude
        {
            get
            {
                return altitude;
            }
            set
            {
                altitude = value;
                NotifyPropertyChanged("Altitude");
            }
        }
        public double AirSpeed
        {
            get
            {
                return airSpeed;
            }
            set
            {
                airSpeed = value;
                NotifyPropertyChanged("AirSpeed");
            }
        }

        public double Heading
        {
            get
            {
                return heading;
            }
            set
            {
                heading = value;
                NotifyPropertyChanged("Heading");
            }
        }
        public double Pitch
        {
            get
            {
                return pitch;
            }
            set
            {
                pitch = value;
                NotifyPropertyChanged("Pitch");
            }
        }
        public double Roll
        {
            get
            {
                return roll;
            }
            set
            {
                roll = value;
                NotifyPropertyChanged("Roll");
            }
        }
        public double SideSlip
        {
            get
            {
                return sideSlip; ;
            }
            set
            {
                sideSlip = value;
                NotifyPropertyChanged("SideSlip");
            }
        }
    }
}
