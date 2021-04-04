using System.Windows.Controls;
using FlightInspectionDesktopApp.Altimeter;

namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Altimeter.xaml
    /// </summary>
    public partial class Altimeter : UserControl
    {
        AltimeterViewModel vm;
        public Altimeter()
        {
            InitializeComponent();

            Properties.Settings.Default.altimeterLow = DataModel.Instance.getMinValueByKey(Properties.Settings.Default.altimeter);
            Properties.Settings.Default.altimeterHigh = DataModel.Instance.getMaxValueByKey(Properties.Settings.Default.altimeter);

            AltimeterModel.CreateModel();
            vm = new AltimeterViewModel(AltimeterModel.Instance);
            this.DataContext = vm;
        }
    }
}
