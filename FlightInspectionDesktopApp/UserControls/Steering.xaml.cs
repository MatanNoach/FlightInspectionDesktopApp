using FlightInspectionDesktopApp.Steering;
using System.Windows.Controls;


namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Steering.xaml
    /// </summary>
    public partial class Steering : UserControl
    {
        SteeringViewModel vm;
        public Steering()
        {
            InitializeComponent();
            SteeringModel.CreateModel();
            vm = new SteeringViewModel(SteeringModel.Instance);
            this.DataContext = vm;
        }
    }
}
