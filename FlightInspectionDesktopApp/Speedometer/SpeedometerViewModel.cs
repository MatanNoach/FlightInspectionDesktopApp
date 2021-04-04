using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInspectionDesktopApp.Speedometer
{
    class SpeedometerViewModel : INotifyPropertyChanged
    {
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

        public double VMAirSpeed { get { return model.AirSpeed; } }

        public double VMSpeedometerAngle { get { return model.SpeedometerAngle; } }
    }
}
