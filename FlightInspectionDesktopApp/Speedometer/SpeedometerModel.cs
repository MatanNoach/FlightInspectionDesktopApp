using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInspectionDesktopApp.Speedometer
{
    class SpeedometerModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static SpeedometerModel speedometerModelIns;
        private double airSpeed;
        private double speedometerAngle;

        private static double angleDiff = Properties.Settings.Default.speedometerEndAngle - Properties.Settings.Default.speedometerStartAngle;
        private static double airSpeedDiff = DataModel.Instance.getMaxValueByKey("airspeed-kt") - DataModel.Instance.getMinValueByKey("airspeed-kt");
        private static double part = angleDiff / airSpeedDiff;

        private SpeedometerModel() { }

        public static SpeedometerModel Instance
        {
            get
            {
                if (speedometerModelIns == null)
                {
                    throw new Exception("SpeedometerModel was not created");
                }
                return speedometerModelIns;
            }
        }

        /// <summary>
        /// Creates a SpeedometerModel.
        /// </summary>
        public static void CreateModel()
        {
            if (speedometerModelIns != null)
            {
                throw new Exception("speedometerModel is already created");
            }
            speedometerModelIns = new SpeedometerModel();
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

        public double SpeedometerAngle
        {
            get
            {
                return speedometerAngle;
            }
            set
            {
                speedometerAngle = value;
                NotifyPropertyChanged("SpeedometerAngle");
            }
        }

        internal double calculateSpeedometerAngle(double speed)
        {
            return Properties.Settings.Default.speedometerStartAngle + speed * part;
        }
    }
}
