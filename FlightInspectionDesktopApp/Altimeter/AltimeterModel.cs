using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Altimeter
{
    class AltimeterModel : INotifyPropertyChanged
    {
        // fields of AlimeterModel
        private double altimeter;
        private static AltimeterModel altimeterModelIns;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// private CTOR of AltimeterModel object
        /// </summary>
        private AltimeterModel() { }

        /// <summary>
        /// a static property of field altimeterModelIns
        /// </summary>
        public static AltimeterModel Instance
        {
            // getter of altimeterModelIns.
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
        /// Creates an AltimeterModel object.
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

        /// <summary>
        /// Property of field altimeter.
        /// </summary>
        public double Altimeter
        {
            // getter of altimeter.
            get
            {
                return altimeter;
            }

            // setter of altimeter.
            set
            {
                altimeter = value;
                NotifyPropertyChanged("Altimeter");
            }
        }
    }
}
