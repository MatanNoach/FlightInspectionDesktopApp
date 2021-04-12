using FlightInspectionDesktopApp.Metadata;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace FlightInspectionDesktopApp
{
    /// <summary>
    /// Interaction logic for InspectorWindow.xaml
    /// </summary>
    public partial class InspectorWindow : Window
    {
        // fields of InspectorWindow.
        private FGViewModel vm;
        private string dllPath;
        private string csvFilePath;
        private UserControls.Graph graphs;

        /// <summary>
        /// CTOR of InspectorWindow
        /// </summary>
        /// <param name="vm"> model of FlightGear </param>
        /// <param name="csvFilePath"> the csv file to load </param>
        /// <param name="dllPath"> a .dll file which detects anomalies </param>
        public InspectorWindow(FGViewModel vm, string csvFilePath, string dllPath)
        {
            InitializeComponent();
            this.vm = vm;
            this.dllPath = dllPath;
            this.csvFilePath = csvFilePath;
            // add a new graphs user control
            graphs = new UserControls.Graph(csvFilePath, this.dllPath)
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            Grid.SetColumn(graphs, 0);
            Grid.SetRow(graphs, 1);
            Grid.SetColumnSpan(graphs, 2);
            Grid.SetRowSpan(graphs, 2);
            UserControls.Children.Add(graphs);
            // set the user's main screen size, in order to use it while displaying FG and InspectorWindow
            Properties.Settings.Default.windowWidth = (int)(SystemParameters.PrimaryScreenWidth);
            Properties.Settings.Default.windowHeight = (int)(SystemParameters.PrimaryScreenHeight);

            Properties.Settings.Default.flightGearWindowWidth = Properties.Settings.Default.windowWidth / 4;
            Properties.Settings.Default.flightGearWindowHeight = Properties.Settings.Default.windowHeight / 2;

            Properties.Settings.Default.InspectorWindowWidth = Properties.Settings.Default.windowWidth * 3 / 4;
            Properties.Settings.Default.InspectorWindowHeight = Properties.Settings.Default.windowHeight;

        }

        /// <summary>
        /// this function disconnects the view-model object when closing the window.
        /// </summary>
        /// <param name="sender"> the sender object </param>
        /// <param name="e"> the event args </param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vm.Disconnect();
        }

        /// <summary>
        /// this function loads the .dll after clicking the related button.
        /// </summary>
        /// <param name="sender"> the sender object </param>
        /// <param name="e"> the event args </param>
        private void LoadDLL_Click(object sender, RoutedEventArgs e)
        {
            // Asks the user to upload a CSV DLL file
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = "Dynamic Linking Library File (*.dll)|*.dll"
            };
            // Update the path in the TextBox
            if (openFile.ShowDialog() == true)
            {
                this.dllPath = System.IO.Path.GetFullPath(openFile.FileName);
            }
            UserControls.Children.Remove(graphs);
            graphs = new UserControls.Graph(csvFilePath, this.dllPath)
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetColumn(graphs, 1);
            Grid.SetRow(graphs, 1);
            Grid.SetColumnSpan(graphs, 2);
            Grid.SetRowSpan(graphs, 2);
            UserControls.Children.Add(graphs);
        }
    }
}
