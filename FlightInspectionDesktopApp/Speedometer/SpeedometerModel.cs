using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Speedometer
{
    class SpeedometerModel : INotifyPropertyChanged
    {
        // fields of SpeedometerModel object.
        private double airSpeed;
        private double speedometerAngle;
        private static SpeedometerModel speedometerModelIns;
        public event PropertyChangedEventHandler PropertyChanged;

        // calculated fields of SpeedometerModel object.
        private static double angleDiff = Properties.Settings.Default.speedometerEndAngle - Properties.Settings.Default.speedometerStartAngle;
        private static double airSpeedDiff = DataModel.Instance.getMaxValueByKey(Properties.Settings.Default.airspeed) -
            DataModel.Instance.getMinValueByKey(Properties.Settings.Default.airspeed);
        private static double part = angleDiff / airSpeedDiff;

        /// <summary>
        /// private CTOR of SpeedometerModel object.
        /// </summary>
        private SpeedometerModel() { }

        /// <summary>
        /// a static property of field speedometerModelIns.
        /// </summary>
        public static SpeedometerModel Instance
        {
            // getter of speedometerModelIns.
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
        /// Creates a SpeedometerModel object.
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

        // Properties:

        /// <summary>
        /// Proeprty of field airSpeed.
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
        /// Property of speedometerAngle.
        /// </summary>
        public double SpeedometerAngle
        {
            // getter of speedometerAngle.
            get
            {
                return speedometerAngle;
            }

            // setter of speedometerAngle.
            set
            {
                speedometerAngle = value;
                NotifyPropertyChanged("SpeedometerAngle");
            }
        }

        /// <summary>
        /// this function calculates the angle of the speed
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        internal double calculateSpeedometerAngle(double speed)
        {
            return Properties.Settings.Default.speedometerStartAngle + speed * part;
        }
    }
}
