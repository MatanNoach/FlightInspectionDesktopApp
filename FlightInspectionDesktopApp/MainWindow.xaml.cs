using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace FlightInspectionDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FGViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new FGViewModel(new FGModel.FGModelImp(new TelnetClient()));
            DataContext = vm;
        }
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
            if (isValid)
            {
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
                    vm.Run(binFolder, PathFG.Text, XMLFileName, PathCSV.Text);
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.Disconnect();
        }
    }
}
