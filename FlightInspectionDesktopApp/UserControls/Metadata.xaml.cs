using System.Windows;
using System.Windows.Controls;
using FlightInspectionDesktopApp.Metadata;

namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Metadata.xaml
    /// </summary>
    public partial class Metadata : UserControl
    {
        MetadataViewModel vm;
        public Metadata()
        {
            InitializeComponent();
            vm = new MetadataViewModel(new MetadataModel());
            this.DataContext = vm;
            /*
            dataAltitude.Text = model.getValueByKeyAndTime("altitude-ft", model.CurrentLineIndex).ToString();
            dataAirSpeed.Text = model.getValueByKeyAndTime("airspeed-kt", model.CurrentLineIndex).ToString();
            dataHeading.Text = model.getValueByKeyAndTime("heading-deg", model.CurrentLineIndex).ToString();
            dataPitch.Text = model.getValueByKeyAndTime("pitch-deg", model.CurrentLineIndex).ToString();
            dataRoll.Text = model.getValueByKeyAndTime("roll-deg", model.CurrentLineIndex).ToString();
            dataSideSlip.Text = model.getValueByKeyAndTime("side-slip-deg", model.CurrentLineIndex).ToString();
            */
        }
    }
}
