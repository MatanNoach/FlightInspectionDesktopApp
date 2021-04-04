﻿using System;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;

namespace FlightInspectionDesktopApp
{/// <summary>
/// Implementation of the main model.
/// </summary>
    class FGModelImp : IFGModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        ITelnetClient telnetClient;
        volatile bool shouldStop;
        // set the default playing speed to 10 Hz
        public int PlayingSpeed { get; set; } = 100;

        /// <summary>
        /// FGModelImp constructor.
        /// </summary>
        /// <param name="telnetClient">telnet client to initialize</param>
        public FGModelImp(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
            this.shouldStop = false;
        }

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
            startInfo.CreateNoWindow = true;
            try
            {
                // Run the process by the ProcessInfo
                using (Process exeProcess = Process.Start(startInfo)) { }
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
        }

        /// <summary>
        /// Runs the flight on Flight Gear based on the given CSV file.
        /// </summary>
        /// <param name="PathCSV">path of the CSV file with the flight data</param>
        public void Start(string PathCSV)
        {
            new Thread(delegate ()
            {
                DataModel model = DataModel.Instance;
                Metadata.MetadataModel metaModel = Metadata.MetadataModel.Instance;
                Steering.SteeringModel steeringModel = Steering.SteeringModel.Instance;

                while (!shouldStop)
                {
                    // currentLine will be null when the StreamReader reaches the end of file
                    if (model.CurrentLineIndex > model.getDataSize())
                    {
                        //? not the final logic
                        Disconnect();
                    }
                    // send a line from the CSV to FG
                    this.telnetClient.Write(model.getLineByIndex(model.CurrentLineIndex));
                    // store each value from the current line in MetadataModel properties 
                    metaModel.Altitude = model.getValueByKeyAndTime("altimeter_indicated-altitude-ft", model.CurrentLineIndex);
                    metaModel.AirSpeed = model.getValueByKeyAndTime("airspeed-kt", model.CurrentLineIndex);
                    metaModel.Heading = model.getValueByKeyAndTime("heading-deg", model.CurrentLineIndex);
                    metaModel.Pitch = model.getValueByKeyAndTime("pitch-deg", model.CurrentLineIndex);
                    metaModel.Roll = model.getValueByKeyAndTime("roll-deg", model.CurrentLineIndex);
                    metaModel.SideSlip = model.getValueByKeyAndTime("side-slip-deg", model.CurrentLineIndex);
                    // store each value from the current line in SteeringModel properties 
                    steeringModel.Throttle = model.getValueByKeyAndTime("throttle_0", model.CurrentLineIndex);
                    steeringModel.Rudder = model.getValueByKeyAndTime("rudder", model.CurrentLineIndex);
                    steeringModel.Elevator = model.getValueByKeyAndTime("elevator", model.CurrentLineIndex);
                    steeringModel.Aileron = model.getValueByKeyAndTime("aileron_0", model.CurrentLineIndex);
                    model.CurrentLineIndex++;
                    // play in 10 Hz:
                    Thread.Sleep(PlayingSpeed);
                }
            }).Start();
        }

        //? All from here and under should be handled in other components
        /// <summary>
        /// Evokes all subscribed methods of PropertyChanged.
        /// </summary>
        /// <param name="propName">name of the property that's been changed</param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        private double aileron;
        private double elevator;
        private double rudder;
        private double throttle;
        private double position;

        public double Aileron
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
        public double Elevator
        {
            get
            {
                return elevator;
            }
            set
            {
                elevator = value;
                NotifyPropertyChanged("Elevator");
            }
        }
        public double Rudder
        {
            get
            {
                return rudder;
            }
            set
            {
                rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }
        public double Throttle
        {
            get
            {
                return throttle;
            }
            set
            {
                throttle = value;
                NotifyPropertyChanged("Throttle");
            }
        }
        public double Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                NotifyPropertyChanged("Position");
            }
        }

    }
}