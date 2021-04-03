using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Steering
{
    class SteeringModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static SteeringModel steeringModelIns;
        private double aileron;
        private double elevator;
        private double throttle;
        private double rudder;

        /// <summary>
        /// Private CTOR for singleton implementation.
        /// </summary>
        private SteeringModel() { }

        public static SteeringModel Instance
        {
            get
            {
                if (steeringModelIns == null)
                {
                    throw new Exception("MetadataModel was not created");
                }
                return steeringModelIns;
            }
        }

        /// <summary>
        /// Creates a SteeringModel.
        /// </summary>
        public static void CreateModel()
        {
            if (steeringModelIns != null)
            {
                throw new Exception("MetadataModel is already created");
            }
            steeringModelIns = new SteeringModel();
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
        public double Aileron
        {
            get
            {
                return aileron;
            }
            set
            {
                aileron = value;
                NotifyPropertyChanged("Aileron");
            }
        }

        public double Elevator
        {
            get
            {
                return elevator;
            }
            set
            {
                elevator = value;
                NotifyPropertyChanged("Elevator");
            }
        }

        public double Throttle
        {
            get
            {
                return throttle;
            }
            set
            {
                throttle = value;
                NotifyPropertyChanged("Throttle");
            }
        }

        public double Rudder
        {
            get
            {
                return rudder;
            }
            set
            {
                rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }
    }
}
