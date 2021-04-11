using System;
using System.ComponentModel;

namespace FlightInspectionDesktopApp.Player
{

    class PlayerModel : INotifyPropertyChanged
    {
        // fields of PlayerModel object

        private DataModel dataModel;
        private string currTime = "0:00";
        private static PlayerModel playerModelInst;
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
        /// Property of the simulator playing speed - set the default playing speed to 10 Hz.
        /// </summary>
        public int PlayingSpeed { get; set; } = 100;

        /// <summary>
        /// Property for the current line index in the csv file
        /// </summary>
        public int CurrentLineIndex
        {
            // getter of currentLineIndex.
            get
            {
                if (this != null)
                {
                    int minutes = dataModel.CurrentLineIndex / 600;
                    int seconds = (dataModel.CurrentLineIndex / 10) % 60;
                    if (seconds > 9)
                    {
                        CurrTime = minutes.ToString() + ":" + seconds.ToString();
                    }
                    else
                    {
                        CurrTime = minutes.ToString() + ":0" + seconds.ToString();
                    }
                }
                return dataModel.CurrentLineIndex;
            }

            // setter of currentLineIndex.
            set
            {
                dataModel.CurrentLineIndex = value;
                NotifyPropertyChanged("CurrentLineIndex");

            }
        }

        /// <summary>
        /// Property of currTime.
        /// </summary>
        public string CurrTime
        {
            // getter of currTime.
            get
            {
                return currTime;
            }

            // setter of currTime.
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
            // getter of MaxLine.
            get
            {
                return dataModel.getDataSize() - 1;
            }
        }

        /// <summary>
        /// Property for the data model.
        /// </summary>
        public DataModel DataModel
        {
            // getter of dataModel.
            get
            {
                return dataModel;
            }
        }

        /// <summary>
        /// This static property returns an instance of PlayerModel object.
        /// </summary>
        public static PlayerModel Instance
        {
            // getter of playerModelInst.
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
        /// This static function creates an instance of PlayerModel object.
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
        /// This function notifes a certain property change by it's name.
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
        /// This function playes the simulator in regular speed
        /// </summary>
        public void Play()
        {
            PlayingSpeed = 100;
            dataModel.NextLine = 1;
        }

        /// <summary>
        /// This function playes the simulator double it's regular speed.
        /// </summary>
        public void fastForward()
        {
            PlayingSpeed /= 2;
            dataModel.NextLine = 1;
        }

        /// <summary>
        /// Thus function pauses the simulator.
        /// </summary>
        public void Pause()
        {
            dataModel.NextLine = 0;
        }

        /// <summary>
        /// This function stops the simualtor and rewind's it to the start.
        /// </summary>
        public void Stop()
        {
            dataModel.CurrentLineIndex = 0;
            dataModel.NextLine = 0;
        }

        /// <summary>
        /// This function playes the simulator in a slow speed.
        /// </summary>
        public void Slower()
        {
            PlayingSpeed = (int)(PlayingSpeed * 1.25);
            dataModel.NextLine = 1;
        }

        /// <summary>
        /// This function playes the simulator in extra slow speed.
        /// </summary>
        public void MuchSlower()
        {
            PlayingSpeed = (int)(PlayingSpeed * 1.5);
            dataModel.NextLine = 1;
        }
    }
}
