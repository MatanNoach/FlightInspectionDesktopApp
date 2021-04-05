using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Steering
{
    class SteeringViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private SteeringModel model;

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

        // properties
        public double VMAileron { get { return model.Aileron; } }
        public double VMElevator { get { return model.Elevator; } }
        public double VMRudder { get { return model.Rudder; } }
        public double VMThrottle { get { return model.Throttle; } }
    }
}
