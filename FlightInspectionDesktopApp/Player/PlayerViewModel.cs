using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Player
{
    class PlayerViewModel : INotifyPropertyChanged
    {
        //fields of PlayerViewModel object
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
        /// CurrentLineIndex proprety for the viewModel.
        /// </summary>
        public int VMCurrentLineIndex
        {
            // getter of CurrentLineIndex.
            get
            {
                return playerModel.CurrentLineIndex;
            }

            // setter of CurrentLineIndex.
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
            // getter of MaxLine.
            get
            {
                return playerModel.MaxLine;
            }
        }

        /// <summary>
        /// CurrTime Property for the view model.
        /// </summary>
        public string VMCurrTime
        {
            // getter of CurrTime.
            get
            {
                return playerModel.CurrTime;
            }

            // setter of CurrTime.
            set
            {
                playerModel.CurrTime = value;
            }
        }

        /// <summary>
        /// This fucntion notifies change in a property by it's name.
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
        /// This function playes the simulator regulary.
        /// </summary>
        public void Play()
        {
            playerModel.Play();
        }

        /// <summary>
        /// This function playes the simulator in fast forward.
        /// </summary>
        public void FastForward()
        {
            playerModel.fastForward();
        }

        /// <summary>
        /// This fucntion pauses the simulator.
        /// </summary>
        public void Pause()
        {
            playerModel.Pause();
        }

        /// <summary>
        /// This function stops the simualtor and restart it.
        /// </summary>
        public void Stop()
        {
            playerModel.Stop();
        }

        /// <summary>
        /// This function playes the simualator in reverse.
        /// </summary>
        public void Slower()
        {
            playerModel.Slower();
        }

        /// <summary>
        /// This function playes the simulator in fast reverse.
        /// </summary>
        public void MuchSlower()
        {
            playerModel.MuchSlower();
        }
    }
}
