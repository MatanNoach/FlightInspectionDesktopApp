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
        public double VMAileron { get { return model.Aileron; } }
        public double VMElevator { get { return model.Elevator; } }

        public double VMRudder { get { return model.Rudder; } }
        public double VMThrottle { get { return model.Throttle; } }
        public double VMAltitude { get { return model.Altitude; } }
        public double VMAirSpeed { get { return model.AirSpeed; } }
        public double VMPosition { get { return model.Position; } }
        public double VMHeading { get { return model.Heading; } }
        public double VMPitch { get { return model.Pitch; } }
        public double VMRoll { get { return model.Roll; } }
        public double VMSideSlip { get { return model.SideSlip; } }


        public void Run(string binFolder, string PathFG, string XMLFileName, string PathCSV)
        {
            model.RunFG(binFolder, PathFG, XMLFileName);
            // for timing:
            Thread.Sleep(10000);
            model.Connect();
            model.Start(PathCSV);
        }
    }
}