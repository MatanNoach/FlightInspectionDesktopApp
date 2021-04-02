using System.Windows.Controls;

namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Metadata.xaml
    /// </summary>
    public partial class Metadata : UserControl
    {
        public Metadata()
        {
            InitializeComponent();
            DataModel model = DataModel.Instance;

            dataAltitude.Text = model.getValueByKeyAndTime("altitude-ft", 240).ToString();
            dataAirSpeed.Text = model.getValueByKeyAndTime("airspeed-kt", 240).ToString();
            dataHeading.Text = model.getValueByKeyAndTime("heading-deg", 240).ToString();
            dataPitch.Text = model.getValueByKeyAndTime("pitch-deg", 240).ToString();
            dataRoll.Text = model.getValueByKeyAndTime("roll-deg", 240).ToString();
            dataSideSlip.Text = model.getValueByKeyAndTime("side-slip-deg", 240).ToString();
        }
    }
}
