using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
using FlightInspectionDesktopApp.Altimeter;

namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Altimeter.xaml
    /// </summary>
    public partial class Altimeter : UserControl
    {
        // field of Altimeter.
        AltimeterViewModel vm;

        /// <summary>
        /// CTOR of Altimeter object.
        /// </summary>
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

    /// <summary>
    /// Conveter which floors values.
    /// </summary>
    class FlooringConverter : IValueConverter
    {
        /// <summary>
        /// Floors values.
        /// </summary>
        /// <param name="value">value that we're binded to</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">none</param>
        /// <param name="culture">none</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Math.Floor((double)value);
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
