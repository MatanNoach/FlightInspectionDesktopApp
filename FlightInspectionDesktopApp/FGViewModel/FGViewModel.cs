using System;
using System.Threading;
using System.ComponentModel;

namespace FlightInspectionDesktopApp
{
    class FGViewModel : INotifyPropertyChanged
    {
        private IFGModel model;

        public FGViewModel(IFGModel model)
        {
            this.model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
             {
                 NotifyPropertyChanged("VM" + e.PropertyName);
             };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void Disconnect()
        {
            model.Disconnect();
        }

        // Properties:
        public float VMAileron { get { return model.Aileron; } }
        public float VMElevator { get { return model.Elevator; } }

        public float VMRudder { get { return model.Rudder; } }
        public float VMThrottle { get { return model.Throttle; } }
        public float VMAltitude { get { return model.Altitude; } }
        public float VMAirSpeed { get { return model.AirSpeed; } }
        public double VMPosition { get { return model.Position; } }
        public float VMHeading { get { return model.Heading; } }
        public float VMPitch { get { return model.Pitch; } }
        public float VMRoll { get { return model.Roll; } }
        public float VMSideSlip { get { return model.SideSlip; } }


        public void Run(string binFolder, string PathFG, string XMLFileName, string PathCSV)
        {
            model.RunFG(binFolder, PathFG, XMLFileName);
            // for timing:
            Thread.Sleep(10000);
            model.Connect(Properties.Settings.Default.portGeneric);
            model.Start(PathCSV);
        }
    }
}