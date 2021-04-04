using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Altimeter
{
    class AltimeterModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static AltimeterModel altimeterModelIns;
        private double altimeter;

        private AltimeterModel() { }

        public static AltimeterModel Instance
        {
            get
            {
                if (altimeterModelIns == null)
                {
                    throw new Exception("AltimeterModel was not created");
                }
                return altimeterModelIns;
            }
        }

        /// <summary>
        /// Creates a AltimeterModel.
        /// </summary>
        public static void CreateModel()
        {
            if (altimeterModelIns != null)
            {
                throw new Exception("AltimeterModel is already created");
            }
            altimeterModelIns = new AltimeterModel();
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

        public double Altimeter
        {
            get
            {
                return altimeter;
            }
            set
            {
                altimeter = value;
                NotifyPropertyChanged("Altimeter");
            }
        }
    }
}
