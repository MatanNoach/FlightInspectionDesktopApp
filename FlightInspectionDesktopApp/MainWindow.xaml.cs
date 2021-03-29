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


        }        
        private void LoadFG_Click(object sender, RoutedEventArgs e)
        {
            // Asks the user to upload an exe file
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Application (*.exe)|*.exe";
            // Update the path in the TextBox
            if (openFile.ShowDialog() == true)
            {
                PathFG.Text = System.IO.Path.GetFullPath(openFile.FileName);
            }
        }

        private void LoadXML_Click(object sender, RoutedEventArgs e)
        {
            // Asks the user to upload an XML document
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "XML Document (*.xml)|*.xml";
            // Update the path in the TextBox
            if (openFile.ShowDialog() == true)
            {
                PathXML.Text = System.IO.Path.GetFullPath(openFile.FileName);

            }
        }

        private void LoadCSV_Click(object sender, RoutedEventArgs e)
        {
            // Asks the user to upload a CSV file
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Microsoft Excel Comma Seperated Values File (*.csv)|*.csv";
            // Update the path in the TextBox
            if (openFile.ShowDialog() == true)
            {
                PathCSV.Text = System.IO.Path.GetFullPath(openFile.FileName);

            }

        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            bool isValid = true;
            // Check if the user uploaded an exe file
            if (PathFG.Text.Length == 0)
            {
                ErrorFG.Visibility = Visibility.Visible;
                isValid = false;
            }
            // Check if the user uploaded an XML document
            if (PathXML.Text.Length == 0)
            {
                ErrorXML.Visibility = Visibility.Visible;
                isValid = false;
            }
            // Check if the user uploaded a CSV file
            if (PathCSV.Text.Length == 0)
            {
                ErrorCSV.Visibility = Visibility.Visible;
                isValid = false;
            }
            String binFolder = Directory.GetParent(PathFG.Text).ToString();
            String actualXML = Directory.GetParent(PathXML.Text).ToString();
            String XMLFileName = System.IO.Path.GetFileNameWithoutExtension(PathXML.Text);
            String targetXML = Directory.GetParent(binFolder).ToString() + "\\data\\Protocol";
            // Check if the XML file is in the right location
            if (!actualXML.Equals(targetXML))
            {
                // if not, ask him to move it
                ErrorXML.Text = "Please move XML file to " + targetXML;
                ErrorXML.Visibility = Visibility.Visible;
                isValid = false;

            }
            else
            {
                ErrorXML.Visibility = Visibility.Hidden;
            }
            if (isValid)
            {
                
                // Create a new ProcessInfo
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = false;
                // Change working directory and choose to run the exe file
                startInfo.WorkingDirectory = binFolder;
                startInfo.FileName = PathFG.Text;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                // Send the simulator settings as command arguments
                startInfo.Arguments = "--telnet=socket,in,10,127.0.0.1,5400,tcp --generic=socket,in,10,127.0.0.1,5400,tcp," +XMLFileName+ " --fdm=null";
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

                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                Thread.Sleep(60000);
                // input from user
                int playingSpeed = 100;

                // connection string
                int port = 5400;
                string hostname = "localhost";

                // Socket for FlightGear
                IPHostEntry host = Dns.GetHostEntry(hostname);
                IPEndPoint ipe = new IPEndPoint(host.AddressList[1], port);
                using (Socket socket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Connect(ipe);
                    using (NetworkStream net_socket = new NetworkStream(socket))
                    {
                        using (StreamWriter output = new StreamWriter(net_socket))
                        {
                            // Reading data from csv file            
                            using (StreamReader src = new StreamReader(PathCSV.Text))
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
        private void PathFG_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If the user uploaded an exe file, hide the error message
            if (PathFG.Text.Length > 0)
            {
                ErrorFG.Visibility = Visibility.Hidden;

            }
        }

        private void PathXML_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If the user uploaded an XML file, hide the error message
            if (PathXML.Text.Length > 0)
            {
                ErrorXML.Visibility = Visibility.Hidden;
            }
        }

        private void PathCSV_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If the user uploaded an CSV file, hide the error message
            if (PathCSV.Text.Length > 0)
            {
                ErrorCSV.Visibility = Visibility.Hidden;
            }

        }
    }
}
