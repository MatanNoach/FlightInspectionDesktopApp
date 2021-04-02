using System.Windows;

namespace FlightInspectionDesktopApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Start_App(object sender, StartupEventArgs e)
        {
            InspectorWindow inspector = new InspectorWindow();
            MainWindow main = new MainWindow();
            inspector.Show();
            main.Show();
        }
    }
}
