using System.IO;
using System.Windows;
using Microsoft.Win32;
using System.Diagnostics;
using System.ComponentModel;
using System.Windows.Controls;

namespace FlightInspectionDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // fields of MainWindow
        FGViewModel vm;
        BackgroundWorker bg;
        LoadingWindow loadingWindow;
        private static string fileFG, fileXML, fileCSV, fileDLL;

        /// <summary>
        /// CTOR of MainWindow.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //set the user's main screen size, in order to use it while displaying FG and InspectorWindow            
            Properties.Settings.Default.windowWidth = (int)(SystemParameters.PrimaryScreenWidth);
            Properties.Settings.Default.windowHeight = (int)(SystemParameters.PrimaryScreenHeight);

            Properties.Settings.Default.flightGearWindowWidth = Properties.Settings.Default.windowWidth / 4;
            Properties.Settings.Default.flightGearWindowHeight = Properties.Settings.Default.windowHeight / 2;

            Properties.Settings.Default.InspectorWindowWidth = Properties.Settings.Default.windowWidth * 4 / 5;
            Properties.Settings.Default.InspectorWindowHeight = Properties.Settings.Default.windowHeight - 30;

            // init backgroundWorker object in order to show loading window
            bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg_RunWorkerCompleted);
        }

        /// <summary>
        /// this function starts flight gear, while the loading window is shown.
        /// </summary>
        /// <param name="sender"> the sender object </param>
        /// <param name="e"> the event args </param>
        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            // run the view model
            string binFolder = Directory.GetParent(fileFG).ToString();
            string XMLFileName = System.IO.Path.GetFileNameWithoutExtension(fileXML);

            DataModel.CreateModel(fileCSV, fileXML);
            FGModelImp.CreateModel(new TelnetClient());
            FGModelImp model = FGModelImp.Instance;

            vm = new FGViewModel(model);
            vm.Run(binFolder, fileFG, XMLFileName);
        }

        /// <summary>
        /// this function starts InspectorWindow and closes loadingWindow, after flight gear fully started.
        /// </summary>
        /// <param name="sender"> the sender object </param>
        /// <param name="e"> the event args </param>
        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // create a new view model, with flight gear model and telnet client
            FGModelImp model = FGModelImp.Instance;

            InspectorWindow inspector = new InspectorWindow(vm, fileCSV, fileDLL);
            inspector.Show();
            loadingWindow.Close();
            vm.Start(fileCSV);
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
                fileFG = PathFG.Text;
            }
        }

        /// <summary>
        /// The function opens the file explorer window for the XML document
        /// </summary>
        /// <param name="sender"> the sender object </param>
        /// <param name="e"> the event args </param>
        private void LoadXML_Click(object sender, RoutedEventArgs e)
        {
            // Asks the user to upload an XML document
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "XML Document (*.xml)|*.xml";
            // Update the path in the TextBox
            if (openFile.ShowDialog() == true)
            {
                PathXML.Text = System.IO.Path.GetFullPath(openFile.FileName);
                fileXML = PathXML.Text;
            }
        }

        /// <summary>
        /// The function opens the file explorer window for the CSV file
        /// </summary>
        /// <param name="sender"> the sender object </param>
        /// <param name="e"> the event args </param>
        private void LoadCSV_Click(object sender, RoutedEventArgs e)
        {
            // Asks the user to upload a CSV file
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Microsoft Excel Comma Seperated Values File (*.csv)|*.csv";
            // Update the path in the TextBox
            if (openFile.ShowDialog() == true)
            {
                PathCSV.Text = System.IO.Path.GetFullPath(openFile.FileName);
                fileCSV = PathCSV.Text;
            }
        }

        /// <summary>
        /// this function loads the .dll after clicking the related button.
        /// </summary>
        /// <param name="sender"> the sender object </param>
        /// <param name="e"> the event args </param>
        private void LoadDLL_Click(object sender, RoutedEventArgs e)
        {
            // Asks the user to upload a CSV DLL file
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Dynamic Linking Library File (*.dll)|*.dll";
            // Update the path in the TextBox
            if (openFile.ShowDialog() == true)
            {
                PathDLL.Text = System.IO.Path.GetFullPath(openFile.FileName);
                fileDLL = PathDLL.Text;
            }
        }

        /// <summary>
        /// validate that all the TextBox fields have data.
        /// </summary>
        /// <returns> True if does, and False if not </returns>
        private bool validateTextboxes()
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
            if (PathDLL.Text.Length == 0)
            {
                ErrorDLL.Visibility = Visibility.Visible;
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// validate that:
        ///     1. the xml file is in the right directory.
        ///     2. the csv file is not open.
        ///     3. there is no open window of FlightGear.
        /// </summary>
        /// <returns> True if does, False if not </returns>
        private bool validateFiles()
        {
            bool isValid = true;

            // get the binFolder, actual xml file location, xml file name and the real location the xml file needs to be placed
            string binFolder = Directory.GetParent(PathFG.Text).ToString();
            string actualXML = Directory.GetParent(PathXML.Text).ToString();
            string targetXML = Directory.GetParent(binFolder).ToString() + "\\data\\Protocol";
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

            // validate that the csv file is not open
            try
            {
                using (FileStream stream = File.Open(PathCSV.Text, FileMode.Open))
                {
                    stream.Close();
                    ErrorCSV.Visibility = Visibility.Hidden;
                }
            }
            catch (IOException)
            {
                ErrorCSV.Text = "Please close the file before inspecting it";
                ErrorCSV.Visibility = Visibility.Visible;
                isValid = false;
            }

            // check that flight gear is not open
            string fgExeName = Path.GetFileNameWithoutExtension(PathFG.Text);
            if (Process.GetProcessesByName(fgExeName).Length > 0)
            {
                ErrorOpenFG.Visibility = Visibility.Visible;
                isValid = false;
            }
            else
            {
                ErrorOpenFG.Visibility = Visibility.Hidden;
            }

            return isValid;
        }

        /// <summary>
        /// The function checks if the user uploaded a FlightGear executable, and hides an error message if exists.
        /// </summary>
        /// <param name="sender"> the sender object </param>
        /// <param name="e"> the event args </param>
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
        /// <param name="sender"> the sender object </param>
        /// <param name="e"> the event args </param>
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
        /// <param name="sender"> the sender object </param>
        /// <param name="e"> the event args </param>
        private void PathCSV_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If the user uploaded an CSV file, hide the error message
            if (PathCSV.Text.Length > 0)
            {
                ErrorCSV.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// this function hides the error message after a .dll file was loaded.
        /// </summary>
        /// <param name="sender"> the sender object </param>
        /// <param name="e"> the event args </param>
        private void PathDLL_TextChanged(object sender, TextChangedEventArgs e)
        {
            // If the user uploaded an DLL file, hide the error message
            if (PathDLL.Text.Length > 0)
            {
                ErrorDLL.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// The function verifies the user input, and starts the loading window.
        /// </summary>
        /// <param name="sender"> the sender object </param>
        /// <param name="e"> the event args </param>
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (validateTextboxes() && validateFiles())
            {
                this.loadingWindow = new LoadingWindow();
                this.loadingWindow.Show();
                bg.RunWorkerAsync();
                Close();
            }
        }
    }
}
