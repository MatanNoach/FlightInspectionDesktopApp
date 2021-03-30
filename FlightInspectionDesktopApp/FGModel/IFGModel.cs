using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
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