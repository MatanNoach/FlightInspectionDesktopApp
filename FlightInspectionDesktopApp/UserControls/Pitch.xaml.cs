using System;
using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
using FlightInspectionDesktopApp.Metadata;

namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Pitch.xaml
    /// </summary>
    public partial class Pitch : UserControl
    {
        private MetadataViewModel vm;
        public Pitch()
        {
            InitializeComponent();
            this.vm = new MetadataViewModel(MetadataModel.Instance);
            this.DataContext = this.vm;
        }
    }


    class PitchDegToStartPointConverter : IValueConverter
    {
        /// <summary>
        /// Converts Pitch values for the UserControl.
        /// </summary>
        /// <param name="value">value that we're bound to</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">none</param>
        /// <param name="culture">none</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double yVal = (double)value / 180.0;
            return new Point(0.5, yVal);
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

    class PitchDegToEndPointConverter : IValueConverter
    {
        /// <summary>
        /// Converts Roll values for Pitch user control.
        /// </summary>
        /// <param name="value">value that we're bound to</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">none</param>
        /// <param name="culture">none</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double xVal = 0.5 + (double)value / 180.0;
            return new Point(xVal, 1);
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
