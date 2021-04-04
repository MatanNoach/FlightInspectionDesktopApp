using FlightInspectionDesktopApp.Steering;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

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

    class AileronValueToJoystickConverter : IValueConverter
    {
        /// <summary>
        /// Converts Aileron [-1,1] values to Joystick values.
        /// </summary>
        /// <param name="value">value that we're binded to</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">JoystickBoundries Ellipse</param>
        /// <param name="culture">none</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double size = (Properties.Settings.Default.bigCircle / 2) - (Properties.Settings.Default.smallCircle / 2);
            return (double)value * size + size;
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

    class ElevatorValueToJoystickConverter : IValueConverter
    {
        /// <summary>
        /// Converts Elevator [-1,1] values to Joystick values.
        /// </summary>
        /// <param name="value">value that we're binded to</param>
        /// <param name="targetType">none</param>
        /// <param name="parameter">JoystickBoundries Ellipse</param>
        /// <param name="culture">none</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double size = (Properties.Settings.Default.bigCircle / 2) - (Properties.Settings.Default.smallCircle / 2);
            return (double)value * -size + size;
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
