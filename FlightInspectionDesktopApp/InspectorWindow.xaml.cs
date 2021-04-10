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
        private FGViewModel vm;
        private string dllPath;
        private string csvFilePath;
        private UserControls.Graph graphs;

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
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetColumn(graphs, 1);
            Grid.SetRow(graphs, 1);
            Grid.SetColumnSpan(graphs, 2);
            Grid.SetRowSpan(graphs, 2);
            UserControls.Children.Add(graphs);
            // set the user's main screen size, in order to use it while displaying FG and InspectorWindow
            Properties.Settings.Default.windowWidth = (int)(SystemParameters.PrimaryScreenWidth) / 2;
            Properties.Settings.Default.windowHeight = (int)(SystemParameters.PrimaryScreenHeight);
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vm.Disconnect();
        }
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
