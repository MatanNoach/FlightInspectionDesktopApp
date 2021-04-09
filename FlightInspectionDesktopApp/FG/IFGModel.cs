using System.ComponentModel;

public interface IFGModel : INotifyPropertyChanged
{
    void Connect();
    void Disconnect();
    void Start(string PathCSV);
    void RunFG(string binFolder, string PathFG, string XMLFileName);
}