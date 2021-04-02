using System.ComponentModel;

interface IFGModel : INotifyPropertyChanged
{
    void Connect();
    void Disconnect();
    void Start(string PathCSV);
    void RunFG(string binFolder, string PathFG, string XMLFileName);

    //? will be removed from here
    // Properties:
    double Aileron { set; get; }
    double Elevator { set; get; }
    double Rudder { get; set; }
    double Throttle { get; set; }
    double Position { get; set; }
}