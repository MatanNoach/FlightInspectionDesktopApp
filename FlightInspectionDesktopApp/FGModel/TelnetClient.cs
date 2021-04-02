using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FlightInspectionDesktopApp
{
    interface ITelnetClient
    {
        void Connect(int port);
        void Write(string command);
        string Read();
        void Disconnect();
        void Send(byte[] get);
    }

    class TelnetClient : ITelnetClient
    {
        StreamWriter output;
        Socket socketWrite;
        NetworkStream netSocketWrite;
        StreamReader reader;
        Socket socketRead;
        NetworkStream netSocketRead;



        public void Connect(int port)
        {
            string hostname = Properties.Settings.Default.hostName;
            // Socket for FlightGear
            IPHostEntry host = Dns.GetHostEntry(hostname);
            IPEndPoint ipe = new IPEndPoint(host.AddressList[1], port);
            IPEndPoint ipeRead = new IPEndPoint(host.AddressList[1], Properties.Settings.Default.portTelnet);
            try
            {
                socketWrite = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socketWrite.Connect(ipe);
                netSocketWrite = new NetworkStream(socketWrite);
                output = new StreamWriter(netSocketWrite);
                socketRead = new Socket(ipeRead.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                while (!socketRead.Connected)
                {
                    socketRead.Connect(ipeRead);
                }
                netSocketRead = new NetworkStream(socketRead);
                reader = new StreamReader(netSocketRead);
            }
            //maybe indicate failure to the user?
            catch { }
        }


        public void Send(byte[] get)
        {
            socketRead.Send(get);
        }

        public void Disconnect()
        {
            output.Close();
            netSocketWrite.Close();
            socketWrite.Close();
            reader.Close();
            //add more
        }

        public string Read()
        {
            //return "";
            string line = reader.ReadLine();
            Console.WriteLine(line);
            return line;
        }

        public void Write(string command)
        {
            output.WriteLine(command);
        }

    }
}
