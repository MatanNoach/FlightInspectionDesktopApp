using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FlightInspectionDesktopApp
{
    interface ITelnetClient
    {
        void Connect();
        void Write(string command);
        string Read();
        void Disconnect();
        void Send(byte[] get);
    }

    /// <summary>
    /// Implementation of a telnet client.
    /// </summary>
    class TelnetClient : ITelnetClient
    {
        // Vars of writing flight data to FG
        StreamWriter writer;
        Socket socketData;
        NetworkStream netSocketWrite;
        // Vars of sending telnet requests and reading the response
        StreamReader reader;
        Socket socketRequests;
        NetworkStream netSocketRead;


        /// <summary>
        /// Connects to FG with the designated ports.
        /// </summary>
        public void Connect()
        {
            string hostname = Properties.Settings.Default.hostName;
            IPHostEntry host = Dns.GetHostEntry(hostname);
            IPEndPoint ipeGen = new IPEndPoint(host.AddressList[1], Properties.Settings.Default.portGeneric);
            IPEndPoint ipeTelnet = new IPEndPoint(host.AddressList[1], Properties.Settings.Default.portTelnet);
            try
            {
                // create a socket for sending flight data to FG and connect to it
                socketData = new Socket(ipeGen.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socketData.Connect(ipeGen);
                // create a stream writer to write the flight data
                netSocketWrite = new NetworkStream(socketData);
                writer = new StreamWriter(netSocketWrite);
                // create a socket for sending and receiving telnet requests and responses
                socketRequests = new Socket(ipeTelnet.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                // make sure that the telnet connection is estblished before continuing
                do
                {
                    socketRequests.Connect(ipeTelnet);
                }
                while (!socketRequests.Connected);
                // create a stream reader to read the telnet responses
                netSocketRead = new NetworkStream(socketRequests);
                reader = new StreamReader(netSocketRead);
            }
            catch (Exception e)
            {
                // indicate in case of a failure in connection
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Sends telnet requests to FG.
        /// </summary>
        /// <param name="getRequest">get request</param>
        public void Send(byte[] getRequest)
        {
            socketRequests.Send(getRequest);
        }

        /// <summary>
        /// Closes all open connections.
        /// </summary>
        public void Disconnect()
        {
            writer.Close();
            netSocketWrite.Close();
            socketData.Close();
            socketRequests.Close();
            netSocketRead.Close();
            reader.Close();
        }

        /// <summary>
        /// Reads telnet responses from FG.
        /// </summary>
        /// <returns>telnet response</returns>
        public string Read()
        {
            return reader.ReadLine();
        }

        /// <summary>
        /// Writes flight data to FG.
        /// </summary>
        /// <param name="data">line from CSV file</param>
        public void Write(string data)
        {
            writer.WriteLine(data);
        }

    }
}
