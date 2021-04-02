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
namespace FlightInspectionDesktopApp.FGModel
{
    class FGModelImp : IFGModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ITelnetClient telnetClient;
        volatile Boolean shouldStop;

        public int PlayingSpeed { get; set; } = 100;

        //CTOR
        public FGModelImp(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            this.shouldStop = false;
        }

        public void RunFG(string binFolder, string PathFG, string XMLFileName)
        {
            // Create a new ProcessInfo
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = false;
            // Change working directory and choose to run the exe file
            startInfo.WorkingDirectory = binFolder;
            startInfo.FileName = PathFG;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            // Send the simulator settings as command arguments
            startInfo.Arguments = "--generic=socket,in,10,127.0.0.1,5400,tcp," + XMLFileName + " --telnet=socket,in,10,127.0.0.1,6400,tcp --fdm=null";
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;
            try
            {
                // Run the process by the ProcessInfo
                using (Process exeProcess = Process.Start(startInfo)) { }
            }
            catch
            {
            }
        }

        public void Connect(int port)
        {
            this.telnetClient.Connect(port);
            string response;
            int first, last;
            do
            {
                Send(Encoding.ASCII.GetBytes("get /sim/sceneryloaded\r\n"));
                response = telnetClient.Read();
                first = response.IndexOf("'");
                last = response.LastIndexOf("'");
                Thread.Sleep(1000);
            }
            while (response.Substring(first + 1, last - first - 1).Equals("false"));
        }

        public void Send(byte[] get)
        {
            this.telnetClient.Send(get);
        }

        public void Disconnect()
        {
            shouldStop = true;
            this.telnetClient.Disconnect();
        }

        public void Start(string PathCSV)
        {
            new Thread(delegate ()
            {
                StreamReader src = new StreamReader(PathCSV);
                string currentLine;
                // currentLine will be null when the StreamReader reaches the end of file
                while (!shouldStop)
                {
                    if ((currentLine = src.ReadLine()) == null)
                    {
                        Disconnect();
                    }
                    this.telnetClient.Write(currentLine);

                    // 10 Hz:
                    Thread.Sleep(PlayingSpeed);
                }
            }).Start();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        private float aileron;
        public float Aileron
        {
            get
            {
                return aileron;
            }
            set
            {
                aileron = value;
                NotifyPropertyChanged("Aileron");
            }
        }
        public float Elevator { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Rudder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Throttle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Altitude { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float AirSpeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Heading { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Pitch { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Roll { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float SideSlip { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}