using System.Windows.Controls;
using FlightInspectionDesktopApp.Speedometer;

namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Speedometer.xaml
    /// </summary>
    public partial class Speedometer : UserControl
    {
        SpeedometerViewModel vm;
        public Speedometer()
        {
            InitializeComponent();
            SpeedometerModel.CreateModel();
            vm = new SpeedometerViewModel(SpeedometerModel.Instance);
            this.DataContext = vm;
        }
    }
}
