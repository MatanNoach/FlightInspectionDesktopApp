using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Player
{

    class PlayerModel : INotifyPropertyChanged
    {
        private double currTime = 0;
        private static PlayerModel playerModelInst;
        private DataModel dataModel;
        // set the default playing speed to 10 Hz
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="model">A data model instance</param>
        private PlayerModel(DataModel model)
        {
            this.dataModel = model;
            dataModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged(e.PropertyName);
            };
        }
        /// <summary>
        /// Property of the simulator playing speed
        /// </summary>
        public int PlayingSpeed { get; set; } = 100;
        /// <summary>
        /// Property for the current line index in the csv file
        /// </summary>
        public int CurrentLineIndex
        {
            get
            {
                if (this != null)
                {
                    CurrTime = dataModel.CurrentLineIndex / (1000 / 100);
                }
                return dataModel.CurrentLineIndex;
            }
            set
            {
                dataModel.CurrentLineIndex = value;
                NotifyPropertyChanged("CurrentLineIndex");

            }
        }


        public double CurrTime
        {
            get { return currTime; }
            set
            {
                currTime = value;
                NotifyPropertyChanged("CurrTime");
            }
        }

        /// <summary>
        /// Property for the max line number in the csv file
        /// </summary>
        public int MaxLine
        {
            get
            {
                return dataModel.getDataSize() - 1;
            }
        }
        /// <summary>
        /// Property for the data model
        /// </summary>
        public DataModel DataModel
        {
            get
            {
                return dataModel;
            }
        }
        /// <summary>
        /// The function returns an instance of PlayerModel
        /// </summary>
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
        /// <summary>
        /// The function creates an instance of PlayerModel
        /// </summary>
        /// <param name="model"></param>
        public static void CreateModel(DataModel model)
        {
            if (playerModelInst != null)
            {
                throw new Exception("PlayerModel was already created");
            }
            playerModelInst = new PlayerModel(model);
        }
        /// <summary>
        /// The function notifes a certain property change by it's name
        /// </summary>
        /// <param name="propName">The properite's name</param>
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        /// <summary>
        /// The function playes the simulator in regular speed
        /// </summary>
        public void Play()
        {
            PlayingSpeed = 100;
            dataModel.NextLine = 1;
        }
        /// <summary>
        /// The function playes the simulator double it's regular speed
        /// </summary>
        public void fastForward()
        {
            PlayingSpeed /= 2;
            dataModel.NextLine = 1;
        }
        /// <summary>
        /// The function pauses the simulator
        /// </summary>
        public void Pause()
        {
            dataModel.NextLine = 0;
        }
        /// <summary>
        /// The function stops the simualtor and rewind's it to the start
        /// </summary>
        public void Stop()
        {
            dataModel.CurrentLineIndex = 0;
            dataModel.NextLine = 0;
        }
        /// <summary>
        /// The function playes the simulator in revserse in regular speed
        /// </summary>
        public void Slower()
        {
            PlayingSpeed = (int)(PlayingSpeed * 1.25);
            dataModel.NextLine = 1;
        }
        /// <summary>
        /// The function playes the simulator in reverse double's it's regular speed
        /// </summary>
        public void MuchSlower()
        {
            PlayingSpeed = (int)(PlayingSpeed * 1.5);
            dataModel.NextLine = 1;
        }
    }
}
