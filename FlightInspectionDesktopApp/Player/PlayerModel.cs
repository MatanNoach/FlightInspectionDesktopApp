using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Player
{
    class PlayerModel : INotifyPropertyChanged
    {
        private static PlayerModel playerModelInst;
        private FGModelImp fgModel;

        public event PropertyChangedEventHandler PropertyChanged;

        private PlayerModel(FGModelImp model)
        {
            this.fgModel = model;
        }
        public int CurrentLine
        {
            get
            {
                return fgModel.DataModel.CurrentLineIndex;
            }
            set
            {
                fgModel.DataModel.CurrentLineIndex = value;
                NotifyPropertyChanged("CurrentLine");
            }
        }
        public int MaxLine
        {
            get
            {
                return fgModel.DataModel.MaxLine;
            }
        }
        public static PlayerModel Instance
        {
            get
            {
                if (playerModelInst == null)
                {
                    throw new Exception("PlayerModel was not created");
                }
                return playerModelInst;
            }
        }
        public static void CreateModel(FGModelImp model)
        {
            if (playerModelInst != null)
            {
                throw new Exception("PlayerModel was already created");
            }
            playerModelInst = new PlayerModel(model);
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
            fgModel.PlayingSpeed = 100;
            fgModel.DataModel.NextLine = 1;
        }
        public void fastForward()
        {
            fgModel.PlayingSpeed = 50;
            fgModel.DataModel.NextLine = 1;
        }
        public void Pause()
        {
            fgModel.DataModel.NextLine = 0;
        }
        public void Stop()
        {
            fgModel.DataModel.CurrentLineIndex = 0;
            fgModel.DataModel.NextLine = 0;
        }
        public void Reverse()
        {
            fgModel.PlayingSpeed = 100;
            fgModel.DataModel.NextLine = -1;
        }
        public void FastReverse()
        {
            fgModel.PlayingSpeed = 50;
            fgModel.DataModel.NextLine = -1;
        }
    }
}
