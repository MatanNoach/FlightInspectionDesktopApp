using System.Windows.Controls;
using FlightInspectionDesktopApp.Speedometer;

namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Speedometer.xaml
    /// </summary>
    public partial class Speedometer : UserControl
    {
        // field of Speedometer.
        SpeedometerViewModel vm;

        /// <summary>
        /// CTOR of Speedometer.
        /// </summary>
        public Speedometer()
        {
            InitializeComponent();
            SpeedometerModel.CreateModel();
            vm = new SpeedometerViewModel(SpeedometerModel.Instance);
            this.DataContext = vm;
        }
    }
}
