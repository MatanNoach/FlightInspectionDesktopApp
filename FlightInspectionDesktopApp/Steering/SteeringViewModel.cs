using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Steering
{
    class SteeringViewModel : INotifyPropertyChanged
    {
        // fields of SteeringViewModel object.
        private SteeringModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// SteeringViewModel constructor.
        /// </summary>
        /// <param name="model">MetadataModel</param>
        public SteeringViewModel(SteeringModel model)
        {
            this.model = model;
            // when a property in SteeringModel changes, indicate it changed in SteeringViewModel as well
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

        // Properties:

        /// <summary>
        /// Property of Aileron for the view-model.
        /// </summary>
        public double VMAileron
        {
            // getter of Aileron.
            get
            {
                return model.Aileron;
            }
        }

        /// <summary>
        /// Property of Elevator for the view-model.
        /// </summary>
        public double VMElevator
        {
            // getter of elevator.
            get
            {
                return model.Elevator;
            }
        }

        /// <summary>
        /// Property of Rudder for the view-model.
        /// </summary>
        public double VMRudder
        {
            // getter of the rudder.
            get
            {
                return model.Rudder;
            }
        }

        /// <summary>
        /// Property of Throttle for the view-model.
        /// </summary>
        public double VMThrottle
        {
            // getter of throttle.
            get
            {
                return model.Throttle;
            }
        }
    }
}
