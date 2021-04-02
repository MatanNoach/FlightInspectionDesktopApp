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
            MetadataModel.CreateModel();
            vm = new MetadataViewModel(MetadataModel.Instance);
            //? we're not sure if it's doing anything
            this.DataContext = vm;
        }
    }
}
