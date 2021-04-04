using System.Windows;

namespace FlightInspectionDesktopApp
{
    /// <summary>
    /// Interaction logic for InspectorWindow.xaml
    /// </summary>
    public partial class InspectorWindow : Window
    {
        public InspectorWindow()
        {
            InitializeComponent();

            // set the user's main screen size, in order to use it while displaying FG and InspectorWindow
            Properties.Settings.Default.windowWidth = (int)(SystemParameters.PrimaryScreenWidth) / 2;
            Properties.Settings.Default.windowHeight = (int)(SystemParameters.PrimaryScreenHeight);
        }
    }
}
