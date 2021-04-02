using System.ComponentModel;

interface IFGModel : INotifyPropertyChanged
{
    void Connect();
    void Disconnect();
    void Start(string PathCSV);
    void RunFG(string binFolder, string PathFG, string XMLFileName);

    // Properties:
    double Aileron { set; get; }
    double Elevator { set; get; }
    double Rudder { get; set; }
    double Throttle { get; set; }
    double Altitude { get; set; }
    double AirSpeed { get; set; }
    double Position { get; set; }
    double Heading { get; set; }
    double Pitch { get; set; }
    double Roll { get; set; }
    double SideSlip { get; set; }
}