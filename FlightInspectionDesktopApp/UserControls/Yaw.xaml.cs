using System.Windows.Controls;
using FlightInspectionDesktopApp.Metadata;

namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Yaw.xaml
    /// </summary>
    public partial class Yaw : UserControl
    {
        // field of Yaw.
        private MetadataViewModel vm;

        /// <summary>
        /// CTOR of Yaw.
        /// </summary>
        public Yaw()
        {
            InitializeComponent();
            this.vm = new MetadataViewModel(MetadataModel.Instance);
            this.DataContext = this.vm;
        }
    }
}
