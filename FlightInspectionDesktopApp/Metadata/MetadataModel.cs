using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Metadata
{
    class MetadataModel : INotifyPropertyChanged
    {
        // fields of MetadataModel object.
        private double altitude;
        private double airSpeed;
        private double heading;
        private double pitch;
        private double roll;
        private double sideSlip;
        private static MetadataModel metadataModelIns;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Private CTOR for singleton implementation.
        /// </summary>
        private MetadataModel() { }

        /// <summary>
        /// a static property of metadataModelIns.
        /// </summary>
        public static MetadataModel Instance
        {
            // getter of metadataModelIns.
            get
            {
                if (metadataModelIns == null)
                {
                    throw new Exception("MetadataModel was not created");
                }
                return metadataModelIns;
            }
        }

        /// <summary>
        /// Creates a MetadataModel.
        /// </summary>
        public static void CreateModel()
        {
            if (metadataModelIns != null)
            {
                throw new Exception("MetadataModel is already created");
            }
            metadataModelIns = new MetadataModel();
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

        // properties:

        /// <summary>
        /// Propery of field altitude.
        /// </summary>
        public double Altitude
        {
            // getter of altitude.
            get
            {
                return altitude;
            }
            // setter of altitude.
            set
            {
                altitude = value;
                NotifyPropertyChanged("Altitude");
            }
        }

        /// <summary>
        /// Property of field airspeed.
        /// </summary>
        public double AirSpeed
        {
            // getter of airSpeed.
            get
            {
                return airSpeed;
            }

            // setter of airSpeed.
            set
            {
                airSpeed = value;
                NotifyPropertyChanged("AirSpeed");
            }
        }

        /// <summary>
        /// Property of field heading.
        /// </summary>
        public double Heading
        {
            // getter of heading.
            get
            {
                return heading;
            }

            // setter of heading.
            set
            {
                heading = value;
                NotifyPropertyChanged("Heading");
            }
        }

        /// <summary>
        /// Property of field pitch.
        /// </summary>
        public double Pitch
        {
            // getter of pitch.
            get
            {
                return pitch;
            }

            // setter of pitch.
            set
            {
                pitch = value;
                NotifyPropertyChanged("Pitch");
            }
        }

        /// <summary>
        /// Property of field roll.
        /// </summary>
        public double Roll
        {
            // getter of roll.
            get
            {
                return roll;
            }

            // setter of roll.
            set
            {
                roll = value;
                NotifyPropertyChanged("Roll");
            }
        }

        /// <summary>
        /// Property of field sideSlip.
        /// </summary>
        public double SideSlip
        {
            // getter of sideSlip.
            get
            {
                return sideSlip; ;
            }

            // setter of sideSlip.
            set
            {
                sideSlip = value;
                NotifyPropertyChanged("SideSlip");
            }
        }
    }
}
