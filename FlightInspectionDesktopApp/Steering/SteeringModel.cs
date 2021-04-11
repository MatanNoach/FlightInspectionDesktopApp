using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Steering
{
    class SteeringModel : INotifyPropertyChanged
    {
        // fields of SteeringModel object.
        private double rudder;
        private double aileron;
        private double elevator;
        private double throttle;
        private static SteeringModel steeringModelIns;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Private CTOR for singleton implementation.
        /// </summary>
        private SteeringModel() { }

        /// <summary>
        /// a static property of steeringModelIns.
        /// </summary>
        public static SteeringModel Instance
        {
            // getter of steeringModeIns.
            get
            {
                if (steeringModelIns == null)
                {
                    throw new Exception("SteeringModel was not created");
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
                throw new Exception("SteeringModel is already created");
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

        // Properties

        /// <summary>
        /// Property of field aileron.
        /// </summary>
        public double Aileron
        {
            // getter of aileron.
            get
            {
                return aileron;
            }

            // setter of aileron.
            set
            {
                aileron = value;
                NotifyPropertyChanged("Aileron");
            }
        }

        /// <summary>
        /// Property of field elevator.
        /// </summary>
        public double Elevator
        {
            // getter of elevator.
            get
            {
                return elevator;
            }

            // setter of elevator.
            set
            {
                elevator = value;
                NotifyPropertyChanged("Elevator");
            }
        }

        /// <summary>
        /// Property of field throttle.
        /// </summary>
        public double Throttle
        {
            // getter of throttle.
            get
            {
                return throttle;
            }

            // setter of throttle.
            set
            {
                throttle = value;
                NotifyPropertyChanged("Throttle");
            }
        }

        /// <summary>
        /// Property of field rudder.
        /// </summary>
        public double Rudder
        {
            // getter of rudder.
            get
            {
                return rudder;
            }

            // setter of rudder.
            set
            {
                rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }
    }
}
