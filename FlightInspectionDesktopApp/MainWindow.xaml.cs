﻿using System;
using System.Collections.Generic;
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


namespace FlightInspectionDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {            
            InitializeComponent();

            // input from user
            int playingSpeed = 100;            
            string csv_file = "C:\\Users\\user\\Documents\\University\\תשפא\\סמסטר ב\\תכנות מתקדם 2\\תרגילים\\Milestone1\\reg_flight.csv";

            // connection string
            int port = 5400;
            string hostname = "localhost";

            // Socket for FlightGear
            IPHostEntry host = Dns.GetHostEntry(hostname);
            IPEndPoint ipe = new IPEndPoint(host.AddressList[1],port);
            using (Socket socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(ipe);
                using (NetworkStream net_socket = new NetworkStream(socket))
                {
                    using (StreamWriter output = new StreamWriter(net_socket))
                    {
                        // Reading data from csv file            
                        using( StreamReader src = new StreamReader(csv_file))
                        {
                            string currentLine;
                            // currentLine will be null when the StreamReader reaches the end of file
                            while ((currentLine = src.ReadLine()) != null)
                            {                
                                output.WriteLine(currentLine);
                                Thread.Sleep(playingSpeed);
                            }
                        }
                    }
                }            
            }            
        }
    }
}
