using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
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

    class YawConverter : IValueConverter
    {
        /// <summary>
        /// Converts Yaw values for Yaw user control.
        /// </summary>
        /// <param name="value">value that we're bound to</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">none</param>
        /// <param name="culture">none</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * -1;
        }

        /// <summary>
        /// Not implemented.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
