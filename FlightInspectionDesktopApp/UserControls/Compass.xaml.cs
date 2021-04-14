using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
using FlightInspectionDesktopApp.Metadata;

namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Compass.xaml
    /// </summary>
    public partial class Compass : UserControl
    {
        // fields of Compass
        private MetadataViewModel vm;

        /// <summary>
        /// CTOR of Compass.
        /// </summary>
        public Compass()
        {
            InitializeComponent();
            MetadataModel.CreateModel();
            this.vm = new MetadataViewModel(MetadataModel.Instance);
            this.DataContext = this.vm;
        }
    }

    /// <summary>
    /// Converter for size of an image.
    /// </summary>
    class ImageSizeConverter : IValueConverter
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
            return (double)value / 2;
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

    class YAxisConverter1 : IValueConverter
    {
        /// <summary>
        /// Converts the image's height.
        /// </summary>
        /// <param name="value">value that we're binded to</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">JoystickBoundries Ellipse</param>
        /// <param name="culture">none</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / 2 - (double)value / 4;
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

    class YAxisConverter2 : IValueConverter
    {
        /// <summary>
        /// Converts the image's height.
        /// </summary>
        /// <param name="value">value that we're binded to</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">JoystickBoundries Ellipse</param>
        /// <param name="culture">none</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (double)value / 2 + (double)value / 4;
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
    class HeadingToCompassConverter : IValueConverter
    {
        /// <summary>
        /// Converts Heading values for compass user control.
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
