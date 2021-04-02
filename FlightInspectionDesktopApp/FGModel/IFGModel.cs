using System.ComponentModel;

interface IFGModel : INotifyPropertyChanged
{
    void Connect(int port);
    void Disconnect();
    void Start(string PathCSV);
    void RunFG(string binFolder, string PathFG, string XMLFileName);

    // Properties:
    float Aileron { set; get; }
    float Elevator { set; get; }
    float Rudder { get; set; }
    float Throttle { get; set; }
    float Altitude { get; set; }
    float AirSpeed { get; set; }
    double Position { get; set; }
    float Heading { get; set; }
    float Pitch { get; set; }
    float Roll { get; set; }
    float SideSlip { get; set; }

}