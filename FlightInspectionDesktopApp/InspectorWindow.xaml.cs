using System.Windows;

namespace FlightInspectionDesktopApp
{
    /// <summary>
    /// Interaction logic for InspectorWindow.xaml
    /// </summary>
    public partial class InspectorWindow : Window
    {
        private FGViewModel vm;
        public InspectorWindow(FGViewModel vm)
        {
            InitializeComponent();
            this.vm = vm;
            // set the user's main screen size, in order to use it while displaying FG and InspectorWindow
            Properties.Settings.Default.windowWidth = (int)(SystemParameters.PrimaryScreenWidth) / 2;
            Properties.Settings.Default.windowHeight = (int)(SystemParameters.PrimaryScreenHeight);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            vm.Disconnect();
        }
    }
}
