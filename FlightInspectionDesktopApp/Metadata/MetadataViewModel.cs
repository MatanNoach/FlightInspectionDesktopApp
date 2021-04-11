using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Metadata
{
    class MetadataViewModel : INotifyPropertyChanged
    {
        // fields of MetadataViewModel
        private MetadataModel model;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// MetadataViewModel constructor.
        /// </summary>
        /// <param name="model">MetadataModel</param>
        public MetadataViewModel(MetadataModel model)
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

        // properties:

        /// <summary>
        /// Property of Altitude from the model object.
        /// </summary>
        public double VMAltitude
        {
            // getter of altitude from the model object.
            get
            {
                return model.Altitude;
            }
        }

        /// <summary>
        /// Property of AirSpeed from the model object.
        /// </summary>
        public double VMAirSpeed
        {
            // getter of airSpeed from the model object.
            get
            {
                return model.AirSpeed;
            }
        }

        /// <summary>
        /// Property of Heading from the model object.
        /// </summary>
        public double VMHeading
        {
            // getter of heading from the model object.
            get
            {
                return model.Heading;
            }
        }

        /// <summary>
        /// Property of Pitch from the model object.
        /// </summary>
        public double VMPitch
        {
            // getter of pitch from the model object.
            get
            {
                return model.Pitch;
            }
        }

        /// <summary>
        /// Property of Roll from the model object.
        /// </summary>
        public double VMRoll
        {
            // getter of roll from the model object.
            get
            {
                return model.Roll;
            }
        }

        /// <summary>
        /// Property of SideSlip from the model object.
        /// </summary>
        public double VMSideSlip
        {
            // getter of sideSlip from the model object.
            get
            {
                return model.SideSlip;
            }
        }
    }
}
