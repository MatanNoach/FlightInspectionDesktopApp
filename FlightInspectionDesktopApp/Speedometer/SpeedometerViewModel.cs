using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Speedometer
{
    class SpeedometerViewModel : INotifyPropertyChanged
    {
        // fields of SpeedometerViewModel object.
        private SpeedometerModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// SpeedometerViewModel constructor.
        /// </summary>
        /// <param name="model">MetadataModel</param>
        public SpeedometerViewModel(SpeedometerModel model)
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

        /// <summary>
        /// Property of AirSpeed for the view-model.
        /// </summary>
        public double VMAirSpeed
        {
            // getter of AirSpeed.
            get
            {
                return model.AirSpeed;
            }
        }

        /// <summary>
        /// Property of SpeedometerAngle for the view-model.
        /// </summary>
        public double VMSpeedometerAngle
        {
            // getter of SpeedometerAngle.
            get
            {
                return model.SpeedometerAngle;
            }
        }
    }
}
