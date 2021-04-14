using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using FlightInspectionDesktopApp.Metadata;

namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Roll.xaml
    /// </summary>
    public partial class Roll : UserControl
    {
        MetadataViewModel vm;
        public Roll()
        {
            InitializeComponent();
            this.vm = new MetadataViewModel(MetadataModel.Instance);
            this.DataContext = this.vm;
        }
    }
    class LineHeightConverter : IValueConverter
    {
        /// <summary>
        /// Converts the image's width and returns the half of it.
        /// </summary>
        /// <param name="value">value that we're binded to</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">JoystickBoundries Ellipse</param>
        /// <param name="culture">none</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value / 2.0) + 8;
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

    class RollConverter : IValueConverter
    {
        /// <summary>
        /// Converts Roll for the user control.
        /// </summary>
        /// <param name="value">value that we're bound to</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">none</param>
        /// <param name="culture">none</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value * (-1);
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
