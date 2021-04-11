using FlightInspectionDesktopApp.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
