using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlightInspectionDesktopApp
{
    interface ITelnetClient
    {
        void Connect(int port);
        void Write(string command);
        string Read();
        void Disconnect();
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
            string hostname = "localhost";
            // Socket for FlightGear
            IPHostEntry host = Dns.GetHostEntry(hostname);
            IPEndPoint ipe = new IPEndPoint(host.AddressList[1], port);
            IPEndPoint ipeRead = new IPEndPoint(host.AddressList[1], 6400);
            try
            {
                socketWrite = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socketWrite.Connect(ipe);
                netSocketWrite = new NetworkStream(socketWrite);
                output = new StreamWriter(netSocketWrite);
                socketRead = new Socket(ipeRead.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                socketRead.Connect(ipeRead);
                netSocketRead = new NetworkStream(socketRead);
                reader = new StreamReader(netSocketRead);
            }
            // maybe indicate failure to the user?
            catch { }
        }



        public void Disconnect()
        {
            output.Close();
            netSocketWrite.Close();
            socketWrite.Close();
            reader.Close();
            netSocketRead.Close();
            socketRead.Close();
        }

        public string Read()
        {
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
