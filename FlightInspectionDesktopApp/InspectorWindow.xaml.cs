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
            string csvPath = ((MainWindow)Application.Current.MainWindow).PathCSV.Text;
            string xmlPath = ((MainWindow)Application.Current.MainWindow).PathXML.Text;
            DataModel.CreateModel(csvPath, xmlPath);
        }
    }
}
