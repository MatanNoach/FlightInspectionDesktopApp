using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;

namespace FlightInspectionDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // The view model for the simulator
        FGViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// The function opens the file explorer window for the FlightGear executable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadFG_Click(object sender, RoutedEventArgs e)
        {
            // Asks the user to upload an exe file
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Application (*.exe)|*.exe";
            // Update the path in the TextBox
            if (openFile.ShowDialog() == true)
            {
                PathFG.Text = Path.GetFullPath(openFile.FileName);
            }
        }
        /// <summary>
        /// The function opens the file explorer window for the XML document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// The function opens the file explorer window for the CSV file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// The function verifies the user input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            // if all the data is valid
            if (isValid)
            {
                // get the binFolder, actual xml file location, xml file name and the real location the xml file needs to be placed
                String binFolder = Directory.GetParent(PathFG.Text).ToString();
                String actualXML = Directory.GetParent(PathXML.Text).ToString();
                String XMLFileName = Path.GetFileNameWithoutExtension(PathXML.Text);
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
                    // run the view model
                    DataModel.CreateModel(PathCSV.Text, PathXML.Text);
                    FGModelImp.CreateModel(new TelnetClient());
                    // create a new view model, with flight gear model and telnet client
                    vm = new FGViewModel(FGModelImp.Instance);
                    InspectorWindow inspector = new InspectorWindow();
                    inspector.Show();
                    Close();
                    vm.Run(binFolder, PathFG.Text, XMLFileName, PathCSV.Text);
                }
            }
        }
        /// <summary>
        /// The function checks if the user uploaded a FlightGear executable, and hides an error message if exists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PathFG_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If the user uploaded an exe file, hide the error message
            if (PathFG.Text.Length > 0)
            {
                ErrorFG.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// The function checks if the user uploaded an XML document, and hides an error message if exists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PathXML_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If the user uploaded an XML file, hide the error message
            if (PathXML.Text.Length > 0)
            {
                ErrorXML.Visibility = Visibility.Hidden;
            }
        }
        /// <summary>
        /// The function checks if the user uploaded a CSV file, and hides an error message if exists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
