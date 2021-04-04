namespace FlightInspectionDesktopApp.Player
{
    class PlayerViewModel
    {
        PlayerModel model;
        public PlayerViewModel(PlayerModel model)
        {
            this.model = model;
        }
        public void Play()
        {
            model.Play();
        }
        public void FastForward()
        {
            model.fastForward();
        }
        public void Pause()
        {
            model.Pause();
        }
        public void Stop()
        {
            model.Stop();
        }
        public void Reverse()
        {
            model.Reverse();
        }
        public void FastReverse()
        {
            model.FastReverse();
        }
    }
}
