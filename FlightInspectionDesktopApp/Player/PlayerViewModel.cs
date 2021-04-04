using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Player
{
    class PlayerViewModel : INotifyPropertyChanged
    {
        PlayerModel playerModel;

        public event PropertyChangedEventHandler PropertyChanged;


        public PlayerViewModel(PlayerModel model)
        {
            this.playerModel = model;
            playerModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM" + e.PropertyName);
            };
        }
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public void Play()
        {
            playerModel.Play();
        }
        public void FastForward()
        {
            playerModel.fastForward();
        }
        public void Pause()
        {
            playerModel.Pause();
        }
        public void Stop()
        {
            playerModel.Stop();
        }
        public void Reverse()
        {
            playerModel.Reverse();
        }
        public void FastReverse()
        {
            playerModel.FastReverse();
        }
        public int VMCurrentLine
        {
            get
            {
                return playerModel.CurrentLine;
            }
            set
            {
                playerModel.CurrentLine = value;
            }
        }
        public int VMMaxLine
        {
            get
            {
                return playerModel.MaxLine;
            }
        }
    }
}
