using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
using FlightInspectionDesktopApp.Player;

namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        // field of PlayerViewModel.
        PlayerViewModel viewModel;

        /// <summary>
        /// CTOR of Player.
        /// </summary>
        public Player()
        {
            InitializeComponent();
            // Create the player model and player view model
            PlayerModel.CreateModel(DataModel.Instance);
            viewModel = new PlayerViewModel(PlayerModel.Instance);
            // Set the view model as the data context
            DataContext = viewModel;
        }

        /// <summary>
        /// The function plays the simualtor on play button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Play_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.Play();
        }
        /// <summary>
        /// The function fast forwards the simualtor on fast forward button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FastForward_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.FastForward();
        }
        /// <summary>
        /// The function pauses the simualtor on pause button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Pause_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.Pause();
        }
        /// <summary>
        /// The function fast reverse the simualtor on fast reverse button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MuchSlower_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.MuchSlower();
        }
        /// <summary>
        /// The function runs the simulator in reverse the simualtor on reverse button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Slower_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.Slower();
        }
        /// <summary>
        /// The function stops the simualtor on stop button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.Stop();
        }
    }

    class PlaySpeedToPercentageConverter : IValueConverter
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
            double playSpeed = 100.0 / (int)value;
            return String.Concat("x", String.Format("{0:0.00}", playSpeed));
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
