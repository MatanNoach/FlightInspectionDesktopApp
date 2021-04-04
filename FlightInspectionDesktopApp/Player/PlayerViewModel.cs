using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Player
{
    class PlayerViewModel : INotifyPropertyChanged
    {
        //The player model
        PlayerModel playerModel;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">A player model instance</param>
        public PlayerViewModel(PlayerModel model)
        {
            this.playerModel = model;
            playerModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM" + e.PropertyName);
            };
        }
        /// <summary>
        /// CurrentLineIndex proprety for the viewModel
        /// </summary>
        public int VMCurrentLineIndex
        {
            get
            {
                return playerModel.CurrentLineIndex;
            }
            set
            {
                playerModel.CurrentLineIndex = value;
            }
        }
        /// <summary>
        /// MaxLine property for the view model
        /// </summary>
        public int VMMaxLine
        {
            get
            {
                return playerModel.MaxLine;
            }
        }
        /// <summary>
        /// The fucntion notify change in a property by it's name
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        /// <summary>
        /// The function playes the simulator regulary
        /// </summary>
        public void Play()
        {
            playerModel.Play();
        }
        /// <summary>
        /// The function playes the simulator in fast forward
        /// </summary>
        public void FastForward()
        {
            playerModel.fastForward();
        }
        /// <summary>
        /// The fucntion pauses the simulator
        /// </summary>
        public void Pause()
        {
            playerModel.Pause();
        }
        /// <summary>
        /// The function stops the simualtor and restart it
        /// </summary>
        public void Stop()
        {
            playerModel.Stop();
        }
        /// <summary>
        /// The function playes the simualator in reverse
        /// </summary>
        public void Reverse()
        {
            playerModel.Reverse();
        }
        /// <summary>
        /// The function playes the simulator in fast reverse
        /// </summary>
        public void FastReverse()
        {
            playerModel.FastReverse();
        }
    }
}
