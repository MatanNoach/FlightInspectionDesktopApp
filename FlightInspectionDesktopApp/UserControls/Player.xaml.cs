using FlightInspectionDesktopApp.Player;
using System.Windows.Controls;
namespace FlightInspectionDesktopApp.UserControls
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : UserControl
    {
        PlayerViewModel viewModel;
        public Player()
        {
            InitializeComponent();
            PlayerModel.CreateModel(FGModelImp.Instance);
            viewModel = new PlayerViewModel(PlayerModel.Instance);
            DataContext = viewModel;
        }


        private void Play_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.Play();
        }

        private void FastForward_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.FastForward();
        }
        private void Pause_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.Pause();
        }

        private void FastReverse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.FastReverse();
        }

        private void Reverse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.Reverse();
        }

        private void Stop_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            viewModel.Stop();
        }
    }
}
