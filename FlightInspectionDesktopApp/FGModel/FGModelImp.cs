using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
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

            string[] genericArgs = { 
                Properties.Settings.Default.cmdGeneric, 
                Properties.Settings.Default.hostIP, 
                Properties.Settings.Default.portGeneric.ToString(), 
                Properties.Settings.Default.protocolType, 
                XMLFileName 
            };
            string genericCMD = string.Join(",", genericArgs);

            string[] telnetArgs = {
                Properties.Settings.Default.cmdTelnet,
                Properties.Settings.Default.hostIP,
                Properties.Settings.Default.portTelnet.ToString(),
                Properties.Settings.Default.protocolType
            };
            string telnetCMD = string.Join(",", telnetArgs);

            string[] processArgs =
            {
                genericCMD,
                telnetCMD,
                Properties.Settings.Default.cmdFDM
            };

            startInfo.Arguments = string.Join(" ", processArgs);
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