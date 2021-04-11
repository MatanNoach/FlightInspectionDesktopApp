using FlightInspectionDesktopApp.Metadata;
using System.Windows.Controls;

namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Yaw.xaml
    /// </summary>
    public partial class Yaw : UserControl
    {
        private MetadataViewModel vm;
        public Yaw()
        {
            InitializeComponent();
            this.vm = new MetadataViewModel(MetadataModel.Instance);
            this.DataContext = this.vm;
        }
    }
}
