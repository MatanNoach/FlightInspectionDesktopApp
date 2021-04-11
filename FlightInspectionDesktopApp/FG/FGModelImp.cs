using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace FlightInspectionDesktopApp
{
    /// <summary>
    /// Implementation of the main model.
    /// </summary>
    class FGModelImp : IFGModel
    {
        // fields of FGModelImp object
        Process exeProcess;
        volatile bool shouldStop;
        ITelnetClient telnetClient;
        private DataModel dataModel;
        private static FGModelImp fgModel;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// a static property of field fgModel.
        /// </summary>
        public static FGModelImp Instance
        {
            // getter of fgModel.
            get
            {
                if (fgModel == null)
                {
                    throw new Exception("FGModel was not created");
                }
                return fgModel;
            }
        }

        /// <summary>
        /// a statuc functoin that creates an object of FGModelImp
        /// </summary>
        /// <param name="telnetClient">telnet client to initialize</param>
        public static void CreateModel(ITelnetClient telnetClient)
        {
            if (fgModel != null)
            {
                throw new Exception("FGModel was created");
            }
            fgModel = new FGModelImp(telnetClient);
        }

        /// <summary>
        /// FGModelImp constructor.
        /// </summary>
        /// <param name="telnetClient">telnet client to initialize</param>
        private FGModelImp(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            this.shouldStop = false;
            dataModel = DataModel.Instance;
        }

        /// <summary>
        /// import SetWindowsPos function from user32 API, in order to set the window's size of FG
        /// </summary>
        /// <param name="hWnd"> the main window handler of the process </param>
        /// <param name="hWndInsertAfter"> always 0 </param>
        /// <param name="x"> the starting point from the left of the screen </param>
        /// <param name="Y"> the starting point from the top side of the screen </param>
        /// <param name="cx"> the new width of the process's window </param>
        /// <param name="cy"> the new height of the process's window </param>
        /// <param name="wFlags"> flags for the api function </param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        public static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);

        /// <summary>
        /// Starts up FlightGear Simulator.
        /// </summary>
        /// <param name="binFolder">location of fgfs.exe file</param>
        /// <param name="PathFG">full path of fgfs.exe file</param>
        /// <param name="XMLFileName">settings file's name</param>
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
            // set the generic flag
            string[] genericArgs = {
                Properties.Settings.Default.cmdGeneric,
                Properties.Settings.Default.hostIP,
                Properties.Settings.Default.portGeneric.ToString(),
                Properties.Settings.Default.protocolType,
                XMLFileName
            };
            string genericCMD = string.Join(",", genericArgs);

            // set the telnet flag
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
                //Properties.Settings.Default.httpd
            };

            startInfo.Arguments = string.Join(" ", processArgs);
            startInfo.RedirectStandardOutput = true;

            try
            {
                // Run the process by the ProcessInfo
                this.exeProcess = Process.Start(startInfo);

                // set the process's window at the left side of the screen
                Thread.Sleep(5000);
                IntPtr handle = exeProcess.MainWindowHandle;
                const short SWP_NOZORDER = 0X4;
                const int SWP_SHOWWINDOW = 0x0040;

                if (handle != IntPtr.Zero)
                {
                    SetWindowPos(handle, 0, 0, 0, (int)(Properties.Settings.Default.windowWidth * 1.25), (int)(Properties.Settings.Default.windowHeight * 1.1), SWP_NOZORDER | SWP_SHOWWINDOW);
                }
            }
            catch (Exception e)
            {
                // print the exception to the user
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Connects the program to FlightGear Simulator.
        /// </summary>
        public void Connect()
        {
            // Connect the telnet client to FG
            this.telnetClient.Connect();
            string response;
            int first, last;
            do
            {
                // Sent a get request to determine whether the simulator is ready
                Send(Encoding.ASCII.GetBytes("get /sim/sceneryloaded\r\n"));
                // read the response and parse it
                response = telnetClient.Read();
                first = response.IndexOf("'");
                last = response.LastIndexOf("'");
                Thread.Sleep(1000);
            }
            // Repeat as long as the prop is "false"
            while (response.Substring(first + 1, last - first - 1).Equals("false"));
        }

        /// <summary>
        /// Sends a telnet request through the tenet client.
        /// </summary>
        /// <param name="getRequest">get request to send</param>
        public void Send(byte[] getRequest)
        {
            this.telnetClient.Send(getRequest);
        }

        /// <summary>
        /// Disconnects all telnet client connections.
        /// </summary>
        public void Disconnect()
        {
            shouldStop = true;
            this.telnetClient.Disconnect();
            this.exeProcess.Kill();
        }

        /// <summary>
        /// Runs the flight on Flight Gear based on the given CSV file.
        /// </summary>
        /// <param name="PathCSV">path of the CSV file with the flight data</param>
        public void Start(string PathCSV)
        {
            new Thread(delegate ()
            {
                Metadata.MetadataModel metaModel = Metadata.MetadataModel.Instance;
                Steering.SteeringModel steeringModel = Steering.SteeringModel.Instance;
                Player.PlayerModel playerModel = Player.PlayerModel.Instance;
                Speedometer.SpeedometerModel speedometerModel = Speedometer.SpeedometerModel.Instance;
                Altimeter.AltimeterModel altimeterModel = Altimeter.AltimeterModel.Instance;

                while (!shouldStop)
                {
                    // send a line from the CSV to FG
                    this.telnetClient.Write(dataModel.getLineByIndex(dataModel.CurrentLineIndex));
                    // store each value from the current line in MetadataModel properties 
                    metaModel.Altitude = dataModel.getValueByKeyAndTime(Properties.Settings.Default.altimeter, dataModel.CurrentLineIndex);
                    metaModel.AirSpeed = dataModel.getValueByKeyAndTime(Properties.Settings.Default.airspeed, dataModel.CurrentLineIndex);
                    metaModel.Heading = dataModel.getValueByKeyAndTime("heading-deg", dataModel.CurrentLineIndex);
                    metaModel.Pitch = dataModel.getValueByKeyAndTime("pitch-deg", dataModel.CurrentLineIndex);
                    metaModel.Roll = dataModel.getValueByKeyAndTime("roll-deg", dataModel.CurrentLineIndex);
                    metaModel.SideSlip = dataModel.getValueByKeyAndTime("side-slip-deg", dataModel.CurrentLineIndex);
                    // store each value from the current line in SteeringModel properties 
                    steeringModel.Throttle = dataModel.getValueByKeyAndTime("throttle_0", dataModel.CurrentLineIndex);
                    steeringModel.Rudder = dataModel.getValueByKeyAndTime("rudder", dataModel.CurrentLineIndex);
                    steeringModel.Elevator = dataModel.getValueByKeyAndTime("elevator", dataModel.CurrentLineIndex);
                    steeringModel.Aileron = dataModel.getValueByKeyAndTime("aileron_0", dataModel.CurrentLineIndex);

                    speedometerModel.AirSpeed = dataModel.getValueByKeyAndTime(Properties.Settings.Default.airspeed, dataModel.CurrentLineIndex);
                    speedometerModel.SpeedometerAngle = speedometerModel.calculateSpeedometerAngle(speedometerModel.AirSpeed);

                    altimeterModel.Altimeter = dataModel.getValueByKeyAndTime(Properties.Settings.Default.altimeter, dataModel.CurrentLineIndex);

                    // read another line from the data model
                    playerModel.DataModel.moveNextLine();
                    // play in 10 Hz:
                    Thread.Sleep(playerModel.PlayingSpeed);
                }
            }).Start();
        }

        ////? All from here and under should be handled in other components
        ///// <summary>
        ///// Evokes all subscribed methods of PropertyChanged.
        ///// </summary>
        ///// <param name="propName">name of the property that's been changed</param>
        //public void NotifyPropertyChanged(string propName)
        //{
        //    if (this.PropertyChanged != null)
        //    {
        //        this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        //    }
        //}

        //private double aileron;
        //private double elevator;
        //private double rudder;
        //private double throttle;
        //private double position;

        //public double Aileron
        //{
        //    get
        //    {
        //        return aileron;
        //    }
        //    set
        //    {
        //        aileron = value;
        //        NotifyPropertyChanged("Aileron");
        //    }
        //}
        //public double Elevator
        //{
        //    get
        //    {
        //        return elevator;
        //    }
        //    set
        //    {
        //        elevator = value;
        //        NotifyPropertyChanged("Elevator");
        //    }
        //}
        //public double Rudder
        //{
        //    get
        //    {
        //        return rudder;
        //    }
        //    set
        //    {
        //        rudder = value;
        //        NotifyPropertyChanged("Rudder");
        //    }
        //}
        //public double Throttle
        //{
        //    get
        //    {
        //        return throttle;
        //    }
        //    set
        //    {
        //        throttle = value;
        //        NotifyPropertyChanged("Throttle");
        //    }
        //}
        //public double Position
        //{
        //    get
        //    {
        //        return position;
        //    }
        //    set
        //    {
        //        position = value;
        //        NotifyPropertyChanged("Position");
        //    }
        //}
        //public DataModel DataModel
        //{
        //    get
        //    {
        //        return dataModel;
        //    }
        //}

    }
}
